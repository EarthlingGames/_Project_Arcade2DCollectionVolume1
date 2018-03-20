using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


public class TankWarrior_GTA : MonoBehaviour {



    TankWarrior_Player  _PlayerScript;



	// Use this for initialization
	void Start () {

	    _PlayerScript = transform.root.gameObject.GetComponent<TankWarrior_Player>();

	}
	// Update is called once per frame
	void Update () {
	
	}




    void OnTriggerEnter2D( Collider2D other )
    {

        if( other.tag == "Armor")
        {
            GameObject root     = other.transform.root.gameObject;
            TankWarrior_AI ai   = root.GetComponent<TankWarrior_AI>();
            TankWarrior_TankControl tankControlScript = ai._TankControl;
            if( tankControlScript != null && tankControlScript._Tank_GTA_Mode )
            {
                // VFX + SFX
                if (GameEngine_Old._SFX)
                {
                    _PlayerScript._SFX_TankStart.Play();
                }

                _PlayerScript._Player_Mode = 2; // switch to tank mode
                _PlayerScript._Driver_Root.SetActive(false);
                // _PlayerScript._Tanks_Root[tankControlScript._Tank_Index].SetActive(true);

                _PlayerScript.gameObject.transform.position = root.transform.position;
                _PlayerScript.gameObject.transform.rotation = root.transform.rotation;

                tankControlScript._2D_Selected.SetActive(false);

                _PlayerScript._Tank_Control = tankControlScript;
                tankControlScript.gameObject.transform.parent = _PlayerScript._Tanks_Root.transform;
                tankControlScript._HP_Bar   =  _PlayerScript._UI_Self_HP_Bar;

                //_PlayerScript._Tanks_Index      = tankControlScript._Tank_Index;
                //_PlayerScript._Tank_Control     = _PlayerScript._Tanks_Root[_PlayerScript._Tanks_Index].GetComponent<TankWarrior_TankControl>();
                //_PlayerScript._Tanks_Root[_PlayerScript._Tanks_Index].GetComponent<TankWarrior_TankControl>()._HP_Bar = _PlayerScript._UI_Self_HP_Bar;



                // Set UI Status
                _PlayerScript._UI_Self.SetActive( true );
                _PlayerScript._Tanks_UI_Btns.SetActive(true);
                _PlayerScript._UI_Self_Name.text = tankControlScript._Name;
                _PlayerScript._UI_Self_HP_Bar.valueCurrent = tankControlScript._HP;
                for (int i = 0; i < _PlayerScript._UI_Self_Icons.Length; i++)
                    _PlayerScript._UI_Self_Icons[i].SetActive(false);
                _PlayerScript._UI_Self_Icons[tankControlScript._Tank_Index-1].SetActive(true);

                for (int i = 0; i < _PlayerScript._UI_Self_FirePowerIndicator.Length; i++)
                    _PlayerScript._UI_Self_FirePowerIndicator[i].SetActive(false);

                for (int i = 0; i < tankControlScript._FirePower; i++)
                    _PlayerScript._UI_Self_FirePowerIndicator[i].SetActive(true);

                // Disable the indicator for the Enemy Tank Unit
                TankWarrior_Player._Instance._UI_Disabled.gameObject.SetActive( false );
                // Disable the indicator for the Elite Tank
                TankWarrior_Player._Instance._Tank_UI_Elite_Indicator.SetActive( false );

                // Tell the level management: one unit less
                TankWarrior_LevelManagement._Instance._UnitsCount--;

                // Gain Game Points
                GameEngine_Old._Instance.API_DisplayPointsEarnOrConsumed( true, 10 );
                GameEngine_Old._Instance.API_GamePointsEarned( 10 );

                root.SetActive( false ); // or destroy, despawn
            }
        }

    }




















}
