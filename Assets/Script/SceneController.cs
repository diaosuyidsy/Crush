// GUI Animator FREE
// Version: 1.1.0
// Compatilble: Unity 5.4.0 or higher, see more info in Readme.txt file.
//
// Developer:										Gold Experience Team (https://www.ge-team.com)
//
// Unity Asset Store:					https://www.assetstore.unity3d.com/en/#!/content/58843
// GE Store:							https://www.ge-team.com/en/products/gui-animator-free/
// Full version on Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Full version on GE Store:			https://www.ge-team.com/en/products/gui-animator-for-unity-ui/
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// GA_FREE_Demo01 class
// - Animates all GUIAnimFREE elements in the scene.
// - Responds to user mouse click or tap on buttons.
//
// Note this class is attached with "-SceneController-" object in "GA FREE - Demo01 (960x600px)" scene.
// ######################################################################

public class SceneController : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Canvas
	public Canvas m_Canvas;

	public GUIAnimFREE m_BottomLeft_Bar;
	public GUIAnimFREE m_BottomLeft_Rainbow;
	public GUIAnimFREE m_LevelIntro_Panel;
	public GUIAnimFREE m_HUD;
	public Sprite Unmute;
	public Sprite Muted;
	public Button MenuToggle;
	public GameObject musicToggle;

	public GameObject primary_game_object;

	// Toggle state of TopLeft, BottomLeft and BottomLeft buttons
	bool m_BottomLeft_IsOn = false;
	bool IsMuted = false;


	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
	void Awake ()
	{
		if (enabled) {
			// Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
			GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
		}
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start ()
	{
		// Disable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable (m_Canvas, true);

		StartCoroutine (MoveInLevelIntro ());
		StartCoroutine (MoveInGUI (1.0f));
	}

	
	#endregion // MonoBehaviour

	// ########################################
	// MoveIn/MoveOut functions
	// ########################################

	#region MoveIn/MoveOut

	public void moveInGui (float moveOutTime)
	{
		StartCoroutine (MoveInGUI (moveOutTime));
	}


	IEnumerator MoveInGUI (float moveOutTime)
	{
		yield return new WaitForSeconds (0.0f);

		MenuToggle.gameObject.SetActive (false);
		m_HUD.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

//		StartCoroutine (MoveOutGUI (moveOutTime));
	}

	public void moveOutGui (float moveOutTime)
	{
		StartCoroutine (MoveOutGUI (moveOutTime));
	}

	IEnumerator MoveOutGUI (float moveOutTime)
	{
		yield return new WaitForSeconds (moveOutTime);

		m_HUD.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		MenuToggle.gameObject.SetActive (true);
	}


	IEnumerator MoveInLevelIntro ()
	{
		yield return new WaitForSeconds (0.0f);

		m_LevelIntro_Panel.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		StartCoroutine (SetupLevel ());
	}

	IEnumerator SetupLevel ()
	{
		yield return new WaitForSeconds (1.0f);

		m_LevelIntro_Panel.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		yield return new WaitForSeconds (0.5f);

		primary_game_object.SetActive (true);

	}

	// MoveIn all primary buttons
	IEnumerator MoveInPrimaryButtons ()
	{
		yield return new WaitForSeconds (1.0f);

		// MoveIn all primary buttons
		m_BottomLeft_Bar.MoveIn (GUIAnimSystemFREE.eGUIMove.Self);

		// Enable all scene switch buttons
		StartCoroutine (EnableAllDemoButtons ());
	}

	// MoveOut all primary buttons
	public void HideAllGUIs ()
	{
		m_BottomLeft_Bar.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	#endregion // MoveIn/MoveOut

	// ########################################
	// Enable/Disable button functions
	// ########################################

	#region Enable/Disable buttons

	// Enable/Disable all scene switch Coroutine
	IEnumerator EnableAllDemoButtons ()
	{
		yield return new WaitForSeconds (1.0f);

		// Enable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable (m_Canvas, true);
	}
	
	// Disable all buttons for a few seconds
	IEnumerator DisableButtonForSeconds (GameObject GO, float DisableTime)
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableButton (GO.transform, false);
		
		yield return new WaitForSeconds (DisableTime);
		
		// Enable all buttons
		GUIAnimSystemFREE.Instance.EnableButton (GO.transform, true);
	}

	#endregion // Enable/Disable buttons

	// ########################################
	// UI Responder functions
	// ########################################

	#region UI Responder

	public void OnButton_BottomLeft ()
	{


		// Disable m_TopLeft_A, m_RightBar_A, m_RightBar_C, m_BottomLeft_A for a few seconds
		StartCoroutine (DisableButtonForSeconds (m_BottomLeft_Bar.gameObject, 0.3f));

		// Toggle m_BottomLeft
		ToggleBottomLeft ();
		
	}

	public void OnButton_Home ()
	{
		GameData.gd.launchersinfomap = new Dictionary<int, LaunchersInfo> ();
		SceneManager.LoadScene ("StartScene");
	}

	public void OnButton_Restart ()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void OnButton_Mute ()
	{
		IsMuted = !IsMuted;
		if (IsMuted) {
			musicToggle.GetComponent<Image> ().sprite = Muted;
			GameData.gd.GetComponent<AudioSource> ().Pause ();
		} else {
			musicToggle.GetComponent<Image> ().sprite = Unmute;
			GameData.gd.GetComponent<AudioSource> ().Play ();
		}

	}

	public void OnButton_NextLevel ()
	{
		if (SceneManager.GetActiveScene ().name != "Level_7")
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	#endregion // UI Responder

	// ########################################
	// Toggle button functions
	// ########################################

	#region Toggle Button

	// Toggle TopLeft buttons

	
	// Toggle BottomLeft buttons
	void ToggleBottomLeft ()
	{
		m_BottomLeft_IsOn = !m_BottomLeft_IsOn;
		if (m_BottomLeft_IsOn == true) {
			// m_BottomLeft_B moves in
			m_BottomLeft_Rainbow.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		} else {
			// m_BottomLeft_B moves out
			m_BottomLeft_Rainbow.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}

	
	#endregion // Toggle Button

	#region Helper Functions


	#endregion
}