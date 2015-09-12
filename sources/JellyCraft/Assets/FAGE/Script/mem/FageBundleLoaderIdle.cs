using UnityEngine;
using System.Collections;

public class FageBundleLoaderIdle : FageState {
	public override void Excute (FageStateMachine stateMachine) {
		base.Excute (stateMachine);
		FageBundleLoader loader = stateMachine as FageBundleLoader;
		if ((loader.expireTime < (Time.unscaledTime - loader.GetUpdateTime())) || loader.flagUpdate) {
			loader.flagUpdate = false;
			loader.ReserveState("FageBundleLoaderCheck");
		} else if (loader.flagLoad) {
			loader.ReserveState("FageBundleLoaderLoad");
		} else if (loader.flagDownload) {
			loader.flagDownload = false;
			loader.ReserveState("FageBundleLoaderDownload");
		}
	}
}
