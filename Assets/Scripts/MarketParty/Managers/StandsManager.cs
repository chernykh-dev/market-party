using System;
using System.Collections.Generic;
using System.Linq;
using MarketParty.Interactables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MarketParty.Managers
{
    public class StandsManager : Singleton<StandsManager>, IInitializable
    {
        private List<Stand> _stands;

        public void Init()
        {
            _stands = FindObjectsByType<Stand>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID).ToList();
        }

        public void FillStands(float ratio)
        {
            foreach (var stand in _stands)
            {
                stand.Init();

                var fillAmount = (int)(stand.MaxProducts * ratio);

                print($"{fillAmount} / {stand.MaxProducts}");

                var indices = SampleDistict(stand.MaxProducts, fillAmount);

                foreach (var index in indices)
                {
                    stand.InitializeProductByTag(index);
                }
            }
        }

        private static HashSet<int> SampleDistict(int n, int k)
        {
            if (k < 0 || k > n)
                throw new ArgumentOutOfRangeException(nameof(k));

            var result = new HashSet<int>(k);

            for (var i = n - k; i < n; i++)
            {
                var t = Random.Range(0, i + 1);      // [0, i]

                if (!result.Add(t))
                    result.Add(i);
            }

            return result;
        }
    }
}