using Uniconta.DataModel;

namespace Case2.ZendoPlugins.DataModels
{
    public class Track : TableDataWithKey
    {
        // TODO: This might need to be changed
        public override int UserTableId { get { return 2051; } }

        public string Artist
        {
            get { return this.GetUserFieldString(0); }
            set { this.SetUserFieldString(0, value); }
        }

        public string Genre
        {
            get { return this.GetUserFieldString(1); }
            set { this.SetUserFieldString(1, value); }
        }

        public long Length
        {
            get { return this.GetUserFieldInt64(2); }
            set { this.SetUserFieldInt64(2, value); }
        }

        public bool LicensePaid
        {
            get { return this.GetUserFieldBoolean(3); }
            set { this.SetUserFieldBoolean(3, value); }
        }
    }
}
