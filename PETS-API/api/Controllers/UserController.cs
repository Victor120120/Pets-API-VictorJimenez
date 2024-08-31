using api.Data;
using api.Dtos.User;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.Include(user => user.pets).ToListAsync();
            var usersDto = users.Select(user => user.ToDto());
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _context.Users.Include(user => user.pets).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto userDto)
        {
            var userModel = userDto.ToUserFromCreateDto();
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDto userDto)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }
            userModel.Age = userDto.Age;
            userModel.FirstName = userDto.FirstName;
            userModel.LastName = userDto.LastName;

            await _context.SaveChangesAsync();

            return Ok(userModel.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{userId}/assign-pet/{petId}")]
        public async Task<IActionResult> AssignPetToUser([FromRoute] int userId, [FromRoute] int petId)
        {
            var user = await _context.Users.Include(u => u.pets).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            if (user.pets.Any(p => p.Id == petId))
            {
                return BadRequest("Pet is already assigned to this user");
            }

            user.pets.Add(pet);
            await _context.SaveChangesAsync();

            return Ok(user.ToDto());
        }

        [HttpPost("create-with-pets")]
        public async Task<IActionResult> CreateUserWithPets([FromBody] CreateUserWithPetsRequestDto requestDto)
        {
            var userModel = requestDto.ToUserFromCreateDto();

            foreach (var petDto in requestDto.Pets)
            {
                var petModel = petDto.ToPetFromCreateDto();
                await _context.Pets.AddAsync(petModel);
                userModel.pets.Add(petModel);
            }

            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = userModel.Id }, userModel.ToDto());
        }
    }
}
