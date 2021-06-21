using LIMS.Core.TypeConverters;
using System.Collections.Generic;
using System.ComponentModel;

namespace LIMS.Core.Infrastructure
{
    public class WebTypeConverter : ITypeConverter
    {
        public void Register()
        {
            TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            TypeDescriptor.AddAttributes(typeof(List<decimal>), new TypeConverterAttribute(typeof(GenericListTypeConverter<decimal>)));
            TypeDescriptor.AddAttributes(typeof(List<string>), new TypeConverterAttribute(typeof(GenericListTypeConverter<string>)));

            //dictionaries
            TypeDescriptor.AddAttributes(typeof(Dictionary<int, int>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<int, int>)));
            TypeDescriptor.AddAttributes(typeof(Dictionary<string, bool>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<string, bool>)));

        }

        public int Order => 0;
    }
}
