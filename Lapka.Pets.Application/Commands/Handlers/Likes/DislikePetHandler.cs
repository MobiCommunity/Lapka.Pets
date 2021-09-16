using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Commands.ShelterPets;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Commands.Handlers.Likes
{
    public class DislikePetHandler : ICommandHandler<DislikePet>
    {
        private readonly IEventProcessor _eventProcessor;
        private readonly IShelterPetRepository _shelterPetRepository;
        private readonly IPetLikeRepository _likeRepository;


        public DislikePetHandler(IEventProcessor eventProcessor, IShelterPetRepository shelterPetRepository,
            IPetLikeRepository likeRepository)
        {
            _eventProcessor = eventProcessor;
            _shelterPetRepository = shelterPetRepository;
            _likeRepository = likeRepository;
        }

        public async Task HandleAsync(DislikePet command)
        {
            ShelterPet pet = await GetShelterPetAsync(command);

            UserLikedPets likedPets = await GetUserLikedPetsListAsync(command);

            likedPets.RemoveLike(pet.Id.Value);

            await _likeRepository.UpdateLikesAsync(likedPets);
            await _eventProcessor.ProcessAsync(likedPets.Events);
        }

        private async Task<UserLikedPets> GetUserLikedPetsListAsync(DislikePet command)
        {
            UserLikedPets likedPets = await _likeRepository.GetLikedPetsAsync(command.UserId);
            if (likedPets is null)
            {
                await _likeRepository.AddUserPetListAsync(new UserLikedPets(command.UserId, new List<Guid>()));
                likedPets = await _likeRepository.GetLikedPetsAsync(command.UserId);
            }

            return likedPets;
        }

        private async Task<ShelterPet> GetShelterPetAsync(DislikePet command)
        {
            ShelterPet pet = await _shelterPetRepository.GetByIdAsync(command.PetId);
            if (pet is null)
            {
                throw new PetNotFoundException(command.PetId);
            }

            return pet;
        }
    }
}