using System;
using UnityEngine;

namespace MarketParty
{
    public class GlobalSingleton<T> : MonoBehaviour where T : MonoBehaviour, IInitializable
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindFirstObjectByType<T>();
                    _instance.Init();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            if (Instance == this)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}