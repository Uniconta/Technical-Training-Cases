using Case2.ZendoPlugins.DataModels;
using Case2.ZendoPlugins.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Uniconta.API.Plugin;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.Common;

namespace Case2.ZendoPlugins
{
    public class PluginLastFMImporter : IPluginBase
    {
        // Fields
        private CrudAPI crudAPI;
        private LastFMAPI lastFMAPI;
        private string errorDescription;

        // Properties
        //summary
        // The Name method to get the name
        //summary
        public string Name { get { return "Zendo - LastFMImporter"; } }

        //summary
        // The Initialize Event to initialize values
        //summary
        public void Intialize()
        {
            this.lastFMAPI = new LastFMAPI(this.crudAPI);
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
            // Getting tracks from LastFM
            var tracks = this.lastFMAPI.GetTop50();
            this.AddTracks(tracks);
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
            this.crudAPI = new CrudAPI(api);
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
            // Include Newtonsoft.Json.dll, should be placed in "C:\Uniconta\PluginPath"
            return new string[] { @"Newtonsoft.Json.dll" };
        }

        //summary
        // The OnExecute Event to perform some event
        //summary
        //returns event
        public event EventHandler OnExecute;

        #region Private Methods
        private async void AddTracks(List<LastFMTrack> tracks)
        {
            var newTracks = new List<Track>();

            foreach (var track in tracks)
            {
                // Check if track already exists
                var filter = new List<PropValuePair>
                {
                    PropValuePair.GenereteWhereElements("Title", typeof(string), track.Name),
                    PropValuePair.GenereteWhereElements("Artist", typeof(string), track.Artist.Name)
                };
                var apiTracks = await this.crudAPI.Query<Track>(filter);

                if (apiTracks.Length == 0)
                {
                    var trackData = track;
                    
                    // Getting more detailed track info
                    var trackInfo = this.lastFMAPI.GetTrackInfo(track);
                    if (trackInfo != null)
                        trackData = trackInfo;

                    // Finalizing data
                    string trackGenre = "Unknown";
                    if(trackData.TopTags != null && trackData.TopTags.Tag.Count != 0)
                    {
                        var genre = trackData.TopTags.Tag[0].Name;
                        
                        // Checking if genre exists, and creates it if it dosnt
                        var apiTrackGenre = await this.FindGenre(genre);
                        if (apiTrackGenre == null)
                            apiTrackGenre = await this.AddGenre(genre);

                        trackGenre = apiTrackGenre._KeyStr;
                    }

                    int trackLentgh = (trackData.Duration / 1000);

                    // Adding new track to list
                    newTracks.Add(new Track
                    {
                        KeyName = trackData.Name,
                        Artist = trackData.Artist.Name,
                        Genre = trackGenre,
                        Length = trackLentgh,
                        LicensePaid = false
                    });
                }
            }

            // Call Insert API
            var errorCode = await crudAPI.Insert(newTracks);
            if(errorCode == ErrorCodes.Succes)
            {
                MessageBox.Show($"{newTracks.Count} new tracks was added.");
            }
        }

        private async Task<TrackGenre> FindGenre(string genre)
        {
            // Call Query API
            var filter = new List<PropValuePair>
            {
                PropValuePair.GenereteWhereElements("KeyName", typeof(string), genre)
            };
            var trackGenres = await this.crudAPI.Query<TrackGenre>(filter);

            // Returns result
            if(trackGenres.Length != 0)
                return trackGenres[0];
            else
                return null;
        }
        
        private async Task<TrackGenre> AddGenre(string genre)
        {
            // Creating new Track
            var newTrackGenre = new TrackGenre
            {
                KeyName = genre
            };

            // Call Insert API
            var errorCode = await this.crudAPI.Insert(newTrackGenre);
            if (errorCode == ErrorCodes.Succes)
                return newTrackGenre;
            else
                return null;
        }
        #endregion
    }
}
