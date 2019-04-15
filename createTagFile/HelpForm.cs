using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace createTagFile
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        { 
            this.richTextBox1.Text = 
@"1. 下單：至【產品管理 / eTag產品管理】的【eTag個人化資料(TPF)檔案維護(BMS-PDM-001-004)】新增檔案。
    a. 於[BMS_TAG_TPF_REQUEST]建立一筆資料。

2. 產生對內TPF給SIS：
	a. URL：{IP}/BMS/Etag/EtagTPFMaintain/BatchCreateEtagTPF
	b. 排程名稱：BatchCreateEtagTPF(產生給SIS的對內TPF)
	c. 排程位置：EtagTPFMaintainController.cs / BatchCreateEtagTPF()
	d. BMS_OPR_JOB：PROGRAM_NAME =""BatchCreateEtagTPF"" =>紀錄匯出對內TPF
	e. 排程主要流程：
		i. 至BMS_TAG_TPF_REQUEST找PRODUCE_STATUS = ""0"" (未產檔) &  APPROVAL_STATUS = ""1""(已核可)
		ii. 取得BMS_OPR_IO：判斷要給哪一個SIS目錄，找BMS_PRO_CODES，CODE_TYPE=54 & NUMBER=""1""，取出多個CODE_ID，比對BMS_TAG_TPF_REQUEST.PURPOSE_ID與CODE_ID，若有對應到，看CODE_ID對應的NUMBER為1或0
			1) NUMBER=""1""：正式用=>將給SIS的檔案放到BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ExportInnerTPF""
			2) NUMBER=""0""：測試用=>將給SIS的檔案放到BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ExportInnerTPF_TEST""
		iii. 取得批次序號：至BMS_PRO_CODES，CODE_TYPE=150，取得ETAG的批次序號(etag編碼需要)，取完後加上產製數量後，再回壓回去，下次取用
		iv. (若有中間有錯，全部要rollback)
			1) 建立BMS_TAG_ETAG
			2) 先產檔在目前應用程式網站目錄底下的Temp目錄底下，在將檔案上傳到SIS的目錄，給SIS的目錄取決上方(2.d.ii)之路徑
			3) 並同時在給SIS的目錄也新增一個 bak 目錄存放備份檔案。
			4) 檔名：TPFData_FETC_BatchNo_YYYYMMDD_流水號_toSIS.csv
			5) commit資料。

3. 定時接收SIS檔案匯入至BMS系統，並同時產生對外TPF給製卡廠
	a. URL：{IP}/BMS/Etag/TPFBatch/CreateOutTPF
	b. 排程名稱：ImportInnerTPF_SIS(匯入SIS的對內TPF)
	c. 排程位置：TPFBatchController.cs / CreateOutTPF()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ImportInnerTPF_SIS""  =>紀錄匯入對內TPF
	e. BMS_OPR_JOB：PROGRAM_NAME = ""BatchCreateOuterEtagTPF"" =>紀錄匯出對外TPF
	f. 排程主要流程：
		i. 匯入對內TPF，必須存在兩個相同檔名，複檔名必須為
			1) 查詢SIS正式Key回傳的檔案路徑：Web.Config->AppSetting->key:TPFJobDownloadFolder
			2) 查詢SIS測試Key回傳的檔案路徑：Web.Config->AppSetting->key:TPFJobDownloadFolder_Test
		ii. 從SIS對應的回傳目錄下載要匯入檔案，先放到目前應用程式網站目錄底下的Temp\TPF_TEMP\的目錄底下，在刪除原本來源的檔案。
		iii. 檔案逐一處理
			1) 將Temp\TPF_TEMP\底下的檔案備份到BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ImportInnerTPF""
			2) 因為Temp\TPF_TEMP\有包含.ok檔，故遇到.ok檔則刪除在往下一個處理。
			3) 匯入SIS的對內TPF：更新BMS_TAG_ETAG，將防偽碼與EPCID回壓。
			4) 產生對外TPF檔：先將檔案產生在Temp\TPF_TEMP\底下，在將檔案上傳到FTP與備份
				a) 對外TPF檔案的備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ExportOuterTPF"" 
				b) 對外TPF上傳到FTP的路徑：Web.Config->AppSetting->key:TPFJobUploadOuterFolder
			5) 若對外TPF產檔失敗，則可人工做檔
				SELECT
				        CONCAT(MODEL.MODEL_NO, ',',
				        E.EPC_ID, ',1,', E.ANTICOUNTERFEIT1 + ',', E.ANTICOUNTERFEIT2, ',,,,'
				        ) AS 'PROD_TYPE,EPC_ID,TRANSACTION_COUNTER,ANTICOUNTERFEIT_1,ANTICOUNTERFEIT_2,DATA_1,DATA_2,DATA_3,DATA_4'
				FROM BMS_TAG_ETAG E WITH (NOLOCK)
				INNER JOIN BMS_TAG_TPF_REQUEST REQ WITH (NOLOCK)
				        ON E.TPF_REQ_ID = REQ.REQ_ID
				INNER JOIN BMS_TAG_MODEL_NO MODEL WITH (NOLOCK)
				        ON REQ.MODEL_NO_ID = MODEL.MODEL_NO_ID
				WHERE E.TPF_REQ_ID = 13
			6) 發信：發送匯入SIS的對內TPF的結果。
			7) 發信：發送產製對外TPF的結果。

4. 定時接收TRFF檔，並產製TRFF210檔案
	a. URL：{IP}/BMS/Etag/ImportTRFFJob/Index
	b. 排程名稱：ImportTRFF(匯入TRFF)
	c. 排程位置：ImportTRFFJobController.cs / Index()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ImportTRFF""  => 紀錄匯入TRFF
	e. BMS_OPR_JOB：PROGRAM_NAME = ""ExportTRFF210"" => 紀錄匯出TRFF210
	f. 排程主要流程：
		i. 取得FTP上的TRFF檔：Web.Config->AppSetting->key:TRFFFolder
		ii. FTP登入帳號：Web.Config->AppSetting->key:TRFFFTPUser / FTP登入密碼：Web.Config->AppSetting->key:TRFFFTPPassword
		iii. 下載檔案至暫存目錄：Web.Config->AppSetting->key:TRFFFolderTemp
		iv. TRFF備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ImportTRFF""
		v. 從檔案暫存目錄逐一處理(檔案內若有失敗就往下一個ETAG繼續處理，不rollback)
			1) 建立BMS_TAG_TRFF
			2) 將檔案內的資訊更新回BMS_TAG_ETAG
			3) 針對每一筆ETAG紀錄BMS_TAG_IO_DETAIL
			4) 寫入Portal歷程
			5) 如果有序號控管=>使用料耗去查BMS_TAG_MODELNO_MAPPING，查出SEQ_CONTROL=""1""，則要排入排程，產TRFF檔給WH
		vi. 產置TRFF210檔：當TRFF是原規範或是補檔時，才需要產TRFF210，(產製失敗可從【產品相關物流管理->產生Tag交貨回覆檔(TRFF210)】進行補產)
			1) 要上傳TRFF210的路徑：Web.Config->AppSetting->key:TRFF210JobFolder
			2) FTP登入帳號：Web.Config->AppSetting->key:TRFF210FTPUser / FTP登入密碼：Web.Config->AppSetting->key:TRFF210FTPPassword
			3) TRFF210備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ProduceTRFF210""
		vii. 發信：發送匯入TRFF的結果。
		viii. 發信：發送匯出TRFF210的結果。

5. 定時產製TRFF檔給WH
	a. URL：{IP}/BMS/Etag/ProduceTRFFJob/Index
	b. 排程名稱：ProduceTRFFJob(產生TRFF)
	c. 排程位置：ProduceTRFFJobController.cs / Index()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ProduceTRFFCSV""  => 紀錄匯出TRFF
	e. 排程主要流程：
		i. 取得待處理資料的查詢條件
		ii. 產生 CSV 檔
		iii. 上傳檔案
			1) TRFF匯出備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ProduceTRFF""
			2) TRFF匯出上傳FTP路徑：Web.Config->AppSetting->key:TRFFCSVJobFolder
			3) FTP登入帳號：Web.Config->AppSetting->key:TRFFCSVJobFTPUser/ FTP登入密碼：Web.Config->AppSetting->key:TRFFCSVJobFTPPassword
		iv. 發信：發送匯出TRFF的結果。

6. 定時接收WT100，並產製WT110
	a. URL：{IP}/BMS/ImportWT100Job/Index
	b. 排程名稱：ImportWT100
	c. 排程位置：ImportWT100JobController.cs / Index()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ImportWT100""  => 紀錄匯入WT100
	e. BMS_OPR_JOB：PROGRAM_NAME = ""ExportWT110""  => 紀錄匯出WT110
	f. 排程主要流程：
		i. WT100匯入的FTP路徑：Web.Config->AppSetting->key:WT100JobFolder
		ii. FTP登入帳號：Web.Config->AppSetting->key:WT100FTPUser / FTP登入密碼：Web.Config->AppSetting->key:WT100FTPPassword
		iii. 下載檔案至暫存目錄：Web.Config->AppSetting->key:WT100FolderTemp
		iv. WT100匯入的備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ImportWT100""
		v. 從檔案暫存目錄逐一處理(檔案內若有失敗就往下一個ETAG繼續處理，不rollback)
			1) 建立BMS_TAG_IO
			2) 將檔案內的資訊更新回BMS_TAG_ETAG
			3) 針對每一筆ETAG紀錄BMS_TAG_IO_DETAIL
			4) 寫入Portal歷程
		vi. 產置WT110檔：(產製失敗可從【產品相關物流管理->產生Tag入庫資料回覆檔(WT110)】進行補產)
			1) 要上傳WT110的路徑：Web.Config->AppSetting->key:WT110JobFolder
			2) FTP登入帳號：Web.Config->AppSetting->key:WT110FTPUser / FTP登入密碼：Web.Config->AppSetting->key:WT110FTPPassword
			3) WT110備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ProduceWT110""
		vii. 發信：發送匯入WT100的結果。
		viii. 發信：發送匯出WT110的結果。

7. 定時接收WT200，並產製WT210
	a. URL：{IP}/BMS/ImportWT200Job/Index
	b. 排程名稱：ImportWT200
	c. 排程位置：ImportWT200JobController.cs / Index()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ImportWT200""  => 紀錄匯入WT200
	e. BMS_OPR_JOB：PROGRAM_NAME = ""ExportWT210""  => 紀錄匯出WT210
	f. 排程主要流程：
		i. WT200匯入的FTP路徑：Web.Config->AppSetting->key:WT200JobFolder
		ii. FTP登入帳號：Web.Config->AppSetting->key:WT200FTPUser / FTP登入密碼：Web.Config->AppSetting->key:WT200FTPPassword
		iii. 下載檔案至暫存目錄：Web.Config->AppSetting->key:WT200FolderTemp
		iv. WT200匯入的備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ImportWT200""
		v. 從檔案暫存目錄逐一處理(檔案內若有失敗就往下一個ETAG繼續處理，不rollback)
			1) 建立BMS_TAG_IO
			2) 將檔案內的資訊更新回BMS_TAG_ETAG
			3) 針對每一筆ETAG紀錄BMS_TAG_IO_DETAIL
			4) 寫入Portal歷程
		vi. 產製WT210檔：(產製失敗可從【產品相關物流管理->產生Tag出貨資料回覆檔(WT210)】進行補產)
			1) 要上傳WT210的路徑：Web.Config->AppSetting->key:WT110JobFolder
			2) FTP登入帳號：Web.Config->AppSetting->key:WT210FTPUser / FTP登入密碼：Web.Config->AppSetting->key:WT210FTPPassword
			3) WT210備份路徑：BMS_OPR_IO.FILE_FOLDER，IO_NAME=""ProduceWT210""
		vii. 發信：發送匯入WT200的結果。
		viii. 發信：發送匯出WT210的結果。";
            this.richTextBox2.Text =
@"1. 下單：至【產品管理 / eTag產品管理】的【P3檔產檔維護(BMS-PDM-001-010)】新增檔案。
	a. 於[BMS_TAG_P3]建立一筆資料。
2. 產生P3給製卡廠：
	a. URL：{IP}/BMS/Etag/P3FileMaintenance/BatchExport
	b. 排程名稱：P3FileMainten(匯出P3檔)
	c. 排程位置：P3FileMaintenanceController.cs / BatchExport()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""P3FileMaintenance"" =>紀錄匯出P3檔
	e. 排程主要流程：
		i. 取得產製P3檔的備份路徑：BMS_OPR_IO.SUCCESS_FOLDER，IO_NAME=""P3檔產檔""
		ii. 產生BMS_TAG_CARD
		iii. 如果P3主檔的「是否有IFF檔」，如果主檔選擇「是」，則需要等到IFF檔回來後，卡片狀態才變更為「W」；如果主檔選擇「否」，則產生BMS_TAG_CARD的時候，卡片狀態為「W」
		iv. 上傳FTP路徑：Web.Config->AppSetting->key:P3_PATH_FTP
		v. FTP帳密：Web.Config->AppSetting->key:TPFFTPUser；key:TPFFTPPassword
		vi. 每10000筆切一個檔
		vii. 發信：發送產製P3檔的結果
3. 定時接收匯入IFF檔
	a. URL：{IP}/BMS/Etag/ImportIFF/Import
	b. 排程名稱：ImportIFF(匯入IFF檔)
	c. 排程位置：ImportIFFController.cs / Import()
	d. BMS_OPR_JOB：PROGRAM_NAME = ""ImportIFF"" =>紀錄匯入IFF檔
	e. 排程主要流程
		i. 匯入IFF的FTP路徑：Web.Config->AppSetting->key:ImportIFFSourcePath
		ii. 取得備份IFF的路徑：BMS_OPR_IO.SUCCESS_FOLDER，IO_NAME=""ImportIFF""
		iii. 發信：發送匯入IFF檔的結果";


        }
    }
}
