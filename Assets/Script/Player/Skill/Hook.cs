using EnumCollection;
using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class Hook : PlayerSkill, ISkill
{
    public Hook(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = false;

        _objectKey = ObjectKey.PlayerHookPrefab;
        MakeProjectile(_objectKey);
    }

    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private readonly int _hook = Animator.StringToHash("Hook");

    public override void Fire()
    {
        var hook = ObjectPool.Instance.DequeueGameObject(_objectKey);
        hook.transform.SetParent(_skillInfo.FireTransform);
        hook.transform.localPosition = Vector3.zero;
        hook.transform.localRotation = Quaternion.identity;

        var hookComponent = hook.GetComponent<HookObject>();
        if (hookComponent != null)
        {
            hookComponent.CallBackCollisionVector3(SetMovePosition);
            hookComponent.SetData(0, _data.ProjectileSpeed, _data.ProjectileDamage);
            hookComponent.Fire();
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
        ObjectPool.Instance.EnqueueGameObject(_objectKey, hookObject);
    }

    public void SetMovePosition(Vector3 targetPosition, HookObject hook)
    {
        if(targetPosition != Vector3.zero)
        {
            _animationEvent.StartCoroutine(HookMovement(targetPosition, hook));
            return;
        }

        HookFail(hook.gameObject);
    }

    private IEnumerator HookMovement(Vector3 targetPosition, HookObject hook)
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
        ObjectPool.Instance.EnqueueGameObject(_objectKey, hookObject);
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
        LookAt();

        _animator.SetTrigger(_skill);
        _animator.SetInteger(_skillEquals, _skillInfo.AnimationCode);

        Fire();
    }

    private void LookAt()
    {
        Vector3 lookPos = new Vector3(_playerPlane.Point.x,
            _player.transform.position.y, _playerPlane.Point.z);

        var distance = Vector3.Distance(lookPos, _player.transform.position);
        if(distance > 0.1f)
            _player.transform.LookAt(lookPos);
    }

    public override void InitializeSkill(SkillInfo info, PlayerSkillData data)
    {
        _data = data;
        _skillInfo = info;
    }
}
