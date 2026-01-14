using UnityEngine;

namespace MarketParty
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField]
        public ProductTag ProductTag { get; set; }
    }
}