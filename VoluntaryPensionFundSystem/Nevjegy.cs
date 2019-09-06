using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VoluntaryPensionFundSystem
{
    public partial class Nevjegy : VoluntaryPensionFundSystem.BaseForm
    {
        public Nevjegy()
        {
            InitializeComponent();

            label1.Text = "Program neve: Önkéntes Nyugdíjpénztári Információs Rendszer";
            label2.Text = "Készítette: Jámbor Zoltán";
            label3.Text = "Szak: Programtervező Informatikus";
            label4.Text = "Évszám: 2014 - 2015";

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
