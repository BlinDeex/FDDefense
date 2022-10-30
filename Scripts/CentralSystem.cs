using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralSystem : MonoBehaviour
{

    public static CentralSystem Instance;


    public int turretRangeIndicatorsID = 0;

    public Dictionary<int, (GameObject, GameObject)> turretRangeIndicators = new();

    Vector3 touchPosWorld;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        CheckTouchInput();
    }

    void CheckTouchInput()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new(touchPosWorld.x, touchPosWorld.y);

            RaycastHit2D[] hitsInfo = Physics2D.RaycastAll(touchPosWorld2D, Camera.main.transform.forward);

            foreach(RaycastHit2D hitInfo in hitsInfo)
            {
                if (hitInfo.collider != null)
                {
                    if (hitInfo.transform.gameObject.TryGetComponent(out TurretInteractions component))
                    {
                        component.UpdateRangeIndicator();

                        if (!turretRangeIndicators[component.turretID].Item2.activeSelf)
                        {
                            RemoveActiveTurretIndicators();
                            turretRangeIndicators[component.turretID].Item2.SetActive(true);
                        }
                        else
                        {
                            turretRangeIndicators[component.turretID].Item2.SetActive(false);
                        }
                        break;
                    }
                }
            }
        }
    }

    public int AddTurretIndicator(GameObject turret, GameObject turretIndicator)
    {
        turretRangeIndicators.Add(turretRangeIndicatorsID, (turret, turretIndicator));
        turretRangeIndicatorsID++;
        return turretRangeIndicatorsID - 1;
    }

    public void RemoveTurretIndicator(int id)
    {
        if (turretRangeIndicators.ContainsKey(id))
        {
            turretRangeIndicators.Remove(id);
        }
    }

    public void RemoveActiveTurretIndicators()
    {
        foreach (int id in turretRangeIndicators.Keys)
        {
            if (turretRangeIndicators[id].Item2.activeSelf)
            {
                turretRangeIndicators[id].Item2.SetActive(false);
            }
        }
    }
}
