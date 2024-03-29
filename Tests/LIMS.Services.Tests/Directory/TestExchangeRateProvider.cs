﻿using LIMS.Domain.Directory;
using LIMS.Core.Plugins;
using LIMS.Services.Directory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Tests.Directory
{
    public class TestExchangeRateProvider : BasePlugin, IExchangeRateProvider {
        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>Exchange rates</returns>
        public async Task<IList<ExchangeRate>> GetCurrencyLiveRates(string exchangeRateCurrencyCode) {
            return await Task.FromResult(new List<ExchangeRate>());
        }

    }
}
