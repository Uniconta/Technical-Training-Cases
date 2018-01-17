using InvoicingAppWPF.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
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
            var debtors = await crudAPI.Query<DebtorClient>();
            this.CB_Customer.ItemsSource = debtors;
            this.CB_Customer.SelectedItem = debtors.FirstOrDefault();
        }

        #region Event Methods
        private async void CB_Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Setting Company
            var company = this.CB_Company.SelectedItem as Company;
            await UnicontaAPIManager.SetCurrentCompany(company);

            this.LoadCustomers();
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var debtor = this.CB_Customer.SelectedItem as DebtorClient;
            var invoiceAPI = UnicontaAPIManager.GetInvoiceAPI();

            // Creating DebtorOrder
            var debtorOrder = new DebtorOrderClient
            {
                _Currency = Currencies.DKK,
            };
            debtorOrder.SetMaster(debtor);

            // Creating DebtorOrderLine's
            var debtorOrderLines = new List<DebtorOrderLineClient>();
            var debtorOrderLine = new DebtorOrderLineClient
            {
                _Item = "OnSiteSupport",
                _Qty = 2.0,
                _Price = 890,
            };
            debtorOrderLine.SetMaster(debtorOrder);
            debtorOrderLines.Add(debtorOrderLine);

            // Calling Invoice API.
            var invoiceResult = await invoiceAPI.PostInvoice(debtorOrder, debtorOrderLines, DateTime.Now, 0, false, SendEmail: true, Emails: "rh@sit.dk", OnlyToThisEmail: true);
            if(invoiceResult.Err != ErrorCodes.Succes)
            {
                MessageBox.Show($"Failed to send invoice: {invoiceResult.Err.ToString()}");
                return;
            }

            MessageBox.Show($"Invoice has been send InvoiceNumber:{invoiceResult.Header._InvoiceNumber}");
        }
    }
}
