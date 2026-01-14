using UnityEngine;

namespace MarketParty
{
    public interface IStand
    {
        public Transform transform { get; }

        bool ContainsProducts();

        bool IsFull();

        Product ReserveRandomProduct(Transform to);

        void ExtractProduct(Product product, Transform to);

        void AddProduct(Transform from);
    }
}