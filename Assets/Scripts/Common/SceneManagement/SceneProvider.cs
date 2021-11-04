using System;
using System.Collections;
using Common.Components.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.SceneManagement
{
    public class SceneProvider
    {
        private readonly ICoroutineRunner _coroutineRunner;
        public static Action OnLoadScene; 

        public SceneProvider(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        public void Load(ScriptableObjects.Scene scene, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadScene(scene, onLoaded));

        private IEnumerator LoadScene(ScriptableObjects.Scene scene, Action onLoaded = null)
        {
            AsyncOperation waitSceneAsync = SceneManager.LoadSceneAsync(scene.sceneName);

            while (!waitSceneAsync.isDone)
                yield return null;
            
            onLoaded?.Invoke();
            OnLoadScene?.Invoke();
        }
    }
}