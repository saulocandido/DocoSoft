﻿

namespace DOCOsoft.UserManagement.Domain.Common
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
