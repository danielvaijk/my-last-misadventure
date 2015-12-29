using UnityEngine;
using System.Collections;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 11/12/2015.

public class GameManager : MonoBehaviour
{
    public float startingTime; // Time is in hours.
    public float defenseTime; // Time is in hours.

    public GameObject player;
    public GameObject rake;

    public Transform spawnPosition;

    [HideInInspector]
    public int daysSurvived = 0;

    public AmbientTime ambientTime = null;

    private bool hasStarted = false;

    private void Start ()
    {
        ambientTime = GetComponent<AmbientTime>();
    }

    // Called every frame of the game.
    private void Update ()
    {
        if (ambientTime.timeOfDay >= (startingTime + defenseTime))
        {
            RakeBrain rakeBrain = rake.GetComponent<RakeBrain>();

            if (!rakeBrain.hasStartedAttack)
            {
                rakeBrain.StartAttack();
                rakeBrain.hasStartedAttack = true;
            }
        }

        // Check when it is (6 o'clock or so) to tell the rake to return.
    }

    /// Called a few times per frame of the game.
    private void OnGUI ()
    {
        if (!hasStarted)
        {
            if (GUILayout.Button("Play"))
            {
                StartGame();
                hasStarted = true;
            }
        }
    }

    // Starts the game.
    private void StartGame ()
    {
        GameObject playerClone = Instantiate(player, spawnPosition.position, Quaternion.identity) as GameObject;

        playerClone.name = "Player";

        // Enable in-game time.
        ambientTime.enabled = true;
        ambientTime.timeOfDay = startingTime;
    }
}