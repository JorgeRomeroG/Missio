﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Missio.Posts;
using Missio.Users;
using MissioServer.Services.Services;

namespace MissioServer.Services
{
    public class MissioContext : DbContext
    {
        private readonly IPasswordHasher<User> _passwordService;
        private readonly IWebClientService _webClientService;

        public DbSet<User> Users { get; set; }
        public DbSet<UserFriends> UsersFriends { get; set; }
        public DbSet<UserCredentials> UsersCredentials { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<StickyPost> StickyPosts { get; set; }

        public MissioContext(DbContextOptions<MissioContext> options, IPasswordHasher<User> passwordService, IWebClientService webClientService) : base(options)
        {
            _webClientService = webClientService;
            _passwordService = passwordService;
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedUsers();
            SeedCredentials();
            SeedPosts();
            SeedFriends();
            SeedStickyPosts();

            void SeedUsers()
            {
                var grecoImage = _webClientService.DownloadData("https://scontent.felp1-1.fna.fbcdn.net/v/t1.0-9/18342049_1371435649562155_317149840395279012_n.jpg?_nc_cat=0&oh=74b6c0226537899a74f499c25b3ddb07&oe=5C00CF82");
                var jorgeImage = _webClientService.DownloadData("https://scontent.felp1-1.fna.fbcdn.net/v/t1.0-9/18342049_1371435649562155_317149840395279012_n.jpg?_nc_cat=0&oh=74b6c0226537899a74f499c25b3ddb07&oe=5C00CF82");
                var greco = new {UserName = "Francisco Greco", Picture = grecoImage, Email = "myEmail@gmail.com", Id = -1, UserFriendsId = -2 };
                var jorge = new {UserName = "Jorge Romero", Picture = jorgeImage, Email = "anotherEmail@gmail.com", Id = -2, UserFriendsId = -1 };
                modelBuilder.Entity<User>().HasData(greco, jorge);
            }

            void SeedCredentials()
            {
                var grecoCredentials = new { Id = -1, UserId = -1, HashedPassword = _passwordService.HashPassword("ElPass") };
                var jorgeCredentials = new { Id = -2, UserId = -2, HashedPassword = _passwordService.HashPassword("Yolo") };
                modelBuilder.Entity<UserCredentials>().HasData(grecoCredentials, jorgeCredentials);
            }

            void SeedFriends()
            {
                var grecoFriends = new {Id = -1, UserId = -1 };
                var jorgeFriends = new {Id = -2, UserId = -2 };
                modelBuilder.Entity<UserFriends>().HasData(grecoFriends, jorgeFriends);
            }

            void SeedPosts()
            {
                modelBuilder.Entity<Post>().HasData(
                    new {AuthorId = -1, PublishedDate = new DateTime(2018, 1, 1), Id = -1, Message = "First message written by Greco", Image = _webClientService.DownloadData("https://images2.alphacoders.com/602/thumb-1920-602223.jpg") },
                    new {AuthorId = -1, PublishedDate = new DateTime(2018, 1, 2), Id = -2, Message = "Second message written by Greco", Image = _webClientService.DownloadData("https://images-eds-ssl.xboxlive.com/image?url=8Oaj9Ryq1G1_p3lLnXlsaZgGzAie6Mnu24_PawYuDYIoH77pJ.X5Z.MqQPibUVTc88m2VEnuTLB8d0DOf71kf9zLge.JQeY40W.K7bxV5oSXFxdH1MZYfvu4.uaO8xIU1V9nbqmD60ax7aG8dgfPHi6nMT.dV6kUzMwNKv4SE0tH_gKhP.hCZYTNLqn36oQndGI_jLJwNDu2j1ZkHhtoFxf2VZU2eXHud5ZP7VXxn4M-&h=1080&w=1920&format=jpg") },
                    new {AuthorId = -2, PublishedDate = new DateTime(2018, 2, 3), Id = -3, Message = "First message written by Jorge" });
            }

            void SeedStickyPosts()
            {
                modelBuilder.Entity<StickyPost>().HasData(new { Id = -1, Message = "A sticky post message", Title="A sticky post title"});
            }
        }
    }
}
