using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoints : MonoBehaviour
{
    public static RespawnPoints Instance;
    public List<Vector2> SpawnPoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }


        Vector3 point1 = new Vector3(-18, 0, -18);
        Vector3 point2 = new Vector3(-11, 0, 18);
        Vector3 point3 = new Vector3(11, 0, 12);
        Vector3 point4 = new Vector3(0, 0, 0);
        Vector3 point5 = new Vector3(-25, 0, 6);
        Vector3 point6 = new Vector3(-1.8f, 0, 30);
        Vector3 point7 = new Vector3(-1.8f, 0, -34);

        SpawnPoints.Add(point1);
        SpawnPoints.Add(point2);
        SpawnPoints.Add(point3);
        SpawnPoints.Add(point4);
        SpawnPoints.Add(point5);
        SpawnPoints.Add(point6);
        SpawnPoints.Add(point7);

    }




}
