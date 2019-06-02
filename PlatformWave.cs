using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Platform Wave")]
public class PlatformWave : ScriptableObject
{
    [SerializeField] GameObject platfomPath;
    [SerializeField] float moveSpeed;

    public List<Transform> GetPlatformPath()
    {
        var wayPoints = new List<Transform>();

        foreach (Transform child in platfomPath.transform)
        {
            wayPoints.Add(child);
        }

        return wayPoints;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
