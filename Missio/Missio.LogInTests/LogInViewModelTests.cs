﻿using System.Threading.Tasks;
using Missio.LocalDatabase;
using Missio.LogIn;
using Missio.LogInRes;
using Missio.Users;
using NSubstitute;
using NUnit.Framework;
using INavigation = Missio.Navigation.INavigation;

namespace Missio.LogInTests
{
    [TestFixture]
    public class LogInViewModelTests
    {
        private LogInViewModel _logInViewModel;
        private INavigation _fakeNavigation;
        private ILoggedInUser _fakeLoggedInUser;
        private IUserRepository _fakeUserRepository;

        [SetUp]
        public void SetUp()
        {
            _fakeNavigation = Substitute.For<INavigation>();
            _fakeLoggedInUser = Substitute.For<ILoggedInUser>();
            _fakeUserRepository = Substitute.For<IUserRepository>();
            _logInViewModel = new LogInViewModel(_fakeNavigation, _fakeUserRepository, _fakeLoggedInUser);
        }

        [Test]
        public void Constructor_NormalConstructor_InitializesUserWithEmptyFields()
        {
            Assert.IsEmpty(_logInViewModel.UserName);
            Assert.IsEmpty(_logInViewModel.Password);
        }

        [Test]
        public void UserNameSetter_ValueIsNull_IsSetToEmptyString()
        {
            _logInViewModel.UserName = null;

            Assert.IsEmpty(_logInViewModel.UserName);
        }

        [Test]
        public void PasswordSetter_ValueIsNull_IsSetToEmptyString()
        {
            _logInViewModel.Password = null;

            Assert.IsEmpty(_logInViewModel.Password);
        }

        [Test]
        public void GoToRegistrationPageCommand_NormalExecute_GoesToRegistrationPage()
        {
            _logInViewModel.GoToRegistrationPageCommand.Execute(null);

            _fakeNavigation.Received(1).GoToPage<RegistrationPage>();
        }

        [Test]
        public async Task LogInCommand_ValidUser_SetsLoggedInUserAndGoesToNextPage()
        {
            var user = new User("Someone", "Password");
            _fakeUserRepository.GetUserIfValid("Someone", "Password").Returns(user);
            _logInViewModel.UserName = "Someone";
            _logInViewModel.Password = "Password";

            await _logInViewModel.LogIn();

            _fakeLoggedInUser.Received(1).LoggedInUser = user;
            await _fakeNavigation.Received(1).GoToPage<MainTabbedPage>();
        }

        [Test]
        public async Task AttemptToLogin_InvalidPassword_DisplaysAlert()
        {
            _logInViewModel.UserName = "Someone";
            _fakeUserRepository.When(x => x.GetUserIfValid("Someone", "")).Throw<InvalidPasswordException>();

            await _logInViewModel.LogIn();

            await _fakeNavigation.Received(1).DisplayAlert(new InvalidPasswordException().AlertTextMessage);
        }

        [Test]
        public async Task AttemptToLogin_InvalidUserName_DisplaysAlert()
        {
            _logInViewModel.UserName = "Someone";
            _fakeUserRepository.When(x => x.GetUserIfValid("Someone", "")).Throw<InvalidUserNameException>();

            await _logInViewModel.LogIn();

            await _fakeNavigation.Received(1).DisplayAlert(new InvalidUserNameException().AlertTextMessage);
        }
    }
}