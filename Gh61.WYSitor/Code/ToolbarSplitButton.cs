using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// Toolbar element containing button and dropdown arrow.
    /// </summary>
    public abstract class ToolbarSplitButton : ToolbarElement
    {
        private readonly MainButtonElement _mainButton;
        private readonly DropDownButtonElement _dropDownButton;

        protected ToolbarSplitButton(string identifier, string name) : base(identifier)
        {
            _mainButton = new MainButtonElement(identifier, name, CreateMainButtonContent, CreateContextMenu, MainButtonClicked);
            _dropDownButton = new DropDownButtonElement(identifier, name, _mainButton);
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
        /// Not used method.
        /// </summary>
        protected override FrameworkElement CreateElement(IBrowserControl browserControl)
        {
            throw new InvalidOperationException("[ToolbarSplitButton] Invalid call to CreateElement. Use CreateElements method instead.");
        }

        /// <summary>
        /// Creates both main a dropdown button controls.
        /// </summary>
        public override IEnumerable<FrameworkElement> CreateElements(IBrowserControl browserControl)
        {
            yield return _mainButton.CreateElementInternal(browserControl);
            yield return _dropDownButton.CreateElementInternal(browserControl);
        }

        /// <summary>
        /// Class helping with standard button behaviour.
        /// </summary>
        private class MainButtonElement : ToolbarButton
        {
            private readonly Func<UIElement> _createButtonContent;
            private readonly Func<IBrowserControl, ContextMenu> _createContextMenu;

            public MainButtonElement(string identifier, string name, Func<UIElement> createButtonContent, Func<IBrowserControl, ContextMenu> createContextMenu, Action<IBrowserControl> onClick)
                : base(identifier + "_main", name, (UIElement)null, onClick)
            {
                _createButtonContent = createButtonContent;
                _createContextMenu = createContextMenu;
            }

            protected override object GetButtonContent() => _createButtonContent();

            public FrameworkElement CreateElementInternal(IBrowserControl browserControl) => CreateElement(browserControl);

            protected override FrameworkElement CreateElement(IBrowserControl browserControl)
            {
                var button = (Button)base.CreateElement(browserControl);
                button.Padding = new Thickness(2, 2, 0, 2);

                var contextMenu = _createContextMenu(browserControl);
                Utils.SetContextMenuAndPosition(contextMenu, button);

                // saving last created context menu, so dropdown button can manipulate with it
                LastContextMenu = contextMenu;

                return button;
            }

            public ContextMenu LastContextMenu
            {
                get;
                private set;
            }
        }

        /// <summary>
        /// Class providing dropdown button behaviour.
        /// </summary>
        private class DropDownButtonElement : ToolbarElement
        {
            private readonly MainButtonElement _mainButton;
            private readonly string _name;

            public DropDownButtonElement(string identifier, string name, MainButtonElement mainButton) : base(identifier + "_dropdown")
            {
                _mainButton = mainButton;
                _name = name;
            }

            public FrameworkElement CreateElementInternal(IBrowserControl browserControl) => CreateElement(browserControl);

            protected override FrameworkElement CreateElement(IBrowserControl browserControl)
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

                Utils.SetContextMenuOpening(_mainButton.LastContextMenu, button);

                return button;
            }
        }
    }
}
