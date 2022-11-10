using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Result_Button : MonoBehaviour
{
    public GameObject ob_fadeout;

    private Button button;
    private Animator animator;
    private Image Fade_panel;
    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        Fade_panel = ob_fadeout.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click_Act_continue()
    {
        StartCoroutine(_continue());
        StartCoroutine(Disabled_button());
    }

    public void Click_Act_quit()
    {
        StartCoroutine(quit());
        StartCoroutine(Disabled_button());
    }

    IEnumerator _continue()
    {
        yield return StartCoroutine(fadeout());
        Debug.Log("continue");
        //SceneManager.LoadScene("main");
    }

    IEnumerator quit()
    {
        yield return StartCoroutine(fadeout());
        Debug.Log("end");
        //Application.Quit();
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

    //ボタン無効化コルーチン
    IEnumerator Disabled_button()
    {
        yield return null;
        
        button = this.GetComponent<Button>();
        animator = this.GetComponent<Animator>();
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
        button.interactable = false;
    }
}
