using UnityEngine;

public class ChargerAttackingState : StateBase
{
    private ChargerEnemyController controller;

    public ChargerAttackingState(ChargerEnemyController _controller) {
        controller = _controller;
    }

    public override void OnEnter() {
        controller.TryAttack();
    }
}