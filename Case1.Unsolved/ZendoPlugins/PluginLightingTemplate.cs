using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniconta.API.Plugin;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.ClientTools.DataModel;
using Uniconta.Common;

namespace ZendoPlugins
{
    public class PluginLightingTemplate : IPluginBase
    {
        // Fields
        private CrudAPI crudAPI;
        private string errorDescription;

        // Properties
        //summary
        // The Name method to get the name
        //summary
        public string Name { get { return "Zendo - LightingTemplate"; } }

        //summary
        // The Initialize Event to initialize values
        //summary
        public void Intialize()
        {
        }

        ////summary
        //// The Execute method to execute on the basis of parameter passed
        ////summary
        ////Params UnicontaBaseEntity master :- To pass the master table
        ////Params UnicontaBaseEntity currentRow :- To pass the current row
        ////Params IEnumerable<UnicontaBaseEntity> source :- To pass List of UnicontaBaseEntity
        ////Params String Command :- pass the command
        ////Params String args :- pass the argument
        ////Returns ErrorCodes
        public ErrorCodes Execute(UnicontaBaseEntity master, UnicontaBaseEntity currentRow, IEnumerable<UnicontaBaseEntity> source, string command, string args)
        {
            if (currentRow == null || !(currentRow is DebtorOrderClient))
            {
                MessageBox.Show("ERROR: No row or wrong type");
                return ErrorCodes.Exception;
            }

            // Parse currentRow
            var order = currentRow as DebtorOrderClient;
            
            // Parse args
            var itemList = args.Split(',').Select(x => x.Split(':')).ToList();

            // Getting Items
            var itemFilter = "";
            itemList.ForEach(item => itemFilter += $"{item[0]};");

            // TODO: Add query and filter to get items, Use the KeyName Field

            // Creating new order lines
            // TODO: Create DebtorOrderLines, Remember to set master

            // Call insert API
            // TODO: Call Insert API

            // Attacting UserDocument to order
            // TODO: Add User Document to Order. Use @"C:\src\Uniconta\Technical-Training-Cases-master\TrainingData\LIGHT-FLOORPANEL.jpg"

            // Calling insert API
            // TODO: Call Insert API

            // Writes file to disk
            // TODO: Read and then write the file to the Disk

            return ErrorCodes.Succes;
        }
        
        //summary
        // The GetDescription method to get the error description
        //summary
        //Returns string
        public string GetErrorDescription()
        {
            return errorDescription;
        }
        
        //summary
        // The SetAPI method to set the api for query database
        //summary
        //Params BaseAPI api :- pass the api
        public void SetAPI(BaseAPI api)
        {
            crudAPI = api as CrudAPI;
        }

        //summary
        // The SetMaster method for setting the master for the entity
        //summary
        //Params List<UnicontaBaseEntity> masters :- pass the master
        public void SetMaster(List<UnicontaBaseEntity> masters)
        {

        }

        ////summary
        //// The GetDependentAssembliesName method to get dependent assembly names(e.g AssemblyName.dll and AssemblyName2.dll) used by SamplePlugin.
        ////summary
        public string[] GetDependentAssembliesName()
        {
            return new string[] { };
        }

        //summary
        // The OnExecute Event to perform some event
        //summary
        //returns event
        public event EventHandler OnExecute;
    }
}
