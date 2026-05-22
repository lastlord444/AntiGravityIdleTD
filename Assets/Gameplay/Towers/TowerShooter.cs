using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;
using AntiGravityTD.Gameplay.Projectiles;

namespace AntiGravityTD.Gameplay.Towers
{
    /// <summary>
    /// TowerTargeting tarafından belirlenen hedefe belirli bir atış hızında mermi fırlatır.
    /// </summary>
    [RequireComponent(typeof(TowerTargeting))]
    public class TowerShooter : MonoBehaviour
    {
        [SerializeField] private TowerTargeting targeting;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float fireRate = 1.0f; // Saniyede atış sayısı
        [SerializeField] private float projectileSpeed = 5.0f;
        [SerializeField] private float damage = 2.0f;

        private float fireCooldown = 0.0f;

        private void Start()
        {
            if (targeting == null)
            {
                targeting = GetComponent<TowerTargeting>();
            }
        }

        private void Update()
        {
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0.0f)
            {
                EnemyHealth target = targeting.CurrentTarget;
                if (target != null)
                {
                    Shoot(target);
                    fireCooldown = 1.0f / fireRate;
                }
            }
        }

        private void Shoot(EnemyHealth target)
        {
            Debug.Log($"[TowerShooter] Fired projectile at: {target.gameObject.name}");

            GameObject projectileInstance;
            if (projectilePrefab != null)
            {
                projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // Prefab yoksa kod üzerinden geçici bir mermi oluştur (PR B veya oyun içinde esneklik sağlar)
                projectileInstance = new GameObject("Projectile");
                projectileInstance.transform.position = transform.position;
            }

            ProjectileMover mover = projectileInstance.GetComponent<ProjectileMover>();
            if (mover == null)
            {
                mover = projectileInstance.AddComponent<ProjectileMover>();
            }

            mover.Initialize(target, projectileSpeed, damage);
        }
    }
}
