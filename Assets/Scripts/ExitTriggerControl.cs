using UnityEngine;
using System.Collections;

public class ExitTriggerControl : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Still in trigger");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger");
    }
}
