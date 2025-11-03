using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4_1_714240047
{
    internal class ProductTest_714240047
    {
        static void Main(string[] args)
        {
            Book_714240047 product1 = new Book_714240047("Book", "C# Object Oriented Solution", "300");
            DVD_714240047 product2 = new DVD_714240047("DVD","Ethernal Sunshine Of The Spotless Mind", "145");

            product1.DisplayInfo();
            product2.DisplayInfo();
        }
    }
}
