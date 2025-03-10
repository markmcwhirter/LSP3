﻿using System.ComponentModel.DataAnnotations; // If you need data annotations

namespace LSP3.Model
{
    public class AuthorSearchModel
    {

        [Display(Name = "First Name")] // Adjust display names as needed 
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? SortOrder { get; set; }
        public string? Direction { get; set; }

    }
}
