using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates;
using Domain.ES.EventStore;
using Infrastructure.ES.EventStore;
using MediatR;

namespace Application.Advertisers.Commands
{
    public class AddAdvertiser : IRequest
    {
        public EventUserInfo EventUserInfo { get; }
        public Guid Id { get; }
        public string Name { get; }


        public AddAdvertiser(EventUserInfo eventUserInfo, Guid id, string name)
        {
            EventUserInfo = eventUserInfo;
            Id = id;
            Name = name;
        }
    }

    public class AddAdvertiserHandler : IRequestHandler<AddAdvertiser>
    {
        private IRepository _repo;

        public AddAdvertiserHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(
            AddAdvertiser command,
            CancellationToken cancellationToken)
        {
            if (command.EventUserInfo == null)
                throw new ApplicationException("User must be defined.");

            var ad = new Advertiser(command.Id, command.Name);
            bool saved = await _repo.Save(command.EventUserInfo, ad);
            if (!saved)
                throw new ApplicationException("Advertiser not added. Was the Id a unique value?");
            return Unit.Value;
        }
    }
}