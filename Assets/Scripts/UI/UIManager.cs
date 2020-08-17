using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UIManager : MonoBehaviour
	{
		public CameraController cameraController;
		public CursorUtility cursorUtility;
		public HeroCharacterController characterController;

		public TextMeshProUGUI cameraIndex;
		public Button cameraChangeButton;
		
		public Toggle debugDestination;
		public Toggle debugCursorPosition;

		string cameraIndexFormat;

		void Reset()
		{
			cameraController = FindObjectOfType<CameraController>();
			cursorUtility = FindObjectOfType<CursorUtility>();
			characterController = FindObjectOfType<HeroCharacterController>();
		}

		void Awake()
		{
			Init();
			Bind();
		}

		void Init()
		{
			cameraIndexFormat = cameraIndex.text;
			cameraIndex.SetText(string.Format(cameraIndexFormat, cameraController.initialCamera + 1));

			debugDestination.isOn = characterController.debugger;
			debugCursorPosition.isOn = cursorUtility.debugger;
		}

		void Bind()
		{
			cameraChangeButton.OnClickAsObservable().Subscribe(e =>
			{
				cameraController.NextCamera();
				cameraIndex.SetText(string.Format(cameraIndexFormat, cameraController.CurrentCameraIndex + 1));
			}).AddTo(cameraController);

			debugDestination.OnValueChangedAsObservable().Subscribe(e =>
			{
				characterController.debugger = e;
			});
			
			debugCursorPosition.OnValueChangedAsObservable().Subscribe(e =>
			{
				cursorUtility.debugger = e;
			});
		}
	}
}