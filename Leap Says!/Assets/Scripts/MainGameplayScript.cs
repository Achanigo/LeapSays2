using UnityEngine;
using System.Collections;
using Leap;
using System.Net;

class LeapMotion
{   
	private Controller controller;
	private Frame f;
	private int activeDirection;
	
	public LeapMotion ()
	{		
		controller = new Controller ();
		activeDirection = 0;
	}
	
	public int countFingers ()
	{
		return f.Fingers.Count;
	}
	
	public bool checkHandsReady ()
	{
		double avgZ = 0.0;
		f = controller.Frame ();
		
		foreach (Hand h in f.Hands)
		{
			avgZ += h.PalmPosition.z;
		}
		
		avgZ /= f.Hands.Count;
		
		if (avgZ >= 100.0 && activeDirection <= 0) 
		{
			activeDirection = 1;
			return true;
		}
		
		if (avgZ <= -100.0 && activeDirection >= 0) 
		{
			activeDirection = -1;
			return true;
		}
		
		return false;
	}
}


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
	public GameObject[] crosses;
	public GameObject signMessage;
	public Texture2D[] signTextures;
	public Texture2D startTexture;
	public Texture2D finishTexture;
	public Texture2D pausetexture;
	
	private bool waitingForPlayer;
	private int[] numberArray;
	private int[]  answerArray;
	private float[] timeArray;
	private int currentIndex;
	private int currentAnswerIndex;
	private int totalOfInputs;
	private float lastInterval;
	private bool loadStartFlag;
	private bool verifyAnswersFlag;
	private bool lastWhistle;
	
	private LeapMotion lm;
	private string name;
	private bool useGUI;
	
	private void HideHandSigns ()
	{
		for (int i = 0; i <= this.currentIndex; i++)
		{
			this.signArray[i].SetActive(false);
		}
	}
	
	private int CalculateTimePoints(float time)
	{
		if (time <= 2)
		{
			return 15;
		}
		else if (time <= 4)
		{
			return 10;
		}
		else if (time <= 6)
		{
			return 5;
		}
		else
		{
			return 1;	
		}
	}
	
	private int ReceiveInput()
	{
		if (lm.checkHandsReady ()) 
		{
			this.audioSource.PlayOneShot (this.cameraAnswerAudio);
			return lm.countFingers ();
		}
		
		return -1;
	}
	
	private IEnumerator VerifyAnswers ()
	{
		yield return new WaitForSeconds (1);
		
		for (int i = 0; i <= this.currentIndex; i++)
		{
			if (GolbalVariables.playerLives > 0)
			{
				this.signArray[i].SetActive(true);
				
				if (this.numberArray[i] == this.answerArray[i])
				{
					// Correct Answer
					this.audioSource.PlayOneShot(this.correctAnswerAudio);
					GolbalVariables.score += (int)(this.timeArray[i] * 10);
				}
				else
				{
					// Wrong Answer
					this.audioSource.PlayOneShot(this.wrongAnswerAudio);
					if (GolbalVariables.playerLives > 0)
					{
						this.crosses[5 - GolbalVariables.playerLives].SetActive(true);
						GolbalVariables.playerLives--;
					}
				}	
			}
			yield return new WaitForSeconds (1);
		}
		
		if (GolbalVariables.playerLives > 0)
		{
			yield return new WaitForSeconds (1);
			
			this.HideHandSigns();
			
			this.currentIndex++;
		}
		this.waitingForPlayer = false;
	}
	
	private IEnumerator LoadStart ()
	{
		this.signMessage.guiTexture.texture = this.startTexture;
		yield return new WaitForSeconds (1);
		
		this.signMessage.SetActive(true);
		audioSource.PlayOneShot(whistleAudio);
		
		yield return new WaitForSeconds (1);
		
		this.signMessage.SetActive(false);
		
		this.numberArray[this.currentIndex] = (int)(Random.Range(0, 9));
		yield return new WaitForSeconds (1);
		
		for (int i = 0; i <= this.currentIndex; i++)
		{
			this.signArray[i].guiTexture.texture = this.signTextures[this.numberArray[i]];
			this.signArray[i].SetActive(true);
			this.audioSource.PlayOneShot(this.inputAudio);
			
			yield return new WaitForSeconds (1);
		}
		
		yield return new WaitForSeconds (3);
		
		this.HideHandSigns();
		
		this.audioSource.PlayOneShot(this.whistleAudio);
		
		this.totalOfInputs = this.currentIndex + 1;
		this.currentAnswerIndex = 0;
		this.waitingForPlayer = true;
		this.lastInterval = Time.time;
	}

	// Use this for initialization
	void Start () 
	{
		this.numberArray = new int[18];
		this.answerArray = new int[18];
		this.timeArray = new float[18];
		this.currentIndex = 0;
		this.waitingForPlayer = false;
		this.loadStartFlag = false;
		this.lastInterval = 0;
		this.lastWhistle = false;
		this.lm = new LeapMotion ();
		this.useGUI = false;
		
		foreach (GameObject cross in this.crosses)
		{
			cross.SetActive(false);	
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (GolbalVariables.playerLives > 0 && this.currentIndex < 18)
		{
			if (!this.waitingForPlayer)
			{
				if (!this.loadStartFlag)
				{
					StartCoroutine(LoadStart());
					this.loadStartFlag = true;
					this.verifyAnswersFlag = false;
				}
			}
			else
			{
				if (this.totalOfInputs > 0)
				{
					int inputReceived = this.ReceiveInput();
					
					if (inputReceived >= 0)
					{
						this.answerArray[this.currentAnswerIndex] = inputReceived;
						this.timeArray[this.currentAnswerIndex] = this.CalculateTimePoints(Time.time - this.lastInterval);
						this.lastInterval = Time.time;
						this.currentAnswerIndex++;
						this.totalOfInputs--;
					}
				}
				else
				{
					if (!this.verifyAnswersFlag)
					{
						StartCoroutine(VerifyAnswers());
						this.verifyAnswersFlag = true;
						this.loadStartFlag = false;
					}
				}
			}
		}
		else
		{
			this.signMessage.guiTexture.texture = this.finishTexture;
			this.signMessage.SetActive(true);
			this.HideHandSigns();
			
			if (!this.lastWhistle)
			{
				this.audioSource.PlayOneShot(this.whistleAudio);
				this.lastWhistle = true;
			}
			
			// Despues regresar al menu
			Application.LoadLevel("MenuScene");
		}
	}
}
