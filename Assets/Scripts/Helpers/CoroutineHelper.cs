using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class CoroutineHelper
{
    public static void SafeStopCoroutine(this MonoBehaviour _owner, Coroutine _coroutine)
    {
        if (_coroutine != null)
        {
            _owner.StopCoroutine(_coroutine);
        }
    }
    public static IEnumerator CallAfterSeconds(Action _action, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        _action();
    }

    //Starts the coroutine on its own, and returns the created Coroutine used with the coroutine
    public static Coroutine StartCallAfterSeconds(this MonoBehaviour _owner, Action _action, float _seconds)
    {
        return _owner.StartCoroutine(CallAfterSeconds(_action, _seconds));
    }
}
