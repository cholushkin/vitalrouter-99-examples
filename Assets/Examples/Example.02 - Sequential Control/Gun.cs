using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VitalRouter;

public class Gun : MonoBehaviour
{
    [InfoBox("Parallel. If commands are published simultaneously, subscribers are called in parallel.")]
    [InfoBox("Sequential. If commands are published simultaneously, wait until the subscriber has processed the first command.")]
    [InfoBox("Drop. If commands are published simultaneously, ignore commands that come later.")]
    [InfoBox("Switch. If the previous asynchronous method is running, it is cancelled and the next asynchronous method is executed.")]
    public CommandOrdering CommandOrderingMode;
    public float BulletSpeed;
    public float BulletLifetime;

    public void Awake()
    {
        // NOTE: Optionally you can store Subscription as IDisposable and unsubscribe dynamically later by _subscription.Dispose();
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
