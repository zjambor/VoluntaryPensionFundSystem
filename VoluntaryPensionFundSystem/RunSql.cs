using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace VoluntaryPensionFundSystem
{
    public partial class RunSql : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        string sql;
        //int darab = 0;
        BackgroundWorker bw = new BackgroundWorker();

        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\RunSql.log", "myListener");

        public RunSql(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
            label1.Visible = false;
        }

        private void bRun_Click(object sender, EventArgs e)
        {
            sql = rtSql.Text;
            tsExit.Enabled = false;
            label1.Visible = true;
            scommand = new SqlCommand(sql, sconn);
            bw.RunWorkerAsync();
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvResults.Visible = false;
            rtSql.Visible = true;
            dgvResults.DataSource = null;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bw_ProgressChanged(object source, ProgressChangedEventArgs e)
        {
            //rtbMessage.Text += e.ProgressPercentage;
            //rtbMessage.Refresh();
            //progressBar1.Value = e.ProgressPercentage;
            //progressBar1.Refresh();
        }

        private void bw_DoWork(object source, DoWorkEventArgs e)
        {
            //sql += "; PRINT @@ROWCOUNT;";
            scommand = new SqlCommand(sql, sconn);

            //SqlDataReader myReader = null;
            //int count = 0;
            //try
            //{
            //    if (sconn.State == ConnectionState.Closed) sconn.Open();
            //    myReader = scommand.ExecuteReader();
                
            //    if (myReader.HasRows)
            //    {
            //        while (myReader.Read())
            //        {
            //            count = myReader.FieldCount;
            //            for (int i = 0; i < count; i++)
            //            {
            //                result += myReader[i].ToString() + "\t";
            //            }
            //            result += "\n";
            //        }
            //    }
            //    myReader.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("SQL hiba: " + ex);
            //}

            try { da.Dispose(); }
            catch { } // első keresés

            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch
            {
                MessageBox.Show("SQL szintaxis hiba!");
            }
        }

        private void bw_RunWorkerCompleted(object source, RunWorkerCompletedEventArgs e)
        {
            dgvResults.DataSource = dt;
            rtSql.Visible = false;
            dgvResults.Visible = true;
            tsNew.Enabled = true;
            label1.Visible = false;
            //label1.Text = darab.ToString() + " sor lekérdezve.";
            tsExit.Enabled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    //SendKeys.Send("{TAB}");
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void dgvResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Szintaxis hiba");
        } 
    }
}