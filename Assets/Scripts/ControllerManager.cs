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

    private Cash tmpCash;
    private Rigidbody tmpRb;

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
    public Transform guidelineGroup;
    public int pointsOnGuideline;  // the number of points on guideline curve
    private Vector3[] guidelineCoordinates;

    public float guidelineMinConfig = 0.3f;
    public float guidelineMaxConfig = 0.7f;

    public float timeToLive = 1f; // in seconds
    
    private float intervalTime;

    private Dictionary<int,Vector3> dragTouchs;

    private Vector3 startMousePos, endMousePos;
    private Vector3 startPos;
    private Vector3 currentPos;
    private Vector3 startToCurrent;

    private float gravity = 9.81f;

    private GameObject plane;

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
        plane = GameObject.Find("Plane");
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

#endif

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SpawnHeapOfMoney());
        }

        if (currentInput == InputState.Down)
        {
            startMousePos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(startMousePos);
        }

        currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startToCurrent = currentPos - startPos;

        if (!drawn && currentInput == InputState.Down)
        {
            drawn = true;
        }

        if (drawn && startToCurrent.magnitude > 0f)
        {
            if (currentInput == InputState.Hold)
            {
                float angle = Vector3.Angle(startToCurrent, Vector3.right);   // you cannot throw the money downward
                float speed = startToCurrent.magnitude * speedFactor;

                guidelineCoordinates = ParabolaCurve.GetCoordinates(throwSpawnPos.position, plane.transform.position.y,
                                                            angle, speed, cashMass, myDrag, pointsOnGuideline);
                DrawCurve(guidelineCoordinates);
            }
            else if (currentInput == InputState.Up)
            {
                float angle = Vector3.Angle(startToCurrent, Vector3.right);   // you cannot throw the money downward
                float speed = startToCurrent.magnitude * speedFactor;

                ThrowMoney(angle, speed);

                SoundManager.inst.PlaySFXOneShot(5);

                drawn = false;

                if (guidelineGroup.childCount > 0)
                {
                    for (int i = 0; i < guidelineGroup.childCount; i++)
                    {
                        Destroy(guidelineGroup.GetChild(i).gameObject, timeToLive);
                    }
                }
            }
        }
    }

	void FixedUpdate () 
	{
        if (GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.TitleThrow)
        {
            intervalTime += Time.deltaTime;
            if (intervalTime >= 0.2f)
            {
                // dragTime = Time.deltaTime * Random.Range(6, 7); 
                ThrowMoneyAuto(Random.Range(20, 60), Random.Range(30, 50) / 5f);
                intervalTime = 0;
            }
            dragTime = 0;
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
            PoolManager.Inst.CreateCash(tmpPos);
            yield return 0;
        }
    }

    void ThrowMoney(float angle, float speed)
    {
        tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
        tmpRb = tmpCash.GetComponent<Rigidbody>();

        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        float throwForce = tmpRb.mass * speed / Time.fixedDeltaTime * GameConfiguration.GetInstance().mouseSensitivity;
        tmpRb.AddForce(dir * throwForce, ForceMode.Force);
        tmpRb.AddTorque(new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)));
        SoundManager.inst.PlaySFXOneShot(5);
    }

	//force mouse sensitivity = 1
	void ThrowMoneyAuto(float angle,float speed)
	{
		tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
        tmpRb = tmpCash.GetComponent<Rigidbody>();

		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		float throwForce = (tmpRb.mass / Time.fixedDeltaTime) * (speed * speedFactor) * 1;
		tmpRb.AddForce(dir * throwForce);
		tmpRb.AddTorque(new Vector3(Random.Range(-1000,1000), Random.Range(-1000,1000), Random.Range(-1000,1000)));
		SoundManager.inst.PlaySFXOneShot(5);
	}
    
    private void DrawCurve(Vector3[] parabolaCoordinates)
    {
        // Destroy existing curve first
        if (guidelineGroup.childCount > 0)
        {
            for (int i = 0; i < guidelineGroup.childCount; i++)
            {
                Destroy(guidelineGroup.GetChild(i).gameObject);
            }
        }

        LineRenderer cachedLine;
        float minC = guidelineMinConfig * parabolaCoordinates.Length;
        float maxC = guidelineMaxConfig * parabolaCoordinates.Length;

        for (int i = 1; i < (int)(parabolaCoordinates.Length); i++)
        {
            cachedLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, guidelineGroup);

            cachedLine.SetPosition(0, parabolaCoordinates[i - 1]);
            cachedLine.SetPosition(1, parabolaCoordinates[i]);

            if (i >= minC)
            {
                float a1 = 1f - (i - minC) / (maxC - minC);
                float a2 = 1f - (i + 1 - minC) / (maxC - minC);

                a1 = (a1 < 0) ? 0 : a1;
                a2 = (a2 < 0) ? 0 : a2;

                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(a1, 0.0f), new GradientAlphaKey(a2, 1.0f) }
                );
                cachedLine.colorGradient = gradient;
            }
        }

    }
}
