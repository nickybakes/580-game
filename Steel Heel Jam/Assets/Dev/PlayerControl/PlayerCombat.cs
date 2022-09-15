using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerCombat : MonoBehaviour
{
    private StarterAssetsInputs _input;
    private GameObject _hitbox;

    private DefaultState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _hitbox = transform.GetChild((int)PlayerChild.Hitbox).gameObject;

        weaponState = new DefaultState(1, _hitbox);
        weaponState.InitHitbox();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.attack)
        {
            weaponState.Attack();
            _input.attack = false;
        }
    }
}
