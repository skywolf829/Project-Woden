using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
    public const int TEST = 0;
    public const int SPOOKSTON = 1;
    public const int SARRAKKEN = 2;
    public const int MADE = 3;
    public const int YET = 4;

    public void loadScene(int level){
        if (level == TEST)
        {
            Application.LoadLevel("RealTestMap");
        }
        else if (level == SPOOKSTON)
        {
            Application.LoadLevel("CastleSpookstonLevel");
        }	
        else if(level == SARRAKKEN)
        {
            Application.LoadLevel("SarrakkenMap");
        }	
	}
}

