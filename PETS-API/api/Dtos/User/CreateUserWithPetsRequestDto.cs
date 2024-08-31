using System.Collections.Generic;

namespace api.Dtos.User
{
    public class CreateUserWithPetsRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<CreatePetRequestDto> Pets { get; set; } = new List<CreatePetRequestDto>();
    }
}
