using UnityEngine;
using System.Collections;

public class startMainMenu : MonoBehaviour 
{
	public GameObject instrucciones;
	public Texture2D normalTexture;
	public Texture2D onMouseOvertexture;
	public Texture2D onMouseDownTexture;
	
	public AudioSource audioSource;
	public AudioClip selectAudio;
	
	void OnMouseDown()
	{
		if (!this.instrucciones.activeInHierarchy)
		{
			this.guiTexture.texture = this.onMouseDownTexture;
			Application.LoadLevel("MainGame");
		}
	}
	
	void OnMouseExit()
	{
		this.guiTexture.texture = this.normalTexture;
	}
	
	void OnMouseEnter()
	{
		this.guiTexture.texture = this.onMouseOvertexture;
	}
}
