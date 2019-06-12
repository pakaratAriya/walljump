using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleItem : InGameItem
{
    public float duration = 5;

    public override void TriggerItem()
    {
        StartCoroutine(StartInvincible());

    }

    IEnumerator StartInvincible()
    {
        myChar.invulnerable = true;
        StartCoroutine(myChar.InvincibleEffect(duration));
        yield return new WaitForSeconds(duration);
        myChar.invulnerable = false;
    }


}
