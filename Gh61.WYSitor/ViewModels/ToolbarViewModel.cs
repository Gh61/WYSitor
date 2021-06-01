using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Gh61.WYSitor.Code;
using Gh61.WYSitor.Interfaces;
using Gh61.WYSitor.Views;
using VisibleElement = System.Tuple<Gh61.WYSitor.Code.ToolbarElement, System.Windows.FrameworkElement>;

namespace Gh61.WYSitor.ViewModels
{
    public class ToolbarViewModel
    {
        private readonly IBrowserControl _browser;
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
                // new items
                case NotifyCollectionChangedAction.Add:
                    foreach (ToolbarElement item in e.NewItems)
                    {
                        var element = item.CreateElement(_browser);

                        // saving for fast access
                        _elements.Add(item.Identifier, new VisibleElement(item, element));

                        // adding to container
                        _container.Items.Insert(e.NewStartingIndex, element);
                    }
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
            if (_browser.CurrentDocument.readyState != "complete")
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
