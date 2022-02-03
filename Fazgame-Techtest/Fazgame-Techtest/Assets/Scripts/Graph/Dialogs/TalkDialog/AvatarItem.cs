using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private List<Sprite> possibleAvatarSprites;

    [HideInInspector] public int avatarId;

    public void ChooseNewAvatar(int newAvatarId)
    {
        avatarId = newAvatarId;
        avatarImage.sprite = possibleAvatarSprites[avatarId];
    }
}
