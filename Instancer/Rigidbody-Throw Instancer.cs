﻿#if VRLABS_INSTANCER_FOUND
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VRLabs.RigidbodyThrow
{
	public class RigidbodyThrow : ScriptableObject
	{
		public const string packageName = "Rigidbody-Throw";
		
		public static string[] excludeRegexs =
		{
			".*\\.cs",
			".*\\.asmdef",
			"package.json"
		};

		[MenuItem("VRLabs/Rigidbody Throw")]
		public static void FancyPackage()
		{
			Type instancerType = AppDomain.CurrentDomain.GetAssemblies()
				.Where(x => x.GetType("VRLabs.Instancer.Instancer") != null)
				.Select(x => x.GetType("VRLabs.Instancer.Instancer")).FirstOrDefault();

			if (instancerType == null)
			{
				Debug.LogError("Instancer not found. To use this functionality, install the VRLabs Instancer from https://github.com/VRLabs/Instancer");	
				return;
			}

			MethodInfo instanceMethod = instancerType.GetMethod("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			if (instanceMethod == null)
			{
				Debug.LogError("Instance method not found");
				return;
			}
			
			var editor = ScriptableObject.CreateInstance<RigidbodyThrow>();
			var script = MonoScript.FromScriptableObject(editor);
			var assetPath =  AssetDatabase.GetAssetPath(script);
			
			instanceMethod.Invoke(null, new object[] { packageName, assetPath, excludeRegexs });
		}
	}	
}
#endif