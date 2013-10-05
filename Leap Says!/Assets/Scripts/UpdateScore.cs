using UnityEngine;
using System.Collections;

public class UpdateScore : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.guiText.text = "Score: " + GolbalVariables.score;
	}
}
