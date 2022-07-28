using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class AdvertiserAddressChanged: EventBase
    {
        public AdvertiserAddressChanged(Guid id, Address address)
        {
            Id = id;
            Address = address;
        }

        public Guid Id { get; }
        public Address Address { get; }
      
    }
}