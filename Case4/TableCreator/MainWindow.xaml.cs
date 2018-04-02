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
            var trackGenres = new TableHeader
            {
                // Key
                _HasPrimaryKey = true,
                _PKprompt = "TrackGenreId",
                _AutoKey = true,

                // Description
                _Name = "TrackGenres",
                _Prompt = "Track Genres",
                _MenuPosition = 3,
                _UserDefinedId = 2050,

                // Settings
                _EditLines = true
            };

            var errorCode = await crudAPI.Insert(trackGenres);
            if(errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show($"Failed to insert Track Genres {errorCode.ToString()}");
                return;
            }
            else
                MessageBox.Show("Track Genres table has been created");
        }

        private async void Button_Tracks(object sender, RoutedEventArgs e)
        {
            //TODO: Change to test Company
            var company = UnicontaAPIManager.GetCompanyByName("TT-WEEK3");
            var crudAPI = UnicontaAPIManager.GetCrudAPI(company);
            

             // Tracks
             var tracks = new TableHeader
            {
                // Key
                _HasPrimaryKey = true,
                _PKprompt = "TrackId",
                _AutoKey = true,

                // Description
                _Name = "Tracks",
                _Prompt = "Tracks",
                _MenuPosition = 3,
                _UserDefinedId = 2051,

                // Settings
                _EditLines = true
            };

            // Inserting Table
            var errorCode = await crudAPI.Insert(tracks);
            if (errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show($"Failed to insert Tracks {errorCode.ToString()}");
                return;
            }
            else
                MessageBox.Show("Tracks table has been created");

            // Creating Fields
            var tracksFields = new List<TableField>();

            // Title
            var trackTitle = new TableField
            {
                _Name = "Title",
                _Prompt = "Title",
                _FieldType = CustomTypeCode.String
            };
            trackTitle.SetMaster(tracks);
            tracksFields.Add(trackTitle);
                      
            // Artist 
            var trackArtist = new TableField
            {
                _Name = "Artist",
                _Prompt = "Artist",
                _FieldType = CustomTypeCode.String
            };
            trackArtist.SetMaster(tracks);
            tracksFields.Add(trackArtist);

            // Genre 
            var trackGenre = new TableField
            {
                _Name = "Genre",
                _Prompt = "Genre",
                _FieldType = CustomTypeCode.String,
                _RefTable = "TrackGenres"
            };
            trackGenre.SetMaster(tracks);
            tracksFields.Add(trackGenre);

            // Vibe 
            var trackVibe = new TableField
            {
                _Name = "Vibe",
                _Prompt = "Vibe",
                _FieldType = CustomTypeCode.Enum,
                _Format = "Dance;Up-beat;Soft;Slow;"
            };
            trackVibe.SetMaster(tracks);
            tracksFields.Add(trackVibe);

            // Length 
            var trackLength = new TableField
            {
                _Name = "Length",
                _Prompt = "Length",
                _FieldType = CustomTypeCode.Integer,
            };
            trackLength.SetMaster(tracks);
            tracksFields.Add(trackLength);

            // LicensePaid 
            var trackLicensePaid = new TableField
            {
                _Name = "LicensePaid",
                _Prompt = "License Paid",
                _FieldType = CustomTypeCode.Boolean,
            };
            trackLicensePaid.SetMaster(tracks);
            tracksFields.Add(trackLicensePaid);
            
            // Inserting Fields
            var fieldsErrorCode = await crudAPI.Insert(tracksFields);
            if (fieldsErrorCode != ErrorCodes.Succes)
            {
                MessageBox.Show($"Failed to insert Tracks {errorCode.ToString()}");
                return;
            }
            else
                MessageBox.Show("Tracks table has been created");
        }
    }
}
