using System;
using UnityEngine;

[Serializable]
public struct CapsuleCast
{
    private Transform _transform;
    private Action<RaycastHit> _callbackAction;
    private Action _endAction;


    public void Awake(Transform transform, Action<RaycastHit> callbackAction, Action endAction)
    {
        _transform = transform;
        _callbackAction = callbackAction;
        _endAction = endAction;
    }

    public float height;
    public float radius;
    public Vector3 offset;
    public LayerMask layerMask;

    private Vector3 direction => _transform.up;

    //TODO: локальный скейл height по скейлу _transform.
    
    private Vector3 CapsulePoint1() => 
        _transform.position + offset - direction * height / 2;

    private Vector3 CapsulePoint2() => 
        CapsulePoint1() + direction * height;

    private void DebugDraw()
    {
        Debug.DrawLine(CapsulePoint1(), CapsulePoint2(), Color.green);
        // Debug.DrawRay(CapsulePoint1(), direction, Color.blue, _height);
    }

    private static bool RaycastCheck(Vector3 origin, Vector3 end, Vector3 dir, float radius, out RaycastHit hit, float maxDistance,
        int layerMask)
    {
        // var value = Physics.CapsuleCast(origin, end, radius, dir, out var hit1, maxDistance, layerMask); //TODO: Разобраться почему не работает
        var value = Physics.Raycast(origin, dir, out RaycastHit hit1, maxDistance, layerMask);
        hit = hit1;
        return value;
    }
    
    public void Update()
    {
        // DebugDraw();
        
        if (!RaycastCheck(CapsulePoint1(), CapsulePoint2(), direction, radius, out RaycastHit hit, height, layerMask))
        {
            _endAction?.Invoke();
            return;
        }

        _callbackAction?.Invoke(hit);
    }
}