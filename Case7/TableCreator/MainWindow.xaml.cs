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

            // Events
            this.B_CreateTable.Click += B_CreateTable_Click;
            this.B_CreateFields.Click += B_CreateFields_Click;
        }

        #region Events
        private async void B_CreateTable_Click(object sender, RoutedEventArgs e)
        {
            // TODO: are we in right company ??
            // await UnicontaAPIManager.SetCurrentCompany(8936);

            var crudAPI = UnicontaAPIManager.GetCrudAPI();

            // TODO: Hack for reloading crudAPI.CompanyEntity.UserTables
            //await UnicontaAPIManager.SetCurrentCompany(UnicontaAPIManager.GetCurrentCompanyId());

            this.MyNewTable = new TableHeader
            {
                _Attachment = true,
                _UserDefinedId = 2750,
                _Name = "MyNewTable",
                _Prompt = "My new table",
                _HasPrimaryKey = true,
                _PKprompt = "MyNewTableId",
                _MenuPosition = 7,
            };

            var errorCode = await crudAPI.Insert(this.MyNewTable);
            if(errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Failed to create table");
                return;
            }
            else
                MessageBox.Show("Table has been created");
        }

        private async void B_CreateFields_Click(object sender, RoutedEventArgs e)
        {
            var crudAPI = UnicontaAPIManager.GetCrudAPI();

            /*
            var table = crudAPI.CompanyEntity.UserTables.SingleOrDefault(t => t._Name == "MyNewTable");
            if (table == null)
            {
                MessageBox.Show("ERROR: Cant find table MyNewTable");
                return;
            }
            */
            
            var newFields = new List<TableField>();

            // String field
            var newStringField = new TableField
            {
                _Name = "MyStringField",
                _Prompt = "My string field",
                _FieldType = CustomTypeCode.String
            };
            newStringField.SetMaster(this.MyNewTable);
            newFields.Add(newStringField);

            // Boolean field
            var newBooleanField = new TableField
            {
                _Name = "MyBooleanField",
                _Prompt = "My boolean field",
                _FieldType = CustomTypeCode.Boolean

            };
            newBooleanField.SetMaster(this.MyNewTable);
            newFields.Add(newBooleanField);

            // Call insert API
            var errorCode = await crudAPI.Insert(newFields);
            if (errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Failed to create fields");
                return;
            }
            else
                MessageBox.Show("Fields has been created");
        }
        #endregion
    }
}
