namespace _3D_SHOOTER
{
    public static class Shaders
    {
        public const string LightingFrag =
              "#version 330\r\n\r\nout vec4 FragColor;\r\nuniform vec4 objectColor; \r\nuniform vec3 lightColor; \r\nuniform vec3 lightPos; \r\n\r\nin vec3 Normal; \r\nin vec3 FragPos; \r\n\r\nvoid main()\r\n{\r\n    float ambientStrength = 0.25;\r\n    vec3 ambient = ambientStrength * lightColor;\r\n    vec3 norm = normalize(Normal);\r\n    vec3 lightDir = normalize(lightPos - FragPos);\r\n    float diff = max(dot(norm, lightDir), 0.0) * 0.75;\r\n    vec3 diffuse = diff * lightColor;\r\n    vec4 result = vec4((ambient + diffuse),1.0) * objectColor;\r\n    FragColor = result;\r\n}";

        public const string TextureLightingFrag =
              "#version 330\r\n\r\nout vec4 FragColor;\r\nuniform vec3 lightColor;\r\nuniform sampler2D texture0;\r\nin vec2 textureCords;\r\nuniform vec3 lightPos;\r\nin vec3 Normal;\r\nin vec3 FragPos;\r\nvoid main()\r\n{\r\nfloat ambientStrength = 0.25f;\r\nvec3 ambient = ambientStrength * lightColor;\r\nvec3 norm = normalize(Normal);\r\nvec3 lightDir = normalize(lightPos - FragPos);\r\nfloat diff = max(dot(norm, lightDir), 0.0);\r\nvec3 diffuse = diff * lightColor;\r\nvec3 objectColor = texture(texture0, textureCords).xyz;\r\nvec3 phong = (ambient + diffuse) * objectColor;\r\nFragColor = vec4(phong, 1.0f);\r\n}";
        
        public const string ShaderFrag =
              "#version 330\r\n\r\nout vec4 FragColor;\r\nuniform vec3 lightColor;\r\n\r\nvoid main()\r\n{\r\n    FragColor = vec4(lightColor, 1.0); // set all 4 vector values to 1.0\r\n}\r\n";
        
        public const string ShaderVert =
              "#version 330\r\n\r\nlayout (location = 0) in vec3 aPos;\r\nlayout (location = 1) in vec3 aNormal;\r\n\r\nuniform mat4 model;\r\nuniform mat4 view;\r\nuniform mat4 projection;\r\n\r\nout vec3 Normal;\r\nout vec3 FragPos;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = vec4(aPos, 1.0) * model * view * projection;\r\n    FragPos = vec3(vec4(aPos, 1.0) * model);\r\n    Normal = -(aNormal * mat3(transpose(inverse(model))));\r\n}";
        
        public const string Shader2DFrag =
              "#version 330\r\n\r\nout vec4 FragColor;\r\nuniform vec4 lightColor;\r\n\r\nvoid main()\r\n{\r\n    FragColor = lightColor; \r\n\r\n}";
        
        public const string Shader2DVert =
              "#version 330\r\n\r\nlayout (location = 0) in vec3 aPos;\r\n\r\nvoid main(void)\r\n{\r\n    gl_Position = vec4(aPos, 1.0);    \r\n}";
        
        public const string TextureFrag =
              "#version 330\r\n\r\nout vec4 FragColor;\r\nuniform sampler2D texture0;\r\n\r\nin vec3 Normal;\r\nin vec3 FragPos;\r\nin vec2 textureCords;\r\n\r\nvoid main()\r\n{\r\n    FragColor = texture(texture0, textureCords);\r\n}";
        
        public const string TextureVert =
              "#version 330\r\n\r\nlayout (location = 0) in vec3 aPos;\r\nlayout (location = 1) in vec3 aNormal;\r\nlayout (location = 2) in vec2 aTexture;\r\n\r\nuniform mat4 model;\r\nuniform mat4 view;\r\nuniform mat4 projection;\r\n\r\nout vec3 Normal;\r\nout vec3 FragPos;\r\nout vec2 textureCords;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = vec4(aPos, 1.0) * model * view * projection;\r\n    textureCords = aTexture;\r\n    FragPos = vec3(vec4(aPos, 1.0) * model);\r\n    Normal = -(aNormal * mat3(transpose(inverse(model))));\r\n}";
        
        public const string Texture2DFrag =
              "#version 330\r\n\r\nout vec4 outputColor;\r\n\r\nin vec2 texCoord;\r\nuniform sampler2D texture0;\r\n\r\nvoid main()\r\n{\r\n    outputColor = texture(texture0, texCoord);\r\n}";
        
        public const string Texture2DVert =
              "#version 330\r\n\r\nlayout(location = 0) in vec3 aPosition;\r\nlayout(location = 1) in vec2 aTexCoord;\r\n\r\nout vec2 texCoord;\r\nuniform mat4 transform;\r\nvoid main(void)\r\n{\r\n    texCoord = aTexCoord;\r\n\r\n    gl_Position = vec4(aPosition, 1.0) * transform;\r\n}";
    }
}
