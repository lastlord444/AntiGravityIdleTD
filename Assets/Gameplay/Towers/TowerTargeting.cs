using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;

namespace AntiGravityTD.Gameplay.Towers
{
    /// <summary>
    /// Kulenin menzilindeki en yakın canlı düşmanı tespit etmesini sağlar.
    /// Hedefi önbelleğe alır ve sadece hedef kaybedildiğinde veya belli aralıklarla yeniden tarar.
    /// </summary>
    public class TowerTargeting : MonoBehaviour
    {
        [SerializeField] private float range = 5.0f;
        [SerializeField] private float rescanInterval = 0.25f;

        private EnemyHealth cachedTarget;
        private float rescanTimer;

        /// <summary>
        /// Kulenin menzil değerini döner.
        /// </summary>
        public float Range => range;

        /// <summary>
        /// Kulenin menzilindeki en yakın canlı düşmanı döner.
        /// Önbellekten döner; geçersizse yeniden tarar.
        /// </summary>
        public EnemyHealth CurrentTarget
        {
            get
            {
                if (!IsTargetValid(cachedTarget))
                {
                    cachedTarget = ScanForClosestTarget();
                }
                return cachedTarget;
            }
        }

        private void Update()
        {
            rescanTimer -= Time.deltaTime;

            // Mevcut hedef geçersizse hemen yeniden tara
            if (!IsTargetValid(cachedTarget))
            {
                cachedTarget = ScanForClosestTarget();
                rescanTimer = rescanInterval;
                return;
            }

            // Periyodik yeniden tarama — daha yakın hedef olabilir
            if (rescanTimer <= 0f)
            {
                cachedTarget = ScanForClosestTarget();
                rescanTimer = rescanInterval;
            }
        }

        /// <summary>
        /// Hedefin hâlâ geçerli, canlı ve menzil içinde olup olmadığını kontrol eder.
        /// </summary>
        private bool IsTargetValid(EnemyHealth target)
        {
            if (target == null || target.IsDead) return false;
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance <= range;
        }

        private EnemyHealth ScanForClosestTarget()
        {
            EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
            EnemyHealth closestEnemy = null;
            float shortestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (EnemyHealth enemy in enemies)
            {
                if (enemy == null || !enemy.IsAlive) continue;

                float distanceToEnemy = Vector3.Distance(currentPosition, enemy.transform.position);
                if (distanceToEnemy <= range && distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        private void OnDrawGizmosSelected()
        {
            // Editör ekranında kulenin menzilini temsil eden bir tel daire çizer
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}

