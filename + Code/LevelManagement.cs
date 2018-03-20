using UnityEngine;
using UnityEngine.UI;
using System.Collections;




// Level progression
// Control when, where and what to spawn






public class LevelManagement : MonoBehaviour {

    public static LevelManagement   _Instance;



    [Header("UI")]
    public CanvasGroup  _UI_Panel_Boss_Status;
    public Text         _UI_Txt_Announcement;



    public BossShip     _Boss_1;
    public BossShip     _Boss_2;
    public BossShip     _Boss_3;



    public Transform    _EnemySpawnPos_A;
    public Transform    _EnemySpawnPos_B;
    public Transform    _EnemySpawnPos_C;
    [HideInInspector]
    public Vector3      _EnemySpawnPos;

    public int          _Phase_Index;
    public int          _Phase_UnitsCount; // Max number of units for this phase
    [HideInInspector]
    public int          _Phase_UnitsDown; // Current defeated units in this phase
    public float        _Spawn_Rate; // How fast do units spawn



    private float       _Timer;



    [Header("SFX")]
    public AudioSource  _Audio;
    public AudioClip    _SFX_BossFightMusic;
    int                 _SFX_LooperIndex;
    public AudioClip[]  _SFX_LooperClips;
    public AudioSource  _SFX_Level_Looper;
    public AudioClip    _SFX_Big_Expo;




    void SwitchingLoopingMusic()
    {
        if( !MPlayer_Controller._Instance._Alive)
        {
            if( _Audio.isPlaying )
                _Audio.Stop();
        }
        if( _Audio.isPlaying )
            return;

        Utilities._Instance.API_SFX_SmoothlyPlayNextClip( _SFX_LooperClips[_SFX_LooperIndex], 4f, _SFX_Level_Looper );
        _SFX_LooperIndex++;
        if( _SFX_LooperIndex >= _SFX_LooperClips.Length )
            _SFX_LooperIndex = 0;
    }












    void Awake()
    {
        _Instance = this;
    }
	// Use this for initialization
	void Start () {

        // SFX
        InvokeRepeating( "SwitchingLoopingMusic", 5, 15f );
                
        _UI_Txt_Announcement.text = "GO";
        Utilities._Instance.CloseWindowFadeInToFadeOut(_UI_Panel_Boss_Status, 4f);
        
        Invoke("Load", 1f);
	}
    void Load()
    {
        GameEngine_Old._Instance.API_UpdatePowerDisplay();
    }
	void FixedUpdate () {
	

        if( !MPlayer_Controller._Instance._Alive )
            return;

        _Timer += Time.deltaTime;



        if( _Phase_Index == 1 )
        {
            if( _Timer > _Spawn_Rate )
            {
                CreateSmallEnemy();
                if( Random.value < .1f )
                    CreateEnemy_Star();
                if( Random.value < .5f )
                    CreateSmallEnemy();
                _Timer = 0;
            }
            if( _Phase_UnitsDown >= _Phase_UnitsCount )
            {
                _Phase_UnitsDown = 0; // reset for new phase
                _Phase_Index++;
            }
            return;
        }

        if( _Phase_Index == 2 )
        {

            if ( _Timer > _Spawn_Rate)
            {
                if( Random.value < .6f )
                    CreateSmallEnemy();
                if( Random.value < .8f )
                    CreateEnemy_Star();
                if( Random.value < .1f )
                    CreateEnemy_Shell();
                _Timer = 0;
            }
            if( _Phase_UnitsDown >= _Phase_UnitsCount )
            {
                _Phase_UnitsDown = 0; // reset for new phase
                _Phase_Index++;
            }
            return;
        }

        if( _Phase_Index == 3)
        {
            // VFX + SFX
            _UI_Txt_Announcement.text = "Destroyer I";
            Utilities._Instance.CloseWindowFadeInToFadeOut(_UI_Panel_Boss_Status, 5f);
            _Boss_1.gameObject.SetActive(true);
            _Phase_Index++;

            if (GameEngine_Old._SFX)
            {
                _SFX_Level_Looper.Stop(); // stop background music
                _Audio.clip = _SFX_BossFightMusic;
                _Audio.Play();
            }
        }

        if( _Phase_Index == 4) // Empty phase, updated by destroying boss one
        {
        }

        if( _Phase_Index == 5 )
        {

            if ( _Timer > _Spawn_Rate )
            {
                if( Random.value < .8f )
                    CreateSmallEnemy();
                if( Random.value < .2f )
                    CreateEnemy_Star();
                if( Random.value < .5f )
                    CreateEnemy_Shell();
                _Timer = 0;
            }

            if( _Phase_UnitsDown >= _Phase_UnitsCount )
            {
                // TODO:: Unlocking Stars
                if (PlayerPrefs.GetInt("Space Ranger Stars") < 1)
                    PlayerPrefs.SetInt("Space Ranger Stars", 1);
                _Phase_UnitsDown = 0; // reset for new phase
                _Phase_Index++;
            }
            return;
        }

        if( _Phase_Index == 6 )
        {
            // VFX + SFX
            _UI_Txt_Announcement.text = "Destroyer II";
            Utilities._Instance.CloseWindowFadeInToFadeOut(_UI_Panel_Boss_Status, 5f);
            _Boss_2.gameObject.SetActive(true);
            _Phase_Index++;
        }

        if( _Phase_Index == 7) // Empty phase, updated by destroying boss one
        {
        }

        if( _Phase_Index == 8 )
        {

            if ( _Timer > _Spawn_Rate )
            {
                if( Random.value < .9f )
                    CreateSmallEnemy();
                if( Random.value < .2f )
                    CreateEnemy_Star();
                if( Random.value < .9f )
                    CreateEnemy_Shell();
                _Timer = 0;
            }
            if( _Phase_UnitsDown >= _Phase_UnitsCount )
            {
                _Phase_UnitsDown = 0; // reset for new phase
                _Phase_Index++;
            }
            return;
        }

        if (_Phase_Index == 9)
        {
            // TODO:: Unlocking Stars
            if (PlayerPrefs.GetInt("Space Ranger Stars") < 2)
                PlayerPrefs.SetInt("Space Ranger Stars", 2);

            // VFX + SFX
            _UI_Txt_Announcement.text = "Destroyer III";
            Utilities._Instance.CloseWindowFadeInToFadeOut(_UI_Panel_Boss_Status, 5f);
            _Boss_3.gameObject.SetActive(true);
            _Phase_Index++;
        }

        if ( _Phase_Index == 10 ) // Empty phase, updated by destroying boss one
        {
        }

        if( _Phase_Index == 11 )
        {
            if ( _Timer > _Spawn_Rate )
            {
                CreateSmallEnemy();
                CreateEnemy_Star();
                CreateEnemy_Shell();
                _Timer = 0;
            }
            if( _Phase_UnitsDown >= _Phase_UnitsCount )
            {
                _Phase_UnitsDown = 0; // reset for new phase
                _Phase_Index++;
            }
            return;
        }

        if( _Phase_Index == 12 ) 
        {
            CreateBoss();
            _Phase_Index++;
        }

        if( _Phase_Index == 13 ) // Empty, updated by boss unit
        {
        }

        if( _Phase_Index == 14 )
        {
            // TODO:: Unlocking Stars
            if (PlayerPrefs.GetInt("Space Ranger Stars") < 3)
                PlayerPrefs.SetInt("Space Ranger Stars", 3);
            _Phase_Index = 999; // game finished
            Invoke("GameFinished", 10f);
        }


	}
    void GameFinished()
    {
        GameEngine_Old._Instance.API_GameFinished();
    }







    void GetSpawnPosition()
    {
        if (Random.value < .3f)
        {
            _EnemySpawnPos = _EnemySpawnPos_A.position;
            return;
        }
        if (Random.value < .6f)
        {
            _EnemySpawnPos = _EnemySpawnPos_B.position;
            return;
        }
        if (Random.value < 1.0f)
        {
            _EnemySpawnPos = _EnemySpawnPos_C.position;
            return;
        }
    }
	private void CreateEnemy_Star(){
        GetSpawnPosition();
		GameObject enemy = Instantiate(Resources.Load("Enemies/EnemyStar"), _EnemySpawnPos, Quaternion.identity)as GameObject;
	}

	private void CreateEnemy_Shell(){
        GetSpawnPosition();
		GameObject enemy = Instantiate(Resources.Load("Enemies/Shell"), _EnemySpawnPos, Quaternion.identity)as GameObject;
	}

	private void CreateSmallEnemy(){
        GetSpawnPosition();
		GameObject enemy = Instantiate(Resources.Load("Enemies/SmallEnemy"), _EnemySpawnPos, Quaternion.identity)as GameObject;
	}

	private void CreateBoss(){
        GetSpawnPosition();
		Vector3 aux_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane));
		_EnemySpawnPos.y = aux_.y;
		GameObject enemy = Instantiate(Resources.Load("Enemies/Boss"), _EnemySpawnPos, Quaternion.identity)as GameObject;
	}














}
