using UnityEngine;
using VitalRouter;

public class GoldDigger : MonoBehaviour
{
    private void Awake()
    {
        Router.Default.Subscribe<GoldDetector.EventGoldCoinDetected>(OnEventGoldCoinDetected);
    }

    private void OnEventGoldCoinDetected(GoldDetector.EventGoldCoinDetected eventGoldCoinDetected, PublishContext context)
    {
        Debug.Log("[GoldDigger] Collect gold coin");
    }
}