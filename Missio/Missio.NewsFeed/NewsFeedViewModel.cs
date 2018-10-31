﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Missio.PostPublication;
using Missio.Posts;
using Missio.Users;
using Mission.ViewModel;
using Xamarin.Forms;
using INavigation = Missio.Navigation.INavigation;

namespace Missio.NewsFeed
{
    public class NewsFeedViewModel : ViewModel, IUpdateViewPosts
    {
        [UsedImplicitly]
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetField(ref _isRefreshing, value);
        }

        [UsedImplicitly]
        public ICommand UpdatePostsCommand { get; }

        [UsedImplicitly]
        public ICommand GoToPublicationPageCommand { get; }

        [UsedImplicitly]
        public ObservableCollection<IPost> Posts { get; } = new ObservableCollection<IPost>();

        private readonly IPostRepository _postRepository;
        private readonly INavigation _navigation;
        private readonly ILoggedInUser _loggedInUser;
        private bool _isRefreshing;

        public NewsFeedViewModel([NotNull] IPostRepository postRepository, [NotNull] INavigation navigation,
            [NotNull] ILoggedInUser loggedInUser)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _loggedInUser = loggedInUser ?? throw new ArgumentNullException(nameof(loggedInUser));
            UpdatePostsCommand = new Command(async () => await UpdatePosts());
            GoToPublicationPageCommand = new Command(async() => await GoToPublicationPage());
            UpdatePosts(); 
        }

        public async Task UpdatePosts()
        {
            Posts.Clear();
            foreach (var post in await _postRepository.GetMostRecentPostsInOrder(_loggedInUser.UserName, _loggedInUser.Password))
                Posts.Add(post);
            IsRefreshing = false;
        }

        private async Task GoToPublicationPage()
        {
            await _navigation.GoToPage<PublicationPage>();
        }
    }
}