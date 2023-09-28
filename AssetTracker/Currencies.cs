using Nager.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using CC = System.ConsoleColor;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Xml.Linq;

namespace AssetTracker
{
    internal class Currencies
    {
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
        internal static void DownloadXml(string filePath)
        {
            if (!File.Exists(filePath) || !IsXmlUpToDate(filePath))
            {
                string xmlUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                try
                {
                    using (WebClient webClient = new WebClient())  //Save
                    {
                        webClient.DownloadFile(xmlUrl, filePath);
                        Console.WriteLine("Downloading up-to-date currency exchange rate data...");
                        Console.WriteLine("Currency exchange rate data downloaded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading ECB currency chart: {ex.Message}");
                }
                
            }
            else
            {
                Console.WriteLine("Currency exchange rate data is up-to-date.");
            }
            
        }
        internal static void UpdateConversionModifier(List<Asset> assetList, string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            foreach (Asset asset in assetList)
            {
                var matchingElements = doc.Descendants().Where(e => (string)e.Attribute("currency") == asset.Currency.Item2);
                foreach (var element in matchingElements)
                {
                    string modifierString = Regex.Match(element.ToString(), @"\d+\.\d+").Value;
                    Console.WriteLine(modifierString);
                    Console.WriteLine(modifierString.GetType());
                    asset.Modifier = Convert.ToDecimal(modifierString);
                    decimal.TryParse(modifierString, CultureInfo.InvariantCulture);
                    {
                        asset.Modifier = Math.Round(modifier, 3);
                        Console.WriteLine(asset.Modifier);
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing modifierString: {modifierString}");
                    }*/
                }
            }
        }
        internal static bool IsXmlUpToDate(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            else
            {
                DateTime lastModifiedDate = File.GetLastWriteTime(filePath);
                return lastModifiedDate.Date == DateTime.Now.Date;
            }
        }
    }
}
//TODO
// Decimal conversion still not working in UpdateConversionMod