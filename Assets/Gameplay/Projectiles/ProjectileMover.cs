using UnityEngine;
using AntiGravityTD.Gameplay.Enemies;

namespace AntiGravityTD.Gameplay.Projectiles
{
    /// <summary>
    /// Fırlatılan mermilerin hedefe doğru ilerlemesini ve ulaştığında hasar vermesini yönetir.
    /// Hedef kaybolduysa, öldüyse veya imha edildiyse mermiyi güvenle temizler.
    /// </summary>
    public class ProjectileMover : MonoBehaviour
    {
        [SerializeField] private float reachThreshold = 0.1f;

        private EnemyHealth target;
        private float speed;
        private float damage;
        private bool isInitialized = false;

        /// <summary>
        /// Mermiyi gerekli parametrelerle başlatır.
        /// </summary>
        public void Initialize(EnemyHealth target, float speed, float damage)
        {
            this.target = target;
            this.speed = speed;
            this.damage = damage;
            this.isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized) return;

            // Hedef kaybolduysa, imha edildiyse veya ölü olarak işaretlendiyse
            if (target == null || target.IsDead)
            {
                Destroy(gameObject);
                return;
            }

            // Hedefe doğru ilerle
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            // Hedefe ulaşıldı mı kontrol et
            if (Vector3.Distance(transform.position, target.transform.position) <= reachThreshold)
            {
                HitTarget();
            }
        }

        private void HitTarget()
        {
            if (target != null && target.IsAlive)
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

