using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace VoluntaryPensionFundSystem
{
    public partial class BaseForm : Form
    {
        private PrintDocument printDocument1 = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        List<Form> openForms = new List<Form>();
        Bitmap memoryImage;

        public BaseForm()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        public void BaseForm_Load(object sender, EventArgs e)
        {

        }

        public virtual void tsNew_Click(object sender, EventArgs e)
        { 
        }

        public virtual void tsSave_Click(object sender, EventArgs e)
        {
        }

        public virtual void tsUpdate_Click(object sender, EventArgs e)
        {
        }

        public virtual void tsSearch_Click(object sender, EventArgs e)
        {
        }

        public virtual void tsFind_Click(object sender, EventArgs e)
        {
        }

        public virtual void tsDelete_Click(object sender, EventArgs e)
        {
        }

        public virtual void tsExit_Click(object sender, EventArgs e)
        {           
        }

        public void bezárToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void mindentBezárToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);

            foreach (Form f in openForms)
            {
                if (f.Name != "Foablak")
                    f.Close();
            }
        }

        public void kilépésToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void nyomtatásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;
            try
            {
                printDocument1.Print();
            }
            catch
            {
                MessageBox.Show("A nyomtatási várólista nem érhető el.");
            }
        }

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Control nextControl;
            ////Checks if the Enter Key was Pressed
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //If so, it gets the next control and applies the focus to it
            //    nextControl = GetNextControl(ActiveControl, !e.Shift);
            //    if (nextControl == null)
            //    {
            //        nextControl = GetNextControl(null, true);
            //    }
            //    nextControl.Focus();
            //    //Finally - it suppresses the Enter Key
            //    e.SuppressKeyPress = true;
            //}
            if (e.KeyCode == Keys.F7)
            {
                if (tsSearch.Enabled)
                {
                    yellowMode();
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.F8)
            {
                if (tsFind.Enabled)
                {
                    runQuery();
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.F4)
            {
                if (tsNew.Enabled)
                {
                    createNew();
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.F10)
            {
                if (tsSave.Enabled)
                {
                    save();
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.F5)
            {
                if (tsDelete.Enabled)
                {
                    delete();
                    e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.F9)
            {
                if (tsUpdate.Enabled)
                {
                    update();
                    e.SuppressKeyPress = true;
                }
            }
        }

        public virtual void yellowMode()
        { }

        public virtual void runQuery()
        { }

        public virtual void createNew()
        { }

        public virtual void save()
        { }

        public virtual void delete()
        { }

        public virtual void update()
        { }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:                    
                    SendKeys.Send("{TAB}");
                    e.SuppressKeyPress = true;
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        private void nyomtatásiKépToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;

            printPreviewDialog1.Document = printDocument1;
            try
            {
                printPreviewDialog1.ShowDialog();
            }
            catch
            {
                MessageBox.Show("A nyomtatási várólista nem érhető el.");
            }
        }

        private void névjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nevjegy nj = new Nevjegy();
            nj.Show();
        }
    }
}
