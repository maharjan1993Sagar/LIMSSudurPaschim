using LIMS.Core;
using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Services.Common;
using LIMS.Services.Events;
using LIMS.Services.Helpers;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using LIMS.Services.Messages;
using LIMS.Services.Stores;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.Customers
{
    public partial class CustomerReminderService : ICustomerReminderService
    {
        #region Fields

        private readonly IRepository<CustomerReminder> _customerReminderRepository;
        private readonly IRepository<CustomerReminderHistory> _customerReminderHistoryRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMediator _mediator;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IStoreService _storeService;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        #endregion

        #region Ctor

        public CustomerReminderService(
            IRepository<CustomerReminder> customerReminderRepository,
            IRepository<CustomerReminderHistory> customerReminderHistoryRepository,
            IRepository<Customer> customerRepository,
            IMediator mediator,
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService,
            IMessageTokenProvider messageTokenProvider,
            IStoreService storeService,

            ICustomerAttributeParser customerAttributeParser,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILanguageService languageService)
        {
            _customerReminderRepository = customerReminderRepository;
            _customerReminderHistoryRepository = customerReminderHistoryRepository;
            _customerRepository = customerRepository;
            _mediator = mediator;
            _emailAccountService = emailAccountService;
            _messageTokenProvider = messageTokenProvider;
            _queuedEmailService = queuedEmailService;
            _storeService = storeService;
            _customerAttributeParser = customerAttributeParser;

            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
            _languageService = languageService;
        }

        #endregion

        #region Utilities

        protected async Task<bool> SendEmail(Customer customer, CustomerReminder customerReminder, string reminderlevelId)
        {
            var reminderLevel = customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId);
            var emailAccount = await _emailAccountService.GetEmailAccountById(reminderLevel.EmailAccountId);

            //retrieve message template data
            var bcc = reminderLevel.BccEmailAddresses;
            var languages = await _languageService.GetAllLanguages();
            var langId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LanguageId);
            if (string.IsNullOrEmpty(langId))
                langId = languages.FirstOrDefault().Id;

            var language = languages.FirstOrDefault(x => x.Id == langId);
            if (language == null)
                language = languages.FirstOrDefault();

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddStoreTokens(liquidObject, null, language, emailAccount);
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, null, language);


            //limit name length
            var toName = CommonHelper.EnsureMaximumLength(customer.GetFullName(), 300);
            var email = new QueuedEmail {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = customer.Email,
                ToName = toName,
                ReplyTo = string.Empty,
                ReplyToName = string.Empty,
                CC = string.Empty,
                Bcc = bcc,
                Subject = "dd",
                Body = "ddw",
                AttachmentFilePath = "",
                AttachmentFileName = "",
                AttachedDownloads = null,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
            };

            await _queuedEmailService.InsertQueuedEmail(email);
            //activity log
            await _customerActivityService.InsertActivity(string.Format("CustomerReminder.{0}", customerReminder.ReminderRule.ToString()), customer.Id, _localizationService.GetResource(string.Format("ActivityLog.{0}", customerReminder.ReminderRule.ToString())), customer, customerReminder.Name);

            return true;
        }


        #region Conditions
        protected async Task<bool> CheckConditions(CustomerReminder customerReminder, Customer customer)
        {
            if (customerReminder.Conditions.Count == 0)
                return true;


            bool cond = false;
            foreach (var item in customerReminder.Conditions)
            {
                if (item.ConditionType == CustomerReminderConditionTypeEnum.CustomerTag)
                {
                    cond = ConditionCustomerTag(item, customer);
                }
                if (item.ConditionType == CustomerReminderConditionTypeEnum.CustomerRole)
                {
                    cond = ConditionCustomerRole(item, customer);
                }
                if (item.ConditionType == CustomerReminderConditionTypeEnum.CustomerRegisterField)
                {
                    cond = ConditionCustomerRegister(item, customer);
                }
                if (item.ConditionType == CustomerReminderConditionTypeEnum.CustomCustomerAttribute)
                {
                    cond = await ConditionCustomerAttribute(item, customer);
                }
            }

            return cond;
        }

        protected bool ConditionProducts(CustomerReminder.ReminderCondition condition, ICollection<string> products)
        {
            bool cond = true;
            if (condition.Condition == CustomerReminderConditionEnum.AllOfThem)
            {
                cond = products.ContainsAll(condition.Products);
            }
            if (condition.Condition == CustomerReminderConditionEnum.OneOfThem)
            {
                cond = products.ContainsAny(condition.Products);
            }

            return cond;
        }
        protected bool ConditionCustomerRole(CustomerReminder.ReminderCondition condition, Customer customer)
        {
            bool cond = false;
            if (customer != null)
            {
                var customerRoles = customer.CustomerRoles;
                if (condition.Condition == CustomerReminderConditionEnum.AllOfThem)
                {
                    cond = customerRoles.Select(x => x.Id).ContainsAll(condition.CustomerRoles);
                }
                if (condition.Condition == CustomerReminderConditionEnum.OneOfThem)
                {
                    cond = customerRoles.Select(x => x.Id).ContainsAny(condition.CustomerRoles);
                }
            }
            return cond;
        }
        protected bool ConditionCustomerTag(CustomerReminder.ReminderCondition condition, Customer customer)
        {
            bool cond = false;
            if (customer != null)
            {
                var customerTags = customer.CustomerTags;
                if (condition.Condition == CustomerReminderConditionEnum.AllOfThem)
                {
                    cond = customerTags.Select(x => x).ContainsAll(condition.CustomerTags);
                }
                if (condition.Condition == CustomerReminderConditionEnum.OneOfThem)
                {
                    cond = customerTags.Select(x => x).ContainsAny(condition.CustomerTags);
                }
            }
            return cond;
        }
        protected bool ConditionCustomerRegister(CustomerReminder.ReminderCondition condition, Customer customer)
        {
            bool cond = false;
            if (customer != null)
            {
                if (condition.Condition == CustomerReminderConditionEnum.AllOfThem)
                {
                    cond = true;
                    foreach (var item in condition.CustomerRegistration)
                    {
                        if (customer.GenericAttributes.Where(x => x.Key == item.RegisterField && x.Value == item.RegisterValue).Count() == 0)
                            cond = false;
                    }
                }
                if (condition.Condition == CustomerReminderConditionEnum.OneOfThem)
                {
                    foreach (var item in condition.CustomerRegistration)
                    {
                        if (customer.GenericAttributes.Where(x => x.Key == item.RegisterField && x.Value == item.RegisterValue).Count() > 0)
                            cond = true;
                    }
                }
            }
            return cond;
        }
        protected async Task<bool> ConditionCustomerAttribute(CustomerReminder.ReminderCondition condition, Customer customer)
        {
            bool cond = false;
            if (customer != null)
            {
                if (condition.Condition == CustomerReminderConditionEnum.AllOfThem)
                {
                    var customCustomerAttributes = customer.GenericAttributes.FirstOrDefault(x => x.Key == "CustomCustomerAttributes");
                    if (customCustomerAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customCustomerAttributes.Value))
                        {
                            var selectedValues = await _customerAttributeParser.ParseCustomerAttributeValues(customCustomerAttributes.Value);
                            cond = true;
                            foreach (var item in condition.CustomCustomerAttributes)
                            {
                                var _fields = item.RegisterField.Split(':');
                                if (_fields.Count() > 1)
                                {
                                    if (selectedValues.Where(x => x.CustomerAttributeId == _fields.FirstOrDefault() && x.Id == _fields.LastOrDefault()).Count() == 0)
                                        cond = false;
                                }
                                else
                                    cond = false;
                            }
                        }
                    }
                }
                if (condition.Condition == CustomerReminderConditionEnum.OneOfThem)
                {

                    var customCustomerAttributes = customer.GenericAttributes.FirstOrDefault(x => x.Key == "CustomCustomerAttributes");
                    if (customCustomerAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customCustomerAttributes.Value))
                        {
                            var selectedValues = await _customerAttributeParser.ParseCustomerAttributeValues(customCustomerAttributes.Value);
                            foreach (var item in condition.CustomCustomerAttributes)
                            {
                                var _fields = item.RegisterField.Split(':');
                                if (_fields.Count() > 1)
                                {
                                    if (selectedValues.Where(x => x.CustomerAttributeId == _fields.FirstOrDefault() && x.Id == _fields.LastOrDefault()).Count() > 0)
                                        cond = true;
                                }
                            }
                        }
                    }
                }
            }
            return cond;
        }
        #endregion

        #region History

        protected async Task UpdateHistory(Customer customer, CustomerReminder customerReminder, string reminderlevelId, CustomerReminderHistory history)
        {
            if (history != null)
            {
                history.Levels.Add(new CustomerReminderHistory.HistoryLevel() {
                    Level = customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });
                if (customerReminder.Levels.Max(x => x.Level) ==
                    customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level)
                {
                    history.Status = (int)CustomerReminderHistoryStatusEnum.CompletedReminder;
                    history.EndDate = DateTime.UtcNow;
                }
                await _customerReminderHistoryRepository.UpdateAsync(history);
            }
            else
            {
                history = new CustomerReminderHistory();
                history.CustomerId = customer.Id;
                history.Status = (int)CustomerReminderHistoryStatusEnum.Started;
                history.StartDate = DateTime.UtcNow;
                history.CustomerReminderId = customerReminder.Id;
                history.ReminderRuleId = customerReminder.ReminderRuleId;
                history.Levels.Add(new CustomerReminderHistory.HistoryLevel() {
                    Level = customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });

                await _customerReminderHistoryRepository.InsertAsync(history);
            }

        }

        protected async Task UpdateHistory(CustomerReminder customerReminder, string reminderlevelId, CustomerReminderHistory history)
        {
            if (history != null)
            {
                history.Levels.Add(new CustomerReminderHistory.HistoryLevel() {
                    Level = customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });
                if (customerReminder.Levels.Max(x => x.Level) ==
                    customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level)
                {
                    history.Status = (int)CustomerReminderHistoryStatusEnum.CompletedReminder;
                    history.EndDate = DateTime.UtcNow;
                }
                await _customerReminderHistoryRepository.UpdateAsync(history);
            }
            else
            {
                history = new CustomerReminderHistory();
                history.Status = (int)CustomerReminderHistoryStatusEnum.Started;
                history.StartDate = DateTime.UtcNow;
                history.CustomerReminderId = customerReminder.Id;
                history.ReminderRuleId = customerReminder.ReminderRuleId;
                history.Levels.Add(new CustomerReminderHistory.HistoryLevel() {
                    Level = customerReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });

                await _customerReminderHistoryRepository.InsertAsync(history);
            }

        }
        protected async Task CloseHistoryReminder(CustomerReminder customerReminder, CustomerReminderHistory history)
        {
            history.Status = (int)CustomerReminderHistoryStatusEnum.CompletedReminder;
            history.EndDate = DateTime.UtcNow;
            await _customerReminderHistoryRepository.UpdateAsync(history);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Gets customer reminder
        /// </summary>
        /// <param name="id">Customer reminder identifier</param>
        /// <returns>Customer reminder</returns>
        public virtual Task<CustomerReminder> GetCustomerReminderById(string id)
        {
            return _customerReminderRepository.GetByIdAsync(id);
        }


        /// <summary>
        /// Gets all customer reminders
        /// </summary>
        /// <returns>Customer reminders</returns>
        public virtual async Task<IList<CustomerReminder>> GetCustomerReminders()
        {
            var query = from p in _customerReminderRepository.Table
                        orderby p.DisplayOrder
                        select p;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a customer reminder
        /// </summary>
        /// <param name="CustomerReminder">Customer reminder</param>
        public virtual async Task InsertCustomerReminder(CustomerReminder customerReminder)
        {
            if (customerReminder == null)
                throw new ArgumentNullException("customerReminder");

            await _customerReminderRepository.InsertAsync(customerReminder);

            //event notification
            await _mediator.EntityInserted(customerReminder);

        }

        /// <summary>
        /// Delete a customer reminder
        /// </summary>
        /// <param name="customerReminder">Customer reminder</param>
        public virtual async Task DeleteCustomerReminder(CustomerReminder customerReminder)
        {
            if (customerReminder == null)
                throw new ArgumentNullException("customerReminder");

            await _customerReminderRepository.DeleteAsync(customerReminder);

            //event notification
            await _mediator.EntityDeleted(customerReminder);

        }

        /// <summary>
        /// Updates the customer reminder
        /// </summary>
        /// <param name="CustomerReminder">Customer reminder</param>
        public virtual async Task UpdateCustomerReminder(CustomerReminder customerReminder)
        {
            if (customerReminder == null)
                throw new ArgumentNullException("customerReminder");

            await _customerReminderRepository.UpdateAsync(customerReminder);

            //event notification
            await _mediator.EntityUpdated(customerReminder);
        }



        public virtual async Task<IPagedList<SerializeCustomerReminderHistory>> GetAllCustomerReminderHistory(string customerReminderId, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from h in _customerReminderHistoryRepository.Table
                        from l in h.Levels
                        select new SerializeCustomerReminderHistory() { CustomerId = h.CustomerId, Id = h.Id, CustomerReminderId = h.CustomerReminderId, Level = l.Level, SendDate = l.SendDate };

            query = from p in query
                    where p.CustomerReminderId == customerReminderId
                    select p;
            return await PagedList<SerializeCustomerReminderHistory>.Create(query, pageIndex, pageSize);
        }

        #endregion

        #region Tasks

        public virtual async Task Task_RegisteredCustomer(string id = "")
        {
            var datetimeUtcNow = DateTime.UtcNow.Date;
            var customerReminder = new List<CustomerReminder>();
            if (String.IsNullOrEmpty(id))
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Active && datetimeUtcNow >= cr.StartDateTimeUtc && datetimeUtcNow <= cr.EndDateTimeUtc
                                          && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.RegisteredCustomer
                                          select cr).ToListAsync();
            }
            else
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Id == id && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.RegisteredCustomer
                                          select cr).ToListAsync();
            }
            foreach (var reminder in customerReminder)
            {
                var customers = await (from cu in _customerRepository.Table
                                       where cu.CreatedOnUtc > reminder.LastUpdateDate && cu.Active && !cu.Deleted
                                       && (!String.IsNullOrEmpty(cu.Email))
                                       && !cu.IsSystemAccount
                                       select cu).ToListAsync();

                foreach (var customer in customers)
                {
                    var history = await (from hc in _customerReminderHistoryRepository.Table
                                         where hc.CustomerId == customer.Id && hc.CustomerReminderId == reminder.Id
                                         select hc).ToListAsync();
                    if (history.Any())
                    {
                        var activereminderhistory = history.FirstOrDefault(x => x.HistoryStatus == CustomerReminderHistoryStatusEnum.Started);
                        if (activereminderhistory != null)
                        {
                            var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                            var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                            if (reminderLevel != null)
                            {
                                if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                                {
                                    var send = await SendEmail(customer, reminder, reminderLevel.Id);
                                    if (send)
                                        await UpdateHistory(customer, reminder, reminderLevel.Id, activereminderhistory);
                                }
                            }
                            else
                            {
                                await CloseHistoryReminder(reminder, activereminderhistory);
                            }
                        }
                        else
                        {
                            if (DateTime.UtcNow > history.Max(x => x.EndDate).AddDays(reminder.RenewedDay) && reminder.AllowRenew)
                            {
                                var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                                if (level != null)
                                {

                                    if (DateTime.UtcNow > customer.CreatedOnUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                                    {
                                        if (await CheckConditions(reminder, customer))
                                        {
                                            var send = await SendEmail(customer, reminder, level.Id);
                                            if (send)
                                                await UpdateHistory(customer, reminder, level.Id, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                        if (level != null)
                        {

                            if (DateTime.UtcNow > customer.CreatedOnUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                            {
                                if (await CheckConditions(reminder, customer))
                                {
                                    var send = await SendEmail(customer, reminder, level.Id);
                                    if (send)
                                        await UpdateHistory(customer, reminder, level.Id, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual async Task Task_LastActivity(string id = "")
        {
            var datetimeUtcNow = DateTime.UtcNow.Date;
            var customerReminder = new List<CustomerReminder>();
            if (String.IsNullOrEmpty(id))
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Active && datetimeUtcNow >= cr.StartDateTimeUtc && datetimeUtcNow <= cr.EndDateTimeUtc
                                          && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.LastActivity
                                          select cr).ToListAsync();
            }
            else
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Id == id && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.LastActivity
                                          select cr).ToListAsync();
            }
            foreach (var reminder in customerReminder)
            {
                var customers = await (from cu in _customerRepository.Table
                                       where cu.LastActivityDateUtc < reminder.LastUpdateDate && cu.Active && !cu.Deleted
                                       && (!String.IsNullOrEmpty(cu.Email))
                                       select cu).ToListAsync();

                foreach (var customer in customers)
                {
                    var history = await (from hc in _customerReminderHistoryRepository.Table
                                         where hc.CustomerId == customer.Id && hc.CustomerReminderId == reminder.Id
                                         select hc).ToListAsync();
                    if (history.Any())
                    {
                        var activereminderhistory = history.FirstOrDefault(x => x.HistoryStatus == CustomerReminderHistoryStatusEnum.Started);
                        if (activereminderhistory != null)
                        {
                            var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                            var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                            if (reminderLevel != null)
                            {
                                if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                                {
                                    var send = await SendEmail(customer, reminder, reminderLevel.Id);
                                    if (send)
                                        await UpdateHistory(customer, reminder, reminderLevel.Id, activereminderhistory);
                                }
                            }
                            else
                            {
                                await CloseHistoryReminder(reminder, activereminderhistory);
                            }
                        }
                        else
                        {
                            if (DateTime.UtcNow > history.Max(x => x.EndDate).AddDays(reminder.RenewedDay) && reminder.AllowRenew)
                            {
                                var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                                if (level != null)
                                {

                                    if (DateTime.UtcNow > customer.LastActivityDateUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                                    {
                                        if (await CheckConditions(reminder, customer))
                                        {
                                            var send = await SendEmail(customer, reminder, level.Id);
                                            if (send)
                                                await UpdateHistory(customer, reminder, level.Id, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                        if (level != null)
                        {
                            if (DateTime.UtcNow > customer.LastActivityDateUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                            {
                                if (await CheckConditions(reminder, customer))
                                {
                                    var send = await SendEmail(customer, reminder, level.Id);
                                    if (send)
                                        await UpdateHistory(customer, reminder, level.Id, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual async Task Task_Birthday(string id = "")
        {
            var datetimeUtcNow = DateTime.UtcNow.Date;
            var customerReminder = new List<CustomerReminder>();
            if (String.IsNullOrEmpty(id))
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Active && datetimeUtcNow >= cr.StartDateTimeUtc && datetimeUtcNow <= cr.EndDateTimeUtc
                                          && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.Birthday
                                          select cr).ToListAsync();
            }
            else
            {
                customerReminder = await (from cr in _customerReminderRepository.Table
                                          where cr.Id == id && cr.ReminderRuleId == (int)CustomerReminderRuleEnum.Birthday
                                          select cr).ToListAsync();
            }

            foreach (var reminder in customerReminder)
            {
                int day = 0;
                if (reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null)
                    day = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault().Day;

                string dateDDMM = DateTime.Now.AddDays(day).ToString("-MM-dd");

                var customers = await (from cu in _customerRepository.Table
                                       where (!String.IsNullOrEmpty(cu.Email)) && cu.Active && !cu.Deleted
                                       && cu.GenericAttributes.Any(x => x.Key == "DateOfBirth" && x.Value.Contains(dateDDMM))
                                       select cu).ToListAsync();

                foreach (var customer in customers)
                {
                    var history = await (from hc in _customerReminderHistoryRepository.Table
                                         where hc.CustomerId == customer.Id && hc.CustomerReminderId == reminder.Id
                                         select hc).ToListAsync();
                    if (history.Any())
                    {
                        var activereminderhistory = history.FirstOrDefault(x => x.HistoryStatus == CustomerReminderHistoryStatusEnum.Started);
                        if (activereminderhistory != null)
                        {
                            var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                            var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                            if (reminderLevel != null)
                            {
                                if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                                {
                                    var send = await SendEmail(customer, reminder, reminderLevel.Id);
                                    if (send)
                                        await UpdateHistory(customer, reminder, reminderLevel.Id, activereminderhistory);
                                }
                            }
                            else
                            {
                                await CloseHistoryReminder(reminder, activereminderhistory);
                            }
                        }
                        else
                        {
                            if (DateTime.UtcNow > history.Max(x => x.EndDate).AddDays(reminder.RenewedDay) && reminder.AllowRenew)
                            {
                                var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                                if (level != null)
                                {
                                    if (await CheckConditions(reminder, customer))
                                    {
                                        var send = await SendEmail(customer, reminder, level.Id);
                                        if (send)
                                            await UpdateHistory(customer, reminder, level.Id, null);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                        if (level != null)
                        {
                            if (await CheckConditions(reminder, customer))
                            {
                                var send = await SendEmail(customer, reminder, level.Id);
                                if (send)
                                    await UpdateHistory(customer, reminder, level.Id, null);
                            }
                        }
                    }
                }

                var activehistory = await (from hc in _customerReminderHistoryRepository.Table
                                           where hc.CustomerReminderId == reminder.Id && hc.Status == (int)CustomerReminderHistoryStatusEnum.Started
                                           select hc).ToListAsync();

                foreach (var activereminderhistory in activehistory)
                {
                    var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                    var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                    var customer = _customerRepository.Table.FirstOrDefault(x => x.Id == activereminderhistory.CustomerId);
                    if (reminderLevel != null && customer != null && customer.Active && !customer.Deleted)
                    {
                        if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                        {
                            var send = await SendEmail(customer, reminder, reminderLevel.Id);
                            if (send)
                                await UpdateHistory(customer, reminder, reminderLevel.Id, activereminderhistory);
                        }
                    }
                    else
                    {
                        await CloseHistoryReminder(reminder, activereminderhistory);
                    }
                }
            }

        }
        #endregion
    }

    public class SerializeCustomerReminderHistory
    {
        public string Id { get; set; }
        public string CustomerReminderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime SendDate { get; set; }
        public int Level { get; set; }
    }
}