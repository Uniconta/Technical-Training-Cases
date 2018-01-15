using System;
using System.Collections.Generic;
using System.IO;
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
using Uniconta.API.System;
using Uniconta.ClientTools;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using ZendoPlugins.DataModels;

namespace ZendoPlugins
{
    /// <summary>
    /// Interaction logic for SpeakerCalculatorView.xaml
    /// </summary>
    public partial class SpeakerCalculatorView : UserControl
    {
        // Fields
        private SQLCache itemInventoryCache;
        private DebtorOrderClient order;
        private CrudAPI crudAPI;

        private int persons;
        private double roomSize;
        private bool inDoor;
        
        public SpeakerCalculatorView(CrudAPI crudAPI)
        {
            this.crudAPI = crudAPI;

            this.LoadInventoryCache();

            // Initialize Fields
            this.persons = 10;
            this.roomSize = 10;
            this.inDoor = true;

            InitializeComponent();

            // Events
            this.S_PersonAmount.ValueChanged += S_PersonAmount_ValueChanged;
            this.S_RoomSize.ValueChanged += S_RoomSize_ValueChanged;
            this.CB_InDoor.Click += CB_InDoor_Click;

            this.BTN_SelectSpeaker.Click += BTN_SelectSpeaker_Click;
        }

        #region Getter / Setter Methods
        public void SetOrder(DebtorOrderClient order)
        {
            this.order = order;
            this.TB_OrderNumber.Text = "OrderNumber: " + this.order.OrderNumber;
        }
        #endregion

        #region Event Methods
        private void S_PersonAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.persons = (int)e.NewValue;
            this.TB_PersonAmount.Text = "Persons: " + this.persons;
            FindBestSpeaker();
        }

        private void S_RoomSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.roomSize = Math.Round(e.NewValue, 2);
            this.TB_RoomSize.Text = "Room Size (m2): " + this.roomSize;
            FindBestSpeaker();
        }

        private void CB_InDoor_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            this.inDoor = (bool)checkBox.IsChecked;
            FindBestSpeaker();
        }
        
        private void BTN_SelectSpeaker_Click(object sender, RoutedEventArgs e)
        {
            var speakerItem = this.LV_Speakers.SelectedItem as InvItemClientUser;
            if(speakerItem == null)
            {
                MessageBox.Show("ERROR: No item seleted");
                return;
            }

            this.AddSpeakerItem(speakerItem);
        }
        #endregion

        #region Private Methods
        private async void LoadInventoryCache()
        {
            // Creating SQLCache for Inventory Items
            // TODO: Load InvItemClientUser SQL Cache 
        }

        private void FindBestSpeaker()
        {
            // Calulating minimum speaker score
            double minScore = 0;
            if (inDoor)
                minScore = persons * roomSize;
            else
                minScore = persons * (roomSize * 2);

            // Updates speaker score
            this.TB_MinScore.Text = "Score: " + minScore;

            // Finding speaker setup that will fit the demard
            // TODO: Find the item using the Cache

            /*
            // This would be the none cached version
            var filter = new List<PropValuePair>
            {
                PropValuePair.GenereteWhereElements("SpeakerScore", typeof(string), " > " + minScore),
                PropValuePair.GenereteWhereElements("Item", typeof(string), "SPEAKER..")
            };
            var items = await crudAPI.Query<InvItemClientUser>(filter);
            */

            // Updating ListView
            this.LV_Speakers.ItemsSource = items;
        }

        private async void AddSpeakerItem(InvItemClientUser speakerItem)
        {
            // Creating new order line
            // TODO: Create Order Line, Remember to setMaster

            // Calling insert API
            // TODO: Call Insert API

            // Opens another uniconta tab
            UnicontaTabs.OpenTab(UnicontaTabs.DebtorOrders, null);
        }
        #endregion
    }
}
