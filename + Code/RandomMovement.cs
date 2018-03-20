using UnityEngine;
using System.Collections;
using DG.Tweening;





public class RandomMovement : MonoBehaviour {


    public float    _Delay;
    public float    _Offset;



	// Use this for initialization
	void Start () {
	    InvokeRepeating("Move", _Delay, 1f);
	}






    void Move()
    {

        // transform.DOMove(
        transform.DOLocalMove(
            new Vector3( transform.position.x + Random.Range(-_Offset, _Offset/2), transform.position.y + Random.Range(-_Offset, _Offset), 0 ),
            5.9f, false);
        

    }

}
