using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Core.EventStore;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Commands.User;
using Stocqres.Domain.Events.Users;
using Stocqres.Infrastructure;

namespace Stocqres.Application.User.Handlers
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IPasswordHasher<Domain.User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IEventBus _eventBus;

        public UserCommandHandler(IPasswordHasher<Domain.User> passwordHasher, IUserRepository userRepository, IEventBus eventBus)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _eventBus = eventBus;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.User(request.Username, request.Email, Domain.Enums.Role.Customer);
            user.SetPassword(request.Password, _passwordHasher);
            var existUser =
                await _userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (existUser != null)
                throw new StocqresException("User with this username or email is currently exist");

            await _userRepository.CreateAsync(user);
            await _eventBus.Publish(new UserCreatedEvent
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            });
            //Save to mongoDb
            //Raise an event to event store
            return Unit.Value;
        }
    }
}
