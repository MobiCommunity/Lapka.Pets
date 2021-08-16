using System.Collections.Generic;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotoAdded : IDomainEvent
    {
        public string PhotoPath { get; }

        public PetPhotoAdded(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}