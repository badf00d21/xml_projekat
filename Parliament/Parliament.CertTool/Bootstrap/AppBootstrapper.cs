using Caliburn.Micro;
using SwollenMvvmToolkit.CaliburnMicro.Bootstrappers;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using SwollenMvvmToolkit.Core.Services;
using Parliament.CertTool.ViewModels;

namespace Parliament.CertTool.Bootstrap
{
    public class AppBootstrapper : MefBootstrapper
    {
        /// <summary>
        /// Defines application startup behavior.
        /// </summary>
        /// <param name="sender"> Object that raised event </param>
        /// <param name="e"> Event arguments </param>
        protected override void OnStartup(object sender, StartupEventArgs args)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        /// <summary>
        /// Configure behavior of Caliburn.Micro framework
        /// </summary>
        protected override void Configure()
        {
            base.Configure();

            ViewLocator.NameTransformer.AddRule
            (
                @"(?<namespace>(.*\.)*)ViewModels\.(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                @"${namespace}Views.${basename}Window"
            );
        }

        /// <summary>
        /// Creates initial batch for composition container.
        /// </summary>
        /// <returns> Created composition batch. </returns>
        protected override CompositionBatch CreateCompositionBatch()
        {
            CompositionBatch batch = base.CreateCompositionBatch();

            batch.AddExportedValue<IFeedbackService>(new MessageBoxFeedback());
            batch.AddExportedValue<IDialogService>(new WindowsDialogService());

            return batch;
        }
    }
}
