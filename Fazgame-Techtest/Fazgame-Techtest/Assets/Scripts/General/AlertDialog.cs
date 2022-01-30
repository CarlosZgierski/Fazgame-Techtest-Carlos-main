using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Provides an alert dialog with a title, a message and confirm/cancel buttons.
/// </summary>
public class AlertDialog : MonoBehaviour
{
    public Text _title, _message;
    public GameObject doubleButtons, singleButton;
    private Listener l;

	private static AlertDialog _instance;

	private static AlertDialog Instance{
		get{
			if(!_instance){		
                AlertDialog prefab = (Resources.Load("AlertDialog") as GameObject).GetComponent<AlertDialog>();
                _instance = Instantiate(prefab) as AlertDialog;
                DontDestroyOnLoad(_instance.gameObject);
			}
            return _instance;
		}
	}

    /// <summary>
    /// Opens the dialog with both "OK" and "Cancel" buttons
    /// </summary>
    /// <param name="message_key">Key of the message to be displayed.</param>
    /// <param name="l">Optional event-listener object.</param>
    /// <param name="title">Optional title string.</param>
    public static void Open(string message_key, Listener l = null, string title = null)
    {
        AlertDialog dialog = GenerateDialog("Mensagem do Diálogo", l, title);
        dialog.DoubleMode = true;
    }

    private static AlertDialog GenerateDialog(string message, Listener l, string title)
    {
        if (title == null)
            title = "Título do Diálogo"; 
        AlertDialog dialog = Instance;

        dialog._title.text = title;
        dialog._message.text = message;
        dialog.l = l;
        dialog.gameObject.SetActive(true);

        return dialog;
    }

    /// <summary>
    /// Opens the dialog without the "Cancel" button.
    /// </summary>
    /// <param name="message">Message to be displayed.</param>
    /// <param name="l">Optional event-listener object.</param>
    /// <param name="title">Optional title string.</param>
    public static void OpenForInfo(string message, Listener l = null, string title = null)
    {
        AlertDialog dialog = GenerateDialog(message, l, title);
        dialog.DoubleMode = false;
    }

    private bool DoubleMode
    {
        set
        {
            doubleButtons.SetActive(value);
            singleButton.SetActive(!value);
        }
    }


    /// <summary>
    /// 2d Toolkit callback for when the OK Button is pressed.
    /// </summary>
    public void OkButtonPressed()
    {
        if(l != null)
            l.OnConfirm();
        Dismiss();
    }

    /// <summary>
    /// 2d Toolkit callback for when the Cancel Button is pressed.
    /// </summary>
    public void CanceButtonPressed()
    {
        if (l != null)
            l.OnCancel();
        Dismiss();
    }

    private void Dismiss()
    {
        l = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// An interface that provides listener methods for this dialog's events.
    /// </summary>
    public interface Listener
    {
        void OnConfirm();
        void OnCancel();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        Dismiss();
    }
}