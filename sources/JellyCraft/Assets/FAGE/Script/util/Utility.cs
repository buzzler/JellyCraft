using UnityEngine;
using System.Collections;
using System.IO;

public class Utility {
	public	static int GetPrefInt(string key) {
		return PlayerPrefs.GetInt(key);
	}

	public	static float GetPrefFloat(string key) {
		return PlayerPrefs.GetFloat(key);
	}

	public	static string GetPrefString(string key) {
		return PlayerPrefs.GetString(key);
	}

	public	static void SetPrefInt(string key, int value) {
		PlayerPrefs.SetInt(key, value);
	}

	public	static void SetPrefFloat(string key, float value) {
		PlayerPrefs.SetFloat(key, value);
	}

	public	static void SetPrefString(string key, string value) {
		PlayerPrefs.SetString(key, value);
	}

	public	static bool HasKey(string key) {
		return PlayerPrefs.HasKey(key);
	}
}
