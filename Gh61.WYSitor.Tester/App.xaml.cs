using System.Globalization;
using System.Threading;
using System.Windows;

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

            base.OnStartup(e);
        }
    }
}
