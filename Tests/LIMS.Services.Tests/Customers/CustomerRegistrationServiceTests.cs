using LIMS.Domain.Data;
using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Security;
using LIMS.Core.Tests.Caching;
using LIMS.Services.Common;
using LIMS.Services.Localization;
using LIMS.Services.Messages;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace LIMS.Services.Customers.Tests
{
    [TestClass()]
    public class CustomerRegistrationServiceTests
    {
        private IRepository<Customer> _customerRepo;
        private IRepository<CustomerHistoryPassword> _customerHistoryRepo;
        private IRepository<CustomerRole> _customerRoleRepo;
        private IRepository<CustomerProduct> _customerProductRepo;
        private IRepository<CustomerNote> _customerNoteRepo;
        private IGenericAttributeService _genericAttributeService;
        private IEncryptionService _encryptionService;
        private ICustomerService _customerService;
        private ICustomerRegistrationService _customerRegistrationService;
        private ILocalizationService _localizationService;
        private CustomerSettings _customerSettings;
        private INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private IMediator _eventPublisher;
        private IStoreService _storeService;
        private SecuritySettings _securitySettings;
        private CommonSettings _commonSettings;
        private IServiceProvider _serviceProvider;
        //this method just help to get rid of repetitive code below
        private void AddCustomerToRegisteredRole(Customer customer)
        {
            customer.CustomerRoles.Add(new CustomerRole
            {
                Active = true,
                IsSystemRole = true,
                SystemName = SystemCustomerRoleNames.Registered
            });
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            _securitySettings = new SecuritySettings
            {
                EncryptionKey = "273ece6f97dd844d97dd8f4d"
            };

            _encryptionService = new EncryptionService(_securitySettings);

            var customer1 = new Customer
            {
                Username = "a@b.com",
                Email = "a@b.com",
                PasswordFormat = PasswordFormat.Hashed,
                Active = true
            };

            string saltKey = _encryptionService.CreateSaltKey(5);
            string password = _encryptionService.CreatePasswordHash("password", saltKey);
            customer1.PasswordSalt = saltKey;
            customer1.Password = password;
            AddCustomerToRegisteredRole(customer1);

            var customer2 = new Customer
            {
                Username = "test@test.com",
                Email = "test@test.com",
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                Active = true
            };
            AddCustomerToRegisteredRole(customer2);

            var customer3 = new Customer
            {
                Username = "user@test.com",
                Email = "user@test.com",
                PasswordFormat = PasswordFormat.Encrypted,
                Password = _encryptionService.EncryptText("password"),
                Active = true
            };
            AddCustomerToRegisteredRole(customer3);

            var customer4 = new Customer
            {
                Username = "registered@test.com",
                Email = "registered@test.com",
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                Active = true
            };
            AddCustomerToRegisteredRole(customer4);

            var customer5 = new Customer
            {
                Username = "notregistered@test.com",
                Email = "notregistered@test.com",
                PasswordFormat = PasswordFormat.Clear,
                Password = "password",
                Active = true
            };

            //trying to recreate

            var eventPublisher = new Mock<IMediator>();
            //eventPublisher.Setup(x => x.PublishAsync(new object()));
            _eventPublisher = eventPublisher.Object;

            _storeService = new Mock<IStoreService>().Object;

            _customerRepo = new LIMS.Services.Tests.MongoDBRepositoryTest<Customer>();
            _customerRepo.Insert(customer1);
            _customerRepo.Insert(customer2);
            _customerRepo.Insert(customer3);
            _customerRepo.Insert(customer4);
            _customerRepo.Insert(customer5);

            _customerRoleRepo = new Mock<IRepository<CustomerRole>>().Object;
            _customerProductRepo = new Mock<IRepository<CustomerProduct>>().Object;
            _customerHistoryRepo = new Mock<IRepository<CustomerHistoryPassword>>().Object;
            _customerNoteRepo = new Mock<IRepository<CustomerNote>>().Object;

            _genericAttributeService = new Mock<IGenericAttributeService>().Object;
            _newsLetterSubscriptionService = new Mock<INewsLetterSubscriptionService>().Object;
            _localizationService = new Mock<ILocalizationService>().Object;
            _serviceProvider = new Mock<IServiceProvider>().Object;
            _customerSettings = new CustomerSettings();
            _commonSettings = new CommonSettings();

            _customerRegistrationService = new CustomerRegistrationService(
                _customerService,
                _encryptionService,
                _newsLetterSubscriptionService,
                _localizationService,
                _storeService,
                _eventPublisher,
                _customerSettings,
                _genericAttributeService);
        }

        [TestMethod()]
        public async Task Ensure_only_registered_customers_can_login()
        {
            Assert.AreEqual(
                CustomerLoginResults.Successful,
                await _customerRegistrationService.ValidateCustomer("registered@test.com", "password"));

            Assert.AreEqual(
                CustomerLoginResults.NotRegistered,
                await _customerRegistrationService.ValidateCustomer("notregistered@test.com", "password"));
        }
    }
}