using UnityEngine;

using System.Collections;

public class FollowTarget : MonoBehaviour
{
	//will be the player we follow
	public GameObject target;
    float sarrakkenFixedx;
    float sarrakkenFixedy;
    
    bool fixedPos;
	//will be the position of the player
	Vector3 targetPosition;
    
	//height that the camera will be rised
	float height = 1f;

    //the smoothness of the camera
    public float smoothTime;
    float nextFpsUpdate;
    float fps;
    int lastFrameCount;
    //dont know
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        sarrakkenFixedx = 29.73f;
        sarrakkenFixedy = -13.37f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > nextFpsUpdate)
        {
            fps = ((float)(Time.frameCount - lastFrameCount));
            nextFpsUpdate = Time.time + 1.0f;
            lastFrameCount = Time.frameCount;
        }
        
        if (!fixedPos)
        {
            //target position is the players position
            targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);           
            //transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
        }
        //transform the position with the smooth camera shift
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        
    }
    public void SetFixed()
    {
        targetPosition = new Vector3(sarrakkenFixedx, sarrakkenFixedy, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        fixedPos = true;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 100.0f, Screen.height / 13.0f, Screen.width / 20f, Screen.height / 15f), "FPS: " + fps);           
    }
}
