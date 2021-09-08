using System.Collections.Generic;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Application.Dto.Pets
{
    public class PetDetailsUserDto : PetDetailsDto
    {
        public IEnumerable<PetEventDto> PetEvents { get; set; }
        public IEnumerable<VisitDto> Visits { get; set; }
    }
}