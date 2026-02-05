using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NoaDebugger
{
    sealed class ApplicationOperateModel : ModelBase
    {
        public Func<bool> OnTransition { get; set; }

        public UnityAction OnFinishTransition { get; set; }

        public void TransitionToInitialScene()
        {
            bool isEarlyReturn = !(OnTransition?.Invoke() ?? true);

            if (isEarlyReturn)
            {
                NoaDebugger.ShowToast(new ToastViewLinker() {_label = NoaDebuggerDefine.CustomApplicationResetText});
                return;
            }

            GlobalCoroutine.Run(_LoadFirstScene(), OnFinishTransition);
        }

        IEnumerator _LoadFirstScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
