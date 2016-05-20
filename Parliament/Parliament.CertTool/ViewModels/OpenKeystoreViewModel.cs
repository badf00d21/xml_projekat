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
    public class OpenKeystoreViewModel : Screen
    {
        [Import]
        public IDialogService DialogService { get; set; }

        public string KeystoreFilePath { get; set; }

        public string Password { get; set; }

        public bool IsCanceled { get; set; }

        public OpenKeystoreViewModel()
        {
            IoC.BuildUp(this);

            DisplayName = "Open Keystore";
        }

        public void BrowseKeystoreLocation()
        {
            IEnumerable<string> selectedFiles = DialogService.OpenFileDialog(new Dictionary<string, object>
            {
                {"Filter", "PFX|*.pfx" }
            });

            if (selectedFiles != null && selectedFiles.Any() && !string.IsNullOrWhiteSpace(selectedFiles.First()))
                KeystoreFilePath = selectedFiles.First();
        }

        public void Ok()
        {
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
