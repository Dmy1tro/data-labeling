namespace DataLabeling.Entities
{
    public abstract class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string PinConfirmation { get; set; }

        public bool EmailConfirmed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Roles { get; set; }

        public string PasswordHash { get; set; }
    }
}
