using Caliburn.Micro;
using Microsoft.AspNet.Identity;
using Parliament.DataAccess.Database;
using Parliament.DataAccess.Entities;
using Parliament.DataAccess.Utils;
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
    public class NewUserViewModel : Screen
    {
        [Import]
        public IFeedbackService FeedbackService;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public BindableCollection<string> Roles { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool NewUserCreated { get; set; }

        public NewUserViewModel()
        {
            IoC.BuildUp(this);

            Roles = new BindableCollection<string>();

            Roles.Add(ParliamentRole.Alderman.ToString());
            Roles.Add(ParliamentRole.Citizen.ToString());
            Roles.Add(ParliamentRole.Chairman.ToString());

            DisplayName = "Create New User";

            NewUserCreated = false;
        }

        public async void Ok()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)
                || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                FeedbackService.ShowErrorMessage("Please fill all fields!");
                return;
            }

            if (Password != ConfirmPassword)
            {
                FeedbackService.ShowErrorMessage("Passwords do not match!");
                return;
            }

            using (var dbContext = new ParliamentDbContext())
            {
                using (var userManager = new ParliamentUserManager(dbContext))
                {
                    ParliamentUser user = await userManager.FindByEmailAsync(Email);

                    if (user != null)
                    {
                        FeedbackService.ShowErrorMessage("Email address is alrady registered.");
                        return;
                    }

                    ParliamentUser newUser = new ParliamentUser(FirstName, LastName, Email, (ParliamentRole)Enum.Parse(typeof(ParliamentRole), Role));

                    IdentityResult registrationResult = await userManager.CreateAsync(newUser, Password);

                    if (registrationResult.Errors.Any())
                    {
                        FeedbackService.ShowErrorMessage(registrationResult.Errors.First());
                        return;
                    }

                    await userManager.AddToRoleAsync(newUser.Id, Role);
                    NewUserCreated = true;
                }
            }

            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
