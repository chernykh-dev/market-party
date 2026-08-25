using System.Collections.Generic;
using UnityEngine;

namespace MarketParty.Characters.Players.Pickables
{
    public class StorageBox : MonoBehaviour, IPickable
    {
        [SerializeField]
        private int _maxCount = 10;

        public Queue<Product> Products = new Queue<Product>();

        public bool IsEmpty => Products.Count == 0;

        public bool IsFull => Products.Count == _maxCount;

        public void AddProduct(Product product)
        {
            Products.Enqueue(product);
        }

        public Product GetProduct()
        {
            return Products.Dequeue();
        }
    }
}