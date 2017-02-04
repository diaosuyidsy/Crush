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

	public string next_level_name;

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
		if(enabled)
		{
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
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, true);

	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update ()
	{
	}
	
	#endregion // MonoBehaviour
	
	// ########################################
	// MoveIn/MoveOut functions
	// ########################################
	
	#region MoveIn/MoveOut

	// MoveIn all primary buttons
	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(1.0f);

		// MoveIn all primary buttons
		m_BottomLeft_Bar.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);

		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}

	// MoveOut all primary buttons
	public void HideAllGUIs()
	{
		m_BottomLeft_Bar.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	#endregion // MoveIn/MoveOut
	
	// ########################################
	// Enable/Disable button functions
	// ########################################
	
	#region Enable/Disable buttons

	// Enable/Disable all scene switch Coroutine
	IEnumerator EnableAllDemoButtons()
	{
		yield return new WaitForSeconds(1.0f);

		// Enable all scene switch buttons
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, true);
	}
	
	// Disable all buttons for a few seconds
	IEnumerator DisableButtonForSeconds(GameObject GO, float DisableTime)
	{
		// Disable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, false);
		
		yield return new WaitForSeconds(DisableTime);
		
		// Enable all buttons
		GUIAnimSystemFREE.Instance.EnableButton(GO.transform, true);
	}
	
	#endregion // Enable/Disable buttons
	
	// ########################################
	// UI Responder functions
	// ########################################
	
	#region UI Responder

	public void OnButton_BottomLeft()
	{


		// Disable m_TopLeft_A, m_RightBar_A, m_RightBar_C, m_BottomLeft_A for a few seconds
		StartCoroutine(DisableButtonForSeconds(m_BottomLeft_Bar.gameObject, 0.3f));

		// Toggle m_BottomLeft
		ToggleBottomLeft();
		
	}

	public void OnButton_Home()
	{
		SceneManager.LoadScene ("StartScene");
	}

	public void OnButton_Restart()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void OnButton_Mute()
	{
		IsMuted = !IsMuted;
		if(IsMuted){
			Camera.main.GetComponent<AudioListener>().enabled = false;
		}else{
			Camera.main.GetComponent<AudioListener>().enabled = true;
		}

	}

	public void OnButton_NextLevel()
	{
		SceneManager.LoadScene (next_level_name);
	}
	
	#endregion // UI Responder
	
	// ########################################
	// Toggle button functions
	// ########################################
	
	#region Toggle Button

	// Toggle TopLeft buttons

	
	// Toggle BottomLeft buttons
	void ToggleBottomLeft()
	{
		m_BottomLeft_IsOn = !m_BottomLeft_IsOn;
		if(m_BottomLeft_IsOn==true)
		{
			// m_BottomLeft_B moves in
			m_BottomLeft_Rainbow.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
		else
		{
			// m_BottomLeft_B moves out
			m_BottomLeft_Rainbow.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		}
	}

	
	#endregion // Toggle Button
}