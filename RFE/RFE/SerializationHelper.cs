using System.Text.Json;

namespace RFE
{
    public static class SerializationHelper
    {
        public static string GetInputValue(string input)
        {
            return JsonSerializer.Deserialize<InputWrapper>(input).input;
        }

        public static async Task<string> ReadStreamAsyncToString(Stream input)
        {
            using (StreamReader stream = new StreamReader(input))
            {
                return await stream.ReadToEndAsync();
            }
        }
    }
}
