using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Infrastructure.UnitOfWork;

namespace Stocqres.Infrastructure.Commands
{
    public class TransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _decoratedCommandHandler;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommandHandler, IUnitOfWork unitOfWork)
        {
            _decoratedCommandHandler = decoratedCommandHandler;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(TCommand command)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _decoratedCommandHandler.HandleAsync(command);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
