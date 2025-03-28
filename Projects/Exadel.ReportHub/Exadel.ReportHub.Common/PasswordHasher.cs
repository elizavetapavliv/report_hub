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

    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        passwordSalt = RandomNumberGenerator.GetBytes(saltSize);
        passwordHash = Rfc2898DeriveBytes.Pbkdf2(password,passwordSalt,iterations,hashAlgorithm, hashSize);
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, passwordSalt, iterations, hashAlgorithm, hashSize);
        
        if(CryptographicOperations.FixedTimeEquals(inputHash, passwordHash))
        {
            return true;
        }
        throw new UnauthorizedAccessException("Password is incorrect");
    }
}
