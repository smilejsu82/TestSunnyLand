using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMovement : MonoBehaviour
{
    public enum eDirection
    {
        None = 0,
        Left = -1,
        Right = 1
    }

    public Transform[] arrLayers;
    public eDirection moveDir;
    public float speed = 1;
    public float leftPosX;
    public float rightPosX;


    private bool isMove = false;
    private Coroutine routine;

    public void MoveStart()
    {
        this.isMove = true;
        if (this.routine != null)
        {
            StopCoroutine(this.routine);
        }
        this.routine = StartCoroutine(this.MoveImpl());
    }

    private IEnumerator MoveImpl()
    {
        while (true)
        {
            if (this.isMove == false)
                break;

            var dir = Vector3.zero;
            if (this.moveDir == eDirection.Left)
            {
                dir = Vector3.left;
            }
            else if (this.moveDir == eDirection.Right)
            {
                dir = Vector3.right;
            }

            foreach (var layer in this.arrLayers)
            {
                layer.Translate(dir * this.speed * Time.deltaTime);

                if (dir == Vector3.left)
                {
                    if (layer.transform.position.x <= this.leftPosX)
                    {
                        var tvec = layer.transform.position;
                        tvec.x = this.rightPosX;
                        layer.transform.position = tvec;
                    }
                }
                else if (dir == Vector3.right)
                {
                    if (layer.transform.position.x >= this.rightPosX)
                    {
                        var tvec = layer.transform.position;
                        tvec.x = this.leftPosX;
                        layer.transform.position = tvec;
                    }
                }
                
            }

            yield return null;
        }
    }

    public void MoveStop()
    {
        this.isMove = false;
    }
}
