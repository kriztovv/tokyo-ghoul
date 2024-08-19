using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scatter : MonoBehaviour
{
    public GameObject prefabToScatter;
    public int numberOfPrefabs = 10;
    public Vector3 scatterAreaSize = new Vector3(10f, 1f, 10f);

    void Start()
    {
        Scatter();
    }

    void Scatter()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(prefabToScatter, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-scatterAreaSize.x / 2f, scatterAreaSize.x / 2f),
                                                                  1, 
                                                                  Random.Range(-scatterAreaSize.z / 2f, scatterAreaSize.z / 2f));
        return randomPosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, scatterAreaSize);
    }
}
