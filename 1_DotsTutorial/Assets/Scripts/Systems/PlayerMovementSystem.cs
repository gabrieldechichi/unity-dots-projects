using Unity.Entities;
using Unity.Transforms;
using UnityInput = UnityEngine.Input;
using UnityKeyCode = UnityEngine.KeyCode;

public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var player = GetSingletonEntity<Player>();
        var translation = GetComponent<Translation>(player);
        
        if (UnityInput.GetKeyDown(UnityKeyCode.UpArrow)) translation.Value.z += 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.DownArrow)) translation.Value.z -= 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.RightArrow)) translation.Value.x += 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.LeftArrow)) translation.Value.x -= 1;

        SetComponent(player, translation);

        var offset = GetComponent<Player>(player).CameraOffset;
        var goRefs = this.GetSingleton<GameObjectRefs>();

        var cam = goRefs.Camera;
        cam.transform.position = translation.Value + offset;
        cam.transform.LookAt(translation.Value);
    }
}