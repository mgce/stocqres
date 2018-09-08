using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Exceptions;

namespace Stocqres.Domain.Exceptions.User
{
    public class UserDoesNotExistException : StocqresException
    {
        private static string code = Codes.User.UserDoesNotExist;
        private static string message = "User does not exist";

        public UserDoesNotExistException() : base(code, message)
        {
        }
    }
}
