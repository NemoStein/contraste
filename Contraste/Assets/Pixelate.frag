uniform sampler2D texture;

void main(void)
{
	vec4 color = texture2D(texture, gl_TexCoord[0].xy);

	vec4 gray = (color.x + color.y + color.z) / 3;
	gray = round(gray * 3) / 3;

	color = color * 0.4 + gray * 0.6;
	
	gl_FragColor = gl_Color * color;
}