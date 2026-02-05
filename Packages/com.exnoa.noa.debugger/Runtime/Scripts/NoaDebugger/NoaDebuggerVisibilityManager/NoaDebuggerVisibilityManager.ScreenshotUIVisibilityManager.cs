namespace NoaDebugger
{
    sealed partial class NoaDebuggerVisibilityManager
    {
        public class ScreenshotUIVisibilityManager
        {
            readonly NoaController.ScreenshotTarget _screenshotTarget;

            bool _originalIsMainViewVisible;
            bool _originalIsControllerVisible;
            bool _originalIsTriggerButtonVisible;
            bool _originalIsOverlayVisible;
            bool _originalIsFloatingWindowVisible;
            bool _originalIsUIElementVisible;

            public ScreenshotUIVisibilityManager(NoaController.ScreenshotTarget screenshotTarget)
            {
                _screenshotTarget = screenshotTarget;
            }

            public void Before()
            {
                _originalIsMainViewVisible = NoaDebuggerVisibilityManager.IsMainViewActive;

                if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.MainView))
                {
                    if (_originalIsMainViewVisible)
                    {
                        NoaDebuggerManager.HideDebugger();
                    }
                }

                _originalIsControllerVisible = NoaDebuggerVisibilityManager.IsControllerActive;
                _originalIsTriggerButtonVisible = NoaDebuggerVisibilityManager.IsTriggerButtonActive;
                _originalIsOverlayVisible = NoaDebuggerVisibilityManager.IsOverlayVisible;
                _originalIsFloatingWindowVisible = NoaDebuggerVisibilityManager.IsFloatingWindowVisible;
                _originalIsUIElementVisible = NoaDebuggerVisibilityManager.IsAllUIElementActive;

                if (_originalIsMainViewVisible)
                {
                    if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.MainView))
                    {
                        if (_originalIsControllerVisible || _originalIsTriggerButtonVisible)
                        {
                            NoaDebuggerVisibilityManager.SetControllerVisible(false);
                            NoaDebuggerVisibilityManager.SetTriggerButtonVisible(false);
                        }

                        if (_originalIsOverlayVisible)
                        {
                            NoaDebuggerVisibilityManager.SetOverlayVisible(false);
                        }

                        if (_originalIsFloatingWindowVisible)
                        {
                            NoaDebuggerVisibilityManager.SetFloatingWindowVisible(false);
                        }
                    }
                }
                else
                {
                    if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.LaunchButton))
                    {
                        if (_originalIsControllerVisible || _originalIsTriggerButtonVisible)
                        {
                            NoaDebuggerVisibilityManager.SetControllerVisible(false);
                            NoaDebuggerVisibilityManager.SetTriggerButtonVisible(false);
                        }
                    }

                    if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.Overlays))
                    {
                        if (_originalIsOverlayVisible)
                        {
                            NoaDebuggerVisibilityManager.SetOverlayVisible(false);
                        }
                    }

                    if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.FloatingWindows))
                    {
                        if (_originalIsFloatingWindowVisible)
                        {
                            NoaDebuggerVisibilityManager.SetFloatingWindowVisible(false);
                        }
                    }
                }

                if (!_screenshotTarget.HasFlag(NoaController.ScreenshotTarget.UIElement))
                {
                    if (_originalIsUIElementVisible)
                    {
                        NoaDebuggerVisibilityManager.SetAllUIElementsVisible(false);
                    }
                }

                NoaDebuggerVisibilityManager.SetToastVisible(false);
            }

            public void After()
            {
                NoaDebuggerVisibilityManager.SetToastVisible(true);

                if (_originalIsControllerVisible)
                {
                    NoaDebuggerVisibilityManager.SetTriggerButtonVisible(true);
                    NoaDebuggerVisibilityManager.SetControllerVisible(true);
                }
                else if (_originalIsTriggerButtonVisible)
                {
                    NoaDebuggerVisibilityManager.SetTriggerButtonVisible(true);
                }

                if (_originalIsOverlayVisible)
                {
                    NoaDebuggerVisibilityManager.SetOverlayVisible(true);
                }

                if (_originalIsFloatingWindowVisible)
                {
                    NoaDebuggerVisibilityManager.SetFloatingWindowVisible(true);
                }

                if (_originalIsMainViewVisible)
                {
                    NoaDebuggerManager.ShowDebugger();
                }

                if (_originalIsUIElementVisible)
                {
                    NoaDebuggerVisibilityManager.SetAllUIElementsVisible(true);
                }
            }
        }
    }
}
