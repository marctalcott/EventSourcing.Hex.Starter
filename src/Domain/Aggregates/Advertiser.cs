using System;
using System.Collections.Generic;
using Domain.ES.Events;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Aggregates
{
    public class Advertiser : AggregateBase
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public PhoneNumber PrimaryPhoneNumber { get; private set; }
        public PhoneNumber SecondaryPhoneNumber { get; private set; }

        public Advertiser(IEnumerable<IEvent> events)
        {
            InitFromEvents(events);
        }

        public Advertiser(Guid id, string name)
        {
            Apply(new AdvertiserAdded(id, name));
        }

        public void Rename(string name)
        {
            if (this.Name != name)
            {
                Apply(new AdvertiserRenamed(this.Id, name));
            }
        }

        public void ChangeEmailAddress(EmailAddress emailAddress)
        {
            
            if (this.EmailAddress == null || !this.EmailAddress.Equals(emailAddress))
            {
                Apply(new AdvertiserEmailAddressChanged(this.Id, emailAddress));
            }
        }
        
        public void ChangeAddress(Address address)
        {
            if (this.Address == null || !this.Address.Equals(address))
            {
                Apply(new AdvertiserAddressChanged(this.Id, address));
            }
        }

        public void ChangePrimaryPhone(PhoneNumber phoneNumber)
        {
            if (this.PrimaryPhoneNumber == null || !this.PrimaryPhoneNumber.Equals(phoneNumber))
            {
                Apply(new AdvertiserPrimaryPhoneChanged(this.Id, phoneNumber));
            }
        }
        public void ChangeSecondaryPhone(PhoneNumber phoneNumber)
        {
            if (this.SecondaryPhoneNumber == null || !this.SecondaryPhoneNumber.Equals(phoneNumber))
            {
                Apply(new AdvertiserSecondaryPhoneChanged(this.Id, phoneNumber));
            }
        }
        
        protected override void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }
        
        protected void When(AdvertiserAdded command)
        {
            Id = command.Id;
            Name = command.Name;
        }

        protected void When(AdvertiserRenamed command)
        {
            this.Name = command.Name;
        }

        protected void When(AdvertiserAddressChanged command)
        {
            this.Address = command.Address;
        }

        protected void When(AdvertiserPrimaryPhoneChanged command)
        {
            this.PrimaryPhoneNumber = command.PhoneNumber;
        }
        protected void When(AdvertiserSecondaryPhoneChanged command)
        {
            this.PrimaryPhoneNumber = command.PhoneNumber;
        }
        protected void When(AdvertiserEmailAddressChanged command)
        {
            this.EmailAddress = command.EmailAddress;
        }
    }
}