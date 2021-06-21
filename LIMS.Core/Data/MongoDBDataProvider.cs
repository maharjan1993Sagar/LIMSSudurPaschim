﻿
using LIMS.Domain.Data;

namespace LIMS.Core.Data
{
    public class MongoDBDataProvider : IDataProvider
    {
        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitDatabase()
        {
            DataSettingsHelper.InitConnectionString();
        }

        #endregion

        public string ConnectionString => DataSettingsHelper.ConnectionString();

    }
}
