using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace BlockForge.Core
{
    /// <summary>
    /// Sahne yükleme yardımcısı. Basit async scene load.
    /// </summary>
    public static class SceneLoader
    {
        private static string _currentlyLoadingScene;
        
        /// <summary>
        /// Sahneyi yükler (additive değil, tam geçiş).
        /// </summary>
        public static void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Sahne adı boş olamaz!");
                return;
            }
            
            if (_currentlyLoadingScene == sceneName)
            {
                Debug.LogWarning($"[SceneLoader] '{sceneName}' zaten yükleniyor.");
                return;
            }
            
            _currentlyLoadingScene = sceneName;
            Debug.Log($"[SceneLoader] '{sceneName}' sahnesine geçiş yapılıyor...");
            SceneManager.LoadScene(sceneName);
            _currentlyLoadingScene = null;
        }
        
        /// <summary>
        /// Sahneyi async olarak yükler (loading bar için).
        /// MonoBehaviour üzerinden çağrılmalı.
        /// </summary>
        public static IEnumerator LoadSceneAsync(string sceneName, System.Action<float> onProgress = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Sahne adı boş olamaz!");
                yield break;
            }
            
            _currentlyLoadingScene = sceneName;
            Debug.Log($"[SceneLoader] '{sceneName}' sahnesine async geçiş başlatılıyor...");
            
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;
            
            while (!asyncOp.isDone)
            {
                // 0-0.9 arası yükleme, 0.9 = hazır
                float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
                onProgress?.Invoke(progress);
                
                if (asyncOp.progress >= 0.9f)
                {
                    asyncOp.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            _currentlyLoadingScene = null;
            Debug.Log($"[SceneLoader] '{sceneName}' sahnesi yüklendi.");
        }
        
        /// <summary>
        /// Mevcut aktif sahnenin adını döner.
        /// </summary>
        public static string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
