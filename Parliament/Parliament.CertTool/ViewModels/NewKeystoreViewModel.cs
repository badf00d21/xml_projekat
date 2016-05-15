using Caliburn.Micro;
using SwollenMvvmToolkit.Core.Services;
using Syringe.ObservableClassAmpoule;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.CertTool.ViewModels
{
    [Observable]
    public class NewKeystoreViewModel : Screen
    {
        [Import]
        public IDialogService DialogService { get; set; }

        [Import]
        public IFeedbackService FeedbackService { get; set; }

        public string KeystoreFilePath { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool IsCanceled { get; set; }

        public NewKeystoreViewModel()
        {
            IoC.BuildUp(this);

            DisplayName = "New Keystore";
        }

        public void BrowseKeystoreLocation()
        {
            string filePath = DialogService.SaveFileDialog(new Dictionary<string, object>
            {
                {"Filter", "PFX|*.pfx" }
            });

            if (!string.IsNullOrWhiteSpace(filePath))
                KeystoreFilePath = filePath;
        }

        public void Ok()
        {
            if (Password != ConfirmPassword)
            {
                FeedbackService.ShowErrorMessage("Passwords do not match!");
                return;
            }

            IsCanceled = false;
            TryClose();
        }

        public void Cancel()
        {
            IsCanceled = true;
            TryClose();
        }
    }
}
