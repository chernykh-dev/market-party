using UnityEngine;

namespace MarketParty.DeadlineScripts
{
    public class WaveActivator : MonoBehaviour
    {
        [field: SerializeField]
        public int Wave { get; set; }

        [field: SerializeField]
        public bool Active { get; set; }
    }
}