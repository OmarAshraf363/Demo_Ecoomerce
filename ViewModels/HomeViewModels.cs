﻿using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class HomeViewModels
    {
        public List<Category> Categories { get; set; }=new List<Category>();
        public List<Customer> Customers { get; set; } = new List<Customer>();

        [Required(ErrorMessage = "Phone Is Requerd")]
        [MaxLength(11,ErrorMessage ="Max Is 11")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Email Is Requerd")]
        [DataType(DataType.EmailAddress,ErrorMessage ="in Valid")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}