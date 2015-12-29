using UnityEngine;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 21/10/2015.

public class AmbientTime : MonoBehaviour
{
    public float timeOfDay; // Time of day in hours (in 24h format).
    public float hourPerMinute;

    public bool freezeTime;

    public Transform sun;

    // Called when this script is loaded or a value has been changed in the inspector.
    private void OnValidate ()
    {
        // If the time of day surpasses 24h (midnight) go to 00h (midnight).
        if (timeOfDay >= 24f)
            timeOfDay = 00f;

        // Apply rotation based on the current <timeOfDay>.
        sun.eulerAngles = new Vector3((360f / 24f) * (timeOfDay - 6), 0f, 0f);
    }

    // Called every frame of the game.
    private void Update ()
    {
        if (!freezeTime)
        {
            // Adds an hour (1) every minute (60 seconds).
            timeOfDay += (hourPerMinute / 60) * Time.deltaTime;

            // If the time of day surpasses 24h (midnight) go to 00h (midnight).
            if (timeOfDay >= 24f)
                timeOfDay = 00f;

            // Apply rotation based on the current <timeOfDay>.
            sun.eulerAngles = new Vector3((360f / 24f) * (timeOfDay - 6), 0f, 0f);
        }
    }
}