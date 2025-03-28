using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Common;
public class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 64;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        passwordSalt = RandomNumberGenerator.GetBytes(SaltSize);
        passwordHash = new Rfc2898DeriveBytes(password, passwordSalt, Iterations, HashAlgorithm).GetBytes(HashSize);
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        byte[] inputHash = new Rfc2898DeriveBytes(password, passwordSalt, Iterations, HashAlgorithm).GetBytes(HashSize);
        
        if(CryptographicOperations.FixedTimeEquals(inputHash, passwordHash))
        {
            return true;
        }
        throw new UnauthorizedAccessException("Password is incorrect");
    }
}
