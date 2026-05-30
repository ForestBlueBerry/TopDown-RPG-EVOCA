using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.AudioSettings;


public class Spawner : MonoBehaviour
{
    private Transform player;
    public List<DataForSpawn> setsMobs;
    public LayerMask layer;
    private HashSet<EnemySO> activeZone = new HashSet<EnemySO>();

    public float waitForSecondsSpawn;

    private Dictionary<int, List<GameObject>> trackedMobs = new Dictionary<int, List<GameObject>>();
    private HashSet<int> activeProcessing = new HashSet<int>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < setsMobs.Count; i++)
        {
            trackedMobs.Add(i, new List<GameObject>());
        }

        InvokeRepeating(nameof(SmartSpawns), 1, 3);
    }

    public void SmartSpawns()
    {
        for (int i = 0; i < setsMobs.Count; i++)
        {
            var data = setsMobs[i];
            if (data.collider == null || data.Mobs.enemySO == null) continue;
            trackedMobs[i].RemoveAll(item => item == null);
            Vector2 distToPlay = data.collider.ClosestPoint(player.position);
            float distSq = (player.position - (Vector3)distToPlay).sqrMagnitude;

            if (distSq < 900f) 
            {
                if (!activeProcessing.Contains(i))
                {
                    int currentCount = trackedMobs[i].Count;
                    if (currentCount < data.Mobs.count)
                    {
                        int toSpawn = data.Mobs.count - currentCount;
                        StartCoroutine(CooldownSpawn(i, toSpawn));
                    }
                }
            }
            else if (distSq > 10000f && trackedMobs[i].Count > 0)
            {
                foreach (GameObject mob in trackedMobs[i])
                {
                    if (mob != null) Destroy(mob);
                }
                trackedMobs[i].Clear(); 
            }
        }
    }

    IEnumerator CooldownSpawn(int slotIndex, int amount)
    {
        activeProcessing.Add(slotIndex);
        var data = setsMobs[slotIndex];

        for (int i = 0; i < amount; i++)
        {
            Vector3 point = RandomPoint(data.collider);
            if (point != Vector3.zero)
            {
                GameObject newMob = Instantiate(data.Mobs.enemySO.enemyPrefab, point, Quaternion.identity);

                trackedMobs[slotIndex].Add(newMob);

                if (newMob.TryGetComponent(out EnemyController enemy))
                {
                    enemy.enemySO = data.Mobs.enemySO;
                    enemy.bounds = data.collider.bounds;
                }
            }
            yield return new WaitForSeconds(waitForSecondsSpawn);
        }
        activeProcessing.Remove(slotIndex);
    }

    public Vector3 RandomPoint(BoxCollider2D col)
    {
        Bounds bounds = col.bounds;
        for (int i = 0; i < 5; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 randPoint = new Vector3(x, y, 0);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randPoint, out hit, 5f, NavMesh.AllAreas)) return hit.position;
        }
        return Vector3.zero;
    }

}
[System.Serializable]
public struct SetMobs
{
    public EnemySO enemySO;
    public int count;
}
[System.Serializable]
public struct DataForSpawn
{
    public BoxCollider2D collider;
    public SetMobs Mobs;
}