using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.Domain
{
    public interface IProjection
    {
        Guid Id { get; set; }
    }
}
