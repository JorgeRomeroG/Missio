﻿using Mission.Model.Data;
using NSubstitute;
using NUnit.Framework;
using ViewModel;

namespace Missio.Tests
{
    [TestFixture]
    public class LogInViewModelTests
    {
        private LogInViewModel LogInViewModel;
        private IAttemptToLogin AttemptToLogin;
        private IGoToView FakeGoToView;

        [SetUp]
        public void SetUp()
        {
            AttemptToLogin = Substitute.For<IAttemptToLogin>();
            FakeGoToView = Substitute.For<IGoToView>();
            LogInViewModel = new LogInViewModel(AttemptToLogin, FakeGoToView);
        }

        [Test]
        public void GetPassword_FieldIsNull_ReturnsEmptyString()
        {
            //Arrange
            //Act
            var password = LogInViewModel.Password;
            //Assert
            Assert.AreEqual("", password);
        }

        [Test]
        public void GetUserName_FieldIsNull_ReturnsEmptyString()
        {
            //Arrange

            //Act
            var name = LogInViewModel.UserName;
            //Assert
            Assert.AreEqual("", name);
        }

        [Test]
        [TestCase("Paco", "ElPass")]
        [TestCase("Jorge", "999")]
        [TestCase("UnMen", "password")]
        public void LogInCommand_GivenUserNameAndPassword_AttemptsToLogin(string userName, string password)
        {
            //Arrange
            LogInViewModel.UserName = userName;
            LogInViewModel.Password = password;
            //Act
            LogInViewModel.LogInCommand.Execute(null);
            //Assert
            AttemptToLogin.Received(1)
                .AttemptToLoginWithUser(Arg.Is<User>(x => x.UserName == userName && x.Password == password));
        }

        [Test]
        public void GoToRegistrationPageCommand_NormalExecute_GoesToRegistrationPage()
        {
            //Arrange

            //Act
            LogInViewModel.GoToRegistrationPageCommand.Execute(null);
            //Assert
            FakeGoToView.Received(1).GoToView("Registration page");
        }
    }
}