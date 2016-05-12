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
        public BindableCollection<KeyPairViewModel> KeyPairs { get; set; }

        public string KeyStoreInfo { get; set; }

        public MainViewModel()
        {
            KeyPairs = new BindableCollection<KeyPairViewModel>();
            KeyStoreInfo = "Keystore type: ";
        }

        public void NewKeystore()
        {
            KeyPairs = new BindableCollection<KeyPairViewModel>();
            KeyStoreInfo = "Keystore type: ";
        }
    }
}
