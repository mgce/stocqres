using Stocqres.Core;

namespace Stocqres.Domain
{
    public class User : AggregateRoot
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Seed { get; set; }
        public Role Role { get; set; }

        protected User()
        {}

        public User(string email, Role role)
        {
            Email = email.ToLowerInvariant();
            Role = role;
        }

        public void SetPassword(string password)
        {

        }
    }
}
