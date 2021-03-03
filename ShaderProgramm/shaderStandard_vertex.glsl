#version 450

layout(location = 0) in vec3 aPosition;

void main()
{
	vec4 positionNew = vec4(aPosition, 1.0);
	gl_Position = positionNew;
}