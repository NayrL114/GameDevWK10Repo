using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private Image innerBarImage;
    private Transform playerTransform;

    private RectTransform playerCanvasT;
    private Camera mainCamera;
    //private Quaternion canvasAngle;
    public RectTransform loadingPanelT;

    private float lerpStartTime;
    public float loadingPanelDurationTime;

    [SerializeField] public Tweener twn;
        
    void Awake()// trying to initialise loadingPanelDurationTime before Start()
    {
        loadingPanelDurationTime = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadingPanelT.sizeDelta = new Vector2(Screen.width, Screen.height);
        lerpStartTime = Time.time;
        //loadingPanelMoveTime = 1f;
        //StartCoroutine(HideLoadingScreen());
        HideLoadingScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {                      
            float currentDistance = Mathf.Abs(Mathf.Clamp(playerTransform.position.x, -5.0f, 5.0f));
            float currentHP = Mathf.Clamp((-0.2f * currentDistance) + 1, 0f, 1.0f);// Linear regression, -1/5x + 1
            //float currentHP = Mathf.Clamp(currentDistance / 1.0f + 1, 0f, 1.0f);
            //Debug.Log("Distance away from X zero is " + currentDistance + " and HP would be " + currentHP);

            innerBarImage.fillAmount = currentHP;

            if (currentHP < 0.5f)
            {
                innerBarImage.color = Color.red;
            }
            else
            {
                innerBarImage.color = Color.green;
            }
            
            //Debug.Log("Showing position data of loading panel during update()");
            //Debug.Log("position is " + loadingPanelT.position);
            //Debug.Log("anchoredPosition is " + loadingPanelT.anchoredPosition);            

            //canvasAngle.eulerAngles = new Vector3(0f, Mathf.Clamp(mainCamera.transform.rotation.y * 120, -180f, 180f), 0f);

            //Debug.Log(new Vector3(0f, mainCamera.transform.rotation.y, 0f));
            //Debug.Log(mainCamera.transform.rotation.y);

            //innerBarImage.rectTransform.rotation.x = Quaternion.Euler(45f, 0f, 0f);

            //playerCanvasT = playerTransform.Find("PlayerCanvas")..GetComponent<RectTransform>();
            //playerCanvasT.rotation.y = mainCamera.transform.rotation.y * 100;
            //playerCanvasT.rotation = Quaternion.Euler(new Vector3(0f, mainCamera.transform.rotation.y * 100, 0f));
            //playerCanvasT.rotation = Quaternion.Euler(0f, mainCamera.transform.rotation.y * 100, 0f);
            //playerCanvasT.rotation = canvasAngle;
            //playerCanvasT.localEulerAngles = new Vector3(0f, Mathf.Clamp(mainCamera.transform.rotation.y * 120, -180f, 180f), 0f);
            //playerCanvasT.LookAt(mainCamera.transform.rotation.y);
            //playerCanvasT.Rotate(0f, mainCamera.transform.rotation.y, 0f);
            //playerCanvasT.SetPositionAndRotation(playerCanvasT.position, Quaternion.Euler(new Vector3(0f, mainCamera.transform.rotation.y, 0f)));// this one seems be working
            //innerBarImage.rectTransform.rotation = playerCanvasT.rotation;
            //playerCanvasT.LookAt(mainCamera.GetComponent<Transform>());

            //Debug.Log(playerCanvasT.rotation);

            //playerCanvasT.LookAt(mainCamera.transform);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HideLoadingScreen();
        }

    }// end of Update()

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            playerCanvasT.rotation = mainCamera.transform.rotation;
            playerCanvasT.rotation = Quaternion.Euler(0f, playerCanvasT.rotation.eulerAngles.y, 0f);

            // I spend two days on HPbar billboarding and could not figure out anything, so I looked up some tutorials. 
            // How To... Billboarding in Unity 2020 - 2D Sprites in 3D, made by gamesplusjames, https://www.youtube.com/watch?v=_LRZcmX_xw0. 
        }
    }

    //IEnumerator LoadFirstLevel()
    public void LoadFirstLevel()// Highly modified LoadFirstLevel() method for 100%, LoadSceneAsync(1) is called via an Invoke inside ShowLoadingScreen(). 
    {        
        ShowLoadingScreen();
        //StartCoroutine(ShowLoadingScreen());

        //SceneManager.LoadSceneAsync(1);
        //SceneManager.sceneLoaded += OnSceneLoad;

        //HideLoadingScreen();
        //StartCoroutine(HideLoadingScreen());
    }

    public void QuitGame()// method for quitting play mode
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            Button quitButton = GameObject.FindWithTag("QuitButton").GetComponent<Button>();
            innerBarImage = GameObject.FindWithTag("PlayerHealthBar").GetComponent<Image>();
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            //mainCamera = GameObject.FindWithTag("MainCamera");
            mainCamera = Camera.main;
            playerCanvasT = playerTransform.Find("PlayerCanvas").GetComponent<RectTransform>();

            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void ShowLoadingScreen()
    {
        //float fractionTime = (Time.time - lerpStartTime) / 3f; // 0.5f is duration time
        lerpStartTime = Time.time;
        /* yield return */StartCoroutine(MoveLoadingScreen(new Vector2(loadingPanelT.anchoredPosition.x, loadingPanelT.anchoredPosition.y),
            new Vector2(0.0f, 0.0f)));

        Invoke("LoadLevel", 2f);
        Invoke("HideLoadingScreen", 3f);
        /*
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(HideLoadingScreen());
        */
    }

    public void HideLoadingScreen()
    {        
        lerpStartTime = Time.time;
        //yield return new WaitForSecondsRealtime(1f);
        //float fractionTime = (Time.time - lerpStartTime) / 3f; // 0.5f is duration time
        /*yield return */StartCoroutine(MoveLoadingScreen(new Vector2(loadingPanelT.anchoredPosition.x, loadingPanelT.anchoredPosition.y),
            new Vector2(loadingPanelT.anchoredPosition.x, - (Screen.height + 1f)))); // -(Screen.height + 1f) is to make sure loading panel can hide properly below screen
    }

    IEnumerator MoveLoadingScreen(Vector2 startPosition, Vector2 endPosition)// vector2.lerp
    {
        while (loadingPanelT.anchoredPosition != endPosition)// modified based on lerp component in Tweener.Update()
        {            
            float fractionTime = (Time.time - lerpStartTime) / loadingPanelDurationTime; // durationTime is initialised in Awake(), and can be manually assigned in Unity inspector
            loadingPanelT.anchoredPosition = Vector2.Lerp(startPosition, endPosition, fractionTime);
            yield return null;
        }        
    }

    //IEnumerator LoadLevel()
    private void LoadLevel()// Putting LoadSceneAsync(1) out seperately for 100%
    {
        SceneManager.LoadSceneAsync(1);
        //yield return null;
    }
    
}// end of everything ;p
