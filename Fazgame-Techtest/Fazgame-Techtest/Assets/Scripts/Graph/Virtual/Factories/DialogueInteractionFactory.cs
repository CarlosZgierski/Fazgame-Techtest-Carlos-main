using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueInteractionFactory : InteractionFactory{

	public List<Interaction> createInteraction(VirtualInteraction virtualInteraction, InteractionController ic)
	{
		VirtualDialog vd = (VirtualDialog) virtualInteraction;
        //GameObject target = ic.GetGameObject(vd.element);
        
        //TODO: verificar que objeto usar aqui, antes era o personagem!
        GameObject target = ic.CreateInteractionGameObject("DialogInteraction");


		DialogueInteraction di = target.AddComponent<DialogueInteraction>();

        //if (vd.activateOnCharClick)
        //{
        //    MouseClickInteraction clickInteraction = target.AddComponent<MouseClickInteraction>();
        //    clickInteraction.RayCastCamera = ic.camera;
        //    di.AddCondiction(clickInteraction);
        //}

		if(vd.HasChildren())
		{
			DialogueQuestionInteraction dqi = target.AddComponent<DialogueQuestionInteraction>();
			List<Interaction> lista = di.SetFalas(vd.Speeches);
			if(lista.Count > 0){
				dqi.AddDependencie(lista[lista.Count -1]);
			}
			Speech[] speechs = {vd.Question};
			dqi.SetFalas(speechs);
		 	lista.Insert(lista.Count,dqi);
			return lista;
		}
		else
		{
			return di.SetFalas(vd.Speeches);
		}
	}
}
