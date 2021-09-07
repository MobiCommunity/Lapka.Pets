﻿using System;
using Lapka.Identity.Application.Dto;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class PetDetailsLostDto : PetDetailsDto
    {
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LostDate { get; set; }
        public AddressDto LostAddress { get; set; }
        public string Description { get; set; }
        public double? Distance { get; set; }
    }
}