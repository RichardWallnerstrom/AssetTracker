using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nager.Country;
using Nager.Country.Currencies;
using CC = System.ConsoleColor;


namespace AssetTracker
{
    public class Asset
    {
        public Asset(string type, string brand, string model, string location, decimal price, 
            DateTime purchaseDate, string countryCode, (string symbol, string isoCode) currency)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Location = location;
            Price = price;
            PurchaseDate = purchaseDate;
            CountryCode = countryCode;
            Currency = currency;
        }

        public string Type { get; set; }
        public string Brand { get; set; }
        
        public string Model { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string CountryCode { get; set; }
        public (string, string) Currency { get; set; }
        public decimal Modifier { get; set; }


        public static void AddAsset(List<Asset> assetList)
        {
            Program.Print("\n What type of asset is this?  ", CC.Cyan);            // Type of Asset
            string type = Console.ReadLine().Trim().ToLower();
            while (type != "computer" && type != "phone" && type != "car")
            {
                Program.Print($"\n {type} is not a valid asset. We currently track computers, phones and cars.  \n", CC.Red);
                Program.Print("\n What type of asset is it?  ", CC.Cyan);
                type = Console.ReadLine().ToLower();
            }
            Program.Print("\n What brand is it?  ", CC.Cyan);                        //Brand
            string brand = Console.ReadLine().Trim();
            while (brand == null || brand.Length == 0 || brand.Length > 14)
            {
                Program.Print("\n\n You must type something in every field. Max 14 characters\n", CC.Red);
                Program.Print("\n What brand is it?  ", CC.Cyan);                        
                brand = Console.ReadLine().Trim();

            }
            Program.Print("\n Which model is it?  ", CC.Cyan);                       //Model
            string model = Console.ReadLine();
            while (model == null || model.Length == 0 || model.Length > 14)
            {
                Program.Print("\n\n You must type something in every field. Max 14 characters\n", CC.Red);
                Program.Print("\n What model is it?  ", CC.Cyan);                        
                model = Console.ReadLine().Trim();

            }
            Program.Print("\n Which country is the office located in?  ", CC.Cyan);          // Country
            string location = Console.ReadLine();
            string countryCode = Currencies.GetCountryCode(location);
            while (countryCode == null)
            {
                Program.Print($"\n {location} is not a valid country. Please enter a valid country name. \n", CC.Red);
                Program.Print("\n Which country is the office located in?  ", CC.Cyan);
                location = Console.ReadLine();
                countryCode = Currencies.GetCountryCode(location);
            }
            location = char.ToUpper(location[0]) + location.Substring(1);
            Currencies.GetCurrency(location);
            Program.Print("\n What was the price in €?  ", CC.Cyan);                         // Price
            decimal price;
            while (true)
            {
                string priceInput = Console.ReadLine();
                if (decimal.TryParse(priceInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price) && price > 0) break;
                else Program.Print("\n Invalid price format. Please enter a valid number. ", CC.DarkRed);
            }
            Program.Print("\n What date was it purchased (yyyy-MM-dd):  ", CC.Cyan);              // Date of purchase
            DateTime purchaseDate;
            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out purchaseDate))
                {
                    Asset newAsset = new Asset(type, brand, model, location, price, purchaseDate, countryCode, Currencies.GetCurrency(location));
                    assetList.Add(newAsset);
                    Program.Print("\n  " + char.ToUpper(newAsset.Brand[0]) + newAsset.Brand.Substring(1) + " " //Print: "Added asset to office in country"
                        + newAsset.Type + " added to the office in "    
                        + char.ToUpper(newAsset.Location[0]) + newAsset.Location.Substring(1) 
                        + ".\n", CC.DarkGreen);
                    break;
                }
                else
                {
                    Program.Print("\n Invalid date format. ", CC.DarkRed);
                }
            }
        }
        public static void DisplayAssets(List<Asset> assetList)
        {
            assetList = assetList.OrderBy(asset => asset.Type).ThenBy(asset => asset.PurchaseDate).ToList();
            if (assetList.Count == 0)
            {
                Program.Print("\n  You haven't added anything to the Asset Tracker.\n", CC.Red);
                return;
            }
            else
            {
                Program.Print(" --------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                Program.Print("\n   TYPE".PadRight(17) + "BRAND".PadRight(17) + "MODEL".PadRight(17) + "LOCATION".PadRight(17) + 
                    "PRICE".PadRight(17) + "PURCHASED".PadRight(20) + "VALUE".PadRight(17), CC.Magenta);
                Program.Print("\n --------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                ConsoleColor color;
                TimeSpan timeSincePurchase;
                double threeYears = 365 * 3;
                double month = 30;
                decimal totalValue = 0;
                foreach (Asset asset in assetList)
                {
                    totalValue += asset.Price;
                    timeSincePurchase = DateTime.Now - asset.PurchaseDate;
                    if (timeSincePurchase.Days > threeYears)                           //Over due
                        color = CC.DarkRed;
                    else if (timeSincePurchase.Days >= (threeYears - (month * 3)))   //3 months or less until 3 year mark  
                        color = CC.Red;         
                    else if (timeSincePurchase.Days > (threeYears - (month * 6)))                    //< 6 months until 3 year mark
                        color = CC.DarkYellow;
                    else                                                                    
                        color = CC.Green;
                    if (asset.Price * asset.Modifier == 0)      // If i dont have the exchange rate
                    {
                        Program.Print("\n".PadRight(4) + $"{asset.Type.PadRight(13)}{asset.Brand.PadRight(17)}{asset.Model.PadRight(17)}" +
                        $"{Program.Truncate(asset.Location).PadRight(18)}{Program.TruncateNumber(asset.Price.ToString("0.##"))} {" €".PadRight(11)}{asset.PurchaseDate.ToShortDateString().ToString().PadRight(17)}  " +
                       $"Unknown {asset.Currency.Item1} ({asset.Currency.Item2})\n", color);
                    }
                    else
                    {
                        Program.Print("\n".PadRight(4) + $"{asset.Type.PadRight(13)}{asset.Brand.PadRight(17)}{asset.Model.PadRight(17)}" +
                        $"{Program.Truncate(asset.Location).PadRight(18)}{Program.TruncateNumber(asset.Price.ToString("0.##"))} {" €".PadRight(11)}{asset.PurchaseDate.ToShortDateString().ToString().PadRight(17)}  " +
                        $"{Program.TruncateNumber((asset.Price * asset.Modifier).ToString("0.##"))} {asset.Currency.Item1}\n", color);
                    }
                }
                Program.Print("\n --------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                Program.Print($" Total value of assets = {totalValue} €\n ");
            }
        }
    }
}

