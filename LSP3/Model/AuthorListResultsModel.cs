﻿using System.ComponentModel.DataAnnotations;

namespace LSP3.Model
{
    public class AuthorListResultsModel
    {
        [Display(Name = "AuthorID")]
        public int AuthorID { get; set; }

        [Display(Name = "Prefix")]
        [DataType(DataType.Text)]
        public string? Prefix { get; set; }

        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [DataType(DataType.Text)]
        public string? MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [Display(Name = "Suffix")]
        [DataType(DataType.Text)]
        public string? Suffix { get; set; }

        [Display(Name = "EMail")]
        [DataType(DataType.Text)]
        public string? EMail { get; set; }

        [Display(Name = "EditLink")]
        [DataType(DataType.Text)]
        public string? EditLink { get; set; }

        [Display(Name = "DeleteLink")]
        [DataType(DataType.Text)]
        public string? DeleteLink { get; set; }
    }
}
