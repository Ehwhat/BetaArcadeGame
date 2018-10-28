using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankArmourManager : MonoBehaviour {

    enum SearchFlags
    {
        Any,
        EmptyOnly,
        FullOnly
    }

    public TankArmourPiece[] armourPieces;

    public void AddPieceNear(Vector2 point)
    {
        TankArmourPiece bestPiece = FindBestPieceAtPoint(point, SearchFlags.EmptyOnly);
        if(bestPiece != null)
        {
            bestPiece.TryEnablePiece();
        }
    }

    public List<TankArmourPickup> RemovePieceNear(Vector2 point)
    {
        TankArmourPiece bestPiece = FindBestPieceAtPoint(point, SearchFlags.FullOnly, true);
        if (bestPiece != null)
        {
            List<TankArmourPickup> pickups = new List<TankArmourPickup>();

            TankArmourPickup pickup;
            if (bestPiece.DisablePiece(out pickup))
            {
                pickups.Add(pickup);
            }

            pickups = bestPiece.ReevaulateChildren(pickups);
            return pickups;
        }
        return null;

    }

    public List<TankArmourPickup> RemoveAll()
    {
        List<TankArmourPickup> pickups = new List<TankArmourPickup>();
        for (int i = 0; i < armourPieces.Length; i++)
        {
            TankArmourPickup pickup;
            if (armourPieces[i].DisablePiece(out pickup))
            {
                pickups.Add(pickup);
            }
        }
        return pickups;
    }

    public TankArmourPiece ReservePieceToward(Vector2 direction, bool wrapAround)
    {
        return FindBestPieceTowards(direction, SearchFlags.EmptyOnly, wrapAround);
    }


    public void ProcessDamage(ProjectileHit hit)
    {
        TankArmourPiece piece = FindBestPieceAtPoint(hit.hitData.point, SearchFlags.FullOnly, true);
        if(piece == null)
        {
            Debug.LogWarning("Something weird happened, I got damaged without finding a best");
        }
        piece.maxHealth -= hit.damage;
        List<TankArmourPickup> pickups = new List<TankArmourPickup>();
        if (piece.maxHealth <= 0)
        {
            TankArmourPickup pickup;
            if (piece.DisablePiece(out pickup))
            {
                pickups.Add(pickup);
            }
            pickups = piece.ReevaulateChildren(pickups);
        }

    }

    private TankArmourPiece FindBestPieceAtPoint(Vector2 point, SearchFlags flag = SearchFlags.Any, bool ignoreRequirements = false, bool ignoreReservations = false)
    {
        if (armourPieces.Length < 1)
            return null;
        TankArmourPiece bestPiece = null;
        float bestDistance = float.PositiveInfinity;

        for (int i = 0; i < armourPieces.Length; i++)
        {
            bool searchValid = (flag == SearchFlags.Any) || (flag == SearchFlags.EmptyOnly && !armourPieces[i].isActive) || (flag == SearchFlags.FullOnly && armourPieces[i].isActive);
            if ((armourPieces[i].IsRequirementSatisfied() || ignoreRequirements) && searchValid && (!armourPieces[i].reserved || ignoreReservations))
            {
                float distance = Vector2.Distance(point, armourPieces[i].transform.position);
                if (distance < bestDistance)
                {
                    bestPiece = armourPieces[i];
                    bestDistance = distance;
                }
            }
        }
        if (bestPiece == null || (!bestPiece.IsRequirementSatisfied() && !ignoreRequirements))
        {
            return null;
        }
        return bestPiece;
    }

    private TankArmourPiece FindBestPieceTowards(Vector2 direction, SearchFlags flag = SearchFlags.Any, bool wrapAround = false, bool ignoreRequirements = false, bool ignoreReservations = false)
    {
        if (armourPieces.Length < 1)
            return null;
        TankArmourPiece bestPiece = null;
        float bestScore = wrapAround ? float.NegativeInfinity : 0;

        for (int i = 0; i < armourPieces.Length; i++)
        {
            bool searchValid = (flag == SearchFlags.Any) || (flag == SearchFlags.EmptyOnly && !armourPieces[i].isActive) || (flag == SearchFlags.FullOnly && armourPieces[i].isActive);
            if ((armourPieces[i].IsRequirementSatisfied() || ignoreRequirements) && searchValid && (!armourPieces[i].reserved || ignoreReservations))
            {
                Vector2 internalDirection = (armourPieces[i].transform.position - transform.position).normalized;
                float score = Vector2.Dot(direction.normalized, internalDirection);
                if (score > bestScore)
                {
                    bestPiece = armourPieces[i];
                    bestScore = score;
                }
            }
        }
        if (bestPiece == null || (!bestPiece.IsRequirementSatisfied() && !ignoreRequirements))
        {
            return null;
        }
        return bestPiece;
    }

}

