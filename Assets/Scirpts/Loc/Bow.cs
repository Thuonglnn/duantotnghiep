using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Arrow")]
    [SerializeField] private Rigidbody arrowPrefab;
    [SerializeField] private Rigidbody arrowPrefab1;

    [SerializeField] private Transform arrowPos;
    [SerializeField] private Transform arrowEquipParent;
    [SerializeField] private float arrowForce = 1000f;
    Rigidbody currentArrow;

    [Header("Bow")]
    [SerializeField] private Transform equipPos;
    [SerializeField] private Transform unEquipPos;
    [SerializeField] private Transform unEquipParent;
    [SerializeField] private Transform equipParent;

    [Header("Bow String")]
    [SerializeField] private Transform bowString;
    [SerializeField] private Transform stringInitialPos;
    [SerializeField] private Transform stringHandPullPos;
    [SerializeField] private Transform stringInitialParent;

    [Header("CrossHair")]
    [SerializeField] private GameObject crossHairPrefab;
    private GameObject currentCrossHair;

    [Header("Bow Audio")]
    AudioSource bowAudio;
    [SerializeField] private AudioClip pullStringClip;
    [SerializeField] private AudioClip releaseStringClip;
    [SerializeField] private AudioClip drawArrowClip;


    private bool canPullString = false;
    private bool canFireArrow = false;

    

    private void Start()
    {
        bowAudio = GetComponent<AudioSource>();
    }
    public void EquipBow()
    {
        this.transform.position = equipPos.position;
        this.transform.rotation = equipPos.rotation;
        this.transform.parent = equipParent;
    }

    public void UnEquipBow()
    {
        this.transform.position = unEquipPos.position;
        this.transform.rotation = unEquipPos.rotation;
        this.transform.parent = unEquipParent;
    }

    public void ShowCrossHair(Vector3 _crossHairPos)
    {
        if (!currentCrossHair)
            currentCrossHair = Instantiate(crossHairPrefab);

        currentCrossHair.transform.position = _crossHairPos;
        currentCrossHair.transform.LookAt(Camera.main.transform);
    }

    public void CancelCrossHair()
    {
        if (currentCrossHair)
            Destroy(currentCrossHair);
    }


    public void PickArrow()
    {
        bowAudio.PlayOneShot(drawArrowClip);
        arrowPos.gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        arrowPos.gameObject.SetActive(false);
    }

    public void PullString()
    {
        bowString.position = stringHandPullPos.position;
        bowString.parent = stringHandPullPos;
    }

    public void PullSound()
    {
        bowAudio.PlayOneShot(pullStringClip);   
    }

    public void ReleaseString()
    {
        bowString.position = stringInitialPos.position;
        bowString.parent = stringInitialParent;
    }

    public void Fire(Vector3 _hitPoint)
    {
        bowAudio.PlayOneShot(releaseStringClip);
        Vector3 _dir = _hitPoint - arrowPos.position;
        currentArrow = Instantiate(arrowPrefab, arrowPos.position, arrowPos.rotation);
        currentArrow.AddForce(_dir * arrowForce, ForceMode.Force);
    }

    // mui ten cuong hoa 
     public void Fire1(Vector3 _hitPoint)
    {
        bowAudio.PlayOneShot(releaseStringClip);
        Vector3 _dir = _hitPoint - arrowPos.position;
        currentArrow = Instantiate(arrowPrefab1, arrowPos.position, arrowPos.rotation);
        currentArrow.AddForce(_dir * arrowForce, ForceMode.Force);
    }
}
