using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.Networking.Transport.Error;

public class SpawnPointManager: MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    int lastSpawnPoint=-1;

    public Vector3 GetRandomSpawnPoint()
    {
        NavMeshHit hit;
        int i=-1;

        while(lastSpawnPoint == i || i==-1)
        {
            i= Random.Range(0, spawnPoints.Length);            
        }
        lastSpawnPoint=i;

        Debug.Log(i);

        if(spawnPoints.Length != 0 && NavMesh.SamplePosition( spawnPoints[i].position, out hit, 1f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else{
            return Vector3.zero;
        }
    }
}