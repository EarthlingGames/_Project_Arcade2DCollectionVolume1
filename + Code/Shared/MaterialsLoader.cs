using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;





// Auto randomly load a mat 


public class MaterialsLoader : MonoBehaviour {


    public Material[]   _Mats;


    public bool         _UseMeshRenderer;
    MeshRenderer        _MeshRenderer;

    public bool         _UseSpriteRenderer;
    SpriteRenderer      _SpriteRenderer;



    void Awake()
    {
        if( _UseMeshRenderer )
            _MeshRenderer   = GetComponent<MeshRenderer>();
        if( _UseSpriteRenderer )
            _SpriteRenderer = GetComponent<SpriteRenderer>();
    }
	// Use this for initialization
	void Start () {
	
        if( _UseMeshRenderer )
        {
            _MeshRenderer.material      = _Mats[Random.Range(0, _Mats.Length)];
            _MeshRenderer.enabled       = true;
        }

        if( _UseSpriteRenderer )
        {
            _SpriteRenderer.material    = _Mats[Random.Range(0, _Mats.Length)];
            _SpriteRenderer.enabled     = true;
        }

	}








	
}
