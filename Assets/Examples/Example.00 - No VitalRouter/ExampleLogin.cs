using UnityEngine;
using Cysharp.Threading.Tasks;

// Non-VitalRouter approach 
// - Harder to reuse or test the pipeline.
// - Each step is inline and mixes control flow with logic.
public class ExampleLogin : MonoBehaviour
{
    void Start()
    {
        TryLoginAsync("user123").Forget();
        TryLoginAsync("banned").Forget();
    }

    async UniTaskVoid TryLoginAsync(string username)
    {
        if (username == "banned")
        {
            Debug.Log("Login blocked: user is banned.");
            return;
        }

        Debug.Log($"Authenticating {username}...");
        await UniTask.Delay(1000); // Simulated async auth
        Debug.Log($"Login successful: {username}");
    }
}