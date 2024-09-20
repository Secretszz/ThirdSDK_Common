// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ManifestProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2023/12/04 19:13:02
// *******************************************

#if UNITY_ANDROID
namespace Bridge.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEngine;
    using System.Xml.Linq;
    using UnityEditor;
    using UnityEditor.Callbacks;

    public static class ManifestProcessor
    {
        /// <summary>
        /// 依赖包文件夹位置
        /// </summary>
        private const string MAIN_LIB_DIR = "unityLibrary/ThirdSDK.androidlib";
        private const string MANIFEST_RELATIVE_PATH = MAIN_LIB_DIR + "/src/main/AndroidManifest.xml";
        private const string BUILD_GRADLE_PATH = MAIN_LIB_DIR + "/build.gradle";
        public const string NATIVE_CODE_DIR = MAIN_LIB_DIR + "/src/main/java/com/bridge";
        public const string STRINGS_XML_PATH = MAIN_LIB_DIR + "/src/main/res/values/strings.xml";
        public const string LIB_Dir = MAIN_LIB_DIR + "/libs";

        public static XNamespace ns = "http://schemas.android.com/apk/res/android";

        public const string FACEBOOK_DEPENDENCIES = "##FACEBOOK_DEPENDENCIES##";
        public const string XHS_DEPENDENCIES = "##XHS_DEPENDENCIES##";
        public const string WX_DEPENDENCIES = "##WX_DEPENDENCIES##";
        public const string ALI_DEPENDENCIES = "##ALI_DEPENDENCIES##";

        public static Dictionary<string, string> ReplaceBuildDefinedCache = new Dictionary<string, string>()
        {
                {FACEBOOK_DEPENDENCIES, string.Empty},
                {XHS_DEPENDENCIES, string.Empty},
                {WX_DEPENDENCIES, string.Empty},
                {ALI_DEPENDENCIES, string.Empty}
        };

        public static List<XElement> QueriesElements = new List<XElement>();
        public static List<XElement> ApplicationElements = new List<XElement>();

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

            RefreshLaunchManifest(projectPath);
        }
        
        private static void RefreshLaunchManifest(string projectPath)
        {
            string manifestPath = Path.Combine(projectPath, MANIFEST_RELATIVE_PATH);

            XDocument manifest;
            try
            {
                manifest = XDocument.Load(manifestPath);
            }
#pragma warning disable 0168
            catch (IOException e)
#pragma warning restore 0168
            {
                LogBuildFailed();
                return;
            }

            XElement elemManifest = manifest.Element("manifest");
            if (elemManifest == null)
            {
                LogBuildFailed();
                return;
            }
            
            XElement queries = elemManifest.Element("queries");
            if (queries == null)
            {
                queries = new XElement("queries");
                elemManifest.Add(queries);
            }

            for (int i = 0; i < QueriesElements.Count; i++)
            {
                queries.Add(QueriesElements[i]);
            }

            XElement elemApplication = elemManifest.Element("application");
            if (elemApplication == null)
            {
                LogBuildFailed();
                return;
            }

            for (int i = 0; i < ApplicationElements.Count; i++)
            {
                elemApplication.Add(ApplicationElements[i]);
            }

            elemManifest.Save(manifestPath);
        }
        
        private static void LogBuildFailed()
        {
            Debug.LogWarning("设置配置失败");
        }
    }
}
#endif
