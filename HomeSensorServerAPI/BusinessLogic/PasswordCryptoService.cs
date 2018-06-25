using HomeSensorServerAPI.Extensions;
using System;
using System.Security.Cryptography;

namespace HomeSensorServerAPI.BusinessLogic
{
    public class PasswordCryptoSerivce
    {
        private const int saltLength = 16;
        private const int hashLength = 30;
        private const int RfcAlgorithmIterations = 2000;

        public string CreateHashString(string passwordToHash)
        {
            byte[] salt = CreateHashingSalt();

            var pbkdf2 = new Rfc2898DeriveBytes(passwordToHash, salt, RfcAlgorithmIterations);

            byte[] hashedPasswordBytes = pbkdf2.GetBytes(hashLength);

            byte[] hashedPasswordWSaltBytes = new byte[saltLength + hashLength];
            Array.Copy(salt, 0, hashedPasswordWSaltBytes, 0, saltLength);
            Array.Copy(hashedPasswordBytes, 0, hashedPasswordWSaltBytes, saltLength, hashLength);

            return Convert.ToBase64String(hashedPasswordWSaltBytes);
        }

        private static byte[] CreateHashingSalt()
        {
            byte[] salt = new byte[saltLength];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        public bool IsPasswordMatching(string savedPasswordHash, string plainPasswordToCompare)
        {
            byte[] salt = new byte[saltLength];
            byte[] savedPasswordHashBytes = Convert.FromBase64String(savedPasswordHash);
            Array.Copy(savedPasswordHashBytes, 0, salt, 0, saltLength);

            byte[] hash = GetPlainPasswordToCompareHashBytes(plainPasswordToCompare, salt);

            bool isSame = hash.Compare(savedPasswordHashBytes, hash, saltLength, 0, hashLength);

            return isSame;
        }

        private static byte[] GetPlainPasswordToCompareHashBytes(string plainPasswordToCompare, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(plainPasswordToCompare, salt, RfcAlgorithmIterations);
            byte[] hash = pbkdf2.GetBytes(hashLength);
            return hash;
        }
    }
}
