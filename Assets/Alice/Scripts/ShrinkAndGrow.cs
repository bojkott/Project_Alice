using UnityEngine;
using System.Collections;
using VRTK;
public class ShrinkAndGrow : MonoBehaviour {

    enum State {Grown, Shrunken, Growing, Shrinking};

    private State state;
    RadialMenuButton resizeButton;
    RadialMenu resizeMenu;
    public float GrownSize = 10.0f;
    public float ShrunkenSize = 1.0f;
    public float ResizeTime = 0.1f;

    public Sprite growIcon;
    public Sprite ShrinkIcon;


	// Update is called once per frame
	void Update () {
        if (resizeButton == null)
            findButton();


        if(state == State.Growing)
        {
            if (changeSize(GrownSize))
                state = State.Grown;
            
        }
        else if(state == State.Shrinking)
        {
            if (changeSize(ShrunkenSize))
                state = State.Shrunken;
        }

    }


    private bool changeSize(float size)
    {
        Vector3 targetSize = new Vector3(size, size, size);
        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime/ResizeTime);

        if (Mathf.Abs(transform.localScale.x - size) <= 0.001f)
        {
            transform.localScale = targetSize;
            return true;
        }
        else
            return false;

    }


    public void Resize()
    {
        
        if (state == State.Grown)
            Shrink();
        else if (state == State.Shrunken)
            Grow();

        
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
                Grow();
            }
                
        }
        
    }

    private void Grow()
    {
        state = State.Growing;
        resizeButton.ButtonIcon = growIcon;
        resizeMenu.RegenerateButtons();
    }

    private void Shrink()
    {
        state = State.Shrinking;
        resizeButton.ButtonIcon = ShrinkIcon;
        resizeMenu.RegenerateButtons();
    }

}
