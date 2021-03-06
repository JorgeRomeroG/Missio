﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataTransferObjects;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Missio.ApplicationResources;
using NUnit.Framework;
using MissioServer.Controllers;
using MissioServer.Services;
using NSubstitute;
using static System.Linq.Enumerable;

namespace MissioServer.Tests
{
    [TestFixture] 
    public class UsersControllerTests
    {
        private static UsersController MakeUsersController(MissioContext missioContext = null, IWebClientService webClientService = null)
        {
            if(missioContext == null)
                missioContext = Utils.MakeMissioContext();
            if(webClientService == null)
                webClientService = Substitute.For<IWebClientService>();
            var passwordService = new MockPasswordService();
            var userService = new UsersService(missioContext, passwordService, webClientService);
            return new UsersController(userService);
        }

        [Test]
        public void ValidateUser_InvalidUserName_ReturnsErrorMessage()
        {
            var usersController = MakeUsersController();

            Assert.ThrowsAsync<InvalidUserNameException>(() => usersController.ValidateUser("Not valid name", ""));
        }

        [Test]
        public void ValidateUser_InvalidPassword_ThrowsException()
        {
            var usersController = MakeUsersController();

            Assert.ThrowsAsync<InvalidPasswordException>(() => usersController.ValidateUser("Francisco Greco", ""));
        }

        [Test]
        public async Task ValidateUser_UserIsValid_ReturnsOk()
        {
            var usersController = MakeUsersController();

            Assert.IsInstanceOf<OkResult>(await usersController.ValidateUser("Francisco Greco", "ElPass"));
        }

        [Test]
        public async Task RegisterUser_UserNameIsTooShort_ThrowsException()
        {
            var usersController = MakeUsersController();
            var registration = new CreateUserDTO("ABC", "Password", "someEmail@gmail.com");

            var result = (ObjectResult) await usersController.RegisterUser(registration);

            Assert.Contains(Strings.UserNameTooShortMessage, (List<string>) result.Value);
        }

        [Test]
        public async Task RegisterUser_UserNameAlreadyInUse_ThrowsException()
        {
            var usersController = MakeUsersController();
            var registration = new CreateUserDTO("Francisco Greco", "Password", "someEmail@gmail.com");

            var result = (ObjectResult)await usersController.RegisterUser(registration);

            Assert.Contains(Strings.UserNameAlreadyInUseMessage, (List<string>) result.Value);
        }

        [Test]
        public async Task RegisterUser_PasswordIsTooShort_ThrowsExceptionAsync()
        {
            var usersController = MakeUsersController();
            var registration = new CreateUserDTO("ABCD", "ABC", "someEmail@gmail.com");

            var result = (ObjectResult) await usersController.RegisterUser(registration);

            Assert.Contains(Strings.PasswordTooShortMessage, (List<string>) result.Value);
        }

        [Test]
        [TestCase("ValidName", "Password", "someEmail@gmail.com")]
        public async Task RegisterUser_EverythingOkButPictureIsNull_SetsDefaultPicture(string name, string password, string email)
        {
            var webClientService = Substitute.For<IWebClientService>();
            var defaultPicture = new byte[1];
            webClientService.DownloadData(Arg.Any<string>()).Returns(defaultPicture);
            var missioContext = Utils.MakeMissioContext();
            var usersController = MakeUsersController(missioContext, webClientService);
            var registration = new CreateUserDTO(name, password, email);

            await usersController.RegisterUser(registration);

            Assert.IsTrue(missioContext.Users.Any(x => x.UserName == name && x.Picture == defaultPicture));
        }

        [Test]
        [TestCase("ValidName", "Password", "someEmail@gmail.com")]
        public async Task RegisterUser_EverythingOk_RegistersUser(string name, string password, string email)
        {
            var missioContext = Utils.MakeMissioContext();
            var usersController = MakeUsersController(missioContext);
            var picture = new byte[1];
            var registration = new CreateUserDTO(name, password, email, picture);

            await usersController.RegisterUser(registration);

            Assert.IsTrue(missioContext.Users.Any(x => x.UserName == name && x.Email == email && x.Picture == picture));
            Assert.IsTrue(missioContext.UsersCredentials.Any(x => x.User.UserName == name && x.HashedPassword == "Hashed" + password));
        }
    }
}
