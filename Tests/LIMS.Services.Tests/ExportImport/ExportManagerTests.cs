using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Services.Media;
using LIMS.Services.Messages;
using LIMS.Services.Stores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace LIMS.Services.ExportImport.Tests
{
    [TestClass()]
    public class ExportManagerTests {
        private IPictureService _pictureService;
        private INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private IExportManager _exportManager;
        private IStoreService _storeService;
        private IServiceProvider _serviceProvider;

        [TestInitialize()]
        public void TestInitialize() {
            _storeService = new Mock<IStoreService>().Object;
            _pictureService = new Mock<IPictureService>().Object;
            _newsLetterSubscriptionService = new Mock<INewsLetterSubscriptionService>().Object;
            _serviceProvider = new Mock<IServiceProvider>().Object;
        }

        protected Address GetTestBillingAddress() {
            return new Address {
                FirstName = "FirstName 1",
                LastName = "LastName 1",
                Email = "Email 1",
                Company = "Company 1",
                City = "City 1",
                Address1 = "Address1a",
                Address2 = "Address1a",
                ZipPostalCode = "ZipPostalCode 1",
                PhoneNumber = "PhoneNumber 1",
                FaxNumber = "FaxNumber 1",
                CreatedOnUtc = new DateTime(2010, 01, 01),
            };
        }

        protected Address GetTestShippingAddress() {
            return new Address {
                FirstName = "FirstName 2",
                LastName = "LastName 2",
                Email = "Email 2",
                Company = "Company 2",
                City = "City 2",
                Address1 = "Address2a",
                Address2 = "Address2b",
                ZipPostalCode = "ZipPostalCode 2",
                PhoneNumber = "PhoneNumber 2",
                FaxNumber = "FaxNumber 2",
                CreatedOnUtc = new DateTime(2010, 01, 01),
            };
        }

        protected Country GetTestCountry() {
            return new Country {
                Name = "United States",
                AllowsBilling = true,
                AllowsShipping = true,
                TwoLetterIsoCode = "US",
                ThreeLetterIsoCode = "USA",
                NumericIsoCode = 1,
                SubjectToVat = true,
                Published = true,
                DisplayOrder = 1
            };
        }

        protected Customer GetTestCustomer() {
            return new Customer {
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "some comment here",
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01)
            };
        }
    }
}