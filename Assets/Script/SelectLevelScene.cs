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
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

#endregion // Namespaces

// ######################################################################
// GA_FREE_Demo05 class
// - Animates all GUIAnimFREE elements in the scene.
// - Responds to user mouse click or tap on buttons.
//
// Note this class is attached with "-SceneController-" object in "GA FREE - Demo05 (960x600px)" scene.
// ######################################################################

public class SelectLevelScene : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################
	
	#region Variables

	// Canvas
	public Canvas m_Canvas;
	
	// GUIAnimFREE objects of title text
	public GUIAnimFREE m_Title1;

	// GUIAnimFREE objects of dialogs
	public GUIAnimFREE m_Panel;

	public GUIAnimFREE[] Level_Panels;

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

		// MoveIn m_Title1 and m_Title2
		StartCoroutine(MoveInTitleGameObjects());

		// Disable all scene switch buttons	
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, false);
	}
	
	#endregion // MonoBehaviour
	
	// ########################################
	// MoveIn/MoveOut functions
	// ########################################
	
	#region MoveIn/MoveOut
	
	// MoveIn m_Title1 and m_Title2
	IEnumerator MoveInTitleGameObjects()
	{
		yield return new WaitForSeconds(0.0f);
		
		// MoveIn m_Title1 and m_Title2
		m_Title1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);

		// MoveIn m_Dialog
		StartCoroutine(MoveInPrimaryButtons());
	}
	
	// MoveIn m_Dialog
	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(0.5f);
		
		// MoveIn dialogs

		foreach(GUIAnimFREE dialog in Level_Panels)
		{
			dialog.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
			GetLevelSetup (dialog);
		}

		
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}

	// MoveOut m_Title1 and m_Title2
	IEnumerator HideTitleTextMeshes()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveOut m_Title1 and m_Title2
		m_Title1.MoveOut(GUIAnimSystemFREE.eGUIMove.Self);

	}
	
	#endregion // MoveIn/MoveOut

	#region Get Level Data


	// Get Star setup
	void GetLevelSetup(GUIAnimFREE Dialog)
	{
		int levelnum = Dialog.GetComponentInParent<LevelInfo>().GetLevelNumber ();
		Levels levels = GameData.gd.Load ();
		if(levels != null)
		{
			Dictionary<int, Level> levelBook = levels.LevelBook;
			if(levelBook != null){
				if(levelBook.ContainsKey (levelnum))
				{
					int starNum = levelBook [levelnum].starNum;
					Dialog.GetComponentInParent<LevelInfo>().Enable_Stars (starNum);
					Dialog.gameObject.GetComponent<Button> ().interactable = true;
				}
			}
		}
	}


	#endregion

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
	
	#region On Button

	public void OnButton_Home()
	{
		SceneManager.LoadScene ("StartScene");
	}

	public void OnButton_Dialog(int llevelnum)
	{
		GUIAnimFREE currentGUI = EventSystem.current.currentSelectedGameObject.GetComponent<GUIAnimFREE> ();
		currentGUI.MoveOut (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		// Disable m_Dialog1 for a few seconds
//		StartCoroutine(DisableButtonForSeconds(m_Dialog1.gameObject, 2.5f));
//		StartCoroutine(DisableButtonForSeconds(m_Dialog2.gameObject, 2.5f));
//		StartCoroutine(DisableButtonForSeconds(m_Dialog3.gameObject, 2.5f));
//		StartCoroutine(DisableButtonForSeconds(m_Dialog4.gameObject, 2.5f));
//		StartCoroutine(DisableButtonForSeconds(to_Right.gameObject, 2.5f));
//		StartCoroutine(DisableButtonForSeconds(to_Left.gameObject, 2.5f));

		StartCoroutine (Load_Level (llevelnum));
	}
	
	public void OnButton_MoveOutAllDialogs()
	{		
		// MoveOut dialogs
//		m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//		m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//		m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//		m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
		

//	public void OnButton_ToLeft()
//	{
//		// If the pointer is not pointed to the last level I have
//		// Then get the last 4 levels
//		if(GameData.gd.Level_Select_Pointer > GameData.gd.min_Level_Select_Pointer)
//		{
//			//Set the To Right Button Enabled
//			if(to_Right.gameObject.GetComponent <Button> ().interactable == false)
//			{
//				to_Right.gameObject.GetComponent <Button> ().interactable = true;
//			}
//
//			OnButton_MoveOutAllDialogs ();
//
//			m_Dialog1 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
//			m_Dialog2 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
//			m_Dialog3 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
//			m_Dialog4 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
//
//			StartCoroutine (MoveInPrimaryButtonsExceptToleftAndRight ());
//
//			// If we reached the end of the Levels I have, disable to right
//			if(GameData.gd.Level_Select_Pointer <= GameData.gd.min_Level_Select_Pointer)
//			{
//				to_Left.gameObject.GetComponent <Button> ().interactable = false;
//			}
//		}
//	}
	
	#endregion // UI Responder

	#region Load Scene

	IEnumerator Load_Level(int levelNum)
	{
		yield return new WaitForSeconds (0.5f);

		SceneManager.LoadScene (levelNum + 1);
	}


	#endregion
	
	// ########################################
	// Move dialog functions
	// ########################################
	
	#region Move Dialog
	
	// MoveIn m_Dialog1
//	IEnumerator Dialog1_MoveIn()
//	{
//		yield return new WaitForSeconds(1.5f);
//		
//		// Reset children of m_Dialog1
//		m_Dialog1.ResetAllChildren();
//		
//		// Moves m_Dialog1 back to screen to screen
//		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//	}
//	
//	// MoveIn m_Dialog2
//	IEnumerator Dialog2_MoveIn()
//	{
//		yield return new WaitForSeconds(1.5f);
//		
//		// Reset children of m_Dialog2
//		m_Dialog2.ResetAllChildren();
//		
//		// Moves m_Dialog2 back to screen to screen
//		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//	}
//	
//	// MoveIn m_Dialog3
//	IEnumerator Dialog3_MoveIn()
//	{
//		yield return new WaitForSeconds(1.5f);
//		
//		// Reset children of m_Dialog3
//		m_Dialog3.ResetAllChildren();
//		
//		// Moves m_Dialog3 back to screen to screen
//		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//	}
//	
//	// MoveIn m_Dialog4
//	IEnumerator Dialog4_MoveIn()
//	{
//		yield return new WaitForSeconds(1.5f);
//		
//		// Reset children of m_Dialog4
//		m_Dialog4.ResetAllChildren();
//		
//		// Moves m_Dialog4 back to screen to screen
//		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
//	}
	
	#endregion // Move Dialog
}
