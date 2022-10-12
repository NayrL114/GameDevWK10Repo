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

    [SerializeField] public Tweener twn;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //loadingPanelT = gameObject.GetChild(1).GetComponent<RectTransform>();
        loadingPanelT.sizeDelta = new Vector2(Screen.width, Screen.height);
        //loadingPanelT.Width = Screen.width;
        //loadingPanelT.Height = Screen.height;
        //HideLoadingScreen();
        StartCoroutine(HideLoadingScreen());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {                      
            float currentDistance = Mathf.Abs(Mathf.Clamp(playerTransform.position.x, -5.0f, 5.0f));
            //float currentDistance = playerTransform.position.x;
            //float absoluteDistance = Mathf.Abs(Mathf.Clamp(currentDistance, -5f, 5f));
            float currentHP = Mathf.Clamp((-0.2f * currentDistance) + 1, 0f, 1.0f);
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


    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            playerCanvasT.rotation = mainCamera.transform.rotation;
            playerCanvasT.rotation = Quaternion.Euler(0f, playerCanvasT.rotation.eulerAngles.y, 0f);

            // I spend two days on the billboarding of HP bar and could not figure out anything, so I looked up some tutorials. 
            // How To... Billboarding in Unity 2020 - 2D Sprites in 3D, made by gamesplusjames, https://www.youtube.com/watch?v=_LRZcmX_xw0. 
        }

    }

    public void LoadFirstLevel()
    //IEnumerator LoadFirstLevel()
    {
        //ShowLoadingScreen();
        StartCoroutine(ShowLoadingScreen());
        //SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnSceneLoad;
        //HideLoadingScreen();
        //StartCoroutine(HideLoadingScreen());
    }

    public void QuitGame()
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

    //public void ShowLoadingScreen()
    IEnumerator ShowLoadingScreen()
    {
        //loadingPanelT.anchoredPosition = new Vector2(0.0f, 0.0f);
        twn.AddTween(loadingPanelT, new Vector3(loadingPanelT.position.x, loadingPanelT.position.y, 0.0f), 
            new Vector3(loadingPanelT.position.x, Screen.height / 2, 0.0f), 0.5f);
        Debug.Log(loadingPanelT.anchoredPosition);
        yield return new WaitForSecondsRealtime(1.5f);
        //yield return StartCoroutine(MoveLoadingPanelUp());
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(HideLoadingScreen());
    }

    //IEnumerator MoveLoadingPanelUp()
    //{
        //twn.AddTween(loadingPanelT, new Vector3(loadingPanelT.position.x, loadingPanelT.position.y, 0.0f), new Vector3(loadingPanelT.anchoredPosition.x, loadingPanelT.anchoredPosition.y, 0.0f), 0.5f);
    //}

    //public void HideLoadingScreen()
    IEnumerator HideLoadingScreen()
    {
        //loadingPanelT.anchoredPosition = new Vector2(0.0f, Screen.height * 2);
        yield return new WaitForSecondsRealtime(1f);
        twn.AddTween(loadingPanelT, new Vector3(loadingPanelT.position.x, loadingPanelT.position.y, 0.0f), new Vector3(loadingPanelT.position.x, - (Screen.height / 2), 0.0f), 0.5f);
    }
}
