using InvoicingApp.Core.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uniconta.Common;
using Uniconta.DataModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InvoicingApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.Initialize();

            this.TB_User.Text = UnicontaAPIManager.GetUserName();

            this.CB_Companies.ItemsSource = UnicontaAPIManager.GetCompanies();
            this.CB_Companies.SelectedItem = UnicontaAPIManager.GetCurrentCompany();
            this.CB_Companies.SelectionChanged += CB_Companies_SelectionChanged;
        }

        #region Get / Set Methods
        public Company GetSelectedCompany() { return this.CB_Companies.SelectedItem as Company; }
        public Debtor GetSelectedCustomer() { return this.CB_Customers.SelectedItem as Debtor; }
        #endregion

        private async void CB_Companies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var company = this.GetSelectedCompany();
            await UnicontaAPIManager.SetCurrentCompany(company);
            LoadDebtors();
        }

        #region Initialize Methods
        private async void Initialize()
        {
            LoadDebtors();
        }

        #endregion


        #region LoadData Methods
        private async void LoadDebtors()
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            var debtors = await crudAPI.Query<Debtor>();
            this.CB_Customers.ItemsSource = debtors;
            this.CB_Customers.SelectedItem = debtors.FirstOrDefault();
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var invoiceAPI = UnicontaAPIManager.GetInvoiceAPI();
            var debtor = this.GetSelectedCustomer();

            // QuickInvoice
            var debtorOrder = new DebtorOrder
            {

            };
            debtorOrder.SetMaster(debtor);

            var debtorOrderLines = new List<DebtorOrderLine>();
            var debtorOrderLine = new DebtorOrderLine
            {

            };
            debtorOrderLine.SetMaster(debtorOrder);
            debtorOrderLines.Add(debtorOrderLine);

            //TODO: Use new API
            var invoiceErrorCode = await invoiceAPI.PostInvoice(debtorOrder, debtorOrderLines, DateTime.Now, 0, false, null, null, true, false);
            if(invoiceErrorCode.Err != ErrorCodes.Succes)
                return;
        }
    }
}
