using UnityEngine;
using System.Collections;
using System;

public class Command : MonoBehaviour
{
    //public GameObject target;
    //public object param;
    //public string method;

    private Func<bool> function;
	
    public void SetAction(Func<bool> function)
    {
        this.function = function;
    }

	public virtual void Execute()
    {
        if(function != null)
            function();

        /*
		if(target != null)
        {
            Debug.Log("Command Execution: Executing method <" + method + "> for target " + target.name + ". Params are: " + (param == null ? "null" : param.ToString()));
            if (param != null)
            {
				target.SendMessage(method, param);
			}
			else
            {
				target.SendMessage(method);
			}
		}
        */
	}
}
