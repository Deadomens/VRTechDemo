using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics ControllerCharacteristics;
    public List<GameObject> ControllerPrefabs;
    public GameObject HandModelPrefab;

    private InputDevice targeDevice;
    private GameObject SpawnController;
    private GameObject SpawnHandModel;
    private Animator HandAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }


    void TryInitialize()
    {

        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(ControllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }


        if (devices.Count > 0)
        {
            targeDevice = devices[0];
            GameObject prefab = ControllerPrefabs.Find(controller => controller.name == targeDevice.name);
            if (prefab)
            {
                SpawnController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Could not find Controller Model");
                SpawnController = Instantiate(ControllerPrefabs[0], transform);
            }

            SpawnHandModel = Instantiate(HandModelPrefab, transform);
            HandAnimator = SpawnHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if(targeDevice.TryGetFeatureValue(CommonUsages.trigger, out float TriggerValue))
        {
            HandAnimator.SetFloat("Trigger", TriggerValue);
        }
        else
        {
            HandAnimator.SetFloat("Trigger", 0);
        }

        if (targeDevice.TryGetFeatureValue(CommonUsages.grip, out float GripValue))
        {
            HandAnimator.SetFloat("Grip", GripValue);
        }
        else
        {
            HandAnimator.SetFloat("Grip", 0);
        }

    }

        // Update is called once per frame
        void Update()
        {

        if (!targeDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if(showController)
            {
                SpawnHandModel.SetActive(false);
                SpawnController.SetActive(true);
            }
            else
            {
                SpawnHandModel.SetActive(true);
                SpawnController.SetActive(false);
            UpdateHandAnimation();
            }
        }

        }
}
