using System.Collections;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Linq;

public class Tutorial_0 : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject uiObject;
    [Space(10)]
    [SerializeField] private float activateTime = 2f;
    [SerializeField] private float deactivateTime = 5f;
    [SerializeField] private float fadeDuration = 0.5f;
    //[SerializeField] private bool UI = false;
    [SerializeField] private float time_UI;
    private bool IS_coroutine = false;
    private float timer_;


    private void Update()
    {
        timer_ += Time.deltaTime;
        Debug.Log(timer_);

        if (timer_ >= time_UI && !IS_coroutine)
        {
            StartCoroutine(UITutorial());
            IS_coroutine = true;
        }
    }




    private IEnumerator UITutorial()
    {
        //Debug.Log("sýfýrladý");
        canvasGroup.alpha = 0;

        yield return new WaitForSeconds(activateTime);

        uiObject.SetActive(true);
        //Debug.Log("aktif");

        canvasGroup.DOFade(1, fadeDuration);

        yield return new WaitForSeconds(deactivateTime);


        canvasGroup.DOFade(0, fadeDuration).OnComplete(() => uiObject.SetActive(false));
        //Debug.Log("deaktif");
        //UI = 
        IS_coroutine = false;
        //Debug.Log("UI kapandý");

    }


}
