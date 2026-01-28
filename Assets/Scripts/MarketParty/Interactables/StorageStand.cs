using DG.Tweening;
using MarketParty.Characters;
using MarketParty.Managers;
using MarketParty.Players;
using MarketParty.Players.Pickables;
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

            product.transform.DOJump(storageBox.transform.position, 0.5f, 1, 0.5f);

            storageBox.AddProduct(product);

            return true;
        }
    }
}