using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        private static readonly object syslock = new object();

        public static T GetInstance()
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
