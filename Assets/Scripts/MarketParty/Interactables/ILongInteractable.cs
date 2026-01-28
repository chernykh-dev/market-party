using System;
using MarketParty.Characters;
using MarketParty.Players;
using MarketParty.Players.Pickables;

namespace MarketParty.Interactables
{
    public interface ILongInteractable
    {
        bool LongInteract(PlayerHands playerHands);
    }
}