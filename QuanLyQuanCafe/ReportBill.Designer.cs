namespace QuanLyQuanCafe
{
    partial class ReportBill
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.QuanLyQuanCafe_9DataSet = new QuanLyQuanCafe.QuanLyQuanCafe_9DataSet();
            this.BillBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.BillTableAdapter = new QuanLyQuanCafe.QuanLyQuanCafe_9DataSetTableAdapters.BillTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.QuanLyQuanCafe_9DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.BillBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyQuanCafe.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1076, 472);
            this.reportViewer1.TabIndex = 0;
            // 
            // QuanLyQuanCafe_9DataSet
            // 
            this.QuanLyQuanCafe_9DataSet.DataSetName = "QuanLyQuanCafe_9DataSet";
            this.QuanLyQuanCafe_9DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BillBindingSource
            // 
            this.BillBindingSource.DataMember = "Bill";
            this.BillBindingSource.DataSource = this.QuanLyQuanCafe_9DataSet;
            // 
            // BillTableAdapter
            // 
            this.BillTableAdapter.ClearBeforeFill = true;
            // 
            // ReportBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 496);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ReportBill";
            this.Text = "ReportBill";
            this.Load += new System.EventHandler(this.ReportBill_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QuanLyQuanCafe_9DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BillBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource BillBindingSource;
        private QuanLyQuanCafe_9DataSet QuanLyQuanCafe_9DataSet;
        private QuanLyQuanCafe_9DataSetTableAdapters.BillTableAdapter BillTableAdapter;
    }
}