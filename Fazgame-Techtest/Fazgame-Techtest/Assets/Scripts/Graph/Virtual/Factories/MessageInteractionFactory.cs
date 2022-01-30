using UnityEngine;
using System.Collections.Generic;

public class MessageInteractionFactory : InteractionFactory {
	
	private static int indexMessage = 0;

	public List<Interaction> createInteraction(VirtualInteraction virtualInteraction, InteractionController ic)
	{
		VirtualMessage vm = (VirtualMessage) virtualInteraction;
		GameObject target = ic.CreateInteractionGameObject("MessageInteraction" + indexMessage++);
		MessageInteraction mi = target.AddComponent<MessageInteraction>();
		mi.content = vm.content;
		List<Interaction> list = new List<Interaction>();
		list.Add(mi);
		return list;
	}
}
