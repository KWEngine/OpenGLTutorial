#version 450

layout(location = 0) in vec3 aPosition;

uniform mat4 uMatrix;

void main()
{
	vec4 positionNew = uMatrix * vec4(aPosition, 1.0);
	gl_Position = positionNew;
}