using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Represents a table row.
    /// </summary>
    public sealed class TableRow : IEnumerable<string>
    {
        private readonly List<string> _items;

        internal bool IsHeader { get; }
        internal bool IsFooter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableRow"/> class.
        /// </summary>
        /// <param name="items">The row items.</param>
        public TableRow(IEnumerable<string> items)
            : this(items, false, false)
        {
        }
        private TableRow(IEnumerable<string> items, bool isHeader, bool isFooter)
        {
            _items = new List<string>(items ?? Array.Empty<string>());

            IsHeader = isHeader;
            IsFooter = isFooter;
        }

        /// <summary>
        /// Gets a row item at the specified table column index.
        /// </summary>
        /// <param name="index">The table column index.</param>
        /// <returns>The row item at the specified table column index.</returns>
        public string this[int index]
        {
            get => _items[index];
        }


        internal static TableRow Header(IEnumerable<string> items)
        {
            return new TableRow(items, true, false);
        }

        internal static TableRow Footer(IEnumerable<string> items)
        {
            return new TableRow(items, false, true);
        }

        internal void Add(string item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
        }

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
