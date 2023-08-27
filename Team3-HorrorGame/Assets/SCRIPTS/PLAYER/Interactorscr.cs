using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactorscr : MonoBehaviour
{
    public LayerMask interactableLayermask;
    // Interactable interactable;
    UnityEvent onInteract;
    public GameObject eInteractUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 7, interactableLayermask))
        {
             if (hit.collider.GetComponent<Interactable>() != false)
            {
                onInteract = hit.collider.GetComponent<Interactable>().onInteract;
                eInteractUI.SetActive(true);
                //if (interactable == null || interactable.ID != hit.collider.GetComponent<Interactable>().ID)
                {
                    //interactable = hit.collider.GetComponent<Interactable>();
                   // Debug.Log("New interactable");
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    onInteract.Invoke();
                    Debug.Log("New interactable");
                }
            }
            if (hit.collider.GetComponent<Interactable>() == false)
            {
                eInteractUI.SetActive(false);
            }
        }
    }
}
