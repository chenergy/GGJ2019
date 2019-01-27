using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsleepNPC : MonoBehaviour
{

    public MORPH3D.M3DCharacterManager M3dModel;
    public string morphName = "G2M_FHMAlienGrey";

    private string BreathId = "";

    public float Frequency = 1;
    private float Direction = 1;
    private float Counter = 0;

    private void Update()
    {
        if (Direction > 0)
        {
            Counter += Time.deltaTime;
            if (Counter > Frequency)
            {
                Direction = 0 - Direction;
            }
        }
        else
        {
            Counter -= Time.deltaTime;
            if (Counter < 0)
            {
                Direction = 0 - Direction;
            }
        }
        var val = Frequency / Counter ;
        M3dModel.SetBlendshapeValue(morphName, val);
    }
}
