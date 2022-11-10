using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FadeUI
{
    public class Fade_Image : MonoBehaviour
    {

        private void Start()
        {
            //FadeUI.Fade_Image fade_Image = new FadeUI.Fade_Image();
        }

        IEnumerator fadein(float fadeSpeed, GameObject ob_fade)
        {

            Image fade_panel = ob_fade.GetComponent<Image>();
            var wait = new WaitForEndOfFrame();
            while (true)
            {
                yield return wait;
                Color color = fade_panel.color;
                color.a -= fadeSpeed * Time.deltaTime;
                fade_panel.color = color;
                if (color.a <= 0) break;
            }
        }

        IEnumerator fadeout(float fadeSpeed, GameObject ob_fade)
        {
            Image fade_panel = ob_fade.GetComponent<Image>();
            var wait = new WaitForEndOfFrame();
            while (true)
            {
                yield return wait;
                Color color = fade_panel.color;
                color.a += fadeSpeed * Time.deltaTime;
                fade_panel.color = color;
                if (color.a >= 1) break;
            }
        }

        //à»â∫ÅAåƒÇ—èoÇµóp
        public void Call_fadein(float fadeSpeed, GameObject ob_fade)
        {
            StartCoroutine(fadein(fadeSpeed, ob_fade));
        }

        public void Call_fadeout(float fadeSpeed, GameObject ob_fade)
        {
            StartCoroutine(fadeout(fadeSpeed, ob_fade));
        }
    }
}
