using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
//using NsExcel = Microsoft.Office.Interop.Excel;

namespace projet_math_winform
{
    public partial class Form1 : Form
    {
        double m_n = 0.0;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e){}

        private void listViewE_SelectedIndexChanged(object sender, EventArgs e){}

        private void button1_Click(object sender, EventArgs e)
        {
            calculate(400);
        }

        private void Form1_Load(object sender, EventArgs e){calculate_n();}

        private void calculate_n()
        {
            if (double.TryParse(textBoxtf.Text.Replace('.', ','), out m_n) && double.TryParse(textBoxdt.Text.Replace('.', ','), out m_n))
            {
                m_n = (double.Parse(textBoxtf.Text.Replace('.', ',')) - 0) / double.Parse(textBoxdt.Text.Replace('.', ','));
            }
        }

        private void textBoxdt_TextChanged(object sender, EventArgs e)
        {calculate_n();}

        private void textBoxtf_TextChanged(object sender, EventArgs e)
        {calculate_n();}

        private List<double> calculate(double Pmax)
        {
            listViewE.Items.Clear();  // clear listview
            double t0 = 0.0;
            double tf = double.Parse(textBoxtf.Text.Replace('.', ','));
            double c = double.Parse(textBoxc.Text.Replace('.', ','));
            double m = double.Parse(textBoxm.Text.Replace('.', ','));  // 0.001
            double dt = double.Parse(textBoxdt.Text.Replace('.', ','));
            double To = double.Parse(textBox1.Text.Replace('.', ','));
            double P0 = double.Parse(textBox2.Text.Replace('.', ','));

            double v;  // v (i-1)
            double vv;  // v (i)
            double vv2;  // v (i) RK2
            double k1;  // K1 RK2
            double k2;  // K2 RK2
            double v1;  // V(i) + K1  // RK2
            double t;
            double x;  // x (i-1)
            double xx = 0.0; // x (i)
            double xx2 = 0.0; // x (i)  RK2
            double p = P0;
            double v0 = 0;
            List<double> tabV = new List<double> { v0 };
            List<double> tabV2 = new List<double> { v0 };  // RK2
            double x0 = 0.0;
            List<double> tabx = new List<double> { x0, x0 };
            List<double> tabx2 = new List<double> { x0, x0 };  // RK2

            ListViewItem item = new ListViewItem(new string[] { t0.ToString(), v0.ToString(), "0", p.ToString(), v0.ToString(), "0" });
            listViewE.Items.Add(item);
            for (int i = 1; i <= m_n; i++)
            {
                v = tabV[i - 1];
                t = t0 + i * dt;
                if (i > 1) p = 0;

                // Euler
                // V :
                vv = v + dt * ((c * v * v / m) + (p / m));
                tabV.Add(vv);

                // x :
                if (i > 1)
                {
                    x = tabx[i - 1];
                    xx = x + dt * vv;
                    tabx.Add(xx);
                }

                // RK2
                // V :
                v = tabV2[i - 1];
                k1 = dt * ((c * v * v / m) + (p / m));
                v1 = v + k1;
                k2 = dt * ((c * v1 * v1 / m) + (p / m));

                vv2 = v + (k1 + k2) / 2;
                // x :
                if (i > 1)  
                {
                    x = tabx[i - 1];
                    xx2 = x + dt * vv2;
                    tabx2.Add(xx2);
                }
                tabV2.Add(vv2);

                // Affichage

                item = new ListViewItem(new string[] { t.ToString(), vv.ToString(), xx.ToString(), p.ToString(), vv2.ToString(), xx2.ToString() });
                listViewE.Items.Add(item);
                export(tabV, "results_EULER.csv");
                export(tabV2, "results_RK2.csv");
                export(tabx, "results_X_EULER.csv");
                export(tabx2, "results_X_RK2.csv");
            }
            return tabV2;
        }

        private void export(List<double> list, string filename)
        {
            using (StreamWriter sw = File.CreateText(filename))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    sw.WriteLine(list[i]);
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        /*
public void ListToExcel(List<string> list)
//  https://stackoverflow.com/questions/2206279/exporting-the-values-in-list-to-excel
{
   //start excel
   NsExcel.ApplicationClass excapp = new Microsoft.Office.Interop.Excel.ApplicationClass();

   //if you want to make excel visible           
   excapp.Visible = true;

   //create a blank workbook
   var workbook = excapp.Workbooks.Add(NsExcel.XlWBATemplate.xlWBATWorksheet);

   //or open one - this is no pleasant, but yue're probably interested in the first parameter
   string workbookPath = "C:\test.xls";
   var workbook = excapp.Workbooks.Open(workbookPath,
       0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
       true, false, 0, true, false, false);

   //Not done yet. You have to work on a specific sheet - note the cast
   //You may not have any sheets at all. Then you have to add one with NsExcel.Worksheet.Add()
   var sheet = (NsExcel.Worksheet)workbook.Sheets[1]; //indexing starts from 1

   //do something usefull: you select now an individual cell
   var range = sheet.get_Range("A1", "A1");
   range.Value2 = "test"; //Value2 is not a typo

   //now the list
   string cellName;
   int counter = 1;
   foreach (var item in list)
   {
       cellName = "A" + counter.ToString();
       var range_ = sheet.get_Range(cellName, cellName);
       range_.Value2 = item.ToString();
       ++counter;
   }

   //you've probably got the point by now, so a detailed explanation about workbook.SaveAs and workbook.Close is not necessary
   //important: if you did not make excel visible terminating your application will terminate excel as well - I tested it
   //but if you did it - to be honest - I don't know how to close the main excel window - maybee somewhere around excapp.Windows or excapp.ActiveWindow
}
*/

        // private void button2_Click(object sender, EventArgs e){}

        /*private void button2_Click_1(object sender, EventArgs e)
        {
            List<double> list;
            for(int i = 0; i < 10; i++)
            {
                list = calculate(i * 4);
                if()
            }
        }*/
    }
}
