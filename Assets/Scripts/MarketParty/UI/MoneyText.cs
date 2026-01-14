using System;
using MarketParty.Managers;
using R3;
using TMPro;
using UnityEngine;

namespace MarketParty.UI
{
    public class MoneyText : MonoBehaviour
    {
        private TMP_Text _moneyText;

        private void Start()
        {
            _moneyText = GetComponent<TMP_Text>();

            MoneyManager.Instance.Money.SubscribeToText(_moneyText);
        }
    }
}