using UnityEngine;
using System.Collections;
using VRTK;
public class ShrinkAndGrow : MonoBehaviour {


    private RadialMenuButton resizeButton;
    private RadialMenu resizeMenu;
    private VRTK_PlayerPresence playerPresence;
    private Rigidbody rb;
    private AudioSource audioSource;

    private float gravityMultiplier = 0;
    private float pointerThickness;
    private float pointerLength;

    public float giantSize = 100.0f;
    public float normalSize = 10.0f;
    public float midgetSize = 1.0f;

    public float resizeTime = 0.2f;
    public float density = 100;

    public VRTK_SimplePointer pointer;

    public Player.Sizes maxSize = Player.Sizes.Giant;
    public Player.Sizes minSize = Player.Sizes.Midget;


    private float passedTime = 0;
    private Vector3 startScale;


    public AudioClip growingSFX;
    public AudioClip shrinkingSFX;

    

    private void Awake()
    {
        startScale = transform.localScale;

        playerPresence = GetComponent<VRTK_PlayerPresence>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        findButton();

        pointerThickness = pointer.pointerThickness;
        pointerLength = pointer.pointerLength;
                
        SetSize(Player.Sizes.Normal);
        ResizePointer();

    }


    
    
	// Update is called once per frame
	void Update () {
            
        if (Player.currentState == Player.State.Growing || Player.currentState == Player.State.Shrinking)
            Resize();



        if (!rb.isKinematic)
        {

            if (gravityMultiplier == 1)
                gravityMultiplier = 0;
            rb.AddForce(-9.81f*gravityMultiplier * rb.mass*transform.up);
            
        }


    }


    private bool changeSize(float size)
    {
        passedTime += Time.deltaTime / resizeTime;
        Vector3 targetSize = new Vector3(size, size, size);

        Vector3 newScale = Vector3.Lerp(startScale, targetSize, passedTime);

        ScaleWithHeadsetAdjust(newScale);

        if (passedTime > 1)
        {
            ScaleWithHeadsetAdjust(targetSize);
            startScale = transform.localScale;
            passedTime = 0;
            return true;
        }
        else
            return false;

    }


    private void ScaleWithHeadsetAdjust(Vector3 newScale)
    {
        Vector3 oldHeadsetPos = VRTK_DeviceFinder.HeadsetTransform().position;
        transform.localScale = newScale;
        Vector3 newHeadsetPos = VRTK_DeviceFinder.HeadsetTransform().position;

        Vector3 difference = oldHeadsetPos - newHeadsetPos;
        difference.y = 0; //Ignore height difference
        transform.position += difference;
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
            ResizePointer();
            gravityMultiplier = targetSize;

            Debug.Log(Player.currentSize);
        }
    }


    private void ResizePointer()
    {
        pointer.enabled = false;
        pointer.pointerThickness = pointerThickness * GetTargetSize(Player.currentSize);
        pointer.pointerLength = pointerLength * GetTargetSize(Player.currentSize);
        pointer.enabled = true;
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

        if (Player.CanGrow())
        {

            SetSize(Player.currentSize - 1);
        }
            
        
    }

        
    public void StartShrinking()
    {
        if (Player.CanShrink())
        {
            SetSize(Player.currentSize + 1);
        }
    }


    public void SetSize(Player.Sizes size)
    {
        if (Player.currentSize != size && ValidSize(size) && Player.currentState == Player.State.Idle)
        {
            if (CheckSpace(GetTargetSize(size)))
            {
               Player.State state;

                if (Player.getSizeIndex(Player.currentSize) > Player.getSizeIndex(size))
                {
                    state = Player.State.Growing;
                    audioSource.clip = growingSFX;
                    
                }
                else
                {
                    state = Player.State.Shrinking;
                    audioSource.clip = shrinkingSFX;
                }

                audioSource.Play();
                Player.currentState = state;                       
           
                Player.currentSize = size;
            }

        }
    }



    public bool ValidSize(Player.Sizes size) //Checks if the size is within the min-max sizes
    {
        if (Player.getSizeIndex(size) >= Player.getSizeIndex(maxSize) && Player.getSizeIndex(size) <= Player.getSizeIndex(minSize))
            return true;
        else
            return false;
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
            return giantSize;
        else if (size == Player.Sizes.Normal)
            return normalSize;
        else if (size == Player.Sizes.Midget)
            return midgetSize;

        return 0;
    }
}
