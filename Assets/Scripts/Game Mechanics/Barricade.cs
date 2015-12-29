using UnityEngine;
using System.Collections;

// Coded by Daniel van Dijk in 20/10/2015, last edited in 11/12/2015.

public class Barricade : MonoBehaviour
{
    public float fixTime; // Time in seconds it takes to fix a part of the barricade.

    public Transform entryPosition; /// TEMPORARY

    public GameObject[] barricadeObjects;

    public float currentBarricadeHealth = 0f; // Do not change.

    private float maxBarricadeHealth = 100f;
    private float plankHealth = 0f; // Amount of time it takes to fix a part of the barricade.
    private float timer = 0f;

    // Called when this script is enabled, before the Update() method.
    private void Start ()
    {
        // Calculate the <fixThreshold> based on the number of barricade objects and its <maxBarricadeHealth>.
        plankHealth = maxBarricadeHealth / barricadeObjects.Length;

        // Calculate the <currentBarricadeHealth> based on the number of active barricade objects.
        foreach (GameObject barricadeObject in barricadeObjects)
        {
            if (barricadeObject.activeInHierarchy)
            {
                currentBarricadeHealth += plankHealth;
            }
        }
    }

    /// POSIBLE BUG WITH <timer> not being reset.
    // Called every fixed frame while the Player is interacting with this GameObject.
    private void InteractConstant ()
    {
        // If this barricades health is not full, then enable the Player to fix it.
        if (currentBarricadeHealth < maxBarricadeHealth)
        {
            // If the amount of time fixing reached <fixTime> then fix a part of the barricade.
            if (timer >= fixTime)
            {
                ActivateNextBarricadeObject();

                currentBarricadeHealth += plankHealth;
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    // Activates the first inactive barricade object iterated.
    private void ActivateNextBarricadeObject ()
    {
        foreach (GameObject barricadeObject in barricadeObjects)
        {
            if (!barricadeObject.activeInHierarchy)
            {
                barricadeObject.SetActive(true);
                return;
            }
        }
    }

    public void DamageBarricade (float damageAmount)
    {
        currentBarricadeHealth -= damageAmount;

        if (currentBarricadeHealth < 0)
        {
            currentBarricadeHealth = 0;
        }

        float plankNumber = Mathf.Abs(currentBarricadeHealth - maxBarricadeHealth) / plankHealth;

        for (int i = 0; i < Mathf.Floor(plankNumber); i++)
        {
            if (barricadeObjects[i].activeInHierarchy)
            {
                barricadeObjects[i].SetActive(false);
            }
        }
    }
}