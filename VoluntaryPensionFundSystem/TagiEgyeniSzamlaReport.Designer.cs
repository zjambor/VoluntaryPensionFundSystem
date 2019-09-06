namespace VoluntaryPensionFundSystem
{
    partial class TagiEgyeniSzamlaReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.spTagiEgyenlegFejBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.VPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spTagiEgyenlegTetelekBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spTagiEgyenlegFej2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spTagiEgyenlegFej3BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.spTagiEgyenlegFejTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spTagiEgyenlegFejTableAdapter();
            this.spTagiEgyenlegTetelekTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spTagiEgyenlegTetelekTableAdapter();
            this.spTagiEgyenlegFej2TableAdapter1 = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spTagiEgyenlegFej2TableAdapter();
            this.spTagiEgyenlegFej3TableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spTagiEgyenlegFej3TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFejBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPFSDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegTetelekBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFej2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFej3BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // spTagiEgyenlegFejBindingSource
            // 
            this.spTagiEgyenlegFejBindingSource.DataMember = "spTagiEgyenlegFej";
            this.spTagiEgyenlegFejBindingSource.DataSource = this.VPFSDataSet;
            // 
            // VPFSDataSet
            // 
            this.VPFSDataSet.DataSetName = "VPFSDataSet";
            this.VPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spTagiEgyenlegTetelekBindingSource
            // 
            this.spTagiEgyenlegTetelekBindingSource.DataMember = "spTagiEgyenlegTetelek";
            this.spTagiEgyenlegTetelekBindingSource.DataSource = this.VPFSDataSet;
            // 
            // spTagiEgyenlegFej2BindingSource
            // 
            this.spTagiEgyenlegFej2BindingSource.DataMember = "spTagiEgyenlegFej2";
            this.spTagiEgyenlegFej2BindingSource.DataSource = this.VPFSDataSet;
            // 
            // spTagiEgyenlegFej3BindingSource
            // 
            this.spTagiEgyenlegFej3BindingSource.DataMember = "spTagiEgyenlegFej3";
            this.spTagiEgyenlegFej3BindingSource.DataSource = this.VPFSDataSet;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "dsTadiEgyFej";
            reportDataSource1.Value = this.spTagiEgyenlegFejBindingSource;
            reportDataSource2.Name = "dsTagiEgyTetelek";
            reportDataSource2.Value = this.spTagiEgyenlegTetelekBindingSource;
            reportDataSource3.Name = "dsTagiEgyFej2";
            reportDataSource3.Value = this.spTagiEgyenlegFej2BindingSource;
            reportDataSource4.Name = "dsTagiEgyFej3";
            reportDataSource4.Value = this.spTagiEgyenlegFej3BindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VoluntaryPensionFundSystem.TagiEgyeniReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 57);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(771, 657);
            this.reportViewer1.TabIndex = 3;
            // 
            // spTagiEgyenlegFejTableAdapter
            // 
            this.spTagiEgyenlegFejTableAdapter.ClearBeforeFill = true;
            // 
            // spTagiEgyenlegTetelekTableAdapter
            // 
            this.spTagiEgyenlegTetelekTableAdapter.ClearBeforeFill = true;
            // 
            // spTagiEgyenlegFej2TableAdapter1
            // 
            this.spTagiEgyenlegFej2TableAdapter1.ClearBeforeFill = true;
            // 
            // spTagiEgyenlegFej3TableAdapter
            // 
            this.spTagiEgyenlegFej3TableAdapter.ClearBeforeFill = true;
            // 
            // TagiEgyeniSzamlaReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(771, 714);
            this.Controls.Add(this.reportViewer1);
            this.Name = "TagiEgyeniSzamlaReport";
            this.Text = "Tagi egyéni folyószámla-egyenleg előnézet";
            this.Load += new System.EventHandler(this.TagiEgyeniSzamlaReport_Load);
            this.Controls.SetChildIndex(this.reportViewer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFejBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPFSDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegTetelekBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFej2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTagiEgyenlegFej3BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource spTagiEgyenlegFejBindingSource;
        private VPFSDataSet VPFSDataSet;
        private VPFSDataSetTableAdapters.spTagiEgyenlegFejTableAdapter spTagiEgyenlegFejTableAdapter;
        private System.Windows.Forms.BindingSource spTagiEgyenlegTetelekBindingSource;
        private VPFSDataSetTableAdapters.spTagiEgyenlegTetelekTableAdapter spTagiEgyenlegTetelekTableAdapter;
        private VPFSDataSetTableAdapters.spTagiEgyenlegFej2TableAdapter spTagiEgyenlegFej2TableAdapter1;
        private System.Windows.Forms.BindingSource spTagiEgyenlegFej2BindingSource;
        private System.Windows.Forms.BindingSource spTagiEgyenlegFej3BindingSource;
        private VPFSDataSetTableAdapters.spTagiEgyenlegFej3TableAdapter spTagiEgyenlegFej3TableAdapter;


    }
}
