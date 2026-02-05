using System;

namespace NoaDebugger
{
    /// <summary>
    /// Specifies the group name of the debug command
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class CommandGroupAttribute : Attribute
    {
        readonly public string _name;
        readonly public int? _order = null;
        readonly public bool? _isCollapsedDefault = null;

        public CommandGroupAttribute(string name)
        {
            _name = name;
        }

        public CommandGroupAttribute(string name, int order)
        {
            _name = name;
            _order = order;
        }

        public CommandGroupAttribute(string name, bool isCollapsedDefault)
        {
            _name = name;
            _isCollapsedDefault = isCollapsedDefault;
        }

        public CommandGroupAttribute(string name, int order, bool isCollapsedDefault)
        {
            _name = name;
            _order = order;
            _isCollapsedDefault = isCollapsedDefault;
        }
    }
}
