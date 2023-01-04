using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REALFireballExplode : MonoBehaviour
{

    public GameObject ExplostionVFX;
   
    private void OnTriggerEnter(Collider other)
    {
        string name = other.gameObject.name;
        if (name.Contains("VR Rig")) return;
        if(name.Contains("Wand")) return;
        if (name.Contains("Hand")) return;
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
        Instantiate(ExplostionVFX, transform.position, transform.rotation);
    }
}
