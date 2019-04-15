using System;
using System.IO;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeCredentials
{
    [TestClass]
    public class FakeTest
    {
        [TestMethod]
        public void FakeTest1()
        {
            // Arrange
            string configName = Settings.DefaultConfigName;
            string[] lines =
            {
                "dbtype=Sqlite",
                "filename=webgoat_coins.sqlite"
            };
            File.WriteAllLines(configName, lines);
            ConfigFile configFile = new ConfigFile(configName);
            configFile.Load();
            IDbProvider dbProvider = DbProviderFactory.Create(configFile);
            string fakeEmail = "someone@somewhere";
            string fakePassword = DateTime.Now.ToString();
            string goodEmail = "bob@ateliergraphique.com";
            string goodPassword = Encoder.Decode("MTIzNDU2");
            string hackEmail = "' or 1 = 1 --";
            string hackPassword = "";

            // Act
            bool loginFail = dbProvider.IsValidCustomerLogin(fakeEmail, fakePassword);
            bool loginOk = dbProvider.IsValidCustomerLogin(goodEmail, goodPassword);
            bool hackFail = dbProvider.IsValidCustomerLogin(hackEmail, hackPassword);

            // Assert
            Assert.IsTrue(loginOk);
            Assert.IsFalse(loginFail);
            Assert.IsFalse(hackFail);
        }
    }
}
