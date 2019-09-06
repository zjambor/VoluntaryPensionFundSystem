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
    public partial class TagiEgyeniSzamlaReport : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;

        public string pnrid, kezdete, vege;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\TagiEgyeniSzamlaReport.log", "myListener");

        public TagiEgyeniSzamlaReport(SqlConnection SqlConn)
        {
            InitializeComponent();

            // connectionstring
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Vpfs"];
            if (settings == null) return;

            // sql kapcsolat
            sconn = new SqlConnection(settings.ConnectionString);

            //this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TagiEgyeniSzamlaReport_Load(object sender, EventArgs e)
        {
            this.spTagiEgyenlegTetelekTableAdapter.Fill(this.VPFSDataSet.spTagiEgyenlegTetelek, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            this.spTagiEgyenlegFejTableAdapter.Fill(this.VPFSDataSet.spTagiEgyenlegFej, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            this.spTagiEgyenlegFej2TableAdapter1.Fill(this.VPFSDataSet.spTagiEgyenlegFej2, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            this.spTagiEgyenlegFej3TableAdapter.Fill(this.VPFSDataSet.spTagiEgyenlegFej3, int.Parse(pnrid), DateTime.Parse(kezdete), DateTime.Parse(vege));
            this.reportViewer1.RefreshReport();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }
    }
}
