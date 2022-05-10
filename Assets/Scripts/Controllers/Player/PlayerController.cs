using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
		if (!isLocalPlayer)
		{
            //Disable camera if we are not the local player
            playerCamera.gameObject.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        //If we are not the main client, don't run this method
		if (!isLocalPlayer)
		{
            return;
		}   
    }
}
