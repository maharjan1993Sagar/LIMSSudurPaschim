using LIMS.Core.Plugins;
using LIMS.Services.Tests.Directory;
using System.Collections.Generic;

namespace LIMS.Services.Tests
{
    public class ServiceTest
    {
        public void PluginInitializator()
        {
            InitPlugins();
        }

        private void InitPlugins()
        {
            var plugins = new List<PluginDescriptor>();
            plugins.Add(new PluginDescriptor(typeof(TestExchangeRateProvider).Assembly,
                null, typeof(TestExchangeRateProvider))
            {
                SystemName = "CurrencyExchange.TestProvider",
                FriendlyName = "Test exchange rate provider",
                Installed = true,
            });
            PluginManager.ReferencedPlugins = plugins;
        }
    }
}