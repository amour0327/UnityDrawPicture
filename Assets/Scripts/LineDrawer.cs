using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour {
	
	[SerializeField]
	protected UITexture tex;
	
	[SerializeField]
	[Range( 1, 10 )]
	protected float lineWidth = 10;

	[SerializeField]
	protected Color lineColor = Color.black;
	
	[SerializeField]
	[Range( 0, 1 )]
	protected float r = 0;
	
	[SerializeField]
	[Range( 0, 1 )]
	protected float g = 0;
	
	[SerializeField]
	[Range( 0, 1 )]
	protected float b = 0;
	
	[SerializeField]
	[Range( 0, 1 )]
	protected float a = 1;
	
	private Vector3 P1 = Vector3.zero;
	private Vector3 P2 = Vector3.zero;
	private Vector3 P3 = Vector3.zero;
	private Texture2D texture;
	
	// Use this for initialization
	 void Start () {
		tex.mainTexture = new Texture2D( Screen.width, Screen.height );
		tex.width = Screen.width;
		tex.height = Screen.height;
		texture = tex.mainTexture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.touchCount == 1) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				P1 = new Vector3( Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0 );
				P2 = Vector3.zero;
				P3 = Vector3.zero;
			}
		
	 		if( Input.GetTouch(0).phase == TouchPhase.Moved ){
				P3 = P2;
				P2 = P1;
				P1 = new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,0);
				
				if ( P1 != Vector3.zero && P2 != Vector3.zero && P3 != Vector3.zero ) {
					Bezier( ( P1 + P2 ) / 2 , P2, ( P2 +P3 ) /2 );
					texture.Apply();
				}
			}
	 	
			if( Input.GetTouch(0).phase == TouchPhase.Ended ){
			}
		}
	}
	
	void OnGUI () {
		//large
		GUI.Label(new Rect(10,Screen.height-20, 100, 20), "line:"+lineWidth.ToString() );
		lineWidth= GUI.HorizontalSlider( new Rect(60, Screen.height-20, 100, 20), lineWidth, 1, 20);
	
		//R
		GUI.Label(new Rect(Screen.width/2,Screen.height-80, 100, 20), "R:"+lineColor.r.ToString() );
		r = GUI.HorizontalSlider(new Rect(Screen.width/2+30, Screen.height-80, 100, 20), r, 0,1);
		
		//G
		GUI.Label(new Rect(Screen.width/2,Screen.height-60, 100, 20), "G:"+lineColor.g.ToString() );
		g = GUI.HorizontalSlider(new Rect(Screen.width/2+30, Screen.height-60, 100, 20), g, 0,1);
		
		//B
		GUI.Label(new Rect(Screen.width/2,Screen.height-40, 100, 20), "B:"+lineColor.b.ToString() );
		b = GUI.HorizontalSlider(new Rect(Screen.width/2+30, Screen.height-40, 100, 20), b, 0,1);
		
		//A
		GUI.Label(new Rect(Screen.width/2,Screen.height-20, 100, 20), "A:"+lineColor.a.ToString() );
		a = GUI.HorizontalSlider(new Rect(Screen.width/2+30, Screen.height-20, 100, 20), a, 0,1);
	
		lineColor= new Color( r, g, b, a );
	}
	
	void Bezier( Vector3 p1, Vector3 p2, Vector3 p3 )
	{
		float t = 0;
		while( t <= 1 ) {
			float x = p1.x * ( 1 - t ) * ( 1 - t ) + 2 * p2.x * ( 1 - t ) * t + p3.x * t * t;
			float y = p1.y * ( 1 - t ) * ( 1 - t ) + 2 * p2.y * ( 1 - t ) * t + p3.y * t * t;
			texture.SetPixel( (int)x, (int)y, lineColor );
			int w;
			int h;
			for( h = 1; h <= lineWidth && (int)y + h < texture.height; ++h ) {
				for( w = 1; w <= lineWidth && (int)x + w < texture.width; ++w){
	       			texture.SetPixel( (int)x + w, (int)y + h, lineColor );
				}
			}
			t += 0.01f;
		}
	}
}
