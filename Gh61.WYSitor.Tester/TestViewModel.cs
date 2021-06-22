using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gh61.WYSitor.Tester
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private const string DefaultHtml = @"<html><head><META content=text/html;utf-8 http-equiv=content-type></head><body style=""font-family:Calibri"">
<P align=""center"">
<span style=""color:#ff0000"">c</span><span style=""color:#ff8000"">o</span><span style=""color:#c0c000"">l</span><span style=""color:#00ff00"">o</span><span style=""color:#00ffff"">r</span><span style=""color:#0000ff"">f</span><span style=""color:#ff00ff"">u</span><span style=""color:#c00000"">l</span>
<span style=""color:#ffffff""><span style=""BACKGROUND-COLOR: #ff0000"">p</span><span style=""BACKGROUND-COLOR: #ff8000"">r</span><span style=""BACKGROUND-COLOR: #ffff00;color:#000000"" >i</span><span style=""BACKGROUND-COLOR: #00ff00"">n</span><span style=""BACKGROUND-COLOR: #00ffff"">c</span><span style=""BACKGROUND-COLOR: #0000ff"">e</span><span style=""BACKGROUND-COLOR: #ff00ff"">s</span><span style=""BACKGROUND-COLOR: #c00000"">s</span></span>
</P>
</body></html>";

        public TestViewModel()
        {
            // Default value
            _sourceCode = DefaultHtml;
        }

        private string _sourceCode;
        public string SourceCode
        {
            get => _sourceCode;
            set
            {
                if (_sourceCode == value) return;
                _sourceCode = value;
                OnPropertyChanged();
            }
        }

        public void RestoreDefaultContent()
        {
            SourceCode = DefaultHtml;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
