using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }
}
