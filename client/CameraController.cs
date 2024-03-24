using Godot;

public partial class CameraController : Node3D
{
    [Export] float mouse_sensitivity = 0.05f;
    [Export] private float _maxVerticalAngle = 90;

    Node3D _cameraRotAxisVertical;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _cameraRotAxisVertical = GetNode<Node3D>("CameraRotAxisVertical");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            _cameraRotAxisVertical.RotateX(-Mathf.DegToRad(mouseEvent.Relative.Y * mouse_sensitivity));
            RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * mouse_sensitivity));

            Vector3 cameraRot = RotationDegrees;
            cameraRot.X = Mathf.Clamp(cameraRot.X, -_maxVerticalAngle, _maxVerticalAngle);
            RotationDegrees = cameraRot;
        }
    }
}