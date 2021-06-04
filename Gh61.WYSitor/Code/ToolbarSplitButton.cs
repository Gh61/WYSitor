using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.ViewModels;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Toolbar element containing button and dropdown arrow.
    /// </summary>
    public abstract class ToolbarSplitButton
    {
        private readonly string _identifier;

        protected ToolbarSplitButton(string identifier, string name)
        {
            _identifier = identifier;

            MainButton = new MainButtonElement(identifier, name, CreateMainButtonContent, CreateContextMenu, MainButtonClicked);
            DropDownButton = new DropDownButtonElement(identifier, name, (MainButtonElement)MainButton);
        }

        /// <summary>
        /// Gets Toolbar element of main part of this SplitButton.
        /// </summary>
        public ToolbarButton MainButton
        {
            get;
        }

        /// <summary>
        /// Gets Toolbar element of dropdown part of this SplitButton.
        /// </summary>
        public ToolbarElement DropDownButton
        {
            get;
        }

        /// <summary>
        /// Registers this button on toolbar.
        /// </summary>
        public void Register(ToolbarViewModel model)
        {
            model.ToolbarElements.Add(MainButton);
            model.ToolbarElements.Add(DropDownButton);
        }

        /// <summary>
        /// Removes this button from toolbar.
        /// </summary>
        public void UnRegister(ToolbarViewModel model)
        {
            var mainName = MainButtonElement.GetIdentifier(_identifier);
            var dropName = DropDownButtonElement.GetIdentifier(_identifier);

            var mainIndex = model.ToolbarElements.IndexOf(e => e.Identifier == mainName);
            var dropIndex = model.ToolbarElements.IndexOf(e => e.Identifier == dropName);

            model.ToolbarElements.RemoveAt(mainIndex);
            model.ToolbarElements.RemoveAt(dropIndex);
        }

        /// <summary>
        /// Creates content for main part of this SplitButton.
        /// </summary>
        protected abstract UIElement CreateMainButtonContent();

        /// <summary>
        /// Action after main button is clicked.
        /// </summary>
        protected abstract void MainButtonClicked(IBrowserControl browserControl);

        /// <summary>
        /// Creates content of dropdown menu.
        /// </summary>
        protected abstract ContextMenu CreateContextMenu(IBrowserControl browserControl);

        /// <summary>
        /// Class helping with standard button behaviour.
        /// </summary>
        private class MainButtonElement : ToolbarButton
        {
            private readonly Func<UIElement> _createButtonContent;
            private readonly Func<IBrowserControl, ContextMenu> _createContextMenu;

            public MainButtonElement(string identifier, string name, Func<UIElement> createButtonContent, Func<IBrowserControl, ContextMenu> createContextMenu, Action<IBrowserControl> onClick)
                : base(GetIdentifier(identifier), name, (UIElement)null, onClick)
            {
                _createButtonContent = createButtonContent;
                _createContextMenu = createContextMenu;
            }

            protected override object GetButtonContent() => _createButtonContent();

            public override FrameworkElement CreateElement(IBrowserControl browserControl)
            {
                var button = (Button)base.CreateElement(browserControl);
                button.Padding = new Thickness(2, 2, 0, 2);

                var contextMenu = _createContextMenu(browserControl);
                contextMenu.HasDropShadow = false;
                contextMenu.PlacementTarget = button;
                contextMenu.Placement = PlacementMode.Left;
                contextMenu.Opened += (s, e) =>
                {
                    contextMenu.HorizontalOffset = -button.ActualWidth;
                    contextMenu.VerticalOffset = button.ActualHeight;
                };

                button.ContextMenu = contextMenu;

                // saving last created context menu, so dropdown button can manipulate with it
                LastContextMenu = contextMenu;

                return button;
            }

            public ContextMenu LastContextMenu
            {
                get;
                private set;
            }

            public static string GetIdentifier(string baseIdentifier) => baseIdentifier + "_main";
        }

        /// <summary>
        /// Class providing dropdown button behaviour.
        /// </summary>
        private class DropDownButtonElement : ToolbarElement
        {
            private readonly MainButtonElement _mainButton;
            private readonly string _name;

            public DropDownButtonElement(string identifier, string name, MainButtonElement mainButton) : base(GetIdentifier(identifier))
            {
                _mainButton = mainButton;
                _name = name;
            }

            public override FrameworkElement CreateElement(IBrowserControl browserControl)
            {
                /*
                 * <ToggleButton Margin="0" Padding="0,7,2,7">
                     <ContentControl Content="{StaticResource Icon_DropDownArrow}"></ContentControl>
                   </ToggleButton>
                 */

                var button = new ToggleButton();
                button.Margin = new Thickness(0);
                button.Padding = new Thickness(0, 7, 2, 7);
                button.Content = ResourceHelper.GetIcon("Icon_DropDownArrow");
                button.ToolTip = _name;

                button.Click += (s, e) =>
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    _mainButton.LastContextMenu.IsOpen = button.IsChecked.Value;
                };
                _mainButton.LastContextMenu.Closed += (s, e) => { button.IsChecked = false; };

                return button;
            }

            public static string GetIdentifier(string baseIdentifier) => baseIdentifier + "_dropdown";
        }
    }
}
