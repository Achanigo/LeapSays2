using UnityEngine;
using System.Collections;

public class MainGameplayScript : MonoBehaviour 
{
	
	public AudioSource audioSource;
	public AudioClip correctAnswerAudio;
	public AudioClip cameraAnswerAudio;
	public AudioClip inputAudio;
	public AudioClip pauserSound;
	public AudioClip whistleAudio;
	public AudioClip wrongAnswerAudio;
	
	
	public GameObject[] signArray;
	public GameObject signMessage;
	public Texture2D[] signTextures;
	public Texture2D startTexture;
	public Texture2D finishTexture;
	public Texture2D pausetexture;
	
	private bool waitingForPlayer;
	private int[] numberArray;
	private int currentIndex;
	
	private void HideHandSigns ()
	{
		for (int i = 0; i <= this.currentIndex; i++)
		{
			this.signArray[i].SetActive(false);
		}
	}
	
	private IEnumerator LoadStart ()
	{
		this.signMessage.guiTexture.texture = this.startTexture;
		this.signMessage.SetActive(true);
		
		yield return new WaitForSeconds (2);
		
		audioSource.PlayOneShot(whistleAudio);
		this.signMessage.SetActive(false);
		
		this.numberArray[this.currentIndex] = (int)(Random.Range(0, 9));
		yield return new WaitForSeconds (2);
		
		for (int i = 0; i <= this.currentIndex; i++)
		{
			this.signArray[i].guiTexture.texture = this.signTextures[this.numberArray[i]];
			this.signArray[i].SetActive(true);
			this.audioSource.PlayOneShot(this.inputAudio);
			
			yield return new WaitForSeconds (2);
		}
		
		yield return new WaitForSeconds (5);
		
		this.HideHandSigns();
		
		this.audioSource.PlayOneShot(this.whistleAudio);
	}

	// Use this for initialization
	void Start () 
	{
		this.numberArray = new int[18];
		this.currentIndex = 0;
		this.waitingForPlayer = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (GolbalVariables.playerLives > 0)
		{
			if (!this.waitingForPlayer)
			{
				StartCoroutine(LoadStart());
				this.waitingForPlayer = true;
			}
			else
			{
				
			}
		}
	}
}
