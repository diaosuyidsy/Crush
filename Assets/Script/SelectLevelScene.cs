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
	public GUIAnimFREE m_Dialog1;
	public GUIAnimFREE m_Dialog2;
	public GUIAnimFREE m_Dialog3;
	public GUIAnimFREE m_Dialog4;

	public GUIAnimFREE to_Right;
	public GUIAnimFREE to_Left;

	private GameObject[] Level_List;
	private Dictionary<int, GUIAnimFREE> Level_HashMap;
	
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
		Level_List = GameObject.FindGameObjectsWithTag ("Level_Select_Button");
		Level_HashMap = new Dictionary<int, GUIAnimFREE> ();

		foreach(GameObject O in Level_List)
		{
			Level_HashMap [O.GetComponent<LevelInfo> ().LevelNumber] = O.GetComponent<GUIAnimFREE> ();
		}

		// MoveIn m_Title1 and m_Title2
		StartCoroutine(MoveInTitleGameObjects());

		// Disable all scene switch buttons	
		// http://docs.unity3d.com/Manual/script-GraphicRaycaster.html
		GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(m_Canvas, false);
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
	
	// MoveIn m_Title1 and m_Title2
	IEnumerator MoveInTitleGameObjects()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn m_Title1 and m_Title2
		m_Title1.MoveIn(GUIAnimSystemFREE.eGUIMove.Self);

		// MoveIn m_Dialog
		StartCoroutine(MoveInPrimaryButtons());
	}
	
	// MoveIn m_Dialog
	IEnumerator MoveInPrimaryButtons()
	{
		yield return new WaitForSeconds(1.0f);
		
		// MoveIn dialogs
		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		// MoveIn buttons to next levels
		to_Right.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		to_Left.MoveIn (GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		// Get the star and Level according to playerprefs
		GetLevelSetup (m_Dialog1);
		GetLevelSetup (m_Dialog2);
		GetLevelSetup (m_Dialog3);
		GetLevelSetup (m_Dialog4);

		
		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}

	IEnumerator MoveInPrimaryButtonsExceptToleftAndRight()
	{
		yield return new WaitForSeconds(1.0f);

		// MoveIn dialogs
		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);	
		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

		// Get the star and Level according to playerprefs
		GetLevelSetup (m_Dialog1);
		GetLevelSetup (m_Dialog2);
		GetLevelSetup (m_Dialog3);
		GetLevelSetup (m_Dialog4);

		// Enable all scene switch buttons
		StartCoroutine(EnableAllDemoButtons());
	}
	
	public void HideAllGUIs()
	{
		// MoveOut dialogs
		m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		
		// MoveOut m_Title1 and m_Title2
		StartCoroutine(HideTitleTextMeshes());
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

		Dictionary<int, Level> levelBook = GameData.gd.Load ().LevelBook;
		if(levelBook != null){
			if(levelBook.ContainsKey (levelnum))
			{
				int starNum = levelBook [levelnum].starNum;
				Dialog.GetComponentInParent<LevelInfo>().Enable_Stars (starNum);
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
		m_Dialog1.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog2.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog3.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
		m_Dialog4.MoveOut(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}

	// When to right button is clicked
	public void OnButton_ToRight()
	{
		// If the pointer is not pointed to the last level I have
		// Then get the next 4 levels
		if(GameData.gd.Level_Select_Pointer < GameData.gd.max_Level_Select_Pointer)
		{
			//Set the To Left Button Enabled
			if(to_Left.gameObject.GetComponent <Button> ().interactable == false)
			{
				to_Left.gameObject.GetComponent <Button> ().interactable = true;
			}


			OnButton_MoveOutAllDialogs ();

			m_Dialog1 = Level_HashMap [++GameData.gd.Level_Select_Pointer];
			m_Dialog2 = Level_HashMap [++GameData.gd.Level_Select_Pointer];
			m_Dialog3 = Level_HashMap [++GameData.gd.Level_Select_Pointer];
			m_Dialog4 = Level_HashMap [++GameData.gd.Level_Select_Pointer];

			StartCoroutine (MoveInPrimaryButtonsExceptToleftAndRight ());

			// If we reached the end of the Levels I have, disable to right
			if(GameData.gd.Level_Select_Pointer >= GameData.gd.max_Level_Select_Pointer)
			{
				to_Right.gameObject.GetComponent <Button> ().interactable = false;
			}
		}

	}

	public void OnButton_ToLeft()
	{
		// If the pointer is not pointed to the last level I have
		// Then get the last 4 levels
		if(GameData.gd.Level_Select_Pointer > GameData.gd.min_Level_Select_Pointer)
		{
			//Set the To Right Button Enabled
			if(to_Right.gameObject.GetComponent <Button> ().interactable == false)
			{
				to_Right.gameObject.GetComponent <Button> ().interactable = true;
			}

			OnButton_MoveOutAllDialogs ();

			m_Dialog1 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
			m_Dialog2 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
			m_Dialog3 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];
			m_Dialog4 = Level_HashMap [(--GameData.gd.Level_Select_Pointer) - 3];

			StartCoroutine (MoveInPrimaryButtonsExceptToleftAndRight ());

			// If we reached the end of the Levels I have, disable to right
			if(GameData.gd.Level_Select_Pointer <= GameData.gd.min_Level_Select_Pointer)
			{
				to_Left.gameObject.GetComponent <Button> ().interactable = false;
			}
		}
	}
	
	#endregion // UI Responder

	#region Load Scene

	IEnumerator Load_Level(int levelNum)
	{
		yield return new WaitForSeconds (1f);

		SceneManager.LoadScene (levelNum + 1);
	}


	#endregion
	
	// ########################################
	// Move dialog functions
	// ########################################
	
	#region Move Dialog
	
	// MoveIn m_Dialog1
	IEnumerator Dialog1_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog1
		m_Dialog1.ResetAllChildren();
		
		// Moves m_Dialog1 back to screen to screen
		m_Dialog1.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog2
	IEnumerator Dialog2_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog2
		m_Dialog2.ResetAllChildren();
		
		// Moves m_Dialog2 back to screen to screen
		m_Dialog2.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog3
	IEnumerator Dialog3_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog3
		m_Dialog3.ResetAllChildren();
		
		// Moves m_Dialog3 back to screen to screen
		m_Dialog3.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	// MoveIn m_Dialog4
	IEnumerator Dialog4_MoveIn()
	{
		yield return new WaitForSeconds(1.5f);
		
		// Reset children of m_Dialog4
		m_Dialog4.ResetAllChildren();
		
		// Moves m_Dialog4 back to screen to screen
		m_Dialog4.MoveIn(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
	}
	
	#endregion // Move Dialog
}
