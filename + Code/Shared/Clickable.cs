using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;



// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;
using System;


// !!! One have to add "Physics Raycaster" to the camera
// Use this to ignore click events on UI



public class Clickable : MonoBehaviour, IPointerClickHandler {


    
    // Use this for initialization
    void Start () {
	
	}





    public void OnPointerClick(PointerEventData eventData)
    {
        
        // This should be project dependent
        if( TankWarrior_Player._Instance != null )
            TankWarrior_Player._Instance.API_UnitBeingClicked( transform.root.gameObject );

    }














}
