using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace GreenerConfigurator.Utilities.Extensions
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        #region [ Constructor(s) ]

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
                throw new Exception("enumType should not be null or any other datatype ");

            EnumType = enumType;
        }


        #endregion

        #region [ Public Property(s) ]

        public Type EnumType { get; private set; }

        #endregion

        #region [ Public Method(s) ]

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }

        #endregion

    }
}
