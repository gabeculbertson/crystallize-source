using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMonoBehaviour : MonoBehaviour {

	RectTransform _rectTransform;
	CanvasGroup _canvasGroup;
	Image _image;

	public RectTransform rectTransform {
		get{
			if(!_rectTransform){
				_rectTransform = GetComponent<RectTransform>();
			}
			return _rectTransform;
		}
	}

	public CanvasGroup canvasGroup{
		get{
			if(!_canvasGroup){
				_canvasGroup = GetComponent<CanvasGroup>();
			}
			return _canvasGroup;
		}
	}

	public Image image {
		get{
			if(!_image){
				_image = GetComponent<Image>();
			}
			return _image;
		}
	}

}
