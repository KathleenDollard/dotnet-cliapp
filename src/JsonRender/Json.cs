using Common;
using System;
using System.Collections.Generic;

namespace JsonRender
{
    public class Json
    {
        public static string Render(int uxLevel, Table table, string? rowPluralTypeName = null)
        {
            var nl = Environment.NewLine;
            var output = "{" + nl;
            var columnNames = new List<string>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.Columns[i];
                columnNames.Add(column.Header.Replace(" ", "_"));
            }

            rowPluralTypeName ??= "data";
            output += @$"   ""{rowPluralTypeName }"" : [{nl}";

            foreach (var row in table.Rows)
            {
                output += @"{" + nl;
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    output += $@"      ""{table.Columns[i].Header}"":""{row[i]}""";
                    output += i < table.Columns.Count - 1 ? ", " + nl  : "";
                }
                output += nl + "   },";
            }
            return output[..^1] + @"
   ]
}";

        }
        public static string Render(int uxLevel, Help help)
        {
            throw new NotImplementedException();
        }
    }
}
