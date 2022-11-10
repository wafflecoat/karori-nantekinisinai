using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result_Event : MonoBehaviour
{
    public GameObject ob_fadeout;
    public GameObject ob_Event;
    private Image Fade_panel;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        Fade_panel = ob_fadeout.GetComponent<Image>();
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Character_drow();
        yield return StartCoroutine(fadein());
        ob_Event.SetActive(true);
        //continueを選択する
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Character_drow()
    {
        if (Start_and_End.win == 1)
        {
            //プレイヤー１を表示
        }
        else if(Start_and_End.win == 2)
        {
            //プレイヤー２を表示
        }
        else
        {
            //引き分け
        }
    }

    IEnumerator fadein()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            Color color = Fade_panel.color;
            color.a -= fadeSpeed * Time.deltaTime;
            Fade_panel.color = color;
            if (color.a <= 0) break;
        }
    }

    IEnumerator fadeout()
    {
        var wait = new WaitForEndOfFrame();
        while (true)
        {
            yield return wait;
            Color color = Fade_panel.color;
            color.a += fadeSpeed * Time.deltaTime;
            Fade_panel.color = color;
            if (color.a >= 1) break;
        }
    }

}
