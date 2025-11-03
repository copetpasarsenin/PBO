using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4_1_714240047
{
    public class Book_714240047 : Product_714240047
    {
        protected string pageCount;

        public Book_714240047(string type, string title, string pagecount) : base(type, title)
        {
            this.pageCount = pagecount;
        }
        public string PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }
        // implementasi metode abstrak
        public override void DisplayInfo()
        {
            Console.WriteLine("Product is a {0} called \"{1}\" and has {2} pages", MyType, MyTitle, PageCount);
        }
    }
}
