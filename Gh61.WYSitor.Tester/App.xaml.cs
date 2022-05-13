using System.Windows;
using Gh61.WYSitor.Localization;

namespace Gh61.WYSitor.Tester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            // Custom resource manager
            ResourceManager.Managers.Insert(0, new MyResources());

            base.OnStartup(e);
        }
    }
}
