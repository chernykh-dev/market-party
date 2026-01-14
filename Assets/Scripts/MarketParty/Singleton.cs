using UnityEngine;

namespace MarketParty
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour, IInitializable
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
                }

                return _instance;
            }
        }
    }
}