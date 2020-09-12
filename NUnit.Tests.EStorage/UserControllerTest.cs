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
    class UserControllerTest
    {
        public static UserController uc { get; set; } = new UserController();
        [Test]
        public async Task SignIn()
        {
            string login = "#Testing";
            string password = "Testing123";
            var result = await uc.SignIn(login, password);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ConfirmAction()
        {
            await SignIn();
            string password = "Testing123";
            var result = await uc.ConfirmAction(password);
            Assert.IsTrue(result);
        } 
        [Test]
        public void Logout()
        {
            var result = uc.Logout();
            Assert.IsTrue(result);
        }
    }
}