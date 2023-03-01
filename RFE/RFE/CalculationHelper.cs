using System.Text;

namespace RFE
{
    public static class CalculationHelper
    {
        public static string GetStringDiff(string left, string right)
        {
            var diff = new StringBuilder();

            if (left.Length != left.Length)
            {
                return string.Empty;
            }

            int? offset = null;
            var lenght = 0;

            for (var i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    if (lenght == 0)
                    {
                        offset = i;
                    }
                    lenght++;
                }
                else if (offset != null && lenght != 0)
                {
                    diff.AppendFormat("{0} {1} ", offset.Value.ToString(), lenght.ToString());
                    offset = null;
                    lenght = 0;
                }
            }
            if (offset != null && lenght != 0)
            {
                diff.AppendFormat("{0} {1} ", offset.Value.ToString(), lenght.ToString());
            }

            return diff.ToString();
        }
    }
}
