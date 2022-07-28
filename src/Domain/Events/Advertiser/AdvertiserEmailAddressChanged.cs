using System;
using System.ComponentModel.DataAnnotations;
using Domain.ES.Events;
using Domain.ValueObjects;

namespace Domain.Events
{
    public class AdvertiserEmailAddressChanged:EventBase
    {
        public AdvertiserEmailAddressChanged(Guid id, EmailAddress emailAddress)
        {
            Id = id;
            EmailAddress = emailAddress;
        }

        public Guid Id { get; }
        public EmailAddress EmailAddress { get; }
      
    }
}