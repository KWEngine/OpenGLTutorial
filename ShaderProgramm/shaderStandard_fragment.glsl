#version 450

layout(location = 0) out vec4 color;

in vec2 vTextureCoordinates;
in vec3 vPosition;
in vec3 vNormal;

uniform sampler2D uTexture;
uniform vec3 uLightPositions[10];
uniform int uLightCount;
uniform vec3 uAmbientLight;

void main()
{
	
	float sumLightPower = 0;

	for(int i = 0; i < uLightCount; i++)
	{
		vec3 currentLightPosition = uLightPositions[i];

		vec3 surfaceToLight = currentLightPosition - vPosition;
		float surfaceToLightDistanceSquared = dot(surfaceToLight, surfaceToLight);
		vec3 surfaceToLightNormalized = normalize(surfaceToLight);

		float dotproduct = max(dot(surfaceToLightNormalized, vNormal), 0.0);

		sumLightPower = sumLightPower + (dotproduct * (10000.0 / surfaceToLightDistanceSquared));
	}

	vec3 textureColor = texture(uTexture, vTextureCoordinates).xyz;
	vec3 lightIntensity = vec3(sumLightPower, sumLightPower, sumLightPower);
	color = vec4(textureColor * (lightIntensity + uAmbientLight), 1.0);
}