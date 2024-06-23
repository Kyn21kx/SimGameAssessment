using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";

    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private float m_speed;

    private Rigidbody2D m_rig;
    private Vector2 m_movementInput;

    private void Start()
    {
        Assert.IsNotNull(this.m_animator, "The animator must be set through the editor!");
        this.m_rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        this.HandleInput();
    }

    private void FixedUpdate()
    {
        //Note: Fixed delta time is unnecessary since, well, it's fixed
        //however, added it to be able to change it in the future (hypotheticalxw)
        this.MoveTowards(this.m_movementInput, this.m_speed, Time.fixedDeltaTime);
    }

    private void MoveTowards(Vector2 direction, float speed, float timeStep)
    {
        const string animMovementParam = "MovingBlend";
        Vector2 normalizedDirection = direction.normalized;
        //TODO: Maybe add a smoothing function like a non linear interpolation for smooth start
        this.m_rig.velocity = normalizedDirection * speed * timeStep;
        float currentSpeed = this.m_rig.velocity.magnitude;

        //Animator / Visual stuff
        this.m_animator.SetFloat(animMovementParam, currentSpeed);
        FacingDirection dir = (FacingDirection)Mathf.CeilToInt(normalizedDirection.x);
        this.FaceTowards(dir);
    }

    private void FaceTowards(FacingDirection direction)
    {
        if (direction == FacingDirection.None) return;
        const float offsetChangeFactor = 0.064f; // This value comes from the animation asset pack we got c:
        float parsedDirection = (float)direction;
        this.m_animator.transform.localScale = new Vector3(1f, 1f, parsedDirection);

        Vector3 pos = this.m_animator.transform.localPosition;
        pos.x = offsetChangeFactor * -parsedDirection;
        this.m_animator.transform.localPosition = pos;
    }

    /// <summary>
    /// Exclusively handle input on the Update function due to dT fidelity
    /// Any actual movement behaviour should be handled on FixedUpdate, since
    /// it's physics based movement.
    /// </summary>
    private void HandleInput()
    {
        this.m_movementInput = new Vector2(
            Input.GetAxisRaw(HORIZONTAL_AXIS),
            Input.GetAxisRaw(VERTICAL_AXIS)
        );
    }
}
