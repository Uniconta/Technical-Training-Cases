using InvoicingAppWPF.Core.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Uniconta.ClientTools.DataModel;
using Uniconta.DataModel;

namespace InvoicingAppWPF.Views
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

            this.LoadCustomers();
        }

        private async void LoadCustomers()
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();

            // TODO: Query for debtors in the current Company

            this.CB_Customer.ItemsSource = debtors;
            this.CB_Customer.SelectedItem = debtors.FirstOrDefault();
        }

        #region Event Methods
        private async void CB_Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Setting Company
            var company = this.CB_Company.SelectedItem as Company;

            // TODO: Set the company to the Current Company

            this.LoadCustomers();
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var debtor = this.CB_Customer.SelectedItem as DebtorClient;
            var invoiceAPI = UnicontaAPIManager.GetInvoiceAPI();

            // Creating DebtorOrder
            // TODO: Create a DebtorOrderClient + Master Relation

            // Creating DebtorOrderLine's
            var debtorOrderLines = new List<DebtorOrderLineClient>();
            // TODO: Create a DebtorOrderLineClient + Master Relation
          
            // Calling Invoice API.
            // TODO: Call InvoiceAPI with Post + Error Handle
        
            MessageBox.Show($"Invoice has been send InvoiceNumber:{invoiceResult.Header._InvoiceNumber}");
        }
    }
}
