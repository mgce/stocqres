using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Commands;
using Stocqres.Core.Dispatcher;
using Stocqres.Core.Domain;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Domain;
using Stocqres.Domain.Commands.Wallet;
using Stocqres.Identity.Domain.Commands;
using Stocqres.Identity.Repositories;
using Stocqres.Infrastructure.EventRepository;
using Stocqres.SharedKernel.Events;

namespace Stocqres.Identity.Application
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>,
        ICommandHandler<CreateInvestorCommand>
    {
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDispatcher _dispatcher;
        private readonly IEventBus _eventBus;

        public UserCommandHandler(IPasswordHasher<Domain.User> passwordHasher, 
            IEventRepository eventRepository, 
            IUserRepository userRepository, IDispatcher dispatcher, IEventBus eventBus)
        {
            _passwordHasher = passwordHasher;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _dispatcher = dispatcher;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(CreateUserCommand command)
        {
            var user = await _userRepository.GetUserByEmailAsync(command.Email);
            if (user != null)
                throw new StocqresException($"User with mail {command.Email} currently exist");
            user = new Domain.User(command.Username, command.Email);
            user.SetPassword(command.Password, _passwordHasher);
            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
        }

        private async Task Act<T>(Guid id, Action<T> action)
        {
            var aggregate = await _eventRepository.GetByIdAsync<T>(id);
            action(aggregate);
            await _eventRepository.SaveAsync(aggregate as IAggregateRoot);
        }

        public async Task HandleAsync(CreateInvestorCommand command)
        {
            var createUserCommand = new CreateUserCommand
            {
                Email = command.Email,
                Password = command.Password,
                Username = command.Username
            };

            await _dispatcher.SendAsync(createUserCommand);

            var user = await _userRepository.GetUserByEmailAsync(command.Email);

            await _eventBus.Publish(new UserForInvestorCreated(user.Id, command.FirstName, command.LastName));
        }
    }
}
