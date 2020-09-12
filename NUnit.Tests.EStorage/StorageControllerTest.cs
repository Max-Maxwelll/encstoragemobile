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
    class StorageControllerTest
    {
        UserControllerTest uc = new UserControllerTest();
        ManageController mc = new ManageController();
        StorageController sc = new StorageController();

        [Test]
        public async Task GetStorages()
        {
            await uc.SignIn();
            var result = sc.GetStorages();
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Create()
        {
            await uc.SignIn();
            var user = await mc.GetUserAsync();
            int rand = new Random().Next(1000, 100000);
            var result = await sc.Create(user.UserName, rand.ToString(), "1111111111111111");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ChangeKey()
        {
            await uc.SignIn();
            await Create();
            var storages = sc.GetStorages();
            string key = "2222222222222222";
            bool result = await sc.ChangeKey(storages.LastOrDefault().Name, "1111111111111111", key, key);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Delete()
        {
            await uc.SignIn();
            await Create();
            var storages = sc.GetStorages();
            bool result = sc.Delete(storages.LastOrDefault().Name);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAll()
        {
            await uc.SignIn();
            await Create();
            bool result = sc.DeleteAll();
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EnterKey()
        {
            await uc.SignIn();
            await Create();
            var storages = sc.GetStorages();
            bool result = sc.EnterKey(storages.LastOrDefault().Name, "1111111111111111");
            Assert.IsTrue(result);
        }
    }
}