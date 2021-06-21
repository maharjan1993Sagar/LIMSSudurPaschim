using LIMS.Domain.AInR;
using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Domain.Logging;
using LIMS.Domain.Messages;
using LIMS.Services.Common;
using LIMS.Services.Directory;
using LIMS.Services.ExportImport.Help;
using LIMS.Services.Logging;
using LIMS.Services.Media;
using LIMS.Services.Messages;
using LIMS.Services.Stores;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LIMS.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager : IExportManager
    {
        #region Fields

       
        private readonly IPictureService _pictureService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IStoreService _storeService;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Ctor

        public ExportManager(
            IPictureService pictureService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IStoreService storeService,
            IServiceProvider serviceProvider)
        {  
            _pictureService = pictureService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _storeService = storeService;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods       
        public virtual byte[] ExpertEarTagToXlsx(IList<EarTag> earTags)
        {
            var properties = new[]
            {
                new PropertyByName<EarTag>("Eartag", p => p.EarTagNo)
            };
            return ExportToXlsx(properties, earTags);
        }

        /// <summary>
        /// Export customer list to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="customers">Customers</param>
        public virtual byte[] ExportCustomersToXlsx(IList<Customer> customers)
        {
            return ExportToXlsx(PropertyByCustomer(), customers);
        }


        /// <summary>
        /// Export customer - personal info to XLSX
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task<byte[]> ExportCustomerToXlsx(Customer customer, string stroreId)
        {
            using (var stream = new MemoryStream())
            {
                IWorkbook xlPackage = new XSSFWorkbook();

                //customer info
                ISheet worksheetCustomer = xlPackage.CreateSheet("CustomerInfo");
                var managerCustomer = PrepareCustomer(customer);
                managerCustomer.WriteToXlsx(worksheetCustomer);

                //address
                var worksheetAddress = xlPackage.CreateSheet("Address");
                var managerAddress = new PropertyManager<Address>(PropertyByAddress());
                managerAddress.WriteCaption(worksheetAddress);
                var row = 1;
                foreach (var item in customer.Addresses)
                {
                    managerAddress.CurrentObject = item;
                    managerAddress.WriteToXlsx(worksheetAddress, row++);
                }

                //activity log
                var customerActivityService = _serviceProvider.GetRequiredService<ICustomerActivityService>();
                var actlogs = await customerActivityService.GetAllActivities(customerId: customer.Id);
                var worksheetLog = xlPackage.CreateSheet("ActivityLogs");
                var managerLog = new PropertyManager<ActivityLog>(PropertyByActivityLog());
                managerLog.WriteCaption(worksheetLog);
                row = 1;
                foreach (var items in actlogs)
                {
                    managerLog.CurrentObject = items;
                    managerLog.WriteToXlsx(worksheetLog, row++);
                }

                //contact us
                var contactUsService = _serviceProvider.GetRequiredService<IContactUsService>();
                var contacts = await contactUsService.GetAllContactUs(customerId: customer.Id);
                var worksheetContact = xlPackage.CreateSheet("MessageContact");
                var managerContact = new PropertyManager<ContactUs>(PropertyByContactForm());
                managerContact.WriteCaption(worksheetContact);
                row = 1;
                foreach (var items in contacts)
                {
                    managerContact.CurrentObject = items;
                    managerContact.WriteToXlsx(worksheetContact, row++);
                }

                //emails
                var queuedEmailService = _serviceProvider.GetRequiredService<IQueuedEmailService>();
                var queuedEmails = await queuedEmailService.SearchEmails("", customer.Email, null, null, false, true, 100, true);
                var worksheetEmails = xlPackage.CreateSheet("Emails");
                var managerEmails = new PropertyManager<QueuedEmail>(PropertyByEmails());
                managerEmails.WriteCaption(worksheetEmails);
                row = 1;
                foreach (var items in queuedEmails)
                {
                    managerEmails.CurrentObject = items;
                    managerEmails.WriteToXlsx(worksheetEmails, row++);
                }

                //Newsletter subscribe - history of change
                var newsletterService = _serviceProvider.GetRequiredService<INewsLetterSubscriptionService>();
                var newsletter = await newsletterService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, stroreId);
                if (newsletter != null)
                {
                    var worksheetNewsletter = xlPackage.CreateSheet("Newsletter subscribe - history of change");
                    var managerNewsletter = new PropertyManager<NewsLetterSubscription>(PropertyByNewsLetterSubscription());
                    var newsletterhistory = await newsletter.GetHistoryObject(_serviceProvider.GetRequiredService<IHistoryService>());
                    managerNewsletter.WriteCaption(worksheetNewsletter);
                    row = 1;
                    foreach (var item in newsletterhistory)
                    {
                        var _tmp = (NewsLetterSubscription)item.Object;

                        var newslettertml = new NewsLetterSubscription() {
                            Active = _tmp.Active,
                            CreatedOnUtc = item.CreatedOnUtc
                        };
                        _tmp.Categories.ToList().ForEach(x => newslettertml.Categories.Add(x));
                        managerNewsletter.CurrentObject = newslettertml;
                        managerNewsletter.WriteToXlsx(worksheetNewsletter, row++);
                    }
                }

                xlPackage.Write(stream);
                return stream.ToArray();
            }

        }


        /// <summary>
        /// Export customer list to xml
        /// </summary>
        /// <param name="customers">Customers</param>
        /// <returns>Result in XML format</returns>
        public virtual async Task<string> ExportCustomersToXml(IList<Customer> customers)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xwSettings = new XmlWriterSettings {
                ConformanceLevel = ConformanceLevel.Auto,
                Async = true,
            };
            var xmlWriter = XmlWriter.Create(stringWriter, xwSettings);
            await xmlWriter.WriteStartDocumentAsync();
            xmlWriter.WriteStartElement("Customers");
            xmlWriter.WriteAttributeString("Version", Core.LIMSVersion.FullVersion);

            foreach (var customer in customers)
            {
                xmlWriter.WriteStartElement("Customer");
                xmlWriter.WriteElementString("CustomerId", null, customer.Id.ToString());
                xmlWriter.WriteElementString("CustomerGuid", null, customer.CustomerGuid.ToString());
                xmlWriter.WriteElementString("Email", null, customer.Email);
                xmlWriter.WriteElementString("Username", null, customer.Username);
                xmlWriter.WriteElementString("Password", null, customer.Password);
                xmlWriter.WriteElementString("PasswordFormatId", null, customer.PasswordFormatId.ToString());
                xmlWriter.WriteElementString("PasswordSalt", null, customer.PasswordSalt);
                xmlWriter.WriteElementString("IsTaxExempt", null, customer.IsTaxExempt.ToString());
                xmlWriter.WriteElementString("AffiliateId", null, customer.AffiliateId);
                xmlWriter.WriteElementString("VendorId", null, customer.VendorId);
                xmlWriter.WriteElementString("Active", null, customer.Active.ToString());


                xmlWriter.WriteElementString("IsGuest", null, customer.IsGuest().ToString());
                xmlWriter.WriteElementString("IsRegistered", null, customer.IsRegistered().ToString());
                xmlWriter.WriteElementString("IsAdministrator", null, customer.IsAdmin().ToString());
                xmlWriter.WriteElementString("IsForumModerator", null, customer.IsForumModerator().ToString());

                xmlWriter.WriteElementString("FirstName", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.FirstName));
                xmlWriter.WriteElementString("LastName", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LastName));
                xmlWriter.WriteElementString("Gender", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Gender));
                xmlWriter.WriteElementString("Company", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Company));

                xmlWriter.WriteElementString("CountryId", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.CountryId));
                xmlWriter.WriteElementString("StateProvinceId", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StateProvinceId));
                xmlWriter.WriteElementString("StreetAddress", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress));
                xmlWriter.WriteElementString("StreetAddress2", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress2));
                xmlWriter.WriteElementString("ZipPostalCode", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.ZipPostalCode));
                xmlWriter.WriteElementString("City", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.City));
                xmlWriter.WriteElementString("Phone", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Phone));
                xmlWriter.WriteElementString("Fax", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Fax));
                xmlWriter.WriteElementString("VatNumber", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.VatNumber));
                xmlWriter.WriteElementString("VatNumberStatusId", null, customer.GetAttributeFromEntity<int>(SystemCustomerAttributeNames.VatNumberStatusId).ToString());
                xmlWriter.WriteElementString("TimeZoneId", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.TimeZoneId));

                foreach (var store in await _storeService.GetAllStores())
                {
                    var newsletter = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    bool subscribedToNewsletters = newsletter != null && newsletter.Active;
                    xmlWriter.WriteElementString(string.Format("Newsletter-in-store-{0}", store.Id), null, subscribedToNewsletters.ToString());
                }

                xmlWriter.WriteElementString("AvatarPictureId", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId)?.ToString());
                xmlWriter.WriteElementString("ForumPostCount", null, customer.GetAttributeFromEntity<int>(SystemCustomerAttributeNames.ForumPostCount).ToString());
                xmlWriter.WriteElementString("Signature", null, customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Signature));

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            await xmlWriter.FlushAsync();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export newsletter subscribers to TXT
        /// </summary>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportNewsletterSubscribersToTxt(IList<NewsLetterSubscription> subscriptions)
        {
            if (subscriptions == null)
                throw new ArgumentNullException("subscriptions");

            const string separator = ",";
            var sb = new StringBuilder();
            foreach (var subscription in subscriptions)
            {
                sb.Append(subscription.Email);
                sb.Append(separator);
                sb.Append(subscription.Active);
                sb.Append(separator);
                sb.Append(subscription.CreatedOnUtc);
                sb.Append(separator);
                sb.Append(subscription.StoreId);
                sb.Append(separator);
                sb.Append(string.Join(';', subscription.Categories));
                sb.Append(Environment.NewLine);  //new line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Export newsletter subscribers to TXT
        /// </summary>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportNewsletterSubscribersToTxt(IList<string> subscriptions)
        {
            if (subscriptions == null)
                throw new ArgumentNullException("subscriptions");

            var sb = new StringBuilder();
            foreach (var subscription in subscriptions)
            {
                sb.Append(subscription);
                sb.Append(Environment.NewLine);  //new line
            }
            return sb.ToString();
        }
        /// <summary>
        /// Export states to TXT
        /// </summary>
        /// <param name="states">States</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual async Task<string> ExportStatesToTxt(IList<StateProvince> states)
        {
            if (states == null)
                throw new ArgumentNullException("states");

            const string separator = ",";
            var sb = new StringBuilder();
            var countryService = _serviceProvider.GetRequiredService<ICountryService>();
            foreach (var state in states)
            {
                var country = await countryService.GetCountryById(state.CountryId);
                sb.Append(country.TwoLetterIsoCode);
                sb.Append(separator);
                sb.Append(state.Name);
                sb.Append(separator);
                sb.Append(state.Abbreviation);
                sb.Append(separator);
                sb.Append(state.Published);
                sb.Append(separator);
                sb.Append(state.DisplayOrder);
                sb.Append(Environment.NewLine);  //new line
            }
            return sb.ToString();
        }


        /// <summary>
        /// Export objects to XLSX
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="properties">Class access to the object through its properties</param>
        /// <param name="itemsToExport">The objects to export</param>
        /// <returns></returns>
        public virtual byte[] ExportToXlsx<T>(PropertyByName<T>[] properties, IEnumerable<T> itemsToExport)
        {
            using (var stream = new MemoryStream())
            {
                IWorkbook xlPackage = new XSSFWorkbook();
                ISheet worksheet = xlPackage.CreateSheet(typeof(T).Name);
                var manager = new PropertyManager<T>(properties);
                manager.WriteCaption(worksheet);

                var row = 1;

                foreach (var items in itemsToExport)
                {
                    manager.CurrentObject = items;
                    manager.WriteToXlsx(worksheet, row++);
                }
                xlPackage.Write(stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Returns the path to the image file by ID
        /// </summary>
        /// <param name="pictureId">Picture ID</param>
        /// <returns>Path to the image file</returns>
        protected virtual async Task<string> GetPictures(string pictureId)
        {
            var picture = await _pictureService.GetPictureById(pictureId);
            return await _pictureService.GetThumbLocalPath(picture);
        }

        private PropertyByName<Address>[] PropertyByAddress()
        {
            var properties = new[]
            {
                    new PropertyByName<Address>("Email", p=>p.Email),
                    new PropertyByName<Address>("FirstName", p=>p.FirstName),
                    new PropertyByName<Address>("LastName", p=>p.LastName),
                    new PropertyByName<Address>("PhoneNumber", p=>p.PhoneNumber),
                    new PropertyByName<Address>("FaxNumber", p=>p.FaxNumber),
                    new PropertyByName<Address>("Address1", p=>p.Address1),
                    new PropertyByName<Address>("Address2", p=>p.Address2),
                    new PropertyByName<Address>("City", p=>p.City),
                    new PropertyByName<Address>("Country", p=> !string.IsNullOrEmpty(p.CountryId) ? _serviceProvider.GetRequiredService<ICountryService>().GetCountryById(p.CountryId).Result?.Name : ""),
                    new PropertyByName<Address>("StateProvince", p=> !string.IsNullOrEmpty(p.StateProvinceId) ? _serviceProvider.GetRequiredService<IStateProvinceService>().GetStateProvinceById(p.StateProvinceId).Result?.Name : ""),
            };
            return properties;
        }

        private PropertyByName<Customer>[] PropertyByCustomer()
        {
            var properties = new[]
            {
                new PropertyByName<Customer>("CustomerId", p => p.Id),
                new PropertyByName<Customer>("CustomerGuid", p => p.CustomerGuid),
                new PropertyByName<Customer>("Email", p => p.Email),
                new PropertyByName<Customer>("Username", p => p.Username),
                new PropertyByName<Customer>("Password", p => p.Password),
                new PropertyByName<Customer>("PasswordFormatId", p => p.PasswordFormatId),
                new PropertyByName<Customer>("PasswordSalt", p => p.PasswordSalt),
                new PropertyByName<Customer>("IsTaxExempt", p => p.IsTaxExempt),
                new PropertyByName<Customer>("AffiliateId", p => p.AffiliateId),
                new PropertyByName<Customer>("VendorId", p => p.VendorId),
                new PropertyByName<Customer>("Active", p => p.Active),
                new PropertyByName<Customer>("IsGuest", p => p.IsGuest()),
                new PropertyByName<Customer>("IsRegistered", p => p.IsRegistered()),
                new PropertyByName<Customer>("IsAdministrator", p => p.IsAdmin()),
                new PropertyByName<Customer>("IsForumModerator", p => p.IsForumModerator()),
                //attributes
                new PropertyByName<Customer>("FirstName", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.FirstName)),
                new PropertyByName<Customer>("LastName", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LastName)),
                new PropertyByName<Customer>("Gender", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Gender)),
                new PropertyByName<Customer>("Company", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Company)),
                new PropertyByName<Customer>("StreetAddress", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress)),
                new PropertyByName<Customer>("StreetAddress2", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress2)),
                new PropertyByName<Customer>("ZipPostalCode", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.ZipPostalCode)),
                new PropertyByName<Customer>("City", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.City)),
                new PropertyByName<Customer>("CountryId", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.CountryId)),
                new PropertyByName<Customer>("StateProvinceId", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StateProvinceId)),
                new PropertyByName<Customer>("Phone", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Phone)),
                new PropertyByName<Customer>("Fax", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Fax)),
                new PropertyByName<Customer>("VatNumber", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.VatNumber)),
                new PropertyByName<Customer>("VatNumberStatusId", p => p.GetAttributeFromEntity<int>(SystemCustomerAttributeNames.VatNumberStatusId)),
                new PropertyByName<Customer>("TimeZoneId", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.TimeZoneId)),
                new PropertyByName<Customer>("AvatarPictureId", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId)),
                new PropertyByName<Customer>("ForumPostCount", p => p.GetAttributeFromEntity<int>(SystemCustomerAttributeNames.ForumPostCount)),
                new PropertyByName<Customer>("Signature", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Signature)),
            };
            return properties;
        }

        private PropertyByName<ActivityLog>[] PropertyByActivityLog()
        {
            var properties = new[]
            {
                new PropertyByName<ActivityLog>("IpAddress", p => p.IpAddress),
                new PropertyByName<ActivityLog>("CreatedOnUtc", p => p.CreatedOnUtc.ToString()),
                new PropertyByName<ActivityLog>("Comment", p => p.Comment),
            };
            return properties;
        }
        private PropertyByName<ContactUs>[] PropertyByContactForm()
        {
            var properties = new[]
            {
                new PropertyByName<ContactUs>("IpAddress", p => p.IpAddress),
                new PropertyByName<ContactUs>("CreatedOnUtc", p => p.CreatedOnUtc.ToString()),
                new PropertyByName<ContactUs>("Email", p => p.Email),
                new PropertyByName<ContactUs>("FullName", p => p.FullName),
                new PropertyByName<ContactUs>("Subject", p => p.Subject),
                new PropertyByName<ContactUs>("Enquiry", p => p.Enquiry),
                new PropertyByName<ContactUs>("ContactAttributeDescription", p => p.ContactAttributeDescription),
            };
            return properties;
        }

        private PropertyByName<QueuedEmail>[] PropertyByEmails()
        {
            var properties = new[]
            {
                new PropertyByName<QueuedEmail>("SentOnUtc", p => p.SentOnUtc.ToString()),
                new PropertyByName<QueuedEmail>("From", p => p.From),
                new PropertyByName<QueuedEmail>("FromName", p => p.FromName),
                new PropertyByName<QueuedEmail>("Subject", p => p.Subject),
                new PropertyByName<QueuedEmail>("Body", p => p.Body),
            };
            return properties;
        }

        private PropertyByName<NewsLetterSubscription>[] PropertyByNewsLetterSubscription()
        {
            var newsletterCategoryService = _serviceProvider.GetRequiredService<INewsletterCategoryService>();

            string GetCategoryNames(IList<string> categoryNames, string separator = ",")
            {
                var sb = new StringBuilder();
                for (int i = 0; i < categoryNames.Count; i++)
                {
                    var category = newsletterCategoryService.GetNewsletterCategoryById(categoryNames[i]).Result;
                    if (category != null)
                    {
                        sb.Append(category.Name);
                        if (i != categoryNames.Count - 1)
                        {
                            sb.Append(separator);
                            sb.Append(" ");
                        }
                    }
                }
                return sb.ToString();
            }
            var properties = new[]
            {
                new PropertyByName<NewsLetterSubscription>("CreatedOnUtc", p => p.CreatedOnUtc.ToString()),
                new PropertyByName<NewsLetterSubscription>("Active", p => p.Active.ToString()),
                new PropertyByName<NewsLetterSubscription>("Categories", p => GetCategoryNames(p.Categories.ToList())),

            };
            return properties;
        }

        private PropertyHelperList<Customer> PrepareCustomer(Customer customer)
        {
            var helper = new PropertyHelperList<Customer>(customer);
            helper.ObjectList.Add(new PropertyHelperList<Customer>("CustomerId", p => p.Id));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("CustomerGuid", p => p.CustomerGuid));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Email", p => p.Email));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Username", p => p.Username));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Password", p => p.Password));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("PasswordFormatId", p => p.PasswordFormatId));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("PasswordSalt", p => p.PasswordSalt));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("IsTaxExempt", p => p.IsTaxExempt));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Active", p => p.Active));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("IsGuest", p => p.IsGuest()));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("IsRegistered", p => p.IsRegistered()));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("IsAdministrator", p => p.IsAdmin()));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("IsForumModerator", p => p.IsForumModerator()));
            //attributes
            helper.ObjectList.Add(new PropertyHelperList<Customer>("FirstName", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.FirstName)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("LastName", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LastName)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Gender", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Gender)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Company", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Company)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("StreetAddress", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("StreetAddress2", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StreetAddress2)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("ZipPostalCode", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.ZipPostalCode)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("City", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.City)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Country",
                p =>
                {
                    var countryid = p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.CountryId);
                    var countryName = "";
                    if (!string.IsNullOrEmpty(countryid))
                        countryName = _serviceProvider.GetRequiredService<ICountryService>().GetCountryById(countryid).Result?.Name;
                    return countryName;
                }
                ));

            helper.ObjectList.Add(new PropertyHelperList<Customer>("StateProvince",
                p =>
                {
                    var stateId = p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.StateProvinceId);
                    var stateName = "";
                    if (!string.IsNullOrEmpty(stateId))
                        stateName = _serviceProvider.GetRequiredService<IStateProvinceService>().GetStateProvinceById(stateId).Result?.Name;
                    return stateName;
                }
                ));

            helper.ObjectList.Add(new PropertyHelperList<Customer>("Phone", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Phone)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Fax", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Fax)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("VatNumber", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.VatNumber)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("ForumPostCount", p => p.GetAttributeFromEntity<int>(SystemCustomerAttributeNames.ForumPostCount)));
            helper.ObjectList.Add(new PropertyHelperList<Customer>("Signature", p => p.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.Signature)));

            return helper;
        }

        #endregion
    }
}
