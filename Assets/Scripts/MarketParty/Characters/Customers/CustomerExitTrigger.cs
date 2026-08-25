using UnityEngine;

namespace MarketParty
{
    public class CustomerExitTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Customer"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}