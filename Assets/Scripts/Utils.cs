using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Collections.Generic;

public static class Utils {
	/// <summary>
	/// Log an event for both unity analytics and amplitude.
	/// </summary>
	public static void LogEvent (string eventLine)
	{
		#if UNITY_WEBGL
		AmplitudeSDKWebGL.Instance.LogEvent (eventLine);
		#else
		Amplitude.Instance.logEvent (eventLine);
		#endif

        Analytics.CustomEvent (eventLine);      
    }

	/// <summary>
	/// Log an event for both unity analytics and amplitude.
	/// </summary>
	public static void LogEvent (string eventLine, Dictionary<string, object> eventProperties)
	{
		#if UNITY_WEBGL
		AmplitudeSDKWebGL.Instance.LogEvent (eventLine, eventProperties);
		#else
		Amplitude.Instance.logEvent (eventLine, eventProperties);
		#endif

        Analytics.CustomEvent (eventLine, eventProperties);
	}

	public static void LogError (string scriptName, string methodName, string errorDescription)
	{
		Debug.LogError (scriptName + " :: " + methodName + " :: " + errorDescription);
		 
		LogEvent ("LogError", new Dictionary<string, object> ()
		{
			{ "Description", errorDescription },
			{ "Method", methodName },
			{ "Script", scriptName }
		});
	}
}
