using DevOne.Security.Cryptography.BCrypt;

namespace Server.Entity
{
    public class User
    {
        public  Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public string LastName { get; set; }
        public required string Password { get; set; }

        public string MakePasswordHash(string password)
        {
            var slat = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(password, slat);
        }

        public bool VerifyPassword(string password)
        {
            return BCryptHelper.CheckPassword(password, Password);
        }
    }
}