using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZendoImporter.DataModels;

namespace ZendoImporter.Core.Helpers
{
    public static class CSVUtils
    {
        public static List<MergeCustomer> ParseCustomers(string path)
        {
            List<MergeCustomer> result = new List<MergeCustomer>();
            
            if (!File.Exists(path))
                throw new FileNotFoundException();

            // Parse File
            var fileData = File.ReadAllText(path);
            var fileLines = fileData.Split('\n').Skip(1).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();

            // Convert to Objects
            fileLines.ForEach(line =>
            {
                var fields = line.Split(';');

                result.Add(new MergeCustomer
                {
                    AccountNumber = fields[0],
                    AccountName = fields[1],
                    Address1 = fields[2],
                    Address2 = fields[3],
                    ZIP = fields[4],
                    City = fields[5],
                    Telephone = fields[6]
                });
            });

            return result;
        }

        public static List<MergeOrder> ParseOrders(string path)
        {
            List<MergeOrder> result = new List<MergeOrder>();

            if (!File.Exists(path))
                throw new FileNotFoundException();

            // Parse File
            var fileData = File.ReadAllText(path);
            var fileLines = fileData.Split('\n').Skip(1).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();

            // Convert to Objects
            fileLines.ForEach(line =>
            {
                var fields = line.Split(';');

                var mergeOrder = new MergeOrder
                {
                    OrderNumber = fields[0],
                    AccountNumber = fields[1],
                    CreatedDate = DateTime.Parse(fields[3])
                };

                var items = fields[2].Split(',').Select(x => x.Split(':')).ToList();
                items.ForEach(itemData =>
                {
                    mergeOrder.Items.Add(new MergeOrderItem
                    {
                        ItemName = itemData[0],
                        Qty = int.Parse(itemData[1])
                    });
                });

                result.Add(mergeOrder);
            });

            return result;
        }

        public static List<MergeItem> ParseItems(string path)
        {
            List<MergeItem> result = new List<MergeItem>();
            
            if (!File.Exists(path))
                throw new FileNotFoundException();

            // Parse File
            var fileData = File.ReadAllText(path);
            var fileLines = fileData.Split('\n').Skip(1).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();

            // Convert to Objects
            fileLines.ForEach(line =>
            {
                var fields = line.Split(';');

                result.Add(new MergeItem
                {
                    Item = fields[0],
                    ItemName = fields[1],
                    SalesPrice = double.Parse(fields[2]),
                    SalesCurrency = fields[3]
                });
            });

            return result;
        }
    }
}
