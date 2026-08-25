using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MarketParty
{
    public class CustomersSpawner : Singleton<CustomersSpawner>, IInitializable
    {
        [SerializeField]
        private GameObject _customerPrefab;

        [SerializeField]
        private float _spawnRadius;

        [SerializeField]
        private int _customersRangeMin = 2;
        [SerializeField]
        private int _customersRangeMax = 5;

        [SerializeField]
        private float _customerSpawnTimeoutRangeMin = 3f;
        [SerializeField]
        private float _customerSpawnTimeoutRangeMax = 6f;

        private int _currentCustomersCount = 0;
        private int _customersCount = 0;

        private Transform _spawnPoint;

        private float _spawnTime = 0f;
        private float _spawnTimeout;

        public void Init()
        {
            _customersCount = Random.Range(_customersRangeMin, _customersRangeMax + 1);

            _spawnPoint = transform.GetChild(0).transform;

            ResetSpawnTimer();
        }

        private void Start()
        {
            if (_customersCount > 0)
            {
                return;
            }

            // TODO
            var _ = Instance;
        }

        private void Update()
        {
            _spawnTime += Time.deltaTime;

            if (_spawnTime > _spawnTimeout)
            {
                SpawnCustomer();

                ResetSpawnTimer();
            }
        }

        private void SpawnCustomer()
        {
            var randomCircle = Random.insideUnitCircle * _spawnRadius;

            var spawnPoint = _spawnPoint.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            Instantiate(_customerPrefab, spawnPoint, Quaternion.identity);

            _currentCustomersCount++;

            if (_currentCustomersCount == _customersCount)
            {
                Destroy(this);
            }
        }

        private void ResetSpawnTimer()
        {
            _spawnTime = 0f;

            _spawnTimeout = Random.Range(_customerSpawnTimeoutRangeMin, _customerSpawnTimeoutRangeMax);
        }

        public void UpDifficult(int currentWave)
        {
            _customersCount += Random.Range(_customersRangeMin + 1 * currentWave,  _customersRangeMax + 2 * currentWave);

            if (_customerSpawnTimeoutRangeMin > 0.1f * currentWave)
            {
                _customerSpawnTimeoutRangeMin -= 0.1f * currentWave;
            }

            if (_customerSpawnTimeoutRangeMax > 0.2f * currentWave)
            {
                _customerSpawnTimeoutRangeMax -= 0.2f * currentWave;
            }
        }
    }
}