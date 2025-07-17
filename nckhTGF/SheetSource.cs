using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nckhTGF
{
    public enum SourceType { Database, Excel, CSV, Generated }
    public enum DatabaseProvider { SqlServer, MySql }
    public class SheetSource
    {
        public SourceType Type { get; set; }
        public string SourcePathOrTableName { get; set; } // table name hoặc file path
        public string ConnectionString { get; set; }
        public DatabaseProvider? Provider { get; set; } // thêm Provider
        public bool IsDatabaseSource => Type == SourceType.Database;
        public bool IsExcelSource => Type == SourceType.Excel;
        public bool IsCSVSource => Type == SourceType.CSV;
        public override string ToString()
        {
            return Type switch
            {
                SourceType.Database => $"{Provider?.ToString() ?? "Unknown"} DB: {SourcePathOrTableName}",
                SourceType.Excel => $"Excel: {SourcePathOrTableName}",
                SourceType.CSV => $"CSV: {SourcePathOrTableName}",
                _ => "Tạo mới (Chưa có nguồn)"
            };
        }
    }
}
