using UnityEngine;
using System.Collections;
using PathologicalGames;
using DG.Tweening;








public class BossShip : MonoBehaviour {


    public int          _Health;
    public Transform    _StartingPos;
    public GameObject   _VFX_Expo;
    public GameObject[] _EngineSmokePos;
    public AudioClip    _SFX_Expo;


    [Header("Fire System")]
    public float        _Fire_StartDelay;
    public GameObject   _Fire_Bullet;
    public float        _Fire_Rate;
    public Transform[]  _Fire_GunHoles;







	// Use this for initialization
	void Start () {
	
        // Use this 10 sec for UI
        Invoke("UnitEnter", 1f);

	}
	// Update is called once per frame
	void Update () {
	
	}
    void UnitEnter()
    {
        InvokeRepeating("Fire", _Fire_StartDelay, _Fire_Rate);
        // To the assembly area
        transform.DOMove(_StartingPos.position, 3.2f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {

		if( other.tag == "Player_Tiny" )
        {
			SpFLib.master.HitColor(gameObject);
			Invoke("ResetColor", 0.1f);

			if( _Health - 1 > 0 ){
				_Health--;
			}
            else
            {
				_Health = 0;
                Instantiate(_VFX_Expo, transform.position, _VFX_Expo.transform.rotation);
                LevelManagement._Instance._Phase_Index++; // To next phase
                LevelManagement._Instance._Audio.Stop();
                if( GameEngine_Old._SFX)
                {
                    AudioSource.PlayClipAtPoint( _SFX_Expo, Camera.main.transform.position );
                }
                // Gain Game Points
                GameEngine_Old._Instance.API_DisplayPointsEarnOrConsumed( true, 200 );
                GameEngine_Old._Instance.API_GamePointsEarned( 200 );

				Destroy(gameObject);
			}
            return;
		}         

    }
    void ResetColor()
    {
        SpFLib.master.NormalColor(gameObject);
    }
    void FixedUpdate()
    {
        /*
        for (int i = 0; i < _EngineSmokePos.Length; i++)
        {
            if (Random.Range(0, 3) == 1)
            {
                SpFLib.master.CreateFire( _EngineSmokePos[i], GameEngine._Instance._Environment, 1f, 1f, 1f);
            }
        }
        */
    }





    public void Fire()
    {

        foreach( Transform hole in _Fire_GunHoles)
        {
            Transform bullet = PoolManager.Pools["PM"].Spawn(_Fire_Bullet.transform, hole.position, hole.rotation);

        }
        

    }








}
