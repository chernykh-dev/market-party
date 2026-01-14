using System.Collections.Generic;
using MarketParty.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarketParty.Managers
{
    public class PlayersManager : Singleton<PlayersManager>, IInitializable
    {
        private List<Player> _playerInputs = new List<Player>();

        public void Init()
        {

        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            var player = playerInput.GetComponent<Player>();

            _playerInputs.Add(player);
        }

        public void EnablePlayerInputs()
        {
            _playerInputs.ForEach(x => x.GetComponent<PlayerInput>().ActivateInput());
        }

        public void DisablePlayerInputs()
        {
            _playerInputs.ForEach(x => x.GetComponent<PlayerInput>().DeactivateInput());
        }
    }
}