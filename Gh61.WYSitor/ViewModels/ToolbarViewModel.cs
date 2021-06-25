using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Views;
using VisibleElement = System.Tuple<Gh61.WYSitor.Code.ToolbarElement, System.Collections.Generic.IReadOnlyCollection<System.Windows.FrameworkElement>>;

namespace Gh61.WYSitor.ViewModels
{
    public class ToolbarViewModel
    {
        private readonly EditorBrowser _browserControl;
        private DispatcherTimer _styleCheckTimer;
        private ItemsControl _container;

        internal ToolbarViewModel(EditorBrowser browser)
        {
            _browserControl = browser;
            _elements = new Dictionary<string, VisibleElement>();
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

                            // removing all created elements
                            foreach (var controlItem in visibleElement.Item2)
                            {
                                _container.Items.Remove(controlItem);
                            }
                            _elements.Remove(item.Identifier);
                        }
                    }

                    if (e.NewItems != null)
                    {
                        int? realInsertIndex = null;

                        foreach (ToolbarElement item in e.NewItems)
                        {
                            // custom exception - better searching for error
                            if(_elements.ContainsKey(item.Identifier))
                                throw new InvalidOperationException($"Cannot add second toolbar element with identifier '{item.Identifier}'.");

                            // create all elements
                            var uiElements = item.CreateElements(_browserControl).ToList().AsReadOnly();

                            // saving for fast access
                            _elements[item.Identifier] = new VisibleElement(item, uiElements);

                            // so the elements never overflow the toolbar (where overflow toggle button is hidden)
                            foreach (var uiElement in uiElements)
                            {
                                ToolBar.SetOverflowMode(uiElement, OverflowMode.Never);
                            }

                            // adding to container
                            if (e.NewStartingIndex < 0 || e.NewStartingIndex + e.NewItems.Count == ToolbarElements.Count) // just to be sure
                            {
                                // adding on the end - easy
                                foreach (var uiElement in uiElements)
                                {
                                    _container.Items.Add(uiElement);
                                }
                            }
                            else
                            {
                                // need to find the real index where tu put these items
                                if (realInsertIndex == null)
                                {
                                    realInsertIndex = ToolbarElements.Take(e.NewStartingIndex).Sum(te => _elements[te.Identifier].Item2.Count);
                                }

                                // adding on index and incrementing
                                foreach (var uiElement in uiElements)
                                {
                                    _container.Items.Insert(realInsertIndex.Value, uiElement);

                                    // increment
                                    realInsertIndex = realInsertIndex.Value + 1;
                                }
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

        /// <summary>
        /// Will set this toolbar to source edit mode.
        /// </summary>
        /// <param name="enabled">true = source mode, false = WYSIWYG</param>
        internal void SetSourceMode(bool enabled)
        {
            if (_container == null)
                return;

            var toolbarItems = _elements.Values
                .Where(v => !v.Item1.EnabledInSourceMode)
                .SelectMany(v => v.Item2)
                .ToList();

            toolbarItems.ForEach(i => i.IsEnabled = !enabled);
        }

        #endregion

        #region Style-check timer

        private void InitStyleCheckTimer()
        {
            _styleCheckTimer = new DispatcherTimer();
            _styleCheckTimer.Interval = TimeSpan.FromMilliseconds(200);
            _styleCheckTimer.Tick += TryCheckStyle;

            // Timer running only when browser is loaded
            _browserControl.Unloaded += (s, e) => _styleCheckTimer.Stop();
            _browserControl.Loaded += (s, e) => _styleCheckTimer.Start();
            if (_browserControl.IsLoaded)
            {
                _styleCheckTimer.Start();
            }
        }

        private void TryCheckStyle(object sender, EventArgs e)
        {
            // no need to check if control is invisible
            if (!_browserControl.IsVisible)
                return;

            if (!_browserControl.CurrentDocument.IsCompletelyLoaded())
                return;

            var items = _elements.Values.Where(el => el.Item1.EnableCheckState);

            // browser is invisible
            if (!_browserControl.Browser.IsVisible)
            {
                items = items.Where(el => el.Item1.EnabledInSourceMode);
            }

            // going through all CheckState elements and calling CheckState
            foreach (var visibleElement in items)
            {
                visibleElement.Item1.CheckState(visibleElement.Item2, _browserControl);
            }
        }

        #endregion
    }
}
