using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


public class TankWarrior_Ammo : MonoBehaviour {








    [HideInInspector]
    public int      _Damage; // assigned by the Tank Control
    public float    _Speed;
    public float    _LifeTime;

    private float   _Timer;
    

    public GameObject   _VFX_Hit_Wall;
    public GameObject   _VFX_Hit_DamageDealt;




    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {
        
	}
    void FixedUpdate()
    {
        _Timer += Time.deltaTime;
        if( _Timer > _LifeTime)
        {
            PoolManager.Pools["PM"].Despawn( transform );
            return;
        }
        transform.Translate( transform.up * Time.deltaTime * _Speed, Space.World );
    }
    void OnEnable() {

        _Timer = 0;

    }





    void OnTriggerEnter2D(Collider2D other) {


        if( other.tag == "Border")
        {
            // VFX + SFX
            PoolManager.Pools["PM"].Spawn( _VFX_Hit_Wall, transform.position, _VFX_Hit_Wall.transform.rotation, null );
            PoolManager.Pools["PM"].Despawn( transform );
            return;
        }


        if( other.tag == "Armor" )
        {
            GameObject  root = other.transform.root.gameObject;

            // Hitting AI
            if( root.name.Contains("AI"))
            {
                TankWarrior_AI ai = root.GetComponent<TankWarrior_AI>();
                TankWarrior_TankControl tankControl = ai._TankControl;
                if (gameObject.name.Contains("EMP"))
                {
                    if( tankControl._HP <= 3 ) 
                        ai.BeingHitByEMP();
                }
                else
                    tankControl.BeingHit(_Damage, other.gameObject.name);
            }

            // Hitting Player
            if( root.name.Contains("Player"))
            {
                TankWarrior_Player player = root.GetComponent<TankWarrior_Player>();
                if (player._Player_Mode == 1)
                {
                    player.API_DriverBeingHit();
                }
                if (player._Player_Mode == 2)
                {
                    TankWarrior_TankControl tankControl = player._Tank_Control;
                    tankControl.BeingHit(_Damage, other.gameObject.name);
                }
            }

            // VFX + SFX
            if( _VFX_Hit_DamageDealt != null ) // used for normal shell
                PoolManager.Pools["PM"].Spawn( _VFX_Hit_DamageDealt, transform.position, _VFX_Hit_Wall.transform.rotation, null );
            else // used for EMP
                PoolManager.Pools["PM"].Spawn( _VFX_Hit_Wall, transform.position, _VFX_Hit_Wall.transform.rotation, null );

            PoolManager.Pools["PM"].Despawn( transform );
        }


	}





}
