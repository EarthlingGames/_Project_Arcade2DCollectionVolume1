using UnityEngine;
using System.Collections;
using PathologicalGames;
using SpFLib;

public class Squad : MonoBehaviour {








    SmoothTransformConstraint _PosConstraint;


    public GameObject   _FirePivot;
    public bool         _Auto;
    [HideInInspector]
    public float        _AutoForwardSpeed;
    public Transform    _EngineSmokePos;






    void Awake()
    {
        _PosConstraint = GetComponent<SmoothTransformConstraint>();
    }
	// Use this for initialization
	void Start () {

        if( _PosConstraint != null )
	        _PosConstraint.positionSpeed = Random.Range(0.06f, 0.09f);

        if (_Auto)
        {
            _AutoForwardSpeed = Random.Range(1f, 2.5f);
            InvokeRepeating("Fire", 0, Random.Range(0.2f, 0.6f));
        }

	}
	// Update is called once per frame
	void Update () {

        if ( !_Auto && GameEngine_Old._Control_Scheme == 2 && Input.GetMouseButtonDown(0) ) {
            Fire();
        }

	}
    void FixedUpdate()
    {

        if (_Auto)
        {
            transform.Translate(transform.right * Time.deltaTime * _AutoForwardSpeed);
            if ( Random.Range(0, 3) == 1 ) {
                    // master.CreateFire( _EngineSmokePos.gameObject, GameEngine._Instance._Environment, 1f, 1f, 1f);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {

        

    }







    // Auto Squad
    public void Fire()
    {
        if( !gameObject.activeInHierarchy )
            return;

        master.CreateBullet(_FirePivot, GameEngine_Old._Instance._Environment, "Basic_A", 1000f);
    }













}
