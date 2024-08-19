using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("kagune"))
        {
            // Perform your desired action here
            Debug.Log("Player entered trigger zone.");

            // Example: Disable this object
            gameObject.SetActive(false);
        }
    }
}
