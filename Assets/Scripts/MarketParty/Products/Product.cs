using MarketParty.Products;
using UnityEngine;

namespace MarketParty
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField]
        public ProductName ProductName { get; set; }

        [field: SerializeField]
        public ProductType ProductType { get; set; }

        [field: SerializeField]
        public ProductTag ProductTag { get; set; }
    }
}