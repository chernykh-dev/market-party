using System;
using MarketParty.Characters;
using MarketParty.Characters.Players;

namespace MarketParty.Interactables
{
    public interface ILongInteractable
    {
        float TimeForInteract { get; }

        bool LongInteract(PlayerHands playerHands);
    }
}