using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Core.Events;
using Stocqres.Core.Exceptions;
using Stocqres.Domain.Commands.Wallet;
using Stocqres.Domain.Events;
using Stocqres.Infrastructure;

namespace Stocqres.Application.Wallet
{
    public class WalletCommandHandler : ICommandHandler<CreateWalletCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IEventBus _eventBus;

        public WalletCommandHandler(IUserRepository userRepository, 
            IWalletRepository walletRepository, IEventBus eventBus)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(CreateWalletCommand command)
        {
            var user = await _userRepository.GetUserAsync(command.UserId);

            if (user == null)
                throw new StocqresException(Codes.User.UserDoesNotExist, "User does not exist");

            if (await _walletRepository.IsExist(user.WalletId))
                throw new StocqresException(Codes.Wallet.WalletExist, $"Wallet for user {user.Username} exist");

            var wallet = new Domain.Wallet(user, command.Amount);
            await _walletRepository.CreateAsync(wallet);
            user.WalletId = wallet.Id;
            await _userRepository.UpdateAsync(user);
            await _eventBus.Publish(new WalletCreatedEvent(wallet.Id, wallet.UserId, wallet.Amount, wallet.Currency));
        }
    }
}
