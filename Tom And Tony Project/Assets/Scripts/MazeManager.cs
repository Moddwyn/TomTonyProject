using System.Collections;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MazeManager : MonoBehaviour
{
	[InfoBox("MAKE SURE YOUR PLAYER HAS TAG \"PLAYER\"")]
	[Required][SerializeField] MazeSpawner mazeSpawner;
	[Required][SerializeField] GameObject mazeGoalPrefab;
	[Required][SerializeField] Material mazeGoalMaterial;
	[Required][SerializeField] Camera mazeCamera;
	[Required][SerializeField] CanvasGroup blackPanel;
	[Required][SerializeField] TMP_Text timeText;
	
	[HorizontalLine(height: 7,color:EColor.Black)]
	[Required][SerializeField] Rigidbody player;
	[MinValue(0)] public float playerSpeed = 2;
	[Required][SerializeField] FloatingJoystick joystick;
	public bool allowMovement;
	
	[HorizontalLine(height: 7,color:EColor.Black)]
	public AudioClip endSound;
	public UnityEvent OnReachEnd;
	[HorizontalLine(height: 7,color:EColor.Black)]
	
	public MazeDifficulty[] difficultyList;
	[Required][Dropdown("GetDifficulties")] public MazeDifficulty currentDifficulty;
	
	MazeGoal mazeGoal;
	bool reachedEnd;
	bool reachedEndUpdate = true;
	Vector2 joystickPosition;
	float time;

	void Start()
	{
		time = 0;
		blackPanel.alpha = 1;
		StartCoroutine(FadeIn());
		player.GetComponent<MeshRenderer>().enabled = false;
		StartGame(0);
	}

	IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 3)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 3);
            blackPanel.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackPanel.alpha = 0f;
    }
	IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 3)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 3);
            blackPanel.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackPanel.alpha = 1f;
    }

	public void StartGame(int index)
	{
		currentDifficulty = GetDifficulties()[index];
		InitDimensions();

		mazeSpawner.Generate();
		SetupEndGoal();
		CameraSetup();

		player.GetComponent<MeshRenderer>().enabled = true;
	}

	void Update()
	{
		joystickPosition = new Vector2(joystick.Horizontal, joystick.Vertical);

		time+= Time.deltaTime;
		timeText.text = "time elapsed: " + time.ToString("F2");
		
		MazeEndUpdate();
	}
	void FixedUpdate()
	{
		if(allowMovement)
			player.velocity = new Vector3(joystickPosition.x,0,joystickPosition.y)
			* Time.fixedDeltaTime * playerSpeed;
	}
	
	void MazeEndUpdate()
	{
		if(reachedEndUpdate)
		{
			reachedEnd = mazeGoal? mazeGoal.finished : false;
			if(reachedEnd)
			{
				StartCoroutine(PlayEndSound());
				reachedEndUpdate = false;
			}
		}
	}

	IEnumerator PlayEndSound()
	{
		allowMovement = false;
		GameManager.Instance.audioSource.PlayOneShot(endSound);
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(4);
		OnReachEnd.Invoke();
	}
	
	void SetupEndGoal()
	{
		mazeGoal = Instantiate(mazeGoalPrefab.gameObject, 
					new Vector3((currentDifficulty.rows-1)*mazeSpawner.CellWidth,
								0,
								(currentDifficulty.columns-1)*mazeSpawner.CellHeight), Quaternion.identity).
								GetComponent<MazeGoal>();
								
		mazeSpawner.transform.GetComponentsInChildren<Transform>().
		ToList<Transform>().FindLast(x=>x.name.Contains("Floor")).
		GetComponent<MeshRenderer>().material = mazeGoalMaterial;
	}
	
	void CameraSetup()
	{
		mazeCamera.transform.position = new Vector3(((currentDifficulty.rows-1)*mazeSpawner.CellWidth),
													30*2,
													((currentDifficulty.columns-1)*mazeSpawner.CellHeight))/2;
		mazeCamera.orthographicSize = currentDifficulty.rows * 3;
	}
	
	void InitDimensions()
	{
		mazeSpawner.Rows = currentDifficulty.rows;
		mazeSpawner.Columns = currentDifficulty.columns;
	}
	
	MazeDifficulty[] GetDifficulties() => difficultyList;	
}