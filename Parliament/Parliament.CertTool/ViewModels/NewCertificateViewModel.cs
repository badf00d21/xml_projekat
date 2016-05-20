using Caliburn.Micro;
using Syringe.ObservableClassAmpoule;
using System.Collections.Generic;
using System.Linq;

namespace Parliament.CertTool.ViewModels
{
    [Observable]
    public class NewCertificateViewModel : Screen
    {
        public BindableCollection<string> CAs { get; set; }

        public string SelectedCA { get; set; }

        public string CommonName { get; set; }

        public string OrganisationUnit { get; set; }

        public string OrganisationName { get; set; }

        public string Alias { get; set; }

        public string StateName { get; set; }

        public string CountryCode { get; set; }

        public string EmailAddress { get; set; }

        public bool IsCertificateAuthority { get; set; }

        public bool IsCanceled { get; set; }

        public NewCertificateViewModel(IEnumerable<string> issuers)
        {
            DisplayName = "Create New Certificate";

            CAs = new BindableCollection<string>();
            CAs.Add("Self signed");
            CAs.AddRange(issuers);

            SelectedCA = CAs.First();
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
