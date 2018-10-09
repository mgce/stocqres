using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Commands;
using Stocqres.Core.Domain;
using Stocqres.Core.Exceptions;
using Stocqres.Domain;
using Stocqres.Domain.Commands.Wallet;
using Stocqres.Identity.Domain.Commands;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Identity.Application
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>
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
            var user = new Domain.User(command.Username, command.Email);
            user.SetPassword(command.Password, _passwordHasher);
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
