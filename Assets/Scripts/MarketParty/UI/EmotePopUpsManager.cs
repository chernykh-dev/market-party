using UnityEngine;

namespace MarketParty.UI
{
    public class EmotePopUpsManager : Singleton<EmotePopUpsManager>, IInitializable
    {
        [SerializeField] private EmotePopUp _emotePopUpPrefab;

        public void Init()
        {

        }

        public void ShowCash(Transform target)
            => ShowEmote(target, "cash");

        public void ShowSadFace(Transform target)
            => ShowEmote(target, "faceSad");

        public void ShowStars(Transform target)
            => ShowEmote(target, "stars");

        private void ShowEmote(Transform target, string emoteName)
        {
            var emotePopUp = Instantiate(_emotePopUpPrefab);
            emotePopUp.transform.position = target.position + target.up * 1f;
            emotePopUp.Show(emoteName);
        }
    }
}