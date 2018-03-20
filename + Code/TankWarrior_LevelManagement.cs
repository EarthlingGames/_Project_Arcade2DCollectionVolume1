using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


public class TankWarrior_LevelManagement : MonoBehaviour {




    public static TankWarrior_LevelManagement   _Instance;
    public Camera       _Cam;


    [HideInInspector]
    public int          _UnitsCount;
    public Transform[]  _SpawnLocs;
    public Transform[]  _SpawnLocsHuge;
    public int          _WaveIndex;


    public GameObject   _Tank_AI;
    public GameObject   _Tank_T1;
    public GameObject   _Tank_T2;
    public GameObject   _Tank_T3;
    public GameObject   _Tank_T4;
    public GameObject   _Tank_T5;
    public GameObject   _Tank_T6;
    public GameObject   _Tank_T7;
    public GameObject   _Tank_T8;
    

    [Header("SFX")]
    public AudioClip    _SFX_StarterMusic;
    public AudioSource  _SFX_Level_Looper;
    public AudioClip[]  _SFX_LooperClips;
    int                 _SFX_LooperIndex;
    public AudioClip    _SFX_WaveIndicator;

    [Header("UI")]
    public CanvasGroup  _UI_WaveIndicator;
    public Text         _UI_WaveIndicator_Txt;











    void Awake()
    {
        _UnitsCount     = 0;
        _Instance       = this;
    }
	// Use this for initialization
	void Start () {
	
        // SFX
        InvokeRepeating( "SwitchingLoopingMusic", 0, 35f );
        if( GameEngine_Old._SFX )
            AudioSource.PlayClipAtPoint(_SFX_StarterMusic, _Cam.gameObject.transform.position);

        InvokeRepeating("CheckingSpawnCondition", 6f, 5f);

	}
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        // SpawnUnits();
        
    }







    void SwitchingLoopingMusic()
    {
        Utilities._Instance.API_SFX_SmoothlyPlayNextClip( _SFX_LooperClips[_SFX_LooperIndex], 10f, _SFX_Level_Looper );
        _SFX_LooperIndex++;
        if( _SFX_LooperIndex >= _SFX_LooperClips.Length )
            _SFX_LooperIndex = 0;
    }
    public void API_LevelMusic( bool action = true )
    {
        if( action )
        {
            if( GameEngine_Old._SFX)
            {
                if( !_SFX_Level_Looper.isPlaying )
                    _SFX_Level_Looper.Play();
            }
        }
        else
        {
            if ( _SFX_Level_Looper.isPlaying )
                _SFX_Level_Looper.Stop();
        }
    }
    public void API_CameraShake( float duration )
    {
        if( !_IsInCameraShaking )
            StartCoroutine( CameraShaking(duration) );
    }
    bool _IsInCameraShaking = false;
    IEnumerator CameraShaking( float duration ) {
        _IsInCameraShaking = true;
        _Cam.gameObject.GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = true;
        yield return new WaitForSeconds(duration);
        _Cam.gameObject.GetComponent<CameraFilterPack_FX_EarthQuake>().enabled = false;
        _IsInCameraShaking = false;
    }









    void CheckingSpawnCondition()
    {
        if( _WaveIndex == 0 || _UnitsCount > 0 )
            return;

        // TODO:: Stars
        if (_WaveIndex == 2)
        {
            if( PlayerPrefs.GetInt( "Tank Warrior Stars") < 1 )
                PlayerPrefs.SetInt( "Tank Warrior Stars", 1 );
        }
        if (_WaveIndex == 6)
        {
            if( PlayerPrefs.GetInt( "Tank Warrior Stars") < 2 )
                PlayerPrefs.SetInt( "Tank Warrior Stars", 2 );
        }

        // TODO::
        // Game Finished
        if (_WaveIndex == 10)
        {
            // TODO:: Stars
            PlayerPrefs.SetInt( "Tank Warrior Stars", 3 );
            GameEngine_Old._Instance.API_GameFinished();
            _WaveIndex = 0;
            return;
        }

        SpawnUnits();
    }
    void SpawnUnits()
    {
        // Use this to stop spawning units
        if( _UnitsCount > 1 )
            return;

        // Show UI
        if (_WaveIndex == 9)
            _UI_WaveIndicator_Txt.text = "Elite\nSquad";
        Utilities._Instance.CloseWindowFadeInToFadeOut( _UI_WaveIndicator, 1.5f );
        if( GameEngine_Old._SFX )
            AudioSource.PlayClipAtPoint( _SFX_WaveIndicator, Camera.main.transform.position );

        for( int i = 0; i<_SpawnLocs.Length; i++)
        {
            if ( (_SpawnLocs[i].gameObject.GetComponent<SpriteRenderer>()).isVisible )
            {
                // Debug.Log("Being Seen");
            }
            else
            {
                // Debug.Log("Not Being Seen");
                GameObject ai   = (GameObject) Instantiate(_Tank_AI, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                GameObject tank = null;

                // Spawn strategy
                if (_WaveIndex == 1) {
                    tank = (GameObject)Instantiate(_Tank_T1, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 2) {
                    tank = (GameObject)Instantiate(_Tank_T2, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 3) {
                    tank = (GameObject)Instantiate(_Tank_T3, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 4) {
                    tank = (GameObject)Instantiate(_Tank_T4, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 5) {
                    tank = (GameObject)Instantiate(_Tank_T5, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 6) {
                    tank = (GameObject)Instantiate(_Tank_T6, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 7) {
                    tank = (GameObject)Instantiate(_Tank_T7, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                if (_WaveIndex == 8) {
                    tank = (GameObject)Instantiate(_Tank_T8, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                }
                // Elite Tigers
                if (_WaveIndex == 9) {
                    tank = (GameObject)Instantiate(_Tank_T6, _SpawnLocs[i].position, _SpawnLocs[i].rotation);
                    tank.GetComponent<TankWarrior_TankControl>()._FirePower         = 7;
                    tank.GetComponent<TankWarrior_TankControl>()._ForwardSpeed      = 4f;
                    tank.GetComponent<TankWarrior_TankControl>()._CannonReloadTime  = 1f;
                    tank.GetComponent<TankWarrior_TankControl>()._RotationScale     = 180f;
                }
                tank.transform.parent = ai.transform;
                ai.GetComponent<TankWarrior_AI>()._TankControl = tank.GetComponent<TankWarrior_TankControl>();
                ai.GetComponent<TankWarrior_AI>()._Alive = true;
                ai.SetActive( true );
                _UnitsCount++;
            }
        }
        // Increase Phase
        _WaveIndex++;
    }







}
