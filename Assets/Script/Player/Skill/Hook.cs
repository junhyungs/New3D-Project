using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using EnumCollection;

public class Hook : PlayerSkill, ISkill
{
    public Hook(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = false;
        MakeProjectile(ObjectKey.PlayerHookPrefab);
    }

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private readonly int _hook = Animator.StringToHash("Hook");

    public override void Fire()
    {
        var hook = ObjectPool.Instance.DequeueGameObject(ObjectKey.PlayerHookPrefab);
        hook.transform.SetParent(_skillInfo.FireTransform);
        hook.transform.localPosition = Vector3.zero;
        hook.transform.localRotation = Quaternion.identity;

        var hookComponent = hook.GetComponent<PlayerHook>();
        if (hookComponent != null)
        {
            hookComponent.CallBackCollisionVector3(SetMovePosition);
            hookComponent.FireHook();
        }
        else
        {
            HookFail(hook);
        }
    }

    private void HookFail(GameObject hookObject)
    {
        _animator.SetTrigger(_skillFail);
        EndSkill = true;

        hookObject.transform.parent = null;
        ObjectPool.Instance.EnqueueGameObject(ObjectKey.PlayerHookPrefab, hookObject);
    }

    public void SetMovePosition(Vector3 targetPosition, PlayerHook hook)
    {
        if(targetPosition != Vector3.zero)
        {
            _animationEvent.StartCoroutine(HookMovement(targetPosition, hook));
            return;
        }

        HookFail(hook.gameObject);
    }

    private IEnumerator HookMovement(Vector3 targetPosition, PlayerHook hook)
    {
        var hookObject = hook.gameObject;
        hookObject.transform.parent = null;

        var enableChains = hook.EnableChains;

        AnimatorSetBool(true);

        Vector3 endPos = new Vector3(targetPosition.x, _rigidBody.position.y, targetPosition.z);
        Vector3 startToEnd = (endPos - _rigidBody.position).normalized;

        LayerMask targetLayer = LayerMask.GetMask("Chain");

        while (Vector3.Dot(startToEnd, endPos - _rigidBody.position) > 0f)
        {
            DisableChain(enableChains, targetLayer);
            Vector3 moveVector = startToEnd * 10f * Time.fixedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + moveVector);

            yield return _waitForFixedUpdate;
        }

        AnimatorSetBool(false);
        EndSkill = true;
        ObjectPool.Instance.EnqueueGameObject(ObjectKey.PlayerHookPrefab, hookObject);
    }

    private void DisableChain(Stack<GameObject> chainStack, LayerMask targetLayer)
    {
        Vector3 checkPoint = _player.transform.position + Vector3.up * 0.7f;
        bool isCollision = Physics.CheckSphere(checkPoint, 1f, targetLayer);

        if (isCollision && chainStack.Count > 0)
        {
            var chain = chainStack.Pop();
            chain.gameObject.SetActive(false);
        }
    }

    private void AnimatorSetBool(bool setbool)
    {
        _player.gameObject.layer = setbool ? LayerMask.NameToLayer("Hookshot_Fly")
            : LayerMask.NameToLayer("Player");

        _animator.SetBool(_hook, setbool);
    }

    public override void Reloading() { }
    public override void OnUpdateSkill() { }
    
    public override void Execute()
    {
        _animator.SetTrigger(_skill);
        _animator.SetInteger(_skillEquals, _skillInfo.AnimationCode);

        Fire();
    }

    public override void InitializeSkill(SkillInfo info)
    {
        _skillInfo = info;
    }
}
