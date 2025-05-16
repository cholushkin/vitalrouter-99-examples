using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

public class Gun : MonoBehaviour
{
    public CommandOrdering CommandOrderingMode;
    public float BulletSpeed;
    public float BulletLifetime;

    public void Awake()
    {
        Router.Default.Subscribe<GunTrigger.EventShootIntention>((cmd, context) =>
        {
            Debug.Log($"Sync handler gets the intention event right away. It doesn't use CommandOrderingMode (bullet:{cmd.BulletIndex})");
        });
        
        Router.Default.SubscribeAwait<GunTrigger.EventShootIntention>(async (cmd, context) =>
        {
            await ProcessBullet(cmd.BulletIndex, context.CancellationToken);
        }, CommandOrderingMode);
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

        // Await without throwing on cancel
        await UniTask
            .Delay((int)(BulletLifetime * 1000), cancellationToken: cancellationToken)
            .SuppressCancellationThrow();

        if (bullet != null)
        {
            Destroy(bullet);
            Debug.Log($"Bullet {bulletIndex} cleaned up (canceled or completed).");
        }
    }

}
