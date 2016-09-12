using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
    private bool open, paused;
    private GameObject[] inventoryItems;
    private int[] inventoryItemsAmounts;

    public Texture inventoryScreen;
    public Texture lightBlast;
    public Texture fireBlast;
    public Texture waterBlast;
    public Texture frostBlast;
    public Texture heal;
    public Texture emberStorm;
    public Texture fateJaveline;

    public Font font;
    private float x, y, w, h;

    private int maxStackSize;
    private int gold;

    private GameObject pauseObject;
    public void Awake()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Inventory"))
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
        inventoryItems = new GameObject[36];
        inventoryItemsAmounts = new int[36];

        maxStackSize = 10;
        gold = 100;
    }

    public bool getInventoryState()
    {
        return open;
    }
    
    public void setPausedState(bool b)
    {
        paused = b;
    }

    public void addItem(GameObject g)
    {
        if (g)
        {
            string tempNam = g.name;
            bool added = false;

            for (int i = 0; i < inventoryItems.Length && !added; i++)
            {
                if (inventoryItems[i].name.Equals(tempNam))
                {
                    inventoryItemsAmounts[i]++;
                    added = true;
                }
            }
            for (int i = 0; i < inventoryItems.Length && !added; i++)
            {
                if (inventoryItems[i] == null)
                {
                    inventoryItems[i] = g;
                    added = true;
                }
            }
        }
    }

    public void addGold(int g)
    {
        gold += g;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (open)
            {
                open = false;
                Time.timeScale = 1;
            }
			else if(!Application.loadedLevelName.Equals("MenuScreen") && !paused)
            {
                open = true;
                Time.timeScale = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (open)
            {
                open = false;
                Time.timeScale = 1;
            }
        }
        if (open)
        {
            w = Screen.width * 0.4f;
            h = Screen.height * 0.9f;
            x = (Screen.width - w) / 2.0f;
            y = (Screen.height - h) / 2.0f;
        }
    }
    public void OnGUI()
    {
        if (open)
        {
            GUI.DrawTexture(new Rect(x, y, w, h), inventoryScreen);
      
            GUI.DrawTexture(new Rect(x + Screen.width * 0.165f, y + Screen.height * 0.125f, Screen.width * 0.041f, Screen.height * 0.082f),
                frostBlast);
            GUI.DrawTexture(new Rect(x + Screen.width * 0.21f, y + Screen.height * 0.125f, Screen.width * 0.041f, Screen.height * 0.082f), 
                lightBlast);
            GUI.DrawTexture(new Rect(x + Screen.width * 0.25f, y + Screen.height * 0.125f, Screen.width * 0.041f, Screen.height * 0.082f), 
                waterBlast);
            GUI.DrawTexture(new Rect(x + Screen.width * 0.293f, y + Screen.height * 0.125f, Screen.width * 0.041f, Screen.height * 0.082f), 
                fireBlast);

            GUI.DrawTexture(new Rect(x + Screen.width * 0.293f, y + Screen.height * 0.305f, Screen.width * 0.041f, Screen.height * 0.082f), 
                fateJaveline);
            GUI.DrawTexture(new Rect(x + Screen.width * 0.25f, y + Screen.height * 0.305f, Screen.width * 0.041f, Screen.height * 0.082f), 
                heal);
            GUI.DrawTexture(new Rect(x + Screen.width * 0.21f, y + Screen.height * 0.305f, Screen.width * 0.041f, Screen.height * 0.082f), 
                emberStorm);

            for(int i = 0; i < inventoryItems.Length; i++)
            {
                if(inventoryItems[i] != null)
                {
                    //GUI.DrawTexture(new Rect(), inventoryItems[i].GetComponent<Animation>());
                }
            }
        }
    }
}

