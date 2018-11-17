using System;
using UnityEngine;
using BestHTTP.SocketIO;

public class GarbageManager : MonoBehaviour {
	public BlockManager BlockManager;

	void Start() {
		//SocketManager.Instance.Socket.On("blockParty/sendGarbage", HandleSendGarbage);
	}

	void HandleSendGarbage(Socket socket, Packet packet, params object[] args) {
		int payload = Convert.ToInt32((double)args[0]);
		SpawnGarbage(payload);
	}

	void SpawnGarbage(int payload) {
        BlockManager.Blocks[0, BlockManager.Rows - 1].Garbage.Width = 6;
        BlockManager.Blocks[0, BlockManager.Rows - 1].Garbage.Height = 1;

        bool isNeighbor = false;
		for(int column = 0; column < BlockManager.Columns; column++) {
			BlockManager.Blocks[column, BlockManager.Rows - 1].Type = 5;
            BlockManager.Blocks[column, BlockManager.Rows - 1].Garbage.IsNeighbor = isNeighbor;
            if(!isNeighbor)
            {
                isNeighbor = true;
            }

			if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Idle) {
				BlockManager.Blocks[column, BlockManager.Rows - 1].State = BlockState.Idle;
			}

			if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Empty || BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Falling) {
				BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.Target = BlockManager.Blocks[column, BlockManager.Rows - 2];
				BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.ContinueFalling();
			}
		}
	}

	void Update() {
		//SocketManager.Instance.Socket.Emit("blockParty/receiveChain", null, null);

		if(Input.GetKeyDown("space")) {
			SpawnGarbage(0);
		}
	}
}