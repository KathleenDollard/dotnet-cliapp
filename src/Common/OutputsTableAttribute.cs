using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class OutputsTableAttribute : Attribute
    {
        readonly string tableName;

        // This is a positional argument
        public OutputsTableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName
        {
            get { return tableName; }
        }
    }
}
