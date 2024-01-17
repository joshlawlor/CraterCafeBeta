using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTapController : MonoBehaviour
{
    public GameObject InteractPopUp;


    private void SetPopUpActive(GameObject popUp, bool isActive)
    {
        if (popUp != null)
        {
            popUp.SetActive(isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetPopUpActive(InteractPopUp, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetPopUpActive(InteractPopUp, false);

        }
    }

}
