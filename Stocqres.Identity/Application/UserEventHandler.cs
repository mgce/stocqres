using System.Threading.Tasks;
using Stocqres.Core.Events;
using Stocqres.Domain.Events.Users;
using Stocqres.Domain.Views;
using Stocqres.Infrastructure.ProjectionWriter;

namespace Stocqres.Identity.Application
{
    public class UserEventHandler : IEventHandler<UserCreatedEvent>, IEventHandler<UserPasswordSettedEvent>, IEventHandler<WalletCreatedEvent>
    {
        private readonly IProjectionWriter _projectionWriter;

        public UserEventHandler(IProjectionWriter projectionWriter)
        {
            _projectionWriter = projectionWriter;
        }

        public async Task HandleAsync(UserCreatedEvent @event)
        {
            var userView = new UserView()
            {
                Id = @event.AggregateId,
                Email = @event.Email,
                Username = @event.Username,
                Role = @event.Role
            };
            await _projectionWriter.AddAsync(userView);
        }

        public async Task HandleAsync(UserPasswordSettedEvent @event)
        {
            await _projectionWriter.UpdateAsync<UserView>(@event.AggregateId, u => { u.Password = @event.Password; });
        }

        public async Task HandleAsync(WalletCreatedEvent @event)
        {
            await _projectionWriter.UpdateAsync<UserView>(@event.AggregateId, 
                u => {
                u.WalletAmount = @event.Wallet.Amount;
                u.Currency = @event.Wallet.Currency;
            });
        }
    }
}
