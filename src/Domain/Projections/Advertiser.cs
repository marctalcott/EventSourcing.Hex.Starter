using System;
using Domain.ES.Events;
using Domain.Events;
using Domain.ValueObjects;
using Domain.ES.Projections;

namespace Domain.Projections
{
    public class AdvertiserView
    {
        public Guid Id { get; set; }

        public string Name { get;  set; }
        public Address Address { get;  set; }
        
        public EmailAddress EmailAddress { get;  set; }
        public PhoneNumber PrimaryPhoneNumber { get;  set; }
        public PhoneNumber SecondaryPhoneNumber { get;  set; }
    }

    public class AdvertiserProjector : Projector<AdvertiserView>, IProjector
    {
        public AdvertiserProjector()
        {
            RegisterHandler<AdvertiserAdded>(WhenAdvertiserAdded);
            RegisterHandler<AdvertiserRenamed>(WhenAdvertiserRenamed);
            RegisterHandler<AdvertiserAddressChanged>(WhenAdvertiserAddressChanged);
            RegisterHandler<AdvertiserPrimaryPhoneChanged>(WhenAdvertiserPrimaryPhoneChanged);
            RegisterHandler<AdvertiserSecondaryPhoneChanged>(WhenAdvertiserSecondaryPhoneChanged);
        }

        public override string GetViewName(string streamId, IEvent @event)
        {
            return $"{nameof(AdvertiserView)}:{streamId}";
        }

        private void WhenAdvertiserAdded(AdvertiserAdded e, AdvertiserView view)
        {
            view.Id = e.Id;
            view.Name = e.Name;
        }

        private void WhenAdvertiserRenamed(AdvertiserRenamed e, AdvertiserView view)
        {
            view.Name = e.Name;
        }

        private void WhenAdvertiserAddressChanged(AdvertiserAddressChanged e, AdvertiserView view)
        {
            view.Address = e.Address;
        }

        private void WhenAdvertiserPrimaryPhoneChanged(AdvertiserPrimaryPhoneChanged e, AdvertiserView view)
        {
            view.PrimaryPhoneNumber = e.PhoneNumber;
        }

        private void WhenAdvertiserSecondaryPhoneChanged(AdvertiserSecondaryPhoneChanged e, AdvertiserView view)
        {
            view.SecondaryPhoneNumber = e.PhoneNumber;
        }
    }
}