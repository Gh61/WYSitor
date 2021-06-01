# WYSitor
WYSitor is WYSIWYG HTML Editor for WPF.

## Why another editor?

### Nothing usable (for my project)
I needed WPF HTML editor for our aplication, so i started searching existing projects.
There are some editors in big library packs, that are priced too high for our ussage.
Then there is project [SmithHtmlEditor](https://github.com/adambarath/SmithHtmlEditor) wich I [forked and migrated to .NET 4.5](https://github.com/Gh61/EasyHtmlEditor) (for usage in our project). In my experience it was very unstable and need many fixes.

Then I found [this solution](https://www.codeproject.com/Tips/870549/Csharp-WPF-WYSIWYG-HTML-Editor) wich is inspiration for this project. *Though it's similar to SmithHtmlEditor, it's using native WPF WebBrowser control (not WindowsFormsHost) wich I hope will be more stable.*

### Extensibility
Also I am planing to create much more friendly API, that will allow developers to work with this control more easily.
There will be API to show/hide/rearrange controls in toolbar, even adding your own.
Also there will be API to work directly with inner WebBrowser control.
