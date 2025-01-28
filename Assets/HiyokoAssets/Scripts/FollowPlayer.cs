using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;  // プレイヤーの位置情報を保持する
    private Vector3 offset;  // プレイヤーとカメラとの初期位置関係を保持する

    // ゲームが始まるときに一度だけ呼ばれるメソッド
    void Start()
    {
        // targetに何も設定されていなければPlayerタグを持つオブジェクトを検索して設定
        if (!target)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                target = go.transform;
            }
        }
        // カメラとプレイヤーの初期位置関係（オフセット）を記録する
        offset = transform.position - target.position;
    }

    // 各フレームの最後に呼ばれるメソッド
    void LateUpdate()
    {
        // プレイヤーの位置にオフセットを加えることで、カメラがプレイヤーを追従する
        transform.position = target.position + offset;
    }
}
