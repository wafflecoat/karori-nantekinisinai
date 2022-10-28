using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_and_End : MonoBehaviour
{
    public GameObject ob_fadeout;
    
    public int winner;//１：1P勝利、２：2P勝利

    
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float StartWait;
    private Image fade_panel;
    private GameObject UI_Canvas;
    private Transform TF_Canvas;

    private GameObject[] number_text;
    private GameObject Start_text;
    private GameObject GameSet_text;
    private GameObject TimeUp_text;


    void Awake()
    {
        UI_Canvas = GameObject.Find("Canvas_texts");
        TF_Canvas = UI_Canvas.GetComponent<RectTransform>();
        fade_panel = ob_fadeout.GetComponent<Image>();
        number_text = new GameObject[3];

        //１～３のイメージを取得
        for (int i = 0; i < 3; i++)
        {
            number_text[i] = TF_Canvas.GetChild(i).gameObject;
        }
        Start_text = TF_Canvas.GetChild(3).gameObject;
        GameSet_text = TF_Canvas.GetChild(4).gameObject;
        TimeUp_text = TF_Canvas.GetChild(5).gameObject;

    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //画面が黒からフェードイン
        yield return StartCoroutine(fadein());

        yield return new WaitForSeconds(1f);
        //３カウント後スタートの表示
        yield return StartCoroutine(Start_Count());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("aaa");
            StartCoroutine(fadeout());
            //SceneManager.LoadScene("main");
        }
    }

    private void OnTriggerEnter(Collider player)
    {

        if(player.gameObject.CompareTag("Player"))
        {
            winner = 1;
            StartCoroutine(fadeout());
        }
        else if (player.gameObject.CompareTag("gruound"))
        {
            winner = 2;
            StartCoroutine(fadeout());
        }
    } 

    IEnumerator fadein()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Color color = fade_panel.color;
            color.a -= fadeSpeed * Time.deltaTime;
            fade_panel.color = color;
            if (color.a <= 0) break;
            Debug.Log("b");
        }
        Debug.Log("a");
    }

    IEnumerator fadeout()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Color color = fade_panel.color;
            color.a += fadeSpeed * Time.deltaTime;
            fade_panel.color = color;
            if (color.a >= 1) break;
        }
        Debug.Log("a");
    }

    IEnumerator Start_Count()
    {
        for(int i = 0; i < 3; i++)
        {
            number_text[i].SetActive(true);
            yield return new WaitForSeconds(1f);
            number_text[i].SetActive(false);
        }

        Start_text.SetActive(true);
        yield return new WaitForSeconds(1f);
        Start_text.SetActive(false);
    }
}
