﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JetBrains.Annotations;
using Mission.Model.Data;
using Mission.Model.LocalProviders;
using Xamarin.Forms;

namespace ViewModel
{
    public class LocalNewsFeedPostsUpdater : INewsFeedPostsUpdater
    {
        private readonly GlobalUser _globalUser;
        private readonly INewsFeedPostsProvider _postsProvider;

        public LocalNewsFeedPostsUpdater(GlobalUser globalUser, INewsFeedPostsProvider postsProvider)
        {
            _globalUser = globalUser;
            _postsProvider = postsProvider;
        }
        
        /// <inheritdoc />
        public void UpdatePosts(ObservableCollection<NewsFeedPost> posts)
        {
            posts.Clear();
            foreach (var post in _postsProvider.GetMostRecentPosts(_globalUser.LoggedInUser))
            {
                posts.Add(post);
            }
        }
    }
    
    public class NewsFeedViewModel
    {
        private readonly INewsFeedPostsUpdater _postsUpdater;

        [UsedImplicitly]
        public ICommand UpdatePostsCommand;

        [UsedImplicitly]
        public ObservableCollection<NewsFeedPost> Posts { get; } = new ObservableCollection<NewsFeedPost>();

        public NewsFeedViewModel([NotNull] INewsFeedPostsUpdater postsUpdater, [NotNull] IOnUserLoggedIn onUserLoggedIn)
        {
            _postsUpdater = postsUpdater ?? throw new ArgumentNullException(nameof(postsUpdater));
            if (onUserLoggedIn == null)
                throw new ArgumentNullException(nameof(onUserLoggedIn));
            UpdatePostsCommand = new Command(UpdatePosts);
            onUserLoggedIn.OnUserLoggedIn += UpdatePosts;
        }

        public void UpdatePosts()
        {
            _postsUpdater.UpdatePosts(Posts);
        }
    }
}