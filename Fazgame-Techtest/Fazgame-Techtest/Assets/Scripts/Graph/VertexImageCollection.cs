using UnityEngine;
using System.Collections.Generic;
using Type = VirtualInteraction.InteractionType;

/// <summary>
/// Stores the state images of all types of graph vertices.
/// </summary>
[System.Serializable]
public sealed class VertexImageCollection
{
    public VertexImages click, dialog, score, answer, sceneLoad, message, getItem, useItem, endGame;

    private Dictionary<Type, VertexImages> table;

    public VertexImages Get(Type t)
    {
        if (table == null)
        {
            table = new Dictionary<Type, VertexImages>();
            table[Type.CLICK] = click;
            table[Type.DIALOG] = dialog;
            table[Type.SCORE] = score;
            table[Type.ANSWER] = answer;
            table[Type.SCENE_LOAD] = sceneLoad;
            table[Type.MESSAGE] = message;
            table[Type.GET_ITEM] = getItem;
            table[Type.USE_ITEM] = useItem;
            table[Type.GAME_END] = endGame;
        }
        return table[t];
    }
}
