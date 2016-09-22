using UnityEngine;
using System.Collections;

public class Alice_gravity : MonoBehaviour {

    public Player.Sizes objectScale = Player.Sizes.Normal;
    public float density = 100;

    private Rigidbody rb;

    private Vector3 extraGravity;

	// Use this for initialization
	void Start () {

        

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();


        SetScale(objectScale);
        
    }


    public void SetScale(Player.Sizes newScale)
    {

        objectScale = newScale;

        float scale = Player.GetSizeScale(objectScale);

        rb.mass = density * Mathf.Pow(scale / Player.GetSizeScale(Player.Sizes.Normal), 3);
        
        extraGravity = Vector3.Scale(Physics.gravity, new Vector3(scale, scale, scale)) - Physics.gravity;
    }


    private void FixedUpdate()
    {
        rb.AddForce(extraGravity, ForceMode.Acceleration);
    }

}
