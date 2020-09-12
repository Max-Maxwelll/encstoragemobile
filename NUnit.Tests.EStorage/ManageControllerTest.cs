using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EncryptedStorageA.App_Code.Controllers;
using NUnit.Framework;

namespace NUnit.Tests.EStorage
{
    [TestFixture]
    class ManageControllerTest
    {
        UserControllerTest uc = new UserControllerTest();
        ManageController mc = new ManageController();
        [Test]
        public async Task GetUserAsync()
        {
            await uc.SignIn();
            var result = await mc.GetUserAsync();
            Assert.IsNotNull(result);
        }

        [Test]
        public void SetSession()
        {
            mc.SetSession("dsdasdasd");
        }
    }
}