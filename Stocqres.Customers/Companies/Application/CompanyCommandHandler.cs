using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Customers.Companies.Domain;
using Stocqres.Customers.Companies.Domain.Commands;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Customers.Companies.Application
{
    public class CompanyCommandHandler : ICommandHandler<CreateCompanyCommand>
    {
        private readonly IEventRepository _eventRepository;

        public CompanyCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task HandleAsync(CreateCompanyCommand command)
        {
            var company = new Company(command.Name);
            company.CreateCompanyStock(command.Code, command.Unit, command.Quantity);
            await _eventRepository.SaveAsync(company);
        }
    }
}
