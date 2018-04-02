using System;
using System.Collections.Generic;

namespace ZendoImporter.DataModels
{
    public class MergeOrder
    {
        public string OrderNumber { get; set; }

        public string AccountNumber { get; set; }

        public List<MergeOrderItem> Items { get; set; }

        public DateTime CreatedDate { get; set; }

        public MergeOrder()
        {
            this.Items = new List<MergeOrderItem>();
        }
    }
}
