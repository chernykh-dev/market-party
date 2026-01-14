using UnityEngine;

namespace MarketParty.Interactables
{
    public class Selectable : MonoBehaviour, ISelectable
    {
        private Renderer _renderer;
        private Color _defaultColor;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _defaultColor = _renderer.material.color;
        }

        public void Select()
        {
            var selectedColor = _defaultColor;
            selectedColor.r = selectedColor.g = selectedColor.b = 0.5f;

            _renderer.material.color = selectedColor;
        }

        public void Deselect()
        {
            _renderer.material.color = _defaultColor;
        }
    }
}