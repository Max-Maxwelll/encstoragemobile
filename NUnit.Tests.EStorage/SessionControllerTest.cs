using EncryptedStorageA.App_Code.Controllers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests.EStorage
{
    [TestFixture]
    public class SessionControllerTest
    {
        SessionController sc = new SessionController();
        UserControllerTest uc = new UserControllerTest();
        [Test]
        public async Task RestoreSession()
        {
            var result = await sc.RestoreSession("dsdasd");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateSession()
        {
            await uc.SignIn();
            var result = await sc.RestoreSession("dsdasdasdasdasd");
            Assert.IsTrue(result);
        }
    }
}
