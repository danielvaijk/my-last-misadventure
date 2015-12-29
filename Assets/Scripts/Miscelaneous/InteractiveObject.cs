using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public bool isOpen;

    private void Interact ()
    {
        Animation thisAnimation = GetComponent<Animation>();
        string animationName = thisAnimation.clip.name;

        thisAnimation[animationName].normalizedTime = isOpen ? 1f : -1f;
        thisAnimation[animationName].speed = isOpen ? -1f : 1f;

        thisAnimation.Play();

        isOpen = !isOpen;

        // Do some shit.
    }
}