using System;
using UnityEngine;
using VitalRouter;
using Random = UnityEngine.Random;

/// Demonstrates usage of VitalRouter to simulate user registration events.
public class ExampleLoginSimpleEvents : MonoBehaviour
{
    // Define a registration command
    public struct EventRegister : ICommand
    {
        public int Code;     // Simulated status code: 200 for success, 409 for conflict
        public string User;  // Username attempting registration
        public int Frame;
    }

    // Counter to simulate multiple registration attempts
    private int attemptCount = 0;

    void Awake()
    {
        // Subscribe to the registration event and handle it
        Router.Default.Subscribe<EventRegister>((cmd, context) =>
        {
            attemptCount++;
            if (cmd.Code == 200)
            {
                Debug.Log($"[Attempt {attemptCount}] Registration successful for user: '{cmd.User}'. Sent on frame {cmd.Frame}, received on frame {Time.frameCount}.");
            }
            else if (cmd.Code == 409)
            {
                Debug.LogWarning($"[Attempt {attemptCount}] Registration failed for user '{cmd.User}': Username already exists (Code: {cmd.Code}). Sent on frame {cmd.Frame}, received on frame {Time.frameCount}.");
            }
            else
            {
                Debug.LogError($"[Attempt {attemptCount}] Registration failed for user '{cmd.User}': Unknown error (Code: {cmd.Code}). Sent on frame {cmd.Frame}, received on frame {Time.frameCount}.");
            }
        });
    }

    void Start()
    {
        // Simulate a few registration attempts with random success/failure
        for (int i = 0; i < 3; i++)
        {
            SimulateRegisterAttempt("admin");
        }
    }

    private void SimulateRegisterAttempt(string username)
    {
        // 70% chance of failure (e.g. conflict, username already exists)
        int statusCode = Random.value > 0.7f ? 200 : 409;
        
        // Publish the simulated registration event
        Router.Default.PublishAsync(new EventRegister
        {
            Code = statusCode,
            User = username,
            Frame = Time.frameCount
        });
    }
}
