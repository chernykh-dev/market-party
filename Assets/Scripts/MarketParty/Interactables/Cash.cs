using System.Collections.Generic;
using System.Linq;
using MarketParty.Characters;
using MarketParty.Managers;
using MarketParty.Players;
using MarketParty.Players.Pickables;
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

        public bool LongInteract(PlayerHands playerHands)
        {
            if (playerHands.CurrentPickable is not EmptyHands emptyHands)
                return false;

            // todo copy of LongInteract(Player).
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

            return true;
        }

        public void RemoveCustomer(Customer customer)
        {
            var newCustomers = _customers.ToList();
            newCustomers.Remove(customer);
            _customers = new Queue<Customer>(newCustomers);

            LevelManager.Instance.FailCustomer(customer);

            for (var i = 0; i < newCustomers.Count; i++)
            {
                newCustomers[i].MoveInQueue(Instantiate(_customersEnterPoint,
                    _customersEnterPoint.transform.position - Vector3.right * i * 0.5f,
                    Quaternion.identity));
            }
        }
    }
}