using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Commands;

namespace Stocqres.Transactions.Orders.Domain
{
    public class ProcessManager
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        private List<ICommand> _unhandledCommands;

        public ProcessManager()
        {
            _unhandledCommands = new List<ICommand>();
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }

        public void ProcessCommand(ICommand command)
        {
            _unhandledCommands.Add(command);
        }

        public void TakeOffCommand(ICommand command)
        {
            _unhandledCommands.RemoveAll(c => _unhandledCommands.Contains(c));
        }

        public void UpdateModifiedDate()
        {
            ModifiedAt = DateTime.Now;
        }

        public List<ICommand> GetUnhandledCommands()
        {
            return _unhandledCommands;
        }
    }
}
