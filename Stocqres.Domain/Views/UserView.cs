using System;
using System.Collections.Generic;
using System.Text;
using Stocqres.Core.Domain;
using Stocqres.Domain.Enums;

namespace Stocqres.Domain.Views
{
    public class UserView : IProjection
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public decimal WalletAmount { get; set; }
        public Currency Currency { get; set; }
    }
}
