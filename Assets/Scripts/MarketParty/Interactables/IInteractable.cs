using System;
using MarketParty.Characters;
using MarketParty.Characters.Players;

namespace MarketParty.Interactables
{
    public interface IInteractable
    {
        bool Interact(PlayerHands playerHands);
    }
}