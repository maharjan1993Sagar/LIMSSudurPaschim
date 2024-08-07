﻿using LIMS.Core.Data;
using LIMS.Domain;
using LIMS.Domain.Data;

namespace LIMS.Services.Tests
{
    public partial class MongoDBRepositoryTest<T> : Repository<T>, IRepository<T> where T : BaseEntity
    {
        public MongoDBRepositoryTest(): base(new MongoDBDataProvider())
        {
            var client = DriverTestConfiguration.Client;
            _database = client.GetDatabase(DriverTestConfiguration.DatabaseNamespace.DatabaseName);
            _database.DropCollection(DriverTestConfiguration.CollectionNamespace.CollectionName);
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }
    }
}