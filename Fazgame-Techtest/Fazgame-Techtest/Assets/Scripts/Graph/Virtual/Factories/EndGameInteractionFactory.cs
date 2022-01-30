using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameInteractionFactory : InteractionFactory {

    private static int indexEndGame = 0;

    public List<Interaction> createInteraction(VirtualInteraction virtualInteraction, InteractionController ic)
    {
        VirtualEndGame veg = (VirtualEndGame)virtualInteraction;
        GameObject target = ic.CreateInteractionGameObject("EndGameInteraction" + indexEndGame++);
        EndGameInteraction egi = target.AddComponent<EndGameInteraction>();
        egi.content = veg.content;
        List<Interaction> list = new List<Interaction>();
        list.Add(egi);
        return list;
        
    }
}
