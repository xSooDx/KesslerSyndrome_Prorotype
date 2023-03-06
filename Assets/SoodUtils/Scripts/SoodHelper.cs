using System.Collections;
using UnityEngine;

namespace SoodUtils
{
    public class SoodHelper : GenericSingletonClass<SoodHelper>
    {
        public static Coroutine StartCoroutineGlobaly(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
    }
}