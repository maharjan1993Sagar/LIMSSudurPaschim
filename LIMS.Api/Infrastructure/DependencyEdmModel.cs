using System;
using System.Collections.Generic;
using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.Common;
using LIMS.Api.DTOs.Customers;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Api.Infrastructure.DependencyManagement;
using LIMS.Core.Configuration;
using Microsoft.AspNet.OData.Builder;

namespace LIMS.Api.Infrastructure
{
    public class DependencyEdmModel : IDependencyEdmModel
    {
        protected void RegisterCommon(ODataConventionModelBuilder builder)
        {
            #region Language model

            builder.EntitySet<LanguageDto>("Language");
            builder.EntityType<LanguageDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Currency model

            builder.EntitySet<CurrencyDto>("Currency");
            builder.EntityType<CurrencyDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Store model

            builder.EntitySet<StoreDto>("Store");
            builder.EntityType<StoreDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Country model

            builder.EntitySet<CountryDto>("Country");
            builder.EntityType<CountryDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region State province model

            builder.EntitySet<StateProvinceDto>("StateProvince");
            builder.EntityType<StateProvinceDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Templates model

            builder.EntitySet<MessageTemplateDto>("CategoryTemplate");
            builder.EntitySet<MessageTemplateDto>("ManufacturerTemplate");
            builder.EntitySet<MessageTemplateDto>("ProductTemplate");
            builder.EntityType<MessageTemplateDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Picture model

            builder.EntitySet<PictureDto>("Picture");
            builder.EntityType<PictureDto>().Count().Filter().OrderBy().Page();

            #endregion
        }

        protected void RegisterCustomers(ODataConventionModelBuilder builder)
        {
            #region Customer

            builder.EntitySet<CustomerDto>("Customer");
            var customer = builder.EntityType<CustomerDto>();
            builder.ComplexType<AddressDto>();

            ActionConfiguration addAddress = customer.Action("AddAddress");
            addAddress.Parameter<string>(nameof(AddressDto.Id)).Required();
            addAddress.Parameter<string>(nameof(AddressDto.City));
            addAddress.Parameter<string>(nameof(AddressDto.Email));
            addAddress.Parameter<string>(nameof(AddressDto.Company));
            addAddress.Parameter<string>(nameof(AddressDto.Address1));
            addAddress.Parameter<string>(nameof(AddressDto.Address2));
            addAddress.Parameter<string>(nameof(AddressDto.LastName));
            addAddress.Parameter<string>(nameof(AddressDto.CountryId));
            addAddress.Parameter<string>(nameof(AddressDto.FaxNumber));
            addAddress.Parameter<string>(nameof(AddressDto.FirstName));
            addAddress.Parameter<string>(nameof(AddressDto.VatNumber));
            addAddress.Parameter<string>(nameof(AddressDto.PhoneNumber));
            addAddress.Parameter<string>(nameof(AddressDto.CustomAttributes));
            addAddress.Parameter<DateTimeOffset>(nameof(AddressDto.CreatedOnUtc));
            addAddress.Parameter<string>(nameof(AddressDto.ZipPostalCode));
            addAddress.Parameter<string>(nameof(AddressDto.StateProvinceId));
            addAddress.Returns<AddressDto>();

            ActionConfiguration updateAddress = customer.Action("UpdateAddress");
            updateAddress.Parameter<string>(nameof(AddressDto.Id)).Required();
            updateAddress.Parameter<string>(nameof(AddressDto.City));
            updateAddress.Parameter<string>(nameof(AddressDto.Email));
            updateAddress.Parameter<string>(nameof(AddressDto.Company));
            updateAddress.Parameter<string>(nameof(AddressDto.Address1));
            updateAddress.Parameter<string>(nameof(AddressDto.Address2));
            updateAddress.Parameter<string>(nameof(AddressDto.LastName));
            updateAddress.Parameter<string>(nameof(AddressDto.CountryId));
            updateAddress.Parameter<string>(nameof(AddressDto.FaxNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.FirstName));
            updateAddress.Parameter<string>(nameof(AddressDto.VatNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.PhoneNumber));
            updateAddress.Parameter<string>(nameof(AddressDto.CustomAttributes));
            updateAddress.Parameter<DateTimeOffset>(nameof(AddressDto.CreatedOnUtc));
            updateAddress.Parameter<string>(nameof(AddressDto.ZipPostalCode));
            updateAddress.Parameter<string>(nameof(AddressDto.StateProvinceId));
            updateAddress.Returns<AddressDto>();

            ActionConfiguration deleteAddress = customer.Action("DeleteAddress");
            deleteAddress.Parameter<string>("addressId");
            deleteAddress.Returns<bool>();

            ActionConfiguration changePassword = customer.Action("SetPassword");
            changePassword.Parameter<string>("password");
            changePassword.Returns<bool>();

            #endregion

            #region Customer Role model

            builder.EntitySet<CustomerRoleDto>("CustomerRole");
            builder.EntityType<CustomerRoleDto>().Count().Filter().OrderBy().Page();

            #endregion

            #region Vendors

            builder.EntitySet<VendorDto>("Vendor");
            builder.EntityType<VendorDto>().Count().Filter().OrderBy().Page();

            #endregion
        }

        public void Register(ODataConventionModelBuilder builder, ApiConfig apiConfig)
        {
            if (apiConfig.SystemModel)
            {
                RegisterCommon(builder);
                RegisterCustomers(builder);
                RegisterAnimal(builder);
            }
        }
        public void RegisterAnimal(ODataConventionModelBuilder builder)
        {

            builder.EntitySet<BreedDto>("BreedReg");
            builder.EntityType<BreedDto>().Count().Filter().OrderBy().Page();


            builder.EntitySet<SpeciesDto>("Species");
            builder.EntityType<SpeciesDto>().Count().Filter().OrderBy().Page();

            builder.EntitySet<FarmDto>("Farm");
            builder.EntityType<FarmDto>().Count().Filter().OrderBy().Page();

            builder.EntitySet<AnimalRegistrationDto>("AnimalRegistration");
            builder.EntityType<AnimalRegistrationDto>().Count().Filter().OrderBy().Page();

            builder.EntitySet<FeedbackDto>("Feedback");
            builder.EntityType<FeedbackDto>().Count().Filter().OrderBy().Page();
        }

        public int Order => 0;
    }
}
