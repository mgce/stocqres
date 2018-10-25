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

        public EventData()
        {
            
        }

        public EventData(Guid Id, Guid AggregateId, string AggregateType, int Version, string Data, string Metadata, DateTime Created)
        {
            this.Id = Id;
            this.Created = Created;
            this.AggregateType = AggregateType;
            this.AggregateId = AggregateId;
            this.Version = Version;
            this.Data = Data;
            this.Metadata = Metadata;
        }
    }
}
