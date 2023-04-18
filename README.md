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

## Features
- stable WYSIWYG HTML editor
- standard WPF control
- HtmlContent TwoWay Binding
- Customizable Toolbar

## Usage
### Standard XAML usage:
```xml
<wySitor:HtmlEditor x:Name="editor" Margin="0 12 0 0" HtmlContent="{Binding SourceCode}"/>
```

*You can of course create this element using code behind and adding it to the view later.*

## Change Toolbar
**Enable/Disable OverflowMode**

With this property you can enable (or disable) the overflow behaviour of Toolbar.
When this is set, all controls that will not fit on the toolbar with current size, will be polaced to oveflow menu (opened via small arrow at the end of toolbar).
```csharp
using Gh61.WYSitor.Code;

// `editor` variable is instance of HtmlEditor from xaml view
editor.Toolbar.EnableOverflowMode = true;
```
*Default value is `false`*

**This code applies to all further examples**
```csharp
using Gh61.WYSitor.Code;

// `editor` variable is instance of HtmlEditor from xaml view
private ObservableCollection<ToolbarElement> ToolbarItems => editor.Toolbar.ToolbarElements;
```

### Get toolbar element
You can get any default button with helper method **ToolbarCommands.Get**
```csharp
var underlineButton = ToolbarCommands.Get(StandardToolbarElement.Underline, editor.Toolbar);
```

Then you can remove this button:
### Remove button
```csharp
private void RemoveUnderlineButton()
{
    var underlineButton = ToolbarCommands.Get(StandardToolbarElement.Underline, editor.Toolbar);
    ToolbarItems.Remove(underlineButton);
}
```
### Change position of buttons
If you want to switch position of some buttons, you can search their index by using their **[Identifier](#manually-identify-toolbar-elements)**
```csharp
private void SwitchBoldAndItalic()
{
    var boldIndex = ToolbarItems.IndexOf(e => e.Identifier == nameof(StandardToolbarElement.Bold));
    var italicIndex = ToolbarItems.IndexOf(e => e.Identifier == nameof(StandardToolbarElement.Italic));

    ToolbarItems.Move(boldIndex, italicIndex);
}
```
### Replace separator
If you want to do something with separators, you need to find them by class **ToolbarSeparatorElement** or by **Indentifier** "Separator{number}". *See [Manually identify toolbar elements](#manually-identify-toolbar-elements)*
```csharp
private void ReplaceSeparator()
{
    var sepIndex = ToolbarItems.IndexOf(e => e is ToolbarSeparatorElement);
    ToolbarItems[sepIndex] = new ToolbarButton("HA", "Custom Separator", new Run("|"), c => { });
}
```

#### Manually identify toolbar elements
- all default toolbar buttons have predictable **Identifier** from *StandardToolbarElement* enum
- **separators** can be identified as *ToolbarSeparatorElement* class
  - they are using Identifiers "Separator{number}" where number is counted from 1 and increasing with every new ToolbarSeparatorElement instance

```csharp
var boldIndex = ToolbarItems.IndexOf(e => e.Identifier == nameof(StandardToolbarElement.Bold));
var firstSepIndex = ToolbarItems.IndexOf(e => e is ToolbarSeparatorElement);
```

## Custom Toolbar Elements
You can add your own elements to toolbar. It waill have access to internal browser behind the [IBrowserControl interface](Gh61.WYSitor/Interfaces/IBrowserControl.cs)
- **ToolbarButton** class
  - can contain any content you want (and there are some prepared icons in `ResourceHelper.Icon_XXX`)
  - can be toggleable
  - can work with browser on click
  - can has disabled automatic focus to browser after click
  - can be enabled in source mode 
- **ToolbarSeparatorElement** class
  - simple create new instance and add it anywhere to ToolbarElements
- **ToolbarSplitButton** class
  - you need to implement this on your own
  - override methods to:
    - create content of main button
    - click on main button
    - create content of dropdown menu
  - can be enabled in source mode 
- **ToolbarElement** class
  - you can implement your very own ToolbarElement that can do anything

### Examples of custom **ToolbarButton**:

#### Simple button
```csharp
var signatureButton = new ToolbarButton(
                "Signature", // Identifier
                "Inserts signature markup", // Title - for tooltip
                ResourceHelper.Icon_Signature, // Content of button (icon)
                b => // action on button click
                {
                    b.GetSelectedRange().PasteHtml("[SIGNATURE]");
                });
ToolbarItems.Add(signatureButton);
```

#### Custom html code preview window
```csharp
SourceCodePreview preview = null;
var previewButton = new ToolbarButton(
    "SourceCodePreview", // Identifier
    "Source code Preview", // Title - for tooltip
    new Bold(new Run("HTML")), // Content of button - bold "HTML" text
    b => // create new window on click and show html preview
    {
        if (preview == null)
        {
            preview = new SourceCodePreview((TestViewModel)this.DataContext);
            preview.Closed += (s, e) =>
            {
                preview = null;
            };
        }
        preview.Show();
        preview.Focus();
    },
    b => preview?.IsVisible == true // Toggle indicator
);
previewButton.DisableEditorFocusAfterClick = true; // will not set focus into editor, when clicken on this button
previewButton.EnabledInSourceMode = true; // this button will be enabled in source mode

ToolbarItems.Add(previewButton);
```

## Localization
Default implementation contains library Gh61.WYSitor.Locale.Default.dll, where is localization for these languages:
- `cs` (Czech)
- ... possibly more to come?
- 
*If you don't want or need this localization, simply drop reference to this DLL.*

**Your own localization:**

You can localize WYSitor easily any way you want with one of these two methods:

### Assigning ResourceManager in code
1. Implement [IResourceManager intarface](Gh61.WYSitor/Localization/IResourceManager.cs)
2. Create instance of your manager
3. Add it to `Gh61.WYSitor.Localization.ResourceManager.Managers` list (preferably at the beginning).

```csharp
using Gh61.WYSitor.Localization;

internal class MyResources : IResourceManager
{ ... }

// Add this to AppStart or somewhere before first HtmlEditor instance is created
ResourceManager.Managers.Insert(0, new MyResources());
```

### Loading ResourceManager from external DLL (Gh61.WYSitor.Locale.XXX.dll)
1. Create project named Gh61.WYSitor.Locale.XXX.dll (where XXX is anything you want)
2. In this project create public class implementing [IResourceManager intarface](Gh61.WYSitor/Localization/IResourceManager.cs)
3. This class needs to have parameterless constructor
4. Place DLL in same folder as Gh61.WYSitor.dll (basicaly just add this project as reference to project, where you are using HtmlEditor)

## Known Issues
### Visible part of editor is not working
*WYSIWYG part is completely white, but you can see in source code editor mode that it's working (when you try to type something).*

This problem occurs, when this component is used in window, that has `AllowTransparent` set to `True`.
This is caused by the fact, that this component is using .NET WPF WebBrowser control, wich is just wrapped ActiveX control and thus not rendered by WPF (not supporting transparency).
You can fix this easily by setting the `AllowTransparent` property to `False` in parent window. There are some workarounds wich you can use, but official statement is that this is not supported.
Some useful links, if you want to try your own solution on this problem:
- [MSDN Forum](https://social.msdn.microsoft.com/Forums/en-US/61a901d3-3273-4d8e-8e08-9441dc11010f/wpf-webbrowser-in-a-transparent-window?forum=wpf)
- [CodeProject](https://www.codeproject.com/Questions/217236/WPF-WebBrowser-Problem-if-AllowsTransparencyequals)
- [StackOverflow](https://stackoverflow.com/questions/19444457/c-sharp-wpf-displaying-webbrowser-on-window-with-allowtransparency-true-does-n)
