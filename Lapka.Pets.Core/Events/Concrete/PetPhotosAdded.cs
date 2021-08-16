using System.Collections.Generic;
using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotosAdded : IDomainEvent
    {
        public List<string> PhotoPath { get; }

        public PetPhotosAdded(List<string> photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}