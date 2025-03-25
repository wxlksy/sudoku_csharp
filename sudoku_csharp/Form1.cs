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
        private Timer saveTimer;
        private List<TextBox> initialCells = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Easy");
            comboBox1.Items.Add("Medium");
            comboBox1.Items.Add("Hard");
            InitializeSudokuGrid();
            LoadSudokuState();

            saveTimer = new Timer();
            saveTimer.Interval = 60000; // 1 minute
            saveTimer.Tick += SaveTimer_Tick;
            saveTimer.Start();
        }

        private void InitializeSudokuGrid()
        {
            tableLayoutPanel1.ColumnCount = 9;
            tableLayoutPanel1.RowCount = 9;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = $"textBox{i}{j}";
                    textBox.Dock = DockStyle.Fill;
                    textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
                    textBox.TextAlign = HorizontalAlignment.Center;
                    textBox.TextChanged += TextBox_TextChanged;

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

                    if (c != '0')
                    {
                        textBox.ReadOnly = true;
                        textBox.BackColor = Color.LightGray;
                        initialCells.Add(textBox);
                    }
                    else
                    {
                        textBox.ReadOnly = false;
                        textBox.BackColor = Color.White;
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateSudoku();
        }

        private void ValidateSudoku()
        {
            bool isValid = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = tableLayoutPanel1.Controls[$"textBox{i}{j}"] as TextBox;
                    if (textBox.ReadOnly) continue;

                    if (!IsValidInRow(i, j, textBox.Text) || !IsValidInColumn(i, j, textBox.Text) || !IsValidInBox(i, j, textBox.Text))
                    {
                        textBox.BackColor = Color.Red;
                        isValid = false;
                    }
                    else
                    {
                        textBox.BackColor = Color.White;
                    }
                }
            }

            if (isValid && IsSudokuComplete())
            {
                MessageBox.Show("Sudoku solved correctly!");
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is TextBox tb)
                    {
                        tb.BackColor = Color.Green;
                    }
                }
            }
        }

        private bool IsValidInRow(int row, int col, string value)
        {
            if (string.IsNullOrEmpty(value)) return true;

            for (int j = 0; j < 9; j++)
            {
                if (j == col) continue;
                TextBox textBox = tableLayoutPanel1.Controls[$"textBox{row}{j}"] as TextBox;
                if (textBox.Text == value) return false;
            }
            return true;
        }

        private bool IsValidInColumn(int row, int col, string value)
        {
            if (string.IsNullOrEmpty(value)) return true;

            for (int i = 0; i < 9; i++)
            {
                if (i == row) continue;
                TextBox textBox = tableLayoutPanel1.Controls[$"textBox{i}{col}"] as TextBox;
                if (textBox.Text == value) return false;
            }
            return true;
        }

        private bool IsValidInBox(int row, int col, string value)
        {
            if (string.IsNullOrEmpty(value)) return true;

            int boxStartRow = (row / 3) * 3;
            int boxStartCol = (col / 3) * 3;

            for (int i = boxStartRow; i < boxStartRow + 3; i++)
            {
                for (int j = boxStartCol; j < boxStartCol + 3; j++)
                {
                    if (i == row && j == col) continue;
                    TextBox textBox = tableLayoutPanel1.Controls[$"textBox{i}{j}"] as TextBox;
                    if (textBox.Text == value) return false;
                }
            }
            return true;
        }

        private bool IsSudokuComplete()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is TextBox tb && string.IsNullOrEmpty(tb.Text))
                {
                    return false;
                }
            }
            return true;
        }

        private void SaveSudokuState()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = tableLayoutPanel1.Controls[$"textBox{i}{j}"] as TextBox;
                    sb.Append(string.IsNullOrEmpty(textBox.Text) ? "0" : textBox.Text);
                }
                sb.AppendLine();
            }
            File.WriteAllText("sudoku_state.txt", sb.ToString());
        }

        private void LoadSudokuState()
        {
            string filePath = "sudoku_state.txt";
            if (File.Exists(filePath))
            {
                LoadSudokuFromFile(filePath);
            }
        }

        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            SaveSudokuState();
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
