﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stocqres.Core.Events;

namespace Stocqres.Core.Domain
{
    public abstract class AggregateRoot : BaseEntity, IAggregateRoot
    {
        private List<IEvent> _uncommitedEvents = new List<IEvent>();
        private int _version { get; set; }

        public int Version => _version;

        protected AggregateRoot()
        {}

        public AggregateRoot(IEnumerable<IEvent> events)
        {
            ApplyEvents(events);
        }

        public List<IEvent> GetUncommitedEvents() => _uncommitedEvents;

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event);
            }
        }

        private void Apply(IEvent e)
        {
            _version++;

            var type = e.GetType();         

            GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(ev => ev.Name == "Apply" && ev.GetParameters().Length == 1)
                .ToDictionary(ev => ev.GetParameters().First().ParameterType, ev => ev)
                .TryGetValue(type, out var info);

            try
            {
                info.Invoke(this, new[] { e });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected void Publish(IEvent e)
        {
            _uncommitedEvents.Add(e);
            Apply(e);
        }
    }
}
