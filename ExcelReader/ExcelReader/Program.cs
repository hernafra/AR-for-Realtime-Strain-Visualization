using System;
using System.IO;
using System.Text;
using System.Data;

using ExcelDataReader;

namespace ExcelReader
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Console.WriteLine("Hello World! \n");
            string path = Directory.GetCurrentDirectory();
            string target = @"Screws1 Reduced Data.xlsx";
            Console.WriteLine("The current directory is {0} \n", path);

            string truepath = path + @"\" + target;
            Console.WriteLine("The truepath is: {0} \n", truepath);

            FileStream fs = File.Open(truepath, FileMode.Open, FileAccess.Read);

            IExcelDataReader exRead = ExcelReaderFactory.CreateOpenXmlReader(fs);

            DataSet result = exRead.AsDataSet();

            Console.WriteLine(result.Tables[0].TableName, "\n");
            
            foreach (DataRow row in result.Tables[0].Rows)
            {
                foreach (DataColumn col in result.Tables[0].Columns)
                {
                    Console.Write(row[col.ColumnName].ToString());
                    Console.Write(" | ");
                }
                Console.Write("\n");
            }

            exRead.Close();
        }
    }
}
