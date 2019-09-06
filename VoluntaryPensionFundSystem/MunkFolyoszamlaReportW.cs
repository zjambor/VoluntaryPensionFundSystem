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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace VoluntaryPensionFundSystem
{
    public partial class MunkFolyoszamlaReportW : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        //private SqlCommand scommand;

        public string pnrid, kezdete, vege;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\TagiEgyeniSzamlaReport.log", "myListener");

        public MunkFolyoszamlaReportW(SqlConnection SqlConn)
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
        }

        private void MunkFolyoszamlaReportW_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'VPFSDataSet.spMunkEgyenlegFej' table. You can move, or remove it, as needed.
            this.spMunkEgyenlegFejTableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegFej, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            // TODO: This line of code loads data into the 'VPFSDataSet.spMunkEgyenlegFej2' table. You can move, or remove it, as needed.
            this.spMunkEgyenlegFej2TableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegFej2, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            // TODO: This line of code loads data into the 'VPFSDataSet.spMunkEgyenlegFej3' table. You can move, or remove it, as needed.
            this.spMunkEgyenlegFej3TableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegFej3, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            // TODO: This line of code loads data into the 'VPFSDataSet.spMunkEgyenlegTetelekBev' table. You can move, or remove it, as needed.
            this.spMunkEgyenlegTetelekBevTableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegTetelekBev, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));

            this.spMunkEgyenlegTetelekBefTableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegTetelekBef, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));

            this.spMunkEgyenlegFejBefTableAdapter.Fill(this.VPFSDataSet.spMunkEgyenlegFejBef, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));

            this.reportViewer1.RefreshReport();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
