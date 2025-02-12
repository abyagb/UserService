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
    public class UserController(IEndUserService userService, IMapper mapper, ILogger<UserController> logger, IValidator<CreateUserViewModel> userValidator) : ControllerBase
    {
        //remove these as they are not being used
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
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel userView) //userView should be renamed to userViewModel
        {
            //have you tested what this looks like when you run the api and validation fails, have you tested that it actually works?
            var validatorResult = await _userValidator.ValidateAsync(userView);
            /**
             * The code below needs to be cleaned up, something like this:
             * if(!validationResult.IsValid)
             * {
             *   return BadRequest(validationResult.Errors);
             * }*
             */
            if (validatorResult.IsValid == false) return BadRequest(validatorResult.Errors);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString(); //this should be moved to a middleware, it is bad practise to set the correlationId here
            //research how to set the correlationId in a middleware and then access it in the controller
            var userDto=_mapper.Map<UserDto>(userView); //there should be spaces between the equals sign
            try
            {
                await _userService.CreateAsync(userDto);
            }
            catch (Exception ex)
            {
                //our log message doesn't need to be so specific to the method
                //a better message would be "Error creating user - Correlation ID: {CorrelationId}"
                _logger.LogError(ex, "Error during CreateAsync - Correlation ID: {CorrelationId}", correlationId);
                return BadRequest();
            }
            // we don't need to log the user creation, this is not necessary
            _logger.LogInformation("User created successfully - Correlation ID: {CorrelationId}", correlationId);
            
            //it is better to return an ok result when creating a resource
            //returning NoContent is not the correct response here. Instead, use a 201 Created Response
            return NoContent(); 
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            //why haven't you used a try catch here when calling the userService? if there is an exception, the api will crash
            var userDto = await _userService.GetUserByIdAsync(id);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString(); //remove this to middleware
            if (userDto == null)
            {
                //why is there a space here?
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                //^^ a user not found should be logged as an error, not a warning
                return NotFound();
            }
            _logger.LogInformation("Fetched user {UserId} - Correlation ID: {CorrelationId}", id, correlationId); //there's no need for this extra log
            return Ok(userDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id,UserDto userDto)
        {
            //clean up your code, there are too many empty lines
            // there needs to be a try catch here
            var user = await _userService.GetUserByIdAsync(id);
            var correlationId = Request.Headers["X-Correlation-ID"].ToString(); //this can be removed

            if (user == null)
            {
                //this should be logged as an error not warning
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                return NotFound();
            }
            await _userService.UpdateAsync(id, userDto); //should be wrapped inside a try catch
            _logger.LogInformation("User {UserId} updated successfully - Correlation ID: {CorrelationId}", id, correlationId); // no need for this log
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id); //should be wrapped in a try catch
            var correlationId = Request.Headers["X-Correlation-ID"].ToString(); //remove this
            if (user == null)
            {
                //should be logged as an error not a warning 
                _logger.LogWarning("User not found - User ID: {UserId}, Correlation ID: {CorrelationId}", id, correlationId);
                return NotFound();
            }
            _logger.LogInformation("User {UserId} deleted successfully - Correlation ID: {CorrelationId}", id, correlationId); //remove this
            await _userService.DeleteAsync(id); //wrap this in a try catchg
            return NoContent();
        }

        
//why are there so many empty lines here? always clean up your code
    }
}
