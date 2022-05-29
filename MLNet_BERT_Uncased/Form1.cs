using BERTTokenizers;
using MLNet_BERT_Uncased.DataStructures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLNet_BERT_Uncased
{
    public partial class Form1 : Form
    {
        BERT model;
        string vocabPath = @"C:\Users\GF63\.nuget\packages\berttokenizers\1.1.0\contentFiles\any\net5.0\Vocabularies\base_uncased_large.txt";
        string modelPath = @"C:\Users\GF63\source\repos\MLNet_BERT_Uncased\MLNet_BERT_Uncased\Assets\Models\bert-base-uncased-squad.onnx";
        public Form1()
        {
            model = new BERT(vocabPath, modelPath);

            InitializeComponent();
        }

        private void btnAnswer_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtContext.Text))
                MessageBox.Show("You must enter a context!");
            else if (String.IsNullOrEmpty(txtQuestion.Text))
                MessageBox.Show("You must enter a question!");
            else
            {
                string output = "";
                foreach (string s in model.Predict(txtContext.Text, txtQuestion.Text))
                {
                    output += s + " ";
                }
                txtAnswer.Text = output;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtContext.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
