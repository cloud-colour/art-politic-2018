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
    
    [SerializeField]
    private Transform throwSpawnPos;

    private Transform tmpCash;
    private float dragTime;
    public float force;
    public float speedFactor;
    public float myDrag = 0.4f; // hard-coded drag
    public float cashMass = 5f;

    private InputState currentInput;
    [SerializeField]
    private bool drawn = false;

    [Header("Line Config")]
	public LineRenderer linePrefab;
	private LineRenderer cachedLine;
    public Transform guideLineGroup;
    public int pointsOnGuideline;  // the number of points on guideline curve
    private Vector3[] guidelineCoordinates;

    public float guidelineRatio = 1f;

    private float intervalTime;

    private Dictionary<int,Vector3> dragTouchs;

    private Vector3 startMousePos, endMousePos;
    private Vector3 startPos;
    private Vector3 currentPos;
    private Vector3 startToCurrent;

    float gravity = 9.81f;

    //idle check
    Vector3 lastMousePos;
	float idleTime;

    void Start () 
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
            GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.OpenSequence);

        dragTouchs = new Dictionary<int,Vector3>();         // used in mobile devices
        drawn = false;

        guidelineCoordinates = new Vector3[pointsOnGuideline];

        gravity = Physics.gravity.magnitude;
    }

	// Update is called once per frame
    void Update () 
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

        if (GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.VictoryWaitInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameSceneManager.GetInstance().GoToNextStage();
            }
            return;
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
            startMousePos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(startMousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            currentInput = InputState.Hold;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentInput = InputState.Up;
        }
        
        currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startToCurrent = currentPos - startPos;

#endif
        /*if(tmpCash != null)
        {
            var v = tmpCash.GetComponent<Rigidbody>().velocity;
            Debug.Log(v + " " + v.magnitude);
        }*/

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SpawnHeapOfMoney());
        }
	}

	void FixedUpdate () 
	{
		if(GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.TitleThrow)
		{
			intervalTime += Time.deltaTime;
			if (intervalTime >= 0.2f)
			{
				// dragTime = Time.deltaTime * Random.Range(6, 7); 
				ThrowMoneyAuto(Random.Range(20, 60), Random.Range(30, 50) / 10f);
				intervalTime = 0;
			}
			dragTime = 0;
		}

        if (!drawn && currentInput == InputState.Down)
        {
            cachedLine = Instantiate(linePrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            cachedLine.positionCount = 2;
            cachedLine.SetPosition(0, throwSpawnPos.position);
            cachedLine.SetPosition(1, throwSpawnPos.position);

            drawn = true;
        }

        if (drawn)
        {
            //Debug.Log(startToEnd.magnitude);

            if (startToCurrent.magnitude > 0f)
            {
                if (currentInput == InputState.Hold)
                {
                    cachedLine.SetPosition(1, throwSpawnPos.position + startToCurrent);

                    float angle = Vector3.Angle(startToCurrent, Vector3.right);   // you cannot throw the money downward
                    float speed = startToCurrent.magnitude * speedFactor;

                    guidelineCoordinates = ParabolaCurve.GetCoordinates(throwSpawnPos.position, GameObject.Find("Plane").transform.position.y,
                                                                angle, speed, cashMass, myDrag, pointsOnGuideline);
                    DrawCurve(guidelineCoordinates);
                }
                else if (currentInput == InputState.Up)
                {
                    currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 targetDir = currentPos - startPos;
                    float angle = Vector3.Angle(targetDir, Vector3.right);   // you cannot throw the money downward
                    float speed = startToCurrent.magnitude * speedFactor;

                    tmpCash = ThrowMoney(angle, speed);

                    SoundManager.inst.PlaySFXOneShot(5);

                    drawn = false;
                    Destroy(cachedLine.gameObject);
                    /*if (guideLineGroup.childCount > 0)
                    {
                        for (int i = 0; i < guideLineGroup.childCount; i++)
                        {
                            Destroy(guideLineGroup.GetChild(i).gameObject);
                        }
                    }*/
                }
            }
        }
    }

    IEnumerator SpawnHeapOfMoney()
    {
        Vector3 tmpPos;
        for(int i = 0 ;i <= 100; i++)
        {
            tmpPos = new Vector3(Random.Range(0, Screen.width), Screen.height, 0);
            tmpPos = Camera.main.ScreenToWorldPoint(tmpPos);
            tmpPos.z = 0;
            tmpCash = PoolManager.Inst.CreateCash(tmpPos);
            yield return 0;
        }
    }

    Transform ThrowMoney(float angle, float speed)
    {
        tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        float throwForce = tmpCash.GetComponent<Rigidbody>().mass * speed / Time.fixedDeltaTime * GameConfiguration.GetInstance().mouseSensitivity;
        tmpCash.GetComponent<Rigidbody>().AddForce(dir * throwForce, ForceMode.Force);
        tmpCash.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
        SoundManager.inst.PlaySFXOneShot(5);

        Debug.Log("dir: " + dir);
        Debug.Log("distance: " + speed);
        Debug.Log("angle: " + angle);
        Debug.Log("throw force: " + dir + " " + throwForce);

        return tmpCash;
    }

	//force mouse sensitivity = 1
	void ThrowMoneyAuto(float angle,float speed)
	{
		tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		float throwForce = (tmpCash.GetComponent<Rigidbody>().mass / Time.fixedDeltaTime) * (speed * speedFactor) * 1;
		tmpCash.GetComponent<Rigidbody>().AddForce(dir * throwForce);
		tmpCash.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1000,1000), Random.Range(-1000,1000), Random.Range(-1000,1000)));
		SoundManager.inst.PlaySFXOneShot(5);
	}
    
    private void DrawCurve(Vector3[] parabolaCoordinates)
    {
        // Destroy existing curve first
        if (guideLineGroup.childCount > 0)
        {
            for (int i = 0; i < guideLineGroup.childCount; i++)
            {
                Destroy(guideLineGroup.GetChild(i).gameObject);
            }
        }

        LineRenderer cachedLine;       

        for (int i = 1; i < (int)(guidelineRatio * parabolaCoordinates.Length); i++)
        {
            cachedLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, guideLineGroup);

            cachedLine.SetPosition(0, parabolaCoordinates[i - 1]);
            cachedLine.SetPosition(1, parabolaCoordinates[i]);
        }
    }
}
