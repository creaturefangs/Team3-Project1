using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsScript : MonoBehaviour
{

    public GameObject footstep;

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            Footsteps();
        }
        else
        {
            StopFootsteps();
        }

        void Footsteps()
        {
            footstep.SetActive(true);
        }

        void StopFootsteps()
        {
            footstep.SetActive(false);
        }
    }
}
