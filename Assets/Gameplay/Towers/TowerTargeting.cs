using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;

namespace AntiGravityTD.Gameplay.Towers
{
    /// <summary>
    /// Kulenin menzilindeki en yakın canlı düşmanı tespit etmesini sağlar.
    /// </summary>
    public class TowerTargeting : MonoBehaviour
    {
        [SerializeField] private float range = 5.0f;

        /// <summary>
        /// Kulenin menzil değerini döner.
        /// </summary>
        public float Range => range;

        /// <summary>
        /// Kulenin menzilindeki en yakın canlı düşmanı bulur.
        /// Herhangi bir düşman bulunamazsa null döner.
        /// </summary>
        public EnemyHealth CurrentTarget => GetClosestTarget();

        private EnemyHealth GetClosestTarget()
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
