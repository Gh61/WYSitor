using System.Collections.Generic;
using System.Windows;
using Gh61.WYSitor.Interfaces;

namespace Gh61.WYSitor.Code
{
    public abstract class ToolbarElement
    {
        protected ToolbarElement(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Gets unique identifier - can be only one in whole toolbar.
        /// </summary>
        public string Identifier
        {
            get;
        }

        /// <summary>
        /// Creates UI control of this element.
        /// </summary>
        /// <param name="browserControl">BrowserControl - to access browser functions.</param>
        protected abstract FrameworkElement CreateElement(IBrowserControl browserControl);

        /// <summary>
        /// Creates UI controls of this element.
        /// </summary>
        /// <param name="browserControl">BrowserControl - to access browser functions.</param>
        public virtual IEnumerable<FrameworkElement> CreateElements(IBrowserControl browserControl)
        {
            yield return CreateElement(browserControl);
        }

        /// <summary>
        /// Enables periodical calling of <see cref="CheckState(IReadOnlyCollection{FrameworkElement}, IBrowserControl)"/>.
        /// </summary>
        public virtual bool EnableCheckState { get; protected set; }

        /// <summary>
        /// Gets or sets whether this element should remain enabled in source edit mode.
        /// </summary>
        public virtual bool EnabledInSourceMode { get; set; }

        /// <summary>
        /// Called periodically (when <see cref="EnableCheckState"/> = true), to allow this element to check its state.
        /// </summary>
        /// <param name="elements">All elements created by <see cref="CreateElements"/>.</param>
        /// <param name="browserControl">BrowserControl - to access browser functions.</param>
        public virtual void CheckState(IReadOnlyCollection<FrameworkElement> elements, IBrowserControl browserControl)
        {
            foreach (var element in elements)
            {
                CheckState(element, browserControl);
            }
        }

        /// <summary>
        /// Called periodically (when <see cref="EnableCheckState"/> = true), to allow this element to check its state.
        /// </summary>
        /// <param name="elements">All element created by <see cref="CreateElement"/>.</param>
        /// <param name="browserControl">BrowserControl - to access browser functions.</param>
        protected virtual void CheckState(FrameworkElement elements, IBrowserControl browserControl){}
    }
}
