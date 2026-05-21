using UnityEngine;
using BlockForge.Core;

namespace BlockForge.AudioVfx
{
    public class VFXManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _lineClearVFXPrefab;
        [SerializeField] private GameObject _mergeVFXPrefab;

        public void Initialize()
        {
            if (_lineClearVFXPrefab == null) _lineClearVFXPrefab = Resources.Load<GameObject>("VFX/LineClearVFX");
            if (_mergeVFXPrefab == null) _mergeVFXPrefab = Resources.Load<GameObject>("VFX/MergeVFX");
            
            Debug.Log("[VFXManager] Initialized.");
        }

        public void PlayLineClearVFX(Vector3 position)
        {
            if (_lineClearVFXPrefab != null)
            {
                var vfx = Instantiate(_lineClearVFXPrefab, position, Quaternion.identity);
                Destroy(vfx, 2f);
            }
        }

        public void PlayMergeVFX(Vector3 position)
        {
             if (_mergeVFXPrefab != null)
            {
                var vfx = Instantiate(_mergeVFXPrefab, position, Quaternion.identity);
                Destroy(vfx, 2f);
            }
        }
    }
}