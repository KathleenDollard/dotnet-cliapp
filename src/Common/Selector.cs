using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class Selector
    {
        private readonly List<SelectorItem> _items = new();
        public IEnumerable<SelectorItem> Items => _items;

        /// <summary>
        /// ItemTypeName is used in text like "Select the [ItemTypeName] you'd like to use"
        /// </summary>
        public string? ItemTypeName { get; set; }
        public bool Sorted { get; set; }

        public SelectorItem Add(SelectorItem newItem)
        {
            _items.Add(newItem);
            return newItem;
        }

        public void AddRange(IEnumerable<SelectorItem> newItems)
        {
            _items.AddRange(newItems);
        }
    }

    public class SelectorItem
    {
        public SelectorItem(string value, string? description = null, bool common = false)
        {
            Value = value;
            Description = description;
            Common = common;
        }

        public string Value { get; set; }

        public string? Description { get; set; } // For future use
        public bool Common { get; set; } // mimics Intellisense, but for now is hard coded in selection item
    }
}
