using UnityEngine;
using VitalRouter;

public class GoldDetector : MonoBehaviour
{
    public struct EventGoldCoinDetected : ICommand
    {
    }

    private void Awake()
    {
        Router.Default.Subscribe<CoinSpawner.EventCoinSpawned>(OnEventCoinSpawned);
    }

    private void OnEventCoinSpawned(CoinSpawner.EventCoinSpawned eventCoinSpawned, PublishContext context)
    {
        Debug.Log($"[GoldDetector] Analyzing coin {eventCoinSpawned.Coin}");
        if (eventCoinSpawned.Coin == CoinSpawner.CoinType.Golden)
            Router.Default.PublishAsync(new EventGoldCoinDetected());
    }
}