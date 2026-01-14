using R3;

namespace MarketParty.Managers
{
    public class MoneyManager : GlobalSingleton<MoneyManager>, IInitializable
    {
        public ReactiveProperty<int> Money { get; set; } = new ReactiveProperty<int>(0);

        public void Init()
        {

        }

        public void AddMoney(int money)
        {
            Money.Value += money;
        }
    }
}