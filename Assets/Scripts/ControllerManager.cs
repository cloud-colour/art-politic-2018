using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputState
{
    Idle,
    Down,
    Hold,
    Up
}

public class ControllerManager : MonoBehaviour {

    private Vector3 startPos,endPos;

    [SerializeField]
    private Transform throwSpawnPos;

    private Transform tmpCash;

    private float dragTime;

    public float force;
    public float distanceFactor;

    private InputState currentInput;

	[Header("Trail Config")]
	[SerializeField]
	private Transform trailPrefab;
	[SerializeField]
	private float trailDragDuration;
	private Transform cacheTrail;

    private float intervalTime;

    private Dictionary<int,Vector3> dragTouchs;


	//idle check
	Vector3 lastMousePos;
	float idleTime;

	void Start () 
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
            GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.OpenSequence);

        dragTouchs = new Dictionary<int,Vector3>();
	}

	// Update is called once per frame
    void FixedUpdate () 
    {
		if(PlayerPrefs.GetInt("exhibition") == 1)
		{
			if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
			{
				if(Input.mousePosition == lastMousePos)
				{
					idleTime += Time.deltaTime;
					DebugCanvas.Inst.SetRestartText(60 - idleTime);
					if(idleTime >= 60)
					{
						DebugCanvas.Inst.Restart();
					}
				}
				else
				{
					idleTime = 0;
					lastMousePos = Input.mousePosition;
				}
			}
		}

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameSceneManager.GetInstance().GoToNextStage();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameSceneManager.GetInstance().GoToPrevStage();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameSceneManager.GetInstance().ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameSceneManager.GetInstance().ReturnToWelcome();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        if (GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.VictoryWaintInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameSceneManager.GetInstance().GoToNextStage();
            }
            return;
        }

        if(GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.TitleThrow)
		{
            intervalTime += Time.deltaTime;
            if (intervalTime >= 0.2f)
            {
                dragTime = Time.deltaTime * Random.Range(6, 7); 
                ThrowMoney(Random.Range(20, 60), Random.Range(6, 8) / 10f);
                intervalTime = 0;
            }
            dragTime = 0;
		}

        if (GameStateManager.GetInstance().GetGameState() != GameStateManager.GameState.GamePlay && 
            GameStateManager.GetInstance().GetGameState() != GameStateManager.GameState.TitleWaitInput)
            return;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            if(Input.touchCount > 3)
            {
                int checkMoveFinger = 0;
                foreach(var touch in Input.touches)
                {
                    if(touch.phase == TouchPhase.Began)
                    {
                        if(!dragTouchs.ContainsKey(touch.fingerId))
                            dragTouchs.Add(touch.fingerId,touch.position);
                        else
                            dragTouchs[touch.fingerId] = touch.position;
                    }
                    if(touch.phase == TouchPhase.Moved)
                    {
                        if(!dragTouchs.ContainsKey(touch.fingerId))
                            continue;
                        if(dragTouchs[touch.fingerId].y > touch.position.y)
                            checkMoveFinger++;
                    }
                }
                if(checkMoveFinger > 3)
                {
                    StartCoroutine(SpawnHeapOfMoney());
                    dragTouchs.Clear();
                }
            }
            else if(Input.touchCount == 1)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
                    currentInput = InputState.Hold;
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    currentInput = InputState.Down;
                    startPos = Input.GetTouch(0).position;
                }
            }
        }else
            currentInput = InputState.Up;
#else
        if (Input.GetMouseButtonDown(0))
        {
            currentInput = InputState.Down;
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            currentInput = InputState.Hold;
        }
        if (!Input.GetMouseButton(0))
        {
            currentInput = InputState.Up;
        }
#endif
        if (currentInput == InputState.Down)
        {
            dragTime = 0;
			cacheTrail = CloneTrail();
        }
        if (currentInput == InputState.Hold)
        {
            dragTime += Time.deltaTime;
        }
        if (dragTime > 0 && currentInput == InputState.Up)
        {
            endPos = Input.mousePosition;
            Vector3 targetDir = startPos - endPos;
            float angle = Vector3.Angle(targetDir, Vector3.left);
            float distance = targetDir.magnitude / Screen.width;
            ThrowMoney(angle, distance);
            SoundManager.inst.PlaySFXOneShot(5);
			cacheTrail = null;
            dragTime = 0;
        }

		//trail renderer
		if(Input.GetMouseButton(0) && dragTime < trailDragDuration)
		{
			if(cacheTrail)
			{
				Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				touchPos.z = -1.96f; //hardcoded trail z pos
				cacheTrail.transform.position = touchPos;
			}
		}



        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SpawnHeapOfMoney());
        }
	}

	Transform CloneTrail()
	{
		var clone = Instantiate(trailPrefab,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity,null);
		return clone;
	}

    IEnumerator SpawnHeapOfMoney()
    {
        Vector3 tmpPos;
        for(int i = 0 ;i <= 100; i++)
        {
            tmpPos = new Vector3( Random.Range(0,Screen.width),Screen.height , 0);
            tmpPos = Camera.main.ScreenToWorldPoint(tmpPos);
            tmpPos.z = 0;
            tmpCash = PoolManager.Inst.CreateCash(tmpPos);
            yield return 0;
        }
    }

    void ThrowMoney(float angle,float distance)
    {
        tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        float throwForce = (force / dragTime) * (distance * distanceFactor) * GameConfiguration.GetInstance().mouseSensivity;
        tmpCash.GetComponent<Rigidbody>().AddForce(dir * throwForce);
        tmpCash.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1000,1000), Random.Range(-1000,1000), Random.Range(-1000,1000)));
        SoundManager.inst.PlaySFXOneShot(5);
    }
}
