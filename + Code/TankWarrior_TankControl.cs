using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


// The General Control Script for any Tank unit






public class TankWarrior_TankControl : MonoBehaviour {


    public string                   _Name;

    public int                      _HP_Max;
    [HideInInspector]
    public EnergyBar                _HP_Bar;
    [HideInInspector]
    public int                      _HP;

    public float                    _ForwardSpeed;
    [Tooltip("Degree per Sec")]
    public float                    _RotationScale;

    public float                    _CannonReloadTime;
    public int                      _FirePower;

    public int                      _Armor_Front;
    public int                      _Armor_Rear;
    public int                      _Armor_Left;
    public int                      _Armor_Right;



    public bool                     _Tank_GTA_Mode; // if true, this tank can be take by the driver
    public int                      _Tank_Index;
    public TransformConstraint[]    _ArmorSet;


    public GameObject               _VFX_Muzzle;
    public GameObject               _VFX_Destroyed;
    public GameObject               _VFX_AfterBurned;
    public GameObject               _Ammo;
    public GameObject               _AmmoEMP;
    public Transform                _Gun_Pivot;
    public DR_LookAt                _Gun_LookAt;

    public GameObject               _2D_Selected;


    public GameObject               _Eye;



    [Header("SFX")]
    public GameObject               _SFX_Expo;
    public GameObject               _SFX_Fire;
    public GameObject               _SFX_Impact;
    [HideInInspector]
    public AudioSource              _SFX_Engine;



	// Use this for initialization
	void Start () {
	
        InitTank();

	}
    // Update is called once per frame
    void FixedUpdate()
    {




	}
    void InitTank()
    {
        _Gun_LookAt._Target = null;
        _HP                 = _HP_Max;
        _2D_Selected.SetActive( false );
        _SFX_Engine = GetComponent<AudioSource>();
        foreach( TransformConstraint con in _ArmorSet )
            con.target = Camera.main.gameObject.transform;
    }






    // APIs
    public void Fire()
    {
        // SFX
        if (GameEngine_Old._SFX)
        {
            Transform clip = PoolManager.Pools["PM"].Spawn(_SFX_Fire, _Gun_Pivot.position, _VFX_Muzzle.transform.rotation, null);
            clip.gameObject.GetComponent<AudioSource>().pitch = Random.Range(.8f, 1.2f);
        }

        TankWarrior_LevelManagement._Instance.API_CameraShake(.2f);
        PoolManager.Pools["PM"].Spawn( _VFX_Muzzle, _Gun_Pivot.position, _VFX_Muzzle.transform.rotation, null );
        Transform ammo = PoolManager.Pools["PM"].Spawn( _Ammo, _Gun_Pivot.position, _Gun_Pivot.rotation, null );
        (ammo.gameObject.GetComponent<TankWarrior_Ammo>())._Damage = _FirePower;

    }
    public void FireEMP()
    {
        TankWarrior_LevelManagement._Instance.API_CameraShake(.2f);
        // PoolManager.Pools["PM"].Spawn( _VFX_Muzzle, _Gun_Pivot.position, _VFX_Muzzle.transform.rotation, null );
        Transform ammo = PoolManager.Pools["PM"].Spawn( _AmmoEMP, _Gun_Pivot.position, _Gun_Pivot.rotation, null );
    }
    public void BeingHit( int damage, string hittingColliderName )
    {
        if( _HP < 0 )
            return;

        // TODO::
        if( GameEngine_Old._Hero_Mode )
            damage = 9999;

        // SFX
        if (GameEngine_Old._SFX)
        {
            Transform clip = PoolManager.Pools["PM"].Spawn(_SFX_Impact, transform.position, transform.rotation, null);
            clip.gameObject.GetComponent<AudioSource>().volume  = Random.Range(.65f, 1.0f);
            clip.gameObject.GetComponent<AudioSource>().pitch   = Random.Range(.72f, 0.9f);
        }

        if( hittingColliderName.Contains("Front"))
            damage = damage - _Armor_Front;

        if( hittingColliderName.Contains("Rear"))
            damage = damage - _Armor_Rear;

        if( hittingColliderName.Contains("Left"))
            damage = damage - _Armor_Left;

        if( hittingColliderName.Contains("Right"))
            damage = damage - _Armor_Right;

        // Ammo fire power less than tank armor, no damage dealt
        if( damage <= 0 )
            return;

        _HP -= damage;

        // Reflect the damage to GUI
        TankWarrior_Player._Instance.API_Refresh_HP_Bar();
        //if( _HP_Bar != null )
        //    _HP_Bar.valueCurrent = _HP;
        //Debug.Log("HP" + _HP.ToString());


        if( _HP <= 0)
        {

            // _HP_Bar.gameObject.SetActive(false);
            PoolManager.Pools["PM"].Spawn( _VFX_Destroyed,      transform.position, transform.rotation, null );
            PoolManager.Pools["PM"].Spawn( _VFX_AfterBurned,    transform.position, _VFX_AfterBurned.transform.rotation, null );

            // 
            if( transform.root.GetComponent<TankWarrior_AI>() != null)
            {
                // Drop something
                if( Random.value < .99f )
                    Instantiate( TankWarrior_Player._Instance._VFX_HP_Drop, transform.position, TankWarrior_Player._Instance._VFX_HP_Drop.transform.rotation );

                // Add Points to the game total score
                GameEngine_Old._Instance.API_DisplayPointsEarnOrConsumed( true, _Tank_Index * 5 );
                GameEngine_Old._Instance.API_GamePointsEarned( _Tank_Index * 5 );
                // 1) Tell Player
                TankWarrior_Player._Instance.API_RankingUp();
                // 2) Tell level control
                TankWarrior_LevelManagement._Instance._UnitsCount--;
                if( GameEngine_Old._SFX )
                    PoolManager.Pools["PM"].Spawn( _SFX_Expo, transform.position, transform.rotation, null );
                Destroy(transform.parent.gameObject);
            }
            else
            {
                // Player's tank is destroyed, which is the same as driver being killed, which will end the game
                gameObject.transform.root.gameObject.SendMessage("API_DriverBeingHit");
                gameObject.SetActive( false );
            }

        }


    }






    public void API_EngineSounOn()
    {
        if( GameEngine_Old._SFX )
        {
            if( transform.parent.gameObject.GetComponent<TankWarrior_AI>() != null )
                _SFX_Engine.volume = .2f;
            if( transform.root.gameObject.GetComponent<TankWarrior_Player>() != null )
                _SFX_Engine.volume = .32f;
            if( !_SFX_Engine.isPlaying )
                _SFX_Engine.PlayDelayed(Random.Range(0, .5f));
        }
    }
    public void API_EngineSounOff()
    {
        _SFX_Engine.Stop();
    }




}
