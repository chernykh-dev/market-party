using System.Collections.Generic;
using UnityEngine;

namespace MarketParty.UI
{
    public class InputPopUpsManager : Singleton<InputPopUpsManager>, IInitializable
    {
        [SerializeField] private InputPopUp _inputPopUpPrefab;

        private Dictionary<string, InputPopUp> _inputPopUps = new();

        private InputPopUp _currentInputPopUp;

        public void Init()
        {

        }

        public void ShowPlaystationCrossButton(Transform target)
            => ShowInput(target, "playstation_button_color_cross");

        public void ShowPlaystationSquareButton(Transform target)
            => ShowInput(target, "playstation_button_color_square");

        private void ShowInput(Transform target, string inputName)
        {
            if (_inputPopUps.TryGetValue($"{target.gameObject.GetInstanceID()}{inputName}", out var existInputPopUp))
            {
                existInputPopUp.ContinueShow();
                return;
            }

            var inputPopUp = Instantiate(_inputPopUpPrefab, target);
            inputPopUp.transform.position = target.position + target.up * 1f;
            inputPopUp.Show(inputName,
                () => _inputPopUps.Remove($"{target.gameObject.GetInstanceID()}{inputName}"));

            _inputPopUps.Add($"{target.gameObject.GetInstanceID()}{inputName}", inputPopUp);
        }
    }
}