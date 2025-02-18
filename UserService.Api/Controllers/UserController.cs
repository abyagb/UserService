using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IEndUserService userService, IMapper mapper, IValidator<CreateUserViewModel> userValidator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel userViewModel)
        {
            var validationResult = userValidator.Validate(userViewModel);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors
               .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage })
                .ToList();

                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation failed",
                    ValidationErrors = validationErrors
                });
            }

            var userDto = mapper.Map<UserDto>(userViewModel);
            await userService.CreateAsync(userDto);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var userDto = await userService.GetUserByIdAsync(id);
            return Ok(userDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, UserDto userDto)
        {
            await userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
