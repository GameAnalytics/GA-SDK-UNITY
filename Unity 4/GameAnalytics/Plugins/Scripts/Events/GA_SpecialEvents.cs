/// <summary>
/// This class handles special events unique to the Unity Wrapper, such as submitting level/scene changes, and delaying application quit
/// until data has been sent.
/// </summary>

using UnityEngine;
using System.Collections;

namespace GameAnalyticsSDK.Events
{
	public class GA_SpecialEvents : MonoBehaviour
	{
		/*[HideInInspector]
		public bool SubmitFpsAverage;
		[HideInInspector]
		public bool SubmitFpsCritical;
		[HideInInspector]
		public bool IncludeSceneChange;
		[HideInInspector]
		public int FpsCriticalThreshold;
		[HideInInspector]
		public int FpsSubmitInterval;*/
		#region private values
		
		private static int _frameCountAvg = 0;
		private static float _lastUpdateAvg = 0f;
		private int _frameCountCrit = 0;
		private float _lastUpdateCrit = 0f;

		private static int _criticalFpsCount = 0;
		
		#endregion
		
		#region unity derived methods
		
		public void Start ()
		{
			StartCoroutine(SubmitFPSRoutine());
			StartCoroutine(CheckCriticalFPSRoutine());
		}
		
		private IEnumerator SubmitFPSRoutine()
		{
			while(Application.isPlaying && GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				yield return new WaitForSeconds(30);
				SubmitFPS();
			}
		}
		
		private IEnumerator CheckCriticalFPSRoutine()
		{
			while(Application.isPlaying && GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				yield return new WaitForSeconds(GameAnalytics.SettingsGA.FpsCirticalSubmitInterval);
				CheckCriticalFPS();
			}
		}
		
		public void Update()
		{
			//average FPS
			if (GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				_frameCountAvg++;
			}
			
			//critical FPS
			if (GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				_frameCountCrit++;
			}
		}
		
		public static void SubmitFPS()
		{
			//average FPS
			if (GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				float timeSinceUpdate = Time.time - _lastUpdateAvg;
				
				if (timeSinceUpdate > 1.0f)
				{
					float fpsSinceUpdate = _frameCountAvg / timeSinceUpdate;
					_lastUpdateAvg = Time.time;
					_frameCountAvg = 0;
					
					if (fpsSinceUpdate > 0)
					{
						GameAnalytics.NewDesignEvent("GA:AverageFPS", ((int)fpsSinceUpdate));
					}
				}
			}

			if (GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				if (_criticalFpsCount > 0)
				{
					GameAnalytics.NewDesignEvent("GA:CriticalFPS", _criticalFpsCount);
					_criticalFpsCount = 0;
				}
			}
		}
		
		public void CheckCriticalFPS()
		{
			//critical FPS
			if (GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				float timeSinceUpdate = Time.time - _lastUpdateCrit;
				
				if (timeSinceUpdate >= 1.0f)
				{
					float fpsSinceUpdate = _frameCountCrit / timeSinceUpdate;
					_lastUpdateCrit = Time.time;
					_frameCountCrit = 0;
					
					if (fpsSinceUpdate <= GameAnalytics.SettingsGA.FpsCriticalThreshold)
					{
						_criticalFpsCount++;
					}
				}
			}
		}
		
		#endregion
	}
}
