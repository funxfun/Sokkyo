using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SendPostEventData
{
//    public string user_id;
    public string device_id;
    public string event_type;
    //        public long time;
    public Dictionary<string, object> event_properties;
    public Dictionary<string, object> user_properties;
    //        public groups;
    public string app_version;
    public string platform;
    public string os_name;
    public string device_brand;
    public string device_manufacturer;
    public string device_model;
    public string carrier;
    public string country;
    public string region;
    public string city;
    public string dma;
    public string language;
//    public float price;
//    public int quantity;
//    public float revenue;
//    public string productId;
//    public string revenueType;
//    public float location_lat;
//    public float location_lng;
    public string ip;
}


/// <summary>
/// A quick and dirty wrapper hack to allow us to log events to Amplitude in Unity WebGL via their HTTP API.
/// </summary>
public class AmplitudeSDKWebGL : MonoBehaviour
{
	public static readonly string API_KEY = "a9d5930b92947b5171639869f1fcb2ec";
    public static readonly string API_URL = "https://api.amplitude.com/httpapi";

    private static AmplitudeSDKWebGL _instance = null;
    private bool _isInitialized = false;
    private bool _isLocationInfoSettingFinished = false;
    private bool _isSessionStarted = false;
    private string _ip = null;
    private string _country = null;
    private string _region = null;
    private string _city = null;
    private static readonly string START_SESSION = "[Amplitude] Start Session";
    private static readonly string END_SESSION = "[Amplitude] End Session";

    public static AmplitudeSDKWebGL Instance
    {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AmplitudeSDKWebGL>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("AmplitudeSDKWebGL");
                    DontDestroyOnLoad(container);
                    _instance = container.AddComponent<AmplitudeSDKWebGL>();
                }
            }
     
            return _instance;
        }
    }

    public void Init()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;

        SetLocationInfo();

        LogEvent(START_SESSION);
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");

        LogEvent(END_SESSION);
    }

    private void SetLocationInfo()
    {
        string url = "";

        StartCoroutine(GetIp2Geo());
    }

    IEnumerator GetIp2Geo() {
        WWW www = new WWW("https://freegeoip.net/json/");
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError("Ip2Geo error: " + www.error);
            yield break;
        }

        try {
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(www.text);
            _ip = values["ip"];
            _country = values["country_name"];
            _region = values["region_name"];
            _city = values["city"];
        } catch (System.Exception e) {
            Debug.LogError(e.Message);
        }
        _isLocationInfoSettingFinished = true;

        string _outputText = "IP address: " + _ip;
        _outputText += ", Country: " + _country;
        _outputText += ", Region: " + _region;
        _outputText += ", City: " + _city;
        Debug.Log("Ip2Geo: " + _outputText);
    }

    private SendPostEventData GetNewSendPostEventData()
    {
        SendPostEventData data = new SendPostEventData();
        data.device_id = SystemInfo.deviceUniqueIdentifier;
        data.app_version = Application.version;
        data.platform = Application.platform.ToString();
        data.os_name = SystemInfo.operatingSystem;
        data.device_manufacturer = SystemInfo.operatingSystemFamily.ToString();
        data.device_brand = SystemInfo.deviceType.ToString();
        data.device_model = SystemInfo.deviceModel;
        data.dma = SystemInfo.deviceName;
        data.language = Application.systemLanguage.ToString();
        data.ip = _ip;
        data.country = _country;
        data.region = _region;
        data.city = _city;
        data.user_properties = new Dictionary<string, object>() {
            {"StoreType", "Stripe"},
            {"platform", "WebGL"}
        };
        return data;
    }

    public void LogEvent(string eventName, Dictionary<string, object> eventProperties = null)
    {
        if (!_isInitialized)
        {
            Init();
        }

        StartCoroutine(LogEventCoroutine(eventName, eventProperties));
    }

     IEnumerator LogEventCoroutine(string eventName, Dictionary<string, object> eventProperties) {
        // Queue everything until we have our location info
        yield return new WaitUntil(() => _isLocationInfoSettingFinished);

        // Queue everything else until [Amplitude] Start Session has been sent
        yield return new WaitUntil(() => (!eventName.Equals(START_SESSION) ? _isSessionStarted : true));

        SendPostEventData newEvent = GetNewSendPostEventData();
        newEvent.event_type = eventName;
        if (eventProperties != null)
            newEvent.event_properties = eventProperties;

        string jsonString = "[" + JsonConvert.SerializeObject(newEvent) + "]";
        string url = API_URL + "?api_key=" + API_KEY + "&event=" + WWW.EscapeURL(jsonString);
//        Debug.Log("url: " + url);

        WWW www = new WWW(url);
        yield return www;

        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError("Amplitude log event " + eventName + " error: " + www.error);
        } else {
            if (eventName.Equals(START_SESSION))
                _isSessionStarted = true;
//            Debug.Log(eventName + " logged: " + www.text);
        }
    }
}
