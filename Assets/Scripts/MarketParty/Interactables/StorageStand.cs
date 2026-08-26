using DG.Tweening;
using MarketParty.Characters;
using MarketParty.Characters.Players;
using MarketParty.Characters.Players.Pickables;
using MarketParty.Managers;
using UnityEngine;

namespace MarketParty.Interactables
{
    public class StorageStand : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private ProductTag _productTag;

        public bool Interact(PlayerHands playerHands)
        {
            if (playerHands.CurrentPickable is not StorageBox storageBox)
                return false;

            if (storageBox.IsFull)
                return false;

            var product = Instantiate(ProductsManager.Instance.GetRandomProductPrefab(_productTag), transform.position, Quaternion.identity);

            // todo add rigidbody, remove parent.
            product.transform.SetParent(storageBox.transform);
            product.transform
                .DOLocalJump(Vector3.up * 0.25f, 0.5f, 1, 0.5f)
                .OnComplete(() =>
                {
                    product.transform.SetParent(null);
                    product.EnablePhysics();
                });

            storageBox.AddProduct(product);

            return true;
        }
    }
}