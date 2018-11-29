using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;

namespace Stocqres.Core.EventSourcing
{
    public static class Extensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

        public static Snapshot ToSnapshot(this IAggregateRoot aggregate)
        {
            var data = JsonConvert.SerializeObject(aggregate, SerializerSettings);
            var metadata = JsonConvert.SerializeObject(aggregate.GetType().AssemblyQualifiedName, SerializerSettings);

            return new Snapshot(Guid.NewGuid(), aggregate.Id, aggregate.GetType().Name, aggregate.Version, data, metadata, DateTime.Now);
        }

        public static IAggregateRoot DeserializeSnapshot<T>(this Snapshot x)
        {
            var aggregateTypeName = JObject.Parse(x.Metadata).Property("Snapshot").Value;
            return (IAggregateRoot)JsonConvert.DeserializeObject(x.Data, Type.GetType((string)aggregateTypeName));
        }
    }
}
