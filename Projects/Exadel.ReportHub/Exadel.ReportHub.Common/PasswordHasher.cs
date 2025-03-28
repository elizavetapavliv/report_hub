using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Common;

public class PasswordHasher
{
    private const int saltSize = 16;
    private const int hashSize = 64;
    private const int iterations = 100000;
    private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        byte[] saltData = RandomNumberGenerator.GetBytes(saltSize);
        passwordSalt = Convert.ToBase64String(saltData);
        byte[] hashedData = Rfc2898DeriveBytes.Pbkdf2(password,saltData,iterations,hashAlgorithm, hashSize);
        passwordHash = Convert.ToBase64String(hashedData);
    }

    public static string GetPasswordHash(string password, string passwordSalt)
    {
        byte[] saltData = Convert.FromBase64String(passwordSalt);
        byte[] hashedData = Rfc2898DeriveBytes.Pbkdf2(password, saltData, iterations, hashAlgorithm, hashSize);

        return Convert.ToBase64String(hashedData);
    }
}
