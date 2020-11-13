﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ExIni;
using MyRoomCustom;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityInjector;
using UnityInjector.Attributes;
using wf;
using Math = System.Math;

namespace CM3D2.MultipleMaids.Plugin
{
    [PluginVersion("23.1.1")]
    [PluginFilter("COM3D2x64")]
    [PluginFilter("CM3D2x86")]
    [PluginFilter("CM3D2VRx64")]
    [PluginName("Multiple maids")]
    [PluginFilter("CM3D2OHx64")]
    [PluginFilter("CM3D2OHVRx64")]
    [PluginFilter("CM3D2OHx86")]
    [PluginFilter("COM3D2_Trialx64")]
    [PluginFilter("CM3D2x64")]
    public partial class MultipleMaids : PluginBase
	{
		//private const string PluginName = "Multiple maids";
		//private const string PluginVersion = "23.1.1";

		public Dictionary<string, int> bgUiArray=new Dictionary<string, int>();
		public Dictionary<string, int> PartsUIArray = new Dictionary<string, int>();
		public Dictionary<string, int> ItemUIArray = new Dictionary<string, int>();
		public Dictionary<string, int> OdoguUIArray = new Dictionary<string, int>();
		public Dictionary<string, int> DanceArray = new Dictionary<string, int>();
		public Dictionary<string, int> bgUiArrayB = new Dictionary<string, int>();

		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			string dataPath = Application.dataPath;
			this.isVR = GameMain.Instance.VRMode;
			this.Preference();
		}

		public void Start()
		{
			this.isVR = GameMain.Instance.VRMode;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			this.mainCameraTransform = GameMain.Instance.MainCamera.gameObject.transform;
		}

		public void ResetProp(Maid maid, MPN idx)
		{
			MaidProp maidProp = MultipleMaids.GetFieldValue<Maid, MaidProp[]>(maid, "m_aryMaidProp")[(int)idx];
			if (maidProp.nTempFileNameRID != 0 && maidProp.nFileNameRID != maidProp.nTempFileNameRID)
			{
				maidProp.boDut = true;
				maidProp.strTempFileName = string.Empty;
				maidProp.nTempFileNameRID = 0;
				maidProp.boTempDut = false;
			}
		}

		private void createMyRoomObject(string filename)
		{
			int num = int.Parse(filename);
			GameObject gameObject = GameObject.Find("Deployment Object Parent");
			if (gameObject == null)
			{
				gameObject = new GameObject("Deployment Object Parent");
			}
			PlacementData.Data data = PlacementData.GetData(num);
			GameObject prefab = data.GetPrefab();
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
			GameObject gameObject3 = new GameObject(gameObject2.name);
			gameObject2.transform.SetParent(gameObject3.transform, true);
			gameObject3.transform.SetParent(gameObject.transform, false);
			GameObject gameObject4 = gameObject3;
			this.doguBObject.Add(gameObject4);
			gameObject4.name = "MYR_" + filename;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			zero.z = 0.4f;
			gameObject4.transform.localPosition = zero;
			gameObject4.transform.localRotation = Quaternion.Euler(zero2);
			this.doguCnt = this.doguBObject.Count - 1;
			this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
			this.gDogu[this.doguCnt].layer = 8;
			this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
			this.gDogu[this.doguCnt].SetActive(false);
			this.gDogu[this.doguCnt].transform.position = gameObject4.transform.position;
			this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
			this.mDogu[this.doguCnt].isScale = false;
			this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
			this.mDogu[this.doguCnt].maid = gameObject4;
			this.mDogu[this.doguCnt].angles = gameObject4.transform.eulerAngles;
			this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
			this.mDogu[this.doguCnt].ido = 1;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
			this.sceneLevel = scene.buildIndex;
			this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
			if (!this.bgObject)
			{
				this.bgObject = GameObject.Find("__GameMain__/BG");
				this.bg = this.bgObject.transform;
			}
			this.mainCamera = GameMain.Instance.MainCamera;
			this.selectList = new ArrayList();
			this.selectList.Add(0);
			this.bGui = false;
			this.isF7 = false;
			this.isF7SInit = false;
			this.isF6 = false;
			if (this.sceneLevel == 3 || this.sceneLevel == 5)
			{
				this.Preference();
				this.init();
				this.isScript = false;
				if (this.sceneLevel == 5)
				{
					this.isF7SInit = true;
				}
			}
			else if (this.sceneLevel == 15)
			{
				this.init();
				this.isScript = true;
			}
			else
			{
				this.allowUpdate = false;
				if (this.okFlg)
				{
					this.init();
				}
			}
		}
		private bool getModKeyPressing(MultipleMaids.modKey key)
		{
			bool result;
			switch (key)
			{
			case MultipleMaids.modKey.Shift:
				result = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				break;
			case MultipleMaids.modKey.Alt:
				result = (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
				break;
			case MultipleMaids.modKey.Ctrl:
				result = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		private Vector3 getHanten(Vector3 vec)
		{
			return new Vector3(360f - vec.x, 360f - (vec.y + 90f) - 90f, vec.z);
		}

		private Vector3 getHanten2(Vector3 vec)
		{
			return new Vector3(360f - vec.x, 360f - vec.y, vec.z);
		}

		public static string Base64FromStringComp(string st)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(st);
			MemoryStream memoryStream = new MemoryStream();
			DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true);
			deflateStream.Write(bytes, 0, bytes.Length);
			deflateStream.Close();
			byte[] inArray = memoryStream.ToArray();
			return Convert.ToBase64String(inArray, Base64FormattingOptions.InsertLineBreaks);
		}

		public static string StringFromBase64Comp(string st)
		{
			byte[] buffer = Convert.FromBase64String(st);
			MemoryStream stream = new MemoryStream(buffer);
			MemoryStream memoryStream = new MemoryStream();
			DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
			for (;;)
			{
				int num = deflateStream.ReadByte();
				if (num == -1)
				{
					break;
				}
				memoryStream.WriteByte((byte)num);
			}
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}

		private void FoceKuchipakuUpdate2(float f_fNowTime, Maid maid, int i)
		{
			if (!this.isFadeOut)
			{
				if (this.m_baKuchipakuPattern[i] != null)
				{
					int num = (int)(f_fNowTime * 30f);
					int num2 = this.m_baKuchipakuPattern[i].Length / 3;
					if (0 < num && num < num2)
					{
						maid.body0.Face.morph.boLipSync = true;
						maid.body0.Face.morph.LipSync1 = maid.body0.Face.morph.LipSync1 * 0.15f + (float)this.m_baKuchipakuPattern[i][num * 3] / 255f * 0.85f * 3f;
						maid.body0.Face.morph.LipSync2 = maid.body0.Face.morph.LipSync2 * 0.15f + (float)this.m_baKuchipakuPattern[i][num * 3 + 1] / 255f * 0.85f * 3f;
						maid.body0.Face.morph.LipSync3 = maid.body0.Face.morph.LipSync3 * 0.15f + (float)this.m_baKuchipakuPattern[i][num * 3 + 2] / 255f * 0.85f * 3f;
						if (this.sceneLevel == 5)
						{
							if (maid.boMabataki && this.isFace[i])
							{
								maid.boMabataki = false;
								TMorph morph = maid.body0.Face.morph;
								float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
								float[] fieldValue2 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
								morph.EyeMabataki = 0f;
								fieldValue2[(int)morph.hash["eyeclose"]] = 0f;
							}
							maid.body0.Face.morph.FixBlendValues_Face();
						}
					}
					else
					{
						maid.body0.Face.morph.boLipSync = false;
					}
				}
			}
		}
		private int GetPix(int i)
		{
			float num = 1f + ((float)Screen.width / 1280f - 1f) * 0.6f;
			return (int)(num * (float)i);
		}

		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}


		internal static TResult GetFieldValue<T, TResult>(T inst, string name)
		{
			TResult result;
			if (inst == null)
			{
				result = default(TResult);
			}
			else
			{
				FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
				if (fieldInfo == null)
				{
					result = default(TResult);
				}
				else
				{
					result = (TResult)((object)fieldInfo.GetValue(inst));
				}
			}
			return result;
		}

		internal static FieldInfo GetFieldInfo<T>(string name)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			return typeof(T).GetField(name, bindingAttr);
		}

		internal static void SetFieldValue<T, TResult>(T inst, string name, Maid maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue2<T, TResult>(T inst, string name, int maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue3<T, TResult>(T inst, string name, Vector3 maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue4<T, TResult>(T inst, string name, Transform maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue5<T, TResult>(T inst, string name, UILabel maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue6<T, TResult>(T inst, string name, Hashtable maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue7<T, TResult>(T inst, string name, float[] maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}

		internal static void SetFieldValue8<T, TResult>(T inst, string name, DynamicSkirtBone maid)
		{
			FieldInfo fieldInfo = MultipleMaids.GetFieldInfo<T>(name);
			fieldInfo.SetValue(inst, maid);
		}
		private static string[] ProcScriptBin(Maid maid, byte[] cd, string filename, bool f_bTemp)
		{
			TBody body = maid.body0;
			List<MultipleMaids.LastParam> list = new List<MultipleMaids.LastParam>();
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(cd), Encoding.UTF8);
			string text = binaryReader.ReadString();
			NDebug.Assert(text == "CM3D2_MENU", "ProcScriptBin 例外 : ヘッダーファイルが不正です。" + text);
			int num = binaryReader.ReadInt32();
			string path = binaryReader.ReadString();
			string text2 = binaryReader.ReadString();
			string text3 = binaryReader.ReadString();
			string text4 = binaryReader.ReadString();
			long num2 = (long)binaryReader.ReadInt32();
			string text5 = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			string[] array = new string[100];
			int num3 = 0;
			try
			{
				for (;;)
				{
					int num4 = (int)binaryReader.ReadByte();
					text5 = string.Empty;
					if (num4 == 0)
					{
						break;
					}
					for (int i = 0; i < num4; i++)
					{
						text5 = text5 + "\"" + binaryReader.ReadString() + "\" ";
					}
					if (!(text5 == string.Empty))
					{
						string stringCom = UTY.GetStringCom(text5);
						string[] stringList = UTY.GetStringList(text5);
						if (stringCom == "end")
						{
							break;
						}
						if (!(stringCom == "name"))
						{
							if (!(stringCom == "set"))
							{
								if (!(stringCom == "setname"))
								{
									if (stringCom == "マテリアル変更")
									{
										int num5 = int.Parse(stringList[2]);
										if (stringList.Length == 4)
										{
											array[1 + num5] = stringList[3];
										}
										else
										{
											for (int i = 3; i < stringList.Length; i++)
											{
												if (stringList[i].Contains(".mate"))
												{
													array[1 + num3] = stringList[i];
													num3++;
												}
											}
										}
									}
									else if (stringCom == "additem")
									{
										array[0] = stringList[1];
									}
								}
							}
						}
					}
				}
				list.Sort((MultipleMaids.LastParam a, MultipleMaids.LastParam b) => a.nOrder - a.nOrder);
				for (int j = 0; j < list.Count; j++)
				{
					MultipleMaids.LastParam lastParam = list[j];
					if (lastParam.strComm == "アイテムパラメータ")
					{
						TBodySkin slot = body.GetSlot(lastParam.aryArgs[0]);
						slot.SetParam(lastParam.aryArgs[1], lastParam.aryArgs[2]);
					}
				}
			}
			catch (Exception ex)
			{
				NDebug.Assert("メニューファイル処理中にエラーが発生しました。" + Path.GetFileName(path), false);
				throw ex;
			}
			binaryReader.Close();
			binaryReader = null;
			return array;
		}

        public MultipleMaids() : base()
        {
            this.isF2 = false;
            this.isF3 = false;
            this.isSF1 = false;
            this.isSF2 = false;
            this.isSF3 = false;
            this.isHS1 = false;
            this.isHS2 = false;
            this.isHS3 = false;
            this.isHS4 = false;
            this.isHS5 = false;
            this.isHS6 = false;
            this.isHS7 = false;
            this.isHS8 = false;
            this.isHS9 = false;
            this.isSS = false;
            this.isSS1 = false;
            this.isSS2 = false;
            this.isSS3 = false;
            this.isSS4 = false;
            this.isSS5 = false;
            this.isSS6 = false;
            this.nameK = "";
            this.nameA = "";
            this.nameS = "";
            this.danceNo1 = 1;
            this.danceNo2 = 2;
            this.danceNo3 = 3;
            this.isSN1 = false;
            this.isSN2 = false;
            this.isSN3 = false;
            this.isSN4 = false;
            this.isSN5 = false;
            this.isSN6 = false;
            this.isShift = false;
            this.isCF1 = false;
            this.isKHG1 = false;
            this.isKHG2 = false;
            this.isKT1 = false;
            this.isSD1 = false;
            this.isScene = false;
            this.isPanel = true;
            this.sPoseCount = 0;
            this.existPose = false;
            this.poseIniStr = "";
            this.poseCount = new int[100];
            this.delCount = new int[100];
            this.delCount2 = new int[100];
            this.maxMaidCnt = 100;
            this.maidCnt = 0;
            this.cameraIti = new Vector3(0f, 0.9f, 0f);
            this.cameraItiDistance = 3f;
            this.armL = new Vector3[100];
            this.isInit = false;
            this.myIndex = 0;
            this.kankyoIndex = 0;
            this.itemIndexB = 0;
            this.poseIndex = new int[100];
            this.itemIndex = new int[100];
            this.itemIndex2 = new int[100];
            this.faceIndex = new int[100];
            this.faceBlendIndex = new int[100];
            this.headEyeIndex = new int[100];
            this.copyIndex = 0;
            this.isCopy = false;
            this.okFlg = false;
            this.isDanceStart1 = false;
            this.isDanceStart1F = false;
            this.isDanceStart1K = false;
            this.isDanceStart2 = false;
            this.isDanceStart2F = false;
            this.isDanceStart3 = false;
            this.isDanceStart3F = false;
            this.isDanceStart3K = false;
            this.isDanceStart4 = false;
            this.isDanceStart4F = false;
            this.isDanceStart4K = false;
            this.isDanceStart5 = false;
            this.isDanceStart5F = false;
            this.isDanceStart5K = false;
            this.isDanceStart6 = false;
            this.isDanceStart6F = false;
            this.isDanceStart6K = false;
            this.isDanceStart7 = false;
            this.isDanceStart7F = false;
            this.isDanceStart7V = false;
            this.isDanceStart8 = false;
            this.isDanceStart8F = false;
            this.isDanceStart8V = false;
            this.isDanceStart8P = false;
            this.isDanceStart9 = false;
            this.isDanceStart9F = false;
            this.isDanceStart9K = false;
            this.isDanceStart9Count = 0;
            this.isDanceStart10 = false;
            this.isDanceStart10F = false;
            this.isDanceStart11 = false;
            this.isDanceStart11F = false;
            this.isDanceStart11V = false;
            this.isDanceStart12 = false;
            this.isDanceStart12F = false;
            this.isDanceStart13 = false;
            this.isDanceStart13F = false;
            this.isDanceStart13K = false;
            this.isDanceStart13Count = 0;
            this.isDanceStart14 = false;
            this.isDanceStart14F = false;
            this.isDanceStart14V = false;
            this.isDanceStart15 = false;
            this.isDanceStart15F = false;
            this.isDanceStart15V = false;
            this.isDanceStart15Count = 0;
            this.danceFace = new float[100];
            this.isF6 = false;
            this.isF6S = false;
            this.dancePos = new Vector3[100];
            this.danceRot = new Quaternion[100];
            this.maxPage = 20;
            this.danceCheck = new float[10];
            this.danceCheckIndex = 0;
            this.danceWait = 0;
            this.danceCount = 0;
            this.bgmIndex = 0;
            this.effectIndex = 0;
            this.cubeSize = 0.12f;
            this.doguIndex = new List<int>();
            this.doguSelectIndex = 0;
            this.doguObject = new List<GameObject>();
            this.doguBObject = new List<GameObject>();
            this.allowUpdate = false;
            this.moveBg = false;
            this.bgIndex = 0;
            this.bgIndexB = 0;
            this.slotIndex = 0;
            this.wearIndex = 0;
            this.bgIndex6 = 0;
            this.FaceName = new string[100];
            this.FaceName2 = new string[100];
            this.FaceName3 = new string[100];
            this.FaceTime = new float[100];
            this.keyFlg = false;
            this.xFlg = new bool[100];
            this.zFlg = new bool[100];
            this.cafeFlg = new bool[100];
            this.cafeCount = new int[100];
            this.isBone = new bool[100];
            this.isBoneN = new bool[100];
            this.isChange = new bool[100];
            this.ui_cam_hide_list_ = new List<UICamera>();
            this.isScreen = false;
            this.isScreen2 = false;
            this.isGui = false;
            this.goSlot = new List<TBodySkin>[500];
            this.bodyHit = new List<TBodyHit>[500];
            this.eyeL = new Vector3[500];
            this.eyeR = new Vector3[500];
            this.shodaiFlg = new bool[500];
            this.comboBoxControl = new ComboBox2();
            this.listStyle = new GUIStyle();
            this.listStyle2 = new GUIStyle();
            this.listStyle3 = new GUIStyle();
            this.listStyle4 = new GUIStyle();
            this.faceCombo = new ComboBox2();
            this.faceInitFlg = false;
            this.isPose = new bool[100];
            this.poseCombo = new ComboBox2();
            this.poseInitFlg = false;
            this.kankyoInitFlg = false;
            this.kankyo2InitFlg = false;
            this.isPoseInit = false;
            this.poseGroupIndex = 0;
            this.isPoseIti = new bool[100];
            this.poseIti = new Vector3[100];
            this.poseGroupCombo = new ComboBox2();
            this.itemCombo = new ComboBox2();
            this.bgmCombo = new ComboBox2();
            this.itemCombo2 = new ComboBox2();
            this.myCombo = new ComboBox2();
            this.bgCombo2 = new ComboBox2();
            this.bgCombo = new ComboBox2();
            this.slotCombo = new ComboBox2();
            this.doguCombo2 = new ComboBox2();
            this.doguCombo = new ComboBox2[9];
            this.doguComboList = new List<GUIContent[]>();
            this.parCombo = new ComboBox2();
            this.parCombo1 = new ComboBox2();
            this.lightCombo = new ComboBox2();
            this.kankyoCombo = new ComboBox2();
            this.isStop = new bool[100];
            this.maidArray = new Maid[100];
            this.danceName = new string[100];
            this.keyArray = new KeyCode[]
            {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Alpha5,
                KeyCode.Alpha6,
                KeyCode.Alpha7
            };
            this.idoFlg = new bool[100];
            this.isIK = new bool[100];
            this.isLock = new bool[100];
            this.isIKAll = false;
            this.faceFlg = false;
            this.faceFlg2 = false;
            this.poseFlg = false;
            this.unLockFlg = false;
            this.sceneFlg = false;
            this.kankyoFlg = false;
            this.kankyo2Flg = false;
            this.modFlg = true;
            this.nmodFlg = false;
            this.doguSelectFlg1 = true;
            this.doguSelectFlg2 = false;
            this.doguSelectFlg3 = false;
            this.isDanceStop = false;
            this.isFace = new bool[100];
            this.isMabataki = new bool[100];
            this.mekure1 = new bool[100];
            this.mekure2 = new bool[100];
            this.zurasi = new bool[100];
            this.mekure1n = new bool[100];
            this.mekure2n = new bool[100];
            this.zurasin = new bool[100];
            this.hanten = new bool[100];
            this.hantenn = new bool[100];
            this.isHanten = false;
            this.kotei = new bool[100];
            this.voice1 = new bool[100];
            this.voice1n = new bool[100];
            this.voice2 = new bool[100];
            this.voice2n = new bool[100];
            this.pHandL = new int[100];
            this.pHandR = new int[100];
            this.isLook = new bool[100];
            this.lookX = new float[100];
            this.lookY = new float[100];
            this.isLookn = new bool[100];
            this.lookXn = new float[100];
            this.lookYn = new float[100];
            this.isPoseEdit = false;
            this.isFaceEdit = false;
            this.isNamida = false;
            this.isTear1 = false;
            this.isTear2 = false;
            this.isTear3 = false;
            this.isShock = false;
            this.isYodare = false;
            this.isHoho = false;
            this.isHoho2 = false;
            this.isHohos = false;
            this.isHohol = false;
            this.isFaceInit = false;
            this.isNamidaH = false;
            this.isSekimenH = false;
            this.isHohoH = false;
            this.isHenkou = false;
            this.isWear = false;
            this.isSkirt = false;
            this.isBra = false;
            this.isPanz = false;
            this.isMaid = false;
            this.isMekure1 = false;
            this.isMekure2 = false;
            this.isZurasi = false;
            this.isMekure1a = false;
            this.isMekure2a = false;
            this.isZurasia = false;
            this.isHeadset = false;
            this.isAccUde = false;
            this.isStkg = false;
            this.isShoes = false;
            this.isGlove = false;
            this.isMegane = false;
            this.isAccSenaka = false;
            this.isBloom = false;
            this.isBloom2 = false;
            this.isBloomA = false;
            this.isDepth = false;
            this.isDepthA = false;
            this.isFog = false;
            this.isSepia = false;
            this.isSepian = false;
            this.isBlur = false;
            this.isBlur2 = false;
            this.isCube = false;
            this.isCube2 = false;
            this.isCube3 = false;
            this.isCube4 = false;
            this.isKamiyure = false;
            this.isSkirtyure = false;
            this.isHairSetting = false;
            this.isSkirtSetting = false;
            this.isVRScroll = true;
            this.isCubeS = false;
            this.isBloomS = false;
            this.isDepthS = false;
            this.isBlurS = false;
            this.isFogS = false;
            this.isToothoff = false;
            this.isNosefook = false;
            this.selectMaidIndex = 0;
            this.isCombo = false;
            this.isCombo2 = false;
            this.isCombo3 = false;
            this.hFlg = false;
            this.h2Flg = false;
            this.mFlg = false;
            this.fFlg = false;
            this.qFlg = false;
            this.sFlg = false;
            this.atFlg = false;
            this.escFlg = false;
            this.yFlg = false;
            this.isVP = false;
            this.isPP = false;
            this.isPP2 = false;
            this.isPP3 = false;
            this.isVA = false;
            this.isKA = false;
            this.isKA2 = false;
            this.isDance = false;
            this.lightIndex = new List<int>();
            this.lightColorR = new List<float>();
            this.lightColorG = new List<float>();
            this.lightColorB = new List<float>();
            this.lightX = new List<float>();
            this.lightY = new List<float>();
            this.lightAkarusa = new List<float>();
            this.lightKage = new List<float>();
            this.lightRange = new List<float>();
            this.lightList = new List<GameObject>();
            this.doguList = new List<int>();
            this.parList = new List<int>();
            this.doguCnt = 0;
            this.selectLightIndex = 0;
            this.doguB2Index = 0;
            this.doguBIndex = new int[9];
            this.parIndex = 0;
            this.parIndex1 = 0;
            this.isVR = false;
            this.isVR2 = true;
            this.isYobidashi = false;
            this.isFadeOut = false;
            this.isBusyInit = false;
            this.isHaiti = false;
            this.isF7 = false;
            this.isF7S = false;
            this.isF7SInit = false;
            this.isGuiInit = false;
            this.speed = 1f;
            this.saveScene = 0;
            this.saveScene2 = 0;
            this.loadScene = 0;
            this.loadPose = new string[100];
            this.isLoadPose = new bool[100];
            this.loadCount = new int[100];
            this.kankyoLoadFlg = false;
            this.nameFlg = false;
            this.kankyoMax = 0;
            this.date = new string[10];
            this.ninzu = new string[10];
            this.ikMaid = 0;
            this.ikBui = 0;
            this.ikMode = new int[100];
            this.ikModeOld = new int[100];
            this.ikMode2 = 0;
            this.ikModeOld2 = 0;
            this.editMaid = 0;
            this.isScript = false;
            this.mDogu = new MouseDrag6[999];
            this.gDogu = new GameObject[999];
            this.mBg = new MouseDrag6();
            this.gBg = new GameObject();
            this.mLight = new MouseDrag6[999];
            this.gLight = new GameObject[999];
            this.ikLeftArm = new IK();
            this.mHead2 = new MouseDrag5[100];
            this.mMaid2 = new MouseDrag5[100];
            this.mMaid = new MouseDrag2[100];
            this.gMaid = new GameObject[100];
            this.mMaidC = new MouseDrag2[100];
            this.gMaidC = new GameObject[100];
            this.mHead = new MouseDrag3[100];
            this.gHead = new GameObject[100];
            this.gMaid2 = new GameObject[100];
            this.gHead2 = new GameObject[100];
            this.mJotai = new MouseDrag3[100];
            this.gJotai = new GameObject[100];
            this.mKahuku = new MouseDrag3[100];
            this.gKahuku = new GameObject[100];
            this.gIKMuneL = new GameObject[100];
            this.mIKMuneL = new MouseDrag[100];
            this.gIKMuneR = new GameObject[100];
            this.mIKMuneR = new MouseDrag[100];
            this.vIKMuneL = new Vector3[100];
            this.vIKMuneR = new Vector3[100];
            this.vIKMuneLSub = new Vector3[100];
            this.vIKMuneRSub = new Vector3[100];
            this.haraCount = new int[100];
            this.haraPosition = new Vector3[100];
            this.muneIKL = new bool[100];
            this.muneIKR = new bool[100];
            this.gIKHandL = new GameObject[100];
            this.mIKHandL = new MouseDrag4[100];
            this.gIKHandR = new GameObject[100];
            this.mIKHandR = new MouseDrag4[100];
            this.gHandL = new GameObject[100];
            this.mHandL = new MouseDrag[100];
            this.gArmL = new GameObject[100];
            this.mArmL = new MouseDrag[100];
            this.gFootL = new GameObject[100];
            this.mFootL = new MouseDrag[100];
            this.gHizaL = new GameObject[100];
            this.mHizaL = new MouseDrag[100];
            this.gHandR = new GameObject[100];
            this.mHandR = new MouseDrag[100];
            this.gArmR = new GameObject[100];
            this.mArmR = new MouseDrag[100];
            this.gFootR = new GameObject[100];
            this.mFootR = new MouseDrag[100];
            this.gHizaR = new GameObject[100];
            this.mHizaR = new MouseDrag[100];
            this.gClavicleL = new GameObject[100];
            this.mClavicleL = new MouseDrag[100];
            this.gClavicleR = new GameObject[100];
            this.mClavicleR = new MouseDrag[100];
            this.gFinger = new GameObject[100, 30];
            this.mFinger = new MouseDrag[100, 30];
            this.gFinger2 = new GameObject[100, 12];
            this.mFinger2 = new MouseDrag[100, 12];
            this.gNeck = new GameObject[100];
            this.mNeck = new MouseDrag3[100];
            this.gSpine = new GameObject[100];
            this.mSpine = new MouseDrag3[100];
            this.gSpine0a = new GameObject[100];
            this.mSpine0a = new MouseDrag3[100];
            this.gSpine1 = new GameObject[100];
            this.mSpine1 = new MouseDrag3[100];
            this.gSpine1a = new GameObject[100];
            this.mSpine1a = new MouseDrag3[100];
            this.gPelvis = new GameObject[100];
            this.mPelvis = new MouseDrag3[100];
            this.HandL = null;
            this.UpperArmL = null;
            this.ForearmL = null;
            this.Head = null;
            this.Spine = null;
            this.Spine0a = null;
            this.Spine1 = null;
            this.Spine1a = null;
            this.Pelvis = null;
            this.Clavicle = null;
            this.IK_hand = null;
            this.IKHandL = new Transform[100];
            this.IKHandR = new Transform[100];
            this.IKMuneL = new Transform[100];
            this.IKMuneR = new Transform[100];
            this.IKMuneLSub = new Transform[100];
            this.IKMuneRSub = new Transform[100];
            this.Neck = new Transform[100];
            this.Pelvis2 = new Transform[100];
            this.Spine12 = new Transform[100];
            this.Spine0a2 = new Transform[100];
            this.Spine2 = new Transform[100];
            this.Spine1a2 = new Transform[100];
            this.Head1 = new Transform[100];
            this.Head2 = new Transform[100];
            this.Head3 = new Transform[100];
            this.HandL1 = new Transform[100];
            this.UpperArmL1 = new Transform[100];
            this.ForearmL1 = new Transform[100];
            this.HandR1 = new Transform[100];
            this.UpperArmR1 = new Transform[100];
            this.ForearmR1 = new Transform[100];
            this.HandL2 = new Transform[100];
            this.UpperArmL2 = new Transform[100];
            this.ForearmL2 = new Transform[100];
            this.HandR2 = new Transform[100];
            this.UpperArmR2 = new Transform[100];
            this.ForearmR2 = new Transform[100];
            this.ClavicleL1 = new Transform[100];
            this.ClavicleR1 = new Transform[100];
            this.Finger = new Transform[100, 40];
            this.Finger2 = new Transform[100, 18];
            this.isSavePose = false;
            this.isSavePose2 = false;
            this.isSavePose3 = false;
            this.isSavePose4 = false;
            this.yotogiFlg = false;
            this.isShosai = false;
            this.isEdit = new bool[100];
            this.isEditNo = 0;
            this.isLoadFace = new bool[100];
            this.isDanceChu = false;
            this.isMessage = false;
            this.isPref = false;
            this.thum_byte_to_base64_ = string.Empty;
            this.depth_field_ = GameMain.Instance.MainCamera.GetComponent<DepthOfFieldScatter>();
            this.sepia_tone_ = GameMain.Instance.MainCamera.GetComponent<SepiaToneEffect>();
            this.SkirtListArray = new DynamicSkirtBone[500][];
            this.JumpChkTArray = new Transform[100];
            this.JumpChkPosArray = new Vector3[100];
            this.gt = new GameObject[70];
            this.wv = new Vector3[70];
            this.bGui = false;
            this.bGuiMessage = false;
            this.screenSize = new Vector2(0f, 0f);
            this.scrollPos = new Vector2(0f, 0f);
            this.inText = "";
            this.inName = "";
            this.inName2 = "";
            this.inName3 = "";
            this.inName4 = "";
            this.fontSize = 25;
            this.mFontSize = 25;
            this.line1 = null;
            this.line2 = null;
            this.page = 0;
            this.dispNo = 0;
            this.dispNoOld = 0;
            this.texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            this.waku = null;
            this.waku2 = null;
            this.sortList = new List<MultipleMaids.SortItem>();
            this.sortListMy = new List<MultipleMaids.SortItemMy>();
            this.itemDataList = new List<MultipleMaids.ItemData>();
            this.itemDataListMod = new List<MultipleMaids.ItemData>();
            this.itemDataListNMod = new List<MultipleMaids.ItemData>();
            this.isIdx1 = false;
            this.isIdx2 = false;
            this.isIdx3 = false;
            this.isIdx4 = false;
            this.gizmoHandL = new GizmoRender[100];
            this.gizmoHandR = new GizmoRender[100];
            this.gizmoFootL = new GizmoRender[100];
            this.gizmoFootR = new GizmoRender[100];
        }

        private bool isCM3D2 = false;

		private byte[][] m_baKuchipakuPattern = new byte[100][];

		private string saveData;

		private bool isDialog = false;

		private List<string> bgNameList = new List<string>();
		private List<string> doguNameList = new List<string>();
		private List<string> itemNameList = new List<string>();
		private List<string> myNameList = new List<string>();

		private string[] bgArray;

		private string[] bgArray21 = new string[]
		{
			"Salon",
			"Syosai",
			"Syosai_Night",
			"DressRoom_NoMirror",
			"MyBedRoom",
			"MyBedRoom_Night",
			"Bathroom",
			"PlayRoom",
			"Pool",
			"SMRoom",
			"PlayRoom2",
			"Salon_Garden",
			"LargeBathRoom",
			"MaidRoom",
			"OiranRoom",
			"Penthouse",
			"Town",
			"Kitchen",
			"Kitchen_Night",
			"Shitsumu",
			"Shitsumu_Night",
			"Salon_Entrance",
			"Bar"
		};

		private string[] bgArray2 = new string[]
		{
			"Theater",
			"Theater_LightOff",
			"LiveStage",
			"LiveStage_Side",
			"LiveStage_use_dance",
			"BackStage",
			"DanceRoom",
			"EmpireClub_Rotary",
			"EmpireClub_Rotary_Night",
			"EmpireClub_Entrance",
			"ShinShitsumu",
			"ShinShitsumu_ChairRot",
			"ShinShitsumu_Night",
			"MyRoom",
			"MyRoom_Night",
			"OpemCafe",
			"OpemCafe_Night",
			"Restaurant",
			"Restaurant_Night",
			"MainKitchen",
			"MainKitchen_Night",
			"MainKitchen_LightOff",
			"BarLounge",
			"Casino",
			"CasinoMini",
			"SMClub",
			"Soap",
			"Spa",
			"Spa_Night",
			"ShoppingMall",
			"ShoppingMall_Night",
			"GameShop",
			"MusicShop",
			"HeroineRoom_A1",
			"HeroineRoom_A1_Night",
			"HeroineRoom_B1",
			"HeroineRoom_B1_Night",
			"HeroineRoom_C1",
			"HeroineRoom_C1_Night",
			"HeroineRoom_A",
			"HeroineRoom_A_Night",
			"HeroineRoom_B",
			"HeroineRoom_B_Night",
			"HeroineRoom_C",
			"HeroineRoom_C_Night",
			"Shukuhakubeya_BedRoom",
			"Shukuhakubeya_BedRoom_Night",
			"Shukuhakubeya_Other_BedRoom",
			"Shukuhakubeya_Living",
			"Shukuhakubeya_Living_Night",
			"Shukuhakubeya_Toilet",
			"Shukuhakubeya_Toilet_Night",
			"Shukuhakubeya_WashRoom",
			"Shukuhakubeya_WashRoom_Night"
		};

		private string[] bgArrayB = new string[]
		{
			"Salon",
			"Salon_Day",
			"Syosai",
			"Syosai_Night",
			"DressRoom_NoMirror",
			"MyBedRoom",
			"MyBedRoom_Night",
			"Bathroom",
			"PlayRoom",
			"Pool",
			"SMRoom",
			"PlayRoom2",
			"Salon_Garden",
			"LargeBathRoom",
			"MaidRoom",
			"OiranRoom",
			"Penthouse",
			"Town",
			"Kitchen",
			"Kitchen_Night",
			"Shitsumu",
			"Shitsumu_Night",
			"Salon_Entrance",
			"Bar"
		};

		private string[] poseGroupArray;

		private string[] poseGroupArray2 = new string[]
		{
			"maid_dressroom01",
			"tennis_kamae_f",
			"senakanagasi_f",
			"work_hansei",
			"inu_taiki_f",
			"syagami_pose_f",
			"densyasuwari_taiki_f",
			"work_kaiwa",
			"dance_cm3d2_001_f1,14.14",
			"dance_cm3d_001_f1,39.25"
		};

		private string[] poseGroupArrayVP = new string[]
		{
			"dance_cm3d_002_end_f1,50.71"
		};

		private string[] poseGroupArrayPP = new string[]
		{
			"dance_cm3d2_002_smt_f,7.76,"
		};

		private string[] poseGroupArrayFB = new string[]
		{
			"dance_cm3d_003_sp2_f1,90.15"
		};

		private string[] poseGroupArrayPP2 = new string[]
		{
			"dance_cm3d2_003_hs_f1,0.01,"
		};

		private string[] poseGroupArrayPP3 = new string[]
		{
			"dance_cm3d2_004_sse_f1,0.01"
		};

		private string[] poseGroupArrayKT = new string[]
		{
			"dance_cm3d_004_kano_f1,124.93"
		};

		private string[] poseGroupArray3 = new string[]
		{
			"turusi_sex_in_taiki_f",
			"rosyutu_pose01_f",
			"rosyutu_aruki_f_once_,1.37",
			"stand_desk1"
		};

		private string[] slotArray = new string[]
		{
			"",
			"acchat",
			"headset",
			"wear",
			"skirt",
			"onepiece",
			"mizugi",
			"bra",
			"panz",
			"stkg",
			"shoes",
			"acckami",
			"megane",
			"acchead",
			"acchana",
			"accmimi",
			"glove",
			"acckubi",
			"acckubiwa",
			"acckamisub",
			"accnip",
			"accude",
			"accheso",
			"accashi",
			"accsenaka",
			"accshippo",
			"accxxx"
		};

		private string[] dance1Array = new string[]
		{
			"0.0289354,ダンス目つむり,頬０涙０",
			"2.7131975,ダンス目あけ,頬０涙０",
			"3.4266715,ダンス目とじ,頬０涙０",
			"4.8365424,ダンス微笑み,頬０涙０",
			"8.0770104,ダンス目とじ,頬０涙０",
			"9.8241599,ダンス目あけ,頬０涙０",
			"10.6756193,ダンスびっくり,頬０涙０",
			"11.898481,ダンス目あけ,頬０涙０",
			"13.031505,ダンス目つむり,頬０涙０",
			"13.3170109,ダンス目あけ,頬０涙０",
			"13.8139987,ダンスウインク,頬１涙０",
			"14.6888243,ダンス目つむり,頬０涙０",
			"16.1757774,ダンスびっくり,頬０涙０",
			"19.1140857,ダンス目とじ,頬１涙０",
			"20.2937843,ダンス目あけ,頬１涙０",
			"21.4119475,ダンス目つむり,頬０涙０",
			"21.9122835,ダンスびっくり,頬０涙０",
			"22.4103193,ダンス目あけ,頬０涙０",
			"24.5163241,ダンスびっくり,頬０涙０",
			"25.7280281,ダンス目あけ,頬０涙０",
			"27.6203067,ダンス真剣,頬２涙０",
			"28.6120243,ダンス目あけ,頬０涙０",
			"29.6222143,ダンス困り顔,頬０涙０",
			"32.0231584,ダンス目つむり,頬０涙０",
			"33.5374119,ダンス目つむり,頬１涙０",
			"34.0225554,ダンス微笑み,頬１涙０",
			"36.7351955,ダンス微笑み,頬０涙０",
			"47.1645593,ダンス誘惑,頬０涙０",
			"53.0552533,ダンス困り顔,頬２涙０",
			"54.2167403,ダンス困り顔,頬０涙０",
			"57.4169907,ダンス目とじ,頬０涙０",
			"58.5372023,ダンス目あけ,頬０涙０",
			"58.9543957,ダンス目あけ,頬１涙０",
			"59.5958731,ダンス微笑み,頬１涙０",
			"60.9297767,ダンスウインク,頬２涙０",
			"62.0667448,ダンス微笑み,頬２涙０",
			"63.8315349,ダンス目とじ,頬２涙０",
			"64.3977156,ダンス微笑み,頬２涙０",
			"64.5789199,ダンス微笑み,頬０涙０",
			"67.8576558,ダンス目とじ,頬１涙０",
			"68.6515446,ダンス目あけ,頬１涙０",
			"70.9590673,ダンスウインク,頬１涙０",
			"71.94191,ダンスびっくり,頬０涙０",
			"73.6387758,ダンス微笑み,頬０涙０",
			"75.5202289,ダンス目あけ,頬０涙０",
			"77.384723,ダンスウインク,頬０涙０",
			"78.778427,ダンス目つむり,頬０涙０",
			"80.3407592,ダンス微笑み,頬０涙０",
			"82.5136545,ダンス目あけ,頬０涙０",
			"82.9853436,ダンス目つむり,頬０涙０",
			"83.7667316,ダンス微笑み,頬０涙０",
			"93.1531707,ダンス目あけ,頬０涙０",
			"96.5580089,ダンス目とじ,頬０涙０",
			"97.2923266,ダンス目あけ,頬０涙０",
			"99.1799196,ダンス微笑み,頬０涙０",
			"101.6659886,ダンス憂い,頬０涙０",
			"103.8095249,ダンス目とじ,頬０涙０",
			"105.0657792,ダンス目あけ,頬０涙０",
			"106.6194406,ダンス目つむり,頬０涙０",
			"107.3798552,ダンス微笑み,頬０涙０",
			"109.3765481,ダンスウインク,頬０涙０",
			"112.23412,ダンス微笑み,頬０涙０",
			"115.6781273,ダンス目とじ,頬１涙０",
			"116.4845218,ダンス目あけ,頬１涙０",
			"118.7426669,ダンスウインク,頬１涙０",
			"119.7673962,ダンスびっくり,頬０涙０",
			"121.4678708,ダンス微笑み,頬０涙０",
			"123.3440586,ダンス目あけ,頬０涙０",
			"125.2192785,ダンスウインク,頬０涙０",
			"126.6157654,ダンス微笑み,頬０涙０",
			"130.3629237,ダンスウインク,頬０涙０",
			"131.1754233,ダンス微笑み,頬０涙０",
			"132.0665986,ダンス目とじ,頬０涙０",
			"133.103525,ダンス目あけ,頬０涙０",
			"133.8736653,ダンス微笑み,頬０涙０",
			"137.6152562,ダンスウインク,頬０涙０",
			"138.4366167,ダンス微笑み,頬０涙０",
			"139.3119606,ダンス目とじ,頬０涙０",
			"139.9764969,ダンス目とじ,頬１涙０",
			"140.357421,ダンス目あけ,頬１涙０",
			"141.7334427,ダンス目つむり,頬１涙０",
			"142.8441298,ダンス微笑み,頬１涙０",
			"144.2874449,ダンス目とじ,頬１涙０",
			"146.4583225,ダンス微笑み,頬１涙０"
		};

		private string[] dance1BArray = new string[]
		{
			"0.0291176,ダンス目つむり,頬０涙０",
			"2.7132095,ダンス目あけ,頬０涙０",
			"3.4266772,ダンス目とじ,頬０涙０",
			"4.8365487,ダンス微笑み,頬０涙０",
			"8.0770184,ダンス目とじ,頬０涙０",
			"9.8241659,ダンス目あけ,頬０涙０",
			"10.6756279,ダンスびっくり,頬０涙０",
			"11.8984884,ダンス目あけ,頬０涙０",
			"13.0315138,ダンス目つむり,頬０涙０",
			"13.3170174,ダンス目あけ,頬０涙０",
			"13.8140084,ダンスウインク,頬１涙０",
			"14.6888306,ダンス目つむり,頬０涙０",
			"16.1757834,ダンスびっくり,頬０涙０",
			"19.1140923,ダンス目とじ,頬１涙０",
			"20.2937906,ダンス目あけ,頬１涙０",
			"21.4119552,ダンス目つむり,頬０涙０",
			"21.9122904,ダンスびっくり,頬０涙０",
			"22.410327,ダンス目あけ,頬０涙０",
			"29.0755758,ダンスびっくり,頬２涙０",
			"29.6222203,ダンス困り顔,頬２涙０",
			"30.1532741,ダンス真剣,頬２涙０",
			"30.7121931,ダンス困り顔,頬０涙０",
			"34.0225617,ダンス憂い,頬０涙０",
			"37.9300073,ダンス憂い,頬１涙０",
			"40.3020859,ダンス憂い,頬０涙０",
			"47.1645672,ダンス誘惑,頬０涙０",
			"49.1242247,ダンス誘惑,頬１涙０",
			"58.5372089,ダンス目あけ,頬０涙０",
			"58.9544059,ダンス目あけ,頬１涙０",
			"59.5958816,ダンス微笑み,頬１涙０",
			"60.9297827,ダンスウインク,頬２涙０",
			"62.0667528,ダンス微笑み,頬２涙０",
			"63.8315406,ダンス目とじ,頬２涙０",
			"64.3977239,ダンス微笑み,頬２涙０",
			"64.5789261,ダンス微笑み,頬０涙０",
			"67.8576657,ダンス目とじ,頬１涙０",
			"68.6515511,ダンス目あけ,頬１涙０",
			"70.9590864,ダンスウインク,頬１涙０",
			"71.941918,ダンスびっくり,頬０涙０",
			"73.6387846,ダンスびっくり,頬１涙０",
			"75.5202391,ダンスびっくり,頬０涙０",
			"77.3847341,ダンスウインク,頬０涙０",
			"78.7784378,ダンス目つむり,頬０涙０",
			"80.3407714,ダンス微笑み,頬０涙０",
			"82.5136664,ダンス目あけ,頬０涙０",
			"82.9853556,ダンス目つむり,頬０涙０",
			"83.766743,ダンス微笑み,頬０涙０",
			"93.1531789,ダンス目あけ,頬０涙０",
			"96.5580175,ダンス目とじ,頬０涙０",
			"97.2923357,ダンス目あけ,頬０涙０",
			"99.179929,ダンス微笑み,頬０涙０",
			"101.6659969,ダンス憂い,頬０涙０",
			"103.8095351,ダンス目とじ,頬０涙０",
			"105.0657883,ダンス目あけ,頬０涙０",
			"106.6194597,ダンス目つむり,頬０涙０",
			"107.3798635,ダンス微笑み,頬０涙０",
			"109.3765563,ダンスウインク,頬０涙０",
			"112.2341368,ダンス微笑み,頬０涙０",
			"115.6781359,ダンス目とじ,頬１涙０",
			"116.4845303,ダンス目あけ,頬１涙０",
			"118.7426754,ダンスウインク,頬１涙０",
			"119.7674047,ダンスびっくり,頬０涙０",
			"121.4678785,ダンスびっくり,頬２涙０",
			"123.3440672,ダンスびっくり,頬０涙０",
			"125.2192862,ダンスウインク,頬０涙０",
			"126.6157742,ダンス微笑み,頬０涙０",
			"130.3629314,ダンスウインク,頬０涙０",
			"131.1754321,ダンス微笑み,頬０涙０",
			"132.0666068,ダンス目とじ,頬０涙０",
			"133.103533,ダンス目あけ,頬０涙０",
			"133.8736738,ダンス微笑み,頬０涙０",
			"137.6152656,ダンスウインク,頬０涙０",
			"138.4366244,ダンス微笑み,頬０涙０",
			"139.3119683,ダンス目とじ,頬０涙０",
			"140.3574301,ダンス目あけ,頬０涙０",
			"141.7334519,ダンス目つむり,頬１涙０",
			"142.8441383,ダンス微笑み,頬１涙０",
			"144.2874537,ダンス目とじ,頬１涙０",
			"146.4583325,ダンス微笑み,頬１涙０"
		};

		private string[] dance1CArray = new string[]
		{
			"0.0291324,ダンス目つむり,頬０涙０",
			"2.7132118,ダンス目あけ,頬０涙０",
			"3.4266787,ダンス目とじ,頬０涙０",
			"4.8365501,ダンス微笑み,頬０涙０",
			"8.0770207,ダンス目とじ,頬０涙０",
			"9.8241673,ダンス目あけ,頬０涙０",
			"10.6756296,ダンスびっくり,頬０涙０",
			"11.8984904,ダンス目あけ,頬０涙０",
			"13.0315167,ダンス目つむり,頬０涙０",
			"13.3170291,ダンス目あけ,頬０涙０",
			"13.8140223,ダンスウインク,頬１涙０",
			"14.688832,ダンス目つむり,頬０涙０",
			"16.1757868,ダンスびっくり,頬０涙０",
			"19.1140971,ダンス目とじ,頬１涙０",
			"20.2937923,ダンス目あけ,頬１涙０",
			"21.4119569,ダンス目あけ,頬０涙０",
			"24.5163327,ダンスびっくり,頬０涙０",
			"25.7280353,ダンス目あけ,頬０涙０",
			"30.1532801,ダンス真剣,頬０涙０",
			"32.0231692,ダンス目つむり,頬１涙０",
			"33.3001626,ダンス微笑み,頬１涙０",
			"34.0225654,ダンス目あけ,頬０涙０",
			"43.3848573,ダンス困り顔,頬１涙０",
			"47.1645701,ダンス誘惑,頬０涙０",
			"53.0553035,ダンス誘惑,頬１涙０",
			"57.4170024,ダンス誘惑,頬０涙０",
			"58.5372151,ダンス目あけ,頬０涙０",
			"58.9544076,ダンス目あけ,頬１涙０",
			"59.5958833,ダンス微笑み,頬１涙０",
			"60.9297864,ダンスウインク,頬２涙０",
			"62.0667545,ダンス微笑み,頬２涙０",
			"63.8315449,ダンス目とじ,頬２涙０",
			"64.3977259,ダンス微笑み,頬２涙０",
			"64.5789316,ダンス微笑み,頬０涙０",
			"67.8576675,ダンス目とじ,頬１涙０",
			"68.65156,ダンス目あけ,頬１涙０",
			"70.9590884,ダンスウインク,頬１涙０",
			"71.941924,ダンスびっくり,頬０涙０",
			"73.6387881,ダンス微笑み,頬０涙０",
			"75.5202437,ダンス目あけ,頬０涙０",
			"77.384737,ダンスウインク,頬０涙０",
			"78.778441,ダンス目つむり,頬０涙０",
			"80.3407743,ダンス微笑み,頬０涙０",
			"82.5136693,ダンス目あけ,頬０涙０",
			"82.9853587,ダンス目つむり,頬０涙０",
			"83.7667462,ダンス微笑み,頬０涙０",
			"93.1531812,ダンス目あけ,頬０涙０",
			"96.55802,ダンス目とじ,頬０涙０",
			"97.2923386,ダンス目あけ,頬０涙０",
			"99.1799313,ダンス微笑み,頬０涙０",
			"101.6659994,ダンス憂い,頬０涙０",
			"103.8095482,ダンス目とじ,頬０涙０",
			"105.0657906,ダンス目あけ,頬０涙０",
			"106.6194617,ダンス目つむり,頬０涙０",
			"107.3798669,ダンス微笑み,頬０涙０",
			"109.3765595,ダンスウインク,頬０涙０",
			"112.23414,ダンス微笑み,頬０涙０",
			"115.678139,ダンス目とじ,頬１涙０",
			"116.4845326,ダンス目あけ,頬１涙０",
			"118.7426777,ダンスウインク,頬１涙０",
			"119.7674076,ダンスウインク,頬０涙０",
			"121.4678808,ダンス微笑み,頬１涙０",
			"123.3440717,ダンス目あけ,頬１涙０",
			"125.2192905,ダンスウインク,頬１涙０",
			"126.6157771,ダンス微笑み,頬１涙０",
			"130.3629348,ダンスウインク,頬１涙０",
			"131.1754353,ダンス微笑み,頬１涙０",
			"132.0666094,ダンス目とじ,頬１涙０",
			"133.1035353,ダンス目あけ,頬１涙０",
			"133.8736773,ダンス微笑み,頬１涙０",
			"137.6152679,ダンスウインク,頬１涙０",
			"138.4366272,ダンス微笑み,頬１涙０",
			"139.3119712,ダンス目とじ,頬１涙０",
			"142.8441409,ダンス微笑み,頬１涙０",
			"144.2874563,ダンス目とじ,頬１涙０",
			"146.4583348,ダンス微笑み,頬１涙０"
		};

		private string[] dance2Array = new string[]
		{
			"0.0245087,ダンス目つむり,頬０涙０",
			"6.1874215,ダンス目あけ,頬０涙０",
			"7.8939134,ダンス微笑み,頬０涙０",
			"8.6394002,ダンス目とじ,頬１涙０",
			"10.1469584,ダンス微笑み,頬１涙０",
			"11.5385531,ダンス目つむり,頬１涙０",
			"14.5702509,ダンス微笑み,頬０涙０",
			"16.0777871,ダンス目つむり,頬０涙０",
			"17.5522362,ダンス微笑み,頬０涙０",
			"20.6005382,ダンスウインク,頬０涙０",
			"21.8098268,ダンス目とじ,頬０涙０",
			"23.9801006,ダンス微笑み,頬０涙０",
			"26.0011122,ダンス目とじ,頬１涙０",
			"27.227092,ダンス微笑み,頬１涙０",
			"29.5132499,ダンスウインク,頬１涙０",
			"31.3025957,ダンス目つむり,頬０涙０",
			"33.0751747,ダンス微笑み,頬０涙０",
			"34.9140438,ダンス目とじ,頬０涙０",
			"37.1835276,ダンス微笑み,頬０涙０",
			"38.575491,ダンスびっくり,頬０涙０",
			"39.5856112,ダンス目つむり,頬０涙０",
			"40.5798992,ダンス微笑み,頬０涙０",
			"44.6879553,ダンスウインク,頬１涙０",
			"45.8476171,ダンス目つむり,頬１涙０",
			"46.8911092,ダンス目あけ,頬０涙０",
			"49.1941493,ダンス目とじ,頬０涙０",
			"50.1219018,ダンス目とじ,",
			"50.3207443,ダンス目とじ,頬１涙０",
			"52.1927258,ダンス憂い,頬２涙０",
			"54.1144681,ダンス目つむり,頬０涙０",
			"56.1521247,ダンス目あけ,頬１涙０",
			"58.2063844,ダンス微笑み,頬１涙０",
			"60.1944024,ダンス目とじ,頬１涙０",
			"61.1718539,ダンス目あけ,頬０涙０",
			"61.9873436,ダンス微笑み,頬０涙０",
			"62.8159188,ダンス目とじ,頬１涙０",
			"64.3563536,ダンスびっくり,頬１涙０",
			"65.334075,ダンス目あけ,頬１涙０",
			"67.3571023,ダンス目つむり,頬０涙０",
			"70.4877671,ダンス目あけ,頬０涙０",
			"71.1840784,ダンス微笑み,頬０涙０",
			"74.0168774,ダンスウインク,頬１涙０",
			"74.5634544,ダンス微笑み,頬１涙０",
			"76.7668078,ダンス困り顔,頬１涙０",
			"78.7216547,ダンス微笑み,頬１涙０",
			"80.2787063,ダンス目あけ,頬１涙０",
			"81.3723206,ダンス目つむり,頬１涙０",
			"83.3271186,ダンスウインク,頬１涙０",
			"83.8409133,ダンス微笑み,頬１涙０",
			"84.8678841,ダンス目とじ,頬１涙０",
			"85.812214,ダンス微笑み,頬０涙０",
			"87.9658468,ダンスウインク,頬０涙０",
			"88.7113643,ダンス目あけ,頬０涙０",
			"91.2957895,ダンスウインク,頬１涙０",
			"91.7927701,ダンスびっくり,頬０涙０",
			"93.3828702,ダンス微笑み,頬０涙０",
			"95.3876998,ダンス微笑み,頬１涙０",
			"96.1166787,ダンス目とじ,頬１涙０",
			"96.9280784,ダンス目とじ,頬０涙０",
			"98.0549856,ダンス目あけ,頬０涙０",
			"100.1754612,ダンスウインク,頬１涙０",
			"101.5670347,ダンス目つむり,頬０涙０",
			"103.6954164,ダンス目とじ,頬０涙０",
			"104.9544798,ダンス目あけ,頬０涙０",
			"107.4890826,ダンスウインク,頬１涙０",
			"108.8806853,ダンス微笑み,頬１涙０"
		};

		private string[] dance3Array = new string[]
		{
			"0.022274,ダンス目つむり,頬０涙０",
			"14.2860347,ダンス微笑み,頬０涙０",
			"16.1746127,ダンス目つむり,頬０涙０",
			"17.5330826,ダンス微笑み,頬０涙０",
			"20.0019509,ダンス目とじ,頬０涙０",
			"20.9458982,ダンス目あけ,頬０涙０",
			"22.6029105,ダンス目つむり,頬０涙０",
			"23.7796127,ダンス目あけ,頬０涙０",
			"25.4249968,ダンス目とじ,頬０涙０",
			"26.5187871,ダンス目あけ,頬０涙０",
			"27.3134497,ダンスウインク,頬０涙０",
			"28.1418322,ダンス微笑み,頬０涙０",
			"29.4012565,ダンス憂い,頬２涙０",
			"31.0244556,ダンス目とじ,頬２涙０",
			"31.3392572,ダンス目とじ,頬０涙０",
			"32.0018848,ダンス目あけ,頬０涙０",
			"33.1284675,ダンス目つむり,頬０涙０",
			"34.0770389,ダンス微笑み,頬０涙０",
			"34.6734482,ダンス微笑み,頬１涙０",
			"35.0879032,ダンス目とじ,頬１涙０",
			"35.9656802,ダンス目とじ,頬０涙０",
			"36.2144141,ダンス微笑み,頬０涙０",
			"37.4404019,ダンス目とじ,頬０涙０",
			"39.3452745,ダンス微笑み,頬０涙０",
			"40.1404475,ダンス目つむり,頬０涙０",
			"40.9356261,ダンス微笑み,頬０涙０",
			"45.5577814,ダンス憂い,頬０涙０",
			"49.3017834,ダンス憂い,頬１涙０",
			"49.6497498,ダンス困り顔,頬１涙０",
			"53.2447342,ダンス困り顔,頬０涙０",
			"53.360699,ダンス目あけ,頬０涙０",
			"56.0447667,ダンス目とじ,頬０涙０",
			"56.9721842,ダンス微笑み,頬０涙０",
			"57.9828307,ダンス目つむり,頬０涙０",
			"59.109298,ダンス微笑み,頬０涙０",
			"60.650019,ダンスウインク,頬０涙０",
			"61.4783468,ダンス微笑み,頬０涙０",
			"63.3835062,ダンス目つむり,頬０涙０",
			"64.510059,ダンス微笑み,頬０涙０",
			"69.0162304,ダンス憂い,頬０涙０",
			"72.2797907,ダンス目つむり,頬０涙０",
			"72.942869,ダンス目あけ,頬０涙０",
			"74.6159535,ダンス微笑み,頬０涙０",
			"76.6868693,ダンス憂い,頬０涙０",
			"77.1670208,ダンス目とじ,頬０涙０",
			"78.0946957,ダンス微笑み,頬０涙０",
			"78.1940517,ダンス微笑み,頬１涙０",
			"79.5194132,ダンス誘惑,頬１涙０",
			"80.6461493,ダンス誘惑,頬０涙０",
			"80.9277707,ダンス微笑み,頬０涙０",
			"82.5677448,ダンス憂い,頬０涙０",
			"84.8356169,ダンス目つむり,頬０涙０",
			"87.6687164,ダンス微笑み,頬０涙０",
			"89.1264935,ダンス目あけ,頬０涙０",
			"90.9986914,ダンスウインク,頬０涙０",
			"92.0256459,ダンス目あけ,頬０涙０",
			"93.4840017,ダンス微笑み,頬０涙０",
			"94.802198,ダンス目あけ,頬０涙０",
			"96.6907082,ダンス目つむり,頬０涙０",
			"98.1483952,ダンス微笑み,頬０涙０",
			"100.2192249,ダンス目あけ,頬０涙０",
			"102.4890645,ダンスウインク,頬０涙０",
			"103.6326527,ダンス微笑み,頬０涙０",
			"106.6140716,ダンス微笑み,頬１涙０",
			"110.1427382,ダンス憂い,頬１涙０",
			"113.1246749,ダンス目あけ,頬１涙０",
			"114.6157696,ダンスウインク,頬１涙０",
			"115.4915232,ダンス微笑み,頬１涙０",
			"116.9159758,ダンス目つむり,頬１涙０",
			"117.4129967,ダンス目つむり,頬０涙０",
			"117.4295456,ダンス困り顔,頬０涙０",
			"119.8679866,ダンス目あけ,頬０涙０",
			"120.9279617,ダンス目つむり,頬０涙０",
			"122.3030549,ダンス微笑み,頬０涙０",
			"123.7443304,ダンス目つむり,頬０涙０",
			"125.848264,ダンス微笑み,頬０涙０",
			"128.3665313,ダンスウインク,頬０涙０",
			"128.979712,ダンス微笑み,頬０涙０",
			"129.3108729,ダンス目とじ,頬０涙０",
			"130.3543889,ダンス微笑み,頬０涙０",
			"133.3698271,ダンスウインク,頬０涙０",
			"133.9991313,ダンス微笑み,頬０涙０",
			"135.8217465,ダンス目つむり,頬０涙０",
			"137.1636281,ダンス微笑み,頬０涙０",
			"138.6048451,ダンスウインク,頬０涙０",
			"139.2674162,ダンス微笑み,頬０涙０",
			"139.8804389,ダンス目つむり,頬０涙０",
			"141.1393675,ダンス微笑み,頬０涙０",
			"142.1831709,ダンス目とじ,頬０涙０",
			"143.094103,ダンス目あけ,頬０涙０",
			"144.0011954,ダンス目とじ,頬０涙０",
			"145.4256086,ダンス微笑み,頬０涙０",
			"147.41356,ダンス目あけ,頬０涙０",
			"148.5731745,ダンス微笑み,頬０涙０",
			"149.9484599,ダンス目つむり,頬０涙０",
			"151.0748992,ダンス微笑み,頬０涙０",
			"152.1682969,ダンスキス,頬０涙０",
			"153.0960132,ダンス微笑み,頬０涙０",
			"153.3944093,ダンスウインク,頬０涙０",
			"154.1397672,ダンス微笑み,頬０涙０",
			"156.2934222,ダンス目つむり,頬０涙０",
			"158.1986857,ダンス微笑み,頬０涙０",
			"161.1475541,ダンス目つむり,頬０涙０"
		};

		private string[] dance4Array = new string[]
		{
			"0.0266233,ダンス目つむり,頬０涙０",
			"9.1715137,ダンス微笑み,頬０涙０",
			"13.3295827,ダンス目つむり,頬０涙０",
			"16.1129108,ダンス目あけ,頬０涙０",
			"18.5981254,ダンス憂い,頬０涙０",
			"20.5694486,ダンス微笑み,頬０涙０",
			"21.6955803,ダンス目あけ,頬０涙０",
			"23.3523559,ダンスウインク,頬０涙０",
			"24.4622903,ダンス微笑み,頬０涙０",
			"25.1912464,ダンス目とじ,頬１涙０",
			"26.4005059,ダンス目あけ,頬１涙０",
			"28.1237971,ダンス微笑み,頬１涙０",
			"30.4593757,ダンス目つむり,頬１涙０",
			"31.9337519,ダンス微笑み,頬１涙０",
			"33.6732875,ダンス目あけ,頬０涙０",
			"34.7004299,ダンス目つむり,頬０涙０",
			"35.3468184,ダンス微笑み,頬０涙０",
			"35.6778679,ダンス目とじ,頬０涙０",
			"36.6225987,ダンス目あけ,頬０涙０",
			"37.6330961,ダンス目つむり,頬０涙０",
			"38.5276398,ダンス微笑み,頬０涙０",
			"39.5710109,ダンス目つむり,頬０涙０",
			"40.2671381,ダンス微笑み,頬０涙０",
			"41.8907169,ダンスウインク,頬０涙０",
			"42.4536922,ダンス微笑み,頬０涙０",
			"42.9837519,ダンス目とじ,頬１涙０",
			"44.0273965,ダンス目あけ,頬０涙０",
			"45.038405,ダンス目つむり,頬０涙０",
			"45.6840922,ダンス目あけ,頬０涙０",
			"46.4574065,ダンスウインク,頬０涙０",
			"46.9375155,ダンス目つむり,頬０涙０",
			"48.3956763,ダンス微笑み,頬０涙０",
			"56.1319704,ダンス困り顔,頬０涙０",
			"58.0002267,ダンス微笑み,頬０涙０",
			"60.0048165,ダンス目つむり,頬１涙０",
			"61.794014,ダンス微笑み,頬１涙０",
			"64.0639671,ダンス目つむり,頬０涙０",
			"65.1295288,ダンス微笑み,頬０涙０",
			"67.7635037,ダンス目つむり,頬０涙０",
			"68.9731254,ダンス目あけ,頬０涙０",
			"69.5526092,ダンスウインク,頬０涙０",
			"70.7784661,ダンス目あけ,頬０涙０",
			"74.0918054,ダンス微笑み,頬０涙０",
			"76.8585898,ダンス目つむり,頬０涙０",
			"78.5982708,ダンス目あけ,頬０涙０",
			"80.4370677,ダンス目つむり,頬０涙０",
			"82.193233,ダンス微笑み,頬０涙０",
			"84.0982955,ダンス目あけ,頬０涙０",
			"85.489736,ダンスウインク,頬１涙０",
			"86.3183552,ダンス目あけ,頬１涙０",
			"87.1963286,ダンス目つむり,頬０涙０",
			"88.7866231,ダンス目あけ,頬０涙０",
			"89.9630331,ダンス微笑み,頬０涙０",
			"92.8124517,ダンス目あけ,頬０涙０",
			"94.121303,ダンス目つむり,頬０涙０",
			"95.9338782,ダンス目あけ,頬０涙０",
			"97.0932997,ダンス微笑み,頬０涙０",
			"99.2634757,ダンス目あけ,頬０涙０",
			"101.2186462,ダンス微笑み,頬０涙０",
			"103.0737806,ダンスウインク,頬０涙０",
			"104.5651607,ダンス目あけ,頬０涙０",
			"105.8573128,ダンス目つむり,頬０涙０",
			"106.818233,ダンス微笑み,頬１涙０",
			"107.7127511,ダンス目あけ,頬１涙０",
			"108.5576711,ダンス微笑み,頬０涙０",
			"110.263984,ダンス目とじ,頬０涙０",
			"111.4897873,ダンス目あけ,頬０涙０",
			"112.5502306,ダンス微笑み,頬０涙０",
			"114.4057486,ダンス目つむり,頬０涙０",
			"116.3935161,ダンス微笑み,頬０涙０",
			"117.5726885,ダンス目つむり,頬０涙０",
			"118.0038674,ダンス目あけ,頬０涙０",
			"119.0971149,ダンス微笑み,頬０涙０",
			"120.7869156,ダンス目つむり,頬０涙０",
			"121.6651188,ダンス微笑み,頬１涙０",
			"123.4872494,ダンス目つむり,頬０涙０",
			"125.6907046,ダンス微笑み,頬０涙０",
			"127.1982003,ダンス目つむり,頬１涙０",
			"127.8774409,ダンス微笑み,頬１涙０",
			"130.5945451,ダンス目つむり,頬０涙０",
			"131.4393078,ダンス目あけ,頬０涙０",
			"133.1290227,ダンス目つむり,頬０涙０",
			"133.9905538,ダンス微笑み,頬０涙０",
			"134.9843155,ダンス目つむり,頬０涙０",
			"135.266171,ダンス目あけ,頬０涙０",
			"135.77969,ダンス目とじ,頬０涙０",
			"136.7244741,ダンス微笑み,頬０涙０",
			"142.4727027,ダンス目あけ,頬０涙０",
			"143.4998149,ダンス目つむり,頬０涙０",
			"144.1459023,ダンス微笑み,頬０涙０",
			"144.4771585,ダンス目とじ,頬０涙０",
			"145.4378672,ダンス目あけ,頬０涙０",
			"146.4319633,ダンス目つむり,頬０涙０",
			"147.3266094,ダンス微笑み,頬０涙０",
			"148.3706491,ダンス目つむり,頬０涙０",
			"149.0661256,ダンス微笑み,頬０涙０",
			"150.6898336,ダンス目つむり,頬０涙０",
			"151.252905,ダンス微笑み,頬０涙０",
			"151.7831517,ダンス目とじ,頬０涙０",
			"152.826711,ダンス目あけ,頬０涙０",
			"153.8376318,ダンス目つむり,頬０涙０",
			"154.483585,ダンス目あけ,頬０涙０",
			"155.2620636,ダンスウインク,頬０涙０",
			"155.7260114,ダンス目つむり,頬０涙０",
			"157.6475926,ダンス微笑み,頬０涙０",
			"161.3752623,ダンス目つむり,頬０涙０",
			"162.9652315,ダンス微笑み,頬１涙０",
			"166.4943732,ダンス目つむり,頬０涙０"
		};

		private string[] dance5Array = new string[]
		{
			"0.0255864,ダンス目つむり,頬０涙０",
			"8.6710788,ダンス微笑み,頬０涙０",
			"19.3232255,ダンス目つむり,頬０涙０",
			"20.6320543,ダンス目あけ,頬０涙０",
			"21.2118098,ダンスウインク,頬０涙０",
			"22.2555254,ダンス微笑み,頬０涙０",
			"26.7121911,ダンス目あけ,頬０涙０",
			"28.7001539,ダンス目とじ,頬０涙０",
			"30.8538794,ダンス目あけ,頬０涙０",
			"34.067741,ダンス目とじ,頬０涙０",
			"36.0390283,ダンス微笑み,頬０涙０",
			"37.8948055,ダンスウインク,頬０涙０",
			"38.540889,ダンス目あけ,頬０涙０",
			"40.9431861,ダンス微笑み,頬０涙０",
			"41.8375326,ダンス目つむり,頬０涙０",
			"43.7759495,ダンス目あけ,頬０涙０",
			"44.7864021,ダンス微笑み,頬０涙０",
			"45.5815713,ダンスびっくり,頬０涙０",
			"46.2774219,ダンス真剣,頬０涙０",
			"47.3211802,ダンス目とじ,頬０涙０",
			"49.2758539,ダンス目あけ,頬０涙０",
			"50.3195751,ダンスウインク,頬０涙０",
			"51.2805392,ダンス目つむり,頬０涙０",
			"52.3906449,ダンスウインク,頬０涙０",
			"53.4838994,ダンス微笑み,頬０涙０",
			"54.8922575,ダンス目つむり,頬０涙０",
			"55.968791,ダンス微笑み,頬０涙０",
			"56.8469745,ダンス目あけ,頬０涙０",
			"60.5082134,ダンスウインク,頬０涙０",
			"63.4414551,ダンス目つむり,頬０涙０",
			"64.8165033,ダンス微笑み,頬０涙０",
			"68.5603114,ダンス目あけ,頬０涙０",
			"68.8419853,ダンス目あけ,頬１涙０",
			"70.1178741,ダンスウインク,頬１涙０",
			"71.1283724,ダンス微笑み,頬１涙０",
			"71.8736779,ダンス目つむり,頬１涙０",
			"75.1095866,ダンス微笑み,頬１涙０",
			"75.1259131,ダンス微笑み,頬０涙０",
			"75.8547985,ダンスウインク,頬０涙０",
			"77.1470399,ダンス目あけ,頬０涙０",
			"79.2512859,ダンス目つむり,頬０涙０",
			"79.5329749,ダンス目つむり,頬１涙０",
			"80.6094302,ダンス微笑み,頬１涙０",
			"81.653418,ダンスウインク,頬１涙０",
			"83.4755651,ダンス微笑み,頬１涙０",
			"83.4921539,ダンス微笑み,頬０涙０",
			"87.6340195,ダンス目つむり,頬０涙０",
			"89.1911486,ダンス微笑み,頬０涙０",
			"92.5875105,ダンス目あけ,頬０涙０",
			"94.3434648,ダンス目つむり,頬０涙０",
			"95.1221181,ダンス微笑み,頬０涙０",
			"96.132745,ダンス目つむり,頬０涙０",
			"96.8284772,ダンス目あけ,頬０涙０",
			"97.5905805,ダンス微笑み,頬０涙０",
			"99.4624237,ダンス目つむり,頬０涙０",
			"99.9263136,ダンス目あけ,頬０涙０",
			"101.7321065,ダンスウインク,頬０涙０",
			"102.3946719,ダンス微笑み,頬０涙０",
			"104.2003796,ダンス目とじ,頬０涙０",
			"105.8242386,ダンス目あけ,頬０涙０",
			"106.7352711,ダンスウインク,頬０涙０",
			"107.1326931,ダンス微笑み,頬０涙０",
			"109.4524232,ダンス目あけ,頬０涙０",
			"110.8270018,ダンス目とじ,頬０涙０",
			"111.6387813,ダンス憂い,頬０涙０",
			"111.9373142,ダンス憂い,頬１涙０",
			"112.9975687,ダンス目つむり,頬１涙０",
			"113.4114977,ダンスびっくり,頬１涙０",
			"113.6768051,ダンスびっくり,頬０涙０",
			"114.8530306,ダンス微笑み,頬０涙０",
			"117.2880137,ダンス目あけ,頬０涙０",
			"119.4088072,ダンスウインク,頬０涙０",
			"120.1209467,ダンス目あけ,頬０涙０",
			"120.5848406,ダンス微笑み,頬０涙０",
			"121.9598759,ダンスウインク,頬０涙０",
			"122.3908134,ダンス微笑み,頬０涙０",
			"123.0201122,ダンス目とじ,頬０涙０",
			"124.2460432,ダンス微笑み,頬０涙０",
			"124.8590759,ダンス微笑み,頬１涙０",
			"125.6708371,ダンス目つむり,頬１涙０",
			"126.9301451,ダンス微笑み,頬１涙０",
			"129.9452919,ダンス目つむり,頬１涙０",
			"132.6290365,ダンス目つむり,頬０涙０",
			"132.6456539,ダンス微笑み,頬０涙０",
			"136.2237555,ダンス目つむり,頬０涙０",
			"140.398868,ダンス微笑み,頬０涙０",
			"143.5631131,ダンス目つむり,頬０涙０"
		};

		private string[] dance6Array = new string[]
		{
			"0.1056333,ダンス目つむり,頬０涙０",
			"4.1370722,ダンス目あけ,頬０涙０",
			"4.716936,ダンス微笑み,頬０涙０",
			"6.0757298,ダンス目つむり,頬０涙０",
			"6.489576,ダンス目あけ,頬０涙０",
			"7.9143288,ダンスウインク,頬０涙０",
			"8.6435937,ダンス微笑み,頬０涙０",
			"9.4716549,ダンス目とじ,頬０涙０",
			"10.4658563,ダンス微笑み,頬０涙０",
			"12.0317613,ダンス目とじ,頬０涙０",
			"12.5121751,ダンス目あけ,頬０涙０",
			"13.8705705,ダンス目あけ,頬１涙０",
			"14.0859648,ダンスウインク,頬１涙０",
			"14.732196,ダンスウインク,頬０涙０",
			"14.7487355,ダンス目あけ,頬０涙０",
			"15.6932582,ダンス微笑み,頬０涙０",
			"16.3722342,ダンス目とじ,頬０涙０",
			"17.3417961,ダンス微笑み,頬０涙０",
			"18.1368307,ダンス目つむり,頬０涙０",
			"18.7336668,ダンス目あけ,頬０涙０",
			"19.1644407,ダンス目あけ,頬１涙０",
			"19.3632219,ダンス目とじ,頬１涙０",
			"20.1911754,ダンス微笑み,頬１涙０",
			"21.4601514,ダンス微笑み,頬０涙０",
			"22.0230116,ダンス目とじ,頬０涙０",
			"22.6695521,ダンス目あけ,頬０涙０",
			"23.2987314,ダンス微笑み,頬０涙０",
			"24.116074,ダンス目とじ,頬０涙０",
			"24.6293347,ダンス目あけ,頬０涙０",
			"25.1266646,ダンスウインク,頬０涙０",
			"25.623347,ダンス微笑み,頬０涙０",
			"26.6436949,ダンス目つむり,頬０涙０",
			"27.670849,ダンス目つむり,頬２涙０",
			"27.9694244,ダンス目あけ,頬２涙０",
			"28.6648753,ダンス目あけ,頬０涙０",
			"29.344066,ダンス微笑み,頬０涙０",
			"30.0896047,ダンスウインク,頬０涙０",
			"30.9841467,ダンス微笑み,頬０涙０",
			"32.1663756,ダンスウインク,頬０涙０",
			"33.1270737,ダンス微笑み,頬０涙０",
			"33.5744364,ダンス微笑み,頬１涙０",
			"34.3596171,ダンス目あけ,頬１涙０",
			"34.856573,ダンス目とじ,頬１涙０",
			"35.370527,ダンス微笑み,頬１涙０",
			"36.4473253,ダンス目つむり,頬１涙０",
			"36.7786237,ダンス目つむり,頬０涙０",
			"36.9277633,ダンス微笑み,頬０涙０",
			"37.9545516,ダンス目つむり,頬０涙０",
			"38.7994574,ダンス目あけ,頬０涙０",
			"40.1247889,ダンスウインク,頬０涙０",
			"41.1021665,ダンス目あけ,頬０涙０",
			"41.7318496,ダンス目とじ,頬０涙０",
			"43.2115258,ダンス目あけ,頬０涙０",
			"43.8410253,ダンス目つむり,頬０涙０",
			"44.4704954,ダンス微笑み,頬０涙０",
			"45.9283783,ダンス目とじ,頬０涙０",
			"46.4418682,ダンス目あけ,頬０涙０",
			"46.9392691,ダンスウインク,頬０涙０",
			"47.469097,ダンス目あけ,頬０涙０",
			"47.9495963,ダンス微笑み,頬０涙０",
			"48.7612471,ダンスウインク,頬０涙０",
			"49.3079598,ダンス微笑み,頬０涙０",
			"50.7824284,ダンスウインク,頬０涙０",
			"51.6501821,ダンス目あけ,頬０涙０",
			"52.7601726,ダンス微笑み,頬０涙０",
			"53.0912922,ダンス微笑み,頬２涙０",
			"53.737242,ダンス目つむり,頬２涙０",
			"54.2674231,ダンス目つむり,頬０涙０",
			"54.6487264,ダンス目あけ,頬０涙０",
			"55.924179,ダンス目つむり,頬０涙０",
			"57.3985464,ダンス目あけ,頬０涙０",
			"58.773529,ダンス微笑み,頬０涙０",
			"59.0058255,ダンス微笑み,頬１涙０",
			"60.4883723,ダンス目つむり,頬１涙０",
			"60.9853889,ダンス目つむり,頬２涙０",
			"61.3333514,ダンス微笑み,頬２涙０",
			"61.7145905,ダンス目とじ,頬２涙０",
			"62.5261133,ダンス微笑み,頬２涙０",
			"63.421065,ダンス目あけ,頬２涙０",
			"64.1334195,ダンス目とじ,頬２涙０",
			"64.6300472,ダンス目とじ,頬０涙０",
			"65.4418734,ダンス目あけ,頬０涙０",
			"66.6846809,ダンス微笑み,頬０涙０",
			"67.4960692,ダンス目とじ,頬０涙０",
			"67.8936653,ダンス目とじ,頬１涙０",
			"68.4076251,ダンス目あけ,頬１涙０",
			"69.5669177,ダンス微笑み,頬１涙０",
			"70.5278282,ダンス目あけ,頬１涙０",
			"71.3063832,ダンス目つむり,頬１涙０",
			"71.7706418,ダンス微笑み,頬１涙０",
			"71.9856643,ダンス微笑み,頬０涙０",
			"72.1245814,ダンス目つむり,頬０涙０",
			"72.6881378,ダンス目あけ,頬０涙０",
			"73.5163777,ダンス微笑み,頬０涙０",
			"74.5433023,ダンス目つむり,頬０涙０",
			"75.5704558,ダンス微笑み,頬０涙０",
			"76.216502,ダンスウインク,頬０涙０",
			"76.8956542,ダンス微笑み,頬０涙０",
			"77.5017607,ダンス目とじ,頬０涙０",
			"78.1478963,ダンス微笑み,頬０涙０",
			"80.1689584,ダンスウインク,頬０涙０",
			"81.1795427,ダンス目あけ,頬０涙０",
			"82.3888767,ダンス微笑み,頬０涙０",
			"83.3832439,ダンス目とじ,頬０涙０",
			"84.1945857,ダンス目あけ,頬０涙０",
			"85.3211796,ダンス微笑み,頬０涙０",
			"86.0670011,ダンス目つむり,頬０涙０",
			"86.9116269,ダンス目あけ,頬０涙０",
			"88.6013744,ダンス微笑み,頬０涙０",
			"89.3800257,ダンス目あけ,頬０涙０",
			"91.1525739,ダンス微笑み,頬０涙０",
			"92.1040373,ダンス目とじ,頬０涙０",
			"92.6338632,ダンス目あけ,頬０涙０",
			"93.1475063,ダンス微笑み,頬０涙０",
			"94.4996871,ダンス目とじ,頬０涙０",
			"95.112665,ダンス目あけ,頬０涙０",
			"96.0238472,ダンス目つむり,頬０涙０",
			"96.5704735,ダンスウインク,頬０涙０",
			"97.0343021,ダンス微笑み,頬０涙０",
			"97.6970563,ダンス目あけ,頬０涙０",
			"98.8137341,ダンス目とじ,頬０涙０",
			"99.7580324,ダンス目あけ,頬０涙０",
			"100.5532161,ダンス微笑み,頬０涙０",
			"102.4086754,ダンス目つむり,頬０涙０",
			"103.0094505,ダンスウインク,頬０涙０",
			"103.6985582,ダンス微笑み,頬０涙０",
			"105.007751,ダンス目つむり,頬０涙０",
			"105.9349613,ダンスウインク,頬０涙０",
			"106.9128751,ダンス目あけ,頬０涙０",
			"108.6854906,ダンスウインク,頬０涙０",
			"109.16566,ダンス微笑み,頬０涙０",
			"110.3918006,ダンス目あけ,頬０涙０",
			"110.789055,ダンス目とじ,頬０涙０",
			"113.0259247,ダンス目あけ,頬０涙０",
			"114.8975843,ダンス微笑み,頬０涙０",
			"115.7339345,ダンス微笑み,頬１涙０",
			"116.578943,ダンス目とじ,頬１涙０",
			"117.0427707,ダンス微笑み,頬１涙０",
			"118.5225416,ダンスウインク,頬１涙０",
			"119.6987569,ダンス目あけ,頬１涙０",
			"119.8150036,ダンス目あけ,頬０涙０",
			"120.8418669,ダンス微笑み,頬０涙０",
			"123.045232,ダンス目つむり,頬０涙０",
			"123.3268688,ダンスウインク,頬０涙０",
			"123.7744712,ダンス微笑み,頬０涙０",
			"124.4534653,ダンス目つむり,頬０涙０",
			"125.16581,ダンス目あけ,頬０涙０",
			"125.6796617,ダンスウインク,頬０涙０"
		};

		private string[] dance6BArray = new string[]
		{
			"42.1789853,ダンス微笑み,頬０涙０",
			"43.8410418,ダンス目とじ,頬０涙０",
			"44.4705037,ダンス微笑み,頬０涙０",
			"45.9283871,ダンスウインク,頬０涙０",
			"46.4418762,ダンス目あけ,頬０涙０",
			"46.9392788,ダンス微笑み,頬０涙０",
			"47.4691039,ダンス目つむり,頬０涙０",
			"47.949604,ダンス真剣,頬０涙０",
			"49.2914611,ダンス目つむり,頬０涙０",
			"50.6004478,ダンス憂い,頬０涙０",
			"51.6501912,ダンス真剣,頬０涙０",
			"52.7601817,ダンス目あけ,頬０涙０",
			"53.7372494,ダンス目つむり,頬０涙０",
			"54.6487355,ダンス微笑み,頬０涙０",
			"55.9241873,ダンス目つむり,頬０涙０",
			"56.7689585,ダンス目あけ,頬０涙０",
			"57.5476122,ダンス目とじ,頬０涙０",
			"59.0058352,ダンス目とじ,頬１涙０",
			"60.9853978,ダンス目とじ,頬２涙０",
			"64.6300557,ダンス目とじ,頬０涙０",
			"67.8936722,ダンス目とじ,頬１涙０"
		};

		private string[] dance6CArray = new string[]
		{
			"0.0301873,ダンス目つむり,頬０涙０",
			"4.0736715,ダンス目あけ,頬０涙０",
			"4.6534743,ダンス目つむり,頬０涙０",
			"5.4210753,ダンス微笑み,頬０涙０",
			"6.0149769,ダンス目つむり,頬０涙０",
			"6.4295996,ダンス目あけ,頬０涙０",
			"7.8564798,ダンス目つむり,頬０涙０",
			"8.5840895,ダンス微笑み,頬０涙０",
			"9.9969366,ダンス目とじ,頬０涙０",
			"12.4453651,ダンス目あけ,頬０涙０",
			"14.0255442,ダンスウインク,頬０涙０",
			"14.690333,ダンス目あけ,頬０涙０",
			"15.6327357,ダンス微笑み,頬０涙０",
			"16.3065226,ダンス目とじ,頬０涙０",
			"17.2758104,ダンス微笑み,頬０涙０",
			"18.0709194,ダンス目つむり,頬０涙０",
			"18.667826,ダンス目あけ,頬０涙０",
			"19.2974712,ダンス目とじ,頬０涙０",
			"20.1424222,ダンス微笑み,頬０涙０",
			"21.9673622,ダンス目とじ,頬０涙０",
			"22.6169959,ダンス目あけ,頬０涙０",
			"23.2296377,ダンス微笑み,頬０涙０",
			"24.0581386,ダンス目とじ,頬０涙０",
			"24.5681199,ダンス目あけ,頬０涙０",
			"25.0650423,ダンスウインク,頬０涙０",
			"25.5620464,ダンス微笑み,頬０涙０",
			"26.5826515,ダンス目つむり,頬０涙０",
			"27.9081267,ダンス目あけ,頬０涙０",
			"29.2840601,ダンス目とじ,頬０涙０",
			"30.0295131,ダンス微笑み,頬０涙０",
			"30.9241651,ダンス目とじ,頬０涙０",
			"32.1003977,ダンスウインク,頬０涙０",
			"32.9287661,ダンス微笑み,頬０涙０",
			"34.2998116,ダンス目あけ,頬０涙０",
			"34.7968219,ダンス目とじ,頬０涙０",
			"35.2938718,ダンス微笑み,頬０涙０",
			"36.3775809,ダンス目つむり,頬０涙０",
			"37.8851325,ダンス目あけ,頬０涙０",
			"38.7316789,ダンス目とじ,頬０涙０",
			"41.0269677,ダンス目あけ,頬０涙０",
			"41.6752899,ダンス目とじ,頬０涙０",
			"43.1527423,ダンス目あけ,頬０涙０",
			"43.7820286,ダンス目つむり,頬０涙０",
			"44.4083353,ダンス目あけ,頬０涙０",
			"45.8652883,ダンス目とじ,頬０涙０",
			"46.7471353,ダンス目あけ,頬０涙０",
			"47.4055082,ダンス微笑み,頬０涙０",
			"47.8815308,ダンス目つむり,頬０涙０",
			"49.4703841,ダンスびっくり,頬０涙０",
			"50.1976579,ダンス目あけ,頬０涙０",
			"51.0252126,ダンス目とじ,頬０涙０",
			"53.6696677,ダンス微笑み,頬０涙０",
			"54.5837739,ダンス目とじ,頬０涙０",
			"55.8695814,ダンス目あけ,頬０涙０",
			"57.327443,ダンス目つむり,頬０涙０",
			"58.1442212,ダンス目あけ,頬０涙０",
			"58.7240316,ダンス微笑み,頬０涙０",
			"60.409074,ダンス目つむり,頬０涙０",
			"61.2672023,ダンス微笑み,頬０涙０",
			"61.6470854,ダンス目とじ,頬０涙０",
			"62.4645489,ダンス微笑み,頬０涙０",
			"63.3565592,ダンス目あけ,頬０涙０",
			"64.0651569,ダンス目とじ,頬０涙０",
			"65.1110234,ダンス目あけ,頬０涙０",
			"66.6141203,ダンス微笑み,頬０涙０",
			"67.4351422,ダンス目とじ,頬０涙０",
			"68.3463498,ダンス目あけ,頬０涙０",
			"68.8943555,ダンス微笑み,頬０涙０",
			"70.4734438,ダンス目あけ,頬０涙０",
			"71.2604297,ダンス目つむり,頬０涙０",
			"71.6939406,ダンス微笑み,頬０涙０",
			"72.0623336,ダンス目つむり,頬０涙０",
			"72.6130917,ダンス目あけ,頬０涙０",
			"73.4516781,ダンス微笑み,頬０涙０",
			"74.4854289,ダンス目つむり,頬０涙０",
			"75.4962562,ダンス微笑み,頬０涙０",
			"76.1527123,ダンスウインク,頬０涙０",
			"76.8353567,ダンス微笑み,頬０涙０",
			"77.4339796,ダンス目とじ,頬０涙０",
			"78.0840366,ダンス微笑み,頬０涙０",
			"80.0959715,ダンスウインク,頬０涙０",
			"81.1254014,ダンス目あけ,頬０涙０",
			"82.3237649,ダンス微笑み,頬０涙０",
			"83.3107997,ダンス目とじ,頬０涙０",
			"84.1395044,ダンス目あけ,頬０涙０",
			"84.579332,ダンス目とじ,頬０涙０",
			"85.2490611,ダンス微笑み,頬０涙０",
			"85.9946281,ダンスウインク,頬０涙０",
			"86.4087282,ダンス目あけ,頬０涙０",
			"88.5404209,ダンス微笑み,頬０涙０",
			"89.3236931,ダンス目あけ,頬０涙０",
			"90.7642867,ダンス目つむり,頬０涙０",
			"92.5750016,ダンス目あけ,頬０涙０",
			"93.0895616,ダンス目とじ,頬０涙０",
			"94.4351864,ダンス微笑み,頬０涙０",
			"95.0464911,ダンス目あけ,頬０涙０",
			"95.9677801,ダンス目つむり,頬０涙０",
			"96.5034897,ダンスウインク,頬０涙０",
			"96.9673895,ダンス微笑み,頬０涙０",
			"97.6299181,ダンス目あけ,頬０涙０",
			"98.7571749,ダンス目とじ,頬０涙０",
			"99.6952435,ダンス目あけ,頬０涙０",
			"100.4836384,ダンス目とじ,頬０涙０",
			"103.1037827,ダンス微笑み,頬０涙０",
			"103.9381784,ダンス目つむり,頬０涙０",
			"104.565201,ダンス目あけ,頬０涙０",
			"105.049217,ダンス目つむり,頬０涙０",
			"105.8673855,ダンスウインク,頬０涙０",
			"106.8462701,ダンス目あけ,頬０涙０",
			"108.625357,ダンスウインク,頬０涙０",
			"109.1038087,ダンス微笑み,頬０涙０",
			"110.3270484,ダンス目あけ,頬０涙０",
			"110.71443,ダンス目とじ,頬０涙０",
			"112.9677907,ダンス目あけ,頬０涙０",
			"114.8403485,ダンス微笑み,頬０涙０",
			"116.5136676,ダンス目とじ,頬０涙０",
			"116.9775334,ダンス微笑み,頬０涙０",
			"118.4484091,ダンスウインク,頬０涙０",
			"119.6412118,ダンス目あけ,頬０涙０",
			"120.8350377,ダンス目つむり,頬０涙０",
			"123.7673895,ダンス目あけ,頬０涙０",
			"124.4299993,ダンス目つむり,頬０涙０",
			"125.6736218,ダンス目あけ,頬０涙０"
		};

		private string[] dance6DArray = new string[]
		{
			"0.0301885,ダンス目つむり,頬０涙０",
			"4.0736755,ダンス目あけ,頬０涙０",
			"4.6534771,ダンス目つむり,頬０涙０",
			"5.4210819,ダンス微笑み,頬０涙０",
			"6.0149783,ダンス目つむり,頬０涙０",
			"6.429601,ダンス目あけ,頬０涙０",
			"7.8564816,ダンス目つむり,頬０涙０",
			"8.5840909,ダンス微笑み,頬０涙０",
			"11.9613602,ダンス目とじ,頬０涙０",
			"12.4453693,ダンス目あけ,頬０涙０",
			"14.0255462,ダンスウインク,頬０涙０",
			"14.6903361,ダンス目あけ,頬０涙０",
			"15.6327374,ダンス微笑み,頬０涙０",
			"16.3065243,ダンス目とじ,頬０涙０",
			"17.2758121,ダンス微笑み,頬０涙０",
			"18.0709211,ダンス目つむり,頬０涙０",
			"18.6678279,ダンス目あけ,頬０涙０",
			"19.2974772,ダンス目とじ,頬０涙０",
			"20.1424239,ダンス微笑み,頬０涙０",
			"21.9673642,ダンス目とじ,頬０涙０",
			"22.616999,ダンス目あけ,頬０涙０",
			"23.2296397,ダンス微笑み,頬０涙０",
			"24.0581418,ダンス目とじ,頬０涙０",
			"24.5681221,ダンス目あけ,頬０涙０",
			"25.0650466,ダンスウインク,頬０涙０",
			"25.5620484,ダンス微笑み,頬０涙０",
			"26.5826552,ダンス目つむり,頬０涙０",
			"27.908129,ダンス目あけ,頬０涙０",
			"29.2840621,ダンス目とじ,頬０涙０",
			"30.0295165,ダンス微笑み,頬０涙０",
			"30.9241668,ダンス目とじ,頬０涙０",
			"32.1003997,ダンスウインク,頬０涙０",
			"32.9287758,ダンス微笑み,頬０涙０",
			"34.2998136,ダンス目あけ,頬０涙０",
			"34.7968236,ダンス目とじ,頬０涙０",
			"35.2938747,ダンス微笑み,頬０涙０",
			"36.3775829,ダンス目つむり,頬０涙０",
			"37.8851345,ダンス目あけ,頬０涙０",
			"38.731682,ダンス目とじ,頬０涙０",
			"41.02697,ダンス目あけ,頬０涙０",
			"41.6752922,ダンス目とじ,頬０涙０",
			"43.1527443,ダンス目あけ,頬０涙０",
			"43.7820303,ダンス目つむり,頬０涙０",
			"44.4083373,ダンス目あけ,頬０涙０",
			"45.86529,ダンス目とじ,頬０涙０",
			"46.7471416,ダンス目あけ,頬０涙０",
			"47.4055099,ダンス微笑み,頬０涙０",
			"47.8815331,ダンス目つむり,頬０涙０",
			"49.4703904,ダンスびっくり,頬０涙０",
			"50.1976682,ダンス目あけ,頬０涙０",
			"51.0252186,ダンス目とじ,頬０涙０",
			"53.66967,ダンス微笑み,頬０涙０",
			"54.5837762,ダンス目とじ,頬０涙０",
			"55.8695839,ダンス目あけ,頬０涙０",
			"57.3274456,ダンス目つむり,頬０涙０",
			"58.1442408,ダンス目あけ,頬０涙０",
			"58.7240342,ダンス微笑み,頬０涙０",
			"60.4090777,ダンス目つむり,頬０涙０",
			"61.2672049,ダンス微笑み,頬０涙０",
			"61.6470877,ダンス目とじ,頬０涙０",
			"62.4645541,ダンス微笑み,頬０涙０",
			"63.3565618,ダンス目あけ,頬０涙０",
			"64.0651592,ダンス目とじ,頬０涙０",
			"65.1110425,ダンス目あけ,頬０涙０",
			"66.6141229,ダンス微笑み,頬０涙０",
			"67.4351479,ダンス目とじ,頬０涙０",
			"68.3463538,ダンス目あけ,頬０涙０",
			"68.8943658,ダンス微笑み,頬０涙０",
			"70.4734461,ダンス目あけ,頬０涙０",
			"71.2604326,ダンス目つむり,頬０涙０",
			"71.6939441,ダンス微笑み,頬０涙０",
			"72.0623367,ダンス目つむり,頬０涙０",
			"72.6130948,ダンス目あけ,頬０涙０",
			"73.4516818,ダンス微笑み,頬０涙０",
			"74.4854314,ダンス目つむり,頬０涙０",
			"75.496259,ダンス微笑み,頬０涙０",
			"76.1527152,ダンスウインク,頬０涙０",
			"76.8353601,ダンス微笑み,頬０涙０",
			"77.4339824,ダンス目とじ,頬０涙０",
			"78.0840392,ダンス微笑み,頬０涙０",
			"80.0959741,ダンスウインク,頬０涙０",
			"81.1254039,ダンス目あけ,頬０涙０",
			"82.3237684,ダンス微笑み,頬０涙０",
			"83.3108022,ダンス目とじ,頬０涙０",
			"84.1395079,ダンス目あけ,頬０涙０",
			"84.5793406,ダンス目とじ,頬０涙０",
			"85.2490639,ダンス微笑み,頬０涙０",
			"85.9946309,ダンスウインク,頬０涙０",
			"86.408737,ダンス目あけ,頬０涙０",
			"88.5404237,ダンス微笑み,頬０涙０",
			"89.3236959,ダンス目あけ,頬０涙０",
			"90.7642953,ダンス目つむり,頬０涙０",
			"92.5750048,ダンス目あけ,頬０涙０",
			"93.0895644,ダンス目とじ,頬０涙０",
			"94.4351892,ダンス微笑み,頬０涙０",
			"95.046494,ダンス目あけ,頬０涙０",
			"95.9677824,ダンス目つむり,頬０涙０",
			"96.5034923,ダンスウインク,頬０涙０",
			"96.9673918,ダンス微笑み,頬０涙０",
			"97.6299206,ダンス目あけ,頬０涙０",
			"98.7571775,ダンス目とじ,頬０涙０",
			"99.695247,ダンス目あけ,頬０涙０",
			"101.0807397,ダンスウインク,頬０涙０",
			"103.1037907,ダンス微笑み,頬０涙０",
			"103.9381878,ダンス目つむり,頬０涙０",
			"104.5652101,ダンス目あけ,頬０涙０",
			"105.0492278,ダンス目つむり,頬０涙０",
			"105.8673884,ダンスウインク,頬０涙０",
			"106.8462735,ダンス目あけ,頬０涙０",
			"108.6253596,ダンスウインク,頬０涙０",
			"109.1038115,ダンス微笑み,頬０涙０",
			"110.3270513,ダンス目あけ,頬０涙０",
			"110.7144331,ダンス目とじ,頬０涙０",
			"112.9677932,ダンス目あけ,頬０涙０",
			"114.8403511,ダンス微笑み,頬０涙０",
			"116.513671,ダンス目とじ,頬０涙０",
			"116.9775366,ダンス微笑み,頬０涙０",
			"118.4484117,ダンスウインク,頬０涙０",
			"119.6412146,ダンス目あけ,頬０涙０",
			"120.8350405,ダンス目つむり,頬０涙０",
			"123.7673923,ダンス目あけ,頬０涙０",
			"124.4300015,ダンス目つむり,頬０涙０",
			"125.6736249,ダンス目あけ,頬０涙０"
		};

		private string[] dance7Array = new string[]
		{
			"0.0254934,ダンス目つむり,頬０涙０",
			"9.5347522,ダンス目あけ,頬１涙０",
			"12.8645317,ダンスウインク,頬１涙０",
			"15.1176127,ダンス目とじ,頬１涙０",
			"16.8736059,ダンス微笑み,頬１涙０",
			"17.6521813,ダンス目つむり,頬１涙０",
			"21.0317796,ダンス目あけ,頬１涙０",
			"26.5319197,ダンス目とじ,頬１涙０",
			"28.5365643,ダンス微笑み,頬１涙０",
			"31.6179826,ダンス微笑み,頬２涙０",
			"31.7007939,ダンスウインク,頬２涙０",
			"32.2806437,ダンスウインク,頬１涙０",
			"32.3634749,ダンス目あけ,頬１涙０",
			"35.7264812,ダンス目とじ,頬１涙０",
			"39.0398186,ダンス目あけ,頬１涙０",
			"40.7296173,ダンス目とじ,頬１涙０",
			"42.5354675,ダンス微笑み,頬１涙０",
			"46.9586709,ダンスウインク,頬１涙０",
			"47.7373846,ダンス目あけ,頬１涙０",
			"48.201299,ダンス微笑み,頬１涙０",
			"49.112299,ダンス目とじ,頬１涙０",
			"50.5205134,ダンス目あけ,頬１涙０",
			"51.3488595,ダンス目とじ,頬１涙０",
			"53.1877481,ダンス微笑み,頬１涙０",
			"55.8714933,ダンス目とじ,頬１涙０",
			"58.0417647,ダンス目あけ,頬１涙０",
			"59.0357747,ダンス目つむり,頬１涙０",
			"59.8642349,ダンス微笑み,頬１涙０",
			"61.5373757,ダンス目とじ,頬１涙０",
			"62.9123482,ダンス微笑み,頬１涙０",
			"64.8009744,ダンス目とじ,頬１涙０",
			"66.2257082,ダンス目あけ,頬１涙０",
			"67.3522055,ダンス目つむり,頬１涙０",
			"68.1143132,ダンス真剣,頬１涙０",
			"69.1580098,ダンス目あけ,頬１涙０",
			"69.7047403,ダンス目とじ,頬１涙０",
			"69.9531662,ダンス目あけ,頬１涙０",
			"71.1957381,ダンスウインク,頬１涙０",
			"71.792047,ダンス微笑み,頬１涙０",
			"74.3433756,ダンスウインク,頬１涙０",
			"75.4699394,ダンス目とじ,頬１涙０",
			"76.1161188,ダンス目あけ,頬１涙０",
			"77.0272149,ダンスウインク,頬１涙０",
			"77.8720777,ダンス目あけ,頬１涙０",
			"79.1643145,ダンス目とじ,頬１涙０",
			"81.3676731,ダンス微笑み,頬１涙０",
			"82.6101634,ダンス目つむり,頬１涙０",
			"84.6810002,ダンス目あけ,頬１涙０",
			"86.9009054,ダンス微笑み,頬１涙０",
			"88.9221132,ダンス目とじ,頬１涙０",
			"91.5230579,ダンス目あけ,頬１涙０",
			"93.4447381,ダンス目とじ,頬１涙０",
			"94.8695266,ダンス微笑み,頬１涙０",
			"96.3771418,ダンス目つむり,頬１涙０",
			"97.2054207,ダンス目あけ,頬１涙０",
			"98.4478771,ダンス目とじ,頬１涙０",
			"99.2762648,ダンス目あけ,頬１涙０",
			"99.7897959,ダンスウインク,頬１涙０",
			"100.6181158,ダンス微笑み,頬１涙０",
			"101.4464336,ダンス目とじ,頬１涙０",
			"102.1091101,ダンス目あけ,頬１涙０",
			"102.6889651,ダンス目とじ,頬１涙０",
			"102.9374377,ダンス目あけ,頬１涙０",
			"103.6498364,ダンス真剣,頬１涙０",
			"105.6212464,ダンス微笑み,頬１涙０",
			"106.6152351,ダンスウインク,頬１涙０",
			"107.4932344,ダンス目あけ,頬１涙０",
			"108.2056767,ダンス微笑み,頬１涙０",
			"110.9391392,ダンス目とじ,頬１涙０",
			"112.1319148,ダンス微笑み,頬１涙０",
			"113.5732471,ダンス目とじ,頬１涙０",
			"113.970835,ダンス目あけ,頬１涙０",
			"115.4287098,ダンス目つむり,頬１涙０",
			"116.0748341,ダンス微笑み,頬１涙０",
			"117.8640231,ダンスウインク,頬１涙０",
			"118.6922868,ダンス微笑み,頬１涙０",
			"123.1818958,ダンス目とじ,頬１涙０",
			"125.4515042,ダンス目あけ,頬１涙０",
			"126.9094131,ダンス目とじ,頬１涙０",
			"127.7543245,ダンス微笑み,頬１涙０",
			"129.0299855,ダンスウインク,頬１涙０",
			"129.9577261,ダンス目あけ,頬１涙０",
			"131.117388,ダンス目つむり,頬１涙０",
			"135.4246707,ダンス目あけ,頬１涙０",
			"136.0376307,ダンス目とじ,頬１涙０",
			"136.2861396,ダンス微笑み,頬１涙０",
			"138.4563617,ダンスウインク,頬１涙０",
			"139.4503752,ダンス目あけ,頬１涙０",
			"140.4610277,ダンス目つむり,頬１涙０",
			"141.0904722,ダンス微笑み,頬１涙０",
			"142.0347931,ダンス目とじ,頬１涙０",
			"144.7020121,ダンス目あけ,頬１涙０",
			"146.7894215,ダンスウインク,頬１涙０",
			"148.7774444,ダンス微笑み,頬１涙０",
			"149.9702115,ダンス目とじ,頬１涙０",
			"150.6825597,ダンス微笑み,頬１涙０",
			"151.3618228,ダンス目とじ,頬１涙０",
			"153.1179212,ダンス目あけ,頬１涙０",
			"154.4432137,ダンスウインク,頬１涙０",
			"155.3212207,ダンス目つむり,頬１涙０",
			"155.8844876,ダンス目あけ,頬１涙０",
			"157.1436185,ダンス目とじ,頬１涙０",
			"158.9327258,ダンス微笑み,頬１涙０",
			"160.3905764,ダンス目つむり,頬１涙０",
			"161.0367273,ダンス目あけ,頬１涙０",
			"161.6662907,ダンス目つむり,頬１涙０",
			"162.0970164,ダンス微笑み,頬１涙０",
			"162.7762231,ダンスウインク,頬１涙０",
			"163.8752985,ダンス目つむり,頬１涙０",
			"164.2397676,ダンス目あけ,頬１涙０",
			"165.7638987,ダンス目とじ,頬１涙０",
			"166.7910016,ダンス微笑み,頬１涙０",
			"168.7790342,ダンスウインク,頬１涙０",
			"174.9451933,微笑み,頬１涙０"
		};

		private string[] danceO1Array = new string[]
		{
			"0.0248034,ダンス目あけ,頬０涙０",
			"5.0888439,ダンス目つむり,頬０涙０",
			"6.1824799,ダンス微笑み,頬０涙０",
			"7.4414916,ダンス目あけ,頬０涙０",
			"8.833125,ダンス目つむり,頬０涙０",
			"9.3299331,ダンス目あけ,頬０涙０",
			"10.6388956,ダンス目とじ,頬０涙０",
			"11.3856612,ダンス微笑み,頬０涙０",
			"12.3297991,ダンス目つむり,頬０涙０",
			"12.8270088,ダンス目あけ,頬０涙０",
			"13.3736739,ダンス微笑み,頬０涙０",
			"13.8705663,ダンス目つむり,頬０涙０",
			"14.7321408,ダンス目あけ,頬０涙０",
			"15.9429314,ダンス目とじ,頬０涙０",
			"16.6220551,ダンス微笑み,頬０涙０",
			"17.8312662,ダンス目とじ,頬１涙０",
			"18.4940956,ダンス目あけ,頬１涙０",
			"19.1402022,ダンス微笑み,頬０涙０",
			"20.0182535,ダンス目つむり,頬０涙０",
			"20.5152483,ダンス目あけ,頬０涙０",
			"21.02862,ダンス目とじ,頬０涙０",
			"21.8900908,ダンス目あけ,頬０涙０",
			"23.5469673,ダンス目つむり,頬０涙０",
			"24.0446683,ダンス微笑み,頬０涙０",
			"24.2924416,ダンス微笑み,頬１涙０",
			"24.955177,ダンス微笑み,頬０涙０",
			"25.6841583,ダンス目とじ,頬０涙０",
			"26.2142163,ダンス目あけ,頬０涙０",
			"27.026027,ダンスウインク,頬０涙０",
			"27.6389532,ダンス微笑み,頬０涙０",
			"28.287417,ダンス目とじ,頬０涙０",
			"29.2648221,ダンス目あけ,頬０涙０",
			"30.6846162,ダンス目つむり,頬０涙０",
			"31.429573,ダンス微笑み,頬０涙０",
			"32.0426931,ダンス目つむり,頬０涙０",
			"32.5397232,ダンス目あけ,頬０涙０",
			"33.3348764,ダンス目つむり,頬０涙０",
			"33.9644592,ダンス目あけ,頬０涙０",
			"34.6437668,ダンス微笑み,頬０涙０",
			"35.2400806,ダンス目つむり,頬０涙０",
			"35.7372438,ダンス目あけ,頬０涙０",
			"36.4825699,ダンス困り顔,頬０涙０",
			"37.675349,ダンス目あけ,頬０涙０",
			"39.0006831,ダンス微笑み,頬０涙０",
			"39.6633549,ダンスウインク,頬０涙０",
			"40.4254631,ダンス微笑み,頬０涙０",
			"41.5188785,ダンス目あけ,頬０涙０",
			"43.0264273,ダンス目つむり,頬０涙０",
			"43.5234626,ダンス目あけ,頬０涙０",
			"44.1693126,ダンス目つむり,頬０涙０",
			"44.666479,ダンス微笑み,頬０涙０",
			"45.2960253,ダンスウインク,頬０涙０",
			"45.7930458,ダンス目あけ,頬０涙０",
			"46.920305,ダンス目とじ,頬０涙０",
			"47.6816172,ダンス目あけ,頬０涙０",
			"48.1952127,ダンス微笑み,頬０涙０",
			"49.089586,ダンス目とじ,頬０涙０",
			"49.5868196,ダンス微笑み,頬０涙０",
			"50.0839213,ダンス目あけ,頬０涙０",
			"50.5974054,ダンス目とじ,頬０涙０",
			"51.0943186,ダンス目あけ,頬０涙０",
			"51.5911962,ダンスウインク,頬０涙０",
			"52.2374554,ダンス目あけ,頬０涙０",
			"52.9167011,ダンス目とじ,頬０涙０",
			"53.4301221,ダンス目あけ,頬０涙０",
			"54.1260639,ダンス微笑み,頬０涙０",
			"55.3188909,ダンス目あけ,頬０涙０",
			"55.815862,ダンス目とじ,頬０涙０",
			"56.6276111,ダンス目あけ,頬０涙０",
			"57.6217922,ダンス目つむり,頬０涙０",
			"58.1189703,ダンス目あけ,頬０涙０",
			"58.8976251,ダンス目つむり,頬０涙０",
			"61.4654354,ダンス目あけ,頬０涙０",
			"61.9789665,ダンス目とじ,頬０涙０",
			"62.6911583,ダンス目あけ,頬０涙０",
			"63.188183,ダンス微笑み,頬０涙０",
			"63.7847385,ダンス目あけ,頬０涙０",
			"64.2817604,ダンス目つむり,頬０涙０",
			"64.7953123,ダンス目あけ,頬０涙０",
			"65.2923202,ダンス目つむり,頬０涙０",
			"65.7891325,ダンス微笑み,頬０涙０",
			"66.5845052,ダンス目つむり,頬０涙０",
			"67.0815234,ダンス微笑み,頬０涙０",
			"67.8437065,ダンス目つむり,頬０涙０",
			"68.3405844,ダンス目あけ,頬０涙０",
			"69.3180142,ダンス目とじ,頬０涙０",
			"69.9308035,ダンス微笑み,頬０涙０",
			"71.0905043,ダンス目とじ,頬０涙０",
			"71.6870392,ダンス目あけ,頬０涙０",
			"72.4489373,ダンス目とじ,頬０涙０",
			"73.0621018,ダンス目あけ,頬０涙０",
			"73.691444,ダンス目つむり,頬０涙０",
			"74.1884704,ダンス目あけ,頬０涙０",
			"75.1165319,ダンス目つむり,頬０涙０",
			"75.6297727,ダンス目あけ,頬０涙０",
			"77.1376455,ダンス目とじ,頬０涙０",
			"77.7836463,ダンス目あけ,頬０涙０",
			"78.6781758,ダンス微笑み,頬０涙０",
			"79.8544445,ダンス目あけ,頬０涙０",
			"80.3348037,ダンス目つむり,頬０涙０",
			"80.8318518,ダンス目あけ,頬０涙０",
			"81.3951088,ダンス微笑み,頬０涙０",
			"82.0743899,ダンス目つむり,頬０涙０",
			"82.5711694,ダンス微笑み,頬０涙０",
			"83.0681756,ダンス目あけ,頬０涙０",
			"84.029034,ダンス目つむり,頬０涙０",
			"84.5262243,ダンス微笑み,頬０涙０",
			"86.1166747,ダンス目つむり,頬０涙０",
			"87.1437551,ダンス目あけ,頬０涙０",
			"87.6407576,ダンス目つむり,頬０涙０",
			"88.5353849,ダンス微笑み,頬０涙０",
			"89.7281617,ダンス目つむり,頬０涙０",
			"90.9704771,ダンス目あけ,頬０涙０",
			"92.5941946,ダンス微笑み,頬０涙０",
			"93.2237164,ダンス目あけ,頬０涙０",
			"93.936085,ダンス微笑み,頬０涙０",
			"94.5656607,ダンス目とじ,頬０涙０",
			"95.0792326,ダンス微笑み,頬０涙０",
			"95.5762157,ダンス目あけ,頬０涙０",
			"96.3216803,ダンスウインク,頬０涙０",
			"97.5476761,ダンス目あけ,頬０涙０",
			"98.1775922,ダンス微笑み,頬０涙０",
			"98.6741619,ダンス目とじ,頬０涙０",
			"100.165198,ダンス微笑み,頬０涙０",
			"100.6787072,ダンス目とじ,頬０涙０",
			"101.3745138,ダンス目あけ,頬０涙０",
			"102.0372215,ダンス目つむり,頬０涙０",
			"102.5341792,ダンス目あけ,頬０涙０",
			"104.6711332,ダンス目つむり,頬０涙０",
			"105.168321,ダンス目あけ,頬０涙０",
			"105.665281,ダンス目つむり,頬０涙０",
			"106.3112397,ダンス目あけ,頬０涙０",
			"106.9912939,ダンス微笑み,頬０涙０",
			"107.4876204,ダンス目あけ,頬０涙０",
			"107.9846223,ダンス目とじ,頬０涙０",
			"108.4816148,ダンス微笑み,頬０涙０",
			"108.9951958,ダンス目あけ,頬０涙０",
			"109.6247042,ダンス目つむり,頬０涙０",
			"110.1217301,ダンス目あけ,頬０涙０",
			"111.2956969,ダンス目とじ,頬０涙０",
			"112.0577312,ダンス微笑み,頬０涙０",
			"112.9854571,ダンス目あけ,頬０涙０",
			"113.4824068,ダンス目つむり,頬０涙０",
			"113.9960155,ダンス微笑み,頬０涙０",
			"115.1556789,ダンス目あけ,頬０涙０",
			"117.1103843,ダンス目とじ,頬０涙０",
			"117.8560177,ダンス目あけ,頬０涙０",
			"118.3530241,ダンス目とじ,頬０涙０",
			"118.9492295,ダンス目あけ,頬０涙０",
			"120.1919104,ダンス目とじ,頬０涙０",
			"120.6887327,ダンス目あけ,頬０涙０",
			"121.1859062,ダンス目つむり,頬０涙０",
			"121.6829463,ダンス目あけ,頬０涙０",
			"122.4282077,ダンス微笑み,頬０涙０",
			"122.9254967,ダンス目あけ,頬０涙０",
			"124.2673603,ダンス微笑み,頬０涙０",
			"125.2447887,ダンス目あけ,頬０涙０",
			"126.8351396,ダンス目つむり,頬０涙０",
			"127.3321805,ダンス目あけ,頬０涙０",
			"127.8457273,ダンス目つむり,頬０涙０",
			"128.3427557,ダンス目あけ,頬０涙０",
			"128.7403292,ダンス目あけ,頬１涙０",
			"129.6018595,ダンス目つむり,頬１涙０",
			"130.2976017,ダンス目あけ,頬１涙０",
			"131.6063447,ダンス目あけ,頬０涙０",
			"131.6890024,ダンス目とじ,頬０涙０",
			"132.1862019,ダンス微笑み,頬０涙０",
			"133.4285045,ダンス目あけ,頬０涙０",
			"133.9089336,ダンス微笑み,頬０涙０",
			"134.4061245,ダンス目あけ,頬０涙０",
			"135.3338284,ダンス目とじ,頬０涙０",
			"138.3159059,ダンス微笑み,頬０涙０",
			"138.8625191,ダンス目あけ,頬０涙０",
			"139.7737537,ダンス微笑み,頬０涙０",
			"141.5132044,ダンス目あけ,頬０涙０",
			"142.9048769,ダンス微笑み,頬０涙０",
			"144.2632617,ダンス目あけ,頬０涙０",
			"145.1908413,ダンス目つむり,頬０涙０",
			"145.6880405,ダンス目あけ,頬０涙０",
			"146.1850726,ダンス微笑み,頬０涙０",
			"147.2784122,ダンス目つむり,頬０涙０",
			"147.7754303,ダンス微笑み,頬０涙０",
			"149.1338639,ダンス目あけ,頬０涙０",
			"149.647464,ダンス微笑み,頬０涙０",
			"150.1444468,ダンス目あけ,頬０涙０",
			"150.6414992,ダンス微笑み,頬０涙０",
			"151.4366475,ダンス目あけ,頬０涙０",
			"151.9336882,ダンス目つむり,頬０涙０",
			"152.795099,ダンス目あけ,頬０涙０",
			"154.1204754,ダンス微笑み,頬０涙０",
			"156.7709551,ダンス目あけ,頬０涙０",
			"158.6928684,ダンス目とじ,頬０涙０",
			"159.7862487,ダンス目あけ,頬０涙０",
			"160.2834026,ダンス微笑み,頬０涙０",
			"161.2442541,ダンス目あけ,頬０涙０",
			"161.7410781,ダンス微笑み,頬０涙０",
			"162.4203407,ダンス目あけ,頬０涙０",
			"163.9941774,ダンス微笑み,頬０涙０",
			"164.7892306,ダンス目あけ,頬０涙０",
			"165.2865575,ダンス目つむり,頬０涙０",
			"166.7276813,ダンス目あけ,頬０涙０"
		};

		private string[] danceO1BArray = new string[]
		{
			"0.0248139,ダンス目あけ,頬０涙０",
			"6.1824899,ダンス微笑み,頬０涙０",
			"7.4414996,ダンス目あけ,頬０涙０",
			"8.8331327,ダンス目つむり,頬０涙０",
			"9.3299408,ダンス目あけ,頬０涙０",
			"10.6389036,ダンス目とじ,頬０涙０",
			"11.3856706,ダンス微笑み,頬０涙０",
			"12.3298085,ダンス目つむり,頬０涙０",
			"12.8270182,ダンス目あけ,頬０涙０",
			"13.3736819,ダンス微笑み,頬０涙０",
			"13.870576,ダンス目つむり,頬０涙０",
			"14.7321485,ダンス目あけ,頬０涙０",
			"15.9429394,ダンス目とじ,頬０涙０",
			"16.6220668,ダンス微笑み,頬０涙０",
			"17.8312742,ダンス目とじ,頬１涙０",
			"18.4941033,ダンス目あけ,頬１涙０",
			"19.1402099,ダンス微笑み,頬０涙０",
			"20.0182609,ダンス目つむり,頬０涙０",
			"20.515256,ダンス目あけ,頬０涙０",
			"21.0286294,ダンス目とじ,頬０涙０",
			"21.8901121,ダンス目あけ,頬０涙０",
			"23.5469787,ダンス目つむり,頬０涙０",
			"24.0446774,ダンス目あけ,頬０涙０",
			"24.2924556,ダンス目あけ,頬１涙０",
			"24.5409982,ダンス目とじ,頬１涙０",
			"24.9551847,ダンス目とじ,頬０涙０",
			"25.2202459,ダンス微笑み,頬０涙０",
			"26.4793617,ダンス目つむり,頬０涙０",
			"27.0921995,ダンス目あけ,頬０涙０",
			"27.7383443,ダンス目とじ,頬０涙０",
			"28.2874245,ダンス微笑み,頬０涙０",
			"29.4968214,ダンス目とじ,頬０涙０",
			"29.9882841,ダンス目あけ,頬０涙０",
			"30.6846242,ダンス目つむり,頬０涙０",
			"31.4295839,ダンス微笑み,頬０涙０",
			"32.0427002,ダンス目つむり,頬０涙０",
			"32.5397332,ダンス目あけ,頬０涙０",
			"33.334885,ダンス目つむり,頬０涙０",
			"33.9644692,ダンス目あけ,頬０涙０",
			"34.6437837,ダンス微笑み,頬０涙０",
			"35.2400908,ダンス目つむり,頬０涙０",
			"35.7372515,ダンス目あけ,頬０涙０",
			"36.4825782,ダンス困り顔,頬０涙０",
			"37.6753564,ダンス目あけ,頬０涙０",
			"39.0006925,ダンス微笑み,頬０涙０",
			"40.6903475,ダンスウインク,頬０涙０",
			"41.5188887,ダンス目あけ,頬０涙０",
			"43.0264379,ダンス目つむり,頬０涙０",
			"43.5234723,ダンス目あけ,頬０涙０",
			"44.1693246,ダンス目つむり,頬０涙０",
			"44.6664887,ダンス微笑み,頬０涙０",
			"45.2960347,ダンスウインク,頬０涙０",
			"45.7930557,ダンス目あけ,頬０涙０",
			"46.2899074,ダンス微笑み,頬０涙０",
			"47.4331334,ダンス目つむり,頬０涙０",
			"48.5097875,ダンス微笑み,頬０涙０",
			"49.2884095,ダンス目あけ,頬０涙０",
			"49.785597,ダンス目つむり,頬０涙０",
			"50.2825883,ダンス目あけ,頬０涙０",
			"51.8398565,ダンス目とじ,頬０涙０",
			"52.8182742,ダンス目あけ,頬０涙０",
			"53.6456927,ダンスウインク,頬０涙０",
			"54.2255704,ダンス目あけ,頬０涙０",
			"55.8158714,ダンス目とじ,頬０涙０",
			"56.6276205,ダンス微笑み,頬０涙０",
			"57.6218019,ダンス目つむり,頬０涙０",
			"58.1189806,ダンス目あけ,頬０涙０",
			"59.0467478,ダンス目つむり,頬０涙０",
			"59.5437095,ダンス目あけ,頬０涙０",
			"60.6370921,ダンス目つむり,頬０涙０",
			"61.134062,ダンス目あけ,頬０涙０",
			"62.194362,ダンス目とじ,頬０涙０",
			"62.6911672,ダンス目あけ,頬０涙０",
			"63.1881936,ダンス微笑み,頬０涙０",
			"63.7847502,ダンス目あけ,頬０涙０",
			"64.2817704,ダンス目つむり,頬０涙０",
			"64.7953234,ダンス目あけ,頬０涙０",
			"65.2923313,ダンス目つむり,頬０涙０",
			"65.7891433,ダンス微笑み,頬０涙０",
			"66.5845138,ダンス目つむり,頬０涙０",
			"67.0815322,ダンス微笑み,頬０涙０",
			"67.8437305,ダンス目つむり,頬０涙０",
			"68.3405941,ダンス目あけ,頬０涙０",
			"69.3180242,ダンス目とじ,頬０涙０",
			"69.9308141,ダンス微笑み,頬０涙０",
			"71.0905162,ダンス目とじ,頬０涙０",
			"71.6870489,ダンス目あけ,頬０涙０",
			"72.4489467,ダンス目とじ,頬０涙０",
			"73.0621124,ダンス目あけ,頬０涙０",
			"73.6914537,ダンス目つむり,頬０涙０",
			"74.188481,ダンス目あけ,頬０涙０",
			"75.1165544,ダンス目つむり,頬０涙０",
			"75.6297844,ダンス目あけ,頬０涙０",
			"77.1376572,ダンス目とじ,頬０涙０",
			"77.7836569,ダンス目あけ,頬０涙０",
			"77.8664603,ダンス目あけ,頬１涙０",
			"78.678186,ダンス微笑み,頬１涙０",
			"79.771466,ダンス微笑み,頬０涙０",
			"79.8544545,ダンス目あけ,頬０涙０",
			"80.3348131,ダンス目つむり,頬０涙０",
			"80.8318623,ダンス目あけ,頬０涙０",
			"81.3951191,ダンス微笑み,頬０涙０",
			"82.0744004,ダンス目つむり,頬０涙０",
			"82.5711794,ダンス微笑み,頬０涙０",
			"83.0681855,ダンス目あけ,頬０涙０",
			"84.029044,ダンス目つむり,頬０涙０",
			"84.5262346,ダンス微笑み,頬０涙０",
			"86.1166875,ダンス目つむり,頬０涙０",
			"87.1437648,ダンス目あけ,頬０涙０",
			"87.6407684,ダンス目つむり,頬０涙０",
			"88.5353957,ダンス微笑み,頬０涙０",
			"89.7281722,ダンス目つむり,頬０涙０",
			"90.9704874,ダンス目あけ,頬０涙０",
			"91.9313332,ダンス目とじ,頬０涙０",
			"92.44517,ダンス微笑み,頬０涙０",
			"93.0910043,ダンス目つむり,頬０涙０",
			"93.8696394,ダンス微笑み,頬０涙０",
			"94.5656713,ダンス目とじ,頬０涙０",
			"95.0792431,ダンス微笑み,頬０涙０",
			"95.5762248,ダンス目あけ,頬０涙０",
			"96.3216906,ダンスウインク,頬０涙０",
			"97.5476866,ダンス目あけ,頬０涙０",
			"98.1776044,ダンス微笑み,頬０涙０",
			"98.6741707,ダンス目とじ,頬０涙０",
			"99.5685773,ダンス微笑み,頬０涙０",
			"100.0657437,ダンス目とじ,頬０涙０",
			"101.0432636,ダンス目あけ,頬０涙０",
			"101.9213009,ダンス微笑み,頬０涙０",
			"102.4182509,ダンス目とじ,頬０涙０",
			"103.8926635,ダンス目あけ,頬０涙０",
			"104.6711452,ダンス目つむり,頬０涙０",
			"105.1683324,ダンス目あけ,頬０涙０",
			"105.6652918,ダンス目つむり,頬０涙０",
			"106.3112525,ダンス目あけ,頬０涙０",
			"106.9913179,ダンス微笑み,頬０涙０",
			"107.4876316,ダンス目あけ,頬０涙０",
			"107.9846332,ダンス目とじ,頬０涙０",
			"108.4816268,ダンス微笑み,頬０涙０",
			"108.9952069,ダンス目あけ,頬０涙０",
			"109.6247136,ダンス目つむり,頬０涙０",
			"110.1217409,ダンス目あけ,頬０涙０",
			"111.295708,ダンス目とじ,頬０涙０",
			"112.0577412,ダンス微笑み,頬０涙０",
			"112.9854676,ダンス目あけ,頬０涙０",
			"113.4824171,ダンス目つむり,頬０涙０",
			"113.9960257,ダンス微笑み,頬０涙０",
			"115.1556891,ダンス目あけ,頬０涙０",
			"117.1103977,ダンス目とじ,頬０涙０",
			"117.8560271,ダンス目あけ,頬０涙０",
			"118.3530352,ダンス目とじ,頬０涙０",
			"118.9492398,ダンス目あけ,頬０涙０",
			"120.191921,ダンス目とじ,頬０涙０",
			"120.6887438,ダンス目あけ,頬０涙０",
			"121.1859171,ダンス目つむり,頬０涙０",
			"121.6829577,ダンス目あけ,頬０涙０",
			"122.428218,ダンス微笑み,頬０涙０",
			"122.9255181,ダンス目あけ,頬０涙０",
			"124.2673711,ダンス微笑み,頬０涙０",
			"125.2448129,ダンス目あけ,頬０涙０",
			"126.8351504,ダンス目つむり,頬０涙０",
			"127.3321911,ダンス目あけ,頬０涙０",
			"127.8457381,ダンス目つむり,頬０涙０",
			"128.3427677,ダンス目あけ,頬０涙０",
			"128.7403394,ダンス目あけ,頬１涙０",
			"129.6018712,ダンス目つむり,頬１涙０",
			"130.2976133,ダンス目あけ,頬１涙０",
			"131.6063555,ダンス目あけ,頬０涙０",
			"131.6890147,ダンス目とじ,頬０涙０",
			"132.1862122,ダンス微笑み,頬０涙０",
			"133.4285153,ダンス目あけ,頬０涙０",
			"133.9089441,ダンス微笑み,頬０涙０",
			"134.4061359,ダンス目あけ,頬０涙０",
			"135.6817449,ダンス微笑み,頬０涙０",
			"138.8625293,ダンス目あけ,頬０涙０",
			"139.7737653,ダンス微笑み,頬０涙０",
			"141.5132155,ダンス目あけ,頬０涙０",
			"142.9048871,ダンス微笑み,頬０涙０",
			"144.2632731,ダンス目あけ,頬０涙０",
			"145.1908544,ダンス目つむり,頬０涙０",
			"145.6880519,ダンス目あけ,頬０涙０",
			"146.1850843,ダンス微笑み,頬０涙０",
			"147.2784227,ダンス目つむり,頬０涙０",
			"147.7754409,ダンス微笑み,頬０涙０",
			"149.133875,ダンス目あけ,頬０涙０",
			"149.6474748,ダンス微笑み,頬０涙０",
			"150.1444679,ダンス目あけ,頬０涙０",
			"150.6415091,ダンス微笑み,頬０涙０",
			"151.4366595,ダンス目あけ,頬０涙０",
			"151.9337101,ダンス目つむり,頬０涙０",
			"152.7951107,ダンス目あけ,頬０涙０",
			"154.1204873,ダンス微笑み,頬０涙０",
			"156.7709659,ダンス目あけ,頬０涙０",
			"158.6928795,ダンス目とじ,頬０涙０",
			"159.7862607,ダンス目あけ,頬０涙０",
			"160.2834245,ダンス微笑み,頬０涙０",
			"161.2442652,ダンス目あけ,頬０涙０",
			"161.7410898,ダンス微笑み,頬０涙０",
			"162.4203518,ダンス目あけ,頬０涙０",
			"163.9941894,ダンス微笑み,頬０涙０",
			"164.7892414,ダンス目あけ,頬０涙０",
			"165.2865697,ダンス目つむり,頬０涙０",
			"166.7276927,ダンス目あけ,頬０涙０"
		};

		private string[] danceO1CArray = new string[]
		{
			"0.025054,ダンス目あけ,頬０涙０",
			"5.0890651,ダンス目つむり,頬０涙０",
			"6.1826635,ダンス微笑み,頬０涙０",
			"7.4418876,ダンス目あけ,頬０涙０",
			"8.8334954,ダンス目つむり,頬０涙０",
			"9.3303063,ダンス目あけ,頬０涙０",
			"10.6392965,ダンス目とじ,頬０涙０",
			"11.3860603,ダンス微笑み,頬０涙０",
			"12.3301757,ダンス目つむり,頬０涙０",
			"12.8273891,ダンス目あけ,頬０涙０",
			"13.3740679,ダンス微笑み,頬０涙０",
			"13.8708996,ダンス目つむり,頬０涙０",
			"14.7325268,ダンス目あけ,頬０涙０",
			"15.9433089,ダンス目とじ,頬０涙０",
			"16.622414,ダンス微笑み,頬０涙０",
			"17.8314957,ダンス目とじ,頬１涙０",
			"18.4944876,ダンス目あけ,頬１涙０",
			"19.1406037,ダンス微笑み,頬０涙０",
			"20.0186142,ダンス目つむり,頬０涙０",
			"20.5154863,ダンス目あけ,頬０涙０",
			"21.0290032,ダンス目とじ,頬０涙０",
			"21.8905013,ダンス目あけ,頬０涙０",
			"22.9840142,ダンス目つむり,頬０涙０",
			"23.8952268,ダンス目あけ,頬０涙０",
			"24.2928519,ダンス目あけ,頬１涙０",
			"24.4914017,ダンス真剣,頬１涙０",
			"24.9555616,ダンス真剣,頬０涙０",
			"24.9886414,ダンスウインク,頬０涙０",
			"25.7009732,ダンス目あけ,頬０涙０",
			"26.8770877,ダンス微笑み,頬０涙０",
			"27.6392782,ダンス目とじ,頬０涙０",
			"28.3704223,ダンス目あけ,頬０涙０",
			"28.9338321,ダンスウインク,頬０涙０",
			"29.5300803,ダンス真剣,頬０涙０",
			"30.1386205,ダンス微笑み,頬０涙０",
			"30.684887,ダンス目つむり,頬０涙０",
			"31.4299742,ダンス微笑み,頬０涙０",
			"32.0428607,ダンス目つむり,頬０涙０",
			"32.5400873,ダンス目あけ,頬０涙０",
			"33.3350894,ダンス目つむり,頬０涙０",
			"33.9648205,ダンス目あけ,頬０涙０",
			"34.6440571,ダンス微笑み,頬０涙０",
			"35.2404882,ダンス目つむり,頬０涙０",
			"35.7376826,ダンス目あけ,頬０涙０",
			"36.4829514,ダンス困り顔,頬０涙０",
			"37.6756621,ダンス目あけ,頬０涙０",
			"39.0010617,ダンス微笑み,頬０涙０",
			"39.6637372,ダンス目あけ,頬０涙０",
			"40.6907564,ダンスウインク,頬０涙０",
			"41.5192859,ダンス目あけ,頬０涙０",
			"43.0268384,ダンス目つむり,頬０涙０",
			"43.5238563,ダンス目あけ,頬０涙０",
			"44.1696322,ダンス目つむり,頬０涙０",
			"44.6668964,ダンス微笑み,頬０涙０",
			"45.2963914,ダンスウインク,頬０涙０",
			"45.7934381,ダンス目あけ,頬０涙０",
			"46.9206483,ダンス目とじ,頬０涙０",
			"47.6820172,ダンス目あけ,頬０涙０",
			"48.1956011,ダンス微笑み,頬０涙０",
			"49.0897861,ダンス目とじ,頬０涙０",
			"49.5872,ダンス微笑み,頬０涙０",
			"50.0842914,ダンス目あけ,頬０涙０",
			"50.5977849,ダンス目とじ,頬０涙０",
			"51.0946758,ダンス目あけ,頬０涙０",
			"51.906545,ダンス目とじ,頬０涙０",
			"52.2707926,ダンス目とじ,頬１涙０",
			"52.734841,ダンス目あけ,頬１涙０",
			"53.6292615,ダンスウインク,頬１涙０",
			"53.8612584,ダンスウインク,頬０涙０",
			"54.4080475,ダンス目あけ,頬０涙０",
			"55.2364364,ダンス微笑み,頬０涙０",
			"55.9321575,ダンス目とじ,頬０涙０",
			"56.9094425,ダンス目あけ,頬０涙０",
			"57.3898872,ダンス微笑み,頬０涙０",
			"57.8872472,ダンス目あけ,頬０涙０",
			"58.384423,ダンス微笑み,頬０涙０",
			"58.8978517,ダンス目つむり,頬０涙０",
			"61.4658833,ダンス目あけ,頬０涙０",
			"61.9791618,ダンス目とじ,頬０涙０",
			"62.6915241,ダンス目あけ,頬０涙０",
			"63.1885905,ダンス微笑み,頬０涙０",
			"63.2881403,ダンス微笑み,頬１涙０",
			"63.7851191,ダンス目あけ,頬１涙０",
			"64.2821216,ダンス目つむり,頬１涙０",
			"64.679739,ダンス目つむり,頬０涙０",
			"64.7956527,ダンス目あけ,頬０涙０",
			"65.2927142,ダンス目つむり,頬０涙０",
			"65.7893797,ダンス微笑み,頬０涙０",
			"66.5848679,ダンス目つむり,頬０涙０",
			"67.0819253,ダンス微笑み,頬０涙０",
			"67.8440942,ダンス目つむり,頬０涙０",
			"68.3408304,ダンス目あけ,頬０涙０",
			"69.3184079,ダンス目とじ,頬０涙０",
			"69.9310119,ダンス微笑み,頬０涙０",
			"71.090943,ダンス目とじ,頬０涙０",
			"71.6874124,ダンス目あけ,頬０涙０",
			"72.4493302,ダンス目とじ,頬０涙０",
			"73.0622523,ダンス目あけ,頬０涙０",
			"73.6918423,ダンス目つむり,頬０涙０",
			"74.1888696,ダンス目あけ,頬０涙０",
			"75.1167973,ダンス目つむり,頬０涙０",
			"75.6300322,ダンス目あけ,頬０涙０",
			"77.1378399,ダンス目とじ,頬０涙０",
			"77.7840437,ダンス目あけ,頬０涙０",
			"78.678482,ダンス微笑み,頬０涙０",
			"79.8546666,ダンス目あけ,頬０涙０",
			"80.335186,ダンス目つむり,頬０涙０",
			"80.8322156,ダンス目あけ,頬０涙０",
			"81.3955196,ダンス微笑み,頬０涙０",
			"82.0746083,ダンス目つむり,頬０涙０",
			"82.5714998,ダンス微笑み,頬０涙０",
			"83.0685602,ダンス目あけ,頬０涙０",
			"84.029288,ダンス目つむり,頬０涙０",
			"84.5264567,ダンス微笑み,頬０涙０",
			"86.117059,ダンス目つむり,頬０涙０",
			"87.1440941,ダンス目あけ,頬０涙０",
			"87.641165,ダンス目つむり,頬０涙０",
			"88.535711,ダンス微笑み,頬０涙０",
			"89.7285503,ダンス目つむり,頬０涙０",
			"90.9708899,ダンス目あけ,頬０涙０",
			"91.9316868,ダンス目とじ,頬０涙０",
			"92.4455383,ダンス微笑み,頬０涙０",
			"93.0913618,ダンス目つむり,頬０涙０",
			"93.8699932,ダンス微笑み,頬０涙０",
			"94.4830797,ダンス目つむり,頬０涙０",
			"95.5267535,ダンス憂い,頬０涙０",
			"97.1668904,ダンス目あけ,頬０涙０",
			"97.6805646,ダンスジト目,頬０涙０",
			"98.6743512,ダンス目とじ,頬０涙０",
			"100.1655587,ダンス微笑み,頬０涙０",
			"100.6790624,ダンス目とじ,頬０涙０",
			"101.3749132,ダンス目あけ,頬０涙０",
			"102.037622,ダンス目つむり,頬０涙０",
			"103.329624,ダンス目あけ,頬０涙０",
			"104.6715312,ダンス目つむり,頬０涙０",
			"105.168518,ダンス目あけ,頬０涙０",
			"105.6656414,ダンス目つむり,頬０涙０",
			"106.3116799,ダンス目あけ,頬０涙０",
			"106.9915206,ダンス微笑み,頬０涙０",
			"107.4879902,ダンス目あけ,頬０涙０",
			"107.9849841,ダンス目とじ,頬０涙０",
			"108.4819812,ダンス微笑み,頬０涙０",
			"108.9954912,ダンス目あけ,頬０涙０",
			"109.6250635,ダンス目つむり,頬０涙０",
			"110.1221093,ダンス目あけ,頬０涙０",
			"111.2960667,ダンス目とじ,頬０涙０",
			"112.0579049,ダンス微笑み,頬０涙０",
			"112.9858725,ダンス目あけ,頬０涙０",
			"113.482831,ダンス目つむり,頬０涙０",
			"113.99642,ダンス微笑み,頬０涙０",
			"115.1560244,ダンス目あけ,頬０涙０",
			"117.1109393,ダンス目とじ,頬０涙０",
			"117.8562286,ダンス目あけ,頬０涙０",
			"118.3533682,ダンス目とじ,頬０涙０",
			"118.9494043,ダンス目あけ,頬０涙０",
			"120.1921417,ダンス目とじ,頬０涙０",
			"120.6889354,ダンス目あけ,頬０涙０",
			"121.1862886,ダンス目つむり,頬０涙０",
			"121.6832297,ダンス目あけ,頬０涙０",
			"122.4285798,ダンス微笑み,頬０涙０",
			"122.9258422,ダンス目あけ,頬０涙０",
			"124.2677634,ダンス微笑み,頬０涙０",
			"125.2454635,ダンス目あけ,頬０涙０",
			"126.8353799,ダンス目つむり,頬０涙０",
			"127.3325665,ダンス目あけ,頬０涙０",
			"127.84607,ダンス目つむり,頬０涙０",
			"128.3431397,ダンス目あけ,頬０涙０",
			"128.7407417,ダンス目あけ,頬１涙０",
			"129.6022398,ダンス目つむり,頬１涙０",
			"130.2977833,ダンス目あけ,頬１涙０",
			"131.6067088,ダンス目あけ,頬０涙０",
			"131.6893856,ダンス目とじ,頬０涙０",
			"132.1866278,ダンス微笑み,頬０涙０",
			"133.4288865,ダンス目あけ,頬０涙０",
			"133.9093136,ダンス微笑み,頬０涙０",
			"134.4065193,ダンス目あけ,頬０涙０",
			"135.3340063,ダンス目とじ,頬０涙０",
			"138.3162714,ダンス微笑み,頬０涙０",
			"138.8629256,ダンス目あけ,頬０涙０",
			"139.7741899,ダンス微笑み,頬０涙０",
			"141.5135796,ダンス目あけ,頬０涙０",
			"142.9052455,ダンス微笑み,頬０涙０",
			"144.2636246,ダンス目あけ,頬０涙０",
			"145.1910657,ダンス目つむり,頬０涙０",
			"145.6884314,ダンス目あけ,頬０涙０",
			"146.1855228,ダンス微笑み,頬０涙０",
			"147.2787785,ダンス目つむり,頬０涙０",
			"147.7757858,ダンス微笑み,頬０涙０",
			"149.1342653,ダンス目あけ,頬０涙０",
			"149.6478081,ダンス微笑み,頬０涙０",
			"150.1448787,ダンス目あけ,頬０涙０",
			"150.6419051,ダンス微笑み,頬０涙０",
			"151.4370124,ダンス目あけ,頬０涙０",
			"151.9340799,ダンス目つむり,頬０涙０",
			"152.7954865,ダンス目あけ,頬０涙０",
			"154.1208494,ダンス微笑み,頬０涙０",
			"156.7713374,ダンス目あけ,頬０涙０",
			"158.6932729,ダンス目とじ,頬０涙０",
			"159.7866285,ダンス目あけ,頬０涙０",
			"160.2836594,ダンス微笑み,頬０涙０",
			"161.2460771,ダンス目あけ,頬０涙０",
			"161.7414584,ダンス微笑み,頬０涙０",
			"162.4205836,ダンス目あけ,頬０涙０",
			"163.9945036,ダンス微笑み,頬０涙０",
			"164.7896303,ダンス目あけ,頬０涙０",
			"165.2867565,ダンス目つむり,頬０涙０",
			"166.7278712,ダンス目あけ,頬０涙０"
		};

		private string[] danceO2Array = new string[]
		{
			"0.0297738,ダンス目つむり,頬０涙０",
			"5.1953901,ダンス微笑み,頬０涙０",
			"6.6035102,ダンス目あけ,頬０涙０",
			"7.6803987,ダンスウインク,頬０涙０",
			"8.7738004,ダンスびっくり,頬０涙０",
			"9.5033579,ダンス目あけ,頬０涙０",
			"10.48015,ダンスウインク,頬０涙０",
			"11.8220275,ダンス目あけ,頬０涙０",
			"13.0478557,ダンス微笑み,頬０涙０",
			"13.9776528,ダンス目とじ,頬０涙０",
			"15.1868901,ダンス微笑み,頬０涙０",
			"15.9655996,ダンス目あけ,頬０涙０",
			"16.4957589,ダンス目つむり,頬０涙０",
			"17.2744539,ダンス目あけ,頬０涙０",
			"18.3015551,ダンス微笑み,頬０涙０",
			"19.5771958,ダンス目とじ,頬１涙０",
			"20.7036819,ダンス目あけ,頬１涙０",
			"20.8363244,ダンス目あけ,頬０涙０",
			"21.2007505,ダンス目つむり,頬０涙０",
			"21.8467029,ダンス目あけ,頬０涙０",
			"22.9899323,ダンス微笑み,頬０涙０",
			"24.5968705,ダンス目つむり,頬０涙０",
			"25.1268915,ダンス目あけ,頬０涙０",
			"25.6240266,ダンス微笑み,頬０涙０",
			"26.2865398,ダンスウインク,頬０涙０",
			"26.8996273,ダンス目あけ,頬０涙０",
			"28.5397437,ダンスウインク,頬１涙０",
			"29.3846391,ダンス微笑み,頬０涙０",
			"30.8448255,ダンスウインク,頬０涙０",
			"31.59048,ダンス目あけ,頬０涙０",
			"32.8496255,ダンス困り顔,頬０涙０",
			"33.5452898,ダンス目つむり,頬０涙０",
			"34.2411796,ダンス微笑み,頬０涙０",
			"35.5926764,ダンス目あけ,頬０涙０",
			"36.089702,ダンス目とじ,頬０涙０",
			"37.0008605,ダンス目あけ,頬０涙０",
			"37.4978324,ダンス目つむり,頬０涙０",
			"38.2102056,ダンス微笑み,頬０涙０",
			"39.0387168,ダンスウインク,頬１涙０",
			"39.9166344,ダンス微笑み,頬０涙０",
			"40.9437704,ダンス目つむり,頬０涙０",
			"41.606258,ダンスびっくり,頬０涙０",
			"42.9151228,ダンス目つむり,頬０涙０",
			"43.544705,ダンス目あけ,頬０涙０",
			"44.1410692,ダンス目つむり,頬０涙０",
			"44.6876719,ダンス目あけ,頬０涙０",
			"45.4498314,ダンス微笑み,頬０涙０",
			"46.0297353,ダンス目とじ,頬０涙０",
			"46.940788,ダンス目あけ,頬０涙０",
			"47.7360521,ダンス微笑み,頬０涙０",
			"48.2330948,ダンス目あけ,頬０涙０",
			"48.7303324,ダンス目とじ,頬０涙０",
			"49.2269386,ダンス目あけ,頬０涙０",
			"50.171336,ダンス微笑み,頬０涙０",
			"52.8881484,ダンス目あけ,頬０涙０",
			"53.3851768,ダンス目つむり,頬０涙０",
			"53.8825331,ダンス微笑み,頬０涙０",
			"55.075063,ダンス目つむり,頬０涙０",
			"55.588638,ダンス目あけ,頬０涙０",
			"56.0855216,ダンス目つむり,頬０涙０",
			"56.5829109,ダンス目あけ,頬０涙０",
			"58.0072809,ダンス微笑み,頬０涙０",
			"58.6038002,ダンス目とじ,頬０涙０",
			"59.1668881,ダンス微笑み,頬０涙０",
			"59.697205,ダンス目とじ,頬０涙０",
			"60.3929971,ダンス目あけ,頬０涙０",
			"61.0390655,ダンスウインク,頬０涙０",
			"61.7348684,ダンス目あけ,頬０涙０",
			"62.4140939,ダンス微笑み,頬０涙０",
			"63.0436964,ダンス目あけ,頬０涙０",
			"63.5572089,ダンス微笑み,頬０涙０",
			"64.6506087,ダンス目あけ,頬１涙０",
			"65.710873,ダンス微笑み,頬０涙０",
			"66.8208555,ダンスウインク,頬０涙０",
			"67.4006956,ダンス目あけ,頬０涙０",
			"67.9308015,ダンス目とじ,頬０涙０",
			"68.5106351,ダンス微笑み,頬０涙０",
			"69.4383778,ダンス目つむり,頬０涙０",
			"70.0513453,ダンス困り顔,頬０涙０",
			"71.111615,ダンス目あけ,頬０涙０",
			"71.7079079,ダンスウインク,頬０涙０",
			"72.5198056,ダンス目あけ,頬０涙０",
			"73.0167616,ダンス目とじ,頬０涙０",
			"73.5800371,ダンス目あけ,頬０涙０",
			"74.1102206,ダンス微笑み,頬０涙０",
			"75.7337211,ダンス目つむり,頬０涙０",
			"76.6282024,ダンス微笑み,頬０涙０",
			"77.3241262,ダンスウインク,頬０涙０",
			"77.9039526,ダンス目あけ,頬０涙０",
			"78.4506701,ダンス目とじ,頬１涙０",
			"79.0305,ダンス微笑み,頬０涙０",
			"79.9416995,ダンス目つむり,頬０涙０",
			"80.5380788,ダンス目とじ,頬０涙０",
			"81.6315671,ダンス目あけ,頬０涙０",
			"82.3437949,ダンス微笑み,頬０涙０",
			"83.1224383,ダンス目とじ,頬０涙０",
			"83.8182397,ダンス目あけ,頬０涙０",
			"84.4974547,ダンス目つむり,頬０涙０",
			"85.0607259,ダンス目あけ,頬０涙０",
			"85.657134,ダンス目とじ,頬０涙０",
			"86.3198229,ダンス目あけ,頬０涙０",
			"87.0487822,ダンス微笑み,頬０涙０",
			"88.6391343,ダンス目とじ,頬０涙０",
			"89.2190046,ダンス微笑み,頬０涙０",
			"89.9313427,ダンス目つむり,頬０涙０",
			"90.5276125,ダンス目あけ,頬０涙０",
			"91.5217696,ダンス目とじ,頬１涙０",
			"92.3169742,ダンス目あけ,頬０涙０",
			"92.8636834,ダンス目つむり,頬０涙０",
			"93.4268082,ダンス目あけ,頬０涙０",
			"94.2718586,ダンス目とじ,頬０涙０",
			"94.8350842,ダンス微笑み,頬０涙０",
			"96.2929389,ダンス目あけ,頬０涙０",
			"96.8894048,ダンスウインク,頬０涙０",
			"97.7342916,ダンス微笑み,頬０涙０",
			"98.6785316,ダンス目あけ,頬０涙０",
			"100.5837092,ダンス微笑み,頬０涙０",
			"102.0581951,ダンス目つむり,頬０涙０",
			"102.8533375,ダンス目あけ,頬０涙０",
			"103.880492,ダンス目つむり,頬０涙０",
			"104.4768816,ダンス目あけ,頬０涙０",
			"105.5040273,ダンスびっくり,頬０涙０",
			"106.0010358,ダンス目つむり,頬０涙０",
			"106.4980106,ダンス微笑み,頬０涙０",
			"107.7239143,ダンス目つむり,頬０涙０",
			"108.3203751,ダンス目あけ,頬０涙０",
			"109.050084,ダンス目つむり,頬０涙０",
			"109.5793837,ダンス微笑み,頬０涙０",
			"110.8052375,ダンス目とじ,頬０涙０",
			"111.4017736,ダンスびっくり,頬０涙０",
			"112.1471176,ダンス目とじ,頬０涙０",
			"113.1917947,ダンス微笑み,頬０涙０",
			"114.2843579,ダンス目あけ,頬０涙０",
			"115.3419637,ダンス目とじ,頬０涙０",
			"115.8886621,ダンス微笑み,頬０涙０",
			"116.4850848,ダンス目あけ,頬０涙０",
			"117.1974533,ダンス困り顔,頬０涙０",
			"118.5890069,ダンス目つむり,頬０涙０",
			"119.1031085,ダンス微笑み,頬０涙０",
			"119.7320926,ダンス目あけ,頬０涙０",
			"120.3616524,ダンス目つむり,頬０涙０",
			"120.891782,ダンス目あけ,頬０涙０",
			"121.388781,ダンス微笑み,頬０涙０",
			"121.9520406,ダンス目あけ,頬０涙０",
			"122.5815641,ダンス目とじ,頬０涙０",
			"124.8014844,ダンス目あけ,頬０涙０",
			"126.1267162,ダンスウインク,頬０涙０",
			"126.6901217,ダンス目あけ,頬０涙０",
			"127.253395,ダンス目とじ,頬１涙０",
			"127.8331932,ダンス微笑み,頬０涙０",
			"128.7444124,ダンス目つむり,頬０涙０",
			"129.3407914,ダンス目とじ,頬０涙０",
			"130.3513381,ダンスびっくり,頬０涙０",
			"131.1464126,ダンス微笑み,頬０涙０",
			"131.8256839,ダンス目とじ,頬０涙０",
			"132.5048858,ダンス目あけ,頬０涙０",
			"133.2836611,ダンス目つむり,頬０涙０",
			"133.7806322,ダンス目あけ,頬０涙０",
			"134.4764311,ダンス微笑み,頬０涙０",
			"135.0397406,ダンス目あけ,頬０涙０",
			"135.6195394,ダンス微笑み,頬０涙０",
			"137.2430821,ダンスウインク,頬０涙０",
			"137.9388576,ダンス微笑み,頬０涙０",
			"138.8997377,ダンス目つむり,頬０涙０",
			"139.8109121,ダンス目あけ,頬０涙０",
			"140.3410711,ダンスウインク,頬１涙０",
			"141.1196922,ダンス目あけ,頬０涙０",
			"141.7326922,ダンス目つむり,頬０涙０",
			"142.2793177,ダンス微笑み,頬０涙０",
			"142.9917547,ダンス目とじ,頬０涙０",
			"143.7040836,ダンス微笑み,頬０涙０",
			"144.2010784,ダンス目つむり,頬０涙０",
			"144.7146294,ダンス目あけ,頬０涙０",
			"145.4766897,ダンスウインク,頬１涙０",
			"146.0565803,ダンス目あけ,頬１涙０",
			"146.3049193,ダンス目あけ,頬０涙０",
			"147.6304179,ダンス目つむり,頬０涙０",
			"148.1272519,ダンス目あけ,頬０涙０",
			"150.2976615,ダンス微笑み,頬０涙０",
			"150.8774554,ダンス目あけ,頬０涙０",
			"152.9814497,ダンス目つむり,頬０涙０",
			"153.5943925,ダンス目あけ,頬０涙０",
			"154.4392713,ダンス微笑み,頬０涙０",
			"156.12907,ダンスウインク,頬０涙０",
			"156.7917081,ダンス目あけ,頬０涙０",
			"157.4710162,ダンス目とじ,頬０涙０",
			"158.4484187,ダンス目あけ,頬０涙０",
			"159.8234713,ダンス目とじ,頬０涙０"
		};

		private string[] danceO2BArray = new string[]
		{
			"0.0298673,ダンス目つむり,頬０涙０",
			"5.1953992,ダンス微笑み,頬０涙０",
			"5.841483,ダンス目あけ,頬０涙０",
			"7.6804064,ダンスウインク,頬０涙０",
			"8.7738081,ダンスびっくり,頬０涙０",
			"9.2872215,ダンス目あけ,頬０涙０",
			"10.4801577,ダンスウインク,頬０涙０",
			"11.1261112,ダンス目あけ,頬０涙０",
			"13.0478634,ダンス微笑み,頬０涙０",
			"13.9776608,ダンス目とじ,頬０涙０",
			"15.1869001,ダンス微笑み,頬０涙０",
			"15.9656105,ダンス目あけ,頬０涙０",
			"16.4957711,ダンス目つむり,頬０涙０",
			"17.2744618,ダンス目あけ,頬０涙０",
			"18.3015694,ダンス微笑み,頬０涙０",
			"19.5772044,ダンス目とじ,頬１涙０",
			"20.7036902,ダンス目あけ,頬１涙０",
			"20.8363327,ダンス目あけ,頬０涙０",
			"21.2007588,ダンス目つむり,頬０涙０",
			"21.8467129,ダンス目あけ,頬０涙０",
			"22.989942,ダンス微笑み,頬０涙０",
			"24.5968916,ダンス目つむり,頬０涙０",
			"25.1269006,ダンス目あけ,頬０涙０",
			"25.6240365,ダンス微笑み,頬０涙０",
			"26.2865475,ダンスウインク,頬０涙０",
			"26.8996353,ダンス目あけ,頬０涙０",
			"28.5397517,ダンス目とじ,頬１涙０",
			"29.3846476,ダンス微笑み,頬０涙０",
			"31.0270964,ダンス真剣,頬０涙０",
			"32.1040436,ダンス微笑み,頬０涙０",
			"33.0481964,ダンス目つむり,頬０涙０",
			"33.5452986,ダンス目あけ,頬０涙０",
			"34.2411905,ダンス微笑み,頬０涙０",
			"35.592689,ダンス目あけ,頬０涙０",
			"36.0897117,ダンス目とじ,頬０涙０",
			"37.0008778,ダンス目あけ,頬０涙０",
			"37.4978427,ダンス目つむり,頬０涙０",
			"38.2102135,ダンス微笑み,頬０涙０",
			"39.0387276,ダンスウインク,頬０涙０",
			"39.9166429,ダンス微笑み,頬０涙０",
			"40.9437807,ダンス目つむり,頬０涙０",
			"41.6062683,ダンスびっくり,頬０涙０",
			"42.9151325,ダンス目つむり,頬０涙０",
			"43.544715,ダンス目あけ,頬０涙０",
			"44.1410801,ダンス目つむり,頬０涙０",
			"44.6876816,ダンス目あけ,頬０涙０",
			"45.4498413,ダンス微笑み,頬０涙０",
			"46.0297464,ダンス目とじ,頬０涙０",
			"46.9407986,ダンス目あけ,頬０涙０",
			"47.7360627,ダンス微笑み,頬０涙０",
			"48.2331045,ダンス目あけ,頬０涙０",
			"48.7303438,ダンス目とじ,頬０涙０",
			"49.6412748,ダンス目あけ,頬０涙０",
			"50.1713456,ダンス微笑み,頬０涙０",
			"52.2753174,ダンス微笑み,頬１涙０",
			"52.8881586,ダンス目つむり,頬１涙０",
			"53.3851865,ダンス目あけ,頬１涙０",
			"53.882543,ダンス微笑み,頬１涙０",
			"55.0750738,ダンス目つむり,頬０涙０",
			"55.588648,ダンス目あけ,頬０涙０",
			"56.0855316,ダンス目つむり,頬０涙０",
			"56.5829215,ダンス目あけ,頬０涙０",
			"58.0072912,ダンス微笑み,頬０涙０",
			"58.6038107,ダンス目とじ,頬０涙０",
			"59.166899,ダンス微笑み,頬０涙０",
			"59.6972159,ダンス目とじ,頬０涙０",
			"60.3930079,ダンス目あけ,頬０涙０",
			"61.0390758,ダンスウインク,頬０涙０",
			"61.7348781,ダンス目とじ,頬０涙０",
			"62.4141041,ダンス微笑み,頬０涙０",
			"63.0437069,ダンス目あけ,頬０涙０",
			"63.5572201,ダンス微笑み,頬０涙０",
			"64.6506198,ダンス目あけ,頬１涙０",
			"65.7108824,ダンス微笑み,頬０涙０",
			"66.8208661,ダンスウインク,頬０涙０",
			"67.4007062,ダンス目あけ,頬０涙０",
			"67.9308112,ダンス目とじ,頬０涙０",
			"68.5106456,ダンス微笑み,頬０涙０",
			"69.438388,ダンス目つむり,頬０涙０",
			"70.0513573,ダンス困り顔,頬０涙０",
			"71.111625,ダンス目あけ,頬０涙０",
			"71.7079176,ダンスウインク,頬０涙０",
			"72.5198164,ダンス目あけ,頬０涙０",
			"73.0167721,ダンス目とじ,頬０涙０",
			"73.5800465,ダンス目あけ,頬０涙０",
			"74.1102306,ダンス微笑み,頬０涙０",
			"75.7337331,ダンス目つむり,頬０涙０",
			"76.6282115,ダンス微笑み,頬０涙０",
			"77.324139,ダンスウインク,頬０涙０",
			"77.9039628,ダンス目あけ,頬０涙０",
			"78.4506812,ダンス目とじ,頬１涙０",
			"79.0305094,ダンス微笑み,頬０涙０",
			"79.9417106,ダンス目つむり,頬０涙０",
			"80.5380924,ダンス目とじ,頬０涙０",
			"81.6315783,ダンスウインク,頬０涙０",
			"82.3438049,ダンス微笑み,頬０涙０",
			"83.1224494,ダンス目とじ,頬０涙０",
			"83.81825,ダンス目あけ,頬０涙０",
			"84.4974646,ダンス目つむり,頬０涙０",
			"85.0607356,ダンス目あけ,頬０涙０",
			"85.657144,ダンス目とじ,頬０涙０",
			"86.3198348,ダンス目あけ,頬０涙０",
			"87.0487937,ダンス微笑み,頬０涙０",
			"88.6391448,ダンス目とじ,頬０涙０",
			"89.2190245,ダンス微笑み,頬０涙０",
			"89.9313558,ダンス目つむり,頬０涙０",
			"90.527623,ダンス目あけ,頬０涙０",
			"91.521783,ダンス目とじ,頬１涙０",
			"92.3169841,ダンス目あけ,頬０涙０",
			"92.8636934,ダンス目つむり,頬０涙０",
			"93.4268201,ダンス目あけ,頬０涙０",
			"94.271868,ダンス目とじ,頬０涙０",
			"94.8350928,ダンス微笑み,頬０涙０",
			"96.29295,ダンス目あけ,頬０涙０",
			"96.8894185,ダンスウインク,頬０涙０",
			"97.7343027,ダンス微笑み,頬０涙０",
			"98.6785427,ダンス目あけ,頬０涙０",
			"100.8487687,ダンス微笑み,頬０涙０",
			"102.0582068,ダンス目つむり,頬０涙０",
			"102.8533497,ダンス目あけ,頬０涙０",
			"103.8805023,ダンス目つむり,頬０涙０",
			"104.476893,ダンス微笑み,頬０涙０",
			"105.5040405,ダンス目あけ,頬０涙０",
			"106.0010489,ダンス目つむり,頬０涙０",
			"106.498022,ダンス目あけ,頬０涙０",
			"107.7239231,ダンス目つむり,頬０涙０",
			"108.3203862,ダンス目あけ,頬０涙０",
			"109.050094,ダンス目つむり,頬０涙０",
			"109.5793942,ダンス微笑み,頬０涙０",
			"110.8052489,ダンス目とじ,頬０涙０",
			"111.4017855,ダンスびっくり,頬０涙０",
			"112.1471287,ダンス目とじ,頬０涙０",
			"113.1918064,ダンス微笑み,頬０涙０",
			"114.2843699,ダンス目あけ,頬０涙０",
			"115.3419748,ダンスウインク,頬０涙０",
			"115.8886735,ダンス微笑み,頬０涙０",
			"116.4850959,ダンス目あけ,頬０涙０",
			"117.1974645,ダンス困り顔,頬０涙０",
			"118.5890172,ダンス目つむり,頬０涙０",
			"119.1031188,ダンス微笑み,頬０涙０",
			"119.7321026,ダンス目あけ,頬０涙０",
			"120.3616632,ダンス目つむり,頬０涙０",
			"120.8917931,ダンス目あけ,頬０涙０",
			"121.3887924,ダンス微笑み,頬０涙０",
			"121.9520509,ダンス目あけ,頬０涙０",
			"122.5815752,ダンス目とじ,頬０涙０",
			"124.801495,ダンス目あけ,頬０涙０",
			"126.1267267,ダンスウインク,頬０涙０",
			"126.6901328,ダンス目あけ,頬０涙０",
			"127.2534055,ダンス目とじ,頬１涙０",
			"127.8332034,ダンス微笑み,頬０涙０",
			"128.7444229,ダンス目つむり,頬０涙０",
			"129.3408022,ダンス目とじ,頬０涙０",
			"130.3513495,ダンス微笑み,頬０涙０",
			"131.1464234,ダンス目あけ,頬０涙０",
			"131.8256951,ダンス目とじ,頬０涙０",
			"132.504896,ダンス目あけ,頬０涙０",
			"133.2836717,ダンス目つむり,頬０涙０",
			"133.7806448,ダンス目あけ,頬０涙０",
			"134.476442,ダンス微笑み,頬０涙０",
			"135.0397523,ダンス目あけ,頬０涙０",
			"135.6195511,ダンス微笑み,頬０涙０",
			"137.2430929,ダンスウインク,頬０涙０",
			"137.9388681,ダンス微笑み,頬０涙０",
			"138.8997494,ダンス目つむり,頬０涙０",
			"139.8109232,ダンス目あけ,頬０涙０",
			"140.3410827,ダンスウインク,頬１涙０",
			"141.1197039,ダンス目あけ,頬０涙０",
			"141.7327045,ダンス目つむり,頬０涙０",
			"142.2793288,ダンス微笑み,頬０涙０",
			"142.9917681,ダンス目とじ,頬０涙０",
			"143.7040945,ダンス微笑み,頬０涙０",
			"144.2010918,ダンス目つむり,頬０涙０",
			"144.7146394,ダンス目あけ,頬０涙０",
			"145.4767011,ダンスウインク,頬１涙０",
			"146.0565917,ダンス目あけ,頬１涙０",
			"146.3049307,ダンス目あけ,頬０涙０",
			"147.6304313,ダンス目つむり,頬０涙０",
			"148.1272624,ダンス目あけ,頬０涙０",
			"149.7840338,ダンス微笑み,頬０涙０",
			"150.8774671,ダンス目あけ,頬０涙０",
			"152.401562,ダンス目つむり,頬０涙０",
			"153.5944053,ダンス目あけ,頬０涙０",
			"154.9362401,ダンス微笑み,頬０涙０",
			"156.1290831,ダンスウインク,頬０涙０",
			"156.7917189,ダンス目あけ,頬０涙０",
			"157.4710279,ダンス目とじ,頬０涙０",
			"158.4484304,ダンス目あけ,頬０涙０",
			"159.8234833,ダンス目とじ,頬０涙０"
		};

		private string[] danceO2CArray = new string[]
		{
			"0.0300643,ダンス目つむり,頬０涙０",
			"5.195647,ダンス微笑み,頬０涙０",
			"7.1836001,ダンス目あけ,頬０涙０",
			"7.6807482,ダンスウインク,頬０涙０",
			"8.7741653,ダンスびっくり,頬０涙０",
			"9.8508565,ダンス目あけ,頬０涙０",
			"10.4804616,ダンスウインク,頬０涙０",
			"12.5513225,ダンス目あけ,頬０涙０",
			"13.0482195,ダンス微笑み,頬０涙０",
			"13.9780271,ダンス目とじ,頬０涙０",
			"15.1872633,ダンス微笑み,頬０涙０",
			"15.9661624,ダンス目あけ,頬０涙０",
			"16.496087,ダンス目つむり,頬０涙０",
			"17.2748673,ダンス目あけ,頬０涙０",
			"18.3019483,ダンス微笑み,頬０涙０",
			"19.5775573,ダンス目とじ,頬１涙０",
			"20.7039251,ダンス目あけ,頬１涙０",
			"20.8365919,ダンス目あけ,頬０涙０",
			"21.2009549,ダンス目つむり,頬０涙０",
			"21.8470821,ダンス目あけ,頬０涙０",
			"22.9903303,ダンス微笑み,頬０涙０",
			"24.5972275,ダンス目つむり,頬０涙０",
			"25.1272507,ダンス目あけ,頬０涙０",
			"25.6243139,ダンス微笑み,頬０涙０",
			"26.2869731,ダンスウインク,頬０涙０",
			"26.9000271,ダンス目あけ,頬０涙０",
			"28.5401463,ダンス目とじ,頬１涙０",
			"29.3850245,ダンス微笑み,頬０涙０",
			"31.0274659,ダンス真剣,頬０涙０",
			"32.1042803,ダンス微笑み,頬０涙０",
			"33.0485667,ダンス目つむり,頬０涙０",
			"33.5455293,ダンス目あけ,頬０涙０",
			"34.2415739,ダンス微笑み,頬０涙０",
			"35.5931058,ダンス目あけ,頬０涙０",
			"36.0899244,ダンス目とじ,頬０涙０",
			"37.0011273,ダンス目あけ,頬０涙０",
			"37.4981315,ダンス目つむり,頬０涙０",
			"38.2104331,ダンス微笑み,頬０涙０",
			"39.0388904,ダンスウインク,頬０涙０",
			"39.9168633,ダンス目あけ,頬０涙０",
			"40.9441479,ダンス目つむり,頬０涙０",
			"41.6066218,ダンスびっくり,頬０涙０",
			"42.9154344,ダンス目つむり,頬０涙０",
			"43.5448912,ダンス目あけ,頬０涙０",
			"44.1414672,ダンス目つむり,頬０涙０",
			"44.6880205,ダンス目あけ,頬０涙０",
			"44.8868614,ダンス目あけ,頬１涙０",
			"45.4503531,ダンス微笑み,頬１涙０",
			"46.0299215,ダンス目とじ,頬１涙０",
			"46.0961247,ダンス目とじ,頬０涙０",
			"46.941186,ダンス目あけ,頬０涙０",
			"47.7364301,ダンス微笑み,頬０涙０",
			"48.2334751,ダンス目あけ,頬０涙０",
			"48.7305297,ダンスウインク,頬０涙０",
			"49.3931197,ダンス目あけ,頬０涙０",
			"50.1715712,ダンス微笑み,頬０涙０",
			"52.8885233,ダンス目あけ,頬０涙０",
			"53.3855622,ダンス目つむり,頬０涙０",
			"53.8828937,ダンス微笑み,頬０涙０",
			"55.0754134,ダンス目つむり,頬０涙０",
			"55.5889961,ダンス目あけ,頬０涙０",
			"56.0859159,ダンス目つむり,頬０涙０",
			"56.5833252,ダンス目あけ,頬０涙０",
			"58.0076815,ダンス微笑み,頬０涙０",
			"58.6042007,ダンス目とじ,頬０涙０",
			"59.167291,ダンス微笑み,頬０涙０",
			"59.697603,ダンス目とじ,頬０涙０",
			"60.3933831,ダンス目あけ,頬０涙０",
			"61.039478,ダンスウインク,頬０涙０",
			"61.735247,ダンス目とじ,頬０涙０",
			"62.4144551,ダンス微笑み,頬０涙０",
			"63.0439621,ダンス目あけ,頬０涙０",
			"63.5574265,ダンス微笑み,頬０涙０",
			"64.6510169,ダンス目あけ,頬１涙０",
			"65.7112063,ダンス微笑み,頬０涙０",
			"66.8212481,ダンスウインク,頬０涙０",
			"67.4009961,ダンス目あけ,頬０涙０",
			"67.9310085,ダンス目とじ,頬０涙０",
			"68.5109929,ダンス微笑み,頬０涙０",
			"69.4386831,ダンス目つむり,頬０涙０",
			"70.05168,ダンス困り顔,頬０涙０",
			"71.1119757,ダンス目あけ,頬０涙０",
			"71.7082854,ダンスウインク,頬０涙０",
			"72.5200591,ダンス目あけ,頬０涙０",
			"73.0170213,ダンス目とじ,頬０涙０",
			"73.5803955,ダンス目あけ,頬０涙０",
			"74.1105992,ダンス微笑み,頬０涙０",
			"75.7340835,ダンス目つむり,頬０涙０",
			"76.628591,ダンス微笑み,頬０涙０",
			"77.3245071,ダンスウインク,頬０涙０",
			"77.9041921,ダンス目あけ,頬０涙０",
			"78.4510077,ダンス目とじ,頬１涙０",
			"79.0308506,ダンス微笑み,頬０涙０",
			"79.9419977,ダンス目つむり,頬０涙０",
			"80.5384802,ダンス目とじ,頬０涙０",
			"82.3441957,ダンス微笑み,頬０涙０",
			"83.1226062,ダンス目とじ,頬０涙０",
			"83.8185895,ダンス目あけ,頬０涙０",
			"84.4978125,ダンス目つむり,頬０涙０",
			"85.0611042,ダンス目あけ,頬０涙０",
			"85.6574756,ダンス目とじ,頬０涙０",
			"86.3202163,ダンス目あけ,頬０涙０",
			"87.0496452,ダンス微笑み,頬０涙０",
			"88.6395103,ダンス目とじ,頬０涙０",
			"89.2193299,ダンス微笑み,頬０涙０",
			"89.9315659,ダンス目つむり,頬０涙０",
			"90.527988,ダンス目あけ,頬０涙０",
			"91.522134,ダンス目とじ,頬１涙０",
			"92.3173191,ダンス目あけ,頬０涙０",
			"92.8639543,ダンス目つむり,頬０涙０",
			"93.4270291,ダンス目あけ,頬０涙０",
			"94.2722569,ダンス目とじ,頬０涙０",
			"94.8353825,ダンス微笑み,頬０涙０",
			"96.293178,ダンス目あけ,頬０涙０",
			"96.8896902,ダンスウインク,頬０涙０",
			"97.7346195,ダンス微笑み,頬０涙０",
			"98.6788583,ダンス目あけ,頬０涙０",
			"101.4950863,ダンス微笑み,頬０涙０",
			"102.0585751,ダンス目つむり,頬０涙０",
			"102.8537098,ダンス目あけ,頬０涙０",
			"103.8808362,ダンス目つむり,頬０涙０",
			"104.4771145,ダンス目あけ,頬０涙０",
			"105.5042229,ダンスびっくり,頬０涙０",
			"106.0013389,ダンス微笑み,頬０涙０",
			"107.6081978,ダンス目つむり,頬０涙０",
			"108.2378599,ダンス微笑み,頬０涙０",
			"108.9999581,ダンス目あけ,頬０涙０",
			"110.0933094,ダンス微笑み,頬０涙０",
			"111.4019666,ダンスびっくり,頬０涙０",
			"112.1474728,ダンス目とじ,頬０涙０",
			"113.1920105,ダンス微笑み,頬０涙０",
			"114.2847613,ダンス目あけ,頬０涙０",
			"114.8451765,ダンス目つむり,頬０涙０",
			"115.3422705,ダンス目あけ,頬０涙０",
			"115.8890507,ダンス微笑み,頬０涙０",
			"116.4854648,ダンス目あけ,頬０涙０",
			"117.1976863,ダンス困り顔,頬０涙０",
			"118.5893987,ダンス目つむり,頬０涙０",
			"119.1033155,ダンス微笑み,頬０涙０",
			"119.7324359,ダンス目あけ,頬０涙０",
			"120.3620222,ダンス目つむり,頬０涙０",
			"120.8921315,ダンス目あけ,頬０涙０",
			"121.3891699,ダンス微笑み,頬０涙０",
			"121.9524186,ダンス目あけ,頬０涙０",
			"122.5818304,ダンス目とじ,頬０涙０",
			"124.8018231,ダンス目あけ,頬０涙０",
			"126.1270803,ダンスウインク,頬０涙０",
			"126.6903663,ダンス目あけ,頬０涙０",
			"127.253769,ダンス目とじ,頬１涙０",
			"127.8336032,ダンス微笑み,頬０涙０",
			"128.7446493,ダンス目つむり,頬０涙０",
			"129.3411993,ダンス目とじ,頬０涙０",
			"130.3517047,ダンス目あけ,頬０涙０",
			"131.1468071,ダンス微笑み,頬０涙０",
			"131.8259072,ダンス目とじ,頬０涙０",
			"132.5050734,ダンス目あけ,頬０涙０",
			"133.2840386,ダンス目つむり,頬０涙０",
			"133.7809829,ダンス目あけ,頬０涙０",
			"134.4766524,ダンス微笑み,頬０涙０",
			"135.0399735,ダンス目あけ,頬０涙０",
			"135.6199129,ダンス微笑み,頬０涙０",
			"137.2434835,ダンスウインク,頬０涙０",
			"137.9392519,ダンス微笑み,頬０涙０",
			"138.9000898,ダンス目つむり,頬０涙０",
			"139.8112847,ダンス目あけ,頬０涙０",
			"140.3414705,ダンスウインク,頬１涙０",
			"141.1199114,ダンス目あけ,頬０涙０",
			"141.7328593,ダンス目つむり,頬０涙０",
			"142.2795104,ダンス微笑み,頬０涙０",
			"142.9922137,ダンス目とじ,頬０涙０",
			"143.7043143,ダンス微笑み,頬０涙０",
			"144.2014405,ダンス目つむり,頬０涙０",
			"144.7149844,ダンス目あけ,頬０涙０",
			"145.4768779,ダンスウインク,頬１涙０",
			"146.0568742,ダンス目あけ,頬１涙０",
			"146.3051668,ダンス目あけ,頬０涙０",
			"147.6307714,ダンス目つむり,頬０涙０",
			"148.1276351,ダンス目あけ,頬０涙０",
			"149.038812,ダンス微笑み,頬０涙０",
			"150.8778169,ダンス目あけ,頬０涙０",
			"151.6398952,ダンス目つむり,頬０涙０",
			"153.5948004,ダンス目あけ,頬０涙０",
			"155.632492,ダンス微笑み,頬０涙０",
			"156.12943,ダンスウインク,頬０涙０",
			"156.7921081,ダンス目あけ,頬０涙０",
			"157.4713769,ダンス目とじ,頬０涙０",
			"158.4488016,ダンス目あけ,頬０涙０",
			"159.823674,ダンス目とじ,頬０涙０"
		};

		private string[] danceO3Array = new string[]
		{
			"0.0277476,ダンス目つむり,頬０涙０",
			"4.6437962,ダンス目あけ,頬０涙０",
			"5.2428244,ダンス目つむり,頬０涙０",
			"8.7218505,ダンス目あけ,頬０涙０",
			"9.251961,ダンス目つむり,頬０涙０",
			"11.7701857,ダンス微笑み,頬０涙０",
			"12.6653996,ダンス目つむり,頬０涙０",
			"13.2610565,ダンス目あけ,頬０涙０",
			"14.5035783,ダンス目つむり,頬０涙０",
			"15.0011558,ダンス目あけ,頬０涙０",
			"15.4975747,ダンス微笑み,頬０涙０",
			"16.7897959,ダンスウインク,頬０涙０",
			"17.3861929,ダンス微笑み,頬０涙０",
			"18.3470727,ダンス目つむり,頬０涙０",
			"19.04283,ダンス目あけ,頬０涙０",
			"19.5398783,ダンス目つむり,頬０涙０",
			"20.3516617,ダンス目あけ,頬０涙０",
			"20.8486952,ダンス目つむり,頬０涙０",
			"21.3455714,ダンス目あけ,頬０涙０",
			"22.5391998,ダンス微笑み,頬０涙０",
			"23.748562,ダンス目つむり,頬０涙０",
			"24.4939611,ダンス目あけ,頬０涙０",
			"25.2725714,ダンス微笑み,頬０涙０",
			"26.6918805,ダンス目あけ,頬０涙０",
			"27.2717315,ダンス目つむり,頬０涙０",
			"28.166371,ダンス微笑み,頬０涙０",
			"28.9118507,ダンス目つむり,頬０涙０",
			"29.7401735,ダンス微笑み,頬０涙０",
			"31.347465,ダンス目あけ,頬０涙０",
			"32.274719,ダンス微笑み,頬０涙０",
			"33.6830661,ダンス目つむり,頬０涙０",
			"34.1800771,ダンス目あけ,頬０涙０",
			"35.1575201,ダンスウインク,頬０涙０",
			"35.6876009,ダンス微笑み,頬０涙０",
			"36.8803817,ダンスウインク,頬０涙０",
			"37.5264943,ダンス目あけ,頬０涙０",
			"38.0235068,ダンス目つむり,頬０涙０",
			"38.58682,ダンス微笑み,頬０涙０",
			"39.1168911,ダンス目つむり,頬０涙０",
			"39.9783887,ダンス目あけ,頬０涙０",
			"40.707297,ダンス目つむり,頬０涙０",
			"41.2871659,ダンス微笑み,頬０涙０",
			"41.9994966,ダンス目あけ,頬０涙０",
			"42.4964722,ダンスウインク,頬０涙０",
			"43.1592386,ダンス微笑み,頬０涙０",
			"43.8881803,ダンス目とじ,頬０涙０",
			"44.4845089,ダンス目あけ,頬０涙０",
			"45.0808665,ダンス微笑み,頬０涙０",
			"45.5778396,ダンスウインク,頬０涙０",
			"46.0917311,ダンス目あけ,頬０涙０",
			"47.2676959,ダンス微笑み,頬０涙０",
			"48.6757439,ダンス目つむり,頬０涙０",
			"49.3054736,ダンス目あけ,頬０涙０",
			"50.3491705,ダンス微笑み,頬０涙０",
			"51.2603457,ダンス目つむり,頬０涙０",
			"51.8400749,ダンス目あけ,頬０涙０",
			"52.8672156,ダンス微笑み,頬０涙０",
			"54.6587437,ダンス目つむり,頬０涙０",
			"55.2220061,ダンス目あけ,頬０涙０",
			"56.1166282,ダンス微笑み,頬０涙０",
			"56.8951502,ダンス目つむり,頬０涙０",
			"59.5954933,ダンス微笑み,頬０涙０",
			"60.1091393,ダンス目あけ,頬０涙０",
			"60.7551569,ダンス微笑み,頬０涙０",
			"61.2523287,ダンス目つむり,頬０涙０",
			"61.7493013,ダンス微笑み,頬０涙０",
			"62.3456743,ダンス目あけ,頬０涙０",
			"63.2899929,ダンス目つむり,頬０涙０",
			"63.7869766,ダンス微笑み,頬０涙０",
			"64.698147,ダンス目つむり,頬０涙０",
			"65.1950232,ダンス目あけ,頬０涙０",
			"65.7087482,ダンス目つむり,頬０涙０",
			"66.238833,ダンス目あけ,頬０涙０",
			"67.0175143,ダンス微笑み,頬０涙０",
			"67.5143602,ダンス誘惑,頬０涙０",
			"68.1935849,ダンス微笑み,頬０涙０",
			"68.8398645,ダンス目つむり,頬０涙０",
			"69.8336947,ダンス微笑み,頬０涙０",
			"70.6799751,ダンス目つむり,頬０涙０",
			"71.2599201,ダンス微笑み,頬０涙０",
			"72.470143,ダンス目あけ,頬０涙０",
			"73.3987113,ダンス微笑み,頬０涙０",
			"74.408486,ダンス目つむり,頬０涙０",
			"75.352631,ダンス目あけ,頬０涙０",
			"76.5950981,ダンス目つむり,頬０涙０",
			"77.9702642,ダンス目あけ,頬０涙０",
			"79.2790523,ダンス目つむり,頬０涙０",
			"79.7926199,ダンス目あけ,頬０涙０",
			"80.2896209,ダンス微笑み,頬０涙０",
			"81.0682574,ダンス目あけ,頬０涙０",
			"82.4432858,ダンス目つむり,頬０涙０",
			"82.9403245,ダンス目あけ,頬０涙０",
			"85.3590395,ダンス微笑み,頬０涙０",
			"86.8189767,ダンス目つむり,頬０涙０",
			"87.3500414,ダンス目あけ,頬０涙０",
			"88.3274912,ダンス微笑み,頬０涙０",
			"89.4705764,ダンス目つむり,頬０涙０",
			"90.000692,ダンス目あけ,頬０涙０",
			"91.6904744,ダンス目つむり,頬０涙０",
			"92.8666935,ダンス目あけ,頬０涙０",
			"94.4735708,ダンス目つむり,頬０涙０",
			"95.9646851,ダンス微笑み,頬０涙０",
			"97.1739635,ダンス目あけ,頬０涙０",
			"98.8307508,ダンス目つむり,頬０涙０",
			"100.8518578,ダンス微笑み,頬０涙０",
			"102.0942337,ダンス目つむり,頬０涙０",
			"102.6079193,ダンス目あけ,頬０涙０",
			"103.5191154,ダンス目つむり,頬０涙０",
			"104.7298107,ダンス微笑み,頬０涙０",
			"105.2434576,ダンス目つむり,頬０涙０",
			"105.7403939,ダンス目あけ,頬０涙０",
			"106.3036694,ダンス微笑み,頬０涙０",
			"107.7780644,ダンス目つむり,頬０涙０",
			"108.2916431,ダンス微笑み,頬０涙０",
			"108.7868209,ダンス目あけ,頬０涙０",
			"109.4991995,ダンス微笑み,頬０涙０",
			"110.1784432,ダンス目あけ,頬０涙０",
			"110.6920057,ダンス目つむり,頬０涙０",
			"111.189001,ダンス微笑み,頬０涙０",
			"111.9210914,ダンス目つむり,頬０涙０",
			"112.6965487,ダンス微笑み,頬０涙０",
			"113.8230916,ダンス目つむり,頬０涙０",
			"114.3201736,ダンス目あけ,頬０涙０",
			"114.9995898,ダンス目つむり,頬０涙０",
			"115.4963348,ダンス微笑み,頬０涙０",
			"116.4074881,ダンス目つむり,頬０涙０",
			"116.9053804,ダンス目あけ,頬０涙０",
			"117.7990916,ダンス目つむり,頬０涙０",
			"118.2961112,ダンス微笑み,頬０涙０",
			"119.3894915,ダンスウインク,頬０涙０",
			"119.8864871,ダンス微笑み,頬０涙０",
			"120.3834582,ダンス目あけ,頬０涙０",
			"121.3278225,ダンス微笑み,頬０涙０",
			"121.9905199,ダンス目つむり,頬０涙０",
			"122.6200554,ダンス微笑み,頬０涙０",
			"123.2164079,ダンス目あけ,頬０涙０",
			"123.7132844,ダンス微笑み,頬０涙０",
			"124.3097871,ダンス目つむり,頬０涙０",
			"124.806803,ダンス目あけ,頬０涙０",
			"125.3037906,ダンス目つむり,頬０涙０",
			"125.917929,ダンス微笑み,頬０涙０",
			"127.5402795,ダンス目つむり,頬０涙０",
			"128.0373392,ダンス目あけ,頬０涙０",
			"128.5341915,ダンス目つむり,頬０涙０",
			"129.03131,ダンス目あけ,頬０涙０",
			"130.2241603,ダンス微笑み,頬０涙０",
			"131.3339618,ダンス目つむり,頬０涙０",
			"131.8310672,ダンス微笑み,頬０涙０",
			"132.3285395,ダンス目あけ,頬０涙０",
			"133.5048359,ダンス目つむり,頬０涙０",
			"134.001809,ダンス微笑み,頬０涙０",
			"134.5816138,ダンス目あけ,頬０涙０",
			"135.0785946,ダンスウインク,頬０涙０",
			"135.641845,ダンス微笑み,頬０涙０",
			"136.3047967,ダンス目あけ,頬０涙０",
			"137.0003399,ダンス微笑み,頬０涙０",
			"137.3813964,ダンスウインク,頬０涙０",
			"138.1599796,ダンス微笑み,頬０涙０",
			"138.657004,ダンス目あけ,頬０涙０",
			"139.1541587,ダンス微笑み,頬０涙０",
			"140.4296828,ダンス目つむり,頬０涙０",
			"140.9431874,ダンス目あけ,頬０涙０",
			"141.4402127,ダンス目つむり,頬０涙０",
			"142.0034954,ダンス微笑み,頬０涙０",
			"142.9974635,ダンス目つむり,頬０涙０",
			"143.4943405,ダンス目あけ,頬０涙０",
			"144.3061989,ダンス目つむり,頬０涙０",
			"144.8032036,ダンス微笑み,頬０涙０",
			"146.5592993,ダンス目つむり,頬０涙０",
			"147.0562801,ダンス目あけ,頬０涙０",
			"147.6692904,ダンス目つむり,頬０涙０",
			"148.1663128,ダンス微笑み,頬０涙０",
			"148.9944415,ダンス目あけ,頬０涙０",
			"149.988618,ダンス目つむり,頬０涙０",
			"150.485629,ダンス微笑み,頬０涙０",
			"151.3967196,ダンス目あけ,頬０涙０",
			"151.9103397,ダンス目つむり,頬０涙０",
			"152.4073193,ダンス微笑み,頬０涙０",
			"155.1427496,ダンス目つむり,頬０涙０",
			"155.6397156,ダンス微笑み,頬０涙０",
			"156.1367534,ダンス目あけ,頬０涙０",
			"156.6669472,ダンス微笑み,頬０涙０",
			"157.3956996,ダンス目あけ,頬０涙０",
			"158.5886588,ダンス微笑み,頬０涙０",
			"160.7091199,ダンス目つむり,頬０涙０",
			"161.206123,ダンス目あけ,頬０涙０",
			"162.6474173,ダンス微笑み,頬０涙０",
			"163.1610102,ダンス目あけ,頬０涙０",
			"163.6580335,ダンス目つむり,頬０涙０",
			"164.1549028,ダンス微笑み,頬０涙０",
			"165.1821455,ダンス目つむり,頬０涙０",
			"165.6791269,ダンス目あけ,頬０涙０",
			"166.1925611,ダンス微笑み,頬０涙０",
			"168.1641369,ダンスウインク,頬０涙０",
			"168.7770765,ダンス目あけ,頬０涙０",
			"169.4727665,ダンス目つむり,頬０涙０",
			"169.9698659,ダンス微笑み,頬０涙０",
			"171.1143876,ダンス目あけ,頬０涙０",
			"171.6114163,ダンス目つむり,頬０涙０",
			"172.638661,ダンス目あけ,頬０涙０",
			"173.8976502,ダンス微笑み,頬０涙０",
			"174.8253872,ダンス目つむり,頬０涙０",
			"175.3058311,ダンス目あけ,頬０涙０",
			"175.8358654,ダンス目つむり,頬０涙０"
		};

		private string[] danceO3BArray = new string[]
		{
			"0.0278229,ダンス目つむり,頬０涙０",
			"4.6438051,ダンス目あけ,頬０涙０",
			"5.2428326,ダンス目つむり,頬０涙０",
			"8.7218587,ダンス目あけ,頬０涙０",
			"9.2519687,ダンス目つむり,頬０涙０",
			"11.7701939,ダンス微笑み,頬０涙０",
			"12.6654079,ダンス目つむり,頬０涙０",
			"13.2610773,ダンス目あけ,頬０涙０",
			"14.5035857,ダンス目つむり,頬０涙０",
			"15.0011724,ダンス目あけ,頬０涙０",
			"15.4975838,ダンス微笑み,頬０涙０",
			"16.7898045,ダンスウインク,頬０涙０",
			"17.3862017,ダンス微笑み,頬０涙０",
			"18.3470887,ダンス目つむり,頬０涙０",
			"19.0428377,ダンス目あけ,頬０涙０",
			"19.5398872,ダンス目つむり,頬０涙０",
			"20.3516699,ダンス目あけ,頬０涙０",
			"20.8487046,ダンス目つむり,頬０涙０",
			"21.3455805,ダンス目あけ,頬０涙０",
			"22.5392084,ダンス微笑み,頬０涙０",
			"23.7485711,ダンス目つむり,頬０涙０",
			"24.4939816,ダンス目あけ,頬０涙０",
			"25.2725851,ダンス微笑み,頬０涙０",
			"26.6919016,ダンス目あけ,頬０涙０",
			"27.2717449,ダンス目つむり,頬０涙０",
			"28.1663841,ダンス微笑み,頬０涙０",
			"28.9118584,ダンス目つむり,頬０涙０",
			"29.7401878,ダンス微笑み,頬０涙０",
			"31.3474741,ダンス目あけ,頬０涙０",
			"32.2747267,ダンス微笑み,頬０涙０",
			"33.6830764,ダンス目つむり,頬０涙０",
			"34.180086,ダンス目あけ,頬０涙０",
			"35.1575309,ダンスウインク,頬０涙０",
			"35.6876106,ダンス微笑み,頬０涙０",
			"36.8803923,ダンスウインク,頬０涙０",
			"37.5265023,ダンス目あけ,頬０涙０",
			"38.0235168,ダンス目つむり,頬０涙０",
			"38.5868279,ダンス微笑み,頬０涙０",
			"39.1169017,ダンス目つむり,頬０涙０",
			"39.9783966,ダンス目あけ,頬０涙０",
			"40.707307,ダンス目つむり,頬０涙０",
			"41.287177,ダンス微笑み,頬０涙０",
			"41.9995054,ダンス目あけ,頬０涙０",
			"42.4964828,ダンスウインク,頬０涙０",
			"43.1592845,ダンス微笑み,頬０涙０",
			"43.8881895,ダンス目とじ,頬０涙０",
			"44.4845174,ダンス目あけ,頬０涙０",
			"45.0808759,ダンス微笑み,頬０涙０",
			"45.5778487,ダンスウインク,頬０涙０",
			"46.0917405,ダンス目あけ,頬０涙０",
			"47.2677061,ダンス微笑み,頬０涙０",
			"48.6757536,ダンス目つむり,頬０涙０",
			"49.3054833,ダンス目あけ,頬０涙０",
			"50.3491807,ダンス微笑み,頬０涙０",
			"51.2603566,ダンス目つむり,頬０涙０",
			"51.8400843,ダンス目あけ,頬０涙０",
			"52.8672255,ダンス微笑み,頬０涙０",
			"54.6587548,ダンス目つむり,頬０涙０",
			"55.2220147,ダンス目あけ,頬０涙０",
			"56.1166385,ダンス微笑み,頬０涙０",
			"56.8951604,ダンス目つむり,頬０涙０",
			"59.5955012,ダンス微笑み,頬０涙０",
			"60.1091615,ダンス目あけ,頬０涙０",
			"60.7551666,ダンス微笑み,頬０涙０",
			"61.2523384,ダンス目つむり,頬０涙０",
			"61.7493118,ダンス微笑み,頬０涙０",
			"62.3456837,ダンス目あけ,頬０涙０",
			"63.2900038,ダンス目つむり,頬０涙０",
			"63.7869866,ダンス微笑み,頬０涙０",
			"64.6981561,ダンス目つむり,頬０涙０",
			"65.1950329,ダンス目あけ,頬０涙０",
			"65.708761,ダンス目つむり,頬０涙０",
			"66.2388435,ダンス目あけ,頬０涙０",
			"67.0175248,ダンス微笑み,頬０涙０",
			"67.5143699,ダンス誘惑,頬０涙０",
			"68.1935948,ダンス微笑み,頬０涙０",
			"68.8398751,ダンス目つむり,頬０涙０",
			"69.8337067,ダンス微笑み,頬０涙０",
			"70.6799836,ダンス目つむり,頬０涙０",
			"71.2599309,ダンス微笑み,頬０涙０",
			"72.4701518,ダンス目あけ,頬０涙０",
			"73.3987245,ダンス微笑み,頬０涙０",
			"74.4084962,ダンス目つむり,頬０涙０",
			"75.352641,ダンス目あけ,頬０涙０",
			"76.5951081,ダンス目つむり,頬０涙０",
			"77.9702733,ダンス目あけ,頬０涙０",
			"79.2790619,ダンス目つむり,頬０涙０",
			"79.7926298,ダンス目あけ,頬０涙０",
			"80.2896303,ダンス微笑み,頬０涙０",
			"81.0682685,ダンス目あけ,頬０涙０",
			"82.4432949,ダンス目つむり,頬０涙０",
			"82.9403376,ダンス目あけ,頬０涙０",
			"85.3590503,ダンス微笑み,頬０涙０",
			"86.8189876,ダンス目つむり,頬０涙０",
			"87.3500526,ダンス目あけ,頬０涙０",
			"88.3275009,ダンス微笑み,頬０涙０",
			"89.4705869,ダンス目つむり,頬０涙０",
			"90.0007025,ダンス目あけ,頬０涙０",
			"91.6904838,ダンス目つむり,頬０涙０",
			"92.8667035,ダンス目あけ,頬０涙０",
			"94.4735813,ダンス目つむり,頬０涙０",
			"95.964697,ダンス微笑み,頬０涙０",
			"97.1739749,ダンス目あけ,頬０涙０",
			"98.830761,ダンス目つむり,頬０涙０",
			"100.8518692,ダンス微笑み,頬０涙０",
			"102.0942445,ダンス目つむり,頬０涙０",
			"102.607929,ダンス目あけ,頬０涙０",
			"103.5191277,ダンス目つむり,頬０涙０",
			"104.7298216,ダンス微笑み,頬０涙０",
			"105.243469,ダンス目つむり,頬０涙０",
			"105.7404047,ダンス目あけ,頬０涙０",
			"106.3036814,ダンス微笑み,頬０涙０",
			"107.7780752,ダンス目つむり,頬０涙０",
			"108.2916531,ダンス微笑み,頬０涙０",
			"108.7868332,ダンス目あけ,頬０涙０",
			"109.4992106,ダンス微笑み,頬０涙０",
			"110.1784543,ダンス目あけ,頬０涙０",
			"110.6920171,ダンス目つむり,頬０涙０",
			"111.1890119,ダンス微笑み,頬０涙０",
			"111.9211022,ダンス目つむり,頬０涙０",
			"112.6965596,ダンス微笑み,頬０涙０",
			"113.8231018,ダンス目つむり,頬０涙０",
			"114.3201935,ダンス目あけ,頬０涙０",
			"114.9996109,ダンス目つむり,頬０涙０",
			"115.4963451,ダンス微笑み,頬０涙０",
			"116.4074981,ダンス目つむり,頬０涙０",
			"116.9053912,ダンス目あけ,頬０涙０",
			"117.7991019,ダンス目つむり,頬０涙０",
			"118.2961214,ダンス微笑み,頬０涙０",
			"119.3895021,ダンスウインク,頬０涙０",
			"119.8864974,ダンス微笑み,頬０涙０",
			"120.3834691,ダンス目あけ,頬０涙０",
			"121.3278345,ダンス微笑み,頬０涙０",
			"121.9905407,ダンス目つむり,頬０涙０",
			"122.6200665,ダンス微笑み,頬０涙０",
			"123.2164185,ダンス目あけ,頬０涙０",
			"123.7132946,ダンス微笑み,頬０涙０",
			"124.3097971,ダンス目つむり,頬０涙０",
			"124.8068138,ダンス目あけ,頬０涙０",
			"125.3038009,ダンス目つむり,頬０涙０",
			"125.9179436,ダンス微笑み,頬０涙０",
			"127.54029,ダンス目つむり,頬０涙０",
			"128.0373498,ダンス目あけ,頬０涙０",
			"128.5342157,ダンス目つむり,頬０涙０",
			"129.0313208,ダンス目あけ,頬０涙０",
			"130.2241723,ダンス微笑み,頬０涙０",
			"131.3339727,ダンス目つむり,頬０涙０",
			"131.8310769,ダンス微笑み,頬０涙０",
			"132.3285498,ダンス目あけ,頬０涙０",
			"133.5048493,ダンス目つむり,頬０涙０",
			"134.001831,ダンス微笑み,頬０涙０",
			"134.5816249,ダンス目あけ,頬０涙０",
			"135.0786054,ダンスウインク,頬０涙０",
			"135.6418564,ダンス微笑み,頬０涙０",
			"136.3048081,ダンス目あけ,頬０涙０",
			"137.0003507,ダンス微笑み,頬０涙０",
			"137.3814061,ダンスウインク,頬０涙０",
			"138.1599893,ダンス微笑み,頬０涙０",
			"138.6570149,ダンス目あけ,頬０涙０",
			"139.1541716,ダンス微笑み,頬０涙０",
			"140.4296968,ダンス目つむり,頬０涙０",
			"140.9431974,ダンス目あけ,頬０涙０",
			"141.4402238,ダンス目つむり,頬０涙０",
			"142.0035071,ダンス微笑み,頬０涙０",
			"142.9974835,ダンス目つむり,頬０涙０",
			"143.4943519,ダンス目あけ,頬０涙０",
			"144.3062103,ダンス目つむり,頬０涙０",
			"144.8032136,ダンス微笑み,頬０涙０",
			"146.5593107,ダンス目つむり,頬０涙０",
			"147.0562904,ダンス目あけ,頬０涙０",
			"147.6693015,ダンス目つむり,頬０涙０",
			"148.1663236,ダンス微笑み,頬０涙０",
			"148.9944532,ダンス目あけ,頬０涙０",
			"149.9886306,ダンス目つむり,頬０涙０",
			"150.485641,ダンス微笑み,頬０涙０",
			"151.396731,ダンス目あけ,頬０涙０",
			"151.9103511,ダンス目つむり,頬０涙０",
			"152.4073299,ダンス微笑み,頬０涙０",
			"155.1427607,ダンス目つむり,頬０涙０",
			"155.639727,ダンス微笑み,頬０涙０",
			"156.1367642,ダンス目あけ,頬０涙０",
			"156.666958,ダンス微笑み,頬０涙０",
			"157.3957098,ダンス目あけ,頬０涙０",
			"158.58867,ダンス微笑み,頬０涙０",
			"160.7091308,ダンス目つむり,頬０涙０",
			"161.2061344,ダンス目あけ,頬０涙０",
			"162.6474278,ダンス微笑み,頬０涙０",
			"163.1610219,ダンス目あけ,頬０涙０",
			"163.6580446,ダンス目つむり,頬０涙０",
			"164.1549145,ダンス微笑み,頬０涙０",
			"165.1821572,ダンス目つむり,頬０涙０",
			"165.6791386,ダンス目あけ,頬０涙０",
			"166.1925722,ダンス微笑み,頬０涙０",
			"168.1641461,ダンスウインク,頬０涙０",
			"168.7770885,ダンス目あけ,頬０涙０",
			"169.472779,ダンス目つむり,頬０涙０",
			"169.9698776,ダンス微笑み,頬０涙０",
			"171.1143985,ダンス目あけ,頬０涙０",
			"171.6114275,ダンス目つむり,頬０涙０",
			"172.6386727,ダンス目あけ,頬０涙０",
			"173.8976599,ダンス微笑み,頬０涙０",
			"174.8253994,ダンス目つむり,頬０涙０",
			"175.3058442,ダンス目あけ,頬０涙０",
			"175.835878,ダンス目つむり,頬０涙０"
		};

		private string[] danceO3CArray = new string[]
		{
			"0.0280279,ダンス目つむり,頬０涙０",
			"4.6441195,ダンス目あけ,頬０涙０",
			"5.243156,ダンス目つむり,頬０涙０",
			"8.7222368,ダンス目あけ,頬０涙０",
			"9.2522985,ダンス目つむり,頬０涙０",
			"11.7705646,ダンス微笑み,頬０涙０",
			"12.2673857,ダンス目つむり,頬０涙０",
			"12.847447,ダンス目あけ,頬０涙０",
			"14.5039669,ダンス目つむり,頬０涙０",
			"15.0013389,ダンス目あけ,頬０涙０",
			"15.4979639,ダンス微笑み,頬０涙０",
			"16.7901543,ダンスウインク,頬０涙０",
			"17.3864866,ダンス微笑み,頬０涙０",
			"18.3474382,ダンス目つむり,頬０涙０",
			"19.0430216,ダンス目あけ,頬０涙０",
			"19.5402547,ダンス目つむり,頬０涙０",
			"20.2029264,ダンス目あけ,頬０涙０",
			"20.6999266,ダンス目つむり,頬０涙０",
			"21.1969065,ダンス目あけ,頬０涙０",
			"21.7104285,ダンス微笑み,頬０涙０",
			"24.4121957,ダンス目あけ,頬０涙０",
			"24.9583858,ダンス目つむり,頬０涙０",
			"25.4550615,ダンス微笑み,頬０涙０",
			"26.6922686,ダンス目あけ,頬０涙０",
			"27.2720821,ダンス目つむり,頬０涙０",
			"28.1667088,ダンス微笑み,頬０涙０",
			"28.9122213,ダンス目つむり,頬０涙０",
			"29.740622,ダンス微笑み,頬０涙０",
			"30.3700531,ダンス目あけ,頬０涙０",
			"32.2751387,ダンス微笑み,頬０涙０",
			"33.6834772,ダンス目つむり,頬０涙０",
			"34.1804757,ダンス目あけ,頬０涙０",
			"35.1578593,ダンスウインク,頬０涙０",
			"35.6879792,ダンス微笑み,頬０涙０",
			"36.8807689,ダンスウインク,頬０涙０",
			"37.526735,ダンス目あけ,頬０涙０",
			"38.0238577,ダンス目つむり,頬０涙０",
			"38.5870369,ダンス微笑み,頬０涙０",
			"39.1172347,ダンス目つむり,頬０涙０",
			"39.9787926,ダンス目あけ,頬０涙０",
			"40.7076867,ダンス目つむり,頬０涙０",
			"41.2045917,ダンス微笑み,頬０涙０",
			"41.999811,ダンス目あけ,頬０涙０",
			"42.4968486,ダンスウインク,頬０涙０",
			"43.1595303,ダンス微笑み,頬０涙０",
			"43.8885809,ダンス目とじ,頬０涙０",
			"44.4848741,ダンス目あけ,頬０涙０",
			"45.0810581,ダンス微笑み,頬０涙０",
			"45.5782276,ダンスウインク,頬０涙０",
			"46.0920897,ダンス目あけ,頬０涙０",
			"47.2680668,ダンス微笑み,頬０涙０",
			"48.6761362,ダンス目つむり,頬０涙０",
			"49.3057866,ダンス目あけ,頬０涙０",
			"50.349406,ダンス微笑み,頬０涙０",
			"51.2607392,ダンス目つむり,頬０涙０",
			"51.8402503,ダンス目あけ,頬０涙０",
			"52.8675035,ダンス微笑み,頬０涙０",
			"54.6591309,ダンス目つむり,頬０涙０",
			"55.222196,ダンス目あけ,頬０涙０",
			"56.116937,ダンス微笑み,頬０涙０",
			"56.895488,ダンス目つむり,頬０涙０",
			"59.5958699,ダンス微笑み,頬０涙０",
			"60.109525,ダンス目あけ,頬０涙０",
			"60.7555381,ダンス微笑み,頬０涙０",
			"61.2525677,ダンス目つむり,頬０涙０",
			"61.7496571,ダンス微笑み,頬０涙０",
			"62.3460153,ダンス目あけ,頬０涙０",
			"63.2903681,ダンス目つむり,頬０涙０",
			"63.7872129,ダンス微笑み,頬０涙０",
			"64.6985165,ダンス目つむり,頬０涙０",
			"65.1954169,ダンス目あけ,頬０涙０",
			"65.7090578,ダンス目つむり,頬０涙０",
			"66.239024,ダンス目あけ,頬０涙０",
			"67.0177324,ダンス微笑み,頬０涙０",
			"67.514594,ダンス誘惑,頬０涙０",
			"68.1939501,ダンス微笑み,頬０涙０",
			"68.6910994,ダンス目つむり,頬０涙０",
			"69.7181872,ダンス目あけ,頬０涙０",
			"71.2104719,ダンス目つむり,頬０涙０",
			"72.2882835,ダンス微笑み,頬０涙０",
			"74.4088606,ダンス目つむり,頬０涙０",
			"75.3529891,ダンス目あけ,頬０涙０",
			"76.5955038,ダンス目つむり,頬０涙０",
			"77.4735286,ダンス目あけ,頬０涙０",
			"79.2792704,ダンス目つむり,頬０涙０",
			"79.7929794,ダンス目あけ,頬０涙０",
			"80.2900158,ダンス微笑み,頬０涙０",
			"81.068599,ダンス目あけ,頬０涙０",
			"82.4435344,ダンス目つむり,頬０涙０",
			"82.9407247,ダンス目あけ,頬０涙０",
			"85.359377,ダンス微笑み,頬０涙０",
			"86.8193411,ダンス目つむり,頬０涙０",
			"87.3503699,ダンス目あけ,頬０涙０",
			"88.3278715,ダンス微笑み,頬０涙０",
			"89.470947,ダンス目つむり,頬０涙０",
			"90.0010247,ダンス目あけ,頬０涙０",
			"91.6908476,ダンス目つむり,頬０涙０",
			"93.2812991,ダンス目あけ,頬０涙０",
			"94.4739189,ダンス目つむり,頬０涙０",
			"95.9650423,ダンス微笑み,頬０涙０",
			"97.1741781,ダンス目あけ,頬０涙０",
			"97.9034061,ダンス目つむり,頬０涙０",
			"99.0297379,ダンス目あけ,頬０涙０",
			"100.0901901,ダンス微笑み,頬０涙０",
			"101.2498244,ダンス目つむり,頬０涙０",
			"101.7468363,ダンス目あけ,頬０涙０",
			"102.4756072,ダンスウインク,頬０涙０",
			"103.519447,ダンス目あけ,頬０涙０",
			"104.3822674,ダンス微笑み,頬０涙０",
			"105.2438413,ダンス目つむり,頬０涙０",
			"105.7407731,ダンス目あけ,頬０涙０",
			"106.3040489,ダンス微笑み,頬０涙０",
			"108.2920351,ダンス目つむり,頬０涙０",
			"108.7871967,ダンス目あけ,頬０涙０",
			"109.4995761,ダンス微笑み,頬０涙０",
			"110.1787942,ダンス目あけ,頬０涙０",
			"110.6922033,ダンス目つむり,頬０涙０",
			"111.1893825,ダンス微笑み,頬０涙０",
			"111.9216065,ダンス目つむり,頬０涙０",
			"112.6967566,ダンス微笑み,頬０涙０",
			"113.8234545,ダンス目つむり,頬０涙０",
			"114.3204632,ダンス目あけ,頬０涙０",
			"114.9999256,ダンス目つむり,頬０涙０",
			"115.4965121,ダンス微笑み,頬０涙０",
			"116.4078935,ダンス目つむり,頬０涙０",
			"116.9056088,ダンス目あけ,頬０涙０",
			"117.7994457,ダンス目つむり,頬０涙０",
			"118.2964684,ダンス微笑み,頬０涙０",
			"119.389873,ダンスウインク,頬０涙０",
			"119.8869364,ダンス微笑み,頬０涙０",
			"120.3838103,ダンス目あけ,頬０涙０",
			"121.3281903,ダンス微笑み,頬０涙０",
			"121.9907372,ダンス目つむり,頬０涙０",
			"122.6204377,ダンス微笑み,頬０涙０",
			"123.2167905,ダンス目あけ,頬０涙０",
			"123.7136587,ダンス微笑み,頬０涙０",
			"124.3101948,ダンス目つむり,頬０涙０",
			"124.8071639,ダンス目あけ,頬０涙０",
			"125.3040803,ダンス目つむり,頬０涙０",
			"125.9196342,ダンス微笑み,頬０涙０",
			"127.5406758,ダンス目つむり,頬０涙０",
			"128.0376976,ダンス目あけ,頬０涙０",
			"128.534588,ダンス目つむり,頬０涙０",
			"129.0316763,ダンス目あけ,頬０涙０",
			"130.2244152,ダンス微笑み,頬０涙０",
			"131.334195,ダンス目つむり,頬０涙０",
			"131.8314136,ダンス微笑み,頬０涙０",
			"132.3287203,ダンス目あけ,頬０涙０",
			"133.5051923,ダンス目つむり,頬０涙０",
			"134.0020505,ダンス微笑み,頬０涙０",
			"134.5819909,ダンス目あけ,頬０涙０",
			"135.0787571,ダンスウインク,頬０涙０",
			"135.6421498,ダンス微笑み,頬０涙０",
			"136.3050237,ダンス目あけ,頬０涙０",
			"137.0007288,ダンス微笑み,頬０涙０",
			"138.6574083,ダンス目あけ,頬０涙０",
			"139.1544783,ダンス微笑み,頬０涙０",
			"140.4299206,ダンス目つむり,頬０涙０",
			"140.9433753,ダンス目あけ,頬０涙０",
			"141.4405859,ダンス目つむり,頬０涙０",
			"142.0038937,ダンス微笑み,頬０涙０",
			"142.9979328,ダンス目つむり,頬０涙０",
			"143.4945296,ダンス目あけ,頬０涙０",
			"144.3064426,ダンス目つむり,頬０涙０",
			"144.8035782,ダンス微笑み,頬０涙０",
			"146.5596725,ダンス目つむり,頬０涙０",
			"147.0566174,ダンス目あけ,頬０涙０",
			"147.6696642,ダンス目つむり,頬０涙０",
			"148.1667031,ダンス微笑み,頬０涙０",
			"148.9948403,ダンス目あけ,頬０涙０",
			"149.9889958,ダンス目つむり,頬０涙０",
			"150.4860319,ダンス微笑み,頬０涙０",
			"151.3970062,ダンス目あけ,頬０涙０",
			"151.9106861,ダンス目つむり,頬０涙０",
			"152.4077288,ダンス微笑み,頬０涙０",
			"155.1431091,ダンス目つむり,頬０涙０",
			"155.6399052,ダンス微笑み,頬０涙０",
			"156.1370724,ダンス目あけ,頬０涙０",
			"156.6673223,ダンス微笑み,頬０涙０",
			"157.3960796,ダンス目あけ,頬０涙０",
			"159.102578,ダンス微笑み,頬０涙０",
			"161.6537493,ダンス目つむり,頬０涙０",
			"162.1508063,ダンス目あけ,頬０涙０",
			"162.6478364,ダンス微笑み,頬０涙０",
			"163.1612455,ダンス目あけ,頬０涙０",
			"163.658271,ダンス目つむり,頬０涙０",
			"164.155083,ダンス微笑み,頬０涙０",
			"165.6463262,ダンス目つむり,頬０涙０",
			"166.1598701,ダンス目あけ,頬０涙０",
			"166.6569165,ダンス微笑み,頬０涙０",
			"168.1645278,ダンスウインク,頬０涙０",
			"168.7774705,ダンス目あけ,頬０涙０",
			"169.4731639,ダンス目つむり,頬０涙０",
			"169.9702682,ダンス微笑み,頬０涙０",
			"171.1147215,ダンス目あけ,頬０涙０",
			"171.6118066,ダンス目つむり,頬０涙０",
			"172.7880662,ダンス目あけ,頬０涙０",
			"174.3122489,ダンス微笑み,頬０涙０",
			"174.8257555,ダンス目つむり,頬０涙０",
			"175.3060309,ダンス目あけ,頬０涙０",
			"175.8363156,ダンス目つむり,頬０涙０"
		};

		private string[] danceO4Array = new string[]
		{
			"0.0300937,ダンス目つむり,頬０涙０",
			"8.4898739,ダンス微笑み,頬０涙０",
			"9.4341606,ダンス目あけ,頬０涙０",
			"10.0139958,ダンス微笑み,頬０涙０",
			"10.511026,ダンス目つむり,頬０涙０",
			"11.2068229,ダンス微笑み,頬０涙０",
			"12.5321507,ダンス目つむり,頬０涙０",
			"13.9569041,ダンス目あけ,頬０涙０",
			"14.6526922,ダンス微笑み,頬０涙０",
			"19.0261494,ダンス目あけ,頬０涙０",
			"19.755182,ダンス微笑み,頬０涙０",
			"20.5173081,ダンス目つむり,頬０涙０",
			"21.3124873,ダンス微笑み,頬０涙０",
			"22.2070846,ダンス目つむり,頬０涙０",
			"23.5986655,ダンス微笑み,頬０涙０",
			"24.7251505,ダンス目つむり,頬０涙０",
			"25.4210702,ダンス目あけ,頬０涙０",
			"27.0444205,ダンス目つむり,頬０涙０",
			"27.5415316,ダンス微笑み,頬１涙０",
			"30.4904193,ダンス目あけ,頬０涙０",
			"31.2691017,ダンス微笑み,頬０涙０",
			"31.7329334,ダンス目つむり,頬０涙０",
			"32.3293384,ダンス目あけ,頬０涙０",
			"33.4393215,ダンス微笑み,頬０涙０",
			"35.4565748,ダンス目つむり,頬０涙０",
			"35.9701456,ダンス微笑み,頬０涙０",
			"37.2788843,ダンス目あけ,頬０涙０",
			"38.0575602,ダンス目つむり,頬０涙０",
			"38.8857498,ダンス微笑み,頬０涙０",
			"39.3994894,ダンス目あけ,頬０涙０",
			"40.3769103,ダンス微笑み,頬０涙０",
			"41.4868002,ダンス目つむり,頬０涙０",
			"41.9837109,ダンス目あけ,頬０涙０",
			"43.1932071,ダンス微笑み,頬０涙０",
			"43.9884239,ダンス目つむり,頬０涙０",
			"44.9824294,ダンス目あけ,頬０涙０",
			"46.5231635,ダンス微笑み,頬０涙０",
			"48.2643006,ダンス目つむり,頬０涙０",
			"49.2251656,ダンス微笑み,頬０涙０",
			"50.6334271,ダンス目つむり,頬０涙０",
			"51.1304413,ダンス微笑み,頬０涙０",
			"51.1470107,ダンス微笑み,頬１涙０",
			"51.6438427,ダンス目あけ,頬１涙０",
			"52.4558415,ダンス微笑み,頬１涙０",
			"53.880504,ダンス微笑み,頬０涙０",
			"54.1787474,ダンス目つむり,頬０涙０",
			"55.056724,ダンス微笑み,頬０涙０",
			"56.9122014,ダンス目つむり,頬０涙０",
			"57.4092418,ダンス微笑み,頬０涙０",
			"58.8007771,ダンス目つむり,頬０涙０",
			"59.7616555,ダンス微笑み,頬０涙０",
			"60.4575168,ダンス目あけ,頬０涙０",
			"61.3686356,ダンス目つむり,頬０涙０",
			"62.0313002,ダンス微笑み,頬０涙０",
			"62.8927915,ダンス目あけ,頬０涙０",
			"66.2897244,ダンス目つむり,頬０涙０",
			"66.7867168,ダンス目あけ,頬０涙０",
			"67.2835922,ダンス微笑み,頬０涙０",
			"67.9462585,ダンス目つむり,頬０涙０",
			"68.4432664,ダンス微笑み,頬０涙０",
			"68.9404111,ダンス目つむり,頬０涙０",
			"69.4373913,ダンス目あけ,頬０涙０",
			"71.0443031,ダンス微笑み,頬０涙０",
			"72.6678851,ダンス目あけ,頬０涙０",
			"73.1814516,ダンス微笑み,頬０涙０",
			"73.9103713,ダンス目つむり,頬０涙０",
			"74.4073741,ダンス微笑み,頬０涙０",
			"75.1858994,ダンス目あけ,頬０涙０",
			"77.2402907,ダンス目つむり,頬０涙０",
			"77.7372931,ダンス微笑み,頬０涙０",
			"78.2539182,ダンス目つむり,頬０涙０",
			"79.0491326,ダンス目あけ,頬０涙０",
			"79.5459931,ダンス目つむり,頬０涙０",
			"81.702135,ダンス微笑み,頬０涙０",
			"82.2165658,ダンス目つむり,頬０涙０",
			"83.5907478,ダンス微笑み,頬０涙０",
			"85.4962461,ダンス目つむり,頬０涙０",
			"87.6992731,ダンス目あけ,頬０涙０",
			"88.6933023,ダンス微笑み,頬０涙０",
			"90.2505457,ダンス目つむり,頬０涙０",
			"90.7639947,ダンス微笑み,頬０涙０",
			"91.3108223,ダンス目つむり,頬０涙０",
			"92.1558268,ダンス微笑み,頬０涙０",
			"93.0006007,ダンス目あけ,頬０涙０",
			"94.4419649,ダンス微笑み,頬０涙０",
			"95.4193522,ダンス目つむり,頬０涙０",
			"95.9329777,ダンス目あけ,頬０涙０",
			"96.695024,ダンス微笑み,頬０涙０",
			"97.4404872,ダンス目あけ,頬０涙０",
			"98.1528755,ダンス微笑み,頬０涙０",
			"98.6506052,ダンス目とじ,頬１涙０",
			"101.9638315,ダンス目とじ,頬０涙０",
			"103.2561702,ダンス目あけ,頬０涙０",
			"105.823832,ダンスウインク,頬０涙０",
			"106.4369603,ダンス微笑み,頬０涙０",
			"107.0663179,ダンス目つむり,頬０涙０",
			"107.5633463,ダンス目あけ,頬０涙０",
			"108.1764871,ダンス微笑み,頬０涙０",
			"109.8993619,ダンス目つむり,頬０涙０",
			"110.3963621,ダンス目あけ,頬０涙０",
			"110.9099297,ダンス微笑み,頬０涙０",
			"111.1915683,ダンス目つむり,頬０涙０",
			"111.688589,ダンス目あけ,頬０涙０",
			"112.1854276,ダンス微笑み,頬０涙０",
			"112.8647089,ダンス目つむり,頬０涙０",
			"113.3617872,ダンス目あけ,頬０涙０",
			"115.134498,ダンス微笑み,頬０涙０",
			"116.3952933,ダンス目つむり,頬０涙０",
			"117.1408757,ダンス目あけ,頬０涙０",
			"117.8366198,ダンス微笑み,頬０涙０",
			"118.350257,ダンス目あけ,頬０涙０",
			"118.8803047,ダンスウインク,頬１涙０",
			"119.8716782,ダンス微笑み,頬１涙０",
			"119.9545495,ダンス微笑み,頬０涙０",
			"120.9485145,ダンス目とじ,頬０涙０",
			"121.7602847,ダンス目あけ,頬０涙０",
			"122.3732297,ダンス微笑み,頬０涙０",
			"123.0033719,ダンス目あけ,頬０涙０",
			"123.9470725,ダンス目つむり,頬０涙０",
			"124.4772043,ダンス微笑み,頬０涙０",
			"125.3221624,ダンス目つむり,頬０涙０",
			"125.8191141,ダンス目あけ,頬０涙０",
			"126.6971811,ダンス目とじ,頬０涙０",
			"127.6580127,ダンス目あけ,頬０涙０",
			"129.132445,ダンス目つむり,頬０涙０",
			"130.2093118,ダンス微笑み,頬０涙０",
			"132.4299291,ダンス目とじ,頬０涙０",
			"133.6551457,ダンス微笑み,頬０涙０",
			"134.7319584,ダンス目あけ,頬０涙０",
			"135.8917059,ダンス微笑み,頬０涙０",
			"141.4083762,ダンスウインク,頬１涙０",
			"142.120712,ダンス微笑み,頬０涙０",
			"143.3466835,ダンス目あけ,頬０涙０",
			"144.5726317,ダンス目とじ,頬０涙０",
			"145.6328481,ダンス微笑み,頬０涙０",
			"147.2231214,ダンスウインク,頬０涙０",
			"147.9193879,ダンス微笑み,頬０涙０",
			"148.9627598,ダンス目つむり,頬０涙０",
			"149.4597608,ダンス微笑み,頬０涙０",
			"150.0395852,ダンス目あけ,頬０涙０",
			"150.7519647,ダンス微笑み,頬０涙０",
			"151.845225,ダンス目とじ,頬１涙０",
			"152.5080276,ダンス微笑み,頬０涙０",
			"153.0048935,ダンス目とじ,頬０涙０",
			"153.6511255,ダンス微笑み,頬１涙０",
			"154.3469341,ダンス目あけ,頬０涙０",
			"154.8438157,ダンス微笑み,頬０涙０",
			"156.0532957,ダンス目つむり,頬０涙０",
			"157.7927724,ダンス微笑み,頬０涙０"
		};

		private string[] danceO4BArray = new string[]
		{
			"0.0301171,ダンス目つむり,頬０涙０",
			"8.4898821,ダンス微笑み,頬０涙０",
			"9.434168,ダンス目あけ,頬０涙０",
			"10.0140041,ダンス微笑み,頬０涙０",
			"10.5110337,ダンス目つむり,頬０涙０",
			"11.2068306,ダンス微笑み,頬０涙０",
			"12.5321601,ダンス目つむり,頬０涙０",
			"13.9569135,ダンス目あけ,頬０涙０",
			"17.1542425,ダンス微笑み,頬０涙０",
			"19.0261565,ダンス目あけ,頬０涙０",
			"19.755192,ダンス微笑み,頬０涙０",
			"20.5173153,ダンス目つむり,頬０涙０",
			"21.3124955,ダンス微笑み,頬０涙０",
			"22.2070934,ダンス目つむり,頬０涙０",
			"23.598673,ダンス微笑み,頬０涙０",
			"24.7251587,ダンス目つむり,頬０涙０",
			"25.4210777,ダンス目あけ,頬０涙０",
			"27.0444276,ダンス目つむり,頬０涙０",
			"27.5415393,ダンス微笑み,頬１涙０",
			"30.490427,ダンス目あけ,頬０涙０",
			"31.2691102,ダンス微笑み,頬０涙０",
			"31.7329588,ダンス目つむり,頬０涙０",
			"32.3293463,ダンス目あけ,頬０涙０",
			"33.4393294,ダンス微笑み,頬０涙０",
			"35.4565851,ダンス目つむり,頬０涙０",
			"35.9701544,ダンス微笑み,頬０涙０",
			"37.2788923,ダンス目あけ,頬０涙０",
			"38.0575679,ダンス目つむり,頬０涙０",
			"38.8857698,ダンス微笑み,頬０涙０",
			"39.3994982,ダンス目あけ,頬０涙０",
			"40.3769209,ダンス微笑み,頬０涙０",
			"41.4868122,ダンス目つむり,頬０涙０",
			"41.9837217,ダンス目あけ,頬０涙０",
			"43.1932156,ダンス微笑み,頬０涙０",
			"43.9884361,ダンス目つむり,頬０涙０",
			"44.9824382,ダンス目あけ,頬０涙０",
			"46.7550372,ダンス微笑み,頬０涙０",
			"48.2975526,ダンス目つむり,頬０涙０",
			"49.0430606,ダンス目あけ,頬０涙０",
			"49.5566399,ダンス目つむり,頬０涙０",
			"50.2035177,ダンス目あけ,頬０涙０",
			"50.8156924,ダンス目とじ,頬０涙０",
			"51.5444641,ダンス目あけ,頬０涙０",
			"52.5053348,ダンス目つむり,頬０涙０",
			"53.0026001,ダンス目あけ,頬０涙０",
			"54.8579273,ダンス微笑み,頬０涙０",
			"55.7525326,ダンス目つむり,頬０涙０",
			"56.547712,ダンス微笑み,頬０涙０",
			"58.8007868,ダンス目つむり,頬０涙０",
			"59.7616646,ダンス微笑み,頬０涙０",
			"60.4575271,ダンス目あけ,頬０涙０",
			"61.3686453,ダンス目つむり,頬０涙０",
			"62.0313097,ダンス微笑み,頬０涙０",
			"62.8928029,ダンス目あけ,頬０涙０",
			"65.047227,ダンス目つむり,頬０涙０",
			"65.5441323,ダンス微笑み,頬０涙０",
			"66.6708379,ダンス目あけ,頬０涙０",
			"67.4825639,ダンス微笑み,頬０涙０",
			"68.3770982,ダンス目つむり,頬０涙０",
			"68.8741286,ダンス目あけ,頬０涙０",
			"68.9404222,ダンス目あけ,頬１涙０",
			"70.2169693,ダンス微笑み,頬１涙０",
			"70.8454416,ダンス微笑み,頬０涙０",
			"72.9826395,ダンス目つむり,頬０涙０",
			"73.5955882,ダンス微笑み,頬０涙０",
			"74.6060841,ダンス目つむり,頬０涙０",
			"75.119762,ダンス微笑み,頬０涙０",
			"75.6167474,ダンス目あけ,頬０涙０",
			"76.9586677,ダンス微笑み,頬０涙０",
			"78.3863306,ダンス目つむり,頬０涙０",
			"78.9000165,ダンス目あけ,頬０涙０",
			"79.6126873,ダンス目つむり,頬０涙０",
			"80.1107526,ダンス目あけ,頬０涙０",
			"80.6077454,ダンス目つむり,頬０涙０",
			"81.1876348,ダンス目あけ,頬０涙０",
			"82.2165775,ダンス目つむり,頬０涙０",
			"83.5907577,ダンス微笑み,頬０涙０",
			"85.4962567,ダンス目つむり,頬０涙０",
			"87.6992845,ダンス目あけ,頬０涙０",
			"88.6933143,ダンス微笑み,頬０涙０",
			"90.2505571,ダンス目つむり,頬０涙０",
			"90.7640053,ダンス微笑み,頬０涙０",
			"91.3108323,ダンス目つむり,頬０涙０",
			"92.1558448,ダンス微笑み,頬０涙０",
			"93.000611,ダンス目あけ,頬０涙０",
			"94.4419755,ダンス微笑み,頬０涙０",
			"95.4193619,ダンス目つむり,頬０涙０",
			"95.9329885,ダンス目あけ,頬０涙０",
			"96.6950371,ダンス微笑み,頬０涙０",
			"97.4404984,ダンス目あけ,頬０涙０",
			"98.1528875,ダンス微笑み,頬０涙０",
			"100.953386,ダンス目とじ,頬１涙０",
			"101.4669128,ダンス目あけ,頬１涙０",
			"101.9638429,ダンス目つむり,頬０涙０",
			"103.2561796,ダンス目あけ,頬０涙０",
			"105.8238411,ダンスウインク,頬０涙０",
			"106.43697,ダンス微笑み,頬０涙０",
			"107.0663293,ダンス目つむり,頬０涙０",
			"107.5633565,ダンス目あけ,頬０涙０",
			"108.1765074,ダンス微笑み,頬０涙０",
			"109.8993728,ダンス目つむり,頬０涙０",
			"110.3963732,ダンス目あけ,頬０涙０",
			"110.9099414,ダンス微笑み,頬０涙０",
			"111.19158,ダンス目つむり,頬０涙０",
			"111.6886004,ダンス目あけ,頬０涙０",
			"112.185437,ダンス微笑み,頬０涙０",
			"112.8647183,ダンス目つむり,頬０涙０",
			"113.3617995,ダンス目あけ,頬０涙０",
			"115.1345097,ダンス微笑み,頬０涙０",
			"116.3953047,ダンス目つむり,頬０涙０",
			"117.1408882,ダンス目あけ,頬０涙０",
			"117.8366303,ダンス微笑み,頬０涙０",
			"118.3502678,ダンス目あけ,頬０涙０",
			"118.8803159,ダンス微笑み,頬０涙０",
			"119.3746894,ダンス目とじ,頬０涙０",
			"121.7602953,ダンス目あけ,頬０涙０",
			"122.3732388,ダンス微笑み,頬０涙０",
			"123.0033827,ダンス目あけ,頬０涙０",
			"123.6323361,ダンス目つむり,頬０涙０",
			"124.1632066,ダンス微笑み,頬０涙０",
			"124.6595132,ダンス目あけ,頬０涙０",
			"125.3718384,ダンス目つむり,頬０涙０",
			"125.8688246,ダンス微笑み,頬０涙０",
			"126.6971919,ダンス目とじ,頬０涙０",
			"127.658023,ダンス目あけ,頬０涙０",
			"129.132455,ダンス目つむり,頬０涙０",
			"130.209322,ダンス微笑み,頬０涙０",
			"132.4299423,ダンス目とじ,頬０涙０",
			"133.6551563,ダンス微笑み,頬０涙０",
			"134.7319695,ダンス目あけ,頬０涙０",
			"135.8917176,ダンス微笑み,頬０涙０",
			"137.6974092,ダンス目とじ,頬１涙０",
			"138.4595136,ダンス目あけ,頬０涙０",
			"139.4700908,ダンス微笑み,頬０涙０",
			"141.4083865,ダンスウインク,頬１涙０",
			"142.1207223,ダンス微笑み,頬０涙０",
			"143.3466955,ダンス目あけ,頬０涙０",
			"144.5726434,ダンス目とじ,頬０涙０",
			"145.6328587,ダンス微笑み,頬０涙０",
			"147.2231311,ダンスウインク,頬０涙０",
			"147.9194007,ダンス微笑み,頬０涙０",
			"148.9627706,ダンス目つむり,頬０涙０",
			"149.4597722,ダンス微笑み,頬０涙０",
			"150.0395961,ダンス目あけ,頬０涙０",
			"150.7519758,ダンス微笑み,頬０涙０",
			"153.004904,ダンス目とじ,頬０涙０",
			"153.6511364,ダンス微笑み,頬１涙０",
			"154.3469464,ダンス目あけ,頬０涙０",
			"154.8438277,ダンス微笑み,頬０涙０",
			"156.0533076,ダンス目つむり,頬０涙０",
			"157.7927832,ダンス微笑み,頬０涙０"
		};

		private string[] danceO4CArray = new string[]
		{
			"0.030267,ダンス目つむり,頬０涙０",
			"8.4902528,ダンス微笑み,頬０涙０",
			"9.4345489,ダンス目あけ,頬０涙０",
			"10.0143576,ダンス微笑み,頬０涙０",
			"10.5112173,ダンス目つむり,頬０涙０",
			"11.2070481,ダンス微笑み,頬０涙０",
			"12.5325433,ダンス目つむり,頬０涙０",
			"13.9572822,ダンス目あけ,頬０涙０",
			"15.9450708,ダンス微笑み,頬０涙０",
			"19.0265699,ダンス目あけ,頬０涙０",
			"19.7555119,ダンス微笑み,頬０涙０",
			"20.5176534,ダンス目つむり,頬０涙０",
			"21.3128747,ダンス微笑み,頬０涙０",
			"22.2074532,ダンス目つむり,頬０涙０",
			"23.5990538,ダンス微笑み,頬０涙０",
			"24.7254285,ダンス目つむり,頬０涙０",
			"25.4214186,ダンス目あけ,頬０涙０",
			"27.0447572,ダンス目つむり,頬０涙０",
			"27.5419079,ダンス微笑み,頬１涙０",
			"30.4906357,ダンス目あけ,頬０涙０",
			"31.2694532,ダンス微笑み,頬０涙０",
			"31.7333283,ダンス目つむり,頬０涙０",
			"32.3297164,ダンス目あけ,頬０涙０",
			"33.4396881,ダンス微笑み,頬０涙０",
			"35.9704227,ダンス目つむり,頬０涙０",
			"36.5834891,ダンス微笑み,頬０涙０",
			"37.2792592,ダンス目あけ,頬０涙０",
			"38.6378299,ダンス目つむり,頬０涙０",
			"39.3998472,ダンス目あけ,頬０涙０",
			"40.3771393,ダンス微笑み,頬０涙０",
			"41.4871691,ダンス目つむり,頬０涙０",
			"41.9840864,ダンス目あけ,頬０涙０",
			"43.1935877,ダンス微笑み,頬０涙０",
			"43.9887902,ダンス目つむり,頬０涙０",
			"44.9828313,ダンス目あけ,頬０涙０",
			"46.7554055,ダンス微笑み,頬０涙０",
			"48.2977716,ダンス目つむり,頬０涙０",
			"49.0433021,ダンス目あけ,頬０涙０",
			"49.5570362,ダンス目つむり,頬０涙０",
			"50.2037298,ダンス目あけ,頬０涙０",
			"50.8161257,ダンス目とじ,頬０涙０",
			"51.5448365,ダンス目あけ,頬０涙０",
			"52.505704,ダンス目つむり,頬０涙０",
			"53.0029768,ダンス目あけ,頬０涙０",
			"54.8581331,ダンス微笑み,頬０涙０",
			"55.7528887,ダンス目つむり,頬０涙０",
			"56.5480704,ダンス微笑み,頬０涙０",
			"57.2605211,ダンス目つむり,頬０涙０",
			"57.7574403,ダンス目あけ,頬０涙０",
			"58.5195547,ダンス目つむり,頬０涙０",
			"59.0330659,ダンス微笑み,頬０涙０",
			"60.3749768,ダンス目あけ,頬０涙０",
			"61.0375841,ダンス目つむり,頬０涙０",
			"61.5347143,ダンス目あけ,頬０涙０",
			"63.3902062,ダンス目つむり,頬１涙０",
			"63.887087,ダンス微笑み,頬１涙０",
			"65.246227,ダンス目つむり,頬１涙０",
			"66.1574453,ダンス目つむり,頬０涙０",
			"66.7871129,ダンス目あけ,頬０涙０",
			"67.2838907,ダンス微笑み,頬０涙０",
			"67.9466183,ダンス目つむり,頬０涙０",
			"68.4435227,ダンス微笑み,頬０涙０",
			"68.9408539,ダンス目つむり,頬０涙０",
			"69.4377976,ダンス目あけ,頬０涙０",
			"71.0444656,ダンス微笑み,頬０涙０",
			"72.6682612,ダンス目あけ,頬０涙０",
			"73.1817877,ダンス微笑み,頬０涙０",
			"73.9107303,ダンス目つむり,頬０涙０",
			"74.4077242,ダンス微笑み,頬０涙０",
			"75.1862447,ダンス目あけ,頬０涙０",
			"76.9590466,ダンス微笑み,頬０涙０",
			"78.3867249,ダンス目つむり,頬０涙０",
			"78.9003621,ダンス目あけ,頬０涙０",
			"79.6129236,ダンス目つむり,頬０涙０",
			"80.110961,ダンス目あけ,頬０涙０",
			"80.6079615,ダンス目つむり,頬０涙０",
			"81.1878523,ダンス目あけ,頬０涙０",
			"82.2168033,ダンス目つむり,頬０涙０",
			"83.591097,ダンス微笑み,頬０涙０",
			"85.4964163,ダンス目つむり,頬０涙０",
			"87.6996526,ダンス目あけ,頬０涙０",
			"88.6937143,ダンス微笑み,頬０涙０",
			"90.2508847,ダンス目つむり,頬０涙０",
			"90.7643659,ダンス微笑み,頬０涙０",
			"91.311033,ダンス目つむり,頬０涙０",
			"92.1562069,ダンス微笑み,頬０涙０",
			"93.0009796,ダンス目あけ,頬０涙０",
			"94.4422668,ダンス微笑み,頬０涙０",
			"95.4196901,ダンス目つむり,頬０涙０",
			"95.9333575,ダンス目あけ,頬０涙０",
			"96.6953932,ダンス微笑み,頬０涙０",
			"97.4408787,ダンス目あけ,頬０涙０",
			"98.1531666,ダンス微笑み,頬０涙０",
			"99.8436332,ダンス目とじ,頬１涙０",
			"101.9641684,ダンス目とじ,頬０涙０",
			"103.2564219,ダンス目あけ,頬０涙０",
			"105.8242265,ダンスウインク,頬０涙０",
			"106.4373528,ダンス微笑み,頬０涙０",
			"107.0666728,ダンス目つむり,頬０涙０",
			"107.5636736,ダンス目あけ,頬０涙０",
			"108.1767973,ダンス微笑み,頬０涙０",
			"109.8996593,ダンス目つむり,頬０涙０",
			"110.396755,ダンス目あけ,頬０涙０",
			"110.9103152,ダンス微笑み,頬０涙０",
			"111.1917779,ダンス目つむり,頬０涙０",
			"111.6889682,ダンス目あけ,頬０涙０",
			"112.1858384,ダンス微笑み,頬０涙０",
			"112.8651149,ダンス目つむり,頬０涙０",
			"113.3620264,ダンス目あけ,頬０涙０",
			"115.1349185,ダンス微笑み,頬０涙０",
			"116.3956696,ダンス目つむり,頬０涙０",
			"117.1411542,ダンス目あけ,頬０涙０",
			"117.8370187,ダンス微笑み,頬０涙０",
			"118.3504873,ダンス目あけ,頬０涙０",
			"118.8806825,ダンス微笑み,頬０涙０",
			"119.3750854,ダンス目とじ,頬０涙０",
			"119.8720354,ダンス微笑み,頬０涙０",
			"121.0151685,ダンス目つむり,頬０涙０",
			"121.5121669,ダンス目あけ,頬０涙０",
			"122.472998,ダンス目つむり,頬０涙０",
			"123.0036484,ダンス微笑み,頬１涙０",
			"123.9474573,ダンス目あけ,頬１涙０",
			"124.2621059,ダンス目あけ,頬０涙０",
			"124.4775798,ダンス微笑み,頬０涙０",
			"125.3226425,ダンス目つむり,頬０涙０",
			"125.8194474,ダンス目あけ,頬０涙０",
			"126.6974442,ダンス目とじ,頬０涙０",
			"127.658393,ダンス目あけ,頬０涙０",
			"129.1328242,ダンス目つむり,頬０涙０",
			"130.2097309,ダンス微笑み,頬０涙０",
			"132.4301299,ダンス目とじ,頬０涙０",
			"133.6554439,ダンス微笑み,頬０涙０",
			"134.7323581,ダンス目あけ,頬０涙０",
			"135.8920625,ダンス微笑み,頬０涙０",
			"137.6977581,ダンス目とじ,頬１涙０",
			"138.4596958,ダンス目あけ,頬０涙０",
			"139.4703078,ダンス微笑み,頬０涙０",
			"143.3470861,ダンス目あけ,頬０涙０",
			"144.5730374,ダンス目とじ,頬０涙０",
			"145.6331894,ダンス微笑み,頬０涙０",
			"147.2234581,ダンスウインク,頬０涙０",
			"147.9212969,ダンス微笑み,頬０涙０",
			"148.962985,ダンス目つむり,頬０涙０",
			"149.46001,ダンス微笑み,頬０涙０",
			"150.0398333,ダンス目あけ,頬０涙０",
			"150.7521919,ダンス微笑み,頬０涙０",
			"151.8456175,ダンス目とじ,頬１涙０",
			"152.5084133,ダンス微笑み,頬０涙０",
			"154.347325,ダンス目あけ,頬０涙０",
			"154.8441034,ダンス微笑み,頬０涙０",
			"156.0536917,ダンス目つむり,頬０涙０",
			"157.7931504,ダンス微笑み,頬０涙０"
		};

		private string[] danceO5Array = new string[]
		{
			"0.0391388,ダンス目つむり,頬０涙０",
			"8.0841753,ダンス目あけ,頬０涙０",
			"9.1444457,ダンス目つむり,頬０涙０",
			"12.6400032,ダンス目あけ,頬０涙０",
			"13.4022622,ダンス目とじ,頬０涙０",
			"13.899072,ダンス目あけ,頬０涙０",
			"14.3960895,ダンス目つむり,頬０涙０",
			"14.8930578,ダンス目あけ,頬０涙０",
			"15.8539179,ダンス目つむり,頬０涙０",
			"16.3508269,ダンス目あけ,頬０涙０",
			"18.0075976,ダンス微笑み,頬０涙０",
			"19.2998186,ダンス目あけ,頬０涙０",
			"20.6966043,ダンス目つむり,頬０涙０",
			"21.806584,ダンス目あけ,頬０涙０",
			"23.446691,ダンス微笑み,頬０涙０",
			"24.3081962,ダンスウインク,頬１涙０",
			"24.9706775,ダンスウインク,頬０涙０",
			"24.987428,ダンス目あけ,頬０涙０",
			"25.8823534,ダンス微笑み,頬０涙０",
			"26.6440465,ダンス目つむり,頬０涙０",
			"27.6547483,ダンス微笑み,頬０涙０",
			"29.2948196,ダンス目あけ,頬０涙０",
			"29.8082863,ダンス目つむり,頬０涙０",
			"30.3052614,ダンス目あけ,頬０涙０",
			"31.580938,ダンスウインク,頬０涙０",
			"32.0779197,ダンス微笑み,頬０涙０",
			"33.4529954,ダンスウインク,頬０涙０",
			"33.9498556,ダンス目あけ,頬０涙０",
			"34.6457936,ダンス目つむり,頬０涙０",
			"35.4078439,ダンス目あけ,頬０涙０",
			"36.7828292,ダンス微笑み,頬０涙０",
			"37.7791121,ダンス目つむり,頬０涙０",
			"38.9553472,ダンス目あけ,頬０涙０",
			"39.9493284,ダンス微笑み,頬０涙０",
			"40.7942571,ダンス目とじ,頬０涙０",
			"41.3077934,ダンス微笑み,頬０涙０",
			"42.2521633,ダンス目つむり,頬０涙０",
			"43.3786765,ダンス微笑み,頬０涙０",
			"43.8756844,ダンス目つむり,頬０涙０",
			"44.9690291,ダンス目あけ,頬０涙０",
			"45.795576,ダンス目つむり,頬０涙０",
			"46.756447,ダンスウインク,頬０涙０",
			"47.5846865,ダンス目あけ,頬０涙０",
			"48.0819792,ダンス目つむり,頬０涙０",
			"49.9868549,ダンス微笑み,頬０涙０",
			"51.7760496,ダンス目つむり,頬０涙０",
			"53.1345202,ダンス微笑み,頬０涙０",
			"53.9794261,ダンス目つむり,頬０涙０",
			"54.9898892,ダンス目あけ,頬０涙０",
			"55.7023582,ダンス目つむり,頬０涙０",
			"56.1993521,ダンス微笑み,頬０涙０",
			"57.6075261,ダンス目とじ,頬０涙０",
			"59.0321464,ダンス目あけ,頬０涙０",
			"60.20854,ダンスウインク,頬０涙０",
			"60.8710957,ダンス目あけ,頬０涙０",
			"61.3681917,ダンス目とじ,頬０涙０",
			"61.8651753,ダンス目あけ,頬０涙０",
			"63.0579636,ダンス微笑み,頬０涙０",
			"63.8365956,ダンス目とじ,頬０涙０",
			"64.5489772,ダンス微笑み,頬０涙０",
			"65.1288097,ダンス目つむり,頬０涙０",
			"66.2884385,ダンス目あけ,頬０涙０",
			"66.7854755,ダンス微笑み,頬０涙０",
			"68.0611279,ダンス目あけ,頬０涙０",
			"69.0053978,ダンス微笑み,頬０涙０",
			"70.6290241,ダンスウインク,頬０涙０",
			"71.2750374,ダンス目あけ,頬０涙０",
			"71.9542587,ダンス微笑み,頬０涙０",
			"73.7931598,ダンス目つむり,頬０涙０",
			"74.3067511,ダンス目あけ,頬０涙０",
			"74.8037051,ダンスウインク,頬０涙０",
			"75.6819214,ダンス微笑み,頬０涙０",
			"76.7586149,ダンス目つむり,頬０涙０",
			"77.6034874,ダンス微笑み,頬０涙０",
			"79.9559955,ダンス目つむり,頬０涙０",
			"80.4530618,ダンス微笑み,頬０涙０",
			"81.6457825,ダンス目あけ,頬０涙０",
			"82.6397911,ダンス目つむり,頬０涙０",
			"83.5508039,ダンス目あけ,頬０涙０",
			"84.2964179,ダンス微笑み,頬０涙０",
			"85.8039659,ダンス目つむり,頬０涙０",
			"86.8974095,ダンス微笑み,頬０涙０",
			"87.6428907,ダンス目あけ,頬０涙０",
			"88.5043834,ダンス目つむり,頬０涙０",
			"90.1776415,ダンス目あけ,頬０涙０",
			"90.9562698,ダンス微笑み,頬０涙０",
			"91.5858224,ダンス目つむり,頬０涙０",
			"92.182158,ダンス微笑み,頬０涙０",
			"93.1430589,ダンス目あけ,頬０涙０",
			"95.3961155,ダンスウインク,頬０涙０",
			"96.1581507,ダンス目あけ,頬０涙０",
			"97.9306798,ダンス目つむり,頬０涙０",
			"98.4278687,ダンス目あけ,頬０涙０",
			"98.9413391,ダンスウインク,頬０涙０",
			"99.4383521,ダンス目あけ,頬０涙０",
			"100.3826659,ダンス微笑み,頬０涙０",
			"100.8796427,ダンス目つむり,頬０涙０",
			"102.3044207,ダンス目あけ,頬０涙０",
			"103.4475032,ダンス目とじ,頬０涙０",
			"103.9444894,ダンス目あけ,頬０涙０",
			"104.491136,ダンス目つむり,頬０涙０",
			"104.9889949,ダンス微笑み,頬０涙０",
			"105.4852202,ダンス目つむり,頬０涙０",
			"105.9823327,ダンス微笑み,頬０涙０",
			"106.4792365,ダンス目あけ,頬０涙０",
			"106.9761874,ダンスウインク,頬０涙０",
			"107.4897025,ダンス目あけ,頬０涙０",
			"107.9868301,ダンス目つむり,頬０涙０",
			"108.4505324,ダンス目あけ,頬０涙０",
			"108.947735,ダンス微笑み,頬０涙０",
			"109.8917982,ダンス目とじ,頬０涙０",
			"110.736809,ダンス目あけ,頬０涙０",
			"111.2338138,ダンス微笑み,頬０涙０",
			"111.8300862,ダンス目つむり,頬０涙０",
			"112.3272876,ダンス目あけ,頬０涙０",
			"112.8408555,ダンス目つむり,頬０涙０",
			"113.3378075,ダンス目あけ,頬０涙０",
			"113.8347997,ダンス目とじ,頬０涙０",
			"114.89508,ダンス目あけ,頬０涙０",
			"115.408704,ダンス微笑み,頬０涙０",
			"116.3860629,ダンスウインク,頬０涙０",
			"117.3635146,ダンス微笑み,頬０涙０",
			"118.2415759,ダンス目あけ,頬０涙０",
			"118.7385558,ダンス目とじ,頬０涙０",
			"119.2355606,ダンス微笑み,頬０涙０",
			"119.6331261,ダンスウインク,頬０涙０",
			"122.8139356,ダンス微笑み,頬０涙０",
			"124.3380883,ダンス目あけ,頬０涙０",
			"125.0338752,ダンスウインク,頬０涙０",
			"125.530732,ダンス目あけ,頬０涙０",
			"126.0285499,ダンス目つむり,頬０涙０",
			"126.5414361,ダンス目あけ,頬０涙０",
			"127.0384443,ダンス目とじ,頬０涙０",
			"127.8336912,ダンス目あけ,頬０涙０",
			"128.3305494,ダンス微笑み,頬０涙０",
			"128.7447901,ダンス微笑み,頬１涙０",
			"128.8277104,ダンス目つむり,頬１涙０",
			"129.3412353,ダンス目あけ,頬１涙０",
			"130.6333903,ダンスウインク,頬１涙０",
			"131.4783239,ダンス微笑み,頬１涙０",
			"131.6107683,ダンス微笑み,頬０涙０",
			"132.4391538,ダンス目とじ,頬０涙０",
			"132.9361725,ダンス目あけ,頬０涙０",
			"133.9300437,ダンス目つむり,頬０涙０",
			"134.4271226,ダンス目あけ,頬０涙０",
			"135.172565,ダンス微笑み,頬０涙０",
			"136.1832631,ダンス目つむり,頬０涙０",
			"137.0283152,ダンス微笑み,頬０涙０",
			"137.5417588,ダンス目つむり,頬０涙０",
			"138.0386589,ダンス目あけ,頬０涙０",
			"138.5357062,ダンスウインク,頬０涙０",
			"139.3640253,ダンス微笑み,頬０涙０",
			"141.6336763,ダンス目つむり,頬０涙０",
			"142.1305633,ダンス目あけ,頬０涙０",
			"142.8097791,ダンス微笑み,頬０涙０",
			"143.3236751,ダンス目つむり,頬０涙０",
			"143.820421,ダンス目あけ,頬０涙０",
			"144.3174528,ダンス微笑み,頬０涙０",
			"144.8147946,ダンス目つむり,頬０涙０",
			"145.3113443,ダンス目あけ,頬０涙０",
			"146.6202436,ダンス微笑み,頬０涙０",
			"147.1185997,ダンス目とじ,頬０涙０",
			"147.6155905,ダンス目あけ,頬０涙０",
			"148.8911582,ダンス微笑み,頬０涙０",
			"149.6864394,ダンス目つむり,頬０涙０",
			"150.1834578,ダンス微笑み,頬０涙０",
			"152.8341155,ダンス目あけ,頬０涙０",
			"153.712065,ダンス目とじ,頬０涙０",
			"154.2091111,ダンス微笑み,頬０涙０",
			"154.7227094,ダンス目あけ,頬０涙０",
			"155.5842329,ダンス目とじ,頬０涙０",
			"156.0812536,ダンス目あけ,頬０涙０",
			"156.57816,ダンス微笑み,頬０涙０",
			"158.4833801,ダンス目つむり,頬０涙０",
			"159.8417969,ダンス目あけ,頬０涙０",
			"160.3388102,ダンス微笑み,頬０涙０",
			"161.3326769,ダンス目とじ,頬０涙０",
			"162.3267981,ダンス目あけ,頬０涙０",
			"163.4367732,ダンス微笑み,頬０涙０",
			"163.9338447,ダンス目つむり,頬０涙０",
			"164.4306784,ダンス目あけ,頬０涙０",
			"165.8389573,ダンス微笑み,頬０涙０",
			"166.6175773,ダンス目とじ,頬０涙０",
			"167.4293709,ダンス目あけ,頬０涙０",
			"167.9263454,ダンス微笑み,頬０涙０",
			"168.4398956,ダンス目あけ,頬０涙０",
			"169.3352858,ダンス目つむり,頬０涙０",
			"170.345136,ダンス目あけ,頬０涙０",
			"172.6478583,ダンス微笑み,頬０涙０",
			"173.393478,ダンス目つむり,頬０涙０",
			"175.132827,ダンス目あけ,頬０涙０",
			"177.0551508,ダンス微笑み,頬０涙０",
			"178.5952671,ダンス目つむり,頬０涙０",
			"179.1088213,ダンス目あけ,頬０涙０",
			"179.6058269,ダンス微笑み,頬０涙０",
			"180.1027838,ダンス目つむり,頬０涙０",
			"180.5998646,ダンス目あけ,頬０涙０",
			"181.0968018,ダンス目つむり,頬０涙０",
			"181.5938382,ダンス目あけ,頬０涙０",
			"182.1074871,ダンス目つむり,頬０涙０",
			"182.9025812,ダンス目あけ,頬０涙０",
			"184.3439437,ダンスウインク,頬０涙０"
		};

		private string[] danceO5BArray = new string[]
		{
			"0.0392137,ダンス目つむり,頬０涙０",
			"8.0841833,ダンス目あけ,頬０涙０",
			"9.1444536,ダンス目つむり,頬０涙０",
			"12.6400115,ダンス目あけ,頬０涙０",
			"13.4022725,ダンス目とじ,頬０涙０",
			"13.8990805,ダンス目あけ,頬０涙０",
			"14.3960967,ダンス目つむり,頬０涙０",
			"14.8930709,ダンス目あけ,頬０涙０",
			"15.8539373,ダンス目つむり,頬０涙０",
			"16.3508346,ダンス目あけ,頬０涙０",
			"18.0076062,ダンス微笑み,頬０涙０",
			"19.299828,ダンス目あけ,頬０涙０",
			"20.6966129,ダンス目つむり,頬０涙０",
			"21.8065988,ダンス目あけ,頬０涙０",
			"23.4466987,ダンス微笑み,頬０涙０",
			"24.3082136,ダンスウインク,頬１涙０",
			"24.9706858,ダンスウインク,頬０涙０",
			"24.9874374,ダンス目あけ,頬０涙０",
			"25.8823614,ダンス微笑み,頬０涙０",
			"26.6440551,ダンス目つむり,頬０涙０",
			"27.6547566,ダンス微笑み,頬０涙０",
			"29.2948519,ダンス目あけ,頬０涙０",
			"29.808296,ダンス目つむり,頬０涙０",
			"30.3052697,ダンス目あけ,頬０涙０",
			"31.5809477,ダンスウインク,頬０涙０",
			"32.0779362,ダンス微笑み,頬０涙０",
			"33.4530085,ダンスウインク,頬０涙０",
			"33.9498718,ダンス目あけ,頬０涙０",
			"34.6458195,ダンス目つむり,頬０涙０",
			"35.407859,ダンス目あけ,頬０涙０",
			"36.7828369,ダンス微笑み,頬０涙０",
			"37.7791258,ダンス目つむり,頬０涙０",
			"38.9553557,ダンス目あけ,頬０涙０",
			"39.9493467,ダンス微笑み,頬０涙０",
			"40.7942648,ダンス目とじ,頬０涙０",
			"41.3078068,ダンス微笑み,頬０涙０",
			"42.2521719,ダンス目つむり,頬０涙０",
			"43.3786939,ダンス微笑み,頬０涙０",
			"43.8756981,ダンス目つむり,頬０涙０",
			"44.9690422,ダンス目あけ,頬０涙０",
			"45.7955857,ダンス目つむり,頬０涙０",
			"46.7564547,ダンスウインク,頬０涙０",
			"47.5846959,ダンス目あけ,頬０涙０",
			"48.0819892,ダンス目つむり,頬０涙０",
			"49.9868641,ダンス微笑み,頬０涙０",
			"51.7760587,ダンス目つむり,頬０涙０",
			"53.1345302,ダンス微笑み,頬０涙０",
			"53.9794372,ダンス目つむり,頬０涙０",
			"54.9899007,ダンス目あけ,頬０涙０",
			"55.7023667,ダンス目つむり,頬０涙０",
			"56.1993618,ダンス微笑み,頬０涙０",
			"57.607535,ダンス目とじ,頬０涙０",
			"59.0321572,ダンス目あけ,頬０涙０",
			"60.2085505,ダンスウインク,頬０涙０",
			"60.8711048,ダンス目あけ,頬０涙０",
			"61.368202,ダンス目とじ,頬０涙０",
			"61.865185,ダンス目あけ,頬０涙０",
			"63.0579721,ダンス微笑み,頬０涙０",
			"63.836607,ダンス目とじ,頬０涙０",
			"64.5489878,ダンス微笑み,頬０涙０",
			"65.1288202,ダンス目つむり,頬０涙０",
			"66.2884482,ダンス目あけ,頬０涙０",
			"66.7854855,ダンス微笑み,頬０涙０",
			"68.0611387,ダンス目あけ,頬０涙０",
			"69.0054084,ダンス微笑み,頬０涙０",
			"70.629036,ダンスウインク,頬０涙０",
			"71.275048,ダンス目あけ,頬０涙０",
			"71.9542681,ダンス微笑み,頬０涙０",
			"73.7931692,ダンス目つむり,頬０涙０",
			"74.3067616,ダンス目あけ,頬０涙０",
			"74.8037167,ダンスウインク,頬０涙０",
			"75.6819317,ダンス微笑み,頬０涙０",
			"76.7586248,ダンス目つむり,頬０涙０",
			"77.6034968,ダンス微笑み,頬０涙０",
			"79.9560046,ダンス目つむり,頬０涙０",
			"80.4530724,ダンス微笑み,頬０涙０",
			"81.645793,ダンス目あけ,頬０涙０",
			"82.6398,ダンス目つむり,頬０涙０",
			"83.5508144,ダンス目あけ,頬０涙０",
			"84.2964276,ダンス微笑み,頬０涙０",
			"85.8039756,ダンス目つむり,頬０涙０",
			"86.8974192,ダンス微笑み,頬０涙０",
			"87.6429018,ダンス目あけ,頬０涙０",
			"88.504394,ダンス目つむり,頬０涙０",
			"90.1776518,ダンス目あけ,頬０涙０",
			"90.9562809,ダンス微笑み,頬０涙０",
			"91.5858335,ダンス目つむり,頬０涙０",
			"92.1821683,ダンス微笑み,頬０涙０",
			"93.1430789,ダンス目あけ,頬０涙０",
			"95.396126,ダンスウインク,頬０涙０",
			"96.1581618,ダンス目あけ,頬０涙０",
			"97.9306901,ダンス目つむり,頬０涙０",
			"98.4278918,ダンス目あけ,頬０涙０",
			"98.9413474,ダンスウインク,頬０涙０",
			"99.4383615,ダンス目あけ,頬０涙０",
			"100.3826767,ダンス微笑み,頬０涙０",
			"100.8796527,ダンス目つむり,頬０涙０",
			"102.3044321,ダンス目あけ,頬０涙０",
			"103.4475143,ダンス目とじ,頬０涙０",
			"103.9444991,ダンス目あけ,頬０涙０",
			"104.4911468,ダンス目つむり,頬０涙０",
			"104.9890057,ダンス微笑み,頬０涙０",
			"105.4852316,ダンス目つむり,頬０涙０",
			"105.9823447,ダンス微笑み,頬０涙０",
			"106.4792476,ダンス目あけ,頬０涙０",
			"106.9761991,ダンスウインク,頬０涙０",
			"107.4897128,ダンス目あけ,頬０涙０",
			"107.9868418,ダンス目つむり,頬０涙０",
			"108.4505432,ダンス目あけ,頬０涙０",
			"108.9477587,ダンス微笑み,頬０涙０",
			"109.8918085,ダンス目とじ,頬０涙０",
			"110.736819,ダンス目あけ,頬０涙０",
			"111.2338235,ダンス微笑み,頬０涙０",
			"111.8300964,ダンス目つむり,頬０涙０",
			"112.3273082,ダンス目あけ,頬０涙０",
			"112.8408655,ダンス目つむり,頬０涙０",
			"113.3378178,ダンス目あけ,頬０涙０",
			"113.8348131,ダンス目とじ,頬０涙０",
			"114.8950908,ダンス目あけ,頬０涙０",
			"115.4087135,ダンス微笑み,頬０涙０",
			"116.3860737,ダンスウインク,頬０涙０",
			"117.3635252,ダンス微笑み,頬０涙０",
			"118.2415899,ダンス目あけ,頬０涙０",
			"118.7385675,ダンス目とじ,頬０涙０",
			"119.2355728,ダンス微笑み,頬０涙０",
			"119.633136,ダンスウインク,頬０涙０",
			"122.8139473,ダンス微笑み,頬０涙０",
			"124.3381012,ダンス目あけ,頬０涙０",
			"125.0338869,ダンスウインク,頬０涙０",
			"125.5307409,ダンス目あけ,頬０涙０",
			"126.028561,ダンス目つむり,頬０涙０",
			"126.5414481,ダンス目あけ,頬０涙０",
			"127.0384539,ダンス目とじ,頬０涙０",
			"127.8337018,ダンス目あけ,頬０涙０",
			"128.3305609,ダンス微笑み,頬０涙０",
			"128.7448029,ダンス微笑み,頬１涙０",
			"128.8277401,ダンス目つむり,頬１涙０",
			"129.3412555,ダンス目あけ,頬１涙０",
			"130.6334015,ダンスウインク,頬１涙０",
			"131.4783345,ダンス微笑み,頬１涙０",
			"131.6107777,ダンス微笑み,頬０涙０",
			"132.4391638,ダンス目とじ,頬０涙０",
			"132.9361842,ダンス目あけ,頬０涙０",
			"133.9300543,ダンス目つむり,頬０涙０",
			"134.4271314,ダンス目あけ,頬０涙０",
			"135.172577,ダンス微笑み,頬０涙０",
			"136.1832742,ダンス目つむり,頬０涙０",
			"137.0283278,ダンス微笑み,頬０涙０",
			"137.5417705,ダンス目つむり,頬０涙０",
			"138.0386698,ダンス目あけ,頬０涙０",
			"138.5357164,ダンスウインク,頬０涙０",
			"139.3640349,ダンス微笑み,頬０涙０",
			"141.6336874,ダンス目つむり,頬０涙０",
			"142.1305833,ダンス目あけ,頬０涙０",
			"142.8097888,ダンス微笑み,頬０涙０",
			"143.3236891,ダンス目つむり,頬０涙０",
			"143.8204324,ダンス目あけ,頬０涙０",
			"144.3174642,ダンス微笑み,頬０涙０",
			"144.8148057,ダンス目つむり,頬０涙０",
			"145.3113557,ダンス目あけ,頬０涙０",
			"146.6439909,ダンス微笑み,頬０涙０",
			"147.1186102,ダンス目とじ,頬０涙０",
			"147.6156038,ダンス目あけ,頬０涙０",
			"148.891169,ダンス微笑み,頬０涙０",
			"149.6864499,ダンス目つむり,頬０涙０",
			"150.1834692,ダンス微笑み,頬０涙０",
			"152.8341266,ダンス目あけ,頬０涙０",
			"153.7120764,ダンス目とじ,頬０涙０",
			"154.2091233,ダンス微笑み,頬０涙０",
			"154.7227211,ダンス目あけ,頬０涙０",
			"155.5842438,ダンス目とじ,頬０涙０",
			"156.0812636,ダンス目あけ,頬０涙０",
			"156.5781714,ダンス微笑み,頬０涙０",
			"158.4834052,ダンス目つむり,頬０涙０",
			"159.8418086,ダンス目あけ,頬０涙０",
			"160.3388213,ダンス微笑み,頬０涙０",
			"161.3326871,ダンス目とじ,頬０涙０",
			"162.3268089,ダンス目あけ,頬０涙０",
			"163.4367843,ダンス微笑み,頬０涙０",
			"163.9338564,ダンス目つむり,頬０涙０",
			"164.4306898,ダンス目あけ,頬０涙０",
			"165.8389696,ダンス微笑み,頬０涙０",
			"166.617589,ダンス目とじ,頬０涙０",
			"167.4293823,ダンス目あけ,頬０涙０",
			"167.9263557,ダンス微笑み,頬０涙０",
			"168.4399065,ダンス目あけ,頬０涙０",
			"169.3352978,ダンス目つむり,頬０涙０",
			"170.3451465,ダンス目あけ,頬０涙０",
			"172.6478694,ダンス微笑み,頬０涙０",
			"173.393492,ダンス目つむり,頬０涙０",
			"175.1328378,ダンス目あけ,頬０涙０",
			"177.0551625,ダンス微笑み,頬０涙０",
			"178.5952783,ダンス目つむり,頬０涙０",
			"179.1088342,ダンス目あけ,頬０涙０",
			"179.6058383,ダンス微笑み,頬０涙０",
			"180.1027943,ダンス目つむり,頬０涙０",
			"180.5998761,ダンス目あけ,頬０涙０",
			"181.0968129,ダンス目つむり,頬０涙０",
			"181.5938491,ダンス目あけ,頬０涙０",
			"182.1074996,ダンス目つむり,頬０涙０",
			"182.9025912,ダンス目あけ,頬０涙０",
			"184.3439551,ダンスウインク,頬０涙０"
		};

		private string[] danceO5CArray = new string[]
		{
			"0.0395724,ダンス目つむり,頬０涙０",
			"8.0845329,ダンス目あけ,頬０涙０",
			"9.1447875,ダンス目つむり,頬０涙０",
			"12.6403915,ダンス目あけ,頬０涙０",
			"13.402506,ダンス目とじ,頬０涙０",
			"13.8994238,ダンス目あけ,頬０涙０",
			"14.3964622,ダンス目つむり,頬０涙０",
			"14.8934028,ダンス目あけ,頬０涙０",
			"15.8543307,ダンス目つむり,頬０涙０",
			"16.3512542,ダンス目あけ,頬０涙０",
			"18.0078879,ダンス微笑み,頬０涙０",
			"19.3001886,ダンス目あけ,頬０涙０",
			"20.6968144,ダンス目つむり,頬０涙０",
			"21.806956,ダンス目あけ,頬０涙０",
			"23.4470431,ダンス微笑み,頬０涙０",
			"24.308584,ダンスウインク,頬１涙０",
			"24.9710236,ダンスウインク,頬０涙０",
			"24.9878457,ダンス目あけ,頬０涙０",
			"25.8825499,ダンス微笑み,頬０涙０",
			"26.6442566,ダンス目つむり,頬０涙０",
			"27.6550445,ダンス微笑み,頬０涙０",
			"29.2951615,ダンス目あけ,頬０涙０",
			"29.8087083,ダンス目つむり,頬０涙０",
			"30.3055839,ダンス目あけ,頬０涙０",
			"31.5812314,ダンスウインク,頬０涙０",
			"32.0782875,ダンス微笑み,頬０涙０",
			"33.4533763,ダンスウインク,頬０涙０",
			"33.9500591,ダンス目あけ,頬０涙０",
			"34.6462429,ダンス目つむり,頬０涙０",
			"35.4082245,ダンス目あけ,頬０涙０",
			"36.7831705,ダンス微笑み,頬０涙０",
			"37.7793023,ダンス目つむり,頬０涙０",
			"38.9557141,ダンス目あけ,頬０涙０",
			"39.9496144,ダンス微笑み,頬０涙０",
			"40.7946201,ダンス目とじ,頬０涙０",
			"41.3081788,ダンス微笑み,頬０涙０",
			"42.2524599,ダンス目つむり,頬０涙０",
			"42.749486,ダンス微笑み,頬０涙０",
			"43.2464477,ダンス目つむり,頬０涙０",
			"44.9693293,ダンス目あけ,頬０涙０",
			"45.7959846,ダンス目つむり,頬０涙０",
			"46.7568185,ダンスウインク,頬０涙０",
			"47.5850572,ダンス目あけ,頬０涙０",
			"48.0821831,ダンス目つむり,頬０涙０",
			"49.9872469,ダンス微笑み,頬０涙０",
			"51.7764265,ダンス目つむり,頬０涙０",
			"53.1347748,ダンス微笑み,頬０涙０",
			"53.9800077,ダンス目つむり,頬０涙０",
			"54.9902288,ダンス目あけ,頬０涙０",
			"55.7026732,ダンス目つむり,頬０涙０",
			"56.1997338,ダンス微笑み,頬０涙０",
			"57.6079113,ダンス目とじ,頬０涙０",
			"59.0324321,ダンス目あけ,頬０涙０",
			"60.208924,ダンスウインク,頬０涙０",
			"60.8714495,ダンス目あけ,頬０涙０",
			"61.3685643,ダンス目とじ,頬０涙０",
			"61.8655314,ダンス目あけ,頬０涙０",
			"63.0583339,ダンス微笑み,頬０涙０",
			"63.8369744,ダンス目とじ,頬０涙０",
			"64.5493445,ダンス微笑み,頬０涙０",
			"65.1292456,ダンス目つむり,頬０涙０",
			"66.2888348,ダンス目あけ,頬０涙０",
			"66.7858356,ダンス微笑み,頬０涙０",
			"68.0615062,ダンス目あけ,頬０涙０",
			"69.0057582,ダンス微笑み,頬０涙０",
			"70.6292427,ダンスウインク,頬０涙０",
			"71.2753847,ダンス目あけ,頬０涙０",
			"71.9546575,ダンス微笑み,頬０涙０",
			"72.9817907,ダンス目とじ,頬０涙０",
			"73.6444636,ダンス目あけ,頬０涙０",
			"75.8974432,ダンス微笑み,頬０涙０",
			"77.6038777,ダンスウインク,頬０涙０",
			"77.8853145,ダンスウインク,頬１涙０",
			"78.9955328,ダンス目あけ,頬１涙０",
			"79.7906241,ダンス目あけ,頬０涙０",
			"79.9564,ダンス目つむり,頬０涙０",
			"80.4534587,ダンス微笑み,頬０涙０",
			"81.6460644,ダンス目あけ,頬０涙０",
			"82.6402139,ダンス目つむり,頬０涙０",
			"83.5511993,ダンス目あけ,頬０涙０",
			"84.2966232,ダンス微笑み,頬０涙０",
			"85.8042696,ダンス目つむり,頬０涙０",
			"86.8976368,ダンス微笑み,頬０涙０",
			"87.6431208,ダンス目あけ,頬０涙０",
			"88.5047398,ダンス目つむり,頬０涙０",
			"90.5920025,ダンス微笑み,頬０涙０",
			"91.3044683,ダンス目つむり,頬０涙０",
			"91.9009066,ダンス微笑み,頬０涙０",
			"93.1434883,ダンス目あけ,頬０涙０",
			"95.3964904,ダンスウインク,頬０涙０",
			"96.1585797,ダンス目あけ,頬０涙０",
			"97.9310635,ダンス目つむり,頬０涙０",
			"98.4282271,ダンス目あけ,頬０涙０",
			"98.9417171,ダンスウインク,頬０涙０",
			"99.438719,ダンス目あけ,頬０涙０",
			"100.3830457,ダンス微笑み,頬０涙０",
			"100.8800376,ダンス目つむり,頬０涙０",
			"102.3048269,ダンス目あけ,頬０涙０",
			"103.4479035,ダンス目とじ,頬０涙０",
			"103.9448638,ダンス目あけ,頬０涙０",
			"104.4915157,ダンス目つむり,頬０涙０",
			"104.989319,ダンス微笑み,頬０涙０",
			"105.4855748,ダンス目つむり,頬０涙０",
			"105.9827298,ダンス微笑み,頬０涙０",
			"106.4796388,ダンス目あけ,頬０涙０",
			"106.9794549,ダンスウインク,頬０涙０",
			"107.4900905,ダンス目あけ,頬０涙０",
			"107.9872518,ダンス目つむり,頬０涙０",
			"108.4509164,ダンス目あけ,頬０涙０",
			"108.9481644,ダンス微笑み,頬０涙０",
			"109.8922216,ダンス目とじ,頬０涙０",
			"110.7372102,ダンス目あけ,頬０涙０",
			"111.2341405,ダンス微笑み,頬０涙０",
			"111.830479,ダンス目つむり,頬０涙０",
			"112.3275901,ダンス目あけ,頬０涙０",
			"112.8411412,ダンス目つむり,頬０涙０",
			"113.3381123,ダンス目あけ,頬０涙０",
			"113.8351487,ダンス目とじ,頬０涙０",
			"114.8952818,ダンス目あけ,頬０涙０",
			"115.4091154,ダンス微笑み,頬０涙０",
			"116.7011856,ダンスウインク,頬０涙０",
			"117.3639075,ダンス微笑み,頬０涙０",
			"118.241845,ダンス目あけ,頬０涙０",
			"118.7388703,ダンス目とじ,頬０涙０",
			"119.2359386,ダンス微笑み,頬０涙０",
			"119.6335001,ダンスウインク,頬０涙０",
			"120.2796127,ダンス微笑み,頬０涙０",
			"121.8037612,ダンス目あけ,頬０涙０",
			"122.4995459,ダンスウインク,頬０涙０",
			"122.9965734,ダンス目あけ,頬０涙０",
			"123.4935362,ダンス目つむり,頬０涙０",
			"124.0070742,ダンス目あけ,頬０涙０",
			"124.5041582,ダンス目とじ,頬０涙０",
			"125.2991315,ダンス目あけ,頬０涙０",
			"125.7962836,ダンス微笑み,頬０涙０",
			"126.2933294,ダンス目つむり,頬０涙０",
			"126.8067026,ダンス目あけ,頬０涙０",
			"128.0990709,ダンスウインク,頬０涙０",
			"128.7451921,ダンスウインク,頬１涙０",
			"128.9438788,ダンス微笑み,頬１涙０",
			"129.9047352,ダンス目とじ,頬１涙０",
			"130.4025142,ダンス目あけ,頬１涙０",
			"131.3956234,ダンス目つむり,頬１涙０",
			"131.6109721,ダンス目つむり,頬０涙０",
			"131.892682,ダンス目あけ,頬０涙０",
			"132.5062371,ダンス微笑み,頬０涙０",
			"133.6488664,ダンス目つむり,頬０涙０",
			"134.4938513,ダンス微笑み,頬０涙０",
			"135.0073128,ダンス目つむり,頬０涙０",
			"135.5043595,ダンス目あけ,頬０涙０",
			"136.0012117,ダンスウインク,頬０涙０",
			"136.829573,ダンス微笑み,頬０涙０",
			"139.0993158,ダンス目つむり,頬０涙０",
			"139.5963628,ダンス目あけ,頬０涙０",
			"140.291998,ダンス微笑み,頬０涙０",
			"140.7891467,ダンス目つむり,頬０涙０",
			"141.2860072,ダンス目あけ,頬０涙０",
			"141.7831864,ダンス微笑み,頬０涙０",
			"142.2801858,ダンス目つむり,頬０涙０",
			"142.777229,ダンス目あけ,頬０涙０",
			"144.0858936,ダンス微笑み,頬０涙０",
			"144.5828687,ダンス目とじ,頬０涙０",
			"145.0799091,ダンス目あけ,頬０涙０",
			"146.3556123,ダンス微笑み,頬０涙０",
			"148.0797751,ダンス目つむり,頬０涙０",
			"148.57684,ダンス微笑み,頬０涙０",
			"150.2997576,ダンスウインク,頬０涙０",
			"151.1778861,ダンス目あけ,頬０涙０",
			"151.691237,ダンス微笑み,頬０涙０",
			"152.1891515,ダンス目あけ,頬０涙０",
			"153.049706,ダンス目とじ,頬０涙０",
			"153.5468319,ダンス目あけ,頬０涙０",
			"154.0437431,ダンス微笑み,頬０涙０",
			"155.9488902,ダンス目つむり,頬０涙０",
			"157.307433,ダンス目あけ,頬０涙０",
			"157.8044198,ダンス微笑み,頬０涙０",
			"158.7984421,ダンス目とじ,頬０涙０",
			"159.8090299,ダンス目あけ,頬０涙０",
			"160.9023677,ダンス微笑み,頬０涙０",
			"161.879848,ダンス目とじ,頬０涙０",
			"162.3769471,ダンス目あけ,頬０涙０",
			"163.3046151,ダンス微笑み,頬０涙０",
			"164.0831248,ダンス目とじ,頬０涙０",
			"164.8951165,ダンス目あけ,頬０涙０",
			"165.408482,ダンス微笑み,頬０涙０",
			"165.9054268,ダンス目あけ,頬０涙０",
			"166.800161,ダンス目つむり,頬０涙０",
			"167.8106278,ダンス目あけ,頬０涙０",
			"172.6482038,ダンス微笑み,頬０涙０",
			"173.3938632,ダンス目つむり,頬０涙０",
			"175.1332498,ダンス目あけ,頬０涙０",
			"175.9283748,ダンス微笑み,頬０涙０",
			"177.5189124,ダンスウインク,頬０涙０",
			"177.9991564,ダンス目あけ,頬０涙０",
			"178.5956529,ダンス目つむり,頬０涙０",
			"179.1090688,ダンス目あけ,頬０涙０",
			"180.1031493,ダンス目つむり,頬０涙０",
			"180.6002207,ダンス目あけ,頬０涙０",
			"181.097004,ダンス目つむり,頬０涙０",
			"181.5940329,ダンス目あけ,頬０涙０",
			"182.1078936,ダンス目つむり,頬０涙０",
			"182.9027717,ダンス目あけ,頬０涙０"
		};

		private string[] dance12Array = new string[]
		{
			"0,通常",
			"0.0170227,ダンス目つむり,頬０涙０",
			"9.0126669,ダンス目あけ,頬０涙０",
			"9.5924729,ダンス目とじ,頬０涙０",
			"10.0894947,ダンス目つむり,頬０涙０",
			"10.7024195,ダンス微笑み,頬０涙０",
			"11.2655739,ダンス目あけ,頬０涙０",
			"11.8952254,ダンスウインク,頬０涙０",
			"12.5081809,ダンス微笑み,頬０涙０",
			"13.1211461,ダンス目つむり,頬０涙０",
			"13.6843089,ダンス微笑み,頬０涙０",
			"14.5457372,ダンス目あけ,頬０涙０",
			"15.2249921,ダンスびっくり,頬０涙０",
			"15.8546584,ダンス目とじ,頬０涙０",
			"16.6664255,ダンス微笑み,頬０涙０",
			"17.2960042,ダンス目あけ,頬０涙０",
			"17.8095573,ダンス目つむり,頬０涙０",
			"18.2402298,ダンス目あけ,頬０涙０",
			"18.9195055,ダンスウインク,頬０涙０",
			"19.5490421,ダンス微笑み,頬０涙０",
			"20.0459072,ダンス目つむり,頬０涙０",
			"20.609355,ダンスウインク,頬０涙０",
			"21.1560094,ダンス目あけ,頬０涙０",
			"21.6198883,ダンス目とじ,頬０涙０",
			"22.1334464,ダンス目あけ,頬０涙０",
			"22.646953,ダンス微笑み,頬０涙０",
			"23.1274696,ダンス目つむり,頬０涙０",
			"23.6243358,ダンス目あけ,頬０涙０",
			"24.1214572,ダンス微笑み,頬０涙０",
			"24.651559,ダンスウインク,頬０涙０",
			"25.1485578,ダンス目あけ,頬０涙０",
			"25.6289972,ダンス微笑み,頬０涙０",
			"26.1093855,ダンス目あけ,頬０涙０",
			"27.0537434,ダンス目とじ,頬０涙０",
			"27.6335633,ダンス微笑み,頬０涙０",
			"29.439345,ダンス目とじ,頬０涙０",
			"30.0688077,ダンス微笑み,頬０涙０",
			"30.665146,ダンス目あけ,頬０涙０",
			"31.3776502,ダンス目とじ,頬０涙０",
			"32.2058791,ダンスウインク,頬０涙０",
			"32.620138,ダンス微笑み,頬０涙０",
			"33.1502555,ダンス目あけ,頬０涙０",
			"33.6472537,ダンス目つむり,頬０涙０",
			"34.1608159,ダンス目あけ,頬０涙０",
			"34.6578312,ダンス微笑み,頬０涙０",
			"35.1714047,ダンス目とじ,頬０涙０",
			"35.6352505,ダンス微笑み,頬０涙０",
			"36.430451,ダンスウインク,頬０涙０",
			"37.2753528,ダンス目あけ,頬０涙０",
			"38.0871304,ダンス微笑み,頬０涙０",
			"39.0977578,ダンスウインク,頬０涙０",
			"40.1413958,ダンス目あけ,頬０涙０",
			"40.8039414,ダンス微笑み,頬０涙０",
			"42.0630359,ダンス目とじ,頬０涙０",
			"42.9577427,ダンス微笑み,頬０涙０",
			"43.5707053,ダンスウインク,頬０涙０",
			"44.1505318,ダンス目あけ,頬０涙０",
			"44.7800624,ダンス目つむり,頬０涙０",
			"45.475851,ダンスびっくり,頬０涙０",
			"46.0225536,ダンス困り顔,頬０涙０",
			"46.602459,ダンスウインク,頬０涙０",
			"47.1159818,ダンス目あけ,頬０涙０",
			"47.9775596,ダンスウインク,頬０涙０",
			"48.6400741,ダンス目あけ,頬０涙０",
			"49.7169333,ダンス微笑み,頬０涙０",
			"50.8600217,ダンスウインク,頬０涙０",
			"51.7877583,ダンス目あけ,頬０涙０",
			"52.8644824,ダンス困り顔,頬０涙０",
			"53.3947336,ダンス目つむり,頬０涙０",
			"53.8917461,ダンス目あけ,頬０涙０",
			"54.5045915,ダンスウインク,頬０涙０",
			"55.001712,ダンス目あけ,頬０涙０",
			"56.1779278,ダンス微笑み,頬０涙０",
			"57.6358837,ダンス目あけ,頬０涙０",
			"58.4972534,ダンス目つむり,頬０涙０",
			"59.1432398,ダンス目あけ,頬０涙０",
			"59.690014,ダンス微笑み,頬０涙０",
			"60.6012459,ダンス目つむり,頬０涙０",
			"61.628364,ダンス目あけ,頬０涙０",
			"62.158518,ダンス目とじ,頬０涙０",
			"62.6555068,ダンス目あけ,頬０涙０",
			"63.8980065,ダンス微笑み,頬０涙０",
			"65.1570623,ダンス目つむり,頬０涙０",
			"65.339297,ダンス目つむり,頬１涙０",
			"65.6374806,ダンス微笑み,頬１涙０",
			"66.1676426,ダンス目つむり,頬１涙０",
			"66.6149355,ダンス微笑み,頬１涙０",
			"67.1947714,ダンス目つむり,頬１涙０",
			"67.6420675,ダンス目つむり,頬０涙０",
			"68.4207001,ダンス目あけ,頬０涙０",
			"68.9673847,ダンス目つむり,頬０涙０",
			"69.5141047,ダンス目あけ,頬０涙０",
			"70.1104637,ダンス目つむり,頬０涙０",
			"71.5352122,ダンス目あけ,頬０涙０",
			"72.1813433,ダンス微笑み,頬０涙０",
			"72.7114799,ダンス困り顔,頬０涙０",
			"73.2415983,ダンス目あけ,頬０涙０",
			"73.6557631,ダンス目あけ,頬１涙０",
			"73.9705673,ダンス目つむり,頬１涙０",
			"74.9148268,ダンス目あけ,頬１涙０",
			"75.4781033,ダンス目つむり,頬１涙０",
			"75.7928872,ダンス目つむり,頬０涙０",
			"76.0246793,ダンス微笑み,頬０涙０",
			"76.571584,ダンス目あけ,頬０涙０",
			"77.1843451,ダンス目つむり,頬０涙０",
			"77.9300034,ダンス目あけ,頬０涙０",
			"78.4932355,ダンス目つむり,頬０涙０",
			"79.1060809,ダンス目あけ,頬０涙０",
			"79.6198113,ダンス目つむり,頬０涙０",
			"80.2161558,ダンス目あけ,頬０涙０",
			"80.9617138,ダンス微笑み,頬０涙０",
			"81.6739871,ダンス目あけ,頬０涙０",
			"81.7402834,ダンス目あけ,頬１涙０",
			"82.3201566,ダンスウインク,頬１涙０",
			"82.8999771,ダンス目あけ,頬１涙０",
			"84.0099457,ダンス目あけ,頬０涙０",
			"84.987367,ダンス目とじ,頬０涙０",
			"86.0640802,ダンス目あけ,頬０涙０",
			"86.6937385,ダンスウインク,頬０涙０",
			"87.2073041,ダンス目あけ,頬０涙０",
			"87.91967,ダンス微笑み,頬０涙０",
			"88.6486185,ダンス目あけ,頬０涙０",
			"89.6094759,ダンス目つむり,頬０涙０",
			"90.6697512,ダンスびっくり,頬０涙０",
			"91.3158203,ダンス目つむり,頬０涙０",
			"91.8625414,ダンス微笑み,頬０涙０",
			"92.6244781,ダンス目あけ,頬０涙０",
			"93.9499267,ダンスウインク,頬０涙０",
			"94.7450329,ダンス微笑み,頬０涙０",
			"95.623036,ダンス目あけ,頬０涙０",
			"96.6006387,ダンス目とじ,頬０涙０",
			"97.6111859,ダンス目あけ,頬０涙０",
			"98.6714327,ダンス目つむり,頬０涙０",
			"99.3836752,ダンスウインク,頬０涙０",
			"99.8476838,ダンス目あけ,頬０涙０",
			"100.4109221,ダンス微笑み,頬０涙０",
			"100.9079491,ダンスびっくり,頬０涙０",
			"101.4048133,ダンスウインク,頬０涙０",
			"101.951639,ダンス目あけ,頬０涙０",
			"102.4155421,ダンス目つむり,頬０涙０",
			"102.9291193,ダンス目あけ,頬０涙０",
			"103.9065076,ダンス微笑み,頬０涙０",
			"104.6023282,ダンス微笑み,頬１涙０",
			"104.7514091,ダンス目とじ,頬１涙０",
			"105.4472619,ダンス目あけ,頬１涙０",
			"105.9774079,ダンスウインク,頬１涙０",
			"106.4411927,ダンス微笑み,頬１涙０",
			"106.8055442,ダンス微笑み,頬０涙０",
			"106.9381852,ダンス目あけ,頬０涙０",
			"107.9652143,ダンスウインク,頬０涙０",
			"108.5782926,ダンス目あけ,頬０涙０",
			"109.4563383,ダンス微笑み,頬０涙０",
			"110.0361653,ダンス目あけ,頬０涙０",
			"110.5827467,ダンスウインク,頬０涙０",
			"111.1129957,ダンス目あけ,頬０涙０",
			"111.9910334,ダンスウインク,頬０涙０",
			"112.5213793,ダンス目あけ,頬０涙０",
			"113.1175738,ダンス目とじ,頬０涙０",
			"113.6808566,ダンス微笑み,頬０涙０",
			"114.57542,ダンス目とじ,頬０涙０",
			"115.2381306,ダンス微笑み,頬０涙０",
			"116.1326915,ダンス目あけ,頬０涙０",
			"116.4308947,ダンス目あけ,頬１涙０",
			"116.8616482,ダンス目とじ,頬１涙０",
			"117.1267072,ダンス目とじ,頬０涙０",
			"117.4084035,ダンス目あけ,頬０涙０",
			"117.8887864,ダンス目とじ,頬０涙０",
			"118.3856372,ダンス目あけ,頬０涙０",
			"119.0152998,ダンス微笑み,頬０涙０",
			"119.6447087,ダンス目つむり,頬０涙０",
			"120.2245023,ダンス目あけ,頬０涙０",
			"120.6055678,ダンス目あけ,頬１涙０",
			"121.1689765,ダンス微笑み,頬１涙０",
			"123.0243041,ダンス微笑み,頬０涙０",
			"123.6539943,ダンス目つむり,頬０涙０",
			"124.1509512,ダンス目あけ,頬０涙０",
			"124.9793095,ダンス目とじ,頬０涙０",
			"126.2051054,ダンス目あけ,頬０涙０",
			"126.7194815,ダンス微笑み,頬０涙０",
			"127.3814862,ダンス目つむり,頬０涙０",
			"128.0439904,ダンス目あけ,頬０涙０",
			"128.9552977,ダンス目とじ,頬０涙０",
			"129.5185556,ダンス目あけ,頬０涙０",
			"130.032138,ダンス目つむり,頬０涙０",
			"130.4794236,ダンス目あけ,頬０涙０",
			"130.959867,ダンス目つむり,頬０涙０",
			"131.3740996,ダンス目つむり,頬１涙０",
			"131.456864,ダンス目あけ,頬１涙０",
			"132.1692266,ダンス目あけ,頬０涙０",
			"132.3018031,ダンスウインク,頬０涙０"
		};

		private string[] dance13Array = new string[]
		{
			"0,通常,",
			"0.0207593,ダンス目つむり,頬０涙０",
			"6.0555881,ダンス目あけ,頬０涙０",
			"6.8341436,ダンス目とじ,頬０涙０",
			"8.3605946,ダンス微笑み,頬０涙０",
			"9.222068,ダンス目あけ,頬０涙０",
			"9.9178364,ダンス目つむり,頬０涙０",
			"10.414841,ダンス目あけ,頬０涙０",
			"11.3923266,ダンスウインク,頬０涙０",
			"11.8727585,ダンス目あけ,頬０涙０",
			"12.4194494,ダンス目つむり,頬０涙０",
			"12.9330035,ダンス目あけ,頬０涙０",
			"13.5128462,ダンスウインク,頬０涙０",
			"14.2468952,ダンス微笑み,頬０涙０",
			"14.8930093,ダンス目あけ,頬０涙０",
			"15.936703,ダンス目とじ,頬０涙０",
			"16.8644652,ダンス微笑み,頬０涙０",
			"17.4276409,ダンス目あけ,頬０涙０",
			"18.4217516,ダンス目つむり,頬０涙０",
			"18.9187499,ダンス目あけ,頬０涙０",
			"19.4157516,ダンス目とじ,頬０涙０",
			"19.8961696,ダンス微笑み,頬０涙０",
			"20.5295394,ダンス目とじ,頬０涙０",
			"21.1590866,ダンス目あけ,頬０涙０",
			"21.6726473,ダンス目とじ,頬０涙０",
			"22.268903,ダンス目あけ,頬０涙０",
			"23.0310885,ダンス微笑み,頬０涙０",
			"23.4618183,ダンス目つむり,頬０涙０",
			"23.9256662,ダンス目あけ,頬０涙０",
			"24.4227072,ダンス目とじ,頬０涙０",
			"25.0024997,ダンス微笑み,頬０涙０",
			"26.397725,ダンス目とじ,頬０涙０",
			"27.3917096,ダンス目あけ,頬０涙０",
			"27.9715626,ダンスウインク,頬０涙０",
			"28.4354173,ダンス目あけ,頬０涙０",
			"28.9324194,ダンス目つむり,頬０涙０",
			"29.4294644,,頬０涙０",
			"30.8707512,ダンス目とじ,頬０涙０",
			"32.5147755,ダンス微笑み,頬０涙０",
			"33.3762734,ダンス目つむり,頬０涙０",
			"35.3476333,ダンス目あけ,頬０涙０",
			"35.894398,ダンスウインク,頬０涙０",
			"36.4245547,ダンス微笑み,頬０涙０",
			"36.9712384,ダンス目あけ,頬０涙０",
			"38.2467882,ダンスウインク,頬０涙０",
			"38.949335,ダンス微笑み,頬０涙０",
			"39.6782994,ダンス目あけ,頬０涙０",
			"40.2250119,ダンスウインク,頬０涙０",
			"40.986962,ダンス微笑み,頬０涙０",
			"41.5337903,ダンス目あけ,頬０涙０",
			"42.4614901,ダンス微笑み,頬０涙０",
			"43.3892732,ダンス目つむり,頬０涙０",
			"43.9524876,ダンス目あけ,頬０涙０",
			"44.4163426,ダンス目つむり,頬０涙０",
			"44.9042919,ダンス微笑み,頬０涙０",
			"45.4344315,ダンスウインク,頬０涙０",
			"46.1137065,ダンス微笑み,頬０涙０",
			"46.9586104,ダンス目つむり,頬０涙０",
			"47.4555734,ダンス目あけ,頬０涙０",
			"47.9690722,ダンスウインク,頬０涙０",
			"48.8305744,ダンス微笑み,頬０涙０",
			"49.4104356,ダンスウインク,頬０涙０",
			"50.4209926,ダンス目あけ,頬０涙０",
			"51.4370567,ダンス目とじ,頬０涙０",
			"52.000339,ダンス目あけ,頬０涙０",
			"52.6133078,ダンスウインク,頬０涙０",
			"53.4913422,ダンス微笑み,頬０涙０",
			"54.2699712,ダンス目つむり,頬０涙０",
			"54.8662756,ダンス目あけ,頬０涙０",
			"55.4130586,ダンス微笑み,頬０涙０",
			"56.1751315,ダンスウインク,頬０涙０",
			"56.804699,ダンス目あけ,頬０涙０",
			"57.3561872,ダンス微笑み,頬０涙０",
			"57.9360547,ダンス目あけ,頬０涙０",
			"58.4992708,ダンス目つむり,頬０涙０",
			"58.9632259,ダンス目あけ,頬０涙０",
			"60.2720456,ダンス微笑み,頬０涙０",
			"60.8682311,ダンス目つむり,頬０涙０",
			"61.497932,ダンス目あけ,頬０涙０",
			"62.3530542,ダンス目つむり,頬０涙０",
			"62.8831989,ダンス目あけ,頬０涙０",
			"63.4512096,ダンス目つむり,頬０涙０",
			"63.9646956,ダンス目あけ,頬０涙０",
			"64.4618305,ダンス目つむり,頬０涙０",
			"64.9587561,ダンス目あけ,頬０涙０",
			"65.3397617,ダンス目あけ,頬１涙０",
			"65.4557559,ダンス目とじ,頬１涙０",
			"65.985788,ダンス微笑み,頬１涙０",
			"66.5159931,ダンス目あけ,頬１涙０",
			"67.5100314,ダンスウインク,頬１涙０",
			"67.642548,ダンスウインク,頬０涙０",
			"68.0069011,ダンス微笑み,頬０涙０",
			"69.9826818,ダンスウインク,頬０涙０",
			"70.5293826,ダンス目あけ,頬０涙０",
			"71.0262581,ダンス微笑み,頬０涙０",
			"71.9043745,ダンスウインク,頬０涙０",
			"73.478264,ダンス目あけ,頬０涙０",
			"73.6604584,ダンス目あけ,頬１涙０",
			"74.356226,ダンス目つむり,頬１涙０",
			"75.2177333,ダンスウインク,頬１涙０",
			"75.8017196,ダンスウインク,頬０涙０",
			"76.4644335,ダンス微笑み,頬０涙０",
			"77.3921912,ダンス目つむり,頬０涙０",
			"77.9553879,ダンス目あけ,頬０涙０",
			"78.750625,ダンス微笑み,頬０涙０",
			"79.9434171,ダンス目とじ,頬０涙０",
			"80.6557864,ダンス目あけ,頬０涙０",
			"81.1693326,ダンスウインク,頬０涙０",
			"81.753066,ダンスウインク,頬１涙０",
			"82.2169462,ダンス目あけ,頬１涙０",
			"82.7139137,ダンス目とじ,頬１涙０",
			"83.6582072,ダンス目あけ,頬１涙０",
			"84.0060373,ダンス目あけ,頬０涙０",
			"84.652209,ダンス微笑み,頬０涙０",
			"85.4970872,ダンス目あけ,頬０涙０",
			"86.1432755,ダンス目とじ,頬０涙０",
			"86.9383948,ダンス微笑み,頬０涙０",
			"87.5557017,ダンス目つむり,頬０涙０",
			"88.1520748,ダンス微笑み,頬０涙０",
			"88.5993498,ダンス目あけ,頬０涙０",
			"88.9471907,ダンスウインク,頬０涙０",
			"89.5602613,ダンス目あけ,頬０涙０",
			"90.0241095,ダンス微笑み,頬０涙０",
			"90.9021485,ダンス目つむり,頬０涙０",
			"91.4985529,ダンス微笑み,頬０涙０",
			"92.1115295,ダンス目あけ,頬０涙０",
			"93.3540663,ダンス微笑み,頬０涙０",
			"94.0720846,ダンスウインク,頬０涙０",
			"94.6022,ダンス目あけ,頬０涙０",
			"95.0163517,ダンス目とじ,頬０涙０",
			"95.4469862,ダンス微笑み,頬０涙０",
			"95.8778661,ダンス目あけ,頬０涙０",
			"96.9214846,ダンス目とじ,頬０涙０",
			"98.2302903,ダンス目あけ,頬０涙０",
			"98.793527,ダンス微笑み,頬０涙０",
			"99.3900367,ダンス目つむり,頬０涙０",
			"99.9257374,ダンス目あけ,頬０涙０",
			"100.5056032,ダンスウインク,頬０涙０",
			"101.0191223,ダンス微笑み,頬０涙０",
			"102.0628984,ダンス目あけ,頬０涙０",
			"102.6757854,ダンス目つむり,頬０涙０",
			"103.1894142,ダンス目あけ,頬０涙０",
			"103.7029453,ダンスウインク,頬０涙０",
			"104.2165091,ダンス微笑み,頬０涙０",
			"104.6141198,ダンス微笑み,頬１涙０",
			"104.8958266,ダンス目とじ,頬１涙０",
			"105.4093899,ダンス目あけ,頬１涙０",
			"106.02947,ダンス微笑み,頬１涙０",
			"106.8080361,ダンス微笑み,頬０涙０",
			"106.9241454,ダンス目あけ,頬０涙０",
			"107.9180548,ダンス微笑み,頬０涙０",
			"109.160574,ダンス目あけ,頬０涙０",
			"109.6576071,ダンス目つむり,頬０涙０",
			"110.1876306,ダンス目あけ,頬０涙０",
			"111.6124197,ダンスウインク,頬０涙０",
			"113.6876744,ダンス目あけ,頬０涙０",
			"114.2344821,ダンス目とじ,頬０涙０",
			"114.7479502,ダンス微笑み,頬０涙０",
			"115.2450443,ダンス目あけ,頬０涙０",
			"116.4212524,ダンス目あけ,頬１涙０",
			"116.6862788,ダンス微笑み,頬１涙０",
			"117.1336639,ダンス微笑み,頬０涙０",
			"117.1999228,ダンス目つむり,頬０涙０",
			"117.8625551,ダンス目あけ,頬０涙０",
			"118.4153234,ダンス目とじ,頬０涙０",
			"119.1773191,ダンス目あけ,頬０涙０",
			"120.5192798,ダンス微笑み,頬０涙０",
			"120.6186574,ダンス微笑み,頬１涙０",
			"121.4304577,ダンス目とじ,頬１涙０",
			"122.3912692,ダンス微笑み,頬１涙０",
			"123.020828,ダンス微笑み,頬０涙０",
			"124.0313899,ダンス目とじ,頬０涙０",
			"125.1965902,ダンス目あけ,頬０涙０",
			"126.1905293,ダンス目つむり,頬０涙０",
			"126.7207119,ダンス目あけ,頬０涙０",
			"128.178587,ダンス微笑み,頬０涙０",
			"129.1725873,ダンス目あけ,頬０涙０",
			"130.2874759,ダンス目つむり,頬０涙０",
			"130.8011378,ダンス目あけ,頬０涙０",
			"131.2650102,ダンス目つむり,頬０涙０",
			"131.3809417,ダンス目つむり,頬１涙０",
			"132.1761747,ダンス目つむり,頬０涙０",
			"133.1866279,ダンス目あけ,頬０涙０",
			"133.9487083,ダンスウインク,頬０涙０"
		};

		private string[] dance13BArray = new string[]
		{
			"0,通常,",
			"0.0208488,ダンス目つむり,頬０涙０",
			"6.0555955,ダンス目あけ,頬０涙０",
			"6.8341502,ダンス目とじ,頬０涙０",
			"8.360602,ダンス微笑み,頬０涙０",
			"9.2220751,ダンス目あけ,頬０涙０",
			"9.9178438,ダンス目つむり,頬０涙０",
			"10.4148467,ダンス目あけ,頬０涙０",
			"11.3923377,ダンスウインク,頬０涙０",
			"11.8727651,ダンス目あけ,頬０涙０",
			"12.4194562,ダンス目つむり,頬０涙０",
			"12.9330107,ダンス目あけ,頬０涙０",
			"13.5128533,ダンスウインク,頬０涙０",
			"14.2469029,ダンス微笑み,頬０涙０",
			"14.8930187,ダンス目あけ,頬０涙０",
			"15.9367104,ダンス目とじ,頬０涙０",
			"16.8644712,ダンス微笑み,頬０涙０",
			"17.4276489,ダンス目あけ,頬０涙０",
			"18.4217604,ダンス目つむり,頬０涙０",
			"18.9187562,ダンス目あけ,頬０涙０",
			"19.4157588,ダンス目とじ,頬０涙０",
			"19.8961759,ダンス微笑み,頬０涙０",
			"20.5295896,ダンス目とじ,頬０涙０",
			"21.1590929,ダンス目あけ,頬０涙０",
			"21.6726536,ダンス目とじ,頬０涙０",
			"22.2689095,ダンス目あけ,頬０涙０",
			"23.0310954,ダンス微笑み,頬０涙０",
			"23.4618277,ダンス目つむり,頬０涙０",
			"23.9256724,ダンス目あけ,頬０涙０",
			"24.4227366,ダンス目とじ,頬０涙０",
			"25.0025083,ダンス微笑み,頬０涙０",
			"26.3977544,ダンス目とじ,頬０涙０",
			"27.3917159,ダンス目あけ,頬０涙０",
			"27.9715714,ダンスウインク,頬０涙０",
			"28.4354227,ダンス目あけ,頬０涙０",
			"28.9324273,ダンス目つむり,頬０涙０",
			"29.4294704,ダンス目あけ,頬０涙０",
			"30.8707591,ダンス目とじ,頬０涙０",
			"32.5147872,ダンス微笑み,頬０涙０",
			"33.3762834,ダンス目つむり,頬０涙０",
			"34.3537236,ダンス目あけ,頬０涙０",
			"35.8944071,ダンス目とじ,頬０涙０",
			"36.4245712,ダンス微笑み,頬０涙０",
			"36.9712467,ダンス目あけ,頬０涙０",
			"38.2467948,ダンスウインク,頬０涙０",
			"38.9493429,ダンス微笑み,頬０涙０",
			"39.6783074,ダンス目あけ,頬０涙０",
			"40.2250199,ダンスウインク,頬０涙０",
			"40.9869692,ダンス微笑み,頬０涙０",
			"41.5338003,ダンス目あけ,頬０涙０",
			"42.4614984,ダンス微笑み,頬０涙０",
			"43.3892826,ダンス目つむり,頬０涙０",
			"43.9524947,ダンス目あけ,頬０涙０",
			"44.4163501,ダンス目つむり,頬０涙０",
			"44.9042993,ダンス微笑み,頬０涙０",
			"45.4344386,ダンスウインク,頬０涙０",
			"46.1137151,ダンス微笑み,頬０涙０",
			"46.9586181,ダンス目つむり,頬０涙０",
			"47.4555808,ダンス目あけ,頬０涙０",
			"47.9690802,ダンス目とじ,頬０涙０",
			"48.5655408,ダンス微笑み,頬０涙０",
			"48.9963259,ダンス目つむり,頬０涙０",
			"49.4104439,ダンス目あけ,頬０涙０",
			"51.4370649,ダンス目とじ,頬０涙０",
			"52.0003475,ダンス目あけ,頬０涙０",
			"53.4913499,ダンス微笑み,頬０涙０",
			"54.2699784,ダンス目つむり,頬０涙０",
			"54.8662833,ダンス目あけ,頬０涙０",
			"55.4130671,ダンス微笑み,頬０涙０",
			"56.1751398,ダンスウインク,頬０涙０",
			"56.8047069,ダンス目あけ,頬０涙０",
			"57.3561963,ダンス微笑み,頬０涙０",
			"57.9360635,ダンス目あけ,頬０涙０",
			"58.6815194,ダンス目つむり,頬０涙０",
			"59.8577642,ダンス目あけ,頬０涙０",
			"60.8682403,ダンス目つむり,頬０涙０",
			"61.4979397,ダンス目あけ,頬０涙０",
			"62.3530622,ダンス目つむり,頬０涙０",
			"62.8832078,ダンス目あけ,頬０涙０",
			"63.4512179,ダンス目つむり,頬０涙０",
			"63.9647036,ダンス目あけ,頬０涙０",
			"64.4618476,ダンス目つむり,頬０涙０",
			"64.9587744,ダンス目あけ,頬０涙０",
			"65.3397699,ダンス目あけ,頬１涙０",
			"65.4557647,ダンス目とじ,頬１涙０",
			"65.9857965,ダンス微笑み,頬１涙０",
			"66.5160011,ダンス目つむり,頬１涙０",
			"66.97993,ダンス目あけ,頬１涙０",
			"67.5100391,ダンス目とじ,頬１涙０",
			"67.6425569,ダンス目とじ,頬０涙０",
			"68.00691,ダンス目あけ,頬０涙０",
			"68.4542899,ダンス目つむり,頬０涙０",
			"68.9512751,ダンス目あけ,頬０涙０",
			"69.4524983,ダンス目つむり,頬０涙０",
			"69.9826906,ダンスウインク,頬０涙０",
			"70.5293909,ダンス目あけ,頬０涙０",
			"71.0262669,ダンス微笑み,頬０涙０",
			"72.2854075,ダンス目あけ,頬０涙０",
			"72.8652849,ダンス目とじ,頬０涙０",
			"73.4782817,ダンス目あけ,頬０涙０",
			"73.6604672,ダンス目あけ,頬１涙０",
			"74.3562474,ダンス目つむり,頬１涙０",
			"75.7851798,ダンスウインク,頬１涙０",
			"75.8017276,ダンスウインク,頬０涙０",
			"76.4644412,ダンス微笑み,頬０涙０",
			"77.3921992,ダンス目つむり,頬０涙０",
			"77.9553974,ダンス目あけ,頬０涙０",
			"78.7506341,ダンス微笑み,頬０涙０",
			"79.9434262,ダンス目とじ,頬０涙０",
			"80.6557941,ダンス目あけ,頬０涙０",
			"81.753074,ダンス目あけ,頬１涙０",
			"82.2169558,ダンス微笑み,頬１涙０",
			"82.7139222,ダンス目とじ,頬１涙０",
			"83.6582166,ダンス目あけ,頬１涙０",
			"84.0060458,ダンス目あけ,頬０涙０",
			"84.6522184,ダンス微笑み,頬０涙０",
			"85.4970958,ダンス目あけ,頬０涙０",
			"86.143292,ダンス目とじ,頬０涙０",
			"86.9384028,ダンス微笑み,頬０涙０",
			"87.5557103,ダンス目つむり,頬０涙０",
			"88.1520845,ダンス微笑み,頬０涙０",
			"88.5993584,ダンス目あけ,頬０涙０",
			"88.947199,ダンスウインク,頬０涙０",
			"89.5602705,ダンス目あけ,頬０涙０",
			"90.0241192,ダンス微笑み,頬０涙０",
			"90.902157,ダンス目つむり,頬０涙０",
			"91.4985641,ダンス微笑み,頬０涙０",
			"92.1115378,ダンス目あけ,頬０涙０",
			"93.3540746,ダンス微笑み,頬０涙０",
			"94.0720937,ダンスウインク,頬０涙０",
			"94.6022105,ダンス目あけ,頬０涙０",
			"95.0163608,ダンス目とじ,頬０涙０",
			"95.4469953,ダンス微笑み,頬０涙０",
			"95.8778767,ダンス目あけ,頬０涙０",
			"96.9214931,ダンス目とじ,頬０涙０",
			"98.2302991,ダンス目あけ,頬０涙０",
			"98.7935358,ダンス微笑み,頬０涙０",
			"99.3900449,ダンス目つむり,頬０涙０",
			"99.9257454,ダンス目あけ,頬０涙０",
			"100.5056129,ダンスウインク,頬０涙０",
			"101.0191305,ダンス微笑み,頬０涙０",
			"102.0629075,ダンス目あけ,頬０涙０",
			"102.6757948,ダンス目つむり,頬０涙０",
			"103.1894225,ダンス目あけ,頬０涙０",
			"103.702953,ダンスウインク,頬０涙０",
			"104.2165205,ダンス微笑み,頬０涙０",
			"104.6141306,ダンス微笑み,頬１涙０",
			"104.8958372,ダンス目とじ,頬１涙０",
			"105.4094002,ダンス目あけ,頬１涙０",
			"106.0294794,ダンス目つむり,頬１涙０",
			"106.8080455,ダンス目つむり,頬０涙０",
			"106.9241557,ダンス目あけ,頬０涙０",
			"108.4151199,ダンス微笑み,頬０涙０",
			"109.160584,ダンス目あけ,頬０涙０",
			"109.6576159,ダンス目つむり,頬０涙０",
			"110.1876423,ダンス目あけ,頬０涙０",
			"111.6124277,ダンス微笑み,頬０涙０",
			"112.2463922,ダンスウインク,頬０涙０",
			"113.6876844,ダンス目あけ,頬０涙０",
			"114.2344901,ダンス目とじ,頬０涙０",
			"114.747959,ダンス微笑み,頬０涙０",
			"115.2450548,ダンス目あけ,頬０涙０",
			"116.4212609,ダンス目あけ,頬１涙０",
			"116.6862894,ダンス微笑み,頬１涙０",
			"117.1336739,ダンス微笑み,頬０涙０",
			"117.1999327,ダンス目つむり,頬０涙０",
			"117.8625645,ダンス目あけ,頬０涙０",
			"118.4153388,ダンス目とじ,頬０涙０",
			"119.1773285,ダンス目あけ,頬０涙０",
			"120.5192883,ダンス微笑み,頬０涙０",
			"120.6186657,ダンス微笑み,頬１涙０",
			"121.4304688,ダンス目とじ,頬１涙０",
			"122.3912769,ダンス微笑み,頬１涙０",
			"123.0208369,ダンス微笑み,頬０涙０",
			"124.0313999,ダンス目とじ,頬０涙０",
			"125.1965996,ダンス目あけ,頬０涙０",
			"126.190539,ダンス目つむり,頬０涙０",
			"126.7207188,ダンス目あけ,頬０涙０",
			"128.1785961,ダンス微笑み,頬０涙０",
			"129.1725973,ダンス目あけ,頬０涙０",
			"130.2874847,ダンス目つむり,頬０涙０",
			"130.8011466,ダンス目あけ,頬０涙０",
			"131.2650202,ダンス目つむり,頬０涙０",
			"131.3809522,ダンス目つむり,頬１涙０",
			"132.1761847,ダンス目つむり,頬０涙０",
			"132.6897297,ダンス目あけ,頬０涙０",
			"133.9487177,ダンスウインク,頬０涙０"
		};

		private string[] dance13CArray = new string[]
		{
			"0,通常,",
			"0.0211251,ダンス目つむり,頬０涙０",
			"6.0559419,ダンス目あけ,頬０涙０",
			"6.8343198,ダンス目とじ,頬０涙０",
			"8.3609184,ダンス微笑み,頬０涙０",
			"9.2222992,ダンス目あけ,頬０涙０",
			"9.9180637,ダンス目つむり,頬０涙０",
			"10.4150452,ダンス目あけ,頬０涙０",
			"12.4196079,ダンス目つむり,頬０涙０",
			"12.9332724,ダンス目あけ,頬０涙０",
			"13.5131781,ダンスウインク,頬０涙０",
			"14.2471141,ダンス微笑み,頬０涙０",
			"14.8934053,ダンス目あけ,頬０涙０",
			"15.9370465,ダンス目とじ,頬０涙０",
			"16.8648279,ダンス微笑み,頬０涙０",
			"17.4279993,ダンス目あけ,頬０涙０",
			"18.4220843,ダンス目つむり,頬０涙０",
			"18.9190866,ダンス目あけ,頬０涙０",
			"19.4160895,ダンス目とじ,頬０涙０",
			"19.8963549,ダンス微笑み,頬０涙０",
			"20.5297926,ダンス目とじ,頬０涙０",
			"21.1594236,ダンス目あけ,頬０涙０",
			"21.6728708,ダンス目とじ,頬０涙０",
			"22.2690934,ダンス目あけ,頬０涙０",
			"23.0312861,ダンス微笑み,頬０涙０",
			"23.4621558,ダンス目つむり,頬０涙０",
			"23.925874,ダンス目あけ,頬０涙０",
			"24.4229667,ダンス目とじ,頬０涙０",
			"25.0026985,ダンス微笑み,頬０涙０",
			"26.3979824,ダンス目とじ,頬０涙０",
			"27.3920498,ダンス目あけ,頬０涙０",
			"27.9717932,ダンスウインク,頬０涙０",
			"28.4356215,ダンス目あけ,頬０涙０",
			"28.9327327,ダンス目つむり,頬０涙０",
			"29.4296828,ダンス目あけ,頬０涙０",
			"30.8710833,ダンス目とじ,頬０涙０",
			"32.5151116,ダンス微笑み,頬０涙０",
			"33.3764607,ダンス目つむり,頬０涙０",
			"34.9338808,ダンス目あけ,頬０涙０",
			"35.894652,ダンス目とじ,頬０涙０",
			"36.4248284,ダンス微笑み,頬０涙０",
			"36.971611,ダンス目あけ,頬０涙０",
			"38.2469733,ダンスウインク,頬０涙０",
			"38.949577,ダンス微笑み,頬０涙０",
			"39.6786552,ダンス目あけ,頬０涙０",
			"40.2252335,ダンスウインク,頬０涙０",
			"40.9873002,ダンス微笑み,頬０涙０",
			"41.5341669,ダンス目あけ,頬０涙０",
			"42.4618861,ダンス微笑み,頬０涙０",
			"43.3897595,ダンス目つむり,頬０涙０",
			"43.9528195,ダンス目あけ,頬０涙０",
			"44.4166021,ダンス目つむり,頬０涙０",
			"44.904612,ダンス微笑み,頬０涙０",
			"45.4347699,ダンスウインク,頬０涙０",
			"46.1139685,ダンス微笑み,頬０涙０",
			"46.9588442,ダンス目つむり,頬０涙０",
			"47.4559386,ダンス目あけ,頬０涙０",
			"47.9694212,ダンス目とじ,頬０涙０",
			"48.8970606,ダンス微笑み,頬０涙０",
			"49.3777041,ダンス目とじ,頬０涙０",
			"49.8745612,ダンス目あけ,頬０涙０",
			"50.4045953,ダンス目つむり,頬０涙０",
			"50.8189971,ダンス目あけ,頬０涙０",
			"51.4374022,ダンス目とじ,頬０涙０",
			"52.0005331,ダンス目あけ,頬０涙０",
			"53.4915403,ダンス微笑み,頬０涙０",
			"54.2703276,ダンス目つむり,頬０涙０",
			"54.8666123,ダンス目あけ,頬０涙０",
			"55.4133152,ダンス微笑み,頬０涙０",
			"56.1754588,ダンスウインク,頬０涙０",
			"56.8048789,ダンス目あけ,頬０涙０",
			"57.3564495,ダンス微笑み,頬０涙０",
			"57.9364076,ダンス目あけ,頬０涙０",
			"58.6816705,ダンス目つむり,頬０涙０",
			"59.8580522,ダンス目あけ,頬０涙０",
			"60.8685969,ダンス目つむり,頬０涙０",
			"61.4980885,ダンス目あけ,頬０涙０",
			"62.3533085,ダンス目つむり,頬０涙０",
			"62.8833503,ダンス目あけ,頬０涙０",
			"63.4514828,ダンス目つむり,頬０涙０",
			"63.9648713,ダンス目あけ,頬０涙０",
			"64.4620506,ダンス目つむり,頬０涙０",
			"64.9589446,ダンス目あけ,頬０涙０",
			"65.3400841,ダンス目あけ,頬１涙０",
			"65.4559808,ダンス目とじ,頬１涙０",
			"65.9860106,ダンス微笑み,頬１涙０",
			"66.5161557,ダンス目つむり,頬１涙０",
			"66.9800957,ダンス目あけ,頬１涙０",
			"67.5102575,ダンス目とじ,頬１涙０",
			"67.6427587,ダンス目とじ,頬０涙０",
			"68.0071107,ダンス目あけ,頬０涙０",
			"68.4546015,ダンス目つむり,頬０涙０",
			"68.9514923,ダンス目あけ,頬０涙０",
			"69.452835,ダンス目つむり,頬０涙０",
			"69.9830253,ダンスウインク,頬０涙０",
			"70.5296133,ダンス目あけ,頬０涙０",
			"71.0265338,ダンス微笑み,頬０涙０",
			"72.2857424,ダンス目あけ,頬０涙０",
			"72.8654583,ダンス目とじ,頬０涙０",
			"73.4784682,ダンス目あけ,頬０涙０",
			"73.6606303,ダンス目あけ,頬１涙０",
			"74.3565547,ダンス目つむり,頬１涙０",
			"74.9196699,ダンスウインク,頬１涙０",
			"75.8019095,ダンスウインク,頬０涙０",
			"76.4647673,ダンス微笑み,頬０涙０",
			"77.3924523,ダンス目つむり,頬０涙０",
			"77.9556602,ダンス目あけ,頬０涙０",
			"78.750964,ダンス微笑み,頬０涙０",
			"79.9437701,ダンス目とじ,頬０涙０",
			"80.6560638,ダンス目あけ,頬０涙０",
			"81.7532682,ダンス目あけ,頬１涙０",
			"82.2173017,ダンス微笑み,頬１涙０",
			"82.7141127,ダンス目とじ,頬１涙０",
			"83.6585952,ダンス目あけ,頬１涙０",
			"84.0063717,ダンス目あけ,頬０涙０",
			"84.6524023,ダンス微笑み,頬０涙０",
			"85.4972783,ダンス目あけ,頬０涙０",
			"86.1435056,ダンス目とじ,頬０涙０",
			"86.9387383,ダンス微笑み,頬０涙０",
			"87.5559885,ダンス目つむり,頬０涙０",
			"88.152273,ダンス微笑み,頬０涙０",
			"88.5995351,ダンス目あけ,頬０涙０",
			"88.947414,ダンスウインク,頬０涙０",
			"89.5604695,ダンス目あけ,頬０涙０",
			"90.0243051,ダンス微笑み,頬０涙０",
			"90.9023686,ダンス目つむり,頬０涙０",
			"91.4988916,ダンス微笑み,頬０涙０",
			"92.1117003,ダンス目あけ,頬０涙０",
			"93.3544347,ダンス微笑み,頬０涙０",
			"94.0724681,ダンスウインク,頬０涙０",
			"94.6025669,ダンス目あけ,頬０涙０",
			"95.0165943,ダンス目とじ,頬０涙０",
			"95.4472094,ダンス微笑み,頬０涙０",
			"95.8782148,ダンス目あけ,頬０涙０",
			"96.9216534,ダンス目とじ,頬０涙０",
			"98.2306749,ダンス目あけ,頬０涙０",
			"98.7937057,ダンス微笑み,頬０涙０",
			"99.3902337,ダンス目つむり,頬０涙０",
			"99.9259444,ダンス目あけ,頬０涙０",
			"100.5058019,ダンスウインク,頬０涙０",
			"101.0194644,ダンス微笑み,頬０涙０",
			"102.0632434,ダンス目あけ,頬０涙０",
			"102.6761244,ダンス目つむり,頬０涙０",
			"103.1896529,ダンス目あけ,頬０涙０",
			"103.7031066,ダンスウインク,頬０涙０",
			"104.2166898,ダンス微笑み,頬０涙０",
			"104.6143758,ダンス微笑み,頬１涙０",
			"104.8962321,ダンス目とじ,頬１涙０",
			"105.4097386,ダンス目あけ,頬１涙０",
			"106.0297212,ダンス目つむり,頬１涙０",
			"106.8083793,ダンス目つむり,頬０涙０",
			"106.9245001,ダンス目あけ,頬０涙０",
			"108.6639303,ダンス微笑み,頬０涙０",
			"109.1607784,ダンス目あけ,頬０涙０",
			"109.6578488,ダンス目つむり,頬０涙０",
			"110.1879987,ダンス目あけ,頬０涙０",
			"111.6126689,ダンス目つむり,頬０涙０",
			"112.2465872,ダンス目あけ,頬０涙０",
			"112.7438216,ダンスウインク,頬０涙０",
			"113.688016,ダンス目あけ,頬０涙０",
			"114.2347319,ダンス目とじ,頬０涙０",
			"114.7482892,ダンス微笑み,頬０涙０",
			"115.2453964,ダンス目あけ,頬０涙０",
			"116.4215326,ダンス目あけ,頬１涙０",
			"116.6865913,ダンス微笑み,頬１涙０",
			"117.1338538,ダンス微笑み,頬０涙０",
			"117.200103,ダンス目つむり,頬０涙０",
			"117.8628929,ダンス目あけ,頬０涙０",
			"118.4156675,ダンス目とじ,頬０涙０",
			"119.1776196,ダンス目あけ,頬０涙０",
			"120.5195563,ダンス微笑み,頬０涙０",
			"120.6189112,ダンス微笑み,頬１涙０",
			"121.4308594,ダンス目とじ,頬１涙０",
			"122.3915069,ダンス微笑み,頬１涙０",
			"123.0211927,ダンス微笑み,頬０涙０",
			"124.0317414,ダンス目とじ,頬０涙０",
			"125.1969606,ダンス目あけ,頬０涙０",
			"126.1906826,ダンス目つむり,頬０涙０",
			"126.7209317,ダンス目あけ,頬０涙０",
			"128.1789724,ダンス微笑み,頬０涙０",
			"129.1727746,ダンス目あけ,頬０涙０",
			"130.2878126,ダンス目つむり,頬０涙０",
			"130.8014491,ダンス目あけ,頬０涙０",
			"131.2652867,ダンス目つむり,頬０涙０",
			"131.3811421,ダンス目つむり,頬１涙０",
			"131.6628105,ダンス目あけ,頬１涙０",
			"132.1763617,ダンス目あけ,頬０涙０",
			"133.9490564,ダンスウインク,頬０涙０"
		};

		private string[] dance13KArray = new string[]
		{
			"0,通常,",
			"0.0251955,ダンス目つむり,頬０涙０",
			"2.0297081,ダンス目あけ,頬０涙０",
			"4.3324486,ダンス困り顔,頬２涙０",
			"5.5583632,にっこり,頬２涙０",
			"6.0552107,ダンス目あけ,頬２涙０",
			"7.0160737,ダンス目つむり,頬０涙０",
			"7.9108997,ダンス微笑み,頬０涙０",
			"9.4847418,ダンス目あけ,頬０涙０",
			"11.1247836,ダンス目とじ,頬０涙０",
			"11.3732825,ダンス目とじ,頬１涙０",
			"12.6655081,ダンス微笑み,頬１涙０",
			"13.5269664,ダンス目あけ,頬１涙０",
			"13.7642839,ダンス目あけ,頬０涙０",
			"14.2116393,ダンス目つむり,頬０涙０",
			"14.7086602,ダンス目あけ,頬０涙０",
			"15.7026599,ダンスウインク,頬０涙０",
			"16.1830619,ダンス目あけ,頬０涙０",
			"16.7132269,ダンス目つむり,頬０涙０",
			"17.2267636,ダンス目あけ,頬０涙０",
			"17.8231713,ダンスウインク,頬１涙０",
			"18.5521064,ダンス微笑み,頬０涙０",
			"19.1980169,ダンス目あけ,頬０涙０",
			"20.5069874,ダンス微笑み,頬０涙０",
			"21.9814248,ダンス目あけ,頬０涙０",
			"23.3743559,ダンスウインク,頬０涙０",
			"23.9373588,ダンス目あけ,頬１涙０",
			"25.0806679,ダンス目とじ,頬０涙０",
			"25.6438866,ダンス目あけ,頬０涙０",
			"26.9029585,ダンス目つむり,頬０涙０",
			"31.3593741,ダンス目あけ,頬０涙０",
			"32.6184391,ダンスウインク,頬０涙０",
			"33.1320249,ダンス目あけ,頬０涙０",
			"33.6786978,ダンス目つむり,頬０涙０",
			"37.9361873,ダンス目あけ,頬０涙０",
			"39.1291656,ダンス微笑み,頬０涙０",
			"40.1728163,ダンスウインク,頬１涙０",
			"41.0840484,ダンス目あけ,頬０涙０",
			"42.7291353,ダンス微笑み,頬０涙０",
			"44.3692508,ダンス目つむり,頬０涙０",
			"44.9324961,ダンス目あけ,頬０涙０",
			"45.8766405,ダンスウインク,頬０涙０",
			"46.5063297,ダンス目あけ,頬０涙０",
			"47.8482422,ダンス微笑み,頬０涙０",
			"49.3889912,ダンスウインク,頬０涙０",
			"50.3165035,ダンス微笑み,頬０涙０",
			"51.3767661,ダンス目つむり,頬０涙０",
			"51.9566399,ダンス目あけ,頬０涙０",
			"53.0005223,ダンス微笑み,頬０涙０",
			"54.1602003,ダンス目つむり,頬０涙０",
			"54.9552195,ダンス微笑み,頬０涙０",
			"55.8830678,ダンスウインク,頬０涙０",
			"56.3800998,ダンス目あけ,頬０涙０",
			"56.8769259,ダンス目とじ,頬０涙０",
			"57.7884919,ダンス目あけ,頬０涙０",
			"58.6000096,ダンス目つむり,頬０涙０",
			"59.1301469,ダンス目あけ,頬０涙０",
			"59.6271954,ダンス目つむり,頬０涙０",
			"60.4058393,ダンス目あけ,頬０涙０",
			"61.3669501,ダンスウインク,頬０涙０",
			"61.8473105,ダンス微笑み,頬０涙０",
			"62.3443256,ダンス目とじ,頬０涙０",
			"62.9905081,ダンス目あけ,頬０涙０",
			"63.9015874,ダンス目つむり,頬０涙０",
			"64.8955037,ダンス微笑み,頬０涙０",
			"65.3925755,ダンス目あけ,頬０涙０",
			"65.9061981,ダンスびっくり,頬０涙０",
			"66.4031964,ダンス微笑み,頬０涙０",
			"66.900185,ダンス目とじ,頬０涙０",
			"67.3970294,ダンス微笑み,頬０涙０",
			"68.4408588,ダンス目つむり,頬０涙０",
			"68.9376878,ダンス目あけ,頬０涙０",
			"69.7496366,ダンスウインク,頬０涙０",
			"70.2466657,ダンス目あけ,頬０涙０",
			"72.6985268,ダンス微笑み,頬０涙０",
			"73.1953361,ダンス目あけ,頬０涙０",
			"73.8250412,ダンス微笑み,頬０涙０",
			"74.3386327,ダンスウインク,頬０涙０",
			"75.3325732,ダンス微笑み,頬０涙０",
			"76.2437907,ダンスウインク,頬０涙０",
			"77.3537624,ダンス目あけ,頬０涙０",
			"78.1323978,ダンス微笑み,頬０涙０",
			"79.010486,ダンス目あけ,頬０涙０",
			"79.9882784,ダンス目とじ,頬０涙０",
			"81.0372761,ダンス目あけ,頬０涙０",
			"81.5344486,ダンス目つむり,頬０涙０",
			"82.0975715,ダンス目あけ,頬０涙０",
			"83.0254721,ダンス微笑み,頬０涙０",
			"83.9364667,ダンス目つむり,頬０涙０",
			"84.4336292,ダンス目あけ,頬０涙０",
			"84.9306575,ダンス微笑み,頬０涙０",
			"85.7921528,ダンスウインク,頬０涙０",
			"86.3056976,ダンス目あけ,頬０涙０",
			"86.8027084,ダンス微笑み,頬０涙０",
			"87.2996905,ダンス目あけ,頬０涙０",
			"88.0286495,ダンス目とじ,頬０涙０",
			"88.8567795,ダンス微笑み,頬０涙０",
			"89.8561412,ダンスウインク,頬０涙０",
			"90.3533761,ダンス目あけ,頬０涙０",
			"90.9000983,ダンスウインク,頬０涙０",
			"91.5295209,ダンス目あけ,頬０涙０",
			"92.3744203,ダンス目とじ,頬０涙０",
			"92.9045348,ダンスウインク,頬０涙０",
			"93.6997496,ダンス目あけ,頬０涙０",
			"94.1305278,ダンス目つむり,頬０涙０",
			"94.6274945,ダンス微笑み,頬０涙０",
			"95.1410272,ダンス目あけ,頬０涙０",
			"95.6378978,ダンス目とじ,頬０涙０",
			"96.1348725,ダンス微笑み,頬０涙０",
			"97.0296676,ダンス目つむり,頬０涙０",
			"97.5267078,ダンス目あけ,頬０涙０",
			"98.2721483,ダンスウインク,頬０涙０",
			"99.0342509,ダンス目あけ,頬０涙０",
			"99.9848215,ダンスウインク,頬０涙０",
			"100.481756,ダンス微笑み,頬０涙０",
			"100.978758,ダンス目あけ,頬０涙０",
			"101.9065872,ダンスウインク,頬０涙０",
			"102.4034792,ダンス微笑み,頬０涙０",
			"103.3973022,ダンス目つむり,頬０涙０",
			"103.8944459,ダンス微笑み,頬０涙０",
			"104.5569719,ダンス目あけ,頬０涙０",
			"105.0541521,ダンス目つむり,頬０涙０",
			"105.5512188,ダンス微笑み,頬０涙０",
			"106.0481792,ダンス目あけ,頬０涙０",
			"107.1415216,ダンスウインク,頬０涙０",
			"108.2846474,ダンス微笑み,頬０涙０",
			"109.262178,ダンス目あけ,頬０涙０",
			"110.6703594,ダンス微笑み,頬０涙０",
			"111.1837972,ダンスウインク,頬０涙０",
			"111.9625047,ダンス目あけ,頬０涙０",
			"113.1551072,ダンス目つむり,頬０涙０",
			"113.6688016,ダンス目あけ,頬０涙０",
			"114.1326738,ダンス微笑み,頬０涙０",
			"114.712519,ダンス目つむり,頬０涙０",
			"115.2095159,ダンス目あけ,頬０涙０",
			"115.7230757,ダンス目つむり,頬０涙０",
			"116.3358555,ダンス目あけ,頬０涙０",
			"116.9325197,ダンスウインク,頬０涙０",
			"118.3406067,ダンス目あけ,頬０涙０",
			"118.8374659,ダンス微笑み,頬０涙０",
			"119.3346119,ダンス目つむり,頬０涙０",
			"122.6644872,ダンス目あけ,頬１涙０",
			"123.9070343,ダンス目とじ,頬０涙０",
			"124.7684815,ダンス微笑み,頬１涙０",
			"125.696246,ダンス目とじ,頬１涙０",
			"126.6405199,ダンス目あけ,頬１涙０",
			"127.4522888,ダンス目つむり,頬０涙０",
			"127.9543354,ダンス目あけ,頬０涙０",
			"128.4512898,ダンス微笑み,頬０涙０",
			"129.2630866,ダンスウインク,頬０涙０",
			"130.2904084,ダンス目あけ,頬０涙０",
			"131.2345056,ダンス目とじ,頬０涙０",
			"132.2119475,ダンス微笑み,頬０涙０",
			"133.6863864,ダンス目あけ,頬０涙０",
			"134.1834223,ダンス目とじ,頬０涙０",
			"134.9950253,ダンス目あけ,頬０涙０",
			"135.7075525,ダンス目あけ,頬１涙０",
			"135.8069179,ダンス目つむり,頬１涙０",
			"136.4530246,ダンス微笑み,頬１涙０",
			"136.5027739,ダンス微笑み,頬０涙０",
			"137.2490577,ダンス目あけ,頬０涙０",
			"138.276022,ダンス微笑み,頬０涙０",
			"140.9434639,ダンス目とじ,頬０涙０"
		};

		private string[] dance15Array = new string[]
		{
			"0.0244373,ダンス目つむり,頬０涙０",
			"8.1921419,ダンス真剣,頬０涙０",
			"10.0310778,ダンスキス,頬０涙０",
			"11.3729807,ダンスウインク,頬０涙０",
			"12.6485482,ダンス目あけ,頬０涙０",
			"13.2283948,ダンスキス,頬０涙０",
			"15.365455,ダンス真剣,頬０涙０",
			"16.3768715,ダンス微笑み,頬０涙０",
			"17.4555632,ダンス目あけ,頬０涙０",
			"19.045942,ダンス目とじ,頬０涙０",
			"20.0399116,ダンス目あけ,頬０涙０",
			"21.1995915,ダンスキス,頬０涙０",
			"22.0113959,ダンス微笑み,頬０涙０",
			"22.6242092,ダンス真剣,頬０涙０",
			"23.0385257,ダンス微笑み,頬０涙０",
			"24.9435833,ダンスキス,頬０涙０",
			"25.8714355,ダンス真剣,頬０涙０",
			"26.8322979,ダンス目つむり,頬０涙０",
			"27.2795984,ダンス憂い,頬０涙０",
			"30.0323892,ダンス真剣,頬０涙０",
			"35.7810111,ダンス目とじ,頬０涙０",
			"36.9572983,ダンス真剣,頬０涙０",
			"38.3818793,ダンス目とじ,頬０涙０",
			"39.2767092,ダンスウインク,頬０涙０",
			"40.0261239,ダンス目つむり,頬０涙０",
			"42.1976454,ダンス目あけ,頬０涙０",
			"43.108821,ダンスキス,頬０涙０",
			"44.5335627,ダンス真剣,頬０涙０",
			"46.2896102,ダンス憂い,頬０涙０",
			"47.9462895,ダンス目つむり,頬０涙０",
			"49.4538347,ダンス微笑み,頬１涙０",
			"51.1933359,ダンス目あけ,頬１涙０",
			"52.1890531,ダンス目つむり,頬１涙０",
			"53.2827596,ダンス目つむり,頬０涙０",
			"57.0771787,ダンス真剣,頬０涙０",
			"61.6992448,ダンス微笑み,頬０涙０",
			"62.6767026,ダンス目あけ,頬０涙０",
			"63.6209676,ダンス目とじ,頬０涙０",
			"64.3855063,ダンス目あけ,頬０涙０",
			"65.1309883,ダンスウインク,頬０涙０",
			"65.5285618,ダンス目つむり,頬０涙０",
			"68.1129863,ダンスウインク,頬０涙０",
			"69.040703,ダンスキス,頬０涙０",
			"69.6370772,ダンス真剣,頬０涙０",
			"72.3706024,ダンス目つむり,頬０涙０",
			"73.033287,ダンス微笑み,頬０涙０",
			"74.0273111,ダンス目つむり,頬０涙０",
			"75.0378458,ダンスウインク,頬０涙０",
			"76.0328962,ダンス真剣,頬０涙０",
			"79.2799457,ダンスウインク,頬０涙０",
			"81.2845224,ダンス微笑み,頬０涙０",
			"85.0312434,ダンス目あけ,頬０涙０",
			"87.1369255,ダンス微笑み,頬０涙０",
			"89.7724846,ダンス目とじ,頬０涙０",
			"91.7770399,ダンス微笑み,頬０涙０",
			"93.8994613,ダンス目つむり,頬０涙０",
			"94.5786928,ダンス憂い,頬０涙０",
			"95.0259985,ダンス憂い,頬１涙０",
			"97.0305543,ダンス微笑み,頬１涙０",
			"98.5381682,ダンス真剣,頬１涙０",
			"100.0308292,ダンス目つむり,頬１涙０",
			"101.5249262,ダンス微笑み,頬１涙０",
			"104.0265165,ダンスキス,頬１涙０",
			"105.8818599,ダンス微笑み,頬１涙０",
			"107.2900852,ダンス真剣,頬１涙０",
			"109.0338797,ダンス目つむり,頬１涙０",
			"109.8621056,ダンス微笑み,頬１涙０",
			"109.8787737,ダンス微笑み,頬０涙０",
			"112.9453398,ダンス真剣,頬０涙０",
			"114.3700753,ダンス目つむり,頬０涙０",
			"115.122713,ダンス目あけ,頬０涙０",
			"116.3652856,ダンス微笑み,頬０涙０",
			"118.6183732,ダンス真剣,頬０涙０",
			"122.7035139,ダンス目つむり,頬０涙０",
			"126.0343835,ダンスウインク,頬０涙０",
			"127.0283788,ダンス真剣,頬０涙０",
			"129.2813044,ダンスウインク,頬０涙０",
			"130.7889477,ダンス目あけ,頬０涙０",
			"132.0366529,ダンス微笑み,頬０涙０",
			"134.7037246,ダンス目つむり,頬０涙０",
			"138.1200687,ダンス真剣,頬０涙０",
			"139.0312258,ダンス微笑み,頬０涙０",
			"140.1080626,ダンス目あけ,頬０涙０",
			"140.7043233,ダンスキス,頬０涙０",
			"141.6236835,ダンス微笑み,頬０涙０",
			"142.6999516,ダンス目つむり,頬０涙０",
			"143.1968722,ダンスウインク,頬０涙０",
			"145.0358056,ダンス微笑み,頬０涙０",
			"146.5267856,ダンス目つむり,頬０涙０",
			"148.6141985,ダンス目あけ,頬０涙０"
		};

		private string[] dance15BArray = new string[]
		{
			"0.0245382,ダンス目つむり,頬０涙０",
			"8.1921504,ダンス真剣,頬０涙０",
			"10.0310855,ダンス目つむり,頬０涙０",
			"11.3729899,ダンスウインク,頬０涙０",
			"12.6485567,ダンス目あけ,頬０涙０",
			"13.228402,ダンス目つむり,頬０涙０",
			"15.3654621,ダンス真剣,頬０涙０",
			"16.3769017,ダンス微笑み,頬０涙０",
			"17.4555695,ダンス目あけ,頬０涙０",
			"19.04595,ダンス目とじ,頬０涙０",
			"20.0399201,ダンス目あけ,頬０涙０",
			"21.1995992,ダンスキス,頬０涙０",
			"22.0114025,ダンスウインク,頬０涙０",
			"22.6242149,ダンス真剣,頬０涙０",
			"23.0385337,ダンス微笑み,頬０涙０",
			"24.9435899,ダンスキス,頬０涙０",
			"25.8714421,ダンス真剣,頬０涙０",
			"26.8323059,ダンス目つむり,頬０涙０",
			"27.279607,ダンス真剣,頬０涙０",
			"30.0323975,ダンス憂い,頬０涙０",
			"35.7810188,ダンス目とじ,頬０涙０",
			"36.9573112,ダンス真剣,頬０涙０",
			"38.3818867,ダンス目とじ,頬０涙０",
			"39.2767195,ダンスウインク,頬０涙０",
			"40.0261322,ダンス目つむり,頬０涙０",
			"42.1976674,ダンス目あけ,頬０涙０",
			"43.1088275,ダンスキス,頬０涙０",
			"44.5335696,ダンス微笑み,頬０涙０",
			"46.2896188,ダンスウインク,頬０涙０",
			"47.9462963,ダンス目つむり,頬０涙０",
			"49.4538421,ダンス微笑み,頬１涙０",
			"51.1933424,ダンス目あけ,頬１涙０",
			"52.1890622,ダンス目つむり,頬１涙０",
			"53.2827815,ダンス目つむり,頬０涙０",
			"57.0771883,ダンス真剣,頬０涙０",
			"61.6992542,ダンス微笑み,頬０涙０",
			"62.6767129,ダンスウインク,頬０涙０",
			"63.3725086,ダンス憂い,頬０涙０",
			"65.5285726,ダンス目つむり,頬０涙０",
			"68.1129968,ダンスウインク,頬０涙０",
			"69.040713,ダンスキス,頬０涙０",
			"69.6370863,ダンス真剣,頬０涙０",
			"72.3706121,ダンス目つむり,頬０涙０",
			"73.033297,ダンス微笑み,頬０涙０",
			"74.0273202,ダンス目つむり,頬０涙０",
			"75.0378683,ダンスウインク,頬０涙０",
			"76.0329184,ダンス真剣,頬０涙０",
			"79.2799548,ダンスウインク,頬０涙０",
			"81.2845318,ダンス微笑み,頬０涙０",
			"85.0312534,ダンス目あけ,頬０涙０",
			"87.1369352,ダンス微笑み,頬０涙０",
			"89.7724937,ダンス目とじ,頬０涙０",
			"91.7770502,ダンス微笑み,頬０涙０",
			"93.8994708,ダンス目つむり,頬０涙０",
			"94.5787022,ダンス微笑み,頬０涙０",
			"97.0305638,ダンス憂い,頬１涙０",
			"98.5381782,ダンス真剣,頬１涙０",
			"100.0308392,ダンス目つむり,頬１涙０",
			"101.524935,ダンス微笑み,頬１涙０",
			"104.0265242,ダンスキス,頬１涙０",
			"105.8818707,ダンス微笑み,頬１涙０",
			"107.2900929,ダンス真剣,頬１涙０",
			"109.0338889,ダンス目つむり,頬１涙０",
			"109.8621153,ダンス微笑み,頬１涙０",
			"109.8787837,ダンス微笑み,頬０涙０",
			"112.945349,ダンス真剣,頬０涙０",
			"113.6080296,ダンス目つむり,頬０涙０",
			"115.1227222,ダンス目あけ,頬０涙０",
			"116.365295,ダンス微笑み,頬０涙０",
			"118.2870452,ダンス真剣,頬０涙０",
			"119.1981817,ダンス目つむり,頬０涙０",
			"119.778046,ダンス真剣,頬０涙０",
			"122.7035236,ダンス目つむり,頬０涙０",
			"125.8686566,ダンスウインク,頬０涙０",
			"127.0283879,ダンス真剣,頬０涙０",
			"129.2813136,ダンス目あけ,頬０涙０",
			"130.8769011,ダンスウインク,頬０涙０",
			"132.0366615,ダンス微笑み,頬０涙０",
			"134.7037349,ダンス目つむり,頬０涙０",
			"138.1200784,ダンス真剣,頬０涙０",
			"139.0312343,ダンス微笑み,頬０涙０",
			"140.108072,ダンス目あけ,頬０涙０",
			"140.7043319,ダンスキス,頬０涙０",
			"141.6236926,ダンス微笑み,頬０涙０",
			"142.699961,ダンス目つむり,頬０涙０",
			"143.1968945,ダンスウインク,頬０涙０",
			"145.0358153,ダンス微笑み,頬０涙０",
			"146.5267973,ダンス目つむり,頬０涙０",
			"148.6142182,ダンス目あけ,頬０涙０",
			"150.0389197,ダンス目とじ,頬０涙０",
			"153.0375153,ダンス目つむり,頬０涙０"
		};

		private string[] dance1KArray = new string[]
		{
			"0.024614,ダンス目とじ,頬１涙０",
			"3.0892278,ダンス微笑み,頬１涙０",
			"5.2098572,ダンス目とじ,頬１涙０",
			"6.9992602,ダンス微笑み,頬１涙０",
			"9.4845271,ダンス微笑み,頬０涙０",
			"11.7375667,ダンス目とじ,頬０涙０",
			"15.5644962,ダンス微笑み,頬０涙０",
			"17.8053091,ダンス目とじ,頬０涙０",
			"18.9335827,ダンス目あけ,頬０涙０",
			"22.1806802,ダンス目とじ,頬０涙０",
			"25.9744178,ダンス微笑み,頬０涙０",
			"29.1558244,ダンス目とじ,頬０涙０",
			"32.8004969,ダンス微笑み,頬０涙０",
			"36.8766835,ダンス目つむり,頬０涙０",
			"37.1959725,ダンス微笑み,頬０涙０",
			"41.3376566,ダンス目とじ,頬０涙０",
			"41.3707587,ダンス目とじ,頬１涙０",
			"42.4641791,ダンス微笑み,頬１涙０",
			"45.0320269,ダンス目とじ,頬１涙０",
			"46.6891689,ダンス誘惑,頬１涙０",
			"47.1034045,ダンス誘惑,頬０涙０",
			"50.2842089,ダンス目とじ,頬０涙０",
			"53.8460456,ダンス微笑み,頬０涙０",
			"54.4424561,ダンス微笑み,頬１涙０",
			"55.8218479,ダンス目つむり,頬１涙０",
			"55.9377951,ダンス微笑み,頬１涙０",
			"57.6274866,ダンス目つむり,頬１涙０",
			"57.7268614,ダンス微笑み,頬１涙０",
			"60.3942744,ダンスウインク,頬１涙０",
			"61.3550997,ダンス微笑み,頬１涙０",
			"63.7904103,ダンスウインク,頬１涙０",
			"64.3371298,ダンスウインク,頬０涙０",
			"64.4729873,ダンス微笑み,頬０涙０",
			"66.2124488,ダンス目つむり,頬０涙０",
			"66.2787238,ダンス微笑み,頬０涙０",
			"67.5212394,ダンス目とじ,頬０涙０",
			"70.1718831,ダンスウインク,頬０涙０",
			"70.7351236,ダンス微笑み,頬０涙０",
			"71.8616567,ダンス目とじ,頬０涙０",
			"73.9994652,ダンス微笑み,頬０涙０",
			"74.4466154,ダンス目つむり,頬０涙０",
			"74.5632566,ダンス微笑み,頬０涙０",
			"74.9443042,ダンス微笑み,頬１涙０",
			"75.4578721,ダンス目とじ,頬１涙０",
			"76.3358862,ダンス微笑み,頬１涙０",
			"77.0648168,ダンスウインク,頬１涙０",
			"77.9262998,ダンス目あけ,頬１涙０",
			"79.8811354,ダンス目つむり,頬１涙０",
			"80.0302585,ダンス目あけ,頬１涙０",
			"80.7426982,ダンス目つむり,頬１涙０",
			"80.8917603,ダンス目あけ,頬１涙０",
			"82.7636732,ダンス目つむり,頬１涙０",
			"82.8631591,ダンス目あけ,頬１涙０",
			"83.7089302,ダンス微笑み,頬１涙０",
			"86.0638965,ダンス目つむり,頬１涙０",
			"86.113604,ダンス目つむり,頬０涙０",
			"87.057886,ダンス微笑み,頬０涙０",
			"88.6481944,ダンス目とじ,頬０涙０",
			"89.4766338,ダンス目つむり,頬０涙０",
			"89.6257232,ダンス微笑み,頬０涙０",
			"91.0007732,ダンス目つむり,頬０涙０",
			"91.183016,ダンス目あけ,頬０涙０",
			"93.369805,ダンス目とじ,頬０涙０",
			"93.5851641,ダンス微笑み,頬０涙０",
			"95.3079402,ダンス目つむり,頬０涙０",
			"95.854837,ダンス目つむり,頬１涙０",
			"96.7162334,ダンス微笑み,頬１涙０",
			"97.9753299,ダンスウインク,頬１涙０",
			"99.7313514,ダンスウインク,頬０涙０",
			"99.7976872,ダンス微笑み,頬０涙０",
			"103.4102772,ダンス目つむり,頬０涙０",
			"103.6256442,ダンス目とじ,頬０涙０",
			"107.0384221,ダンス微笑み,頬０涙０",
			"107.154347,ダンス微笑み,頬２涙０",
			"107.8998016,ダンスウインク,頬２涙０",
			"108.5790526,ダンス目とじ,頬２涙０",
			"111.0473509,ダンス目とじ,頬０涙０",
			"112.0937347,ダンス微笑み,頬０涙０",
			"114.114855,ダンス目とじ,頬０涙０",
			"118.1072473,ダンス微笑み,頬０涙０",
			"119.2007989,ダンス目とじ,頬０涙０",
			"122.3995212,ダンス微笑み,頬０涙０",
			"123.0623415,ダンスウインク,頬０涙０",
			"124.2053132,ダンス微笑み,頬０涙０",
			"126.8889363,ダンス目つむり,頬０涙０",
			"126.9883747,ダンス目あけ,頬０涙０",
			"128.08194,ダンス目つむり,頬０涙０",
			"128.2145676,ダンス目あけ,頬０涙０",
			"129.5729429,ダンス目つむり,頬０涙０",
			"129.7054499,ダンス微笑み,頬０涙０",
			"130.3213277,ダンス目とじ,頬０涙０",
			"133.3198854,ダンス微笑み,頬０涙０",
			"135.3576243,ダンス目つむり,頬０涙０",
			"135.5730125,ダンス微笑み,頬０涙０",
			"136.6497773,ダンスウインク,頬０涙０",
			"137.1634093,ダンス目とじ,頬０涙０",
			"138.9525847,ダンス目あけ,頬０涙０",
			"140.2456437,ダンス微笑み,頬０涙０",
			"143.9400383,ダンスウインク,頬０涙０"
		};

		private string[] dance3KArray = new string[]
		{
			"0.0260803,ダンス目つむり,頬０涙０",
			"4.0184395,ダンス目あけ,頬０涙０",
			"5.1615692,ダンス誘惑,頬０涙０",
			"7.1494116,ダンス目つむり,頬０涙０",
			"8.0275314,ダンス目あけ,頬０涙０",
			"8.7399897,ダンス微笑み,頬０涙０",
			"11.5894007,ダンス目つむり,頬０涙０",
			"13.727419,ダンス目あけ,頬０涙０",
			"14.9864808,ダンス微笑み,頬０涙０",
			"18.1341635,ダンス誘惑,頬０涙０",
			"19.7080238,ダンス目つむり,頬０涙０",
			"22.4911905,ダンス微笑み,頬０涙０",
			"26.3015251,ダンス目つむり,頬０涙０",
			"26.3841892,ダンス微笑み,頬０涙０",
			"27.5274546,ダンス誘惑,頬０涙０",
			"29.995886,ダンス目つむり,頬０涙０",
			"32.0667651,ダンス微笑み,頬０涙０",
			"35.0156108,ダンス目つむり,頬０涙０",
			"35.3800493,ダンス誘惑,頬０涙０",
			"38.5441231,ダンス目つむり,頬０涙０",
			"40.0187064,ダンス微笑み,頬０涙０",
			"44.2622979,ダンス微笑み,頬１涙０",
			"46.3497327,ダンス目つむり,頬１涙０",
			"48.2382773,ダンス誘惑,頬１涙０",
			"51.3882856,ダンス目つむり,頬１涙０",
			"51.7194997,ダンス微笑み,頬１涙０",
			"53.8068852,ダンス目つむり,頬１涙０",
			"53.9725849,ダンス目つむり,頬０涙０",
			"55.3807463,ダンス目あけ,頬０涙０",
			"58.8597556,ダンス誘惑,頬０涙０",
			"63.0350735,ダンス微笑み,頬０涙０",
			"66.0170919,ダンス目つむり,頬０涙０",
			"66.4975289,ダンス真剣,頬０涙０",
			"67.4915161,ダンス目つむり,頬０涙０",
			"70.1589845,ダンス微笑み,頬０涙０",
			"72.5280088,ダンス目とじ,頬０涙０",
			"72.5611251,ダンス目とじ,頬１涙０",
			"74.383284,ダンス誘惑,頬１涙０",
			"78.6576917,ダンス微笑み,頬１涙０",
			"80.9604379,ダンス微笑み,頬２涙０",
			"81.6229175,ダンスウインク,頬２涙０",
			"82.4017119,ダンスウインク,頬１涙０",
			"82.53426,ダンス誘惑,頬１涙０",
			"84.7043682,ダンス目とじ,頬１涙０",
			"85.5659747,ダンス目とじ,頬０涙０",
			"86.1125967,ダンス微笑み,頬０涙０",
			"87.6215505,ダンス目つむり,頬０涙０",
			"88.8638847,ダンス微笑み,頬０涙０",
			"91.4319683,ダンスウインク,頬０涙０",
			"92.6744348,ダンス微笑み,頬０涙０",
			"94.2646141,ダンス目とじ,頬０涙０",
			"96.5178361,ダンス誘惑,頬０涙０",
			"99.1190625,ダンス誘惑,頬１涙０",
			"100.4278586,ダンスウインク,頬１涙０",
			"101.1401811,ダンス誘惑,頬１涙０",
			"101.9519786,ダンス誘惑,頬０涙０",
			"103.1446088,ダンス目つむり,頬０涙０",
			"104.5030744,ダンス誘惑,頬０涙０",
			"106.3757308,ダンスウインク,頬０涙０",
			"107.7010112,ダンス目とじ,頬０涙０",
			"110.2852569,ダンス微笑み,頬０涙０",
			"112.5550391,ダンス目つむり,頬０涙０",
			"113.930068,ダンス微笑み,頬０涙０",
			"116.133477,ダンス誘惑,頬０涙０",
			"119.9935955,ダンス目つむり,頬０涙０",
			"121.4016705,ダンス誘惑,頬０涙０",
			"123.3400281,ダンス目つむり,頬０涙０",
			"123.687935,ダンス真剣,頬０涙０",
			"128.9892622,ダンス目つむり,頬０涙０",
			"129.0886005,ダンス真剣,頬０涙０",
			"131.3918331,ダンス目つむり,頬０涙０",
			"131.6730031,ダンス誘惑,頬０涙０",
			"134.9410897,ダンス目つむり,頬０涙０",
			"135.1742106,ダンス誘惑,頬０涙０",
			"139.7797389,ダンス目つむり,頬０涙０",
			"141.6517621,ダンス微笑み,頬０涙０",
			"143.8585818,ダンス目つむり,頬０涙０",
			"144.3390419,ダンス誘惑,頬０涙０",
			"146.8572418,ダンス目つむり,頬０涙０",
			"146.9897104,ダンス誘惑,頬０涙０",
			"147.8677376,ダンス目つむり,頬０涙０",
			"147.9505508,ダンス誘惑,頬０涙０",
			"150.418991,ダンス目つむり,頬０涙０",
			"150.5680987,ダンス誘惑,頬０涙０",
			"151.7277449,ダンス目つむり,頬０涙０",
			"151.7940191,ダンス誘惑,頬０涙０",
			"152.6720602,ダンス目つむり,頬０涙０",
			"153.5670626,ダンス誘惑,頬０涙０",
			"155.6710016,ダンス目とじ,頬０涙０",
			"160.6413847,ダンス微笑み,頬０涙０"
		};

		private string[] dance4KArray = new string[]
		{
			"0.0390596,ダンス目つむり,頬０涙０",
			"3.2030766,ダンス目あけ,頬０涙０",
			"4.0809924,ダンス目つむり,頬０涙０",
			"5.4065284,ダンス目あけ,頬０涙０",
			"6.5989211,ダンス真剣,頬０涙０",
			"7.0961806,ダンス目あけ,頬０涙０",
			"7.7091605,ダンス微笑み,頬０涙０",
			"8.5882934,ダンス目とじ,頬０涙０",
			"9.5325854,ダンス目あけ,頬０涙０",
			"10.3443782,ダンス目つむり,頬０涙０",
			"11.2387443,ダンス目あけ,頬０涙０",
			"11.8022357,ダンス目つむり,頬０涙０",
			"12.332284,ダンス目あけ,頬０涙０",
			"13.0776496,ダンス微笑み,頬０涙０",
			"13.9227005,ダンス目つむり,頬０涙０",
			"14.9664734,ダンス目あけ,頬０涙０",
			"15.8774996,ダンス目つむり,頬０涙０",
			"16.6394797,ダンス目あけ,頬０涙０",
			"17.7338043,ダンス微笑み,頬０涙０",
			"20.1191753,ダンス目つむり,頬０涙０",
			"21.2129083,ダンス目あけ,頬０涙０",
			"21.8587027,ダンス目つむり,頬０涙０",
			"23.1177732,ダンス真剣,頬０涙０",
			"23.7640867,ダンス目つむり,頬０涙０",
			"24.5758114,ダンス目あけ,頬０涙０",
			"25.4372818,ダンス微笑み,頬０涙０",
			"26.4312645,ダンス目とじ,頬０涙０",
			"26.9364811,ダンス目あけ,頬０涙０",
			"27.416991,ダンス微笑み,頬０涙０",
			"27.9139351,ダンス目あけ,頬０涙０",
			"28.7919943,ダンス目つむり,頬０涙０",
			"29.288993,ダンス目あけ,頬０涙０",
			"30.0676118,ダンス微笑み,頬０涙０",
			"30.7798625,ダンス真剣,頬０涙０",
			"31.2770056,ダンス目あけ,頬０涙０",
			"31.7740211,ダンス目とじ,頬０涙０",
			"33.0827398,ダンス目あけ,頬０涙０",
			"34.225821,ダンス目つむり,頬０涙０",
			"34.8719282,ダンス目あけ,頬０涙０",
			"35.6340238,ダンス目とじ,頬０涙０",
			"36.3133177,ダンス目あけ,頬０涙０",
			"36.9591235,ダンス微笑み,頬０涙０",
			"37.7214098,ダンス真剣,頬０涙０",
			"38.4668949,ダンス目あけ,頬０涙０",
			"38.9639066,ダンス微笑み,頬０涙０",
			"39.8087537,ダンス目つむり,頬０涙０",
			"40.753213,ダンス目あけ,頬０涙０",
			"41.250107,ダンス微笑み,頬０涙０",
			"42.4097571,ダンス目つむり,頬０涙０",
			"43.6026647,ダンス目あけ,頬０涙０",
			"44.364642,ダンス目つむり,頬０涙０",
			"45.4083385,ダンス目あけ,頬０涙０",
			"45.9896155,ダンス微笑み,頬０涙０",
			"47.8780179,ダンス目つむり,頬０涙０",
			"48.805956,ダンス目あけ,頬０涙０",
			"49.319361,ダンス目つむり,頬０涙０",
			"49.8165016,ダンス微笑み,頬０涙０",
			"50.4294441,ダンス目あけ,頬０涙０",
			"50.9265534,ダンス微笑み,頬０涙０",
			"51.6719757,ダンス目あけ,頬０涙０",
			"52.2352248,ダンス目とじ,頬０涙０",
			"53.1960982,ダンス微笑み,頬０涙０",
			"54.4883578,ダンス目つむり,頬０涙０",
			"55.0515963,ダンス真剣,頬０涙０",
			"55.5485673,ダンス微笑み,頬０涙０",
			"56.376913,ダンス目つむり,頬０涙０",
			"57.6525316,ダンス目あけ,頬０涙０",
			"58.6299651,ダンス真剣,頬０涙０",
			"59.1274178,ダンス微笑み,頬０涙０",
			"60.4357454,ダンス目とじ,頬０涙０",
			"61.5622908,ダンス微笑み,頬０涙０",
			"62.3906209,ダンス誘惑,頬０涙０",
			"62.9206238,ダンス目あけ,頬０涙０",
			"63.4176381,ダンス目とじ,頬０涙０",
			"64.2522709,ダンス微笑み,頬０涙０",
			"65.4450488,ダンス目つむり,頬０涙０",
			"67.3674266,ダンス目あけ,頬０涙０",
			"68.874319,ダンスウインク,頬０涙０",
			"69.7358011,ダンス誘惑,頬０涙０",
			"71.4421884,ダンス目とじ,頬０涙０",
			"71.9390185,ダンス微笑み,頬０涙０",
			"73.2645416,ダンス誘惑,頬０涙０",
			"75.0721743,ダンス目つむり,頬０涙０",
			"75.850774,ダンス目あけ,頬０涙０",
			"76.5963036,ダンス目とじ,頬１涙０",
			"78.7499597,ダンス微笑み,頬０涙０",
			"79.8598282,ダンス目つむり,頬０涙０",
			"80.6054225,ダンス目あけ,頬０涙０",
			"81.4503634,ダンス目とじ,頬１涙０",
			"82.6928624,ダンス目あけ,頬０涙０",
			"83.9253542,ダンス微笑み,頬０涙０",
			"84.5880833,ダンス目つむり,頬０涙０",
			"85.4330045,ダンス目あけ,頬０涙０",
			"87.1227716,ダンス目とじ,頬１涙０",
			"88.2161675,ダンス目あけ,頬０涙０",
			"88.8954039,ダンス目とじ,頬１涙０",
			"89.9556404,ダンス目あけ,頬０涙０",
			"90.4692401,ダンス微笑み,頬０涙０",
			"91.247861,ダンス目つむり,頬０涙０",
			"92.6286299,ダンス目あけ,頬０涙０",
			"93.1256608,ダンス微笑み,頬０涙０",
			"94.7326308,ダンス目つむり,頬０涙０",
			"96.2074643,ダンス微笑み,頬０涙０",
			"97.4826829,ダンス目とじ,頬１涙０",
			"98.4269923,ダンス目あけ,頬０涙０",
			"98.923955,ダンス微笑み,頬０涙０",
			"101.2267519,ダンスウインク,頬０涙０",
			"101.7237647,ダンス微笑み,頬０涙０",
			"102.3952831,ダンス目とじ,頬１涙０",
			"103.5880625,頬０涙０,頬１涙０",
			"104.6151758,ダンス目つむり,頬１涙０",
			"105.2115332,ダンス微笑み,頬１涙０",
			"106.3379196,ダンス目あけ,頬１涙０",
			"107.0339041,ダンス目つむり,頬１涙０",
			"107.5309541,ダンス目あけ,頬１涙０",
			"108.0774198,ダンス目つむり,頬１涙０",
			"108.5746314,ダンス目あけ,頬１涙０",
			"109.7508698,ダンス微笑み,頬１涙０",
			"111.5093991,ダンス目あけ,頬１涙０",
			"112.3046067,ダンス微笑み,頬１涙０",
			"113.2654448,ダンス目つむり,頬１涙０",
			"114.4913987,ダンス目あけ,頬１涙０",
			"115.4522613,ダンス目つむり,頬１涙０",
			"116.4297193,ダンス誘惑,頬１涙０",
			"117.8875679,ダンス目あけ,頬１涙０",
			"119.0637975,ダンスウインク,頬１涙０",
			"119.8588381,ダンス目あけ,頬１涙０",
			"120.5713371,ダンス微笑み,頬１涙０",
			"121.4502964,ダンス目つむり,頬１涙０",
			"122.8418159,ダンス微笑み,頬１涙０",
			"123.5542051,ダンス目つむり,頬１涙０",
			"124.5150954,ダンス目あけ,頬１涙０",
			"126.0723479,ダンス真剣,頬１涙０",
			"126.5693759,ダンス目あけ,頬１涙０",
			"128.5241966,ダンス微笑み,頬１涙０",
			"129.7501548,ダンス目つむり,頬１涙０",
			"131.3245341,ダンス目あけ,頬１涙０",
			"132.5504769,ダンス目つむり,頬１涙０",
			"133.0474803,ダンス真剣,頬１涙０",
			"133.5444937,ダンス目つむり,頬１涙０",
			"134.7704211,ダンス誘惑,頬１涙０",
			"135.7643844,ダンス目とじ,頬１涙０",
			"137.3547992,ダンス誘惑,頬０涙０",
			"138.4316691,ダンス目あけ,頬０涙０",
			"139.3096544,ダンス目とじ,頬１涙０",
			"140.8174766,ダンス微笑み,頬０涙０",
			"141.7783429,ダンス目あけ,頬０涙０",
			"142.2755077,ダンス微笑み,頬０涙０",
			"144.9924189,ダンス目とじ,頬１涙０",
			"145.8371839,ダンス目あけ,頬０涙０",
			"146.4337298,ダンス目とじ,頬１涙０",
			"146.930758,ダンス目あけ,頬０涙０",
			"147.7259549,ダンス目つむり,頬０涙０",
			"148.4548888,ダンス目あけ,頬０涙０",
			"149.6176231,ダンス目とじ,頬１涙０",
			"150.8105866,頬０涙０,頬１涙０",
			"151.3076522,ダンス微笑み,頬１涙０",
			"155.0682134,ダンス目つむり,頬１涙０",
			"156.1780017,ダンス目あけ,頬１涙０",
			"156.8408702,ダンス目とじ,頬１涙０"
		};

		private string[] dance5KArray = new string[]
		{
			"0.0396033,ダンス目つむり,頬０涙０",
			"4.6280096,ダンスびっくり,頬０涙０",
			"5.7050551,ダンス目あけ,頬０涙０",
			"6.3509241,ダンスびっくり,頬０涙０",
			"6.8978401,ダンス目あけ,頬０涙０",
			"7.5935994,ダンス目つむり,頬０涙０",
			"8.0904364,ダンスびっくり,頬０涙０",
			"8.6207361,ダンスあくび,頬０涙０",
			"9.1177453,ダンス目あけ,頬０涙０",
			"9.7969933,ダンス目つむり,頬０涙０",
			"10.3437299,ダンス誘惑,頬０涙０",
			"10.9896161,ダンスあくび,頬０涙０",
			"11.6358858,ダンス誘惑,頬０涙０",
			"12.2690924,ダンスあくび,頬０涙０",
			"12.9982056,ダンス憂い,頬０涙０",
			"13.6109843,ダンスあくび,頬０涙０",
			"14.7377321,ダンス憂い,頬０涙０",
			"16.4772244,ダンスあくび,頬０涙０",
			"17.6865928,ダンス目あけ,頬０涙０",
			"18.647452,ダンスあくび,頬０涙０",
			"19.2272799,ダンス目あけ,頬０涙０",
			"19.7242839,ダンス目つむり,頬０涙０",
			"21.1492044,ダンス微笑み,頬０涙０",
			"21.6629241,ダンス目つむり,頬０涙０",
			"22.2593865,ダンス微笑み,頬０涙０",
			"23.1540294,ダンス目つむり,頬０涙０",
			"23.9822813,ダンス微笑み,頬０涙０",
			"25.5890706,ダンス目つむり,頬０涙０",
			"26.152534,ダンス目あけ,頬０涙０",
			"26.6329136,ダンス目あけ,頬１涙０",
			"26.6493727,ダンス目とじ,頬１涙０",
			"27.1297728,ダンス目とじ,頬０涙０",
			"27.1466008,ダンス微笑み,頬０涙０",
			"28.1736289,ダンス目とじ,頬１涙０",
			"29.6479013,ダンス微笑み,頬０涙０",
			"30.8586266,ダンス目とじ,頬１涙０",
			"31.4716167,ダンス微笑み,頬０涙０",
			"32.1011165,ダンス目とじ,頬１涙０",
			"32.6147253,ダンス微笑み,頬０涙０",
			"33.6252572,ダンス目あけ,頬０涙０",
			"34.1554073,ダンス目つむり,頬０涙０",
			"34.751771,ダンス微笑み,頬０涙０",
			"35.2819319,頬１涙０,頬０涙０",
			"35.994296,ダンスウインク,頬０涙０",
			"36.4910808,ダンス微笑み,頬０涙０",
			"37.2036194,ダンス目つむり,頬０涙０",
			"37.7172656,ダンス目あけ,頬０涙０",
			"38.2473213,ダンスびっくり,頬０涙０",
			"38.6781054,ダンス目つむり,頬０涙０",
			"39.1751174,ダンス困り顔,頬０涙０",
			"39.6884748,ダンス目つむり,頬０涙０",
			"40.1856658,ダンス困り顔,頬０涙０",
			"40.6826704,ダンス目つむり,頬０涙０",
			"41.3121853,ダンス憂い,頬０涙０",
			"41.9251778,ダンス困り顔,頬０涙０",
			"42.6044004,ダンス目あけ,頬０涙０",
			"43.4658169,ダンス困り顔,頬０涙０",
			"44.1781956,ダンス目つむり,頬０涙０",
			"44.7580922,ダンス困り顔,頬０涙０",
			"45.271593,ダンス憂い,頬０涙０",
			"45.768416,ダンス目つむり,頬０涙０",
			"46.282213,ダンス困り顔,頬０涙０",
			"47.0276875,ダンス目つむり,頬０涙０",
			"47.5743614,ダンス憂い,頬０涙０",
			"48.1721347,ダンス困り顔,頬０涙０",
			"48.8510931,ダンス目つむり,頬０涙０",
			"49.5470494,ダンス目あけ,頬０涙０",
			"50.1601642,ダンス目つむり,頬０涙０",
			"50.7071879,ダンス微笑み,頬０涙０",
			"51.4853765,ダンス目あけ,頬０涙０",
			"52.1979465,ダンス目とじ,頬１涙０",
			"52.810553,ダンス微笑み,頬０涙０",
			"53.953791,ダンス目とじ,頬１涙０",
			"54.6827628,ダンス微笑み,頬０涙０",
			"55.1962656,ダンス目とじ,頬１涙０",
			"55.7099719,ダンス微笑み,頬０涙０",
			"56.4883267,ダンス目とじ,頬１涙０",
			"57.0020716,頬０涙０,頬１涙０",
			"57.634405,ダンス目あけ,頬１涙０",
			"58.1313996,ダンス微笑み,頬１涙０",
			"58.6449571,ダンスウインク,頬１涙０",
			"60.0200183,ダンス目あけ,頬１涙０",
			"61.1298184,ダンス目つむり,頬１涙０",
			"61.6271871,ダンス微笑み,頬１涙０",
			"63.2339589,ダンス目とじ,頬１涙０",
			"63.8137515,ダンス目あけ,頬０涙０",
			"64.6087878,ダンス目つむり,頬０涙０",
			"65.2551219,ダンス目あけ,頬０涙０",
			"65.8348686,ダンス微笑み,頬０涙０",
			"66.7803862,ダンス目とじ,頬１涙０",
			"67.426576,ダンス微笑み,頬０涙０",
			"68.3873503,ダンス目あけ,頬０涙０",
			"69.6296583,ダンス目つむり,頬０涙０",
			"70.8723672,ダンス微笑み,頬０涙０",
			"72.2306221,ダンス目つむり,頬０涙０",
			"73.2578957,ダンス目あけ,頬０涙０",
			"74.0366363,ダンス微笑み,頬０涙０",
			"75.9251849,ダンス目とじ,頬１涙０",
			"76.7068426,ダンス微笑み,頬０涙０",
			"78.4461995,ダンス目あけ,頬０涙０",
			"79.3902854,ダンスびっくり,頬０涙０",
			"80.6992684,ダンス目つむり,頬０涙０",
			"81.2956575,ダンス目あけ,頬０涙０",
			"81.7926826,ダンス微笑み,頬０涙０",
			"82.6043913,ダンス目つむり,頬０涙０",
			"83.1014298,ダンス目あけ,頬０涙０",
			"83.6315454,ダンス微笑み,頬０涙０",
			"84.1782615,ダンスウインク,頬０涙０",
			"84.9403394,ダンス真剣,頬０涙０",
			"85.5203575,ダンスウインク,頬１涙０",
			"86.0173418,ダンス目あけ,頬０涙０",
			"86.5143937,ダンス微笑み,頬０涙０",
			"87.7568521,ダンス目つむり,頬０涙０",
			"89.9602397,ダンス目あけ,頬０涙０",
			"92.2464358,ダンス真剣,頬０涙０",
			"92.925665,ダンス目あけ,頬０涙０",
			"93.8202772,ダンス微笑み,頬０涙０",
			"95.4272323,ダンス真剣,頬０涙０",
			"96.603522,ダンス目とじ,頬１涙０",
			"97.4152117,ダンス微笑み,頬０涙０",
			"98.3595464,ダンス目とじ,頬１涙０",
			"99.1712962,ダンス微笑み,頬０涙０",
			"101.1427694,ダンス目つむり,頬０涙０",
			"101.8385518,ダンス微笑み,頬０涙０",
			"103.0644409,ダンス目とじ,頬１涙０",
			"103.6774197,ダンス目あけ,頬０涙０",
			"104.390518,ダンス微笑み,頬０涙０",
			"107.0908515,ダンスウインク,頬０涙０",
			"107.85319,ダンス目あけ,頬０涙０",
			"108.598628,ダンス目とじ,頬１涙０",
			"110.9013902,ダンス目あけ,頬０涙０",
			"111.3984036,ダンスウインク,頬０涙０",
			"111.8953993,ダンス微笑み,頬０涙０",
			"112.4255391,ダンス目つむり,頬０涙０",
			"112.9557248,ダンス目あけ,頬０涙０",
			"113.634905,ダンス微笑み,頬０涙０",
			"114.4807964,ダンスウインク,頬０涙０",
			"115.6239309,ダンス目あけ,頬０涙０",
			"116.203752,ダンス微笑み,頬０涙０",
			"117.2639834,ダンス目つむり,頬０涙０",
			"117.8272732,ダンス目あけ,頬０涙０",
			"118.6224812,ダンス誘惑,頬０涙０",
			"119.4342395,ダンス微笑み,頬０涙０",
			"120.7761214,ダンス目とじ,頬１涙０",
			"121.5713243,ダンス目あけ,頬０涙０",
			"122.2671617,ダンス微笑み,頬０涙０",
			"124.4389083,ダンス目とじ,頬１涙０",
			"124.9855888,ダンス微笑み,頬０涙０",
			"126.1783707,ダンス目とじ,頬１涙０",
			"128.8787817,ダンス目あけ,頬０涙０",
			"130.0716366,ダンス微笑み,頬０涙０",
			"131.1152948,ダンス誘惑,頬０涙０",
			"132.5068272,ダンス微笑み,頬０涙０",
			"135.6380069,ダンス目つむり,頬０涙０",
			"136.3501589,ダンス目あけ,頬０涙０",
			"137.2118055,ダンス目とじ,頬１涙０",
			"138.8518956,ダンス目あけ,頬０涙０",
			"139.7630667,ダンス微笑み,頬０涙０",
			"140.9227413,ダンス目つむり,頬０涙０",
			"142.1484799,ダンス微笑み,頬０涙０"
		};

		private string[] dance6KArray = new string[]
		{
			"0.036175,ダンス微笑み,頬０涙０",
			"3.7300757,ダンス目つむり,頬０涙０",
			"4.5086424,ダンス目あけ,頬０涙０",
			"5.121861,ダンスウインク,頬０涙０",
			"6.5796808,ダンス目あけ,頬０涙０",
			"7.3418987,ダンス目とじ,頬１涙０",
			"8.3357805,ダンス目あけ,頬０涙０",
			"9.1473065,ダンス目とじ,頬１涙０",
			"9.9592909,ダンス目あけ,頬０涙０",
			"11.7153791,ダンス目とじ,頬１涙０",
			"12.2952409,ダンス目あけ,頬０涙０",
			"13.1761195,ダンスウインク,頬０涙０",
			"13.8387702,ダンス微笑み,頬０涙０",
			"15.3629026,ダンス真剣,頬０涙０",
			"15.8599018,ダンス微笑み,頬０涙０",
			"17.0029839,ダンス目とじ,頬１涙０",
			"18.7590473,ダンスウインク,頬０涙０",
			"19.9519153,ダンス目とじ,頬１涙０",
			"21.2273204,ダンス微笑み,頬０涙０",
			"23.266118,ダンスウインク,頬０涙０",
			"23.8119451,ダンス微笑み,頬０涙０",
			"24.723066,ダンス目とじ,頬１涙０",
			"26.8268841,ダンス目あけ,頬０涙０",
			"28.0860957,ダンス微笑み,頬０涙０",
			"28.997283,頬１涙０,頬０涙０",
			"31.3994054,ダンス微笑み,頬０涙０",
			"32.8083942,ダンスウインク,頬０涙０",
			"34.4983885,ダンス微笑み,頬０涙０",
			"35.1444748,ダンス目とじ,頬１涙０",
			"36.7846935,ダンス目あけ,頬０涙０",
			"37.8945346,ダンス目つむり,頬０涙０",
			"39.5015531,ダンス目あけ,頬０涙０",
			"40.263759,ダンス微笑み,頬０涙０",
			"40.7606647,ダンス目とじ,頬１涙０",
			"42.0557987,ダンス目あけ,頬０涙０",
			"43.9111719,ダンス目とじ,頬１涙０",
			"46.3464857,ダンス目あけ,頬１涙０",
			"48.1522452,ダンス微笑み,頬１涙０",
			"48.9807275,ダンスウインク,頬１涙０",
			"49.544368,ダンス微笑み,頬１涙０",
			"51.2847698,ダンス目つむり,頬１涙０",
			"51.7983644,ダンス目あけ,頬１涙０",
			"52.2955702,ダンスウインク,頬１涙０",
			"52.8420791,ダンス目あけ,頬１涙０",
			"54.3828055,ダンス目とじ,頬１涙０",
			"56.3871624,ダンス目あけ,頬０涙０",
			"57.8120563,ダンス目つむり,頬０涙０",
			"58.3088733,ダンス目あけ,頬０涙０",
			"59.186959,ダンス目つむり,頬０涙０",
			"60.2970881,ダンス微笑み,頬０涙０",
			"61.1086674,ダンス目とじ,頬１涙０",
			"61.7052458,ダンス目あけ,頬０涙０",
			"62.4010476,ダンス目とじ,頬１涙０",
			"63.0637844,ダンス微笑み,頬０涙０",
			"64.621036,ダンス目とじ,頬１涙０",
			"66.5758103,ダンス微笑み,頬０涙０",
			"68.5141728,ダンス目あけ,頬０涙０",
			"69.0111686,ダンス目とじ,頬１涙０",
			"69.8610453,ダンス目あけ,頬０涙０",
			"70.4740333,ダンス目つむり,頬０涙０",
			"71.6171752,ダンス微笑み,頬０涙０",
			"72.594544,ダンス目とじ,頬１涙０",
			"74.1186787,ダンス目あけ,頬０涙０",
			"75.8250269,ダンス微笑み,頬０涙０",
			"77.0012924,ダンス目とじ,頬０涙０",
			"77.7798773,ダンス目あけ,頬０涙０",
			"78.3098734,ダンス目とじ,頬１涙０",
			"78.8924856,ダンス微笑み,頬０涙０",
			"80.1018221,ダンス目あけ,頬０涙０",
			"81.0129553,ダンス目とじ,頬１涙０",
			"82.0567555,ダンス目あけ,頬０涙０",
			"83.2328941,ダンス微笑み,頬０涙０",
			"84.0447243,ダンス目とじ,頬１涙０",
			"84.6411139,ダンス目あけ,頬０涙０",
			"85.1381017,ダンスウインク,頬０涙０",
			"85.8668782,ダンス目あけ,頬０涙０",
			"86.6289165,ダンス微笑み,頬０涙０",
			"87.6397212,ダンス目つむり,頬０涙０",
			"89.130934,ダンス目あけ,頬０涙０",
			"89.6610695,ダンス目とじ,頬１涙０",
			"91.5331325,ダンス目あけ,頬０涙０",
			"92.9744391,ダンス目とじ,頬１涙０",
			"93.8360059,頬０涙０,頬１涙０",
			"95.0949703,ダンスウインク,頬１涙０",
			"95.7742078,ダンス目あけ,頬１涙０",
			"96.834478,ダンス目とじ,頬１涙０",
			"97.9465763,ダンス目あけ,頬０涙０",
			"99.1561289,ダンス目つむり,頬０涙０",
			"101.0280017,ダンス目あけ,頬０涙０",
			"101.6078097,ダンス目つむり,頬０涙０",
			"102.0882631,ダンス目あけ,頬０涙０",
			"102.6019702,ダンス目つむり,頬０涙０",
			"103.0989922,ダンス目あけ,頬０涙０",
			"103.728346,ダンス目とじ,頬１涙０",
			"106.4620237,ダンス微笑み,頬０涙０",
			"107.4405146,ダンス目とじ,頬１涙０",
			"108.03702,ダンス目あけ,頬０涙０",
			"109.0473016,ダンス目つむり,頬０涙０",
			"109.5444695,ダンス微笑み,頬０涙０",
			"110.3396607,ダンス目とじ,頬１涙０",
			"111.4662246,頬０涙０,頬１涙０",
			"112.2945519,ダンス目つむり,頬１涙０",
			"112.7915439,ダンス微笑み,頬１涙０",
			"113.3713775,頬１涙０,頬１涙０",
			"115.4587698,ダンス微笑み,頬０涙０",
			"116.8852332,ダンス目あけ,頬０涙０",
			"117.4981878,ダンス目とじ,頬１涙０",
			"118.8234318,ダンス目あけ,頬０涙０",
			"119.8507105,ダンス目つむり,頬０涙０",
			"120.3474533,ダンス微笑み,頬０涙０",
			"121.9711262,ダンス目つむり,頬０涙０",
			"122.8326132,ダンス目あけ,頬０涙０",
			"123.3294618,ダンス微笑み,頬０涙０",
			"124.7872962,ダンス目つむり,頬０涙０",
			"125.632328,ダンス目あけ,頬０涙０",
			"126.1291906,ダンスウインク,頬０涙０",
			"127.5209759,ダンス目とじ,頬１涙０"
		};

		private string[] dance9KArray = new string[]
		{
			"0.0251303,ダンス困り顔,頬２涙０",
			"1.9301919,ダンス困り顔,頬１涙０",
			"3.007089,ダンス目つむり,頬１涙０",
			"3.3218118,ダンス微笑み,頬１涙０",
			"5.0778952,ダンス目あけ,頬１涙０",
			"6.2871867,ダンスウインク,頬１涙０",
			"7.0996736,ダンス目とじ,頬１涙０",
			"10.6449037,ダンス微笑み,頬１涙０",
			"12.3346211,ダンス目とじ,頬１涙０",
			"13.4115518,ダンス目つむり,頬１涙０",
			"13.7594561,ダンス微笑み,頬１涙０",
			"14.5215538,ダンスウインク,頬１涙０",
			"16.559437,ダンス微笑み,頬１涙０",
			"17.9013182,ダンス目とじ,頬１涙０",
			"18.8456493,ダンス目とじ,頬０涙０",
			"20.4358368,ダンス微笑み,頬０涙０",
			"21.4797496,ダンス目つむり,頬０涙０",
			"21.6288348,ダンス微笑み,頬０涙０",
			"22.4075416,ダンス目とじ,頬０涙０",
			"23.8322042,ダンス微笑み,頬０涙０",
			"24.7102151,ダンス目つむり,頬０涙０",
			"24.7763459,ダンス微笑み,頬０涙０",
			"25.0084347,ダンス目つむり,頬０涙０",
			"25.1575478,ダンス微笑み,頬０涙０",
			"25.5908692,ダンスウインク,頬０涙０",
			"26.4356159,ダンス微笑み,頬０涙０",
			"27.1315037,ダンス目つむり,頬０涙０",
			"27.2142135,ダンス微笑み,頬０涙０",
			"27.9101564,ダンス目つむり,頬０涙０",
			"27.9929887,ダンス微笑み,頬０涙０",
			"29.74902,ダンス目つむり,頬０涙０",
			"29.8319145,ダンス微笑み,頬０涙０",
			"32.0683677,ダンス目とじ,頬０涙０",
			"33.1947017,ダンスウインク,頬０涙０",
			"34.0562058,ダンス微笑み,頬０涙０",
			"35.6966305,ダンス目あけ,頬０涙０",
			"36.9061941,ダンス目つむり,頬０涙０",
			"36.9890079,ダンス微笑み,頬０涙０",
			"37.5688901,ダンス目とじ,頬０涙０",
			"39.1426883,ダンス微笑み,頬０涙０",
			"40.6833693,ダンスウインク,頬０涙０",
			"41.2797778,ダンス微笑み,頬０涙０",
			"45.0238291,ダンス目とじ,頬０涙０",
			"46.945586,ダンス目とじ,頬１涙０",
			"48.1384266,ダンス微笑み,頬１涙０",
			"49.0992177,ダンス目つむり,頬１涙０",
			"49.182054,ダンス微笑み,頬１涙０",
			"51.2529109,ダンスウインク,頬１涙０",
			"52.2964574,ダンス微笑み,頬１涙０",
			"55.4615983,ダンスウインク,頬１涙０",
			"56.737186,ダンス微笑み,頬１涙０",
			"60.3154805,ダンス目とじ,頬１涙０",
			"61.7403643,ダンス目とじ,頬２涙０",
			"63.7780245,ダンス微笑み,頬２涙０",
			"64.8217549,ダンス微笑み,頬１涙０",
			"66.1471019,ダンスウインク,頬１涙０",
			"67.870052,ダンス微笑み,頬１涙０",
			"70.8851555,ダンス目とじ,頬１涙０",
			"72.3596319,ダンス微笑み,頬１涙０",
			"75.2608713,ダンス微笑み,頬０涙０",
			"75.4266004,ダンス目とじ,頬０涙０",
			"79.0050582,ダンス誘惑,頬０涙０",
			"80.5788442,ダンス目つむり,頬０涙０",
			"80.6616779,ダンス誘惑,頬０涙０",
			"82.6993367,ダンス目つむり,頬０涙０",
			"84.7205229,ダンス微笑み,頬０涙０",
			"87.7024854,ダンス微笑み,頬１涙０",
			"89.1272463,ダンス目とじ,頬１涙０",
			"91.3501805,ダンス目あけ,頬１涙０",
			"92.3441826,ダンス微笑み,頬１涙０",
			"96.9497428,ダンス目つむり,頬１涙０",
			"98.1093816,ダンス微笑み,頬１涙０",
			"100.1138192,ダンスウインク,頬１涙０",
			"101.1764996,ダンス目あけ,頬１涙０",
			"102.2700755,ダンス目つむり,頬１涙０",
			"102.3529349,ダンス微笑み,頬１涙０",
			"105.8815912,ダンス目とじ,頬１涙０",
			"106.4117229,ダンス目とじ,頬０涙０",
			"109.7913454,ダンス微笑み,頬０涙０",
			"111.9988145,ダンスウインク,頬０涙０",
			"113.7051323,ダンス微笑み,頬０涙０",
			"115.5771988,ダンス目とじ,頬０涙０",
			"115.7758256,ダンス目とじ,頬１涙０",
			"117.3497771,ダンスウインク,頬１涙０",
			"119.2052436,ダンス微笑み,頬１涙０",
			"122.7358893,ダンス目とじ,頬１涙０",
			"123.8460401,ダンス目とじ,頬０涙０",
			"125.7677489,ダンス微笑み,頬０涙０",
			"130.7378432,ダンス目つむり,頬０涙０",
			"132.4441127,ダンスウインク,頬０涙０",
			"133.7031645,ダンス微笑み,頬０涙０",
			"140.0847225,ダンス目つむり,頬０涙０",
			"142.4372033,ダンスウインク,頬０涙０",
			"143.6796818,ダンス目とじ,頬０涙０",
			"145.551709,ダンス目あけ,頬０涙０",
			"147.2911633,ダンス微笑み,頬０涙０",
			"150.4238392,ダンスウインク,頬０涙０",
			"151.5170965,ダンス目とじ,頬０涙０",
			"155.1783597,ダンス微笑み,頬０涙０",
			"158.2105417,ダンスウインク,頬０涙０",
			"158.7736249,ダンス微笑み,頬０涙０",
			"160.8280841,ダンス目つむり,頬０涙０",
			"160.9109019,ダンス微笑み,頬０涙０",
			"162.1037063,ダンス微笑み,頬１涙０",
			"162.7663898,ダンスウインク,頬１涙０",
			"164.3070714,ダンス目とじ,頬１涙０"
		};

		private List<string> strList2 = new List<string>();
		private List<string> strListE = new List<string>();
		private List<string> strListE2 = new List<string>();
		private List<string> strListS = new List<string>();
		private List<string> strListD = new List<string>();

		private string strS = "";

		private int countS = 0;

		private ArrayList groupList;

		private string[] poseArray;

		private string[] poseArray2 = new string[]
		{
			"pose_taiki_f",
			"pose_01_f",
			"pose_02_f",
			"pose_03_f",
			"pose_04_f",
			"pose_ero_01_loop_f",
			"pose_ero_02_loop_f",
			"pose_ero_03_loop_f",
			"pose_ero_04_loop_f",
			"pose_ero_05_loop_f",
			"pose_ero_06_loop_f",
			"pose_kakkoii_01_loop_f",
			"pose_kakkoii_02_loop_f",
			"pose_kakkoii_03_loop_f",
			"pose_kakkoii_04_loop_f",
			"pose_kakkoii_05_loop_f",
			"pose_kakkoii_06_loop_f",
			"pose_kawaii_01_loop_f",
			"pose_kawaii_02_loop_f",
			"pose_kawaii_03_loop_f",
			"pose_kawaii_04_loop_f",
			"pose_kawaii_05_loop_f",
			"pose_kawaii_06_loop_f",
			"edit_pose21_001_f",
			"edit_pose21_002_f",
			"edit_pose21_003_f",
			"edit_pose21_mune_taiki_f",
			"edit_pose21_mune_tate_f_once_",
			"edit_pose21_mune_yoko_f_once_",
			"maid_dressroom01",
			"kaiwa_tati_hutuu1_taiki_f",
			"maid_dressroom02",
			"poseizi_taiki_f",
			"maid_dressroom03",
			"kaiwa_tati_yorokobub_taiki_f",
			"maid_stand02akireloop",
			"stand_madogiwa",
			"kaiwa_tati_akireb_taiki_f",
			"kaiwa_tati_udekumu_taiki_f",
			"maid_stand02listenloop",
			"sys_munehide",
			"poseizi2_kakusu_taiki_f",
			"maid_stand03_base",
			"maid_stand02",
			"maid_stand02tere",
			"kaiwa_tati_ubiawase_taiki_f",
			"kaiwa_tati_teawase_taiki_f",
			"kaiwa_tati_hakusyu_taiki_2_f",
			"maid_view1",
			"maid_comehome2_loop_",
			"kaiwa_tati_hohokaki_taiki_f",
			"kaiwa_tati_tere_taiki_f",
			"sys_muneporo",
			"kaiwa_tati_yorokobua_taiki_f",
			"kaiwa_tati_odoroku_taiki_f",
			"kaiwa_tati_tutorial_1_taiki_f",
			"maid_stand05",
			"kaiwa_tati_tutorial_2_taiki_f",
			"stand_annai",
			"kaiwa_tati_teofuru_taiki_f",
			"kaiwa_tati_munetataku_taiki_f",
			"kaiwa_tati_yubisasu_taiki_f",
			"kaiwa_tati_iya_taiki_f",
			"hinpyoukai_gattu_taiki_f",
			"hinpyoukai_tewatasi_taiki_f",
			"kaiwa_tati_yorokobu_taiki_f",
			"kaiwa_tati_akire_taiki_f",
			"momi_momi_f",
			"soji_hakisouji",
			"soji_hataki",
			"soji_syokkiarai",
			"work_ryouri_nabe_mazeru",
			"work_sentakuhosu",
			"soji_mop_itazurago",
			"kaiwa_tati_hirumi_taiki_f",
			"tennis_kamae_f",
			"kaiwa_tati_ayamaru_taiki_f",
			"kaiwa_tati_syazai_taiki_f",
			"stand_akire",
			"kaiwa_tati_ibalu_taiki_f",
			"work_kaimono_itazurago",
			"kaiwa_tati_kuyasi_taiki_2_f",
			"soji_houki_itazurago",
			"work_ryouri_houtyou",
			"work_demukae_itazurago",
			"kaiwa_tati_mo_taiki_f",
			"poseizi2_zeccyougo_f",
			"poseizi_zeccyougo_f",
			"fukisouji1",
			"fukisouji1_vibe",
			"soji_tubo",
			"soji_mop",
			"soji_mop_vibe",
			"work_mizuyari_itazurago",
			"work_sentakuhosu_itazurago",
			"soji_hataki_itazurago",
			"fukisouji1_itazurago",
			"maidcho_oha1",
			"senakanagasi_f",
			"paizuri_taiki_f",
			"paizuri_fera_shaseigo_f",
			"osuwariaibu1",
			"self_ir_kansou_f",
			"inu_pose_f",
			"rosyutu_hounyou_taiki_f",
			"work_hansei",
			"sex_osuwari_taiki",
			"item_candy0_osuwari",
			"rosyutu_pose06_f",
			"sit_bed1",
			"hanyou_dogeza_taiki_f",
			"hanyou_dogeza_aisatu_f",
			"hanyou_dogeza_f",
			"inu_taiki_f",
			"soji_zoukin",
			"soji_zoukin_itazurago",
			"massage_f",
			"mp_arai_taiki_f",
			"midasinami_esthe_f2",
			"midasinami_esthe_f",
			"syagami_pose_f",
			"hanyou_kizetu_f",
			"densyasuwari_taiki_f",
			"ocha_pose_taiki_f",
			"work_kaiwa",
			"work_kaiwa_itazurago",
			"work_hon",
			"work_hon_itazurago",
			"work_saihou",
			"work_sentaku_tatamu",
			"midasinami_kadou_f",
			"work_mimi_f",
			"work_mimi_itazurago_f",
			"sit_yasumi1",
			"sit_tukue",
			"sleep1",
			"kaiwa_sofa_utumuku_taiki_f",
			"kaiwa_sofa_teawasea_taiki2_f",
			"kaiwa_sofa_teawase_taiki_f",
			"kaiwa_sofa_hazukasii_taiki_f",
			"op_osyaku_taiki_f",
			"op_wine_taiki_f",
			"kaiwa_sofa_1_f",
			"kaiwa_sofa_noridasu_1_taiki_f",
			"kaiwa_sofa_noridasu_2_taiki_f",
			"kaiwa_sofa_kangaerua_taiki_f",
			"kaiwa_sofa_kangaerub_taiki_f",
			"kaiwa_sofa_kangaeru_taiki_f",
			"kaiwa_sofa_tere_taiki_f",
			"kaiwa_sofa_konwakua_taiki_f",
			"kaiwa_sofa_odoroki_taiki_f",
			"kaiwa_sofa_konwaku_2_taiki_f",
			"kaiwa_sofa_konwaku_taiki_f",
			"dance_cm3d2_001_f1,14.14",
			"dance_cm3d2_001_f1,18.72",
			"dance_cm3d2_001_f1,15.34",
			"dance_cm3d2_001_f1,35.20",
			"dance_cm3d2_001_f1,36.15",
			"dance_cm3d2_001_f1,74.72",
			"dance_cm3d2_001_f1,74.52",
			"dance_cm3d2_001_f1,74.13",
			"dance_cm3d2_001_f1,63.53",
			"dance_cm3d2_001_f1,64.41",
			"dance_cm3d2_001_f1,80.41",
			"dance_cm3d2_001_f1,80.62",
			"dance_cm3d2_001_f1,81.47",
			"dance_cm3d2_001_f1,68.36",
			"dance_cm3d2_001_f1,68.49",
			"dance_cm3d2_001_f1,70.25",
			"dance_cm3d2_001_f1,70.64",
			"dance_cm3d2_001_f1,71.36",
			"dance_cm3d2_001_f1,72.26",
			"dance_cm3d2_001_f1,72.45",
			"dance_cm3d2_001_f1,73.23",
			"dance_cm3d2_001_f1,82.98",
			"dance_cm3d2_001_f1,83.77",
			"dance_cm3d2_001_f1,86.05",
			"dance_cm3d2_001_f1,94.06",
			"dance_cm3d2_001_f1,94.52",
			"dance_cm3d2_001_f1,95.0",
			"dance_cm3d2_001_f1,60.32",
			"dance_cm3d2_001_f1,60.76",
			"dance_cm3d2_001_f1,61.36",
			"dance_cm3d2_001_f1,150.0",
			"dance_cm3d_001_f1,39.25",
			"dance_cm3d_001_f1,8.29",
			"dance_cm3d_001_f1,11.47",
			"dance_cm3d_001_f1,12.67",
			"dance_cm3d_001_f1,14.42",
			"dance_cm3d_001_f1,18.45",
			"dance_cm3d_001_f1,24.43",
			"dance_cm3d_001_f1,52.57",
			"dance_cm3d_001_f1,56.83",
			"dance_cm3d_001_f1,58.18",
			"dance_cm3d_001_f1,62.87",
			"dance_cm3d_001_f1,63.84",
			"dance_cm3d_001_f1,69.52",
			"dance_cm3d_001_f1,70.52",
			"dance_cm3d_001_f1,71.31",
			"dance_cm3d_001_f1,72.67",
			"dance_cm3d_001_f1,73.94",
			"dance_cm3d_001_f1,77.55",
			"dance_cm3d_001_f1,79.78",
			"dance_cm3d_001_f1,82.56",
			"dance_cm3d_001_f1,85.71",
			"dance_cm3d_001_f1,105.82",
			"dance_cm3d_001_f1,107.48",
			"dance_cm3d_001_f1,107.92",
			"dance_cm3d_001_f1,115.03",
			"dance_cm3d_001_f1,116.54"
		};

		private string[] poseArrayVP2 = new string[]
		{
			"dance_cm3d_002_end_f1,50.71",
			"dance_cm3d_002_end_f1,53.04",
			"dance_cm3d_002_end_f1,102.88",
			"dance_cm3d_002_end_f1,75.18",
			"dance_cm3d_002_end_f1,79.34",
			"dance_cm3d_002_end_f1,97.01",
			"dance_cm3d_002_end_f1,89.85",
			"dance_cm3d_002_end_f1,26.74",
			"dance_cm3d_002_end_f1,100.30",
			"dance_cm3d_002_end_f1,101.38",
			"dance_cm3d_002_end_f1,124.85",
			"dance_cm3d_002_end_f1,35.40",
			"dance_cm3d_002_end_f1,107.98",
			"dance_cm3d_002_end_f1,106.71",
			"dance_cm3d_002_end_f1,36.51",
			"dance_cm3d_002_end_f1,47.54",
			"dance_cm3d_002_end_f1,118.35",
			"dance_cm3d_002_end_f1,43.37",
			"dance_cm3d_002_end_f1,31.22",
			"dance_cm3d_002_end_f1,90.71",
			"dance_cm3d_002_end_f1,25.78",
			"dance_cm3d_002_end_f1,24.85",
			"dance_cm3d_002_end_f1,29.21",
			"dance_cm3d_002_end_f1,29.53",
			"dance_cm3d_002_end_f1,29.72",
			"dance_cm3d_002_end_f1,128.61",
			"dance_cm3d_002_end_f1,133.56",
			"dance_cm3d_002_end_f1,138.26",
			"dance_cm3d_002_end_f1,63.84",
			"dance_cm3d_002_end_f1,170"
		};

		private string[] poseArrayPP = new string[]
		{
			"dance_cm3d2_002_smt_f,7.76,",
			"dance_cm3d2_002_smt_f,8.73,",
			"dance_cm3d2_002_smt_f,10.26,",
			"dance_cm3d2_002_smt_f,11.78,",
			"dance_cm3d2_002_smt_f,28.56,",
			"dance_cm3d2_002_smt_f,29.07,",
			"dance_cm3d2_002_smt_f,29.54,",
			"dance_cm3d2_002_smt_f,29.98,",
			"dance_cm3d2_002_smt_f,30.49,",
			"dance_cm3d2_002_smt_f,51.11,",
			"dance_cm3d2_002_smt_f,49.22,",
			"dance_cm3d2_002_smt_f,135.30,",
			"dance_cm3d2_002_smt_f,133.17,",
			"dance_cm3d2_002_smt_f,19.51,",
			"dance_cm3d2_002_smt_f,44.27,",
			"dance_cm3d2_002_smt_f,17.82,",
			"dance_cm3d2_002_smt_f,18.25,",
			"dance_cm3d2_002_smt_f,31.94,",
			"dance_cm3d2_002_smt_f,33.14,",
			"dance_cm3d2_002_smt_f,87.56,",
			"dance_cm3d2_002_smt_f,86.23,",
			"dance_cm3d2_002_smt_f,135.79,",
			"dance_cm3d2_002_smt_f,79.31,",
			"dance_cm3d2_002_smt_f,120.31,",
			"dance_cm3d2_002_smt_f,21.65,",
			"dance_cm3d2_002_smt_f,43.17,",
			"dance_cm3d2_002_smt_f,43.53,",
			"dance_cm3d2_002_smt_f,25.47,",
			"dance_cm3d2_002_smt_f,34.71,",
			"dance_cm3d2_002_smt_f,35.96,",
			"dance_cm3d2_002_smt_f,36.21,",
			"dance_cm3d2_002_smt_f,36.90,",
			"dance_cm3d2_002_smt_f,45.50,",
			"dance_cm3d2_002_smt_f,45.66,",
			"dance_cm3d2_002_smt_f,46.32,",
			"dance_cm3d2_002_smt_f,46.80,",
			"dance_cm3d2_002_smt_f,48.29,",
			"dance_cm3d2_002_smt_f,58.42,",
			"dance_cm3d2_002_smt_f,59.73,",
			"dance_cm3d2_002_smt_f,104.45,",
			"dance_cm3d2_002_smt_f,117.89,",
			"dance_cm3d2_002_smt_f,106.94,",
			"dance_cm3d2_002_smt_f,107.76,",
			"dance_cm3d2_002_smt_f,111.89,",
			"dance_cm3d2_002_smt_f,113.68,",
			"dance_cm3d2_002_smt_f,117.04,",
			"dance_cm3d2_002_smt_f,121.93,",
			"dance_cm3d2_002_smt_f,163.22,",
			"dance_cm3d2_002_smt_f,129.73,",
			"dance_cm3d2_002_smt_f,143.45,"
		};

		private string[] poseArrayFB = new string[]
		{
			"dance_cm3d_003_sp2_f1,90.15",
			"dance_cm3d_003_sp2_f1,102.35",
			"dance_cm3d_003_sp2_f1,66.56",
			"dance_cm3d_003_sp2_f1,103.36",
			"dance_cm3d_003_sp2_f1,103.86",
			"dance_cm3d_003_sp2_f1,105.19",
			"dance_cm3d_003_sp2_f1,100.05",
			"dance_cm3d_003_sp2_f1,99.55",
			"dance_cm3d_003_sp2_f1,19.54",
			"dance_cm3d_003_sp2_f1,21.34",
			"dance_cm3d_003_sp2_f1,11.84",
			"dance_cm3d_003_sp2_f1,14.69",
			"dance_cm3d_003_sp2_f1,24.44",
			"dance_cm3d_003_sp2_f1,32.47",
			"dance_cm3d_003_sp2_f1,47.97",
			"dance_cm3d_003_sp2_f1,48.38",
			"dance_cm3d_003_sp2_f1,51.32",
			"dance_cm3d_003_sp2_f1,56.47",
			"dance_cm3d_003_sp2_f1,61.64",
			"dance_cm3d_003_sp2_f1,68.00",
			"dance_cm3d_003_sp2_f1,69.35",
			"dance_cm3d_003_sp2_f1,69.80",
			"dance_cm3d_003_sp2_f1,72.68",
			"dance_cm3d_003_sp2_f1,77.29",
			"dance_cm3d_003_sp2_f1,82.81",
			"dance_cm3d_003_sp2_f1,83.98",
			"dance_cm3d_003_sp2_f1,92.09",
			"dance_cm3d_003_sp2_f1,101.40",
			"dance_cm3d_003_sp2_f1,104.48",
			"dance_cm3d_003_sp2_f1,106.61",
			"dance_cm3d_003_sp2_f1,106.78",
			"dance_cm3d_003_sp2_f1,108.43",
			"dance_cm3d_003_sp2_f1,109.41",
			"dance_cm3d_003_sp2_f1,111.23",
			"dance_cm3d_003_sp2_f1,112.67",
			"dance_cm3d_003_sp2_f1,112.89",
			"dance_cm3d_003_sp2_f1,114.03",
			"dance_cm3d_003_sp2_f1,115.61"
		};

		private string[] poseArrayKT = new string[]
		{
			"dance_cm3d_004_kano_f1,124.93",
			"dance_cm3d_004_kano_f1,125.34",
			"dance_cm3d_004_kano_f1,10.86",
			"dance_cm3d_004_kano_f1,11.94",
			"dance_cm3d_004_kano_f1,15.55",
			"dance_cm3d_004_kano_f1,165.36",
			"dance_cm3d_004_kano_f1,16.31",
			"dance_cm3d_004_kano_f1,16.54",
			"dance_cm3d_004_kano_f1,17.0",
			"dance_cm3d_004_kano_f1,31.19,",
			"dance_cm3d_004_kano_f1,125.99",
			"dance_cm3d_004_kano_f1,54.01",
			"dance_cm3d_004_kano_f1,54.76",
			"dance_cm3d_004_kano_f1,117.2",
			"dance_cm3d_004_kano_f1,117.83",
			"dance_cm3d_004_kano_f1,118.64",
			"dance_cm3d_004_kano_f1,167.22",
			"dance_cm3d_004_kano_f1,70.38",
			"dance_cm3d_004_kano_f1,70.95",
			"dance_cm3d_004_kano_f1,71.4",
			"dance_cm3d_004_kano_f1,71.9",
			"dance_cm3d_004_kano_f1,17.8",
			"dance_cm3d_004_kano_f1,21.74",
			"dance_cm3d_004_kano_f1,25.18",
			"dance_cm3d_004_kano_f1,27.92",
			"dance_cm3d_004_kano_f1,28.36",
			"dance_cm3d_004_kano_f1,28.83",
			"dance_cm3d_004_kano_f1,29.24,",
			"dance_cm3d_004_kano_f1,31.87,",
			"dance_cm3d_004_kano_f1,47.14",
			"dance_cm3d_004_kano_f1,39.18,",
			"dance_cm3d_004_kano_f1,41.12,",
			"dance_cm3d_004_kano_f1,44.02",
			"dance_cm3d_004_kano_f1,46.09",
			"dance_cm3d_004_kano_f1,46.71",
			"dance_cm3d_004_kano_f1,55.85",
			"dance_cm3d_004_kano_f1,57.19",
			"dance_cm3d_004_kano_f1,59.61",
			"dance_cm3d_004_kano_f1,74.4,",
			"dance_cm3d_004_kano_f1,75.68,",
			"dance_cm3d_004_kano_f1,76.1,",
			"dance_cm3d_004_kano_f1,26.49",
			"dance_cm3d_004_kano_f1,35.38,",
			"dance_cm3d_004_kano_f1,53.04",
			"dance_cm3d_004_kano_f1,35.86,",
			"dance_cm3d_004_kano_f1,64.7",
			"dance_cm3d_004_kano_f1,77.24,",
			"dance_cm3d_004_kano_f1,77.59,",
			"dance_cm3d_004_kano_f1,78.15,",
			"dance_cm3d_004_kano_f1,80.34,",
			"dance_cm3d_004_kano_f1,82.57,",
			"dance_cm3d_004_kano_f1,90.18,",
			"dance_cm3d_004_kano_f1,95.55,",
			"dance_cm3d_004_kano_f1,104.05",
			"dance_cm3d_004_kano_f1,106.37",
			"dance_cm3d_004_kano_f1,110.24,",
			"dance_cm3d_004_kano_f1,111.29,",
			"dance_cm3d_004_kano_f1,112.24,",
			"dance_cm3d_004_kano_f1,137.23",
			"dance_cm3d_004_kano_f1,170.07"
		};

		private string[] poseArrayPP2 = new string[]
		{
			"dance_cm3d2_003_hs_f1,0.01,",
			"dance_cm3d2_003_hs_f1,8.24",
			"dance_cm3d2_003_hs_f1,14.9",
			"dance_cm3d2_003_hs_f1,20.06",
			"dance_cm3d2_003_hs_f1,32.69,",
			"dance_cm3d2_003_hs_f1,35.16,",
			"dance_cm3d2_003_hs_f1,40.36,",
			"dance_cm3d2_003_hs_f1,42.89,",
			"dance_cm3d2_003_hs_f1,49.03",
			"dance_cm3d2_003_hs_f1,48.12",
			"dance_cm3d2_003_hs_f1,52.22",
			"dance_cm3d2_003_hs_f1,53.44,",
			"dance_cm3d2_003_hs_f1,59.0",
			"dance_cm3d2_003_hs_f1,61.66",
			"dance_cm3d2_003_hs_f1,62.1",
			"dance_cm3d2_003_hs_f1,65.11",
			"dance_cm3d2_003_hs_f1,69.596",
			"dance_cm3d2_003_hs_f1,73.68",
			"dance_cm3d2_003_hs_f1,28.51",
			"dance_cm3d2_003_hs_f1,70.67",
			"dance_cm3d2_003_hs_f1,75.91",
			"dance_cm3d2_003_hs_f1,81.57",
			"dance_cm3d2_003_hs_f1,82.07",
			"dance_cm3d2_003_hs_f1,82.6",
			"dance_cm3d2_003_hs_f1,82.97",
			"dance_cm3d2_003_hs_f1,88.82",
			"dance_cm3d2_003_hs_f1,89.4",
			"dance_cm3d2_003_hs_f1,91.41",
			"dance_cm3d2_003_hs_f1,96.81",
			"dance_cm3d2_003_hs_f1,93.88",
			"dance_cm3d2_003_hs_f1,99.19,",
			"dance_cm3d2_003_hs_f1,100.06,",
			"dance_cm3d2_003_hs_f1,102.81,",
			"dance_cm3d2_003_hs_f1,103.27,",
			"dance_cm3d2_003_hs_f1,104.12,",
			"dance_cm3d2_003_hs_f1,122.44",
			"dance_cm3d2_003_hs_f1,122.9",
			"dance_cm3d2_003_hs_f1,123.3",
			"dance_cm3d2_003_hs_f1,123.73",
			"dance_cm3d2_003_hs_f1,126.03",
			"dance_cm3d2_003_hs_f2,45.07,",
			"dance_cm3d2_003_hs_f2,49.91,",
			"dance_cm3d2_003_hs_f2,48.89,",
			"dance_cm3d2_003_hs_f2,46.53,",
			"dance_cm3d2_003_hs_f2,52.38,",
			"dance_cm3d2_003_hs_f2,55.14,",
			"dance_cm3d2_003_hs_f2,57.16,",
			"dance_cm3d2_003_hs_f2,57.39,",
			"dance_cm3d2_003_hs_f3,4.86,",
			"dance_cm3d2_003_hs_f4,46.65,",
			"dance_cm3d2_003_hs_f3,92.15,",
			"dance_cm3d2_003_hs_f3,93.03,",
			"dance_cm3d2_003_hs_f4,64.93,",
			"dance_cm3d2_003_hs_f4,65.27,",
			"dance_cm3d2_003_hs_f4,95.77,",
			"dance_cm3d2_003_hs_f4,96.76,",
			"dance_cm3d2_003_hs_f3,42.5,",
			"dance_cm3d2_003_hs_f4,126.4,",
			"dance_cm3d2_003_hs_f3,77.69,",
			"dance_cm3d2_003_hs_f4,77.7,",
			"dance_cm3d2_003_hs_f3,106.32,",
			"dance_cm3d2_003_hs_f4,106.49,",
			"dance_cm3d2_003_hs_f1,106.53,",
			"dance_cm3d2_003_hs_f4,101.73,",
			"dance_cm3d2_003_hs_f4,102.2,",
			"dance_cm3d2_003_hs_f3,107.09,",
			"dance_cm3d2_003_hs_f4,107.09,",
			"dance_cm3d2_003_hs_f3,40.46,",
			"dance_cm3d2_003_hs_f4,40.46,",
			"dance_cm3d2_003_hs_f3,88.22,",
			"dance_cm3d2_003_hs_f4,88.22,"
		};

		private string[] poseArrayPP3 = new string[]
		{
			"dance_cm3d2_004_sse_f1,0.01",
			"dance_cm3d2_004_sse_f1,12.42",
			"dance_cm3d2_004_sse_f1,14.14",
			"dance_cm3d2_004_sse_f1,15.13",
			"dance_cm3d2_004_sse_f1,16.2,",
			"dance_cm3d2_004_sse_f1,17.16,",
			"dance_cm3d2_004_sse_f1,17.69,",
			"dance_cm3d2_004_sse_f1,18.06,",
			"dance_cm3d2_004_sse_f1,21.29,",
			"dance_cm3d2_004_sse_f1,21.85",
			"dance_cm3d2_004_sse_f1,22.34,",
			"dance_cm3d2_004_sse_f1,23.25,",
			"dance_cm3d2_004_sse_f1,24.0,",
			"dance_cm3d2_004_sse_f1,24.39,",
			"dance_cm3d2_004_sse_f1,26.59,",
			"dance_cm3d2_004_sse_f1,29.61,",
			"dance_cm3d2_004_sse_f1,31.87,",
			"dance_cm3d2_004_sse_f1,33.11,",
			"dance_cm3d2_004_sse_f1,33.78,",
			"dance_cm3d2_004_sse_f1,34.53,",
			"dance_cm3d2_004_sse_f1,34.86,",
			"dance_cm3d2_004_sse_f1,35.32,",
			"dance_cm3d2_004_sse_f1,41.25,",
			"dance_cm3d2_004_sse_f1,42.1,",
			"dance_cm3d2_004_sse_f1,47.84,",
			"dance_cm3d2_004_sse_f1,51.94,",
			"dance_cm3d2_004_sse_f1,58.69,",
			"dance_cm3d2_004_sse_f1,67.92,",
			"dance_cm3d2_004_sse_f1,100.93,",
			"dance_cm3d2_004_sse_f1,102.05,",
			"dance_cm3d2_004_sse_f1,107.18",
			"dance_cm3d2_004_sse_f1,107.49",
			"dance_cm3d2_004_sse_f1,107.78",
			"dance_cm3d2_004_sse_f1,108.2",
			"dance_cm3d2_004_sse_f1,108.6",
			"dance_cm3d2_004_sse_f1,111.03",
			"dance_cm3d2_004_sse_f1,113.53",
			"dance_cm3d2_004_sse_f1,115.57",
			"dance_cm3d2_004_sse_f1,116.81",
			"dance_cm3d2_004_sse_f1,117.34",
			"dance_cm3d2_004_sse_f1,125.13",
			"dance_cm3d2_004_sse_f1,142.81,",
			"dance_cm3d2_004_sse_f1,148.03",
			"dance_cm3d2_004_sse_f1,149.68,",
			"dance_cm3d2_004_sse_f1,151.08,",
			"dance_cm3d2_004_sse_f1,154.81,",
			"dance_cm3d2_004_sse_f1,158.35",
			"dance_cm3d2_004_sse_f2,1.0,",
			"dance_cm3d2_004_sse_f2,14.02,",
			"dance_cm3d2_004_sse_f2,15.05,",
			"dance_cm3d2_004_sse_f2,17.13,",
			"dance_cm3d2_004_sse_f2,24.88,",
			"dance_cm3d2_004_sse_f2,26.51,",
			"dance_cm3d2_004_sse_f2,32.0,",
			"dance_cm3d2_004_sse_f2,41.25,",
			"dance_cm3d2_004_sse_f2,42.01,",
			"dance_cm3d2_004_sse_f2,52.34,",
			"dance_cm3d2_004_sse_f2,83.69,",
			"dance_cm3d2_004_sse_f2,85.94,",
			"dance_cm3d2_004_sse_f2,113.64,",
			"dance_cm3d2_004_sse_f2,123.52,",
			"dance_cm3d2_004_sse_f2,158.55,"
		};

		private string[] poseArray4 = new string[]
		{
			"turusi_sex_in_taiki_f",
			"turusi_sex_shaseigo_naka_f",
			"turusi_sex_shaseigo_soto_f",
			"mokuba_sissin_f",
			"poseizi2_taiki_f",
			"hentai_pose_03_f",
			"osuwariaibu2",
			"rosyutu_pose03_f",
			"kousoku_aibu_hibu_sissin_taiki_f",
			"rosyutu_pose01_f",
			"rosyutu_pose02_f",
			"rosyutu_pose04_f",
			"rosyutu_pose05_f",
			"rosyutu_taiki_omocya_f",
			"rosyutu_omocya_taiki_f",
			"rosyutu_omocya_zeccyougo_f",
			"rosyutu_tati_vibe_onani_zeccyougo_f",
			"ran3p_housi_taiki_f",
			"ran3p_housi_shaseigo_f",
			"manguri_in_taiki_f",
			"manguri_shaseigo_naka_f",
			"manguri_taiki_f",
			"manguri_shaseigo_soto_f",
			"ran3p_seijyoui_kuti_shaseigo_soto_f",
			"ran3p_seijyoui_kuti_sissin_taiki_f",
			"nefera_shasei_kuti_nomi02_b_f",
			"ran3p_2ana_in_taiki_f",
			"haimenrituia_zikkyou_1_f",
			"rosyutu_aruki_f_once_,1.37",
			"rosyutu_aruki_f_once_,2.35",
			"rosyutu_aruki_omocya_f,1.4",
			"rosyutu_aruki_omocya_f,2.72",
			"rosyutu_omocya_aruki_f_once_,1.54",
			"rosyutu_omocya_aruki_f_once_,2.33",
			"stand_desk1",
			"soji_tukue",
			"soji_tukuefuki_salon"
		};

		private string[] poseArray5 = new string[]
		{
			"dance_mc_001_p01a_f1_once_",
			"dance_mc_001_p01a_f2_once_",
			"dance_mc_001_p01a_f3_once_",
			"dance_mc_001_p01b_f1_once_",
			"dance_mc_001_p01b_f2_once_",
			"dance_mc_001_p01b_f3_once_",
			"dance_mc_001_p02_f1_once_",
			"dance_mc_001_p02_f2_once_",
			"dance_mc_001_p02_f3_once_",
			"dance_mc_001_p03_good_f1_once_",
			"dance_mc_001_p03_good_f2_once_",
			"dance_mc_001_p03_good_f3_once_",
			"dance_mc_001_p04_bad_f1_once_",
			"dance_mc_001_p04_bad_f2_once_",
			"dance_mc_001_p04_bad_f3_once_",
			"dance_mc_001_p05_f1_once_",
			"dance_mc_001_p05_f2_once_",
			"dance_mc_001_p05_f3_once_",
			"dance_mc_002_p01_f1_once_",
			"dance_mc_002_p01_f2_once_",
			"dance_mc_002_p01_f3_once_",
			"dance_mc_002_p02_f1_once_",
			"dance_mc_002_p02_f2_once_",
			"dance_mc_002_p02_f3_once_",
			"dance_mc_002_p03_good_f1_once_",
			"dance_mc_002_p03_good_f2_once_",
			"dance_mc_002_p03_good_f3_once_",
			"dance_mc_002_p04_bad_f1_once_",
			"dance_mc_002_p04_bad_f2_once_",
			"dance_mc_002_p04_bad_f3_once_",
			"dance_mc_002_p05_f1_once_",
			"dance_mc_002_p05_f2_once_",
			"dance_mc_002_p05_f3_once_",
			"dance_mc_003_p01_f1_once_",
			"dance_mc_003_p01_f2_once_",
			"dance_mc_003_p01_f3_once_",
			"dance_mc_003_p02_f1_once_",
			"dance_mc_003_p02_f2_once_",
			"dance_mc_003_p02_f3_once_",
			"dance_mc_003_p03_good_f1_once_",
			"dance_mc_003_p03_good_f2_once_",
			"dance_mc_003_p03_good_f3_once_",
			"dance_mc_003_p04_bad_f1_once_",
			"dance_mc_003_p04_bad_f2_once_",
			"dance_mc_003_p04_bad_f3_once_",
			"dance_mc_003_p05_f1_once_",
			"dance_mc_003_p05_f2_once_",
			"dance_mc_003_p05_f3_once_",
			"dance_mc_004_p01_f1_once_",
			"dance_mc_004_p01_f2_once_",
			"dance_mc_004_p01_f3_once_",
			"dance_mc_004_p02_f1_once_",
			"dance_mc_004_p02_f2_once_",
			"dance_mc_004_p02_f3_once_",
			"dance_mc_004_p03_good_f1_once_",
			"dance_mc_004_p03_good_f2_once_",
			"dance_mc_004_p03_good_f3_once_",
			"dance_mc_004_p04_bad_f1_once_",
			"dance_mc_004_p04_bad_f2_once_",
			"dance_mc_004_p04_bad_f3_once_",
			"dance_mc_004_p05_f1_once_",
			"dance_mc_004_p05_f2_once_",
			"dance_mc_004_p05_f3_once_",
			"dance_mc_005_p01_f1_once_",
			"dance_mc_005_p01_f2_once_",
			"dance_mc_005_p01_f3_once_",
			"dance_mc_005_p02_f1_once_",
			"dance_mc_005_p02_f2_once_",
			"dance_mc_005_p02_f3_once_",
			"dance_mc_005_p03_good_f1_once_",
			"dance_mc_005_p03_good_f2_once_",
			"dance_mc_005_p03_good_f3_once_",
			"dance_mc_005_p04_bad_f1_once_",
			"dance_mc_005_p04_bad_f2_once_",
			"dance_mc_005_p04_bad_f3_once_",
			"dance_mc_005_p05_f1_once_",
			"dance_mc_005_p05_f2_once_",
			"dance_mc_005_p05_f3_once_"
		};

		private string[] poseArray6 = new string[]
		{
			"dance_cm3d_001_f1",
			"dance_cm3d_002_end_f1",
			"dance_cm3d_003_sp2_f1",
			"dance_cm3d2_001_f1",
			"dance_cm3d2_001_f2",
			"dance_cm3d2_001_f3",
			"dance_cm3d21_001_nmf_f1",
			"dance_cm3d21_001_nmf_f2",
			"dance_cm3d21_001_nmf_f3",
			"dance_cm3d21_002_bid_f1",
			"dance_cm3d21_002_bid_f2",
			"dance_cm3d21_002_bid_f3",
			"dance_cm3d21_003_kad_f1",
			"dance_cm3d21_003_kad_f2",
			"dance_cm3d21_003_kad_f3",
			"dance_cm3d21_004_lm_f1",
			"dance_cm3d21_004_lm_f2",
			"dance_cm3d21_004_lm_f3",
			"dance_cm3d21_005_moe_f1",
			"dance_cm3d21_005_moe_f2",
			"dance_cm3d21_005_moe_f3"
		};

		private string[] faceArray = new string[]
		{
			"通常",
			"笑顔",
			"目を見開いて",
			"びっくり",
			"照れ叫び",
			"引きつり笑顔",
			"誘惑",
			"発情",
			"困った",
			"余韻弱",
			"思案伏せ目",
			"微笑み",
			"優しさ",
			"ダンス憂い",
			"照れ",
			"恥ずかしい",
			"怒り",
			"ジト目",
			"ダンスジト目",
			"ダンス誘惑",
			"ドヤ顔",
			"ダンスウインク",
			"あーん",
			"ためいき",
			"接吻",
			"苦笑い",
			"まぶたギュ",
			"にっこり",
			"ぷんすか",
			"ダンスキス",
			"目口閉じ",
			"居眠り安眠",
			"エロ絶頂",
			"エロ舌責快楽",
			"エロ舌責",
			"エロフェラ嫌悪",
			"エロ舐め愛情",
			"エロ痛み我慢３",
			"閉じ舐め愛情",
			"閉じフェラ愛情"
		};

		private string[] faceBlendArray = new string[]
		{
			"頬０涙０",
			"頬１涙０",
			"頬２涙０",
			"頬３涙０",
			"頬１涙１",
			"頬２涙１",
			"頬３涙１",
			"頬１涙２",
			"頬２涙２",
			"頬３涙２",
			"頬３涙３"
		};

		private string[] itemArray = new string[]
		{
			",",
			"handitem,HandItemR_SmartPhone_I_.menu",
			"handitem,HandItemR_Chusyaki_I_.menu",
			"handitem,HandItemR_Nei_Heartful_I_.menu",
			"handitem,HandItemR_Book_I_.menu",
			"handitem,HandItemR_Otama_I_.menu",
			"handitem,HandItemR_Houchou_I_.menu",
			"handitem,HandItemR_Usuba_Houchou_I_.menu",
			"handitem,HandItemR_Shaker_I_.menu",
			"kousoku_upper,KousokuU_TekaseOne_I_.menu",
			"kousoku_upper,KousokuU_TekaseTwo_I_.menu",
			"kousoku_lower,KousokuL_AshikaseUp_I_.menu",
			"kousoku_upper,KousokuU_TekaseTwo_I_.menu",
			"kousoku_lower,KousokuL_AshikaseDown_I_.menu",
			"kousoku_upper,KousokuU_TekaseTwoDown_I_.menu",
			"kousoku_upper,KousokuU_Ushirode_I_.menu",
			"kousoku_upper,KousokuU_SMRoom_Haritsuke_I_.menu",
			"handitem,HandItemL_Dance_Hataki_I_.menu",
			"handitem,HandItemL_Dance_Mop_I_.menu",
			"handitem,HandItemL_Karte_I_.menu",
			"handitem,HandItemL_Cracker_I_.menu",
			"handitem,HandItemL_Katuramuki_Daikon_I_.menu",
			"handitem,HandItemR_Etoile_Teacup_I_.menu",
			"handitem,HandItemL_Etoile_Saucer_I_.menu",
			"handitem,HandItemR_Etoile_Teacup_I_.menu",
			"handitem,HandItemR_Vibe_I_.menu",
			"handitem,HandItemR_VibePink_I_.menu",
			"handitem,HandItemR_VibeBig_I_.menu",
			"handitem,HandItemR_AnalVibe_I_.menu",
			"accvag,accVag_Vibe_I_.menu",
			"accvag,accVag_VibeBig_I_.menu",
			"accvag,accVag_VibePink_I_.menu",
			"handitem,HandItemH_SoutouVibe_I_.menu",
			"accanl,accAnl_AnalVibe_I_.menu"
		};

		private string[] itemBArray = new string[]
		{
			"handitem,HandItemR_SmartPhone_I_.menu",
			"handitem,HandItemL_Karte_I_.menu",
			"handitem,HandItemR_Chusyaki_I_.menu",
			"handitem,HandItemL_Cracker_I_.menu",
			"handitem,HandItemL_Karaoke_Mike_I_.menu",
			"handitem,HandItemR_Nei_Heartful_I_.menu",
			"handitem,HandItemR_Book_I_.menu",
			"handitem,HandItemL_Etoile_Saucer_I_.menu",
			"handitem,HandItemR_Etoile_Teacup_I_.menu",
			"handitem,HandItemR_Otama_I_.menu",
			"handitem,HandItemR_Houchou_I_.menu",
			"handitem,HandItemR_Usuba_Houchou_I_.menu",
			"handitem,HandItemL_Katuramuki_Daikon_I_.menu",
			"handitem,HandItemR_Shaker_I_.menu",
			"handitem,HandItemR_WineGlass_I_.menu",
			"handitem,HandItemR_WineBottle_I_.menu",
			"handitem,HandItemR_Mop_I_.menu",
			"handitem,HandItemR_Houki_I_.menu",
			"handitem,HandItemL_Dance_Hataki_I_.menu",
			"handitem,HandItemL_Dance_Mop_I_.menu",
			"kousoku_upper,KousokuU_SMRoom_Haritsuke_I_.menu",
			"handitem,HandItemR_Chu-B_Lip_I_.menu",
			"handitem,HandItemR_Vibe_I_.menu",
			"handitem,HandItemR_VibePink_I_.menu",
			"handitem,HandItemR_VibeBig_I_.menu",
			"handitem,HandItemR_AnalVibe_I_.menu",
			"handitem,HandItemH_SoutouVibe_I_.menu"
		};

		private string[] itemB2Array = new string[]
		{
			"handitem,HandItemR_Curry_I_.menu",
			"handitem,HandItemR_Pasta_I_.menu",
			"handitem,HandItemR_Omurice1_I_.menu",
			"handitem,HandItemR_Omurice2_I_.menu",
			"handitem,HandItemR_Omurice3_I_.menu",
			"handitem,HandItemR_Kushiyaki_I_.menu",
			"handitem,HandItemR_Tomorokoshi_I_.menu",
			"handitem,HandItemR_Tomorokoshi_yaki_I_.menu",
			"handitem,HandItemR_BeerBottle(cap_on)_I_.menu",
			"handitem,HandItemR_BeerBottle(cap_off)_I_.menu",
			"handitem,HandItemR_BeerGlass_I_.menu",
			"handitem,HandItemR_TropicalGlass_I_.menu",
			"handitem,HandItemR_MilkBottle(cap_on)_I_.menu",
			"handitem,HandItemR_MilkBottle(cap_off)_I_.menu",
			"handitem,HandItemR_Ochoko_I_.menu",
			"handitem,HandItemR_Spoon_Curry_I_.menu",
			"handitem,HandItemR_Spoon_Omurice_I_.menu",
			"handitem,HandItemR_Folk_I_.menu",
			"handitem,HandItemR_Mugcup_I_.menu",
			"handitem,HandItemR_Crops_Suika_I_.menu",
			"handitem,HandItemR_Suika_I_.menu",
			"handitem,HandItemR_Natumikan_I_.menu",
			"handitem,HandItemR_Ninjin_I_.menu",
			"handitem,HandItemR_Tomato_I_.menu",
			"handitem,HandItemR_Satumaimo_I_.menu",
			"handitem,HandItemL_Karaoke_Mike_I_.menu",
			"handitem,HandItemR_Hanabi_I_.menu",
			"handitem,HandItemR_Senkouhanabi_I_.menu",
			"handitem,HandItemR_Diary_I_.menu",
			"handitem,HandItemR_DVD1_I_.menu",
			"handitem,HandItemR_DVD2_I_.menu",
			"handitem,HandItemR_DVD3_I_.menu",
			"handitem,HandItemR_DVD4_I_.menu",
			"handitem,HandItemR_DVD5_I_.menu",
			"handitem,HandItemR_Jyouro_I_.menu",
			"handitem,HandItemR_Kobin_I_.menu",
			"handitem,HandItemR_Scoop_I_.menu",
			"handitem,HandItemR_Shell_I_.menu",
			"handitem,HandItemR_Shihen_I_.menu",
			"handitem,HandItemR_Uchiwa_I_.menu"
		};

		private string[] itemB3Array = new string[]
		{
			"handitem,HandItemR_Furaidopoteto_I_.menu",
			"handitem,HandItemR_Ketchup_I_.menu",
			"handitem,HandItemR_MelonSoda_I_.menu",
			"handitem,HandItemR_Spoon_Pafe_I_.menu"
		};

		private string[] itemB4Array = new string[]
		{
			"handitem,HandItemR_karaoke_maracas_I_.menu",
			"handitem,HandItemR_karaoke_sensu_I_.menu",
			"handitem,HandItemR_cocktail_red_I_.menu",
			"handitem,HandItemR_cocktail_blue_I_.menu",
			"handitem,HandItemR_cocktail_yellow_I_.menu",
			"handitem,HandItemR_pretzel_I_.menu",
			"handitem,HandItemR_smoothie_red_I_.menu",
			"handitem,HandItemR_smoothie_green_I_.menu"
		};

		private int[] myArray;

		private List<string[]> doguBArray = new List<string[]>();

		private string[] bgmArray;

		private string[] bgmArray2 = new string[]
		{
			"bgm008",
			"bgm001",
			"bgm002",
			"bgm003",
			"bgm004",
			"bgm005",
			"bgm006",
			"bgm007",
			"bgm009",
			"bgm010",
			"bgm011",
			"bgm012",
			"bgm013",
			"bgm014",
			"bgm015",
			"bgm016",
			"bgm017"
		};

		private string[] parArray;

		private string[] parArray1;

		private string[] parArray2 = new string[]
		{
			"mirror1",
			"mirror2",
			"mirror3",
			"Mob_Man_Stand001",
			"Mob_Man_Stand002",
			"Mob_Man_Stand003",
			"Mob_Man_Sit001",
			"Mob_Man_Sit002",
			"Mob_Man_Sit003",
			"Mob_Girl_Stand001",
			"Mob_Girl_Stand002",
			"Mob_Girl_Stand003",
			"Mob_Girl_Sit001",
			"Mob_Girl_Sit002",
			"Mob_Girl_Sit003",
			"Salon:65",
			"Salon:63",
			"Salon:69"
		};

		private string[] parArray3 = new string[]
		{
			"Particle/pLineY",
			"Particle/pLineP02",
			"Particle/pHeart01",
			"Particle/pLine_act2",
			"Particle/pstarY_act2"
		};

		private string[] doguB1Array = new string[]
		{
			"Odogu_XmasTreeMini_photo_ver",
			"Odogu_KadomatsuMini_photo_ver",
			"Odogu_VirginRoad_photo_ver",
			"Odogu_ClassRoomDesk_photo_ver",
			"Odogu_ClassRoomChair_photo_ver",
			"Odogu_Kitchen_photo_ver",
			"Odogu_TableFlower_photo_ver",
			"Odogu_Kadou_photo_ver",
			"Odogu_Dresser_photo_ver",
			"Odogu_KadouChair_photo_ver",
			"Odogu_DresserChair_photo_ver",
			"Odogu_MaidRoomChair_photo_ver",
			"Odogu_HeroineChair_muku",
			"Odogu_HeroineChair_mazime",
			"Odogu_HeroineChair_rindere",
			"Odogu_HeroineChair_tsumdere",
			"Odogu_HeroineChair_cooldere",
			"Odogu_HeroineChair_junshin",
			"photo_ver/Odogu_Etoile_Chair_photo_ver",
			"Odogu_LoveSofa",
			"Odogu_PublicToiletBenki_photo_ver",
			"Odogu_Sukebeisu_photo_ver",
			"Odogu_Mat_photo_ver",
			"Odogu_WineGlass_photo_ver",
			"Odogu_Manaita_photo_ver",
			"Odogu_Nabe_photo_ver",
			"Odogu_Sentaku_Kago_photo_ver",
			"Odogu_Sentaku_Towel_photo_ver",
			"Odogu_Sentakumono_photo_ver",
			"Odogu_SalonScreen_photo_ver",
			"Odogu_TrumpTowerSmall_photo_ver",
			"Odogu_TrumpTowerBig_photo_ver",
			"Odogu_VVLight_photo_ver",
			"Odogu_OXCamera_photo_ver",
			"Odogu_HandCameraVV_photo_ver",
			"Odogu_PC_photo_ver",
			"Odogu_PC_Monitor_photo_ver",
			"Odogu_PC_Keyboard_photo_ver",
			"Odogu_PC_Mouse_photo_ver",
			"Odogu_NoteBook_photo_ver",
			"Odogu_TabletPC",
			"Odogu_Seikaku_Tsundere_photo_ver",
			"Odogu_Seikaku_Jyunshin_photo_ver",
			"Odogu_Seikaku_Cool_photo_ver",
			"Megane001_z2_Scenario_Model",
			"PlayAreaOut",
			"DesktopScreen",
			"Odogu_PR_Table_photo_ver",
			"Odogu_PR_Table_Chuuka_photo_ver",
			"Odogu_ChuukaSet_chahan_photo_ver",
			"Odogu_ChuukaSet_gyouza_photo_ver",
			"Odogu_ChuukaSet_mabo_photo_ver",
			"Odogu_ChuukaSet_tea_photo_ver",
			"Odogu_PR_Table_Wasyoku_photo_ver",
			"Odogu_WasyokuSet_gohan_photo_ver",
			"Odogu_WasyokuSet_hashi_photo_ver",
			"Odogu_WasyokuSet_misoshiru_photo_ver",
			"Odogu_WasyokuSet_nimono_photo_ver",
			"Odogu_WasyokuSet_ocha_photo_ver",
			"Odogu_PR_Table_Yousyoku_photo_ver",
			"Odogu_YousyokuSet_ChickenRice_photo_ver",
			"Odogu_YousyokuSet_Coffee_photo_ver",
			"Odogu_YousyokuSet_CornSoup_photo_ver",
			"Odogu_YousyokuSet_Hamburg_photo_ver",
			"Odogu_YousyokuSet_SakiwareSpoon_photo_ver",
			"Odogu_LongDaiza_photo_ver",
			"Odogu_Omurice1",
			"Odogu_Omurice3",
			"Odogu_OmuriceH",
			"Odogu_OmuriceKao1",
			"Odogu_OmuriceKao2",
			"Odogu_OmuriceOppai",
			"Odogu_AcquaPazza",
			"Odogu_Sandwich",
			"Odogu_vichyssoise",
			"Odogu_BirthdayCake",
			"Odogu_Shortcake",
			"Odogu_MontBlanc",
			"Odogu_Pafe",
			"Odogu_Smoothie_Red",
			"Odogu_Smoothie_Green",
			"Odogu_Cocktail_Blue",
			"Odogu_Cocktail_Red",
			"Odogu_Cocktail_Yellow",
			"Odogu_Coffiecup",
			"Odogu_WineBottle(cap_off)",
			"Odogu_WineBottle(cap_on)",
			"Odogu_Jyouro",
			"Odogu_Planter_Red",
			"Odogu_Planter_Lightblue",
			"Odogu_MariGold"
		};

		private string[] doguB2Array = new string[]
		{
			"BGanimal_cat",
			"BGanimal_dog",
			"BGanimal_niwatori",
			"BGanimal_suzume",
			"BGOdogu_Game_Nei_USB",
			"BGodogu_bbqgrill",
			"BGodogu_bucket",
			"BGodogu_coolerbox",
			"BGodogu_game_darts",
			"BGodogu_game_dartsboard",
			"BGodogu_nabe_huta",
			"BGodogu_nabe_water",
			"BGodogu_natumikan",
			"BGodogu_rb_chair",
			"BGodogu_rb_duck",
			"BGodogu_rb_obon",
			"BGodogu_rb_tokkuri",
			"BGodogu_saracorn",
			"BGodogu_saraimo",
			"BGodogu_saratomato",
			"BGodogu_sunanoshiro",
			"BGodogu_sunanoyama",
			"BGodogu_tsutsuhanabi",
			"BGodogu_ukiwa",
			"BGOdogu_Game_Wanage",
			"BGOdogu_Game_Wa",
			"BGodogu_vf_crops_corn",
			"BGodogu_vf_crops_gekkabijin",
			"BGodogu_vf_crops_gekkabijinflower",
			"BGodogu_vf_crops_himawari",
			"BGodogu_vf_crops_natsumikan",
			"BGodogu_vf_crops_suika",
			"BGodogu_vf_crops_zakuro",
			"BGodogu_villa_table",
			"BGodogu_villa_tvrimocon",
			"BGodogu_villabr_sideboard"
		};

		private string[] doguB3Array = new string[]
		{
			"BGodogu_pafe",
			"BGodogu_furaidopoteto",
			"BGodogu_karaoketable",
			"BGodogu_omuriceh",
			"BGodogu_omuricekao1",
			"BGodogu_omuricekao2",
			"BGodogu_omuriceoppai"
		};

		private string[] doguB4Array = new string[]
		{
			"BGodogu_kakigori",
			"BGodogu_pretzel_sara",
			"BGodogu_karaoke_box"
		};

		private string[] doguArray = new string[]
		{
			"",
			"Odogu_SalonSofa_Dance",
			"Odogu_SalonSofa_4p",
			"Odogu_Girochin_A",
			"Odogu_Sankakumokuba",
			"Odogu_SMRoom2_SankakuMokuba",
			"Odogu_XmasTreeMini",
			"Odogu_KadomatsuMini",
			"nei",
			"Odogu_Kitchen",
			"Odogu_TableFlower",
			"Odogu_Kadou",
			"Odogu_Dresser",
			"Odogu_KadouChair",
			"Odogu_DresserChair",
			"Odogu_MaidRoomChair",
			"Odogu_PublicToiletBenki",
			"Odogu_Sukebeisu",
			"Odogu_Mat",
			"Odogu_Seikaku_Tsundere",
			"Odogu_Seikaku_Jyunshin",
			"Odogu_Seikaku_Cool",
			"Odogu_Manaita",
			"Odogu_Nabe",
			"Odogu_NoteBook",
			"Odogu_Sankousyo",
			"Odogu_Sentaku_Kago",
			"Odogu_Sentaku_Towel",
			"Odogu_Sentakumono",
			"Odogu_SalonScreen_photo_ver",
			"mirror1",
			"mirror2",
			"mirror3",
			"Mob_Man_Stand001",
			"Mob_Man_Stand002",
			"Mob_Man_Stand003",
			"Mob_Man_Sit001",
			"Mob_Man_Sit002",
			"Mob_Man_Sit003",
			"Mob_Girl_Stand001",
			"Mob_Girl_Stand002",
			"Mob_Girl_Stand003",
			"Mob_Girl_Sit001",
			"Mob_Girl_Sit002",
			"Mob_Girl_Sit003"
		};

		private readonly string[,] fingerLArray = new string[18, 20]
		{
				{
						"34.295,312.104,324.273", "15.394,25.241,350.373", "20.461,0.848,280.982", "0,0,0", "0.526,352.867,281.423",
						"353.396,1.051,264.680", "353.396,1.051,264.680", "0,0,0", "352.949,0.31,278.848", "353.035,0.725,264.714",
						"353.035,0.72,264.714", "0,0,0", "349.555,8.454,279.030", "352.552,0.263,264.766", "352.389,359.993,265.796",
						"0,0,0",
						"343.981,16.077,280.608", "352.468,0.199,264.775", "351.281,358.421,268.888", "0,0,0"
				},
				{
						"37.675,241.318,254.053", "0,0,323.093", "0,0,308.937", "0,0,0", "3.732,343.47,1.938", "0,0,359.531", "0,0,2",
						"0,0,0",
						"1.136,9.706,357.840", "0,0,2.621", "0,0,2", "0,0,0", "353.169,27.291,294.989", "0,0,294.766", "0,0,314", "0,0,0",
						"350.617,37.852,281.614", "0,0,312.219", "0,0,340", "0,0,0"
				},
				{
						"36.557,326.954,354.866", "359.802,2.553,3.498", "0,0,0", "0,0,0", "0.342,350.91,10.349", "0,0,0",
						"0.004,359.947,7.564",
						"0,0,0", "359.95,359.455,7.651", "0,0.005,0", "0.001,359.991,7.564", "0,0,0", "359.65,9.553,7.785", "0,359.986,0",
						"-0.006,0.068,8.567", "0,0,0", "359.449,17.351,11.740", "0,0,0", "359.972,0.211,11.454", "0,0,0"
				},
				{
						"61.475,252.161,261.300", "0,0,339.048", "0,0,298.872", "0,0,0", "2.375,352.711,12.400", "0,0,-0.001", "0,0,0",
						"0,0,0",
						"0.004,359.702,285.362", "0,0,275.016", "0,0,280.003", "0,0,0", "357.899,9.369,273.487", "0,0,277.364",
						"0,0,277.962",
						"0,0,0", "354.839,16.549,265.469", "0,0,270.384", "0,0,277.962", "0,0,0"
				},
				{
						"61.424,258.766,282.140", "0.000,0.000,4.093", "0.000,0.000,2.723", "0.000,0.000,0.000", "7.055,358.219,8.024",
						"0.000,0.000,1.723", "0.000,0.000,4.723", "0.000,0.000,0.000", "2.269,4.256,285.515", "0.000,0.000,269.965",
						"0.000,0.000,284.083", "0.000,0.000,0.000", "357.991,6.890,279.329", "0.000,0.000,278.202", "0.000,0.000,285.086",
						"0.000,0.000,0.000", "355.608,7.458,277.057", "0.000,0.000,271.083", "0.000,0.000,278.976", "0.000,0.000,0.000"
				},
				{
						"46.671,306.762,335.431", "0,0,14.093", "0,0,10", "0,0,0", "6.482,354.419,20.980", "0,0,3.031", "0,0,11.500",
						"0,0,0",
						"359.491,0.525,22.127", "0,0,6.121", "0,0,11.500", "0,0,0", "8.546,1.654,297.741", "358.341,355.307,274.490",
						"356.613,354.609,311.990", "0,0,0", "0.705,14.269,303.444", "7.907,358.29,276.091", "2.683,359.114,297.719", "0,0,0"
				},
				{
						"56.688,273.239,285.761", "358.735,354.929,346.552", "2.969,1.682,297.057", "0.000,0.000,0.000",
						"4.027,357.389,294.125",
						"0.000,0.000,298.175", "0.000,0.000,298.175", "0.000,0.000,0.000", "358.365,358.994,292.706", "0.000,0.000,293.293",
						"0.000,0.000,298.175", "0.000,0.000,0.000", "356.079,2.490,301.023", "0.000,0.000,295.903", "0.000,0.000,299.178",
						"0.000,0.000,0.000", "352.265,6.009,315.463", "0.975,0.743,301.842", "1.469,1.116,305.364", "0.000,0.000,0.000"
				},
				{
						"67.872,279.444,302.953", "0,0,337.359", "0,0,339.191", "0,0,0", "4.043,0.01,302.902", "0,0,297.998", "0,0,303.117",
						"0,0,0", "358.23,358.646,292.364", "0,0,282.720", "0,0,317.117", "0,0,0", "355.555,1.756,286.692", "0,0,284.720",
						"0,0,311.117", "0,0,0", "353.267,3.705,284.869", "0,0,286.720", "0,0,293.117", "0,0,0"
				},
				{
						"66.013,286.289,308.474", "0,0,340.986", "0,0,344", "0,0,0", "2.508,358.678,310.894", "0,0,311.770", "0,0,312",
						"0,0,0",
						"358.23,358.646,300.432", "0,0,294.047", "0,0,326", "0,0,0", "357.401,2.606,294.708", "0,0,296.047", "0,0,320",
						"0,0,0",
						"356.995,5.429,292.784", "0,0,298.047", "0,0,302", "0,0,0"
				},
				{
						"60.436,272.844,296.493", "0,0,348.834", "0,0,350.582", "0,0,0", "1.041,354.696,316.952", "0,0,314.200",
						"0,0,322.252",
						"0,0,0", "358.246,358.687,298.420", "0,0,292.046", "0,0,324.965", "0,0,0", "359.266,3.515,286.670", "0,0,288.096",
						"0,0,318.851", "0,0,0", "359.946,8.744,278.862", "0,0,284.377", "0,0,326.156", "0,0,0"
				},
				{
						"66.172,263.32,285.172", "0,0,344.846", "0,0,344.594", "0,0,0", "1.041,354.696,318.964", "0,0,320.212",
						"0,0,324.264",
						"0,0,0", "358.246,358.687,308.431", "0,0,306.058", "0,0,342.977", "0,0,0", "359.266,3.515,302.682", "0,0,308.107",
						"0,0,336.863", "0,0,0", "359.946,8.744,300.874", "0,0,308.389", "0,0,323.167", "0,0,0"
				},
				{
						"49.969,309.852,326.527", "0,0,350.986", "0,0,0", "0,0,0", "2.508,358.678,334.894", "0,0,331.770", "0,0,354",
						"0,0,0",
						"359.954,359.659,326.416", "0,0,332.047", "0,0,352", "0,0,0", "355.584,1.768,326.759", "0,0,332.047", "0,0,352",
						"0,0,0",
						"351.467,3.083,329.020", "0,0,332.047", "0,0,352", "0,0,0"
				},
				{
						"37.31,332.725,353.098", "359.806,12.141,356.455", "0.077,7.956,345.412", "0,0,0", "1.605,10.089,322.122", "0,0,0",
						"0.004,359.944,8.054", "0,0,0", "1.045,1.817,322.775", "0,0.005,0", "0.001,359.99,8.054", "0,0,0",
						"0.637,357.309,320.357",
						"0,359.986,0", "359.994,0.073,9.056", "0,0,0", "1.852,354.557,324.586", "0,0,0", "359.969,0.225,11.944", "0,0,0"
				},
				{
						"37.1,331.926,354.675", "0.189,19.371,0.981", "0.342,7.325,354.667", "0,0,0", "359.701,358.134,11.461", "0,0,0",
						"0,0,0",
						"0,0,0", "351.828,350.289,350.168", "0,0.005,0", "0,0,0", "0,0,0", "336.924,352.897,334.074",
						"358.62,355.86,341.288",
						"0,0,1.003", "0,0,0", "326.908,358.533,318.060", "359.929,359.758,327.356", "0,0,3.893", "0,0,0"
				},
				{
						"36.690,324.108,344.865", "0.000,0.000,7.370", "0.000,0.000,4.000", "0.000,0.000,0.000", "359.408,3.725,2.949",
						"0.000,0.000,0.000", "0.000,0.000,4.000", "0.000,0.000,0.000", "359.954,359.659,0.369", "0.000,0.000,3.881",
						"0.000,0.000,4.000", "0.000,0.000,0.000", "0.072,357.960,0.448", "0.000,0.000,2.118", "0.000,0.000,5.003",
						"0.000,0.000,0.000", "0.429,0.023,1.889", "0.000,0.000,0.000", "0.000,0.000,3.893", "0.000,0.000,0.000"
				},
				{
						"38.201,321.230,11.138", "0.000,0.000,334.109", "0.000,0.000,307.407", "0.000,0.000,0.000", "9.111,343.386,18.026",
						"0.000,0.000,313.897", "0.000,0.000,312.072", "0.000,0.000,0.000", "359.989,359.701,14.713", "0.000,0.000,313.823",
						"0.000,0.000,304.768", "0.000,0.000,0.000", "357.414,10.554,9.332", "0.000,0.000,316.359", "0.000,0.000,304.768",
						"0.000,0.000,0.000", "356.447,24.593,11.256", "0.000,0.000,308.726", "0.000,0.000,297.768", "0.000,0.000,0.000"
				},
				{
						"38.193,236.284,262.362", "0,0,349.370", "0,0,342", "0,0,0", "359.238,351.379,353.021", "0,0,340", "0,0,348",
						"0,0,0",
						"0.193,359.514,334.361", "0,0,293.881", "0,0,342", "0,0,0", "357.553,5.279,318.522", "0,0,324.118", "0,0,335.003",
						"0,0,0",
						"359.867,16.753,349.932", "0,0,344", "0,0,347.893", "0,0,0"
				},
				{
						"22.356,219.111,250.337", "0,0,18.961", "0,0,19.043", "0,0,0", "5.844,15.731,16.696", "0.388,359.495,283.827",
						"0.166,0.325,305.878", "0,0,0", "8.449,9.825,14.199", "359.15,359.151,277.258", "0.029,0.057,305.876", "0,0,0",
						"9.964,5.714,14.290", "358.109,358.766,283.998", "359.792,359.575,306.882", "0,0,0", "11.447,6.015,22.084",
						"357.993,358.756,299.347", "358.991,358.389,291.971", "0,0,0"
				}
		};
		private readonly string[,] fingerRArray  = new string[18, 20]
        {
                {
                        "325.705,47.897,324.273", "344.606,334.759,350.373", "339.538,359.153,280.982", "0,0,180", "359.474,7.133,281.423",
                        "6.604,358.95,264.680", "6.604,358.949,264.680", "0,0,180", "7.051,359.69,278.848", "6.965,359.275,264.714",
                        "6.965,359.28,264.714", "0,0,180", "10.445,351.546,279.030", "7.448,359.738,264.766", "7.611,0.007,265.796",
                        "0,0,180",
                        "16.019,343.923,280.608", "7.531,359.801,264.775", "8.719,1.579,268.888", "0,0,180"
                },
                {
                        "322.325,118.682,254.053", "0,0,323.093", "0,0,308.937", "0,0,180", "356.268,16.53,1.938", "0,0,359.531", "0,0,2",
                        "0,0,180", "358.864,350.294,357.840", "0,0,2.621", "0,0,2", "0,0,180", "6.831,332.709,294.989", "0,0,294.766",
                        "0,0,314",
                        "0,0,180", "9.383,322.148,281.614", "0,0,312.219", "0,0,340", "0,0,180"
                },
                {
                        "323.443,33.046,354.866", "0.198,357.447,3.498", "0,0,0", "0,0,180", "359.658,9.09,10.349", "0,0,0",
                        "-0.004,0.053,7.564",
                        "0,0,180", "0.05,0.546,7.651", "0,-0.005,0", "-0.001,0.009,7.564", "0,0,180", "0.35,350.447,7.785", "0,0.014,0",
                        "0.006,359.932,8.567", "0,0,180", "0.551,342.649,11.740", "0,0,0", "0.028,359.789,11.454", "0,0,180"
                },
                {
                        "298.525,107.839,261.300", "0,0,339.048", "0,0,298.872", "0,0,180", "357.625,7.289,12.400", "0,0,-0.001", "0,0,0",
                        "0,0,180", "-0.004,0.298,285.362", "0,0,275.016", "0,0,280.003", "0,0,180", "2.101,350.632,273.487", "0,0,277.364",
                        "0,0,277.962", "0,0,180", "5.161,343.451,265.469", "0,0,270.384", "0,0,277.962", "0,0,180"
                },
                {
                        "327.126,64.058,325.308", "12.900,352.478,7.703", "324.905,355.320,4.543", "0.000,0.000,180.000",
                        "359.800,7.581,7.372",
                        "6.899,359.266,358.505", "6.856,359.329,12.717", "0.000,0.000,180.000", "7.051,359.690,278.848",
                        "6.965,359.275,264.714",
                        "6.965,359.280,264.714", "0.000,0.000,180.000", "10.445,351.546,279.030", "7.448,359.738,264.766",
                        "7.611,0.007,265.796",
                        "0.000,0.000,180.000", "16.019,343.923,280.608", "7.531,359.801,264.775", "8.719,1.579,268.888",
                        "0.000,0.000,180.000"
                },
                {
                        "301.899,73.705,316.719", "0,0,359.593", "0,0,8.500", "0,0,180", "355.481,0.377,0.758", "0,0,13.531", "0,0,6",
                        "0,0,180",
                        "1.165,354.105,1.174", "0,0,9.121", "0,0,4.500", "0,0,180", "5.744,4.02,293.533", "0,0,271.266", "0,0,323.500",
                        "0,0,180",
                        "7.212,357.213,288.675", "0,0,276.719", "0,0,311.500", "0,0,180"
                },
                {
                        "310.936,132.177,251.420", "0.000,0.000,336.956", "0.000,0.000,305.429", "0.000,0.000,180.000",
                        "356.306,359.185,296.401",
                        "0.000,0.000,293.366", "0.000,0.000,299.293", "0.000,0.000,180.000", "1.714,0.772,284.468", "0.000,0.000,292.449",
                        "0.000,0.000,302.587", "0.000,0.000,180.000", "5.046,352.713,283.053", "359.748,359.712,301.092",
                        "359.321,359.470,308.252",
                        "0.000,0.000,180.000", "7.536,350.179,291.169", "0.450,0.209,298.961", "0.755,0.894,317.694", "0.000,0.000,180.000"
                },
                {
                        "289.962,92.332,293.140", "0,0,334.989", "0,0,338.006", "0,0,180", "355.957,359.99,296.976", "0,0,283.776",
                        "0,0,296.006",
                        "0,0,180", "1.77,1.354,286.438", "0,0,272.053", "0,0,310.006", "0,0,180", "4.445,358.244,280.766", "0,0,274.053",
                        "0,0,304.006", "0,0,180", "6.733,356.295,278.944", "0,0,276.053", "0,0,286.006", "0,0,180"
                },
                {
                        "293.987,73.711,308.474", "0,0,340.986", "0,0,344", "0,0,180", "357.492,1.322,310.894", "0,0,311.770", "0,0,312",
                        "0,0,180",
                        "1.77,1.354,300.432", "0,0,294.047", "0,0,326", "0,0,180", "2.599,357.394,294.708", "0,0,296.047", "0,0,320",
                        "0,0,180",
                        "3.005,354.571,292.784", "0,0,298.047", "0,0,302", "0,0,180"
                },
                {
                        "299.564,87.156,296.493", "0,0,348.834", "0,0,350.582", "0,0,180", "358.959,5.304,316.952", "0,0,314.200",
                        "0,0,322.252",
                        "0,0,180", "1.754,1.313,298.420", "0,0,292.046", "0,0,324.965", "0,0,180", "0.734,356.485,286.670", "0,0,288.096",
                        "0,0,318.851", "0,0,180", "0.054,351.256,278.862", "0,0,284.377", "0,0,326.156", "0,0,180"
                },
                {
                        "293.828,96.68,285.172", "0,0,344.846", "0,0,344.594", "0,0,180", "358.959,5.304,318.964", "0,0,320.212",
                        "0,0,324.264",
                        "0,0,180", "1.754,1.313,308.431", "0,0,306.058", "0,0,342.977", "0,0,180", "0.734,356.485,302.682", "0,0,308.107",
                        "0,0,336.863", "0,0,180", "0.054,351.256,300.874", "0,0,308.389", "0,0,323.167", "0,0,180"
                },
                {
                        "310.031,50.148,326.527", "0,0,350.986", "0,0,0", "0,0,180", "357.492,1.322,334.894", "0,0,331.770", "0,0,354",
                        "0,0,180",
                        "0.046,0.341,326.416", "0,0,332.047", "0,0,352", "0,0,180", "4.416,358.232,326.759", "0,0,332.047", "0,0,352",
                        "0,0,180",
                        "8.533,356.917,329.020", "0,0,332.047", "0,0,352", "0,0,180"
                },
                {
                        "322.69,27.275,353.097", "0.101,346.533,355.479", "359.901,351.098,343.704", "0,0,180", "357.614,349.201,316.546",
                        "0,0,0",
                        "-0.004,0.056,8.057", "0,0,180", "358.923,357.955,317.503", "0,-0.005,0", "-0.001,0.01,8.057", "0,0,180",
                        "359.402,3.131,314.755", "0,0.012,0.007", "0.006,359.927,9.059", "0,0,180", "358.592,6.511,318.614", "0,0,0.012",
                        "0.031,359.775,11.946", "0,0,180"
                },
                {
                        "322.9,28.074,354.675", "359.811,340.629,0.981", "359.658,352.675,354.667", "0,0,180", "0.299,1.866,11.461",
                        "0,0,0",
                        "0,0,0", "0,0,180", "8.172,9.711,350.168", "0,-0.005,0", "0,0,0", "0,0,180", "23.076,7.103,334.074",
                        "1.38,4.14,341.288",
                        "0,0,1.003", "0,0,180", "33.092,1.468,318.060", "0.071,0.242,327.356", "0,0,3.893", "0,0,180"
                },
                {
                        "323.310,35.892,344.865", "0.000,0.000,7.370", "0.000,0.000,4.000", "0.000,0.000,180.000", "0.592,356.275,2.949",
                        "0.000,0.000,0.000", "0.000,0.000,4.000", "0.000,0.000,180.000", "0.046,0.341,0.369", "0.000,0.000,3.881",
                        "0.000,0.000,4.000", "0.000,0.000,180.000", "359.928,2.040,0.448", "0.000,0.000,2.118", "0.000,0.000,5.003",
                        "0.000,0.000,180.000", "359.571,359.977,1.889", "0.000,0.000,0.000", "0.000,0.000,3.893", "0.000,0.000,180.000"
                },
                {
                        "321.799,38.770,11.138", "0.000,0.000,334.109", "0.000,0.000,307.407", "0.000,0.000,180.000",
                        "350.889,16.614,18.026",
                        "0.000,0.000,313.897", "0.000,0.000,312.072", "0.000,0.000,180.000", "0.011,0.299,14.713", "0.000,0.000,313.823",
                        "0.000,0.000,304.768", "0.000,0.000,180.000", "2.586,349.446,9.332", "0.000,0.000,316.359", "0.000,0.000,304.768",
                        "0.000,0.000,180.000", "3.553,335.408,11.256", "0.000,0.000,308.726", "0.000,0.000,297.768", "0.000,0.000,180.000"
                },
                {
                        "321.807,123.716,262.362", "0,0,349.370", "0,0,342", "0,0,180", "0.762,8.621,353.021", "0,0,340", "0,0,348",
                        "0,0,180",
                        "359.807,0.486,334.361", "0,0,293.881", "0,0,342", "0,0,180", "2.447,354.721,318.522", "0,0,324.118", "0,0,335.003",
                        "0,0,180", "0.133,343.247,349.932", "0,0,344", "0,0,347.893", "0,0,180"
                },
                {
                        "333.958,142.358,250.847", "0,0,18.961", "0,0,19.043", "0,0,180", "354.156,344.269,16.696", "359.612,0.505,283.827",
                        "359.834,359.675,305.878", "0,0,180", "351.552,350.175,14.199", "0.85,0.849,277.258", "359.971,359.943,305.877",
                        "0,0,180",
                        "350.036,354.286,14.290", "1.891,1.234,283.998", "0.208,0.425,306.882", "0,0,180", "348.554,353.985,22.084",
                        "2.006,1.244,299.347", "1.009,1.611,291.971", "0,0,180"
                }
        };

		private readonly int[] tunArray = new int[2023]
		{
				1342, 1343, 1350, 1351, 1352, 1922, 1928, 2995, 2996, 2997, 2998, 2999, 3000, 3001, 3002, 3003, 3004, 3005, 3006, 3007,
				3008, 3009, 3010, 3011, 3012, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021, 3022, 3023, 3024, 3025, 3026, 3027,
				3028, 3029, 3030, 3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041, 3042, 3043, 3044, 3045, 3046, 3047,
				3048, 3049, 3050, 3051, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061, 3062, 3063, 3064, 3065, 3066, 3067,
				3068, 3069, 3070, 3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081, 3082, 3083, 3084, 3085, 3086, 3087,
				3088, 3089, 3090, 3091, 3092, 3093, 3094, 3095, 3096, 3097, 3098, 3099, 3100, 3101, 3102, 3103, 3104, 3105, 3106, 3107,
				3108, 3109, 3110, 3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121, 3122, 3123, 3124, 3125, 3126, 3127,
				3128, 3129, 3130, 3131, 3132, 3133, 3134, 3135, 3136, 3137, 3138, 3139, 3140, 3141, 3142, 3143, 3144, 3145, 3146, 3147,
				3148, 3149, 3150, 3151, 3152, 3153, 3154, 3155, 3156, 3157, 3158, 3159, 3160, 3161, 3162, 3163, 3164, 3165, 3166, 3167,
				3168, 3169, 3170, 3171, 3172, 3173, 3174, 3175, 3176, 3177, 3178, 3179, 3180, 3181, 3182, 3183, 3184, 3185, 3186, 3187,
				3188, 3189, 3190, 3191, 3192, 3193, 3194, 3195, 3196, 3197, 3198, 3199, 3200, 3201, 3202, 3203, 3204, 3205, 3206, 3207,
				3208, 3209, 3210, 3211, 3212, 3213, 3214, 3215, 3216, 3217, 3218, 3219, 3220, 3221, 3222, 3223, 3224, 3225, 3226, 3227,
				3228, 3229, 3230, 3231, 3232, 3233, 3234, 3235, 3236, 3237, 3238, 3239, 3240, 3241, 3242, 3243, 3244, 3245, 3246, 3247,
				3248, 3249, 3250, 3251, 3252, 3253, 3254, 3255, 3256, 3257, 3258, 3259, 3260, 3261, 3262, 3263, 3264, 3265, 3266, 3267,
				3268, 3269, 3270, 3271, 3272, 3273, 3274, 3275, 3276, 3277, 3278, 3279, 3280, 3281, 3282, 3283, 3284, 3285, 3286, 3287,
				3288, 3289, 3290, 3291, 3292, 3293, 3294, 3295, 3296, 3297, 3298, 3299, 3300, 3301, 3302, 3303, 3304, 3305, 3306, 3307,
				3308, 3309, 3310, 3311, 3312, 3313, 3314, 3315, 3316, 3317, 3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327,
				3328, 3329, 3330, 3331, 3332, 3333, 3334, 3335, 3336, 3337, 3338, 3339, 3340, 3341, 3342, 3343, 3344, 3345, 3346, 3347,
				3348, 3349, 3350, 3351, 3352, 3353, 3354, 3355, 3356, 3357, 3358, 3359, 3360, 3361, 3362, 3363, 3364, 3365, 3366, 3367,
				3368, 3369, 3370, 3371, 3372, 3373, 3374, 3375, 3376, 3377, 3378, 3379, 3380, 3381, 3382, 3383, 3384, 3385, 3386, 3387,
				3388, 3389, 3390, 3391, 3392, 3393, 3394, 3395, 3396, 3397, 3398, 3399, 3400, 3401, 3402, 3403, 3404, 3405, 3406, 3407,
				3408, 3409, 3410, 3411, 3412, 3413, 3414, 3415, 3416, 3417, 3418, 3419, 3420, 3421, 3422, 3423, 3424, 3425, 3426, 3427,
				3428, 3429, 3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438, 3439, 3440, 3441, 3442, 3443, 3444, 3445, 3446, 3447,
				3448, 3449, 3450, 3451, 3452, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466, 3467,
				3468, 3469, 3470, 3471, 3472, 3473, 3474, 3475, 3476, 3477, 3478, 3479, 3480, 3481, 3482, 3483, 3484, 3485, 3486, 3487,
				3488, 3489, 3490, 3491, 3492, 3493, 3494, 3495, 3496, 3497, 3498, 3499, 3500, 3501, 3502, 3503, 3504, 3505, 3506, 3507,
				3508, 3509, 3510, 3511, 3512, 3513, 3514, 3515, 3516, 3517, 3518, 3519, 3520, 3521, 3522, 3523, 3524, 3525, 3526, 3527,
				3528, 3529, 3530, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541, 3542, 3543, 3544, 3545, 3546, 3547,
				3548, 3549, 3550, 3551, 3552, 3553, 3554, 3555, 3556, 3557, 3558, 3559, 3560, 3561, 3562, 3563, 3564, 3565, 3566, 3567,
				3568, 3569, 3570, 3571, 3572, 3573, 3574, 3575, 3576, 3577, 3578, 3579, 3580, 3581, 3582, 3583, 3584, 3585, 3586, 3587,
				3588, 3589, 3590, 3591, 3592, 3593, 3594, 3595, 3596, 3597, 3598, 3599, 3600, 3601, 3602, 3603, 3604, 3605, 3606, 3607,
				3608, 3609, 3610, 3611, 3612, 3613, 3614, 3615, 3616, 3617, 3618, 3619, 3620, 3621, 3622, 3623, 3624, 3625, 3626, 3627,
				3628, 3629, 3630, 3631, 3632, 3633, 3634, 3635, 3636, 3637, 3638, 3639, 3640, 3641, 3642, 3643, 3644, 3645, 3646, 3647,
				3648, 3649, 3650, 3651, 3652, 3653, 3654, 3655, 3656, 3657, 3658, 3659, 3660, 3661, 3662, 3663, 3664, 3665, 3666, 3667,
				3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683, 3684, 3685, 3686, 3687,
				3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3702, 3703, 3704, 3705, 3706, 3707,
				3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3717, 3718, 3719, 3720, 3721, 3722, 3723, 3724, 3725, 3726, 3727,
				3728, 3729, 3730, 3731, 3732, 3733, 3734, 3735, 3736, 3737, 3738, 3739, 3740, 3741, 3742, 3743, 3744, 3745, 3746, 3747,
				3748, 3749, 3750, 3751, 3753, 3754, 3755, 3756, 3757, 3758, 3760, 3761, 3762, 3763, 3764, 3765, 3766, 3767, 3768, 3769,
				3770, 3771, 3772, 3773, 3774, 3775, 3776, 3777, 3778, 3779, 3780, 3781, 3782, 3783, 3784, 3785, 3786, 3787, 3788, 3789,
				3790, 3791, 3792, 3793, 3794, 3795, 3796, 3797, 3798, 3799, 3800, 3801, 3802, 3803, 3804, 3805, 3806, 3807, 3808, 3809,
				3810, 3811, 3812, 3813, 3814, 3815, 3816, 3817, 3818, 3819, 3820, 3821, 3822, 3823, 3824, 3825, 3826, 3827, 3828, 3829,
				3830, 3831, 3832, 3833, 3834, 3835, 3836, 3837, 3838, 3839, 3840, 3841, 3842, 3843, 3844, 3845, 3846, 3847, 3848, 3849,
				3850, 3851, 3852, 3853, 3854, 3855, 3856, 3857, 3858, 3859, 3860, 3861, 3862, 3863, 3864, 3865, 3866, 3867, 3868, 3869,
				3870, 3871, 3872, 3873, 3874, 3875, 3876, 3877, 3878, 3879, 3880, 3881, 3882, 3883, 3884, 3885, 3886, 3887, 3888, 3889,
				3890, 3891, 3892, 3893, 3894, 3895, 3896, 3897, 3898, 3899, 3900, 3901, 3902, 3903, 3904, 3905, 3906, 3907, 3908, 3909,
				3910, 3911, 3912, 3913, 3914, 3915, 3916, 3917, 3918, 3919, 3920, 3921, 3922, 3923, 3924, 3925, 3926, 3927, 3928, 3929,
				3930, 3931, 3932, 3933, 3934, 3935, 3936, 3937, 3938, 3939, 3940, 3941, 3942, 3943, 3944, 3945, 3946, 3947, 3948, 3949,
				3950, 3951, 3952, 3953, 3954, 3955, 3956, 3957, 3958, 3959, 3960, 3961, 3962, 3963, 3964, 3965, 3966, 3967, 3968, 3969,
				3970, 3971, 3972, 3973, 3974, 3975, 3976, 3977, 3978, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 3988, 3989,
				3990, 3991, 3992, 3993, 3994, 3995, 3996, 3997, 3998, 3999, 4000, 4001, 4002, 4003, 4004, 4005, 4006, 4007, 4008, 4009,
				4010, 4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021, 4022, 4023, 4024, 4025, 4026, 4027, 4028, 4029,
				4030, 4031, 4032, 4033, 4034, 4035, 4036, 4037, 4038, 4039, 4040, 4041, 4042, 4043, 4044, 4045, 4046, 4047, 4048, 4049,
				4050, 4051, 4052, 4053, 4054, 4055, 4056, 4057, 4058, 4059, 4060, 4061, 4062, 4063, 4064, 4065, 4066, 4067, 4068, 4069,
				4070, 4071, 4072, 4073, 4074, 4075, 4076, 4077, 4078, 4079, 4080, 4081, 4082, 4083, 4084, 4085, 4086, 4087, 4088, 4089,
				4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097, 4098, 4099, 4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109,
				4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 4119, 4120, 4121, 4122, 4123, 4124, 4125, 4126, 4127, 4128, 4129,
				4130, 4131, 4132, 4133, 4134, 4135, 4136, 4137, 4138, 4139, 4140, 4528, 4529, 4530, 4531, 4532, 4533, 4534, 4535, 4536,
				4537, 4538, 4539, 4540, 4541, 4542, 4543, 4544, 4545, 4546, 4547, 4548, 4549, 4550, 4551, 4552, 4553, 4554, 4555, 4556,
				4557, 4558, 4559, 4560, 4561, 4562, 4563, 4564, 4565, 4566, 4567, 4568, 4569, 4570, 4571, 4572, 4573, 4574, 4575, 4576,
				4577, 4578, 4579, 4580, 4581, 4582, 4583, 4584, 4585, 4586, 4587, 4588, 4589, 4590, 4591, 4592, 4593, 4594, 4595, 4596,
				4597, 4598, 4599, 4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607, 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616,
				4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626, 4627, 4628, 4629, 4630, 4631, 4632, 4633, 4634, 4635, 4636,
				4637, 4638, 4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646, 4647, 4648, 4649, 4650, 4651, 4652, 4653, 4654, 4655, 4656,
				4657, 4658, 4659, 4660, 4661, 4662, 4663, 4664, 4665, 4666, 4667, 4668, 4669, 4670, 4671, 4672, 4673, 4674, 4675, 4676,
				4677, 4678, 4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686, 4687, 4688, 4689, 4690, 4691, 4692, 4693, 4694, 4695, 4696,
				4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712, 4713, 4714, 4715, 4716,
				4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726, 4727, 4728, 4729, 4730, 4731, 4732, 4733, 4734, 4735, 4736,
				4737, 4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746, 4747, 4748, 4749, 4750, 4751, 4752, 4753, 4754, 4755, 4756,
				4757, 4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766, 4767, 4768, 4769, 4770, 4771, 4772, 4773, 4774, 4775, 4776,
				4777, 4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786, 4787, 4788, 4789, 4790, 4791, 4792, 4793, 4794, 4795, 4796,
				4797, 4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806, 4807, 4808, 4809, 4810, 4811, 4812, 4813, 4814, 4815, 4816,
				4817, 4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826, 5573, 5574, 5575, 5576, 5577, 5578, 5579, 5580, 5581, 5582,
				5583, 5584, 5585, 5586, 5587, 5588, 5589, 5590, 5591, 5592, 5593, 5594, 5595, 5596, 5597, 5598, 5599, 5600, 5601, 5602,
				5603, 5604, 5605, 5606, 5607, 5608, 5609, 5610, 5611, 5612, 5613, 5614, 5615, 5616, 5617, 5618, 5619, 5620, 5621, 5622,
				5623, 5624, 5625, 5626, 5627, 5628, 5629, 5630, 5631, 5632, 5633, 5634, 5635, 5636, 5637, 5638, 5639, 5640, 5641, 5642,
				5643, 5644, 5645, 5646, 5647, 5648, 5649, 5650, 5651, 5652, 5653, 5654, 5655, 5656, 5657, 5658, 5659, 5660, 5662, 5663,
				5664, 5665, 5666, 5896, 5897, 5898, 5899, 5900, 5901, 5902, 5903, 5904, 5905, 5906, 5907, 5908, 5909, 5910, 5911, 5912,
				5913, 5914, 5915, 5916, 5917, 5918, 5919, 5920, 5921, 5922, 5923, 5924, 5925, 5926, 5927, 5928, 5929, 5930, 5931, 5932,
				5933, 5934, 5935, 5936, 5937, 5938, 5939, 5940, 5941, 5942, 5943, 5944, 5945, 5946, 5947, 5948, 5949, 5950, 5951, 5952,
				5953, 5954, 5955, 5956, 5957, 5958, 5959, 5960, 5961, 5962, 5963, 5964, 5965, 5966, 5967, 5968, 5969, 5970, 5971, 5972,
				5973, 5974, 5975, 5976, 5977, 5978, 5979, 5980, 5981, 5982, 5983, 5984, 5985, 5986, 5987, 5988, 5989, 5990, 5991, 5992,
				5993, 5994, 5995, 5996, 5997, 5998, 5999, 6000, 6001, 6002, 6003, 6004, 6055, 6056, 6057, 6058, 6059, 6060, 6061, 6062,
				6063, 6064, 6065, 6066, 6067, 6068, 6069, 6070, 6071, 6072, 6073, 6074, 6075, 6076, 6077, 6078, 6079, 6080, 6081, 6082,
				6083, 6084, 6085, 6086, 6087, 6088, 6089, 6090, 6091, 6092, 6093, 6094, 6095, 6096, 6097, 6098, 6099, 6100, 6101, 6102,
				6103, 6104, 6105, 6106, 6107, 6108, 6109, 6110, 6111, 6112, 6113, 6114, 6115, 6116, 6117, 6118, 6119, 6120, 6121, 6122,
				6123, 6124, 6125, 6126, 6127, 6128, 6129, 6130, 6131, 6132, 6133, 6134, 6135, 6136, 6137, 6138, 6139, 6140, 6141, 6142,
				6143, 6144, 6145, 6146, 6147, 6148, 6149, 6150, 6151, 6152, 6153, 6154, 6428, 6429, 6430, 6431, 6432, 6433, 6434, 6435,
				6436, 6437, 6438, 6439, 6440, 6441, 6442, 6443, 6444, 6445, 6446, 6447, 6448, 6449, 6450, 6451, 6452, 6453, 6454, 6455,
				6456, 6457, 6458, 6459, 6460, 6461, 6462, 6463, 6464, 6465, 6466, 6467, 6468, 6469, 6470, 6471, 6472, 6473, 6474, 6495,
				6496, 6497, 6498, 6515, 6516, 6517, 6518, 6519, 6520, 6521, 6522, 6523, 6524, 6525, 6526, 6527, 6528, 6529, 6530, 6531,
				6532, 6533, 6534, 6535, 6536, 6537, 6538, 6539, 6540, 6541, 6542, 6543, 6544, 6545, 6546, 6547, 6548, 6549, 6550, 6551,
				6552, 6553, 6554, 6555, 6556, 6557, 6558, 6559, 6560, 6561, 6562, 6563, 6564, 6888, 6889, 6890, 6891, 6892, 6893, 6894,
				6895, 6896, 6897, 6898, 6899, 6900, 6901, 6902, 6903, 6904, 6905, 6929, 6930, 6931, 6932, 6933, 6934, 6935, 6936, 6937,
				6938, 6939, 6940, 6941, 6942, 6943, 6944, 6945, 6946, 6947, 6948, 6949, 6950, 6951, 6952, 6953, 6954, 6955, 6956, 6957,
				6958, 6959, 6960, 6961, 6962, 6963, 6964, 6965, 6966, 6967, 6968, 6969, 6970, 6971, 6972, 6973, 6974, 6975, 6976, 6977,
				6978, 6979, 6980, 6981, 6982, 6983, 6984, 6985, 6986, 6987, 6988, 6989, 6990, 6991, 6992, 6993, 6994, 6995, 6996, 6997,
				8909, 8910, 8911, 8912, 8913, 8914, 8915, 8916, 8917, 8918, 8919, 8920, 8921, 8922, 8923, 8924, 8925, 8926, 8927, 8928,
				8929, 8930, 8931, 8932, 8933, 8934, 8935, 8936, 8937, 8938, 9375, 9376, 9377, 9378, 9379, 9380, 9381, 9382, 9383, 9384,
				9385, 9386, 9387, 9388, 9389, 9390, 9391, 9392, 9393, 9394, 9395, 9396, 9512, 9513, 9514, 9515, 9516, 9517, 9518, 9519,
				9520, 9521, 9522, 9528, 9529, 9530, 9531, 9532, 9533, 9534, 9535, 9536, 9537, 9538, 9539, 9540, 9541, 9542, 9543, 9544,
				9548, 9549, 9550
		};
		private readonly int[] coolArray = new int[2253]
		{
				2415, 2416, 2423, 2424, 2425, 3219, 3225, 3435, 3436, 3437, 3438, 3439, 3440, 3441, 3442, 3443, 3444, 3445, 3446, 3447,
				3448, 3449, 3450, 3451, 3452, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466, 3467,
				3468, 3469, 3470, 3471, 3472, 3473, 3474, 3475, 3476, 3477, 3478, 3479, 3480, 3481, 3482, 3483, 3484, 3485, 3486, 3487,
				3488, 3489, 3490, 3491, 3492, 3493, 3494, 3495, 3496, 3497, 3498, 3499, 3500, 3501, 3502, 3503, 3504, 3505, 3506, 3507,
				3508, 3509, 3510, 3511, 3512, 3513, 3514, 3515, 3516, 3517, 3518, 3519, 3520, 3521, 3522, 3523, 3524, 3525, 3526, 3527,
				3528, 3529, 3530, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541, 3542, 3543, 3544, 3545, 3546, 3547,
				3548, 3549, 3550, 3551, 3552, 3553, 3554, 3555, 3556, 3557, 3558, 3559, 3560, 3561, 3562, 3563, 3564, 3565, 3566, 3567,
				3568, 3569, 3570, 3571, 3572, 3573, 3574, 3575, 3576, 3577, 3578, 3579, 3580, 3581, 3582, 3583, 3584, 3585, 3586, 3587,
				3588, 3589, 3590, 3591, 3592, 3593, 3594, 3595, 3596, 3597, 3598, 3599, 3600, 3601, 3602, 3603, 3604, 3605, 3606, 3607,
				3608, 3609, 3610, 3611, 3612, 3613, 3614, 3615, 3616, 3617, 3618, 3619, 3620, 3621, 3622, 3623, 3624, 3625, 3626, 3627,
				3628, 3629, 3630, 3631, 3632, 3633, 3634, 3635, 3636, 3637, 3638, 3639, 3640, 3641, 3642, 3643, 3644, 3645, 3646, 3647,
				3648, 3649, 3650, 3651, 3652, 3653, 3654, 3655, 3656, 3657, 3658, 3659, 3660, 3661, 3662, 3663, 3664, 3665, 3666, 3667,
				3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683, 3684, 3685, 3686, 3687,
				3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3702, 3703, 3704, 3705, 3706, 3707,
				3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3717, 3718, 3719, 3720, 3721, 3722, 3723, 3724, 3725, 3726, 3727,
				3728, 3729, 3730, 3731, 3732, 3733, 3734, 3735, 3736, 3737, 3738, 3739, 3740, 3741, 3742, 3743, 3744, 3745, 3746, 3747,
				3748, 3749, 3750, 3751, 3752, 3753, 3754, 3755, 3756, 3757, 3758, 3759, 3760, 3761, 3762, 3763, 3764, 3765, 3766, 3767,
				3768, 3769, 3770, 3771, 3772, 3773, 3774, 3775, 3776, 3777, 3778, 3779, 3780, 3781, 3782, 3783, 3784, 3785, 3786, 3787,
				3788, 3789, 3790, 3791, 3792, 3793, 3794, 3795, 3796, 3797, 3798, 3799, 3800, 3801, 3802, 3803, 3804, 3805, 3806, 3807,
				3808, 3809, 3810, 3811, 3812, 3813, 3814, 3815, 3816, 3817, 3818, 3819, 3820, 3821, 3822, 3823, 3824, 3825, 3826, 3827,
				3828, 3829, 3830, 3831, 3832, 3833, 3834, 3835, 3836, 3837, 3838, 3839, 3840, 3841, 3842, 3843, 3844, 3845, 3846, 3847,
				3848, 3849, 3850, 3851, 3852, 3853, 3854, 3855, 3856, 3857, 3858, 3859, 3860, 3861, 3862, 3863, 3864, 3865, 3866, 3867,
				3868, 3869, 3870, 3871, 3872, 3873, 3874, 3875, 3876, 3877, 3878, 3879, 3880, 3881, 3882, 3883, 3884, 3885, 3886, 3887,
				3888, 3889, 3890, 3891, 3892, 3893, 3894, 3895, 3896, 3897, 3898, 3899, 3900, 3901, 3902, 3903, 3904, 3905, 3906, 3907,
				3908, 3909, 3910, 3911, 3912, 3913, 3914, 3915, 3916, 3917, 3918, 3919, 3920, 3921, 3922, 3923, 3924, 3925, 3926, 3927,
				3928, 3929, 3930, 3931, 3932, 3933, 3934, 3935, 3936, 3937, 3938, 3939, 3940, 3941, 3942, 3943, 3944, 3945, 3946, 3947,
				3948, 3949, 3950, 3951, 3952, 3953, 3954, 3955, 3956, 3957, 3958, 3959, 3960, 3961, 3962, 3963, 3964, 3965, 3966, 3967,
				3968, 3969, 3970, 3971, 3972, 3973, 3974, 3975, 3976, 3977, 3978, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987,
				3988, 3989, 3990, 3991, 3992, 3993, 3994, 3995, 3996, 3997, 3998, 3999, 4000, 4001, 4002, 4003, 4004, 4005, 4006, 4007,
				4008, 4009, 4010, 4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021, 4022, 4023, 4024, 4025, 4026, 4027,
				4028, 4029, 4030, 4031, 4032, 4033, 4034, 4035, 4036, 4037, 4038, 4039, 4040, 4041, 4042, 4043, 4044, 4045, 4046, 4047,
				4048, 4049, 4050, 4051, 4052, 4053, 4054, 4055, 4056, 4057, 4058, 4059, 4060, 4061, 4062, 4063, 4064, 4065, 4066, 4067,
				4068, 4069, 4070, 4071, 4072, 4073, 4074, 4075, 4076, 4077, 4078, 4079, 4080, 4081, 4082, 4083, 4084, 4085, 4086, 4087,
				4088, 4089, 4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097, 4098, 4099, 4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107,
				4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 4119, 4120, 4121, 4122, 4123, 4124, 4125, 4126, 4127,
				4128, 4129, 4130, 4131, 4132, 4133, 4134, 4135, 4136, 4137, 4138, 4139, 4140, 4141, 4142, 4143, 4144, 4145, 4146, 4147,
				4148, 4149, 4150, 4151, 4152, 4153, 4154, 4155, 4156, 4157, 4158, 4159, 4160, 4161, 4162, 4163, 4164, 4165, 4166, 4499,
				4500, 4501, 4502, 4503, 4504, 4505, 4506, 4507, 4508, 4509, 4510, 4511, 4512, 4513, 4514, 4515, 4516, 4517, 4518, 4519,
				4520, 4521, 4522, 4523, 4524, 4525, 4526, 4527, 4528, 4529, 4530, 4531, 4532, 4533, 4534, 4535, 4536, 4537, 4538, 4539,
				4540, 4541, 4542, 4543, 4544, 4545, 4546, 4547, 4548, 4549, 4550, 4551, 4552, 4553, 4554, 4555, 4556, 4557, 4558, 4559,
				4560, 4561, 4562, 4563, 4564, 4565, 4566, 4567, 4568, 4569, 4570, 4571, 4572, 4573, 4574, 4575, 4576, 4577, 4578, 4579,
				4580, 4581, 4582, 4583, 4584, 4585, 4586, 4587, 4588, 4589, 4590, 4591, 4592, 4593, 4594, 4595, 4596, 4597, 4598, 4599,
				4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607, 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4618,
				4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626, 4627, 4628, 4629, 4630, 4631, 4632, 4633, 4634, 4635, 4636, 4637, 4638,
				4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646, 4647, 4648, 4649, 4650, 4651, 4652, 4653, 4654, 4655, 4656, 4657, 4658,
				4659, 4660, 4661, 4662, 4663, 4664, 4665, 4666, 4667, 4668, 4669, 4670, 4671, 4672, 4673, 4674, 4675, 4676, 4677, 4678,
				4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686, 4687, 4688, 4689, 4690, 4690, 4691, 4692, 4693, 4694, 4695, 4696, 4697,
				4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712, 4713, 4714, 4715, 4716, 4717,
				4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726, 4727, 4728, 4729, 4730, 4731, 4732, 4733, 4734, 4735, 4736, 4737,
				4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746, 4747, 4748, 4749, 4750, 4751, 4752, 4753, 4754, 4755, 4756, 4757,
				4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766, 4767, 4768, 4769, 4770, 4771, 4772, 4773, 4774, 4775, 4776, 4777,
				4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786, 4787, 4788, 4789, 4790, 4791, 4792, 4793, 4794, 4795, 4796, 4797,
				4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806, 4807, 4808, 4809, 4810, 4811, 4812, 4813, 4814, 4815, 4816, 4817,
				4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826, 4827, 4828, 4829, 4830, 4831, 4832, 4833, 4834, 4835, 4836, 4837,
				4838, 4839, 4840, 4841, 4842, 4843, 4844, 4845, 4846, 4847, 4848, 4849, 4850, 4851, 4852, 4853, 4854, 4855, 4856, 4857,
				4858, 4859, 4860, 4861, 4862, 4863, 4864, 4865, 4866, 4867, 4868, 4869, 4870, 4871, 4872, 4873, 4874, 4875, 4876, 4877,
				4878, 4879, 4880, 4881, 4882, 4883, 4884, 4995, 4996, 4997, 4998, 4999, 5000, 5001, 5002, 5003, 5004, 5005, 5006, 5007,
				5008, 5009, 5010, 5011, 5012, 5013, 5014, 5015, 5016, 5017, 5018, 5019, 5020, 5021, 5022, 5023, 5024, 5025, 5026, 5027,
				5028, 5029, 5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 5042, 5043, 5044, 5045, 5046, 5047,
				5048, 5049, 5050, 5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058, 5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5067,
				5068, 5069, 5070, 5071, 5072, 5073, 5074, 5075, 5076, 5077, 5078, 5079, 5303, 5304, 5305, 5306, 5307, 5308, 5309, 5310,
				5311, 5312, 5313, 5314, 5315, 5316, 5317, 5318, 5319, 5320, 5321, 5322, 5323, 5324, 5325, 5326, 5327, 5328, 5329, 5330,
				5331, 5332, 5333, 5334, 5335, 5336, 5337, 5338, 5339, 5484, 5485, 5486, 5487, 5488, 5489, 5490, 5491, 5492, 5493, 5494,
				5495, 5496, 5497, 5498, 5499, 5500, 5501, 5502, 5503, 5504, 5505, 5506, 5507, 5508, 5509, 5510, 5511, 5512, 5513, 5514,
				5515, 5516, 5517, 5518, 5519, 5520, 5521, 5522, 5523, 5524, 5525, 5526, 5527, 5528, 5529, 5530, 5531, 5532, 5533, 5534,
				5535, 5536, 5537, 5538, 5539, 5540, 5541, 5542, 5543, 5544, 5545, 5546, 5547, 5548, 5549, 5550, 5551, 5552, 5553, 5554,
				5555, 5556, 5557, 5558, 5559, 5560, 5561, 5562, 5563, 5564, 5565, 5566, 5567, 5568, 5569, 5570, 5571, 5572, 5573, 5574,
				5575, 5576, 5577, 5578, 5579, 5580, 5581, 5582, 5583, 5584, 5585, 5586, 5587, 5588, 5589, 5590, 5591, 5592, 5593, 5594,
				5595, 5596, 5597, 5598, 5599, 5600, 5601, 5602, 5603, 5604, 5605, 5606, 5607, 5608, 5609, 5610, 5611, 5612, 5613, 5614,
				5615, 5616, 5617, 5618, 5619, 5620, 5621, 5622, 5623, 5624, 5625, 5626, 5627, 5628, 5629, 5630, 5631, 5632, 5633, 5634,
				5635, 5636, 5637, 5638, 5639, 5640, 5641, 5642, 5643, 5644, 5645, 5646, 5647, 5648, 5649, 5650, 5651, 5652, 5653, 5654,
				5655, 5656, 5657, 5658, 5659, 5660, 5661, 5662, 5663, 5664, 5665, 5666, 5667, 5668, 5669, 5670, 5671, 5672, 5673, 5674,
				5675, 5676, 5677, 5678, 5679, 5680, 5681, 5682, 5683, 5684, 5685, 5686, 5687, 5688, 5689, 5690, 5691, 5692, 5693, 5694,
				5695, 5696, 5697, 5698, 5699, 5700, 5701, 5702, 5703, 5704, 6112, 6113, 6114, 6115, 6116, 6117, 6118, 6119, 6120, 6121,
				6122, 6123, 6124, 6125, 6126, 6127, 6128, 6129, 6130, 6131, 6132, 6133, 6134, 6135, 6136, 6137, 6138, 6139, 6140, 6141,
				6142, 6143, 6144, 6145, 6146, 6147, 6148, 6149, 6150, 6151, 6152, 6153, 6154, 6155, 6156, 6157, 6158, 6159, 6160, 6161,
				6162, 6163, 6164, 6165, 6166, 6167, 6168, 6169, 6170, 6171, 6172, 6173, 6174, 6175, 6176, 6177, 6178, 6179, 6180, 6181,
				6182, 6183, 6184, 6185, 6186, 6187, 6188, 6189, 6190, 6191, 6192, 6193, 6194, 6195, 6196, 6197, 6198, 6199, 6200, 6201,
				6202, 6203, 6204, 6205, 6206, 6207, 6208, 6209, 6210, 6211, 6212, 6213, 6214, 6215, 6216, 6217, 6218, 6219, 6220, 6221,
				6222, 6223, 6224, 6225, 6226, 6227, 6228, 6229, 6230, 6231, 6232, 6233, 6234, 6235, 6236, 6237, 6238, 6239, 6240, 6241,
				6242, 6243, 6244, 6245, 6246, 6247, 6248, 6249, 6250, 6251, 6252, 6253, 6254, 6255, 6256, 6257, 6258, 6259, 6260, 6261,
				6262, 6263, 6264, 6265, 6266, 6267, 6268, 6269, 6270, 6271, 6272, 6273, 6274, 6275, 6276, 6277, 6278, 6279, 6280, 6281,
				6282, 6283, 6284, 6285, 6286, 6287, 6288, 6289, 6290, 6291, 6292, 6293, 6294, 6295, 6296, 6297, 6298, 6299, 6300, 6301,
				6302, 6303, 6304, 6305, 6306, 6307, 6308, 6309, 6310, 6311, 6312, 6313, 6314, 6315, 6316, 6317, 6318, 6319, 6320, 6321,
				6322, 6323, 6324, 6325, 6326, 6327, 6328, 6329, 6330, 6331, 6332, 6333, 6334, 6335, 6336, 6337, 6338, 6339, 6340, 6341,
				6342, 6343, 6344, 6345, 6346, 6347, 6348, 6349, 6350, 6351, 6352, 6353, 6354, 6355, 6356, 6357, 6358, 6359, 6360, 6361,
				6362, 6363, 6364, 6365, 6366, 6367, 6368, 6369, 6370, 6371, 6372, 6373, 6374, 6375, 6376, 6377, 6378, 6379, 6380, 6381,
				6382, 6383, 6384, 6385, 6386, 6387, 6388, 6389, 6390, 6391, 6392, 6393, 6394, 6395, 6396, 6397, 6398, 6399, 6400, 6401,
				6402, 6403, 6404, 6405, 6406, 6407, 6408, 6409, 6410, 6411, 6412, 6413, 6414, 6415, 6416, 6417, 6418, 6419, 6420, 6421,
				6422, 6423, 6424, 6425, 6426, 6427, 6428, 6429, 6430, 6431, 6432, 6433, 6434, 6435, 6436, 6437, 6438, 6439, 6440, 6441,
				6442, 6443, 6444, 6445, 6446, 6447, 6448, 6449, 6450, 6451, 6452, 6453, 6454, 6455, 6456, 6457, 6458, 6459, 6460, 6461,
				6462, 6463, 6464, 6465, 6466, 6467, 6468, 6469, 6470, 6471, 6472, 6473, 6474, 6475, 6476, 6477, 6478, 6479, 6480, 6481,
				6482, 6483, 6484, 6485, 6486, 6487, 6488, 6489, 6490, 6491, 6492, 6493, 6494, 6495, 6496, 6497, 6498, 6499, 6500, 6501,
				6502, 6503, 6504, 6505, 6506, 6507, 6508, 6509, 6510, 6511, 6512, 6513, 6514, 6515, 6516, 6517, 6518, 6519, 6520, 6521,
				6522, 6523, 6524, 6525, 6526, 6527, 6528, 6529, 6530, 6531, 6532, 6533, 6534, 6535, 6536, 6537, 6538, 6539, 6540, 6541,
				6542, 6543, 6544, 6545, 6546, 6547, 6548, 6549, 6550, 6551, 6552, 6553, 6554, 6555, 6556, 6557, 6558, 6559, 6560, 6561,
				6562, 6563, 6564, 6565, 6566, 6567, 6568, 6569, 6570, 6571, 6572, 6573, 6574, 6575, 6576, 6577, 6578, 6579, 6580, 6581,
				6582, 6583, 6584, 6585, 6586, 6587, 6588, 6589, 6590, 6591, 6592, 6593, 6594, 6595, 6596, 6597, 6598, 6599, 6600, 6601,
				6602, 6603, 6604, 6605, 6606, 6607, 6608, 6609, 6610, 6611, 6612, 6613, 6614, 6615, 6616, 6617, 6618, 6619, 6620, 6621,
				6622, 6623, 6624, 6625, 6626, 6627, 6628, 6629, 6630, 6631, 6632, 6633, 6634, 6635, 6636, 6637, 6638, 6639, 6640, 6641,
				6642, 6643, 6644, 6645, 6646, 6647, 6648, 6649, 6650, 6651, 6652, 6653, 6654, 6655, 6824, 6825, 6826, 6827, 6828, 6829,
				6830, 6831, 6832, 6833, 6834, 6835, 6836, 6837, 6838, 6839, 6840, 6841, 6842, 6843, 6844, 6845, 6846, 6847, 6848, 6849,
				6850, 6851, 6852, 6853, 6854, 6855, 6856, 6857, 6858, 6859, 6860, 6861, 6862, 6863, 6864, 6865, 6866, 6867, 6868, 6869,
				6870, 6892, 6893, 6894, 6907, 6908, 6909, 6910, 6911, 6912, 6913, 6914, 6915, 6916, 6917, 6918, 6919, 6920, 6921, 6922,
				6923, 6924, 6925, 6946, 6947, 6948, 6949, 6950, 6951, 6952, 6953, 6954, 6955, 6956, 6957, 6958, 6959, 6960, 6961, 6962,
				6963, 6964, 6965, 6966, 6967, 6968, 6969, 6970, 6971, 6972, 6973, 6974, 6975, 6976, 6977, 6978, 6979, 6980, 6981, 6982,
				6983, 6984, 6985, 6986, 6987, 6988, 6989, 6990, 6991, 6992, 6993, 6994, 6995, 6996, 6997, 6998, 6999, 7000, 7001, 7002,
				7003, 7004, 7005, 7006, 7007, 7008, 7009, 7010, 7011, 7012, 7013, 7014, 7015, 7016, 7017, 7018, 7019, 7020, 7021, 7022,
				7023, 7024, 7025, 7026, 7027, 7028, 7029, 7030, 7031, 7032, 8827, 8828, 8829, 8830, 8831, 8832, 8833, 8834, 8835, 8836,
				8837, 8838, 8839, 8840, 8841, 8842, 8843, 8844, 8845, 8846, 8847, 8848, 8849, 8850, 8851, 8852, 8853, 8854, 8855, 9422,
				9423, 9424, 9425, 9426, 9427, 9428, 9429, 9430, 9431, 9432, 9433, 9434, 9435, 9436, 9437, 9438, 9439, 9440, 9441, 9442,
				9443, 9444, 9560, 9561, 9562, 9563, 9564, 9565, 9566, 9567, 9568, 9569, 9570, 9576, 9577, 9578, 9579, 9580, 9581, 9582,
				9583, 9584, 9585, 9586, 9587, 9588, 9589, 9590, 9591, 9592, 9596, 9597, 9598
		};
		private readonly int[] pureArray = new int[2299]
		{
				1256, 1257, 1264, 1265, 1266, 1446, 1452, 2518, 2519, 2520, 2521, 2522, 2523, 2524, 2525, 2526, 2527, 2528, 2529, 2530,
				2531, 2532, 2533, 2534, 2535, 2536, 2537, 2538, 2539, 2540, 2541, 2542, 2543, 2544, 2545, 2546, 2547, 2548, 2549, 2550,
				2551, 2552, 2553, 2554, 2555, 2556, 2557, 2558, 2559, 2560, 2561, 2562, 2563, 2564, 2565, 2566, 2567, 2568, 2569, 2570,
				2571, 2572, 2573, 2574, 2575, 2576, 2577, 2578, 2579, 2580, 2581, 2582, 2583, 2584, 2585, 2586, 2587, 2588, 2589, 2590,
				2591, 2592, 2593, 2594, 2595, 2596, 2597, 2598, 2599, 2600, 2601, 2602, 2603, 2604, 2605, 2606, 2607, 2608, 2609, 2610,
				2611, 2612, 2613, 2614, 2615, 2616, 2617, 2618, 2619, 2620, 2621, 2622, 2623, 2624, 2625, 2626, 2627, 2628, 2629, 2630,
				2631, 2632, 2633, 2634, 2635, 2636, 2637, 2638, 2639, 2640, 2641, 2642, 2643, 2644, 2645, 2646, 2647, 2648, 2649, 2650,
				2651, 2652, 2653, 2654, 2655, 2656, 2657, 2658, 2659, 2660, 2661, 2662, 2663, 2664, 2665, 2666, 2667, 2668, 2669, 2670,
				2671, 2672, 2673, 2674, 2675, 2676, 2677, 2678, 2679, 2680, 2681, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2689, 2690,
				2691, 2692, 2693, 2694, 2695, 2696, 2697, 2698, 2699, 2700, 2701, 2702, 2703, 2704, 2705, 2706, 2707, 2708, 2709, 2710,
				2711, 2712, 2713, 2714, 2715, 2716, 2717, 2718, 2719, 2720, 2721, 2722, 2723, 2724, 2725, 2726, 2727, 2728, 2729, 2730,
				2731, 2732, 2733, 2734, 2735, 2736, 2737, 2738, 2739, 2740, 2741, 2742, 2743, 2744, 2745, 2746, 2747, 2748, 2749, 2750,
				2751, 2752, 2753, 2754, 2755, 2756, 2757, 2758, 2759, 2760, 2761, 2762, 2763, 2764, 2765, 2766, 2767, 2768, 2769, 2770,
				2771, 2772, 2773, 2774, 2775, 2776, 2777, 2778, 2779, 2780, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788, 2789, 2790,
				2791, 2792, 2793, 2794, 2795, 2796, 2797, 2798, 2799, 2800, 2801, 2802, 2803, 2804, 2805, 2806, 2807, 2808, 2809, 2810,
				2811, 2812, 2813, 2814, 2815, 2816, 2817, 2818, 2819, 2820, 2821, 2822, 2823, 2824, 2825, 2826, 2827, 2828, 2829, 2830,
				2831, 2832, 2833, 2834, 2835, 2836, 2837, 2838, 2839, 2840, 2841, 2842, 2843, 2844, 2845, 2846, 2847, 2848, 2849, 2850,
				2851, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860, 2861, 2862, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870,
				2871, 2872, 2873, 2874, 2875, 2876, 2877, 2878, 2879, 2880, 2881, 2882, 2883, 2884, 2885, 2886, 2887, 2888, 2889, 2890,
				2891, 2892, 2893, 2894, 2895, 2896, 2897, 2898, 2899, 2900, 2901, 2902, 2903, 2904, 2905, 2906, 2907, 2908, 2909, 2910,
				2911, 2912, 2913, 2914, 2915, 2916, 2917, 2918, 2919, 2920, 2921, 2922, 2923, 2924, 2925, 2926, 2927, 2928, 2929, 2930,
				2931, 2932, 2933, 2934, 2935, 2936, 2937, 2938, 2939, 2940, 2941, 2942, 2943, 2944, 2945, 2946, 2947, 2948, 2949, 2950,
				2951, 2952, 2953, 2954, 2955, 2956, 2957, 2958, 2959, 2960, 2961, 2962, 2963, 2964, 2965, 2966, 2967, 2968, 2969, 2970,
				2971, 2972, 2973, 2974, 2975, 2976, 2977, 2978, 2979, 2980, 2981, 2982, 2983, 2984, 2985, 2986, 2987, 2988, 2989, 2990,
				2991, 2992, 2993, 2994, 2995, 2996, 2997, 2998, 2999, 3000, 3001, 3002, 3003, 3004, 3005, 3006, 3007, 3008, 3009, 3010,
				3011, 3012, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021, 3022, 3023, 3024, 3025, 3026, 3027, 3028, 3029, 3030,
				3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041, 3042, 3043, 3044, 3045, 3046, 3047, 3048, 3049, 3050,
				3051, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061, 3062, 3063, 3064, 3065, 3066, 3067, 3068, 3069, 3070,
				3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081, 3082, 3083, 3084, 3085, 3086, 3087, 3088, 3089, 3090,
				3091, 3092, 3093, 3094, 3095, 3096, 3097, 3098, 3099, 3100, 3101, 3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109, 3110,
				3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121, 3122, 3123, 3124, 3125, 3126, 3127, 3128, 3129, 3130,
				3131, 3132, 3133, 3134, 3135, 3136, 3137, 3138, 3139, 3140, 3141, 3142, 3143, 3144, 3145, 3146, 3147, 3148, 3149, 3150,
				3151, 3152, 3153, 3154, 3155, 3156, 3157, 3158, 3159, 3160, 3161, 3162, 3163, 3164, 3165, 3166, 3167, 3168, 3169, 3170,
				3171, 3172, 3173, 3174, 3175, 3176, 3177, 3178, 3179, 3180, 3181, 3182, 3183, 3184, 3185, 3186, 3187, 3188, 3189, 3190,
				3191, 3192, 3193, 3194, 3195, 3196, 3197, 3198, 3199, 3200, 3201, 3202, 3203, 3204, 3205, 3206, 3207, 3208, 3209, 3210,
				3211, 3212, 3213, 3214, 4082, 4083, 4084, 4085, 4086, 4087, 4088, 4089, 4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097,
				4098, 4099, 4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117,
				4118, 4119, 4120, 4121, 4122, 4123, 4124, 4125, 4126, 4127, 4128, 4129, 4130, 4131, 4132, 4133, 4134, 4135, 4136, 4137,
				4138, 4139, 4140, 4141, 4142, 4143, 4144, 4145, 4146, 4147, 4148, 4149, 4150, 4151, 4152, 4153, 4154, 4155, 4156, 4157,
				4158, 4159, 4160, 4161, 4162, 4163, 4164, 4165, 4166, 4167, 4168, 4169, 4170, 4171, 4172, 4172, 4173, 4174, 4175, 4176,
				4177, 4178, 4179, 4180, 4181, 4182, 4183, 4184, 4185, 4186, 4187, 4188, 4189, 4190, 4191, 4192, 4193, 4194, 4195, 4196,
				4197, 4198, 4199, 4200, 4201, 4202, 4203, 4204, 4205, 4206, 4207, 4208, 4209, 4210, 4211, 4212, 4213, 4214, 4215, 4216,
				4217, 4218, 4219, 4220, 4221, 4222, 4223, 4224, 4225, 4226, 4227, 4228, 4229, 4230, 4231, 4232, 4233, 4234, 4235, 4236,
				4237, 4238, 4239, 4240, 4241, 4242, 4243, 4244, 4245, 4246, 4247, 4248, 4249, 4250, 4251, 4252, 4253, 4254, 4255, 4256,
				4257, 4258, 4259, 4260, 4261, 4262, 4263, 4264, 4265, 4266, 4267, 4268, 4269, 4270, 4271, 4272, 4273, 4274, 4275, 4276,
				4277, 4278, 4279, 4280, 4281, 4282, 4283, 4284, 4285, 4286, 4287, 4288, 4289, 4290, 4291, 4292, 4293, 4294, 4295, 4296,
				4297, 4298, 4299, 4300, 4301, 4302, 4303, 4304, 4305, 4306, 4307, 4308, 4309, 4310, 4311, 4312, 4567, 4568, 4569, 4570,
				4571, 4572, 4573, 4574, 4575, 4576, 4577, 4578, 4579, 4580, 4581, 4582, 4583, 4584, 4585, 4586, 4587, 4588, 4589, 4590,
				4591, 4592, 4593, 4594, 4595, 4596, 4597, 4598, 4599, 4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607, 4608, 4609, 4610,
				4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626, 4627, 4628, 4629, 4630,
				4631, 4632, 4633, 4634, 4635, 4636, 4637, 4638, 4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646, 4647, 4648, 4649, 4650,
				4651, 4652, 4653, 4654, 4655, 4656, 4657, 4658, 4659, 4660, 4661, 4662, 4663, 4664, 4665, 4666, 4667, 4668, 4669, 4670,
				4671, 4672, 4673, 4674, 4675, 4676, 4677, 4678, 4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686, 4687, 4688, 4689, 4690,
				4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710,
				4711, 4712, 4713, 4714, 4715, 4716, 4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726, 4727, 4728, 4729, 4730,
				4731, 4732, 4733, 4734, 4735, 4736, 4737, 4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746, 4747, 4748, 4749, 4750,
				4751, 4752, 4753, 4754, 4755, 4756, 4757, 4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766, 4767, 4768, 4769, 4770,
				4771, 4772, 4773, 4774, 4775, 4776, 4777, 4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786, 4787, 4788, 4789, 4790,
				4791, 4792, 4793, 4794, 4795, 4796, 4797, 4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806, 4807, 4808, 4809, 4810,
				4811, 4812, 4813, 4814, 4815, 4816, 4817, 4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826, 4827, 4828, 4829, 4830,
				4831, 4832, 4833, 4834, 4835, 4836, 4837, 4838, 4839, 4840, 4841, 4842, 4843, 4844, 4845, 4846, 4847, 4848, 4849, 4850,
				4851, 4852, 4853, 4854, 4855, 4856, 4857, 4858, 4859, 4860, 4861, 4862, 4863, 4864, 4865, 4866, 4867, 4868, 4869, 4870,
				4871, 4872, 4873, 4874, 4875, 4876, 4877, 4878, 4879, 4880, 4881, 4882, 4883, 4884, 4885, 4886, 4887, 4888, 4889, 4890,
				4891, 4892, 4893, 4894, 4895, 4896, 4897, 4898, 4899, 4900, 4901, 4902, 4903, 4904, 4905, 4906, 4907, 4908, 4909, 4910,
				4911, 4912, 4913, 4914, 4915, 4916, 4917, 4918, 4919, 4920, 4921, 4922, 4923, 4924, 4925, 4926, 4927, 4928, 4929, 4930,
				4931, 4932, 4933, 4934, 4935, 4936, 4937, 4938, 4939, 4940, 4941, 4942, 4943, 4944, 4945, 4946, 4947, 4948, 4949, 4950,
				4951, 4952, 4953, 4954, 4955, 4956, 4957, 4958, 4959, 4960, 4961, 4962, 4963, 4964, 4965, 4966, 4967, 4968, 4969, 4970,
				4971, 4972, 4973, 4974, 4975, 4976, 4977, 4978, 4979, 4980, 4981, 4982, 4983, 4984, 4985, 4986, 4987, 4988, 4989, 4990,
				4991, 4992, 4993, 4994, 4995, 4996, 4997, 4998, 4999, 5000, 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010,
				5011, 5012, 5013, 5014, 5015, 5016, 5017, 5018, 5019, 5020, 5021, 5022, 5023, 5024, 5025, 5026, 5027, 5028, 5029, 5030,
				5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 5042, 5043, 5044, 5045, 5046, 5047, 5048, 5049, 5050,
				5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058, 5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5067, 5068, 5069, 5070,
				5071, 5072, 5073, 5074, 5075, 5076, 5077, 5078, 5079, 5080, 5081, 5082, 5083, 5084, 5085, 5086, 5087, 5088, 5089, 5090,
				5091, 5092, 5093, 5094, 5095, 5096, 5097, 5098, 5099, 5100, 5205, 5206, 5207, 5208, 5209, 5210, 5211, 5212, 5213, 5214,
				5215, 5216, 5217, 5218, 5219, 5220, 5221, 5222, 5223, 5224, 5225, 5226, 5227, 5228, 5229, 5230, 5231, 5232, 5233, 5234,
				5235, 5236, 5237, 5238, 5239, 5240, 5241, 5242, 5243, 5244, 5245, 5246, 5247, 5248, 5249, 5250, 5251, 5252, 5253, 5254,
				5255, 5256, 5257, 5258, 5259, 5260, 5261, 5262, 5263, 5264, 5265, 5266, 5267, 5268, 5269, 5270, 5271, 5272, 5273, 5274,
				5275, 5276, 5277, 5278, 5279, 5280, 5281, 5282, 5283, 5284, 5285, 5286, 5287, 5288, 5289, 5290, 5291, 5292, 5293, 5294,
				5295, 5296, 5297, 5298, 5299, 5300, 5301, 5302, 5303, 5304, 5305, 5306, 5307, 5308, 5309, 5310, 5311, 5312, 5313, 5314,
				5315, 5316, 5317, 5318, 5319, 5320, 5321, 5322, 5323, 5324, 5325, 5326, 5327, 5328, 5329, 5330, 5331, 5332, 5333, 5334,
				5335, 5336, 5337, 5338, 5339, 5340, 5341, 5342, 5343, 5344, 5345, 5346, 5347, 5348, 5349, 5350, 5351, 5352, 5353, 5354,
				5355, 5356, 5357, 5358, 5359, 5360, 5361, 5362, 5363, 5364, 5365, 5366, 5367, 5368, 5369, 5370, 5371, 5372, 5373, 5374,
				5375, 5376, 5377, 5378, 5379, 5380, 5381, 5382, 5383, 5384, 5385, 5386, 5387, 5388, 5389, 5390, 5391, 5392, 5393, 5394,
				5395, 5396, 5397, 5398, 5399, 5400, 5401, 5402, 5403, 5404, 5405, 5406, 5407, 5408, 5409, 5410, 5411, 5412, 5413, 5414,
				5415, 5416, 5417, 5418, 5419, 5420, 5421, 5422, 5423, 5424, 5425, 5426, 5427, 5428, 5429, 5430, 5431, 5432, 5433, 5434,
				5435, 5436, 5437, 5438, 5439, 5440, 5441, 5442, 5443, 5444, 5445, 5446, 5447, 5448, 5449, 5450, 5451, 5452, 5453, 5454,
				5455, 5456, 5457, 5458, 5459, 5460, 5461, 5462, 5463, 5464, 5465, 5467, 5468, 5469, 5470, 5471, 5472, 5473, 5474, 5475,
				5476, 5477, 5478, 5479, 5480, 5481, 5482, 5483, 5484, 5485, 5486, 5487, 5488, 5489, 5490, 5491, 5492, 5493, 5494, 5495,
				5496, 5497, 5498, 5499, 5500, 5501, 5502, 5503, 5504, 5505, 5506, 5507, 5508, 5509, 5510, 5511, 5512, 5513, 5514, 5515,
				5516, 5517, 5518, 5519, 5520, 5521, 5522, 5523, 5524, 5525, 5526, 5527, 5528, 5529, 5530, 5531, 5532, 5533, 5534, 5535,
				5536, 5537, 5538, 5539, 5540, 5541, 5542, 5543, 5544, 5545, 5546, 5547, 5548, 5549, 5550, 5823, 5824, 5825, 5826, 5827,
				5828, 5829, 5830, 5831, 5832, 5833, 5834, 5835, 5836, 5837, 5838, 5839, 5840, 5841, 5842, 5843, 5844, 5845, 5846, 5847,
				5848, 5849, 5850, 5851, 5852, 5853, 5854, 5855, 5856, 5857, 5858, 5859, 5860, 5861, 5862, 5863, 5864, 5865, 5866, 5867,
				5868, 5869, 5870, 5871, 5872, 5873, 5874, 5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882, 5883, 5884, 5885, 5886, 5887,
				5888, 5889, 5890, 5891, 5892, 5893, 5914, 5915, 5916, 5917, 5918, 5919, 5920, 5928, 5929, 5930, 5931, 5932, 5933, 5934,
				5935, 5936, 5937, 5938, 5939, 5940, 5941, 5942, 5943, 5944, 5945, 5946, 5947, 5948, 5949, 5950, 5951, 5952, 5953, 5954,
				5955, 5956, 5957, 5958, 5959, 5960, 5961, 5962, 5963, 5964, 5965, 5966, 5967, 5968, 5969, 5970, 5971, 5972, 5973, 5974,
				5975, 5976, 5977, 5978, 5979, 5980, 5981, 5982, 5983, 5984, 5985, 5986, 5987, 5988, 5989, 5990, 5991, 5992, 5993, 5994,
				5995, 5996, 5997, 5998, 5999, 6000, 6001, 6002, 6003, 6004, 6005, 6006, 6007, 6008, 6009, 6010, 6011, 6012, 6013, 6014,
				6015, 6016, 6017, 6018, 6019, 6020, 6021, 6022, 6023, 6024, 6025, 6026, 6027, 6028, 6029, 6030, 6031, 6032, 6033, 6034,
				6035, 6036, 6037, 6038, 6039, 6040, 6041, 6042, 6043, 6044, 6045, 6046, 6047, 6048, 6049, 6050, 6051, 6052, 6053, 6054,
				6055, 6056, 6057, 6058, 6059, 6060, 6061, 6062, 6063, 6064, 6065, 6066, 6067, 6068, 6069, 6070, 6071, 6072, 6073, 6074,
				6075, 6076, 6077, 6078, 6079, 6080, 6081, 6082, 6083, 6084, 6085, 6086, 6087, 6088, 6089, 6090, 6091, 6092, 6093, 6390,
				6391, 6392, 6393, 6394, 6395, 6396, 6397, 6398, 6399, 6400, 6401, 6402, 6403, 6404, 6405, 6406, 6407, 6408, 6409, 6410,
				6411, 6412, 6413, 6414, 6415, 6416, 6417, 6418, 6419, 6420, 6421, 6422, 6423, 6424, 6425, 6426, 6427, 6428, 6429, 6430,
				6431, 6432, 6433, 6434, 6435, 6436, 6437, 6438, 6439, 6440, 6441, 6442, 6443, 6444, 6445, 6446, 6447, 6448, 6449, 6450,
				6451, 6452, 6453, 6454, 6455, 6456, 6457, 6458, 6459, 6460, 6461, 6462, 6463, 6464, 6465, 6466, 6467, 6468, 6469, 6470,
				6471, 6472, 6473, 6474, 6475, 6476, 6477, 6478, 6479, 6480, 6481, 6482, 6483, 6484, 6485, 6486, 6487, 6488, 6489, 6490,
				6491, 6492, 6493, 6494, 6495, 6496, 6497, 6498, 6499, 6500, 6501, 6502, 6503, 6504, 6505, 6506, 6507, 6508, 6509, 6510,
				6511, 6512, 6513, 6774, 6775, 6776, 6968, 6969, 6970, 6971, 6972, 6973, 6974, 6975, 6976, 6977, 6978, 6979, 6980, 6981,
				6982, 6983, 6984, 6985, 6986, 6987, 6988, 6989, 6990, 6991, 6992, 6993, 6994, 6995, 6996, 6997, 8923, 8924, 8925, 8926,
				8927, 8928, 8929, 8930, 8931, 8932, 8933, 8934, 8935, 8936, 8937, 8938, 8939, 8940, 8941, 8942, 8943, 8944, 8945, 8946,
				8947, 8948, 8949, 8950, 8951, 9363, 9364, 9365, 9366, 9367, 9368, 9369, 9370, 9371, 9372, 9373, 9374, 9375, 9376, 9377,
				9378, 9379, 9380, 9381, 9382, 9383, 9384, 9385, 9501, 9502, 9503, 9504, 9505, 9506, 9507, 9508, 9509, 9510, 9511, 9513,
				9517, 9518, 9519, 9520, 9521, 9522, 9523, 9524, 9525, 9526, 9527, 9528, 9529, 9530, 9531, 9532, 9536, 9537, 9538
		};
		private readonly int[] yanArray = new int[2359]
		{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
				33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
				64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94,
				95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
				121, 122, 123, 124, 125, 126, sbyte.MaxValue, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142,
				143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167,
				168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192,
				193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217,
				218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242,
				243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, byte.MaxValue, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265,
				266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290,
				291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 1004, 1005, 1006, 1007, 1008, 1009,
				1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 1019, 1020, 1021, 1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029,
				1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049,
				1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1062, 1063, 1064, 1065, 1066, 1067, 1068, 1069,
				1070, 1071, 1072, 1073, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084, 1085, 1086, 1087, 1088, 1089,
				1090, 1091, 1092, 1093, 1094, 1095, 1096, 1097, 1098, 1099, 1100, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109,
				1110, 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129,
				1130, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149,
				1150, 1151, 1152, 1153, 1154, 1155, 1156, 1157, 1158, 1159, 1160, 1161, 1162, 1163, 1164, 1165, 1166, 1167, 1168, 1169,
				1170, 1171, 1172, 1173, 1174, 1175, 1176, 1177, 1178, 1179, 1180, 1181, 1182, 1183, 1184, 1185, 1186, 1187, 1188, 1189,
				1190, 1191, 1192, 1193, 1194, 1195, 1196, 1197, 1198, 1199, 1200, 1201, 1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209,
				1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223, 1224, 1225, 1226, 1227, 1228, 1229,
				1230, 1231, 1232, 1233, 1234, 1235, 1236, 1237, 1238, 1239, 1240, 1241, 1242, 1243, 1244, 1245, 1246, 1247, 1248, 1249,
				1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1259, 1260, 1261, 1262, 1263, 1264, 1265, 1266, 1267, 1268, 1269,
				1270, 1271, 1272, 1273, 1274, 1275, 1276, 1277, 1278, 1279, 1280, 1281, 1282, 1283, 1284, 1285, 1286, 1287, 1288, 1289,
				1290, 1291, 1292, 1293, 1294, 1295, 1296, 1297, 1298, 1299, 1300, 1301, 1302, 1303, 5210, 5211, 5212, 5213, 5214, 5215,
				5216, 5217, 5218, 5219, 5220, 5221, 5222, 5223, 5224, 5225, 5226, 5227, 5228, 5229, 5230, 5231, 5232, 5233, 5234, 5235,
				5236, 5237, 5238, 5239, 5240, 5241, 5242, 5243, 5244, 5245, 5246, 5247, 5248, 5249, 5250, 5251, 5252, 5253, 5254, 5255,
				5256, 5257, 5258, 5259, 5260, 5261, 5262, 5263, 5264, 5265, 5266, 5267, 5268, 5269, 5270, 5271, 5272, 5273, 5274, 5275,
				5276, 5277, 5278, 5279, 5280, 5281, 5282, 5283, 5284, 5285, 5286, 5287, 5288, 5289, 5290, 5291, 5292, 5293, 5294, 5295,
				5296, 5297, 5298, 5299, 5300, 5301, 5302, 5303, 5304, 5305, 5306, 5307, 5308, 5309, 5310, 5311, 5312, 5313, 5314, 5315,
				5316, 5317, 5318, 5319, 5320, 5321, 5322, 5323, 5324, 5325, 5326, 5327, 5328, 5329, 5330, 5331, 5332, 5333, 5334, 5335,
				5336, 5337, 5338, 5339, 5340, 5341, 5342, 5343, 5344, 5345, 5346, 5347, 5348, 5349, 5350, 7405, 7406, 7407, 7408, 7409,
				7410, 7411, 7412, 7413, 7414, 7415, 7416, 7417, 7418, 7419, 7420, 7421, 7422, 7423, 7424, 7425, 7426, 7427, 7428, 7429,
				7430, 7431, 7432, 7433, 7434, 7435, 7436, 7437, 7438, 7439, 7440, 7441, 7442, 7443, 7444, 7445, 7446, 7447, 7448, 7449,
				7450, 7451, 7452, 7453, 7454, 7455, 7456, 7457, 7458, 7459, 7460, 7461, 7462, 7463, 7464, 7465, 7466, 7467, 7468, 7469,
				7470, 7471, 7472, 7473, 7474, 7475, 7476, 7477, 7478, 7479, 7480, 7481, 7482, 7483, 7484, 7485, 7486, 7487, 7488, 7489,
				7490, 7491, 7492, 7493, 7494, 7495, 7496, 7497, 7498, 7499, 7500, 7501, 7502, 7503, 7504, 7505, 7506, 7507, 7508, 7509,
				7510, 7511, 7512, 7513, 7514, 7515, 7516, 7517, 7518, 7519, 7520, 7521, 7522, 7523, 7524, 7525, 7526, 7527, 7528, 7529,
				7530, 7531, 7532, 7533, 7534, 7535, 7536, 7537, 7538, 7539, 7540, 7541, 7542, 7543, 7544, 7545, 7546, 7547, 7548, 7549,
				7550, 7551, 7552, 7553, 7554, 7555, 7556, 7557, 7558, 7559, 7560, 7561, 7562, 7563, 7564, 7565, 7566, 7567, 7568, 7569,
				7570, 7571, 7572, 7573, 7574, 7575, 7576, 7577, 7578, 7579, 7580, 7581, 7582, 7583, 7584, 7585, 7586, 7587, 7588, 7589,
				7590, 7591, 7592, 7593, 7594, 7595, 7596, 7597, 7598, 7599, 7600, 7601, 7602, 7603, 7604, 7605, 7606, 7607, 7608, 7609,
				7610, 7611, 7612, 7613, 7614, 7615, 7616, 7617, 7618, 7619, 7620, 7621, 7622, 7623, 7624, 7625, 7626, 7627, 7628, 7629,
				7630, 7631, 7632, 7633, 7634, 7635, 7636, 7637, 7638, 7639, 7640, 7641, 7642, 7643, 7644, 7645, 7646, 7647, 7648, 7649,
				7650, 7651, 7652, 7653, 7654, 7655, 7656, 7657, 7658, 7659, 7660, 7661, 7662, 7663, 7664, 7665, 7666, 7667, 7668, 7669,
				7670, 7671, 7672, 7673, 7674, 7675, 7676, 7677, 7678, 7679, 7680, 7681, 7682, 7683, 7684, 7685, 7686, 7687, 7688, 7689,
				7690, 7691, 7692, 7693, 7694, 7695, 7696, 7697, 7698, 7699, 7700, 7701, 7702, 7703, 7704, 7705, 7706, 7707, 7708, 7709,
				7710, 7711, 7712, 7713, 7714, 7715, 7716, 7717, 7718, 7719, 7720, 7721, 7722, 7723, 7724, 7725, 7726, 7727, 7728, 7729,
				7730, 7731, 7732, 7733, 7734, 7735, 7736, 7737, 7738, 7739, 7740, 7741, 7742, 7743, 7744, 7745, 7746, 7747, 7748, 7749,
				7750, 7751, 7752, 7753, 7754, 7755, 7756, 7757, 7758, 7759, 7760, 7761, 7762, 7763, 7764, 7765, 7766, 7767, 7768, 7769,
				7770, 7771, 7772, 7773, 7774, 7775, 7776, 7777, 7778, 7779, 7780, 7781, 7782, 7783, 7784, 7785, 7786, 7787, 7788, 7789,
				7790, 7791, 7792, 7793, 7794, 7795, 7796, 7797, 7798, 7799, 7800, 7801, 7802, 7803, 7804, 7805, 7806, 7807, 7808, 7809,
				7810, 7811, 7812, 7813, 7814, 7815, 7816, 7817, 7818, 7819, 7820, 7821, 7822, 7823, 7824, 7825, 7826, 7827, 7828, 7829,
				7830, 7831, 7832, 7833, 7834, 7835, 7836, 7837, 7838, 7839, 7840, 7841, 7842, 7843, 7844, 7845, 7846, 7847, 7848, 7849,
				7850, 7851, 7852, 7853, 7854, 7855, 7856, 7857, 7858, 7859, 7860, 7861, 7862, 7863, 7864, 7865, 7866, 7867, 7868, 7869,
				7870, 7871, 7872, 7873, 7874, 7875, 7876, 7877, 7878, 7879, 7880, 7881, 7882, 7883, 7884, 7885, 7886, 7887, 7888, 7889,
				7890, 7891, 7892, 7893, 7894, 7895, 7896, 7897, 7898, 7899, 7900, 7901, 7902, 7903, 7904, 7905, 7906, 7907, 7908, 7909,
				7910, 7911, 7912, 7913, 7914, 7915, 7916, 7917, 7918, 7919, 7920, 7921, 7922, 7923, 7924, 7925, 7926, 7927, 7928, 7929,
				7930, 7931, 7932, 7933, 7934, 7935, 7936, 7937, 7938, 7939, 7940, 7941, 7942, 7943, 7944, 7945, 7946, 7947, 7948, 7949,
				7950, 7951, 7952, 7953, 7954, 7955, 7956, 7957, 7958, 7959, 7960, 7961, 7962, 7963, 7964, 7965, 7966, 7967, 7968, 7969,
				7970, 7971, 7972, 7973, 7974, 7975, 7976, 7977, 7978, 7979, 7980, 7981, 7982, 7983, 7984, 7985, 7986, 7987, 7988, 7989,
				7990, 7991, 7992, 7993, 7994, 7995, 7996, 7997, 7998, 7999, 8000, 8001, 8002, 8003, 8004, 8005, 8006, 8007, 8008, 8009,
				8010, 11333, 11334, 11335, 11336, 11337, 11338, 11339, 11340, 11341, 11342, 11343, 11344, 11345, 11346, 11347, 11348, 11349,
				11350, 11351, 11352, 11353, 11354, 11355, 11356, 11357, 11358, 11359, 11360, 11361, 11362, 11363, 11364, 11365, 11366,
				11367, 11368, 11369, 11370, 11371, 11372, 11373, 11374, 11375, 11376, 11377, 11378, 11379, 11380, 11381, 11382, 11383,
				11384, 11385, 11386, 11387, 11388, 11389, 11390, 11391, 11392, 11393, 11394, 11395, 11396, 11397, 11398, 11399, 11400,
				11401, 11402, 11403, 11404, 11405, 11406, 11407, 11408, 11409, 11410, 11411, 11412, 11413, 11414, 11415, 11416, 11417,
				11418, 11419, 11420, 11421, 11422, 11423, 11424, 11425, 11426, 11427, 11428, 11429, 11430, 11431, 11432, 11433, 11434,
				11435, 11436, 11437, 11438, 11439, 11440, 11441, 11442, 11443, 11444, 11445, 11446, 11447, 11448, 11449, 11450, 11451,
				11452, 11453, 11454, 11455, 11456, 11457, 11458, 11459, 11460, 11461, 11462, 11463, 11464, 11465, 11466, 11467, 11468,
				11469, 11470, 11471, 11472, 11473, 11474, 11475, 11476, 11477, 11478, 11479, 11480, 11481, 11482, 11483, 11484, 11485,
				11486, 11487, 11488, 11489, 11490, 11491, 11492, 11493, 11494, 11495, 11496, 11497, 11498, 11499, 11500, 11501, 11502,
				11503, 11504, 11505, 11506, 11507, 11508, 11509, 11510, 11511, 11512, 11513, 11514, 11515, 11516, 11517, 11518, 11519,
				11520, 11521, 11522, 11523, 11524, 11525, 11526, 11527, 11528, 11529, 11530, 11531, 11532, 11533, 11534, 11535, 11536,
				11537, 11538, 11539, 11540, 11541, 11542, 11543, 11544, 11545, 11546, 11547, 11548, 11549, 11550, 11551, 11552, 11553,
				11554, 11555, 11556, 11557, 11558, 11559, 11560, 11561, 11562, 11563, 11564, 11565, 11566, 11567, 11568, 11569, 11570,
				11571, 11572, 11573, 11574, 11575, 11576, 11577, 11578, 11579, 11580, 11581, 11582, 11583, 11584, 11585, 11586, 11587,
				11588, 11589, 11590, 11591, 11592, 11593, 11594, 11595, 11596, 11597, 11598, 11599, 11600, 11601, 11602, 11603, 11604,
				11605, 11606, 11607, 11608, 11609, 11610, 11611, 11612, 11613, 11614, 11615, 11616, 11617, 11618, 11619, 11989, 11990,
				11991, 11992, 11993, 11994, 11995, 11996, 11997, 11998, 11999, 12000, 12001, 12002, 12003, 12004, 12005, 12006, 12007,
				12008, 12009, 12010, 12011, 12012, 12013, 12014, 12015, 12016, 12017, 12018, 12019, 12020, 12021, 12022, 12023, 12024,
				12025, 12026, 12027, 12028, 12029, 12030, 12031, 12032, 12033, 12034, 12035, 12036, 12037, 12038, 12039, 12040, 12041,
				12042, 12043, 12044, 12045, 12046, 12047, 12048, 12049, 12050, 12051, 12052, 12053, 12054, 12055, 12056, 12057, 12058,
				12059, 12060, 12061, 12062, 12063, 12064, 12065, 12066, 12067, 12068, 12069, 12070, 12071, 12072, 12073, 12078, 12079,
				12080, 12081, 12082, 12083, 12084, 12085, 12086, 12087, 12088, 12089, 12090, 12091, 12092, 12093, 12094, 12095, 12096,
				12097, 12098, 12099, 12100, 12101, 12102, 12103, 12104, 12105, 12106, 12107, 12108, 12109, 12110, 12111, 12112, 12113,
				12114, 12115, 12116, 12117, 12118, 12119, 12120, 12121, 12122, 12123, 12124, 12125, 12126, 12127, 12128, 12129, 12130,
				12131, 12132, 12133, 12134, 12135, 12136, 12137, 12138, 12139, 12140, 12141, 12142, 12143, 12144, 12145, 12146, 12147,
				12148, 12149, 12150, 12151, 12152, 12153, 12154, 12155, 12156, 12157, 12158, 12159, 12160, 12161, 12162, 12163, 12164,
				12165, 12166, 12167, 12168, 12169, 12170, 12171, 12172, 12173, 12174, 12175, 12176, 12177, 12178, 12179, 12180, 12181,
				12182, 12183, 12184, 12185, 12186, 12187, 12188, 12189, 12190, 12191, 12192, 12193, 12194, 12195, 12196, 12197, 12198,
				12199, 12200, 12201, 12202, 12203, 12204, 12205, 12206, 12207, 12208, 12209, 12210, 12211, 12212, 12213, 12214, 12215,
				12216, 12217, 12218, 12219, 12220, 12221, 12222, 12223, 12224, 12225, 12226, 12227, 12228, 12229, 12230, 12231, 12232,
				12233, 12234, 12235, 12236, 12237, 12238, 12239, 12240, 12241, 12242, 12243, 12244, 12245, 12246, 12247, 12248, 12249,
				12250, 12251, 12252, 12253, 12254, 12255, 12256, 12257, 12258, 12259, 12260, 12261, 12262, 12263, 12264, 12265, 12266,
				12267, 12268, 12269, 12270, 12271, 12272, 12273, 12274, 12275, 12276, 12277, 12278, 12289, 12290, 12291, 12292, 12293,
				12294, 12295, 12296, 12297, 12298, 12299, 12300, 12301, 12302, 12303, 12304, 12305, 12306, 12307, 12308, 12309, 12310,
				12311, 12312, 12313, 12315, 12316, 12317, 12318, 12319, 12320, 12321, 12322, 12323, 12324, 12325, 12326, 12327, 12328,
				12329, 12330, 12331, 12332, 12333, 12334, 12335, 12336, 12337, 12338, 12339, 12340, 12341, 12342, 12343, 12344, 12345,
				12346, 12347, 12348, 12349, 12350, 12351, 12352, 12353, 12354, 12355, 12356, 12357, 12358, 12359, 12360, 12361, 12362,
				12363, 12364, 12365, 12366, 12367, 12368, 12369, 12370, 12371, 12372, 12373, 12374, 12375, 12376, 12377, 12378, 12379,
				12380, 12381, 12382, 12383, 12384, 12385, 12386, 12387, 12388, 12389, 12390, 12391, 12392, 12393, 12394, 12395, 12396,
				12397, 12398, 12399, 12400, 12401, 12402, 12403, 12404, 12405, 12406, 12407, 12408, 12409, 12410, 12633, 12634, 12635,
				12636, 12637, 12638, 12639, 12640, 12641, 12642, 12643, 12644, 12645, 12646, 12647, 12648, 12649, 12650, 12651, 12652,
				12653, 12654, 12655, 12656, 12657, 12658, 12659, 12660, 12661, 12662, 12663, 12664, 12665, 12666, 12667, 12668, 12669,
				12670, 12671, 12672, 12673, 12674, 12675, 12676, 12677, 12678, 12679, 12680, 12681, 12682, 12683, 12684, 12685, 12686,
				12687, 12688, 12689, 12690, 12691, 12692, 12693, 12694, 12695, 12696, 12697, 12698, 12699, 12700, 12701, 12702, 12703,
				12704, 12705, 12706, 12707, 12708, 12709, 12710, 12711, 12712, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720,
				12721, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729, 12730, 12731, 12732, 12733, 12734, 12735, 12736, 12737,
				12738, 12739, 12740, 12741, 12742, 12743, 12744, 12745, 12746, 12747, 12748, 12749, 12750, 12751, 12752, 12753, 12754,
				12755, 12756, 12757, 12758, 12759, 12760, 12761, 12762, 12763, 12764, 12765, 12766, 12767, 12768, 12769, 12770, 12771,
				12772, 12773, 12774, 12775, 12776, 12777, 12778, 12779, 12780, 12781, 12782, 12783, 12784, 12785, 12786, 12787, 12788,
				12789, 12790, 12791, 12792, 12793, 12794, 12795, 12796, 12797, 12798, 12799, 12800, 12801, 12802, 12803, 12804, 12805,
				12806, 12807, 12808, 12809, 12810, 12811, 12812, 12813, 12814, 12815, 12816, 12817, 12818, 12819, 12820, 12821, 12822,
				12823, 12824, 12825, 12826, 12827, 12828, 12829, 12830, 12831, 12832, 12833, 12834, 12835, 12836, 12837, 12838, 12839,
				12840, 12841, 12842, 12843, 12844, 12845, 12846, 12847, 12848, 12849, 12850, 12851, 12852, 12853, 12855, 12856, 12857,
				12858, 12859, 12860, 12861, 12862, 12863, 12864, 12865, 12866, 12867, 12868, 12869, 12870, 12871, 12872, 12873, 12874,
				12875, 12876, 12877, 12878, 12879, 12880, 12881, 12882, 12883, 12884, 12885, 12886, 12887, 12888, 12889, 12890, 12891,
				12892, 12893, 12894, 12895, 12896, 12897, 12898, 12899, 12900, 12901, 12902, 12903, 12904, 12905, 12906, 12907, 12908,
				12909, 12910, 12911, 12912, 12913, 12914, 12915, 12916, 12917, 12918, 12919, 12920, 12921, 12922, 12923, 12924, 12925,
				12926, 12927, 12928, 12929, 12930, 12931, 12932, 12933, 12934, 12935, 12936, 12937, 12938, 12939, 12940, 12941, 12942, 12943
		};
		private readonly int[] h0Array = new int[5371]
		{
				53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
				84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 117, 118, 119,
				120, 121, 122, 123, 124, 125, 126, sbyte.MaxValue, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141,
				142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166,
				167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191,
				192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216,
				217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241,
				242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, byte.MaxValue, 256, 257, 258, 259, 260, 261, 262, 263, 264,
				265, 266, 267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289,
				290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314,
				315, 316, 317, 318, 319, 320, 321, 322, 323, 324, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 343, 344, 345, 346,
				347, 348, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374,
				375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399,
				400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424,
				425, 426, 427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449,
				450, 451, 452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474,
				475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499,
				500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524,
				525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541, 542, 543, 544, 545, 546, 547, 548, 549,
				550, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574,
				575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 610, 611,
				612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636,
				637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661,
				662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686,
				687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711,
				712, 713, 714, 715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736,
				737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761,
				762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786,
				787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800, 801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811,
				812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827, 828, 829, 830, 831, 832, 833, 834, 835, 836,
				837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861,
				862, 863, 864, 865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875, 876, 877, 878, 879, 880, 881, 882, 883, 884, 885, 886,
				887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 898, 899, 900, 901, 902, 903, 904, 905, 906, 907, 908, 909, 910, 911,
				912, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 930, 931, 932, 933, 954, 955, 956,
				957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 970, 971, 972, 973, 974, 975, 976, 977, 978, 979, 980, 981,
				982, 983, 984, 985, 986, 987, 988, 989, 990, 991, 992, 993, 994, 995, 996, 997, 998, 999, 1000, 1001, 1002, 1003, 1004,
				1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 1019, 1020, 1021, 1022, 1023, 1024,
				1025, 1026, 1027, 1028, 1029, 1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 1040, 1041, 1042, 1043, 1044,
				1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1062, 1063, 1064,
				1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084,
				1085, 1086, 1087, 1088, 1089, 1090, 1091, 1092, 1093, 1094, 1095, 1096, 1097, 1098, 1099, 1100, 1101, 1102, 1103, 1104,
				1105, 1106, 1107, 1108, 1109, 1110, 1111, 1112, 1113, 1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124,
				1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144,
				1145, 1146, 1147, 1148, 1149, 1150, 1151, 1152, 1153, 1154, 1155, 1156, 1157, 1158, 1159, 1160, 1161, 1162, 1163, 1164,
				1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172, 1173, 1174, 1175, 1176, 1177, 1178, 1179, 1180, 1181, 1182, 1183, 1184,
				1185, 1186, 1187, 1188, 1189, 1190, 1191, 1192, 1193, 1194, 1195, 1196, 1197, 1198, 1199, 1200, 1201, 1202, 1203, 1204,
				1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223, 1224,
				1225, 1226, 1227, 1228, 1229, 1230, 1231, 1232, 1233, 1234, 1235, 1236, 1237, 1238, 1239, 1240, 1241, 1242, 1243, 1244,
				1245, 1246, 1247, 1248, 1249, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1259, 1260, 1261, 1262, 1263, 1264,
				1265, 1266, 1267, 1268, 1269, 1270, 1271, 1272, 1273, 1274, 1275, 1276, 1277, 1278, 1279, 1280, 1281, 1284, 1285, 1286,
				1287, 1288, 1289, 1290, 1291, 1292, 1293, 1294, 1295, 1296, 1297, 1298, 1299, 1300, 1301, 1302, 1303, 1304, 1305, 1306,
				1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316, 1317, 1318, 1319, 1320, 1321, 1322, 1323, 1324, 1325, 1326,
				1327, 1328, 1329, 1330, 1331, 1332, 1333, 1334, 1335, 1336, 1337, 1338, 1339, 1340, 1341, 1342, 1343, 1344, 1345, 1346,
				1347, 1348, 1349, 1350, 1351, 1352, 1353, 1354, 1355, 1356, 1357, 1358, 1359, 1360, 1361, 1362, 1363, 1364, 1365, 1366,
				1367, 1368, 1369, 1370, 1371, 1372, 1373, 1374, 1375, 1376, 1377, 1378, 1379, 1380, 1381, 1701, 1702, 1703, 1704, 1705,
				1706, 1707, 1708, 1709, 1710, 1711, 1712, 1713, 1714, 1715, 1716, 1717, 1718, 1719, 1720, 1721, 1722, 1723, 1724, 1725,
				1726, 1727, 1728, 1729, 1730, 1731, 1732, 1733, 1734, 1735, 1736, 1737, 1738, 1739, 1740, 1741, 1742, 1743, 1744, 1745,
				1746, 1747, 1748, 1749, 1750, 1751, 1752, 1753, 1754, 1755, 1756, 1757, 1758, 1759, 1760, 1761, 1762, 1764, 1765, 1766,
				1767, 1768, 1769, 1770, 1771, 1772, 1773, 1774, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782, 1783, 1784, 1785, 1790,
				1791, 1792, 1793, 1794, 1795, 1796, 1797, 1798, 1799, 1800, 1801, 1802, 1803, 1804, 1805, 1808, 1809, 1810, 1811, 1812,
				1813, 1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823, 1824, 1825, 1826, 1827, 1828, 1829, 1830, 1831, 1832,
				1833, 1834, 1835, 1836, 1837, 1838, 1839, 1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849, 1850, 1851, 1852,
				1853, 1854, 1855, 1856, 1857, 1858, 1859, 1860, 1861, 1862, 1863, 1864, 1865, 1866, 1867, 1868, 1869, 1870, 1871, 1872,
				1873, 1874, 1875, 1876, 2296, 2297, 2298, 2299, 2300, 2301, 2302, 2303, 2304, 2305, 2306, 2307, 2308, 2309, 2310, 2311,
				2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2326, 2327, 2328, 2329, 2330, 2331,
				2332, 2333, 2334, 2335, 2336, 2337, 2338, 2339, 2340, 2341, 2342, 2343, 2344, 2345, 2346, 2347, 2348, 2349, 2350, 2351,
				2352, 2353, 2354, 2355, 2356, 2357, 2358, 2359, 2360, 2361, 2362, 2363, 2364, 2365, 2366, 2367, 2368, 2369, 2370, 2371,
				2372, 2373, 2374, 2375, 2376, 2377, 2378, 2379, 2380, 2381, 2382, 2383, 2384, 2385, 2386, 2387, 2388, 2389, 2390, 2391,
				2392, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2402, 2403, 2404, 2405, 2406, 2407, 2408, 2409, 2410, 2411,
				2412, 2413, 2414, 2415, 2416, 2417, 2418, 2419, 2420, 2421, 2422, 2423, 2424, 2425, 2426, 2427, 2428, 2429, 2430, 2431,
				2432, 2433, 2434, 2435, 2436, 2437, 2438, 2439, 2440, 2441, 2442, 2443, 2444, 2445, 2446, 2447, 2448, 2449, 2450, 2451,
				2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459, 2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469, 2470, 2471,
				2472, 2473, 2474, 2475, 2476, 2477, 2478, 2479, 2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 2489, 2490, 2491,
				2492, 2493, 2494, 2495, 2496, 2497, 2498, 2499, 2500, 2501, 2502, 2503, 2504, 2505, 2506, 2507, 2508, 2509, 2510, 2511,
				2512, 2513, 2514, 2515, 2516, 2517, 2518, 2519, 2520, 2521, 2522, 2523, 2524, 2525, 2526, 2527, 2528, 2529, 2530, 2531,
				2532, 2533, 2534, 2535, 2536, 2537, 2538, 2539, 2540, 2541, 2542, 2543, 2544, 2545, 2546, 2547, 2548, 2549, 2550, 2551,
				2552, 2553, 2554, 2555, 2556, 2557, 2558, 2559, 2560, 2561, 2562, 2563, 2564, 2565, 2566, 2567, 2568, 2569, 2570, 2571,
				2572, 2573, 2574, 2575, 2576, 2577, 2578, 2579, 2580, 2581, 2582, 2583, 2584, 2585, 2586, 2587, 2588, 2589, 2590, 2591,
				2592, 2593, 2594, 2595, 2596, 2597, 2598, 2599, 2600, 2601, 2602, 2603, 2604, 2605, 2606, 2607, 2608, 2609, 2610, 2611,
				2612, 2613, 2614, 2615, 2616, 2617, 2618, 2619, 2620, 2621, 2622, 2623, 2624, 2625, 2626, 2627, 2628, 2629, 2630, 2631,
				2632, 2633, 2634, 2635, 2636, 2637, 2638, 2639, 2640, 2641, 2642, 2643, 2644, 2645, 2646, 2647, 2648, 2649, 2650, 2651,
				2652, 2653, 2654, 2655, 2656, 2657, 2658, 2659, 2660, 2661, 2662, 2663, 2664, 2665, 2666, 2667, 2668, 2669, 2670, 2671,
				2672, 2673, 2674, 2675, 2676, 2677, 2678, 2679, 2680, 2681, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2689, 2690, 2691,
				2692, 2693, 2694, 2695, 2696, 2697, 2698, 2699, 2700, 2701, 2702, 2703, 2704, 2705, 2706, 2707, 2708, 2709, 2710, 2711,
				2712, 2713, 2714, 2715, 2716, 2717, 2718, 2719, 2720, 2721, 2722, 2723, 2724, 2725, 2726, 2727, 2728, 2729, 2730, 2731,
				2732, 2733, 2734, 2735, 2736, 2737, 2738, 2739, 2740, 2741, 2742, 2743, 2744, 2745, 2746, 2747, 2748, 2749, 2750, 2751,
				2752, 2753, 2754, 2755, 2756, 2757, 2758, 2759, 2760, 2761, 2762, 2763, 2764, 2765, 2766, 2767, 2768, 2769, 2770, 2771,
				2772, 2773, 2774, 2775, 2776, 2778, 2779, 2780, 2781, 2782, 2783, 2784, 2785, 2786, 2787, 2788, 2789, 2790, 2791, 2792,
				2793, 2794, 2797, 2799, 2800, 2803, 2804, 2805, 2806, 2807, 2808, 2809, 2810, 2811, 2812, 2813, 2814, 2815, 2816, 2817,
				2818, 2819, 2820, 2821, 2822, 2823, 2824, 2825, 2826, 2827, 2828, 2829, 2830, 2831, 2832, 2833, 2834, 2835, 2836, 2837,
				2838, 2839, 2840, 2841, 2842, 2843, 2844, 2845, 2846, 2847, 2848, 2849, 2850, 2851, 2852, 2853, 2854, 2855, 2856, 2857,
				2858, 2859, 2860, 2861, 2862, 2863, 2864, 2865, 2866, 2867, 2868, 2869, 2870, 2871, 2872, 2873, 2874, 2875, 2876, 2877,
				2878, 2879, 2880, 2881, 2882, 2883, 2884, 2885, 2886, 2887, 2888, 2889, 2890, 2891, 2892, 2893, 2894, 2895, 2896, 2897,
				2898, 2899, 2900, 2901, 2902, 2903, 2904, 2905, 2906, 2907, 2908, 2909, 2910, 2935, 2936, 2937, 2938, 2939, 2940, 2941,
				2942, 2943, 2944, 2945, 2946, 2947, 2948, 2949, 2950, 2951, 2952, 2953, 2954, 2955, 2956, 2957, 2958, 2959, 2960, 2961,
				2962, 2963, 2964, 2965, 2966, 2967, 2968, 2969, 2970, 2971, 2972, 2973, 2974, 2975, 2976, 2977, 2978, 2979, 2980, 2981,
				2982, 2983, 2984, 2985, 2986, 2987, 2988, 2989, 2990, 2991, 2992, 2993, 2994, 2995, 2996, 2997, 2998, 2999, 3000, 3001,
				3002, 3003, 3004, 3005, 3006, 3007, 3008, 3009, 3010, 3011, 3012, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021,
				3022, 3023, 3024, 3025, 3026, 3027, 3028, 3029, 3030, 3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041,
				3042, 3043, 3044, 3045, 3046, 3047, 3048, 3049, 3050, 3051, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061,
				3062, 3063, 3064, 3065, 3066, 3067, 3068, 3069, 3070, 3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081,
				3082, 3083, 3084, 3085, 3086, 3087, 3088, 3089, 3090, 3091, 3092, 3093, 3094, 3095, 3096, 3097, 3098, 3099, 3100, 3101,
				3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109, 3110, 3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121,
				3122, 3123, 3124, 3125, 3126, 3127, 3128, 3129, 3130, 3131, 3132, 3133, 3134, 3135, 3136, 3137, 3138, 3139, 3140, 3141,
				3142, 3143, 3144, 3145, 3146, 3147, 3148, 3149, 3150, 3151, 3152, 3153, 3154, 3155, 3156, 3157, 3158, 3159, 3160, 3161,
				3162, 3163, 3164, 3165, 3166, 3167, 3168, 3169, 3170, 3171, 3172, 3173, 3174, 3175, 3176, 3177, 3178, 3179, 3180, 3181,
				3182, 3183, 3184, 3185, 3186, 3187, 3188, 3189, 3190, 3191, 3192, 3193, 3194, 3195, 3196, 3197, 3198, 3199, 3200, 3201,
				3202, 3203, 3204, 3205, 3206, 3207, 3208, 3209, 3210, 3211, 3212, 3213, 3214, 3215, 3216, 3217, 3218, 3219, 3220, 3221,
				3222, 3223, 3224, 3225, 3226, 3227, 3228, 3229, 3230, 3231, 3232, 3233, 3234, 3235, 3236, 3237, 3238, 3239, 3240, 3241,
				3242, 3243, 3244, 3245, 3246, 3247, 3248, 3249, 3250, 3251, 3252, 3253, 3254, 3255, 3256, 3257, 3258, 3259, 3260, 3261,
				3262, 3263, 3264, 3265, 3266, 3267, 3268, 3269, 3270, 3271, 3272, 3273, 3274, 3275, 3276, 3277, 3278, 3279, 3280, 3281,
				3282, 3283, 3284, 3285, 3286, 3287, 3288, 3289, 3290, 3291, 3292, 3293, 3294, 3295, 3296, 3297, 3298, 3299, 3300, 3301,
				3302, 3303, 3304, 3305, 3306, 3315, 3316, 3317, 3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327, 3328, 3329,
				3330, 3331, 3332, 3333, 3334, 3335, 3336, 3337, 3338, 3339, 3340, 3341, 3342, 3343, 3344, 3345, 3346, 3347, 3348, 3349,
				3350, 3351, 3352, 3353, 3354, 3355, 3356, 3357, 3358, 3363, 3364, 3365, 3366, 3367, 3368, 3369, 3370, 3371, 3372, 3373,
				3374, 3375, 3376, 3377, 3378, 3379, 3380, 3381, 3382, 3383, 3384, 3385, 3386, 3387, 3388, 3389, 3390, 3391, 3392, 3393,
				3394, 3395, 3396, 3397, 3398, 3399, 3400, 3401, 3402, 3403, 3404, 3405, 3406, 3415, 3416, 3417, 3418, 3419, 3420, 3421,
				3422, 3423, 3424, 3425, 3426, 3427, 3428, 3429, 3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438, 3439, 3440, 3441,
				3442, 3443, 3444, 3445, 3446, 3447, 3448, 3449, 3450, 3451, 3452, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3460, 3461,
				3462, 3463, 3464, 3465, 3466, 3467, 3468, 3469, 3470, 3471, 3472, 3473, 3474, 3475, 3476, 3477, 3478, 3479, 3480, 3481,
				3482, 3483, 3484, 3485, 3486, 3487, 3488, 3489, 3490, 3491, 3492, 3493, 3494, 3495, 3496, 3497, 3498, 3499, 3500, 3501,
				3502, 3503, 3504, 3505, 3506, 3507, 3508, 3509, 3510, 3511, 3512, 3513, 3514, 3515, 3516, 3517, 3518, 3519, 3520, 3521,
				3522, 3523, 3524, 3525, 3526, 3527, 3528, 3529, 3530, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541,
				3542, 3543, 3544, 3545, 3546, 3547, 3548, 3549, 3550, 3551, 3552, 3553, 3554, 3555, 3556, 3558, 3559, 3560, 3561, 3562,
				3563, 3564, 3565, 3566, 3567, 3568, 3570, 3571, 3572, 3573, 3574, 3575, 3576, 3577, 3578, 3579, 3580, 3581, 3582, 3583,
				3584, 3585, 3586, 3587, 3588, 3589, 3590, 3591, 3592, 3593, 3594, 3595, 3596, 3597, 3598, 3599, 3600, 3601, 3602, 3603,
				3604, 3605, 3606, 3607, 3608, 3609, 3610, 3611, 3612, 3613, 3614, 3615, 3616, 3617, 3618, 3619, 3620, 3621, 3622, 3623,
				3624, 3625, 3626, 3627, 3628, 3629, 3630, 3631, 3632, 3633, 3634, 3635, 3636, 3637, 3638, 3639, 3640, 3641, 3642, 3643,
				3644, 3645, 3646, 3647, 3648, 3649, 3650, 3651, 3652, 3653, 3654, 3655, 3656, 3657, 3658, 3659, 3660, 3661, 3662, 3663,
				3664, 3665, 3666, 3667, 3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683,
				3684, 3685, 3686, 3687, 3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3702, 3703,
				3704, 3705, 3706, 3707, 3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3717, 3718, 3719, 3720, 3721, 3722, 3723,
				3724, 3725, 3726, 3727, 3728, 3729, 3730, 3731, 3732, 3733, 3734, 3735, 3736, 3737, 3738, 3739, 3740, 3741, 3742, 3743,
				3744, 3745, 3746, 3747, 3748, 3749, 3750, 3751, 3752, 3753, 3754, 3755, 3756, 3757, 3758, 3759, 3760, 3761, 3762, 3763,
				3764, 3765, 3766, 3767, 3768, 3769, 3770, 3771, 3772, 3773, 3774, 3775, 3776, 3777, 3778, 3779, 3780, 3781, 3782, 3783,
				3784, 3785, 3786, 3787, 3788, 3789, 3790, 3791, 3792, 3793, 3794, 3795, 3796, 3797, 3798, 3799, 3800, 3801, 3802, 3803,
				3804, 3805, 3806, 3807, 3808, 3809, 3810, 3811, 3812, 3813, 3814, 3815, 3816, 3817, 3818, 3819, 3820, 3821, 3822, 3823,
				3824, 3825, 3826, 3827, 3828, 3829, 3830, 3831, 3832, 3833, 3834, 3835, 3836, 3837, 3838, 3839, 3840, 3841, 3842, 3843,
				3844, 3845, 3846, 3847, 3848, 3849, 3850, 3851, 3852, 3853, 3854, 3855, 3856, 3857, 3858, 3859, 3860, 3861, 3862, 3863,
				3864, 3865, 3866, 3867, 3868, 3869, 3870, 3871, 3872, 3873, 3874, 3875, 3876, 3877, 3878, 3879, 3880, 3881, 3882, 3883,
				3884, 3885, 3886, 3887, 3888, 3889, 3890, 3891, 3892, 3893, 3894, 3895, 3896, 3897, 3898, 3899, 3900, 3901, 3902, 3903,
				3904, 3905, 3906, 3907, 3908, 3909, 3910, 3911, 3912, 3913, 3914, 3915, 3916, 3917, 3918, 3919, 3920, 3921, 3922, 3923,
				3924, 3925, 3926, 3927, 3928, 3929, 3930, 3931, 3932, 3933, 3934, 3935, 3936, 3937, 3938, 3939, 3940, 3941, 3942, 3943,
				3944, 3945, 3946, 3947, 3948, 3949, 3950, 3951, 3952, 3953, 3954, 3955, 3956, 3957, 3958, 3959, 3960, 3961, 3962, 3963,
				3964, 3965, 4004, 4005, 4006, 4007, 4008, 4009, 4010, 4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021,
				4022, 4023, 4024, 4025, 4026, 4027, 4028, 4029, 4032, 4033, 4034, 4035, 4036, 4037, 4038, 4039, 4040, 4041, 4042, 4043,
				4044, 4045, 4046, 4047, 4048, 4049, 4050, 4051, 4052, 4053, 4054, 4055, 4056, 4057, 4058, 4059, 4060, 4061, 4062, 4063,
				4064, 4065, 4066, 4067, 4072, 4073, 4074, 4075, 4076, 4077, 4078, 4079, 4080, 4081, 4082, 4083, 4084, 4085, 4086, 4087,
				4088, 4089, 4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097, 4098, 4099, 4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107,
				4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 4120, 4121, 4124, 4125, 4126, 4127, 4128, 4129, 4130,
				4131, 4132, 4133, 4134, 4135, 4136, 4137, 4138, 4139, 4140, 4141, 4142, 4143, 4144, 4145, 4146, 4147, 4148, 4149, 4150,
				4151, 4152, 4153, 4154, 4155, 4156, 4157, 4158, 4159, 4160, 4161, 4162, 4163, 4164, 4165, 4166, 4167, 4168, 4169, 4170,
				4171, 4172, 4173, 4174, 4175, 4176, 4177, 4178, 4179, 4180, 4181, 4182, 4183, 4184, 4185, 4186, 4187, 4188, 4189, 4190,
				4191, 4192, 4193, 4194, 4195, 4196, 4197, 4198, 4199, 4200, 4201, 4202, 4203, 4204, 4205, 4206, 4207, 4208, 4209, 4210,
				4211, 4212, 4213, 4214, 4215, 4216, 4217, 4218, 4219, 4220, 4221, 4222, 4223, 4224, 4225, 4226, 4227, 4228, 4229, 4230,
				4231, 4232, 4233, 4234, 4235, 4236, 4237, 4238, 4239, 4240, 4241, 4242, 4243, 4244, 4245, 4246, 4247, 4248, 4249, 4250,
				4251, 4252, 4253, 4254, 4255, 4256, 4257, 4258, 4259, 4260, 4261, 4262, 4263, 4264, 4265, 4266, 4267, 4268, 4269, 4270,
				4271, 4272, 4273, 4274, 4275, 4276, 4277, 4278, 4279, 4280, 4281, 4282, 4283, 4284, 4285, 4286, 4287, 4288, 4289, 4290,
				4291, 4292, 4293, 4294, 4295, 4296, 4297, 4298, 4299, 4300, 4301, 4302, 4303, 4304, 4305, 4306, 4307, 4308, 4309, 4310,
				4311, 4312, 4313, 4314, 4315, 4316, 4317, 4318, 4319, 4320, 4321, 4322, 4323, 4324, 4325, 4326, 4327, 4328, 4329, 4330,
				4331, 4332, 4333, 4334, 4335, 4336, 4337, 4338, 4339, 4340, 4341, 4342, 4343, 4344, 4345, 4346, 4347, 4348, 4349, 4350,
				4351, 4352, 4353, 4354, 4355, 4356, 4357, 4358, 4359, 4360, 4361, 4362, 4363, 4364, 4365, 4366, 4367, 4368, 4369, 4370,
				4371, 4372, 4373, 4374, 4375, 4376, 4377, 4380, 4381, 4384, 4385, 4387, 4388, 4390, 4391, 4393, 4394, 4395, 4396, 4397,
				4398, 4399, 4400, 4401, 4402, 4403, 4404, 4405, 4406, 4407, 4408, 4409, 4410, 4411, 4433, 4434, 4435, 4436, 4437, 4438,
				4439, 4440, 4441, 4442, 4443, 4444, 4445, 4446, 4447, 4448, 4449, 4450, 4451, 4452, 4453, 4454, 4455, 4456, 4457, 4458,
				4459, 4460, 4461, 4462, 4463, 4464, 4465, 4466, 4467, 4468, 4469, 4470, 4471, 4472, 4473, 4474, 4475, 4476, 4477, 4478,
				4479, 4480, 4481, 4482, 4483, 4484, 4485, 4486, 4487, 4488, 4489, 4490, 4491, 4492, 4493, 4494, 4495, 4496, 4497, 4498,
				4499, 4500, 4501, 4502, 4503, 4504, 4505, 4506, 4507, 4508, 4509, 4510, 4511, 4512, 4513, 4514, 4515, 4516, 4517, 4518,
				4519, 4520, 4521, 4522, 4523, 4524, 4525, 4526, 4527, 4528, 4529, 4530, 4531, 4532, 4533, 4534, 4535, 4536, 4537, 4538,
				4539, 4540, 4541, 4542, 4543, 4544, 4545, 4546, 4547, 4548, 4549, 4550, 4551, 4552, 4553, 4554, 4555, 4556, 4557, 4560,
				4561, 4564, 4565, 4566, 4567, 4568, 4569, 4570, 4571, 4572, 4573, 4574, 4575, 4576, 4577, 4578, 4579, 4580, 4581, 4582,
				4583, 4584, 4585, 4586, 4587, 4588, 4589, 4590, 4591, 4592, 4593, 4594, 4595, 4596, 4597, 4598, 4599, 5650, 5651, 5652,
				5653, 5654, 5655, 5656, 5657, 5658, 5659, 5660, 5661, 5662, 5663, 5664, 5665, 5666, 5667, 5668, 5669, 5670, 5671, 5672,
				5673, 5674, 5675, 5676, 5677, 5678, 5679, 5680, 5681, 5682, 5683, 5684, 5685, 5686, 5687, 5688, 5689, 5690, 5691, 5692,
				5693, 5694, 5695, 5696, 5697, 5698, 5699, 5700, 5701, 5702, 5703, 5704, 5705, 5706, 5707, 5708, 5709, 5710, 5711, 5712,
				5713, 5714, 5715, 5716, 5717, 5718, 5719, 5720, 5721, 5722, 5723, 5724, 5725, 5726, 5727, 5728, 5733, 5734, 5735, 5736,
				5737, 5738, 5739, 5740, 5741, 5742, 5743, 5744, 5745, 5746, 5747, 5748, 5749, 5750, 5751, 5752, 5753, 5754, 5755, 5756,
				5757, 5758, 5759, 5760, 5761, 5762, 5763, 5764, 5765, 5766, 5767, 5768, 5769, 5770, 5771, 5772, 5773, 5774, 5775, 5776,
				5777, 5778, 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786, 5787, 5788, 5789, 5790, 5791, 5792, 5793, 5794, 5795, 5796,
				5797, 5798, 5799, 5800, 5801, 5802, 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810, 5811, 5812, 5813, 5814, 5815, 5816,
				5817, 5818, 5819, 5820, 5821, 5822, 5823, 5824, 5825, 5826, 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834, 5835, 5836,
				5837, 5838, 5839, 5840, 5841, 5842, 5843, 5844, 5845, 5846, 5847, 5848, 5849, 5850, 5851, 5852, 5853, 5854, 5855, 5856,
				5857, 5858, 5859, 5860, 5861, 5862, 5863, 5864, 5865, 5866, 5867, 5868, 5869, 5870, 5871, 5872, 5873, 5874, 5875, 5876,
				5877, 5878, 5879, 5880, 5881, 5882, 5883, 5884, 5885, 5886, 5887, 5888, 5889, 5890, 5891, 5892, 5893, 5894, 5895, 5896,
				5897, 5898, 5899, 5900, 5901, 5902, 5903, 5904, 5905, 5906, 5907, 5908, 5909, 5910, 5911, 5912, 5913, 5914, 5915, 5916,
				5917, 5918, 5919, 5920, 5921, 5922, 5923, 5924, 5925, 5926, 5927, 5928, 5929, 5930, 5931, 5932, 5933, 5934, 5935, 5936,
				5937, 5938, 5939, 5940, 5941, 6122, 6123, 6124, 6125, 6126, 6127, 6128, 6129, 6130, 6131, 6132, 6133, 6134, 6135, 6136,
				6137, 6138, 6139, 6140, 6141, 6142, 6143, 6144, 6145, 6146, 6147, 6148, 6149, 6150, 6151, 6152, 6153, 6154, 6155, 6156,
				6157, 6158, 6159, 6160, 6161, 6162, 6163, 6164, 6165, 6166, 6167, 6168, 6169, 6170, 6171, 6172, 6173, 6174, 6175, 6176,
				6177, 6178, 6179, 6180, 6181, 6182, 6183, 6184, 6185, 6186, 6187, 6188, 6189, 6190, 6191, 6192, 6193, 6194, 6195, 6196,
				6197, 6198, 6199, 6200, 6201, 6202, 6203, 6204, 6205, 6206, 6207, 6208, 6209, 6210, 6211, 6212, 6213, 6214, 6215, 6216,
				6217, 6218, 6219, 6220, 6221, 6222, 6223, 6224, 6225, 6226, 6230, 6231, 6232, 6233, 6234, 6235, 6236, 6237, 6238, 6239,
				6240, 6241, 6242, 6243, 6244, 6245, 6246, 6247, 6248, 6249, 6250, 6251, 6252, 6253, 6254, 6255, 6256, 6257, 6258, 6259,
				6260, 6261, 6262, 6263, 6264, 6265, 6266, 6267, 6268, 6269, 6270, 6271, 6272, 6273, 6274, 6275, 6276, 6277, 6278, 6279,
				6280, 6281, 6282, 6283, 6284, 6285, 6286, 6287, 6288, 6289, 6290, 6291, 6292, 6296, 6297, 6298, 6299, 6300, 6301, 6302,
				6303, 6304, 6305, 6306, 6307, 6308, 6309, 6310, 6311, 6312, 6313, 6314, 6315, 6316, 6317, 6318, 6319, 6320, 6321, 6322,
				6323, 6324, 6325, 6326, 6327, 6328, 6329, 6330, 6331, 6332, 6333, 6334, 6335, 6336, 6337, 6338, 6339, 6340, 6341, 6342,
				6343, 6344, 6345, 6346, 6347, 6348, 6349, 6350, 6351, 6352, 6353, 6354, 6355, 6356, 6357, 6358, 6359, 6360, 6361, 6362,
				6363, 6364, 6365, 6366, 6367, 6368, 6369, 6370, 6371, 6372, 6373, 6374, 6375, 6376, 6377, 6378, 6379, 6380, 6381, 6382,
				6383, 6384, 6385, 6386, 6387, 6388, 6389, 6390, 6391, 6392, 6393, 6394, 6395, 6396, 6397, 6398, 6399, 6400, 6401, 6402,
				6403, 6404, 6405, 6406, 6407, 6408, 6409, 6410, 6411, 6412, 6413, 6414, 6415, 6416, 6417, 6418, 6419, 6420, 6421, 6422,
				6423, 6424, 6425, 6426, 6427, 6428, 6429, 6430, 6431, 6432, 6433, 6434, 6435, 6436, 6437, 6438, 6439, 6440, 6441, 6442,
				6443, 6444, 6445, 6446, 6447, 6448, 6449, 6450, 6451, 6452, 6453, 6454, 6455, 6456, 6457, 6458, 6459, 6460, 6461, 6462,
				6463, 6464, 6465, 6466, 6467, 6468, 6469, 6470, 6471, 6472, 6473, 6474, 6475, 6476, 6477, 6478, 6479, 6480, 6481, 6482,
				6483, 6484, 6485, 6486, 6487, 6488, 6489, 6490, 6491, 6492, 6493, 6494, 6495, 6496, 6497, 6498, 6499, 6500, 6501, 6502,
				6503, 6504, 6505, 6506, 6507, 6508, 6509, 6510, 6511, 6512, 6513, 6514, 6515, 6516, 6517, 6518, 6519, 6520, 6521, 6522,
				6523, 6524, 6525, 6526, 6527, 6528, 6529, 6530, 6531, 6532, 6533, 6534, 6535, 6536, 6537, 6538, 6539, 6540, 6541, 6542,
				6543, 6544, 6545, 6546, 6547, 6548, 6549, 6550, 6551, 6552, 6553, 6554, 6555, 6556, 6557, 6558, 6559, 6560, 6561, 6562,
				6563, 6564, 6565, 6566, 6567, 6569, 6570, 6571, 6572, 6573, 6574, 6575, 6576, 6577, 6578, 6579, 6580, 6581, 6582, 6583,
				6584, 6585, 6586, 6587, 6588, 6589, 6590, 6591, 6592, 6593, 6594, 6595, 6596, 6597, 6598, 6599, 6600, 6601, 6602, 6603,
				6604, 6605, 6606, 6607, 6608, 6609, 6610, 6611, 6612, 6613, 6614, 6615, 6616, 6617, 6618, 6619, 6620, 6621, 6622, 6623,
				6624, 6625, 6626, 6627, 6628, 6629, 6630, 6631, 6632, 6633, 6634, 6762, 6763, 6764, 6765, 6766, 6767, 6768, 6769, 6770,
				6771, 6772, 6773, 6774, 6775, 6776, 6777, 6778, 6779, 6780, 6781, 6782, 6783, 6784, 6785, 6786, 6787, 6788, 6789, 6790,
				6791, 6792, 6793, 6794, 6795, 6796, 6797, 6798, 6799, 6800, 6801, 6802, 6803, 6804, 6805, 6806, 6807, 6808, 6809, 6810,
				6811, 6812, 6813, 6814, 6815, 6816, 6817, 6818, 6819, 6820, 6821, 6822, 6823, 6824, 6825, 6826, 6827, 6828, 6829, 6830,
				6831, 6832, 6833, 6834, 6835, 6836, 6837, 6838, 6839, 6840, 6841, 6842, 6843, 6844, 6845, 6846, 6847, 6848, 6849, 6850,
				6851, 6852, 6853, 6854, 6855, 6856, 6857, 6858, 6859, 6860, 6861, 6862, 6863, 6864, 6865, 6866, 6867, 6868, 6869, 6870,
				6871, 6872, 6873, 6874, 6875, 6876, 6877, 6878, 6879, 6880, 6881, 6882, 6883, 6884, 6885, 6886, 6887, 6888, 6889, 6890,
				6891, 6892, 6894, 6895, 6896, 6897, 6898, 6899, 6901, 6902, 6903, 6904, 6905, 6906, 6907, 6908, 6909, 6910, 6911, 6912,
				6913, 6914, 6915, 6916, 6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924, 6925, 6926, 6927, 6928, 6929, 6930, 6931, 6932,
				6933, 6934, 6935, 6936, 6937, 6942, 6943, 6944, 6945, 6946, 6947, 6950, 6951, 6952, 6953, 6954, 6955, 6956, 6957, 6960,
				6961, 6964, 6965, 6966, 6967, 6968, 6969, 6970, 6971, 6972, 6973, 6976, 6977, 6978, 6979, 6980, 6981, 6982, 6983, 6984,
				6985, 6986, 6987, 6988, 6989, 6990, 6991, 6995, 6996, 6997, 6998, 6999, 7000, 7001, 7002, 7003, 7004, 7005, 7006, 7007,
				7008, 7009, 7010, 7011, 7012, 7013, 7014, 7015, 7016, 7017, 7018, 7019, 7020, 7021, 7022, 7023, 7024, 7025, 7026, 7027,
				7028, 7029, 7030, 7031, 7032, 7033, 7034, 7035, 7036, 7037, 7038, 7039, 7040, 7041, 7042, 7043, 7044, 7045, 7046, 7047,
				7048, 7049, 7050, 7051, 7052, 7053, 7054, 7055, 7056, 7057, 7058, 7059, 7060, 7061, 7062, 7063, 7064, 7065, 7066, 7067,
				7068, 7069, 7070, 7071, 7072, 7073, 7074, 7075, 7076, 7077, 7078, 7079, 7080, 7081, 7082, 7083, 7084, 7085, 7086, 7087,
				7088, 7089, 7090, 7091, 7092, 7093, 7094, 7095, 7096, 7097, 7098, 7099, 7100, 7101, 7102, 7103, 7104, 7105, 7106, 7108,
				7109, 7110, 7111, 7112, 7113, 7114, 7115, 7116, 7117, 7118, 7119, 7120, 7121, 7122, 7123, 7124, 7125, 7126, 7127, 7128,
				7129, 7130, 7131, 7132, 7133, 7134, 7135, 7136, 7137, 7138, 7139, 7140, 7141, 7142, 7143, 7144, 7145, 7146, 7147, 7148,
				7149, 7150, 7151, 7152, 7153, 7154, 7155, 7156, 7157, 7158, 7159, 7160, 7161, 7162, 7163, 7164, 7165, 7166, 7167, 7168,
				7169, 7170, 7171, 7172, 7173, 7174, 7175, 7176, 7177, 7178, 7179, 7180, 7181, 7182, 7183, 7184, 7185, 7186, 7187, 7188,
				7189, 7190, 7195, 7196, 7197, 7198, 7199, 7200, 7201, 7202, 7203, 7204, 7205, 7206, 7207, 7208, 7209, 7210, 7211, 7212,
				7213, 7215, 7216, 7217, 7218, 7219, 7220, 7221, 7222, 7223, 7224, 7225, 7226, 7227, 7228, 7229, 7230, 7231, 7233, 7234,
				7235, 7236, 7237, 7238, 7239, 7240, 7241, 7242, 7243, 7244, 7245, 7246, 7247, 7248, 7249, 7250, 7251, 7252, 7253, 7254,
				7255, 7256, 7257, 7258, 7259, 7260, 7261, 7262, 7263, 7264, 7265, 7266, 8149, 8150, 8151, 8152, 8153, 8154, 8155, 8156,
				8157, 8158, 8159, 8160, 8161, 8162, 8163, 8164, 8165, 8166, 8167, 8168, 8183, 8184, 8185, 8186, 8187, 8188, 8189, 8190,
				8191, 8192, 8193, 8194, 8195, 8196, 8197, 8198, 8199, 8200, 8201, 8202, 8203, 8217, 8218, 8219, 8220, 8221, 8222, 8223,
				8224, 8225, 8226, 8227, 8228, 8229, 8237, 8238, 8239, 8240, 8241, 8242, 8243, 8244, 8245, 8246, 8247, 8248, 8249, 8250,
				8251, 8252, 8253, 8254, 8255, 8256, 8257, 8258, 8259, 8260, 8261, 8262, 8263, 8264, 8265, 8266, 8267, 8268, 8269, 8270,
				8271, 8272, 8273, 8274, 8275, 8276, 8277, 8278, 8279, 8280, 8281, 8282, 8283, 8284, 8285, 8286, 8287, 8288, 8289, 8290,
				8291, 8292, 8293, 8294, 8295, 8296, 8297, 8298, 8299, 8300, 8301, 8302, 8303, 8304, 8305, 8306, 8307, 8308, 8309, 8310,
				8311, 8312, 8313, 8314, 8315, 8316, 8317, 8318, 8319, 8320, 8321, 8322, 8323, 8324, 8325, 8326, 8327, 8328, 8329, 8330,
				8331, 8332, 8333, 8334, 8335, 8336, 8337, 8338, 8339, 8340, 8341, 8342, 8343, 8344, 8345, 8346, 8347, 8348, 8349, 8350,
				8351, 8352, 8353, 8354, 8355, 8356, 8357, 8358, 8359, 8360, 8361, 8362, 8363, 8364, 8365, 8366, 8367, 8368, 8369, 8370,
				8371, 8372, 8373, 8374, 8375, 8376, 8377, 8378, 8379, 8380, 8381, 8382, 8383, 8384, 8385, 8386, 8387, 8388, 8389, 8390,
				8391, 8392, 8393, 8394, 8395, 8396, 8397, 8398, 8399, 8400, 8401, 8402, 8403, 8404, 8405, 8406, 8407, 8408, 8409, 8410,
				8411, 8412, 8413, 8414, 8415, 8416, 8417, 8418, 8419, 8420, 8421, 8422, 8423, 8424, 8425, 8426, 8427, 8428, 8429, 8430,
				8431, 8432, 8433, 8434, 8435, 8436, 8437, 8438, 8439, 8440, 8441, 8442, 8443, 8444, 8445, 8446, 8447, 8448, 8449, 8450,
				8451, 8452, 8453, 8454, 8455, 8456, 8457, 8458, 8459, 8462, 8463, 8465, 8466, 8467, 8468, 8469, 8470, 8471, 8472, 8473,
				8474, 8475, 8476, 8477, 8478, 8479, 8480, 8656, 8657, 8658, 8659, 8660, 8661, 8662, 8663, 8664, 8665, 8666, 8667, 8668,
				8669, 8670, 8671, 8672, 8673, 8674, 8675, 8676, 8677, 8678, 8679, 8680, 8681, 8682, 8683, 8684, 8685, 8686, 8687, 8688,
				8689, 8690, 8691, 8692, 8693, 8694, 8695, 8696, 8697, 8698, 8699, 8700, 8701, 8702, 8703, 8704, 8705, 8706, 8707, 8708,
				8709, 8710, 8711, 8712, 8713, 8714, 8715, 8716, 8717, 8718, 8719, 8720, 8721, 8722, 8723, 8724, 8725, 8726, 8727, 8728,
				8729, 8730, 8731, 8732, 8733, 8734, 8735, 8736, 8737, 8738, 8739, 8740, 8741, 8742, 8743, 8744, 8745, 8746, 8747, 8748,
				8749, 8752, 8753, 8754, 8755, 8756, 8757, 8758, 8759, 8760, 8761, 8762, 8763, 8764, 8765, 8766, 8767, 8768, 8769, 8770,
				8771, 8772, 8773, 8774, 8775, 8776, 8777, 8778, 8779, 8780, 8781, 8782, 8783, 8784, 8785, 8786, 8787, 8788, 8789, 8790,
				8791, 9210, 9211, 9212, 9213, 9214, 9215, 9216, 9217, 9218, 9219, 9220, 9221, 9222, 9223, 9224, 9225, 9226, 9227, 9228,
				9229, 9230, 9231, 9232, 9233, 9234, 9235, 9236, 9237, 9238, 9239, 9240, 9241, 9242, 9243, 9244, 9245, 9246, 9247, 9248,
				9249, 9250, 9251, 9252, 9253, 9254, 9255, 9256, 9257
		};
		private readonly int[] h1Array = new int[5472]
		{
				225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249,
				250, 251, 252, 253, 254, byte.MaxValue, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 270, 271, 272,
				273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297,
				298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322,
				323, 324, 325, 326, 327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347,
				348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372,
				373, 374, 375, 376, 377, 378, 379, 380, 381, 382, 383, 384, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405,
				406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429, 430,
				431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451, 452, 453, 454, 455,
				456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474, 475, 476, 477, 478, 479, 480,
				481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 496, 501, 502, 503, 504, 505, 506, 507, 508, 509,
				510, 511, 515, 516, 517, 518, 519, 520, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540,
				541, 542, 543, 544, 545, 546, 547, 548, 549, 550, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565,
				566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 585, 586, 587, 588, 589, 590,
				591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615,
				616, 617, 618, 619, 620, 621, 622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640,
				641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662, 663, 664, 665,
				666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690,
				691, 692, 693, 694, 695, 696, 697, 698, 699, 700, 701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715,
				716, 717, 718, 719, 720, 721, 722, 723, 724, 725, 726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740,
				741, 742, 743, 744, 745, 746, 747, 748, 749, 750, 751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765,
				766, 767, 768, 769, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800, 801, 802,
				803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825, 826, 827,
				828, 829, 830, 831, 832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850, 851, 852,
				853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875, 876, 877,
				878, 879, 880, 881, 882, 883, 884, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 898, 899, 900, 901, 902,
				903, 904, 905, 906, 907, 908, 909, 910, 911, 912, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927,
				928, 929, 930, 931, 932, 933, 934, 935, 936, 937, 938, 939, 940, 941, 942, 943, 944, 945, 946, 947, 948, 949, 950, 951, 952,
				953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 970, 971, 972, 973, 974, 975, 976, 977,
				978, 979, 980, 981, 982, 983, 984, 985, 986, 987, 988, 989, 990, 991, 992, 993, 994, 995, 996, 997, 998, 999, 1000, 1001,
				1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016, 1017, 1018, 1019, 1020, 1021,
				1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029, 1030, 1031, 1032, 1033, 1034, 1035, 1036, 1037, 1038, 1039, 1040, 1041,
				1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1060, 1061,
				1062, 1063, 1064, 1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081,
				1082, 1083, 1084, 1085, 1086, 1087, 1088, 1089, 1090, 1091, 1092, 1093, 1114, 1115, 1116, 1117, 1118, 1119, 1120, 1121,
				1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1139, 1140, 1141,
				1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1150, 1151, 1152, 1153, 1154, 1155, 1156, 1157, 1158, 1159, 1160, 1161,
				1162, 1163, 1164, 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172, 1173, 1174, 1175, 1176, 1177, 1178, 1179, 1180, 1181,
				1182, 1183, 1184, 1185, 1186, 1187, 1188, 1189, 1190, 1191, 1192, 1193, 1194, 1195, 1196, 1197, 1198, 1199, 1200, 1201,
				1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221,
				1222, 1223, 1224, 1225, 1226, 1227, 1228, 1229, 1230, 1231, 1232, 1233, 1234, 1235, 1236, 1237, 1238, 1239, 1240, 1241,
				1242, 1243, 1244, 1245, 1246, 1247, 1248, 1249, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1259, 1260, 1261,
				1262, 1263, 1264, 1265, 1266, 1267, 1268, 1269, 1270, 1271, 1272, 1273, 1294, 1295, 1296, 1297, 1298, 1299, 1300, 1301,
				1302, 1303, 1304, 1305, 1306, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316, 1317, 1318, 1319, 1320, 1321,
				1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1332, 1333, 1334, 1335, 1336, 1337, 1338, 1339, 1340, 1341,
				1342, 1343, 1344, 1345, 1346, 1347, 1348, 1349, 1350, 1351, 1352, 1353, 1354, 1355, 1356, 1357, 1358, 1359, 1360, 1361,
				1362, 1363, 1364, 1365, 1366, 1367, 1368, 1369, 1374, 1375, 1376, 1377, 1378, 1379, 1380, 1381, 1382, 1383, 1384, 1385,
				1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397, 1398, 1399, 1400, 1401, 1410, 1411, 1412, 1413,
				1414, 1415, 1416, 1417, 1418, 1419, 1421, 1422, 1423, 1424, 1425, 1426, 1427, 1428, 1429, 1430, 1431, 1432, 1433, 1434,
				1435, 1436, 1437, 1438, 1439, 1440, 1441, 1442, 1443, 1444, 1445, 1446, 1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454,
				1455, 1456, 1457, 1458, 1459, 1460, 1461, 1462, 1463, 1464, 1465, 1466, 1467, 1468, 1469, 1470, 1471, 1472, 1473, 1474,
				1475, 1476, 1477, 1478, 1479, 1480, 1481, 1482, 1483, 1484, 1485, 1486, 1487, 1488, 1489, 1490, 1491, 1492, 1493, 1494,
				1495, 1496, 1497, 1498, 1499, 1500, 1501, 1502, 1503, 1504, 1505, 1506, 1507, 1508, 1509, 1510, 1511, 1512, 1513, 1514,
				1515, 1516, 1517, 1518, 1519, 1520, 1521, 1522, 1523, 1524, 1525, 1526, 1527, 1528, 1529, 1530, 1531, 1532, 1533, 1534,
				1535, 1536, 1537, 1538, 1539, 1540, 1541, 1542, 1543, 1544, 1545, 1546, 1547, 1548, 1549, 1550, 1551, 1552, 1553, 1554,
				1555, 1556, 1557, 1558, 1559, 1560, 1561, 1562, 1563, 1564, 1565, 1566, 1567, 1568, 1569, 1570, 1571, 1572, 1573, 1574,
				1575, 1576, 1577, 1578, 1579, 1580, 1581, 1582, 1583, 1584, 1585, 1586, 1587, 1588, 1589, 1590, 1591, 1592, 1593, 1594,
				1595, 1596, 1597, 1598, 1599, 1600, 1601, 1602, 1603, 1604, 1605, 1606, 1607, 1608, 1609, 1610, 1611, 1612, 1613, 1614,
				1615, 1616, 1617, 1618, 1619, 1620, 1621, 1622, 1623, 1624, 1625, 1626, 1627, 1628, 1629, 1630, 1631, 1632, 1633, 1634,
				1635, 1636, 1637, 1638, 1639, 1640, 1641, 1642, 1643, 1644, 1645, 1646, 1647, 1648, 1649, 1650, 1651, 1652, 1653, 1654,
				1655, 1656, 1657, 1658, 1659, 1660, 1661, 1662, 1663, 1664, 1665, 1666, 1667, 1668, 1669, 1670, 1671, 1672, 1673, 1674,
				1675, 1676, 1677, 1678, 1679, 1680, 1681, 1682, 1683, 1684, 1685, 1686, 1687, 1688, 1689, 1690, 1691, 1692, 1693, 1694,
				1695, 1696, 1697, 1698, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706, 1707, 1708, 1709, 1710, 1711, 1712, 1713, 1714,
				1715, 1716, 1717, 1718, 1719, 1720, 1721, 1722, 1723, 1724, 1725, 1726, 1727, 1728, 1729, 1730, 1731, 1732, 1733, 1754,
				1755, 1756, 1757, 1758, 1759, 1760, 1761, 1762, 1763, 1764, 1765, 1766, 1767, 1768, 1769, 1770, 1771, 1772, 1773, 1774,
				1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782, 1783, 1784, 1785, 1786, 1787, 1788, 1789, 1790, 1791, 1792, 1793, 1794,
				1795, 1796, 1797, 1798, 1799, 1800, 1801, 1802, 1803, 1804, 1805, 1806, 1807, 1808, 1809, 1810, 1811, 1812, 1813, 1814,
				1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823, 1824, 1825, 1826, 1827, 1828, 1829, 1830, 1831, 1832, 1833, 1834,
				1835, 1836, 1837, 1838, 1839, 1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849, 1850, 1851, 1852, 1853, 1854,
				1855, 1856, 1857, 1858, 1859, 1860, 1861, 1862, 1863, 1864, 1865, 1866, 1867, 1868, 1869, 1870, 1871, 1872, 1873, 1894,
				1895, 1896, 1897, 1898, 1899, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908, 1909, 1910, 1911, 1912, 1913, 1914,
				1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922, 1923, 1924, 1925, 1930, 1931, 1932, 1933, 1934, 1935, 1936, 1937, 1938,
				1939, 1940, 1941, 1942, 1943, 1944, 1945, 1946, 1947, 1948, 1949, 1950, 1951, 1952, 1953, 1954, 1955, 1956, 1957, 1958,
				1959, 1960, 1961, 1962, 1963, 1964, 1965, 1966, 1967, 1968, 1969, 1970, 1971, 1972, 1973, 1974, 1975, 1976, 1977, 1978,
				1979, 1980, 1981, 1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998,
				1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2013, 2014, 2015, 2016, 2017, 2171, 2172,
				2173, 2174, 2175, 2176, 2177, 2178, 2179, 2180, 2181, 2182, 2183, 2184, 2185, 2186, 2187, 2188, 2189, 2190, 2191, 2192,
				2193, 2194, 2195, 2196, 2197, 2198, 2199, 2200, 2201, 2202, 2203, 2204, 2205, 2206, 2207, 2208, 2209, 2210, 2211, 2212,
				2213, 2214, 2215, 2216, 2217, 2218, 2219, 2220, 2221, 2222, 2223, 2224, 2225, 2226, 2227, 2228, 2229, 2230, 2231, 2232,
				2233, 2234, 2235, 2236, 2237, 2238, 2239, 2240, 2241, 2242, 2243, 2244, 2245, 2246, 2247, 2248, 2249, 2250, 2251, 2252,
				2253, 2254, 2255, 2256, 2257, 2258, 2259, 2260, 2261, 2262, 2263, 2264, 2265, 2266, 2267, 2268, 2269, 2270, 2271, 2272,
				2273, 2274, 2275, 2276, 2277, 2278, 2279, 2280, 2281, 2282, 2283, 2284, 2285, 2286, 2287, 2288, 2289, 2290, 2291, 2292,
				2293, 2294, 2295, 2296, 2297, 2298, 2299, 2300, 2301, 2302, 2303, 2304, 2305, 2306, 2307, 2308, 2309, 2310, 2311, 2312,
				2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2326, 2327, 2328, 2329, 2330, 2331, 2332,
				2333, 2334, 2335, 2336, 2337, 2338, 2339, 2340, 2341, 2342, 2343, 2344, 2345, 2346, 2347, 2348, 2349, 2350, 2351, 2352,
				2353, 2354, 2355, 2356, 2357, 2358, 2359, 2360, 2361, 2362, 2363, 2364, 2365, 2366, 2367, 2368, 2369, 2370, 2371, 2372,
				2373, 2374, 2375, 2376, 2377, 2378, 2379, 2380, 2381, 2382, 2383, 2384, 2385, 2386, 2387, 2388, 2389, 2390, 2391, 2392,
				2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2402, 2403, 2404, 2405, 2406, 2407, 2408, 2409, 2410, 2411, 2412,
				2413, 2414, 2415, 2416, 2417, 2418, 2419, 2420, 2421, 2422, 2423, 2424, 2425, 2426, 2427, 2428, 2429, 2430, 2431, 2432,
				2433, 2434, 2435, 2436, 2437, 2438, 2439, 2440, 2441, 2442, 2443, 2444, 2445, 2446, 2447, 2448, 2449, 2450, 2451, 2452,
				2453, 2454, 2455, 2456, 2457, 2458, 2459, 2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469, 2470, 2471, 2472,
				2473, 2474, 2475, 2476, 2477, 2478, 2479, 2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 2489, 2490, 2491, 2492,
				2493, 2494, 2495, 2496, 2497, 2498, 2499, 2500, 2501, 2502, 2503, 2504, 2505, 2506, 2507, 2508, 2509, 2510, 2511, 2512,
				2513, 2514, 2515, 2516, 2517, 2518, 2519, 2520, 2521, 2522, 2523, 2524, 2525, 2526, 2527, 2528, 2529, 2530, 2531, 2532,
				2533, 2534, 2535, 2536, 2537, 2538, 2539, 2540, 2541, 2542, 2543, 2544, 2545, 2546, 2547, 2548, 2549, 2550, 2551, 2552,
				2553, 2554, 2555, 2556, 2557, 2558, 2559, 2560, 2595, 2596, 2597, 2598, 2599, 2600, 2601, 2602, 2603, 2604, 2605, 2606,
				2607, 2608, 2609, 2610, 2611, 2612, 2613, 2614, 2615, 2616, 2617, 2618, 2619, 2620, 2621, 2622, 2623, 2624, 2625, 2626,
				2627, 2628, 2629, 2630, 2631, 2632, 2633, 2634, 2635, 2636, 2637, 2638, 2639, 2640, 2641, 2642, 2643, 2644, 2645, 2646,
				2647, 2648, 2649, 2650, 2651, 2652, 2653, 2654, 2655, 2656, 2657, 2658, 2659, 2660, 2661, 2662, 2663, 2664, 2665, 2666,
				2667, 2668, 2669, 2670, 2671, 2672, 2673, 2674, 2675, 2676, 2677, 2678, 2679, 2680, 2681, 2682, 2683, 2684, 2685, 2687,
				2688, 2689, 2690, 2691, 2692, 2693, 2694, 2695, 2696, 2697, 2698, 2699, 2700, 2701, 2702, 2703, 2704, 2705, 2706, 2707,
				2708, 2709, 2710, 2711, 2712, 2713, 2714, 2715, 2716, 2717, 2718, 2719, 2720, 2721, 2722, 2723, 2724, 2725, 2726, 2727,
				2728, 2746, 2747, 2748, 2749, 2750, 2751, 2752, 2753, 2754, 2755, 2756, 2757, 2758, 2759, 2760, 2761, 2762, 2763, 2764,
				2765, 2766, 2767, 2768, 2769, 2770, 2771, 2772, 2773, 2774, 2775, 2776, 2777, 2778, 2779, 2780, 2781, 2782, 2783, 2784,
				2785, 2786, 2787, 2788, 2789, 2790, 2791, 2792, 2793, 2794, 2795, 2796, 2797, 2798, 2799, 2800, 2801, 2802, 2803, 2804,
				2805, 2806, 2807, 2808, 2809, 2810, 2811, 2812, 2813, 2814, 2815, 2816, 2817, 2818, 2819, 2820, 2821, 2822, 2823, 2824,
				2825, 2826, 2827, 2828, 2829, 2830, 2831, 2832, 2833, 2834, 2835, 2836, 2837, 2838, 2839, 2840, 2841, 2842, 2843, 2844,
				2845, 2846, 2847, 2848, 2849, 2850, 2851, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860, 2861, 2862, 2863, 2864,
				2865, 2866, 2867, 2868, 2869, 2870, 2871, 2872, 2873, 2874, 2875, 2876, 2877, 2878, 2879, 2880, 2881, 2882, 2883, 2884,
				2885, 2886, 2887, 2888, 2889, 2890, 2891, 2896, 2898, 2899, 2902, 2903, 2904, 2905, 2906, 2907, 2908, 2909, 2910, 2911,
				2912, 2913, 2914, 2927, 2928, 2929, 2930, 2931, 2932, 2933, 2934, 2935, 2936, 2937, 2938, 2939, 2940, 2941, 2942, 2943,
				2944, 2945, 2946, 2947, 2948, 2949, 2950, 2951, 2952, 2953, 2954, 2955, 2956, 2957, 2958, 2959, 2960, 2962, 2963, 2964,
				2965, 2966, 2967, 2968, 2969, 2970, 2971, 2972, 2973, 2974, 2975, 2976, 2977, 2978, 2979, 2980, 2981, 2982, 2983, 2984,
				2985, 2986, 2987, 2988, 2989, 2990, 2991, 2992, 2993, 2994, 2995, 2996, 2997, 2998, 2999, 3000, 3001, 3002, 3003, 3004,
				3005, 3006, 3007, 3008, 3009, 3010, 3011, 3012, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021, 3022, 3023, 3024,
				3025, 3026, 3027, 3028, 3029, 3030, 3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041, 3042, 3043, 3044,
				3045, 3046, 3047, 3048, 3049, 3050, 3051, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061, 3062, 3063, 3064,
				3065, 3066, 3067, 3068, 3069, 3070, 3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081, 3082, 3083, 3084,
				3085, 3086, 3087, 3088, 3089, 3090, 3091, 3092, 3093, 3094, 3095, 3096, 3097, 3098, 3099, 3100, 3101, 3102, 3103, 3104,
				3105, 3106, 3107, 3108, 3109, 3110, 3113, 3115, 3116, 3117, 3118, 3121, 3123, 3125, 3126, 3129, 3131, 3132, 3133, 3134,
				3135, 3136, 3137, 3138, 3139, 3140, 3141, 3142, 3143, 3144, 3145, 3146, 3147, 3148, 3149, 3150, 3151, 3152, 3153, 3154,
				3155, 3156, 3157, 3158, 3159, 3160, 3161, 3162, 3163, 3164, 3165, 3166, 3167, 3168, 3169, 3170, 3171, 3172, 3173, 3174,
				3175, 3176, 3177, 3178, 3179, 3180, 3181, 3182, 3183, 3184, 3185, 3186, 3187, 3188, 3189, 3190, 3191, 3192, 3193, 3194,
				3195, 3196, 3197, 3198, 3199, 3200, 3201, 3202, 3203, 3204, 3205, 3206, 3207, 3208, 3209, 3210, 3211, 3212, 3213, 3214,
				3215, 3216, 3217, 3218, 3219, 3220, 3221, 3222, 3223, 3224, 3225, 3226, 3227, 3228, 3229, 3230, 3231, 3232, 3233, 3234,
				3235, 3236, 3237, 3238, 3239, 3240, 3241, 3242, 3243, 3244, 3245, 3246, 3247, 3248, 3249, 3250, 3251, 3252, 3253, 3254,
				3255, 3256, 3257, 3258, 3259, 3260, 3261, 3262, 3263, 3264, 3265, 3266, 3267, 3268, 3269, 3270, 3271, 3272, 3273, 3274,
				3275, 3276, 3277, 3278, 3279, 3280, 3281, 3282, 3283, 3284, 3285, 3286, 3287, 3288, 3289, 3290, 3331, 3332, 3333, 3334,
				3335, 3336, 3337, 3338, 3339, 3340, 3341, 3342, 3343, 3344, 3345, 3346, 3347, 3348, 3349, 3350, 3351, 3352, 3353, 3354,
				3355, 3356, 3357, 3358, 3359, 3360, 3361, 3362, 3363, 3364, 3365, 3366, 3367, 3368, 3369, 3370, 3371, 3372, 3373, 3374,
				3383, 3384, 3385, 3386, 3387, 3388, 3389, 3390, 3391, 3392, 3393, 3394, 3395, 3396, 3397, 3398, 3399, 3400, 3401, 3402,
				3403, 3404, 3405, 3406, 3407, 3408, 3409, 3410, 3411, 3412, 3413, 3414, 3415, 3416, 3417, 3418, 3423, 3424, 3425, 3426,
				3427, 3428, 3429, 3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438, 3439, 3440, 3441, 3442, 3443, 3444, 3445, 3446,
				3447, 3448, 3449, 3450, 3451, 3452, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466,
				3467, 3468, 3469, 3470, 3471, 3472, 3473, 3474, 3475, 3476, 3477, 3478, 3479, 3480, 3481, 3482, 3483, 3484, 3485, 3486,
				3487, 3488, 3489, 3490, 3491, 3492, 3493, 3494, 3495, 3496, 3497, 3498, 3499, 3500, 3501, 3502, 3503, 3504, 3505, 3506,
				3507, 3508, 3509, 3510, 3511, 3512, 3513, 3514, 3515, 3516, 3517, 3518, 3519, 3520, 3521, 3522, 3523, 3524, 3525, 3526,
				3527, 3528, 3529, 3530, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541, 3542, 3543, 3544, 3545, 3546,
				3547, 3548, 3549, 3550, 3551, 3552, 3553, 3554, 3555, 3556, 3557, 3558, 3559, 3560, 3561, 3562, 3563, 3564, 3565, 3566,
				3567, 3568, 3569, 3570, 3571, 3572, 3573, 3574, 3575, 3576, 3577, 3578, 3579, 3580, 3581, 3582, 3583, 3584, 3585, 3586,
				3587, 3588, 3589, 3590, 3591, 3592, 3593, 3594, 3595, 3596, 3597, 3598, 3599, 3600, 3601, 3602, 3603, 3604, 3605, 3606,
				3607, 3608, 3609, 3610, 3611, 3612, 3613, 3614, 3615, 3616, 3617, 3619, 3620, 3621, 3622, 3623, 3624, 3625, 3626, 3627,
				3628, 3629, 3630, 3631, 3632, 3633, 3634, 3635, 3636, 3637, 3638, 3639, 3640, 3641, 3642, 3643, 3644, 3645, 3646, 3647,
				3648, 3649, 3650, 3651, 3652, 3653, 3654, 3655, 3656, 3657, 3658, 3659, 3660, 3661, 3662, 3663, 3664, 3665, 3666, 3667,
				3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683, 3684, 3685, 3686, 3687,
				3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3702, 3703, 3704, 3705, 3706, 3707,
				3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3717, 3718, 3719, 3720, 3721, 3722, 3723, 3724, 3725, 3726, 3727,
				3728, 3729, 3730, 3731, 3732, 3733, 3734, 3735, 3736, 3737, 3738, 3739, 3740, 3741, 3742, 3743, 3744, 3745, 3746, 3747,
				3748, 3749, 3750, 3751, 3752, 3753, 3754, 3755, 3756, 3757, 3758, 3759, 3760, 3761, 3762, 3763, 3764, 3765, 3766, 3767,
				3768, 3769, 3770, 3771, 3772, 3773, 3774, 3775, 3776, 3777, 3778, 3779, 3780, 3781, 3782, 3783, 3784, 3785, 3786, 3787,
				3788, 3789, 3790, 3791, 3792, 3793, 3794, 3795, 3796, 3797, 3798, 3799, 3800, 3801, 3802, 3803, 3804, 3805, 3806, 3807,
				3808, 3809, 3810, 3811, 3812, 3813, 3814, 3815, 3816, 3817, 3818, 3843, 3844, 3845, 3846, 3847, 3848, 3849, 3850, 3851,
				3852, 3853, 3854, 3855, 3856, 3857, 3858, 3859, 3860, 3861, 3862, 3863, 3864, 3865, 3866, 3867, 3868, 3869, 3870, 3871,
				3872, 3873, 3874, 3875, 3876, 3877, 3878, 3879, 3880, 3881, 3882, 3883, 3884, 3885, 3886, 3887, 3888, 3889, 3890, 3891,
				3892, 3893, 3894, 3895, 3896, 3897, 3898, 3899, 3900, 3901, 3902, 3903, 3904, 3905, 3906, 3907, 3908, 3909, 3910, 3911,
				3912, 3913, 3914, 3915, 3916, 3917, 3918, 3919, 3920, 3921, 3922, 3923, 3924, 3925, 3926, 3927, 3928, 3929, 3930, 3931,
				3932, 3933, 3934, 3935, 3936, 3937, 3938, 3939, 3940, 3941, 3942, 3943, 3944, 3945, 3946, 3947, 3948, 3949, 3950, 3951,
				3952, 3953, 3954, 3955, 3956, 3957, 3958, 3959, 3960, 3961, 3962, 3963, 3964, 3965, 3966, 3967, 3968, 3969, 3970, 3971,
				3972, 3973, 3974, 3975, 3976, 3977, 3978, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 3988, 3989, 3990, 3991,
				3992, 3993, 3994, 3995, 3996, 3997, 3998, 3999, 4000, 4001, 4002, 4003, 4004, 4005, 4006, 4007, 4008, 4009, 4010, 4011,
				4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021, 4022, 4023, 4024, 4025, 4026, 4027, 4028, 4029, 4030, 4031,
				4032, 4033, 4034, 4035, 4036, 4037, 4038, 4039, 4040, 4041, 4042, 4043, 4044, 4045, 4046, 4047, 4048, 4049, 4050, 4051,
				4052, 4053, 4054, 4055, 4056, 4057, 4058, 4059, 4060, 4061, 4062, 4071, 4072, 4073, 4074, 4075, 4076, 4077, 4078, 4079,
				4080, 4081, 4082, 4083, 4084, 4085, 4086, 4087, 4088, 4089, 4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097, 4098, 4099,
				4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 4119,
				4120, 4121, 4122, 4123, 4124, 4125, 4126, 4127, 4128, 4129, 4130, 4131, 4132, 4133, 4134, 4135, 4136, 4137, 4138, 4139,
				4140, 4141, 4142, 4143, 4144, 4145, 4146, 4147, 4148, 4149, 4150, 4151, 4152, 4153, 4154, 4155, 4156, 4157, 4158, 4159,
				4160, 4161, 4162, 4163, 4164, 4165, 4166, 4167, 4168, 4169, 4170, 4171, 4172, 4173, 4174, 4175, 4176, 4177, 4178, 4179,
				4180, 4181, 4182, 4183, 4184, 4185, 4186, 4187, 4188, 4189, 4190, 4191, 4192, 4193, 4194, 4195, 4196, 4197, 4198, 4199,
				4200, 4201, 4202, 4203, 4204, 4205, 4206, 4207, 4208, 4209, 4210, 4211, 4212, 4213, 4214, 4215, 4216, 4217, 4218, 4219,
				4220, 4221, 4222, 4223, 4224, 4225, 4226, 4227, 4228, 4229, 4230, 4231, 4232, 4233, 4234, 4235, 4236, 4237, 4238, 4239,
				4240, 4489, 4490, 4491, 4492, 4493, 4494, 4495, 4496, 4497, 4498, 4499, 4500, 4501, 4502, 4503, 4504, 4505, 4506, 4507,
				4508, 4509, 4510, 4511, 4512, 4513, 4514, 4515, 4516, 4517, 4518, 4519, 4520, 4521, 4522, 4523, 4524, 4525, 4526, 4527,
				4528, 4529, 4530, 4531, 4532, 4533, 4534, 4535, 4536, 4537, 4538, 4539, 4540, 4541, 4542, 4543, 4544, 4545, 4546, 4547,
				4548, 4549, 4550, 4551, 4552, 4553, 4554, 4555, 4556, 4557, 4558, 4559, 4560, 4561, 4562, 4563, 4564, 4565, 4566, 4567,
				4568, 4569, 4570, 4571, 4572, 4573, 4574, 4575, 4576, 4577, 4578, 4579, 4580, 4581, 4582, 4583, 4584, 4585, 4586, 4587,
				4588, 4589, 4590, 4591, 4592, 4593, 4594, 4595, 4596, 4597, 4598, 4599, 4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607,
				4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626, 4627,
				4628, 4629, 4630, 4631, 4632, 4633, 4634, 4635, 4636, 4637, 4638, 4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646, 4647,
				4648, 4649, 4650, 4651, 4652, 4653, 4654, 4655, 4656, 4657, 4658, 4659, 4660, 4661, 6283, 6284, 6285, 6286, 6287, 6288,
				6289, 6290, 6291, 6292, 6293, 6294, 6295, 6296, 6297, 6298, 6299, 6300, 6301, 6302, 6303, 6304, 6305, 6306, 6307, 6308,
				6309, 6310, 6311, 6312, 6313, 6314, 6315, 6316, 6317, 6318, 6319, 6320, 6321, 6322, 6323, 6324, 6325, 6326, 6327, 6328,
				6329, 6330, 6331, 6332, 6333, 6334, 6335, 6336, 6337, 6338, 6339, 6340, 6341, 6342, 6343, 6344, 6345, 6346, 6347, 6348,
				6349, 6350, 6351, 6352, 6353, 6354, 6355, 6356, 6357, 6358, 6359, 6360, 6361, 6362, 6363, 6364, 6365, 6366, 6367, 6368,
				6369, 6370, 6371, 6372, 6373, 6374, 6375, 6376, 6377, 6378, 6379, 6380, 6381, 6382, 6383, 6384, 6385, 6386, 6387, 6388,
				6389, 6390, 6391, 6392, 6393, 6394, 6395, 6396, 6397, 6398, 6399, 6400, 6401, 6402, 6403, 6404, 6405, 6406, 6407, 6408,
				6409, 6410, 6411, 6412, 6413, 6414, 6415, 6416, 6417, 6418, 6419, 6420, 6421, 6422, 6423, 6424, 6425, 6426, 6427, 6428,
				6429, 6430, 6431, 6432, 6433, 6434, 6435, 6436, 6437, 6438, 6439, 6440, 6441, 6442, 6443, 6444, 6445, 6446, 6447, 6448,
				6449, 6450, 6451, 6452, 6453, 6454, 6455, 6456, 6457, 6458, 6459, 6460, 6461, 6462, 6463, 6464, 6465, 6466, 6467, 6468,
				6469, 6470, 6471, 6472, 6473, 6474, 6475, 6476, 6477, 6478, 6479, 6480, 6481, 6482, 6483, 6484, 6485, 6486, 6487, 6488,
				6489, 6490, 6491, 6492, 6493, 6494, 6495, 6496, 6497, 6498, 6499, 6500, 6501, 6502, 6503, 6504, 6505, 6506, 6507, 6508,
				6509, 6510, 6511, 6512, 6513, 6514, 6515, 6516, 6517, 6518, 6519, 6520, 6521, 6522, 6523, 6524, 6525, 6526, 6527, 6528,
				6529, 6530, 6531, 6532, 6533, 6534, 6535, 6536, 6537, 6538, 6539, 6540, 6541, 6542, 6543, 6544, 6545, 6546, 6547, 6548,
				6549, 6550, 6551, 6552, 6553, 6554, 6555, 6556, 6557, 6558, 6559, 6560, 6561, 6562, 6563, 6564, 6565, 6566, 6567, 6568,
				6569, 6570, 6571, 6572, 6573, 6574, 6575, 6576, 6577, 6578, 6579, 6580, 6581, 6582, 6583, 6584, 6585, 6586, 6587, 6588,
				6589, 6590, 6591, 6592, 6593, 6594, 6595, 6596, 6597, 6598, 6599, 6600, 6601, 6602, 6603, 6604, 6605, 6606, 6607, 6608,
				6609, 6610, 6611, 6612, 6613, 6614, 6615, 6616, 6617, 6618, 6619, 6620, 6621, 6622, 6623, 6624, 6625, 6626, 6627, 6628,
				6629, 6630, 6631, 6632, 6633, 6634, 6635, 6636, 6637, 6638, 6639, 6640, 6641, 6642, 6643, 6644, 6645, 6646, 6647, 6648,
				6649, 6650, 6651, 6652, 6653, 6654, 6655, 6656, 6657, 6658, 6659, 6660, 6661, 6662, 6663, 6664, 6665, 6666, 6667, 6668,
				6669, 6670, 6671, 6672, 6673, 6674, 6675, 6676, 6677, 6678, 6679, 6680, 6681, 6682, 6683, 6684, 6685, 6686, 6687, 6688,
				6689, 6690, 6691, 6692, 6693, 6694, 6695, 6696, 6697, 6698, 6699, 6700, 6701, 6702, 6703, 6704, 6705, 6706, 6707, 6708,
				6709, 6710, 6711, 6712, 6713, 6714, 6715, 6716, 6717, 6718, 6719, 6720, 6721, 6722, 6723, 6724, 6725, 6726, 6727, 6728,
				6729, 6730, 6731, 6732, 6733, 6734, 6735, 6736, 6737, 6738, 6739, 6740, 6741, 6742, 6743, 6744, 6745, 6746, 6747, 6748,
				6749, 6750, 6751, 6752, 6753, 6754, 6755, 6756, 6757, 6758, 6759, 6760, 6761, 6762, 6763, 6764, 6765, 6766, 6767, 6768,
				6769, 6770, 6771, 6772, 6773, 6774, 6775, 6776, 6777, 6778, 6780, 6782, 6783, 6784, 6785, 6786, 6787, 6788, 6789, 6790,
				6791, 6792, 6793, 6794, 6795, 6796, 6797, 6798, 6799, 6800, 6801, 6802, 6803, 6804, 6805, 6806, 6807, 6808, 6809, 6810,
				6811, 6812, 6813, 6814, 6815, 6816, 6817, 6818, 6819, 6820, 6821, 6822, 6823, 6824, 6825, 6826, 6827, 6828, 6829, 6830,
				6831, 6832, 6833, 6834, 6835, 6836, 6837, 6838, 6839, 6840, 6841, 6842, 6843, 6844, 6845, 6846, 6847, 6848, 6849, 6850,
				6851, 6852, 6853, 6854, 6855, 6856, 6857, 6858, 6859, 6860, 6861, 6862, 6863, 6864, 6865, 6866, 6867, 6868, 6869, 6870,
				6871, 6872, 6873, 6874, 6875, 6876, 6877, 6878, 6879, 6880, 6881, 6882, 6883, 6884, 6885, 6886, 6887, 6888, 6889, 6890,
				6891, 6892, 6893, 6894, 6895, 6896, 6897, 6898, 6899, 6900, 6901, 6902, 6903, 6904, 6905, 6906, 6907, 6908, 6909, 6910,
				6911, 6912, 6913, 6914, 6915, 6916, 6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924, 6925, 6926, 6927, 6928, 6929, 6930,
				6931, 6932, 6933, 6934, 6935, 6936, 6937, 6938, 6939, 6940, 6941, 6942, 6943, 6944, 6945, 6946, 6947, 6948, 6949, 6950,
				6951, 6952, 6953, 6954, 6955, 6956, 6957, 6958, 6959, 6960, 6961, 6962, 6963, 6964, 6965, 6966, 6967, 6968, 6969, 6970,
				6971, 6972, 6973, 6974, 6975, 6976, 6977, 6978, 6979, 6980, 6981, 6982, 6983, 6984, 6985, 6986, 6987, 6988, 6989, 6990,
				6991, 6992, 6993, 6994, 6995, 6996, 6997, 6998, 6999, 7000, 7001, 7002, 7003, 7006, 7007, 7008, 7009, 7010, 7011, 7012,
				7013, 7014, 7015, 7016, 7017, 7018, 7019, 7020, 7021, 7022, 7023, 7024, 7025, 7026, 7027, 7028, 7029, 7030, 7031, 7032,
				7033, 7034, 7035, 7036, 7037, 7038, 7039, 7040, 7041, 7042, 7043, 7044, 7045, 7046, 7047, 7048, 7049, 7050, 7051, 7052,
				7053, 7054, 7055, 7056, 7057, 7058, 7059, 7060, 7061, 7062, 7063, 7064, 7065, 7066, 7067, 7068, 7069, 7070, 7071, 7072,
				7073, 7074, 7075, 7076, 7077, 7078, 7079, 7080, 7081, 7082, 7083, 7084, 7085, 7086, 7087, 7088, 7089, 7090, 7091, 7092,
				7093, 7094, 7095, 7096, 7097, 7098, 7099, 7100, 7101, 7102, 7103, 7104, 7105, 7106, 7107, 7108, 7109, 7110, 7111, 7112,
				7113, 7114, 7115, 7116, 7117, 7118, 7119, 7120, 7121, 7122, 7123, 7124, 7125, 7126, 7127, 7128, 7129, 7130, 7131, 7132,
				7133, 7134, 7135, 7136, 7137, 7138, 7139, 7140, 7141, 7142, 7143, 7144, 7145, 7146, 7147, 7148, 7149, 7150, 7151, 7152,
				7153, 7154, 7155, 7156, 7157, 7158, 7159, 7160, 7161, 7162, 7163, 7164, 7165, 7166, 7167, 7168, 7169, 7170, 7171, 7172,
				7173, 7175, 7176, 7177, 7178, 7179, 7180, 7183, 7184, 7185, 7186, 7187, 7188, 7189, 7190, 7191, 7192, 7193, 7194, 7195,
				7196, 7197, 7198, 7199, 7200, 7201, 7202, 7203, 7204, 7205, 7206, 7207, 7208, 7209, 7210, 7211, 7212, 7213, 7214, 7215,
				7216, 7217, 7218, 7219, 7220, 7221, 7222, 7223, 7224, 7225, 7226, 7227, 7228, 7229, 7230, 7231, 7232, 7233, 7234, 7235,
				7236, 7237, 7238, 7239, 7240, 7241, 7242, 7243, 7244, 7245, 7246, 7247, 7248, 7249, 7250, 7251, 7252, 7253, 7254, 7255,
				7256, 7257, 7258, 7259, 7260, 7261, 7262, 7263, 7264, 7265, 7266, 7267, 7268, 7269, 7270, 7271, 7272, 7273, 7274, 7275,
				7276, 7277, 7278, 7279, 7280, 7281, 7282, 7283, 7284, 7285, 7286, 7287, 7288, 7289, 7290, 7291, 7292, 7293, 7294, 7295,
				7296, 7297, 7298, 7299, 7300, 7301, 7302, 7303, 7304, 7305, 7306, 7307, 7308, 7309, 7310, 7311, 7312, 7313, 7314, 7315,
				7316, 7317, 7318, 7319, 7320, 7321, 7322, 7323, 7324, 7325, 7326, 7327, 7328, 7329, 7330, 7331, 7332, 7333, 7334, 7335,
				7336, 7337, 7338, 7339, 7340, 7341, 7342, 7343, 7344, 7345, 7346, 7347, 7348, 7349, 7350, 7351, 7352, 7353, 7354, 7355,
				7356, 7357, 7358, 7359, 7360, 7361, 7362, 7363, 7364, 7365, 7366, 7367, 7368, 7369, 7370, 7371, 7372, 7373, 7374, 7375,
				7376, 7377, 7378, 7379, 7380, 7381, 7382, 7383, 7384, 7385, 7386, 7387, 7388, 7389, 7390, 7391, 7392, 7393, 7394, 7395,
				7396, 7397, 7398, 7399, 7400, 7401, 7402, 7403, 7404, 7405, 7406, 7407, 7408, 7409, 7410, 7411, 7412, 7413, 7414, 7415,
				7416, 7417, 7418, 7419, 7420, 7421, 7422, 7423, 7424, 7426, 7427, 7428, 7429, 7430, 7431, 7432, 7433, 7434, 7435, 7436,
				7437, 7438, 7439, 7440, 7441, 7442, 7443, 7444, 7445, 7446, 7447, 7448, 7449, 7450, 7451, 7452, 7453, 7454, 7455, 7456,
				7457, 7458, 7459, 7460, 7461, 7462, 7463, 7464, 7465, 7466, 7467, 7468, 7469, 7470, 7471, 7472, 7473, 7529, 7530, 7531,
				7532, 7533, 7534, 7535, 7536, 7537, 7538, 7539, 7540, 7541, 7542, 7543, 7544, 7545, 7546, 7547, 7548, 7549, 7550, 7551,
				7552, 7553, 7554, 7555, 7556, 7557, 7558, 7559, 7560, 7561, 7562, 7563, 7564, 7565, 7566, 7567, 7568, 7569, 7570, 7571,
				7572, 7573, 7574, 7575, 7576, 7577, 7578, 7579, 7580, 7582, 7583, 7584, 7585, 7586, 8476, 8477, 8478, 8479, 8480, 8481,
				8482, 8483, 8484, 8485, 8486, 8487, 8488, 8490, 8491, 8492, 8493, 8494, 8495, 8496, 8497, 8498, 8499, 8500, 8501, 8502,
				8503, 8504, 8505, 8506, 8507, 8508, 8509, 8510, 8511, 8512, 8513, 8514, 8515, 8516, 8517, 8518, 8519, 8520, 8521, 8522,
				8523, 8524, 8525, 8526, 8527, 8528, 8529, 8530, 8531, 8538, 8539, 8540, 8541, 8542, 8543, 8544, 8545, 8546, 8547, 8548,
				8549, 8550, 8551, 8552, 8553, 8554, 8555, 8556, 8557, 8558, 8567, 8568, 8569, 8570, 8571, 8572, 8573, 8574, 8575, 8576,
				8577, 8578, 8579, 8580, 8581, 8582, 8583, 8584, 8585, 8586, 8587, 8588, 8589, 8590, 8591, 8592, 8593, 8594, 8595, 8596,
				8598, 8599, 8600, 8601, 8602, 8603, 8604, 8605, 8606, 8607, 8608, 8609, 8610, 8611, 8612, 8613, 8614, 8615, 8616, 8617,
				8618, 8619, 8620, 8621, 8622, 8623, 8624, 8625, 8626, 8627, 8628, 8629, 8630, 8631, 8632, 8633, 8634, 8635, 8636, 8637,
				8638, 8639, 8640, 8641, 8642, 8643, 8644, 8645, 8646, 8647, 8648, 8649, 8650, 8651, 8652, 8653, 8654, 8655, 8656, 8657,
				8658, 8659, 8660, 8661, 8662, 8663, 8664, 8665, 8666, 8667, 8668, 8669, 8670, 8671, 8672, 8673, 8674, 8675, 8676, 8677,
				8678, 8679, 8680, 8681, 8682, 8683, 8684, 8685, 8686, 8687, 8688, 8689, 8690, 8691, 8692, 8693, 8694, 8695, 8696, 8697,
				8698, 8699, 8700, 8701, 8702, 8703, 8704, 8705, 8706, 8707, 8708, 8709, 8710, 8711, 8712, 8713, 8714, 8715, 8716, 8717,
				8718, 8719, 8720, 8721, 8722, 8723, 8724, 8725, 8726, 8727, 8728, 8729, 8730, 8731, 8732, 8733, 8734, 8735, 8736, 8737,
				8738, 8739, 8740, 8741, 8742, 8743, 8744, 8745, 8746, 8747, 8748, 8749, 8750, 8751, 8752, 8753, 8754, 8755, 8756, 8757,
				8758, 8759, 8760, 8761, 8762, 8763, 8764, 8765, 8766, 8767, 8768, 8769, 8777, 8778, 8779, 8780, 8781, 8782, 8783, 8784,
				8785, 8786, 8787, 8788, 8789, 8790, 8791, 8792, 8793, 8794, 8795, 8796, 8797, 8798, 8799, 8800, 8801, 8802, 8803, 8804,
				8805, 8806, 8807, 8808, 8809, 8810, 8811, 8812, 8813, 8814, 8815, 8818, 8819, 8820, 8821, 8822, 8823, 8824, 8825, 8826,
				8827, 8828, 8829, 8830, 8831, 8832, 8833, 8834, 8835, 8836, 8837, 8838, 8839, 8840, 8841, 8842, 8843, 8844, 8845, 8846,
				8847, 8848, 8849, 8850, 8851, 8852, 8853, 8854, 8855, 8858, 8859, 8860, 8861, 8862, 8863, 8864, 8865, 8866, 8867, 8868,
				8869, 8870, 8871, 8872, 8873, 8874, 8875, 8876, 8877, 8878, 8879, 8880, 8881, 8882, 8883, 8884, 8885, 8886, 8887, 8888,
				8889, 8890, 8891, 8892, 8893, 8894, 8895, 8896, 8897, 8898, 8899, 8900, 8901, 8902, 8903, 8904, 8905, 8906, 8907, 8909,
				8914, 8915, 8916, 8917, 8918, 8919, 8921, 8922, 8923, 8924, 8925, 8926, 8927, 8928, 8929, 8930, 8931, 8932, 8933, 8934,
				8935, 8936, 8937, 8938, 8939, 8940, 8941, 8942, 8943, 8944, 8945, 8946, 8947, 8948, 8949, 8950, 8951, 8952, 8953, 8954,
				8955, 8956, 8957, 8958, 8959, 8960, 8961, 8962, 8963, 8964, 8965, 8966, 8967, 8968, 8969, 8970, 8971, 8972, 8973, 8974,
				8975, 8976, 8977, 8978, 8979, 8980, 8981, 8982, 8983, 8984, 8985, 8986, 8987, 8988, 8989, 8990, 8991, 8992, 8993, 8994,
				8995, 8996, 8997, 8998, 8999
		};
		private readonly int[] h2Array = new int[5405]
		{
				27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 59, 60, 61, 62, 63, 64, 65,
				66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96,
				97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121,
				122, 123, 124, 125, 126, sbyte.MaxValue, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
				144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168,
				169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193,
				194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218,
				219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243,
				244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, byte.MaxValue, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266,
				267, 268, 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288, 289, 290, 291,
				292, 293, 294, 295, 296, 297, 298, 303, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 317, 318, 319, 320, 321, 322, 326,
				327, 328, 329, 330, 331, 332, 333, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350, 351,
				352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376,
				377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401,
				402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426,
				427, 428, 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449, 450, 451,
				452, 453, 454, 455, 456, 457, 458, 459, 460, 461, 462, 463, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488,
				489, 490, 491, 492, 493, 494, 495, 496, 497, 498, 499, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 510, 511, 512, 513,
				514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538,
				539, 540, 541, 542, 543, 544, 545, 546, 547, 548, 549, 550, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563,
				564, 565, 566, 567, 568, 569, 570, 571, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600,
				601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622, 623, 624, 625,
				626, 627, 628, 629, 630, 631, 632, 633, 634, 635, 636, 637, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650,
				651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675,
				676, 677, 678, 679, 680, 681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 693, 694, 695, 696, 697, 698, 699, 700,
				701, 702, 703, 704, 705, 706, 707, 708, 709, 710, 711, 712, 713, 714, 715, 716, 717, 718, 719, 720, 721, 722, 723, 724, 725,
				726, 727, 728, 729, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 740, 741, 742, 743, 744, 745, 746, 747, 748, 749, 750,
				751, 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771, 772, 773, 774, 775,
				776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800,
				801, 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821, 822, 823, 824, 825,
				826, 827, 828, 829, 830, 831, 832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846, 847, 848, 849, 850,
				851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866, 867, 868, 869, 870, 871, 872, 873, 874, 875,
				876, 877, 878, 879, 880, 881, 882, 883, 884, 885, 886, 887, 888, 889, 890, 891, 892, 893, 894, 895, 916, 917, 918, 919, 920,
				921, 922, 923, 924, 925, 926, 927, 928, 929, 930, 931, 932, 933, 934, 935, 936, 937, 938, 939, 940, 941, 942, 943, 944, 945,
				946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 970,
				971, 972, 973, 974, 975, 976, 977, 978, 979, 980, 981, 982, 983, 984, 985, 986, 987, 988, 989, 990, 991, 992, 993, 994, 995,
				996, 997, 998, 999, 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014, 1015, 1016,
				1017, 1018, 1019, 1020, 1021, 1022, 1023, 1024, 1025, 1026, 1027, 1028, 1029, 1030, 1031, 1032, 1033, 1034, 1035, 1036,
				1037, 1038, 1039, 1040, 1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051, 1052, 1053, 1054, 1055, 1056,
				1057, 1058, 1059, 1060, 1061, 1062, 1063, 1064, 1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073, 1074, 1075, 1076,
				1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084, 1085, 1086, 1087, 1088, 1089, 1090, 1091, 1092, 1093, 1094, 1095, 1096,
				1097, 1098, 1099, 1100, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109, 1110, 1111, 1112, 1113, 1114, 1115, 1116,
				1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135, 1136,
				1137, 1138, 1139, 1140, 1141, 1142, 1143, 1144, 1145, 1146, 1147, 1148, 1149, 1150, 1151, 1152, 1153, 1154, 1155, 1156,
				1157, 1158, 1159, 1160, 1161, 1162, 1163, 1164, 1165, 1166, 1167, 1168, 1169, 1170, 1171, 1172, 1173, 1174, 1175, 1176,
				1177, 1178, 1179, 1180, 1181, 1182, 1183, 1184, 1185, 1186, 1187, 1188, 1189, 1190, 1191, 1192, 1193, 1194, 1195, 1196,
				1197, 1198, 1199, 1200, 1201, 1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216,
				1217, 1218, 1219, 1220, 1221, 1222, 1223, 1224, 1225, 1226, 1227, 1228, 1229, 1230, 1231, 1232, 1233, 1234, 1235, 1236,
				1237, 1238, 1239, 1240, 1241, 1242, 1243, 1244, 1245, 1246, 1247, 1248, 1249, 1250, 1251, 1252, 1253, 1254, 1255, 1256,
				1257, 1258, 1259, 1260, 1261, 1262, 1263, 1264, 1265, 1266, 1267, 1268, 1269, 1270, 1271, 1272, 1273, 1274, 1275, 1276,
				1277, 1278, 1279, 1280, 1281, 1282, 1283, 1284, 1285, 1286, 1287, 1288, 1289, 1290, 1291, 1292, 1293, 1294, 1295, 1296,
				1297, 1298, 1299, 1300, 1301, 1302, 1303, 1304, 1305, 1306, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316,
				1317, 1318, 1319, 1320, 1321, 1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1332, 1333, 1334, 1335, 1336,
				1337, 1338, 1339, 1340, 1341, 1342, 1343, 1344, 1345, 1346, 1347, 1348, 1349, 1350, 1351, 1352, 1353, 1354, 1355, 1356,
				1357, 1358, 1359, 1360, 1361, 1362, 1363, 1364, 1365, 1366, 1367, 1368, 1369, 1370, 1371, 1372, 1373, 1374, 1375, 1376,
				1377, 1378, 1379, 1380, 1381, 1382, 1383, 1384, 1385, 1386, 1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396,
				1397, 1398, 1399, 1400, 1401, 1402, 1403, 1404, 1405, 1406, 1407, 1408, 1409, 1410, 1411, 1412, 1413, 1414, 1415, 1416,
				1417, 1418, 1419, 1420, 1421, 1422, 1423, 1424, 1425, 1426, 1427, 1428, 1429, 1430, 1431, 1432, 1433, 1434, 1435, 1436,
				1437, 1438, 1439, 1440, 1441, 1442, 1443, 1444, 1445, 1446, 1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454, 1455, 1456,
				1457, 1458, 1459, 1460, 1461, 1462, 1463, 1464, 1465, 1466, 1467, 1468, 1469, 1470, 1471, 1472, 1473, 1474, 1475, 1476,
				1477, 1478, 1479, 1480, 1481, 1482, 1483, 1484, 1485, 1486, 1487, 1488, 1489, 1490, 1491, 1492, 1493, 1494, 1495, 1496,
				1497, 1498, 1499, 1500, 1501, 1502, 1503, 1504, 1505, 1506, 1507, 1508, 1509, 1510, 1511, 1512, 1513, 1514, 1515, 1516,
				1517, 1518, 1519, 1520, 1521, 1522, 1523, 1528, 1529, 1530, 1531, 1532, 1533, 1534, 1535, 1536, 1537, 1538, 1539, 1540,
				1541, 1542, 1543, 1544, 1545, 1546, 1547, 1548, 1549, 1550, 1551, 1552, 1553, 1554, 1555, 1556, 1557, 1558, 1559, 1560,
				1561, 1562, 1563, 1564, 1565, 1566, 1567, 1568, 1569, 1570, 1571, 1572, 1573, 1574, 1575, 1576, 1577, 1578, 1579, 1580,
				1581, 1582, 1583, 1584, 1585, 1586, 1587, 1588, 1589, 1590, 1591, 1592, 1593, 1594, 1595, 1604, 1605, 1606, 1607, 1608,
				1609, 1610, 1611, 1612, 1613, 1614, 1615, 1616, 1617, 1618, 1619, 1620, 1621, 1622, 1623, 1624, 1625, 1626, 1627, 1628,
				1629, 1630, 1631, 1632, 1633, 1634, 1635, 1636, 1637, 1638, 1639, 1640, 1641, 1642, 1643, 1644, 1645, 1646, 1647, 1648,
				1649, 1650, 1651, 1652, 1653, 1654, 1655, 1656, 1657, 1658, 1659, 1660, 1661, 1662, 1663, 1664, 1665, 1666, 1667, 1668,
				1669, 1670, 1671, 1672, 1673, 1674, 1675, 1676, 1677, 1678, 1679, 1680, 1681, 1682, 1683, 1684, 1685, 1686, 1687, 1688,
				1689, 1690, 1691, 1692, 1693, 1694, 1695, 1696, 1697, 1698, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706, 1707, 1708,
				1709, 1710, 1711, 1712, 1713, 1714, 1715, 1716, 1717, 1718, 1719, 1720, 1721, 1722, 1723, 1724, 1725, 1726, 1727, 1728,
				1729, 1730, 1731, 1732, 1733, 1734, 1735, 1736, 1737, 1738, 1739, 1740, 1741, 1742, 1743, 1744, 1745, 1746, 1747, 1748,
				1749, 1750, 1751, 1752, 1753, 1754, 1755, 1756, 1757, 1758, 1759, 1760, 1761, 1762, 1763, 1764, 1765, 1766, 1767, 1768,
				1769, 1770, 1771, 1772, 1773, 1774, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782, 1783, 1784, 1785, 1786, 1787, 1788,
				1789, 1790, 1791, 1792, 1793, 1794, 1795, 1796, 1797, 1798, 1799, 1800, 1801, 1802, 1803, 1804, 1805, 1806, 1807, 1808,
				1809, 1810, 1811, 1812, 1813, 1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823, 1824, 1825, 1826, 1827, 1828,
				1829, 1830, 1831, 1832, 1833, 1834, 1835, 1836, 1837, 1838, 1839, 1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848,
				1849, 1850, 1851, 1852, 1853, 1854, 1855, 1856, 1857, 1858, 1859, 1860, 1861, 1862, 1863, 1864, 1865, 1866, 1867, 1868,
				1869, 1870, 1871, 1872, 1873, 1874, 1875, 1876, 1877, 1878, 1879, 1880, 1881, 1882, 1883, 1884, 1885, 1886, 1887, 1888,
				1889, 1890, 1891, 1892, 1893, 1894, 1895, 1896, 1897, 1898, 1899, 1900, 1901, 1902, 1903, 1904, 1905, 1906, 1907, 1908,
				1909, 1910, 1911, 1912, 1913, 1914, 1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922, 1923, 1924, 1925, 1926, 1927, 1928,
				1929, 1930, 1931, 1932, 1933, 1934, 1935, 1936, 1937, 1938, 1939, 1940, 1941, 1942, 1943, 1944, 1945, 1946, 1947, 1948,
				1949, 1950, 1951, 1952, 1953, 1954, 1955, 1956, 1957, 1958, 1959, 1960, 1961, 1962, 1963, 1964, 1965, 1966, 1967, 1968,
				1969, 1970, 1971, 1972, 1973, 1974, 1975, 1976, 1977, 1978, 1979, 1980, 1981, 1982, 1983, 1984, 1985, 1987, 1988, 1989,
				1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009,
				2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029,
				2030, 2031, 2032, 2033, 2034, 2035, 2036, 2037, 2038, 2039, 2040, 2041, 2042, 2043, 2044, 2045, 2046, 2047, 2048, 2049,
				2050, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2058, 2059, 2060, 2061, 2062, 2063, 2064, 2065, 2066, 2067, 2068, 2069,
				2070, 2071, 2072, 2073, 2074, 2075, 2076, 2077, 2078, 2079, 2080, 2081, 2082, 2083, 2084, 2085, 2086, 2087, 2088, 2089,
				2090, 2091, 2092, 2093, 2094, 2095, 2096, 2097, 2098, 2099, 2100, 2101, 2102, 2103, 2104, 2105, 2106, 2107, 2108, 2109,
				2110, 2111, 2112, 2113, 2114, 2115, 2116, 2117, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2125, 2126, 2127, 2128, 2129,
				2130, 2131, 2132, 2133, 2134, 2135, 2136, 2137, 2138, 2139, 2140, 2141, 2142, 2143, 2144, 2145, 2146, 2147, 2148, 2149,
				2150, 2151, 2152, 2153, 2154, 2155, 2156, 2157, 2158, 2159, 2160, 2161, 2162, 2163, 2164, 2165, 2166, 2167, 2168, 2169,
				2170, 2171, 2172, 2173, 2174, 2175, 2176, 2177, 2178, 2179, 2180, 2181, 2182, 2183, 2184, 2185, 2186, 2187, 2188, 2189,
				2190, 2191, 2192, 2193, 2194, 2195, 2196, 2197, 2198, 2199, 2200, 2201, 2202, 2203, 2204, 2205, 2206, 2207, 2208, 2209,
				2210, 2211, 2212, 2213, 2214, 2215, 2216, 2217, 2218, 2219, 2220, 2221, 2222, 2223, 2224, 2225, 2226, 2227, 2228, 2229,
				2230, 2231, 2232, 2233, 2234, 2235, 2236, 2237, 2238, 2239, 2240, 2241, 2242, 2243, 2244, 2245, 2246, 2247, 2248, 2249,
				2250, 2251, 2252, 2253, 2254, 2255, 2256, 2257, 2258, 2259, 2260, 2261, 2262, 2263, 2264, 2265, 2266, 2267, 2268, 2269,
				2270, 2271, 2272, 2273, 2274, 2275, 2276, 2277, 2278, 2279, 2280, 2281, 2282, 2283, 2284, 2285, 2286, 2287, 2288, 2289,
				2290, 2291, 2292, 2293, 2294, 2295, 2296, 2297, 2298, 2299, 2300, 2301, 2302, 2303, 2304, 2305, 2306, 2307, 2308, 2309,
				2310, 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2326, 2327, 2328, 2329,
				2330, 2331, 2332, 2333, 2334, 2335, 2336, 2337, 2338, 2339, 2340, 2341, 2342, 2343, 2344, 2345, 2346, 2347, 2348, 2349,
				2350, 2351, 2352, 2353, 2354, 2355, 2356, 2357, 2358, 2359, 2360, 2361, 2362, 2363, 2364, 2365, 2366, 2367, 2368, 2369,
				2370, 2371, 2372, 2373, 2374, 2375, 2376, 2377, 2378, 2379, 2380, 2381, 2382, 2383, 2384, 2385, 2386, 2387, 2388, 2389,
				2390, 2391, 2392, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2402, 2403, 2404, 2405, 2406, 2407, 2408, 2409,
				2410, 2411, 2412, 2413, 2414, 2415, 2416, 2417, 2418, 2419, 2420, 2421, 2422, 2423, 2424, 2425, 2426, 2427, 2428, 2429,
				2430, 2431, 2432, 2433, 2434, 2435, 2436, 2437, 2438, 2439, 2440, 2441, 2442, 2443, 2444, 2445, 2446, 2447, 2448, 2449,
				2450, 2451, 2452, 2453, 2454, 2455, 2456, 2457, 2458, 2459, 2460, 2461, 2462, 2463, 2464, 2465, 2466, 2467, 2468, 2469,
				2470, 2471, 2472, 2473, 2474, 2475, 2476, 2477, 2478, 2479, 2480, 2481, 2482, 2483, 2484, 2485, 2486, 2487, 2488, 2489,
				2490, 2525, 2526, 2527, 2528, 2529, 2530, 2531, 2532, 2533, 2534, 2535, 2536, 2537, 2538, 2539, 2540, 2541, 2542, 2543,
				2544, 2545, 2546, 2547, 2548, 2549, 2550, 2551, 2552, 2553, 2554, 2555, 2556, 2557, 2558, 2559, 2560, 2561, 2562, 2563,
				2564, 2565, 2566, 2567, 2568, 2569, 2570, 2571, 2572, 2573, 2574, 2575, 2576, 2577, 2578, 2579, 2580, 2581, 2582, 2583,
				2584, 2585, 2586, 2587, 2588, 2589, 2590, 2591, 2592, 2593, 2594, 2595, 2596, 2597, 2598, 2599, 2600, 2601, 2602, 2603,
				2604, 2605, 2606, 2607, 2608, 2609, 2610, 2611, 2612, 2613, 2614, 2615, 2616, 2617, 2618, 2619, 2620, 2621, 2622, 2623,
				2624, 2625, 2626, 2627, 2628, 2629, 2630, 2631, 2632, 2633, 2634, 2635, 2636, 2637, 2638, 2639, 2640, 2641, 2642, 2643,
				2644, 2645, 2646, 2647, 2648, 2649, 2650, 2651, 2652, 2653, 2654, 2655, 2656, 2657, 2658, 2659, 2660, 2661, 2662, 2663,
				2664, 2665, 2666, 2667, 2668, 2669, 2670, 2671, 2672, 2673, 2674, 2675, 2676, 2677, 2678, 2679, 2680, 2681, 2682, 2683,
				2684, 2685, 2686, 2687, 2688, 2689, 2690, 2691, 2692, 2693, 2694, 2695, 2696, 2697, 2698, 2699, 2700, 2701, 2702, 2703,
				2704, 2705, 2706, 2707, 2708, 2709, 2710, 2711, 2712, 2713, 2714, 2715, 2716, 2717, 2718, 2719, 2720, 2721, 2722, 2723,
				2724, 2725, 2726, 2727, 2728, 2729, 2730, 2731, 2732, 2733, 2734, 2735, 2736, 2737, 2738, 2739, 2740, 2741, 2742, 2743,
				2744, 2745, 2746, 2747, 2748, 2749, 2750, 2751, 2752, 2753, 2754, 2755, 2756, 2757, 2758, 2759, 2760, 2761, 2762, 2763,
				2764, 2765, 2766, 2767, 2768, 2769, 2770, 2771, 2772, 2773, 2774, 2775, 2776, 2777, 2778, 2779, 2780, 2781, 2782, 2783,
				2784, 2785, 2786, 2787, 2788, 2789, 2790, 2791, 2792, 2793, 2794, 2795, 2796, 2797, 2798, 2799, 2800, 2801, 2802, 2803,
				2804, 2805, 2806, 2808, 2809, 2810, 2811, 2812, 2813, 2814, 2816, 2817, 2818, 2819, 2820, 2821, 2822, 2823, 2824, 2825,
				2826, 2827, 2828, 2829, 2830, 2831, 2832, 2833, 2834, 2835, 2836, 2837, 2838, 2839, 2840, 2841, 2842, 2843, 2844, 2845,
				2846, 2847, 2848, 2849, 2850, 2851, 2852, 2853, 2854, 2855, 2856, 2857, 2858, 2859, 2860, 2861, 2862, 2863, 2888, 2889,
				2890, 2891, 2892, 2893, 2894, 2895, 2896, 2897, 2898, 2899, 2900, 2901, 2902, 2903, 2904, 2905, 2906, 2907, 2908, 2909,
				2910, 2911, 2912, 2913, 2914, 2915, 2916, 2917, 2918, 2919, 2920, 2921, 2922, 2923, 2924, 2925, 2926, 2927, 2928, 2929,
				2930, 2931, 2932, 2933, 2934, 2935, 2936, 2937, 2938, 2939, 2940, 2941, 2942, 2943, 2944, 2945, 2946, 2947, 2948, 2949,
				2950, 2951, 2952, 2953, 2954, 2955, 2956, 2957, 2958, 2959, 2960, 2961, 2962, 2963, 2964, 2965, 2966, 2967, 2968, 2969,
				2970, 2971, 2972, 2973, 2974, 2975, 2976, 2977, 2978, 2979, 2980, 2981, 2982, 2983, 2984, 2985, 2986, 2987, 2988, 2989,
				2990, 2991, 2992, 2993, 2994, 2995, 2996, 2997, 2998, 2999, 3000, 3001, 3002, 3003, 3004, 3005, 3006, 3007, 3008, 3009,
				3010, 3011, 3012, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021, 3022, 3023, 3024, 3025, 3026, 3027, 3028, 3029,
				3030, 3031, 3032, 3033, 3034, 3035, 3036, 3037, 3038, 3039, 3040, 3041, 3042, 3043, 3044, 3045, 3046, 3047, 3048, 3049,
				3050, 3051, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061, 3062, 3063, 3064, 3065, 3066, 3067, 3068, 3069,
				3070, 3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081, 3082, 3083, 3084, 3085, 3086, 3087, 3088, 3089,
				3090, 3091, 3092, 3093, 3094, 3095, 3096, 3097, 3098, 3099, 3100, 3101, 3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109,
				3110, 3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121, 3122, 3123, 3124, 3125, 3126, 3127, 3128, 3129,
				3130, 3131, 3132, 3133, 3134, 3135, 3136, 3137, 3138, 3139, 3140, 3141, 3142, 3143, 3144, 3145, 3146, 3147, 3188, 3189,
				3190, 3191, 3192, 3193, 3194, 3195, 3196, 3197, 3198, 3199, 3200, 3201, 3202, 3203, 3204, 3205, 3206, 3207, 3208, 3209,
				3210, 3211, 3212, 3213, 3214, 3215, 3216, 3217, 3218, 3219, 3220, 3221, 3222, 3223, 3224, 3225, 3226, 3227, 3228, 3229,
				3230, 3231, 3240, 3241, 3242, 3243, 3244, 3245, 3246, 3247, 3248, 3249, 3250, 3251, 3252, 3253, 3254, 3255, 3256, 3257,
				3258, 3259, 3260, 3261, 3262, 3263, 3264, 3265, 3266, 3267, 3268, 3269, 3270, 3271, 3272, 3273, 3274, 3275, 3276, 3277,
				3278, 3279, 3280, 3281, 3282, 3283, 3284, 3285, 3286, 3287, 3288, 3289, 3290, 3291, 3292, 3293, 3294, 3295, 3296, 3297,
				3298, 3299, 3304, 3305, 3306, 3307, 3308, 3309, 3310, 3311, 3312, 3313, 3314, 3315, 3316, 3317, 3318, 3319, 3320, 3321,
				3322, 3323, 3332, 3333, 3334, 3335, 3336, 3337, 3338, 3339, 3340, 3341, 3342, 3343, 3344, 3345, 3346, 3347, 3348, 3349,
				3350, 3351, 3352, 3353, 3354, 3355, 3356, 3357, 3358, 3359, 3360, 3361, 3362, 3363, 3364, 3365, 3366, 3367, 3368, 3369,
				3370, 3371, 3372, 3373, 3374, 3375, 3376, 3377, 3378, 3379, 3380, 3381, 3382, 3383, 3384, 3385, 3386, 3387, 3388, 3389,
				3390, 3391, 3392, 3393, 3394, 3395, 3396, 3397, 3398, 3399, 3400, 3401, 3402, 3403, 3404, 3405, 3406, 3407, 3408, 3409,
				3410, 3411, 3412, 3413, 3414, 3415, 3416, 3417, 3418, 3419, 3420, 3421, 3422, 3423, 3424, 3425, 3426, 3427, 3428, 3429,
				3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438, 3439, 3440, 3441, 3442, 3443, 3444, 3445, 3446, 3447, 3448, 3449,
				3450, 3451, 3452, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466, 3467, 3468, 3469,
				3470, 3471, 3472, 3473, 3474, 3475, 3476, 3477, 3478, 3479, 3480, 3481, 3482, 3483, 3484, 3485, 3486, 3487, 3488, 3489,
				3490, 3491, 3492, 3493, 3494, 3495, 3496, 3497, 3498, 3499, 3500, 3501, 3502, 3503, 3504, 3505, 3506, 3507, 3508, 3509,
				3510, 3511, 3512, 3513, 3514, 3515, 3516, 3517, 3518, 3519, 3520, 3521, 3522, 3523, 3524, 3525, 3526, 3527, 3528, 3529,
				3530, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541, 3542, 3543, 3544, 3545, 3546, 3547, 3548, 3549,
				3550, 3551, 3552, 3553, 3554, 3555, 3556, 3557, 3558, 3559, 3560, 3561, 3562, 3563, 3564, 3565, 3566, 3567, 3568, 3569,
				3570, 3571, 3572, 3573, 3574, 3575, 3576, 3577, 3578, 3579, 3580, 3581, 3582, 3583, 3584, 3585, 3586, 3587, 3588, 3589,
				3590, 3591, 3592, 3593, 3594, 3595, 3596, 3597, 3598, 3599, 3600, 3601, 3602, 3603, 3604, 3605, 3936, 3937, 3938, 3939,
				3940, 3941, 3942, 3943, 3944, 3945, 3946, 3947, 3948, 3949, 3950, 3951, 3952, 3953, 3954, 3955, 3956, 3957, 3958, 3959,
				3960, 3961, 3962, 3963, 3964, 3965, 3966, 3967, 3968, 3969, 3970, 3971, 3972, 3973, 3974, 3975, 3976, 3977, 3978, 3979,
				3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 3988, 3989, 3990, 3991, 3992, 3993, 3994, 3995, 3996, 3997, 3998, 3999,
				4000, 4001, 4002, 4003, 4004, 4005, 4006, 4007, 4008, 4009, 4010, 4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019,
				4020, 4021, 4022, 4023, 4024, 4025, 4026, 4027, 4028, 4029, 4030, 4031, 4032, 4033, 4034, 4035, 4036, 4037, 4038, 4039,
				4040, 4041, 4042, 4043, 4044, 4045, 4046, 4047, 4048, 4049, 4050, 4051, 4052, 4053, 4054, 4055, 4056, 4057, 4058, 4059,
				4060, 4061, 4062, 4063, 4064, 4065, 4066, 4067, 4068, 4069, 4070, 4071, 4072, 4073, 4074, 4075, 4076, 4077, 4078, 4079,
				4080, 4081, 4082, 4083, 4084, 4085, 4086, 4087, 4088, 4089, 4090, 4091, 4092, 4093, 4094, 4095, 4096, 4097, 4098, 4099,
				4100, 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 4119,
				4120, 4121, 4122, 4123, 4124, 4125, 4126, 4127, 4128, 4129, 4130, 4131, 4132, 4133, 4134, 4135, 4136, 4137, 4138, 4139,
				4140, 4141, 4142, 4143, 4144, 4145, 4146, 4147, 4148, 4149, 4150, 4151, 4152, 4153, 4154, 4155, 4156, 4157, 4158, 4159,
				4160, 4161, 4162, 4163, 4164, 4165, 4166, 4167, 4168, 4169, 4170, 4171, 4172, 4173, 4174, 4175, 4176, 4177, 4178, 4179,
				4180, 4181, 4182, 4183, 4184, 4185, 4186, 4187, 4188, 4189, 4190, 4191, 4192, 4193, 4194, 4195, 4196, 4197, 4198, 4199,
				4208, 4209, 4210, 4211, 4212, 4213, 4214, 4215, 4216, 4217, 4218, 4219, 4220, 4221, 4222, 4223, 4224, 4225, 4226, 4227,
				4228, 4229, 4230, 4231, 4232, 4233, 4234, 4235, 4236, 4237, 4238, 4239, 4240, 4241, 4242, 4243, 4244, 4245, 4246, 4247,
				4248, 4249, 4250, 4251, 5358, 5359, 5360, 5361, 5362, 5363, 5364, 5365, 5366, 5367, 5368, 5369, 5370, 5371, 5372, 5373,
				5374, 5375, 5376, 5377, 5378, 5379, 5380, 5381, 5382, 5383, 5384, 5385, 5386, 5387, 5388, 5389, 5390, 5391, 5392, 5393,
				5394, 5395, 5396, 5397, 5398, 5399, 5400, 5401, 5402, 5403, 5404, 5405, 5407, 5408, 5409, 5410, 5411, 5412, 5413, 5414,
				5415, 5416, 5417, 5418, 5419, 5420, 5421, 5422, 5423, 5424, 5425, 5426, 5427, 5428, 5429, 5430, 5431, 5432, 5433, 5434,
				5435, 5436, 5437, 5438, 5439, 5440, 5441, 5442, 5443, 5444, 5445, 5446, 5447, 5448, 5449, 5450, 5451, 5452, 5453, 5454,
				5455, 5456, 5457, 5458, 5459, 5460, 5461, 5462, 5463, 5464, 5465, 5466, 5467, 5468, 5469, 5470, 5471, 5472, 5473, 5474,
				5475, 5476, 5477, 5478, 5479, 5480, 5481, 5482, 5483, 5484, 5485, 5486, 5487, 5488, 5489, 5490, 5491, 5492, 5493, 5494,
				5495, 5496, 5497, 5498, 5499, 5500, 5501, 5502, 5503, 5504, 5505, 5506, 5507, 5508, 5509, 5510, 5511, 5512, 5513, 5514,
				5515, 5516, 5517, 5518, 5519, 5520, 5521, 5522, 5523, 5524, 5525, 5526, 5527, 5528, 5529, 5530, 5531, 5532, 5533, 5534,
				5535, 5536, 5537, 5538, 5539, 5540, 5541, 5542, 5543, 5544, 5545, 5546, 5547, 5548, 5549, 5550, 5551, 5552, 5553, 5554,
				5555, 5556, 5557, 5558, 5559, 5560, 5561, 5562, 5563, 5564, 5565, 5566, 5567, 5568, 5569, 5570, 5571, 5572, 5573, 5574,
				5575, 5576, 5577, 5578, 5579, 5580, 5581, 5582, 5583, 5584, 5585, 5586, 5587, 5588, 5589, 5590, 5591, 5592, 5593, 5594,
				5595, 5596, 5597, 5598, 5599, 5600, 5601, 5602, 5603, 5604, 5605, 5606, 5607, 5608, 5609, 5610, 5611, 5612, 5613, 5614,
				5615, 5616, 5617, 5618, 5619, 5620, 5621, 5622, 5623, 5624, 5625, 5626, 5627, 5628, 5629, 5630, 5631, 5632, 5633, 5634,
				5635, 5636, 5637, 5638, 5639, 5640, 5641, 5642, 5643, 5644, 5645, 5646, 5647, 5648, 5649, 5650, 5651, 5652, 5653, 5654,
				5655, 5656, 5657, 5658, 5659, 5660, 5661, 5662, 5663, 5664, 5665, 5666, 5667, 5668, 5669, 5670, 5671, 5672, 5673, 5674,
				5675, 5676, 5677, 5678, 5679, 5680, 5681, 5682, 5683, 5684, 5685, 5686, 5687, 5688, 5689, 5690, 5691, 5692, 5693, 5694,
				5695, 5696, 5697, 5698, 5699, 5700, 5701, 5702, 5703, 5704, 5705, 5706, 5707, 5708, 5709, 5710, 5711, 5712, 5713, 5714,
				5715, 5716, 5717, 5718, 5719, 5720, 5721, 5722, 5723, 5724, 5725, 5726, 5727, 5728, 5729, 5730, 5731, 5732, 5733, 5734,
				5735, 5736, 5737, 5738, 5739, 5740, 5741, 5742, 5743, 5744, 5745, 5746, 5747, 5748, 5749, 5750, 5751, 5752, 5753, 5754,
				5755, 5756, 5757, 5758, 5759, 5760, 5761, 5762, 5763, 5764, 5765, 5766, 5767, 5768, 5769, 5770, 5771, 5772, 5773, 5774,
				5775, 5776, 5777, 5778, 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786, 5787, 5788, 5789, 5790, 5791, 5792, 5793, 5794,
				5795, 5796, 5797, 5798, 5799, 5800, 5801, 5802, 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810, 5811, 5812, 5813, 5814,
				5815, 5816, 5817, 5818, 5819, 5820, 5821, 5822, 5823, 5824, 5825, 5829, 5830, 5831, 5832, 5833, 5834, 5835, 5836, 5837,
				5838, 5839, 5840, 5841, 5842, 5843, 5844, 5845, 5846, 5847, 5848, 5849, 5850, 5851, 5852, 5853, 5854, 5855, 5856, 5857,
				5858, 5859, 5860, 5861, 5862, 5863, 5864, 5865, 5866, 5867, 5868, 5869, 5870, 5871, 5873, 5875, 5876, 5877, 5878, 5879,
				5880, 5881, 5882, 5883, 5884, 5885, 5886, 5887, 5888, 5889, 5890, 5891, 5892, 5893, 5894, 5895, 5896, 5897, 5898, 5899,
				5900, 5901, 5902, 5903, 5904, 5905, 5906, 5907, 5908, 5909, 5910, 5911, 5912, 5913, 5914, 5915, 5916, 5917, 5918, 5919,
				5920, 5921, 5922, 5923, 5924, 5925, 5926, 5927, 5928, 5929, 5930, 5931, 5932, 5933, 5934, 5935, 5936, 5937, 5938, 5939,
				5940, 5941, 5942, 5943, 5944, 5945, 5946, 5947, 5948, 5949, 5950, 5951, 5952, 5953, 5954, 5955, 5956, 5957, 5958, 5959,
				5960, 5961, 5962, 5963, 5964, 5965, 5966, 5967, 5968, 5969, 5970, 5971, 5972, 5973, 5974, 5975, 5976, 5977, 5978, 5979,
				5980, 5981, 5982, 5983, 5984, 5985, 5986, 5987, 5988, 5989, 5990, 5991, 5992, 5993, 5994, 5995, 5996, 5997, 5998, 5999,
				6000, 6001, 6002, 6003, 6004, 6005, 6006, 6007, 6008, 6009, 6010, 6011, 6012, 6013, 6014, 6015, 6016, 6017, 6018, 6019,
				6020, 6021, 6022, 6023, 6024, 6025, 6026, 6027, 6028, 6029, 6030, 6031, 6032, 6033, 6034, 6162, 6163, 6164, 6165, 6166,
				6167, 6168, 6169, 6170, 6171, 6172, 6173, 6174, 6175, 6176, 6177, 6178, 6179, 6180, 6181, 6182, 6183, 6184, 6185, 6186,
				6187, 6188, 6189, 6190, 6191, 6192, 6193, 6194, 6195, 6196, 6197, 6198, 6199, 6200, 6201, 6202, 6203, 6204, 6205, 6206,
				6207, 6208, 6209, 6210, 6211, 6212, 6213, 6214, 6215, 6216, 6217, 6218, 6219, 6220, 6221, 6222, 6223, 6224, 6225, 6226,
				6227, 6228, 6229, 6230, 6231, 6232, 6233, 6234, 6235, 6236, 6237, 6238, 6239, 6240, 6241, 6242, 6243, 6244, 6245, 6246,
				6247, 6248, 6249, 6250, 6251, 6252, 6253, 6254, 6255, 6256, 6257, 6258, 6259, 6260, 6261, 6262, 6263, 6264, 6265, 6266,
				6267, 6268, 6269, 6270, 6271, 6272, 6273, 6274, 6275, 6276, 6277, 6278, 6279, 6280, 6281, 6282, 6283, 6284, 6285, 6286,
				6287, 6288, 6289, 6290, 6291, 6292, 6294, 6295, 6296, 6297, 6298, 6299, 6300, 6301, 6302, 6303, 6304, 6305, 6306, 6307,
				6308, 6309, 6310, 6311, 6312, 6313, 6314, 6315, 6316, 6317, 6318, 6319, 6320, 6321, 6322, 6323, 6324, 6325, 6326, 6327,
				6328, 6329, 6330, 6331, 6332, 6333, 6334, 6335, 6336, 6337, 6338, 6339, 6340, 6341, 6342, 6343, 6344, 6348, 6349, 6350,
				6351, 6352, 6353, 6354, 6355, 6356, 6357, 6358, 6359, 6360, 6361, 6362, 6363, 6364, 6365, 6366, 6371, 6372, 6373, 6374,
				6375, 6376, 6377, 6378, 6379, 6380, 6381, 6382, 6383, 6384, 6385, 6386, 6389, 6390, 6393, 6394, 6395, 6396, 6397, 6398,
				6399, 6400, 6401, 6402, 6403, 6404, 6405, 6406, 6407, 6408, 6409, 6410, 6411, 6412, 6413, 6414, 6415, 6416, 6417, 6418,
				6419, 6420, 6421, 6422, 6423, 6424, 6425, 6426, 6427, 6428, 6429, 6430, 6431, 6432, 6433, 6434, 6435, 6436, 6437, 6438,
				6439, 6440, 6441, 6442, 6443, 6444, 6445, 6446, 6447, 6448, 6449, 6462, 6463, 6464, 6465, 6466, 6467, 6468, 6469, 6470,
				6471, 6472, 6473, 6474, 6475, 6476, 6477, 6478, 6479, 6480, 6481, 6482, 6483, 6484, 6485, 6486, 6487, 6488, 6489, 6490,
				6491, 6492, 6493, 6494, 6495, 6496, 6497, 6498, 6499, 6500, 6501, 6502, 6503, 6504, 6505, 6506, 6507, 6508, 6509, 6510,
				6511, 6512, 6513, 6514, 6515, 6516, 6517, 6518, 6519, 6520, 6521, 6522, 6523, 6524, 6525, 6526, 6527, 6528, 6529, 6530,
				6531, 6532, 6533, 6534, 6535, 6536, 6537, 6538, 6539, 6540, 6541, 6542, 6543, 6544, 6545, 6546, 6547, 6548, 6549, 6550,
				6551, 6552, 6553, 6554, 6555, 6556, 6557, 6558, 6559, 6560, 6561, 6562, 6563, 6564, 6565, 6566, 6567, 6568, 6569, 6570,
				6571, 6572, 6573, 6574, 6575, 6576, 6577, 6578, 6579, 6580, 6581, 6582, 6583, 6584, 6585, 6586, 6587, 6588, 6589, 6590,
				6591, 6592, 6593, 6594, 6595, 6596, 6597, 6598, 6599, 6600, 6601, 6602, 6603, 6604, 6605, 6606, 6607, 6608, 6609, 6610,
				6611, 6612, 6613, 6614, 6615, 6616, 6617, 6618, 6619, 6620, 6621, 6622, 6623, 6624, 6625, 6626, 6627, 6628, 6629, 6630,
				6631, 6632, 6633, 6634, 6635, 6636, 6637, 6638, 6639, 6640, 6641, 6642, 6643, 6644, 6645, 6646, 6647, 6648, 6649, 6650,
				6651, 6652, 6653, 6654, 6655, 6656, 6657, 6658, 6659, 6660, 6661, 6662, 6663, 6664, 6665, 8433, 8434, 8435, 8436, 8437,
				8438, 8439, 8440, 8441, 8442, 8443, 8444, 8445, 8446, 8447, 8448, 8449, 8450, 8451, 8452, 8453, 8454, 8455, 8456, 8457,
				8458, 8459, 8460, 8461, 8462, 8463, 8464, 8465, 8466, 8467, 8468, 8469, 8470, 8471, 8472, 8473, 8474, 8475, 8476, 8477,
				8478, 8479, 8480, 8481, 8482, 8483, 8484, 8485, 8486, 8487, 8494, 8495, 8496, 8497, 8498, 8499, 8500, 8501, 8502, 8503,
				8504, 8505, 8506, 8507, 8508, 8509, 8510, 8511, 8512, 8513, 8521, 8522, 8523, 8524, 8525, 8526, 8527, 8528, 8529, 8530,
				8531, 8532, 8533, 8534, 8535, 8536, 8537, 8538, 8539, 8540, 8541, 8542, 8543, 8544, 8545, 8546, 8547, 8548, 8549, 8550,
				8551, 8552, 8553, 8554, 8555, 8556, 8557, 8558, 8559, 8560, 8561, 8562, 8563, 8564, 8565, 8566, 8567, 8568, 8569, 8570,
				8571, 8572, 8573, 8574, 8575, 8576, 8577, 8578, 8579, 8580, 8581, 8582, 8583, 8584, 8585, 8586, 8587, 8588, 8589, 8590,
				8591, 8592, 8593, 8594, 8595, 8596, 8597, 8598, 8599, 8600, 8601, 8602, 8603, 8604, 8605, 8606, 8607, 8608, 8609, 8610,
				8611, 8612, 8613, 8614, 8615, 8616, 8617, 8618, 8619, 8620, 8621, 8622, 8623, 8624, 8625, 8626, 8627, 8628, 8629, 8630,
				8631, 8632, 8633, 8634, 8635, 8636, 8637, 8638, 8639, 8640, 8641, 8642, 8643, 8644, 8645, 8646, 8647, 8648, 8649, 8650,
				8651, 8652, 8653, 8654, 8655, 8656, 8657, 8658, 8659, 8660, 8661, 8662, 8663, 8664, 8665, 8666, 8667, 8668, 8669, 8670,
				8671, 8672, 8673, 8674, 8675, 8676, 8677, 8678, 8679, 8680, 8681, 8682, 8683, 8684, 8685, 8686, 8687, 8688, 8689, 8690,
				8691, 8692, 8693, 8694, 8695, 8696, 8697, 8698, 8699, 8700, 8701, 8702, 8703, 8704, 8705, 8706, 8707, 8708, 8709, 8710,
				8711, 8712, 8713, 8714, 8715, 8716, 8717, 8718, 8719, 8720, 8721, 8722, 8723, 8724, 8725, 8726, 8727, 8728, 8729, 8730,
				8731, 8732, 8733, 8734, 8735, 8736, 8737, 8738, 8739, 8740, 8741, 8742, 8743, 8744, 8745, 8746, 8747, 8748, 8749, 8750,
				8751, 8752, 8753, 8754, 8755, 8756, 8757, 8758, 8759, 8760, 8761, 8762, 8763, 8764, 8765, 8766, 8767, 8768, 8769, 8770,
				8771, 8774, 8775, 8776, 8777, 8778, 8779, 8780, 8781, 8782, 8783, 8784, 8785, 8786, 8787, 8788, 8789, 8790, 8791, 8792,
				8793, 8794, 8795, 8796, 8797, 8798, 8799, 8800, 8801, 8802, 8803, 8804, 8805, 8806, 8807, 8808, 8809, 8810, 8811, 8812,
				8813, 8814, 8815, 8816, 8817, 8818, 8819, 8820, 8821, 8822, 8823, 8824, 8825, 8826, 8827, 8828, 8829, 8830, 8831, 8832,
				8833, 8838, 8839, 8840, 8841, 8842, 8843, 8844, 8845, 8846, 8847, 8848, 8849, 8850, 8851, 8852, 8853, 8854, 8855, 8856,
				8857, 8858, 8859, 8860, 8861, 8862, 8863, 8864, 8865, 8866, 8867, 8868, 8869, 8870, 8871, 8872, 8873, 8874, 8875, 8876,
				8877, 8878, 8879, 8880, 8881, 8882, 8883, 8884, 8885, 8886, 8887, 8888, 8889, 8890, 8891, 8892, 8893, 8894, 8895, 8896,
				8897, 8898, 8899, 8900, 8901, 8902, 8903, 8904, 8905, 8906, 8907, 8908, 8909, 8910, 8911, 8912, 8913, 8914, 8915, 8916,
				8917, 8918, 8919, 8920, 8921, 8922, 8923, 8924, 8925, 8926, 8927, 8928, 8929, 8930, 8931, 8932, 8933, 8934, 8935, 8936,
				8937, 8938, 8946, 8947, 8948, 8949, 8950, 8951, 8952, 8953, 8954, 8955, 8956, 8957, 8958, 8959, 8960, 8961, 8962, 8963,
				8964, 8965, 8966, 8967, 8968, 8969, 8970, 8971, 8972, 8973, 8974, 8975, 8976, 8977, 8978, 8979, 8980, 9827, 9828, 9829,
				9830, 9831, 9832, 9833, 9834, 9843, 9844, 9845, 9846, 9847, 9848, 9849, 9850, 9851, 9852, 9853, 9854, 9855, 9856, 9857,
				9858, 9859, 9860, 9861, 9862, 9863, 9864, 9865, 9866, 9867, 9868, 9869, 9870, 9871, 9872, 9873, 9874
		};
		private readonly int[] h0tArray = new int[2289]
		{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
				33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 105, 1369, 1382, 1383, 1384, 1385, 1386,
				1387, 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397, 1398, 1399, 1400, 1401, 1402, 1403, 1404, 1405, 1406,
				1407, 1408, 1409, 1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1419, 1420, 1421, 1422, 1423, 1424, 1425, 1426,
				1427, 1428, 1429, 1430, 1431, 1432, 1433, 1434, 1435, 1436, 1437, 1438, 1439, 1440, 1441, 1442, 1443, 1444, 1445, 1446,
				1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454, 1455, 1456, 1457, 1458, 1459, 1460, 1461, 1462, 1463, 1464, 1465, 1466,
				1467, 1468, 1469, 1470, 1471, 1472, 1473, 1474, 1475, 1476, 1477, 1478, 1479, 1480, 1481, 1482, 1483, 1484, 1485, 1486,
				1487, 1488, 1489, 1490, 1491, 1492, 1493, 1494, 1495, 1496, 1497, 1498, 1499, 1500, 1501, 1502, 1503, 1504, 1505, 1506,
				1507, 1508, 1509, 1510, 1511, 1512, 1513, 1514, 1515, 1516, 1517, 1518, 1519, 1520, 1521, 1522, 1523, 1524, 1525, 1526,
				1527, 1528, 1529, 1530, 1531, 1532, 1533, 1534, 1535, 1536, 1537, 1538, 1539, 1540, 1541, 1542, 1543, 1544, 1545, 1546,
				1547, 1548, 1549, 1550, 1551, 1552, 1553, 1554, 1555, 1556, 1557, 1558, 1559, 1560, 1561, 1562, 1563, 1564, 1565, 1566,
				1567, 1568, 1569, 1570, 1571, 1572, 1573, 1574, 1575, 1576, 1577, 1578, 1579, 1580, 1581, 1582, 1583, 1584, 1585, 1586,
				1587, 1588, 1589, 1590, 1591, 1592, 1593, 1594, 1595, 1596, 1597, 1598, 1599, 1600, 1601, 1602, 1603, 1604, 1605, 1606,
				1607, 1608, 1609, 1610, 1611, 1612, 1613, 1614, 1615, 1616, 1617, 1618, 1619, 1620, 1621, 1622, 1623, 1624, 1625, 1626,
				1627, 1628, 1629, 1630, 1631, 1632, 1633, 1634, 1635, 1636, 1637, 1638, 1639, 1640, 1641, 1642, 1643, 1644, 1645, 1646,
				1647, 1648, 1649, 1650, 1651, 1652, 1653, 1654, 1655, 1656, 1657, 1658, 1659, 1660, 1661, 1662, 1663, 1664, 1665, 1666,
				1667, 1668, 1669, 1670, 1671, 1672, 1673, 1674, 1675, 1676, 1677, 1678, 1679, 1680, 1681, 1682, 1683, 1684, 1685, 1686,
				1687, 1688, 1689, 1690, 1691, 1692, 1693, 1694, 1695, 1696, 1697, 1698, 1699, 1700, 1877, 1878, 1879, 1880, 1881, 1882,
				1883, 1884, 1885, 1886, 1887, 1888, 1889, 1890, 1891, 1892, 1893, 1894, 1895, 1896, 1897, 1898, 1899, 1900, 1901, 1902,
				1903, 1904, 1905, 1906, 1907, 1908, 1909, 1910, 1911, 1912, 1913, 1914, 1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922,
				1923, 1924, 1925, 1926, 1927, 1928, 1929, 1930, 1931, 1932, 1933, 1934, 1935, 1936, 1937, 1938, 1939, 1940, 1941, 1942,
				1943, 1944, 1945, 1946, 1947, 1948, 1949, 1950, 1951, 1952, 1953, 1954, 1955, 1956, 1957, 1958, 1959, 1960, 1961, 1962,
				1963, 1964, 1965, 1966, 1967, 1968, 1969, 1970, 1971, 1972, 1973, 1974, 1975, 1976, 1977, 1978, 1979, 1980, 1981, 1982,
				1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002,
				2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022,
				2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034, 2035, 2036, 2037, 2038, 2039, 2040, 2041, 2042,
				2043, 2044, 2045, 2046, 2047, 2048, 2049, 2050, 2051, 2052, 2053, 2054, 2055, 2056, 2057, 2058, 2059, 2060, 2061, 2062,
				2063, 2064, 2065, 2066, 2067, 2068, 2069, 2070, 2071, 2072, 2073, 2074, 2075, 2076, 2077, 2078, 2079, 2080, 2081, 2082,
				2083, 2084, 2085, 2086, 2087, 2088, 2089, 2090, 2091, 2092, 2093, 2094, 2095, 2096, 2097, 2098, 2099, 2100, 2101, 2102,
				2103, 2104, 2105, 2106, 2107, 2108, 2109, 2110, 2111, 2112, 2113, 2114, 2115, 2116, 2117, 2118, 2119, 2120, 2121, 2122,
				2123, 2124, 2125, 2126, 2127, 2128, 2129, 2130, 2131, 2132, 2133, 2134, 2135, 2136, 2137, 2138, 2139, 2140, 2141, 2142,
				2143, 2144, 2145, 2146, 2147, 2148, 2149, 2150, 2151, 2152, 2153, 2154, 2155, 2156, 2157, 2158, 2159, 2160, 2161, 2162,
				2163, 2164, 2165, 2166, 2167, 2168, 2169, 2170, 2171, 2172, 2173, 2174, 2175, 2176, 2177, 2178, 2179, 2180, 2181, 2182,
				2183, 2184, 2185, 2186, 2187, 2188, 2189, 2190, 2191, 2192, 2193, 2194, 2195, 2196, 2197, 2198, 2199, 2200, 2201, 2202,
				2203, 2204, 2205, 2206, 2207, 2208, 2209, 2210, 2211, 2212, 2213, 2214, 2215, 2216, 2217, 2218, 2219, 2220, 2221, 2222,
				2223, 2224, 2225, 2226, 2227, 2228, 2229, 2230, 2231, 2232, 2233, 2234, 2235, 2236, 2237, 2238, 2239, 2240, 2241, 2242,
				2243, 2244, 2245, 2246, 2247, 2248, 2249, 2250, 2251, 2252, 2253, 2254, 2255, 2256, 2257, 2258, 2259, 2260, 2261, 2262,
				2263, 2264, 2265, 2266, 2267, 2268, 2269, 2270, 2271, 2272, 2273, 2274, 2275, 2276, 2277, 2278, 2279, 2280, 2281, 2282,
				2283, 2284, 2285, 2286, 2287, 2288, 2289, 2290, 2291, 2292, 2293, 2294, 2295, 4600, 4601, 4602, 4603, 4604, 4605, 4606,
				4607, 4608, 4609, 4610, 4611, 4612, 4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626,
				4627, 4628, 4629, 4630, 4631, 4632, 4633, 4634, 4635, 4636, 4637, 4638, 4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646,
				4647, 4648, 4649, 4650, 4651, 4652, 4653, 4654, 4655, 4656, 4657, 4658, 4659, 4660, 4661, 4662, 4663, 4664, 4665, 4666,
				4667, 4668, 4669, 4670, 4671, 4672, 4673, 4674, 4675, 4676, 4677, 4678, 4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686,
				4687, 4688, 4689, 4690, 4691, 4692, 4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706,
				4707, 4708, 4709, 4710, 4711, 4712, 4713, 4714, 4715, 4716, 4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726,
				4727, 4728, 4729, 4730, 4731, 4732, 4733, 4734, 4735, 4736, 4737, 4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746,
				4747, 4748, 4749, 4750, 4751, 4752, 4753, 4754, 4755, 4756, 4757, 4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766,
				4767, 4768, 4769, 4770, 4771, 4772, 4773, 4774, 4775, 4776, 4777, 4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786,
				4787, 4788, 4789, 4790, 4791, 4792, 4793, 4794, 4795, 4796, 4797, 4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806,
				4807, 4808, 4809, 4810, 4811, 4812, 4813, 4814, 4815, 4816, 4817, 4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826,
				4827, 4828, 4829, 4830, 4831, 4832, 4833, 4834, 4835, 4836, 4837, 4838, 4839, 4840, 4841, 4842, 4843, 4844, 4845, 4846,
				4847, 4848, 4849, 4850, 4851, 4852, 4853, 4854, 4855, 4856, 4857, 4858, 4859, 4860, 4861, 4862, 4863, 4864, 4865, 4866,
				4867, 4868, 4869, 4870, 4871, 4872, 4873, 4874, 4876, 4877, 4878, 4879, 4880, 4881, 4882, 4883, 4884, 4885, 4886, 4887,
				4888, 4889, 4890, 4891, 4892, 4893, 4894, 4895, 4896, 4897, 4898, 4899, 4900, 4901, 4902, 4903, 4904, 4905, 4906, 4907,
				4908, 4909, 4910, 4911, 4912, 4913, 4914, 4915, 4916, 4917, 4918, 4919, 4920, 4921, 4922, 4923, 4924, 4925, 4926, 4927,
				4928, 4929, 4930, 4931, 4932, 4933, 4934, 4935, 4936, 4937, 4938, 4939, 4940, 4941, 4942, 4943, 4944, 4945, 4946, 4947,
				4948, 4949, 4979, 4980, 4981, 4982, 4983, 4984, 4997, 4998, 4999, 5000, 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008,
				5009, 5010, 5011, 5012, 5013, 5014, 5015, 5016, 5017, 5018, 5019, 5020, 5021, 5022, 5023, 5024, 5025, 5026, 5027, 5028,
				5029, 5030, 5031, 5032, 5033, 5034, 5035, 5036, 5037, 5038, 5039, 5040, 5041, 5042, 5043, 5044, 5045, 5046, 5047, 5048,
				5049, 5050, 5051, 5052, 5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5067, 5068, 5069, 5070, 5071, 5072, 5073, 5074,
				5075, 5076, 5077, 5078, 5079, 5080, 5081, 5082, 5083, 5084, 5085, 5086, 5087, 5088, 5089, 5090, 5091, 5092, 5093, 5094,
				5095, 5096, 5097, 5098, 5099, 5100, 5101, 5102, 5103, 5104, 5105, 5106, 5107, 5108, 5109, 5110, 5111, 5112, 5113, 5114,
				5115, 5116, 5117, 5118, 5119, 5120, 5121, 5122, 5123, 5124, 5125, 5126, 5127, 5128, 5129, 5130, 5131, 5132, 5133, 5134,
				5135, 5136, 5137, 5138, 5139, 5140, 5141, 5142, 5143, 5144, 5145, 5146, 5147, 5148, 5149, 5150, 5151, 5152, 5153, 5154,
				5155, 5156, 5157, 5158, 5159, 5160, 5161, 5162, 5163, 5164, 5165, 5166, 5167, 5168, 5169, 5170, 5171, 5172, 5173, 5174,
				5175, 5176, 5177, 5178, 5179, 5180, 5181, 5182, 5183, 5184, 5185, 5186, 5187, 5188, 5189, 5190, 5191, 5192, 5193, 5194,
				5195, 5196, 5197, 5198, 5199, 5200, 5201, 5202, 5203, 5204, 5205, 5206, 5207, 5208, 5209, 5210, 5211, 5212, 5213, 5214,
				5215, 5216, 5217, 5218, 5219, 5220, 5221, 5222, 5223, 5224, 5225, 5226, 5227, 5228, 5229, 5230, 5231, 5232, 5233, 5234,
				5235, 5236, 5237, 5238, 5239, 5240, 5241, 5242, 5243, 5244, 5245, 5246, 5247, 5248, 5249, 5250, 5251, 5252, 5253, 5254,
				5255, 5256, 5257, 5258, 5259, 5260, 5261, 5262, 5263, 5264, 5265, 5266, 5267, 5268, 5269, 5270, 5271, 5272, 5273, 5274,
				5275, 5276, 5277, 5278, 5279, 5280, 5281, 5282, 5283, 5284, 5285, 5286, 5287, 5288, 5289, 5290, 5291, 5292, 5293, 5294,
				5295, 5296, 5297, 5298, 5299, 5300, 5301, 5302, 5303, 5304, 5305, 5306, 5307, 5308, 5309, 5310, 5311, 5312, 5313, 5314,
				5315, 5316, 5317, 5318, 5319, 5320, 5321, 5322, 5323, 5324, 5325, 5326, 5327, 5328, 5329, 5330, 5331, 5332, 5333, 5334,
				5335, 5336, 5337, 5338, 5339, 5340, 5341, 5342, 5343, 5344, 5345, 5346, 5347, 5348, 5349, 5350, 5351, 5352, 5353, 5354,
				5355, 5356, 5357, 5358, 5359, 5360, 5361, 5362, 5363, 5364, 5365, 5366, 5367, 5368, 5369, 5370, 5371, 5372, 5373, 5374,
				5375, 5376, 5377, 5378, 5379, 5380, 5381, 5382, 5383, 5384, 5385, 5386, 5387, 5388, 5389, 5390, 5391, 5392, 5393, 5394,
				5395, 5396, 5397, 5398, 5399, 5400, 5401, 5402, 5403, 5404, 5405, 5406, 5407, 5408, 5409, 5410, 5411, 5412, 5413, 5414,
				5415, 5416, 5417, 5418, 5419, 5420, 5421, 5422, 5423, 5424, 5425, 5426, 5427, 5428, 5429, 5430, 5431, 5432, 5433, 5434,
				5435, 5436, 5437, 5438, 5439, 5440, 5441, 5442, 5443, 5444, 5445, 5446, 5447, 5448, 5449, 5450, 5451, 5452, 5453, 5454,
				5455, 5456, 5457, 5458, 5459, 5460, 5461, 5462, 5463, 5464, 5465, 5466, 5467, 5468, 5469, 5470, 5471, 5472, 5473, 5474,
				5475, 5476, 5477, 5478, 5479, 5480, 5481, 5482, 5483, 5484, 5485, 5486, 5487, 5488, 5489, 5490, 5491, 5492, 5493, 5494,
				5495, 5496, 5497, 5498, 5499, 5500, 5501, 5502, 5503, 5504, 5505, 5506, 5507, 5508, 5509, 5510, 5511, 5512, 5513, 5514,
				5515, 5516, 5517, 5518, 5519, 5520, 5521, 5522, 5523, 5524, 5525, 5526, 5527, 5528, 5529, 5530, 5531, 5532, 5533, 5534,
				5535, 5536, 5537, 5538, 5539, 5540, 5541, 5542, 5543, 5544, 5545, 5546, 5547, 5548, 5549, 5550, 5551, 5552, 5553, 5554,
				5555, 5556, 5557, 5558, 5559, 5560, 5561, 5562, 5563, 5564, 5565, 5566, 5567, 5568, 5569, 5570, 5571, 5572, 5573, 5574,
				5575, 5576, 5577, 5578, 5579, 5580, 5581, 5582, 5583, 5584, 5585, 5586, 5587, 5588, 5589, 5590, 5591, 5592, 5593, 5594,
				5595, 5596, 5597, 5598, 5599, 5600, 5601, 5602, 5603, 5604, 5605, 5606, 5607, 5608, 5609, 5610, 5611, 5612, 5613, 5614,
				5615, 5616, 5617, 5618, 5619, 5620, 5621, 5622, 5623, 5624, 5625, 5626, 5627, 5628, 5629, 5630, 5631, 5632, 5633, 5634,
				5635, 5636, 5637, 5638, 5639, 5640, 5641, 5642, 5643, 5644, 5645, 5646, 5647, 5648, 5649, 7267, 7268, 7269, 7270, 7271,
				7272, 7273, 7274, 7275, 7276, 7277, 7278, 7279, 7280, 7281, 7282, 7283, 7284, 7285, 7286, 7287, 7288, 7289, 7290, 7291,
				7292, 7293, 7294, 7295, 7296, 7297, 7298, 7299, 7300, 7301, 7302, 7303, 7304, 7305, 7306, 7307, 7308, 7309, 7310, 7311,
				7312, 7313, 7314, 7315, 7316, 7317, 7318, 7319, 7320, 7321, 7322, 7323, 7324, 7325, 7326, 7327, 7328, 7329, 7330, 7331,
				7332, 7333, 7334, 7335, 7336, 7337, 7338, 7339, 7340, 7341, 7342, 7343, 7344, 7345, 7346, 7347, 7348, 7349, 7350, 7351,
				7352, 7353, 7354, 7355, 7356, 7357, 7358, 7359, 7360, 7361, 7362, 7363, 7364, 7365, 7366, 7367, 7368, 7369, 7370, 7371,
				7372, 7373, 7374, 7375, 7376, 7377, 7378, 7379, 7380, 7381, 7382, 7383, 7384, 7385, 7386, 8042, 8043, 8044, 8045, 8046,
				8047, 8048, 8049, 8050, 8051, 8052, 8053, 8054, 8055, 8056, 8057, 8058, 8059, 8060, 8061, 8062, 8063, 8064, 8065, 8066,
				8067, 8068, 8069, 8070, 8071, 8072, 8073, 8074, 8075, 8076, 8077, 8078, 8079, 8080, 8081, 8082, 8083, 8084, 8085, 8086,
				8087, 8088, 8089, 8090, 8091, 8092, 8093, 8094, 8095, 8096, 8097, 8098, 8099, 8100, 8101, 8102, 8103, 8104, 8105, 8106,
				8107, 8108, 8109, 8110, 8111, 8112, 8113, 8114, 8115, 8116, 8117, 8118, 8119, 8120, 8121, 8122, 8123, 8124, 8125, 8126,
				8127, 8128, 8129, 8130, 8131, 8132, 8133, 8134, 8135, 8136, 8137, 8138, 8139, 8140, 8141, 8142, 8143, 8144, 8145, 8146,
				8147, 8148, 8792, 8793, 8794, 8795, 8796, 8797, 8798, 8799, 8800, 8801, 8802, 8803, 8804, 8805, 8806, 8807, 8808, 8809,
				8810, 8811, 8812, 8813, 8814, 8815, 8816, 8817, 8818, 8819, 8820, 8821, 8822, 8823, 8824, 8825, 8826, 8827, 8828, 8829,
				8830, 8831, 8832, 8833, 8834, 8835, 8836, 8837, 8838, 8839, 8840, 8841, 8842, 8843, 8844, 8845, 8846, 8847, 8848, 8849,
				8850, 8851, 8852, 8853, 8854, 8855, 8856, 8857, 8858, 8859, 8860, 8861, 8862, 8863, 8864, 8865, 8866, 8867, 8868, 8869,
				8870, 8871, 8872, 8873, 8874, 8875, 8876, 8877, 8878, 8879, 8880, 8881, 8882, 8883, 8884, 8885, 8886, 8887, 8888, 8889,
				8890, 8891, 8892, 8893, 8894, 8895, 8896, 8897, 8898, 8899, 8900, 8901, 8902, 8903, 8904, 8905, 8906, 8907, 8908, 8909,
				8910, 8911, 8912, 8913, 8914, 8915, 8916, 8917, 8918, 8919, 8920, 8921, 8922, 8923, 8924, 8925, 8926, 8927, 8928, 8929,
				8930, 8931, 8932, 8933, 8934, 8935, 8936, 8937, 8938, 8939, 8940, 8941, 8942, 8943, 8944, 8945, 8946, 8947, 8948, 8949,
				8950, 8951, 8952, 8953, 8954, 8955, 8956, 8957, 8958, 8959, 8960, 8961, 8962, 8963, 8964, 8965, 8966, 8967, 8968, 8969,
				8970, 8971, 8972, 8973, 8974, 8975, 8976, 8977, 8978, 8979, 8980, 8981, 8982, 8983, 8984, 8985, 8986, 8987, 8988, 8989,
				8990, 8991, 8992, 8993, 8994, 8995, 8996, 8997, 8998, 8999, 9000, 9001, 9002, 9003, 9004, 9005, 9006, 9007, 9008, 9009,
				9010, 9011, 9012, 9013, 9014, 9015, 9016, 9017, 9018, 9019, 9020, 9021, 9022, 9023, 9024, 9025, 9026, 9027, 9028, 9029,
				9030, 9031, 9032, 9033, 9034, 9035, 9036, 9037, 9038, 9039, 9040, 9041, 9042, 9043, 9044, 9045, 9046, 9047, 9048, 9049,
				9050, 9051, 9052, 9053, 9054, 9297, 9302, 9458, 9760
		};
		private readonly int[] h1tArray = new int[2573]
		{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
				33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
				64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94,
				95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
				121, 122, 123, 124, 125, 126, sbyte.MaxValue, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142,
				143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167,
				168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192,
				193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217,
				218, 219, 220, 221, 222, 223, 224, 277, 1524, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030,
				2031, 2032, 2033, 2034, 2035, 2036, 2037, 2038, 2039, 2040, 2041, 2042, 2043, 2044, 2045, 2046, 2047, 2048, 2049, 2050,
				2051, 2052, 2053, 2054, 2055, 2056, 2057, 2058, 2059, 2060, 2061, 2062, 2063, 2064, 2065, 2066, 2067, 2068, 2069, 2070,
				2071, 2072, 2073, 2074, 2075, 2076, 2077, 2078, 2079, 2080, 2081, 2082, 2083, 2084, 2085, 2086, 2087, 2088, 2089, 2090,
				2091, 2092, 2093, 2094, 2095, 2096, 2097, 2098, 2099, 2100, 2101, 2102, 2103, 2104, 2105, 2106, 2107, 2108, 2109, 2110,
				2111, 2112, 2113, 2114, 2115, 2116, 2117, 2118, 2119, 2120, 2121, 2122, 2123, 2124, 2125, 2126, 2127, 2128, 2129, 2130,
				2131, 2132, 2133, 2134, 2135, 2136, 2137, 2138, 2139, 2140, 2141, 2142, 2143, 2144, 2145, 2146, 2147, 2148, 2149, 2150,
				2151, 2152, 2153, 2154, 2155, 2156, 2157, 2158, 2159, 2160, 2161, 2162, 2163, 2164, 2165, 2167, 2168, 2169, 2170, 4241,
				4242, 4243, 4244, 4245, 4246, 4247, 4248, 4249, 4250, 4251, 4252, 4253, 4254, 4255, 4256, 4257, 4258, 4259, 4260, 4261,
				4262, 4263, 4264, 4265, 4266, 4267, 4268, 4269, 4270, 4271, 4272, 4273, 4274, 4275, 4276, 4277, 4278, 4279, 4280, 4281,
				4282, 4283, 4284, 4285, 4286, 4287, 4288, 4289, 4290, 4291, 4292, 4293, 4294, 4295, 4296, 4297, 4298, 4299, 4300, 4301,
				4302, 4303, 4304, 4305, 4306, 4307, 4308, 4309, 4310, 4311, 4312, 4313, 4314, 4315, 4316, 4317, 4318, 4319, 4320, 4321,
				4322, 4323, 4324, 4325, 4326, 4327, 4328, 4329, 4330, 4331, 4332, 4333, 4334, 4335, 4336, 4337, 4338, 4339, 4340, 4341,
				4342, 4343, 4344, 4345, 4346, 4347, 4348, 4349, 4350, 4351, 4352, 4353, 4354, 4355, 4356, 4357, 4358, 4359, 4360, 4361,
				4362, 4363, 4364, 4365, 4366, 4367, 4368, 4369, 4370, 4371, 4372, 4373, 4374, 4375, 4376, 4377, 4378, 4379, 4380, 4381,
				4382, 4383, 4384, 4385, 4386, 4387, 4388, 4389, 4390, 4391, 4392, 4393, 4394, 4395, 4396, 4397, 4398, 4399, 4400, 4401,
				4402, 4403, 4404, 4405, 4406, 4407, 4408, 4409, 4410, 4411, 4412, 4413, 4414, 4415, 4416, 4417, 4418, 4419, 4420, 4421,
				4422, 4423, 4424, 4425, 4426, 4427, 4428, 4429, 4430, 4431, 4432, 4433, 4434, 4435, 4436, 4437, 4438, 4439, 4440, 4441,
				4442, 4443, 4444, 4445, 4446, 4447, 4448, 4449, 4450, 4451, 4452, 4453, 4454, 4455, 4456, 4457, 4458, 4459, 4460, 4461,
				4462, 4463, 4464, 4465, 4466, 4467, 4468, 4469, 4470, 4471, 4472, 4473, 4474, 4475, 4476, 4477, 4478, 4479, 4480, 4481,
				4482, 4483, 4484, 4485, 4486, 4487, 4488, 4662, 4663, 4664, 4665, 4666, 4667, 4668, 4669, 4670, 4671, 4672, 4673, 4674,
				4675, 4676, 4677, 4678, 4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686, 4687, 4688, 4689, 4690, 4691, 4692, 4693, 4694,
				4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712, 4713, 4714,
				4715, 4716, 4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726, 4727, 4728, 4729, 4730, 4731, 4732, 4733, 4734,
				4735, 4736, 4737, 4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746, 4747, 4748, 4749, 4750, 4751, 4752, 4753, 4754,
				4755, 4756, 4757, 4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766, 4767, 4768, 4769, 4770, 4771, 4772, 4773, 4774,
				4775, 4776, 4777, 4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786, 4787, 4788, 4789, 4790, 4791, 4792, 4793, 4794,
				4795, 4796, 4797, 4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806, 4807, 4808, 4809, 4810, 4811, 4812, 4813, 4814,
				4815, 4816, 4817, 4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826, 4827, 4828, 4829, 4830, 4831, 4832, 4833, 4834,
				4835, 4836, 4837, 4838, 4839, 4840, 4841, 4842, 4843, 4844, 4845, 4846, 4847, 4848, 4849, 4850, 4851, 4852, 4853, 4854,
				4855, 4856, 4857, 4858, 4859, 4860, 4861, 4862, 4863, 4864, 4865, 4866, 4867, 4868, 4869, 4870, 4871, 4872, 4873, 4874,
				4875, 4876, 4877, 4878, 4879, 4880, 4881, 4882, 4883, 4884, 4885, 4886, 4887, 4888, 4889, 4890, 4891, 4892, 4893, 4894,
				4895, 4896, 4897, 4898, 4899, 4900, 4901, 4902, 4903, 4904, 4905, 4906, 4907, 4908, 4909, 4910, 4911, 4914, 4915, 4916,
				4917, 4918, 4919, 4920, 4921, 4922, 4923, 4924, 4925, 4926, 4927, 4928, 4929, 4930, 4931, 4932, 4933, 4934, 4935, 4936,
				4937, 4938, 4939, 4940, 4941, 4942, 4943, 4944, 4945, 4946, 4947, 4948, 4949, 4950, 4951, 4952, 4953, 4954, 4955, 4956,
				4957, 4958, 4959, 4960, 4961, 4962, 4963, 4964, 4965, 4966, 4967, 4968, 4969, 4970, 4971, 4972, 4974, 4975, 4976, 4977,
				4978, 4979, 4980, 4981, 4982, 4983, 4984, 4985, 4986, 4987, 4988, 4989, 4990, 4991, 4992, 4993, 4994, 4995, 4996, 4997,
				5039, 5040, 5041, 5042, 5043, 5044, 5045, 5046, 5047, 5048, 5049, 5050, 5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058,
				5059, 5060, 5061, 5062, 5063, 5064, 5065, 5066, 5119, 5120, 5121, 5122, 5123, 5124, 5125, 5126, 5127, 5128, 5129, 5130,
				5131, 5132, 5133, 5134, 5135, 5136, 5137, 5138, 5139, 5140, 5141, 5142, 5143, 5144, 5145, 5146, 5147, 5148, 5149, 5150,
				5151, 5152, 5153, 5154, 5155, 5156, 5157, 5158, 5159, 5160, 5161, 5162, 5163, 5164, 5165, 5166, 5167, 5168, 5169, 5170,
				5171, 5172, 5173, 5174, 5175, 5176, 5177, 5178, 5179, 5180, 5181, 5182, 5183, 5184, 5185, 5186, 5187, 5188, 5189, 5190,
				5191, 5192, 5193, 5194, 5195, 5196, 5197, 5198, 5199, 5200, 5201, 5202, 5203, 5204, 5205, 5206, 5207, 5208, 5209, 5210,
				5220, 5221, 5222, 5223, 5224, 5225, 5226, 5227, 5228, 5229, 5230, 5231, 5232, 5233, 5234, 5235, 5236, 5237, 5238, 5239,
				5240, 5241, 5242, 5243, 5244, 5245, 5246, 5247, 5248, 5249, 5250, 5251, 5252, 5253, 5254, 5255, 5256, 5257, 5258, 5259,
				5260, 5261, 5262, 5263, 5264, 5265, 5266, 5267, 5268, 5269, 5270, 5271, 5272, 5273, 5274, 5275, 5276, 5277, 5278, 5279,
				5280, 5281, 5282, 5283, 5284, 5285, 5286, 5287, 5288, 5289, 5290, 5291, 5293, 5295, 5296, 5297, 5298, 5299, 5300, 5301,
				5302, 5303, 5304, 5305, 5306, 5307, 5308, 5309, 5310, 5311, 5312, 5313, 5314, 5315, 5316, 5317, 5318, 5319, 5320, 5321,
				5322, 5323, 5324, 5325, 5326, 5327, 5328, 5329, 5330, 5331, 5332, 5333, 5334, 5335, 5336, 5337, 5338, 5339, 5340, 5341,
				5342, 5343, 5344, 5345, 5346, 5347, 5348, 5349, 5350, 5351, 5352, 5353, 5354, 5355, 5356, 5357, 5358, 5359, 5360, 5361,
				5362, 5363, 5364, 5365, 5366, 5367, 5368, 5369, 5370, 5371, 5372, 5373, 5374, 5375, 5376, 5377, 5378, 5379, 5380, 5381,
				5382, 5383, 5384, 5385, 5386, 5387, 5388, 5389, 5390, 5391, 5392, 5393, 5394, 5395, 5396, 5397, 5398, 5399, 5400, 5401,
				5402, 5403, 5404, 5405, 5406, 5407, 5408, 5409, 5410, 5411, 5412, 5413, 5414, 5415, 5416, 5417, 5418, 5419, 5420, 5421,
				5422, 5423, 5424, 5425, 5426, 5427, 5428, 5429, 5430, 5431, 5432, 5433, 5434, 5435, 5436, 5437, 5438, 5439, 5440, 5441,
				5442, 5443, 5444, 5445, 5446, 5447, 5448, 5449, 5450, 5451, 5452, 5453, 5454, 5455, 5456, 5457, 5458, 5459, 5460, 5461,
				5462, 5463, 5464, 5465, 5466, 5467, 5468, 5469, 5470, 5471, 5472, 5473, 5474, 5475, 5476, 5477, 5478, 5479, 5480, 5481,
				5482, 5483, 5484, 5485, 5486, 5487, 5488, 5489, 5490, 5491, 5492, 5493, 5494, 5495, 5496, 5497, 5498, 5499, 5500, 5501,
				5502, 5503, 5504, 5505, 5506, 5507, 5508, 5509, 5510, 5511, 5512, 5513, 5514, 5515, 5516, 5517, 5518, 5519, 5773, 5774,
				5775, 5776, 5777, 5778, 5779, 5780, 5781, 5782, 5783, 5784, 5785, 5786, 5787, 5788, 5789, 5790, 5791, 5792, 5793, 5794,
				5795, 5796, 5797, 5798, 5799, 5800, 5801, 5802, 5803, 5804, 5805, 5806, 5807, 5808, 5809, 5810, 5811, 5812, 5813, 5814,
				5815, 5816, 5817, 5818, 5819, 5820, 5821, 5822, 5823, 5824, 5825, 5826, 5827, 5828, 5829, 5830, 5831, 5832, 5833, 5834,
				5835, 5836, 5837, 5838, 5839, 5840, 5841, 5842, 5843, 5844, 5845, 5846, 5847, 5848, 5849, 5850, 5851, 5852, 5853, 5854,
				5855, 5856, 5857, 5858, 5859, 5860, 5861, 5862, 5863, 5864, 5865, 5866, 5867, 5868, 5869, 5870, 5871, 5872, 5873, 5874,
				5875, 5876, 5877, 5878, 5879, 5880, 5881, 5882, 5883, 5884, 5885, 5886, 5887, 5888, 5889, 5890, 5891, 5892, 5893, 5894,
				5895, 5896, 5897, 5898, 5899, 5900, 5901, 5902, 5903, 5904, 5905, 5906, 5907, 5908, 5909, 5910, 5911, 5912, 5913, 5914,
				5915, 5916, 5917, 5918, 5919, 5920, 5921, 5922, 5923, 5924, 5925, 5926, 5927, 5928, 5929, 5930, 5931, 5932, 5933, 5934,
				5935, 5936, 5937, 5938, 5939, 5940, 5941, 5942, 5943, 5944, 5945, 5946, 5947, 5948, 5949, 5950, 5951, 5952, 5953, 5954,
				5955, 5956, 5957, 5958, 5959, 5960, 5961, 5962, 5963, 5964, 5965, 5966, 5967, 5968, 5969, 5970, 5971, 5972, 5973, 5974,
				5975, 5976, 5977, 5978, 5979, 5980, 5981, 5982, 5983, 5984, 5985, 5986, 5987, 5988, 5989, 5990, 5991, 5992, 5993, 5994,
				5995, 5996, 5997, 5998, 5999, 6000, 6001, 6002, 6003, 6004, 6005, 6006, 6007, 6008, 6009, 6010, 6011, 6012, 6013, 6014,
				6015, 6016, 6017, 6018, 6019, 6020, 6021, 6022, 6023, 6024, 6025, 6026, 6027, 6028, 6029, 6030, 6031, 6032, 6033, 6034,
				6035, 6036, 6037, 6038, 6039, 6040, 6041, 6042, 6043, 6044, 6045, 6046, 6047, 6048, 6049, 6050, 6051, 6052, 6053, 6054,
				6055, 6056, 6057, 6058, 6059, 6060, 6061, 6062, 6063, 6064, 6065, 6066, 6067, 6068, 6069, 6070, 6071, 6072, 6073, 6074,
				6075, 6076, 6077, 6078, 6079, 6080, 6081, 6082, 6083, 6084, 6085, 6086, 6087, 6088, 6089, 6090, 6091, 6092, 6093, 6094,
				6095, 6096, 6097, 6098, 6099, 6100, 6101, 6102, 6103, 6104, 6105, 6106, 6107, 6108, 6109, 6110, 6111, 6112, 6113, 6114,
				6115, 6116, 6117, 6118, 6119, 6120, 6121, 6122, 6123, 6124, 6125, 6126, 6127, 6128, 6129, 6130, 6131, 6132, 6133, 6134,
				6135, 6136, 6137, 6138, 6139, 6140, 6141, 6142, 6143, 6144, 6145, 6146, 6147, 6148, 6149, 6150, 6151, 6152, 6153, 6154,
				6155, 6156, 6157, 6158, 6159, 6160, 6161, 6162, 6163, 6164, 6165, 6166, 6167, 6168, 6169, 6170, 6171, 6172, 6173, 6174,
				6175, 6176, 6177, 6178, 6179, 6180, 6181, 6182, 6183, 6184, 6185, 6186, 6187, 6188, 6189, 6190, 6191, 6192, 6193, 6194,
				6195, 6196, 6197, 6198, 6199, 6200, 6201, 6202, 6203, 6204, 6205, 6206, 6207, 6208, 6209, 6210, 6211, 6212, 6213, 6214,
				6215, 6216, 6217, 6218, 6219, 6220, 6221, 6222, 6223, 6224, 6225, 6226, 6227, 6228, 6229, 6230, 6231, 6232, 6233, 6234,
				6235, 6236, 6237, 6238, 6239, 6240, 6241, 6242, 6243, 6244, 6245, 6246, 6247, 6248, 6249, 6250, 6251, 6252, 6253, 6254,
				6255, 6257, 6258, 6259, 6260, 6261, 6262, 6263, 6264, 6265, 6266, 6267, 6268, 6269, 6270, 6271, 6272, 6273, 6274, 6275,
				6276, 6277, 6278, 6279, 6280, 6281, 6282, 7587, 7588, 7589, 7590, 7591, 7592, 7593, 7594, 7595, 7596, 7597, 7598, 7599,
				7600, 7601, 7602, 7603, 7604, 7605, 7606, 7607, 7608, 7609, 7610, 7611, 7612, 7613, 7614, 7615, 7616, 7617, 7618, 7619,
				7620, 7621, 7622, 7623, 7624, 7625, 7626, 7627, 7628, 7629, 7630, 7631, 7632, 7633, 7634, 7635, 7636, 7637, 7638, 7639,
				7640, 7641, 7642, 7643, 7644, 7645, 7646, 7647, 7648, 7649, 7650, 7651, 7652, 7653, 7654, 7655, 7656, 7657, 7658, 7659,
				7660, 7661, 7662, 7663, 7664, 7665, 7666, 7667, 7668, 7669, 7670, 7671, 7672, 7673, 7674, 7675, 7676, 7677, 7678, 7679,
				7680, 7681, 7682, 7683, 7684, 7685, 7686, 7687, 7688, 7689, 7690, 7691, 7692, 7693, 7694, 7695, 7696, 7697, 7698, 7699,
				7700, 7701, 7702, 7703, 7704, 7705, 7706, 7707, 7708, 7709, 7710, 7711, 7712, 7713, 7714, 7715, 7716, 7717, 7718, 7719,
				7720, 7721, 7722, 7723, 7724, 7725, 7726, 7727, 7728, 7729, 7730, 7731, 7732, 7733, 7734, 7735, 7736, 7737, 7738, 7739,
				7740, 7741, 7742, 7743, 7744, 7745, 7746, 7747, 7748, 7749, 7750, 7751, 7752, 7753, 7754, 7755, 7756, 7757, 7758, 7759,
				7760, 7761, 7762, 7763, 7764, 7765, 7766, 7767, 7768, 7769, 7770, 7771, 7772, 7773, 7774, 7775, 7776, 7777, 7778, 7779,
				7780, 7781, 7782, 7783, 7784, 7785, 7786, 7787, 7788, 7789, 7790, 7791, 7792, 7793, 7794, 7795, 7796, 7797, 7798, 7799,
				7800, 7801, 7802, 7803, 7804, 7805, 7806, 7807, 7808, 7809, 7810, 7811, 7812, 7813, 7814, 7815, 7816, 7817, 7818, 7819,
				7820, 7821, 7822, 7823, 7824, 7825, 7826, 7827, 7828, 7829, 7830, 7831, 7832, 7833, 7834, 7835, 7836, 7837, 7838, 7839,
				7840, 7841, 7842, 7843, 7844, 7845, 7846, 7847, 7848, 7849, 7850, 7851, 7852, 7853, 7854, 7855, 7856, 7857, 7858, 7859,
				7860, 7861, 7862, 7863, 7864, 7865, 7866, 7867, 7868, 7869, 7870, 7871, 7872, 7873, 7874, 7875, 7876, 7877, 7878, 7879,
				7880, 7881, 7882, 7883, 7884, 7885, 7886, 7887, 7888, 7889, 7890, 7891, 7892, 7893, 7894, 7895, 7896, 7897, 7898, 7899,
				7900, 7901, 7902, 7903, 7904, 7905, 7906, 7907, 7908, 7909, 7910, 7911, 7912, 7913, 7914, 7915, 7916, 7917, 7918, 7919,
				7920, 7921, 7922, 7923, 7924, 7925, 7926, 7927, 7928, 7929, 7930, 7931, 7932, 7933, 7934, 7935, 7936, 7937, 7938, 7939,
				7940, 7941, 7942, 7943, 7944, 7945, 7946, 7947, 7948, 7949, 7950, 7951, 7952, 7953, 7954, 7955, 7956, 7957, 7958, 7959,
				7960, 7961, 7962, 7963, 7964, 7965, 7966, 7967, 7968, 7969, 7970, 7971, 7972, 7973, 7974, 7975, 7976, 7977, 7978, 7979,
				7980, 7981, 7982, 7983, 7984, 7985, 7986, 7987, 7988, 7989, 7990, 7991, 7992, 7993, 7994, 7995, 7996, 7997, 7998, 7999,
				8000, 8001, 8002, 8003, 8004, 8005, 8006, 8007, 8008, 8009, 8010, 8011, 8012, 8013, 8014, 8015, 8016, 8017, 8018, 8019,
				8020, 8021, 8022, 8023, 8024, 8025, 8026, 8027, 8028, 8029, 8030, 8031, 8032, 8033, 8034, 8035, 8036, 8037, 8038, 8039,
				8040, 8041, 8042, 8043, 8044, 8045, 8046, 8047, 8048, 8049, 8050, 8051, 8052, 8053, 8054, 8055, 8056, 8057, 8058, 8059,
				8060, 8061, 8062, 8063, 8064, 8065, 8066, 8067, 8068, 8069, 8070, 8071, 8072, 8073, 8074, 8075, 8076, 8077, 8078, 8079,
				8080, 8081, 8082, 8083, 8084, 8085, 8086, 8087, 8088, 8089, 8090, 8091, 8092, 8093, 8094, 8095, 8096, 8097, 8098, 8099,
				8100, 8101, 8102, 8103, 8104, 8105, 8106, 8107, 8108, 8109, 8110, 8111, 8112, 8113, 8114, 8115, 8116, 8117, 8118, 8119,
				8120, 8121, 8122, 8123, 8124, 8125, 8126, 8127, 8128, 8129, 8130, 8131, 8132, 8133, 8134, 8135, 8136, 8137, 8138, 8139,
				8140, 8141, 8142, 8143, 8144, 8145, 8146, 8147, 8148, 8149, 8150, 8151, 8152, 9000, 9001, 9002, 9003, 9004, 9005, 9006,
				9007, 9008, 9009, 9010, 9011, 9012, 9013, 9014, 9015, 9016, 9017, 9018, 9019, 9020, 9021, 9022, 9023, 9024, 9025, 9026,
				9027, 9028, 9029, 9030, 9031, 9032, 9033, 9034, 9035, 9036, 9037, 9038, 9039, 9040, 9041, 9042, 9043, 9044, 9045, 9046,
				9047, 9048, 9049, 9050, 9051, 9052, 9053, 9054, 9055, 9056, 9057, 9058, 9059, 9060, 9061, 9062, 9063, 9064, 9065, 9066,
				9067, 9068, 9069, 9070, 9071, 9072, 9073, 9074, 9075, 9076, 9077, 9078, 9079, 9080, 9081, 9082, 9083, 9084, 9085, 9086,
				9087, 9088, 9089, 9090, 9091, 9092, 9093, 9094, 9095, 9096, 9097, 9098, 9099, 9100, 9101, 9102, 9103, 9104, 9105, 9106,
				9107, 9108, 9109, 9110, 9111, 9112, 9113, 9114, 9115, 9116, 9117, 9896, 9901
		};
		private readonly int[] h2tArray = new int[2519]
		{
				79, 1687, 3606, 3607, 3608, 3609, 3610, 3611, 3612, 3613, 3614, 3615, 3616, 3617, 3618, 3619, 3620, 3621, 3622, 3623, 3624,
				3625, 3626, 3627, 3628, 3629, 3630, 3631, 3632, 3633, 3634, 3635, 3636, 3637, 3638, 3639, 3640, 3641, 3642, 3643, 3644,
				3645, 3646, 3647, 3648, 3649, 3650, 3651, 3652, 3653, 3654, 3655, 3656, 3657, 3658, 3659, 3660, 3661, 3662, 3663, 3664,
				3665, 3666, 3667, 3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683, 3684,
				3685, 3686, 3687, 3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3702, 3703, 3704,
				3705, 3706, 3707, 3708, 3709, 3710, 3711, 3712, 3713, 3714, 3715, 3716, 3717, 3718, 3719, 3720, 3721, 3722, 3723, 3724,
				3725, 3726, 3727, 3728, 3729, 3730, 3731, 3732, 3733, 3734, 3735, 3736, 3737, 3738, 3739, 3740, 3741, 3742, 3743, 3744,
				3745, 3746, 3747, 3748, 3749, 3750, 3751, 3752, 3753, 3754, 3755, 3756, 3757, 3758, 3759, 3760, 3761, 3762, 3763, 3764,
				3765, 3766, 3767, 3768, 3769, 3770, 3771, 3772, 3773, 3774, 3775, 3776, 3777, 3778, 3779, 3780, 3781, 3782, 3783, 3784,
				3785, 3786, 3787, 3788, 3789, 3790, 3791, 3792, 3793, 3794, 3795, 3796, 3797, 3798, 3799, 3800, 3801, 3802, 3803, 3804,
				3805, 3806, 3807, 3808, 3809, 3810, 3811, 3812, 3813, 3814, 3815, 3816, 3817, 3818, 3819, 3820, 3821, 3822, 3823, 3824,
				3825, 3826, 3827, 3828, 3829, 3830, 3831, 3832, 3833, 3834, 3835, 3836, 3837, 3838, 3839, 3840, 3841, 3842, 3843, 3844,
				3845, 3846, 3847, 3848, 3849, 3850, 3851, 3852, 3853, 3854, 3855, 3856, 3857, 3858, 3859, 3860, 3861, 3862, 3863, 3864,
				3865, 3866, 3867, 3868, 3869, 3870, 3871, 3872, 3873, 3874, 3875, 3876, 3877, 3878, 3879, 3880, 3881, 3882, 3883, 3884,
				3885, 3886, 3887, 3888, 3889, 3890, 3891, 3892, 3893, 3894, 3895, 3896, 3897, 3898, 3899, 3900, 3901, 3902, 3903, 3904,
				3905, 3906, 3907, 3908, 3909, 3910, 3911, 3912, 3913, 3914, 3915, 3916, 3917, 3918, 3919, 3920, 3921, 3922, 3923, 3924,
				3925, 3926, 3927, 3928, 3929, 3930, 3931, 3932, 3933, 3934, 3935, 4444, 4445, 4446, 4447, 4448, 4449, 4450, 4451, 4452,
				4453, 4454, 4455, 4456, 4457, 4458, 4459, 4460, 4461, 4462, 4463, 4464, 4465, 4466, 4467, 4468, 4469, 4470, 4471, 4472,
				4473, 4474, 4475, 4476, 4477, 4478, 4479, 4480, 4481, 4482, 4483, 4484, 4485, 4486, 4487, 4488, 4489, 4490, 4491, 4492,
				4493, 4494, 4495, 4496, 4497, 4498, 4499, 4500, 4501, 4502, 4503, 4504, 4505, 4506, 4507, 4508, 4509, 4510, 4511, 4512,
				4513, 4514, 4515, 4516, 4517, 4518, 4519, 4520, 4521, 4522, 4523, 4524, 4525, 4526, 4527, 4528, 4529, 4530, 4531, 4532,
				4533, 4534, 4535, 4536, 4537, 4538, 4539, 4540, 4541, 4542, 4543, 4544, 4545, 4546, 4547, 4548, 4549, 4550, 4551, 4552,
				4553, 4554, 4555, 4556, 4557, 4558, 4559, 4560, 4561, 4562, 4563, 4564, 4565, 4566, 4567, 4568, 4569, 4570, 4571, 4572,
				4573, 4574, 4575, 4576, 4577, 4578, 4579, 4580, 4581, 4582, 4583, 4584, 4585, 4586, 4587, 4588, 4589, 4590, 4591, 4592,
				4593, 4594, 4595, 4596, 4597, 4598, 4599, 4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607, 4608, 4609, 4610, 4611, 4612,
				4613, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 4626, 4627, 4628, 4629, 4630, 4631, 4632,
				4633, 4634, 4635, 4636, 4637, 4638, 4639, 4640, 4641, 4642, 4643, 4644, 4645, 4646, 4647, 4648, 4649, 4650, 4651, 4652,
				4653, 4654, 4655, 4656, 4657, 4658, 4659, 4660, 4661, 4662, 4663, 4664, 4665, 4666, 4667, 4668, 4669, 4670, 4671, 4672,
				4673, 4674, 4675, 4676, 4677, 4678, 4679, 4680, 4681, 4682, 4683, 4684, 4685, 4686, 4687, 4688, 4689, 4690, 4691, 4692,
				4693, 4694, 4695, 4696, 4697, 4698, 4699, 4700, 4701, 4702, 4703, 4704, 4705, 4706, 4707, 4708, 4709, 4710, 4711, 4712,
				4713, 4714, 4715, 4716, 4717, 4718, 4719, 4720, 4721, 4722, 4723, 4724, 4725, 4726, 4727, 4728, 4729, 4730, 4731, 4732,
				4733, 4734, 4735, 4736, 4737, 4738, 4739, 4740, 4741, 4742, 4743, 4744, 4745, 4746, 4747, 4748, 4749, 4750, 4751, 4752,
				4753, 4754, 4755, 4756, 4757, 4758, 4759, 4760, 4761, 4762, 4763, 4764, 4765, 4766, 4767, 4768, 4769, 4770, 4771, 4772,
				4773, 4774, 4775, 4776, 4777, 4778, 4779, 4780, 4781, 4782, 4783, 4784, 4785, 4786, 4787, 4788, 4789, 4790, 4791, 4792,
				4793, 4794, 4795, 4796, 4797, 4798, 4799, 4800, 4801, 4802, 4803, 4804, 4805, 4806, 4807, 4808, 4809, 4813, 4814, 4815,
				4816, 4817, 4818, 4819, 4820, 4821, 4822, 4823, 4824, 4825, 4826, 4827, 4828, 4829, 4830, 4831, 4832, 4833, 4834, 4835,
				4836, 4837, 4838, 4839, 4840, 4841, 4842, 4843, 4844, 4845, 4846, 4847, 4848, 4849, 4850, 4851, 4852, 4853, 4854, 4855,
				4856, 4857, 4858, 4859, 4860, 4861, 4862, 4863, 4864, 4865, 4866, 4867, 4868, 4869, 4870, 4871, 4872, 4873, 4874, 4875,
				4876, 4877, 4878, 4879, 4880, 4881, 4882, 4883, 4884, 4885, 4886, 4887, 4888, 4898, 4899, 4901, 4902, 4903, 4905, 4906,
				4907, 4908, 4909, 4910, 4911, 4912, 4913, 4914, 4915, 4916, 4917, 4918, 4919, 4920, 4921, 4922, 4923, 4924, 4925, 4926,
				4927, 4928, 4929, 4930, 4931, 4932, 4933, 4934, 4935, 4936, 4937, 4938, 4990, 4991, 4992, 4993, 4994, 4995, 4996, 4997,
				4998, 4999, 5000, 5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010, 5011, 5012, 5013, 5014, 5015, 5016, 5017,
				5018, 5019, 5020, 5021, 5022, 5023, 5024, 5025, 5026, 5027, 5028, 5029, 5030, 5031, 5035, 5036, 5037, 5038, 5039, 5040,
				5041, 5042, 5043, 5044, 5045, 5046, 5047, 5048, 5049, 5050, 5051, 5052, 5053, 5054, 5055, 5056, 5057, 5058, 5059, 5060,
				5061, 5062, 5063, 5064, 5065, 5066, 5067, 5068, 5069, 5070, 5071, 5072, 5073, 5074, 5075, 5076, 5077, 5078, 5079, 5080,
				5081, 5082, 5083, 5084, 5085, 5086, 5087, 5088, 5089, 5090, 5091, 5092, 5093, 5094, 5095, 5096, 5097, 5098, 5099, 5100,
				5101, 5102, 5103, 5104, 5105, 5106, 5107, 5108, 5109, 5110, 5111, 5112, 5113, 5114, 5115, 5116, 5117, 5118, 5119, 5120,
				5121, 5122, 5123, 6713, 6714, 6715, 6716, 6717, 6718, 6719, 6720, 6721, 6722, 6723, 6724, 6725, 6726, 6727, 6728, 6729,
				6730, 6731, 6732, 6733, 6734, 6735, 6736, 6737, 6738, 6739, 6740, 6741, 6742, 6743, 6744, 6745, 6746, 6747, 6748, 6749,
				6750, 6751, 6752, 6753, 6754, 6755, 6756, 6757, 6758, 6759, 6760, 6761, 6762, 6763, 6764, 6765, 6766, 6767, 6768, 6769,
				6770, 6771, 6772, 6773, 6774, 6775, 6776, 6777, 6778, 6779, 6780, 6781, 6782, 6783, 6784, 6785, 6786, 6787, 6788, 6789,
				6790, 6791, 6792, 6793, 6794, 6795, 6796, 6797, 6798, 6799, 6800, 6801, 6802, 6803, 6804, 6805, 6806, 6807, 6808, 6809,
				6810, 6811, 6812, 6813, 6814, 6815, 6816, 6817, 6818, 6819, 6820, 6821, 6822, 6823, 6824, 6825, 6826, 6827, 6828, 6829,
				6830, 6831, 6832, 6833, 6834, 6835, 6836, 6837, 6838, 6839, 6840, 6841, 6842, 6843, 6844, 6845, 6846, 6847, 6848, 6849,
				6850, 6851, 6852, 6853, 6854, 6855, 6856, 6857, 6858, 6859, 6860, 6861, 6862, 6863, 6864, 6865, 6866, 6867, 6868, 6869,
				6870, 6871, 6872, 6873, 6874, 6875, 6876, 6877, 6878, 6879, 6880, 6881, 6882, 6883, 6884, 6885, 6886, 6887, 6888, 6889,
				6890, 6891, 6892, 6893, 6894, 6895, 6896, 6897, 6898, 6899, 6900, 6901, 6902, 6903, 6904, 6905, 6906, 6907, 6908, 6909,
				6910, 6911, 6912, 6913, 6914, 6915, 6916, 6917, 6918, 6919, 6920, 6921, 6922, 6923, 6924, 6925, 6926, 6927, 6928, 6929,
				6930, 6931, 6932, 6933, 6934, 6935, 6936, 6937, 6938, 6939, 6940, 6941, 6942, 6943, 6944, 6945, 6946, 6947, 6948, 6949,
				6950, 6951, 6952, 6953, 6954, 6955, 6956, 6957, 6958, 6959, 6960, 6961, 6962, 6963, 6964, 6965, 6966, 6967, 6968, 6969,
				6970, 6971, 6972, 6973, 6974, 6975, 6976, 6977, 6978, 6979, 6980, 6981, 6982, 6983, 6984, 6985, 6986, 6987, 6988, 6989,
				6990, 6991, 6992, 6993, 6994, 6995, 6996, 6997, 6998, 6999, 7000, 7001, 7002, 7003, 7004, 7005, 7006, 7007, 7008, 7009,
				7010, 7011, 7012, 7013, 7014, 7015, 7016, 7017, 7018, 7019, 7020, 7021, 7022, 7023, 7024, 7025, 7026, 7027, 7028, 7029,
				7030, 7031, 7032, 7033, 7034, 7035, 7036, 7037, 7038, 7039, 7040, 7041, 7042, 7043, 7044, 7045, 7046, 7047, 7048, 7049,
				7050, 7051, 7052, 7053, 7054, 7055, 7056, 7057, 7058, 7059, 7060, 7061, 7062, 7063, 7064, 7065, 7066, 7067, 7068, 7069,
				7070, 7071, 7072, 7073, 7074, 7075, 7076, 7077, 7078, 7079, 7080, 7081, 7082, 7083, 7084, 7085, 7086, 7087, 7088, 7089,
				7090, 7091, 7092, 7093, 7094, 7095, 7096, 7097, 7098, 7099, 7100, 7101, 7102, 7103, 7104, 7105, 7106, 7107, 7108, 7109,
				7110, 7111, 7112, 7113, 7114, 7115, 7116, 7117, 7118, 7119, 7120, 7121, 7122, 7123, 7124, 7125, 7126, 7127, 7128, 7129,
				7130, 7131, 7132, 7133, 7134, 7135, 7136, 7137, 7138, 7139, 7140, 7141, 7142, 7143, 7144, 7145, 7146, 7147, 7148, 7149,
				7150, 7151, 7152, 7153, 7154, 7155, 7156, 7157, 7158, 7159, 7160, 7161, 7162, 7163, 7164, 7165, 7166, 7167, 7168, 7169,
				7170, 7171, 7172, 7173, 7174, 7175, 7176, 7177, 7178, 7179, 7180, 7181, 7182, 7183, 7184, 7185, 7186, 7187, 7188, 7189,
				7190, 7191, 7192, 7193, 7194, 7195, 7196, 7197, 7198, 7199, 7200, 7201, 7202, 7203, 7204, 7205, 7206, 7207, 7208, 7209,
				7210, 7211, 7212, 7213, 7214, 7215, 7216, 7217, 7218, 7219, 7220, 7221, 7222, 7223, 7224, 7225, 7226, 7227, 7228, 7229,
				7230, 7231, 7232, 7233, 7234, 7235, 7236, 7237, 7238, 7239, 7240, 7241, 7242, 7243, 7244, 7245, 7246, 7247, 7248, 7249,
				7250, 7251, 7252, 7253, 7254, 7255, 7256, 7257, 7258, 7259, 7260, 7261, 7262, 7263, 7264, 7265, 7266, 7267, 7268, 7269,
				7270, 7271, 7272, 7273, 7274, 7275, 7276, 7277, 7278, 7279, 7280, 7281, 7282, 7283, 7284, 7285, 7286, 7287, 7288, 7289,
				7290, 7291, 7292, 7293, 7294, 7295, 7296, 7297, 7298, 7299, 7300, 7301, 7302, 7303, 7304, 7305, 7306, 7307, 7308, 7309,
				7310, 7311, 7312, 7313, 7314, 7315, 7316, 7317, 7318, 7319, 7320, 7321, 7322, 7323, 7324, 7325, 7326, 7327, 7328, 7329,
				7330, 7331, 7332, 7333, 7334, 7335, 7336, 7337, 7338, 7339, 7340, 7341, 7342, 7343, 7344, 7345, 7346, 7347, 7348, 7349,
				7350, 7351, 7352, 7353, 7354, 7355, 7356, 7357, 7358, 7359, 7360, 7361, 7362, 7363, 7364, 7365, 7366, 7367, 7368, 7369,
				7370, 7371, 7372, 7373, 7374, 7375, 7376, 7377, 7378, 7379, 7380, 7381, 7382, 7383, 7384, 7386, 7387, 7388, 7389, 7390,
				7391, 7392, 7393, 7394, 7395, 7396, 7397, 7398, 7399, 7400, 7401, 7402, 7403, 7404, 7405, 7406, 7407, 7408, 7409, 7410,
				7411, 7412, 7413, 7414, 7415, 7416, 7417, 7418, 7419, 7420, 7421, 7422, 7423, 7424, 7425, 7426, 7427, 7428, 7429, 7430,
				7431, 7432, 7433, 7434, 7435, 7436, 7437, 7438, 7439, 7440, 7441, 7442, 7443, 7444, 7445, 7446, 7447, 7448, 7449, 7450,
				7451, 7452, 7453, 7454, 7455, 7456, 7457, 7458, 7459, 7460, 7461, 7462, 7463, 7464, 7465, 7466, 7467, 7468, 7469, 7470,
				7471, 7472, 7473, 7474, 7475, 7476, 7477, 7478, 7479, 7480, 7481, 7482, 7483, 7484, 7485, 7486, 7487, 7488, 7489, 7490,
				7491, 7492, 7493, 7494, 7495, 7496, 7497, 7498, 7499, 7500, 7501, 7502, 7503, 7504, 7505, 7506, 7507, 7508, 7509, 7510,
				7511, 7512, 7513, 7514, 7515, 7516, 7517, 7518, 7519, 7520, 7521, 7522, 7523, 7524, 7525, 7526, 7527, 7528, 7529, 7530,
				7531, 7532, 7533, 7534, 7535, 7536, 7537, 7538, 7539, 7540, 7541, 7542, 7543, 7544, 7545, 7546, 7547, 7548, 7549, 7550,
				7551, 7552, 7553, 7554, 7555, 7556, 7557, 7558, 7559, 7560, 7561, 7562, 7563, 7564, 7565, 7567, 7568, 7569, 7570, 7571,
				7572, 7573, 7578, 7579, 7580, 7581, 7582, 7583, 7584, 7585, 7586, 7587, 7588, 7589, 7590, 7591, 7592, 7593, 7595, 7596,
				7597, 7598, 7599, 7600, 7601, 7602, 7603, 7604, 7605, 7606, 7607, 7608, 7609, 7610, 7611, 7612, 7613, 7614, 7615, 7616,
				7617, 7618, 7619, 7620, 7621, 7622, 7623, 7624, 7625, 7626, 7627, 7628, 7629, 7630, 7631, 7632, 7633, 7634, 7635, 7636,
				7637, 7638, 7639, 7640, 7641, 7642, 7643, 7644, 7645, 7646, 7647, 7648, 7649, 7650, 7651, 7652, 7653, 7654, 7655, 7656,
				7657, 7658, 7659, 7660, 7661, 7662, 7663, 7664, 7665, 7666, 7668, 7669, 7670, 7671, 7672, 7673, 7674, 7675, 7676, 7677,
				7678, 7679, 7680, 7681, 7682, 7683, 7684, 7685, 7686, 7687, 7688, 7689, 7690, 7693, 7694, 7695, 7696, 7697, 7698, 7699,
				7700, 7701, 7702, 7703, 7704, 7705, 7707, 7708, 7709, 7710, 7711, 7712, 7713, 7716, 7718, 7719, 7720, 7721, 7722, 7723,
				7724, 7725, 7726, 7727, 7728, 7729, 7730, 7731, 7732, 7733, 7734, 7735, 7736, 7737, 7738, 7739, 7740, 7741, 7742, 7743,
				7744, 7745, 7746, 7747, 7748, 7749, 7750, 7751, 7752, 7753, 7754, 7755, 7756, 7757, 7758, 7760, 7761, 7762, 7763, 7764,
				7765, 7766, 7767, 7768, 7769, 7770, 7771, 7772, 7773, 7774, 7775, 7776, 7777, 7778, 7779, 7780, 7781, 7782, 7783, 7784,
				7785, 7786, 7787, 7789, 7790, 7791, 7792, 7793, 7794, 7797, 7798, 7799, 7800, 7801, 7802, 7803, 7804, 7805, 7806, 7807,
				7808, 7809, 7810, 7811, 7812, 7813, 7814, 7815, 7816, 7817, 7819, 7820, 7821, 7822, 7823, 7824, 7825, 7826, 7827, 7828,
				7829, 7830, 7831, 7832, 7833, 7835, 7836, 7837, 7838, 7839, 7840, 7841, 7842, 7843, 7844, 7845, 7846, 7847, 7848, 7849,
				7850, 7851, 7852, 7853, 7854, 7855, 7856, 7857, 7858, 7859, 7860, 7861, 7862, 7863, 7864, 7865, 7866, 7867, 7868, 7869,
				7870, 7871, 7872, 7873, 7874, 7876, 7877, 7878, 7879, 7880, 7881, 7882, 7883, 7884, 7885, 7886, 7887, 7888, 7889, 7890,
				7891, 7892, 7893, 7894, 7895, 7896, 7897, 7898, 9265, 9266, 9267, 9268, 9269, 9270, 9271, 9272, 9273, 9274, 9275, 9276,
				9277, 9278, 9279, 9280, 9281, 9282, 9283, 9284, 9285, 9286, 9287, 9288, 9289, 9290, 9291, 9292, 9293, 9294, 9295, 9296,
				9297, 9298, 9299, 9300, 9301, 9302, 9303, 9304, 9305, 9306, 9307, 9308, 9309, 9310, 9311, 9312, 9313, 9314, 9315, 9316,
				9317, 9318, 9319, 9320, 9321, 9322, 9323, 9324, 9325, 9326, 9327, 9328, 9329, 9330, 9331, 9332, 9333, 9334, 9335, 9336,
				9337, 9338, 9339, 9340, 9341, 9342, 9343, 9344, 9345, 9346, 9347, 9348, 9349, 9350, 9351, 9352, 9353, 9354, 9355, 9356,
				9357, 9358, 9359, 9360, 9361, 9362, 9363, 9364, 9365, 9366, 9367, 9368, 9369, 9370, 9371, 9372, 9373, 9374, 9375, 9376,
				9377, 9378, 9379, 9380, 9381, 9382, 9383, 9384, 9385, 9386, 9387, 9388, 9389, 9390, 9391, 9392, 9393, 9394, 9395, 9396,
				9397, 9398, 9399, 9400, 9401, 9402, 9403, 9404, 9405, 9406, 9407, 9408, 9409, 9410, 9411, 9412, 9413, 9414, 9415, 9416,
				9417, 9418, 9419, 9420, 9421, 9422, 9423, 9424, 9425, 9426, 9427, 9428, 9429, 9430, 9431, 9432, 9433, 9434, 9435, 9436,
				9437, 9438, 9439, 9440, 9441, 9442, 9443, 9444, 9445, 9446, 9447, 9448, 9449, 9450, 9451, 9452, 9453, 9454, 9455, 9456,
				9457, 9458, 9459, 9460, 9461, 9462, 9463, 9464, 9465, 9466, 9467, 9468, 9469, 9470, 9471, 9472, 9473, 9474, 9475, 9476,
				9477, 9478, 9479, 9480, 9481, 9482, 9483, 9484, 9485, 9486, 9487, 9488, 9489, 9490, 9491, 9492, 9493, 9494, 9495, 9496,
				9497, 9498, 9499, 9500, 9501, 9502, 9503, 9504, 9505, 9506, 9507, 9508, 9509, 9510, 9511, 9512, 9513, 9514, 9515, 9516,
				9517, 9518, 9519, 9520, 9521, 9522, 9523, 9524, 9525, 9526, 9527, 9528, 9529, 9530, 9531, 9532, 9533, 9534, 9535, 9536,
				9537, 9538, 9539, 9540, 9541, 9542, 9543, 9544, 9545, 9546, 9547, 9548, 9549, 9550, 9551, 9552, 9553, 9554, 9555, 9556,
				9557, 9558, 9559, 9560, 9561, 9562, 9563, 9564, 9565, 9566, 9567, 9568, 9569, 9570, 9571, 9572, 9573, 9574, 9575, 9576,
				9577, 9578, 9579, 9580, 9581, 9582, 9583, 9584, 9585, 9586, 9587, 9588, 9589, 9590, 9591, 9592, 9593, 9594, 9595, 9596,
				9597, 9598, 9599, 9600, 9601, 9602, 9603, 9604, 9605, 9606, 9607, 9608, 9609, 9610, 9611, 9612, 9613, 9614, 9615, 9616,
				9617, 9618, 9619, 9620, 9621, 9622, 9623, 9624, 9625, 9626, 9627, 9628, 9629, 9630, 9631, 9632, 9633, 9634, 9635, 9636,
				9637, 9638, 9639, 9640, 9641, 9642, 9643, 9644, 9645, 9646, 9647, 9648, 9649, 9650, 9651, 9652, 9653, 9654, 9655, 9656,
				9657, 9658, 9659, 9660, 9661, 9662, 9663, 9664, 9665, 9666, 9667, 9668, 9669, 9670, 9671, 9914, 9918, 9919
		};
		private readonly int[] headEyeArray = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		private bool isF2;
		private bool isF3;
		private bool isSF1;
		private bool isSF2;
		private bool isSF3;
		private bool isHS1;
		private bool isHS2;
		private bool isHS3;
		private bool isHS4;
		private bool isHS5;
		private bool isHS6;
		private bool isHS7;
		private bool isHS8;
		private bool isHS9;
		private bool isSS;
		private bool isSS1;
		private bool isSS2;
		private bool isSS3;
		private bool isSS4;
		private bool isSS5;
		private bool isSS6;

		private string nameK;
		private string nameA;
		private string nameS;

		private int danceNo1;
		private int danceNo2;
		private int danceNo3;

		private bool isSN1;
		private bool isSN2;
		private bool isSN3;
		private bool isSN4;
		private bool isSN5;
		private bool isSN6;
		private bool isShift;
		private bool isCF1;
		private bool isKHG1;
		private bool isKHG2;
		private bool isKT1;
		private bool isSD1;
		private bool isScene;
		private bool isPanel;

		private int sPoseCount;

		private bool existPose;

		private string poseIniStr;

		private int[] poseCount;
		private int[] delCount;
		private int[] delCount2;
		private int maxMaidCnt;
		private int maidCnt;

		private Material m_material;

		private Vector3 cameraIti;
		private Vector3 cameraIti2;
		private Vector2 cameraItiAngle;

		private float cameraItiDistance;

		private Vector3[] armL;

		private bool isInit;

		private int myIndex;
		private int kankyoIndex;
		private int itemIndexB;
		private int[] poseIndex;
		private int[] itemIndex;
		private int[] itemIndex2;
		private int[] faceIndex;
		private int[] faceBlendIndex;
		private int[] headEyeIndex;
		private int copyIndex;

		private bool isCopy;
		private bool okFlg;
		private bool isDanceStart1;
		private bool isDanceStart1F;
		private bool isDanceStart1K;
		private bool isDanceStart2;
		private bool isDanceStart2F;
		private bool isDanceStart3;
		private bool isDanceStart3F;
		private bool isDanceStart3K;
		private bool isDanceStart4;
		private bool isDanceStart4F;
		private bool isDanceStart4K;
		private bool isDanceStart5;
		private bool isDanceStart5F;
		private bool isDanceStart5K;
		private bool isDanceStart6;
		private bool isDanceStart6F;
		private bool isDanceStart6K;
		private bool isDanceStart7;
		private bool isDanceStart7F;
		private bool isDanceStart7V;
		private bool isDanceStart8;
		private bool isDanceStart8F;
		private bool isDanceStart8V;
		private bool isDanceStart8P;
		private bool isDanceStart9;
		private bool isDanceStart9F;
		private bool isDanceStart9K;

		private int isDanceStart9Count;

		private bool isDanceStart10;
		private bool isDanceStart10F;
		private bool isDanceStart11;
		private bool isDanceStart11F;
		private bool isDanceStart11V;
		private bool isDanceStart12;
		private bool isDanceStart12F;
		private bool isDanceStart13;
		private bool isDanceStart13F;
		private bool isDanceStart13K;

		private int isDanceStart13Count;

		private bool isDanceStart14;
		private bool isDanceStart14F;
		private bool isDanceStart14V;
		private bool isDanceStart15;
		private bool isDanceStart15F;
		private bool isDanceStart15V;

		private int isDanceStart15Count;

		private float[] danceFace;

		private bool isF6;
		private bool isF6S;

		private Vector3[] dancePos;

		private Quaternion[] danceRot;

		private int maxPage;

		private float[] danceCheck;

		private int danceCheckIndex;
		private int danceWait;
		private int danceCount;
		private int bgmIndex;
		private int effectIndex;

		private float cubeSize;

		private List<int> doguIndex;

		private int doguSelectIndex;

		private List<GameObject> doguObject;
		private List<GameObject> doguBObject;

		private GameObject kami;

		private bool allowUpdate;
		private bool moveBg;

		private int bgIndex;
		private int bgIndexB;
		private int slotIndex;
		private int wearIndex;
		private int bgIndex6;

		private string[] FaceName;
		private string[] FaceName2;
		private string[] FaceName3;
		private float[] FaceTime;

		private bool keyFlg;
		private bool[] xFlg;
		private bool[] zFlg;
		private bool[] cafeFlg;

		private int[] cafeCount;

		private bool[] isBone;
		private bool[] isBoneN;
		private bool[] isChange;

		private List<UICamera> ui_cam_hide_list_;

		private bool isScreen;
		private bool isScreen2;
		private bool isGui;

		private List<TBodySkin>[] goSlot;
		private List<TBodyHit>[] bodyHit;
		private Vector3[] eyeL;
		private Vector3[] eyeR;

		private bool[] shodaiFlg;

		private Vector3 softG;
		private Vector3 softG2;
		private UltimateOrbitCamera m_UOCamera;
		private GUIContent[] comboBoxList;
		private ComboBox2 comboBoxControl;

		private GUIStyle listStyle;
		private GUIStyle listStyle2;
		private GUIStyle listStyle3;
		private GUIStyle listStyle4;

		private ComboBox2 faceCombo;
		private GUIContent[] faceComboList;

		private bool faceInitFlg;
		private bool[] isPose;

		private ComboBox2 poseCombo;
		private GUIContent[] poseComboList;

		private bool poseInitFlg;
		private bool kankyoInitFlg;
		private bool kankyo2InitFlg;
		private bool isPoseInit;

		private int poseGroupIndex;
		private bool[] isPoseIti;

		private Vector3[] poseIti;
		private ComboBox2 poseGroupCombo;
		private GUIContent[] poseGroupComboList;
		private ComboBox2 itemCombo;
		private GUIContent[] itemComboList;
		private ComboBox2 bgmCombo;
		private GUIContent[] bgmComboList;
		private ComboBox2 itemCombo2;
		private GUIContent[] itemCombo2List;
		private ComboBox2 myCombo;
		private GUIContent[] myComboList;
		private ComboBox2 bgCombo2;
		private GUIContent[] bgCombo2List;
		private ComboBox2 bgCombo;
		private GUIContent[] bgComboList;
		private ComboBox2 slotCombo;
		private GUIContent[] slotComboList;
		private ComboBox2 doguCombo2;
		private GUIContent[] doguCombo2List;
		private ComboBox2[] doguCombo;

		private List<GUIContent[]> doguComboList;
		private ComboBox2 parCombo;
		private GUIContent[] parComboList;
		private ComboBox2 parCombo1;
		private GUIContent[] parCombo1List;
		private ComboBox2 lightCombo;
		private GUIContent[] lightComboList;
		private ComboBox2 kankyoCombo;
		private GUIContent[] kankyoComboList;

		private ArrayList selectList;
		private Maid editSelectMaid;
		private bool[] isStop;
		private Maid[] maidArray;
		private string[] danceName;
		private KeyCode[] keyArray;

		private bool[] idoFlg;
		private bool[] isIK;
		private bool[] isLock;

		private bool isIKAll;
		private bool faceFlg;
		private bool faceFlg2;
		private bool poseFlg;
		private bool unLockFlg;
		private bool sceneFlg;
		private bool kankyoFlg;
		private bool kankyo2Flg;
		private bool modFlg;
		private bool nmodFlg;
		private bool doguSelectFlg1;
		private bool doguSelectFlg2;
		private bool doguSelectFlg3;
		private bool isDanceStop;

		private bool[] isFace;
		private bool[] isMabataki;
		private bool[] mekure1;
		private bool[] mekure2;
		private bool[] zurasi;
		private bool[] mekure1n;
		private bool[] mekure2n;
		private bool[] zurasin;
		private bool[] hanten;
		private bool[] hantenn;

		private bool isHanten;

		private bool[] kotei;
		private bool[] voice1;
		private bool[] voice1n;
		private bool[] voice2;
		private bool[] voice2n;

		private int[] pHandL;
		private int[] pHandR;
		private bool[] isLook;

		private float[] lookX;
		private float[] lookY;
		private bool[] isLookn;
		private float[] lookXn;
		private float[] lookYn;

		private bool isPoseEdit;
		private bool isFaceEdit;
		private bool isNamida;
		private bool isTear1;
		private bool isTear2;
		private bool isTear3;
		private bool isShock;
		private bool isYodare;
		private bool isHoho;
		private bool isHoho2;
		private bool isHohos;
		private bool isHohol;
		private bool isFaceInit;
		private bool isNamidaH;
		private bool isSekimenH;
		private bool isHohoH;
		private bool isHenkou;

		private bool isWear;
		private bool isSkirt;
		private bool isBra;
		private bool isPanz;

		private bool isMaid;
		private bool isMekure1;
		private bool isMekure2;
		private bool isZurasi;
		private bool isMekure1a;
		private bool isMekure2a;
		private bool isZurasia;

		private bool isHeadset;
		private bool isAccUde;
		private bool isStkg;
		private bool isShoes;
		private bool isGlove;
		private bool isMegane;
		private bool isAccSenaka;

		private bool isBloom;
		private bool isBloom2;
		private bool isBloomA;
		private bool isDepth;
		private bool isDepthA;
		private bool isFog;
		private bool isSepia;
		private bool isSepian;
		private bool isBlur;
		private bool isBlur2;
		private bool isCube;
		private bool isCube2;
		private bool isCube3;
		private bool isCube4;

		private bool isKamiyure;
		private bool isSkirtyure;
		private bool isHairSetting;
		private bool isSkirtSetting;
		private bool isVRScroll;

		private bool isCubeS;
		private bool isBloomS;
		private bool isDepthS;
		private bool isBlurS;
		private bool isFogS;

		private float bloom1;
		private float bloom2;
		private float bloom3;
		private float bloom4;
		private float bloom5;

		private float blur1;
		private float blur2;
		private float blur3;
		private float blur4;

		private float depth1;
		private float depth2;
		private float depth3;
		private float depth4;

		private float fog1;
		private float fog2;
		private float fog3;
		private float fog4;
		private float fog5;
		private float fog6;
		private float fog7;

		private float bokashi;
		private float kamiyure;
		private float kamiyure2;
		private float kamiyure3;
		private float kamiyure4;
		private float skirtyure2;
		private float skirtyure3;
		private float skirtyure4;

		private float eyeclose;
		private float eyeclose2;
		private float eyeclose3;
		private float eyeclose6;
		private float eyeclose7;
		private float eyeclose8;

		private float hitomih;
		private float hitomis;
		private float mayuha;
		private float mayuup;
		private float mayuv;
		private float mayuvhalf;

		private float moutha;
		private float mouths;
		private float mouthdw;
		private float mouthup;

		private float tangout;
		private float tangup;
		private float eyebig;
		private float eyeclose5;
		private float mayuw;
		private float mouthhe;
		private float mouthc;
		private float mouthi;
		private float mouthuphalf;

		private float tangopen;
		private bool isToothoff;
		private bool isNosefook;
		private int selectMaidIndex;

		private bool isCombo;
		private bool isCombo2;
		private bool isCombo3;

		private bool hFlg;
		private bool h2Flg;
		private bool mFlg;
		private bool fFlg;
		private bool qFlg;
		private bool sFlg;
		private bool atFlg;
		private bool escFlg;
		private bool yFlg;

		private bool isVP;
		private bool isPP;
		private bool isPP2;
		private bool isPP3;
		private bool isVA;
		private bool isKA;
		private bool isKA2;

		private CameraMain mainCamera;
		private Transform mainCameraTransform;
		private Transform bg;
		private GameObject bgObject;

		private bool isDance;

		private List<int> lightIndex;
		private List<float> lightColorR;
		private List<float> lightColorG;
		private List<float> lightColorB;
		private List<float> lightX;
		private List<float> lightY;
		private List<float> lightAkarusa;
		private List<float> lightKage;
		private List<float> lightRange;
		private List<GameObject> lightList;
		private List<int> doguList;
		private List<int> parList;

		private int doguCnt;
		private float lightX6;
		private float lightY6;
		private int selectLightIndex;
		private int doguB2Index;

		private int[] doguBIndex;
		private int parIndex;
		private int parIndex1;
		private int sceneLevel;

		private bool isVR;
		private bool isVR2;
		private bool isYobidashi;
		private bool isFadeOut;
		private bool isBusyInit;
		private bool isHaiti;
		private bool isF7;
		private bool isF7S;
		private bool isF7SInit;
		private bool isGuiInit;

		private float speed;
		private int saveScene;
		private int saveScene2;
		private int loadScene;
		private string[] loadPose;
		private bool[] isLoadPose;
		private int[] loadCount;
		private bool kankyoLoadFlg;
		private bool nameFlg;
		private int kankyoMax;

		private string[] date;
		private string[] ninzu;
		private int ikMaid;
		private int ikBui;
		private int[] ikMode;
		private int[] ikModeOld;
		private int ikMode2;
		private int ikModeOld2;
		private int editMaid;
		private bool isScript;

		private Stopwatch sw;
		private AudioSource audioSourceBgm;
		private MouseDrag6[] mDogu;
		private GameObject[] gDogu;
		private MouseDrag6 mBg;
		private GameObject gBg;
		private MouseDrag6[] mLight;
		private GameObject[] gLight;
		private IK ikLeftArm;

		private MouseDrag5[] mHead2;
		private MouseDrag5[] mMaid2;
		private MouseDrag2[] mMaid;
		private GameObject[] gMaid;
		private MouseDrag2[] mMaidC;
		private GameObject[] gMaidC;
		private MouseDrag3[] mHead;
		private GameObject[] gHead;
		private GameObject[] gMaid2;
		private GameObject[] gHead2;
		private MouseDrag3[] mJotai;
		private GameObject[] gJotai;
		private MouseDrag3[] mKahuku;
		private GameObject[] gKahuku;

		private GameObject[] gIKMuneL;
		private MouseDrag[] mIKMuneL;
		private GameObject[] gIKMuneR;
		private MouseDrag[] mIKMuneR;


		private Vector3[] vIKMuneL;
		private Vector3[] vIKMuneR;
		private Vector3[] vIKMuneLSub;
		private Vector3[] vIKMuneRSub;

		private int[] haraCount;
		private Vector3[] haraPosition;
		private bool[] muneIKL;
		private bool[] muneIKR;

		private GameObject[] gIKHandL;
		private MouseDrag4[] mIKHandL;
		private GameObject[] gIKHandR;
		private MouseDrag4[] mIKHandR;
		private GameObject[] gHandL;
		private MouseDrag[] mHandL;
		private GameObject[] gArmL;
		private MouseDrag[] mArmL;
		private GameObject[] gFootL;
		private MouseDrag[] mFootL;
		private GameObject[] gHizaL;
		private MouseDrag[] mHizaL;
		private GameObject[] gHandR;
		private MouseDrag[] mHandR;
		private GameObject[] gArmR;
		private MouseDrag[] mArmR;
		private GameObject[] gFootR;
		private MouseDrag[] mFootR;
		private GameObject[] gHizaR;
		private MouseDrag[] mHizaR;
		private GameObject[] gClavicleL;
		private MouseDrag[] mClavicleL;
		private GameObject[] gClavicleR;
		private MouseDrag[] mClavicleR;
		private GameObject[,] gFinger;
		private MouseDrag[,] mFinger;
		private GameObject[,] gFinger2;
		private MouseDrag[,] mFinger2;
		private GameObject[] gNeck;
		private MouseDrag3[] mNeck;
		private GameObject[] gSpine;
		private MouseDrag3[] mSpine;
		private GameObject[] gSpine0a;
		private MouseDrag3[] mSpine0a;
		private GameObject[] gSpine1;
		private MouseDrag3[] mSpine1;
		private GameObject[] gSpine1a;
		private MouseDrag3[] mSpine1a;
		private GameObject[] gPelvis;
		private MouseDrag3[] mPelvis;

		private Transform HandL;
		private Transform UpperArmL;
		private Transform ForearmL;
		private Transform Head;
		private Transform Spine;
		private Transform Spine0a;
		private Transform Spine1;
		private Transform Spine1a;
		private Transform Pelvis;
		private Transform Clavicle;
		private Transform IK_hand;
		private Transform[] IKHandL;
		private Transform[] IKHandR;
		private Transform[] IKMuneL;
		private Transform[] IKMuneR;
		private Transform[] IKMuneLSub;
		private Transform[] IKMuneRSub;
		private Transform[] Neck;
		private Transform[] Pelvis2;
		private Transform[] Spine12;
		private Transform[] Spine0a2;
		private Transform[] Spine2;
		private Transform[] Spine1a2;
		private Transform[] Head1;
		private Transform[] Head2;
		private Transform[] Head3;
		private Transform[] HandL1;
		private Transform[] UpperArmL1;
		private Transform[] ForearmL1;
		private Transform[] HandR1;
		private Transform[] UpperArmR1;
		private Transform[] ForearmR1;
		private Transform[] HandL2;
		private Transform[] UpperArmL2;
		private Transform[] ForearmL2;
		private Transform[] HandR2;
		private Transform[] UpperArmR2;
		private Transform[] ForearmR2;
		private Transform[] ClavicleL1;
		private Transform[] ClavicleR1;
		private Transform[,] Finger;
		private Transform[,] Finger2;

		private bool isSavePose;
		private bool isSavePose2;
		private bool isSavePose3;
		private bool isSavePose4;
		private Vector3 bipPosition;
		private Vector3 bipRotation;

		private bool yotogiFlg;
		private bool isShosai;
		private bool[] isEdit;
		private int isEditNo;

		private bool[] isLoadFace;
		private bool isDanceChu;
		private bool isMessage;

		private GameObject editUI;
		private bool isPref;
		private string thum_file_path_;
		private string thum_byte_to_base64_;

		private GameObject cameraObj;
		private Camera subcamera;
		private DepthOfFieldScatter depth_field_;
		private GlobalFog fog_;
		private SepiaToneEffect sepia_tone_;
		private DynamicSkirtBone[][] SkirtListArray;
		private Transform[] JumpChkTArray;
		private Vector3[] JumpChkPosArray;
		private GameObject[] gt;
		private Vector3[] wv;

		private bool bGui;
		private bool bGuiMessage;
		private Rect rectWin;
		private Rect rectWin2;
		private Vector2 screenSize;
		private Vector2 scrollPos;

		private string inText;
		private string inName;
		private string inName2;
		private string inName3;
		private string inName4;
		private int fontSize;
		private int mFontSize;

		private Texture2D line1;
		private Texture2D line2;

		private int page;
		private int dispNo;
		private int dispNoOld;

		private Texture2D texture2D;
		private Texture2D waku;
		private Texture2D waku2;

		// Token: 0x040002D8 RID: 728
		private List<MultipleMaids.SortItem> sortList;
		private List<MultipleMaids.SortItemMy> sortListMy;
		private List<MultipleMaids.ItemData> itemDataList;
		private List<MultipleMaids.ItemData> itemDataListMod;
		private List<MultipleMaids.ItemData> itemDataListNMod;

		private bool isIdx1;
		private bool isIdx2;
		private bool isIdx3;
		private bool isIdx4;

		private Material m_material2;
		private Material m_material3;

		private GizmoRender[] gizmoHandL;
		private GizmoRender[] gizmoHandR;
		private GizmoRender[] gizmoFootL;
		private GizmoRender[] gizmoFootR;

		private enum modKey
		{
			Shift,
			Alt,
			Ctrl
		}

		private class LastParam
		{
			public LastParam(int f_nOrder, string f_strComm, params string[] f_argArgs)
			{
				this.nOrder = f_nOrder;
				this.strComm = f_strComm;
				this.aryArgs = new string[f_argArgs.Length];
				f_argArgs.CopyTo(this.aryArgs, 0);
			}
			public int nOrder;
			public string strComm = string.Empty;
			public string[] aryArgs;
		}

		private class SortItem
		{
			public int order;
			public string name = string.Empty;
			public string menu = string.Empty;
			public Texture2D tex = null;
		}

		private class SortItemMy
		{
			public int order;
			public string name = string.Empty;
			public string menu = string.Empty;
			public Texture tex = null;
		}

		private class ItemData
		{
			public override bool Equals(object obj)
			{
				MultipleMaids.ItemData itemData = obj as MultipleMaids.ItemData;
				return itemData != null && this.name == itemData.name && this.menu == itemData.menu;
			}

			public override int GetHashCode()
			{
				return this.menu.GetHashCode();
			}

			public int order;
			public string name = string.Empty;
			public string menu = string.Empty;
			public byte[] cd = null;
			public string info = string.Empty;
			public Texture2D tex = null;
		}

		public class ItemData2
		{
			public ItemData2(CsvParser csv, int csv_y)
			{
				int num = 0;
				this.id = csv.GetCellAsInteger(num++, csv_y);
				this.name = csv.GetCellAsString(num++, csv_y);
				this.category_id = csv.GetCellAsInteger(num++, csv_y);
				this.prefab_name = csv.GetCellAsString(num++, csv_y);
				this.asset_name = csv.GetCellAsString(num++, csv_y);
				this.possession_flag = csv.GetCellAsString(num++, csv_y);
				string cellAsString = csv.GetCellAsString(num++, csv_y);
				this.seasonal = !string.IsNullOrEmpty(cellAsString);
				if (this.seasonal)
				{
					string[] array = cellAsString.Split(new char[]
					{
						','
					});
					this.seasonal_month = new int[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						this.seasonal_month[i] = int.Parse(array[i]);
					}
				}
				else
				{
					this.seasonal_month = new int[0];
				}
				this.init_visible = (csv.GetCellAsString(num++, csv_y) == "○");
				this.init_position = csv.GetCellAsVector3(num++, csv_y, ',');
				this.init_rotation = csv.GetCellAsVector3(num++, csv_y, ',');
				this.init_scale = csv.GetCellAsVector3(num++, csv_y, ',');
				this.init_ui_position_scale = csv.GetCellAsReal(num++, csv_y);
				this.init_ui_rotation_scale = csv.GetCellAsReal(num++, csv_y);
			}

			public readonly int id;
			public readonly string name;
			public readonly int category_id;
			public readonly string prefab_name;
			public readonly string asset_name;
			public readonly bool seasonal;
			public readonly string possession_flag;
			public readonly int[] seasonal_month;
			public readonly bool init_visible;
			public readonly Vector3 init_position;
			public readonly Vector3 init_rotation;
			public readonly Vector3 init_scale;
			public readonly float init_ui_position_scale;
			public readonly float init_ui_rotation_scale;
		}
	}
}
