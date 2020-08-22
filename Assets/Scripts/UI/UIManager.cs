using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UIManager : MonoBehaviour
	{

		public TextMeshProUGUI cameraIndex;
		public Button cameraChangeButton;
		
		public Toggle fixCamera;
		public Toggle debugDestination;
		public Toggle debugCursorPosition;

		public TMP_InputField input;
		public Button openButton;
		public Button repo1Button;
		public Button repo2Button;


		string cameraIndexFormat;
		
		HeroController controller;
		CameraController cameraController;

		void Start()
		{
			Init();
			Bind();
		}

		void Init()
		{
			controller = ObjectManager.GetPlayer().GetController();
			cameraController = CameraController.Instance;
			
			cameraIndexFormat = cameraIndex.text;
			cameraIndex.SetText(string.Format(cameraIndexFormat, cameraController.initialCamera + 1));

			fixCamera.isOn = cameraController.fixCameraToTarget;
			debugDestination.isOn = controller.debugger;
			debugCursorPosition.isOn = CursorUtility.GetDebugger();
		}

		void Bind()
		{
			cameraChangeButton.OnClickAsObservable().Subscribe(e =>
			{
				cameraController.NextCamera();
				cameraIndex.SetText(string.Format(cameraIndexFormat, cameraController.CurrentCameraIndex + 1));
			}).AddTo(cameraController);

			fixCamera.OnValueChangedAsObservable().Subscribe(e =>
			{
				cameraController.fixCameraToTarget = e;
			}).AddTo(cameraController);

			cameraController.onFixCameraToggle.AddListener(() =>
			{
				fixCamera.SetIsOnWithoutNotify(cameraController.fixCameraToTarget);
			});

			debugDestination.OnValueChangedAsObservable().Subscribe(e =>
			{
				controller.debugger = e;
			}).AddTo(controller);

			debugCursorPosition.OnValueChangedAsObservable().Subscribe(CursorUtility.SetDebugger);
			
			repo1Button.onClick.AddListener(() =>
			{
				Native.Global.OpenURL("https://github.com/cqtd/league-of-legends-tests");
			});
			
			repo2Button.onClick.AddListener(() =>
			{
				Native.Global.OpenURL("https://github.com/cqtd/league-of-legends-clone");
			});
			
			openButton.onClick.AddListener(() =>
			{
				Native.Global.OpenURL(input.text);
			});
		}
	}
}