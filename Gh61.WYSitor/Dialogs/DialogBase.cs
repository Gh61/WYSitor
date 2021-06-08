using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Gh61.WYSitor.Code;

namespace Gh61.WYSitor.Dialogs
{
    public abstract class DialogBase : Window, INotifyPropertyChanged
    {
        protected DialogBase()
        {
            this.HideIcon();
            ApplyStyles();
        }

        protected void ApplyStyles()
        {
            Resources.MergedDictionaries.Add(ResourceHelper.Styles.Value);

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;
        }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected void OkClicked(object sender, RoutedEventArgs e) => DialogResult = true;

        protected void CancelClicked(object sender, RoutedEventArgs e) => DialogResult = false;

        protected void TextBoxSelectOnFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAllText();
            }
        }

        protected void LabelClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label && label.Target != null)
            {
                label.Target.Focus();
            }
        }
    }
}
