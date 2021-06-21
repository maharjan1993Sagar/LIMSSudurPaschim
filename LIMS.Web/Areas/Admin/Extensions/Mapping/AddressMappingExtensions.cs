using LIMS.Domain.Common;
using LIMS.Services.Common;
using LIMS.Services.Directory;
using LIMS.Web.Areas.Admin.Models.Common;
using System;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class AddressMappingExtensions
    {
        public static async Task<AddressModel> ToModel(this Address entity, ICountryService countryService = null, IStateProvinceService stateProvinceService = null)
        {
            var address = entity.MapTo<Address, AddressModel>();
            if (!string.IsNullOrEmpty(address.CountryId) && countryService != null)
            {
                address.CountryName = (await countryService.GetCountryById(address.CountryId))?.Name;
            }
            if (!string.IsNullOrEmpty(address.StateProvinceId) && stateProvinceService != null)
            {
                address.StateProvinceName = (await stateProvinceService.GetStateProvinceById(address.StateProvinceId))?.Name;
            }

            return address;
        }

        public static Address ToEntity(this AddressModel model)
        {
            return model.MapTo<AddressModel, Address>();
        }

        public static Address ToEntity(this AddressModel model, Address destination)
        {
            return model.MapTo(destination);
        }
        public static async Task PrepareCustomAddressAttributes(this AddressModel model,
            Address address,
            IAddressAttributeService addressAttributeService,
            IAddressAttributeParser addressAttributeParser)
        {
            //this method is very similar to the same one in LIMS.Web project
            if (addressAttributeService == null)
                throw new ArgumentNullException("addressAttributeService");

            if (addressAttributeParser == null)
                throw new ArgumentNullException("addressAttributeParser");

            var attributes = await addressAttributeService.GetAllAddressAttributes();
            foreach (var attribute in attributes)
            {
                var attributeModel = new AddressModel.AddressAttributeModel {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    IsRequired = attribute.IsRequired,
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = attribute.AddressAttributeValues;
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new AddressModel.AddressAttributeValueModel {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(attributeValueModel);
                    }
                }

                //set already selected attributes
                var selectedAddressAttributes = address != null ? address.CustomAttributes : null;

                model.CustomAddressAttributes.Add(attributeModel);
            }
        }
    }
}