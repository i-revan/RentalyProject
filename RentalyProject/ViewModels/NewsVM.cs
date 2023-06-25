﻿using RentalyProject.Models;

namespace RentalyProject.ViewModels
{
    public class NewsVM
    {
        public IEnumerable<News> News { get; set; }
        public IEnumerable<News>? NewsForPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public News? news { get;set; }  

    }
}
