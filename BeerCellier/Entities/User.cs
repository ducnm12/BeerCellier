using PwdHasher;
using System.ComponentModel.DataAnnotations;

namespace BeerCellier.Entities
{
    public class User
    {
        private static readonly PasswordHasher hasher = new PasswordHasher();

        public int ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(100)]
        public string Hash { get; set; }

        public User() { }
        
        public User(string username, string password)
        {
            Username = username;

            var hashedPassword = hasher.HashIt(password);

            Salt = hashedPassword.Salt;
            Hash = hashedPassword.Hash;
        }

        public bool IsPasswordValid(string password)
        {
            return hasher.Check(password, new HashedPassword(Hash, Salt));
        }        
    }
}