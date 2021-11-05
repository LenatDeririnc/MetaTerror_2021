using UnityEngine;

namespace Common.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 position);
        void Destroy(GameObject gameObject);
    }
}