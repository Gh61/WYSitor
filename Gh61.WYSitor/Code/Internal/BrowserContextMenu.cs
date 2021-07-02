using System.Windows.Controls;
using System.Windows.Input;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Localization;
using Gh61.WYSitor.Views;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Custom WebBrowser context menu.
    /// </summary>
    internal class BrowserContextMenu
    {
        private readonly IBrowserControl _browser;
        private readonly ContextMenu _contextMenu;

        public BrowserContextMenu(EditorBrowser browserControl)
        {
            _browser = browserControl;

            _contextMenu = CreateContextMenu();
            browserControl.ContextMenu = _contextMenu;
        }

        public bool IsOpen
        {
            get => _contextMenu.IsOpen;
            set => _contextMenu.IsOpen = value;
        }

        private ContextMenu CreateContextMenu()
        {
            var contextMenu = new ContextMenu();

            BindCommands(contextMenu);

            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Undo});
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Redo});
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Cut});
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Copy});
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Paste});
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.Delete, Header = ResourceManager.ContextMenu_Delete}); // Delete is not localized in default
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(new MenuItem() {Command = ApplicationCommands.SelectAll});

            return contextMenu;
        }

        private void BindCommands(ContextMenu contextMenu)
        {
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Undo, "Undo"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Redo, "Redo"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Cut,  "Cut"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Copy, "Copy"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Paste,"Paste"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.Delete, "Delete"));
            contextMenu.CommandBindings.Add(CreateBinding(ApplicationCommands.SelectAll, "SelectAll", false));
        }

        private CommandBinding CreateBinding(RoutedUICommand command, string browserCommandName, bool checkCanExecute = true)
        {
            var commandBinding = new CommandBinding(command);
            if (checkCanExecute)
            {
                commandBinding.CanExecute += (s, e) =>
                {
                    e.CanExecute = _browser.QueryCommandEnabled(browserCommandName);
                };
            }
            commandBinding.Executed += (s, e) =>
            {
                _browser.ExecuteCommand(browserCommandName);
            };

            return commandBinding;
        }
    }
}
