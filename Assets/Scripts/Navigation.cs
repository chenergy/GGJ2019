using UnityEngine;

public class Navigation : MonoBehaviour
{
    public LayerMask traversableLayer;
    public Transform centerEyeAnchor;
    public float moveSpeed = 0.1f;
    public void Update()
    {
        Vector3 nextPos = transform.position;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W))
        {
            nextPos += centerEyeAnchor.transform.forward * Time.deltaTime;
        }
#endif

        // Controller movement.
        Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            if (axis.y > 0.1f)
            {
                nextPos += centerEyeAnchor.transform.forward * Time.deltaTime;
            }
        }

        Vector3 up = -transform.up + centerEyeAnchor.transform.forward;
        Ray rayD = new Ray(nextPos, -transform.up);
        Ray rayF = new Ray(nextPos, centerEyeAnchor.transform.forward);
        RaycastHit hitD;
        RaycastHit hitF;
        bool d = Physics.Raycast(rayD, out hitD, 10f, traversableLayer);
        bool f = Physics.Raycast(rayF, out hitF, 1f, traversableLayer);
        transform.up = Vector3.Lerp(transform.up, (hitD.normal * hitD.distance + hitF.normal * hitF.distance).normalized, Time.deltaTime * 2);
        transform.position = d ? hitD.point + transform.up.normalized : transform.position;
    }
}