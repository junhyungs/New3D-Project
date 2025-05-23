using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using PlayerComponent;

public class Ladder : MonoBehaviour, IInteractionGameObject
{
    private BoxCollider _ladderCollider;

    private float _worldLowPositionY;
    private float _worldHighPositionY;

    private void Awake()
    {
        CarculateLadderSize();
    }

    private void CarculateLadderSize()
    {
        _ladderCollider = GetComponent<BoxCollider>();

        var center = _ladderCollider.center;

        var lowPoint = center.y - (_ladderCollider.size.y / 2);
        var highPoint = center.y + (_ladderCollider.size.y / 2);

        var worldLowPosition = transform.TransformPoint(new Vector3(0f, lowPoint, 0f));
        var worldHighPosition = transform.TransformPoint(new Vector3(0f, highPoint, 0f));

        _worldLowPositionY = worldLowPosition.y;
        _worldHighPositionY = worldHighPosition.y;
    }

    public void Interact()
    {
        Player player = PlayerManager.Instance.Player;

        if (player == null)
            return;

        player.StateHandler.ToClimbingState((_worldLowPositionY, _worldHighPositionY));
        player.transform.SetParent(transform);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;

        var lowPosition = new Vector3(transform.position.x, _worldLowPositionY, transform.position.z);
        var highPosition = new Vector3(transform.position.x, _worldHighPositionY, transform.position.z);

        Gizmos.DrawWireSphere(lowPosition, 0.5f);
        Gizmos.DrawWireSphere(highPosition, 0.5f);
    }
}
