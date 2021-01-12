using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController LeftTelelportRay;
    public XRController RightTelelportRay;
    public InputHelpers.Button TeleportActivationButton;
    public float activationThreshold = 0.1f;

    public XRRayInteractor leftinteractorray;
    public XRRayInteractor rightinteractorray;

    public bool enableLeftTeleport { get; set; } = true;
    public bool enableRightTeleport { get; set; } = true;

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;


        if (RightTelelportRay)
        {
            bool rightinteractorrayhovering = rightinteractorray.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            RightTelelportRay.gameObject.SetActive(enableRightTeleport && CheckIfActivated(RightTelelportRay));
        }

        if (LeftTelelportRay)
        {
            bool isleftinteractorrayhovering = leftinteractorray.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            LeftTelelportRay.gameObject.SetActive(enableLeftTeleport && CheckIfActivated(LeftTelelportRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, TeleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
