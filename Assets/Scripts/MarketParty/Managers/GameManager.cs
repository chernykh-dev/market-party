using System;
using UnityEngine;

namespace MarketParty.Managers
{
    public class GameManager : GlobalSingleton<GameManager>, IInitializable
    {
        public void Init()
        {
            MusicManager.Instance.PlayDefaultMusic();
        }

        private void Awake()
        {
            var _ = Instance;
        }
    }
}