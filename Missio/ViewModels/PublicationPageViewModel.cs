﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Domain;
using Domain.DataTransferObjects;
using Domain.Repositories;
using JetBrains.Annotations;
using Xamarin.Forms;
using INavigation = Missio.Navigation.INavigation;

namespace ViewModels
{
    public class PublicationPageViewModel
    {
        private readonly NameAndPassword _nameAndPassword;
        private readonly Action _postPublishedCallBack;
        private readonly IPostRepository _postRepository;
        private readonly INavigation _navigation;
        private string _postText;
        
        [UsedImplicitly]
        public ICommand PublishPostCommand { get; }

        [UsedImplicitly]
        public string PostText
        {
            get => _postText ?? "";
            set => _postText = value;
        }

        public PublicationPageViewModel([NotNull] IPostRepository postRepository, [NotNull] NameAndPassword nameAndPassword,
            [NotNull] Action postPublishedCallBack, [NotNull] INavigation navigation)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _nameAndPassword = nameAndPassword ?? throw new ArgumentNullException(nameof(nameAndPassword));
            _postPublishedCallBack = postPublishedCallBack ?? throw new ArgumentNullException(nameof(postPublishedCallBack));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            PublishPostCommand = new Command(async() => await PublishPost());
        }

        public async Task PublishPost()
        {
            await _postRepository.PublishPost(new CreatePostDTO(_nameAndPassword, PostText, null));
            _postPublishedCallBack();
            await _navigation.ReturnToPreviousPage();
        }
    }
}