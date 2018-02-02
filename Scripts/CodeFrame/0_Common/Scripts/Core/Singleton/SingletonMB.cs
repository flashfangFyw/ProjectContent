// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace ffDevelopmentSpace
{
    /// <summary>
    /// 继承自 MonoBehaviour 单例，适用于场景内唯一的对象的脚本挂靠，
    /// </summary>微软hollens 写法
    /// <typeparam name="T"></typeparam>
    public class SingletonMB<T> : MonoBehaviour where T : SingletonMB<T>
    {
        private static T _Instance;

        /// <summary>
        /// 为与Singleton<T> 区分，所以 取单例时用Instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<T>();
                }
                return _Instance;
            }
        }
    }
}