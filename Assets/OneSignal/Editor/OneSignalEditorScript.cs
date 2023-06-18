﻿/**
 * Modified MIT License
 * 
 * Copyright 2016 OneSignal
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * 1. The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * 2. All copies of substantial portions of the Software may only be used in connection
 * with services provided by OneSignal.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.IO;

using UnityEditor;

#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;

[InitializeOnLoad]
public class OneSignalEditorScriptAndroid : AssetPostprocessor {

   /// <summary>Instance of the PlayServicesSupport resolver</summary>
   public static object svcSupport;

   private static readonly string PluginName = "OneSignal";
   
   // Both are set to LATEST to ensure compability between other plugins.
   // PLAY_SERVICES_VERSION   - Tested with versions 10.2.1 to 11.2.0
   // ANDROID_SUPPORT_VERSION - Tested with versions 26.0.0 through 26.0.1
   private static readonly string PLAY_SERVICES_VERSION = "+";
   private static readonly string ANDROID_SUPPORT_VERSION = "+";

   static OneSignalEditorScriptAndroid() {
      createOneSignalAndroidManifest();
      addGMSLibrary();
   }

   private static void addGMSLibrary() {
      Type playServicesSupport = Google.VersionHandler.FindClass(
         "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
      if (playServicesSupport == null)
         return;

      svcSupport = svcSupport ?? Google.VersionHandler.InvokeStaticMethod(
        playServicesSupport, "CreateInstance",
        new object[] {
                PluginName,
                EditorPrefs.GetString("AndroidSdkRoot"),
                "ProjectSettings"
        });

      Google.VersionHandler.InvokeInstanceMethod(
         svcSupport, "DependOn",
         new object[] {
            "com.google.android.gms",
            "play-services-gcm",
            PLAY_SERVICES_VERSION
         },
         namedArgs: new Dictionary<string, object>() {
             {"packageIds", new string[] { "extra-google-m2repository" } }
         });

      Google.VersionHandler.InvokeInstanceMethod(
         svcSupport, "DependOn",
         new object[] {
            "com.google.android.gms",
            "play-services-location",
            PLAY_SERVICES_VERSION
         },
         namedArgs: new Dictionary<string, object>() {
             {"packageIds", new string[] { "extra-google-m2repository" } }
         });


      Google.VersionHandler.InvokeInstanceMethod(
         svcSupport, "DependOn",
         new object[] {
            "com.android.support",
            "customtabs",
            ANDROID_SUPPORT_VERSION
         },
         namedArgs: new Dictionary<string, object>() {
             {"packageIds", new string[] { "extra-android-m2repository" } }
         });
         
         
      Google.VersionHandler.InvokeInstanceMethod(
         svcSupport, "DependOn",
         new object[] {
            "com.android.support",
            "support-v4",
            ANDROID_SUPPORT_VERSION
         },
         namedArgs: new Dictionary<string, object>() {
             {"packageIds", new string[] { "extra-android-m2repository" } }
         });
   }
   
   // Copies `AndroidManifestTemplate.xml` to `AndroidManifest.xml`
   //   then replace `${manifestApplicationId}` with current packagename in the Unity settings.
   private static void createOneSignalAndroidManifest() {
      string oneSignalConfigPath = "Assets/Plugins/Android/OneSignalConfig/";
      string manifestFullPath = oneSignalConfigPath + "AndroidManifest.xml";

      File.Copy(oneSignalConfigPath + "AndroidManifestTemplate.xml", manifestFullPath, true);

      StreamReader streamReader = new StreamReader(manifestFullPath);
      string body = streamReader.ReadToEnd();
      streamReader.Close();

      #if UNITY_5_6_OR_NEWER
         body = body.Replace("${manifestApplicationId}", PlayerSettings.applicationIdentifier);
      #else
         body = body.Replace("${manifestApplicationId}", PlayerSettings.bundleIdentifier);
      #endif
      using (var streamWriter = new StreamWriter(manifestFullPath, false)) {
         streamWriter.Write(body);
      }
   }
}
#endif