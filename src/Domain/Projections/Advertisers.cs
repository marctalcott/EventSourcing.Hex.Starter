using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ES.Events;
using Domain.ES.Projections;
using Domain.Events; 

namespace Domain.Projections
{

    public class AdvertiserItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class AdvertisersView
    {
        public List<AdvertiserItem> Advertisers { get; }

        public AdvertisersView()
        {
            Advertisers = new List<AdvertiserItem>();
        }
    }

    public class AdvertisersProjector : Projector<AdvertisersView>
    {
        public AdvertisersProjector()
        {
            RegisterHandler<AdvertiserAdded>(WhenAdvertiserAdded);
        }

        public override string GetViewName(string streamId, IEvent @event)
        {
            return $"{nameof(AdvertisersView)}";
        }

        private void WhenAdvertiserAdded(AdvertiserAdded e, AdvertisersView view)
        {
            if (view.Advertisers.Any(x => x.Id == e.Id))
                return;

            var advertiserInfo = new AdvertiserItem()
            {
                Id = e.Id,
                Name = e.Name
            };
            view.Advertisers.Add(advertiserInfo);
        }
    }
}
