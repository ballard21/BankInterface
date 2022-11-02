using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JobAssignment
{
    public class DatasTable
    {
        public List<string> Columns;
        public List<string[]> Data;
    }
    public class DataExtractor
    {
        public static DatasTable GetDatasTable(DataTable table)
        {

            var columns = new List<string>();
            var tableData = new List<string[]>();
            foreach (DataColumn column in table.Columns)
                columns.Add(column.ColumnName);

            foreach (DataRow drow in table.Rows)
            {
                var data = new List<string>();
                for (int x = 0; x < columns.Count; x++)
                    data.Add(drow[x].ToString());
                tableData.Add(data.ToArray());

            }
            return new DatasTable
            {
                Columns = columns,
                Data = tableData,

            };
        }
    }
}