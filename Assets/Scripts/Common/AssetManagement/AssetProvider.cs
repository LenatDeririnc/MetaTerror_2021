using UnityEngine;

namespace Common.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(GameObject gameObject) => 
            Object.Instantiate(gameObject);

        public GameObject Instantiate(GameObject gameObject, Transform parent) => 
            Object.Instantiate(gameObject, parent: parent);
        
        public GameObject Instantiate(GameObject gameObject, Vector3 position, Transform parent) => 
            Object.Instantiate(gameObject, position, Quaternion.identity, parent);

        public GameObject Instantiate(string path, Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }

        public void Destroy(GameObject gameObject) => 
            Object.Destroy(gameObject);
    }
}