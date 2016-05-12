using SwollenMvvmToolkit.CaliburnMicro.ViewModels;
using Syringe.ObservableClassAmpoule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parliament.CertTool.ViewModels
{
    [Observable]
    public class KeyPairViewModel : ViewModel
    {
        public string AliasName { get; set; }

        public string LastModified { get; set; }
    }
}
