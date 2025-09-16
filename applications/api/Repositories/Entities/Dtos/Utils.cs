using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Repositories
{
    public class Utils
    {
        public static class HashString
        {
            public static string GetSha256FromString(string strData)
            {
                var message = Encoding.ASCII.GetBytes(strData);
                var hashValue = SHA256.HashData(message);
                return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
            }
        }
    }
}
