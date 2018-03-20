using UnityEngine;
using System.Collections;




// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;







public class TankWarrior_Player : MonoBehaviour {



    public static TankWarrior_Player   _Instance;


    [Header("General Information")]
    public int          _Player_Mode; // 1: Driver Mode; 2: Tank Mode
    public bool         _Player_Defeated;
    public Transform    _TouchPoint;
    public LayerMask    _Layer_To_Click;
    public Transform    _UI_Disabled;    


    [Header("(Tank) Related")]
    public GameObject   _Tanks_UI_Btns;
    GameObject          _Tank_Current_Target;
    GameObject          _Tank_Previous_Target;
    public Button       _Tank_UI_Fire_Btn;
    public Button       _Tank_UI_FireEMP_Btn;
    public GameObject   _Tanks_Root;
    [HideInInspector]
    public int          _Tanks_Index;
    public float[]      _Tanks_ForwardSpeed;
    public float[]      _Tanks_RotationSpeed;
    [HideInInspector]
    public TankWarrior_TankControl _Tank_Control; // Control script for the current controlled tank

    int                 _Tank_Ranking_Num   = 4; // The total amount of enemy tanks to killed to be legible to upgraded
    public int          _Tank_Ranking_Kill  = 0; // Current kills; Reset after promotion
    public CanvasGroup  _Tank_UI_Promotion;
    [HideInInspector]
    public bool         _Tank_Elite;
    public GameObject   _Tank_UI_Elite_Indicator;
    public GameObject   _Tank_EMP_VFX;


    [Header("(Driver) Related")]
    public GameObject   _Driver_Root;
    public float        _Driver_Walk_Speed;
    public Animator     _Driver_Animator;


    [HideInInspector]
    public float        _Input_Horizontal;
    [HideInInspector]
    public float        _Input_Vertical;
    



    [Header("UI Status")]
    public GameObject   _UI_Opponent;
    public Text         _UI_Opponent_Name;
    public GameObject[] _UI_Opponent_FirePowerIndicator;
    public EnergyBar    _UI_Opponent_HP_Bar;
    public GameObject[] _UI_Opponent_Icons;

    public GameObject   _UI_Self;
    public Text         _UI_Self_Name;
    public GameObject[] _UI_Self_FirePowerIndicator;
    public EnergyBar    _UI_Self_HP_Bar;
    public GameObject[] _UI_Self_Icons;
    public EnergyBar    _UI_Self_ReloadingBar;
    public GameObject   _VFX_Add_HP;
    public GameObject   _VFX_HP_Drop;

    [Header("SFX")]
    [HideInInspector]
    public AudioSource  _Audio;
    public AudioClip    _SFX_NoFunds;
    public AudioClip    _SFX_GainHP;
    public AudioSource  _SFX_TankStart;
    public AudioSource  _SFX_TankExit;
    public AudioClip    _SFX_RankUp;
    public AudioClip    _SFX_ShootEMP;


    [Header("Sign")]
    public EnergyBarToolkit.EnergyBarFollowObject   _Disabled_Sign;




    void Awake()
    {
        _Instance   = this;
        _Audio      = GetComponent<AudioSource>();
    }
	// Use this for initialization
	void Start () {


        // Init
        _Tank_Current_Target    = null;
        _Tank_Previous_Target   = null;
        _Tank_Elite = false;
        _Tank_UI_Elite_Indicator.SetActive( false );
        _Tank_UI_Promotion.gameObject.SetActive( false );
        // _UI_Disabled.gameObject.SetActive( false );
        _UI_Self_ReloadingBar.gameObject.SetActive( false );
        _UI_Opponent.SetActive( false );
        _UI_Self.SetActive( false );
        _Tanks_UI_Btns.SetActive( false );

	}
	// Update is called once per frame
	void Update () {

        if( _Player_Defeated )
            return;

        // Tank Mode
        if ( _Player_Mode == 2 ) 
        {
            // Check if an enemy is selected
            if ( Input.GetMouseButtonDown(0) )
//              && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() )
            {
                // Debug.Log(Utilities._Instance.GetClickWorldPosition(Vector3.zero));
                GameObject target = Utilities._Instance.GetClickedObjectRootWithName("Click");

                // Rotate the cannon to face the target
                if (target != null)
                {

                }

            }
        }


        // Keyboard Inputs Specific
        if ( GameEngine_Old._Control_Scheme == 2)
        {
            _Input_Horizontal   = Input.GetAxis ("Horizontal");
		    _Input_Vertical     = Input.GetAxis ("Vertical");

            // Tank Mode
            if( _Player_Mode == 2 )
            {
                // Exit the Tank mode to driver mode
                if (Input.GetKeyDown(KeyCode.E))
                {
                    API_Exit_Vehicle();
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _Tank_Control.Fire();
                }
            }

        }


	}
    void FixedUpdate()
    {

        if( _Player_Defeated )
            return;

        // Hide "Disabled Sign" when AI is destroyed
        if( _Disabled_Sign.gameObject.activeInHierarchy && 
            ( _Disabled_Sign.followObject == null || !_Disabled_Sign.followObject.activeInHierarchy) )
            _Disabled_Sign.gameObject.SetActive( false );

        // TODO
        // Mode: Driver
        if (_Player_Mode == 1)
        {
            if (Mathf.Abs(_Input_Vertical) > .2f)
                _Driver_Animator.SetBool("IsWalking", true);
            else
                _Driver_Animator.SetBool("IsWalking", false);

            transform.RotateAround(transform.position, -transform.forward, _Input_Horizontal * Time.deltaTime * 300f);
            transform.Translate(_Input_Vertical * _Driver_Walk_Speed * transform.up * Time.deltaTime, Space.World);
        }


        // TODO
        // Mode: Tank
        if ( _Player_Mode == 2 )
        {
            if (_Tank_Control._Gun_LookAt._Target == null)
            {
                // Disable UI status display when not targeting
                _UI_Opponent.SetActive(false);
            }


            transform.RotateAround(transform.position, -transform.forward, _Tank_Control._RotationScale * Time.deltaTime * _Input_Horizontal );
            /*
            if (Mathf.Abs(_Input_Vertical) > .25f)
            {
                if( _Input_Vertical > 0 )
                    transform.RotateAround(transform.position, -transform.forward, _Tank_Control._RotationScale * Time.deltaTime * _Input_Horizontal );
                else
                    transform.RotateAround(transform.position, -transform.forward, _Tank_Control._RotationScale * Time.deltaTime * -_Input_Horizontal );
            }
            */

            // Forwarding
            if( _Input_Vertical > .1f )
                transform.Translate( _Input_Vertical * _Tank_Control._ForwardSpeed * transform.up * Time.deltaTime, Space.World);
            // Back
            else
                transform.Translate( _Input_Vertical * _Tank_Control._ForwardSpeed / 1.5f * transform.up * Time.deltaTime, Space.World);

            // SFX
            if( Mathf.Abs( _Input_Vertical ) > .1f )
                _Tank_Control.API_EngineSounOn();
            else
                _Tank_Control.API_EngineSounOff();
        }



        // transform.Translate(0, Time.deltaTime, 0, Space.Self);
        // Debug.Log(_Input_Horizontal);



    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if( other.tag == "Power_Up")
        {
            if( _Player_Mode == 2 )
            {
                // VFX + SFX
                if( GameEngine_Old._SFX )
                    _Audio.PlayOneShot(_SFX_GainHP);

                PoolManager.Pools["PM"].Spawn( _VFX_Add_HP, transform.position, _VFX_Add_HP.transform.rotation, null );
                _Tank_Control._HP++;

                // Add Game Points
                GameEngine_Old._Instance.API_DisplayPointsEarnOrConsumed( true, 20 );
                GameEngine_Old._Instance.API_GamePointsEarned( 20 );

                if( _Tank_Control._HP > _Tank_Control._HP_Max) 
                    _Tank_Control._HP = _Tank_Control._HP_Max;
                API_Refresh_HP_Bar();
                Destroy( other.gameObject );
            }
        }

    }

















    // API
    public void API_Fire()
    {
        API_Reloading();
        _Tank_Control.Fire();
    }
    public void API_FireEMP()
    {

        // checking points available
        if (GameEngine_Old._Points < GameEngine_Old._Cost_EMP )
        {
            if (GameEngine_Old._SFX)
                _Audio.PlayOneShot(_SFX_NoFunds, 1f);
            return;
        }

        if (GameEngine_Old._SFX)
            _Audio.PlayOneShot(_SFX_ShootEMP, 1f);

        PoolManager.Pools["PM"].Spawn( _Tank_EMP_VFX, _Tank_Control._Gun_Pivot.position, _Tank_EMP_VFX.transform.rotation, null );

        API_Reloading();
        _Tank_Control.FireEMP();
        GameEngine_Old._Instance.API_DisplayPointsEarnOrConsumed(false, GameEngine_Old._Cost_EMP);
        GameEngine_Old._Instance.API_GamePointsConsumed(GameEngine_Old._Cost_EMP);
    }
    public void API_Exit_Vehicle()
    {
        _Player_Mode = 1;
        // _Tanks_Root[_Tanks_Index].SetActive(false);
        _Driver_Root.SetActive(true);
        _UI_Self.SetActive(false);
        _Tanks_UI_Btns.SetActive(false);
        Destroy(_Tank_Control.gameObject);
        // Drop a HP pack
        Instantiate( _VFX_HP_Drop, transform.position, _VFX_HP_Drop.transform.rotation );
        // SFX
        if (GameEngine_Old._SFX)
            _SFX_TankExit.Play();
    }
    public void API_Refresh_HP_Bar()
    {
        // Update Status
        if( _UI_Opponent.activeInHierarchy )
        {
            _UI_Opponent_HP_Bar.valueCurrent    = _Tank_Control._Gun_LookAt._Target.GetComponent<TankWarrior_AI>()._TankControl._HP;
        }
        _UI_Self_HP_Bar.valueCurrent            = _Tank_Control._HP;
    }
    public void API_DriverBeingHit()
    {
        _Player_Defeated = true;
        _Driver_Root.SetActive( false );
        GameEngine_Old._Instance.API_GameOver();
    }
    public void API_Reloading()
    {
        _Tank_UI_Fire_Btn.enabled       = false;
        _Tank_UI_FireEMP_Btn.enabled    = false;
        StartCoroutine(Reloading());
    }
    IEnumerator Reloading()
    {
        _UI_Self_ReloadingBar.gameObject.SetActive( true );
        _UI_Self_ReloadingBar.SetValueMax( (int)(_Tank_Control._CannonReloadTime*100) );
        _UI_Self_ReloadingBar.valueCurrent  = (int)(_Tank_Control._CannonReloadTime*100);
        _UI_Self_ReloadingBar.gameObject.SetActive( true );
        while ( _UI_Self_ReloadingBar.valueCurrent > 0 )
        {
            _UI_Self_ReloadingBar.valueCurrent--;
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(0.02f);
        _UI_Self_ReloadingBar.gameObject.SetActive( false );
        _Tank_UI_Fire_Btn.enabled       = true;
        _Tank_UI_FireEMP_Btn.enabled    = true;
    }
    public void API_RankingUp()
    {
        // Check Conditions
        _Tank_Ranking_Kill += 1;
        if( _Tank_Ranking_Kill >= _Tank_Ranking_Num)
            _Tank_Ranking_Kill = 0;
        else
            return;
        // SFX
        if( GameEngine_Old._SFX )
            _Audio.PlayOneShot( _SFX_RankUp, 1f );
        // + Stuff, refill HP
        _Tank_Control._FirePower        += 1;
        _Tank_Control._CannonReloadTime -= .2f;
        _Tank_Control._ForwardSpeed     += .5f;
        _Tank_Control._RotationScale    += 5f;
        _Tank_Control._HP = _Tank_Control._HP_Max;
        // Safe check
        if( _Tank_Control._FirePower > _UI_Self_FirePowerIndicator.Length )
            _Tank_Control._FirePower = _UI_Self_FirePowerIndicator.Length;
        if( _Tank_Control._CannonReloadTime < .4f ) // min reloading time
            _Tank_Control._CannonReloadTime = .4f;
        // Update GUI
        API_Refresh_HP_Bar();
        for (int i = 0; i < _UI_Self_FirePowerIndicator.Length; i++)
            _UI_Self_FirePowerIndicator[i].SetActive(false);
        for (int i = 0; i < _Tank_Control._FirePower; i++)
            _UI_Self_FirePowerIndicator[i].SetActive(true);

        var orignalScale = _Tank_UI_Promotion.gameObject.transform.localScale;
        _Tank_UI_Promotion.gameObject.transform.localScale = new Vector3( .1f, .1f, .1f);
        _Tank_UI_Promotion.gameObject.transform.DOScale( orignalScale, .5f);
        Utilities._Instance.CloseWindowFadeInToFadeOut( _Tank_UI_Promotion, 3f );
        _Tank_UI_Elite_Indicator.SetActive( true );
    }
    public void API_UnitBeingClicked(GameObject target)
    {
        // Unit Icon Selection Check
        if (GameObject.FindGameObjectsWithTag("Enemy_Normal").Length > 0)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy_Normal"))
            {
                if( target == obj )
                    continue;
                obj.GetComponent<TankWarrior_AI>()._TankControl._2D_Selected.SetActive( false );
            }
        }

        // New Target
        _Tank_Control._Gun_LookAt._Target = target;
        (target.GetComponent<TankWarrior_AI>())._TankControl._2D_Selected.SetActive(true);

        // Set the UI for the enemy
        _UI_Opponent.SetActive(true);
        _UI_Opponent_Name.text = (target.GetComponent<TankWarrior_AI>())._TankControl._Name;
        _UI_Opponent_HP_Bar.valueCurrent = (target.GetComponent<TankWarrior_AI>())._TankControl._HP;
        for (int i = 0; i < _UI_Opponent_Icons.Length; i++)
            _UI_Opponent_Icons[i].SetActive(false);
        // The index of tank units starting with 1, so minus 1 here
        _UI_Opponent_Icons[(target.GetComponent<TankWarrior_AI>())._TankControl._Tank_Index - 1].SetActive(true);
        for (int i = 0; i < _UI_Opponent_FirePowerIndicator.Length; i++)
            _UI_Opponent_FirePowerIndicator[i].SetActive(false);
        for (int i = 0; i < (target.GetComponent<TankWarrior_AI>())._TankControl._FirePower; i++)
            _UI_Opponent_FirePowerIndicator[i].SetActive(true);
    }











    // TODO::
    // Touch Input
    public void Touch_Move(Vector2 move){
        if( _Player_Defeated )
            return;
        _Input_Horizontal   = move.x;
        _Input_Vertical     = move.y;
	}
    public void Touch_Move_End()
    {
        if( _Player_Defeated )
            return;
        _Input_Horizontal   = 0;
        _Input_Vertical     = 0;
    }




}
