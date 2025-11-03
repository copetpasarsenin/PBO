using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4_1_714240047
{
    public class DVD_714240047 : Product_714240047
    {
        protected string duration;

        public DVD_714240047(string type, string title, string duratio)
        {
           MyType = "DVD";
            MyTitle = title;
            this.duration = duration;
            Console.WriteLine("ini dari class DVD");
        }

        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        // implementasi metode abstrak
        public override void DisplayInfo()
        {
            Console.WriteLine("Product is a {0} called \"{1}\" and has duration of {2} minutes", MyType, MyTitle, Duration);
        }
    }
}
