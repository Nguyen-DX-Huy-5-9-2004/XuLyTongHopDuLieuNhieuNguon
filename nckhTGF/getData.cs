using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using connectSqlSever;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Runtime.InteropServices;
using System.IO;
using DevExpress.Spreadsheet.Drawings;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraRichEdit.Model;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraCharts.Native;
using DevExpress.CodeParser;
using Microsoft.Web.WebView2.WinForms;
using System.Security.Policy;
using System.Threading.Tasks;
using static nckhTGF.SheetSource;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DevExpress.XtraSpreadsheet.Model;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;

namespace nckhTGF
{
    public partial class getData : RibbonForm
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private formConnectDB FormConnectDB;
        private FormCNDBmySQL formCNDBmySQL;
        AccordionControlElement currentSelectedElement = null;
        private AccordionControlElement previousSelectedElement = null;
        private bool isCheckedChanged = false;
        int loadingProgress = 0;
        string webUrlToNavigate = "";
        private Dictionary<string, SheetSource> sheetSources = new();
        private List<List<int>> danhSachHangTrung;
        private int currentDuplicateIndex = 1;
        private System.Windows.Forms.Timer debounceTimer = new System.Windows.Forms.Timer();
        private List<int> allDuplicateRowsOrdered = null;

        public getData()
        {
            InitializeComponent();
            spreadsheetControl.CellValueChanged += SpreadsheetControl1_CellValueChanged;
            spreadsheetControl.Document.SheetInserted += Spreadsheet_SheetInserted;
            spreadsheetControl.Document.SheetRemoved += Spreadsheet_SheetRemoved;
            spreadsheetControl.Document.SheetRenamed += Spreadsheet_SheetRenamed;

            spreadsheetControl.ActiveSheetChanged += SpreadsheetControl1_ActiveSheetChanged;
            UpdateSheetListToComboBox();


            debounceTimer.Interval = 1800;
            debounceTimer.Tick += DebounceTimer_Tick;

            treeView1.CheckBoxes = true;
            webView21.Dock = DockStyle.Fill;
            webView21.Source = new Uri("about:blank");
        }
        private void getData_Load(object sender, EventArgs e)
        {
            this.Hide();
            
            
            //MessageBox.Show("Gán tên 1");
            using (loginForm LGform = new loginForm())
            {
                var result = LGform.ShowDialog();
                //MessageBox.Show(result.ToString());
                //MessageBox.Show(LGform.ShowDialog().ToString());
                //MessageBox.Show(DialogResult.OK.ToString());
                if (result != DialogResult.OK)
                {
                    this.Close();
                }
            }

            SetAccordionItemDefaultStyle(accordionControlElementTable);
            SetAccordionItemDefaultStyle(accordionControlElementSDLK);
            SetAccordionItemDefaultStyle(accordionControlElementReport);
            //MessageBox.Show(loginForm.TypeUser);
            if (loginForm.TypeUser == "SV")//Client
            {
                //MessageBox.Show("Gán tên SV");
                //select [Họ tên] from [dataNCKH2].[dbo].[AccSV] where [Mã sinh viên] = 124
                //btnDone.Enabled = false;
                label4.Text = "Sinh viên: " + loginForm.CodeUser + "\n";
                string connectionString = "Server=TUGEND-FAKAS\\TGF_SQLSEVER;Database=dataNCKH2;User Id=sa;Password=Huy@0865567532;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "SELECT [Họ tên] FROM [dbo].[AccSV] WHERE [Mã sinh viên] = @MaSV";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaSV", loginForm.CodeUser);

                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                string hoTen = result.ToString();
                                label4.Text += "Họ tên: " + hoTen;
                            }
                            else
                            {
                                label4.Text += "Họ tên: @Không tìm thấy.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi truy vấn tên sinh viên: " + ex.Message);
                    }
                }
            }
            else if (loginForm.TypeUser == "GV")
            {
                //MessageBox.Show("Gán tên GV");
                label4.Text = "Giảng viên: " + loginForm.CodeUser + "\n";
                string connectionString = "Server=TUGEND-FAKAS\\TGF_SQLSEVER;Database=dataNCKH2;User Id=sa;Password=Huy@0865567532;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string query = "SELECT [Họ tên] FROM [dbo].[AccGV] WHERE [Mã giảng viên] = @MaGV";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaGV", loginForm.CodeUser);

                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                string hoTen = result.ToString();
                                label4.Text += "Họ tên: " + hoTen;
                            }
                            else
                            {
                                label4.Text += "Họ tên: @Không tìm thấy.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi truy vấn tên sinh viên: " + ex.Message);
                    }
                }
            }
            else if (loginForm.TypeUser == "Client")
            {
                //MessageBox.Show("Gán tên Khách");
                label4.Text = "Người dùng: \'Khách\'" + "\n" + " ";
            }

            this.Show();
        }
        private void markBook(object sender, EventArgs e)
        {
            accordionControl1.SelectedElement = accordionControlElementTable;
        }
        private void SetAccordionItemDefaultStyle(AccordionControlElement element)
        {
            element.Appearance.Normal.BackColor = Color.FromArgb(200, 200, 255);
            element.Appearance.Normal.Font = new Font(accordionControl1.Font, FontStyle.Regular);
            element.Appearance.Normal.Options.UseBackColor = true;
            element.Appearance.Normal.Options.UseFont = true;
        }
        public void ResetApp()
        {
            //this.Hide();

            getData_Load(this, EventArgs.Empty);
            spreadsheetControl.CreateNewDocument();
            spreadsheetControlData.CreateNewDocument();
            UpdateSheetListToComboBox();
            UpdateTreeView();
            webView21.Dock = DockStyle.Fill;
            webView21.Source = new Uri("about:blank");
            foreach (Form frm in Application.OpenForms.Cast<Form>().ToList())
            {
                if (frm != this)
                {
                    frm.Close();
                }
            }

        }

        private void SpreadsheetControl1_ActiveSheetChanged(object sender, EventArgs e)
        {
            var currentSheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            string sheetName = currentSheet.Name;

            if (cbbSLTaable.SelectedItem == null || cbbSLTaable.SelectedItem.ToString() != sheetName)
            {
                cbbSLTaable.SelectedItem = sheetName;
            }
            UpdateTreeView();
        }

        private void SpreadsheetControl1_CellValueChanged(object sender, DevExpress.XtraSpreadsheet.SpreadsheetCellEventArgs e)
        {
            if (e.Cell.RowIndex == 0)
            {
                UpdateTreeView();
            }
        }

        private void Spreadsheet_SheetInserted(object sender, DevExpress.Spreadsheet.SheetInsertedEventArgs e)
        {
            UpdateTreeView();
        }


        private void Spreadsheet_SheetRemoved(object sender, DevExpress.Spreadsheet.SheetRemovedEventArgs e)
        {
            UpdateTreeView();
        }

        private void Spreadsheet_SheetRenamed(object sender, DevExpress.Spreadsheet.SheetRenamedEventArgs e)
        {
            UpdateSheetListToComboBox();
            UpdateTreeView();
        }

        private void moveFirst(object sender, ItemClickEventArgs e)
        {
            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();

            int firstRecordRowIndex = 1;
            int firstColumnIndex = usedRange.LeftColumnIndex;
            sheet.SelectedCell = sheet.Cells[firstRecordRowIndex, firstColumnIndex];

            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);

            this.BeginInvoke(new Action(() =>
            {
                SendKeys.SendWait("{UP}");
                this.BeginInvoke(new Action(() =>
                {
                    SendKeys.SendWait("{DOWN}");
                }));
            }));
        }
        private void moveEnd(object sender, ItemClickEventArgs e)
        {
            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();

            int lastRowIndex = usedRange.BottomRowIndex - 1;
            int firstColumnIndex = usedRange.LeftColumnIndex;
            sheet.SelectedCell = sheet.Cells[lastRowIndex, firstColumnIndex];
            SetForegroundWindow(spreadsheetControl.Handle);
            SendKeys.SendWait("{DOWN}");

        }
        private void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                UncheckAllNodes(node.Nodes);
            }
        }

        private void CheckAllNodes(TreeNode node, bool isChecked)
        {
            node.Checked = isChecked;
            foreach (TreeNode child in node.Nodes)
            {
                CheckAllNodes(child, isChecked);
            }
        }

        private void closeThisSheetClick(object sender, ItemClickEventArgs e)
        {
            //var workbook = spreadsheetControl.Document;
            //DevExpress.Spreadsheet.Worksheet activeSheet = workbook.Worksheets.ActiveWorksheet;

            //string removedSheetName = activeSheet.Name;

            //if (workbook.Worksheets.Count > 1)
            //{
            //    workbook.Worksheets.Remove(activeSheet);
            //}
            //else
            //{
            //    try
            //    {
            //        if (workbook.Worksheets["NewSheet"] == null)
            //        {
            //            DevExpress.Spreadsheet.Worksheet newSheet = workbook.Worksheets.Add();
            //            newSheet.Name = "NewSheet";
            //            workbook.Worksheets.ActiveWorksheet = newSheet;
            //        }
            //        workbook.Worksheets.Remove(activeSheet);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Đã xóa hết các sheet cũ, không thể xóa nữa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}

            //if (workbook.Worksheets.Count > 0)
            //{
            //    spreadsheetControl.Document.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
            //    cbbSLTaable.SelectedItem = workbook.Worksheets[0].Name;
            //}

            //UpdateSheetListToComboBox();
            //UpdateTreeView();
            var workbook = spreadsheetControl.Document;
            DevExpress.Spreadsheet.Worksheet activeSheet = workbook.Worksheets.ActiveWorksheet;
            string removedSheetName = activeSheet.Name;

            if (workbook.Worksheets.Count > 1)
            {
                workbook.Worksheets.Remove(activeSheet);
            }
            else
            {
                if (workbook.Worksheets["NewSheet"] == null)
                {
                    DevExpress.Spreadsheet.Worksheet newSheet = workbook.Worksheets.Add();
                    try
                    {
                        newSheet.Name = "NewSheet";
                    }
                    catch
                    {
                        newSheet.Name = Guid.NewGuid().ToString();
                    }
                    workbook.Worksheets.ActiveWorksheet = newSheet;
                }
                try
                {
                    workbook.Worksheets.Remove(activeSheet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xóa hết các sheet cũ, không thể xóa nữa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (workbook.Worksheets.Count > 0)
            {
                spreadsheetControl.Document.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
            }

            UpdateSheetListToComboBox();
            if (cbbSLTaable.Items.Contains(workbook.Worksheets[0].Name))
            {
                cbbSLTaable.SelectedItem = workbook.Worksheets[0].Name;
            }

            UpdateTreeView();
        }


        private void closeAllSheet(object sender, ItemClickEventArgs e)
        {
            var workbook = spreadsheetControl.Document;
            try
            {
                DevExpress.Spreadsheet.Worksheet newSheet;

                bool hasNewSheet1 = workbook.Worksheets.Any(ws => ws.Name == "NewSheet1");

                newSheet = workbook.Worksheets.Add();
                newSheet.Name = hasNewSheet1 ? "NewSheet2" : "NewSheet1";

                workbook.Worksheets.ActiveWorksheet = newSheet;

                for (int i = workbook.Worksheets.Count - 1; i >= 0; i--)
                {
                    var sheet = workbook.Worksheets[i];
                    if (sheet != newSheet)
                        workbook.Worksheets.Remove(sheet);
                }

                if (newSheet.Name == "NewSheet2" && !workbook.Worksheets.Any(ws => ws.Name == "NewSheet1"))
                {
                    newSheet.Name = "NewSheet1";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý sheet: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateSheetListToComboBox();
            UpdateTreeView();
        }

        public static void FormatTable(SpreadsheetControl spreadsheet)
        {
            if (spreadsheet == null || spreadsheet.Document == null)
                return;

            IWorkbook workbook = spreadsheet.Document;
            DevExpress.Spreadsheet.Worksheet worksheet = spreadsheet.ActiveWorksheet;

            DevExpress.Spreadsheet.CellRange usedRange = worksheet.GetUsedRange();

            DevExpress.Spreadsheet.Row headerRow = worksheet.Rows[usedRange.TopRowIndex];
            for (int col = usedRange.LeftColumnIndex; col <= usedRange.RightColumnIndex; col++)
            {
                DevExpress.Spreadsheet.Cell cell = worksheet.Cells[headerRow.Index, col];
                cell.Alignment.WrapText = true;
                cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            }

            headerRow.Height = 40;

            for (int col = usedRange.LeftColumnIndex; col <= usedRange.RightColumnIndex; col++)
            {
                worksheet.Columns[col].AutoFit();
            }
        }

        private void FormatHeaderRow()
        {
            IWorkbook workbook = spreadsheetControl.Document;

            for (int sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
            {
                DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[sheetIndex];

                worksheet.Rows[0].Alignment.WrapText = true;

                worksheet.Rows[0].AutoFit();

                int lastRowIndex = worksheet.Rows.LastUsedIndex;
                int lastColIndex = worksheet.Columns.LastUsedIndex;

                if (lastRowIndex < 0 || lastColIndex < 0)
                    continue;

                DevExpress.Spreadsheet.CellRange usedRange = worksheet.Range.FromLTRB(0, 0, lastColIndex, lastRowIndex);
                usedRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                usedRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Columns.AutoFit(0, lastColIndex);
            }
        }


        private void AdjustColumnWidth()
        {
            IWorkbook workbook = spreadsheetControl.Document;
            for (int sheetIndex = 1; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
            {
                DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[sheetIndex];

                int totalColumns = worksheet.Columns.LastUsedIndex + 1;

                int ngaySinhColumnIndex = -1;
                for (int col = 0; col < totalColumns; col++)
                {
                    string headerText = worksheet.Cells[0, col].DisplayText;

                    if (headerText.Trim().Equals("Ngày sinh", StringComparison.OrdinalIgnoreCase))
                    {
                        ngaySinhColumnIndex = col;
                        break;
                    }
                }

                if (ngaySinhColumnIndex == -1)
                {
                    continue;
                }

                int beforeLastColumnIndex = totalColumns - 3;

                for (int col = ngaySinhColumnIndex + 1; col < beforeLastColumnIndex; col++)
                {
                    string headerText = worksheet.Cells[0, col].DisplayText;

                    if (headerText.IndexOf("Xếp loại", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        continue;
                    }

                    worksheet.Columns[col].Width = 180;
                }
            }
        }
        private void UpdateSheetListToComboBox()
        {
            cbbSLTaable.Items.Clear();

            foreach (DevExpress.Spreadsheet.Worksheet sheet in spreadsheetControl.Document.Worksheets)
            {
                cbbSLTaable.Items.Add(sheet.Name);
            }

            if (cbbSLTaable.Items.Count > 0)
                cbbSLTaable.SelectedIndex = 0;
        }
        //private void UpdateTreeView()
        //{
        //    //MessageBox.Show("UpdateTreeView()");
        //    treeView1.Nodes.Clear();
        //    var workbook = spreadsheetControl.Document;
        //    foreach (DevExpress.Spreadsheet.Worksheet sheet in spreadsheetControl.Document.Worksheets)
        //    {
        //        if (sheet.Name == "Sheet1") continue;
        //        TreeNode rootNode = new TreeNode(sheet.Name);
        //        DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();
        //        int colCount = usedRange.ColumnCount;
        //        int topRow = usedRange.TopRowIndex;
        //        int bottomRow = usedRange.BottomRowIndex;
        //        int startCol = usedRange.LeftColumnIndex;

        //        for (int i = 0; i < colCount; i++)
        //        {
        //            int colIndex = startCol + i;

        //            string fieldName = sheet.Cells[topRow, colIndex].DisplayText;
        //            if (!string.IsNullOrWhiteSpace(fieldName))
        //            {
        //                TreeNode fieldNode = new TreeNode(fieldName);
        //                fieldNode.Checked = false;
        //                rootNode.Nodes.Add(fieldNode);
        //            }
        //        }
        //        rootNode.Checked = false;
        //        rootNode.Expand();
        //        treeView1.Nodes.Add(rootNode);
        //    }
        //    string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
        //    DevExpress.Spreadsheet.Worksheet sheet1 = spreadsheetControl.Document.Worksheets[selectedSheetName];

        //    if (string.IsNullOrEmpty(selectedSheetName)) return;
        //    if (sheet1 != null)
        //    {
        //        spreadsheetControl.Document.Worksheets.ActiveWorksheet = sheet1;
        //    }

        //    UncheckAllNodes(treeView1.Nodes);

        //    foreach (TreeNode node in treeView1.Nodes)
        //    {
        //        if (node.Text == selectedSheetName)
        //        {
        //            CheckAllNodes(node, true);
        //            break;
        //        }
        //    }
        //}

        private void UpdateTreeView()
        {
            treeView1.AfterCheck -= changCheckEvent;

            treeView1.Nodes.Clear();
            var workbook = spreadsheetControl.Document;
            foreach (DevExpress.Spreadsheet.Worksheet sheet in workbook.Worksheets)
            {
                if (sheet.Name == "Sheet1") continue;

                TreeNode rootNode = new TreeNode(sheet.Name);
                DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();
                int colCount = usedRange.ColumnCount;
                int topRow = usedRange.TopRowIndex;
                int bottomRow = usedRange.BottomRowIndex;
                int startCol = usedRange.LeftColumnIndex;

                for (int i = 0; i < colCount; i++)
                {
                    int colIndex = startCol + i;
                    string fieldName = sheet.Cells[topRow, colIndex].DisplayText;

                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        TreeNode fieldNode = new TreeNode(fieldName);

                        var dataStartRow = topRow + 1;
                        var columnRange = sheet.Range.FromLTRB(colIndex, dataStartRow, colIndex, bottomRow);
                        var fontColor = columnRange.Font.Color;

                        fieldNode.Checked = (fontColor.ToArgb() != Color.FromArgb(216, 216, 216).ToArgb());
                        rootNode.Nodes.Add(fieldNode);
                    }
                }

                rootNode.Checked = true;
                rootNode.Expand();
                treeView1.Nodes.Add(rootNode);
            }

            string selectedSheetName = cbbSLTaable.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedSheetName))
            {
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Text == selectedSheetName)
                    {
                        CheckAllNodes(node, true);
                        break;
                    }
                }
            }

            treeView1.AfterCheck += changCheckEvent;
        }


        private DevExpress.Spreadsheet.Worksheet GetOrCreateSheet(string sheetName)
        {
            //MessageBox.Show("GetOrCreateSheet(string sheetName)");
            var workbook = spreadsheetControl.Document;

            foreach (var sheet in workbook.Worksheets)
            {
                if (sheet.Name == sheetName)
                    return sheet;
            }

            var newSheet = workbook.Worksheets.Add();
            newSheet.Name = sheetName;
            return newSheet;
        }
        private DevExpress.Spreadsheet.Worksheet GetOrCreateSheet(IWorkbook workbook, string desiredName)
        {
            var existingSheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == desiredName);
            if (existingSheet != null)
            {
                existingSheet.Cells.Clear();
                return existingSheet;
            }

            string safeName = desiredName;
            int counter = 1;

            while (workbook.Worksheets.Any(ws => ws.Name == safeName))
            {
                safeName = $"{desiredName}_{counter++}";
            }

            var newSheet = workbook.Worksheets.Add();
            newSheet.Name = safeName;

            return newSheet;
        }
        private void MoveSheet1ToEnd(IWorkbook workbook)
        {
            DevExpress.Spreadsheet.WorksheetCollection sheets = workbook.Worksheets;

            if (!sheets.Any(ws => ws.Name == "Sheet1")) return;

            DevExpress.Spreadsheet.Worksheet sheet1 = sheets["Sheet1"];


            if (sheet1.Index == sheets.Count - 1) return;
            DevExpress.Spreadsheet.Worksheet newSheet = sheets.Add("Sheet1_Temp");
            newSheet.CopyFrom(sheet1);
            sheets.Remove(sheet1);
            newSheet.Name = "Sheet1";
            workbook.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
        }
        private void cbbSLTaable_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbbSLTaable.SelectedItem == null) return;

            //string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
            //if (string.IsNullOrEmpty(selectedSheetName)) return;

            //DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets[selectedSheetName];
            //if (sheet == null) return;

            //spreadsheetControl.Document.Worksheets.ActiveWorksheet = sheet;

            //UncheckAllNodes(treeView1.Nodes);

            //foreach (TreeNode node in treeView1.Nodes)
            //{
            //    if (node.Text == selectedSheetName)
            //    {
            //        CheckAllNodes(node, true);
            //        break;
            //    }
            //}

            //ApplyTreeViewToSheet(selectedSheetName);

            if (cbbSLTaable.SelectedItem == null) return;

            string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedSheetName)) return;

            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets[selectedSheetName];
            if (sheet == null) return;

            spreadsheetControl.Document.Worksheets.ActiveWorksheet = sheet;

            treeView1.AfterCheck -= changCheckEvent;

            UncheckAllNodes(treeView1.Nodes);

            TreeNode nodeToCheck = null;

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text == selectedSheetName)
                {
                    nodeToCheck = node;
                    CheckAllNodes(node, true);
                    break;
                }
            }

            treeView1.AfterCheck += changCheckEvent;

            // Gọi thủ công sự kiện changCheckEvent nếu nodeToCheck không null
            if (nodeToCheck != null)
            {
                var args = new TreeViewEventArgs(nodeToCheck, TreeViewAction.Unknown);
                changCheckEvent(treeView1, args);
            }

            ApplyTreeViewToSheet(selectedSheetName);
        }


        //private void cbbSLTaable_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //MessageBox.Show("cbbSLTaable_SelectedIndexChanged(object sender, EventArgs e)");
        //    string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
        //    DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets[selectedSheetName];
        //    if (string.IsNullOrEmpty(selectedSheetName)) return;
        //    if (sheet != null)
        //    {
        //        spreadsheetControl.Document.Worksheets.ActiveWorksheet = sheet;
        //    }

        //    UncheckAllNodes(treeView1.Nodes);

        //    foreach (TreeNode node in treeView1.Nodes)
        //    {
        //        if (node.Text == selectedSheetName)
        //        {
        //            CheckAllNodes(node, true);
        //            break;
        //        }
        //    }
        //    UpdateTreeView();
        //}
        private void changCheckEvent(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show("bjkb");
            isCheckedChanged = true;
            treeView1.AfterCheck -= changCheckEvent;
            TreeNode currentNode = e.Node;

            if (currentNode.Parent == null)
            {
                foreach (TreeNode child in currentNode.Nodes)
                {
                    child.Checked = currentNode.Checked;
                }
            }
            else
            {
                string sheetName = currentNode.Parent.Text;
                DevExpress.Spreadsheet.Worksheet sheet;
                try
                {
                    sheet = spreadsheetControl.Document.Worksheets[sheetName];
                }
                catch
                {
                    return;
                }
                var usedRange = sheet.GetUsedRange();
                if (usedRange == null || usedRange.RowCount == 0 || usedRange.ColumnCount == 0)
                    return;


                int topRow = sheet.GetUsedRange().TopRowIndex;
                int bottomRow = sheet.GetUsedRange().BottomRowIndex;
                int startCol = sheet.GetUsedRange().LeftColumnIndex;
                int colCount = sheet.GetUsedRange().ColumnCount;

                int colIndex = -1;
                for (int i = 0; i < colCount; i++)
                {
                    int col = startCol + i;
                    string header = sheet.Cells[topRow, col].DisplayText;
                    if (header == currentNode.Text)
                    {
                        colIndex = col;
                        break;
                    }
                }

                if (colIndex != -1)
                {
                    int dataStartRow = topRow + 1;
                    if (dataStartRow <= bottomRow)
                    {
                        var columnRange = sheet.Range.FromLTRB(colIndex, dataStartRow, colIndex, bottomRow);

                        if (currentNode.Checked)
                        {
                            columnRange.Font.Color = Color.Black;
                        }
                        else
                        {
                            columnRange.Font.Color = Color.FromArgb(216, 216, 216);
                        }
                    }
                }
            }
            if (debounceTimer == null)
            {
                debounceTimer = new System.Windows.Forms.Timer();
                debounceTimer.Interval = 1800;
                debounceTimer.Tick += DebounceTimer_Tick;
            }
            debounceTimer.Stop();
            debounceTimer.Start();
            treeView1.AfterCheck += changCheckEvent;
        }
        private void ApplyTreeViewToSheet(string sheetName)
        {
            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets[sheetName];
            if (sheet == null) return;

            var usedRange = sheet.GetUsedRange();
            if (usedRange == null || usedRange.RowCount == 0 || usedRange.ColumnCount == 0)
                return;

            int topRow = usedRange.TopRowIndex;
            int bottomRow = usedRange.BottomRowIndex;
            int startCol = usedRange.LeftColumnIndex;
            int colCount = usedRange.ColumnCount;

            foreach (TreeNode sheetNode in treeView1.Nodes)
            {
                if (sheetNode.Text == sheetName)
                {
                    foreach (TreeNode colNode in sheetNode.Nodes)
                    {
                        string colName = colNode.Text;
                        bool isChecked = colNode.Checked;

                        int colIndex = -1;
                        for (int i = 0; i < colCount; i++)
                        {
                            int col = startCol + i;
                            string header = sheet.Cells[topRow, col].DisplayText;
                            if (header == colName)
                            {
                                colIndex = col;
                                break;
                            }
                        }

                        if (colIndex != -1)
                        {
                            int dataStartRow = topRow + 1;
                            if (dataStartRow <= bottomRow)
                            {
                                var columnRange = sheet.Range.FromLTRB(colIndex, dataStartRow, colIndex, bottomRow);

                                if (isChecked)
                                {
                                    columnRange.Font.Color = Color.Black;
                                }
                                else
                                {
                                    columnRange.Font.Color = Color.FromArgb(216, 216, 216);
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }

        private void accordionControl1_SelectedElementChanged(object sender, DevExpress.XtraBars.Navigation.SelectedElementChangedEventArgs e)
        {
            //MessageBox.Show("accordionControl1_SelectedElementChanged(object sender, DevExpress.XtraBars.Navigation.SelectedElementChangedEventArgs e)");
            if (previousSelectedElement != null)
            {
                SetAccordionItemDefaultStyle(previousSelectedElement);
            }

            if (e.Element != null)
            {
                e.Element.Appearance.Normal.BackColor = Color.White;
                e.Element.Appearance.Normal.Options.UseBackColor = true;
                e.Element.Appearance.Normal.Options.UseFont = true;

                previousSelectedElement = e.Element;
            }

            if (e.Element == accordionControlElementTable)
                navigationFrameShow.SelectedPage = navigationPageExcel;
            else if (e.Element == accordionControlElementSDLK)
                navigationFrameShow.SelectedPage = navigationPageSDLK;
            else if (e.Element == accordionControlElementReport)
                navigationFrameShow.SelectedPage = navigationPageReport;
        }
        //kết thúc các hàm default và format

        //CN gọp bảng theo khóa
        private void callFormMergeClick(object sender, ItemClickEventArgs e)
        {
            //MessageBox.Show("callFormMergeClick(object sender, ItemClickEventArgs e)");
            var mergeForm = new MergeSheet(spreadsheetControl);
            mergeForm.ShowDialog();
            FormatHeaderRow();
            UpdateSheetListToComboBox();
            UpdateTreeView();
            //AdjustColumnWidth();
            //MoveSheet1ToEnd(workbook);
        }


        //CN: kết nối sql sever
        private void loadSQLSeverClick(object sender, ItemClickEventArgs e)
        {
            if (FormConnectDB == null || FormConnectDB.IsDisposed)
            {
                FormConnectDB = new formConnectDB();
                FormConnectDB.Show();

            }
            else if (!FormConnectDB.Visible)
            {
                FormConnectDB.Show();
                FormConnectDB.WindowState = FormWindowState.Normal;
                FormConnectDB.BringToFront();
            }
            else
            {
                FormConnectDB.BringToFront();
            }
        }
        //import dữ liệu từ sql sever vào spreadsheet
        //public void LoadDataToSpreadsheet(string connectionString, string[] tables)
        //{
        //    MessageBox.Show("LoadDataToSpreadsheet(string connectionString, string[] tables)");
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        MessageBox.Show("Không tìm thấy thông tin kết nối!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            IWorkbook workbook = spreadsheetControl.Document;
        //            workbook.BeginUpdate();

        //            foreach (string tableName in tables)
        //            {
        //                string query = $"SELECT * FROM [{tableName}]";
        //                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
        //                DataTable dataTable = new DataTable();
        //                adapter.Fill(dataTable);

        //                var sheet = GetOrCreateSheet(workbook, tableName);

        //                for (int col = 0; col < dataTable.Columns.Count; col++)
        //                {
        //                    sheet.Cells[0, col].Value = dataTable.Columns[col].ColumnName;
        //                }

        //                for (int row = 0; row < dataTable.Rows.Count; row++)
        //                {
        //                    for (int col = 0; col < dataTable.Columns.Count; col++)
        //                    {
        //                        sheet.Cells[row + 1, col].Value = dataTable.Rows[row][col]?.ToString();
        //                    }
        //                }
        //                sheet.Columns.AutoFit(0, dataTable.Columns.Count);
        //            }
        //            //FormatTable();
        //            AdjustColumnWidth();
        //            FormatHeaderRow();
        //            MoveSheet1ToEnd(workbook);

        //            workbook.EndUpdate();

        //            UpdateSheetListToComboBox();
        //            UpdateTreeView();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Lỗi khi tải dữ liệu LoadDataToSpreadsheet: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
        public void LoadDataToSpreadsheet(string connectionString, string[] tables)
        {
            using SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();
                var workbook = spreadsheetControl.Document;
                workbook.BeginUpdate();

                foreach (string tableName in tables)
                {
                    string query = $"SELECT * FROM [{tableName}]";
                    SqlDataAdapter adapter = new(query, connection);
                    DataTable dt = new();
                    adapter.Fill(dt);

                    var sheet = GetOrCreateSheet(workbook, tableName);
                    FillSheetFromDataTable(sheet, dt);

                    sheet.Tag = new SheetSource
                    {
                        Type = SourceType.Database,
                        SourcePathOrTableName = tableName,
                        ConnectionString = connectionString,
                        Provider = DatabaseProvider.SqlServer
                    };
                    sheetSources[sheet.Name] = (SheetSource)sheet.Tag;
                }
                //IWorkbook workbookFM = spreadsheetControl.Document;
                //AdjustColumnWidth();
                FormatHeaderRow();
                //MoveSheet1ToEnd(workbook);
                workbook.EndUpdate();
                UpdateSheetListToComboBox();
                UpdateTreeView();
                FinishSheetUpdates(workbook);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            //ShowSheetSourcesInfo();
        }



        //Query SQL sever
        public void LoadDataToSpreadsheet(DataTable data, string sheetName)
        {
            //MessageBox.Show("LoadDataToSpreadsheet(DataTable data, string sheetName)");
            int rowCount = data.Rows.Count + 1;
            int colCount = data.Columns.Count;

            object[,] dataArray = new object[rowCount, colCount];

            for (int col = 0; col < colCount; col++)
            {
                dataArray[0, col] = data.Columns[col].ColumnName;
            }

            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    dataArray[row + 1, col] = data.Rows[row][col]?.ToString();
                }
            }

            spreadsheetControl.BeginUpdate();
            try
            {
                var workbook = spreadsheetControl.Document;

                var oldSheet = workbook.Worksheets.FirstOrDefault(s => s.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
                if (oldSheet != null)
                {
                    workbook.Worksheets.Remove(oldSheet);
                }

                var newSheet = workbook.Worksheets.Add(sheetName);
                var range = newSheet.Range.FromLTRB(0, 0, colCount - 1, rowCount - 1);

                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        range[row, col].Value = CellValue.FromObject(dataArray[row, col]);
                    }
                }
            }
            finally
            {
                spreadsheetControl.EndUpdate();
            }
            IWorkbook workbookFM = spreadsheetControl.Document;
            //AdjustColumnWidth();
            FormatHeaderRow();
            MoveSheet1ToEnd(workbookFM);
            workbookFM.EndUpdate();
            UpdateSheetListToComboBox();
            UpdateTreeView();
        }

        //CN load mySQL
        private void loadDataMySQL(object sender, ItemClickEventArgs e)
        {
            if (formCNDBmySQL == null || formCNDBmySQL.IsDisposed)
            {
                formCNDBmySQL = new FormCNDBmySQL();
            }

            formCNDBmySQL.Show();
            formCNDBmySQL.WindowState = FormWindowState.Normal;
            formCNDBmySQL.BringToFront();
        }
        //public void LoadMySQLTablesToSpreadsheet(string connectionString, string[] tables)
        //{
        //    MessageBox.Show("LoadMySQLTablesToSpreadsheet(string connectionString, string[] tables)");
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        var workbook = spreadsheetControl.Document;
        //        workbook.BeginUpdate();
        //        try
        //        {
        //            foreach (string tableName in tables)
        //            {
        //                string query = $"SELECT * FROM `{tableName}`";
        //                MySqlCommand cmd = new MySqlCommand(query, conn);
        //                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //                DataTable dt = new DataTable();
        //                adapter.Fill(dt);

        //                DevExpress.Spreadsheet.Worksheet sheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == tableName) ?? workbook.Worksheets.Add();
        //                sheet.Name = tableName;

        //                for (int col = 0; col < dt.Columns.Count; col++)
        //                {
        //                    sheet.Cells[0, col].Value = dt.Columns[col].ColumnName;
        //                }
        //                for (int row = 0; row < dt.Rows.Count; row++)
        //                {
        //                    for (int col = 0; col < dt.Columns.Count; col++)
        //                    {
        //                        sheet.Cells[row + 1, col].Value = dt.Rows[row][col]?.ToString();
        //                    }
        //                }
        //            }
        //        }

        //        finally
        //        {
        //            IWorkbook workbookFM = spreadsheetControl.Document;
        //            AdjustColumnWidth();
        //            FormatHeaderRow();
        //            MoveSheet1ToEnd(workbookFM);
        //            workbookFM.EndUpdate();
        //            UpdateSheetListToComboBox();
        //            UpdateTreeView();
        //        }
        //    }
        //} 
        public void LoadMySQLTablesToSpreadsheet(string connectionString, string[] tables)
        {
            using MySqlConnection conn = new(connectionString);
            conn.Open();

            var workbook = spreadsheetControl.Document;
            workbook.BeginUpdate();

            foreach (string tableName in tables)
            {
                string query = $"SELECT * FROM `{tableName}`";
                MySqlCommand cmd = new(query, conn);
                MySqlDataAdapter adapter = new(cmd);
                DataTable dt = new();
                adapter.Fill(dt);

                var sheet = GetOrCreateSheet(workbook, tableName);
                FillSheetFromDataTable(sheet, dt);

                sheet.Tag = new SheetSource
                {
                    Type = SourceType.Database,
                    SourcePathOrTableName = tableName,
                    ConnectionString = connectionString,
                    Provider = DatabaseProvider.MySql
                };
                sheetSources[sheet.Name] = (SheetSource)sheet.Tag;
            }

            FinishSheetUpdates(workbook);
            //ShowSheetSourcesInfo();
        }


        private void ImportFlatFile(object sender, ItemClickEventArgs e)
        {
            //MessageBox.Show("ImportFlatFile(object sender, ItemClickEventArgs e)");
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn file dữ liệu",
                Filter = "Flat Files (*.csv;*.txt;*.tsv)|*.csv;*.txt;*.tsv|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                string separator = PromptForSeparator();

                if (string.IsNullOrWhiteSpace(separator))
                {
                    MessageBox.Show("Nhập dấu phân cách hợp lệ!");
                    return;
                }

                if (separator == "\\t")
                    separator = "\t";

                ImportFlatFileToSpreadsheet(filePath, separator);
                IWorkbook workbookFM = spreadsheetControl.Document;
                //AdjustColumnWidth();
                FormatHeaderRow();
                MoveSheet1ToEnd(workbookFM);
                workbookFM.EndUpdate();
                UpdateSheetListToComboBox();
                UpdateTreeView();
                //ShowSheetSourcesInfo();
            }
        }

        //private void ImportFlatFileToSpreadsheet(string filePath, string separator)
        //{
        //    MessageBox.Show("ImportFlatFileToSpreadsheet(string filePath, string separator)");
        //    try
        //    {
        //        IWorkbook workbook = spreadsheetControl.Document;
        //        DevExpress.Spreadsheet.Worksheet sheet = workbook.Worksheets.ActiveWorksheet;
        //        workbook.BeginUpdate();

        //        sheet.Cells.Clear();

        //        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

        //        if (lines.Length == 0)
        //        {
        //            MessageBox.Show("File rỗng.");
        //            return;
        //        }

        //        string[] headers = lines[0].Split(new string[] { separator }, StringSplitOptions.None);
        //        for (int col = 0; col < headers.Length; col++)
        //        {
        //            sheet.Cells[0, col].Value = headers[col];
        //        }
        //        var data = new List<object[]>();

        //        for (int row = 1; row < lines.Length; row++)
        //        {
        //            string[] values = lines[row].Split(new string[] { separator }, StringSplitOptions.None);
        //            data.Add(values.Cast<object>().ToArray());
        //        }
        //        for (int row = 0; row < data.Count; row++)
        //        {
        //            for (int col = 0; col < headers.Length; col++)
        //            {
        //                sheet.Cells[row + 1, col].SetValue(data[row][col]);
        //            }
        //        }
        //        sheet.Columns.AutoFit(0, headers.Length);
        //        workbook.EndUpdate();

        //        MessageBox.Show("Đã import dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi import file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        //private string PromptForSeparator()
        //{
        //    return Microsoft.VisualBasic.Interaction.InputBox(
        //        "Nhập dấu phân cách (ví dụ: , hoặc ; hoặc | hoặc \\t):",
        //        "Chọn dấu phân cách",
        //        ","
        //    );
        //}

        //private void importFromExcel(object sender, ItemClickEventArgs e)
        //{
        //    MessageBox.Show("importFromExcel(object sender, ItemClickEventArgs e)");
        //    XtraOpenFileDialog xtraOpenFileDialog = new XtraOpenFileDialog
        //    {
        //        Filter = "Excel Files|*.xlsx;*.xls",
        //        Title = "Chọn file Excel để import"
        //    };

        //    if (xtraOpenFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        spreadsheetControl.LoadDocument(xtraOpenFileDialog.FileName);
        //        IWorkbook workbookFM = spreadsheetControl.Document;
        //        AdjustColumnWidth();
        //        FormatHeaderRow();
        //        MoveSheet1ToEnd(workbookFM);
        //        workbookFM.EndUpdate();
        //        UpdateSheetListToComboBox();
        //        UpdateTreeView();
        //    }
        //}
        private void importFromExcel(object sender, ItemClickEventArgs e)
        {
            XtraOpenFileDialog dialog = new() { Filter = "Excel Files|*.xlsx;*.xls" };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string filePath = dialog.FileName;
            using var tempSpreadsheet = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            tempSpreadsheet.LoadDocument(filePath);

            var workbook = spreadsheetControl.Document;
            var tempWorkbook = tempSpreadsheet.Document;

            foreach (var tempSheet in tempWorkbook.Worksheets)
            {
                string originalName = tempSheet.Name;
                string newName = originalName;

                if (workbook.Worksheets.Any(s => s.Name == originalName))
                {
                    var result = MessageBox.Show($"Sheet \"{originalName}\" đã tồn tại. Đổi tên và thêm vào không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                        continue;

                    newName = $"{originalName}_EXC";
                }

                var sheet = workbook.Worksheets.Add(newName);
                sheet.CopyFrom(tempSheet);

                sheet.Tag = new SheetSource
                {
                    Type = SourceType.Excel,
                    SourcePathOrTableName = filePath
                };
                sheetSources[sheet.Name] = (SheetSource)sheet.Tag;
            }

            //AdjustColumnWidth();
            FormatHeaderRow();
            MoveSheet1ToEnd(workbook);
            UpdateSheetListToComboBox();
            UpdateTreeView();
        }


        private void ImportFlatFileToSpreadsheet(string filePath, string separator)
        {
            IWorkbook workbook = spreadsheetControl.Document;
            string baseSheetName = Path.GetFileNameWithoutExtension(filePath);
            string sheetName = baseSheetName;

            if (workbook.Worksheets.Any(s => s.Name == sheetName))
            {
                var result = MessageBox.Show($"Sheet \"{sheetName}\" đã tồn tại. Có đổi tên và thêm vào không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                sheetName = $"{baseSheetName}_FF";
            }

            var sheet = workbook.Worksheets.Add(sheetName);
            workbook.BeginUpdate();

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                MessageBox.Show("File rỗng");
                return;
            }

            string[] headers = lines[0].Split(new string[] { separator }, StringSplitOptions.None);
            for (int col = 0; col < headers.Length; col++)
                sheet.Cells[0, col].Value = headers[col];

            for (int row = 1; row < lines.Length; row++)
            {
                string[] values = lines[row].Split(new string[] { separator }, StringSplitOptions.None);
                for (int col = 0; col < values.Length && col < headers.Length; col++)
                    sheet.Cells[row, col].Value = values[col];
            }

            sheet.Columns.AutoFit(0, headers.Length);
            workbook.EndUpdate();

            sheet.Tag = new SheetSource
            {
                Type = SourceType.CSV,
                SourcePathOrTableName = filePath
            };
            sheetSources[sheet.Name] = (SheetSource)sheet.Tag;

            MessageBox.Show("Import thành công!");
        }



        //private void ImportFlatFile(object sender, ItemClickEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Title = "Chọn file dữ liệu",
        //        Filter = "Flat Files (*.csv;*.txt;*.tsv)|*.csv;*.txt;*.tsv|All Files (*.*)|*.*"
        //    };

        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        string filePath = openFileDialog.FileName;

        //        string separator = PromptForSeparator();

        //        if (string.IsNullOrWhiteSpace(separator))
        //        {
        //            MessageBox.Show("Nhập dấu phân cách hợp lệ!");
        //            return;
        //        }

        //        if (separator == "\\t")
        //            separator = "\t";

        //        ImportFlatFileToSpreadsheet(filePath, separator);

        //        var activeSheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
        //        activeSheet.Tag = new SheetSource
        //        {
        //            Type = SheetSource.SourceType.CSV,
        //            SourcePathOrTableName = filePath
        //        };

        //        IWorkbook workbookFM = spreadsheetControl.Document;
        //        AdjustColumnWidth();
        //        FormatHeaderRow();
        //        MoveSheet1ToEnd(workbookFM);
        //        workbookFM.EndUpdate();
        //        UpdateSheetListToComboBox();
        //        UpdateTreeView();
        //    }
        //}
        private string PromptForSeparator()
        {
            return Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập dấu phân cách (ví dụ: , hoặc ; hoặc | hoặc \\t):",
                "Chọn dấu phân cách",
                ","
            );
        }
        //private void ImportFlatFileToSpreadsheet(string filePath, string separator)
        //{
        //    try
        //    {
        //        IWorkbook workbook = spreadsheetControl.Document;
        //        DevExpress.Spreadsheet.Worksheet sheet = workbook.Worksheets.ActiveWorksheet;
        //        workbook.BeginUpdate();
        //        sheet.Cells.Clear();

        //        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        //        if (lines.Length == 0)
        //        {
        //            MessageBox.Show("File rỗng.");
        //            return;
        //        }

        //        string[] headers = lines[0].Split(new string[] { separator }, StringSplitOptions.None);
        //        for (int col = 0; col < headers.Length; col++)
        //        {
        //            sheet.Cells[0, col].Value = headers[col];
        //        }

        //        for (int row = 1; row < lines.Length; row++)
        //        {
        //            string[] values = lines[row].Split(new string[] { separator }, StringSplitOptions.None);
        //            for (int col = 0; col < headers.Length; col++)
        //            {
        //                if (col < values.Length)
        //                    sheet.Cells[row, col].SetValue(values[col]);
        //            }
        //        }

        //        sheet.Columns.AutoFit(0, headers.Length);
        //        workbook.EndUpdate();

        //        MessageBox.Show("Đã import dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi import file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void importFromExcel(object sender, ItemClickEventArgs e)
        //{
        //    XtraOpenFileDialog xtraOpenFileDialog = new XtraOpenFileDialog
        //    {
        //        Filter = "Excel Files|*.xlsx;*.xls",
        //        Title = "Chọn file Excel để import"
        //    };

        //    if (xtraOpenFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        string filePath = xtraOpenFileDialog.FileName;
        //        spreadsheetControl.LoadDocument(filePath);

        //        IWorkbook workbook = spreadsheetControl.Document;
        //        foreach (DevExpress.Spreadsheet.Worksheet sheet in workbook.Worksheets)
        //        {
        //            sheet.Tag = new SheetSource
        //            {
        //                Type = SheetSource.SourceType.Excel,
        //                SourcePathOrTableName = filePath
        //            };
        //        }

        //        IWorkbook workbookFM = spreadsheetControl.Document;
        //        AdjustColumnWidth();
        //        FormatHeaderRow();
        //        MoveSheet1ToEnd(workbookFM);
        //        workbookFM.EndUpdate();
        //        UpdateSheetListToComboBox();
        //        UpdateTreeView();
        //    }
        //}
        private void FillSheetFromDataTable(DevExpress.Spreadsheet.Worksheet sheet, DataTable dt)
        {
            for (int col = 0; col < dt.Columns.Count; col++)
                sheet.Cells[0, col].Value = dt.Columns[col].ColumnName;

            for (int row = 0; row < dt.Rows.Count; row++)
                for (int col = 0; col < dt.Columns.Count; col++)
                    sheet.Cells[row + 1, col].Value = dt.Rows[row][col]?.ToString();

            sheet.Columns.AutoFit(0, dt.Columns.Count);
        }
        private void FinishSheetUpdates(IWorkbook workbook)
        {
            //AdjustColumnWidth();
            FormatHeaderRow();
            MoveSheet1ToEnd(workbook);
            workbook.EndUpdate();
            UpdateSheetListToComboBox();
            UpdateTreeView();
        }


        //CN cập nhật dữ liệu: 
        private void DoneClick(object sender, ItemClickEventArgs e)
        {
            ShowSheetSourcesInfo();
            //MessageBox.Show("DoneClick(object sender, ItemClickEventArgs e)");
            SaveAllSheetsBackToOriginalSource();
            //string connectionString = formConnectDB.ConnectionStringDB;
            //IWorkbook workbook = spreadsheetControl.Document;

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    List<string> databaseTables = GetDatabaseTables(conn);

            //    foreach (DevExpress.Spreadsheet.Worksheet sheet in workbook.Worksheets)
            //    {
            //        string originalTableName = sheet.Name;
            //        string finalTableName = originalTableName;

            //        if (!databaseTables.Contains(originalTableName))
            //        {
            //            DialogResult result = MessageBox.Show(
            //                $"Bảng bạn muốn lưu trữ chưa tồn tại trong database.\nBạn có muốn tạo bảng '{originalTableName}' trong database không?",
            //                "Tạo bảng mới?",
            //                MessageBoxButtons.YesNo,
            //                MessageBoxIcon.Question
            //            );

            //            if (result == DialogResult.No)
            //            {
            //                continue;
            //            }

            //            using (var inputForm = new InputBox($"Nhập tên bảng muốn tạo thay cho '{originalTableName}':", originalTableName))
            //            {
            //                if (inputForm.ShowDialog() == DialogResult.OK)
            //                {
            //                    MessageBox.Show("oke");
            //                    MessageBox.Show(inputForm.InputText.Trim());
            //                    finalTableName = inputForm.InputText.Trim();
            //                    if (string.IsNullOrWhiteSpace(finalTableName))
            //                    {
            //                        MessageBox.Show("Tên bảng không hợp lệ. Bỏ qua sheet này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                        continue;
            //                    }
            //                }
            //                else
            //                {
            //                    continue;
            //                }
            //            }
            //        }

            //        try
            //        {
            //            string dropTableQuery = $"DROP TABLE IF EXISTS [{finalTableName}];";
            //            using (SqlCommand cmd = new SqlCommand(dropTableQuery, conn))
            //            {
            //                cmd.ExecuteNonQuery();
            //            }

            //            DataTable dt = ConvertWorksheetToDataTable(sheet);
            //            CreateNewTable(conn, finalTableName, dt);
            //            InsertDataIntoTable(conn, finalTableName, dt);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"Lỗi khi cập nhật bảng {finalTableName}: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}
        }
        public void SaveAllSheetsBackToOriginalSource()
        {
            var workbook = spreadsheetControl.Document;

            foreach (DevExpress.Spreadsheet.Worksheet sheet in workbook.Worksheets)
            {
                if (!sheetSources.TryGetValue(sheet.Name, out var source))
                {
                    var result = MessageBox.Show($"Sheet \"{sheet.Name}\" không có thông tin nguồn. Bạn có muốn lưu vào bảng mới trong SQL Server không?",
                                 "Lưu dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes) SaveSheetToSQL(sheet);
                    continue;
                }

                if (loginForm.TypeUser == "SV" &&
                    source.Type == SourceType.Database &&
                    (source.ConnectionString.Contains("Initial Catalog=dataNCKH2", StringComparison.OrdinalIgnoreCase) ||
                     source.ConnectionString.Contains("Database=dataNCKH2", StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show($"Người dùng hiện tại(SV - dataNCKH2) không có quyền lưu Sheet \"{sheet.Name}\".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }

                SaveSheetBackToOriginalSource(sheet);
            }

            MessageBox.Show("Lưu các sheet hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void SaveSheetBackToOriginalSource(DevExpress.Spreadsheet.Worksheet sheet)
        {
            if (!sheetSources.TryGetValue(sheet.Name, out var source))
            {
                DialogResult result = MessageBox.Show(
                    $"Sheet \"{sheet.Name}\" không có thông tin nguồn.\nBạn có muốn lưu sheet này vào cơ sở dữ liệu mặc định không?", "Xác nhận lưu sheet", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveSheetToSQL(sheet);
                }
                else
                {
                    MessageBox.Show($"Sheet \"{sheet.Name}\" đã bị bỏ qua và không được lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return;
            }

            switch (source.Type)
            {
                case SourceType.Database:
                    SaveSheetToSQL(sheet, source);
                    break;

                case SourceType.Excel:
                    try
                    {
                        SaveSingleSheetToExcel(sheet, source.SourcePathOrTableName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu sheet \"{sheet.Name}\" vào Excel:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                //case SourceType.Excel:
                //    spreadsheetControl.SaveDocument(source.SourcePathOrTableName);
                //    //MessageBox.Show($"Sheet \"{sheet.Name}\" đã được lưu lại vào file Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    break;

                case SourceType.CSV:
                    SaveSheetToCSV(sheet, source.SourcePathOrTableName);
                    //MessageBox.Show($"Sheet \"{sheet.Name}\" đã được lưu lại vào file CSV.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    DialogResult result = MessageBox.Show($"Sheet \"{sheet.Name}\" không có thông tin nguồn.\nBạn có muốn lưu sheet này vào cơ sở dữ liệu mặc định không?", "Xác nhận lưu sheet", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SaveSheetToSQL(sheet);
                    }
                    else
                    {
                        MessageBox.Show($"Sheet \"{sheet.Name}\" đã bị bỏ qua và không được lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
        }
        //private void SaveSheetToSQL(DevExpress.Spreadsheet.Worksheet sheet, SheetSource source = null)
        //{
        //    string connectionString;
        //    string tableName;

        //    if (source != null && source.Type == SourceType.Database)
        //    {
        //        connectionString = source.ConnectionString;
        //        tableName = source.SourcePathOrTableName;
        //    }
        //    else
        //    {
        //        connectionString = "Data Source=TUGEND-FAKAS\\TGF_SQLSEVER; Initial Catalog=workDB; User ID=sa; Password=Huy@0865567532";
        //        tableName = $"Sheet_{sheet.Name}_{DateTime.Now:yyyyMMddHHmmss}";
        //    }

        //    try
        //    {
        //        DataTable table = SpreadsheetToDataTable(sheet);
        //        //MessageBox.Show("OK");
        //        using SqlConnection conn = new(connectionString);
        //        conn.Open();

        //        using SqlCommand drop = new($"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE [{tableName}];", conn);
        //        drop.ExecuteNonQuery();
        //        using SqlCommand create = new($"CREATE TABLE [{tableName}] (" +
        //            string.Join(", ", table.Columns.Cast<DataColumn>().Select(c => $"[{c.ColumnName}] NVARCHAR(MAX)")) + ");", conn);
        //        create.ExecuteNonQuery();
        //        foreach (DataRow row in table.Rows)
        //        {
        //            string values = string.Join(", ", row.ItemArray.Select(v => $"N'{v.ToString().Replace("'", "''")}'"));
        //            string query = $"INSERT INTO [{tableName}] VALUES ({values})";
        //            using SqlCommand insert = new(query, conn);
        //            insert.ExecuteNonQuery();
        //        }

        //        //MessageBox.Show($"Sheet \"{sheet.Name}\" đã được lưu vào SQL Server (bảng: {tableName}).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi lưu sheet \"{sheet.Name}\" vào SQL Server:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void SaveSheetToSQL(DevExpress.Spreadsheet.Worksheet sheet, SheetSource source = null)
        {
            string connectionString;
            string tableName;

            if (source != null && source.Type == SourceType.Database)
            {
                connectionString = source.ConnectionString;
                tableName = source.SourcePathOrTableName;

                if (connectionString.Contains("Port=", StringComparison.OrdinalIgnoreCase)
                    /*||connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase)*/)
                {
                    try
                    {
                        SaveSheetToMySQL(sheet, connectionString, tableName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu sheet \"{sheet.Name}\" vào MySQL:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            else
            {
                connectionString = "Data Source=TUGEND-FAKAS\\TGF_SQLSEVER; Initial Catalog=workDB; User ID=sa; Password=Huy@0865567532";
                tableName = $"Sheet_{sheet.Name}_{DateTime.Now:yyyyMMddHHmmss}";
            }

            try
            {
                DataTable table = SpreadsheetToDataTable(sheet);
                using SqlConnection conn = new(connectionString);
                conn.Open();

                using SqlCommand drop = new($"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE [{tableName}];", conn);
                drop.ExecuteNonQuery();

                using SqlCommand create = new($"CREATE TABLE [{tableName}] (" +
                    string.Join(", ", table.Columns.Cast<DataColumn>().Select(c => $"[{c.ColumnName}] NVARCHAR(MAX)")) + ");", conn);
                create.ExecuteNonQuery();

                foreach (DataRow row in table.Rows)
                {
                    string values = string.Join(", ", row.ItemArray.Select(v => $"N'{v.ToString().Replace("'", "''")}'"));
                    string query = $"INSERT INTO [{tableName}] VALUES ({values})";
                    using SqlCommand insert = new(query, conn);
                    insert.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sheet \"{sheet.Name}\" vào SQL Server:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSheetToMySQL(DevExpress.Spreadsheet.Worksheet sheet, string connectionString, string tableName)
        {
            DataTable table = SpreadsheetToDataTable(sheet);
            using MySqlConnection conn = new(connectionString);
            conn.Open();

            using MySqlCommand drop = new($"DROP TABLE IF EXISTS `{tableName}`;", conn);
            drop.ExecuteNonQuery();

            using MySqlCommand create = new(
                $"CREATE TABLE `{tableName}` (" +
                string.Join(", ", table.Columns.Cast<DataColumn>().Select(c => $"`{c.ColumnName}` TEXT")) + ");", conn);
            create.ExecuteNonQuery();

            foreach (DataRow row in table.Rows)
            {
                string values = string.Join(", ", row.ItemArray.Select(v => $"'{MySqlHelper.EscapeString(v.ToString())}'"));
                string query = $"INSERT INTO `{tableName}` VALUES ({values})";
                using MySqlCommand insert = new(query, conn);
                insert.ExecuteNonQuery();
            }
        }
        private void SaveSingleSheetToExcel(DevExpress.Spreadsheet.Worksheet sourceSheet, string filePath)
        {
            SpreadsheetControl tempControl = new SpreadsheetControl();

            IWorkbook tempWorkbook = tempControl.Document;
            tempWorkbook.BeginUpdate();

            try
            {
                DevExpress.Spreadsheet.Worksheet newSheet = tempWorkbook.Worksheets.Add();
                newSheet.Name = sourceSheet.Name;
                newSheet.CopyFrom(sourceSheet);

                for (int i = tempWorkbook.Worksheets.Count - 1; i >= 0; i--)
                {
                    if (tempWorkbook.Worksheets[i] != newSheet)
                    {
                        tempWorkbook.Worksheets.RemoveAt(i);
                    }
                }

                newSheet.Visible = true;
                tempWorkbook.Worksheets.ActiveWorksheet = newSheet;
            }
            finally
            {
                tempWorkbook.EndUpdate();
            }

            tempWorkbook.SaveDocument(filePath, DocumentFormat.Xlsx);
        }




        private void SaveSheetToCSV(DevExpress.Spreadsheet.Worksheet sheet, string filePath)
        {
            using StreamWriter writer = new(filePath, false, Encoding.UTF8);
            int rowCount = sheet.GetUsedRange().RowCount;
            int colCount = sheet.GetUsedRange().ColumnCount;

            for (int row = 0; row < rowCount; row++)
            {
                List<string> values = new();
                for (int col = 0; col < colCount; col++)
                    values.Add(sheet.Cells[row, col].Value?.ToString() ?? "");

                writer.WriteLine(string.Join(";", values));
            }
        }

        private DataTable SpreadsheetToDataTable(DevExpress.Spreadsheet.Worksheet sheet)
        {
            DataTable dt = new();
            int colCount = sheet.GetUsedRange().ColumnCount;
            int rowCount = sheet.GetUsedRange().RowCount;

            for (int col = 0; col < colCount; col++)
                dt.Columns.Add(sheet.Cells[0, col].Value.ToString());

            for (int row = 1; row < rowCount; row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < colCount; col++)
                    dr[col] = sheet.Cells[row, col].Value?.ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }




        private void ShowSheetSourcesInfo()
        {
            if (sheetSources == null || sheetSources.Count == 0)
            {
                MessageBox.Show("Không có thông tin nguồn nào được lưu.");
                return;
            }

            StringBuilder sb = new();

            foreach (var kvp in sheetSources)
            {
                string sheetName = kvp.Key;
                SheetSource source = kvp.Value;

                sb.AppendLine($"Sheet: {sheetName}");
                sb.AppendLine($"  - Nguồn: {source.Type}");
                sb.AppendLine($"  - Đường dẫn / Tên bảng: {source.SourcePathOrTableName}");

                if (!string.IsNullOrEmpty(source.ConnectionString) && source.Type == SourceType.Database)
                    sb.AppendLine($"  - Chuỗi kết nối: {source.ConnectionString}");

                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString(), "Thay đổi sẽ được lưu theo nguồn ban đầu của sheet: ");
        }

        private List<string> GetDatabaseTables(SqlConnection conn)
        {
            MessageBox.Show("GetDatabaseTables(SqlConnection conn)");
            List<string> tableNames = new List<string>();

            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }

            return tableNames;
        }
        private DataTable ConvertWorksheetToDataTable(DevExpress.Spreadsheet.Worksheet sheet)
        {
            //MessageBox.Show("ConvertWorksheetToDataTable(DevExpress.Spreadsheet.Worksheet sheet)");
            DataTable dt = new DataTable(sheet.Name);
            int startRow = 0;
            int startColumn = 0;

            int lastUsedRow = sheet.GetUsedRange().BottomRowIndex;
            int lastUsedColumn = sheet.GetUsedRange().RightColumnIndex;

            for (int col = startColumn; col <= lastUsedColumn; col++)
            {
                string header = sheet.Cells[startRow, col].DisplayText;
                if (string.IsNullOrWhiteSpace(header))
                {
                    header = $"Column{col}";
                }

                dt.Columns.Add(header, typeof(string));
            }

            for (int row = startRow + 1; row <= lastUsedRow; row++)
            {
                DataRow dataRow = dt.NewRow();
                for (int col = startColumn; col <= lastUsedColumn; col++)
                {
                    DevExpress.Spreadsheet.Cell cell = sheet.Cells[row, col];

                    string cellValue = cell.DisplayText?.Trim();
                    dataRow[col - startColumn] = string.IsNullOrEmpty(cellValue) ? DBNull.Value : (object)cellValue;
                }

                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        private void CreateNewTable(SqlConnection conn, string tableName, DataTable dt)
        {
            //MessageBox.Show("Creat CreateNewTable(SqlConnection conn, string tableName, DataTable dt)");
            List<string> columnDefinitions = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                string columnName = col.ColumnName.Replace("'", "''");
                columnDefinitions.Add($"[{columnName}] NVARCHAR(MAX)");
            }
            //MessageBox.Show("Đến chỗ nii ròi");
            string createTableQuery = $"CREATE TABLE [{tableName}] ({string.Join(", ", columnDefinitions)});";

            using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }


        private void InsertDataIntoTable(SqlConnection conn, string tableName, DataTable dt)
        {
            //MessageBox.Show(" InsertDataIntoTable(SqlConnection conn, string tableName, DataTable dt)");
            foreach (DataRow row in dt.Rows)
            {
                List<string> columnNames = new List<string>();
                List<string> parameterNames = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                int paramIndex = 0;

                foreach (DataColumn col in dt.Columns)
                {
                    string colName = col.ColumnName;
                    object value = row[col];

                    string paramName = $"@param{paramIndex++}";

                    columnNames.Add($"[{colName}]");
                    parameterNames.Add(paramName);
                    parameters.Add(new SqlParameter(paramName, value ?? DBNull.Value));
                }

                string insertQuery = $"INSERT INTO [{tableName}] ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", parameterNames)});";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.ExecuteNonQuery();
                }
            }
        }



        //CN Tạo báo cáo:

        private async void CreateReportClick(object sender, ItemClickEventArgs e)
        {
            //MessageBox.Show("CreateReportClick(object sender, ItemClickEventArgs e)");
            if (treeView1.Nodes.Count == 0 || cbbSLTaable.SelectedItem == null) return;

            string sheetName = cbbSLTaable.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                MessageBox.Show("Vui lòng chọn sheet để tạo báo cáo.");
                return;
            }

            IWorkbook workbook = spreadsheetControl.Document;
            DevExpress.Spreadsheet.Worksheet sheet = workbook.Worksheets[sheetName];
            if (sheet == null)
            {
                MessageBox.Show($"Không tìm thấy sheet '{sheetName}'.");
                return;
            }
            string connectionString = "Server=TUGEND-FAKAS\\TGF_SQLSEVER;Database=workDB;User ID=sa;Password=Huy@0865567532;";
            string tableName = sheetName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string dropTableQuery = $"IF OBJECT_ID('[dbo].[{tableName}]', 'U') IS NOT NULL DROP TABLE [dbo].[{tableName}]";
                using (SqlCommand dropCmd = new SqlCommand(dropTableQuery, conn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();
                int rowCount = usedRange.RowCount;
                int colCount = usedRange.ColumnCount;

                if (rowCount < 2)
                {
                    MessageBox.Show("Sheet không có dữ liệu để tạo bảng.");
                    return;
                }

                //MessageBox.Show("Oke1");
                List<string> columnNames = new List<string>();
                for (int col = 0; col < colCount; col++)
                {
                    string columnName = sheet.Cells[0, col].Value.ToString();
                    if (string.IsNullOrWhiteSpace(columnName))
                        columnName = $"Column{col + 1}";
                    //columnName = columnName.Replace(" ", "_");
                    columnNames.Add(columnName);
                }

                string createTableQuery = $"CREATE TABLE [dbo].[{tableName}] (";
                foreach (string col in columnNames)
                {
                    createTableQuery += $"[{col}] NVARCHAR(MAX),";
                }
                createTableQuery = createTableQuery.TrimEnd(',') + ")";
                using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                {
                    createCmd.ExecuteNonQuery();
                }


                for (int row = 1; row < rowCount; row++)
                {
                    List<string> values = new List<string>();
                    for (int col = 0; col < colCount; col++)
                    {
                        string cellValue = sheet.Cells[row, col].Value.ToString().Replace("'", "''");
                        values.Add($"N'{cellValue}'");
                    }

                    string insertQuery = $"INSERT INTO [dbo].[{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}]"))}) " +
                                         $"VALUES ({string.Join(",", values)})";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.ExecuteNonQuery();
                    }
                }

                //MessageBox.Show($"Đã tạo bảng dữ liệu '{tableName}' với {rowCount - 1} dòng dữ liệu từ sheet '{sheetName}'.");

                List<string> columnsToKeep = new List<string>();

                foreach (TreeNode tableNode in treeView1.Nodes)
                {
                    if (tableNode.Text == cbbSLTaable.Text)
                    {
                        foreach (TreeNode columnNode in tableNode.Nodes)
                        {
                            if (columnNode.Checked)
                            {
                                string columnName = columnNode.Text;
                                columnsToKeep.Add(columnName);
                            }
                        }
                        break;
                    }
                }

                List<string> currentColumns = new List<string>();

                string getColumnQuery = $"SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('[dbo].[{tableName}]')";
                using (SqlCommand colCmd = new SqlCommand(getColumnQuery, conn))
                {
                    using (SqlDataReader reader = colCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            currentColumns.Add(reader.GetString(0));
                        }
                    }
                }

                var columnsToDrop = currentColumns.Except(columnsToKeep).ToList();

                foreach (string colToDrop in columnsToDrop)
                {
                    string dropColumnQuery = $"ALTER TABLE [dbo].[{tableName}] DROP COLUMN [{colToDrop}]";//Cos exc
                    //MessageBox.Show(dropColumnQuery);
                    //MessageBox.Show("làm đến đây r");
                    try
                    {
                        using (SqlCommand dropCmd = new SqlCommand(dropColumnQuery, conn))
                        {
                            dropCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex) { }
                }

                //MessageBox.Show($"Đã xóa {columnsToDrop.Count} cột không được chọn khỏi bảng '{tableName}'.");
            }

            string newSheetName = cbbSLTaable.Text;

            IWorkbook workbookData = spreadsheetControlData.Document;
            DevExpress.Spreadsheet.Worksheet newSheet;

            if (workbookData.Worksheets.Contains(newSheetName))
            {
                workbookData.Worksheets.Remove(workbookData.Worksheets[newSheetName]);
            }
            newSheet = workbookData.Worksheets.Add();
            newSheet.Name = newSheetName;
            workbookData.Worksheets.ActiveWorksheet = newSheet;

            string query = $"SELECT * FROM [dbo].[{cbbSLTaable.Text}]";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        newSheet.Cells[0, col].Value = dt.Columns[col].ColumnName;
                    }

                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            newSheet.Cells[row + 1, col].Value = dt.Rows[row][col]?.ToString();
                        }
                    }
                }
            }

            DevExpress.Spreadsheet.CellRange displayRange = newSheet.GetUsedRange();
            int lastColumnIndex = displayRange.RightColumnIndex;
            DevExpress.Spreadsheet.CellRange headerRange = newSheet.Range.FromLTRB(0, 0, lastColumnIndex, 0);
            headerRange.Alignment.WrapText = true;
            newSheet.Columns.AutoFit(0, lastColumnIndex);
            newSheet.Rows[0].AutoFit();

            spreadsheetControlData.Refresh();
            XtraMessageBoxArgs args = new XtraMessageBoxArgs();
            args.Caption = "Thông báo";
            args.Text = $"Đã tải dữ liệu bảng '{cbbSLTaable.Text}' vào sheet mới trong spreadsheetControlData.\n\nBạn có muốn xem lại dữ liệu báo cáo không?";
            args.Buttons = new DialogResult[] { DialogResult.Yes, DialogResult.No };
            args.Icon = System.Drawing.SystemIcons.Question;

            DialogResult result = XtraMessageBox.Show(args);

            if (result == DialogResult.Yes)
            {
                string url = "";
                if (loginForm.TypeUser == "SV")
                {
                    string maSV = loginForm.CodeUser;
                    string reportName = "SV";
                    url = $"http://tugend-fakas:85/Reports/powerbi/{reportName}?filter=BangDiem/MaSV%20eq%20%27{maSV}%27&rs:Embed=true";
                }
                else
                {
                    string reportName = "GV";
                    url = $"http://tugend-fakas:85/Reports/powerbi/{reportName}?filter&rs:Embed=true";
                }
                await ShowProgressBarAndTask2(url);
            }
            else
            {
                //MessageBox.Show(loginForm.TypeUser);
                string url = "";
                if (loginForm.TypeUser == "SV")
                {
                    string maSV = loginForm.CodeUser;
                    string reportName = "SV";
                    url = $"http://tugend-fakas:85/Reports/powerbi/{reportName}?filter=BangDiem/MaSV%20eq%20%27{maSV}%27&rs:Embed=true";
                }
                else
                {
                    string reportName = "GV";
                    url = $"http://tugend-fakas:85/Reports/powerbi/{reportName}?filter&rs:Embed=true";
                }

                await ShowProgressBarAndNavigateAsync(url);
            }

        }
        private async Task ShowProgressBarAndNavigateAsync(string url)
        {
            webView21.Dock = DockStyle.Fill;
            webView21.Source = new Uri("about:blank");
            //await Task.Delay(300);
            //MessageBox.Show("Oke");
            webView21.Source = new Uri(url);
            //MessageBox.Show("Task ShowProgressBarAndNavigateAsync(string url)");
            groupControl1.Visible = true;
            label2.Visible = true;
            progressBarControlLoading.Visible = true;
            progressBarControlLoading.EditValue = 0;

            for (int i = 0; i <= 100; i++)
            {
                progressBarControlLoading.EditValue = i;
                await Task.Delay(40); // mỗi 40ms tăng 1% 
            }

            progressBarControlLoading.Visible = false;
            label2.Visible = false;
            groupControl1.Visible = false;


            accordionControl1.SelectedElement = accordionControlElementReport;
        }

        private async Task ShowProgressBarAndTask2(string url)
        {
            //MessageBox.Show(" async Task ShowProgressBarAndTask2(string url)");
            accordionControl1.SelectedElement = accordionControlElementSDLK;
            webView21.Dock = DockStyle.Fill;
            webView21.Source = new Uri("about:blank");
            //await Task.Delay(300);
            webView21.Source = new Uri(url);

            label3.Visible = true;
            progressBarControl1.Visible = true;
            progressBarControl1.EditValue = 0;
            for (int i = 0; i <= 100; i++)
            {
                progressBarControl1.EditValue = i;
                await Task.Delay(50);
            }
            progressBarControl1.Visible = false;
            label3.Visible = false;
        }

        private void timerLoading_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(" timerLoading_Tick(object sender, EventArgs e)");
            loadingProgress += 2;
            if (loadingProgress <= 100)
            {
                progressBarControlLoading.EditValue = loadingProgress;
            }
            else
            {
                timerLoading.Stop();
                progressBarControlLoading.Visible = false;
                loadingProgress = 0;

                webView21.Dock = DockStyle.Fill;
                webView21.Source = new Uri(webUrlToNavigate);
                accordionControl1.SelectedElement = accordionControlElementReport;
            }
        }

        //Chức năng đăng xuất:
        private void dangXuat()
        {
            //loginForm LGRorm = new loginForm();
            //LGRorm.ShowDialog();
            //this.Close();

            //this.Hide();
            //this.Hide();
            ResetApp();
            //using (loginForm LGform = new loginForm())
            //{
            //    var result = LGform.ShowDialog();
            //    if (result == DialogResult.OK)
            //    {
            //        this.Show();
            //        ResetApp();
            //    }
            //    else
            //    {
            //        //MessageBox.Show("else");
            //        Program.RestartApp();
            //    }
            //}
        }
        private void btnLogOutClick(object sender, ItemClickEventArgs e)
        {
            //loginForm LGRorm = new loginForm();
            //LGRorm.ShowDialog();
            //this.Close();

            //this.Hide();

            //using (loginForm LGform = new loginForm())
            //{
            //    var result = LGform.ShowDialog();
            //    if (result == DialogResult.OK)
            //    {
            //        this.Show();
            //        ResetApp();
            //    }
            //    else
            //    {
            //        //MessageBox.Show("else");
            //        Program.RestartApp();
            //    }
            //}
            //dangXuat();
            ResetApp();
        }

        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            //if (isCheckedChanged)
            //{
            //    checkTrug(spreadsheetControl, treeView1);

            //    if (danhSachHangTrung.Count > 0)
            //    {
            //        ribbonPageGroup7.Visible = true;

            //        int totalDuplicates = danhSachHangTrung.Sum(group => group.Count);
            //        barStaticItem4.Caption = $"Đã phát hiện {danhSachHangTrung.Count} nhóm với tổng {totalDuplicates} dòng bị trùng lặp";


            //        int firstRow = danhSachHangTrung[0][0];  
            //        string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
            //        //MessageBox.Show("oke4");

            //        DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            //        DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();

            //        int firstRecordRowIndex = 2;
            //        int firstColumnIndex = usedRange.LeftColumnIndex;
            //        sheet.SelectedCell = sheet.Cells[firstRecordRowIndex, firstColumnIndex];
            //        spreadsheetControl.Focus();
            //        SetForegroundWindow(spreadsheetControl.Handle);

            //        this.BeginInvoke(new Action(() =>
            //        {
            //            SendKeys.SendWait("{UP}");

            //            this.BeginInvoke(new Action(() =>
            //            {
            //                //MessageBox.Show("oke2");
            //                sheet.SelectedCell = sheet.Cells[firstRow, sheet.GetUsedRange().LeftColumnIndex];
            //                spreadsheetControl.ActiveView.ScrollLineUpDown(firstRow-1);
            //                //this.BeginInvoke(new Action(() =>
            //                //{
            //                //    SendKeys.SendWait("{DOWN}");
            //                //}));
            //                SetForegroundWindow(spreadsheetControl.Handle);
            //            }));
            //        }));
            //    }
            //    else
            //    {
            //        ribbonPageGroup7.Visible = false;
            //    }
            //    isCheckedChanged = false;
            //}
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(" DebounceTimer_Tick(object sender, EventArgs e)");
            debounceTimer.Stop();

            if (isCheckedChanged)
            {
                //MessageBox.Show("đã chạy");
                checkTrug(spreadsheetControl, treeView1);
                allDuplicateRowsOrdered = danhSachHangTrung.SelectMany(group => group).ToList();
                currentDuplicateIndex = 1;
                //MessageBox.Show(danhSachHangTrung.Count.ToString());
                if (danhSachHangTrung.Count > 0)
                {
                    ribbonPageGroup7.Visible = true;

                    int totalDuplicates = danhSachHangTrung.Sum(group => group.Count);
                    barStaticItem4.Caption = $"Đã phát hiện {danhSachHangTrung.Count} nhóm với tổng {totalDuplicates} dòng bị trùng lặp";
                    //MessageBox.Show("đã chạy");
                    int firstRow = danhSachHangTrung[0][0];
                    string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
                    var sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
                    var usedRange = sheet.GetUsedRange();



                    int firstRecordRowIndex = 2;
                    int firstColumnIndex = usedRange.LeftColumnIndex;
                    sheet.SelectedCell = sheet.Cells[firstRecordRowIndex, firstColumnIndex];
                    spreadsheetControl.Focus();
                    SetForegroundWindow(spreadsheetControl.Handle);
                    this.BeginInvoke(new Action(() =>
                    {
                        SendKeys.SendWait("{UP}");
                        this.BeginInvoke(new Action(() =>
                        {
                            //sheet.SelectedCell = sheet.Cells[firstRow, usedRange.LeftColumnIndex];
                            sheet.Rows[firstRow].Select();
                            spreadsheetControl.ActiveView.ScrollLineUpDown(firstRow - 2);
                            SetForegroundWindow(spreadsheetControl.Handle);
                        }));
                    }));
                }
                else
                {
                    ribbonPageGroup7.Visible = false;
                    var sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
                    //var usedRange = sheet.GetUsedRange();
                    //usedRange.Fill.BackgroundColor = Color.Empty;
                }

                isCheckedChanged = false;
            }
        }

        public void checkTrug(SpreadsheetControl spreadsheetControl, TreeView treeView1)
        {
            danhSachHangTrung = new List<List<int>>();

            if (cbbSLTaable.SelectedItem == null) return;

            string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
            var sheet = spreadsheetControl.Document.Worksheets[selectedSheetName];
            if (sheet == null) return;

            var usedRange = sheet.GetUsedRange();
            int startRow = usedRange.TopRowIndex + 1;
            int endRow = usedRange.BottomRowIndex;

            TreeNode sheetNode = null;
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Text == selectedSheetName)
                {
                    sheetNode = node;
                    break;
                }
            }
            if (sheetNode == null) return;

            List<int> selectedCols = new List<int>();
            for (int col = usedRange.LeftColumnIndex; col <= usedRange.RightColumnIndex; col++)
            {
                string header = sheet.Cells[usedRange.TopRowIndex, col].DisplayText?.Trim();
                foreach (TreeNode fieldNode in sheetNode.Nodes)
                {
                    if (fieldNode.Checked && fieldNode.Text == header)
                    {
                        selectedCols.Add(col);
                        break;
                    }
                }
            }

            if (selectedCols.Count == 0) return;

            Dictionary<string, List<int>> keyToRowIndices = new Dictionary<string, List<int>>();
            for (int row = startRow; row <= endRow; row++)
            {
                StringBuilder keyBuilder = new StringBuilder();
                foreach (int col in selectedCols)
                {
                    string value = sheet.Cells[row, col].DisplayText?.Trim() ?? "";
                    keyBuilder.Append(value).Append("|");
                }

                string key = keyBuilder.ToString();
                if (!keyToRowIndices.ContainsKey(key))
                {
                    keyToRowIndices[key] = new List<int>();
                }
                keyToRowIndices[key].Add(row);
            }

            usedRange.Fill.BackgroundColor = Color.Empty;
            Random rand = new Random();

            foreach (var pair in keyToRowIndices)
            {
                if (pair.Value.Count > 1)
                {
                    danhSachHangTrung.Add(pair.Value);

                    var color = System.Drawing.Color.FromArgb(
                        100 + rand.Next(156),
                        100 + rand.Next(156),
                        100 + rand.Next(156)
                    );

                    var usedRange1 = sheet.GetUsedRange();
                    int startCol = usedRange1.LeftColumnIndex;
                    int endCol = usedRange1.RightColumnIndex;

                    foreach (int rowIndex in pair.Value)
                    {
                        var dataRange = sheet.Range.FromLTRB(startCol, rowIndex, endCol, rowIndex);
                        dataRange.Fill.BackgroundColor = color;
                    }
                }
            }

        }

        private void barButtonItem51_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (allDuplicateRowsOrdered == null || allDuplicateRowsOrdered.Count <= 1)
            {
                return;
            }

            if (currentDuplicateIndex >= allDuplicateRowsOrdered.Count)
            {
                currentDuplicateIndex = 0;
            }

            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();

            //int firstRecordRowIndex = 1;
            int firstColumnIndex = usedRange.LeftColumnIndex;


            int targetRow = allDuplicateRowsOrdered[currentDuplicateIndex];
            currentDuplicateIndex++;
            //sheet.Rows[targetRow].Select();
            //sheet.SelectedCell = sheet.Cells[firstRecordRowIndex, firstColumnIndex];
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);

            this.BeginInvoke(new Action(() =>
            {
                //SendKeys.SendWait("{UP}");
                spreadsheetControl.ActiveView.ScrollLineUpDown(-(targetRow + 40));
                this.BeginInvoke(new Action(() =>
                {
                    sheet.Rows[targetRow].Select();
                    //int scrollOffset = targetRow - 2 -6;
                    spreadsheetControl.ActiveView.ScrollLineUpDown(targetRow - 2 - 6);
                    SetForegroundWindow(spreadsheetControl.Handle);
                }));
            }));
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);

        }
        private void barButtonItem49_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (allDuplicateRowsOrdered == null || allDuplicateRowsOrdered.Count <= 1)
                return;

            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            DevExpress.Spreadsheet.CellRange usedRange = sheet.GetUsedRange();
            int firstColumnIndex = usedRange.LeftColumnIndex;

            int currentRowIndex = sheet.SelectedCell?.TopRowIndex ?? -1;

            if (currentRowIndex == allDuplicateRowsOrdered[0])
            {
                currentDuplicateIndex = allDuplicateRowsOrdered.Count - 1;
            }
            else
            {
                currentDuplicateIndex--;

                if (currentDuplicateIndex < 0)
                    currentDuplicateIndex = allDuplicateRowsOrdered.Count - 1;
            }

            int targetRow = allDuplicateRowsOrdered[currentDuplicateIndex];

            SetForegroundWindow(spreadsheetControl.Handle);
            this.BeginInvoke(new Action(() =>
            {
                //SendKeys.SendWait("{UP}");
                spreadsheetControl.ActiveView.ScrollLineUpDown(-(targetRow + 40));
                this.BeginInvoke(new Action(() =>
                {
                    sheet.Rows[targetRow].Select();
                    //int scrollOffset = targetRow - 2 -6;
                    spreadsheetControl.ActiveView.ScrollLineUpDown(targetRow - 2 - 6);
                    SetForegroundWindow(spreadsheetControl.Handle);
                }));
            }));
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);
        }

        private void barButtonItem54_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Ý tưởng chưa lm: Lưu dòng trùng tiếp theo, Xóa dòng đang trỏ hiện tại, gọi lại hàm tìm banr ghi trùng  , setText, nếu chỉ số dòng trùng tiếp theo không có trong danh sách trùng mới thì thôi có nghĩa đang trỏ vào dòng trùng đầu tiên trong danh sách trùng mới, nếu có thì di chuyển đến chỉ số dòng trùng đã lưu -1
            if (allDuplicateRowsOrdered == null || allDuplicateRowsOrdered.Count == 0)
                return;

            DevExpress.Spreadsheet.Worksheet sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            int currentRowIndex = sheet.SelectedCell?.TopRowIndex ?? -1;

            if (currentRowIndex == -1)
                return;

            if (!allDuplicateRowsOrdered.Contains(currentRowIndex))
            {
                MessageBox.Show("Dòng hiện tại không nằm trong danh sách trùng.");
                return;
            }
            sheet.Rows[currentRowIndex].Delete();


            checkTrug(spreadsheetControl, treeView1);
            allDuplicateRowsOrdered = danhSachHangTrung.SelectMany(group => group).ToList();
            currentDuplicateIndex = 1;
            //MessageBox.Show(danhSachHangTrung.Count.ToString());
            if (danhSachHangTrung.Count > 0)
            {
                ribbonPageGroup7.Enabled = true;
                ribbonPageGroup7.Visible = true;

                int totalDuplicates = danhSachHangTrung.Sum(group => group.Count);
                barStaticItem4.Caption = $"Đã phát hiện {danhSachHangTrung.Count} nhóm với tổng {totalDuplicates} dòng bị trùng lặp";
                //MessageBox.Show("đã chạy");
                int firstRow = danhSachHangTrung[0][0];
                string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
                var usedRange = sheet.GetUsedRange();
                int firstRecordRowIndex = 2;
                int firstColumnIndex = usedRange.LeftColumnIndex;
                sheet.SelectedCell = sheet.Cells[firstRecordRowIndex, firstColumnIndex];
                spreadsheetControl.Focus();
                SetForegroundWindow(spreadsheetControl.Handle);
                this.BeginInvoke(new Action(() =>
                {
                    SendKeys.SendWait("{UP}");
                    this.BeginInvoke(new Action(() =>
                    {
                        //sheet.SelectedCell = sheet.Cells[firstRow, usedRange.LeftColumnIndex];
                        sheet.Rows[firstRow].Select();
                        spreadsheetControl.ActiveView.ScrollLineUpDown(firstRow - 2);
                        SetForegroundWindow(spreadsheetControl.Handle);
                    }));
                }));
            }
            else
            {
                ribbonPageGroup7.Visible = false;
                ribbonPageGroup7.Enabled = false;
                var usedRange = sheet.GetUsedRange();
                usedRange.Fill.BackgroundColor = Color.Empty;
            }
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);
        }

        private void barButtonItem50_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (danhSachHangTrung == null || danhSachHangTrung.Count == 0)
                return;

            var sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            List<int> rowsToDelete = new List<int>();

            foreach (var group in danhSachHangTrung)
            {
                if (group.Count <= 1)
                    continue;

                var sortedGroup = group.OrderBy(i => i).ToList();
                for (int i = 0; i < sortedGroup.Count - 1; i++)
                {
                    rowsToDelete.Add(sortedGroup[i]);
                }
            }
            if (rowsToDelete.Count == 0)
            {
                return;
            }
            //string rowIndexes = string.Join(", ", rowsToDelete.OrderBy(i => i));
            //MessageBox.Show($"Các dòng sẽ bị xóa:\n{rowIndexes}");
            foreach (var rowIndex in rowsToDelete.OrderByDescending(i => i))
            {
                sheet.Rows[rowIndex].Delete();
            }
            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);
            checkTrug(spreadsheetControl, treeView1);
            if (danhSachHangTrung.IsNullOrEmpty())
            {
                if (cbbSLTaable.SelectedItem == null) return;
                var usedRange = sheet.GetUsedRange();
                usedRange.Fill.BackgroundColor = Color.Empty;

                ribbonPageGroup7.Enabled = false;
                int totalDuplicates = danhSachHangTrung.Sum(group => group.Count);
                if (totalDuplicates == 0)
                {
                    barStaticItem4.Caption = $"Sheet hiện tại không tồn tại bản ghi trùng lặp";
                }
            }
            else
            {
                MessageBox.Show("Xử lý bản ghi trùng thất bại thảm hại!");
            }
        }

        private void barButtonItem53_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (danhSachHangTrung == null || danhSachHangTrung.Count == 0)
                return;

            var sheet = spreadsheetControl.Document.Worksheets.ActiveWorksheet;
            List<int> rowsToDelete = new List<int>();
            foreach (var group in danhSachHangTrung)
            {
                if (group.Count <= 1)
                    continue;

                var sortedGroup = group.OrderBy(i => i).ToList();
                for (int i = 1; i < sortedGroup.Count; i++)
                {
                    rowsToDelete.Add(sortedGroup[i]);
                }
            }

            if (rowsToDelete.Count == 0)
            {
                MessageBox.Show("Không có dòng nào cần xóa.");
                return;
            }
            string rowIndexes = string.Join(", ", rowsToDelete.OrderBy(i => i));
            MessageBox.Show($"Các dòng sẽ bị xóa (giữ lại dòng đầu mỗi nhóm):\n{rowIndexes}");
            foreach (var rowIndex in rowsToDelete.OrderByDescending(i => i))
            {
                sheet.Rows[rowIndex].Delete();
            }

            spreadsheetControl.Focus();
            SetForegroundWindow(spreadsheetControl.Handle);

            checkTrug(spreadsheetControl, treeView1);
            if (danhSachHangTrung.IsNullOrEmpty())
            {
                if (cbbSLTaable.SelectedItem == null) return;
                var usedRange = sheet.GetUsedRange();
                usedRange.Fill.BackgroundColor = Color.Empty;

                ribbonPageGroup7.Enabled = false;
                int totalDuplicates = danhSachHangTrung.Sum(group => group.Count);
                if (totalDuplicates == 0)
                {
                    barStaticItem4.Caption = $"Sheet hiện tại không tồn tại bản ghi trùng lặp";
                }
            }
            else
            {
                MessageBox.Show("Xử lý bản ghi trùng thất bại thảm hại!");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //dangXuat();
            ResetApp();
        }





        //public List<List<int>> checkTrug(SpreadsheetControl spreadsheetControl, TreeView treeView1)
        //{
        //    List<List<int>> duplicateRowGroups = new List<List<int>>();

        //    if (cbbSLTaable.SelectedItem == null) return duplicateRowGroups;

        //    string selectedSheetName = cbbSLTaable.SelectedItem.ToString();
        //    var sheet = spreadsheetControl.Document.Worksheets[selectedSheetName];
        //    if (sheet == null) return duplicateRowGroups;

        //    var usedRange = sheet.GetUsedRange();
        //    int startRow = usedRange.TopRowIndex + 1;
        //    int endRow = usedRange.BottomRowIndex;

        //    TreeNode sheetNode = null;
        //    foreach (TreeNode node in treeView1.Nodes)
        //    {
        //        if (node.Text == selectedSheetName)
        //        {
        //            sheetNode = node;
        //            break;
        //        }
        //    }
        //    if (sheetNode == null) return duplicateRowGroups;

        //    List<int> selectedCols = new List<int>();
        //    for (int col = usedRange.LeftColumnIndex; col <= usedRange.RightColumnIndex; col++)
        //    {
        //        string header = sheet.Cells[usedRange.TopRowIndex, col].DisplayText?.Trim();
        //        foreach (TreeNode fieldNode in sheetNode.Nodes)
        //        {
        //            if (fieldNode.Checked && fieldNode.Text == header)
        //            {
        //                selectedCols.Add(col);
        //                break;
        //            }
        //        }
        //    }

        //    if (selectedCols.Count == 0) return duplicateRowGroups;

        //    Dictionary<string, List<int>> keyToRowIndices = new Dictionary<string, List<int>>();
        //    for (int row = startRow; row <= endRow; row++)
        //    {
        //        StringBuilder keyBuilder = new StringBuilder();
        //        foreach (int col in selectedCols)
        //        {
        //            string value = sheet.Cells[row, col].DisplayText?.Trim() ?? "";
        //            keyBuilder.Append(value).Append("|");
        //        }

        //        string key = keyBuilder.ToString();
        //        if (!keyToRowIndices.ContainsKey(key))
        //        {
        //            keyToRowIndices[key] = new List<int>();
        //        }
        //        keyToRowIndices[key].Add(row);
        //    }

        //    // Chỉ lấy các nhóm có nhiều hơn 1 dòng
        //    foreach (var pair in keyToRowIndices)
        //    {
        //        if (pair.Value.Count > 1)
        //        {
        //            duplicateRowGroups.Add(pair.Value);
        //        }
        //    }

        //    return duplicateRowGroups;
        //}





    }
}
//private void MergeSheetsByKeyColumn(string keyColumnName)
//{
//    MessageBox.Show("MergeSheetsByKeyColumn(string keyColumnName)");
//    var workbook = spreadsheetControl.Document;
//    var mergedSheet = GetOrCreateSheet("Tổng hợp");
//    mergedSheet.Cells.Clear();

//    // Lưu dữ liệu theo khóa
//    var mergedData = new Dictionary<string, Dictionary<string, string>>();
//    var allColumns = new HashSet<string>();

//    foreach (var sheet in workbook.Worksheets)
//    {
//        if (sheet == mergedSheet || sheet.GetUsedRange().RowCount <= 1)
//            continue;

//        var usedRange = sheet.GetUsedRange();
//        int colCount = usedRange.ColumnCount;
//        int rowCount = usedRange.RowCount;

//        var columns = new List<string>();
//        for (int c = 0; c < colCount; c++)
//        {
//            string colName = sheet.Cells[usedRange.TopRowIndex, c].DisplayText.Trim();
//            columns.Add(colName);
//            allColumns.Add(colName);
//        }

//        int keyColIndex = columns.IndexOf(keyColumnName);
//        if (keyColIndex == -1) continue;

//        for (int r = usedRange.TopRowIndex + 1; r <= usedRange.BottomRowIndex; r++)
//        {
//            string key = sheet.Cells[r, keyColIndex].DisplayText.Trim();
//            if (string.IsNullOrEmpty(key)) continue;

//            if (!mergedData.ContainsKey(key))
//                mergedData[key] = new Dictionary<string, string>();

//            for (int c = 0; c < colCount; c++)
//            {
//                string colName = columns[c];
//                string value = sheet.Cells[r, c].DisplayText;
//                mergedData[key][colName] = value;
//            }
//        }
//    }

//    var colList = allColumns.ToList();
//    for (int i = 0; i < colList.Count; i++)
//        mergedSheet.Cells[0, i].Value = colList[i];

//    int row = 1;
//    foreach (var kvp in mergedData)
//    {
//        var dataRow = kvp.Value;
//        for (int i = 0; i < colList.Count; i++)
//        {
//            string colName = colList[i];
//            if (dataRow.ContainsKey(colName))
//                mergedSheet.Cells[row, i].Value = dataRow[colName];
//        }
//        row++;
//    }

//    workbook.Worksheets.ActiveWorksheet = mergedSheet;
//}


//private void LoadDataFromDatabaseToSpreadsheet(SpreadsheetControl spreadsheet, string connectionString, string tableName)
//{
//    MessageBox.Show("LoadDataFromDatabaseToSpreadsheet(SpreadsheetControl spreadsheet, string connectionString, string tableName)");
//    try
//    {
//        using (SqlConnection conn = new SqlConnection(connectionString))
//        {
//            conn.Open();
//            string query = $"SELECT * FROM [{tableName}]";
//            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
//            DataTable dataTable = new DataTable();
//            adapter.Fill(dataTable);

//            IWorkbook workbook = spreadsheet.Document;
//            DevExpress.Spreadsheet.Worksheet sheet = workbook.Worksheets[0];
//            sheet.Cells.Clear();

//            for (int col = 0; col < dataTable.Columns.Count; col++)
//            {
//                sheet.Cells[0, col].Value = dataTable.Columns[col].ColumnName;
//            }

//            for (int row = 0; row < dataTable.Rows.Count; row++)
//            {
//                for (int col = 0; col < dataTable.Columns.Count; col++)
//                {
//                    sheet.Cells[row + 1, col].Value = dataTable.Rows[row][col]?.ToString();
//                }
//            }

//            sheet.Name = tableName;
//        }
//    }
//    catch (Exception ex)
//    {
//        MessageBox.Show("Lỗi khi tải dữ liệu từ database:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//    }
//}


//private void LoadMySQLDataToSpreadsheet(string connectionString)
//{
//    MessageBox.Show("LoadMySQLDataToSpreadsheet(string connectionString)");
//    using (MySqlConnection connection = new MySqlConnection(connectionString))
//    {
//        try
//        {
//            connection.Open();
//            string query = "SHOW TABLES"; // Lấy danh sách bảng trong database
//            MySqlCommand cmd = new MySqlCommand(query, connection);
//            MySqlDataReader reader = cmd.ExecuteReader();

//            List<string> tables = new List<string>();
//            while (reader.Read())
//            {
//                tables.Add(reader[0].ToString());
//            }
//            reader.Close();
//            UpdateSheetListToComboBox();
//        }
//        catch (Exception ex)
//        {
//            MessageBox.Show("Lỗi khi tải dữ liệu từ MySQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }
//    }
//}