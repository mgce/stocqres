using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Infrastructure.EventRepository
{
    public class EventData
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string Data { get; set; }
        public string Metadata { get; set; }

        public EventData(Guid aggregateId, string aggregateType, int version, string data, string metadata)
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            AggregateType = aggregateType;
            AggregateId = aggregateId;
            Version = version;
            Data = data;
            Metadata = metadata;
        }
    }
}
