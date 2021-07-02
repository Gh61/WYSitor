using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// {ext:Resource}
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    internal class ResourceExtension : MarkupExtension
    {
        private readonly Type _defaultResourcesType = typeof(Localization.ResourceManager);

        private string _member;

        public ResourceExtension()
        {
        }

        public ResourceExtension(string member)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        [ConstructorArgument("member")]
        public string Member
        {
            get => this._member;
            set => this._member = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this._member == null)
                throw new InvalidOperationException("Missing member path.");

            int length = this._member.IndexOf('.');
            string qualifiedTypeName = length >= 0
                ? _member.Substring(0, length)
                : throw new ArgumentException($"Invalid resource path '{_member}'");

            Type type;
            try
            {
                if (serviceProvider == null)
                    throw new ArgumentNullException(nameof(serviceProvider));

                var resolverService = (IXamlTypeResolver) serviceProvider.GetService(typeof(IXamlTypeResolver));
                if (resolverService == null)
                    throw new ArgumentException("Missing IXamlTypeResolver service in context.", nameof(IXamlTypeResolver));

                type = resolverService.Resolve(qualifiedTypeName);
            }
            catch
            {
                // catch exception in design mode (where IXamlTypeResolver is missing)
                if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                {
                    // using default resources type
                    type = _defaultResourcesType;
                }
                else
                {
                    throw;
                }
            }

            var name = _member.Substring(length + 1, _member.Length - length - 1);

            if (GetStaticPropertyValue(type, name, out var text))
                return text;

            throw new ArgumentException($"Invalid resource path '{_member}'");
        }

        private static bool GetStaticPropertyValue(Type type, string name, out object value)
        {
            Type type2 = type;
            do
            {
                PropertyInfo property = type2.GetProperty(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (property != null)
                {
                    value = property.GetValue(null, null);
                    return true;
                }

                type2 = type2.BaseType;
            }
            while (type2 != null);

            value = null;
            return false;
        }
    }
}
