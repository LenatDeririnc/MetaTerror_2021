using System;
using System.Collections;
using Common.Components;
using Common.Components.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.SceneManagement
{
    public class SceneProvider
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _curtain;
        public static Action OnLoadSceneAction; 

        public SceneProvider(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            _coroutineRunner = coroutineRunner;
            _curtain = curtain;
        }

        public void Load(ScriptableObjects.Scene scene, Action onLoaded = null)
        {
            _curtain.Show(() => _coroutineRunner.StartCoroutine(LoadScene(scene, onLoaded)));
        }

        private IEnumerator LoadScene(ScriptableObjects.Scene scene, Action onLoaded = null)
        {
            AsyncOperation waitSceneAsync = SceneManager.LoadSceneAsync(scene.sceneName);

            while (!waitSceneAsync.isDone)
                yield return null;
            
            onLoaded?.Invoke();
            OnLoadSceneAction?.Invoke();
            OnLoadedScene();
        }

        private void OnLoadedScene()
        {
            _curtain.Hide();
        }
    }
}