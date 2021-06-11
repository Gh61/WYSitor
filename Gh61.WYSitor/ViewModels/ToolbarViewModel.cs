using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Views;
using VisibleElement = System.Tuple<Gh61.WYSitor.Code.ToolbarElement, System.Windows.FrameworkElement>;

namespace Gh61.WYSitor.ViewModels
{
    public class ToolbarViewModel
    {
        private readonly EditorBrowser _browser;
        private DispatcherTimer _styleCheckTimer;
        private ItemsControl _container;

        internal ToolbarViewModel(EditorBrowser browser)
        {
            _browser = browser;
            _elements = new Dictionary<string, Tuple<ToolbarElement, FrameworkElement>>();
            ToolbarElements = new ObservableCollection<ToolbarElement>();
            ToolbarElements.CollectionChanged += ToolbarChanged;

            InitStyleCheckTimer();
        }

        /// <summary>
        /// Sets toolbar container, where <see cref="ToolbarElements"/> will be managed.
        /// </summary>
        internal void SetToolbarContainer(ItemsControl container)
        {
            // changing container means moving all elements to new container
            List<object> oldItems = null;
            if (_container != null)
            {
                oldItems = _container.Items.Cast<object>().ToList();
                _container.Items.Clear();
            }

            _container = container;
            _container.Items.Clear();

            // filling new container with old items
            oldItems?.ForEach(i => _container.Items.Add(i));
        }

        /// <summary>
        /// Gets toolbar container, where are created Toolbar Elements.
        /// </summary>
        /// <returns></returns>
        internal ItemsControl GetToolbarContainer() => _container;

        /// <summary>
        /// Collection of all toolbar elements is it's shown in toolbar above the browser.
        /// Adding, removing and changing position reflects on toolbar UI.
        /// </summary>
        public ObservableCollection<ToolbarElement> ToolbarElements
        {
            get;
        }

        #region Manage toolbar items

        private readonly IDictionary<string, VisibleElement> _elements;

        private void ToolbarChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // just to be sure
            if (_container == null)
                _container = new ItemsControl();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:

                    if (e.OldItems != null)
                    {
                        foreach (ToolbarElement item in e.OldItems)
                        {
                            var visibleElement = _elements[item.Identifier];

                            _container.Items.Remove(visibleElement.Item2);
                            _elements.Remove(item.Identifier);
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ToolbarElement item in e.NewItems)
                        {
                            var uiElement = item.CreateElement(_browser);

                            // so the element never overflow the toolbar (where overflow toggle button is hidden)
                            ToolBar.SetOverflowMode(uiElement, OverflowMode.Never);

                            // custom exception - better searching for error
                            if(_elements.ContainsKey(item.Identifier))
                                throw new InvalidOperationException($"Cannot add second toolbar element with identifier '{item.Identifier}'.");

                            // saving for fast access
                            _elements[item.Identifier] = new VisibleElement(item, uiElement);

                            // adding to container
                            if (e.NewStartingIndex < 0) // just to be sure
                            {
                                _container.Items.Add(uiElement);
                            }
                            else
                            {
                                _container.Items.Insert(e.NewStartingIndex, uiElement);
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    // since we're using default ObservableCollection, Reset is only called when collection is Cleared
                    _container.Items.Clear();
                    _elements.Clear();

                    // waiting for elements to be removed from visual tree
                    _container.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);

                    break;
            }
        }

        #endregion

        #region Style-check timer

        private void InitStyleCheckTimer()
        {
            _styleCheckTimer = new DispatcherTimer();
            _styleCheckTimer.Interval = TimeSpan.FromMilliseconds(200);
            _styleCheckTimer.Tick += TryCheckStyle;

            // Timer running only when browser is loaded
            _browser.Unloaded += (s, e) => _styleCheckTimer.Stop();
            _browser.Loaded += (s, e) => _styleCheckTimer.Start();
            if (_browser.IsLoaded)
            {
                _styleCheckTimer.Start();
            }
        }

        private void TryCheckStyle(object sender, EventArgs e)
        {
            // no need to check if editor is invisible
            if (!_browser.IsVisible)
                return;

            if (!_browser.CurrentDocument.IsCompletelyLoaded())
                return;

            // going through all CheckState elements and calling CheckState
            foreach (var visibleElement in _elements.Values.Where(el => el.Item1.EnableCheckState))
            {
                visibleElement.Item1.CheckState(visibleElement.Item2, _browser);
            }
        }

        #endregion
    }
}
