using UnityEngine;
using System.Collections;
using VRTK;
public class ShrinkAndGrow : MonoBehaviour {


    RadialMenuButton resizeButton;
    RadialMenu resizeMenu;
    private VRTK_PlayerPresence playerPresence;
    private Rigidbody rb;

    public float GrownSize = 10.0f;
    public float ShrunkenSize = 1.0f;
    public float ResizeTime = 0.1f;
    public float density = 100;

    public Sprite growIcon;
    public Sprite ShrinkIcon;
    public VRTK_SimplePointer pointer;


    bool initalized = false;

    private void Initalize()
    {

        if (playerPresence == null || rb == null || resizeButton == null)
        {
            playerPresence = GetComponent<VRTK_PlayerPresence>();
            rb = GetComponent<Rigidbody>();
            findButton();
        }
        else
        {
            StartGrowing();
            playerPresence.SetFallingPhysicsOnlyParams(false);
            initalized = true;
        }
            
    }
    



	// Update is called once per frame
	void Update () {
        if (!initalized)
            Initalize();

        if (Player.currentState == Player.State.Growing)
            Grow();
        else if (Player.currentState == Player.State.Shrinking)
            Shrink();


        if (playerPresence.IsFalling())
        {
            if (rb && rb.velocity == Vector3.zero)
            {
                playerPresence.StopPhysicsFall();
                playerPresence.SetFallingPhysicsOnlyParams(false);
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


    private void Grow()
    {
        playerPresence.SetFallingPhysicsOnlyParams(false);
        pointer.enabled = false;
        if (changeSize(GrownSize))
        {
            Player.currentState = Player.State.Grown;
            rb.mass = density * Mathf.Pow(GrownSize, 3);
            playerPresence.SetFallingPhysicsOnlyParams(true);
            pointer.enabled = true;

        }
    }

    private void Shrink()
    {
        playerPresence.SetFallingPhysicsOnlyParams(false);
        pointer.enabled = false;
        if (changeSize(ShrunkenSize))
        {
            Player.currentState = Player.State.Shrunken;
            rb.mass = density * Mathf.Pow(ShrunkenSize, 3);
            playerPresence.SetFallingPhysicsOnlyParams(true);
            pointer.enabled = true;
        }
    }

    public void Resize()
    {
        

        if (Player.currentState == Player.State.Grown)
            StartShrinking();
        else if (Player.currentState == Player.State.Shrunken)
            StartGrowing();

        
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

    private void StartGrowing()
    {
        Player.currentState = Player.State.Growing;
        resizeButton.ButtonIcon = growIcon;
        resizeMenu.RegenerateButtons();
    }

    private void StartShrinking()
    {
        Player.currentState = Player.State.Shrinking;
        resizeButton.ButtonIcon = ShrinkIcon;
        resizeMenu.RegenerateButtons();
    }

}
