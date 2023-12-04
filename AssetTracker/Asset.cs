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


namespace AssetTrackerEfCore {
    public class Asset {
        public Asset(string type, string brand, string model, string location, decimal price,
            DateTime purchaseDate, string countryCode) {
            Type = type;
            Brand = brand;
            Model = model;
            Location = location;
            Price = price;
            PurchaseDate = purchaseDate;
            CountryCode = countryCode;
        }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }

        public string Model { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string CountryCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Modifier { get; set; }


        public static void AddAsset() {
            string type = setAssetType();
            string brand = setAssetBrand();
            string model = setAssetModel();
            string location = setAssetLocation();
            decimal price = setAssetPrice();
            DateTime purchaseDate = setAssetPurchaseDate();

            string countryCode = Currencies.GetCountryCode(location);
            Currencies.GetCurrency(location);
            Asset newAsset = new Asset(type, brand, model, location, price, purchaseDate, countryCode);
            CurrencyInfo currencyInfo = Currencies.GetCurrency(location);
            newAsset.CurrencySymbol = currencyInfo.Symbol;
            newAsset.CurrencyCode = currencyInfo.IsoCode;

            using (var dbContext = new AssetContext()) {
                dbContext.Assets.Add(newAsset);
                dbContext.SaveChanges();
            }
            Program.Print("\n  " + char.ToUpper(newAsset.Brand[0]) + newAsset.Brand.Substring(1) + " " //Print: "Added asset to office in country"
                + newAsset.Type + " added to the office in "
                + char.ToUpper(newAsset.Location[0]) + newAsset.Location.Substring(1)
                + ".\n", CC.DarkGreen);
        }
        private static string setAssetType() {
            Program.Print("\n What type of asset is this?  ", CC.Cyan);            // Type of Asset
            string type = Console.ReadLine().Trim().ToLower();
            while (type != "computer" && type != "phone" && type != "car") {
                Program.Print($"\n {type} is not a valid asset. We currently track computers, phones and cars.  \n", CC.Red);
                Program.Print("\n What type of asset is it?  ", CC.Cyan);
                type = Console.ReadLine().ToLower();
            }
            return type;
        }
        private static string setAssetBrand() {
            Program.Print("\n What brand is it?  ", CC.Cyan);                        //Brand
            string brand = Console.ReadLine().Trim();
            while (brand == null || brand.Length == 0 || brand.Length > 14) {
                Program.Print("\n\n You must type something in every field. Max 14 characters\n", CC.Red);
                Program.Print("\n What brand is it?  ", CC.Cyan);
                brand = Console.ReadLine().Trim();
            }
            return brand;
        }
        private static string setAssetModel() {
            Program.Print("\n Which model is it?  ", CC.Cyan);                       //Model
            string model = Console.ReadLine();
            while (model == null || model.Length == 0 || model.Length > 14) {
                Program.Print("\n\n You must type something in every field. Max 14 characters\n", CC.Red);
                Program.Print("\n What model is it?  ", CC.Cyan);
                model = Console.ReadLine().Trim();
            }
            return model;
        }
        private static string setAssetLocation() {
            Program.Print("\n Which country is the office located in?  ", CC.Cyan);          // Country
            string location = Console.ReadLine();
            string countryCode = Currencies.GetCountryCode(location);
            while (countryCode == null) {
                Program.Print($"\n {location} is not a valid country. Please enter a valid country name. \n", CC.Red);
                Program.Print("\n Which country is the office located in?  ", CC.Cyan);
                location = Console.ReadLine();
                countryCode = Currencies.GetCountryCode(location);
            }
            location = char.ToUpper(location[0]) + location.Substring(1);
            Currencies.GetCurrency(location);
            return location;
        }
        private static decimal setAssetPrice() {
            Program.Print("\n What was the price in €?  ", CC.Cyan);                         // Price
            decimal price;
            while (true) {
                string priceInput = Console.ReadLine();
                if (decimal.TryParse(priceInput.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price) && price > 0)
                    break;
                else
                    Program.Print("\n Invalid price format. Please enter a valid number. ", CC.DarkRed);
            }
            return price;
        }
        private static DateTime setAssetPurchaseDate() {
            Program.Print("\n What date was it purchased (yyyy-MM-dd):  ", CC.Cyan);              // Date of purchase
            DateTime purchaseDate;
            while (true) {
                if (DateTime.TryParse(Console.ReadLine(), out purchaseDate)) {
                    return purchaseDate;
                } else {
                    Program.Print("\n Invalid date format. ", CC.DarkRed);
                }
            }
        }

        public static void EditAsset() {
            using (var context = new AssetContext()) {
                int assetIdParsed;
                Asset assetToEdit;

                Program.Print("Please type the ID number of the asset you wish to edit: ");
                string assetId = Console.ReadLine();

                while (!int.TryParse(assetId, out assetIdParsed)) {
                    Program.Print($"Invalid input. Please type a valid ID number: ");
                    assetId = Console.ReadLine();
                }

                assetToEdit = context.Assets.FirstOrDefault(asset => asset.Id == assetIdParsed);

                if (assetToEdit == null) {
                    Program.Print($"Asset ID: {assetId} could not be found!", CC.Red);
                } else {
                    assetToEdit.Type = setAssetType();
                    assetToEdit.Brand = setAssetBrand();
                    assetToEdit.Model = setAssetModel();
                    assetToEdit.Location = setAssetLocation();
                    assetToEdit.Price = setAssetPrice();
                    assetToEdit.PurchaseDate = setAssetPurchaseDate();

                    context.SaveChanges();
                    Console.WriteLine("Asset updated successfully.");
                }
            }
        }
        public static void FindAsset() {
            string searchTerm = Console.ReadLine();
            using (var context = new AssetContext()) {
                List<Asset> foundAssets = context.Assets.Where(
                    asset =>
                        asset.Type.Contains(searchTerm) ||
                        asset.Brand.Contains(searchTerm) ||
                        asset.Model.Contains(searchTerm)
                ).ToList();
                DisplayAssets(foundAssets);
            }
        }
        public static void DisplayAssets(List<Asset> assets = null) {
            List<Asset> assetsToBeDisplayed;
            if (assets == null) {
                using (var context = new AssetContext()) {
                    assetsToBeDisplayed = context.Assets
                        .OrderBy(asset => asset.Type)
                        .ThenBy(asset => asset.PurchaseDate)
                        .ToList();
                }
            } else {
                assetsToBeDisplayed = assets;
            }
            if (assetsToBeDisplayed.Count == 0) {
                Program.Print("\n  You haven't added anything to the Asset Tracker.\n", CC.Red);
                return;
            } else {
                Program.Print(" -------------------------------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                Program.Print("\n   ID".PadRight(6) + "TYPE".PadRight(17) + "BRAND".PadRight(17) + "MODEL".PadRight(17) + "LOCATION".PadRight(17) +
                    "PRICE".PadRight(17) + "PURCHASED".PadRight(20) + "VALUE".PadRight(17), CC.Magenta);
                Program.Print("\n -------------------------------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                ConsoleColor color;
                TimeSpan timeSincePurchase;
                double threeYears = 365 * 3;
                double month = 30;
                decimal totalValue = 0;
                foreach (Asset asset in assetsToBeDisplayed) {
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
                    if (asset.Price * asset.Modifier == 0)      // If I don't have the exchange rate
                    {
                        Program.Print("\n".PadRight(4) + $"{asset.Id.ToString().PadRight(6)}{asset.Type.PadRight(13)}{asset.Brand.PadRight(17)}{asset.Model.PadRight(17)}" +
                        $"{Program.Truncate(asset.Location).PadRight(18)}{Program.TruncateNumber(asset.Price.ToString("0.##"))} {" €".PadRight(11)}{asset.PurchaseDate.ToShortDateString().ToString().PadRight(17)}  " +
                        $"Unknown {asset.CurrencySymbol} ({asset.CurrencyCode})\n", color);
                    } else {
                        Program.Print("\n".PadRight(4) + $"{asset.Id.ToString().PadRight(6)}{asset.Type.PadRight(13)}{asset.Brand.PadRight(17)}{asset.Model.PadRight(17)}" +
                        $"{Program.Truncate(asset.Location).PadRight(18)}{Program.TruncateNumber(asset.Price.ToString("0.##"))} {" €".PadRight(11)}{asset.PurchaseDate.ToShortDateString().ToString().PadRight(17)}  " +
                        $"{Program.TruncateNumber((asset.Price * asset.Modifier).ToString("0.##"))} {asset.CurrencySymbol}\n", color);
                    }
                }
                Program.Print("\n -------------------------------------------------------------------------------------------------------------------------------------\n ", CC.DarkBlue);
                Program.Print($" Total value of assets = {totalValue} €\n ");
            }
        }
    }

}



