using System;
using UnityEngine;

namespace MarketParty
{
    public class StandProductPlace : MonoBehaviour
    {
        public Product Product { get; set; }

        public bool IsEmpty { get; set; } = true;

        public void SetProduct(Product productPrefab)
        {
            Product = Instantiate(productPrefab, transform);

            IsEmpty = false;
        }
    }
}