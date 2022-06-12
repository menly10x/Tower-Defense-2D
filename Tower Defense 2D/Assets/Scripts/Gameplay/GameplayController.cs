using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameplayController : MonoBehaviour
{
    public Transform gameplayParent;

    //public LayerMask towerPlacementLayer;
    int fingerID = -1;

    Vector3 touchStart;
    public SpriteRenderer map;
    private float mapMinX, mapMinY, mapMaxX, mapMaxY;
    private float zoomOutMin, zoomOutMax;
    private bool isMultiTouch = false;

    private void Awake()
    {
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
                isMultiTouch = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider != null)
                {
                    if (hit.collider.tag.Equals("TowerPlacement"))
                    {
                        UIController.instance.btnBuyTower.transform.DOKill();
                        UIController.instance.btnBuyTower.SetActive(false);
                        UIController.instance.btnBuyTower.transform.position = hit.collider.transform.position;
                        UIController.instance.btnBuyTower.transform.localScale = new Vector3(0, 0, 0);
                        UIController.instance.btnBuyTower.SetActive(true);
                        UIController.instance.btnBuyTower.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
                    }
                }
                else
                {
                    if (UIController.instance.btnBuyTower.transform.localScale == new Vector3(1, 1, 1))
                    {
                        UIController.instance.btnBuyTower.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                        {
                            UIController.instance.btnBuyTower.SetActive(false);
                        });
                    }
                    else
                    {
                        touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                }
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

                    zoom(difference * 0.01f);
                }
            }
            else if (Input.GetMouseButton(0) && !isMultiTouch)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider == null && !UIController.instance.btnBuyTower.activeSelf)
                {
                    Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    Camera.main.transform.position = ClampCamera(Camera.main.transform.position + direction);
                }
            }
        }

        if (!UIController.instance.btnBuyTower.activeSelf)
        {
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void zoom(float increment)
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
