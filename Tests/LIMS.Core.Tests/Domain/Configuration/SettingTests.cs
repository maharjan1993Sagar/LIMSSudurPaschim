﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LIMS.Domain.Configuration.Tests
{
    [TestClass()]
    public class SettingTests {
        [TestMethod()]
        public void test_constructor_and_overriden_ToString() {
            Setting setting = new Setting("customName", "customValue");
            Assert.AreEqual(setting.Name, "customName");
            Assert.AreEqual(setting.Value, "customValue");
            
            Assert.AreEqual(setting.Name, setting.ToString());
        }
    }
}