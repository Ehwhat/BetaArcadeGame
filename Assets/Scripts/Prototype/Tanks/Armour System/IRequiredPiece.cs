using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiredPiece {
    bool IsActive();
}

public interface IRequiresPieces
{
    bool IsRequirementSatisfied();
}