using UnityEngine;
using System;

public class BlockManager : MonoBehaviour {
	public Block[,] Blocks;
	public Block[] NewRowBlocks;
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
                Blocks[column, row].State = BlockState.Empty;
                Blocks[column, row].Type = -1;
			}
		}

		if(MinigameManager.Mode == BlockPartyModes.Survival) {
			NewRowBlocks = new Block[Columns];
			for(int column = 0; column < Columns; column++) {
				NewRowBlocks[column] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity);
				NewRowBlocks[column].name = "New Row Block [" + column + "]";
				NewRowBlocks[column].transform.SetParent(BlockParent.transform, false);
				NewRowBlocks[column].Column = column;
				NewRowBlocks[column].Row = -1;
			}
		}

		GameManager.Instance.StateChanged += HandleStateChanged;
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

			CreateNewRowBlocks();
		}
	}

	public int GetRandomBlockType(int column, int row) {
		int type;
		do {
			type = UnityEngine.Random.Range(0, Block.TypeCount);
		} while((column != 0 && Blocks[column - 1, row].Type == type) || (row != 0 && Blocks[column, row - 1].Type == type));
		return type;
	}

	public void CreateNewRowBlocks() {
		for(int column = 0; column < Columns; column++) {
			NewRowBlocks[column].State = BlockState.Idle;
			NewRowBlocks[column].Type = GetRandomBlockType(column, 0);
		}
	}

	void HandleStateChanged(object sender, EventArgs args) {
		if(GameManager.Instance.State == GameManager.GameState.MinigameEnd) {
			KillBlocks();
		}
	}

	public void KillBlocks() {
		for(int column = 0; column < Columns; column++) {
			for(int row = 0; row < Rows; row++) {
				Blocks[column, row].Clearer.Clear(true);
				Blocks[column, row].Killer.Kill();
			}
		}
	}
}