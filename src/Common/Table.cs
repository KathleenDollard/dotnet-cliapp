using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    /// <summary>
    /// A renderable table.
    /// </summary>
    public sealed class Table 
    {
        private readonly List<TableColumn> _columns= new List<TableColumn>();
        private readonly List<TableRow> _rows= new List<TableRow>();

        /// <summary>
        /// Gets the table columns.
        /// </summary>
        public IReadOnlyList<TableColumn> Columns => _columns;

        /// <summary>
        /// Gets the table rows.
        /// </summary>
        public IReadOnlyList<TableRow> Rows => _rows;

         /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string? Title { get; set; }
        public bool DisplayClosingBar { get; set; }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumn(TableColumn column)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (_rows.Count > 0)
            {
                throw new InvalidOperationException("Cannot add new columns to table with existing rows.");
            }

            _columns.Add(column);
            return this;
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumn(string header,
                               TableColumnType columnType = TableColumnType.Default,
                               TableColumnAlignment alignment = TableColumnAlignment.Left)
            => AddColumn(new TableColumn(header, columnType, alignment ));

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(IEnumerable<string> columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            var rowColumnCount = columns.Count();
            if (rowColumnCount > _columns.Count)
            {
                throw new InvalidOperationException("The number of row columns are greater than the number of table columns.");
            }

            _rows.Add(new TableRow(columns));

            // Need to add missing columns?
            if (rowColumnCount < _columns.Count)
            {
                var diff = _columns.Count - rowColumnCount;
                for (int i = 0; i < diff; i++)
                {
                    _rows.Last().Add(string.Empty);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(params string[] columns) 
            => AddRow(columns.ToList());
    }
}
