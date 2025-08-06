using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

namespace SimpleFilter
{
    public class FireInterceptor : ICommandInterceptor
    {
        private Gun _gun;

        public FireInterceptor(Gun gun)
        {
            _gun = gun;
        }
        
        public ValueTask InvokeAsync<T>(T command, PublishContext context, PublishContinuation<T> next) where T : ICommand
        {
            if (_gun.Bullets <= 0)
            {
                Debug.Log("[FireFilter] No bullets left! Command cancelled.");
                return default;  // Don't call next, effectively cancelling command
            }
        
            return next(command, context);
        }
    }
    

    [Routes]
    public partial class Gun : MonoBehaviour
    {
        public float BulletSpeed;
        public float BulletLifetime;
        public int Bullets;

        private void Awake()
        {
            // Register routes to the default router at runtime
            MapTo(Router.Default, new FireInterceptor(this));
        }

        // Note: check who call this method (called from generated class)
        [Route] 
        [Filter(typeof(FireInterceptor))]
        private async UniTask On(GunTrigger.EventShootIntention cmd, CancellationToken cancellationToken = default)
        {
            await ProcessBullet(cmd.BulletIndex, cancellationToken);
        }

        private async UniTask ProcessBullet(int bulletIndex, CancellationToken cancellationToken)
        {
            --Bullets;
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