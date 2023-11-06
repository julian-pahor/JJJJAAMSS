using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPool : MonoBehaviour
{
    public ObjectPooler bulletPool;
    public ObjectPooler spikePool;
    public ObjectPooler orbiterPool;

    [ContextMenu("Clear screen")]
    public void ClearBullets()
    {
        bulletPool.DespawnAll();
        spikePool.DespawnAll();
        orbiterPool.DespawnAll();
    }
}
