using UnityEngine;
using System.Collections;


// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;






public class GameEngine_Old : MonoBehaviour {


    // Project wild settings

    public static bool  _Debug_Mode         = false;
    public static bool  _Hero_Mode          = false;
    public static bool  _Platform_Google    = true;
    public static bool  _Platform_Amazon    = false;
    public static bool  _Platform_iOS       = false;
    public static int   _Control_Scheme     = 2; // 1: Touch; 2: Keyboard + Mouse; 3: Controller


    // Game Points Consumption
    public static int   _Cost_EMP = 500;

    // Game Settings
    public static GameEngine_Old    _Instance;
    public static int           _Points;
    public static bool          _SFX;
    public static bool          _VFX;


    public GameObject           _Player_Object;
    public MPlayer_Controller   _Player_Script;
    public GameObject           _Environment;



    [Header("UI/Score")]
    public CanvasGroup          _UI_Points_Earned_Consumed;
    public Text                 _UI_Points_Earned_Consumed_Amount;
    public Text                 _UI_Txt_GamePoints;
    public Text                 _UI_Txt_Power_A_Num; // Display how many times can player use this power
    public Text                 _UI_Txt_Power_B_Num; // Display how many times can player use this power
    [HideInInspector]
    public int                  _Power_A_Points;
    [HideInInspector]
    public int                  _Power_B_Points;


    [Header("UI/Game")]
    public GameObject           _UI_Panel_Continue;
    public GameObject           _UI_Panel_GameOver;
    public GameObject           _UI_Panel_GamePause;
    public GameObject           _UI_Panel_GameStart;
    public GameObject           _UI_Panel_GameFinished;


    [Header("UI/Controls")]
    public GameObject[]         _UI_TouchControls;




    [Header("SFX")]
    public AudioClip            _SFX_GameOver;
    public AudioClip            _SFX_GameUIClicked;
    public AudioClip            _SFX_GameFinished;


    [Header("Settings")]
    public GameObject           _Settings_VFX_On;
    public GameObject           _Settings_VFX_Off;
    public GameObject           _Settings_SFX_On;
    public GameObject           _Settings_SFX_Off;










    void Awake()
    {
        _Instance = this;
    }
	// Use this for initialization
	void Start () {
	

        if (_Debug_Mode)
        {
            // PlayerPrefs.SetInt("Game Points", 700);
            _SFX = true;
        }

        // Auto Select Input Scheme
#if UNITY_ANDROID
        _Control_Scheme = 1;
#endif

#if UNITY_EDITOR
        _Control_Scheme = 2;
#endif

        LoadingPlayerPrefs();
        API_Settings_Load();

        // Init
        _UI_Points_Earned_Consumed.gameObject.SetActive( false );
        API_SwitchControls(_Control_Scheme);
        _Power_A_Points = 200;
        _Power_B_Points = 500;

	}
	// Update is called once per frame
	void Update () {
	
        if( _Debug_Mode)
        {
            if( Input.GetKeyDown(KeyCode.Space) )
                Time.timeScale = Time.timeScale > .5f ? 0 : 1f;
            if( Input.GetKeyDown(KeyCode.P) )
                PlayerPrefs.DeleteAll();
        }

        // General Commands
        if( Input.GetKeyDown(KeyCode.Escape))
        {
            if( _UI_Panel_GamePause.activeInHierarchy )
                API_HidePauseMenu();
        }


	}













    // PlayerPrefs
    void LoadingPlayerPrefs()
    {
        _Points = PlayerPrefs.GetInt("Game Points");
        _UI_Txt_GamePoints.text = _Points.ToString();
    }










    // APIs
    public void API_GamePointsEarned(int amount)
    {
        _Points += amount;
        _UI_Txt_GamePoints.text = _Points.ToString();
        PlayerPrefs.SetInt("Game Points", _Points);
    }
    public void API_GamePointsConsumed(int amount)
    {
        _Points -= amount;
        _UI_Txt_GamePoints.text = _Points.ToString();
        PlayerPrefs.SetInt("Game Points", _Points);
    }
    public void API_DisplayPointsEarnOrConsumed(bool action, int amount)
    {
        // Debug.Log("+:" + amount.ToString());
        _UI_Points_Earned_Consumed.alpha = 1f;
        _UI_Points_Earned_Consumed_Amount.text = (action ? "+" : "-") + amount.ToString();
        _UI_Points_Earned_Consumed.gameObject.SetActive( true );
        _UI_Points_Earned_Consumed.gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        _UI_Points_Earned_Consumed.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), .5f);
        Invoke("FadeOut", 1f);
    }
    void FadeOut()
    {
        Utilities._Instance.CloseWindowFadeOut(_UI_Points_Earned_Consumed);
    }
    public void API_IncreaseScore(int points)
    {
        API_GamePointsEarned( points );
        LevelManagement._Instance._Phase_UnitsDown++;
        API_UpdatePowerDisplay();
    }
    public void API_UpdatePowerDisplay()
    {
        // Debug.Log("Power Num():" + _Points / _Power_A_Points + "[" + _Power_B_Points + "]");
        _UI_Txt_Power_A_Num.text = ( _Points / _Power_A_Points ).ToString();
        _UI_Txt_Power_B_Num.text = ( _Points / _Power_B_Points ).ToString();
    }
    public bool API_PowerAvailable( int index)
    {
        if( index == 1)
        {
            if( _Points >= _Power_A_Points )
                return true;
        }
        if( index == 2)
        {
            if( _Points >= _Power_B_Points )
                return true;
        }
        return false;
    }
    public void API_ShowPauseMenu()
    {
        _UI_Panel_GamePause.SetActive( true );
        // TODO
        // Need better solution for this
        Time.timeScale  = 0;
    }
    public void API_HidePauseMenu()
    {
        _UI_Panel_GamePause.SetActive( false );
        Time.timeScale  = 1f;
    }
    public void API_LoadLevel(string lvlName)
    {
        Application.LoadLevel(lvlName);
    }
    public void API_RestartLevel()
    {
        // Resume the time, if paused
        if( Time.timeScale < .1f )
            Time.timeScale = 1f;
        // Clear the remaining units
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Enemy_Tiny");
        for( int i = 0; i < objs.Length; i++ )
            Destroy( objs[i] );
        Utilities.ReloadCurrentActiveScene();
    }
    public void API_StartingLevel()
    {
        _UI_Panel_GameStart.SetActive( false );
        Time.timeScale = 1f;
    }
    public void API_Quit()
    {
        Application.Quit();
    }
    public void API_GameOver()
    {
        // Show Panel
        Invoke("ShowGameOverSound", 1f);
        Invoke("ShowGameOverUI", 2f);
    }
    void ShowGameOverUI()
    {
        _UI_Panel_GameOver.SetActive( true );
    }
    void ShowGameOverSound()
    {
        if( _SFX )
            AudioSource.PlayClipAtPoint( _SFX_GameOver, Camera.main.gameObject.transform.position );
    }
    public void API_GameFinished()
    {
        if( _SFX )
            AudioSource.PlayClipAtPoint(_SFX_GameFinished, Camera.main.gameObject.transform.position);
        _UI_Panel_GameFinished.SetActive( true );
    }
    public void API_SwitchControls(int index)
    {
        _Control_Scheme = index;
        if( _Control_Scheme != 1)
        {
            foreach ( GameObject ui in _UI_TouchControls )
            {
                ui.SetActive( false );
            }
        }
    }
    public void API_SFX_GameUIClicked()
    {
        if( _SFX )
            AudioSource.PlayClipAtPoint( _SFX_GameUIClicked, Camera.main.gameObject.transform.position );
    }








    // Settings
    public void API_Settings_Load()
    {
        // First time playing
        if( PlayerPrefs.GetInt("Game VFX") == 0 )
            PlayerPrefs.SetInt("Game VFX", 1); // default, no bloom effect
        if( PlayerPrefs.GetInt("Game SFX") == 0 )
            PlayerPrefs.SetInt("Game SFX", 2);
        // 
        _VFX = false;
        _SFX = false;
        if (PlayerPrefs.GetInt("Game VFX") == 2)
        {
            _VFX = true;
        }
        else
        {
            Camera.main.gameObject.GetComponent< UnityStandardAssets.ImageEffects.BloomOptimized >().enabled = false;
        }
        if (PlayerPrefs.GetInt("Game SFX") == 2)
        {
            _SFX = true;
        }
    }
    public void API_Settings_RefreshUI()
    {
        _Settings_VFX_On.SetActive( false );
        _Settings_SFX_On.SetActive( false );
        if( _VFX )
            _Settings_VFX_On.SetActive( true );
        if( _SFX )
            _Settings_SFX_On.SetActive( true );
    }
    public void API_Settings_Switch_VFX()
    {   
        _VFX = !_VFX;
        API_Settings_RefreshUI();
        if (_VFX)
        {
            PlayerPrefs.SetInt("Game VFX", 2);
            Camera.main.gameObject.GetComponent< UnityStandardAssets.ImageEffects.BloomOptimized >().enabled = true;
        }
        else
        {
            PlayerPrefs.SetInt("Game VFX", 1);
            Camera.main.gameObject.GetComponent< UnityStandardAssets.ImageEffects.BloomOptimized >().enabled = false;
        }
    }
    public void API_Settings_Switch_SFX()
    {
        _SFX = !_SFX;
        API_Settings_RefreshUI();
        if (_SFX)
            PlayerPrefs.SetInt("Game SFX", 2);
        else
        {
            PlayerPrefs.SetInt("Game SFX", 1);
            foreach( AudioSource audio in FindObjectsOfType(typeof(AudioSource)))
            {
                if( audio.isPlaying )
                    audio.Stop();
            }
        }
    }





}
