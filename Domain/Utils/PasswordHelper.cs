using System;
using System.Security.Cryptography;

namespace api_ecommerce.Domain.Utils
{
    public static class PasswordHelper
    {
        public static (string hash, string salt) HashPassword(string password, int iterations = 100000)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Senha inválida.");
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        public static bool Verify(string password, string hashBase64, string saltBase64, int iterations = 100000)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashBase64) || string.IsNullOrWhiteSpace(saltBase64))
                return false;
            byte[] salt = Convert.FromBase64String(saltBase64);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hash) == hashBase64;
        }

        internal static bool VerifyPassword(string senha, string senhaHash, string senhaSalt)
        {
            return Verify(senha, senhaHash, senhaSalt);
        }
    }
}