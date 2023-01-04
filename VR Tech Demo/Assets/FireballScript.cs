using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    public GameObject ExplostionVFX;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(ExplostionVFX, transform.position, transform.rotation);
    }
}
