using UnityEngine;
using VitalRouter;

public class GunTrigger : MonoBehaviour
{
    public float IntentionCooldown;
    private int _currentBulletIndex = 0;
    private float _currentCooldown;

    public class EventShootIntention : ICommand
    {
        public int BulletIndex;
    }

    void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0)
        {
            Debug.Log($"My finger is pulling the trigger for bullet {_currentBulletIndex}");
            VitalRouter.Router.Default.PublishAsync(new EventShootIntention { BulletIndex = _currentBulletIndex++ });
            _currentCooldown = IntentionCooldown;
        }
    }
}