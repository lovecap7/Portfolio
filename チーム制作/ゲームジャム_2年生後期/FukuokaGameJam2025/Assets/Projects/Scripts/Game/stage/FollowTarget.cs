using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target; // 追従する対象（プレイヤーなど）
    [SerializeField] private float smoothSpeed = 5f; // 追従のなめらかさ
    [SerializeField] private Vector3 offset; // ずらしたい距離（例: カメラを少し上に）

    void LateUpdate()
    {
        if (target == null) return;

        // 目標位置
        Vector3 desiredPosition = target.position + offset;

        // 線形補間(Lerp)でなめらかに追従
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 実際の位置を更新
        transform.position = smoothedPosition;
    }
}