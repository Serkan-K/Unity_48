using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Cinemachine;

public class Floating_UI : MonoBehaviour
{
    [SerializeField] private Transform player_pos, lookAt;
    [SerializeField] private Vector3 offset;
    [Space(10)]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject uiObject;
    [Space(10)]
    [SerializeField] private float range_;
    [SerializeField] private float activateTime = 2f;
    [SerializeField] private float deactivateTime = 5f;
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] CinemachineVirtualCamera virtual_camera;
    private bool isCoroutineRunning = false;

    private void Start()
    {
        uiObject.SetActive(false);
    }

    private void Update()
    {
        Transform look_At = virtual_camera.LookAt;

        float distanceToPlayer = Vector3.Distance(lookAt.position, player_pos.position);

        if (distanceToPlayer < range_ && !isCoroutineRunning)
        {
            StartCoroutine(UITutorial());
            isCoroutineRunning = true;
        }

        Vector3 pos = Camera.main.WorldToScreenPoint(lookAt.position + offset);
        transform.position = pos;
    }

    private IEnumerator UITutorial()
    {
        canvasGroup.alpha = 0;
        uiObject.SetActive(true);
        canvasGroup.DOFade(1, fadeDuration);

        yield return new WaitForSeconds(activateTime);

        canvasGroup.DOFade(0, fadeDuration).OnComplete(()
            =>
        { uiObject.SetActive(false); isCoroutineRunning = false; });
    }






    private void OnDrawGizmos()
    {
        //--- çizgi için
        //if (player_pos != null)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawLine(transform.position, player_pos.position);


        //    float distanceToPlayer = Vector3.Distance(transform.position, player_pos.position);
        //    string distanceText = $"Distance: {distanceToPlayer:F2}"; // 2 ondalýk basamak
        //    Handles.Label((transform.position + player_pos.position) / 2, distanceText); // Çizginin ortasýna mesafeyi yazdýr
        //}


        Gizmos.color = Color.green; // Küre rengini istediðiniz gibi ayarlayabilirsiniz
        Gizmos.DrawWireSphere(transform.position, range_); // UI'ýn etrafýnda bir küre çiz
    }



}

