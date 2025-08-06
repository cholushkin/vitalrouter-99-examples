using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

namespace SimpleRoutes
{

    [Routes]
    public partial class Gun : MonoBehaviour
    {
        public float BulletSpeed;
        public float BulletLifetime;

        private void Awake()
        {
            // Register routes to the default router at runtime
            MapTo(Router.Default);
        }


        // Note: check who call this method (called from generated class)
        [Route] 
        private async UniTask On(GunTrigger.EventShootIntention cmd, CancellationToken cancellationToken = default)
        {
            await ProcessBullet(cmd.BulletIndex, cancellationToken);
        }

        private async UniTask ProcessBullet(int bulletIndex, CancellationToken cancellationToken)
        {
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bullet.name = $"Bullet_{bulletIndex}";
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;

            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.linearVelocity = transform.forward * BulletSpeed;

            await UniTask
                .Delay((int)(BulletLifetime * 1000), cancellationToken: cancellationToken)
                .SuppressCancellationThrow();

            if (bullet != null)
            {
                Destroy(bullet);
                Debug.Log($"[Gun] Bullet {bulletIndex} cleaned up.");
            }
        }
    }
}