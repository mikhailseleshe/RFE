using System.Text;

namespace RFE
{
    public class EncodingHelper
    {
        public static string Base64Decode(string input)
        {
            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Escape(string input)
        {
            return input = input.Replace("\"", string.Empty).Replace("\\", string.Empty);
        }
    }
}
