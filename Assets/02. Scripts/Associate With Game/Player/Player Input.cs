using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action OnLeftClickDown;
    public static event Action OnLeftClickHold;

    public static event Action OnRightClickDown;
    
    public static event Action<bool> OnShiftHold;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnLeftClickDown?.Invoke();
        }
        else if(Input.GetKey(KeyCode.Mouse0))
        {
            OnLeftClickHold?.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnRightClickDown?.Invoke();
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            OnShiftHold?.Invoke(true);
        }
        else
        {
            OnShiftHold?.Invoke(false);
        }
    }
}
