using System.IO;
using System.Web;
using Moq;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FackCredentials
{
    [TestClass]
    public class MockTest
    {
        [TestMethod]
        public void MockTest1()
        {
            // Arrange
            string configName = Settings.DefaultConfigName;
            string[] lines =
            {
                "dbtype=Sqlite",
                "filename=webgoat_coins.sqlite"
            };
            File.WriteAllLines(configName, lines);
            var mock = new Mock<HttpServerUtilityBase>();
            mock.Setup(x => x.MapPath(It.IsAny<string>())).Returns(Settings.DefaultConfigName);
            HttpServerUtilityBase wrapperServer = mock.Object;
            Settings.Init(wrapperServer);
            IDbProvider dbProvider = Settings.CurrentDbProvider;
            string email = "bob@ateliergraphique.com";
            string pwd = Encoder.Decode("MTIzNDU2");

            // Act
            bool loginOk = dbProvider.IsValidCustomerLogin(email, pwd);

            // Assert
            Assert.IsTrue(loginOk);
            mock.Verify(x => x.MapPath(It.IsAny<string>()), Times.AtLeastOnce());
        }
    }
}
