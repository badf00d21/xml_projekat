using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parliament.Security
{
	public static class XMLUtils
	{
		public static void SignXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			SignedXml signedXml = new SignedXml(document);

			signedXml.SigningKey = certificate.PrivateKey;

			KeyInfo keyInfo = new KeyInfo();
			KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
			keyInfo.AddClause(keyInfoData);

			signedXml.KeyInfo = keyInfo;

			Reference reference = new Reference();
			reference.Uri = "";

			XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(env);

			signedXml.AddReference(reference);
			signedXml.ComputeSignature();

			XmlElement xmlDigitalSignature = signedXml.GetXml();
			document.DocumentElement.AppendChild(document.ImportNode(xmlDigitalSignature, true));
		}

		public static void EncryptXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			EncryptedXml eXml = new EncryptedXml();
			EncryptedData edElement = eXml.Encrypt(document.DocumentElement, certificate);

			EncryptedXml.ReplaceElement(document.DocumentElement, edElement, false);
		}

		public static void DecryptXmlDocument(XmlDocument document, X509Certificate2 certificate)
		{
			EncryptedXml eXml = new EncryptedXml(document);
			eXml.AddKeyNameMapping("rsaKey", certificate.PrivateKey);

			eXml.DecryptDocument();
		}

		public static void AddTimestampAndSerialNumber(XmlDocument document, int serialNumber)
		{
			document.DocumentElement.SetAttribute("RedniBrojPoruke", serialNumber.ToString());
			document.DocumentElement.SetAttribute("VremeSlanjaPoruke", DateTime.Now.ToString());
		}
	}
}
