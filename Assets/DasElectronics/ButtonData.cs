using UnityEngine;

public class ButtonData : MonoBehaviour, IElectronic
{
    public bool isActive = false;
    public bool IsEnergized => isActive;

    public void ReceiveEnergy() { }

    public void Interact()
    {
        isActive = !isActive;
        if (isActive)
            NotifyOutput();
        else
            ResetChain();
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

    private void ResetChain()
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