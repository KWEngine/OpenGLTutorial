#version 450

layout(location = 0) out vec4 color;

in vec2 vTextureCoordinates;

uniform sampler2D uTexture;
uniform int uIsCollider;

const vec4 colliderColor = vec4(1,0.5,1,1);

void main()
{
	color = texture(uTexture, vTextureCoordinates);
	if(uIsCollider > 0)
	{
		color *= colliderColor;
	}
}