using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Uniconta.API.Plugin;
using Uniconta.API.Service;
using Uniconta.ClientTools.DataModel;
using Uniconta.ClientTools.Util;
using Uniconta.Common;
using Uniconta.DataModel;

namespace PluginSample
{
    public class CreditorPaymentFormatPlugin : ICreditorPaymentFormatPlugin
    {
        private string name;
        private BaseAPI baseApi;
       
        //summary
        // The Name method to get the name
        //summary
        public string Name
        {
            get { return "Remittering"; }
        }

        public FileextensionsTypes FileExtension
        {
            get
            {
                return FileextensionsTypes.TXT;
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
            ErrorCodes err = ErrorCodes.CouldNotCreate;

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = UtilFunctions.GetFilteredExtensions(FileExtension)
            };

            bool? userClickedSave = sfd.ShowDialog();
            if (userClickedSave == true)
            {
                try
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        err = Generate(source.Cast<CreditorTransOpenClient>().ToList() , (CreditorPaymentFormat)master, stream);
                        
                        if (err != ErrorCodes.Succes)
                        {
                            var error = GetErrorDescription();
                            MessageBox.Show(error);
                        }
                        stream.Close();

                        return err;
                    }
                }
                catch (Exception ex)
                {
                    exceptionMsg = ex.Message; 
                    return err;
                }
            }

            return err;
        }


        //summary
        // The GetDescription method to get the error description
        //summary
        //Returns string
        public string GetErrorDescription()
        {
            return exceptionMsg;
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
        // The SetMaster method for setting the master for the entity
        //summary
        //Params List<UnicontaBaseEntity> masters :- pass the master
        public void SetMaster(List<UnicontaBaseEntity> masters)
        {

        }

        //summary
        // The OnExecute Event to perform some event
        //summary
        //returns event
        public event EventHandler OnExecute;

        string exceptionMsg;

        public ErrorCodes Generate(List<CreditorTransOpenClient> transactions, CreditorPaymentFormat paymentSetup, Stream stream)
        {
            try
            {
                generateFile(transactions, paymentSetup, stream);
                return ErrorCodes.Succes;
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return ErrorCodes.Exception;
            }
        }

        public void generateFile(List<CreditorTransOpenClient> Trans, CreditorPaymentFormat setup, Stream stream)
        {
            // Write Logic here
        }

        ////summary
        //// The GetDependentAssembliesName method to get dependent assembly names(e.g AssemblyName.dll and AssemblyName2.dll) used by SamplePlugin.
        ////summary
        public string[] GetDependentAssembliesName()
        {
            return new string[] { "DependentAssembly.dll" };
        }
    }
}
