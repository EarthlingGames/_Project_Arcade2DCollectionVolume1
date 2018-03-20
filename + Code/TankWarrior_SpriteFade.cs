using UnityEngine;
using System.Collections;

// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


public class TankWarrior_SpriteFade : MonoBehaviour {


    public float            _FadeOffset;
    public SpriteRenderer[] _SpriteRenderers;


	// Use this for initialization
	void Start () {
	    InvokeRepeating("Fade", 1, .5f);
	}
	// Update is called once per frame
	void Update () {
	
	}


    void Fade()
    {
        foreach ( SpriteRenderer SR in _SpriteRenderers )
        {
            // Debug.Log("Alpha:" + SR.color.a);
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, SR.color.a - _FadeOffset  );
            if( SR.color.a < 0.001f )
            {
                Destroy( gameObject );
            }
        }
    }



}
