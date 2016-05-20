using Caliburn.Micro;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Parliament.DataAccess.Database;
using Parliament.Security;
using SwollenMvvmToolkit.CaliburnMicro.ViewModels;
using SwollenMvvmToolkit.Core.Services;
using Syringe.ObservableClassAmpoule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.CertTool.ViewModels
{
    [Export]
    [Observable]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : Screen
    {
        private bool _isKeystoreChanged;
        private string _keystorePath;
        private string _keystorePassword;
        private Pkcs12Store _keystore;
        private Dictionary<string, X509Certificate2> _certificates;

        public IWindowManager WindowManager { get; set; }
        public IFeedbackService FeedbackService { get; set; }
        public IDialogService DialogService { get; set; }

        public BindableCollection<CertificateViewModel> Certificates { get; set; }

        public BindableCollection<UserViewModel> Users { get; set; }
        public CertificateViewModel SelectedCertificate { get; set; }

        public bool CanExportCertificate { get { return SelectedCertificate != null; } }

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager, IFeedbackService feedbackService, IDialogService dialogService)
        {
            _isKeystoreChanged = false;
            _keystorePath = string.Empty;
            _keystorePassword = string.Empty;
            _keystore = new Pkcs12Store();
            _certificates = new Dictionary<string, X509Certificate2>();

            DisplayName = "Parliament User And Certificate Manager";

            WindowManager = windowManager;
            FeedbackService = feedbackService;
            DialogService = dialogService;

            Certificates = new BindableCollection<CertificateViewModel>();
            Users = new BindableCollection<UserViewModel>();

            using (var dbContext = new ParliamentDbContext())
            {
                foreach (var user in dbContext.Users)
                {
                    Users.Add(new UserViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = user.Role.ToString()
                    });
                }
            }
        }

        public void NewKeystore()
        {
            if (_isKeystoreChanged)
            {
                bool? result = FeedbackService.ShowWarningMessage("Would you like to save changes to current keystore first?");

                if (result.HasValue && result == true)
                    SaveKeystore();
            }

            NewKeystoreViewModel keystoreDialog = new NewKeystoreViewModel();
            WindowManager.ShowDialog(keystoreDialog);

            if (!keystoreDialog.IsCanceled)
            {
                _keystorePath = keystoreDialog.KeystoreFilePath;
                _keystorePassword = keystoreDialog.Password;

                Certificates.Clear();
                _isKeystoreChanged = false;

                _keystore = new Pkcs12Store();
                _certificates = new Dictionary<string, X509Certificate2>();
            }
        }

        public void OpenKeystore()
        {
            if (_isKeystoreChanged)
            {
                bool? result = FeedbackService.ShowWarningMessage("Would you like to save changes to current keystore first?");

                if (result.HasValue && result == true)
                    SaveKeystore();
            }

            OpenKeystoreViewModel keystoreDialog = new OpenKeystoreViewModel();
            WindowManager.ShowDialog(keystoreDialog);

            if (keystoreDialog.IsCanceled || string.IsNullOrWhiteSpace(keystoreDialog.KeystoreFilePath))
                return;

            using (var fileStream = new FileStream(keystoreDialog.KeystoreFilePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    _keystore = new Pkcs12Store();

                    _keystorePath = keystoreDialog.KeystoreFilePath;
                    _keystorePassword = keystoreDialog.Password;
                    _isKeystoreChanged = false;

                    Certificates.Clear();
                    _certificates = new Dictionary<string, X509Certificate2>();

                    X509Certificate2Collection certificateCollection = new X509Certificate2Collection();
                    certificateCollection.Import(_keystorePath, _keystorePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                    foreach (var certificate in certificateCollection)
                    {
                        _certificates.Add(certificate.SerialNumber, certificate);

                        string certAlias = certificate.FriendlyName.Split(',').First(s => s.Contains("GIVENNAME=")).Replace("GIVENNAME=", "").Trim();
                        string certIssuer = certificate.Issuer.Split(',').First(s => s.Contains("G=")).Replace("G=", "").Trim();

                        Certificates.Add(new CertificateViewModel
                        {
                            Alias = certAlias,
                            SerialNumber = certificate.SerialNumber,
                            Issuer = certAlias == certIssuer ? "Self signed" : certIssuer,
                            ValidFrom = certificate.NotBefore.ToString(),
                            ValidUntil = certificate.NotAfter.ToString(),
                            IsCA = true
                        });

                        CertUtils.AddCertificateToStore(certificate, _keystore, _keystorePassword);
                    }

                    foreach (var certificate in Certificates)
                        if (Certificates.Any(c => c.Alias == certificate.Issuer))
                            certificate.Issuer += string.Format(" ({0})", Certificates.First(c => c.Alias == certificate.Issuer).SerialNumber);

                }
                catch
                {
                    FeedbackService.ShowErrorMessage(string.Format("Could not open: '{0}'", keystoreDialog.KeystoreFilePath));
                }
            }

        }

        public void SaveKeystore()
        {
            if (string.IsNullOrWhiteSpace(_keystorePath) || string.IsNullOrWhiteSpace(_keystorePassword))
            {
                NewKeystoreViewModel keystoreDialog = new NewKeystoreViewModel();
                WindowManager.ShowDialog(keystoreDialog);

                if (!keystoreDialog.IsCanceled)
                {
                    _keystorePath = keystoreDialog.KeystoreFilePath;
                    _keystorePassword = keystoreDialog.Password;
                }
            }

            using (var fileStream = new FileStream(_keystorePath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    _keystore.Save(fileStream, _keystorePassword.ToArray(), new SecureRandom());
                    _isKeystoreChanged = false;
                    FeedbackService.ShowInfoMessage("Keystore successfully saved!");
                }
                catch
                {
                    FeedbackService.ShowErrorMessage("Keystore could not be saved.");
                }
            }

        }

        public void CreateUser()
        {
            NewUserViewModel userDialog = new NewUserViewModel();
            WindowManager.ShowDialog(userDialog);

            if (!userDialog.NewUserCreated)
                return;

            Users.Add(new UserViewModel
            {
                FirstName = userDialog.FirstName,
                LastName = userDialog.LastName,
                Email = userDialog.Email,
                Role = userDialog.Role
            });

            FeedbackService.ShowInfoMessage("User created successfully!");
        }

        public void CreateCertificate()
        {
            List<string> avaliableCAs = new List<string>();

            foreach (var cert in Certificates.Where(c => c.IsCA))
                avaliableCAs.Add(string.Format("{0} ({1})", cert.Alias, cert.SerialNumber));

            NewCertificateViewModel certificateDialog = new NewCertificateViewModel(avaliableCAs);
            WindowManager.ShowDialog(certificateDialog);

            if (certificateDialog.IsCanceled)
                return;

            IList oids = new ArrayList();
            oids.Add(X509Name.CN);
            oids.Add(X509Name.OU);
            oids.Add(X509Name.O);
            oids.Add(X509Name.ST);
            oids.Add(X509Name.C);
            oids.Add(X509Name.E);
            oids.Add(X509Name.GivenName);


            IList values = new ArrayList();
            values.Add(certificateDialog.CommonName);
            values.Add(certificateDialog.OrganisationUnit);
            values.Add(certificateDialog.OrganisationName);
            values.Add(certificateDialog.StateName);
            values.Add(certificateDialog.CountryCode);
            values.Add(certificateDialog.EmailAddress);
            values.Add(certificateDialog.Alias);

            X509Name subjectName = new X509Name(oids, values);
            X509Certificate2 certificate = null;
            string issuerName = "";

            if (certificateDialog.SelectedCA == "Self signed")
            {
                if (certificateDialog.IsCertificateAuthority)
                    certificate = CertUtils.CreateCertificateAuthorityCertificate(subjectName, _keystore, _keystorePassword);
                else
                    certificate = CertUtils.CreateSelfSignedCertificate(subjectName, _keystore, _keystorePassword);

                issuerName = "Self signed";
            }
            else
            {
                certificate = CertUtils.IssueCertificate(subjectName, _certificates[certificateDialog.SelectedCA.Split('(').Last().Replace(")", "").Trim()],
                    _keystore, _keystorePassword, certificateDialog.IsCertificateAuthority);
                issuerName = certificateDialog.SelectedCA;
            }

            _certificates.Add(certificate.SerialNumber, certificate);

            Certificates.Add(new CertificateViewModel
            {
                Alias = certificateDialog.Alias,
                SerialNumber = certificate.SerialNumber,
                Issuer = issuerName,
                ValidFrom = certificate.NotBefore.ToString(),
                ValidUntil = certificate.NotAfter.ToString(),
                IsCA = certificateDialog.IsCertificateAuthority
            });

            _isKeystoreChanged = true;
        }

        public void ImportCertificates()
        {
            OpenKeystoreViewModel keystoreDialog = new OpenKeystoreViewModel();
            WindowManager.ShowDialog(keystoreDialog);

            if (keystoreDialog.IsCanceled)
                return;

            try
            {
                X509Certificate2Collection certificateCollection = new X509Certificate2Collection();
                certificateCollection.Import(keystoreDialog.KeystoreFilePath, keystoreDialog.Password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                foreach (var certificate in certificateCollection)
                {
                    _certificates.Add(certificate.SerialNumber, certificate);

                    string certAlias = certificate.FriendlyName.Split(',').First(s => s.Contains("GIVENNAME=")).Replace("GIVENNAME=", "").Trim();
                    string certIssuer = certificate.Issuer.Split(',').First(s => s.Contains("G=")).Replace("G=", "").Trim();

                    CertificateViewModel newCert = new CertificateViewModel
                    {
                        Alias = certAlias,
                        SerialNumber = certificate.SerialNumber,
                        Issuer = certAlias == certIssuer ? "Self signed" : certIssuer,
                        ValidFrom = certificate.NotBefore.ToString(),
                        ValidUntil = certificate.NotAfter.ToString(),
                        IsCA = true
                    };

                    Certificates.Add(newCert);

                    if (Certificates.Any(c => c.Alias == certIssuer))
                        newCert.Issuer += string.Format(" ({0})", Certificates.First(c => c.Alias == certIssuer).SerialNumber);

                    CertUtils.AddCertificateToStore(certificate, _keystore, _keystorePassword);
                }

            }
            catch
            {
                FeedbackService.ShowErrorMessage(string.Format("Could not open: '{0}'", keystoreDialog.KeystoreFilePath));
            }
        }

        public void ExportCertificate()
        {
            if (SelectedCertificate == null)
                return;

            string filePath = DialogService.SaveFileDialog(new Dictionary<string, object>
                {
                    {"Filter", "PFX|*.pfx" }
                });

            if (string.IsNullOrWhiteSpace(filePath))
                return;

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    Pkcs12Store keystore = new Pkcs12Store();
                    CertUtils.AddCertificateToStore(_certificates[SelectedCertificate.SerialNumber], keystore, _keystorePassword);
                    keystore.Save(fileStream, _keystorePassword.ToArray(), new SecureRandom());

                    FeedbackService.ShowInfoMessage("Certificate exported successfully!");
                }
                catch
                {
                    FeedbackService.ShowErrorMessage("Keystore could not be saved.");
                }
            }
        }

        public void About()
        {
            FeedbackService.ShowInfoMessage("Tool for maniging certificates and users in Parliament");
        }

        public void CloseApplication()
        {
            TryClose();
        }

        public override void CanClose(Action<bool> callback)
        {
            if (_isKeystoreChanged)
            {
                bool? result = FeedbackService.ShowWarningMessage("Would you like to save changes to current keystore first?");

                if (result.HasValue && result == true)
                    SaveKeystore();

                callback(result.HasValue);
            }
            else
            {
                callback(true);
            }
        }
    }
}
