using UnityEngine;

// Coded by Daniel van Dijk in 21/10/2015, last edited in 21/10/2015.

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance;

    public RaycastHit hit;

    public KeyCode interactKey;

    private void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Confined)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Confined;

            if (Cursor.visible)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
    }

    // Called at a fixed frame rate.
    private void FixedUpdate ()
    {
        Ray interactRay = new Ray(transform.position, transform.forward);

        // Shoot a raycast based on our <ray> and return the hit information to <hit>.
        if (Physics.Raycast(interactRay, out hit, interactDistance))
        {
            ///Debug.Log(string.Format("Press {0} to interact with {1}", interactKey, interactableObject.objectName));

            if (hit.transform.tag == "Interactable")
            {
                if (Input.GetKeyDown(interactKey))
                {
                    // If we clicked the <interactKey> then send a "Interact" message.
                    hit.collider.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                }
                else if (Input.GetKey(interactKey))
                {
                    // If we hold the <interactKey> then send a constant "InteractConstant" message.
                    hit.collider.SendMessage("InteractConstant", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    // REMOVE
    private void OnGUI()
    {
        if (hit.transform != null)
        {
            InteractiveObject interactiveObject = hit.transform.GetComponent<InteractiveObject>();

            if (interactiveObject != null)
            {
                Rect labelRect = new Rect(Screen.width / 2 - 200f, Screen.height - 50f, 400f, 50f);
                string labelText = string.Format("<size=20>Press 'E' to interact with the <color=blue>{0}</color></size>", hit.transform.name);

                GUI.Label(labelRect, labelText);
            }
        }
    }
}