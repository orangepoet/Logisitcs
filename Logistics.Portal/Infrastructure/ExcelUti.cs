using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;

namespace Logistics.Infrastructure {
    public static class ExcelUti {
        private const int fileSize = 100000;
        private static readonly object syncobj = new object();
        public static byte[] ReadFileContents(DataTable dt, int x = 3, int y = 0) {
            try {
                string path = GetTemplatePath(dt.TableName);
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    byte[] buffer = new byte[fileSize];
                    using (Stream stream = new MemoryStream(buffer)) {
                        HSSFWorkbook workBook = new HSSFWorkbook(fs);
                        var sheet1 = workBook.GetSheetAt(0);
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            var row = sheet1.CreateRow(i + x);
                            for (int j = 0; j < dt.Columns.Count; j++) {
                                row.CreateCell(j + y).SetCellValue(dt.Rows[i][j].ToString());
                            }
                        }
                        workBook.Write(stream);
                        return buffer;
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine("ReadFileContents comes across Exception: " + ex.Message);
                throw;
            }
        }

        private static string GetTemplatePath(string dtName) {
            string path = "";
            switch (dtName) {
                case "Customer":
                    path = HttpContext.Current.Server.MapPath(@"~/Excels/Templates/客户管理.xls");
                    break;
                default:
                    break;
            }
            return path;
        }
    }
}