using UnityEngine;

public class CableInvertedL : MonoBehaviour, IElectronic
{
    public bool isEnergized = false;
    public bool IsEnergized => isEnergized;

    public void ReceiveEnergy()
    {
        if (isEnergized) return;
        isEnergized = true;
        NotifyOutput();
    }

    public void ResetEnergy()
    {
        if (!isEnergized) return;
        isEnergized = false;
        PropagateReset();
    }

    private void NotifyOutput()
    {
        Transform outputTransform = transform.Find("Output");
        if (outputTransform == null) return;

        Collider[] hits = Physics.OverlapBox(
            outputTransform.position,
            outputTransform.localScale * 0.4f,
            Quaternion.identity,
            LayerMask.GetMask("Electronics")
        );

        foreach (Collider hit in hits)
        {
            IElectronic electronic = hit.GetComponent<IElectronic>();
            if (electronic != null)
                electronic.ReceiveEnergy();
        }
    }

    private void PropagateReset()
    {
        Transform outputTransform = transform.Find("Output");
        if (outputTransform == null) return;

        Collider[] hits = Physics.OverlapBox(
            outputTransform.position,
            outputTransform.localScale * 0.4f,
            Quaternion.identity,
            LayerMask.GetMask("Electronics")
        );

        foreach (Collider hit in hits)
        {
            CableStraight cs = hit.GetComponent<CableStraight>();
            if (cs != null) cs.ResetEnergy();

            CableL cl = hit.GetComponent<CableL>();
            if (cl != null) cl.ResetEnergy();

            CableInvertedL cil = hit.GetComponent<CableInvertedL>();
            if (cil != null) cil.ResetEnergy();

            ColliderDoor cd = hit.GetComponent<ColliderDoor>();
            if (cd != null) cd.ResetEnergy();
        }
    }
}