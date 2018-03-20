using UnityEngine;
using System.Collections;



// Smoothly look at the _Target
// the UP vector is _Source local Y axis
// (use SmoothLookAtConstraint may have Gimbal lock)


public class DR_LookAt : MonoBehaviour {



    public GameObject   _Target;
    public GameObject   _Source;


    [Tooltip("Constant Rotation Mode")]
    public bool         _LinearMode;
    [Tooltip("Degrees per Sec")]
    public float        _RotationSpeed;










    Vector3 localForwardPos;



	// Use this for initialization
	void Start () {
	
        localForwardPos = new Vector3( 0, 2f, 0 );

	}
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos;

        // When no target is selected, we will just look forward
        if (_Target == null)
        {
            targetPos = transform.root.transform.TransformPoint( localForwardPos );
        }
        else
        {
            targetPos = _Target.transform.position;
        }

        Vector3 direction       = targetPos - _Source.transform.position;
        Quaternion toRotation   = Quaternion.LookRotation(direction, _Source.transform.up);

        if (_LinearMode)
        {
            float angle = Quaternion.Angle(_Source.transform.rotation, toRotation);
            float timeToComplete    = angle / _RotationSpeed;
            float aimPercentage     = Mathf.Min(1f, Time.deltaTime / timeToComplete);
            // Constant rotation
            transform.rotation      = Quaternion.Slerp(_Source.transform.rotation, toRotation, aimPercentage);
        }
        else
        {
            // Fast In, slow out, rotation
            transform.rotation = Quaternion.Slerp( _Source.transform.rotation, toRotation, Time.deltaTime );
        }


    }






}
