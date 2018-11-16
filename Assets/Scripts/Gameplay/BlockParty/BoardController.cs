using UnityEngine;

public class BoardController : MonoBehaviour {
	public BlockManager BlockManager;
	public Block SelectedBlock;
	public Camera Camera;
	public AudioSource AudioSource;
	public AudioClip SlideClip;

	void Update() {
		if(GameManager.Instance.State == GameManager.GameState.MinigamePlay) {
			if(Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null && hit.collider.name.Contains("Block")) {
					Block block = hit.collider.gameObject.GetComponent<Block>();
					if(block.State == BlockState.Idle && block.Row >= 0 && block.Row < BlockManager.Rows - 1) {
						SelectedBlock = block;
					}
				}
			}

			if(Input.GetMouseButtonUp(0)) {
				SelectedBlock = null;
			}

			if(SelectedBlock != null) {
				float leftEdge = SelectedBlock.transform.parent.position.x + SelectedBlock.Column - SelectedBlock.transform.localScale.x / 2;
				float rightEdge = SelectedBlock.transform.parent.position.x + SelectedBlock.Column + SelectedBlock.transform.localScale.x / 2;
				Block leftBlock = null, rightBlock = null;
				Vector3 mousePosition = Input.mousePosition;

				if(Camera.ScreenToWorldPoint(mousePosition).x < leftEdge &&
					SelectedBlock.State == BlockState.Idle &&
					SelectedBlock.Column - 1 >= 0 &&
					(BlockManager.Blocks[SelectedBlock.Column - 1, SelectedBlock.Row].State == BlockState.Idle ||
					BlockManager.Blocks[SelectedBlock.Column - 1, SelectedBlock.Row].State == BlockState.Empty) &&
					(SelectedBlock.Row + 1 == BlockManager.Rows || (SelectedBlock.Row + 1 < BlockManager.Rows && 
					BlockManager.Blocks[SelectedBlock.Column - 1, SelectedBlock.Row + 1].State != BlockState.Falling &&
					BlockManager.Blocks[SelectedBlock.Column - 1, SelectedBlock.Row + 1].State != BlockState.WaitingToFall))) {
						leftBlock = BlockManager.Blocks[SelectedBlock.Column - 1, SelectedBlock.Row];
						rightBlock = SelectedBlock;
						SelectedBlock = leftBlock;
				}

				if(Camera.ScreenToWorldPoint(mousePosition).x > rightEdge &&
					SelectedBlock.State == BlockState.Idle &&
					SelectedBlock.Column + 1 < BlockManager.Columns &&
					(BlockManager.Blocks[SelectedBlock.Column + 1, SelectedBlock.Row].State == BlockState.Idle ||
					BlockManager.Blocks[SelectedBlock.Column + 1, SelectedBlock.Row].State == BlockState.Empty) &&
					(SelectedBlock.Row + 1 == BlockManager.Rows || (SelectedBlock.Row + 1 < BlockManager.Rows && 
					BlockManager.Blocks[SelectedBlock.Column + 1, SelectedBlock.Row + 1].State != BlockState.Falling &&
					BlockManager.Blocks[SelectedBlock.Column + 1, SelectedBlock.Row + 1].State != BlockState.WaitingToFall))) {
						leftBlock = SelectedBlock;
						rightBlock = BlockManager.Blocks[SelectedBlock.Column + 1, SelectedBlock.Row];
						SelectedBlock = rightBlock;
				}

				if(leftBlock != null && rightBlock != null) {
					SetupSlide(leftBlock, SlideDirection.Right);
					SetupSlide(rightBlock, SlideDirection.Left);

					leftBlock.Slider.Slide(SlideDirection.Right);
					rightBlock.Slider.Slide(SlideDirection.Left);

					AudioSource.clip = SlideClip;
					AudioSource.Play();
				}
			}
		}
	}

	void SetupSlide(Block block, SlideDirection direction) {
		Block target = direction == SlideDirection.Left ? BlockManager.Blocks[block.Column - 1, block.Row] : BlockManager.Blocks[block.Column + 1, block.Row];
		block.Slider.TargetState = target.State;
		block.Slider.TargetType = target.Type;
	}
}