namespace MySuperStore.Utils
{
    public static class TotalCalculator
    {
        public static decimal CalculateTotal(IEnumerable<(decimal Price, int Quantity)> products)
        {
            return products.Sum(p => p.Price * p.Quantity);
        }

        public static string ToCurrency(decimal value)
        {
            return value.ToString("C", new System.Globalization.CultureInfo("en-US"));
        }
    }
}
