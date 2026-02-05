#if NOA_DEBUGGER
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorShortcutSettings : PackageEditorSettingsBase
    {
        bool _enableAllShortcuts;

        List<ShortcutAction> _shortcutActions;

        bool _isDetectingInput;
        ShortcutAction _currentAction;
        Dictionary<ShortcutActionType, bool> _foldoutStates;

        readonly EditorWindow _window;

        public PackageEditorShortcutSettings(NoaDebuggerSettings settings, EditorWindow window) : base(settings)
        {
            _window = window;
        }

        public override void ResetTmpDataWithSettings()
        {
            _enableAllShortcuts = _settings.EnableAllShortcuts;
            _shortcutActions = new List<ShortcutAction>();
            foreach (var shortcutAction in _settings.EnabledShortcutActions)
            {
                var action = new ShortcutAction(shortcutAction.Type)
                {
                    combination = new ShortcutInputCombination
                    {
                        isEnabled = shortcutAction.combination.isEnabled,
                        keyboard = new ShortcutKeyboardBinding
                        {
                            key = shortcutAction.combination.keyboard.key,
                            modifiers = shortcutAction.combination.keyboard.modifiers
                        }
                    }
                };

                _shortcutActions.Add(action);
            }
        }

        public override void ApplySettings()
        {
            _settings.EnableAllShortcuts = _enableAllShortcuts;
            _settings.EnabledShortcutActions = _shortcutActions;
        }

        public override void ResetDefault()
        {
            _enableAllShortcuts = NoaDebuggerDefine.DEFAULT_ENABLE_ALL_SHORTCUTS;
            _shortcutActions = NoaDebuggerShortcutSettings.GetDefaultShortcutActions();
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Shortcuts", ResetDefault);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            _enableAllShortcuts = EditorGUILayout.Toggle("Enable all shortcuts", _enableAllShortcuts);

            EditorGUILayout.Space(1);

            EditorGUILayout.LabelField("Shortcut commands");

            EditorGUILayout.HelpBox(
                "The control key on macOS is not supported for shortcut registration.",
                MessageType.Info);

            foreach (var action in _shortcutActions)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                NoaDebuggerDefine.ShortCutTriggerType triggerType = NoaDebuggerDefine.GetTriggerType(action.Type);

                EditorGUILayout.BeginHorizontal();
                {
                    string commandNameStr = action.GetDisplayTextByActionType();

                    if (_foldoutStates == null)
                    {
                        _foldoutStates = new Dictionary<ShortcutActionType, bool>();
                    }

                    _foldoutStates.TryAdd(action.Type, false);

                    _foldoutStates[action.Type] = EditorGUILayout.Foldout(
                        _foldoutStates[action.Type],
                        commandNameStr,
                        true
                    );

                    action.combination.isEnabled = EditorGUILayout.Toggle(
                        action.combination.isEnabled,
                        GUILayout.Width(20)
                    );

                    if (GUILayout.Button("Reset", GUILayout.Width(50)))
                    {
                        _OpenSettingsResetDialog(
                            $"{commandNameStr} shortcut setting",
                            () => action.combination = NoaDebuggerShortcutSettings.GetDefaultCombination(action.Type));
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(2);

                if (_foldoutStates[action.Type])
                {
                    EditorGUI.indentLevel++;

                    using (new EditorGUI.DisabledGroupScope(!action.combination.isEnabled))
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel("Trigger type");
                            EditorGUILayout.LabelField(
                                action.GetDisplayTextByTriggerType(),
                                GUILayout.MaxWidth(150));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel("Keyboard shortcut");

                            var isEditing = _isDetectingInput && _currentAction == action;
                            var displayText = isEditing ? "Waiting for input..." : action.GetDisplayTextByCombination();

                            var style = new GUIStyle(EditorStyles.label);
                            if (isEditing)
                            {
                                style.normal.textColor = NoaDebuggerDefine.TextColors.LogLightBlue;
                            }

                            EditorGUILayout.LabelField(displayText, style, GUILayout.MaxWidth(150));

                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Edit", GUILayout.Width(40)))
                            {
                                if (!isEditing)
                                {
                                    _isDetectingInput = true;
                                    _currentAction = action;
                                }
                                else
                                {
                                    _isDetectingInput = false;
                                    _currentAction = null;
                                }
                                _window.Repaint();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            if (_isDetectingInput && _currentAction != null)
            {
                var current = Event.current;

                if (current != null && current.type == EventType.KeyDown)
                {
                    if (current.keyCode == KeyCode.Escape)
                    {
                        _isDetectingInput = false;
                        _currentAction = null;
                        current.Use();
                        _window.Repaint();
                        return;
                    }

                    if (IsModifierKey(current.keyCode))
                    {
                        current.Use();
                        return;
                    }

                    bool IsModifierKey(KeyCode keyCode)
                    {
                        return keyCode == KeyCode.LeftShift ||
                               keyCode == KeyCode.RightShift ||
                               keyCode == KeyCode.LeftControl ||
                               keyCode == KeyCode.RightControl ||
                               keyCode == KeyCode.LeftAlt ||
                               keyCode == KeyCode.RightAlt ||
                               keyCode == KeyCode.LeftCommand ||
                               keyCode == KeyCode.RightCommand;
                    }

                    var modifiers = new List<ShortcutModifierKey>();
#if UNITY_EDITOR_OSX
                    if (current.command)
                    {
                        modifiers.Add(ShortcutModifierKey.Ctrl);
                    }
                    if (current.alt)
                    {
                        modifiers.Add(ShortcutModifierKey.Alt);
                    }
                    if (current.shift)
                    {
                        modifiers.Add(ShortcutModifierKey.Shift);
                    }
                    if (current.control)
                    {
                        LogModel.LogWarning("The control key cannot be used for shortcuts on macOS. Use ⌘ instead.");
                    }
#else
                    if (current.control)
                    {
                        modifiers.Add(ShortcutModifierKey.Ctrl);
                    }
                    if (current.alt)
                    {
                        modifiers.Add(ShortcutModifierKey.Alt);
                    }
                    if (current.shift)
                    {
                        modifiers.Add(ShortcutModifierKey.Shift);
                    }
#endif
                    _currentAction.combination.keyboard.modifiers = modifiers;

                    _currentAction.combination.keyboard.key = UnityInputUtils.GetCurrentKey(current);

                    _isDetectingInput = false;
                    _currentAction = null;
                    current.Use();
                }
            }
        }
    }
}
#endif
