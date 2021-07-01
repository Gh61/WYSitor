# WYSitor
WYSitor is WYSIWYG HTML Editor for WPF.

### Preview
![](Readme/preview.png?raw=true)

## Why another editor?

### Nothing usable (for my project)
I needed WPF HTML editor for our aplication, so i started searching existing projects.
There are some editors in big library packs, that are priced too high for our ussage.
Then there is project [SmithHtmlEditor](https://github.com/adambarath/SmithHtmlEditor) wich I [forked and migrated to .NET 4.5](https://github.com/Gh61/EasyHtmlEditor) (for usage in our project). In my experience it was very unstable and need many fixes.

Then I found [this solution](https://www.codeproject.com/Tips/870549/Csharp-WPF-WYSIWYG-HTML-Editor) wich is inspiration for this project. *Though it's similar to SmithHtmlEditor, it's using native WPF WebBrowser control (not WindowsFormsHost) wich I hope will be more stable.* Inspiration for some core functionality comes also from [SmithHtmlEditor](https://github.com/adambarath/SmithHtmlEditor).

### Extensibility
Also I am planing to create much more friendly API, that will allow developers to work with this control more easily.
There will be API to show/hide/rearrange controls in toolbar, even adding your own, wich can then react with internal browser control.

## Features
- TODO

## Usage
TODO

## Known Issues
#### Visible part of editor is not working
*WYSIWYG part is completely white, but you can see in source code editor mode that it's working (when you try to type something).*

This problem occurs, when this component is used in window, that has `AllowTransparent` set to `True`.
This is caused by the fact, that this component is using .NET WPF WebBrowser control, wich is just wrapped ActiveX control and thus not rendered by WPF (not supporting transparency).
You can fix this easily by setting the `AllowTransparent` property to `False` in parent window. There are some workarounds wich you can use, but official statement is that this is not supported.
Some useful links, if you want to try your own solution on this problem:
- [MSDN Forum](https://social.msdn.microsoft.com/Forums/en-US/61a901d3-3273-4d8e-8e08-9441dc11010f/wpf-webbrowser-in-a-transparent-window?forum=wpf)
- [CodeProject](https://www.codeproject.com/Questions/217236/WPF-WebBrowser-Problem-if-AllowsTransparencyequals)
- [StackOverflow](https://stackoverflow.com/questions/19444457/c-sharp-wpf-displaying-webbrowser-on-window-with-allowtransparency-true-does-n)
