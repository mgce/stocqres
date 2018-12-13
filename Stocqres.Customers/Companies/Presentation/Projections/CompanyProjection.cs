using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;

namespace Stocqres.Customers.Companies.Presentation.Projections
{
    public class CompanyProjection : IProjection
    {
        public Guid Id { get; set; }
        public Guid CompanyId => Id;
        public string Name { get; set; }
        public string StockCode { get; set; }
        public int StockUnit { get; set; }
        public int StockQuantity { get; set; }

        public CompanyProjection()
        {}

        public CompanyProjection(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
