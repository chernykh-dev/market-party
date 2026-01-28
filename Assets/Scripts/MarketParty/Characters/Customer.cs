using System.Collections;
using MarketParty.Interactables;
using MarketParty.Managers;
using MarketParty.UI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MarketParty.Characters
{
    public class Customer : MonoBehaviour
    {
        [SerializeField]
        private CustomerWaitingUI _customerWaitingUIPrefab;

        private NavMeshAgent _agent;
        private CharacterAnimator _characterAnimator;

        private Transform _targetTransform;

        private CustomerWaitingUI _customerWaitingUI;

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

                _customerWaitingUI = Instantiate(_customerWaitingUIPrefab, transform);

                _customerWaitingUI.transform.position = transform.position + transform.up * 1f;

                _customerWaitingUI.Setup(this);

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
                var queuePosition = PlacesManager.Instance.CashTransform.QueuePositionForCustomer(this);

                MoveTo(queuePosition.transform);
            }
            else
            {
                MoveToRandomStand();
            }
        }

        public void ServeCustomer()
        {
            EmotePopUpsManager.Instance.ShowCash(transform);

            Destroy(_customerWaitingUI.gameObject);

            var queuePointToDestroy = _targetTransform;

            MoveTo(PlacesManager.Instance.ExitTransform);

            Destroy(queuePointToDestroy.gameObject);
        }

        public void MoveInQueue(CustomerQueuePoint queuePoint)
        {
            MoveTo(queuePoint.transform);
        }

        public void FailCustomer()
        {
            EmotePopUpsManager.Instance.ShowSadFace(transform);

            FindFirstObjectByType<Cash>().RemoveCustomer(this);

            var queuePointToDestroy = _targetTransform;

            MoveTo(PlacesManager.Instance.ExitTransform);

            Destroy(queuePointToDestroy.gameObject);
        }
    }
}