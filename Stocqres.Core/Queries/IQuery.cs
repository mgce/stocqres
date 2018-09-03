using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Stocqres.Core.Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
