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
    class AccountControllerTest
    {
        UserControllerTest uc = new UserControllerTest();
        //ManageController mc = new ManageController();
        AccountController ac = new AccountController();
        StorageController sc = new StorageController();
        StorageControllerTest sct = new StorageControllerTest();
        [Test]
        public async Task GetAccounts()
        {
            await uc.SignIn();
            await sct.EnterKey();
            var result = ac.GetAccounts();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Create()
        {
            await uc.SignIn();
            await sct.EnterKey();
            var result = await ac.Create("1", "2", "3", "4");
            Assert.IsTrue(result);
        }
    }
}