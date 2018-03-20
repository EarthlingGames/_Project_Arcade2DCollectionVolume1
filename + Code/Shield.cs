using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    OnEnable();
	}
    void OnDisable() {
        transform.localScale = new Vector3(.1f, .1f, 1f);
        (GetComponent<CircleCollider2D>()).enabled = false;
    }
    void OnEnable()
    {
        transform.localScale = new Vector3(.1f, .1f, 1f);
        (GetComponent<CircleCollider2D>()).enabled = true;
    }



    void OnTriggerEnter2D(Collider2D other) {

        if( other.tag == "Enemy_Tiny")
        {
            Destroy(other);
        }

    }


}
