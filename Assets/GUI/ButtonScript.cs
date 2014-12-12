using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (Text))]
public class ButtonScript : MonoBehaviour {
	public enum States {
		Active,
		Clicked,
		Disabled,
	}
	public Sprite activeButtonSprite;
	public Sprite clickedButtonSprite;
	
	[HideInInspector] [SerializeField]
	private GameObject _textGO;
	[HideInInspector] [SerializeField]
	private Text _text;
	[SerializeField]
	public string text { 
		get {
			if (_text != null)
				return _text.text;
			else 
				return null;
		}
		set {
			if (_textGO == null) {
				_textGO = new GameObject ("text");
				_text = _textGO.AddComponent <Text> ();
				_text.color = Color.black;
				_text.alignment = TextAnchor.MiddleCenter;
				_text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			}
			_textGO.SetActive (true);
			_textGO.transform.SetParent(this.transform, false);
			_textGO.transform.localPosition = new Vector3 (0, 0, -0.1f);
			_textGO.transform.localScale = Vector3.one;
			_textGO.GetComponent <RectTransform> ().sizeDelta = this.size;
			_text = _textGO.GetComponent <Text> ();
			_text.text = value;
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
					if (text != null) {
						var iconImage = this._textGO.GetComponent <Image> ();
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
			if (_currentImage == null) {
				_currentImage = GetComponent <Image> ();
			}
			_currentImage.type = Image.Type.Sliced;
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
