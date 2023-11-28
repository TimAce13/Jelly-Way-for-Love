 using UnityEngine;

public class PlayerControls3D : MonoBehaviour
{
    [SerializeField] public float runSpeed;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _moveSpeed;

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        _rigidbody.AddForce(0, 0, runSpeed * Time.deltaTime);

        /*        Vector3 direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;
                _rigidbody.AddForce(direction * _moveSpeed * Time.deltaTime, ForceMode.VelocityChange);*/
    }
}
