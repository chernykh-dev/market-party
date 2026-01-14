using System.Collections;
using MarketParty.Managers;
using MarketParty.UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MarketParty.Characters
{
    public class Customer : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private CharacterAnimator _characterAnimator;

        private Transform _targetTransform;

        private bool _isInteracting = false;

        private int _visitedStands = 0;
        private int _currentProducts = 0;
        private int _expectedProducts;

        public CustomerQueuePoint QueuePoint { get; private set; }

        public int TakedProducts => _currentProducts;

        public int ExpectedProducts => _expectedProducts;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _characterAnimator = GetComponent<CharacterAnimator>();

            QueuePoint = GetComponentInChildren<CustomerQueuePoint>();

            _agent.avoidancePriority = Random.Range(0, 100);
        }

        private void Start()
        {
            _expectedProducts = Random.Range(1, 4);

            LevelManager.Instance.AddCustomer(this);

            MoveTo(PlacesManager.Instance.EnterTransform);
        }

        private void Update()
        {
            _characterAnimator.SetVelocity(_agent.velocity.sqrMagnitude);

            if (_agent.remainingDistance > 0.1f || _isInteracting)
            {
                return;
            }

            if (_targetTransform.TryGetComponent(out IStand stand))
            {
                transform.LookAt(stand.transform);

                _isInteracting = true;

                if (stand.ContainsProducts())
                {
                    _characterAnimator.SetInteract();

                    StartCoroutine(WaitForInteract(stand));
                }
                else
                {
                    _characterAnimator.SetEmoteNo();

                    EmotePopUpsManager.Instance.ShowSadFace(transform);

                    StartCoroutine(WaitForAnimation());
                }

                return;
            }

            if (_targetTransform.TryGetComponent(out CustomerQueuePoint customerQueuePoint))
            {
                _isInteracting = true;

                return;
            }

            MoveToRandomStand();
        }

        private void MoveTo(Transform target)
        {
            _targetTransform = target;
            _agent.SetDestination(_targetTransform.position);
        }

        private void MoveToRandomStand()
        {
            IStand randomStand;

            do
            {
                randomStand = PlacesManager.Instance.GetRandomStand();
            }
            while (randomStand.transform == _targetTransform);

            MoveTo(randomStand.transform);
        }

        private IEnumerator WaitForInteract(IStand stand)
        {
            var product = stand.ReserveRandomProduct(transform);

            yield return WaitForAnimation();

            stand.ExtractProduct(product, transform);

            _currentProducts++;

            if (_currentProducts == _expectedProducts)
            {
                EmotePopUpsManager.Instance.ShowStars(transform);
            }
        }

        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            _isInteracting = false;

            _visitedStands++;

            if (_visitedStands == _expectedProducts)
            {
                MoveTo(PlacesManager.Instance.CashTransform.QueuePositionForCustomer(this).transform);
            }
            else
            {
                MoveToRandomStand();
            }
        }

        public void ServeCustomer()
        {
            EmotePopUpsManager.Instance.ShowCash(transform);

            var queuePointToDestroy = _targetTransform;

            MoveTo(PlacesManager.Instance.ExitTransform);

            Destroy(queuePointToDestroy.gameObject);
        }

        public void MoveInQueue(CustomerQueuePoint queuePoint)
        {
            MoveTo(queuePoint.transform);
        }
    }
}