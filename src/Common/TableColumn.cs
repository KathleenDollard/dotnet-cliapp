using System;

namespace Common
{
    /// <summary>
    /// Represents a table column.
    /// </summary>
    public sealed class TableColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// </summary>
        /// <param name="header">The <see cref="string"/> instance to use as the table column header.</param>
        public TableColumn(string header,
                           TableColumnType columnType = TableColumnType.Default,
                           TableColumnAlignment alignment = TableColumnAlignment.Left)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Alignment = alignment;
            ColumnType = columnType;
        }

        /// <summary>
        /// Gets the column header.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// Gets or sets the column footer.
        /// </summary>
        public string? Footer { get; set; }

        public TableColumnType ColumnType { get; set; }
        public TableColumnAlignment Alignment { get; set; }

        public bool Hide { get; set; }
    }
}
