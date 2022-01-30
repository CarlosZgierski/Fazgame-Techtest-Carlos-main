using UnityEngine;
using System.Collections.Generic;
using Type = VirtualInteraction.InteractionType;

/// <summary>
/// Holds the images used for a graph vertex in different states.
/// </summary>
[System.Serializable]
public class VertexImages
{
    public Sprite original, first;

    /// <summary>
    /// Checks if this object fits the minimum requirements for usage, that is, is not null and has an original sprite.
    /// </summary>
    public static bool Valid(VertexImages vImg)
    {
        return vImg != null && vImg.original != null;
    }
}
