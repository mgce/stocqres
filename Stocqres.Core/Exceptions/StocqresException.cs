using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Exceptions
{
    public class StocqresException : Exception
    {
        public string Code { get; set; }
        public int? Status { get; set; }

        public StocqresException(string message) : base(message)
        {
            
        }

        public StocqresException(string message, int status) : base(message)
        {
            Status = status;
        }

        public StocqresException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public StocqresException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public StocqresException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public StocqresException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
