using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3_1_714240047
{
    class Program
    {
        static void Main(string[] args)
        {
            string grade;
            do
            {
                Console.Clear();

                Console.WriteLine("MENENTUKAN INDEKS PRESTASI MAHASISWA");
                Console.WriteLine("\nMasukan Nama Mahasiswa");
                
                String nama = Console.ReadLine();

                Console.Write("Masukkan Nilai : ");

                int nilai = Convert.ToInt16(Console.ReadLine());

                String[] grades = { "A", "B", "C", "D" };

                if (nilai >= 85)
                {
                    Console.WriteLine("Indeks nilai {0} adalah {1}", nama, grades[0]);
                }
                else if (nilai >= 70 && nilai < 85)
                {
                    Console.WriteLine("Indeks nilai {0} adalah {1}", nama, grades[1]);
                }
                else if (nilai >= 60 && nilai < 70)
                {
                    Console.WriteLine("Indeks nilai {0} adalah {1}", nama, grades[2]);
                }
                else
                {
                    Console.WriteLine("Indeks nilai {0} adalah {1}", nama, grades[3]);
                }

                Console.Write("Masukkan Indeks yang ditampilkan : ");
                char indeks = Convert.ToChar(Console.ReadLine());
                Console.WriteLine("Indeks prestasi {0} adalah ", nama);
                prestasi(indeks);

                Console.Write("\nIngin mengulang kembali (Y/T) : ");

            }
            while (Console.ReadLine() == "Y");

        }

        private static void prestasi(char indeks)
        {
            switch (indeks)
            {
                case 'A':
                    Console.WriteLine("sangat baik");
                    break;
                case 'B':
                    Console.WriteLine("baik");
                    break;
                case 'C':
                    Console.WriteLine("cukup");
                    break;
                case 'D':
                    Console.WriteLine("buruk");
                    break;
                default:
                    Console.WriteLine("Invalid indeks prestasi");
                    break;
            }
        }
    }
}