using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain;
using UserService.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IEndUserService userService, IMapper mapper, IValidator<CreateUserViewModel> userValidator, IValidator<EditUserViewModel> editUserValidator) : ControllerBase
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, EditUserViewModel viewModel)
        {
            var validationResult = editUserValidator.Validate(viewModel);
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
            var userPatchDto = MapToPatchDto(id, viewModel);
            await userService.UpdateAsync(id, userPatchDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await userService.DeleteAsync(id);
            return NoContent();
        }

        private static UserPatchDto MapToPatchDto(Guid userId, EditUserViewModel viewModel)
        {
            try
            {
                var patchDto = new UserPatchDto
                {
                    UserId = userId,
                    FieldsToUpdate = new Dictionary<string, object>()
                };

                var viewModelType = typeof(EditUserViewModel);

                foreach (var property in viewModelType.GetProperties())
                {
                    var viewModelValue = property.GetValue(viewModel);
                    if (viewModelValue != null)
                    {
                        patchDto.FieldsToUpdate.Add(property.Name, viewModelValue);
                    }
                }
                return patchDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error mapping model to Patch DTO", ex);
            }
        }
    }
}
