﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stocqres.Core.Commands;
using Stocqres.Customers.Api.Companies.Commands;
using Stocqres.Customers.Companies.Domain;
using Stocqres.Infrastructure.EventRepository;

namespace Stocqres.Customers.Companies.Application
{
    public class CompanyCommandHandler : 
        ICommandHandler<CreateCompanyCommand>, 
        ICommandHandler<ChargeCompanyCommand>,
        ICommandHandler<AddStocksToCompanyCommand>
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

        public async Task HandleAsync(ChargeCompanyCommand command)
        {
            var company = await _eventRepository.GetByIdAsync<Company>(command.CompanyId);
            company.ChargeCompanyStock(command.OrderId, command.Quantity);
            await _eventRepository.SaveAsync(company);
        }

        public async Task HandleAsync(AddStocksToCompanyCommand command)
        {
            var company = await _eventRepository.GetByIdAsync<Company>(command.CompanyId);
            company.AddStocksToCompany(command.OrderId, command.Quantity, command.StockCode);
            await _eventRepository.SaveAsync(company);
        }
    }
}
