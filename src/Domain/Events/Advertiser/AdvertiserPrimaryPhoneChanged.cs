using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class AdvertiserPrimaryPhoneChanged:EventBase
    {
        public AdvertiserPrimaryPhoneChanged(Guid id, PhoneNumber phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }

        public Guid Id { get; }
        public PhoneNumber PhoneNumber { get; }
      
    }
}