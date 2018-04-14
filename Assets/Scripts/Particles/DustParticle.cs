using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticle : WallJumpParticle {
    SpriteRenderer spt;
    Color originColor;
    bool fade = false;
    float alpha = 1.0f;
    private void Awake()
    {
        spt = GetComponent<SpriteRenderer>();
        originColor = spt.color;
    }
    private void OnEnable()
    {
        spt.color = originColor;
        fade = false;
        alpha = 1.0f;
        Invoke("SetFade", 0.417f);
    }

    private void Update()
    {
        if (fade && alpha > 0)
        {
            alpha -= 0.2f;
        }
        spt.color = new Color(originColor.r, originColor.g, originColor.b, alpha);
        if (alpha <=0)
        {
            PoolManager.Despawn(this);
        }
    }

    private void SetFade()
    {
        fade = true;
    }

}
