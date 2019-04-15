using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;



namespace createTagFile
{
    public partial class Form1 : Form
    {
        private string innerTpfFileName = "TPFData_FETC_BatchNO_YYYYMMDD_流水號_toSIS.csv";
        private string outerTpfFileName = "TPF申請單號_RECN_型號_VCID_數量_YYYYMMDD_vvvv.csv";
        //private string WT100FileName = "WT100_Warehouse_YYYYMMDD_vvvv.csv";
        //private string WT200FileName = "WT200_Warehouse_YYYYMMDD_vvvv.csv";
        private string P3FileName = "PersoData_RECN_CardType_yyyymmdd_vvvv.xml";
        private string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private HelpForm helpForm;
        private string lastFileName;
        public Form1()
        {
            InitializeComponent();
            ResetListView();
            this.fileType.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.outputFilePath.Text = desktopPath;
        }

        /// <summary> 產檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateEtagFile_Click(object sender, EventArgs e)
        {
            try
            {
                string FeatureName = this.fileType.SelectedItem.ToString().ToUpper();
                lastFileName = string.Empty;
                if (!FeatureName.StartsWith("WT") && !validate("0"))
                {
                    return;
                }
                switch (this.fileType.SelectedItem.ToString().ToUpper())
                {
                    case "INNERTPF_FROM_SIS":
                        this.GenerateInnerTpfFromSIS();
                        break;
                    case "TRFF":
                        this.GenerateTRFF();
                        break;
                    case "WT100":
                    case "WT200":
                        this.GenerateWT();
                        break;
                    case "IFF":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        /// <summary> 產生範本檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateSampleFile_Click(object sender, EventArgs e)
        {
            try
            {

                lastFileName = string.Empty;
                if (!validate("1"))
                {
                    return;
                }

                String writePath = string.Empty;
                String duplicatePath = string.Empty;
                int j = 0;
                switch (this.fileType.SelectedItem.ToString().ToUpper())
                {
                    case "WT100":
                    case "WT200":
                        #region 寫檔
                        writePath = Path.Combine(this.outputFilePath.Text, this.fileType.SelectedItem.ToString() + "_Sample");
                        duplicatePath = writePath + ".csv";
                        while (File.Exists(duplicatePath))
                        {
                            if (j == 0)
                            {
                                duplicatePath = writePath + "(1).csv";
                            }
                            else
                            {
                                duplicatePath = writePath + "(" + j + ").csv";
                            }
                            j++;
                        }
                        writePath = duplicatePath;
                        lastFileName = Path.GetFileName(writePath);
                        using (FileStream CSVFile = System.IO.File.Open(writePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            using (StreamWriter sw = new StreamWriter(CSVFile, Encoding.Default))
                            {
                                //sw.WriteLine("SeqNo,Tag_SN,Action_Code,ITEM_NUM,BOX_NUM,Destination,Lot_Num,Roll_Num");
                                sw.WriteLine("EPC_ID,料號");
                                sw.WriteLine("105013AA4531582146455443,S13000000001");
                            }
                        }
                        AddTextToResult(string.Format("檔案已產生：{0}", writePath));
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        /// <summary> 瀏覽檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            this.txtPath.Text = openFileDialog1.FileName;
        }

        /// <summary> 檔案類型 變更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetListView();
                ComboBox cb = (ComboBox)sender;
                String nameRule = "輸入檔名規則：";
                String inputFileType = "輸入檔案，請提供 {0} 的檔案";
                string[] rule;

                this.btnGenerateSampleFile.Enabled = false;
                this.txt_PO.Enabled = false;
                this.txt_NO.Enabled = false;
                this.txt_partNumber.Enabled = false;
                this.actionCode.Enabled = false;
                this.OrderType.Enabled = false;
                this.txt_tpfReqId.Enabled = false;
                this.txt_ErpCorpId.Enabled = false;
                this.actionCode.Items.Clear();
                this.OrderType.Items.Clear();

                switch (cb.SelectedItem.ToString().ToUpper())
                {
                    case "INNERTPF_FROM_SIS":
                        inputFileType = String.Format(inputFileType, "對內TPF To SIS(BMS產出)");
                        nameRule += innerTpfFileName;
                        rule = SplitFileNameRule(innerTpfFileName);
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[0], "固定" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[1], "固定" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[2], "3碼數字，左邊補0" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[3], "8碼數字，西元年月日" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[4], "3碼數字，左邊補0" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[5], "固定" }));
                        break;
                    case "TRFF":
                        this.txt_PO.Enabled = true;
                        this.txt_NO.Enabled = true;
                        this.txt_partNumber.Enabled = true;
                        this.txt_ErpCorpId.Enabled = true;
                        inputFileType = String.Format(inputFileType, "對外TPF(BMS產出)");
                        nameRule += outerTpfFileName;
                        rule = SplitFileNameRule(outerTpfFileName);
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[0], "14碼，TPF+YYYMMDD+3碼流水序號(左邊補0)" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[1], "檔案接受者代號：FETC、SIRIT、TECO、MITAC、YDT" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[2], "產品型號" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[3], "1、2、3、4、5" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[4], "數量" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[5], "8碼數字，西元年月日" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[6], "4碼數字，流水號，左邊補0" }));
                        break;
                    case "WT100":
                        this.btnGenerateSampleFile.Enabled = true;
                        this.txt_tpfReqId.Enabled = true;
                        this.txt_ErpCorpId.Enabled = true;
                        inputFileType = "";//String.Format(inputFileType, "WT100(可下載範本修改再輸入)");
                        nameRule = String.Empty;
                        this.actionCode.Enabled = true;
                        this.actionCode.Items.AddRange(
                            new ComboboxItem[]{
                                new ComboboxItem(){Value = "WH",Text ="WH:進貨" },
                                new ComboboxItem(){Value = "FW",Text ="FW:挾疵品修復入庫" },
                                new ComboboxItem(){Value = "SB",Text ="SB:銷貨退回" }
                            });
                        this.actionCode.SelectedIndex = 0;
                        //nameRule += WT100FileName;
                        //rule = SplitFileNameRule(WT100FileName);
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[0], "固定" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[1], "固定" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[2], "8碼數字，西元年月日" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[3], "4碼數字，流水號，左邊補0" }));
                        break;
                    case "WT200":
                        this.btnGenerateSampleFile.Enabled = true;
                        this.txt_tpfReqId.Enabled = true;
                        this.txt_ErpCorpId.Enabled = true;
                        inputFileType = "";//String.Format(inputFileType, "WT200(可下載範本修改再輸入)");
                        nameRule = String.Empty;

                        this.OrderType.Enabled = true;
                        this.OrderType.Items.AddRange(
                            new ComboboxItem[]{
                                new ComboboxItem(){Value = "KB",Text ="KB:寄銷" },
                                new ComboboxItem(){Value = "TA",Text ="TA:賣斷" },
                                new ComboboxItem(){Value = "KA",Text ="KA:寄銷取回" },
                                new ComboboxItem(){Value = "RE",Text ="RE:賣斷退貨" }
                        });
                        this.OrderType.SelectedIndex = 0;

                        this.actionCode.Enabled = true;
                        this.actionCode.Items.AddRange(
                            new ComboboxItem[]{
                                new ComboboxItem(){Value = "SS",Text ="SS:特殊商品出貨" },
                                new ComboboxItem(){Value = "SC",Text ="SC:一般出貨" },
                                new ComboboxItem(){Value = "SR",Text ="SR:移倉作業" }
                        });
                        this.actionCode.SelectedIndex = 0;
                        //nameRule += WT200FileName;
                        //rule = SplitFileNameRule(WT200FileName);
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[0], "固定" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[1], "固定" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[2], "8碼數字，西元年月日" }));
                        //this.listView1.Items.Add(new ListViewItem(new string[] { rule[3], "4碼數字，流水號，左邊補0" }));
                        break;
                    case "IFF":
                        inputFileType = String.Format(inputFileType, "P3(BMS產出)");
                        nameRule += P3FileName;
                        rule = SplitFileNameRule(P3FileName);
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[0], "固定" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[1], "檔案接受者代號：FEIB、TSB、GDTECO" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[2], "卡片類型" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[3], "8碼數字，西元年月日" }));
                        this.listView1.Items.Add(new ListViewItem(new string[] { rule[4], "4碼數字，流水號，左邊補0" }));
                        break;
                    default:
                        nameRule = String.Empty;
                        inputFileType = String.Empty;
                        break;
                }
                this.fileNameRuleDescription.Text = nameRule;
                this.lblInputFileType.Text = inputFileType;
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        /// <summary> 顯示結果
        /// </summary>
        /// <param name="txt"></param>
        private void AddTextToResult(string txt)
        {
            this.txtResult.Text =
                this.txtResult.Text +
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\t" +
                txt +
                Environment.NewLine +
                "----------------------------------------------------------" +
                Environment.NewLine;

            this.txtResult.SelectionStart = this.txtResult.Text.Length;
            this.txtResult.ScrollToCaret();
            this.txtResult.Focus();
        }

        /// <summary> 驗證
        /// </summary>
        /// <param name="validateType">0:產檔、1:範例檔</param>
        /// <returns></returns>
        private bool validate(string validateType)
        {
            //"INNERTPF_FROM_SIS", "TRFF", "WT100", "WT200", "IFF"

            bool res = true; ; if (validateType == "0" && String.IsNullOrWhiteSpace(this.txtPath.Text))
            {
                res = false;
                MessageBox.Show("請選擇輸入檔案位置");
            }
            else if (String.IsNullOrWhiteSpace(this.outputFilePath.Text))
            {
                res = false;
                MessageBox.Show("請選擇存檔位置");
            }
            else if (validateType == "0" && !File.Exists(this.txtPath.Text))
            {
                res = false;
                MessageBox.Show("找不到檔案，請重新輸入");
            }
            else if (validateType == "0" && this.fileType.SelectedItem.ToString().ToUpper() == "TRFF" &&
                (String.IsNullOrWhiteSpace(this.txt_PO.Text) || String.IsNullOrWhiteSpace(this.txt_NO.Text) || String.IsNullOrWhiteSpace(this.txt_partNumber.Text)))
            {
                res = false;
                MessageBox.Show("請輸入右方的訂單編號、訂單項次、料號");
            }
            return res;
        }

        /// <summary> 產生對內TPF(From SIS)
        /// </summary>
        private void GenerateInnerTpfFromSIS()
        {
            try
            {
                string line = string.Empty;

                string newName = "";
                StringBuilder final = new StringBuilder();
                using (FileStream myFile = System.IO.File.Open(txtPath.Text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamReader myReader = new StreamReader(myFile))
                    {
                        newName = myFile.Name.Substring(myFile.Name.LastIndexOf(@"\") + 1);
                        newName = newName.Substring(0, newName.LastIndexOf("_"));
                        string[] temp1;
                        List<int> list;
                        String checkStr;
                        string newEpcid = "";
                        string a1 = "", a2 = "";

                        final.AppendLine("EPC_ID,ANTICOUNTERFEIT_1,ANTICOUNTERFEIT_2,EPC_ID_SIS");
                        while ((line = myReader.ReadLine()) != null)
                        {
                            #region 讀檔，產生驗證碼
                            if (line.IndexOf("EPC_ID") == -1 && !String.IsNullOrEmpty(line))
                            {
                                //取得EPCID，計算驗證碼
                                temp1 = line.Split(',');
                                a1 = temp1[0].Substring(0, 14);
                                //XOR PID 
                                list = new List<int>();
                                for (int i = 0; i < a1.Length / 2; i++)
                                {
                                    checkStr = a1.Substring(i * 2, 2);
                                    list.Add(Convert.ToByte(checkStr, 16));
                                }
                                checkStr = "";
                                var temp = 0x00;
                                foreach (var item in list)
                                {
                                    if (list.IndexOf(item) == 0)
                                        temp = item;
                                    else
                                        temp = temp ^ item;
                                }
                                checkStr = temp.ToString();
                                if (checkStr.Length == 1)
                                {
                                    checkStr = "0" + checkStr;
                                }
                                else if (checkStr.Length > 2)
                                {
                                    checkStr = checkStr.Substring(0, 2);
                                }
                                a2 = a1.Substring(0, 14) + checkStr;
                                newEpcid = a2 + "46455443";
                                final.AppendLine(temp1[0] + "," + temp1[0].Substring(0, 16) + "," + a2 + "," + newEpcid);
                            }
                            #endregion
                        }
                    }
                }
                String writePath = Path.Combine(this.outputFilePath.Text, newName + "_toPCBS");
                int j = 0;
                string duplicatePath = writePath + ".csv";
                while (File.Exists(duplicatePath))
                {
                    if (j == 0)
                    {
                        duplicatePath = writePath + "(1).csv";
                    }
                    else
                    {
                        duplicatePath = writePath + "(" + j + ").csv";
                    }
                    j++;
                }
                writePath = duplicatePath;
                lastFileName = Path.GetFileName(writePath);
                using (FileStream CSVFile = System.IO.File.Open(writePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(CSVFile))
                    {
                        sw.WriteLine(final.ToString());        //寫入文字 
                    }
                }
                AddTextToResult(string.Format("檔案已產生：{0}", writePath));
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        /// <summary> 產生TRFF檔案 
        /// </summary>
        private void GenerateTRFF()
        {
            try
            {
                string line = string.Empty;

                string newName = "";
                StringBuilder final = new StringBuilder();
                using (FileStream myFile = System.IO.File.Open(txtPath.Text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamReader myReader = new StreamReader(myFile))
                    {
                        string[] tpfName = Path.GetFileNameWithoutExtension(myFile.Name).Split('_');
                        newName = "TRFF_" + tpfName[0] + "_FETC_" + this.txt_PO.Text + "_" + this.txt_NO.Text + "_"
                            + this.txt_partNumber.Text + "_" + tpfName[4] + "_" + DateTime.Now.ToString("yyyyMMdd") + "_0001" + "_" + txt_ErpCorpId.Text;
                        string[] temp1;
                        int i = 1;

                        final.AppendLine("Sequence_Number,TID,EPC,Sirit_Version_Number,Sirit_Key_Index,Sirit_Key_Ring_ID,Status,Lot_Num,Roll_Num,Box_Num");
                        while ((line = myReader.ReadLine()) != null)
                        {
                            #region 讀檔，產生驗證碼
                            if (line.IndexOf("EPC_ID") == -1 && !String.IsNullOrEmpty(line))
                            {
                                temp1 = line.Split(',');
                                final.AppendLine(
                                    i.ToString() + "," +
                                    "E" + DateTime.Now.ToString("yyyyMMddHHmmss") + i.ToString().PadLeft(9, '0') + "," +
                                    temp1[1] + ",12,12,12345678,01,1234567,12,B012345");
                                i++;
                            }
                            #endregion
                        }
                    }
                }
                String writePath = Path.Combine(this.outputFilePath.Text, newName);
                int j = 0;
                string duplicatePath = writePath + ".csv";
                while (File.Exists(duplicatePath))
                {
                    if (j == 0)
                    {
                        duplicatePath = writePath + "(1).csv";
                    }
                    else
                    {
                        duplicatePath = writePath + "(" + j + ").csv";
                    }
                    j++;
                }
                writePath = duplicatePath;
                lastFileName = Path.GetFileName(writePath);
                using (FileStream CSVFile = System.IO.File.Open(writePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(CSVFile))
                    {
                        sw.WriteLine(final.ToString());        //寫入文字 
                    }
                }
                AddTextToResult(string.Format("檔案已產生：{0}", writePath));
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        /// <summary> 產生WT100 & WT200
        /// </summary>
        private void GenerateWT()
        {
            try
            {
                string TpfReqId = txt_tpfReqId.Text.Trim();
                List<string> lstTagInfo = new List<string>();
                TagInfo tag = new TagInfo();
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BMSContext"].ToString();
                string strSQL = @"
                                    SELECT ETAG.EPC_ID, TRFF.TAG_PART_NUMBER FROM BMS_TAG_ETAG ETAG WITH(NOLOCK)
                                    INNER JOIN BMS_TAG_TRFF TRFF WITH(NOLOCK) ON ETAG.TRFF_ID = TRFF.TRFF_ID
                                    WHERE 
                                    ETAG.TPF_REQ_ID = @tpfReqId
                                ";
                using(SqlConnection cn = new SqlConnection(ConnectionString))
                {
                     cn.Open();
                     using(SqlCommand cmd = new SqlCommand(strSQL, cn))
                     {
                         cmd.Parameters.Add("@tpfReqId", TpfReqId);
                        //2.回傳DataReader的值
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lstTagInfo.Add(dr["EPC_ID"].ToString() + "," + dr["TAG_PART_NUMBER"].ToString());
                            }
                        }
                     }
                }
                if (lstTagInfo.Count <= 0)
                {                  
                    AddTextToResult(string.Format("TPF_REQ_ID:{0}查無相關TAG資料。", TpfReqId));
                    return;
                }



                string line = string.Empty;

                string newName = "";
                StringBuilder final = new StringBuilder();
                string WTtype = this.fileType.SelectedItem.ToString().ToUpper();
                newName = WTtype + "_" + txt_ErpCorpId.Text + "_" + DateTime.Now.ToString("yyyyMMdd") + "_0001";
                string[] temp1;
                int i = 1;
                final.AppendLine(string.Format("SeqNo,Tag_SN,Action_Code,ITEM_NUM,BOX_NUM,Destination,{0}PO_Num,Lot_Num,Roll_Num", WTtype == "WT200"?"Order_Type,":""));
                foreach (var item in lstTagInfo)
                {
                    temp1 = item.Split(',');
                    final.AppendLine(
                        i.ToString() + "," +
                        temp1[0] + "," +
                        (this.actionCode.SelectedItem as ComboboxItem).Value.ToString() + "," +
                        temp1[1] + "," + "B012345,1000001466,"+
                        (WTtype == "WT200" ? (this.OrderType.SelectedItem as ComboboxItem).Value.ToString() + "," : "") +                        
                        "1234567890,123456,12"
                        );
                    i++;
                }
                //using (FileStream myFile = System.IO.File.Open(txtPath.Text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                //{
                //    using (StreamReader myReader = new StreamReader(myFile))
                //    {
                //        newName = this.fileType.SelectedItem.ToString().ToUpper() + "_Warehouse_" + DateTime.Now.ToString("yyyyMMdd") + "_0001";
                //        string[] temp1;
                //        int i = 1;

                //        final.AppendLine("SeqNo,Tag_SN,Action_Code,ITEM_NUM,BOX_NUM,Destination,PO Num,Lot_Num,Roll_Num");
                //        while ((line = myReader.ReadLine()) != null)
                //        {
                //            #region 讀檔，產生驗證碼
                //            if (line.IndexOf("EPC_ID") == -1 && !String.IsNullOrEmpty(line))
                //            {
                //                temp1 = line.Split(',');
                //                final.AppendLine(
                //                    i.ToString() + "," +
                //                    temp1[0] + "," +
                //                    (this.actionCode.SelectedItem as ComboboxItem).Value.ToString() + "," +
                //                    temp1[1] + "," + "B012345,1988888888,123456,12"
                //                    );
                //                i++;
                //            }
                //            #endregion
                //        }
                //    }
                //}

                String writePath = Path.Combine(this.outputFilePath.Text, newName);
                int j = 0;
                string duplicatePath = writePath + ".csv";
                while (File.Exists(duplicatePath))
                {
                    if (j == 0)
                    {
                        duplicatePath = writePath + "(1).csv";
                    }
                    else
                    {
                        duplicatePath = writePath + "(" + j + ").csv";
                    }
                    j++;
                }
                writePath = duplicatePath;
                lastFileName = Path.GetFileName(writePath);
                using (FileStream CSVFile = System.IO.File.Open(writePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(CSVFile))
                    {
                        sw.WriteLine(final.ToString());        //寫入文字 
                    }
                }
                AddTextToResult(string.Format("檔案已產生：{0}", writePath));
            }
            catch (Exception ex)
            {
                AddTextToResult(string.Format("系統錯誤：{0}", ex.Message));
            }
        }

        public class TagInfo
        {   
            public string EPC_ID {get;set;}
            public string TAG_PART_NUMBER { get; set; }
        }

        /// <summary> 重繪檔名規則說明清單
        /// </summary>
        private void ResetListView()
        {
            this.listView1.Clear();
            this.listView1.Columns.Add("itemName", "項目");
            this.listView1.Columns.Add("itemDesc", "說明");
            listView1.Columns["itemName"].Width = 100;
            listView1.Columns["itemDesc"].Width = 370;
        }

        /// <summary> 拆檔名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string[] SplitFileNameRule(string fileName)
        {
            string file = fileName.Substring(0, fileName.LastIndexOf('.'));
            string[] res = file.Split('_');
            return res;
        }

        /// <summary> 禁止拖曳檔名規則說明清單的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        /// <summary> 選擇存檔路徑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            this.outputFilePath.Text = folderBrowserDialog1.SelectedPath;
        }

        /// <summary> ComboBox使用物件
        /// </summary>
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary> 按下Help按鈕的事件，開啟提示視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void help_Click(object sender, EventArgs e)
        {
            if (helpForm != null)
            {
                helpForm.Dispose();
            }
            helpForm = new HelpForm();
            helpForm.Show();
        }

        /// <summary> 拖曳檔案效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        /// <summary> 拖曳檔案事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            for (int i = 0; i < s.Length; i++)
            {
                this.txtPath.Text = s[i];
            }
        }

        /// <summary> 開啟產檔目錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openDirector_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(lastFileName))
            {
                System.Diagnostics.Process.Start(this.outputFilePath.Text);
            }
            else
            {
                System.Diagnostics.Process.Start(@"explorer.exe", @"/select," + lastFileName);
            }

        }

    }
}
