using UnityEngine;
using System.Collections;



// This trigger will invoke "_TriggeredUnitFunction" in "_TriggeredUnit", after its collider contacting with an unit with tag "_TriggerUnitTag" 


public class FunctionTrigger : MonoBehaviour {

    public string           _TriggerUnitTag;
    public GameObject       _TriggeredUnit;
    public string           _TriggeredUnitFunction;


	// Use this for initialization
	void Start () {
	
	}



    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == _TriggerUnitTag)
        {
            _TriggeredUnit.SendMessage(_TriggeredUnitFunction);
            Destroy( gameObject );
        }

    }





}
