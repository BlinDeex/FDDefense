using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractions : MonoBehaviour
{
    public GameObject rangeIndicator;
    public Collider2D rangeCollider;
    public GameObject parentTurret;

    public int turretID;
    private void Awake()
    {
        parentTurret = transform.parent.gameObject;
        rangeCollider = transform.GetComponent<Collider2D>();
        rangeIndicator = transform.parent.Find("RangeIndicator").gameObject;
        float range = gameObject.transform.parent.gameObject.GetComponent<BasicTurretLogic>().range;
        rangeCollider.transform.localScale = new Vector3(range, range, range);
    }
    void Start()
    {
        turretID = CentralSystem.Instance.AddTurretIndicator(parentTurret, rangeIndicator);
    }

    public void UpdateRangeIndicator()
    {
        float range = parentTurret.GetComponent<BasicTurretLogic>().range;
        rangeIndicator.transform.localScale = new Vector3(range, range, range);
    }
}
