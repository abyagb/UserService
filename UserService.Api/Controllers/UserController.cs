using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.Validators;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IEndUserService userService,IMapper mapper) : ControllerBase
    {
        private readonly IEndUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
       
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel userView)
        {
            var validator = new CreateUserValidator();
            var result=await validator.ValidateAsync(userView);
            if (result.IsValid == false) return BadRequest(result.Errors);
            var userDto=_mapper.Map<UserDto>(userView);
            try
            {
                await _userService.CreateAsync(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }
            return Ok(userDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id,UserDto userDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.UpdateAsync(id, userDto);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }

        

    }
}
