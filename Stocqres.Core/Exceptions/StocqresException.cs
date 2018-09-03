using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Exceptions
{
    public class StocqresException : Exception
    {
        public string Code { get; set; }

        public StocqresException(string message) : base(message)
        {
            
        }
    }
}
