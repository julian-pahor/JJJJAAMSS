using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public ObjectPooler origin;

	public delegate void Action();
	public Action OnDespawn;
	public bool despawned = false;
	/// <summary>
	/// Despawns the object
	/// </summary>
	public void Despawn()
	{
		origin.Despawn(this);
	}	
}
