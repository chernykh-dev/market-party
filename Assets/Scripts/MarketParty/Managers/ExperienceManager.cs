using R3;
using UnityEngine;

namespace MarketParty.Managers
{
    public class ExperienceManager : GlobalSingleton<ExperienceManager>, IInitializable
    {
        public ReactiveProperty<int> Experience { get; } = new ReactiveProperty<int>(0);

        public void Init()
        {

        }

        public void AddExperience(int amount)
        {
            Experience.Value += amount;
        }

        public void ResetExperience()
        {
            Experience.Value = 0;
        }
    }
}