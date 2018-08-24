using System;
using System.Security.Cryptography;

namespace HomeSensorServerAPI.PasswordCryptography
{
    public class PasswordCryptoSerivce
    {
        public int SaltLength { get; } = 16;
        public int HashLength { get; } = 30;
        public int RfcAlgorithmIterations { get; } = 2000;

        public string CreateHashString(string passwordToHash)
        {
            byte[] salt = CreateHashingSalt();

            var pbkdf2 = new Rfc2898DeriveBytes(passwordToHash, salt, RfcAlgorithmIterations);

            byte[] hashedPasswordBytes = pbkdf2.GetBytes(HashLength);

            byte[] hashedPasswordWSaltBytes = new byte[SaltLength + HashLength];
            Array.Copy(salt, 0, hashedPasswordWSaltBytes, 0, SaltLength);
            Array.Copy(hashedPasswordBytes, 0, hashedPasswordWSaltBytes, SaltLength, HashLength);

            return Convert.ToBase64String(hashedPasswordWSaltBytes);
        }

        private byte[] CreateHashingSalt()
        {
            byte[] salt = new byte[SaltLength];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        public bool IsPasswordMatching(string savedPasswordHash, string plainPasswordToCompare)
        {
            byte[] salt = new byte[SaltLength];
            byte[] savedPasswordHashBytes = Convert.FromBase64String(savedPasswordHash);
            Array.Copy(savedPasswordHashBytes, 0, salt, 0, SaltLength);

            byte[] hash = GetPlainPasswordToCompareHashBytes(plainPasswordToCompare, salt);

            bool isSame = hash.Compare(savedPasswordHashBytes, hash, SaltLength, 0, HashLength);

            return isSame;
        }

        private byte[] GetPlainPasswordToCompareHashBytes(string plainPasswordToCompare, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(plainPasswordToCompare, salt, RfcAlgorithmIterations);
            byte[] hash = pbkdf2.GetBytes(HashLength);
            return hash;
        }
    }
}
