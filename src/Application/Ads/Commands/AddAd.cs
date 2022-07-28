using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.Enums;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Ads.Commands
{
    public class AddAd : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public string Name { get; }
        public AdStatus AdStatus { get; }
        public bool Active { get; }

        public AddAd(EventUserInfo eventUserInfo, Guid id, string name, AdStatus adStatus, bool active)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Name = name;
            AdStatus = adStatus;
            Active = active;
        }
    }

    public class AddAdHandler : IRequestHandler<AddAd>
    {
        private IRepository _repo;

        public AddAdHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            AddAd command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var ad = new Ad(command.Id, command.Name, command.AdStatus, command.Active);
            bool saved = await _repo.Save(command.EventUserInfo, ad);
            
            string aggName = "Ad";
            if (!saved)
                throw new ApplicationException($"{aggName} not added. Was the Id a unique value?");
            return Unit.Value;
        }
    }
}