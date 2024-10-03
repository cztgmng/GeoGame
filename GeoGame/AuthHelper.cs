using System.Text;
using System.Security.Cryptography;

namespace GeoGame
{
    public class AuthHelper()
    {
        public static string CalculateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // byte to hex
                }
                return builder.ToString();
            }
        }
        private static readonly Dictionary<string, DateTime> tokens = new Dictionary<string, DateTime>();
        private static readonly TimeSpan expirationDuration = TimeSpan.FromHours(12);

        public static void AddToken(string token)
        {
            CleanupExpiredTokens();

            tokens[token] = DateTime.Now.Add(expirationDuration);
        }
        public static bool TokenExist(string token)
        {
            CleanupExpiredTokens();

            return tokens.ContainsKey(token);
        }
        private static void CleanupExpiredTokens()
        {
            lock (tokens)
            {
                var expiredTokens = new List<string>();
                foreach (var pair in tokens)
                {
                    if (pair.Value < DateTime.Now)
                        expiredTokens.Add(pair.Key);
                }

                foreach (var token in expiredTokens)
                {
                    tokens.Remove(token);
                }
            }
        }
    }
}