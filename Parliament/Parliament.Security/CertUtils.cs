using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.Security
{
    public static class CertUtils
    {
        public static X509Certificate2 CreateSelfSignedCertificate(string subjectName, string[] subjectAlternativeNames, KeyPurposeID[] usages)
        {
            // It's self-signed, so these are the same.
            var issuerName = subjectName;

            var random = GetSecureRandom();
            var subjectKeyPair = GenerateKeyPair(random, 2048);

            // It's self-signed, so these are the same.
            var issuerKeyPair = subjectKeyPair;

            var serialNumber = GenerateSerialNumber(random);
            var issuerSerialNumber = serialNumber; // Self-signed, so it's the same serial number.

            const bool isCertificateAuthority = false;
            var certificate = GenerateCertificate(random, subjectName, subjectKeyPair, serialNumber,
                                                  subjectAlternativeNames, issuerName, issuerKeyPair,
                                                  issuerSerialNumber, isCertificateAuthority,
                                                  usages);
            return ConvertCertificate(certificate, subjectKeyPair, random);
        }

        public static X509Certificate2 CreateCertificateAuthorityCertificate(string subjectName, string[] subjectAlternativeNames, KeyPurposeID[] usages)
        {
            // It's self-signed, so these are the same.
            var issuerName = subjectName;

            var random = GetSecureRandom();
            var subjectKeyPair = GenerateKeyPair(random, 2048);

            // It's self-signed, so these are the same.
            var issuerKeyPair = subjectKeyPair;

            var serialNumber = GenerateSerialNumber(random);
            var issuerSerialNumber = serialNumber; // Self-signed, so it's the same serial number.

            const bool isCertificateAuthority = true;
            var certificate = GenerateCertificate(random, subjectName, subjectKeyPair, serialNumber,
                                                  subjectAlternativeNames, issuerName, issuerKeyPair,
                                                  issuerSerialNumber, isCertificateAuthority,
                                                  usages);
            return ConvertCertificate(certificate, subjectKeyPair, random);
        }

        public static Org.BouncyCastle.X509.X509Certificate GenerateCertificate(SecureRandom random,
                                                           string subjectName,
                                                           AsymmetricCipherKeyPair subjectKeyPair,
                                                           BigInteger subjectSerialNumber,
                                                           string[] subjectAlternativeNames,
                                                           string issuerName,
                                                           AsymmetricCipherKeyPair issuerKeyPair,
                                                           BigInteger issuerSerialNumber,
                                                           bool isCertificateAuthority,
                                                           KeyPurposeID[] usages)
        {
            var certificateGenerator = new X509V3CertificateGenerator();

            certificateGenerator.SetSerialNumber(subjectSerialNumber);

            // Set the signature algorithm. This is used to generate the thumbprint which is then signed
            // with the issuer's private key. We'll use SHA-256, which is (currently) considered fairly strong.
            const string signatureAlgorithm = "SHA256WithRSA";
            certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);

            var issuerDN = new X509Name(issuerName);
            certificateGenerator.SetIssuerDN(issuerDN);

            // Note: The subject can be omitted if you specify a subject alternative name (SAN).
            var subjectDN = new X509Name(subjectName);
            certificateGenerator.SetSubjectDN(subjectDN);

            // Our certificate needs valid from/to values.
            var notBefore = DateTime.UtcNow.Date;
            var notAfter = notBefore.AddYears(2);

            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);

            // The subject's public key goes in the certificate.
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            AddAuthorityKeyIdentifier(certificateGenerator, issuerDN, issuerKeyPair, issuerSerialNumber);
            AddSubjectKeyIdentifier(certificateGenerator, subjectKeyPair);
            AddBasicConstraints(certificateGenerator, isCertificateAuthority);

            if (usages != null && usages.Any())
                AddExtendedKeyUsage(certificateGenerator, usages);

            if (subjectAlternativeNames != null && subjectAlternativeNames.Any())
                AddSubjectAlternativeNames(certificateGenerator, subjectAlternativeNames);

            // The certificate is signed with the issuer's private key.
            var certificate = certificateGenerator.Generate(issuerKeyPair.Private, random);
            return certificate;
        }

        private static AsymmetricCipherKeyPair GenerateKeyPair(SecureRandom random, int strength)
        {
            var keyGenerationParameters = new KeyGenerationParameters(random, strength);

            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            var subjectKeyPair = keyPairGenerator.GenerateKeyPair();
            return subjectKeyPair;
        }

        private static BigInteger GenerateSerialNumber(SecureRandom random)
        {
            var serialNumber =
                BigIntegers.CreateRandomInRange(
                    BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            return serialNumber;
        }

        private static void AddAuthorityKeyIdentifier(X509V3CertificateGenerator certificateGenerator,
                                                     X509Name issuerDN,
                                                     AsymmetricCipherKeyPair issuerKeyPair,
                                                     BigInteger issuerSerialNumber)
        {
            var authorityKeyIdentifierExtension =
                new AuthorityKeyIdentifier(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(issuerKeyPair.Public),
                    new GeneralNames(new GeneralName(issuerDN)),
                    issuerSerialNumber);
            certificateGenerator.AddExtension(
                X509Extensions.AuthorityKeyIdentifier.Id, false, authorityKeyIdentifierExtension);
        }

        private static void AddSubjectAlternativeNames(X509V3CertificateGenerator certificateGenerator,
                                                       IEnumerable<string> subjectAlternativeNames)
        {
            var subjectAlternativeNamesExtension =
                new DerSequence(
                    subjectAlternativeNames.Select(name => new GeneralName(GeneralName.DnsName, name))
                                           .ToArray<Asn1Encodable>());

            certificateGenerator.AddExtension(
                X509Extensions.SubjectAlternativeName.Id, false, subjectAlternativeNamesExtension);
        }

        private static void AddExtendedKeyUsage(X509V3CertificateGenerator certificateGenerator, KeyPurposeID[] usages)
        {
            certificateGenerator.AddExtension(
                X509Extensions.ExtendedKeyUsage.Id, false, new ExtendedKeyUsage(usages));
        }

        private static void AddBasicConstraints(X509V3CertificateGenerator certificateGenerator,
                                                bool isCertificateAuthority)
        {
            certificateGenerator.AddExtension(
                X509Extensions.BasicConstraints.Id, true, new BasicConstraints(isCertificateAuthority));
        }

        private static void AddSubjectKeyIdentifier(X509V3CertificateGenerator certificateGenerator,
                                                    AsymmetricCipherKeyPair subjectKeyPair)
        {
            var subjectKeyIdentifierExtension =
                new SubjectKeyIdentifier(
                    SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(subjectKeyPair.Public));
            certificateGenerator.AddExtension(
                X509Extensions.SubjectKeyIdentifier.Id, false, subjectKeyIdentifierExtension);
        }

        private static X509Certificate2 ConvertCertificate(Org.BouncyCastle.X509.X509Certificate certificate,
                                                           AsymmetricCipherKeyPair subjectKeyPair,
                                                           SecureRandom random)
        {
            // Now to convert the Bouncy Castle certificate to a .NET certificate.
            // See http://web.archive.org/web/20100504192226/http://www.fkollmann.de/v2/post/Creating-certificates-using-BouncyCastle.aspx
            // ...but, basically, we create a PKCS12 store (a .PFX file) in memory, and add the public and private key to that.
            var store = new Pkcs12Store();

            // What Bouncy Castle calls "alias" is the same as what Windows terms the "friendly name".
            string friendlyName = certificate.SubjectDN.ToString();

            // Add the certificate.
            var certificateEntry = new X509CertificateEntry(certificate);
            store.SetCertificateEntry(friendlyName, certificateEntry);

            // Add the private key.
            store.SetKeyEntry(friendlyName, new AsymmetricKeyEntry(subjectKeyPair.Private), new[] { certificateEntry });

            // Convert it to an X509Certificate2 object by saving/loading it from a MemoryStream.
            // It needs a password. Since we'll remove this later, it doesn't particularly matter what we use.
            const string password = "password";
            var stream = new MemoryStream();
            store.Save(stream, password.ToCharArray(), random);

            var convertedCertificate =
                new X509Certificate2(stream.ToArray(),
                                     password,
                                     X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return convertedCertificate;
        }

        private static void WriteCertificate(X509Certificate2 certificate, string outputFileName)
        {
            // This password is the one attached to the PFX file. Use 'null' for no password.
            const string password = "password";
            var bytes = certificate.Export(X509ContentType.Pfx, password);
            File.WriteAllBytes(outputFileName, bytes);
        }

        private static SecureRandom GetSecureRandom()
        {
           
            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);
            return random;
        }
    }
}
