namespace VoluntaryPensionFundSystem
{
    partial class MunkFolyoszamlaReportW
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.spMunkEgyenlegFejBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.VPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spMunkEgyenlegFej2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spMunkEgyenlegFej3BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spMunkEgyenlegTetelekBevBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spMunkEgyenlegTetelekBefBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spMunkEgyenlegFejBefBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.spMunkEgyenlegFejTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegFejTableAdapter();
            this.spMunkEgyenlegFej2TableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegFej2TableAdapter();
            this.spMunkEgyenlegFej3TableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegFej3TableAdapter();
            this.spMunkEgyenlegTetelekBevTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegTetelekBevTableAdapter();
            this.spMunkEgyenlegTetelekBefTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegTetelekBefTableAdapter();
            this.spMunkEgyenlegFejBefTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spMunkEgyenlegFejBefTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFejBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPFSDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFej2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFej3BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegTetelekBevBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegTetelekBefBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFejBefBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // spMunkEgyenlegFejBindingSource
            // 
            this.spMunkEgyenlegFejBindingSource.DataMember = "spMunkEgyenlegFej";
            this.spMunkEgyenlegFejBindingSource.DataSource = this.VPFSDataSet;
            // 
            // VPFSDataSet
            // 
            this.VPFSDataSet.DataSetName = "VPFSDataSet";
            this.VPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spMunkEgyenlegFej2BindingSource
            // 
            this.spMunkEgyenlegFej2BindingSource.DataMember = "spMunkEgyenlegFej2";
            this.spMunkEgyenlegFej2BindingSource.DataSource = this.VPFSDataSet;
            // 
            // spMunkEgyenlegFej3BindingSource
            // 
            this.spMunkEgyenlegFej3BindingSource.DataMember = "spMunkEgyenlegFej3";
            this.spMunkEgyenlegFej3BindingSource.DataSource = this.VPFSDataSet;
            // 
            // spMunkEgyenlegTetelekBevBindingSource
            // 
            this.spMunkEgyenlegTetelekBevBindingSource.DataMember = "spMunkEgyenlegTetelekBev";
            this.spMunkEgyenlegTetelekBevBindingSource.DataSource = this.VPFSDataSet;
            // 
            // spMunkEgyenlegTetelekBefBindingSource
            // 
            this.spMunkEgyenlegTetelekBefBindingSource.DataMember = "spMunkEgyenlegTetelekBef";
            this.spMunkEgyenlegTetelekBefBindingSource.DataSource = this.VPFSDataSet;
            // 
            // spMunkEgyenlegFejBefBindingSource
            // 
            this.spMunkEgyenlegFejBefBindingSource.DataMember = "spMunkEgyenlegFejBef";
            this.spMunkEgyenlegFejBefBindingSource.DataSource = this.VPFSDataSet;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "dsMunkFej1";
            reportDataSource1.Value = this.spMunkEgyenlegFejBindingSource;
            reportDataSource2.Name = "dsMunkFej2";
            reportDataSource2.Value = this.spMunkEgyenlegFej2BindingSource;
            reportDataSource3.Name = "dsMunkFej3";
            reportDataSource3.Value = this.spMunkEgyenlegFej3BindingSource;
            reportDataSource4.Name = "dsMunkTetelekBev";
            reportDataSource4.Value = this.spMunkEgyenlegTetelekBevBindingSource;
            reportDataSource5.Name = "dsMunkTetelekBef";
            reportDataSource5.Value = this.spMunkEgyenlegTetelekBefBindingSource;
            reportDataSource6.Name = "dsMunkFejBef";
            reportDataSource6.Value = this.spMunkEgyenlegFejBefBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource5);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource6);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VoluntaryPensionFundSystem.MunkFszlaReport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 57);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(771, 657);
            this.reportViewer1.TabIndex = 3;
            // 
            // spMunkEgyenlegFejTableAdapter
            // 
            this.spMunkEgyenlegFejTableAdapter.ClearBeforeFill = true;
            // 
            // spMunkEgyenlegFej2TableAdapter
            // 
            this.spMunkEgyenlegFej2TableAdapter.ClearBeforeFill = true;
            // 
            // spMunkEgyenlegFej3TableAdapter
            // 
            this.spMunkEgyenlegFej3TableAdapter.ClearBeforeFill = true;
            // 
            // spMunkEgyenlegTetelekBevTableAdapter
            // 
            this.spMunkEgyenlegTetelekBevTableAdapter.ClearBeforeFill = true;
            // 
            // spMunkEgyenlegTetelekBefTableAdapter
            // 
            this.spMunkEgyenlegTetelekBefTableAdapter.ClearBeforeFill = true;
            // 
            // spMunkEgyenlegFejBefTableAdapter
            // 
            this.spMunkEgyenlegFejBefTableAdapter.ClearBeforeFill = true;
            // 
            // MunkFolyoszamlaReportW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(771, 714);
            this.Controls.Add(this.reportViewer1);
            this.Name = "MunkFolyoszamlaReportW";
            this.Text = "Munkáltatói folyószámla-egyenleg előnézet";
            this.Load += new System.EventHandler(this.MunkFolyoszamlaReportW_Load);
            this.Controls.SetChildIndex(this.reportViewer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFejBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VPFSDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFej2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFej3BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegTetelekBevBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegTetelekBefBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMunkEgyenlegFejBefBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource spMunkEgyenlegFejBindingSource;
        private VPFSDataSet VPFSDataSet;
        private System.Windows.Forms.BindingSource spMunkEgyenlegFej2BindingSource;
        private System.Windows.Forms.BindingSource spMunkEgyenlegFej3BindingSource;
        private System.Windows.Forms.BindingSource spMunkEgyenlegTetelekBevBindingSource;
        private VPFSDataSetTableAdapters.spMunkEgyenlegFejTableAdapter spMunkEgyenlegFejTableAdapter;
        private VPFSDataSetTableAdapters.spMunkEgyenlegFej2TableAdapter spMunkEgyenlegFej2TableAdapter;
        private VPFSDataSetTableAdapters.spMunkEgyenlegFej3TableAdapter spMunkEgyenlegFej3TableAdapter;
        private VPFSDataSetTableAdapters.spMunkEgyenlegTetelekBevTableAdapter spMunkEgyenlegTetelekBevTableAdapter;
        private System.Windows.Forms.BindingSource spMunkEgyenlegTetelekBefBindingSource;
        private VPFSDataSetTableAdapters.spMunkEgyenlegTetelekBefTableAdapter spMunkEgyenlegTetelekBefTableAdapter;
        private System.Windows.Forms.BindingSource spMunkEgyenlegFejBefBindingSource;
        private VPFSDataSetTableAdapters.spMunkEgyenlegFejBefTableAdapter spMunkEgyenlegFejBefTableAdapter;

    }
}
