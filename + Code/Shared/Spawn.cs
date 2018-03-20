using UnityEngine;
using System.Collections;



using PathologicalGames;











public class Spawn : MonoBehaviour {




    public GameObject   _Unit;
    public float        _Rate;
    public float        _Duration;
    [HideInInspector]
    public float        _Time;
    [HideInInspector]
    public float        _TimeTotal;
    [HideInInspector]
    public bool         _Start;
    public Transform[]  _Pos;







	// Use this for initialization
	void Start () {
	    _Start = false;
	}
	
	// Update is called once per frame
	void Update () {
	
        if( _Start )
        {
            _Time       += Time.deltaTime;
            _TimeTotal  += Time.deltaTime;
            if( _Time > _Rate )
            {
                _Time = 0;
                Transform u = PoolManager.Pools["PM"].Spawn(_Unit, _Pos[Random.Range(0, _Pos.Length)].position, _Pos[Random.Range(0, _Pos.Length)].rotation);
                u.gameObject.name = _Unit.name;
                u.parent = null;
            }
            if( _TimeTotal > _Duration )
            {
                _Start = false;
                Destroy(gameObject);
            }
        }

	}





    void OnTriggerEnter(Collider other) {

        if (_Start)
            return;

        if (other.gameObject.tag == "[Player]")
        {
            Debug.Log(other.gameObject.name+" triggered");
            _Start = true;
        }

    }







}
