using DependentAssembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Uniconta.API.Plugin;
using Uniconta.API.Service;
using Uniconta.Common;

namespace PluginSample
{
    //Summary
    // The class Plugin implements IPluginBase interface
    //Summary
    public class Plugin : IPluginBase
    {
        private string name;
        private BaseAPI baseApi;
        private string errorDescription;

        //summary
        // The Name method to get the name
        //summary
        public string Name
        {
            get
            {
                return name;
            }
        }

        //summary
        // The Initialize Event to initialize values
        //summary
        public void Intialize()
        {
            name = "SamplePlugin";
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
            //Dependent Assembly class object creation
            TestClass obj = new TestClass();

            MessageBox.Show(string.Format("Command:{0}, Args:{1},Result:Success Hello World",command, args));
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
            baseApi = api;
        }

        //summary
        // The SetMaster method fIor setting the master for the entity
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
            return null;
        }

        //summary
        // The OnExecute Event to perform some event
        //summary
        //returns event
        public event EventHandler OnExecute;
    }
}
