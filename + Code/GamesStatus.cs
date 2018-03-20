using UnityEngine;
using System.Collections;


// Custom includes
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PathologicalGames;


// This script displays the ranking of each game


public class GamesStatus : MonoBehaviour {

	



    public GameObject[] _Game_1_Stars;
    public GameObject[] _Game_2_Stars;









	// Use this for initialization
	void Start () {
        Init();
	}
	// Update is called once per frame
	void Update () {
	
	}















    void Init()
    {

        Clear( _Game_1_Stars );
        Clear( _Game_2_Stars );
        Load( "Space Ranger", _Game_1_Stars );
        Load( "Tank Warrior", _Game_2_Stars );

    }
    void Clear(GameObject[] objs)
    {
        foreach( GameObject obj in objs )
            obj.SetActive( false );
    }
    void Load( string gameName, GameObject[] objs)
    {
        // Z. B.: Space Ranger Stars
        // Z. B.: Tank Warrior Stars
        for( int i = 0; i < PlayerPrefs.GetInt( gameName + " Stars" ); i++ )
            objs[i].SetActive( true );
    }



}
