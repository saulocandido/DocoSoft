using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result<UserDto>>
    {
        public UpdateUserCommand(Guid id, string firstName, string lastName, string email)
        {
            Id=id;
            FirstName=firstName;
            LastName=lastName;
            Email=email;
        }

        public Guid Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }

}
