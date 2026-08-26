using MarketParty.Products;
using UnityEngine;

namespace MarketParty
{
    public class Product : MonoBehaviour
    {
        [field: SerializeField]
        public ProductName ProductName { get; set; }

        [field: SerializeField]
        public ProductType ProductType { get; set; }

        [field: SerializeField]
        public ProductTag ProductTag { get; set; }

        public void EnablePhysics()
        {
            var meshCollider = gameObject.AddComponent<BoxCollider>();
            // meshCollider.convex = true;
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.5f;                    // лёгкий относительно «бесконечной» коробки
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.solverIterations = 16;          // против продавливания в стопке
            rb.maxAngularVelocity = 20f;
        }

        public void DisablePhysics()
        {
            Destroy(gameObject.AddComponent<Rigidbody>());
            Destroy(gameObject.AddComponent<BoxCollider>());
        }
    }
}