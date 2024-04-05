namespace BuyBikeShop.Data
{
    public static class Utils
    {
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

      
            string firstLetterCapitalized = char.ToUpper(input[0]) + input.Substring(1).ToLower();
            return firstLetterCapitalized;
        }
    }
}
