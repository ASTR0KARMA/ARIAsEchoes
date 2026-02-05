using System.Collections.Generic;

namespace NoaDebugger.DebugCommand
{
    abstract class CommandBase
    {
        const string DEFAULT_GROUP_NAME = "Others";
        const int DEFAULT_ORDER = int.MaxValue;

        public string DisplayName { get; }
        public string GroupName { get; }
        public int? GroupOrder { get; }
        public bool? IsCollapsedDefault { get; }
        public string TagName { get; }
        public string Description { get; }
        public int Order { get; }

        protected abstract string TypeName { get; }

        protected CommandBase(CommandBaseInfo info)
        {
            DisplayName = info._displayName;
            GroupName = info._groupName ?? CommandBase.DEFAULT_GROUP_NAME;
            GroupOrder = info._groupOrder;
            IsCollapsedDefault = info._isCollapsedDefault;
            TagName = info._tagName;
            Description = info._description;
            Order = info._order ?? CommandBase.DEFAULT_ORDER;
        }

        public virtual Dictionary<string, string> CreateDetailContext()
        {
            var context = new Dictionary<string, string>
            {
                {"Type", TypeName}
            };

            if (TagName != null)
            {
                context.Add("TagName", TagName);
            }

            if (Order != CommandBase.DEFAULT_ORDER)
            {
                context.Add("Order", $"{Order}");
            }

            return context;
        }
    }

    struct CommandBaseInfo
    {
        public string _displayName;
        public string _groupName;
        public int? _groupOrder;
        public bool? _isCollapsedDefault;
        public string _tagName;
        public string _description;
        public int? _order;

        public CommandBaseInfo(string displayName, string groupName = null, int? groupOrder = null,
            bool? isCollapsedDefault = null, string tagName = null, string description = null, int? order = null)
        {
            _displayName = displayName;
            _groupName = groupName;
            _groupOrder = groupOrder;
            _isCollapsedDefault = isCollapsedDefault;
            _tagName = tagName;
            _description = description;
            _order = order;
        }
    }
}
