using Uniconta.DataModel;

namespace TableCreator.DataModels
{
    public class MyNewTable : TableDataWithKey
    {
        public override int UserDefinedId { get { return 2750; } }

        public string MyStringField
        {
            get { return this.GetUserFieldString(0); }
            set { this.SetUserFieldString(0, value); }
        }

        public bool MyBooleanField
        {
            get { return this.GetUserFieldBoolean(1); }
            set { this.SetUserFieldBoolean(1, value); }
        }
    }
}
