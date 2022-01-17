using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using UnityKeyCode = UnityEngine.KeyCode;
using UnityVector3 = UnityEngine.Vector3;

public partial class PlayerMovementSystem : SystemBase
{
    private UnityVector3 smoothVelocity;
    protected override void OnUpdate()
    {
        var playerEntity = GetSingletonEntity<Player>();
        var translation = GetComponent<Translation>(playerEntity);
        
        if (UnityInput.GetKeyDown(UnityKeyCode.UpArrow)) translation.Value.z += 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.DownArrow)) translation.Value.z -= 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.RightArrow)) translation.Value.x += 1;
        if (UnityInput.GetKeyDown(UnityKeyCode.LeftArrow)) translation.Value.x -= 1;

        SetComponent(playerEntity, translation);

        var player = GetComponent<Player>(playerEntity);
        var offset = GetComponent<Player>(playerEntity).CameraOffset;
        var goRefs = this.GetSingleton<GameObjectRefs>();

        var cam = goRefs.Camera;
        var fromPos = cam.transform.position;
        var targetPos = translation.Value + offset;
        cam.transform.position =
            Vector3.SmoothDamp(fromPos, targetPos, ref smoothVelocity, player.SmoothTime, player.MaxSpeed);

        var lookDir = translation.Value - targetPos;
        cam.transform.rotation = Quaternion.LookRotation(lookDir, UnityVector3.up);
    }
}