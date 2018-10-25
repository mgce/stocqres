using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stocqres.Core.Events;

namespace Stocqres.Infrastructure.EventRepository
{
    public static class Extensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };


        public static EventData ToEventData(this IEvent @event, Guid aggregateId, string aggregateType, int verion, DateTime createdAt)
        {
            var data = JsonConvert.SerializeObject(@event, SerializerSettings);
            var eventHeaders = new Dictionary<string, string>
            {
                {
                    "EventType", @event.GetType().AssemblyQualifiedName
                }
            };
            var metadata = JsonConvert.SerializeObject(eventHeaders, SerializerSettings);
            return new EventData(Guid.NewGuid(), aggregateId, aggregateType, verion, data, metadata, createdAt);
        }

        public static IEvent DeserializeEvent(this EventData x)
        {
            var eventTypeName = JObject.Parse(x.Metadata).Property("EventType").Value;
            return (IEvent)JsonConvert.DeserializeObject(x.Data, Type.GetType((string)eventTypeName));
        }
    }
}
