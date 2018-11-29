﻿using System;
using System.Collections.Generic;
using System.Linq;
using Missio.Posts;
using Missio.Users;

namespace Missio.NewsFeedTests
{
    public static class Utils
    {
        public static Post MakeDummyPost()
        {
            return new Post(new User("Francisco Greco", new byte[4], "email@gmail.com"), "A message", new DateTime(2018, 1, 1));
        }

        public static IOrderedEnumerable<IPost> MakeSortedDummyPost()
        {
            return new List<IPost> { MakeDummyPost() }.OrderBy(x => x.GetPostPriority());
        }
    }
}