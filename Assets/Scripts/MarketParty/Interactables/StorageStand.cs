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

            var randomStorageBoxPoint = new Vector3(Random.Range(-0.19f, 0.19f), 0f, Random.Range(-0.15f, 0.15f));

            // todo add rigidbody, remove parent.
            product.transform.SetParent(storageBox.transform);
            product.transform.DOLocalJump(randomStorageBoxPoint, 0.5f, 1, 0.5f);

            storageBox.AddProduct(product);

            return true;
        }
    }
}