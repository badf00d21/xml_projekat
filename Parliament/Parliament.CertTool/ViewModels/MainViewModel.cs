using Caliburn.Micro;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Parliament.DataAccess.Databases;
using SwollenMvvmToolkit.CaliburnMicro.ViewModels;
using SwollenMvvmToolkit.Core.Services;
using Syringe.ObservableClassAmpoule;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

        public IWindowManager WindowManager { get; set; }
        public IFeedbackService FeedbackService { get; set; }
        public IDialogService DialogService { get; set; }

        public BindableCollection<CertificateViewModel> Certificates { get; set; }

        public BindableCollection<UserViewModel> Users { get; set; }
        public object X509V3CertificateGenerator { get; private set; }

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager, IFeedbackService feedbackService, IDialogService dialogService)
        {
            _isKeystoreChanged = false;

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
            }          
        }

        public void OpenKeystore()
        {

        }

        public void SaveKeystore()
        {

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
            NewCertificateViewModel certificateDialog = new NewCertificateViewModel();
            WindowManager.ShowDialog(certificateDialog);

            if (certificateDialog.IsCanceled)
                return;
        }

        public void ImportCertificate()
        {

        }

        public void ExportCertificate()
        {

        }

        public void About()
        {

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
        }
    }
}
