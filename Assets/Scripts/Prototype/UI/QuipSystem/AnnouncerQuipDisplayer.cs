using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerQuipDisplayer : QuipDisplayer {

    public Animator announcerAnimator;

    private void Start()
    {
        ChangeColour(Color.red);
    }

    public override void Activate()
    {
        announcerAnimator.SetBool("Announcing", true);
        base.Activate();
    }

    public override void Deactivate()
    {
        announcerAnimator.SetBool("Announcing", false);
        base.Deactivate();
    }

}
