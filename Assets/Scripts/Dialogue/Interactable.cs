using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Transform interactableObjectActionContainer;
    [HideInInspector]
    public Button interactableObjectActionPrefab;

    //[HideInInspector]
    //public Collider col;
    [HideInInspector]
    public Button instantiatedPrefab;

    [HideInInspector]
    public Transform player;
    /// <summary>
    /// this is awake on Interactable
    /// </summary>
    public virtual void Awake()
    {
        interactableObjectActionContainer = GameObject.Find("InteractableMenuContainer").transform;
        interactableObjectActionPrefab = Resources.Load<Button>("Prefabs/InteractableMenuPrefab");
    }
    /// <summary>
    /// This is the base function for Interactable object
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("Intereact with base class");
    }

    /// <summary>
    /// This function provide by UnityEngine
    /// And used to check if The Player approaching The Interactable Obeject e.g Drop Item or NPC
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") {
            player = col.transform;
            GenerateInteractableObjectAction(col);
        }
    }

    /// <summary>
    /// This function provide by UnityEngine
    /// And used to check if The Player leave The Interactable Obeject to destroy instantiated objects
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
            if (instantiatedPrefab != null) {
                Destroy(instantiatedPrefab.gameObject);
                player = null;
            }
    }

    /// <summary>
    /// This function used for generate button on UI to interact with the interactable object
    /// </summary>
    /// <param name="col"></param>
    private void GenerateInteractableObjectAction(Collider col)
    {
        instantiatedPrefab = Instantiate(interactableObjectActionPrefab, interactableObjectActionContainer);
        var txt = instantiatedPrefab.GetComponentInChildren<Text>();
        txt.text = $"{name}";
        instantiatedPrefab.onClick.AddListener(delegate { Interact(); });
    }
}
