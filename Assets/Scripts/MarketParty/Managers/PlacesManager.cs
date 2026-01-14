using System.Collections.Generic;
using MarketParty.Interactables;
using UnityEngine;

namespace MarketParty.Managers
{
    public class PlacesManager : Singleton<PlacesManager>, IInitializable
    {
        public Transform EnterTransform { get; private set; }

        public Transform ExitTransform { get; private set; }

        public Cash CashTransform { get; private set; }

        public List<IStand> Stands { get; private set; }

        public void Init()
        {
            EnterTransform = GameObject.FindGameObjectWithTag("Enter").transform;

            ExitTransform = GameObject.FindGameObjectWithTag("Exit").transform;

            CashTransform = GameObject.FindGameObjectWithTag("Cash").GetComponent<Cash>();

            Stands = new List<IStand>(FindObjectsByType<Stand>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        }

        public IStand GetRandomStand()
        {
            return Stands[Random.Range(0, Stands.Count)];
        }
    }
}