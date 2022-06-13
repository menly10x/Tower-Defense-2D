﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;


    //public LayerMask towerPlacementLayer;
    int fingerID = -1;

    Vector3 touchStart;
    public SpriteRenderer map;
    private float mapMinX, mapMinY, mapMaxX, mapMaxY;
    private float zoomOutMin, zoomOutMax;
    private bool isMultiTouch = false;
    private bool isDragging = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

#if !UNITY_EDITOR
     fingerID = 0; 
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        GetMapSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject(fingerID))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = false;
                isMultiTouch = false;
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touchCount == 2)
            {
                if (!UIController.instance.btnBuyTower.activeSelf)
                {
                    isMultiTouch = true;
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    Zoom(difference * 0.01f);
                }
            }
            else if (Input.GetMouseButton(0) && !isMultiTouch)
            {
                if (!UIController.instance.btnBuyTower.activeSelf)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (direction.x > 0.01f || direction.x < -0.01f || direction.y > 0.01f || direction.y < -0.01f)
                    {
                        isDragging = true;
                    }

                    Camera.main.transform.position = ClampCamera(Camera.main.transform.position + direction);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!isDragging && !isMultiTouch)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                    if (hit.collider != null)
                    {
                        if (hit.collider.tag.Equals("TowerPlacement"))
                        {
                            UIController.instance.OpenBtnBuyTower(hit.collider.transform, hit.collider.transform.parent.GetSiblingIndex());
                        }
                    }
                    else
                    {
                        UIController.instance.CloseBtnBuyTower();
                    }
                }
            }

            if (!UIController.instance.btnBuyTower.activeSelf)
            {
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            }
        }
    }

    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
        Camera.main.transform.position = ClampCamera(Camera.main.transform.position);
    }

    private void GetMapSize()
    {
        zoomOutMin = 1;
        zoomOutMax = Camera.main.orthographicSize;

        mapMinX = map.transform.position.x - map.bounds.size.x / 2f;
        mapMaxX = map.transform.position.x + map.bounds.size.x / 2f;

        mapMinY = map.transform.position.y - map.bounds.size.y / 2f;
        mapMaxY = map.transform.position.y + map.bounds.size.y / 2f;
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newy = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newy, targetPosition.z);
    }
    
}
