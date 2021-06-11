using System.Reflection;
using System.Windows.Controls;

namespace Gh61.WYSitor.Code.Interop
{
    internal static class InteropHelper
    {
        public static bool TryGetInternalComWebBrowser(WebBrowser webBrowser, out object comWebBrowser)
        {
            comWebBrowser = null;

            // trying to get private field of COM component of browser
            var comWebBrowserField = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (comWebBrowserField == null)
            {
                // was not found - ending
                return false;
            }

            // getting private COM component of browser
            comWebBrowser = comWebBrowserField.GetValue(webBrowser);
            if (comWebBrowser == null)
            {
                // is null, ending
                return false;
            }

            // successfully found
            return true;
        }

        public static bool TryHideScriptErrors(WebBrowser webBrowser)
        {
            const bool hide = true;

            if (InteropHelper.TryGetInternalComWebBrowser(webBrowser, out var comWebBrowser))
            {
                // setting script errors to silent
                comWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] { hide });
                return true;
            }

            return false;
        }
    }
}
