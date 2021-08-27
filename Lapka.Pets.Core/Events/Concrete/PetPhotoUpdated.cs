using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotoUpdated : IDomainEvent
    {
        public string PhotoPath { get; }

        public PetPhotoUpdated(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}