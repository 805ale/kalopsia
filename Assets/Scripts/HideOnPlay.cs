using UnityEngine;
using System.Collections;


public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    // Start method
    // Automatically hide the mesh when on play mode
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
