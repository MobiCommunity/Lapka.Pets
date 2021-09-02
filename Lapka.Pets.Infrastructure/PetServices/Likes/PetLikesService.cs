using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Pets.Application.Exceptions;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Infrastructure.PetServices.Likes
{
    public class PetLikesService : IPetLikesService
    {
        private readonly IPetLikeRepository _petLikeRepository;

        public PetLikesService(IPetLikeRepository petLikeRepository)
        {
            _petLikeRepository = petLikeRepository;
        }
        public async Task LikePet(Guid petId, Guid userId)
        {
            UserLikedPets likedPets = await _petLikeRepository.GetLikedPetsAsync(userId);
            
            if (!likedPets.LikedPets.Contains(petId))
            {
                likedPets.LikedPets.Add(petId);
            }
            else
            {
                throw new UserLikePetAlreadyException(userId, petId);
            }
            
            await _petLikeRepository.UpdateLikes(likedPets);
        }

        public async Task DislikePet(Guid petId, Guid userId)
        {
            UserLikedPets likedPets = await _petLikeRepository.GetLikedPetsAsync(userId);
            
            if (likedPets.LikedPets.Contains(petId))
            {
                likedPets.LikedPets.Remove(petId);
            }
            else
            {
                throw new UserDoesNotLikePetException(userId, petId);
            }
            
            await _petLikeRepository.UpdateLikes(likedPets);
        }

        public async Task<IEnumerable<Guid>> GetUserLikedPetIdsAsync(Guid userId)
        {
            UserLikedPets likedPets = await _petLikeRepository.GetLikedPetsAsync(userId);

            return likedPets.LikedPets;
        }
        
        public async Task AddUserPetList(Guid userId)
        {
            await _petLikeRepository.AddUserPetList(new UserLikedPets(userId, new List<Guid>()));
        }

        public async Task DeleteUserPetList(Guid userId)
        {
            IEnumerable<Guid> likes = await GetUserLikedPetIdsAsync(userId);
            UserLikedPets userLikes = new UserLikedPets(userId, likes.ToList());
            
            await _petLikeRepository.DeleteUserPetList(userLikes);
        }
    }
}