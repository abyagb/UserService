using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.Validators;
using UserService.Api.ViewModels;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IEndUserService userService, IMapper mapper, ILogger<UserController> logger, IValidator<CreateUserViewModel> userValidator) : ControllerBase
    {
        private readonly IEndUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;
        private readonly IValidator<CreateUserViewModel> _userValidator = userValidator;
       
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel userView)
        {
            var validatorResult = _userValidator.Validate(userView);
            if (validatorResult.IsValid == false) return BadRequest(validatorResult.Errors);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString();
            var userDto=_mapper.Map<UserDto>(userView);
            try
            {
                await _userService.CreateAsync(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CreateAsync - Correlation ID: {CorrelationId}", correlationId);
                return BadRequest();
            }
            _logger.LogInformation("User created successfully - Correlation ID: {CorrelationId}", correlationId);
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString();
            if (userDto == null)
            {
                
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                return NotFound();
            }
            _logger.LogInformation("Fetched user {UserId} - Correlation ID: {CorrelationId}", id, correlationId);
            return Ok(userDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id,UserDto userDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString();

            if (user == null)
            {
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                return NotFound();
            }
            await _userService.UpdateAsync(id, userDto);
            _logger.LogInformation("User {UserId} updated successfully - Correlation ID: {CorrelationId}", id, correlationId);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString();
            if (user == null)
            {
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                return NotFound();
            }
            _logger.LogInformation("User {UserId} deleted successfully - Correlation ID: {CorrelationId}", id, correlationId);
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        

    }
}
