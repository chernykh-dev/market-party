using System;
using UnityEngine;

namespace MarketParty
{
    public class DontDestoy : MonoBehaviour
    {
        [SerializeField] private string _tagForDestroy;

        private GameObject _instance;

        private void Awake()
        {
            if (!_instance)
            {
                _instance = gameObject;

                DontDestroyOnLoad(gameObject);

                return;
            }

            if (_instance == gameObject)
                return;

            Destroy(gameObject);
        }
    }
}