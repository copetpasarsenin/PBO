using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P9_714240047.lib
{
    internal class Excel
    {
        public void ExportToExcel(DataGridView dataGridView, string searchData)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Membuat sheet baru dengan nama Sheet1
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Export Headers (Baris pertama di Excel)
                for (int j = 1; j <= dataGridView.ColumnCount; j++)
                {
                    if (dataGridView.Columns[j - 1].HeaderText != null)
                    {
                        worksheet.Cells[1, j].Value = dataGridView.Columns[j - 1].HeaderText;
                    }
                }

                // Export Data (Dimulai dari baris kedua di Excel)
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView.Rows[i].Cells.Count; j++)
                    {
                        string cellValue = (dataGridView.Rows[i].Cells[j].Value != null ?
                                            dataGridView.Rows[i].Cells[j].Value.ToString() : "");

                        worksheet.Cells[i + 2, j + 1].Value = cellValue;
                    }
                }

                // Menyimpan file berdasarkan path yang dipilih (searchData)
                FileInfo excelFile = new FileInfo(searchData);
                excelPackage.SaveAs(excelFile);
            }
        }
    }
}