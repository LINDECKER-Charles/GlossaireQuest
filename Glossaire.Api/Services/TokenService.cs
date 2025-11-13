using System.Security.Cryptography;

namespace TechQuiz.Api.Services
{
    public class TokenService
    {
        public static string GenerateHexToken(int charLength)
        {
            if (charLength % 2 != 0)
                throw new ArgumentException("La longueur doit être paire (2 chars = 1 byte).");

            int bytesLength = charLength / 2;
            byte[] buffer = new byte[bytesLength];
            RandomNumberGenerator.Fill(buffer);
            return Convert.ToHexString(buffer); // Sortie EXACTE = charLength
        }
    }
}
