using Seq2SeqLearn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleTranslator
{
    public partial class SimpleTranslator : Form
    {
        public SimpleTranslator()
        {
            InitializeComponent();
        }

        AttentionSeq2Seq ss;
        Thread mainThread;

        delegate void ProgressDeleget(CostEventArg e);
    

        void ss_IterationDone(object sender, EventArgs e)
        {

            CostEventArg ep = e as CostEventArg;

            this.Invoke(new ProgressDeleget(Progress), ep);
        }
        public void Progress(CostEventArg cost)
        {
            label5.Text = cost.Iteration.ToString(); 
            label3.Text = cost.Cost.ToString();
            label2.Text = DateTime.Now.ToLongTimeString();
        }

        private void Train()
        {
            ss.Train(300);

            ss.Save();
        }

        private void Train_Click(object sender, EventArgs e)
        {
            TrainButton.Enabled = false;
            StopTrainingButton.Enabled = true;
            label1.Text = DateTime.Now.ToLongTimeString();

            this.PredictButton.Enabled = false;
            ResultTxtBox.Enabled = false;
            SrcTxtBox.Enabled = false;

            mainThread = new Thread(new ThreadStart(Train));
            mainThread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            this.TrainButton.Enabled = true; 
            ss.Load();
            this.PredictButton.Enabled = true;
            ResultTxtBox.Enabled = true;
            SrcTxtBox.Enabled = true;
        }

        private void Stop_Click(object sender, EventArgs e)
        {

            if (mainThread != null)
                mainThread.Abort();
            ss.Save();
            this.PredictButton.Enabled = true;
            ResultTxtBox.Enabled = true;
            SrcTxtBox.Enabled = true;
            StopTrainingButton.Enabled = false;
            TrainButton.Enabled = true;
        }

        private void Predict_Click(object sender, EventArgs e)
        {
            var pred = ss.Predict(SrcTxtBox.Text.ToLower().Trim().Split(' ').ToList());
            ResultTxtBox.Clear();
             
            int i = 0;
            foreach (var item in pred)
            {
                ResultTxtBox.Text += item + " ";
                 
                i++;
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {


            var data_sents_raw1 = File.ReadAllLines("en.txt");
            var data_sents_raw2 = File.ReadAllLines("ar.txt");

            List<List<string>> input = new List<List<string>>();
            List<List<string>> output = new List<List<string>>();
            for (int i = 0; i < data_sents_raw1.Length; i++)
            {
                input.Add(data_sents_raw1[i].ToLower().Trim().Split(' ').ToList());
                output.Add(data_sents_raw2[i].ToLower().Trim().Split(' ').ToList());
            }


            ss = new AttentionSeq2Seq(64, 32, 1, input, output, true);

            ss.IterationDone += ss_IterationDone;
            this.TrainButton.Enabled = true; 
        }
    }
}
