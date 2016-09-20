using UnityEngine;
using System.Collections;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Alice_Interact : MonoBehaviour {

    VRTK_InteractableObject interObj;

    public enum PlayerSize { Grown, Shrunken, Both }

    public PlayerSize RequiredPlayerSize;

    // Use this for initialization
    void Start () {
        interObj = GetComponent<VRTK_InteractableObject>();
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(PlayerIsInCorrectState())
        {
            interObj.enabled = true;
        }
        else
        {
            interObj.enabled = false;
        }
	}


    public bool PlayerIsInCorrectState()
    {
        if (RequiredPlayerSize == PlayerSize.Both)
            return true;
        else if (RequiredPlayerSize == PlayerSize.Grown && Player.currentState == Player.State.Grown)
            return true;
        else if (RequiredPlayerSize == PlayerSize.Shrunken && Player.currentState == Player.State.Shrunken)
            return true;
        else
            return false;
    }
}
