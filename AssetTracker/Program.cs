using Nager.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CC = System.ConsoleColor;

namespace AssetTracker
{
    internal class Program
    {
        internal static void Print(string text, CC fgColor = CC.White, CC bgColor = CC.Black)
        {
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.Write(text);
            Console.ResetColor();
        }
        internal static string GetCountryCode(string countryName)
        {
            var countryProvider = new CountryProvider();
            var country = countryProvider.GetCountries().FirstOrDefault(c => c.CommonName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
            return country?.Alpha2Code.ToString();
        }
        internal static (string Symbol, string Currency) GetCurrency(string countryName)
        {
            var countryProvider = new CountryProvider();
            var country = countryProvider.GetCountries().FirstOrDefault(c => c.CommonName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

            if (country != null)
            {
                var currency = country.Currencies.FirstOrDefault();
                if (currency != null)
                {
                    string symbol = currency.Symbol;
                    string isoCode = currency.IsoCode;
                    Program.Print($"Country: {countryName} uses {currency.Symbol} ({currency.IsoCode})");
                    return (symbol, isoCode);
                }

                else
                {
                    Program.Print($"No currency information found for {countryName}.", CC.Red);
                    return (String.Empty, String.Empty);
                }
            }
            else
            {
                Program.Print($"Country: {countryName} not found.", CC.Red);
                return (String.Empty, String.Empty);
            }

        }

        internal static void Main()
        {
            List<Asset> assetList = new List<Asset>();
            Print(" ---------------------------------\n | ", CC.DarkBlue);
            Print(" Welcome to the AssetTracker ", CC.DarkYellow);
            Print(" | \n ---------------------------------\n", CC.DarkBlue);
            while (true)
            {
                Print("\n Press a desired key to select an option.\n\n", CC.Blue, CC.Black);
                Print(  " Press \"A\" to add an Asset\n" +
                        " Press \"D\" to display Assets\n" +
                        " Press \"Q\" to quit\n", CC.Cyan);     
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Hide the key from the console
                char keyChar = char.ToLower(keyInfo.KeyChar);
                if (keyChar == 'q')
                {
                    Print("\nExiting application...\n", CC.Red);
                    break;
                }
                else if (keyChar == 'a') Asset.AddAsset(assetList);
                else if (keyChar == 'd') Asset.DisplayAssets(assetList);
                else Print($"\n\n          {keyChar} is not a valid option!\n\n", CC.Red);
            }   
        }
    }
   
}


