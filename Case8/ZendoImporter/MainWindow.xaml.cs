using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using ZendoImporter.Core.Helpers;
using ZendoImporter.Core.Managers;

namespace ZendoImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Events
            this.B_Customers.Click += B_Customers_Click;
            this.B_Items.Click += B_Items_Click;
            this.B_Orders.Click += B_Orders_Click;
        }

        private async void B_Customers_Click(object sender, RoutedEventArgs e)
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            var customers = CSVUtils.ParseCustomers(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Customers.csv");
            PBTB_Customers.Text = $"0/{customers.Count}";

            // Creating Insert List
            var newDebtorClients = new List<DebtorClient>();
            customers.ForEach(customer =>
            {
                // TODO: Fix KeyStr being the same
                //var accountNumber = int.Parse(customer.AccountNumber) + 20000;
                var accountNumber = customer.AccountNumber;

                newDebtorClients.Add(new DebtorClient
                {
                    _Account = accountNumber.ToString(),
                    _Name = customer.AccountName,
                    _Address1 = customer.Address1,
                    _Address2 = customer.Address2,
                    _ZipCode = customer.ZIP,
                    _Phone = customer.Telephone
                });
            });
            
            // Calling insert API
            var errorCode = await crudAPI.Insert(newDebtorClients);
            if (errorCode != ErrorCodes.Succes)
                MessageBox.Show($"ERROR: Failed to import customers {errorCode.ToString()}");
        }

        private async void B_Items_Click(object sender, RoutedEventArgs e)
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            var items = CSVUtils.ParseItems(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Items.csv");
            PBTB_Items.Text = $"0/{items.Count}";
            
            // Creating Insert List
            var newInvItemClients = new List<InvItemClient>();
            items.ForEach(item =>
            {
                newInvItemClients.Add(new InvItemClient
                {
                    _Item = item.Item,
                    _Name = item.ItemName,
                    _SalesPrice1 = item.SalesPrice,
                    _Group = "Grp1"
                });
            });

            // Calling insert API
            var errorCode = await crudAPI.Insert(newInvItemClients);
            if (errorCode != ErrorCodes.Succes)
                MessageBox.Show($"ERROR: Failed to import items {errorCode.ToString()}");
        }

        private async void B_Orders_Click(object sender, RoutedEventArgs e)
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            var orders = CSVUtils.ParseOrders(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Orders.csv");
            PBTB_Orders.Text = $"0/{orders.Count}";

            // Creating SQLCache's
            SQLCache customerCache = crudAPI.CompanyEntity.GetCache(typeof(DebtorClient));
            if (customerCache == null)
                customerCache = await crudAPI.CompanyEntity.LoadCache(typeof(DebtorClient), crudAPI);

            SQLCache inventoryCache = crudAPI.CompanyEntity.GetCache(typeof(InvItemClient));
            if (inventoryCache == null)
                inventoryCache = await crudAPI.CompanyEntity.LoadCache(typeof(InvItemClient), crudAPI);


            // Creating Insert List
            var newDebtorOrderClients = new List<DebtorOrderClient>();
            orders.ForEach(order =>
            {
                // TODO: Fix KeyStr being the same
                var accountNumber = (int.Parse(order.AccountNumber) + 20000).ToString();

                // Finding customer in cache
                var customer = customerCache.Get(accountNumber) as DebtorClient;

                var newDebtorOrderClient = new DebtorOrderClient
                {
                    // TODO: Fix this bug
                    _Created = order.CreatedDate
                };
                newDebtorOrderClient.SetMaster(customer);
                newDebtorOrderClients.Add(newDebtorOrderClient);
            });
            
            // Calling insert API
            var errorCode = await crudAPI.Insert(newDebtorOrderClients);
            if (errorCode != ErrorCodes.Succes)
                MessageBox.Show($"ERROR: Failed to import orders {errorCode.ToString()}");

            // Creating order lines
            var newDebtorOrderLineClients = new List<DebtorOrderLineClient>();
            var inventoryList = inventoryCache.GetRecords as InvItemClient[];
            
            var index = 0;
            newDebtorOrderClients.ForEach(debtorOrder =>
            {
                var orderItems = orders[index].Items;
                orderItems.ForEach(item =>
                {
                    var inventoryItem = inventoryList.FirstOrDefault(i => i.Name == item.ItemName);

                    var orderLine = new DebtorOrderLineClient
                    {
                        _Item = inventoryItem.Item,
                        _Qty = 1,
                        _Price = inventoryItem.SalesPrice1
                    };
                    orderLine.SetMaster(debtorOrder);
                    newDebtorOrderLineClients.Add(orderLine);
                });

                index++;
            });
            
            // Calling insert API
            var errorCode2 = await crudAPI.Insert(newDebtorOrderLineClients);
            if (errorCode2 != ErrorCodes.Succes)
                MessageBox.Show($"ERROR: Failed to import order lines {errorCode2.ToString()}");
        }
    }
}
