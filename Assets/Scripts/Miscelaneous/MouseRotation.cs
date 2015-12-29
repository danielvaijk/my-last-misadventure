using UnityEngine;
using System.Collections;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 21/10/2015.

public class MouseRotation : MonoBehaviour
{
    public RotationType rotationType = RotationType.XY;

    public float sensitivityX;
    public float sensitivityY;

    public float minimumY;
    public float maximumY;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public enum RotationType { XY, X, Y };

    // Called every frame of the game.
    private void Update ()
    {
        if (rotationType == RotationType.XY)
        {
            // Get <Mouse X> input data with sensitivity.
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            // Get <Mouse Y> input data with sensitivity and clamp it between the maximum and minimum values.
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            // Apply the rotation values to this GameObjects Euler Angles.
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        }
        else if (rotationType == RotationType.X)
        {
            // Get <Mouse X> input data with sensitivity.
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            // Apply the rotation values to this GameObjects Euler Angles.
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, 0f);
        }
        else
        {
            // Get <Mouse Y> input data with sensitivity and clamp it between the maximum and minimum values.
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            // Apply the rotation values to this GameObjects Euler Angles.
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0f);
        }
    }
}