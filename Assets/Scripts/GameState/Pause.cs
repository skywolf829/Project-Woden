using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
    private bool paused;
    public Texture button;
    private GUIStyle style;
    private RectOffset over;
    public FontStyle fontStyle;
    public Font font;
    public Inventory invent;

    string[] buttonsText;

    public void Awake()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Pause"))
        {
            if (this.gameObject.GetInstanceID() != i.GetInstanceID())
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        style = new GUIStyle();
        //over = new RectOffset(10, 10, 0, 0);

        //style.overflow = over;
        style.alignment = TextAnchor.MiddleCenter;
        //style.fontStyle = fontStyle;
        //style.font = font;
        buttonsText = new string[3];
        buttonsText[0] = "Continue";
        buttonsText[1] = "Menu Screen";
        buttonsText[2] = "Quit";
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 1;
                invent.setPausedState(paused);
            }
            else if(!invent.getInventoryState())
            {
                paused = true;
                Time.timeScale = 0;
                invent.setPausedState(paused);
            }
        }
    }
    public void OnGUI()
    {
        
        if (paused)
        {
            //style.fontSize = Screen.width / 50;
            GUI.contentColor = Color.white;
            GUI.skin.font = font;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), button);
            GUI.backgroundColor = Color.clear;

            
            for(int i = 0; i < buttonsText.Length; i++)
            {
                if(GUI.Button(
                    new Rect(Screen.width / 2.0f - buttonsText[i].Length * font.fontSize / 2f, 
                    Screen.height / 20f * (5 + i),
                    buttonsText[i].Length * font.fontSize, 
                    Screen.height / 20f), 
                    buttonsText[i]))
                {
                    eventLoad(i);
                }
            }
            /*
            if (GUI.Button(new Rect(Screen.width / 2.0f, Screen.height / 20f * 5f, 180, Screen.height / 20f), "Menu Screen"))
            {
                Application.LoadLevel("MenuScreen");
            }
            else if (GUI.Button(new Rect(Screen.width / 2.0f, Screen.height / 20f * 6f, 100, Screen.height / 20f), "Quit"))
            {
                Application.Quit();
            }
            else if (GUI.Button(new Rect(Screen.width / 2.0f, Screen.height / 20f * 7f, 100, Screen.height / 20f), "Quit"))
            {
                Application.Quit();
            }
            */
        }
    }
    private void eventLoad(int i)
    {
        if(i == 0)
        {
            paused = false;
            Time.timeScale = 1;
            invent.setPausedState(paused);
        }
        else if(i == 1)
        {
            paused = false;
            Time.timeScale = 1;
            invent.setPausedState(paused);
            Application.LoadLevel("MenuScreen");
        }
        else if(i == 2)
        {
            paused = false;
            Time.timeScale = 1;
            invent.setPausedState(paused);
            Application.Quit();
        }
    }
}

