using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MarketParty.Characters;
using MarketParty.Managers;
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
        private List<StandProductPlace> _products;
        private int _maxProducts;

        private StandInfoPopUp _standInfoPopUp;

        private void Start()
        {
            _products = GetComponentsInChildren<StandProductPlace>().ToList();
            _maxProducts = _products.Count;

            for (var i = 0; i < _maxProducts; i++)
            {
                _products[i].SetProduct(ProductsManager.Instance.GetRandomProductPrefab(_productTag));

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

            var product = _products[randomProduct].Product;

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
                var product = _products[i].Product;

                if (product.gameObject.activeSelf)
                {
                    continue;
                }

                product.transform.position = from.position;
                product.gameObject.SetActive(true);
                _availableProducts.Add(i);

                product.transform.DOJump(_products[i].transform.position, 1f, 1, 0.5f);

                return;
            }
        }

        public void Interact(Player player)
        {
            if (IsFull())
            {
                player.CharacterAnimator.SetEmoteNo();
                return;
            }

            player.CharacterAnimator.SetInteract();
            AddProduct(player.transform);
            LevelManager.Instance.AddReceivedExperience(10);
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