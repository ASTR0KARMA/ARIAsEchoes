using System;
using System.Collections.Generic;

namespace NoaDebugger
{
    /// <summary>
    /// Manages the registration of custom data
    /// </summary>
    public static class CustomInformationRegister
    {
        /// <summary>
        /// Adds a custom group
        /// </summary>
        public static void AddGroup(string name, string displayName = "", int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.AddCustomGroup(name, displayName, order);
        }

        /// <summary>
        /// Adds a key-value pair to the custom group
        /// </summary>
        [Obsolete("use AddImmutableStringKeyValue instead.")]
        public static void AddKeyValue(string groupName, string keyName, Func<string> getValue, string displayName = "",
                                       int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds an immutable int key-value pair to the custom group
        /// </summary>
        public static void AddImmutableIntKeyValue(string groupName, string keyName, Func<int> getValue, string displayName = "",
                                                   int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds an immutable bool key-value pair to the custom group
        /// </summary>
        public static void AddImmutableBoolKeyValue(string groupName, string keyName, Func<bool> getValue, string displayName = "",
                                                    int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds an immutable string key-value pair to the custom group
        /// </summary>
        public static void AddImmutableStringKeyValue(string groupName, string keyName, Func<string> getValue, string displayName = "",
                                                      int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds an immutable float key-value pair to the custom group
        /// </summary>
        public static void AddImmutableFloatKeyValue(string groupName, string keyName, Func<float> getValue, string displayName = "",
                                                     int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds an immutable enum key-value pair to the custom group
        /// </summary>
        public static void AddImmutableEnumKeyValue(string groupName, string keyName, Func<Enum> getValue, string displayName = "",
                                                    int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetImmutableParameter(getValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds a mutable int key-value pair to the custom group
        /// </summary>
        public static void AddMutableIntKeyValue(string groupName, string keyName, Func<int> getValue, Action<int> setValue, string displayName = "",
                                                 int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetParameter(groupName, getValue, setValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds a mutable bool key-value pair to the custom group
        /// </summary>
        public static void AddMutableBoolKeyValue(string groupName, string keyName, Func<bool> getValue, Action<bool> setValue, string displayName = "",
                                                  int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetParameter(groupName, getValue, setValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds a mutable string key-value pair to the custom group
        /// </summary>
        public static void AddMutableStringKeyValue(string groupName, string keyName, Func<string> getValue, Action<string> setValue, string displayName = "",
                                                    int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetParameter(groupName, getValue, setValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds a mutable float key-value pair to the custom group
        /// </summary>
        public static void AddMutableFloatKeyValue(string groupName, string keyName, Func<float> getValue, Action<float> setValue, string displayName = "",
                                                   int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetParameter(groupName, getValue, setValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Adds a mutable enum key-value pair to the custom group
        /// </summary>
        public static void AddMutableEnumKeyValue(string groupName, string keyName, Func<Enum> getValue, Action<Enum> setValue, string displayName = "",
                                                  int order = Int32.MaxValue)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            InformationCustomKeyValue keyValue = new InformationCustomKeyValue(keyName, displayName, order);
            keyValue.SetParameter(groupName, getValue, setValue);

            presenter.AddCustomGroupKeyValue(groupName, keyValue);
        }

        /// <summary>
        /// Retrieves the int value associated with the specified key from the custom data
        /// </summary>
        public static NoaCustomInformationIntValue GetCustomInformationIntValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetCustomIntValue(groupName, keyName);
        }

        /// <summary>
        /// Retrieves the float value associated with the specified key from the custom data
        /// </summary>
        public static NoaCustomInformationFloatValue GetCustomInformationFloatValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetCustomFloatValue(groupName, keyName);
        }

        /// <summary>
        /// Retrieves the bool value associated with the specified key from the custom data
        /// </summary>
        public static NoaCustomInformationBoolValue GetCustomInformationBoolValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetCustomBoolValue(groupName, keyName);
        }

        /// <summary>
        /// Retrieves the string value associated with the specified key from the custom data
        /// </summary>
        public static NoaCustomInformationStringValue GetCustomInformationStringValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetCustomStringValue(groupName, keyName);
        }

        /// <summary>
        /// Retrieves the Enum value associated with the specified key from the custom data
        /// </summary>
        public static NoaCustomInformationEnumValue GetCustomInformationEnumValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return default;
            }

            return presenter.GetCustomEnumValue(groupName, keyName);
        }

        /// <summary>
        /// Retrieves group information that matches the specified group name from the custom data
        /// </summary>
        public static NoaCustomInformationGroup GetCustomInformationGroup(string groupName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetNoaCustomGroup(groupName);
        }

        /// <summary>
        /// Retrieves all custom data
        /// </summary>
        public static List<NoaCustomInformationGroup> GetCustomInformationAll()
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetAllCustomGroups();
        }

        /// <summary>
        /// Removes a key-value pair from the custom group
        /// </summary>
        public static void RemoveKeyValue(string groupName, string keyName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.RemoveCustomKeyValue(groupName, keyName);
        }

        /// <summary>
        /// Removes the custom group
        /// </summary>
        public static void RemoveGroup(string groupName)
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.RemoveCustomGroup(groupName);
        }

        /// <summary>
        /// Removes all custom groups
        /// </summary>
        public static void RemoveAll()
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.RemoveAllCustomGroup();
        }

        /// <summary>
        /// Refresh the content displayed in the Custom tab
        /// </summary>
        public static void RefreshView()
        {
            InformationPresenter presenter = NoaDebugger.GetPresenter<InformationPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.RefreshCustomView();
        }
    }
}
