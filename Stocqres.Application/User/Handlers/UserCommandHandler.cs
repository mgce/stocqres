using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Commands;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Domain;
using Stocqres.Domain.Commands.User;
using Stocqres.Domain.Commands.Wallet;
using Stocqres.Domain.Events.Wallet;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Application.User.Handlers
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>, ICommandHandler<CreateWalletCommand>
    {
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IEventRepository _eventRepository;

        public UserCommandHandler(IPasswordHasher<Domain.User> passwordHasher, IEventRepository eventRepository)
        {
            _passwordHasher = passwordHasher;
            _eventRepository = eventRepository; 
        }

        public async Task HandleAsync(CreateUserCommand command)
        {
            var user = new Domain.User(command.Username, command.Email, Domain.Enums.Role.Customer);
            user.SetPassword(command.Password, _passwordHasher);
            await _eventRepository.SaveAsync(user);
        }

        public async Task HandleAsync(CreateWalletCommand command)
        {
            var user = await _eventRepository.GetByIdAsync<Domain.User>(command.UserId);

            if (user == null)
                throw new StocqresException(Codes.UserCodes.UserDoesNotExist, "UserCodes does not exist");

            if (user.Wallet != null)
                throw new StocqresException(Codes.WalletCodes.WalletExist, $"WalletCodes for user {user.Username} currently exist");

            user.AssignWallet(new Domain.Wallet(command.Amount));

            await _eventRepository.SaveAsync(user);
        }

        private async Task Act<T>(Guid id, Action<T> action)
        {
            var aggregate = await _eventRepository.GetByIdAsync<T>(id);
            action(aggregate);
            await _eventRepository.SaveAsync(aggregate as IAggregateRoot);
        }
    }
}
