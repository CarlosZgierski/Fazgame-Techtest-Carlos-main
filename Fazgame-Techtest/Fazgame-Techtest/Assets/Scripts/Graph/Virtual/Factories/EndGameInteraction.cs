using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameInteraction : Interaction {

    public string content;

    public iTween.EaseType BeforeEaseType = iTween.EaseType.easeOutBack;
    public float beforeTime = .3f;

    public iTween.EaseType AfterEaseType = iTween.EaseType.easeInBack;
    public float afterTime = .3f;

    private List<GameObject> gameObjectEndGame;

    new void Awake()
    {
        base.Awake();
        gameObjectEndGame = new List<GameObject>();
    }

    public override void Step()
    {
            Do();
    }

    public bool EndGameEndAddEndGameInteraction()
    {
        if (gameObjectEndGame != null && gameObjectEndGame.Count > 0)
        {
            iTween.ScaleTo(gameObjectEndGame[0], iTween.Hash(iT.ScaleTo.scale, new Vector3(0, 0, 0), iT.ScaleTo.easetype, AfterEaseType, iT.ScaleTo.time, afterTime, iT.ScaleTo.oncompletetarget, gameObject, iT.ScaleTo.oncomplete, "DestroyEnd", iT.ScaleTo.oncompleteparams, this.GetInstanceID()));
        }
        return true;
    }

    public void DestroyEnd(int instanceId)
    {
        if (instanceId == GetInstanceID() && gameObjectEndGame.Count > 0)
        {
            End();
            foreach (GameObject gameObjectMessage in gameObjectEndGame)
            {
                Destroy(gameObjectMessage);
            }
            gameObjectEndGame.Clear();
        }
    }

    public override void _End()
    {
        base._End();
        InteractionController.ControllerInstance.RemoveBoxInteraction(this);
    }

    public override void _Do()
    {

    }
}
