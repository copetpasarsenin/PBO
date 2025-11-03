namespace P4_1_714240047
{
    public abstract class Product_714240047
    {
        private string myType;
        private string myTitle;
        public Product_714240047()
        {
          
        }
        // Constructor
        public Product_714240047(string type, string title)
        {
            myType = type;
            myTitle = title;
        }

        // properti untuk MyType
        public string MyType
        {
            get { return myType; }
            set { myType = value; }
        }

        // properti untuk MyTitle
        public string MyTitle
        {
            get { return myTitle; }
            set { myTitle = value; }
        }

        // metode abstrak untuk menampilkan informasi product
        public abstract void DisplayInfo();
    }
}