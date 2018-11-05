using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
	public Dictionary<string, GameObject> Blocks;
	public GameObject BlockPrefab;

	void Awake() {
		Blocks = new Dictionary<string, GameObject>();
	}

	public void SpawnBlock(string blockId) {
		GameObject block = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity);
		Blocks.Add(blockId, block);
		block.name = "Block " + blockId;
		block.transform.SetParent(transform);
	}

	public void DestroyBlock(string blockId) {
		if(Blocks.ContainsKey(blockId)) {
			Destroy(Blocks[blockId]);
		}
	}
}
