using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku_csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Easy");
            comboBox1.Items.Add("Medium");
            comboBox1.Items.Add("Hard");
            InitializeSudokuGrid();
        }

        private void InitializeSudokuGrid()
        {

            tableLayoutPanel1.ColumnCount = 9;
            tableLayoutPanel1.RowCount = 9;
            var a =  tableLayoutPanel1.ColumnStyles;


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = $"textBox{i}{j}";
                    textBox.Dock = DockStyle.Fill;
                    textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
                    textBox.TextAlign = HorizontalAlignment.Center;

                    tableLayoutPanel1.Controls.Add(textBox, j, i);
                }
            }
        }

        private void LoadSudokuFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length != 9)
            {
                MessageBox.Show("Invalid file format.");
                return;
            }

            for (int i = 0; i < 9; i++)
            {
                string line = lines[i];
                if (line.Length != 9)
                {
                    MessageBox.Show("Invalid file format.");
                    return;
                }

                for (int j = 0; j < 9; j++)
                {
                    char c = line[j];
                    TextBox textBox = tableLayoutPanel1.Controls[$"textBox{i}{j}"] as TextBox;
                    textBox.Text = c == '0' ? "" : c.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string difficulty = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(difficulty))
            {
                MessageBox.Show("Please select a difficulty level.");
                return;
            }

            string filePath = null;
            switch (difficulty.ToLower())
            {
                case "easy":
                    filePath = @"C:\Users\1969-23\Documents\sudoku_csharp\sudoku_csharp\easy.txt";
                    break;
                case "medium":
                    filePath = @"C:\Users\1969-23\Documents\sudoku_csharp\sudoku_csharp\medium.txt";
                    break;
                case "hard":
                    filePath = @"C:\Users\1969-23\Documents\sudoku_csharp\sudoku_csharp\hard.txt";
                    break;
            }

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                LoadSudokuFromFile(filePath);
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
