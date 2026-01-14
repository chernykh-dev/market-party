using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MarketParty.Managers
{
    public class ProductsManager : GlobalSingleton<ProductsManager>, IInitializable
    {
        private List<Product> _productPrefabs;

        public void Init()
        {
            _productPrefabs = Resources.LoadAll<Product>("Products").ToList();
        }

        public Product GetRandomProductPrefab(ProductTag productTag)
        {
            return _productPrefabs
                .Where(x => x.ProductTag == productTag)
                .OrderBy(x => Random.Range(0, 100))
                .FirstOrDefault();
        }
    }
}