using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AirBridge : MonoBehaviour
{
    [SerializeField] private GameObject _bridgeSegmentPrefab;
    [SerializeField] private int _amountOfSegmentsToSpawn = 10;
    [SerializeField] private float _spawnInterval = 0.1f;
    [SerializeField] private float _spawnOffset = 2f;
    public void Appear()
    {
        Debug.Log("Bridge appears");
        StartCoroutine(SpawnSegments());
    }

    private IEnumerator SpawnSegments()
    {
        for (int i = 0; i < _amountOfSegmentsToSpawn; i++)
        {
            Vector3 positionToSpawnAt = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + (i*_spawnOffset));
            Instantiate(_bridgeSegmentPrefab, positionToSpawnAt, new Quaternion(0, 1, 0, 0), transform);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
