using DOCOsoft.UserManagement.Application.Users.Commands.CreateUser;
using DOCOsoft.UserManagement.Application.Users.Commands.DeleteUser;
using DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Application.Users.Queries.GetAllRoles;
using DOCOsoft.UserManagement.Application.Users.Queries.GetAllUser;
using DOCOsoft.UserManagement.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DOCOsoft.UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="command">User data</param>
        /// <returns>The created user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { result.Message, result.Errors });

            return CreatedAtAction(nameof(GetUserById), new { id = result.Data?.Id }, result.Data);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="command">Updated user data</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Message = "Mismatched UserId in URL and payload" });

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return NotFound(new { result.Message, result.Errors });

            return NoContent();
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            if (!result.IsSuccess)
                return NotFound(new { result.Message, result.Errors });

            return NoContent();
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>The user details</returns>
        [HttpGet("{id:guid}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Result<UserDto>>> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            if (!result.IsSuccess)
                return NotFound(new { result.Message, result.Errors });

            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserListDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            if (!result.IsSuccess)
                return BadRequest(new { result.Message, result.Errors });

            return Ok(result.Data);
        }

        /// <summary>
        /// Retrieves all available roles.
        /// </summary>
        /// <returns>List of roles</returns>
        [HttpGet("roles")]
        [ProducesResponseType(typeof(RoleListDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _mediator.Send(new GetAllRolesQuery());
            if (!result.IsSuccess)
                return NotFound(new { result.Message, result.Errors });

            return Ok(result.Data);
        }
    }
}
