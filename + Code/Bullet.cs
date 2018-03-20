using UnityEngine;
using System.Collections;
using PathologicalGames;
using SpFLib;



public class Bullet : MonoBehaviour {





    public float    _Damage;
    public float    _Speed;
    public float    _LifeTime;

    private float   _Timer;
    Rigidbody2D     _Rig;






    void Awake()
    {
        _Rig = gameObject.GetComponent<Rigidbody2D>();
    }
	// Use this for initialization
	void Start () {
     
        GetComponent<AudioSource>().pitch = Random.Range(.8f, 1.2f);
        if( !GameEngine_Old._SFX )   
            GetComponent<AudioSource>().enabled = false;

	}
    void FixedUpdate()
    {
        _Timer += Time.deltaTime;
        if( _Timer > _LifeTime)
        {
            _Rig.isKinematic = true; // reset the momentum  to zero
            PoolManager.Pools["PM"].Despawn( transform );
        }
    }
    void OnEnable() {

        _Timer = 0;
        _Rig.isKinematic = false;
        Vector2 forceVector = new Vector2( -transform.right.x, -transform.right.y);
        _Rig.AddForce( forceVector * _Speed  );

    }





    void OnTriggerEnter2D(Collider2D other) {


        if( other.tag == "Player" )
        {
            _Rig.isKinematic = true; // reset the momentum  to zero
            master.InvocateBulletExplotions( this.gameObject, 0.2f, GameEngine_Old._Instance._Environment );
            // VFX + SFX
            GameEngine_Old._Instance._Player_Script.API_UnitDown();
            PoolManager.Pools["PM"].Despawn( transform );
        }


	}





}
