#version 450

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTextureCoordinates;

uniform mat4 uMatrix;

out vec2 vTextureCoordinates;

void main()
{
	vTextureCoordinates = aTextureCoordinates;
	vec4 positionNew = uMatrix * vec4(aPosition, 1.0);
	gl_Position = positionNew;
}