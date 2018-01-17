using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using Uniconta.DataModel;
using ZendoImporter.Core.Helpers;
using ZendoImporter.Core.Managers;

namespace ZendoImporter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.CB_Company.ItemsSource = UnicontaAPIManager.GetCompanies();
            this.CB_Company.SelectedItem = UnicontaAPIManager.GetCurrentCompany();
            this.CB_Company.SelectionChanged += CB_Company_SelectionChanged;
        }
        
        #region Event Methods
        private async void CB_Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Setting Company
            var company = this.CB_Company.SelectedItem as Company;
            await UnicontaAPIManager.SetCurrentCompany(company);
        }
        #endregion

        private async void B_Customers_Click(object sender, RoutedEventArgs e)
        {
            // Getting CrudAPI
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            // Parsing CSV
            var customers = CSVUtils.ParseCustomers(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Customers.csv");

            // Creating Insert List
            var newDebtorClients = new List<DebtorClient>();
            foreach (var customer in customers)
            {
                // Parsing Account Number
                var accountNumber = (int.Parse(customer.AccountNumber) + 20000).ToString();

                // TODO: Add the customer to the newDebtorClients List
            };

            // Calling insert API
            // TODO: Call the insert API to insert newDebtorClients
        }

        private async void B_Items_Click(object sender, RoutedEventArgs e)
        {
            // Getting CrudAPI
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            // Parsing CSV
            var items = CSVUtils.ParseItems(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Items.csv");
            
            // Creating Insert List
            var newInvItemClients = new List<InvItemClient>();
            foreach(var item in items)
            {
                // TODO: Add the item to the newInvItemClients List
            };

            // Calling insert API
            // TODO: Call the insert API to insert newInvItemClients
        }

        private async void B_Orders_Click(object sender, RoutedEventArgs e)
        {
            // Getting CrudAPI
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            crudAPI.RunInTransaction = false;

            // Parsing CSV
            var orders = CSVUtils.ParseOrders(@"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\CompanyData\Finace-Orders.csv");

            // Creating SQLCache's
            // TODO: Create a customer (DebtorClient) SQLCache

            // TODO: Create a inventory (InvItemClient) SQLCache
            
            // Creating Insert List
            var newDebtorOrderClients = new List<DebtorOrderClient>();
            foreach(var order in orders)
            {
                // Parsing Account Number
                var accountNumber = (int.Parse(order.AccountNumber) + 20000).ToString();

                // Finding customer in cache
                // TODO: Use the customerCache to get the customer by accountNumber

                // TODO: Add the order to the newDebtorOrderClients List + SetMaster
            };
            
            // Calling insert API
            var errorCode = await crudAPI.Insert(newDebtorOrderClients);
            if (errorCode != ErrorCodes.Succes)
                MessageBox.Show($"ERROR: Failed to import orders {errorCode.ToString()}");

            // Creating order lines
            var newDebtorOrderLineClients = new List<DebtorOrderLineClient>();
            var inventoryList = inventoryCache.GetRecords as InvItemClient[];
            
            var index = 0;
            foreach (var debtorOrder in newDebtorOrderClients)
            {
                var orderItems = orders[index].Items;
                foreach(var item in orderItems)
                {
                    var inventoryItem = inventoryList.FirstOrDefault(i => i.Name == item.ItemName);
                    
                    // TODO: Add the item to the newDebtorOrderLineClients List + SetMaster
                };

                index++;
            };

            // Calling insert API
            // TODO: Call the insert API to insert newDebtorOrderLineClients
        }
    }
}
