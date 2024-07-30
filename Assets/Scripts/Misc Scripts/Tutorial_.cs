using System.Collections;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Linq;

public class Tutorial_ : MonoBehaviour
{
    [SerializeField] private Transform player_pos;
    [SerializeField] private float range_;
    [Space(10)]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject uiObject;
    [Space(10)]
    [SerializeField] private float activateTime = 2f;
    [SerializeField] private float deactivateTime = 5f;
    [SerializeField] private float fadeDuration = 0.5f;
    //[SerializeField] private bool UI = false;


    private bool IS_coroutine = false;


    private void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player_pos.position);

        if (distanceToPlayer < range_ && !IS_coroutine)
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





    private void OnDrawGizmos()
    {

        //if (player_pos != null)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawLine(transform.position, player_pos.position);


        //    float distanceToPlayer = Vector3.Distance(transform.position, player_pos.position);
        //    string distanceText = $"Distance: {distanceToPlayer:F0}";
        //    Handles.Label((transform.position + player_pos.position) / 2, distanceText); // Çizginin ortasýna mesafeyi yazdýr
        //}

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range_);
    }
}
