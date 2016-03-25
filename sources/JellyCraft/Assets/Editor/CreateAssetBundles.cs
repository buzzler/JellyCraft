using UnityEditor;
using UnityEngine;
using System;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Build AssetBundles iOS")]
	static void BuildAllAssetBundlesIOS () {
		BuildPipeline.BuildAssetBundles ("AssetBundles/iOS", BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.iOS);
	}

	[MenuItem ("Assets/Build AssetBundles Android")]
	static void BuildAllAssetBundlesAndroid () {
		BuildPipeline.BuildAssetBundles ("AssetBundles/Android", BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.Android);
	}

	[MenuItem ("Assets/Build AssetBundles Mobile")]
	static void BuildAllAssetBundleMobile() {
		BuildAllAssetBundlesIOS();
		BuildAllAssetBundlesAndroid();
	}
}