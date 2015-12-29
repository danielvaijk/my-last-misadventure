using UnityEngine;
using System.Collections;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 29/12/2015.

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed;
    public float runningSpeed;

    public AudioClip[] stepSounds;

    private float timer = 0f;

    private bool isGrounded = false;

    private AudioSource audioSource = null;

    private void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Called every frame of the game.
    private void Update ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get the movement direction vector based on the found input.
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Transform the movement direction vector from the global to local axis.
        Vector3 localMovementDirection = transform.TransformDirection(movementDirection);

        // Use the speed based on if we are pressing the run input or not.
        float currentSpeed = (Input.GetButton("Run") && verticalInput > 0) ? runningSpeed : walkingSpeed;

        // Change this GameObjects position based on the <localMovementDirection> and the <currentSpeed>.
        transform.position += localMovementDirection * currentSpeed * Time.deltaTime;

        if (movementDirection.magnitude > 0.2f)
        {
            if (timer >= 2 / currentSpeed)
            {
                AudioClip randomClip = stepSounds[Random.Range(0, stepSounds.Length)];

                audioSource.PlayOneShot(randomClip);

                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        Ray ray = new Ray(transform.position + Vector3.down * transform.localScale.y, Vector3.down);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            isGrounded = (Vector3.Distance(ray.origin, hit.point) <= 0.2f);
        }
        else
        {
            isGrounded = false;
        }

        if (!isGrounded)
        {
            transform.position += Vector3.down * 9.81f * Time.deltaTime;
        }
    }
}