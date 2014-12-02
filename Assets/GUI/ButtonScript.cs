using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (Image))]
public class ButtonScript : MonoBehaviour {
	public enum States {
		Active,
		Clicked,
		Disabled,
	}
	public Sprite activeButtonSprite;
	public Sprite clickedButtonSprite;
	
	[HideInInspector] [SerializeField]
	private GameObject _iconGO;
	[HideInInspector] [SerializeField]
	private Sprite _icon;
	[SerializeField]
	public Sprite icon { 
		get {
			return _icon;
		}
		set {
			if (value == null) {
				if (_iconGO != null) 
					_iconGO.SetActive (false);
				_icon = value;
			}
			else {
				if (_iconGO == null) {
					_iconGO = new GameObject ("icon");
					_iconGO.AddComponent <Image> ();
				}
				_iconGO.SetActive (true);
				_iconGO.transform.SetParent(this.transform, false);
				_iconGO.transform.localPosition = new Vector3 (0, 0, -0.1f);
				_iconGO.transform.localScale = Vector3.one;
				var iconRect = _iconGO.GetComponent <RectTransform> ();
				iconRect.anchorMax = new Vector2 (0.5f, 1f);
				iconRect.anchorMin = new Vector2 (0.5f, 1f);
				var iconImage = _iconGO.GetComponent <Image> ();
				iconImage.SetNativeSize ();
				iconImage.sprite = value; 
				_icon = value;
			}
		}
	}
	
	public event EventHandler Clicked;
	
	[HideInInspector] [SerializeField]
	private States _currentState;
	[SerializeField]
	public States currentState {
		get {
			return _currentState;
		}
		set {
			switch (value) {
				case States.Active: {
					this.currentImage.sprite = activeButtonSprite;
					var color = this.currentImage.color;
					color.a = 1f;
					this.currentImage.color = color;
					if (icon != null) {
						var iconImage = this._iconGO.GetComponent <Image> ();
						color = iconImage.color;
						color.a = 1f;
						iconImage.color = color;
					}
					break;
				}
				case States.Clicked: {
					if (clickedButtonSprite != null)
						this.currentImage.sprite = clickedButtonSprite;
					break;
				}
				case States.Disabled: {
					var color = this.currentImage.color;
					color.a = 0.5f;
					this.currentImage.color = color;
					if (icon != null) {
						var iconImage = this._iconGO.GetComponent <Image> ();
						color = iconImage.color;
						color.a = 0.5f;
						iconImage.color = color;
					}
					break;
				}
			}
			_currentState = value;
		}
	}
	
	[HideInInspector] [SerializeField]
	private Vector2 _size;
	[SerializeField]
	public Vector2 size {
		get {
			return _size;
		}
		set {
			_size = value;
			this.rectTransform.sizeDelta = _size;
			collider.size = new Vector3 (_size.x, _size.y, 1);
		}
	}
	
	[HideInInspector] [SerializeField]
	private RectTransform _rectTransform;
	[SerializeField]
	public RectTransform rectTransform { 
		get {
			if (_rectTransform == null)
				_rectTransform = GetComponent <RectTransform> ();
			return _rectTransform;
		}
	}
	
	[HideInInspector] [SerializeField]	
	private BoxCollider _collider;
	[SerializeField]
	public BoxCollider collider { 
		get {
			if (_collider == null)
				_collider = GetComponent <BoxCollider> ();
			return _collider;
		}
	}	
	
	[HideInInspector] [SerializeField]
	private Image _currentImage;
	[SerializeField]
	public Image currentImage { 
		get {
			if (_currentImage == null)
				_currentImage = GetComponent <Image> ();
			return _currentImage;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	void OnMouseDown () {
		if (this.currentState != States.Disabled) {
			this.currentState = States.Clicked;
			if (this.Clicked != null)
				this.Clicked (this, null);
		}
	}
	void OnMouseUp () {
		if (currentState != States.Disabled)
			this.currentState = States.Active;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
