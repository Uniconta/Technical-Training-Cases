using Case9.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uniconta.Common;
using Uniconta.DataModel;
using Xamarin.Forms;

namespace Case9.Pages
{
    public partial class MainPage : ContentPage
	{
        // Fields
        private Debtor selectedCustomer;
        private DebtorOrder selectedOrder;

        private bool isTimerStarted;
        private DateTime startTime;

        public MainPage()
		{
			InitializeComponent();
            Initialize();

            // Creating Timer
            Device.StartTimer(TimeSpan.FromSeconds(0), () =>
            {
                if (isTimerStarted)
                {
                    var time = DateTime.Now - this.startTime;
                    TotalTime.Text = $"{Math.Round(time.TotalMinutes, 1)} min";
                }

                return true; // True = Repeat again, False = Stop the timer
            });

        }

        #region Initialize / Refresh Methods
        private async void Initialize()
        {
            // Saying hello to the current user
            WelcomeMessage.Text = $"Welcome {UnicontaAPIManager.GetCurrentUsername()}";

            // Creating CompanyPicker
            CompanyPicker.ItemDisplayBinding = new Binding("Name");
            CompanyPicker.ItemsSource = UnicontaAPIManager.GetCompanies();
            CompanyPicker.SelectedItem = UnicontaAPIManager.GetCurrentCompany();
            CompanyPicker.SelectedIndexChanged += CompanyPicker_SelectedIndexChanged;

            // Create CustomerPicker
            CustomerPicker.ItemDisplayBinding = new Binding("KeyName");
            CustomerPicker.SelectedIndexChanged += CustomerPicker_SelectedIndexChanged;
            
            // Create ToggleButton
            ToggleButton.Clicked += ToggleButton_Clicked;

            // Refreshing Data
            await RefreshCustomers();
        }

        private async Task RefreshCustomers()
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();
            var customers = await crudAPI.Query<Debtor>();
            this.selectedCustomer = customers.FirstOrDefault();
            CustomerPicker.ItemsSource = customers;
            CustomerPicker.SelectedItem = this.selectedCustomer;
        }
        #endregion

        #region Picker Event Methods
        private async void CompanyPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            var company = picker.ItemsSource[selectedIndex] as Company;
            await UnicontaAPIManager.SetCurrentCompany(company);
            
            await RefreshCustomers();
        }

        private void CustomerPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            var customer = picker.ItemsSource[selectedIndex] as Debtor;
            this.selectedCustomer = customer;
        }
        #endregion

        #region Toggle Event Methods
        private void ToggleButton_Clicked(object sender, EventArgs e)
        {
            if(this.isTimerStarted)
            {
                isTimerStarted = false;
                ToggleButton.Text = "Start Time";
                AddOrderLine();
            }
            else
            {
                isTimerStarted = true;
                this.startTime = DateTime.Now;
                ToggleButton.Text = "End Time";
            }
        }
        #endregion

        private async void AddOrderLine()
        {
            var invoiceAPI = UnicontaAPIManager.GetInvoiceAPI();
            var newOrder = new DebtorOrder
            {

            };
            newOrder.SetMaster(this.selectedCustomer);

            var timeUsage = (DateTime.Now - this.startTime).TotalHours;
            if (timeUsage < 0.5)
                timeUsage = 0.5;

            var newOrderLines = new List<DebtorOrderLine>();
            var newOrderLine = new DebtorOrderLine
            {
                _Text = "Onsite",
                _Qty = timeUsage,
                _Price = 890,
                _Currency = Currencies.DKK,
            };
            newOrderLine.SetMaster(newOrder);
            newOrderLines.Add(newOrderLine);

            var invoiceresult = await invoiceAPI.PostInvoice(newOrder, newOrderLines, DateTime.Now, -1, false);
            if(invoiceresult.Err != ErrorCodes.Succes)
            {

            }
        }
    }
}
