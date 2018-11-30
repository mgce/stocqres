﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Domain;

namespace Stocqres.Infrastructure.EventRepository
{
    public interface IEventRepository
    {
        Task<T> GetByIdAsync<T>(Guid id);
        Task SaveAsync(IAggregateRoot aggregate);
        Task TakeSnapshotAsync<T>(Guid aggregateId) where T : IAggregateRoot;
    }
}
