using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankArmourManager))]
public class TankArmourManagerEditor : Editor {

    const float pieceRadius = 0.07f;

    TankArmourManager manager;

    int selectedPiece = -1;
    int secondSelectedPiece = -1;

    int selectedLine = -1;

    bool currentMousePressedState = false;

    private void OnEnable()
    {
        
        manager = (TankArmourManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Find All Pieces"))
        {
            manager.armourPieces = manager.GetComponentsInChildren<TankArmourPiece>();
        }

        if (GUILayout.Button("Remove Null Pieces"))
        {
            for (int i = 0; i < manager.armourPieces.Length; i++)
            {
                TankArmourPiece piece = manager.armourPieces[i];
                for (int j = piece.requiredPiecesToBeActive.Count-1; j > -1; j--)
                {
                    if(piece.requiredPiecesToBeActive[j] == null)
                    {
                        piece.requiredPiecesToBeActive.RemoveAt(j);
                    }
                }
                for (int j = piece.piecesRequiriedBy.Count - 1; j > -1; j--)
                {
                    if (piece.piecesRequiriedBy[j] == null)
                    {
                        piece.piecesRequiriedBy.RemoveAt(j);
                    }
                }
            }
        }

        EditorGUILayout.HelpBox(
            "Controls:\n\n" +
            "Hold CTRL to interact with armour system\n\n" +
            "Dragging and clicking creates a dependecy to another armour node\n\n" +
            "Holding CTRL+ALT and clicking dependency lines deletes the dependency\n\n" +
            "Holding CTRL+SHIFT and clicking activates the nearest piece of armour\n\n" +
            "Holding CTRL+ALT+SHIFT and clicking deactivates the nearest piece of armour"
            , MessageType.Info, true);


    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }

    public virtual void OnSceneGUI()
    {
        TankArmourPiece[] pieces = manager.armourPieces;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == null)
                continue;

            Handles.color = Color.yellow;
            Handles.CircleHandleCap(0, pieces[i].transform.position, Quaternion.identity, pieceRadius/2, EventType.Repaint);

            if (selectedPiece == i && !Event.current.alt && !Event.current.shift)
            {
                Handles.color = Color.green;
            }else if (secondSelectedPiece == i && !Event.current.alt && !Event.current.shift)
            {
                Handles.color = Color.blue;
            }
            else
            {
                Handles.color = Color.cyan;
            }
            
            float handleSizeMod = HandleUtility.GetHandleSize(pieces[i].transform.position);
            Handles.CircleHandleCap(0, pieces[i].transform.position, Quaternion.identity, pieceRadius, EventType.Repaint);
            List<TankArmourPiece> requiredPieces = pieces[i].requiredPiecesToBeActive;

            for (int j = 0; j < requiredPieces.Count; j++)
            {
                if (selectedPiece == i && selectedLine == j && Event.current.alt && !Event.current.shift)
                {
                    Handles.color = Color.red;
                }
                else
                {
                    Handles.color = Color.cyan;
                }
                Vector2 direction = (requiredPieces[j].transform.position - pieces[i].transform.position).normalized;
                Handles.DrawAAPolyLine((Vector2)pieces[i].transform.position + (direction * pieceRadius / 2), (Vector2)requiredPieces[j].transform.position);
            }

            if(selectedPiece == i && !Event.current.alt && !Event.current.shift)
            {
                Handles.color = Color.green;
                Vector2 worldPos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                Handles.DrawAAPolyLine(pieces[i].transform.position, worldPos);
            }
        }

        ProcessEvents(Event.current);

        if (currentMousePressedState && !Event.current.alt && !Event.current.shift)
        {
            CheckHoveringOverSecondPieces();
            SceneView.RepaintAll();
        }
        else
        {
            secondSelectedPiece = -1;
        }

        if (Event.current.alt && Event.current.control && !Event.current.shift)
        {
            CheckHoveringOverRequirementLine();
        }

    } 

    private void ProcessEvents(Event e)
    {
        if (e.control)
        {
            int id = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);
            GUIUtility.hotControl = id;

            switch (e.type)
            {
                case EventType.MouseDown:

                    if(!e.alt && !e.shift)
                    {
                        CheckHoveringOverPiece();
                    }
                    else if (!e.alt && e.shift)
                    {
                        ActivatePieceIfPossible();
                        e.Use();
                    }else if (e.alt && e.shift)
                    {
                        DeactivatePieceIfPossible();
                        e.Use();
                    }
                    else
                    {
                        RemoveRequirementIfPossible();
                    }
                    currentMousePressedState = true;
                    break;
                case EventType.MouseUp:
                    AddRequirementIfPossible();
                    currentMousePressedState = false;
                    selectedPiece = -1;
                    SceneView.RepaintAll();
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckHoveringOverPiece()
    {
        TankArmourPiece[] pieces = manager.armourPieces;

        for (int i = 0; i < pieces.Length; i++)
        {
            float handleSizeMod = HandleUtility.GetHandleSize(pieces[i].transform.position);
            if (HandleUtility.DistanceToCircle(pieces[i].transform.position, pieceRadius) <= 0)
            {
                selectedPiece = i;
                SceneView.RepaintAll();
                return;
            }

        }

    }

    private void CheckHoveringOverRequirementLine()
    {
        TankArmourPiece[] pieces = manager.armourPieces;

        for (int i = 0; i < pieces.Length; i++)
        {
            for (int j = 0; j < pieces[i].requiredPiecesToBeActive.Count; j++)
            {
                float handleSizeMod = HandleUtility.GetHandleSize(pieces[i].transform.position);
                if (HandleUtility.DistanceToLine(pieces[i].transform.position, pieces[i].requiredPiecesToBeActive[j].transform.position) <= 5)
                {
                    selectedPiece = i;
                    selectedLine = j;
                    SceneView.RepaintAll();
                    return;
                }
            }

        }
        selectedPiece = -1;
        selectedLine = -1;

    }

    private void ActivatePieceIfPossible()
    {
        manager.AddPieceNear(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
    }

    private void DeactivatePieceIfPossible()
    {
        manager.RemovePieceNear(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin);
    }

    private void AddRequirementIfPossible()
    {
        if (selectedPiece == -1 || secondSelectedPiece == -1)
            return;
        manager.armourPieces[selectedPiece].AddRequirement(manager.armourPieces[secondSelectedPiece]);
    }

    private void RemoveRequirementIfPossible()
    {
        if (selectedPiece == -1 || selectedLine == -1)
            return;
        manager.armourPieces[selectedPiece].RemoveRequirement(manager.armourPieces[selectedPiece].requiredPiecesToBeActive[selectedLine]);
    }

    private void CheckHoveringOverSecondPieces()
    {
        TankArmourPiece[] pieces = manager.armourPieces;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (i == selectedPiece)
                continue;
            float handleSizeMod = HandleUtility.GetHandleSize(pieces[i].transform.position);
            if (HandleUtility.DistanceToCircle(pieces[i].transform.position, pieceRadius) <= 0)
            {
                secondSelectedPiece = i;
                SceneView.RepaintAll();
                return;
            }

        }
        secondSelectedPiece = -1;

    }


}
