using UnityEngine;

/// <summary>
/// This class manages a dialog for creating and editting a certain model class.
/// A dialog can be used in two ways: to CREATE a new object or to EDIT an existing one. In both cases, it will have differente
/// actions for the setup and submit operations. To do so, this class has some virtual functions that must be overriden for each 
/// type of dialog.
/// </summary>
/// <typeparam name="T">A model class to create or edit by this dialog</typeparam>
public class SetupDialog<T> : MonoBehaviour where T : class
{
    public DialogManager manager;
    private T edittedObject = null;
    private EditListener listener;
    private bool isOpen = false;

	internal bool error = false;

    /// <summary>
    /// Returns the existing object under edition or null if this is a CREATION dialog.
    /// </summary>
    protected T EdittingObject
    {
        get
        {
            return edittedObject;
        }
    }

	internal bool CheckError
    {
		set
        {
			error = value; 
		}
		get
        {
			bool tmp = error;
			error = false;
			return tmp;
		}
	}

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
    }

    /// <summary>
    /// Opens this dialog for editing the existing object T.
    /// </summary>
    /// <param name="obj">Object to be edited.</param>
    /// <param name="listener">Optional callback to be called on submition</param>
    public void OpenFromSaved(T obj, EditListener listener = null)
    {
        isOpen = true;
        manager.ActivateCamera(gameObject);
        this.edittedObject = obj;
        this.listener = listener;
        SetupFromSaved(obj);
    }

    /// <summary>
    /// Opens this dialog for a new object setup.
    /// </summary>
    public void OpenForNew()
    {
        isOpen = true;
        manager.ActivateCamera(gameObject);
        SetupForNew();
    }

    /// <summary>
    /// Override this with your dialog setup for new objects.
    /// </summary>
    protected virtual void SetupForNew() { }

    /// <summary>
    /// Override this with your dialog setup for existing objects.
    /// </summary>
    /// <param name="obj">Object to be edited</param>
    protected virtual void SetupFromSaved(T obj) { }

    /// <summary>
    /// Override this with your submition code that creates a model class objects.
    /// </summary>
    protected virtual void SubmitNew() { }

    /// <summary>
    /// Override this with your submition code that edits the existing object with new data.
    /// </summary>
    /// <param name="obj">Object to be edited</param>
    protected virtual void SubmitToSaved(T obj) { }


    /// <summary>
    /// Tells if this dialog is currently being used to edit or create an object.
    /// </summary>
    public bool IsOpenFromSaved
    {
        get
        {
            return edittedObject != null;
        }
    }

    /// <summary>
    /// Closes the dialog WITHOUT submitting it.
    /// </summary>
    public void Close()
    {
//		ServerAssets.Instance.ClearAssetsSyncs ();
        isOpen = false;
        manager.DeactivateCurrentCamera();
        this.edittedObject = null;
        this.listener = null;
    }

    /// <summary>
    /// Submits the dialog and calls the appropriate submition functions. Override this to do any pre-submiting checking.
    /// </summary>
    public virtual void Submit()
    {
		if (edittedObject != null) 
        {
			SubmitToSaved (this.edittedObject);
			if (listener != null)
				listener.OnEditionComplete (edittedObject);
		} 
        else 
        {
			SubmitNew ();
		}
        if (!CheckError)
        {
            Close();
            GraphManager.SSave();
        }
        else
        {
            AlertDialog.OpenForInfo("Não é possível salvar um diálogo sem mensagens ou perguntas.");
        }
    }

    /// <summary>
    /// A listener to be called when the editing proccess is complete.
    /// </summary>
    public interface EditListener
    {
        void OnEditionComplete(T obj);
    }
}