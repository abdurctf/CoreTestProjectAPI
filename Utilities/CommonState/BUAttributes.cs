using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.CommonState
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BUAttributes : Attribute
    {
        private bool _isNftLog = true;

        public bool isNftLog
        {
            get
            { return _isNftLog; }
            set
            {
                if (value != _isNftLog)
                {
                    _isNftLog = value;
                }
            }
        }

        private string _displayValProperty;

        public string displayValProperty
        {
            get
            { return _displayValProperty; }
            set
            {
                if (value != _displayValProperty)
                {
                    _displayValProperty = value;
                }
            }
        }
        public bool IsDeletable { get; set; } = false;
    }


    [AttributeUsage(AttributeTargets.Class)]
    public class TableBlockAttribute : System.Attribute
    {
        public readonly string DisplayBlock;

        public TableBlockAttribute(string blockName)  // url is a positional parameter
        {
            this.DisplayBlock = blockName;
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class HideDetailViewAttribute : System.Attribute
    {

    }
}
