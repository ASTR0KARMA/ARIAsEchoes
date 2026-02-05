#if NOA_DEBUGGER
using NoaDebugger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebuggerDemo
{
    public class DebugCommandRegistrationSample
    {
        const string CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME = "[Demo] AddDynamicCommand";
        const string CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME_EXTERNAL_LINK = "[Demo] ExternalLinkCommand";
        const string COMMUNICATION_OPERATIONS_GROUP_NAME = "Object Control";
        const string EXTERNAL_LINK_GROUP_NAME = "External Link";

        UnityAction<ObjectType, int> onSpawnObject;
        UnityAction onDestroyAll;

        ObjectType spawnObjectType;
        int spawnObjectCount;
        bool isAutoSpawn;
        float spawnObjectInterval;

        MonoBehaviour monoBehaviour;
        bool isProcessAutoSpawn = false;
        List<CommandDefinition> _dynamicCommandDefinitions;

        public enum ObjectType
        {
            Cube,
            Sphere,
            Capsule
        }

        public DebugCommandRegistrationSample(UnityAction<ObjectType, int> onSpawnObject, UnityAction onDestroyAll, MonoBehaviour monoBehaviour)
        {
            _dynamicCommandDefinitions = new List<CommandDefinition>();
            this.onSpawnObject = onSpawnObject;
            this.onDestroyAll = onDestroyAll;
            this.monoBehaviour = monoBehaviour;
            ResetDynamicVariables();
            AddInformationCategorySample();
            AddDebugCommandSample();
            AddDynamicDebugCommandSample();
            AddDynamicDebugCommandExternalLinkSample();
            DebugCommandRegister.RefreshProperty();
        }

        public void Destroy()
        {
            onDestroyAll?.Invoke();
            isProcessAutoSpawn = default;
            onSpawnObject = default;
            onDestroyAll = default;
            monoBehaviour = default;
            foreach (CommandDefinition commandDefinition in _dynamicCommandDefinitions)
            {
                DebugCommandRegister.RemoveCommand(commandDefinition);
            }
        }

        void ResetDynamicVariables()
        {
            spawnObjectType = ObjectType.Cube;
            spawnObjectCount = 1;
            spawnObjectInterval = 1.0f;
            SetIsAutoSpawn(true);
        }

        void AddInformationCategorySample()
        {
            string groupName = "Runtime";
            string keyName = "Current date and time";
            if (!string.IsNullOrEmpty(CustomInformationRegister.GetCustomInformationStringValue(groupName, keyName)?.Value))
            {
                return;
            }

            CustomInformationRegister.AddImmutableStringKeyValue(
                groupName,
                keyName,
                () => DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss"));
        }

        void AddDebugCommandSample()
        {
            var instance = DebugCommandRegister.GetCategoryInstance<ApiCommandSample>("[Demo] AddCommand");
            if (instance != null)
            {
                return;
            }

            DebugCommandRegister.AddCategory<ApiCommandSample>("[Demo] AddCommand");
        }

        void AddDynamicDebugCommandSample()
        {
            CommandDefinition dynamicCommandObjectType =
                DebugCommandRegister.CreateMutableEnumProperty(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                    "Object Type",
                    () => spawnObjectType,
                    value => spawnObjectType = value,
                    new Attribute[]
                    {
                        new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME)
                    });

            DebugCommandRegister.AddCommand(dynamicCommandObjectType);

            CommandDefinition dynamicCommandCount =
                DebugCommandRegister.CreateMutableIntProperty(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                    "Spawn Count",
                    () => spawnObjectCount,
                    value => spawnObjectCount = value,
                    new Attribute[]
                    {
                        new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME),
                        new CommandInputRangeAttribute(min: 1, max: 10),
                    });

            DebugCommandRegister.AddCommand(dynamicCommandCount);

            CommandDefinition dynamicCommandAutoSpawn =
                DebugCommandRegister.CreateMutableBoolProperty(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                    "Auto Spawn",
                    () => isAutoSpawn,
                    value => SetIsAutoSpawn(value),
                    new Attribute[]
                    {
                        new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME)
                    });

            DebugCommandRegister.AddCommand(dynamicCommandAutoSpawn);

            CommandDefinition dynamicCommandInterval =
                DebugCommandRegister.CreateMutableFloatProperty(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                    "Interval",
                    () => spawnObjectInterval,
                    value => { spawnObjectInterval = value; },
                    new Attribute[]
                    {
                        new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME),
                        new CommandInputRangeAttribute(min: 0.3f, max: 2.0f),
                    });

            DebugCommandRegister.AddCommand(dynamicCommandInterval);

            CommandDefinition commandMethodSpawn =
                DebugCommandRegister.CreateMethod(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                    "Spawn",
                    () => onSpawnObject(spawnObjectType, spawnObjectCount),
                    new Attribute[]
                    {
                        new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME)
                    });

            DebugCommandRegister.AddCommand(commandMethodSpawn);

            CommandDefinition commandMethodDestroyAll = DebugCommandRegister.CreateMethod(
                CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME,
                "DestroyAll",
                () => onDestroyAll(),
                new Attribute[]
                {
                    new CommandGroupAttribute(COMMUNICATION_OPERATIONS_GROUP_NAME)
                });

            DebugCommandRegister.AddCommand(commandMethodDestroyAll);

            _dynamicCommandDefinitions.Add(dynamicCommandObjectType);
            _dynamicCommandDefinitions.Add(dynamicCommandCount);
            _dynamicCommandDefinitions.Add(dynamicCommandAutoSpawn);
            _dynamicCommandDefinitions.Add(dynamicCommandInterval);
            _dynamicCommandDefinitions.Add(commandMethodSpawn);
            _dynamicCommandDefinitions.Add(commandMethodDestroyAll);
        }

        void AddDynamicDebugCommandExternalLinkSample()
        {
            CommandDefinition commandOfficialSiteLink =
                DebugCommandRegister.CreateMethod(
                    CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME_EXTERNAL_LINK,
                    "Official Site",
                    () => Application.OpenURL("https://www.noadebugger.dmmgames.com/"),
                    new Attribute[]
                    {
                        new CommandGroupAttribute(EXTERNAL_LINK_GROUP_NAME)
                    });

            DebugCommandRegister.AddCommand(commandOfficialSiteLink);

            CommandDefinition commandStoreLink = DebugCommandRegister.CreateMethod(
                CATEGORY_ADD_DYNAMIC_DEBUG_COMMAND_NAME_EXTERNAL_LINK,
                "Store",
                () => Application.OpenURL("https://u3d.as/3cCN"),
                new Attribute[]
                {
                    new CommandGroupAttribute(EXTERNAL_LINK_GROUP_NAME)
                });

            DebugCommandRegister.AddCommand(commandStoreLink);

            _dynamicCommandDefinitions.Add(commandOfficialSiteLink);
            _dynamicCommandDefinitions.Add(commandStoreLink);
        }

        void SetIsAutoSpawn(bool isAutoSpawnFlag)
        {
            isAutoSpawn = isAutoSpawnFlag;

            if (isAutoSpawn)
            {
                monoBehaviour.StartCoroutine(AutoSpawn());
            }
        }

        void SpawnObject()
        {
            onSpawnObject?.Invoke(spawnObjectType, spawnObjectCount);
        }

        IEnumerator AutoSpawn()
        {
            if (isProcessAutoSpawn)
            {
                yield break;
            }

            while (isAutoSpawn)
            {
                isProcessAutoSpawn = true;
                float totalInterval = 0;

                while (totalInterval < spawnObjectInterval)
                {
                    if (Time.timeScale > 0)
                    {
                        totalInterval += Time.deltaTime;
                    }

                    yield return null;
                }

                SpawnObject();
                isProcessAutoSpawn = false;
            }
        }
    }
}
#endif
