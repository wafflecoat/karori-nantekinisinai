using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result_Event : MonoBehaviour
{
    public GameObject ob_fadeout;
    public GameObject ob_Event;
    public GameObject ob_win;
    [SerializeField] GameObject Gal_image;
    [SerializeField] GameObject Seiso_image;
    private Image Fade_panel;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        Fade_panel = ob_fadeout.GetComponent<Image>();
        ob_fadeout.SetActive(true);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Start_and_End.win = 2;
        Character_drow();
        yield return StartCoroutine(fadein());
        ob_Event.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Character_drow()
    {
        if (Start_and_End.win == 1)
        {
            ob_win.SetActive(true);
            //プレイヤー１を表示
            Gal_image.SetActive(true);
        }
        else if(Start_and_End.win == 2)
        {
            ob_win.SetActive(true);
            //プレイヤー２を表示
            Seiso_image.SetActive(true);
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
