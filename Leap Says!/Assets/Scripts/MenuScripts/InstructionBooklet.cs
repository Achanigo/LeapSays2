using UnityEngine;
using System.Collections;

public class InstructionBooklet : MonoBehaviour 
{
	void Start ()
	{
		this.gameObject.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.gameObject.SetActive(false);
		}
	}
}
