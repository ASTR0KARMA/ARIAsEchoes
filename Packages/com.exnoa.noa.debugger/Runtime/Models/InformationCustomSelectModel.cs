using System;
using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    sealed class InformationCustomSelectModel : InformationBaseSelectModel
    {
        CustomInformationModel _model;

        readonly List<InformationCustomGroup> _customGroups;
        ISelectableGroupProvider _groupProvider;

        public override string TabName => "Custom";
        protected override ISelectableGroupProvider GroupProvider => _groupProvider;

        public InformationCustomSelectModel() : base(InformationTabType.Custom)
        {
            _customGroups = new List<InformationCustomGroup>();
            _groupProvider = new InformationCustomProviderAdapter(_customGroups);
        }

        public void UpdateCustomInfo(CustomInformationModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _model = model;

            _customGroups.Clear();
            var newGroups = model.GetAllGroups();
            if (newGroups != null)
            {
                _customGroups.AddRange(newGroups);
            }

            _groupProvider = new InformationCustomProviderAdapter(_customGroups);
        }

        IKeyValueParser _CreateGroupParser(string groupName, List<InformationCustomKeyValue> items)
        {
            var itemParsers = new List<IKeyValueParser>();

            foreach (var item in items.OrderBy(i => i.Order))
            {
                string key = item.Key;
                string value = item.Parameter.GetStringValue();
                itemParsers.Add(new KeyValueParser(key, value));
            }

            return new KeyObjectParser(groupName, itemParsers.ToArray());
        }
    }
}
