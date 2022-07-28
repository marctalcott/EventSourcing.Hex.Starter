using System;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class AdvertiserSecondaryPhoneChanged:EventBase
    {
        public AdvertiserSecondaryPhoneChanged(Guid id, PhoneNumber phoneNumber)
        {
            Id = id;
            PhoneNumber = phoneNumber;
        }

        public Guid Id { get; }
        public PhoneNumber PhoneNumber { get; }
      
    }
}