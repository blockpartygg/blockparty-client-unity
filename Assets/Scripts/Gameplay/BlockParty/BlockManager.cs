using UnityEngine;

public class BlockManager : MonoBehaviour {
	public Block[,] Blocks;
	public Block BlockPrefab;
	public GameObject BlockParent;
	public const int Columns = 8, Rows = 9;

	void Awake() {
		Blocks = new Block[Columns, Rows];
	}

	void Start() {
		for(int x = 0; x < Columns; x++) {
			for(int y = 0; y < Rows; y++) {
				Blocks[x, y] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity);
				Blocks[x, y].name = "Block [" + x + ", " + y + "]";
				Blocks[x, y].transform.SetParent(BlockParent.transform, false);
				Blocks[x, y].State = BlockState.Idle;
				Blocks[x, y].Position.x = x;
				Blocks[x, y].Position.y = y;
				Blocks[x, y].Type = GetRandomBlockType(x, y);
			}
		}
	}

	public int GetRandomBlockType(int x, int y) {
		int type;
		do {
			type = Random.Range(0, Block.TypeCount);
		} while((x != 0 && Blocks[x - 1, y].Type == type) || (y != 0 && Blocks[x, y - 1].Type == type));
		return type;
	}
}