﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.Entities;

namespace Lapka.Pets.Application.Commands.Handlers
{
    public class UpdateLostPetHandler : ICommandHandler<UpdateLostPet>
    {
        private readonly ILostPetService _petService;

        public UpdateLostPetHandler(ILostPetService petService)
        {
            _petService = petService;
        }

        public async Task HandleAsync(UpdateLostPet command)
        {
            LostPet pet = await _petService.GetAsync(command.Id);
            _petService.ValidIfUserIsOwnerOfPet(pet, command.UserId);

            pet.Update(command.Name, command.Race, command.Species, command.Sex,
                DateTime.Now.Subtract(TimeSpan.FromDays(365 * command.Age)), command.Weight,
                command.Color, command.OwnerName, command.PhoneNumber, command.LostDate, command.LostAddress,
                command.Description);

            await _petService.UpdateAsync(pet);
        }
    }
}