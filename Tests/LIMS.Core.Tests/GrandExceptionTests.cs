using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LIMS.Core.Tests
{
    [TestClass()]
    public class LIMSExceptionTests {
        [TestMethod()]
        public void pass_individual_message_to_exception() {
            try {
                throw new LIMSException("lorem ipsum 123");
            }
            catch(Exception ex) {
                Assert.AreEqual("lorem ipsum 123", ex.Message);
            }            
        }
    }
}