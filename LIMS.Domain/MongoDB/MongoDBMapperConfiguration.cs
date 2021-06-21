using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Domain.Documents;
using LIMS.Domain.Logging;
using LIMS.Domain.Media;
using LIMS.Domain.Messages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace LIMS.Domain.MongoDB
{
    public static class MongoDBMapperConfiguration
    {

        /// <summary>
        /// Register MongoDB mappings
        /// </summary>
        public static void RegisterMongoDBMappings()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            BsonSerializer.RegisterSerializer(typeof(DateTime), new BsonUtcDateTimeSerializer());

            //global set an equivalent of [BsonIgnoreExtraElements] for every Domain Model
            var cp = new ConventionPack();
            cp.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("ApplicationConventions", cp, t => true);

            RegisterClassAddress();
            RegisterClassCustomer();
            RegisterClassCustomerAction();
            RegisterClassActionCondition();
            RegisterClassCustomerHistoryPassword();
            RegisterClassCustomerReminder();
            RegisterClassReminderCondition();
            RegisterClassCustomerReminderHistory();
            RegisterClassCustomerRole();
            RegisterClassLog();
            RegisterClassDownload();
            RegisterClassCampaign();
            RegisterClassEmailAccount();
            RegisterClassFormAttribute();
            RegisterClassMessageTemplate();
            RegisterClassQueuedEmail();
            RegisterClassCurrency();
            RegisterClassDocument();
        }

        private static void RegisterClassAddress()
        {
            BsonClassMap.RegisterClassMap<Address>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerId);
            });
        }
        private static void RegisterClassCustomer()
        {
            BsonClassMap.RegisterClassMap<Customer>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }

        private static void RegisterClassCustomerAction()
        {
            BsonClassMap.RegisterClassMap<CustomerAction>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReactionType);
            });
        }
        private static void RegisterClassActionCondition()
        {
            BsonClassMap.RegisterClassMap<CustomerAction.ActionCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerActionConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }

        private static void RegisterClassCustomerHistoryPassword()
        {
            BsonClassMap.RegisterClassMap<CustomerHistoryPassword>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.PasswordFormat);
            });
        }
        private static void RegisterClassCustomerReminder()
        {
            BsonClassMap.RegisterClassMap<CustomerReminder>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Condition);
                cm.UnmapMember(c => c.ReminderRule);
            });
        }
        private static void RegisterClassReminderCondition()
        {
            BsonClassMap.RegisterClassMap<CustomerReminder.ReminderCondition>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ConditionType);
                cm.UnmapMember(c => c.Condition);
            });
        }
        private static void RegisterClassCustomerReminderHistory()
        {
            BsonClassMap.RegisterClassMap<CustomerReminderHistory>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.ReminderRule);
                cm.UnmapMember(c => c.HistoryStatus);
            });
        }
        private static void RegisterClassCustomerRole()
        {
            BsonClassMap.RegisterClassMap<CustomerRole>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerId);
            });
        }
        private static void RegisterClassLog()
        {
            BsonClassMap.RegisterClassMap<Log>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.LogLevel);
            });
        }
        private static void RegisterClassDownload()
        {
            BsonClassMap.RegisterClassMap<Download>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DownloadBinary);
            });
        }
        private static void RegisterClassCampaign()
        {
            BsonClassMap.RegisterClassMap<Campaign>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.CustomerHasOrdersCondition);
                cm.UnmapMember(c => c.CustomerHasShoppingCartCondition);
            });
        }
        private static void RegisterClassEmailAccount()
        {
            BsonClassMap.RegisterClassMap<EmailAccount>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.FriendlyName);
            });
        }
        private static void RegisterClassFormAttribute()
        {
            BsonClassMap.RegisterClassMap<InteractiveForm.FormAttribute>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.AttributeControlType);
            });
        }
        private static void RegisterClassMessageTemplate()
        {
            BsonClassMap.RegisterClassMap<MessageTemplate>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DelayPeriod);
            });
        }
        private static void RegisterClassQueuedEmail()
        {
            BsonClassMap.RegisterClassMap<QueuedEmail>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.Priority);
            });
        }
        private static void RegisterClassCurrency()
        {
            BsonClassMap.RegisterClassMap<Currency>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.RoundingType);
            });
        }
        private static void RegisterClassDocument()
        {
            BsonClassMap.RegisterClassMap<Document>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(c => c.DocumentStatus);
                cm.UnmapMember(c => c.Reference);
            });
        }
    }
}
