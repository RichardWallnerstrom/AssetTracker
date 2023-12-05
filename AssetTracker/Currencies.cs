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

namespace AssetTrackerEfCore {
    internal class Currencies {
        internal static string GetCountryCode(string countryName) { // Using Nager.Country to find information about countries
            var countryProvider = new CountryProvider();
            var country = countryProvider.GetCountries().FirstOrDefault(c => c.CommonName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
            return country?.Alpha2Code.ToString();
        }
        internal static CurrencyInfo GetCurrency(string countryName) {
            var countryProvider = new CountryProvider();
            var country = countryProvider.GetCountries().FirstOrDefault(c => c.CommonName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

            if (country != null) {
                var currency = country.Currencies.FirstOrDefault();
                if (currency != null) {
                    string symbol = currency.Symbol;
                    string isoCode = currency.IsoCode;
                    return new CurrencyInfo { IsoCode = isoCode, Symbol = symbol };
                } else {
                    Program.Print($" No currency information found for {countryName}.", CC.Red);
                }
            } else {
                Program.Print($" Country: {countryName} not found.", CC.Red);
            }
            return new CurrencyInfo();
        }
        internal static void DownloadXml(string filePath) {  // Check if ecb exchange data is up to date. If not download it. 
            if (!File.Exists(filePath) || !IsXmlUpToDate(filePath)) {
                string xmlUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
                try {
                    using (WebClient webClient = new WebClient()) {
                        webClient.DownloadFile(xmlUrl, filePath);
                        Program.Print("\n Downloading todays currency exchange rate data...", CC.DarkYellow);
                        Program.Print("\n Currency exchange rate data downloaded successfully.\n\n", CC.Green);
                    }
                }
                catch (Exception ex) {
                    Program.Print($"\n\n Error downloading ECB currency chart: {ex.Message}\n\n", CC.Red);
                }
            } else {
                Program.Print("\n\n --> Currency exchange rate data is up-to-date <--\n\n", CC.Green);
            }

        }
        internal static void UpdateConversionModifier(List<Asset> assetList, string filePath) {    // Find the and update correct exchange rate depending on Country IsoCode. 
            XDocument doc = XDocument.Load(filePath);

            foreach (Asset asset in assetList) {
                if (asset.CurrencyCode == "EUR") {
                    asset.Modifier = 1;
                } else {
                    var matchingElements = doc.Descendants().Where(e => (string)e.Attribute("currency") == asset.CurrencyCode);
                    foreach (var element in matchingElements) {
                        string modifierString = Regex.Match(element.ToString(), @"\d+\.\d+").Value;   // Match number.number
                        if (decimal.TryParse(modifierString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal modifier)) {
                            asset.Modifier = modifier;
                        } else {
                            Program.Print($"Error parsing modifierString: {modifierString}", CC.Red);
                        }
                    }
                }
            }
        }
        internal static bool IsXmlUpToDate(string filePath) {
            if (!File.Exists(filePath))
                return false;
            else {
                DateTime lastModifiedDate = File.GetLastWriteTime(filePath);
                return lastModifiedDate.Date == DateTime.Now.Date;
            }
        }
    }
}