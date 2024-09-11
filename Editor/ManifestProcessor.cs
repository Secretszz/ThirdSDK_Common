// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ManifestProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 19:13:02
// *******************************************

using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_ANDROID
namespace Bridge.Common
{
    using System.Xml.Linq;
    using UnityEditor;
    using UnityEditor.Callbacks;

    public static class ManifestProcessor
    {
        /// <summary>
        /// 依赖包文件夹位置
        /// </summary>
        public const string MAIN_LIB_DIR = "unityLibrary/ThirdSDK.androidlib";
        public const string NATIVE_CODE_DIR = MAIN_LIB_DIR + "/src/main/java/com/bridge";
        public const string MANIFEST_RELATIVE_PATH = MAIN_LIB_DIR + "/src/main/AndroidManifest.xml";
        public const string STRINGS_XML_PATH = MAIN_LIB_DIR + "/src/main/res/values/strings.xml";
        public const string BUILD_GRADLE_PATH = MAIN_LIB_DIR + "/build.gradle";
        public const string LIB_Dir = MAIN_LIB_DIR + "/libs";

        public static XNamespace ns = "http://schemas.android.com/apk/res/android";

        public const string FACEBOOK_DEPENDENCIES = "##FACEBOOK_DEPENDENCIES##";
        public const string XHS_DEPENDENCIES = "##XHS_DEPENDENCIES##";
        public const string WX_DEPENDENCIES = "##WX_DEPENDENCIES##";

        public static Dictionary<string, string> ReplaceBuildDefinedCache = new Dictionary<string, string>()
        {
                {FACEBOOK_DEPENDENCIES, string.Empty},
                {XHS_DEPENDENCIES, string.Empty},
                {WX_DEPENDENCIES, string.Empty}
        };

        [PostProcessBuild(10100)]
        public static void OnPostprocessBuild(BuildTarget target, string projectPath)
        {
            var filePath = Path.Combine(projectPath, BUILD_GRADLE_PATH);
            var code = new StringBuilder(File.ReadAllText(filePath));
            foreach (var temp in ReplaceBuildDefinedCache)
            {
                code = code.Replace(temp.Key, temp.Value);
            }
            File.WriteAllText(filePath, code.ToString());
        }
    }
}
#endif
