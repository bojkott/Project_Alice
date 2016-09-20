using UnityEngine;
using System.Collections;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class Alice_Interact : MonoBehaviour {

    VRTK_InteractableObject interObj;

    public enum PlayerSize { Giant, Normal, Midget, All}

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

        if (RequiredPlayerSize == PlayerSize.All)
            return true;

        if (RequiredPlayerSize == PlayerSize.Giant && Player.currentSize == Player.Sizes.Giant)
            return true;
        

        if (RequiredPlayerSize == PlayerSize.Normal && Player.currentSize == Player.Sizes.Normal)
            return true;


        if (RequiredPlayerSize == PlayerSize.Midget && Player.currentSize == Player.Sizes.Midget)
            return true;
        

        return false;
    }
}
