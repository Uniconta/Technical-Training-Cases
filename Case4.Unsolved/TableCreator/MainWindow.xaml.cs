using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TableCreator.Core.Managers;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;
using Uniconta.DataModel;

namespace TableCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fields
        private TableHeader MyNewTable;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        // MenuPosition
        // 0 = General Ledger
        // 1 = Customer
        // 2 = Vendor
        // 3 = Inventory
        // 4 = Project
        // 5 = Company
        // 6 = Tools
        // 7 = None
        // 8 = CRM

        private async void Button_TrackGenres(object sender, RoutedEventArgs e)
        {
            //TODO: Change to test Company
            var company = UnicontaAPIManager.GetCompanyByName("TT-WEEK3");
            var crudAPI = UnicontaAPIManager.GetCrudAPI(company);

            // TrackGenres
            // TODO: Create a TableHeader with the TrackGenre

            // TODO: Call Insert API to insert the TrackGenere Table
        }

        private async void Button_Tracks(object sender, RoutedEventArgs e)
        {
            //TODO: Change to test Company
            var company = UnicontaAPIManager.GetCompanyByName("TT-WEEK3");
            var crudAPI = UnicontaAPIManager.GetCrudAPI(company);

            // Tracks
            // TODO: Create a TableHeader with the Track

            // Inserting Table
            // TODO: Call Insert API to insert the Track Table

            // Creating Fields
            var tracksFields = new List<TableField>();

            // Title
            // TODO: Create a TableField for the Title

            // Artist 
            // TODO: Create a TableField for the Artist

            // Genre 
            // TODO: Create a TableField for the Genre

            // Vibe 
            // TODO: Create a TableField for the Vibe

            // Length 
            // TODO: Create a TableField for the Length

            // LicensePaid 
            // TODO: Create a TableField for the LicensePaid

            // Inserting Fields
            // TODO: Call Insert API to insert the Track Fields
        }
    }
}
