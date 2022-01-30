using UnityEngine;
using System.Collections.Generic;
public interface InteractionFactory{
	List<Interaction> createInteraction(VirtualInteraction virtualInteraction, InteractionController ic);
}
