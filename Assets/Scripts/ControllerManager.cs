using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    private Vector3 startPos,endPos;

    [SerializeField]
    private Transform throwSpawnPos;

    private Transform tmpCash;

    private float dragTime;

    public int force;
    public int distanceFactor;

	[Header("Trail Config")]
	[SerializeField]
	private Transform trailPrefab;
	[SerializeField]
	private float trailDragDuration;
	private Transform cacheTrail;

	void Start () 
    {
        
	}
	
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 startDraw = Camera.main.ScreenToWorldPoint(startPos + new Vector3(0, 0, 100));
        Vector3 endDraw = Camera.main.ScreenToWorldPoint(endPos + new Vector3(0, 0, 100));
        Gizmos.DrawLine(startDraw, endDraw);
    }

	// Update is called once per frame
	void Update () 
    {
		
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
            float distance = targetDir.magnitude;
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            tmpCash.GetComponent<Rigidbody>().AddForce(dir * ( (force / dragTime) * (distance * distanceFactor )));

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
	}

	Transform CloneTrail()
	{
		var clone = Instantiate(trailPrefab,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity,null);
		return clone;
	}
}
