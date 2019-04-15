namespace createTagFile
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnGenerateFile = new System.Windows.Forms.Button();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.fileType = new System.Windows.Forms.ComboBox();
            this.btnGenerateSampleFile = new System.Windows.Forms.Button();
            this.fileNameRuleDescription = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lblFileType = new System.Windows.Forms.Label();
            this.lblInputFileType = new System.Windows.Forms.Label();
            this.txt_NO = new System.Windows.Forms.TextBox();
            this.txt_partNumber = new System.Windows.Forms.TextBox();
            this.txt_PO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.outputFilePath = new System.Windows.Forms.TextBox();
            this.btnSavePath = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.actionCode = new System.Windows.Forms.ComboBox();
            this.help = new System.Windows.Forms.Button();
            this.openDirector = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_tpfReqId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_ErpCorpId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.OrderType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnGenerateFile
            // 
            this.btnGenerateFile.ForeColor = System.Drawing.Color.Red;
            this.btnGenerateFile.Location = new System.Drawing.Point(666, 401);
            this.btnGenerateFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateFile.Name = "btnGenerateFile";
            this.btnGenerateFile.Size = new System.Drawing.Size(104, 28);
            this.btnGenerateFile.TabIndex = 11;
            this.btnGenerateFile.Text = "產生檔案";
            this.btnGenerateFile.UseVisualStyleBackColor = true;
            this.btnGenerateFile.Click += new System.EventHandler(this.btnGenerateEtagFile_Click);
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.Location = new System.Drawing.Point(666, 260);
            this.btnBrowsePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(104, 28);
            this.btnBrowsePath.TabIndex = 8;
            this.btnBrowsePath.Text = "輸入檔案";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
            // 
            // txtPath
            // 
            this.txtPath.AllowDrop = true;
            this.txtPath.Location = new System.Drawing.Point(13, 263);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(628, 30);
            this.txtPath.TabIndex = 6;
            this.txtPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtPath_DragDrop);
            this.txtPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtPath_DragEnter);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(13, 323);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(628, 146);
            this.txtResult.TabIndex = 12;
            // 
            // fileType
            // 
            this.fileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileType.FormattingEnabled = true;
            this.fileType.Items.AddRange(new object[] {
            "InnerTPF_From_SIS",
            "TRFF",
            "WT100",
            "WT200"});
            this.fileType.Location = new System.Drawing.Point(148, 11);
            this.fileType.Margin = new System.Windows.Forms.Padding(4);
            this.fileType.Name = "fileType";
            this.fileType.Size = new System.Drawing.Size(143, 31);
            this.fileType.TabIndex = 1;
            this.fileType.SelectedIndexChanged += new System.EventHandler(this.fileType_SelectedIndexChanged);
            // 
            // btnGenerateSampleFile
            // 
            this.btnGenerateSampleFile.Enabled = false;
            this.btnGenerateSampleFile.Location = new System.Drawing.Point(666, 365);
            this.btnGenerateSampleFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateSampleFile.Name = "btnGenerateSampleFile";
            this.btnGenerateSampleFile.Size = new System.Drawing.Size(104, 28);
            this.btnGenerateSampleFile.TabIndex = 10;
            this.btnGenerateSampleFile.Text = "產生範本檔";
            this.btnGenerateSampleFile.UseVisualStyleBackColor = true;
            this.btnGenerateSampleFile.Visible = false;
            this.btnGenerateSampleFile.Click += new System.EventHandler(this.btnGenerateSampleFile_Click);
            // 
            // fileNameRuleDescription
            // 
            this.fileNameRuleDescription.AutoSize = true;
            this.fileNameRuleDescription.Location = new System.Drawing.Point(18, 40);
            this.fileNameRuleDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fileNameRuleDescription.Name = "fileNameRuleDescription";
            this.fileNameRuleDescription.Size = new System.Drawing.Size(0, 23);
            this.fileNameRuleDescription.TabIndex = 9;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.GridLines = true;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(13, 65);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(470, 157);
            this.listView1.TabIndex = 11;
            this.listView1.TileSize = new System.Drawing.Size(188, 20);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Location = new System.Drawing.Point(18, 15);
            this.lblFileType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(186, 23);
            this.lblFileType.TabIndex = 12;
            this.lblFileType.Text = "選擇要產生的檔案";
            // 
            // lblInputFileType
            // 
            this.lblInputFileType.AutoSize = true;
            this.lblInputFileType.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInputFileType.ForeColor = System.Drawing.Color.Red;
            this.lblInputFileType.Location = new System.Drawing.Point(13, 235);
            this.lblInputFileType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInputFileType.Name = "lblInputFileType";
            this.lblInputFileType.Size = new System.Drawing.Size(0, 24);
            this.lblInputFileType.TabIndex = 14;
            // 
            // txt_NO
            // 
            this.txt_NO.Enabled = false;
            this.txt_NO.Location = new System.Drawing.Point(619, 86);
            this.txt_NO.MaxLength = 5;
            this.txt_NO.Name = "txt_NO";
            this.txt_NO.Size = new System.Drawing.Size(151, 30);
            this.txt_NO.TabIndex = 3;
            // 
            // txt_partNumber
            // 
            this.txt_partNumber.Enabled = false;
            this.txt_partNumber.Location = new System.Drawing.Point(619, 113);
            this.txt_partNumber.MaxLength = 12;
            this.txt_partNumber.Name = "txt_partNumber";
            this.txt_partNumber.Size = new System.Drawing.Size(151, 30);
            this.txt_partNumber.TabIndex = 4;
            // 
            // txt_PO
            // 
            this.txt_PO.Enabled = false;
            this.txt_PO.Location = new System.Drawing.Point(619, 59);
            this.txt_PO.MaxLength = 10;
            this.txt_PO.Name = "txt_PO";
            this.txt_PO.Size = new System.Drawing.Size(151, 30);
            this.txt_PO.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(505, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "訂單編號(10碼)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(512, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 23);
            this.label2.TabIndex = 19;
            this.label2.Text = "訂單項次(5碼)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(533, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "料號(11碼)";
            // 
            // outputFilePath
            // 
            this.outputFilePath.Location = new System.Drawing.Point(13, 293);
            this.outputFilePath.Name = "outputFilePath";
            this.outputFilePath.Size = new System.Drawing.Size(628, 30);
            this.outputFilePath.TabIndex = 7;
            // 
            // btnSavePath
            // 
            this.btnSavePath.Location = new System.Drawing.Point(666, 290);
            this.btnSavePath.Name = "btnSavePath";
            this.btnSavePath.Size = new System.Drawing.Size(104, 28);
            this.btnSavePath.TabIndex = 9;
            this.btnSavePath.Text = "產檔位置";
            this.btnSavePath.UseVisualStyleBackColor = true;
            this.btnSavePath.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(498, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 23);
            this.label4.TabIndex = 24;
            this.label4.Text = "ActionCode(2碼)";
            // 
            // actionCode
            // 
            this.actionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionCode.Enabled = false;
            this.actionCode.FormattingEnabled = true;
            this.actionCode.Location = new System.Drawing.Point(619, 140);
            this.actionCode.Name = "actionCode";
            this.actionCode.Size = new System.Drawing.Size(151, 31);
            this.actionCode.TabIndex = 5;
            // 
            // help
            // 
            this.help.Location = new System.Drawing.Point(666, 8);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(104, 28);
            this.help.TabIndex = 26;
            this.help.Text = "流程說明";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // openDirector
            // 
            this.openDirector.Location = new System.Drawing.Point(666, 436);
            this.openDirector.Name = "openDirector";
            this.openDirector.Size = new System.Drawing.Size(104, 28);
            this.openDirector.TabIndex = 27;
            this.openDirector.Text = "開啟產檔目錄";
            this.openDirector.UseVisualStyleBackColor = true;
            this.openDirector.Click += new System.EventHandler(this.openDirector_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(498, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 23);
            this.label5.TabIndex = 28;
            this.label5.Text = "TPF_REQ_ID";
            // 
            // txt_tpfReqId
            // 
            this.txt_tpfReqId.Enabled = false;
            this.txt_tpfReqId.Location = new System.Drawing.Point(619, 197);
            this.txt_tpfReqId.MaxLength = 8;
            this.txt_tpfReqId.Name = "txt_tpfReqId";
            this.txt_tpfReqId.Size = new System.Drawing.Size(151, 30);
            this.txt_tpfReqId.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(498, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 23);
            this.label6.TabIndex = 30;
            this.label6.Text = "物流ERP_CORP_ID";
            // 
            // txt_ErpCorpId
            // 
            this.txt_ErpCorpId.Enabled = false;
            this.txt_ErpCorpId.Location = new System.Drawing.Point(619, 226);
            this.txt_ErpCorpId.MaxLength = 10;
            this.txt_ErpCorpId.Name = "txt_ErpCorpId";
            this.txt_ErpCorpId.Size = new System.Drawing.Size(151, 30);
            this.txt_ErpCorpId.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(498, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 23);
            this.label7.TabIndex = 32;
            this.label7.Text = "Order_Type";
            // 
            // OrderType
            // 
            this.OrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrderType.Enabled = false;
            this.OrderType.FormattingEnabled = true;
            this.OrderType.Location = new System.Drawing.Point(619, 170);
            this.OrderType.Name = "OrderType";
            this.OrderType.Size = new System.Drawing.Size(151, 31);
            this.OrderType.TabIndex = 33;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 482);
            this.Controls.Add(this.OrderType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_ErpCorpId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_tpfReqId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.openDirector);
            this.Controls.Add(this.help);
            this.Controls.Add(this.actionCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSavePath);
            this.Controls.Add(this.outputFilePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_PO);
            this.Controls.Add(this.txt_partNumber);
            this.Controls.Add(this.txt_NO);
            this.Controls.Add(this.lblInputFileType);
            this.Controls.Add(this.lblFileType);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.fileNameRuleDescription);
            this.Controls.Add(this.btnGenerateSampleFile);
            this.Controls.Add(this.fileType);
            this.Controls.Add(this.btnGenerateFile);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnBrowsePath);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Tag交換檔案產生工具 v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnGenerateFile;
        private System.Windows.Forms.Button btnBrowsePath;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.ComboBox fileType;
        private System.Windows.Forms.Button btnGenerateSampleFile;
        private System.Windows.Forms.Label fileNameRuleDescription;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.Label lblInputFileType;
        private System.Windows.Forms.TextBox txt_NO;
        private System.Windows.Forms.TextBox txt_partNumber;
        private System.Windows.Forms.TextBox txt_PO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox outputFilePath;
        private System.Windows.Forms.Button btnSavePath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox actionCode;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button openDirector;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_tpfReqId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_ErpCorpId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox OrderType;
    }
}

