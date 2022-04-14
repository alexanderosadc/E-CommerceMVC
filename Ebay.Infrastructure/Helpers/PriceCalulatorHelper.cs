namespace Ebay.Infrastructure.Helpers
{
    public static class PriceCalulatorHelper
    {
        public static double GetFinalPrice(double currentPrice, double discountsSum)
        {
            var discountValue = ((currentPrice * discountsSum) / 100);
            return currentPrice - discountValue;
        }
    }
}
