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
    public class CertificateViewModel : ViewModel
    {
        public string SerialNumber { get; set; }

        public string Alias { get; set; }

        public string Issuer { get; set; }

        public string ValidFrom { get; set; }

        public string ValidUntil { get; set; }
    }
}
