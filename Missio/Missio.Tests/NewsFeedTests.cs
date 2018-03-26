﻿using System.Collections.Generic;
using Mission.Model.Data;
using Mission.Model.LocalProviders;
using NUnit.Framework;
using Xamarin.UITest;

namespace Missio.Tests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    [Category("UITests")]
    public class NewsFeedTests
    {
        private IApp app;
        private readonly Platform platform;

        public NewsFeedTests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        private static object[] GetOnAppearTestData()
        {
            var testData = new object[LocalUserDatabase.ValidUsers.Count];
            for (var i = 0; i < LocalUserDatabase.ValidUsers.Count; i++)
            {
                var user = LocalUserDatabase.ValidUsers[i];
                testData[i] = new object[] {user, LocalNewsFeedPostProvider.GetMostRecentPostsAsStrings(user)};
            }
            return testData;
        }

        [Test]
        [TestCaseSource(nameof(GetOnAppearTestData))]
        public void OnAppear_GivenNewsFeedPosts_DisplaysPosts(User user, List<string> expectedPosts)
        {
            //Arrange and act
            AppInitializer.TryToLogIn(app, user);

            //Assert
            foreach (var expectedPost in expectedPosts)
            {
                app.WaitForElement(c => c.Text(expectedPost));
            }
        }

        [Test]
        public void AddPostButton_NormalClick_GoesToPublicationPage()
        {
            //Arrange
            AppInitializer.LogIn(app);
            //Act
            app.Tap(c => c.Button("AddPostButton"));
            //Assert
            app.WaitForElement(c => c.Marked("PublicationPage"));
        }
    }
}