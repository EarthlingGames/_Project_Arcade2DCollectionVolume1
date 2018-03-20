using UnityEngine;
using System.Collections;

public class UnitTrigger : MonoBehaviour {


    public string           _TriggerUnitTag;
    public GameObject       _TriggeredUnit;




	// Use this for initialization
	void Start () {
	
	}



    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == _TriggerUnitTag)
        {
            _TriggeredUnit.SendMessage("API_ActiveUnit");
            Destroy( gameObject );
        }

    }


}
