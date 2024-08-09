using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.rHandAim.weight = 1;
        actions.lHandIK.weight = 1;

    }
    public override void UpdateState(ActionStateManager actions)
    {
        actions.rHandAim.weight=Mathf.Lerp(actions.rHandAim.weight,1,10*Time.deltaTime);
        actions.lHandIK.weight=Mathf.Lerp(actions.lHandIK.weight,1,10*Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.R)&&CanReload(actions))
        {
            actions.SwitchState(actions.Reload);
        }
    }

    bool CanReload(ActionStateManager action)
    {
        if(action.weaponAmmo.currentAmmo==action.weaponAmmo.clipSize) return false;
        else if(action.weaponAmmo.extraAmmo==0) return false;
        else return true;

    }
}
