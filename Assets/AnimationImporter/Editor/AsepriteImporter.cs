﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using AnimationImporter.Boomlagoon.JSON;
using UnityEditor;
using System.IO;

namespace AnimationImporter
{
	public class AsepriteImporter
	{
		// ================================================================================
		//  const
		// --------------------------------------------------------------------------------

		const string ASEPRITE_STANDARD_PATH_WINDOWS = @"C:\Program Files (x86)\Aseprite\Aseprite.exe";
		const string ASEPRITE_STANDARD_PATH_MACOSX = @"/Applications/Aseprite.app/Contents/MacOS/aseprite";

		public static string standardApplicationPath
		{
			get
			{
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					return ASEPRITE_STANDARD_PATH_WINDOWS;
				}
				else
				{
					return ASEPRITE_STANDARD_PATH_MACOSX;
				}
			}
		}

		// ================================================================================
		//  calling Aseprite for creating PNG and JSON files
		// --------------------------------------------------------------------------------

		/// <summary>
		/// calls the Aseprite application which then should output a png with all sprites and a corresponding JSON
		/// </summary>
		/// <returns></returns>
		public static bool CreateSpriteAtlasAndMetaFile(string applicationPath, string additionalCommandLineArguments, string assetBasePath, string fileName, string assetName, bool saveSpritesToSubfolder = true)
		{
			char delimiter = '\"';
			string parameters = "--data " + delimiter + assetName + ".json" + delimiter + " --sheet " + delimiter + assetName + ".png" + delimiter + " --sheet-pack --list-tags --format json-array " + delimiter + fileName + delimiter;

			if (!string.IsNullOrEmpty(additionalCommandLineArguments))
			{
				parameters = additionalCommandLineArguments + " " + parameters;
			}

			bool success = CallAsepriteCLI(applicationPath, assetBasePath, parameters) == 0;

			// move png and json file to subfolder
			if (success && saveSpritesToSubfolder)
			{
				// create subdirectory
				if (!Directory.Exists(assetBasePath + "/Sprites"))
					Directory.CreateDirectory(assetBasePath + "/Sprites");

				string target = assetBasePath + "/Sprites/" + assetName + ".json";
				if (File.Exists(target))
					File.Delete(target);
				File.Move(assetBasePath + "/" + assetName + ".json", target);

				target = assetBasePath + "/Sprites/" + assetName + ".png";
				if (File.Exists(target))
					File.Delete(target);
				File.Move(assetBasePath + "/" + assetName + ".png", target);
			}

			return success;
		}

		private static int CallAsepriteCLI(string asepritePath, string path, string buildOptions)
		{
			string workingDirectory = Application.dataPath.Replace("Assets", "") + path;

			System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
			start.Arguments = "-b " + buildOptions;
			start.FileName = asepritePath;
			start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			start.CreateNoWindow = true;
			start.UseShellExecute = false;
			start.WorkingDirectory = workingDirectory;

			// Run the external process & wait for it to finish
			using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(start))
			{
				proc.WaitForExit();
				// Retrieve the app's exit code
				return proc.ExitCode;
			}
		}

		// ================================================================================
		//  JSON IMPORT
		// --------------------------------------------------------------------------------

		public static ImportedAnimationSheet GetAnimationInfo(JSONObject root)
		{
			if (root == null)
			{
				Debug.LogWarning("Error importing JSON animation info: JSONObject is NULL");
				return null;
			}

			ImportedAnimationSheet animationSheet = new ImportedAnimationSheet();

			// import all informations from JSON

			if (!root.ContainsKey("meta"))
			{
				Debug.LogWarning("Error importing JSON animation info: no 'meta' object");
				return null;
			}
			var meta = root["meta"].Obj;
			GetMetaInfosFromJSON(animationSheet, meta);

			if (GetAnimationsFromJSON(animationSheet, meta) == false)
			{
				return null;
			}		

			if (GetFramesFromJSON(animationSheet, root) == false)
			{
				return null;
			}

			animationSheet.ApplyCreatedFrames();

			return animationSheet;
		}

		private static void GetMetaInfosFromJSON(ImportedAnimationSheet animationSheet, JSONObject meta)
		{
			var size = meta["size"].Obj;
			animationSheet.width = (int)size["w"].Number;
			animationSheet.height = (int)size["h"].Number;
		}

		private static bool GetAnimationsFromJSON(ImportedAnimationSheet animationSheet, JSONObject meta)
		{
			if (!meta.ContainsKey("frameTags"))
			{
				Debug.LogWarning("No 'frameTags' found in JSON created by Aseprite.");
				IssueVersionWarning();
				return false;
			}

			var frameTags = meta["frameTags"].Array;
			foreach (var item in frameTags)
			{
				JSONObject frameTag = item.Obj;
				ImportedAnimation anim = new ImportedAnimation();
				anim.name = frameTag["name"].Str;
				anim.firstSpriteIndex = (int)(frameTag["from"].Number);
				anim.lastSpriteIndex = (int)(frameTag["to"].Number);

				animationSheet.animations.Add(anim);
			}

			return true;
		}

		private static bool GetFramesFromJSON(ImportedAnimationSheet animationSheet, JSONObject root)
		{
			var list = root["frames"].Array;

			if (list == null)
			{
				Debug.LogWarning("No 'frames' array found in JSON created by Aseprite.");
				IssueVersionWarning();
				return false;
			}

			foreach (var item in list)
			{
				ImportedAnimationFrame frame = new ImportedAnimationFrame();
				frame.name = Path.GetFileNameWithoutExtension(item.Obj["filename"].Str);

				var frameValues = item.Obj["frame"].Obj;
				frame.width = (int)frameValues["w"].Number;
				frame.height = (int)frameValues["h"].Number;
				frame.x = (int)frameValues["x"].Number;
				frame.y = animationSheet.height - (int)frameValues["y"].Number - frame.height; // unity has a different coord system

				frame.duration = (int)item.Obj["duration"].Number;

				animationSheet.frames.Add(frame);
			}

			return true;
		}

		private static void IssueVersionWarning()
		{
			Debug.LogWarning("Please use official Aseprite 1.1.1 or newer.");
		}
	}
}