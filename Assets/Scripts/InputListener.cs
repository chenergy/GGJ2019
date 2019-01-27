using UnityEngine;
using UnityEngine.UI;

public class InputListener : MonoBehaviour
{
    public bool debug;
    public Text debugText;
    public GameObject controller;
    public GameObject controllerRay;
    private GameObject _grabbed;
    private Vector3 _relativeOffset;
    private Quaternion _startParentRotation;
    private Quaternion _startChildRotation;

    public void Start()
    {
        debugText.gameObject.SetActive(debug);
    }

    public void Update()
    {
        OVRInput.Update();
        Vector3 localPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);
        Quaternion localRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        controller.transform.localPosition = localPos;
        controller.transform.localRotation = localRot;

        // Controller raycast.
        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        RaycastHit hit;
        GameObject target = null;
        string targetName;
        bool triggerDown = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        float width = triggerDown ? 2 : 1;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            controllerRay.transform.localScale = new Vector3(width, width, hit.distance);
            target = hit.collider.gameObject;
            targetName = target.name;
            if (_grabbed == null && target.tag == "Interactable" && triggerDown)
            {
                _grabbed = target;
                _relativeOffset = controller.transform.InverseTransformPoint(_grabbed.transform.position);
                _startParentRotation = controller.transform.rotation;
                _startChildRotation = _grabbed.transform.rotation;
                _grabbed.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else
        {
            controllerRay.transform.localScale = new Vector3(width, width, 1000f);
            targetName = "None";
        }

        if (_grabbed != null && !triggerDown)
        {
            _grabbed.GetComponent<Rigidbody>().useGravity = true;
            _grabbed = null;
        }

        if (_grabbed != null)
        {
            _grabbed.transform.position = controller.transform.TransformPoint(_relativeOffset);
            _grabbed.transform.rotation = controller.transform.rotation * Quaternion.Inverse(_startParentRotation) * _startChildRotation;
        }

        if (debug)
        {
            Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            string tag = "None";
            if (target != null)
            {
                tag = target.tag;
            }

            string grabbedName = _grabbed == null ? "None" : _grabbed.name;
            debugText.text = $"localPos={localPos}\nlocalRot={localRot}\ntarget={targetName}\ntag={tag}\ngrabbed={grabbedName}\naxis={axis}\ntriggerDown={triggerDown}";
        }
    }
}