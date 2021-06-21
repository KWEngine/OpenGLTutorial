#version 450

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTextureCoordinates;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aTangent;
layout(location = 4) in vec3 aBiTangent;

uniform mat4 uMatrix;

out vec2 vTextureCoordinates;

void main()
{
	vTextureCoordinates = aTextureCoordinates;
	gl_Position = uMatrix * vec4(aPosition, 1.0);
}