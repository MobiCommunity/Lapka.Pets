﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Lapka.Pets.Api.Models.Request
{
    public class AddSoonEventRequest
    {
        [Required]
        public DateTime DateOfEvent { get; set; }
        [Required]
        public string DescriptionOfEvent { get; set; }
    }
}