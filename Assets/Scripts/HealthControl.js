
var health1 : Texture2D;// 1 of health
var health2 : Texture2D;// 2 of health 
var health3 : Texture2D; // 3 of health

static var lives = 3;
function Start () {

}

function Update () {
	switch(lives)
	{
		case 1:
			guiTexture.texture = health1;
		break;
		
		case 2:
			guiTexture.texture = health2;
		break;
		
		case 3: 
			guiTexture.texture =health3;
		case 0:
			//defeat script here
		break;
	}
}