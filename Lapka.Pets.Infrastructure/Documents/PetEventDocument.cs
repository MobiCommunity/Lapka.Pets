﻿using System;

namespace Lapka.Pets.Infrastructure.Documents
{
    public class PetEventDocument
    {
        public Guid Id { get; set; }
        public DateTime DateOfEvent { get; set; }
        public string DescriptionOfEvent { get; set; }
    }
}