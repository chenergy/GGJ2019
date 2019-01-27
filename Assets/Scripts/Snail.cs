using UnityEngine;

public class Snail : MonoBehaviour
{
    public Transform cameraRig;
    public Transform centerEyeAnchor;

    public void Update()
    {
        transform.rotation = centerEyeAnchor.rotation;
        transform.position = cameraRig.position - cameraRig.up;
    }
}

