using System.Globalization;

namespace MySuperStore.Utils.Utils
{
    public static class PriceFormatter
    {
        public static string ToCurrency(decimal amount)
        {
            return amount.ToString("C", new CultureInfo("en-US"));
        }
    }
}
