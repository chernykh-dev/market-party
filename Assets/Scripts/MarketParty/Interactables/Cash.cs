using System.Collections.Generic;
using System.Linq;
using MarketParty.Characters;
using MarketParty.Managers;
using UnityEngine;

namespace MarketParty.Interactables
{
    public class Cash : MonoBehaviour, ILongInteractable
    {
        private CustomerQueuePoint _customersEnterPoint;

        private Queue<Customer> _customers = new Queue<Customer>();

        private void Awake()
        {
            _customersEnterPoint = GetComponentInChildren<CustomerQueuePoint>();
        }

        public CustomerQueuePoint QueuePositionForCustomer(Customer customer)
        {
            if (_customers.Count == 0)
            {
                AddCustomer(customer);
                return Instantiate(_customersEnterPoint, _customersEnterPoint.transform.position, Quaternion.identity);
            }

            var lastCustomerQueuePoint = Instantiate(_customersEnterPoint,
                _customersEnterPoint.transform.position - Vector3.right * _customers.Count * 0.5f, Quaternion.identity);

            AddCustomer(customer);

            return lastCustomerQueuePoint;
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Enqueue(customer);
        }

        public void LongInteract(Player player)
        {
            if (_customers.Count > 0)
            {
                var customer = _customers.Dequeue();

                customer.ServeCustomer();

                LevelManager.Instance.AddReceivedExperience(20);

                LevelManager.Instance.ServeCustomer(customer);

                var customersList = _customers.ToList();

                for (var i = 0; i < customersList.Count; i++)
                {
                    customersList[i].MoveInQueue(Instantiate(_customersEnterPoint,
                        _customersEnterPoint.transform.position - Vector3.right * i * 0.5f,
                        Quaternion.identity));
                }
            }
        }
    }
}