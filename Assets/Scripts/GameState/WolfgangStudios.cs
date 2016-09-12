using UnityEngine;
using System.Collections;

public class WolfgangStudios : MonoBehaviour
{
    private float screenTime;
    private float screenStart;
    public void Start()
    {
        screenStart = Time.time;
        screenTime = 5.0f;
    }

    public void Update()
    {
        if (Input.anyKey)
        {
            Application.LoadLevel("MenuScreen");
        }
        else if (Time.time > screenStart + screenTime)
        {
            Application.LoadLevel("MenuScreen");
        }

    }
}