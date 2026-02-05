mergeInto(LibraryManager.library, {
    NoaDebuggerSetupVisibilityChangeCallback: function(callbackPtr) {
        document.addEventListener('visibilitychange', function() {
            var isVisible = !document.hidden ? 1 : 0;
            dynCall('vi', callbackPtr, [isVisible]);
        });
    },

    NoaDebuggerIsPageVisible: function() {
        return !document.hidden;
    }
});
