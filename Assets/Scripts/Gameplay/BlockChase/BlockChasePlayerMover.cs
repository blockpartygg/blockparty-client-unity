using UnityEngine;

public class BlockChasePlayerMover : MonoBehaviour {
    public BlockChasePlayerState State;
    public PlayerController Controller;
    public float MovementSpeed;
	public float RotationSpeed;
    
    void FixedUpdate() {
        if(Controller.enabled) {
            if(Controller.Direction != Vector2.zero) {
			    Vector3 velocity = new Vector3(Controller.Direction.x, 0f, Controller.Direction.y) * MovementSpeed * Time.deltaTime;
			    transform.Translate(velocity, Space.World);
			    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity * -1f), RotationSpeed * Time.deltaTime);

                State.Position = transform.position;
                State.Velocity = velocity;
		    }
        }
        else {
            transform.position = new Vector3(State.Position.x, 0f, State.Position.z);
        }
    }
}