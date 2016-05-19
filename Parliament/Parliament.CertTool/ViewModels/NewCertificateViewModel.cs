using Caliburn.Micro;
using Syringe.ObservableClassAmpoule;

namespace Parliament.CertTool.ViewModels
{
    [Observable]
    public class NewCertificateViewModel : Screen
    {
        public BindableCollection<int> CAs { get; set; }

        public string Surname { get; set; }

        public string CommonName { get; set; }

        public string OrganisationUnit { get; set; }

        public string OrganisationName { get; set; }

        public string GivenName { get; set; }

        public string StateName { get; set; }

        public string CountryCode { get; set; }

        public string EmailAddress { get; set; }

        public bool IsCanceled { get; set; }

        public NewCertificateViewModel()
        {
            DisplayName = "Create New Certificate";
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
