using UnityEngine;
using System.Collections;
using VRTK;
public class ShrinkAndGrow : MonoBehaviour {


    RadialMenuButton resizeButton;
    RadialMenu resizeMenu;
    private VRTK_PlayerPresence playerPresence;
    private Rigidbody rb;

    public float GiantSize = 100.0f;
    public float NormalSize = 10.0f;
    public float MidgetSize = 1.0f;

    public float ResizeTime = 0.1f;
    public float density = 100;

    public VRTK_SimplePointer pointer;

    private float PointerThickness;
    private float PointerLength;

    public Player.Sizes MaxSize = Player.Sizes.Giant;
    public Player.Sizes MinSize = Player.Sizes.Midget;


    private float gravityMultiplier = 0;

    bool initalized = false;

    private void Initalize()
    {

        if (playerPresence == null || rb == null || resizeButton == null)
        {
            playerPresence = GetComponent<VRTK_PlayerPresence>();
            rb = GetComponent<Rigidbody>();
            findButton();

            PointerThickness = pointer.pointerThickness;
            PointerLength = pointer.pointerLength;
        }
        else
        {
            SetSize(Player.Sizes.Normal);
            playerPresence.SetFallingPhysicsOnlyParams(false);
            initalized = true;
        }
            
    }
    
	// Update is called once per frame
	void Update () {
        if (!initalized)
            Initalize();

        if (Player.currentState == Player.State.Growing || Player.currentState == Player.State.Shrinking)
            Resize();



        if (!rb.isKinematic)
        {
            if (rb && rb.velocity == Vector3.zero)
            {
                playerPresence.StopPhysicsFall();
                playerPresence.SetFallingPhysicsOnlyParams(false);
            }
            else
            {
                if (gravityMultiplier == 1)
                    gravityMultiplier = 0;
                rb.AddForce(-9.81f*gravityMultiplier * rb.mass*transform.up);
            }
        }


    }


    private bool changeSize(float size)
    {
                
        Vector3 targetSize = new Vector3(size, size, size);
        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime/ResizeTime);

        if (Mathf.Abs(transform.localScale.x - size) <= 0.01f)
        {
            transform.localScale = targetSize;
            
            return true;
        }
        else
            return false;

    }


    private void Resize()
    {
        playerPresence.SetFallingPhysicsOnlyParams(false);
        pointer.enabled = false;
        float targetSize = GetTargetSize(Player.currentSize);
        if (changeSize(targetSize))
        {
            rb.mass = density * Mathf.Pow(targetSize, 3);
            Player.currentState = Player.State.Idle;
            playerPresence.SetFallingPhysicsOnlyParams(true);
            pointer.enabled = true;
            pointer.pointerThickness = PointerThickness * targetSize;
            pointer.pointerLength = PointerLength * targetSize;
            gravityMultiplier = targetSize;

            Debug.Log(Player.currentSize);
        }
    }

    void findButton()
    {
        GameObject resizer = GameObject.FindGameObjectWithTag("Resizer");
        if(resizer != null && resizer.activeSelf)
        {
            resizeMenu = resizer.GetComponent<RadialMenu>();
            if(resizeMenu != null)
            {
                resizeButton = resizeMenu.GetButton(0);
            }
                
        }
        
    }

    public void StartGrowing()
    {
        

        if (Player.getSizeIndex(Player.currentSize) > 0 && Player.getSizeIndex(Player.currentSize) >= Player.getSizeIndex(MaxSize))
        {
            
            float targetSize = GetTargetSize(Player.currentSize - 1);
            if (CheckSpace(targetSize))
            {
                Player.currentSize = Player.currentSize - 1;
                Player.currentState = Player.State.Growing;
                resizeMenu.RegenerateButtons();
            }
        }
            
        
    }

    public void StartShrinking()
    {

        if (Player.getSizeIndex(Player.currentSize) < Player.numberOfSizes-1 && Player.getSizeIndex(Player.currentSize) <= Player.getSizeIndex(MinSize))
        {

            float targetSize = GetTargetSize(Player.currentSize + 1);
            if (CheckSpace(targetSize))
            {
                Player.currentSize = Player.currentSize + 1;
                Player.currentState = Player.State.Shrinking;
                resizeMenu.RegenerateButtons();
            }
        }

        Player.currentState = Player.State.Shrinking;
        resizeMenu.RegenerateButtons();
    }


    public void SetSize(Player.Sizes size)
    {
        if (Player.currentSize != size && Player.getSizeIndex(Player.currentSize) >= Player.getSizeIndex(MaxSize) && Player.getSizeIndex(Player.currentSize) <= Player.getSizeIndex(MinSize))
        {
            if (CheckSpace(GetTargetSize(size)))
            {
               Player.State state;

                if (Player.getSizeIndex(Player.currentSize) > Player.getSizeIndex(size))
                    state = Player.State.Growing;
                else
                    state = Player.State.Shrinking;

                Player.currentState = state;                       
           
                Player.currentSize = size;
            }

        }
    }


    private bool CheckSpace(float size)
    {
        Vector3 origin = transform.position;
        origin.y += size / 2;

        float x = size * 2;
        float y = size;
        float z = size * 2;

       RaycastHit hit;
       if(Physics.Raycast(origin, Vector3.forward, out hit, size))
        {
            z -= size - hit.distance;
        }


        if (Physics.Raycast(origin, Vector3.back, out hit, size))
        {
            z -= size - hit.distance;
        }

        if (Physics.Raycast(origin, Vector3.right, out hit, size))
        {
            x -= size - hit.distance;
        }


        if (Physics.Raycast(origin, Vector3.left, out hit, size))
        {
            x -= size - hit.distance;
        }

        if (Physics.Raycast(origin, Vector3.up, out hit, size / 2))
        {
            y -= size / 2 - hit.distance;
        }

        if (x < size || y < size || z < size)
            return false;

        return true;
    }


    public float GetTargetSize(Player.Sizes size)
    {
        if (size == Player.Sizes.Giant)
            return GiantSize;
        else if (size == Player.Sizes.Normal)
            return NormalSize;
        else if (size == Player.Sizes.Midget)
            return MidgetSize;

        return 0;
    }
}
