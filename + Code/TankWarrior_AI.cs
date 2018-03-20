using UnityEngine;
using System.Collections;

public class TankWarrior_AI : MonoBehaviour {


    public bool _Alive;


    public TankWarrior_TankControl  _TankControl;
    bool                            _AutoMoveOn = true;


	// Use this for initialization
	void Start () {
	
        InvokeRepeating("Scan",     2f, .93f);
        InvokeRepeating("AutoMove", 2f, 2.1f);
        InvokeRepeating("AutoFire", 2f, 3.1f);

	}
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {



        


    }



    void Scan()
    {
        if( !_Alive )
            return;

        if( TankWarrior_Player._Instance._Player_Defeated )
            return;

        if (Utilities.LineOfSight2D(_TankControl._Eye, TankWarrior_Player._Instance.gameObject))
        {
            // Use this to break the co-routine which controls < auto movement >
            _AutoMoveOn = false;

            // Debug.Log(name + "-see Player");
            if (_TankControl._Gun_LookAt._Target == null)
                _TankControl._Gun_LookAt._Target = TankWarrior_Player._Instance.gameObject;
            var relativeToSelf = transform.InverseTransformPoint(TankWarrior_Player._Instance.gameObject.transform.position);
            if (relativeToSelf.x > 1f && !_IsInRotating )
                StartCoroutine(TankTaticRotating());
            if (relativeToSelf.x < -1f && !_IsInRotating )
                StartCoroutine(TankTaticRotating());
        }
        else
        {
            _TankControl._Gun_LookAt._Target = null;
            _AutoMoveOn = true;
        }
    }
    bool _IsInRotating = false;
    IEnumerator TankTaticRotating()
    {
        _IsInRotating = true;
        var relativeToSelf = transform.InverseTransformPoint(TankWarrior_Player._Instance.gameObject.transform.position);
        float backwardsRate = 5f;
        while (_Alive && relativeToSelf.x > 1f)
        {
            Move_BackLeft(backwardsRate);
            yield return new WaitForSeconds(0.01f);
            relativeToSelf = transform.InverseTransformPoint(TankWarrior_Player._Instance.gameObject.transform.position);
        }
        while (_Alive && relativeToSelf.x < -1f)
        {
            Move_BackRight(backwardsRate);
            yield return new WaitForSeconds(0.01f);
            relativeToSelf = transform.InverseTransformPoint(TankWarrior_Player._Instance.gameObject.transform.position);
        }
        _IsInRotating = false;
    }




    void AutoMove()
    {
        if( !_Alive )
            return;

        if( _AutoMoveOn && !_IsInAutoMove )
            StartCoroutine(AutoMoving());
    }
    bool _IsInAutoMove = false;
    IEnumerator AutoMoving()
    {
        _IsInAutoMove   = true;
        _AutoMoveOn     = true;
        // Enter searching mode
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 2f);
        _TankControl.API_EngineSounOn();
        float updateRate = 0.02f;
        while( _AutoMoveOn && hit.collider == null )
        {
            transform.Translate(_TankControl._ForwardSpeed * transform.up * Time.deltaTime, Space.World);
            yield return new WaitForSeconds(updateRate);
            hit = Physics2D.Raycast(transform.position, transform.up, 2f);
        }
        float freeForwardingMeter = 5f;
        if( Random.value < .4f )
        {
            while (_AutoMoveOn)
            {
                // Move_BackRight(2f);
                transform.RotateAround(transform.position, -transform.forward, _TankControl._RotationScale / 5f * Time.deltaTime * -1f);
                yield return new WaitForSeconds(updateRate);
                hit = Physics2D.Raycast(transform.position, transform.up, freeForwardingMeter);
                // Debug.DrawRay(transform.position, freeForwardingMeter*transform.up, Color.green, 0.01f);
                if( hit.collider == null ) 
                    break;
            }
        }
        else
        {
            while (_AutoMoveOn)
            {
                // Move_BackLeft(2f);
                transform.RotateAround(transform.position, -transform.forward, _TankControl._RotationScale / 5f * Time.deltaTime );
                yield return new WaitForSeconds(updateRate);
                hit = Physics2D.Raycast(transform.position, transform.up, freeForwardingMeter);
                // Debug.DrawRay(transform.position, freeForwardingMeter*transform.up, Color.red, 0.01f);
                if( hit.collider == null ) 
                    break;
            }
        }
        _TankControl.API_EngineSounOff();
        _IsInAutoMove = false;
    }
    // Movement
    void Move_BackRight(float scale)
    {
        //transform.Translate(-_TankControl._ForwardSpeed / scale * transform.up * Time.deltaTime, Space.World);
        transform.RotateAround(transform.position, -transform.forward, _TankControl._RotationScale / scale * Time.deltaTime * -1f);
    }
    void Move_BackLeft(float scale)
    {
        //transform.Translate(-_TankControl._ForwardSpeed / scale * transform.up * Time.deltaTime, Space.World);
        transform.RotateAround(transform.position, -transform.forward, _TankControl._RotationScale / scale * Time.deltaTime );
    }








    // Fire
    void AutoFire()
    {
        if( !_Alive )
            return;

        if( TankWarrior_Player._Instance._Player_Defeated )
            return;

        // Distance check
        if( Vector3.Distance( transform.position, TankWarrior_Player._Instance.gameObject.transform.position ) > 10f )
            return;

        if ( Utilities.LineOfSight2D(_TankControl._Eye, TankWarrior_Player._Instance.gameObject) && 
                Utilities.FacingObject( _TankControl._Gun_LookAt.gameObject, TankWarrior_Player._Instance.gameObject, 40f ) )
        {
            _TankControl.Fire();
        }
    }






    public void BeingHitByEMP()
    {
        _TankControl._Tank_GTA_Mode                 = true;
        _TankControl._Gun_LookAt._Target            = null;
        _TankControl._2D_Selected.SetActive( false );
        _AutoMoveOn = false;
        _Alive      = false;
        TankWarrior_Player._Instance._UI_Opponent.SetActive( false );


        TankWarrior_Player._Instance._UI_Disabled.gameObject.SetActive( true );
        TankWarrior_Player._Instance._UI_Disabled.gameObject.GetComponent<EnergyBarToolkit.EnergyBarFollowObject>().followObject = gameObject;

        // This code will not work, if the Canvas Scaler is set to "Scale With Size", only work with "Constant Pixel Size"
        TankWarrior_Player._Instance._UI_Disabled.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        // Debug.Log(Camera.main.WorldToScreenPoint(transform.position));
    }




}
