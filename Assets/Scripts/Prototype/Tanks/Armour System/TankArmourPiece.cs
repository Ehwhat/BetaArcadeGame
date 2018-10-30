using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[SelectionBase]
public class TankArmourPiece : MonoBehaviour, IRequiredPiece, IRequiresPieces {

    public enum RequirementType
    {
        And,
        Or
    }

    public bool isActive
    {
        get { return gameObject.activeSelf; }
        private set { gameObject.SetActive(value); }
    }
    public float maxHealth = 20f;
    public TankArmourPickup pickupPrefab;

    public List<TankArmourPiece> requiredPiecesToBeActive = new List<TankArmourPiece>();
    public List<TankArmourPiece> piecesRequiriedBy = new List<TankArmourPiece>();
    public RequirementType requirementType = RequirementType.And;
    public bool reserved = false;
    public float phaseTime = 0.4f;
    public Material phaseMaterial;
    public SpriteRenderer renderer;

    public void Start()
    {
        gameObject.SetActive(isActive);
    }

    

    public void OnPickup()
    {
        StartCoroutine(OnPickupRoutine());
    }

    private IEnumerator OnPickupRoutine()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(props);
        Vector4 pos = transform.position;
        props.SetVector("_WorldPos", pos);
        props.SetFloat("_Amount", 0);
        renderer.SetPropertyBlock(props);
        
        float elapsedTime = 0;
        while (elapsedTime < phaseTime)
        {
            props.SetVector("_WorldPos", pos);
            props.SetFloat("_Amount", elapsedTime / phaseTime);
            renderer.SetPropertyBlock(props);
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        props.SetVector("_WorldPos", pos);
        props.SetFloat("_Amount", 1);
        renderer.SetPropertyBlock(props);
    }

    private void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(isActive);
    }

    public bool TryEnablePiece()
    {
        if (IsRequirementSatisfied())
        {
            if (!isActive)
            {
                SetActive(true);
                OnPickup();
            }
            return true;
        }
        return false;
    }

    public bool DisablePiece(out TankArmourPickup pickup,bool spawnPickup = true)
    {
        pickup = null;
        if (isActive)
        {
            SetActive(false);
            if (spawnPickup)
                pickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity);
            return true;
        }
        return false;
    }


    public List<TankArmourPickup> ReevaluateExistance()
    {
        List<TankArmourPickup> droppedPickups = new List<TankArmourPickup>();
        if (!IsRequirementSatisfied())
        {
            TankArmourPickup pickup;
            if (DisablePiece(out pickup))
            {
                droppedPickups.Add(pickup);
            }
        }
        ReevaulateChildren(droppedPickups);
        return droppedPickups;
    }

    public List<TankArmourPickup> ReevaluateExistance(List<TankArmourPickup> droppedPickups)
    {
        if (!IsRequirementSatisfied())
        {
            TankArmourPickup pickup;
            if (DisablePiece(out pickup))
            {
                droppedPickups.Add(pickup);
            }
        }
        ReevaulateChildren(droppedPickups);
        return droppedPickups;
    }

    public List<TankArmourPickup> ReevaulateChildren()
    {
        List<TankArmourPickup> droppedPickups = new List<TankArmourPickup>();
        for (int i = 0; i < piecesRequiriedBy.Count; i++)
        {
            piecesRequiriedBy[i].ReevaluateExistance(droppedPickups);
        }
        return droppedPickups;
    }

    public List<TankArmourPickup> ReevaulateChildren(List<TankArmourPickup> droppedPickups)
    {
        for (int i = 0; i < piecesRequiriedBy.Count; i++)
        {
            droppedPickups.AddRange(piecesRequiriedBy[i].ReevaluateExistance());
        }
        return droppedPickups;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void AddRequirement(TankArmourPiece piece)
    {
        if (requiredPiecesToBeActive.Contains(piece))
            return;
        requiredPiecesToBeActive.Add(piece);
        piece.AddRequiredBy(this);
    }

    private void AddRequiredBy(TankArmourPiece piece)
    {
        piecesRequiriedBy.Add(piece);
    }

    public void RemoveRequirement(TankArmourPiece piece)
    {
        requiredPiecesToBeActive.Remove(piece);
        piece.RemoveRequiredBy(this);
    }

    private void RemoveRequiredBy(TankArmourPiece piece)
    {
        piecesRequiriedBy.Remove(piece);
    }

    public bool IsRequirementSatisfied()
    {
        for (int i = 0; i < requiredPiecesToBeActive.Count; i++)
        {
            if (requirementType == RequirementType.And)
            {
                if (!requiredPiecesToBeActive[i].IsActive())
                {
                    return false;
                }
            }
            else
            {
                if (requiredPiecesToBeActive[i].IsActive())
                {
                    return true;
                }
            }
        }
        return requirementType == RequirementType.And;
    }
}
