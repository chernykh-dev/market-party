using MarketParty.Managers;
using R3;
using TMPro;
using UnityEngine;

namespace MarketParty.UI
{
    public class ExperienceText : MonoBehaviour
    {
        private TMP_Text _experienceText;

        private void Start()
        {
            _experienceText = GetComponent<TMP_Text>();

            ExperienceManager.Instance.Experience.SubscribeToText(_experienceText);
        }
    }
}