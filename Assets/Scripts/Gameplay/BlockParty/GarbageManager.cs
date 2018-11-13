using System;
using UnityEngine;
using BestHTTP.SocketIO;

public class GarbageManager : MonoBehaviour {
	public BlockManager BlockManager;
	float elapsed;
	bool sentGarbage;
	void Start() {
		SocketManager.Instance.Socket.On("blockParty/sendGarbage", HandleSendGarbage);
	}

	void HandleSendGarbage(Socket socket, Packet packet, params object[] args) {
		Debug.Log("Received send garbage");
		int payload = Convert.ToInt32((double)args[0]);
		SpawnGarbage(payload);
	}

	void SpawnGarbage(int payload) {
		for(int column = 0; column < BlockManager.Columns; column++) {
			if(BlockManager.Blocks[column, BlockManager.Rows - 1].State == BlockState.Empty) {
				BlockManager.Blocks[column, BlockManager.Rows - 1].Type = 5;

				if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Idle) {
					BlockManager.Blocks[column, BlockManager.Rows - 1].State = BlockState.Idle;
				}

				if(BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Empty || BlockManager.Blocks[column, BlockManager.Rows - 2].State == BlockState.Falling) {
					BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.Target = BlockManager.Blocks[column, BlockManager.Rows - 2];
					BlockManager.Blocks[column, BlockManager.Rows - 1].Faller.ContinueFalling();
				}
			}
		}
	}

	void Update() {
		elapsed += Time.deltaTime;

		if(elapsed >= 5f && !sentGarbage) {
			sentGarbage = true;
			SocketManager.Instance.Socket.Emit("blockParty/receiveChain", null, null);
			Debug.Log("Sent receive chain");
		}
	}
}