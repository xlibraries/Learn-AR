using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpwanManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_RaycastHitList = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spwanablePrefab;

    Camera arCam;
    GameObject spwanedObject;

    // Start is called before the first frame update
    void Start()
    {
        spwanedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if(m_RaycastManager.Raycast(Input.GetTouch(0).position, m_RaycastHitList))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && spwanedObject == null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag == "Spwanable")
                    {
                        spwanedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpwanPrefab(m_RaycastHitList[0].pose.position);
                    }
                }

                else if(Input.GetTouch(0).phase == TouchPhase.Moved && spwanedObject != null)
                {
                    spwanedObject.transform.position = m_RaycastHitList[0].pose.position;
                }

                if(Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    spwanedObject = null;
                }
            }
        }
    }

    private void SpwanPrefab(Vector3 spwanPosition)
    {
        spwanedObject = Instantiate(spwanablePrefab, spwanPosition, Quaternion.identity);
    }
}
