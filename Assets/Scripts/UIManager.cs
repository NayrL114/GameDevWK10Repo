using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private Image innerBarImage;
    private Transform playerTransform;
    private Transform playerCanvasT;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            float currentDistance = Mathf.Abs(Mathf.Clamp(playerTransform.position.x, -5.0f, 5.0f));
            //float currentDistance = playerTransform.position.x;
            //float absoluteDistance = Mathf.Abs(Mathf.Clamp(currentDistance, -5f, 5f));
            float currentHP = Mathf.Clamp(1.0f / currentDistance, 0f, 1.0f);
            Debug.Log("Distance away from zero is " + currentDistance + " and HP would be " + currentHP);

            innerBarImage.fillAmount = currentHP;

            if (currentHP < 0.5f)
            {
                innerBarImage.color = Color.red;
            }
            else
            {
                innerBarImage.color = Color.green;
            }

            //Transform playerCanvas = innerBarImage.parent.parent;
            //innerBarImage.rectTransform.rotation = playerCanvasT.rotation;
        }

        
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("WalkingScene");
        SceneManager.sceneLoaded += OnSceneLoad;
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
            //playerCanvasT = innerBarImage.parent.parent;

            quitButton.onClick.AddListener(QuitGame);
        }
    }
}
