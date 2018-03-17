using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    private Vector3 startPos,endPos;

    [SerializeField]
    private Transform throwSpawnPos;

    private Transform tmpCash;

    private float dragTime;

    public float force;
    public float distanceFactor;

	[Header("Trail Config")]
	[SerializeField]
	private Transform trailPrefab;
	[SerializeField]
	private float trailDragDuration;
	private Transform cacheTrail;

	void Start () 
    {
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.OpenSequence);
	}

	// Update is called once per frame
	void Update () 
    {
        if (GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.VictoryWaintInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameSceneManager.GetInstance().GoToNextStage();
            }
            return;
        }

        if (GameStateManager.GetInstance().GetGameState() != GameStateManager.GameState.GamePlay)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            tmpCash = PoolManager.Inst.CreateCash(throwSpawnPos.position);
            dragTime = 0;
            startPos = Input.mousePosition;
			cacheTrail = CloneTrail();
        }

        dragTime += Time.deltaTime;
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;

            Vector3 targetDir = startPos - endPos;
            float angle = Vector3.Angle(targetDir, Vector3.left);
            float distance = targetDir.magnitude / Screen.width;
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            tmpCash.GetComponent<Rigidbody>().AddForce(dir * ( (force / dragTime) * (distance * distanceFactor )));
            tmpCash.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1000,1000), Random.Range(-1000,1000), Random.Range(-1000,1000)));
            SoundManager.inst.PlaySFXOneShot(5);
            Debug.Log("Start : " + startPos + " End : " + endPos + " angle : "+angle);
			cacheTrail = null;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
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
}
