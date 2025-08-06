using UnityEngine;
using VitalRouter;

public class CoinSpawner : MonoBehaviour
{
    public enum CoinType
    {
        Golden,
        Silver
    }
    
    public struct EventCoinSpawned : ICommand
    {
        public Vector3 Position;
        public CoinType Coin;
    }

    public float CoinSpawnDelay;
    
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnCoinRnd), 0f, CoinSpawnDelay);
    }
    
    public void SpawnCoinRnd()
    {
        var rndCoinPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        var rndCoinType = Random.value > 0.5f ? CoinType.Golden : CoinType.Silver;
        Debug.Log($"{rndCoinType} coin spawned at {rndCoinPosition}");
        
        Router.Default.PublishAsync(new EventCoinSpawned
            {
                Position = rndCoinPosition, 
                Coin = rndCoinType
            });
    }
}