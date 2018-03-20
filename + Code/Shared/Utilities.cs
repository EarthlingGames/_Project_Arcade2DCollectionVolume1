using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


using DG.Tweening;



public class Utilities : MonoBehaviour {

    public static Utilities _Instance;


    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        
    }
    void Update()
    {
        if( _ObservingClickEvent && Input.GetMouseButtonDown(0) )
            GetClickWorldPosition(Vector3.zero);
    }






    /// <summary>
    /// Utilities
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool LineOfSight(GameObject source, GameObject target, float YaxisOffset)
    {
        RaycastHit hit;
        var rayDirection = new Vector3( target.transform.position.x, target.transform.position.y + YaxisOffset, target.transform.position.z )
            - source.transform.position;
        if (Physics.Raycast(source.transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject == target)
            {
                return true;
            }
        }
        return false;
    }
    public static bool FacingObject(GameObject source, GameObject target, float Angle)
    {
        if  ( Vector3.Angle(source.transform.forward, target.transform.position - source.transform.position ) < Angle) {
            return true;
        }
        return false;
    }
    // Returns true if there are any colliders overlapping the sphere defined by position and radius in world coordinates.
    public static bool InsideSomething( Vector3 pos, float checkingRadius )
    {
        if ( Physics.CheckSphere( pos, checkingRadius) )
        {
            return true;
        }
        return false;
    }
    public static bool LineOfSight2D(GameObject source, GameObject target)
    {
        var direction = target.transform.position - source.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(source.transform.position, direction, 50f);
        if ( hit.collider != null )
        {
            // Debug.Log(hit.collider.name);
            if( hit.collider.transform.root.gameObject == target )
                return true;
        }
        return false;
    }









    // Fade in and Fade out UIs
    public void CloseWindowFadeOut( CanvasGroup canvas )
    {
        StartCoroutine(ThreadCloseWindowFadeOut(canvas));
    }
    IEnumerator ThreadCloseWindowFadeOut( CanvasGroup canvas )
    {
        canvas.alpha = 1f;
        canvas.DOFade(0, .5f);
        yield return new WaitForSeconds(.5f);
        canvas.gameObject.SetActive(false);
    }
    //
    public void CloseWindowFadeIn( CanvasGroup canvas )
    {
        StartCoroutine(ThreadCloseWindowFadeIn(canvas));
    }
    IEnumerator ThreadCloseWindowFadeIn( CanvasGroup canvas )
    {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 0;
        canvas.DOFade(1, .5f);
        yield return new WaitForSeconds(.5f);
    }
    public void CloseWindowFadeInToFadeOut( CanvasGroup canvas, float duration )
    {
        _Duration = duration;
        StartCoroutine(ThreadCloseWindowFadeInToFadeOut(canvas));
    }
    float _Duration = 0;
    IEnumerator ThreadCloseWindowFadeInToFadeOut( CanvasGroup canvas )
    {
        canvas.gameObject.SetActive(true);
        canvas.alpha = 0;
        canvas.DOFade(1, .5f);
        yield return new WaitForSeconds(_Duration);
        canvas.alpha = 1;
        canvas.DOFade(0, .5f);
        yield return new WaitForSeconds(.5f);
        canvas.gameObject.SetActive(false);
    }









    // Get the position (on the ground in the game world) when click at the screen
    public bool         _ObservingClickEvent;
    public LayerMask    _Layer_To_Click; // select the layer(s) you want to detect collides within, and ignore the others
    public Vector3 GetClickWorldPosition(Vector3 defaultPos)
    {

        // TODO:: (UI Blocking)
        // The physics raycast is a different system and doesn't see UI elements. So you can not cause the UI to block physics raycasts. However you have options:
        // What you can do is cast two rays. One raycast for your UI elements and one for your physics objects. If your UI raycast detects a UI element, you know a UI element is in the way, and you shouldn't cast your physics ray cast.
        // Or, you can use the event system (which is already casting a ray into the UI elements) to tell you if you're over a UI element:
        // UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
        if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() )
            return defaultPos; // We hit an UI element, simple return the default position; (only works on iOS)

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, _Layer_To_Click))
        {
            // Debug.Log("Clicking:"+hit.point);
            return hit.point;
        }
        return defaultPos;
    }
    // Use to return the root gameobject (which contains _Name) we clicked in world space
    public GameObject GetClickedObjectRootWithName( string _Name )
    {
        // These line are not working with 2D, why?
        //if ( UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() )
        //    return null; // We hit an UI element, simple return the default position;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, _Layer_To_Click))
        {
            if( hit.collider.gameObject.name.Contains(_Name) )
                return hit.collider.transform.root.gameObject;
        }
        return null;
    }











    // Get the direction (vector) from source pointing towards target
    public static Vector3 GetDirectionFromSourceToTarget( Vector3 source, Vector3 target )
    {
        return (target - source).normalized;
    }








    // In mathematics, a percentage is a number or ratio expressed as a fraction of 100. 
    public static int GetPercentageAsInteger( float Dividend, float Divisor )
    {
        float percentage = Dividend / Divisor;
        percentage = Mathf.Round(percentage * 100f);
        return (int)percentage; // z.B.: 30%
    }
    public static float GetPercentageAsFloat( float Dividend, float Divisor )
    {
        float percentage = Dividend / Divisor;
        percentage = Mathf.Round(percentage * 100f) / 100f;
        return percentage; // z.B.: 0.3
    }








    // Scene Management
    public static void ReloadCurrentActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }











    // Audio, SFX
    public void API_SFX_SmoothlyPlayNextClip( AudioClip next, float emptyDuration, AudioSource player )
    {
        if( !_IsInSwitchingClips )
            StartCoroutine( SwitchingClips(next, emptyDuration, player ));
    }
    bool _IsInSwitchingClips = false;
    IEnumerator SwitchingClips( AudioClip next, float emptyDuration, AudioSource player)
    {
        _IsInSwitchingClips = true;

        if ( GameEngine_Old._SFX && !player.isPlaying )
            player.Play();

        float orignalVolume = player.volume;
        while( player.volume > .02f )
        {
            player.volume -= 0.01f;
            yield return new WaitForSeconds(.02f);
        }
        yield return new WaitForSeconds(emptyDuration);
        player.clip = next;

        if ( GameEngine_Old._SFX && !player.isPlaying )
            player.Play();

        while( player.volume < orignalVolume )
        {
            player.volume += 0.01f;
            yield return new WaitForSeconds(.02f);
        }
        _IsInSwitchingClips = false;
    }






}
