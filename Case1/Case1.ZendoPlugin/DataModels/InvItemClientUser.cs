using Uniconta.ClientTools.DataModel;

namespace Case1.ZendoPlugins.DataModels
{
    public class InvItemClientUser : InvItemClient
    {
        public string BarcodeNumber
        {
            get { return this.GetUserFieldString(0); }
            set { this.SetUserFieldString(0, value); }
        }

        public long SpeakerScore
        {
            get { return this.GetUserFieldInt64(1); }
            set { this.SetUserFieldInt64(1, value); }
        }
    }
}
