using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rechner
{
    public partial class Form1 : Form
    {
        // Array für die Eingaben
        private List<string> inputArray = new List<string>();

        public Form1()
        {
            InitializeComponent();
            // Event-Handler für alle relevanten Buttons
            button0.Click += Button_Click;
            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            button10.Click += Button_Click;
            button12.Click += Button_Click;
            button14.Click += Button_Click;
            button15.Click += Button_Click;
            button16.Click += Button_Click;
            button17.Click += Button_Click;
            // = Button
            button13.Click += ButtonEquals_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                inputArray.Add(btn.Text);
                UpdateTextBox();
            }
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            string expression = string.Join("", inputArray);
            try
            {
                // Einfache Auswertung mit DataTable
                var result = new System.Data.DataTable().Compute(expression.Replace("%", "/100"), null);
                richTextBox1.Text = result.ToString();
                inputArray.Clear();
                inputArray.Add(result.ToString());
            }
            catch
            {
                richTextBox1.Text = "Fehler";
                inputArray.Clear();
            }
        }

        private void UpdateTextBox()
        {
            richTextBox1.Text = string.Join(" ", inputArray);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {

        }
    }
}
