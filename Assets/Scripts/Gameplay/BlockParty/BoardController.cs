using UnityEngine;

public class BoardController : MonoBehaviour {
	public BlockManager BlockManager;
	public Block SelectedBlock;
	public Camera Camera;

	void Update() {
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null && hit.collider.name.Contains("Block")) {
				Block block = hit.collider.gameObject.GetComponent<Block>();
				if(block.State == BlockState.Idle && block.Position.y < BlockManager.Rows - 1) {
					SelectedBlock = block;
				}
			}
		}

		if(Input.GetMouseButtonUp(0)) {
			SelectedBlock = null;
		}
	}
}