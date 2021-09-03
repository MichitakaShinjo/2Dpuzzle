using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallscript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("syoutotu");
        if (collision.gameObject.tag == "Bomb")
        {
            Destroy(collision.gameObject);

        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
