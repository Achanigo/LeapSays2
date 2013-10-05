using UnityEngine;
using System.Collections;

public class MainGameplayScript : MonoBehaviour 
{
	public GameObject[] signArray;
	public GUITexture[] signTextures;
	private int[] numberMatrix;
	private int currentIndex;
	
	// Use this for initialization
	void Start () 
	{
		this.numberMatrix = new int[18];
		this.currentIndex = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
