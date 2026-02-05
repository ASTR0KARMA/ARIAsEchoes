mergeInto(LibraryManager.library, {

    NoaDebuggerRegisterTouchFocusEvent: function()
    {
        var canvas = document.querySelector("#unity-canvas");
        if(!canvas)
        {
            console.warn("#unity-canvas not found.");
            return;
        }
        canvas.tabIndex = -1;
        canvas.addEventListener('touchstart', () =>
        {
			if(document.activeElement==canvas)
			{
				return;
			}

            if (document.activeElement.matches('input, textarea, select, [contenteditable]'))
            {
				return;
            }

			canvas.focus();
        }, { passive: true });
    },

    NoaDebuggerCopyClipboard: async function(str)
    {
        try
        {
            const text = UTF8ToString(str);
            await navigator.clipboard.writeText(text);
        }
        catch(e)
        {
        }
    },
});
