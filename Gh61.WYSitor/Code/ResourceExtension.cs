using System;
using System.Reflection;
using System.Windows.Markup;

namespace Gh61.WYSitor.Code
{
    /// <summary>
    /// {ext:Resource}
    /// </summary>
    internal class ResourceExtension : MarkupExtension
    {
        private string _member;

        public ResourceExtension()
        {
        }

        public ResourceExtension(string member)
        {
            _member = member;
        }

        [ConstructorArgument("member")]
        public string Member
        {
            get => this._member;
            set => this._member = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            int length = this._member.IndexOf('.');
            string qualifiedTypeName = length >= 0
                ? _member.Substring(0, length)
                : throw new ArgumentException($"Invalid resource path '{_member}'");

            var resolverService = (IXamlTypeResolver) serviceProvider.GetService(typeof(IXamlTypeResolver));
            if (resolverService == null)
                throw new InvalidOperationException("Missing IXamlTypeResolver service in context.");

            var type = resolverService.Resolve(qualifiedTypeName);
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
