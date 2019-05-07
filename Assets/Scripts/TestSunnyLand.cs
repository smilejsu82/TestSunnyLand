using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestSunnyLand : MonoBehaviour
{
    public enum ePlayerState
    {
        None = -1,
        Idle, 
        Jump,
        Run
    }
    public LayerMovement[] arrLayerMovement;
    public GameObject playerGo;
    public Button[] arrBtns;
    private Animator anim;
    private Coroutine routine;

    private void Start()
    {
        int i = 0;
        foreach (var btn in this.arrBtns)
        {
            var btnIdx = i;
            btn.onClick.AddListener(() => {
                this.ChangeState((ePlayerState)btnIdx);
            });
            i++;
        }
        foreach (var layerMovement in this.arrLayerMovement)
        {
            layerMovement.MoveStart();
        }

        this.anim = this.playerGo.GetComponent<Animator>();

        this.ChangeState(ePlayerState.Run);
        
    }

    private void ChangeState(ePlayerState state)
    {
        switch (state)
        {
            case ePlayerState.Idle:
                {
                    this.anim.Play("player_idle");
                    foreach (var layerMovement in this.arrLayerMovement)
                    {
                        layerMovement.MoveStop();
                    }
                }
                break;

            case ePlayerState.Jump:
                {
                    if (this.routine != null)
                    {
                        StopCoroutine(this.routine);
                    }
                    this.routine = StartCoroutine(this.JumpImpl());

                }
                break;

            case ePlayerState.Run:
                {
                    foreach (var layerMovement in this.arrLayerMovement)
                    {
                        layerMovement.MoveStart();
                    }

                    this.anim.SetTrigger("Run");
                }
                break;
        }
    }

    private IEnumerator JumpImpl()
    {
        this.anim.SetTrigger("Jump");
        var jumpAnimClip = this.anim.runtimeAnimatorController.animationClips.Where(x => x.name == "player_jump").First();

        Debug.Log(jumpAnimClip);

        yield return new WaitForSeconds(jumpAnimClip.length);

        this.ChangeState(ePlayerState.Run);
    }


}
