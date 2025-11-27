using System;

namespace GreenerConfigurator.ClientCore.Models
{
    public class EnumModel<T>
    {
        public string Name
        {
            get
            {
                var temp = string.Empty;

                Type enumType = typeof(T);
                Enum value = (Enum)Enum.ToObject(enumType, EnumItem);

                try
                {
                    temp = value.GetDisplayAttributeName();
                }
                catch { temp = value.ToString(); }

                return temp;
            }
        }

        public T EnumItem { get; set; }
    }
}
