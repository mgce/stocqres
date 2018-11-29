using System;
using System.Collections.Generic;
using System.Text;

namespace Stocqres.Core.EventSourcing
{
    public class Snapshot
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public int SnapshottedVersion { get; set; }
        public string Data { get; set; }
        public string Metadata { get; set; }

        public Snapshot()
        {

        }

        public Snapshot(Guid id, Guid aggregateId, string aggregateType, int snapshottedVersion, string data, string metadata, DateTime created)
        {
            Id = id;
            Created = created;
            AggregateType = aggregateType;
            AggregateId = aggregateId;
            SnapshottedVersion = snapshottedVersion;
            Data = data;
            Metadata = metadata;
        }
    }
}
