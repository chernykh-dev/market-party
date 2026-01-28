using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MarketParty.Characters;
using MarketParty.Managers;
using MarketParty.Players;
using MarketParty.Players.Pickables;
using MarketParty.UI;
using UnityEngine;

namespace MarketParty.Interactables
{
    public class Stand : MonoBehaviour, IStand, IInteractable, IInfoable
    {
        [SerializeField]
        private ProductTag _productTag;

        [SerializeField]
        private StandInfoPopUp _standInfoPopUpPrefab;

        private List<int> _availableProducts = new List<int>();
        private List<StandProductPlace> _productPlaces;
        private int _maxProducts;

        private StandInfoPopUp _standInfoPopUp;

        private void Start()
        {
            _productPlaces = GetComponentsInChildren<StandProductPlace>().ToList();
            _maxProducts = _productPlaces.Count;

            for (var i = 0; i < _maxProducts; i++)
            {
                _productPlaces[i].SetProduct(ProductsManager.Instance.GetRandomProductPrefab(_productTag));

                _availableProducts.Add(i);
            }
        }

        public bool ContainsProducts()
        {
            return _availableProducts.Count > 0;
        }

        public bool IsFull()
        {
            return _availableProducts.Count == _maxProducts;
        }

        public Product ReserveRandomProduct(Transform to)
        {
            var randomProduct = _availableProducts[Random.Range(0, _availableProducts.Count)];

            _availableProducts.Remove(randomProduct);

            var product = _productPlaces[randomProduct].Product;

            _productPlaces[randomProduct].IsEmpty = true;

            product.transform.DOJump(to.transform.position, 1f, 1, 0.5f)
                .OnComplete(() => product.gameObject.SetActive(false));

            return product;
        }

        public void ExtractProduct(Product product, Transform to)
        {

        }

        public void AddProduct(Transform from)
        {
            for (var i = 0; i < _maxProducts; i++)
            {
                var product = _productPlaces[i].Product;

                if (product.gameObject.activeSelf)
                {
                    continue;
                }

                product.transform.position = from.position;
                product.gameObject.SetActive(true);
                _availableProducts.Add(i);

                product.transform.DOJump(_productPlaces[i].transform.position, 1f, 1, 0.5f);

                return;
            }
        }

        public void AddProduct(Product product)
        {
            for (var i = 0; i < _maxProducts; i++)
            {
                if (!_productPlaces[i].IsEmpty)
                {
                    continue;
                }


            }
        }

        public bool Interact(PlayerHands playerHands)
        {
            if (playerHands.CurrentPickable is not StorageBox storageBox)
                return false;

            var player = playerHands.Player;

            if (IsFull())
            {
                // todo возможно это побочный эффект.
                // Требуется ли разделять - не подходит предмет для объекта или интеракция не удалась???
                return false;
            }

            if (storageBox.IsEmpty)
            {
                return false;
            }

            var product = storageBox.GetProduct();

            if (product.ProductTag != _productTag)
            {
                return false;
            }



            // AddProduct(player.transform);
            LevelManager.Instance.AddReceivedExperience(10);

            return true;
        }

        public void ShowInfo()
        {
            if (_standInfoPopUp)
            {
                _standInfoPopUp.SetTexts(_availableProducts.Count, _maxProducts);
                return;
            }

            _standInfoPopUp = Instantiate(_standInfoPopUpPrefab, transform);
            _standInfoPopUp.transform.position = transform.position + transform.up * 2f;
            _standInfoPopUp.SetTexts(_availableProducts.Count, _maxProducts);
        }

        public void HideInfo()
        {
            if (!_standInfoPopUp)
                return;

            Destroy(_standInfoPopUp.gameObject);
            _standInfoPopUp = null;
        }
    }
}