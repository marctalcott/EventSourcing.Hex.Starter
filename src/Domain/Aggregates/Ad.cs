using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.ES.Events;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Aggregates
{
    public class Ad : AggregateBase
    {
        public string Name { get; private set; }
        public AdStatus AdStatus { get; private set; }
        public bool Active { get; private set; }

        public string Headline { get; }
        public string Copy { get; }
        public string CallToAction { get; }
        public ContactInformation ContactInformation { get; }
        public string[] Images { get; }

        public Ad(IEnumerable<IEvent> events)
        {
            InitFromEvents(events);
        }

        public Ad(Guid id, string name, AdStatus adStatus, bool active)
        {
            Apply(new AdAdded(id, name, adStatus, active));
        }

        public void Rename(string name)
        {
            if (this.Name != name)
            {
                Apply(new AdRenamed(this.Id, name));
            }
        }

        public void Activate()
        {
            if (!Active)
            {
                Apply(new AdActivated(this.Id));
            }
        }

        public void Deactivate()
        {
            if (Active)
            {
                Apply(new AdDeactivated(this.Id));
            }
        }

        public void AdRenamed(string name)
        {
            if (this.Name != name)
            {
                Apply(new AdRenamed(this.Id, name));
            }
        }
        protected override void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }
        
        protected void When(AdAdded command)
        {
            Id = command.Id;
            Name = command.Name;
            AdStatus = command.AdStatus;
            Active = command.Active;
        }

        protected void When(AdRenamed command)
        {
            this.Name = command.Name;
        }

        protected void When(AdActivated command)
        {
            this.Active = true;
        }

        public void When(AdDeactivated command)
        {
            this.Active = false;
        }
    }
}