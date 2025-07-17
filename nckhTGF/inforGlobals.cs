using DevExpress.XtraExport.Xlsx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nckhTGF
{
    internal class inforGlobals
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string ConnectionString { get; set; }
        public static string SelectedDatabase { get; set; }
        public static string[] SelectedTables { get; set; }

        // Danh sách tất cả sheet trong app (mỗi cái có tên và nguồn dữ liệu)
        public static List<SheetInfo> SheetList { get; set; } = new List<SheetInfo>();
    }
}
