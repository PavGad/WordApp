using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Authentication;
using System.Security.Cryptography;
using WordApp.Domain.Interfaces;

namespace WordApp.Domain.Services
{
    public class PasswordService : IPasswordService
    {
        public bool CheckPassword(string password, string hash)
        {
            var saltBase64 = hash.Split('-')[1];
            var salt = Convert.FromBase64String(saltBase64);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return $"{hashed}-{saltBase64}" == hash;
        }

        public string HashPassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new AuthenticationException("Password is required");
            }

            byte[] salt = new Byte[128 / 8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return $"{hashed}-{Convert.ToBase64String(salt)}";
        }
    }
}
