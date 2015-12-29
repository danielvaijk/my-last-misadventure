using UnityEngine;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Pathfinding;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 11/12/2015.

public class RakeBrain : MonoBehaviour
{
    public float maxDamage;
    public float minDamage;

    public float outsideSpeed;
    public float insideSpeed;

    public Transform player;
    public Transform startingPoint;

    [HideInInspector]
    public bool hasStartedAttack = false;

    public GameManager gameManager;

    private int attackAmount = 0;
    private int currentWaypoint = 0;

    private float distanceToPlayer = 0f;

    private bool canAttack = false;
    private bool canEnterHouse = false;
    private bool isinsideHouse = false;

    private Transform currentTarget = null;
    private Transform entryPoint = null;

    private List<Transform> nonPlayerTargets = null;

    private CharacterController controller = null;

    private Path currentPath = null;
    private Seeker seeker = null;


    // Start attack process against the Player.
    public void StartAttack()
    {
        nonPlayerTargets = GetBarricades();
        attackAmount = Random.Range(1, 6);

        canAttack = true;
    }

    public void ReturnToStart ()
    {
        gameManager.daysSurvived++;
        nonPlayerTargets = new List<Transform>();

        currentTarget = startingPoint;

        Path path = seeker.StartPath(transform.position, currentTarget.position, OnPathCompleted);

        if (!path.error)
        {
            currentPath = path;
            currentWaypoint = 0;

            return;
        }
    }

    private void Start ()
    {
        controller = this.GetComponent<CharacterController>();
        seeker = this.GetComponent<Seeker>();
    }

    // Called every frame of the game.
    private void Update()
    {
        if (isinsideHouse)
        {
            Debug.Log("Rake: I'm inside the house.");

            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (currentPath == null)
            {
                currentTarget = nonPlayerTargets[Random.Range(0, nonPlayerTargets.Count)];

                seeker.StartPath(transform.position, currentTarget.position, OnPathCompleted);
                return;
            }
            else
            {
                if (currentWaypoint >= currentPath.vectorPath.Count)
                {
                    currentPath = null;
                    return;
                }

                PathMove(insideSpeed);
            }
        }

        if (canAttack)
        {
            if (canEnterHouse)
            {
                Debug.Log("Rake: Entering the house.");

                if (currentPath == null)
                {
                    currentTarget = nonPlayerTargets[0];
                    entryPoint = currentTarget;

                    Path path = seeker.StartPath(transform.position, currentTarget.position);

                    if (!path.error)
                    {
                        currentPath = path;
                        currentWaypoint = 0;
                        return;
                    }
                }
                else
                {
                    if (currentWaypoint >= currentPath.vectorPath.Count)
                    {
                        if (gameManager.ambientTime.freezeTime)
                        {
                            gameManager.ambientTime.freezeTime = false;
                        }

                        transform.position = entryPoint.GetComponent<Barricade>().entryPosition.position;
                        nonPlayerTargets = new List<Transform>();

                        foreach (Transform waypoint in gameManager.transform.Find("Waypoints"))
                        {
                            nonPlayerTargets.Add(waypoint);
                        }

                        canEnterHouse = false;
                        isinsideHouse = true;

                        return;
                    }

                    PathMove(outsideSpeed);
                }
            }
            else
            {
                if (!gameManager.ambientTime.freezeTime)
                {
                    gameManager.ambientTime.freezeTime = true;
                }

                if (currentPath == null)
                {
                    if (attackAmount > 0)
                    {
                        int randomTarget = Random.Range(0, nonPlayerTargets.Count);

                        // Pathfinding
                        currentTarget = nonPlayerTargets[randomTarget];

                        seeker.StartPath(transform.position, currentTarget.position, OnPathCompleted);

                        attackAmount--;
                        return;
                    }
                    else
                    {
                        Debug.Log("Rake: I Couldn't break into the house... Returning to start position.");

                        ReturnToStart();
                    }
                }
                else
                {
                    // If we have reached our last waypoint, then we have reached the end of this path.
                    if (currentWaypoint >= currentPath.vectorPath.Count)
                    {
                        if (currentTarget.tag == "Barricade")
                        {
                            Barricade targetBarricade = currentTarget.GetComponent<Barricade>();

                            targetBarricade.DamageBarricade(Random.Range(minDamage, maxDamage + 1));

                            if (targetBarricade.currentBarricadeHealth <= 0f)
                            {
                                Debug.Log("Rake: I broke a barricade... Entering the house.");

                                // Play entering animation.
                                /// TEMPORARY
                                transform.position = targetBarricade.entryPosition.position;

                                canEnterHouse = true;
                                return;
                            }
                        }
                        else
                        {
                            canAttack = false;
                            hasStartedAttack = false;
                        }

                        currentTarget = null;
                        currentPath = null;
                        return;
                    }

                    PathMove(outsideSpeed);
                }
            }
        }
    }

    private void OnPathCompleted (Path path)
    {
        if (!path.error)
        {
            currentPath = path;
            currentWaypoint = 0;
            return;
        }
    }

    private void PathMove (float speed)
    {
        Vector3 direction = (currentPath.vectorPath[currentWaypoint] - transform.position).normalized;
        direction *= speed * Time.deltaTime;

        controller.SimpleMove(direction);

        if (Vector3.Distance(transform.position, currentPath.vectorPath[currentWaypoint]) <= 1f)
        {
            currentWaypoint++;
        }
    }

    // Return an open barricade, if any.
    private List<Transform> GetBarricades()
    {
        List<Transform> targetBarricades = new List<Transform>();
        List<GameObject> barricades = GameObject.FindGameObjectsWithTag("Barricade").ToList();

        // Check if there are any open barricades.
        foreach (GameObject barricade in barricades)
        {
            Barricade barricadeComponent = barricade.GetComponent<Barricade>();

            if (barricadeComponent.currentBarricadeHealth <= 0)
            {
                Debug.Log("Rake: I found an entry left open, going to enter the house.");

                targetBarricades.Add(barricade.transform);
                canEnterHouse = true;

                return targetBarricades;
            }
        }

        int amountOfBarricades = Random.Range(1, barricades.Count + 1);

        // Select a random <amountOfBarricades> to attack.
        for (int i = 0; i < amountOfBarricades; i++)
        {
            int randomIndex = Random.Range(0, barricades.Count);

            targetBarricades.Add(barricades[randomIndex].transform);

            barricades.Remove(barricades[randomIndex]);
        }

        return targetBarricades;
    }
}