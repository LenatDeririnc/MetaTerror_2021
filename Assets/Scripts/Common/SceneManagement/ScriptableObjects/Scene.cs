using UnityEngine;

namespace Common.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Scene Info", menuName = "Scene Info")]
    public class Scene : ScriptableObject
    {
        public string sceneName;
    }
}