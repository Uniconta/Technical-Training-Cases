using PluginEmulator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Uniconta.API.Plugin;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;

namespace LoadPluginEmulator
{
    public partial class MainWindow : Window
    {
        UnicontaBaseEntity masterTable = null;
        UnicontaBaseEntity detailTable = null;
        IEnumerable<UnicontaBaseEntity> entityCollection = null;
        string selectedInterface = string.Empty;
        List<Type> tablestype;
        List<Type> childTablestype;
        Type type;

        public MainWindow()
        {
            InitializeComponent();
            lbMaster.Visibility = Visibility.Collapsed;
            lbMasterDtl.Visibility = Visibility.Collapsed;
            btnMstData.Visibility = Visibility.Collapsed;
            btnchildData.Visibility = Visibility.Collapsed;
            chkMaster.IsEnabled = false;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayLoginScreen();
        }

        private async void DisplayLoginScreen()
        {
            var logon = new LoginWindow();
            logon.Owner = this;
            logon.ShowDialog();

            if (logon.DialogResult.HasValue && logon.DialogResult.Value)
            {
                await DemoInitializer.SetupCompanies();

                cmbCompanies.ItemsSource = DemoInitializer.Companies;

                if (DemoInitializer.CurrentSession.User._DefaultCompany != 0)
                {
                    var comp = DemoInitializer.Companies.Where(c => c.CompanyId == DemoInitializer.CurrentSession.User._DefaultCompany).FirstOrDefault();
                    cmbCompanies.SelectedItem = comp;
                }
                else if (DemoInitializer.Companies.Count() > 0)
                    cmbCompanies.SelectedItem = DemoInitializer.Companies[0];

                else
                    MessageBox.Show("You do not have any access to company.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
                this.Close();
        }

        private async void SetCompany(int companyId)
        {
            await DemoInitializer.SetCompany(companyId);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ComboBoxItem)cmbInterface.SelectedItem).Content;

            if (selectedItem != null)
            {
                selectedInterface = selectedItem.ToString();

                if (selectedInterface == "IPluginBase")
                {
                    lblMasterOnly.Visibility = Visibility.Visible;
                    chkMaster.Visibility = Visibility.Visible;
                    lblMstChld.Visibility = Visibility.Visible;
                    chkMstChild.Visibility = Visibility.Visible;
                    lbMaster.ItemsSource = null;
                    lbMasterDtl.Items.Clear();
                    dgMasterTableData.ItemsSource = null;
                    dgchildTableData.ItemsSource = null;
                    chkMstChild.IsChecked = false;
                    chkMaster.IsChecked = false;
                    tab4.Content = null;
                    tab1.IsSelected = true;
                }
                else if (selectedInterface == "ICreditorPaymentFormatPlugin")
                {
                    chkMstChild.IsChecked = false;
                    chkMaster.IsChecked = false;
                    lblMasterOnly.Visibility = Visibility.Collapsed;
                    chkMaster.Visibility = Visibility.Collapsed;
                    lblMstChld.Visibility = Visibility.Visible;
                    chkMstChild.Visibility = Visibility.Visible;
                    lbMaster.ItemsSource = null;
                    lbMasterDtl.ItemsSource = null;
                    dgMasterTableData.ItemsSource = null;
                    dgchildTableData.ItemsSource = null;
                    tab4.Content = null;
                    tab1.IsSelected = true;

                }
                else if (selectedInterface == "IContentPluginBase")
                {
                    chkMstChild.IsChecked = false;
                    chkMaster.IsChecked = false;
                    lblMasterOnly.Visibility = Visibility.Collapsed;
                    chkMaster.Visibility = Visibility.Collapsed;
                    lblMstChld.Visibility = Visibility.Collapsed;
                    chkMstChild.Visibility = Visibility.Collapsed;
                    lbMaster.ItemsSource = null;
                    lbMasterDtl.ItemsSource = null;
                    dgMasterTableData.ItemsSource = null;
                    dgchildTableData.ItemsSource = null;
                    tab4.Content = null;
                    tab4.IsSelected = true;
                }
            }
        }
    
        private void chkMstChild_Checked(object sender, RoutedEventArgs e)
        {
            lbMaster.Visibility = Visibility.Visible;
            lbMasterDtl.Visibility = Visibility.Visible;
            tab3.Visibility = Visibility.Visible;
            btnchildData.Visibility = Visibility.Visible;
            btnMstData.Visibility = Visibility.Visible;
            lbMaster.ItemsSource = null;
            chkMaster.IsChecked = false;
            chkMaster.IsEnabled = true;
            dgMasterTableData.ItemsSource = null;
            dgchildTableData.ItemsSource = null;
            tab1.IsSelected = true;
            lblChildTable.Visibility = Visibility.Visible;

            if (selectedInterface == "ICreditorPaymentFormatPlugin")
            {
                var xList = new List<string>();
                xList.Add("CreditorPaymentFormatClient");
                lbMaster.ItemsSource = xList;
            }
            else
            {
                bindTablelist();
            }
        }
        private void chkMaster_Checked(object sender, RoutedEventArgs e)
        {
            bindTablelist();
            lbMasterDtl.Visibility = Visibility.Collapsed;
            lblChildTable.Visibility = Visibility.Collapsed;
            tab3.Visibility = Visibility.Collapsed;
            btnchildData.Visibility = Visibility.Collapsed;
            chkMstChild.IsChecked = false;
            dgchildTableData.ItemsSource = null;
            tab1.IsSelected = true;
            dgMasterTableData.ItemsSource = null;
            dgchildTableData.ItemsSource = null;
        }

        private void lbMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbMaster.SelectedItem == null) return;
            var selectedItem = lbMaster.SelectedItem.ToString();

            if (selectedItem == "CreditorPaymentFormatClient")
            {
                masterTable = new CreditorPaymentFormatClient();
                lbMasterDtl.Items.Add("CreditorTransOpenClient");
            }
            else
            {
                var selectedType = string.Empty;
                var endIndex = selectedItem.IndexOf('(') - 1;
                if (endIndex > 0)
                    selectedType = selectedItem.Substring(0, endIndex).Replace(" ", "");
                else
                    selectedType = selectedItem;

                Type sltype = (from l in tablestype where l.Name.Split('.').Last() == selectedType select l).FirstOrDefault();
                if (sltype == null) return;

                var obj = Activator.CreateInstance(sltype);
                if (obj == null) return;

                masterTable = (UnicontaBaseEntity)obj;
                var properties = obj.GetType().GetProperties();
                var itemInfos = new ObservableCollection<ItemInfo>();

                foreach (var property in properties)
                {
                    if (property.PropertyType.IsInterface && property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        itemInfos.Add(new ItemInfo() { Caption = property.PropertyType.GenericTypeArguments[0].Name, ItemType = property.PropertyType.GenericTypeArguments[0], ItemObject = null });
                    }
                }

                var xList = new List<string>();

                childTablestype = new List<Type>();

                foreach (var childTable in itemInfos)
                {
                    var clientTableAttr = childTable.ItemType.GetCustomAttributes(typeof(ClientTableAttribute), true);
                    if (clientTableAttr == null || clientTableAttr.Length == 0)
                    {
                        xList.Add(childTable.ItemType.Name);
                    }
                    else
                    {
                        var attr = (ClientTableAttribute)clientTableAttr[0];
                        xList.Add(string.Format("{0} ({1})", childTable.ItemType.Name, Uniconta.ClientTools.Localization.lookup(attr.LabelKey)));
                    }
                    childTablestype.Add(childTable.ItemType);
                }

                lbMasterDtl.ItemsSource = xList.OrderBy(x => x).ToList();
            }
        }

        private void bindTablelist()
        {
            var xlist = new List<string>();
            var api = DemoInitializer.GetBaseAPI;

            tablestype = Global.GetTables(api.CompanyEntity); // Standard tables
            tablestype.AddRange(Global.GetUserTables(api.CompanyEntity)); // User-defined Tables

            foreach (var type in tablestype)
            {
                var clientTableAttr = type.GetCustomAttributes(typeof(ClientTableAttribute), true);
                if (clientTableAttr == null || clientTableAttr.Length == 0)
                    xlist.Add(type.Name);
                else
                {
                    var attr = (ClientTableAttribute)clientTableAttr[0];
                    xlist.Add(string.Format("{0} ({1})", type.Name, Uniconta.ClientTools.Localization.lookup(attr.LabelKey)));
                }
            }
            lbMaster.ItemsSource = xlist.OrderBy(x => x).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInterface != string.Empty)
            {
                masterTable = null;
                detailTable = null;
                if ((bool)chkMaster.IsChecked == true)
                {
                    var selectedChildTableItem = dgchildTableData.SelectedItem as UnicontaBaseEntity;
                    detailTable = selectedChildTableItem;
                    var selectedMasterTableItem = dgMasterTableData.SelectedItem as UnicontaBaseEntity;

                    detailTable = selectedMasterTableItem;
                    if (detailTable != null)
                    {
                        TestSelectedInterface();
                    }
                    else
                    {
                        MessageBox.Show("Please Select Master Table row");
                        return;
                    }
                }

                if ((bool)chkMstChild.IsChecked)
                {
                    var selectedMasterTableItem = dgMasterTableData.SelectedItem as UnicontaBaseEntity;
                    masterTable = selectedMasterTableItem;
                    var selectedChildTableItem = dgchildTableData.SelectedItem as UnicontaBaseEntity;
                    detailTable = selectedChildTableItem;

                    if (detailTable != null)
                    {
                        TestSelectedInterface();
                    }
                    else if (selectedInterface == "ICreditorPaymentFormatPlugin")
                    {
                        if (masterTable != null)
                        {
                            TestSelectedInterface();
                        }
                        else
                        {
                            MessageBox.Show("Please select Master Table row");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select Child Table row");
                        return;
                    }
                }
                else
                {
                    if (selectedInterface == "IContentPluginBase")
                        TestSelectedInterface();
                }
            }
            else { MessageBox.Show("Please select Interface"); }

        }

        private void TestSelectedInterface()
        {
            string errDesc = string.Empty;
            string res = string.Empty;

            // TODO: Change this to the direct path
            var pluginPath = @"C:\src\Uniconta\Technical-Training-Cases-master\Case1\ZendoPlugins\bin\Debug\ZendoPlugins.dll";
            var plugin = Plugin.LoadAssembly(pluginPath);

            if (plugin != null)
            {
                try
                {
                    if (selectedInterface == "IPluginBase")
                    {
                        // TODO: Change this to the right class
                        var type = plugin.GetTypeFromName("PluginLightingTemplate");
                        var pluginObj = Activator.CreateInstance(type) as IPluginBase;
                        var dependentAssemblies = pluginObj.GetDependentAssembliesName();
                        if (dependentAssemblies != null)
                            DependentAssemblies.AddDependentDllName(dependentAssemblies);
                        pluginObj.SetAPI(DemoInitializer.GetBaseAPI);
                        pluginObj.Intialize();
                        var result = pluginObj.Execute(masterTable, detailTable, entityCollection, "", "LIGHT-FLOORPANEL:4,LIGHT-MOVEHEAD:2,LIGHT-SPOTLIGHT:4");
                        res = result.ToString();
                        errDesc = pluginObj.GetErrorDescription();

                    }
                    else if (selectedInterface == "ICreditorPaymentFormatPlugin")
                    {
                        var type = plugin.GetTypeFromName("CreditorPaymentFormatPlugin"); // In PluginSample.dll CreditorPaymentFormatPlugin Class implements ICreditorPaymentFormatPlugin
                        var pluginObj = Activator.CreateInstance(type) as ICreditorPaymentFormatPlugin;
                        var dependentAssemblies = pluginObj.GetDependentAssembliesName();
                        if (dependentAssemblies != null)
                            DependentAssemblies.AddDependentDllName(dependentAssemblies);
                        pluginObj.SetAPI(DemoInitializer.GetBaseAPI);
                        pluginObj.Intialize();
                        var result = pluginObj.Execute(masterTable, detailTable, entityCollection, "CreditorPaymentFormatPluginTestCommand", "CreditorPaymentFormatPluginTestArgs");
                        res = result.ToString();
                        errDesc = pluginObj.GetErrorDescription();
                    }
                    else if (selectedInterface == "IContentPluginBase")
                    {
                        // TODO: Change this to the right class
                        var type = plugin.GetTypeFromName("PluginSpeakerCalculator"); // In PluginSample.dll PluginContent Class implements IContentPluginBase
                        var pluginObj = Activator.CreateInstance(type) as IContentPluginBase;
                        var dependentAssemblies = pluginObj.GetDependentAssembliesName();
                        if (dependentAssemblies != null)
                            DependentAssemblies.AddDependentDllName(dependentAssemblies);
                        pluginObj.SetAPI(DemoInitializer.GetBaseAPI);
                        pluginObj.Intialize();
                        pluginObj.SetContent(tab4);
                        pluginObj.OnPageClose();
                        tab4.IsSelected = true;
                        var result = pluginObj.Execute(null, null, null, "PluginContentTestCommand", "PluginContentTestArgs");
                        res = result.ToString();
                        errDesc = pluginObj.GetErrorDescription();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception");
                }
            }

            if (string.IsNullOrEmpty(errDesc))
                MessageBox.Show(res);
            else
            {
                var desc = string.Format("{0}:{1}", res.ToString(), errDesc);
                MessageBox.Show(desc);
            }
        }

        private void lbMasterDtl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbMasterDtl.SelectedItem == null) return;
            var selectedItem = lbMasterDtl.SelectedItem.ToString();
            if (selectedItem == "CreditorTransOpenClient")
            {
                detailTable = new CreditorTransOpenClient();
            }
            else
            {
                var selectedType = string.Empty;
                var endIndex = selectedItem.IndexOf('(') - 1;
                if (endIndex > 0)
                    selectedType = selectedItem.Substring(0, endIndex).Replace(" ", "");
                else
                    selectedType = selectedItem;

                Type type = (from l in childTablestype where l.Name.Split('.').Last() == selectedType select l).SingleOrDefault();
                if (type == null) return;

                var obj = Activator.CreateInstance(type);

                if (obj == null) return;

                detailTable = (UnicontaBaseEntity)obj;
            }
        }

        private void cmbCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seletedItem = e.AddedItems[0] as Uniconta.DataModel.Company;
            if (seletedItem != null)
            {
                SetCompany(seletedItem.CompanyId);
            }
        }

        private async void btnMstData_Click(object sender, RoutedEventArgs e)
        {
            if (masterTable != null)
            {
                busyIndicator.IsBusy = true;
                var api = DemoInitializer.GetBaseAPI;
                var collection = await api.Query(masterTable, null, null);
                if ((bool)chkMaster.IsChecked)
                    entityCollection = collection;
                dgMasterTableData.ItemsSource = collection;
                dgchildTableData.ItemsSource = null;
                tab2.IsSelected = true;
                busyIndicator.IsBusy = false;
            }
            else
            {
                MessageBox.Show("Please select Master Table");
            }
        }

        private async void btnchildData_Click(object sender, RoutedEventArgs e)
        {
            busyIndicator.IsBusy = true;
            var selectedMasterTableItem = dgMasterTableData.SelectedItem as UnicontaBaseEntity;
            var api = DemoInitializer.GetBaseAPI;

            if (selectedInterface == "ICreditorPaymentFormatPlugin" && detailTable != null)
            {
                var collection = await api.Query(detailTable, null, null);
                entityCollection = collection;
                dgchildTableData.ItemsSource = collection;
            }

            else if (selectedMasterTableItem != null && detailTable != null)
            {
                masterTable = selectedMasterTableItem;
                var masterRecords = new List<UnicontaBaseEntity>();
                masterRecords.Add(selectedMasterTableItem);
                var collection = await api.Query(detailTable, masterRecords, null);
                entityCollection = collection;
                dgchildTableData.ItemsSource = collection;
                tab3.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Please select Master and Child Table Table Row");
            }

            busyIndicator.IsBusy = false;
        }

    }
}
