﻿using Demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class AddOrder
    {
        public Product Product { get; set; }
        
        public int? CustomerId { get; set; }

        public byte OrderStatus { get; set; }

        
        public DateOnly OrderDate { get; set; }

        
        public DateOnly RequiredDate { get; set; }

        
        public DateOnly? ShippedDate { get; set; }

        public int StoreId { get; set; }

        public int StaffId { get; set; }
    }
}