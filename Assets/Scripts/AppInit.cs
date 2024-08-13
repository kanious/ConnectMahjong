using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInit : MonoBehaviour
{
	//[SerializeField] RectTransform RcPanel;
	//Rect RcLastSafeArea = new Rect(0, 0, 0, 0);

	[SerializeField] UIRoot _uiRoot = null;
	[SerializeField] UILabel _label = null;

	private void Awake()
	{
		if (_uiRoot)
		{
			Rect safeArea = Screen.safeArea;

			float ratio = safeArea.height / safeArea.width;
			int width = _uiRoot.manualWidth;
			_uiRoot.manualHeight = (int)(width * ratio);

			//_uiRoot.manualWidth = (int)safeArea.width;
			//_uiRoot.manualHeight = (int)safeArea.height;

			if (_label)
			{
				_label.text = string.Format("w:{0}, h:{1}, ratio:{2}", (int)safeArea.width, (int)safeArea.height, ratio);
			}
		}
	}

	//void Refresh()
	//{
	//	Rect safeArea = GetSafeArea();

	//	if (safeArea != RcLastSafeArea)
	//		ApplySafeArea(safeArea);
	//}

	//Rect GetSafeArea()
	//{
	//	return Screen.safeArea;
	//}

	//void ApplySafeArea(Rect r)
	//{
	//	RcLastSafeArea = r;

	//	Vector2 anchorMin = r.position;
	//	Vector2 anchorMax = r.position + r.size;
	//	anchorMin.x /= Screen.width;
	//	anchorMin.y /= Screen.height;
	//	anchorMax.x /= Screen.width;
	//	anchorMax.y /= Screen.height;
	//	RcPanel.anchorMin = anchorMin;
	//	RcPanel.anchorMax = anchorMax;
	//}
}
