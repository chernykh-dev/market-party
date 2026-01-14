using TMPro;
using UnityEngine;

namespace MarketParty.UI
{
    public class StandInfoPopUp : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _availableProductsText;

        [SerializeField]
        private TMP_Text _maxProductsText;

        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }

        public void SetTexts(int availableProducts, int maxProducts)
        {
            _availableProductsText.text = availableProducts.ToString();
            _maxProductsText.text = maxProducts.ToString();
        }
    }
}