using Caliburn.Micro;
using SwollenMvvmToolkit.CaliburnMicro.ViewModels;
using Syringe.ObservableClassAmpoule;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.CertTool.ViewModels
{
    [Export]
    [Observable]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : ViewModel
    {
        public IWindowManager WindowManager { get; set; }

        public BindableCollection<CertificateViewModel> Certificates { get; set; }

        public BindableCollection<UserViewModel> Users { get; set; }

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager)
        {
            WindowManager = windowManager;

            Certificates = new BindableCollection<CertificateViewModel>();
            Users = new BindableCollection<UserViewModel>();
        }

        public void NewKeystore()
        {
            
        }

        public void OpenKeystore()
        {

        }

        public void CreateUser()
        {

        }

        public void CreateCertificate()
        {

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

        }
    }
}
