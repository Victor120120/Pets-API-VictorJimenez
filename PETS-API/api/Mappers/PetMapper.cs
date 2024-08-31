using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Pet;
using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class PetMapper
    {
        public static PetDto ToDto(this Pet pet)
        {
            return new PetDto
            {
                Name = pet.Name,
                Animal = pet.Animal
            };
        }

        public static Pet ToPetFromCreateDto(this CreatePetRequestDto petDto)
        {
            return new Pet
            {
                Name = petDto.Name,
                Animal = petDto.Animal
            };
        }
    }
}