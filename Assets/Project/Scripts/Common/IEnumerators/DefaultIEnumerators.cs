using System;
using System.Collections;
using UnityEngine;

namespace Common.IEnumerators
{
    public static class DefaultIEnumerators
    {
        public static IEnumerator EasyTimer(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }
    }
}