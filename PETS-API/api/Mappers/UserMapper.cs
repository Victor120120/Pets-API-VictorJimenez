using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Pet;
using api.Dtos.User;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User user){
            return new UserDto {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                PetList = user.pets.Select(PetMapper.ToDto).ToList()
            };
        }

        public static User ToUserFromCreateDto(this CreateUserRequestDto createUserRequest){
            return new User{
                FirstName = createUserRequest.FirstName,
                LastName = createUserRequest.LastName,
                Age = createUserRequest.Age
            };
        }

         public static User ToUserFromCreateDto(this CreateUserWithPetsRequestDto requestDto)
    {
        return new User
        {
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Age = requestDto.Age,
            pets = new List<Pet>() // Inicializar la lista de mascotas
        };
    }

    }
}