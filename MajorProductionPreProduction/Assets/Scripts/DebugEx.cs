using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEx
{
    public static void Log(object message, UnityEngine.Object context = null)
    {
#if UNITY_EDITOR
        Debug.Log(message.ToString(), context);
#endif
    }
}
