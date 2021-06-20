#version 450

layout(location = 0) out vec4 color;

in vec2 vTextureCoordinates;

uniform float uOffset;
uniform sampler2D uTexture;

void main()
{
	color = texture(uTexture, vTextureCoordinates * vec2(uOffset, 1));
}