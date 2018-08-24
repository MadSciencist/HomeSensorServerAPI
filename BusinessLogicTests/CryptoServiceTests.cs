using HomeSensorServerAPI.PasswordCryptography;
using NUnit.Framework;

namespace BusinessLogicTests
{
    class CryptoServiceTests
    {
        [Test]
        public void CanCreateHashString()
        {
            var password = "pasSS2$%swss112ord@@11.,,";
            var service = new PasswordCryptoSerivce();
            var hashedPassword = service.CreateHashString(password);

            Assert.IsInstanceOf<string>(hashedPassword);
        }

        [Test]
        public void CanHashAndDehashPassowrd()
        {
            var password = "pasSS2$%swss112ord@@11.,,";
            var service = new PasswordCryptoSerivce();
            var hashedPassword = service.CreateHashString(password);
            
            Assert.IsTrue(service.IsPasswordMatching(hashedPassword, password));
        }
    }
}
