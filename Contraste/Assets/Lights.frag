uniform sampler2D texture;

uniform vec4 spot0 = vec4(0);
uniform vec4 spot1 = vec4(0);
uniform vec4 spot2 = vec4(0);
uniform vec4 spot3 = vec4(0);
uniform vec4 spot4 = vec4(0);
uniform vec4 spot5 = vec4(0);
uniform vec4 spot6 = vec4(0);
uniform vec4 spot7 = vec4(0);
uniform vec4 spot8 = vec4(0);
uniform vec4 spot9 = vec4(0);
uniform vec4 spot10 = vec4(0);
uniform vec4 spot11 = vec4(0);
uniform vec4 spot12 = vec4(0);
uniform vec4 spot13 = vec4(0);
uniform vec4 spot14 = vec4(0);
uniform vec4 spot15 = vec4(0);
uniform vec4 spot16 = vec4(0);
uniform vec4 spot17 = vec4(0);
uniform vec4 spot18 = vec4(0);
uniform vec4 spot19 = vec4(0);
uniform vec4 spot20 = vec4(0);
uniform vec4 spot21 = vec4(0);
uniform vec4 spot22 = vec4(0);
uniform vec4 spot23 = vec4(0);
uniform vec4 spot24 = vec4(0);
uniform vec4 spot25 = vec4(0);
uniform vec4 spot26 = vec4(0);
uniform vec4 spot27 = vec4(0);
uniform vec4 spot28 = vec4(0);
uniform vec4 spot29 = vec4(0);
uniform vec4 spot30 = vec4(0);
uniform vec4 spot31 = vec4(0);

vec4 spots[] = vec4[]
(
	spot1,  spot2,  spot3,  spot4,  spot5,  spot6,  spot7,  spot8,
	spot9,  spot10, spot11, spot12, spot13, spot14, spot15, spot16,
	spot17, spot18, spot19, spot20, spot21, spot22, spot23, spot24,
	spot25, spot26, spot27, spot28, spot29, spot30, spot31, spot0
);

uniform float light = 0.75;

void main(void)
{
	vec4 color = texture2D(texture, gl_TexCoord[0].xy);
	color -= 1.0 - light;

	for (int i = 0; i < spots.length(); ++i)
	{
		vec4 spot = spots[i];
		float intensity = clamp((spot.z - distance(spot.xy, gl_FragCoord.xy)) / spot.z, 0, 1);
		color += vec4
		(
			intensity * 1.2 * spot.w,
			intensity * 1.0 * spot.w,
			intensity * 0.9 * spot.w,
			intensity
		);
	}
	
	gl_FragColor = gl_Color * color;
}