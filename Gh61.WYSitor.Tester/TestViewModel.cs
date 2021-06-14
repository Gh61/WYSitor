using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gh61.WYSitor.Tester
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public TestViewModel()
        {
            // Default value
            _sourceCode = @"
<HEAD><TITLE>doc</TITLE>
<META content=text/html;utf-8 http-equiv=content-type>
<STYLE type=text/css>ol,p,ul{font-family:Calibri,Candara,Segoe,Segoe UI,Optima,Arial,sans-serif}p{margin-bottom:0;margin-top:0}</STYLE>
</HEAD>
<BODY>
<P align=center><FONT style=""BACKGROUND-COLOR: #ffff00"">H<STRONG>a</STRONG></FONT>h<EM><FONT color=#ff0000>a</FONT></EM>h<U>a</U>h</P></BODY>";
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
