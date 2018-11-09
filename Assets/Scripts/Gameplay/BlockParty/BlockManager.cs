using UnityEngine;

public class BlockManager : MonoBehaviour {
	public Block[,] Blocks;
	public Block BlockPrefab;
	public GameObject BlockParent;
	public BlockPartyMinigameManager MinigameManager;
	public const int Columns = 6, Rows = 13; // 12 visible and 1 for new blocks
	const int survivalModeStartingRows = 6;

	void Awake() {
		Application.targetFrameRate = 60;
		Blocks = new Block[Columns, Rows];
		for(int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++) {
				Blocks[column, row] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity);
				Blocks[column, row].name = "Block [" + column + ", " + row + "]";
				Blocks[column, row].transform.SetParent(BlockParent.transform, false);
				Blocks[column, row].Column = column;
				Blocks[column, row].Row = row;
			}
		}
	}

	void Start() {
		if(MinigameManager.Mode == BlockPartyModes.TimeAttack) {
			for(int row = 0; row < Rows; row++) {
				for(int column = 0; column < Columns; column++) {
					Blocks[column, row].State = BlockState.Idle;
					Blocks[column, row].Type = GetRandomBlockType(column, row);
				}
			}
		}
		else if(MinigameManager.Mode == BlockPartyModes.Survival) {
			for(int row = 0; row < survivalModeStartingRows; row++) {
				for(int column = 0; column < Columns; column++) {
					Blocks[column, row].State = BlockState.Idle;
					Blocks[column, row].Type = GetRandomBlockType(column, row);
				}
			}
		}
	}

	public int GetRandomBlockType(int column, int row) {
		int type;
		do {
			type = Random.Range(0, Block.TypeCount);
		} while((column != 0 && Blocks[column - 1, row].Type == type) || (row != 0 && Blocks[column, row - 1].Type == type));
		return type;
	}
}