using System;
using Domain.ES.Events;
using Domain.ES.Projections;
using Domain.Events;

namespace Domain.Projections
{
    public class CampaignView
    {
        public Guid Id { get; set; }

        public Guid AdvertiserId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CampaignProjector : Projector<CampaignView>, IProjector
    {
        public CampaignProjector()
        {
            RegisterHandler<CampaignAdded>(WhenCampaignAdded);
            RegisterHandler<CampaignRenamed>(WhenCampaignRenamed);
            RegisterHandler<CampaignDateRangeChanged>(WhenCampaignDateRangeChanged);
        }

        public override string GetViewName(string streamId, IEvent @event)
        {
            return $"{nameof(CampaignView)}:{streamId}";
        }

        private void WhenCampaignAdded(CampaignAdded e, CampaignView view)
        {
            view.Id = e.Id;
            view.AdvertiserId = e.AdvertiserId;
            view.Name = e.Name;
            view.StartDate = e.StartDate;
            view.EndDate = e.EndDate;
        }

        private void WhenCampaignRenamed(CampaignRenamed e, CampaignView view)
        {
            view.Name = e.Name;
        }

        private void WhenCampaignDateRangeChanged(CampaignDateRangeChanged e, CampaignView view)
        {
            view.StartDate = e.StartDate;
            view.EndDate = e.EndDate;
        }
    }
}