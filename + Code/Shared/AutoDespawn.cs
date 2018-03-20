using UnityEngine;
using System.Collections;
using PathologicalGames;



public class AutoDespawn : MonoBehaviour {


    public bool         _DoDestroy;
    public float        _Duration;
    







	// Use this for initialization
	void Start () {
	}
     void OnEnable()
    {
        // Destroy( gameObject, _Duration );
        Invoke("Despawn", _Duration);
    }





    public void Despawn()
    {
        if (_DoDestroy)
        {
            Destroy(gameObject);
            return;
        }

        PoolManager.Pools["PM"].Despawn(this.transform);
    }


}
