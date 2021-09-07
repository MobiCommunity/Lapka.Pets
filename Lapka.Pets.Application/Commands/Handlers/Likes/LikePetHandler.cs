using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class LikePetHandler : ICommandHandler<LikePet>
    {
        private readonly IShelterPetRepository _shelterPetRepository;
        private readonly IPetLikeRepository _likeRepository;

        public LikePetHandler(IShelterPetRepository shelterPetRepository, IPetLikeRepository likeRepository)
        {
            _shelterPetRepository = shelterPetRepository;
            _likeRepository = likeRepository;
        }
        public async Task HandleAsync(LikePet command)
        {
            ShelterPet pet = await _shelterPetRepository.GetByIdAsync(command.PetId);
            if (pet == null)
            {
                throw new PetNotFoundException(command.PetId);
            }
            
            UserLikedPets likedPets = await _likeRepository.GetLikedPetsAsync(command.UserId);
            if (likedPets == null)
            {
                await _likeRepository.AddUserPetListAsync(new UserLikedPets(command.UserId, new List<Guid>()));
                likedPets = await _likeRepository.GetLikedPetsAsync(command.UserId);
            }
            likedPets.LikedPets.Add(pet.Id.Value);
            
            await _likeRepository.UpdateLikesAsync(likedPets);
        }
    }
}