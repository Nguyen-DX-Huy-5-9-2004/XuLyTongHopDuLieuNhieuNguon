using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSpreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Spreadsheet;


namespace nckhTGF
{
    public partial class MergeSheet : DevExpress.XtraEditors.XtraForm
    {
        private SpreadsheetControl spreadsheetControl;
        public MergeSheet(SpreadsheetControl control)
        {
            InitializeComponent();
            spreadsheetControl = control;
            LoadSheets();
            checkedListBoxControl1.ItemCheck += checkedListBoxControl1_ItemCheck;
            simpleButton1.Click += simpleButton1_Click;
            simpleButton2.Click += simpleButton2_Click;
            simpleButton3.Click += simpleButton3_Click;
        }

        private void LoadSheets()
        {
            checkedListBoxControl1.Items.Clear();
            foreach (DevExpress.Spreadsheet.Worksheet sheet in spreadsheetControl.Document.Worksheets)
            {
                checkedListBoxControl1.Items.Add(sheet.Name, true);
            }
            UpdateOrderList();
            UpdateAttributeComboBox();
        }

        private void checkedListBoxControl1_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                UpdateOrderList();
                UpdateAttributeComboBox();
            });
        }

        private void UpdateOrderList()
        {
            listBoxControl1.Items.Clear();
            foreach (CheckedListBoxItem item in checkedListBoxControl1.CheckedItems)
            {
                listBoxControl1.Items.Add(item.Value.ToString());
            }
        }

        private void UpdateAttributeComboBox()
        {
            comboBoxEdit1.Properties.Items.Clear();
            var selectedSheets = checkedListBoxControl1.CheckedItems
                .OfType<CheckedListBoxItem>()
                .Select(x => x.Value.ToString())
                .ToList();

            if (selectedSheets.Count < 2) return;

            var commonColumns = new HashSet<string>(GetHeadersFromSheet(selectedSheets[0]));

            for (int i = 1; i < selectedSheets.Count; i++)
            {
                commonColumns.IntersectWith(GetHeadersFromSheet(selectedSheets[i]));
            }

            comboBoxEdit1.Properties.Items.AddRange(commonColumns.ToArray());
            if (comboBoxEdit1.Properties.Items.Count > 0)
                comboBoxEdit1.SelectedIndex = 0;
        }

        private List<string> GetHeadersFromSheet(string sheetName)
        {
            var sheet = spreadsheetControl.Document.Worksheets[sheetName];
            var headers = new List<string>();
            var usedRange = sheet.GetUsedRange();

            for (int col = 0; col < usedRange.ColumnCount; col++)
            {
                var text = sheet.Cells[0, col].DisplayText;
                if (!string.IsNullOrEmpty(text))
                    headers.Add(text);
            }
            return headers;
        }

        private void simpleButton1_Click(object sender, EventArgs e) // Move up
        {
            int index = listBoxControl1.SelectedIndex;
            if (index > 0)
            {
                var item = listBoxControl1.SelectedItem;
                listBoxControl1.Items.RemoveAt(index);
                listBoxControl1.Items.Insert(index - 1, item);
                listBoxControl1.SelectedIndex = index - 1;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e) // Move down
        {
            int index = listBoxControl1.SelectedIndex;
            if (index < listBoxControl1.Items.Count - 1 && index != -1)
            {
                var item = listBoxControl1.SelectedItem;
                listBoxControl1.Items.RemoveAt(index);
                listBoxControl1.Items.Insert(index + 1, item);
                listBoxControl1.SelectedIndex = index + 1;
            }
        }


private void simpleButton3_Click(object sender, EventArgs e)
    {
        var sheetNames = listBoxControl1.Items.Cast<string>().ToList();
        string keyColumn = comboBoxEdit1.SelectedItem?.ToString();

        if (sheetNames.Count < 2 || string.IsNullOrEmpty(keyColumn))
        {
            XtraMessageBox.Show("Chọn ít nhất 2 sheet và 1 khóa chung để gộp.");
            return;
        }

        var mergedData = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        var headersPerSheet = new Dictionary<string, List<string>>();
        var allExtraHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string sheetName in sheetNames)
        {
            var sheet = spreadsheetControl.Document.Worksheets[sheetName];
            var usedRange = sheet.GetUsedRange();
            var headers = new List<string>();

            for (int col = 0; col < usedRange.ColumnCount; col++)
            {
                headers.Add(sheet.Cells[0, col].DisplayText);
            }

            headersPerSheet[sheetName] = headers;
            int keyIndex = headers.IndexOf(keyColumn);
            if (keyIndex == -1) continue;

            for (int row = 1; row < usedRange.RowCount; row++)
            {
                string key = sheet.Cells[row, keyIndex].DisplayText?.Trim();
                if (string.IsNullOrEmpty(key)) continue;

                if (!mergedData.ContainsKey(key))
                    mergedData[key] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int col = 0; col < headers.Count; col++)
                {
                    if (col == keyIndex) continue;

                    string header = headers[col];
                    string cellValue = sheet.Cells[row, col].DisplayText;

                    if (!mergedData[key].ContainsKey(header))
                        mergedData[key][header] = cellValue;

                    allExtraHeaders.Add(header);
                }
            }
        }

        var finalHeaders = new List<string> { keyColumn };
        finalHeaders.AddRange(allExtraHeaders);

        int rowCount = mergedData.Count + 1;
        int colCount = finalHeaders.Count;

        object[,] dataArray = new object[rowCount, colCount];

        for (int col = 0; col < colCount; col++)
        {
            dataArray[0, col] = finalHeaders[col];
        }

        int dataRow = 1;
        foreach (var kvp in mergedData)
        {
            string key = kvp.Key;
            var rowDict = kvp.Value;

            dataArray[dataRow, 0] = key;
            for (int col = 1; col < colCount; col++)
            {
                string header = finalHeaders[col];
                if (rowDict.TryGetValue(header, out string value))
                {
                    dataArray[dataRow, col] = value;
                }
            }
            dataRow++;
        }

        spreadsheetControl.BeginUpdate();
        try
        {
            var existingSheet = spreadsheetControl.Document.Worksheets.FirstOrDefault(s => s.Name == "SheetMerge");
            if (existingSheet != null)
            {
                spreadsheetControl.Document.Worksheets.Remove(existingSheet);
            }

            var mergeSheet = spreadsheetControl.Document.Worksheets.Add("SheetMerge");
            var range = mergeSheet.Range.FromLTRB(0, 0, colCount - 1, rowCount - 1);

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

        XtraMessageBox.Show("Đã gộp dữ liệu thành công theo khóa vào SheetMerge.");
        this.Close();
    }




    //private void simpleButton3_Click(object sender, EventArgs e) // Merge horizontally by key
    //{
    //    var sheetNames = listBoxControl1.Items.Cast<string>().ToList();
    //    string keyColumn = comboBoxEdit1.SelectedItem?.ToString();

    //    if (sheetNames.Count < 2 || string.IsNullOrEmpty(keyColumn))
    //    {
    //        XtraMessageBox.Show("Vui lòng chọn ít nhất 2 sheet và 1 khóa chung để gộp.");
    //        return;
    //    }

    //    var mergedData = new Dictionary<string, Dictionary<string, string>>();
    //    var headersPerSheet = new Dictionary<string, List<string>>();

    //    foreach (string sheetName in sheetNames)
    //    {
    //        var sheet = spreadsheetControl.Document.Worksheets[sheetName];
    //        var usedRange = sheet.GetUsedRange();

    //        var headers = new List<string>();
    //        for (int col = 0; col < usedRange.ColumnCount; col++)
    //        {
    //            headers.Add(sheet.Cells[0, col].DisplayText);
    //        }
    //        headersPerSheet[sheetName] = headers;

    //        int keyIndex = headers.IndexOf(keyColumn);
    //        if (keyIndex == -1) continue;

    //        for (int row = 1; row < usedRange.RowCount; row++)
    //        {
    //            string key = sheet.Cells[row, keyIndex].DisplayText;
    //            if (!mergedData.ContainsKey(key))
    //                mergedData[key] = new Dictionary<string, string>();

    //            for (int col = 0; col < headers.Count; col++)
    //            {
    //                string column = headers[col];
    //                if (column == keyColumn) continue;
    //                string cellValue = sheet.Cells[row, col].DisplayText;
    //                mergedData[key][column] = cellValue;
    //            }
    //        }
    //    }

    //    var mergeSheet = spreadsheetControl.Document.Worksheets.Add("SheetMerge");
    //    var finalHeaders = new List<string> { keyColumn };
    //    //Tránh trùng/ghi đè cột: foreach (string sheetName in sheetNames)
    //    //{
    //    //    finalHeaders.AddRange(headersPerSheet[sheetName].Where(h => h != keyColumn).Select(h => sheetName + "." + h));
    //    //}
    //    foreach (string sheetName in sheetNames)
    //    {
    //        finalHeaders.AddRange(headersPerSheet[sheetName].Where(h => h != keyColumn));
    //    }

    //    for (int col = 0; col < finalHeaders.Count; col++)
    //    {
    //        mergeSheet.Cells[0, col].Value = finalHeaders[col];
    //    }

    //    int dataRow = 1;
    //    foreach (var key in mergedData.Keys)
    //    {
    //        //của Tránh trùng/ghi đè cột: mergeSheet.Cells[dataRow, 0].Value = key;
    //        mergeSheet.Cells[dataRow, 0].Value = key;
    //        for (int col = 1; col < finalHeaders.Count; col++)
    //        {
    //            string header = finalHeaders[col];
    //            if (mergedData[key].ContainsKey(header))
    //            {
    //                mergeSheet.Cells[dataRow, col].Value = mergedData[key][header];
    //            }
    //        }
    //        dataRow++;
    //    }

    //    XtraMessageBox.Show("Đã gộp dữ liệu thành công theo khóa vào SheetMerge.");
    //    this.Close();
    //}
}
}