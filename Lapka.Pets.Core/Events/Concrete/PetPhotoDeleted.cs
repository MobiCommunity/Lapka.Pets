using Lapka.Pets.Core.Events.Abstract;

namespace Lapka.Pets.Core.Events.Concrete
{
    public class PetPhotoDeleted : IDomainEvent
    {
        public string PhotoPath { get; }

        public PetPhotoDeleted(string photoPath)
        {
            PhotoPath = photoPath;
        }
    }
}