using System;
using System.Collections.Generic;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Dto.Pets
{
    public abstract class PetDetailsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Species Species { get; set; }
        public string Race { get; set; }
        public string MainPhotoPath { get; set; }
        public IEnumerable<string> PhotoPaths { get; set; }
        public DateTime BirthDay { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
        public bool Sterilization { get; set; }
    }
}