using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [SerializeField] private Texture2D[] UpCursors;
    [SerializeField] private Texture2D[] DownCursors;
    [SerializeField] private Texture2D[] DodgeCursors;

    private bool isUp = true;

    private bool isAnimated = false;

    private int cursorIndex = 6;

    private float frameRate = .15f;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        //updateCursor(getCurrentCursor());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    isUp = false;
        //    if (!isAnimated) updateCursor(getCurrentCursor());
        //}
        //if (Input.GetKeyUp(KeyCode.Mouse0)) 
        //{
        //    isUp = true;
        //    if (!isAnimated) updateCursor(getCurrentCursor());
        //}
    }

    public void setCursorIndex(int newIndex)
    {
        cursorIndex = newIndex;

        //if(!isAnimated) updateCursor(getCurrentCursor());
    }

    public void dodge()
    {
        //StartCoroutine("dodgeAnimation");
    }

    private IEnumerator dodgeAnimation()
    {
        isAnimated = true;

        updateCursor(DodgeCursors[0]);
        yield return new WaitForSeconds(frameRate);
        updateCursor(DodgeCursors[1]);
        yield return new WaitForSeconds(frameRate);

        isAnimated = false;

        //updateCursor(getCurrentCursor());
    }

    //private Texture2D getCurrentCursor()
    //{
    //    //return (isUp) ? UpCursors[cursorIndex] : DownCursors[cursorIndex];
    //}

    private void updateCursor(Texture2D texture)
    {
        //Cursor.SetCursor(texture, new Vector2(3, 1), CursorMode.Auto);
    }
}
