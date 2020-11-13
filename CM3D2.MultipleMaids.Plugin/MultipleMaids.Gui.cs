using ExIni;
using MyRoomCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace CM3D2.MultipleMaids.Plugin
{
    public partial class MultipleMaids
    {
		public void OnGUI()
		{
			for (int i = 0; i < this.maidCnt; i++)
			{
				if (this.isPoseIti[i])
				{
					Maid maid = this.maidArray[i];
					this.isPoseIti[i] = false;
					maid.transform.position = this.poseIti[i];
					Vector3 eulerAngles = maid.transform.eulerAngles;
					for (int j = 0; j < 10; j++)
					{
						maid.transform.RotateAround(maid.transform.position, Vector3.right, -maid.transform.rotation.eulerAngles.x);
						maid.transform.RotateAround(maid.transform.position, Vector3.forward, -maid.transform.rotation.eulerAngles.z);
					}
					Transform transform = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
					transform.position = new Vector3(this.poseIti[i].x, transform.position.y, this.poseIti[i].z);
					maid.transform.eulerAngles = eulerAngles;
				}
			}
			GUIStyle guistyle = "box";
			guistyle.fontSize = this.GetPix(11);
			guistyle.alignment = TextAnchor.UpperRight;
			if (this.bGui)
			{
				if (this.isGuiInit || this.screenSize != new Vector2((float)Screen.width, (float)Screen.height))
				{
					this.isGuiInit = false;
					this.screenSize = new Vector2((float)Screen.width, (float)Screen.height);
				}
				if (this.sceneLevel != 5 && this.sceneLevel != 14)
				{
					if (this.kankyoFlg || this.kankyo2Flg)
					{
						this.rectWin.Set(0f, 0f, (float)this.GetPix(210), (float)Screen.height * 0.9f);
					}
					else
					{
						this.rectWin.Set(0f, 0f, (float)this.GetPix(170), (float)Screen.height * 0.9f);
					}
				}
				else if (this.kankyoFlg || this.kankyo2Flg)
				{
					this.rectWin.Set(0f, 0f, (float)this.GetPix(210), (float)Screen.height * 0.9f * 0.85f);
				}
				else
				{
					this.rectWin.Set(0f, 0f, (float)this.GetPix(170), (float)Screen.height * 0.9f * 0.85f);
				}
				this.rectWin.x = this.screenSize.x - this.rectWin.width;
				this.rectWin.y = (float)this.GetPix(65);
				if (this.sceneLevel == 14)
				{
					this.rectWin.x = this.screenSize.x - this.rectWin.width - (float)this.GetPix(23);
				}
				this.comboBoxControl.height = this.rectWin.height;
				this.faceCombo.height = this.rectWin.height;
				this.poseCombo.height = this.rectWin.height;
				this.poseGroupCombo.height = this.rectWin.height;
				this.itemCombo.height = this.rectWin.height;
				this.bgmCombo.height = this.rectWin.height;
				this.itemCombo2.height = this.rectWin.height;
				this.myCombo.height = this.rectWin.height;
				this.bgCombo2.height = this.rectWin.height;
				this.kankyoCombo.height = this.rectWin.height;
				this.bgCombo.height = this.rectWin.height;
				this.slotCombo.height = this.rectWin.height;
				this.doguCombo2.height = this.rectWin.height;
				for (int i = 0; i < this.doguCombo.Length; i++)
				{
					this.doguCombo[i].height = this.rectWin.height;
				}
				this.parCombo.height = this.rectWin.height;
				this.parCombo1.height = this.rectWin.height;
				this.lightCombo.height = this.rectWin.height;
				GameMain.Instance.MainCamera.SetControl(true);
				if (!this.sceneFlg && !this.faceFlg && !this.poseFlg && !this.kankyoFlg && !this.kankyo2Flg && !this.isF6 && this.okFlg)
				{
					if (Input.GetAxis("Mouse ScrollWheel") != 0f)
					{
						GameMain.Instance.MainCamera.SetControl(!this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)));
					}
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc), "", guistyle);
				}
				else if (this.sceneFlg)
				{
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc5), "", guistyle);
					Rect rect = new Rect(0f, 0f, 0f, 0f);
					this.dispNo = 0;
					for (int i = 0; i < 10; i++)
					{
						rect = new Rect(0f, 0f, (float)this.GetPix(170), (float)this.GetPix(36));
						rect.x = this.screenSize.x - rect.width;
						rect.y = this.rectWin.y + (float)this.GetPix(64 + 50 * i);
						if (rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
						{
							this.dispNo = i + 1;
							break;
						}
					}
					if (this.saveScene2 > 0)
					{
						this.dispNo = 0;
					}
					if (this.dispNo == 0)
					{
						this.texture2D = null;
						this.dispNoOld = 0;
					}
					else if (this.dispNo != this.dispNoOld)
					{
						this.dispNoOld = this.dispNo;
						this.texture2D = null;
						try
						{
							string path = string.Concat(new object[]
							{
								Path.GetFullPath(".\\"),
								"Mod\\MultipleMaidsScene\\",
								this.page * 10 + this.dispNo,
								".png"
							});
							if (File.Exists(path))
							{
								FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
								BinaryReader binaryReader = new BinaryReader(input);
								byte[] data = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
								binaryReader.Close();
								this.texture2D = new Texture2D(640, 360);
								this.texture2D.LoadImage(data);
							}
							else
							{
								IniKey iniKey = base.Preferences["scene"]["ss" + (this.page * 10 + this.dispNo)];
								if (iniKey.Value != null && iniKey.Value != "")
								{
									byte[] data2 = Convert.FromBase64String(iniKey.Value);
									this.texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
									this.texture2D.LoadImage(data2);
								}
							}
						}
						catch
						{
						}
					}
					if (this.texture2D != null)
					{
						if (this.waku == null)
						{
							this.waku = this.MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
							this.waku2 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.4f));
						}
						Rect position = new Rect(rect.x - (float)this.texture2D.width - (float)this.GetPix(18), rect.y - (float)(this.texture2D.height / 2) + (float)this.GetPix(12), (float)(this.texture2D.width + this.GetPix(12)), (float)(this.texture2D.height + this.GetPix(12)));
						Rect position2 = new Rect(rect.x - (float)this.texture2D.width - (float)this.GetPix(12), rect.y - (float)(this.texture2D.height / 2) + (float)this.GetPix(18), (float)this.texture2D.width, (float)this.texture2D.height);
						Rect position3 = new Rect(rect.x - (float)this.texture2D.width - (float)this.GetPix(16), rect.y - (float)(this.texture2D.height / 2) + (float)this.GetPix(14), (float)(this.texture2D.width + this.GetPix(12)), (float)(this.texture2D.height + this.GetPix(12)));
						if (position.y + position.height > (float)Screen.height)
						{
							float num = position3.y + position3.height - (float)Screen.height;
							position.y -= num;
							position2.y -= num;
							position3.y -= num;
						}
						if (position.y < 0f)
						{
							float num = position3.y;
							position.y -= num;
							position2.y -= num;
							position3.y -= num;
						}
						GUI.DrawTexture(position3, this.waku2);
						GUI.DrawTexture(position, this.waku);
						GUI.DrawTexture(position2, this.texture2D);
					}
				}
				else if (this.kankyoFlg)
				{
					if (this.bgmCombo.isClickedComboButton || this.bgCombo.isClickedComboButton || this.parCombo.isClickedComboButton)
					{
						if (Input.GetAxis("Mouse ScrollWheel") != 0f)
						{
							GameMain.Instance.MainCamera.SetControl(!this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)));
						}
					}
					else if (!this.yotogiFlg && this.sceneLevel != 3 && this.sceneLevel != 5 && this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
					{
						if (Input.GetMouseButtonDown(0))
						{
							Input.ResetInputAxes();
						}
					}
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc6), "", guistyle);
				}
				else if (this.kankyo2Flg)
				{
					if (Input.GetAxis("Mouse ScrollWheel") != 0f)
					{
						GameMain.Instance.MainCamera.SetControl(!this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)));
					}
					if (!this.yotogiFlg && this.sceneLevel != 3 && this.sceneLevel != 5 && this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
					{
						if (Input.GetMouseButtonDown(0))
						{
							Input.ResetInputAxes();
						}
					}
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc7), "", guistyle);
				}
				else if (this.poseFlg)
				{
					if (this.poseGroupCombo.isClickedComboButton || this.poseCombo.isClickedComboButton || this.itemCombo.isClickedComboButton)
					{
						if (Input.GetAxis("Mouse ScrollWheel") != 0f)
						{
							GameMain.Instance.MainCamera.SetControl(!this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)));
						}
					}
					else if (!this.yotogiFlg && this.sceneLevel != 3 && this.sceneLevel != 5 && this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
					{
						if (Input.GetMouseButtonDown(0))
						{
							Input.ResetInputAxes();
						}
					}
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc4), "", guistyle);
				}
				else
				{
					if (this.faceCombo.isClickedComboButton)
					{
						if (Input.GetAxis("Mouse ScrollWheel") != 0f)
						{
							GameMain.Instance.MainCamera.SetControl(!this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)));
						}
					}
					else if (!this.yotogiFlg && this.sceneLevel != 3 && this.sceneLevel != 5 && this.rectWin.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
					{
						if (Input.GetMouseButtonDown(0))
						{
							Input.ResetInputAxes();
						}
					}
					this.rectWin = GUI.Window(129, this.rectWin, new GUI.WindowFunction(this.GuiFunc2), "", guistyle);
				}
			}
			if (this.bGuiMessage)
			{
				this.screenSize = new Vector2((float)Screen.width, (float)Screen.height);
				this.rectWin2.Set(0f, 0f, (float)Screen.width * 0.4f, (float)Screen.height * 0.15f);
				this.rectWin2.x = this.screenSize.x / 2f - this.rectWin2.width / 2f;
				if (this.sceneLevel == 5)
				{
					this.rectWin2.y = this.screenSize.y * 0.94f - this.rectWin2.height;
				}
				else
				{
					this.rectWin2.y = this.screenSize.y - this.rectWin2.height;
				}
				this.rectWin2 = GUI.Window(129, this.rectWin2, new GUI.WindowFunction(this.GuiFunc3), "", guistyle);
			}
		}
		private void GuiFunc3(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "button";
			guistyle2.fontSize = this.GetPix(16);
			GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(4), (float)this.GetPix(80), (float)this.GetPix(25)), "名前", guistyle);
			this.inName = GUI.TextField(new Rect((float)this.GetPix(35), (float)this.GetPix(4), (float)this.GetPix(120), (float)this.GetPix(20)), this.inName);
			GUI.Label(new Rect((float)this.GetPix(180), (float)this.GetPix(4), (float)this.GetPix(100), (float)this.GetPix(25)), "サイズ", guistyle);
			this.fontSize = (int)GUI.HorizontalSlider(new Rect((float)this.GetPix(220), (float)this.GetPix(9), (float)this.GetPix(100), (float)this.GetPix(20)), (float)this.fontSize, 25f, 60f);
			if (this.fontSize != this.mFontSize)
			{
				this.mFontSize = this.fontSize;
				GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
				GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
				MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
				MessageClass messageClass = new MessageClass(gameObject2, messageWindowMgr);
				UILabel component = UTY.GetChildObject(gameObject2, "MessageViewer/MsgParent/Message", false).GetComponent<UILabel>();
				MultipleMaids.SetFieldValue2<UILabel, int>(component, "mFontSize", this.fontSize);
			}
			GUI.Label(new Rect((float)this.GetPix(325), (float)this.GetPix(4), (float)this.GetPix(100), (float)this.GetPix(25)), this.fontSize + "pt", guistyle);
			Rect position = new Rect((float)this.GetPix(8), (float)this.GetPix(26), this.rectWin2.width - (float)this.GetPix(15), (float)this.GetPix(52));
			this.inText = GUI.TextArea(position, this.inText, 93);
			if (GUI.Button(new Rect((float)this.GetPix(8), (float)this.GetPix(82), (float)this.GetPix(60), (float)this.GetPix(20)), "決定", guistyle2))
			{
				this.isMessage = true;
				this.bGuiMessage = false;
				GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
				GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
				MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
				messageWindowMgr.OpenMessageWindowPanel();
				MessageClass messageClass = new MessageClass(gameObject2, messageWindowMgr);
				UILabel component = UTY.GetChildObject(gameObject2, "MessageViewer/MsgParent/Message", false).GetComponent<UILabel>();
				component.ProcessText();
				MultipleMaids.SetFieldValue2<UILabel, int>(component, "mFontSize", this.fontSize);
				MultipleMaids.SetFieldValue5<MessageClass, UILabel>(messageClass, "message_label_", component);
				messageClass.SetText(this.inName, this.inText, "", 0);
				messageClass.FinishChAnime();
			}
		}
		private void GuiFunc5(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "button";
			guistyle2.fontSize = this.GetPix(12);
			guistyle2.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect((float)this.GetPix(50), (float)this.GetPix(6), (float)this.GetPix(100), (float)this.GetPix(25)), "シーン管理", guistyle);
			if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(3), (float)this.GetPix(23), (float)this.GetPix(23)), "→", guistyle2))
			{
				this.faceFlg = false;
				this.poseFlg = false;
				this.sceneFlg = false;
				this.kankyoFlg = true;
				this.kankyo2Flg = false;
				this.bGui = true;
				this.isGuiInit = true;
				this.copyIndex = 0;
			}
			int num = 50;
			if (GUI.Button(new Rect((float)this.GetPix(25), (float)this.GetPix(31), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle2))
			{
				this.page--;
				if (this.page < 0)
				{
					this.page = this.maxPage - 1;
				}
				int i = 0;
				while (i < 10)
				{
					this.date[i] = "未保存";
					this.ninzu[i] = "";
					string path = string.Concat(new object[]
					{
						Path.GetFullPath(".\\"),
						"Mod\\MultipleMaidsScene\\",
						this.page * 10 + i + 1,
						".png"
					});
					if (File.Exists(path))
					{
						FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
						BinaryReader binaryReader = new BinaryReader(input);
						byte[] array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
						byte[] value = new byte[]
						{
							array[36],
							array[35],
							array[34],
							array[33]
						};
						int count = BitConverter.ToInt32(value, 0) - 8;
						byte[] bytes = array.Skip(49).Take(count).ToArray<byte>();
						string text = Encoding.UTF8.GetString(bytes);
						text = MultipleMaids.StringFromBase64Comp(text);
						if (text != "")
						{
							string[] array2 = text.Split(new char[]
							{
								'_'
							});
							if (array2.Length >= 2)
							{
								string[] array3 = array2[0].Split(new char[]
								{
									','
								});
								this.date[i] = array3[0];
								this.ninzu[i] = array3[1] + "人";
							}
						}
					}
					else
					{
						IniKey iniKey = base.Preferences["scene"]["s" + (this.page * 10 + i + 1)];
						if (iniKey.Value != null && iniKey.Value.ToString() != "")
						{
							string[] array2 = iniKey.Value.Split(new char[]
							{
								'_'
							});
							if (array2.Length >= 2)
							{
								string[] array3 = array2[0].Split(new char[]
								{
									','
								});
								this.date[i] = array3[0];
								this.ninzu[i] = array3[1] + "人";
							}
						}
					}
					//IL_3CD:
					i++;
					continue;
					//goto IL_3CD;
				}
			}
			if (GUI.Button(new Rect((float)this.GetPix(115), (float)this.GetPix(31), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle2))
			{
				this.page++;
				if (this.page >= this.maxPage)
				{
					this.page = 0;
				}
				int i = 0;
				while (i < 10)
				{
					this.date[i] = "未保存";
					this.ninzu[i] = "";
					string path = string.Concat(new object[]
					{
						Path.GetFullPath(".\\"),
						"Mod\\MultipleMaidsScene\\",
						this.page * 10 + i + 1,
						".png"
					});
					if (File.Exists(path))
					{
						FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
						BinaryReader binaryReader = new BinaryReader(input);
						byte[] array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
						byte[] value = new byte[]
						{
							array[36],
							array[35],
							array[34],
							array[33]
						};
						int count = BitConverter.ToInt32(value, 0) - 8;
						byte[] bytes = array.Skip(49).Take(count).ToArray<byte>();
						string text = Encoding.UTF8.GetString(bytes);
						text = MultipleMaids.StringFromBase64Comp(text);
						if (text != "")
						{
							string[] array2 = text.Split(new char[]
							{
								'_'
							});
							if (array2.Length >= 2)
							{
								string[] array3 = array2[0].Split(new char[]
								{
									','
								});
								this.date[i] = array3[0];
								this.ninzu[i] = array3[1] + "人";
							}
						}
					}
					else
					{
						IniKey iniKey = base.Preferences["scene"]["s" + (this.page * 10 + i + 1)];
						if (iniKey.Value != null && iniKey.Value.ToString() != "")
						{
							string[] array2 = iniKey.Value.Split(new char[]
							{
								'_'
							});
							if (array2.Length >= 2)
							{
								string[] array3 = array2[0].Split(new char[]
								{
									','
								});
								this.date[i] = array3[0];
								this.ninzu[i] = array3[1] + "人";
							}
						}
					}
					//IL_6B3:
					i++;
					continue;
					//goto IL_6B3;
				}
			}
			GUI.Label(new Rect((float)this.GetPix(60), (float)this.GetPix(32), (float)this.GetPix(100), (float)this.GetPix(25)), this.page * 10 + 1 + " ～ " + (this.page * 10 + 10), guistyle);
			if (this.saveScene2 > 0 && string.IsNullOrEmpty(this.thum_byte_to_base64_) && File.Exists(this.thum_file_path_))
			{
				try
				{
					Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
					texture2D.LoadImage(File.ReadAllBytes(this.thum_file_path_));
					float num2 = (float)texture2D.width / (float)texture2D.height;
					Vector2 vector = new Vector2(960f, 540f);
					int num3 = texture2D.width;
					int num4 = texture2D.height;
					if (vector.x < (float)texture2D.width && vector.y < (float)texture2D.height)
					{
						num3 = (int)vector.x;
						num4 = Mathf.RoundToInt((float)num3 / num2);
						if (vector.y < (float)num4)
						{
							num4 = (int)vector.y;
							num3 = Mathf.RoundToInt((float)num4 * num2);
						}
					}
					else if (vector.x < (float)texture2D.width)
					{
						num3 = (int)vector.x;
						num4 = Mathf.RoundToInt((float)num3 / num2);
					}
					else if (vector.y < (float)texture2D.height)
					{
						num4 = (int)vector.y;
						num3 = Mathf.RoundToInt((float)num4 * num2);
					}
					TextureScale.Bilinear(texture2D, num3, num4);
					byte[] array = texture2D.EncodeToPNG();
					string text2 = "tEXt";
					string text3 = "Comment";
					string text4 = this.saveData;
					text4 = MultipleMaids.Base64FromStringComp(text4);
					List<byte> list = new List<byte>();
					int num5 = 4;
					int length = text2.Length;
					int length2 = text3.Length;
					int num6 = 1;
					int length3 = text4.Length;
					int num7 = 4;
					int num8 = num5 + length + length2 + num6 + length3 + num7;
					int num9 = length2 + num6 + length3;
					byte[] collection = new byte[]
					{
						(byte)(num9 >> 24),
						(byte)(num9 >> 16),
						(byte)(num9 >> 8),
						(byte)num9
					};
					byte[] bytes2 = Encoding.UTF8.GetBytes(text2);
					byte[] bytes3 = Encoding.UTF8.GetBytes(text3);
					list.Add(0);
					byte[] bytes4 = Encoding.UTF8.GetBytes(text4);
					List<byte> list2 = new List<byte>(1000000);
					list2.AddRange(bytes2);
					list2.AddRange(bytes3);
					list2.Add(0);
					list2.AddRange(bytes4);
					uint num10 = new CRC32().Calc(list2.ToArray());
					byte[] collection2 = new byte[]
					{
						(byte)(num10 >> 24),
						(byte)(num10 >> 16),
						(byte)(num10 >> 8),
						(byte)num10
					};
					List<byte> list3 = new List<byte>();
					for (int i = 0; i < 33; i++)
					{
						list3.Add(array[i]);
					}
					List<byte> list4 = new List<byte>();
					for (int i = 33; i < array.Length; i++)
					{
						list4.Add(array[i]);
					}
					List<byte> list5 = new List<byte>(1000000);
					list5.AddRange(list3);
					list5.AddRange(collection);
					list5.AddRange(bytes2);
					list5.AddRange(bytes3);
					list5.Add(0);
					list5.AddRange(bytes4);
					list5.AddRange(collection2);
					list5.AddRange(list4);
					array = list5.ToArray();
					string path = string.Concat(new object[]
					{
						Path.GetFullPath(".\\"),
						"Mod\\MultipleMaidsScene\\",
						this.saveScene2,
						".png"
					});
					File.WriteAllBytes(path, array);
					this.thum_file_path_ = "";
					this.saveScene2 = 0;
				}
				catch
				{
				}
			}
			for (int i = 0; i < 10; i++)
			{
				GUI.Label(new Rect((float)this.GetPix(5), (float)this.GetPix(60 + num * i), (float)this.GetPix(25), (float)this.GetPix(25)), string.Concat(this.page * 10 + i + 1), guistyle);
				if (GUI.Button(new Rect((float)this.GetPix(20), (float)this.GetPix(78 + num * i), (float)this.GetPix(50), (float)this.GetPix(20)), "保存", guistyle2))
				{
					this.saveScene = this.page * 10 + i + 1;
					this.saveScene2 = this.saveScene;
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					this.isScreen2 = true;
					if (!this.isMessage)
					{
						this.ui_cam_hide_list_.Clear();
						UICamera[] array4 = NGUITools.FindActive<UICamera>();
						foreach (UICamera uicamera in array4)
						{
							if (uicamera.GetComponent<Camera>().enabled)
							{
								uicamera.GetComponent<Camera>().enabled = false;
								this.ui_cam_hide_list_.Add(uicamera);
							}
						}
					}
					else
					{
						this.editUI = GameObject.Find("/UI Root/Camera");
						if (this.editUI != null)
						{
							this.editUI.SetActive(false);
						}
						if (GameMain.Instance.CMSystem.ViewFps)
						{
							GameObject childObject = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/FpsCounter", false);
							childObject.SetActive(false);
						}
						GameObject childObject2 = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/SystemDialog", false);
						GameObject childObject3 = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/SystemShortcut", false);
						childObject2.SetActive(false);
						childObject3.SetActive(false);
					}
					this.isGui = this.bGui;
					this.bGui = false;
					try
					{
						this.thum_byte_to_base64_ = string.Empty;
						this.thum_file_path_ = Path.Combine(Path.GetTempPath(), "cm3d2_" + Guid.NewGuid().ToString() + ".png");
						GameMain.Instance.MainCamera.ScreenShot(this.thum_file_path_, 1, true);
					}
					catch
					{
					}
				}
				GUI.Label(new Rect((float)this.GetPix(25), (float)this.GetPix(60 + num * i), (float)this.GetPix(100), (float)this.GetPix(25)), this.date[i], guistyle);
				GUI.Label(new Rect((float)this.GetPix(130), (float)this.GetPix(60 + num * i), (float)this.GetPix(100), (float)this.GetPix(25)), this.ninzu[i], guistyle);
				if (this.date[i] != "未保存")
				{
					if (GUI.Button(new Rect((float)this.GetPix(100), (float)this.GetPix(78 + num * i), (float)this.GetPix(50), (float)this.GetPix(20)), "読込", guistyle2))
					{
						this.loadScene = this.page * 10 + i + 1;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
				}
			}
		}

		private void GuiFunc7(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "label";
			guistyle2.fontSize = this.GetPix(11);
			guistyle2.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle3 = "button";
			guistyle3.fontSize = this.GetPix(20);
			GUIStyle guistyle4 = "button";
			guistyle4.fontSize = this.GetPix(14);
			guistyle4.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle5 = "button";
			guistyle5.fontSize = this.GetPix(12);
			guistyle5.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle6 = new GUIStyle("toggle");
			guistyle6.fontSize = this.GetPix(13);
			CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
			Maid maid = this.maidArray[this.selectMaidIndex];
			if (!this.kankyo2InitFlg)
			{
				this.listStyle2.normal.textColor = Color.white;
				this.listStyle2.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle2.onHover.background = (this.listStyle2.hover.background = new Texture2D(2, 2));
				RectOffset padding = this.listStyle2.padding;
				RectOffset padding2 = this.listStyle2.padding;
				RectOffset padding3 = this.listStyle2.padding;
				int num = this.listStyle2.padding.bottom = this.GetPix(0);
				num = (padding3.top = num);
				num = (padding2.right = num);
				padding.left = num;
				this.listStyle2.fontSize = this.GetPix(11);
				this.listStyle3.normal.textColor = Color.white;
				this.listStyle3.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle3.onHover.background = (this.listStyle3.hover.background = new Texture2D(2, 2));
				RectOffset padding4 = this.listStyle3.padding;
				RectOffset padding5 = this.listStyle3.padding;
				num = (this.listStyle3.padding.top = this.GetPix(0));
				num = (padding5.right = num);
				padding4.left = num;
				this.listStyle3.padding.bottom = this.GetPix(0);
				this.listStyle3.fontSize = this.GetPix(14);
				this.bgCombo2.selectedItemIndex = this.bgIndexB;
				this.bgCombo2List = new GUIContent[this.bgArray.Length];
				int i = 0;
				while (i < this.bgArray.Length)
				{
					string text = this.bgArray[i];
					if (text == null)
					{
						goto IL_1662;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-1 == null)
					if (bgUiArray == null)
					{
						bgUiArray = new Dictionary<string, int>(120)
						{
							{
								"Salon",
								0
							},
							{
								"Syosai",
								1
							},
							{
								"Syosai_Night",
								2
							},
							{
								"DressRoom_NoMirror",
								3
							},
							{
								"MyBedRoom",
								4
							},
							{
								"MyBedRoom_Night",
								5
							},
							{
								"MyBedRoom_NightOff",
								6
							},
							{
								"Bathroom",
								7
							},
							{
								"PlayRoom",
								8
							},
							{
								"Pool",
								9
							},
							{
								"SMRoom",
								10
							},
							{
								"PlayRoom2",
								11
							},
							{
								"Salon_Garden",
								12
							},
							{
								"LargeBathRoom",
								13
							},
							{
								"MaidRoom",
								14
							},
							{
								"OiranRoom",
								15
							},
							{
								"Penthouse",
								16
							},
							{
								"Town",
								17
							},
							{
								"Kitchen",
								18
							},
							{
								"Kitchen_Night",
								19
							},
							{
								"Shitsumu",
								20
							},
							{
								"Shitsumu_Night",
								21
							},
							{
								"Salon_Entrance",
								22
							},
							{
								"Bar",
								23
							},
							{
								"Toilet",
								24
							},
							{
								"Train",
								25
							},
							{
								"SMRoom2",
								26
							},
							{
								"LockerRoom",
								27
							},
							{
								"Oheya",
								28
							},
							{
								"Salon_Day",
								29
							},
							{
								"ClassRoom",
								30
							},
							{
								"ClassRoom_Play",
								31
							},
							{
								"HoneymoonRoom",
								32
							},
							{
								"OutletPark",
								33
							},
							{
								"BigSight",
								34
							},
							{
								"BigSight_Night",
								35
							},
							{
								"PrivateRoom",
								36
							},
							{
								"PrivateRoom_Night",
								37
							},
							{
								"Sea",
								38
							},
							{
								"Sea_Night",
								39
							},
							{
								"Yashiki",
								40
							},
							{
								"Yashiki_Day",
								41
							},
							{
								"Yashiki_Pillow",
								42
							},
							{
								"rotenburo",
								43
							},
							{
								"rotenburo_night",
								44
							},
							{
								"villa",
								45
							},
							{
								"villa_night",
								46
							},
							{
								"villa_bedroom",
								47
							},
							{
								"villa_bedroom_night",
								48
							},
							{
								"villa_farm",
								49
							},
							{
								"villa_farm_night",
								50
							},
							{
								"karaokeroom",
								51
							},
							{
								"Theater",
								52
							},
							{
								"Theater_LightOff",
								53
							},
							{
								"LiveStage",
								54
							},
							{
								"LiveStage_Side",
								55
							},
							{
								"LiveStage_use_dance",
								56
							},
							{
								"BackStage",
								57
							},
							{
								"DanceRoom",
								58
							},
							{
								"EmpireClub_Rotary",
								59
							},
							{
								"EmpireClub_Rotary_Night",
								60
							},
							{
								"EmpireClub_Entrance",
								61
							},
							{
								"ShinShitsumu",
								62
							},
							{
								"ShinShitsumu_ChairRot",
								63
							},
							{
								"ShinShitsumu_Night",
								64
							},
							{
								"MyRoom",
								65
							},
							{
								"MyRoom_Night",
								66
							},
							{
								"OpemCafe",
								67
							},
							{
								"OpemCafe_Night",
								68
							},
							{
								"Restaurant",
								69
							},
							{
								"Restaurant_Night",
								70
							},
							{
								"MainKitchen",
								71
							},
							{
								"MainKitchen_Night",
								72
							},
							{
								"MainKitchen_LightOff",
								73
							},
							{
								"BarLounge",
								74
							},
							{
								"Casino",
								75
							},
							{
								"CasinoMini",
								76
							},
							{
								"SMClub",
								77
							},
							{
								"Soap",
								78
							},
							{
								"Spa",
								79
							},
							{
								"Spa_Night",
								80
							},
							{
								"ShoppingMall",
								81
							},
							{
								"ShoppingMall_Night",
								82
							},
							{
								"GameShop",
								83
							},
							{
								"MusicShop",
								84
							},
							{
								"HeroineRoom_A1",
								85
							},
							{
								"HeroineRoom_A1_Night",
								86
							},
							{
								"HeroineRoom_B1",
								87
							},
							{
								"HeroineRoom_B1_Night",
								88
							},
							{
								"HeroineRoom_C1",
								89
							},
							{
								"HeroineRoom_C1_Night",
								90
							},
							{
								"HeroineRoom_A",
								91
							},
							{
								"HeroineRoom_A_Night",
								92
							},
							{
								"HeroineRoom_B",
								93
							},
							{
								"HeroineRoom_B_Night",
								94
							},
							{
								"HeroineRoom_C",
								95
							},
							{
								"HeroineRoom_C_Night",
								96
							},
							{
								"Shukuhakubeya_BedRoom",
								97
							},
							{
								"Shukuhakubeya_BedRoom_Night",
								98
							},
							{
								"Shukuhakubeya_Other_BedRoom",
								99
							},
							{
								"Shukuhakubeya_Living",
								100
							},
							{
								"Shukuhakubeya_Living_Night",
								101
							},
							{
								"Shukuhakubeya_Toilet",
								102
							},
							{
								"Shukuhakubeya_Toilet_Night",
								103
							},
							{
								"Shukuhakubeya_WashRoom",
								104
							},
							{
								"Shukuhakubeya_WashRoom_Night",
								105
							},
							{
								"opemcafe_rance10",
								106
							},
							{
								"opemcafe_rance10_night",
								107
							},
							{
								"opemcafe_riddlejoker",
								108
							},
							{
								"opemcafe_riddlejoker_night",
								109
							},
							{
								"opemcafe_wanko",
								110
							},
							{
								"opemcafe_wanko_night",
								111
							},
							{
								"opemcafe_raspberry",
								112
							},
							{
								"opemcafe_raspberry_night",
								113
							},
							{
								"seacafe",
								114
							},
							{
								"seacafe_night",
								115
							},
							{
								"com3d2pool",
								116
							},
							{
								"com3d2pool_night",
								117
							},
							{
								"shrine",
								118
							},
							{
								"shrine_night",
								119
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-1.TryGetValue(text, out num))
					//{
					//	goto IL_1662;
					//}
					if (!bgUiArray.TryGetValue(text, out num))
					{
						goto IL_1662;
					}
					switch (num)
					{
						case 0:
							this.bgCombo2List[i] = new GUIContent("サロン");
							break;
						case 1:
							this.bgCombo2List[i] = new GUIContent("書斎");
							break;
						case 2:
							this.bgCombo2List[i] = new GUIContent("書斎(夜)");
							break;
						case 3:
							this.bgCombo2List[i] = new GUIContent("ドレスルーム");
							break;
						case 4:
							this.bgCombo2List[i] = new GUIContent("自室");
							break;
						case 5:
							this.bgCombo2List[i] = new GUIContent("自室(夜)");
							break;
						case 6:
							this.bgCombo2List[i] = new GUIContent("自室(消灯)");
							break;
						case 7:
							this.bgCombo2List[i] = new GUIContent("風呂");
							break;
						case 8:
							this.bgCombo2List[i] = new GUIContent("プレイルーム");
							break;
						case 9:
							this.bgCombo2List[i] = new GUIContent("プール");
							break;
						case 10:
							this.bgCombo2List[i] = new GUIContent("SMルーム");
							break;
						case 11:
							this.bgCombo2List[i] = new GUIContent("プレイルーム2");
							break;
						case 12:
							this.bgCombo2List[i] = new GUIContent("サロン(中庭)");
							break;
						case 13:
							this.bgCombo2List[i] = new GUIContent("大浴場");
							break;
						case 14:
							this.bgCombo2List[i] = new GUIContent("メイド部屋");
							break;
						case 15:
							this.bgCombo2List[i] = new GUIContent("花魁ルーム");
							break;
						case 16:
							this.bgCombo2List[i] = new GUIContent("ペントハウス");
							break;
						case 17:
							this.bgCombo2List[i] = new GUIContent("街");
							break;
						case 18:
							this.bgCombo2List[i] = new GUIContent("キッチン");
							break;
						case 19:
							this.bgCombo2List[i] = new GUIContent("キッチン(夜)");
							break;
						case 20:
							this.bgCombo2List[i] = new GUIContent("執務室");
							break;
						case 21:
							this.bgCombo2List[i] = new GUIContent("執務室(夜)");
							break;
						case 22:
							this.bgCombo2List[i] = new GUIContent("エントランス");
							break;
						case 23:
							this.bgCombo2List[i] = new GUIContent("バー");
							break;
						case 24:
							this.bgCombo2List[i] = new GUIContent("トイレ");
							break;
						case 25:
							this.bgCombo2List[i] = new GUIContent("電車");
							break;
						case 26:
							this.bgCombo2List[i] = new GUIContent("地下室");
							break;
						case 27:
							this.bgCombo2List[i] = new GUIContent("ロッカールーム");
							break;
						case 28:
							this.bgCombo2List[i] = new GUIContent("四畳半部屋");
							break;
						case 29:
							this.bgCombo2List[i] = new GUIContent("サロン(昼)");
							break;
						case 30:
							this.bgCombo2List[i] = new GUIContent("教室");
							break;
						case 31:
							this.bgCombo2List[i] = new GUIContent("教室(夜伽)");
							break;
						case 32:
							this.bgCombo2List[i] = new GUIContent("ハネムーンルーム");
							break;
						case 33:
							this.bgCombo2List[i] = new GUIContent("アウトレットパーク");
							break;
						case 34:
							this.bgCombo2List[i] = new GUIContent("ビッグサイト");
							break;
						case 35:
							this.bgCombo2List[i] = new GUIContent("ビッグサイト(夜)");
							break;
						case 36:
							this.bgCombo2List[i] = new GUIContent("プライベートルーム");
							break;
						case 37:
							this.bgCombo2List[i] = new GUIContent("プライベートルーム(夜)");
							break;
						case 38:
							this.bgCombo2List[i] = new GUIContent("海");
							break;
						case 39:
							this.bgCombo2List[i] = new GUIContent("海(夜)");
							break;
						case 40:
							this.bgCombo2List[i] = new GUIContent("屋敷(夜)");
							break;
						case 41:
							this.bgCombo2List[i] = new GUIContent("屋敷");
							break;
						case 42:
							this.bgCombo2List[i] = new GUIContent("屋敷(夜・枕)");
							break;
						case 43:
							this.bgCombo2List[i] = new GUIContent("露天風呂");
							break;
						case 44:
							this.bgCombo2List[i] = new GUIContent("露天風呂(夜)");
							break;
						case 45:
							this.bgCombo2List[i] = new GUIContent("ヴィラ1F");
							break;
						case 46:
							this.bgCombo2List[i] = new GUIContent("ヴィラ1F(夜)");
							break;
						case 47:
							this.bgCombo2List[i] = new GUIContent("ヴィラ2F");
							break;
						case 48:
							this.bgCombo2List[i] = new GUIContent("ヴィラ2F(夜)");
							break;
						case 49:
							this.bgCombo2List[i] = new GUIContent("畑");
							break;
						case 50:
							this.bgCombo2List[i] = new GUIContent("畑(夜)");
							break;
						case 51:
							this.bgCombo2List[i] = new GUIContent("カラオケルーム");
							break;
						case 52:
							this.bgCombo2List[i] = new GUIContent("劇場");
							break;
						case 53:
							this.bgCombo2List[i] = new GUIContent("劇場(夜)");
							break;
						case 54:
							this.bgCombo2List[i] = new GUIContent("ステージ");
							break;
						case 55:
							this.bgCombo2List[i] = new GUIContent("ステージ(ライト)");
							break;
						case 56:
							this.bgCombo2List[i] = new GUIContent("ステージ(オフ)");
							break;
						case 57:
							this.bgCombo2List[i] = new GUIContent("ステージ裏");
							break;
						case 58:
							this.bgCombo2List[i] = new GUIContent("トレーニングルーム");
							break;
						case 59:
							this.bgCombo2List[i] = new GUIContent("ロータリー");
							break;
						case 60:
							this.bgCombo2List[i] = new GUIContent("ロータリー(夜)");
							break;
						case 61:
							this.bgCombo2List[i] = new GUIContent("エントランス");
							break;
						case 62:
							this.bgCombo2List[i] = new GUIContent("執務室");
							break;
						case 63:
							this.bgCombo2List[i] = new GUIContent("執務室(椅子)");
							break;
						case 64:
							this.bgCombo2List[i] = new GUIContent("執務室(夜)");
							break;
						case 65:
							this.bgCombo2List[i] = new GUIContent("主人公部屋");
							break;
						case 66:
							this.bgCombo2List[i] = new GUIContent("主人公部屋(夜)");
							break;
						case 67:
							this.bgCombo2List[i] = new GUIContent("カフェ");
							break;
						case 68:
							this.bgCombo2List[i] = new GUIContent("カフェ(夜)");
							break;
						case 69:
							this.bgCombo2List[i] = new GUIContent("レストラン");
							break;
						case 70:
							this.bgCombo2List[i] = new GUIContent("レストラン(夜)");
							break;
						case 71:
							this.bgCombo2List[i] = new GUIContent("キッチン");
							break;
						case 72:
							this.bgCombo2List[i] = new GUIContent("キッチン(夜)");
							break;
						case 73:
							this.bgCombo2List[i] = new GUIContent("キッチン(オフ)");
							break;
						case 74:
							this.bgCombo2List[i] = new GUIContent("バー");
							break;
						case 75:
							this.bgCombo2List[i] = new GUIContent("カジノ");
							break;
						case 76:
							this.bgCombo2List[i] = new GUIContent("カジノミニ");
							break;
						case 77:
							this.bgCombo2List[i] = new GUIContent("SMクラブ");
							break;
						case 78:
							this.bgCombo2List[i] = new GUIContent("ソープ");
							break;
						case 79:
							this.bgCombo2List[i] = new GUIContent("スパ");
							break;
						case 80:
							this.bgCombo2List[i] = new GUIContent("スパ(夜)");
							break;
						case 81:
							this.bgCombo2List[i] = new GUIContent("ショッピングモール");
							break;
						case 82:
							this.bgCombo2List[i] = new GUIContent("ショッピングモール(夜)");
							break;
						case 83:
							this.bgCombo2List[i] = new GUIContent("ゲームショップ");
							break;
						case 84:
							this.bgCombo2List[i] = new GUIContent("ミュージックショップ");
							break;
						case 85:
							this.bgCombo2List[i] = new GUIContent("無垢部屋");
							break;
						case 86:
							this.bgCombo2List[i] = new GUIContent("無垢部屋(夜)");
							break;
						case 87:
							this.bgCombo2List[i] = new GUIContent("真面目部屋");
							break;
						case 88:
							this.bgCombo2List[i] = new GUIContent("真面目部屋(夜)");
							break;
						case 89:
							this.bgCombo2List[i] = new GUIContent("凜デレ部屋");
							break;
						case 90:
							this.bgCombo2List[i] = new GUIContent("凜デレ部屋(夜)");
							break;
						case 91:
							this.bgCombo2List[i] = new GUIContent("ツンデレ部屋");
							break;
						case 92:
							this.bgCombo2List[i] = new GUIContent("ツンデレ部屋(夜)");
							break;
						case 93:
							this.bgCombo2List[i] = new GUIContent("クーデレ部屋");
							break;
						case 94:
							this.bgCombo2List[i] = new GUIContent("クーデレ部屋(夜)");
							break;
						case 95:
							this.bgCombo2List[i] = new GUIContent("純真部屋");
							break;
						case 96:
							this.bgCombo2List[i] = new GUIContent("純真部屋(夜)");
							break;
						case 97:
							this.bgCombo2List[i] = new GUIContent("宿泊-ベッドルーム");
							break;
						case 98:
							this.bgCombo2List[i] = new GUIContent("宿泊-ベッドルーム(夜)");
							break;
						case 99:
							this.bgCombo2List[i] = new GUIContent("宿泊-他ベッドルーム(夜)");
							break;
						case 100:
							this.bgCombo2List[i] = new GUIContent("宿泊-リビング");
							break;
						case 101:
							this.bgCombo2List[i] = new GUIContent("宿泊-リビング(夜)");
							break;
						case 102:
							this.bgCombo2List[i] = new GUIContent("宿泊-トイレ");
							break;
						case 103:
							this.bgCombo2List[i] = new GUIContent("宿泊-トイレ(夜)");
							break;
						case 104:
							this.bgCombo2List[i] = new GUIContent("宿泊-洗面所");
							break;
						case 105:
							this.bgCombo2List[i] = new GUIContent("宿泊-洗面所(夜)");
							break;
						case 106:
							this.bgCombo2List[i] = new GUIContent("ランス10");
							break;
						case 107:
							this.bgCombo2List[i] = new GUIContent("ランス10");
							break;
						case 108:
							this.bgCombo2List[i] = new GUIContent("リドル");
							break;
						case 109:
							this.bgCombo2List[i] = new GUIContent("リドル");
							break;
						case 110:
							this.bgCombo2List[i] = new GUIContent("わんこ");
							break;
						case 111:
							this.bgCombo2List[i] = new GUIContent("わんこ");
							break;
						case 112:
							this.bgCombo2List[i] = new GUIContent("ラズベリー");
							break;
						case 113:
							this.bgCombo2List[i] = new GUIContent("ラズベリー");
							break;
						case 114:
							this.bgCombo2List[i] = new GUIContent("シーカフェ");
							break;
						case 115:
							this.bgCombo2List[i] = new GUIContent("シーカフェ");
							break;
						case 116:
							this.bgCombo2List[i] = new GUIContent("プール");
							break;
						case 117:
							this.bgCombo2List[i] = new GUIContent("プール");
							break;
						case 118:
							this.bgCombo2List[i] = new GUIContent("神社");
							break;
						case 119:
							this.bgCombo2List[i] = new GUIContent("神社");
							break;
						default:
							goto IL_1662;
					}
				IL_16E1:
					Dictionary<string, string> saveDataDic = CreativeRoomManager.GetSaveDataDic();
					if (saveDataDic != null)
					{
						foreach (KeyValuePair<string, string> keyValuePair in saveDataDic)
						{
							if (this.bgArray[i] == keyValuePair.Key)
							{
								this.bgCombo2List[i] = new GUIContent(keyValuePair.Value);
							}
						}
					}
					i++;
					continue;
				IL_1662:
					string text2 = this.bgArray[i];
					for (int j = 0; j < this.bgNameList.Count; j++)
					{
						string[] array = this.bgNameList[j].Split(new char[]
						{
							','
						});
						if (text2 == array[0])
						{
							text2 = array[1];
						}
					}
					this.bgCombo2List[i] = new GUIContent(text2);
					goto IL_16E1;
				}
				this.slotCombo.selectedItemIndex = 0;
				this.slotComboList = new GUIContent[this.slotArray.Length];
				i = 0;
				while (i < this.slotArray.Length)
				{
					string text = this.slotArray[i];
					if (text == null)
					{
						goto IL_1C03;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-2 == null)
					if (PartsUIArray == null)
					{
						//<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-2 = new Dictionary<string, int>(26)
						PartsUIArray = new Dictionary<string, int>(26)
						{
							{
								"acchat",
								0
							},
							{
								"headset",
								1
							},
							{
								"wear",
								2
							},
							{
								"skirt",
								3
							},
							{
								"onepiece",
								4
							},
							{
								"mizugi",
								5
							},
							{
								"bra",
								6
							},
							{
								"panz",
								7
							},
							{
								"stkg",
								8
							},
							{
								"shoes",
								9
							},
							{
								"acckami",
								10
							},
							{
								"megane",
								11
							},
							{
								"acchead",
								12
							},
							{
								"acchana",
								13
							},
							{
								"accmimi",
								14
							},
							{
								"glove",
								15
							},
							{
								"acckubi",
								16
							},
							{
								"acckubiwa",
								17
							},
							{
								"acckamisub",
								18
							},
							{
								"accnip",
								19
							},
							{
								"accude",
								20
							},
							{
								"accheso",
								21
							},
							{
								"accashi",
								22
							},
							{
								"accsenaka",
								23
							},
							{
								"accshippo",
								24
							},
							{
								"accxxx",
								25
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-2.TryGetValue(text, out num))
					//{
					//	goto IL_1C03;
					//}
					if (!PartsUIArray.TryGetValue(text, out num))
					{
						goto IL_1C03;
					}
					switch (num)
					{
						case 0:
							this.slotComboList[i] = new GUIContent("帽子");
							break;
						case 1:
							this.slotComboList[i] = new GUIContent("ヘッドドレス");
							break;
						case 2:
							this.slotComboList[i] = new GUIContent("トップス");
							break;
						case 3:
							this.slotComboList[i] = new GUIContent("ボトムス");
							break;
						case 4:
							this.slotComboList[i] = new GUIContent("ワンピース");
							break;
						case 5:
							this.slotComboList[i] = new GUIContent("水着");
							break;
						case 6:
							this.slotComboList[i] = new GUIContent("ブラジャー");
							break;
						case 7:
							this.slotComboList[i] = new GUIContent("パンツ");
							break;
						case 8:
							this.slotComboList[i] = new GUIContent("靴下");
							break;
						case 9:
							this.slotComboList[i] = new GUIContent("靴");
							break;
						case 10:
							this.slotComboList[i] = new GUIContent("前髪");
							break;
						case 11:
							this.slotComboList[i] = new GUIContent("メガネ");
							break;
						case 12:
							this.slotComboList[i] = new GUIContent("アイマスク");
							break;
						case 13:
							this.slotComboList[i] = new GUIContent("鼻");
							break;
						case 14:
							this.slotComboList[i] = new GUIContent("耳");
							break;
						case 15:
							this.slotComboList[i] = new GUIContent("手袋");
							break;
						case 16:
							this.slotComboList[i] = new GUIContent("ネックレス");
							break;
						case 17:
							this.slotComboList[i] = new GUIContent("チョーカー");
							break;
						case 18:
							this.slotComboList[i] = new GUIContent("リボン");
							break;
						case 19:
							this.slotComboList[i] = new GUIContent("乳首");
							break;
						case 20:
							this.slotComboList[i] = new GUIContent("腕");
							break;
						case 21:
							this.slotComboList[i] = new GUIContent("へそ");
							break;
						case 22:
							this.slotComboList[i] = new GUIContent("足首");
							break;
						case 23:
							this.slotComboList[i] = new GUIContent("背中");
							break;
						case 24:
							this.slotComboList[i] = new GUIContent("しっぽ");
							break;
						case 25:
							this.slotComboList[i] = new GUIContent("前穴");
							break;
						default:
							goto IL_1C03;
					}
				IL_1C1C:
					i++;
					continue;
				IL_1C03:
					this.slotComboList[i] = new GUIContent(this.slotArray[i]);
					goto IL_1C1C;
				}
				this.myCombo.selectedItemIndex = 0;
				this.myComboList = new GUIContent[this.myArray.Length];
				List<int> categoryIDList = PlacementData.CategoryIDList;
				this.myComboList[0] = new GUIContent("");
				for (i = 1; i < this.myArray.Length; i++)
				{
					this.myComboList[i] = new GUIContent(PlacementData.GetCategoryName(categoryIDList[i - 1]));
				}
				this.itemCombo2.selectedItemIndex = 0;
				this.itemCombo2List = new GUIContent[this.itemBArray.Length];
				i = 0;
				while (i < this.itemBArray.Length)
				{
					string text = this.itemBArray[i];
					if (text == null)
					{
						goto IL_2E56;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-3 == null)
					if (ItemUIArray == null)
					{
						//<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-3 = new Dictionary<string, int>(108)
						ItemUIArray = new Dictionary<string, int>(108)
						{
							{
								"handitem,HandItemR_WineGlass_I_.menu",
								0
							},
							{
								"handitem,HandItemR_WineBottle_I_.menu",
								1
							},
							{
								"handitem,handitemr_racket_I_.menu",
								2
							},
							{
								"handitem,HandItemR_Hataki_I_.menu",
								3
							},
							{
								"handitem,HandItemR_Mop_I_.menu",
								4
							},
							{
								"handitem,HandItemR_Houki_I_.menu",
								5
							},
							{
								"handitem,HandItemR_Zoukin2_I_.menu",
								6
							},
							{
								"handitem,HandItemR_Chu-B_Lip_I_.menu",
								7
							},
							{
								"handitem,HandItemR_Mimikaki_I_.menu",
								8
							},
							{
								"handitem,HandItemR_Pen_I_.menu",
								9
							},
							{
								"handitem,HandItemR_Otama_I_.menu",
								10
							},
							{
								"handitem,HandItemR_Houchou_I_.menu",
								11
							},
							{
								"handitem,HandItemR_Book_I_.menu",
								12
							},
							{
								"handitem,HandItemR_Puff_I_.menu",
								13
							},
							{
								"handitem,HandItemR_Rip_I_.menu",
								14
							},
							{
								"handitem,HandItemL_Shisyuu_I_.menu",
								15
							},
							{
								"handitem,HandItemR_Hari_I_.menu",
								16
							},
							{
								"handitem,HandItemL_Sara_I_.menu",
								17
							},
							{
								"handitem,HandItemR_Sponge_I_.menu",
								18
							},
							{
								"kousoku_upper,KousokuU_TekaseOne_I_.menu",
								19
							},
							{
								"kousoku_upper,KousokuU_TekaseTwo_I_.menu",
								20
							},
							{
								"kousoku_lower,KousokuL_BathTowel_I_.menu",
								21
							},
							{
								"kousoku_upper,KousokuU_Ushirode_I_.menu",
								22
							},
							{
								"kousoku_upper,KousokuU_SMRoom_Haritsuke_I_.menu",
								23
							},
							{
								"kousoku_upper,KousokuU_SMRoom2_Haritsuke_I_.menu",
								24
							},
							{
								"handitem,HandItemL_Dance_Hataki_I_.menu",
								25
							},
							{
								"handitem,HandItemL_Dance_Mop_I_.menu",
								26
							},
							{
								"handitem,HandItemL_Dance_Zoukin_I_.menu",
								27
							},
							{
								"handitem,HandItemL_Kozara_I_.menu",
								28
							},
							{
								"handitem,HandItemR_Teacup_I_.menu",
								29
							},
							{
								"handitem,HandItemL_Teasaucer_I_.menu",
								30
							},
							{
								"handitem,HandItemR_Wholecake_I_.menu",
								31
							},
							{
								"handitem,HandItemR_Menu_I_.menu",
								32
							},
							{
								"handitem,HandItemR_Vibe_I_.menu",
								33
							},
							{
								"handitem,HandItemR_VibePink_I_.menu",
								34
							},
							{
								"handitem,HandItemR_VibeBig_I_.menu",
								35
							},
							{
								"handitem,HandItemR_AnalVibe_I_.menu",
								36
							},
							{
								"handitem,HandItemH_SoutouVibe_I_.menu",
								37
							},
							{
								"accvag,accVag_Vibe_I_.menu",
								38
							},
							{
								"accvag,accVag_VibeBig_I_.menu",
								39
							},
							{
								"accvag,accVag_VibePink_I_.menu",
								40
							},
							{
								"accanl,accAnl_AnalVibe_I_.menu",
								41
							},
							{
								"accanl,accAnl_Photo_NomalVibe_I_.menu",
								42
							},
							{
								"accanl,accAnl_Photo_VibeBig_I_.menu",
								43
							},
							{
								"accanl,accAnl_Photo_VibePink_I_.menu",
								44
							},
							{
								"handitem,HandItemR_Curry_I_.menu",
								45
							},
							{
								"handitem,HandItemL_Karaoke_Mike_I_.menu",
								46
							},
							{
								"handitem,HandItemR_Pasta_I_.menu",
								47
							},
							{
								"handitem,HandItemR_Omurice1_I_.menu",
								48
							},
							{
								"handitem,HandItemR_Omurice2_I_.menu",
								49
							},
							{
								"handitem,HandItemR_Omurice3_I_.menu",
								50
							},
							{
								"handitem,HandItemR_BeerBottle(cap_on)_I_.menu",
								51
							},
							{
								"handitem,HandItemR_BeerBottle(cap_off)_I_.menu",
								52
							},
							{
								"handitem,HandItemR_BeerGlass_I_.menu",
								53
							},
							{
								"handitem,HandItemR_Crops_Suika_I_.menu",
								54
							},
							{
								"handitem,HandItemR_Diary_I_.menu",
								55
							},
							{
								"handitem,HandItemR_DVD1_I_.menu",
								56
							},
							{
								"handitem,HandItemR_DVD2_I_.menu",
								57
							},
							{
								"handitem,HandItemR_DVD3_I_.menu",
								58
							},
							{
								"handitem,HandItemR_DVD4_I_.menu",
								59
							},
							{
								"handitem,HandItemR_DVD5_I_.menu",
								60
							},
							{
								"handitem,HandItemR_Folk_I_.menu",
								61
							},
							{
								"handitem,HandItemR_Hanabi_I_.menu",
								62
							},
							{
								"handitem,HandItemR_Jyouro_I_.menu",
								63
							},
							{
								"handitem,HandItemR_Kobin_I_.menu",
								64
							},
							{
								"handitem,HandItemR_Kushiyaki_I_.menu",
								65
							},
							{
								"handitem,HandItemR_MilkBottle(cap_on)_I_.menu",
								66
							},
							{
								"handitem,HandItemR_MilkBottle(cap_off)_I_.menu",
								67
							},
							{
								"handitem,HandItemR_Mugcup_I_.menu",
								68
							},
							{
								"handitem,HandItemR_Natumikan_I_.menu",
								69
							},
							{
								"handitem,HandItemR_Ninjin_I_.menu",
								70
							},
							{
								"handitem,HandItemR_Ochoko_I_.menu",
								71
							},
							{
								"handitem,HandItemR_Satumaimo_I_.menu",
								72
							},
							{
								"handitem,HandItemR_Scoop_I_.menu",
								73
							},
							{
								"handitem,HandItemR_Senkouhanabi_I_.menu",
								74
							},
							{
								"handitem,HandItemR_Shell_I_.menu",
								75
							},
							{
								"handitem,HandItemR_Shihen_I_.menu",
								76
							},
							{
								"handitem,HandItemR_Spoon_Curry_I_.menu",
								77
							},
							{
								"handitem,HandItemR_Spoon_Omurice_I_.menu",
								78
							},
							{
								"handitem,HandItemR_Suika_I_.menu",
								79
							},
							{
								"handitem,HandItemR_Tomato_I_.menu",
								80
							},
							{
								"handitem,HandItemR_Tomorokoshi_I_.menu",
								81
							},
							{
								"handitem,HandItemR_Tomorokoshi_yaki_I_.menu",
								82
							},
							{
								"handitem,HandItemR_TropicalGlass_I_.menu",
								83
							},
							{
								"handitem,HandItemR_Uchiwa_I_.menu",
								84
							},
							{
								"handitem,HandItemR_Ukiwa_I_.menu",
								85
							},
							{
								"handitem,HandItemR_Furaidopoteto_I_.menu",
								86
							},
							{
								"handitem,HandItemR_Ketchup_I_.menu",
								87
							},
							{
								"handitem,HandItemR_MelonSoda_I_.menu",
								88
							},
							{
								"handitem,HandItemR_Spoon_Pafe_I_.menu",
								89
							},
							{
								"handitem,HandItemR_karaoke_maracas_I_.menu",
								90
							},
							{
								"handitem,HandItemR_karaoke_sensu_I_.menu",
								91
							},
							{
								"handitem,HandItemR_cocktail_red_I_.menu",
								92
							},
							{
								"handitem,HandItemR_cocktail_blue_I_.menu",
								93
							},
							{
								"handitem,HandItemR_cocktail_yellow_I_.menu",
								94
							},
							{
								"handitem,HandItemR_pretzel_I_.menu",
								95
							},
							{
								"handitem,HandItemR_smoothie_red_I_.menu",
								96
							},
							{
								"handitem,HandItemR_smoothie_green_I_.menu",
								97
							},
							{
								"handitem,HandItemL_Etoile_Saucer_I_.menu",
								98
							},
							{
								"handitem,HandItemR_Etoile_Teacup_I_.menu",
								99
							},
							{
								"handitem,HandItemL_Katuramuki_Daikon_I_.menu",
								100
							},
							{
								"handitem,HandItemR_Usuba_Houchou_I_.menu",
								101
							},
							{
								"handitem,HandItemL_Karte_I_.menu",
								102
							},
							{
								"handitem,HandItemR_Chusyaki_I_.menu",
								103
							},
							{
								"handitem,HandItemL_Cracker_I_.menu",
								104
							},
							{
								"handitem,HandItemR_Nei_Heartful_I_.menu",
								105
							},
							{
								"handitem,HandItemR_Shaker_I_.menu",
								106
							},
							{
								"handitem,HandItemR_SmartPhone_I_.menu",
								107
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-3.TryGetValue(text, out num))
					//{
					//	goto IL_2E56;
					//}
					if (!ItemUIArray.TryGetValue(text, out num))
					{
						goto IL_2E56;
					}
					switch (num)
					{
						case 0:
							this.itemCombo2List[i] = new GUIContent("ワイングラス");
							break;
						case 1:
							this.itemCombo2List[i] = new GUIContent("ワインボトル");
							break;
						case 2:
							this.itemCombo2List[i] = new GUIContent("ラケット");
							break;
						case 3:
							this.itemCombo2List[i] = new GUIContent("ハタキ");
							break;
						case 4:
							this.itemCombo2List[i] = new GUIContent("モップ");
							break;
						case 5:
							this.itemCombo2List[i] = new GUIContent("ほうき");
							break;
						case 6:
							this.itemCombo2List[i] = new GUIContent("雑巾");
							break;
						case 7:
							this.itemCombo2List[i] = new GUIContent("Chu-B Lip");
							break;
						case 8:
							this.itemCombo2List[i] = new GUIContent("耳かき");
							break;
						case 9:
							this.itemCombo2List[i] = new GUIContent("ペン");
							break;
						case 10:
							this.itemCombo2List[i] = new GUIContent("おたま");
							break;
						case 11:
							this.itemCombo2List[i] = new GUIContent("包丁");
							break;
						case 12:
							this.itemCombo2List[i] = new GUIContent("本");
							break;
						case 13:
							this.itemCombo2List[i] = new GUIContent("パフ");
							break;
						case 14:
							this.itemCombo2List[i] = new GUIContent("リップ");
							break;
						case 15:
							this.itemCombo2List[i] = new GUIContent("刺繍");
							break;
						case 16:
							this.itemCombo2List[i] = new GUIContent("針");
							break;
						case 17:
							this.itemCombo2List[i] = new GUIContent("皿");
							break;
						case 18:
							this.itemCombo2List[i] = new GUIContent("スポンジ");
							break;
						case 19:
							this.itemCombo2List[i] = new GUIContent("手枷1");
							break;
						case 20:
							this.itemCombo2List[i] = new GUIContent("手枷2");
							break;
						case 21:
							this.itemCombo2List[i] = new GUIContent("バストレイ");
							break;
						case 22:
							this.itemCombo2List[i] = new GUIContent("後ろ手拘束具");
							break;
						case 23:
							this.itemCombo2List[i] = new GUIContent("磔台");
							break;
						case 24:
							this.itemCombo2List[i] = new GUIContent("磔台2");
							break;
						case 25:
							this.itemCombo2List[i] = new GUIContent("ダンスハタキ");
							break;
						case 26:
							this.itemCombo2List[i] = new GUIContent("ダンスモップ");
							break;
						case 27:
							this.itemCombo2List[i] = new GUIContent("ダンス雑巾");
							break;
						case 28:
							this.itemCombo2List[i] = new GUIContent("小皿");
							break;
						case 29:
							this.itemCombo2List[i] = new GUIContent("ティーカップ");
							break;
						case 30:
							this.itemCombo2List[i] = new GUIContent("ティーソーサー");
							break;
						case 31:
							this.itemCombo2List[i] = new GUIContent("ホールケーキ");
							break;
						case 32:
							this.itemCombo2List[i] = new GUIContent("メニュー表");
							break;
						case 33:
							this.itemCombo2List[i] = new GUIContent("バイブ");
							break;
						case 34:
							this.itemCombo2List[i] = new GUIContent("ピンクバイブ");
							break;
						case 35:
							this.itemCombo2List[i] = new GUIContent("太バイブ");
							break;
						case 36:
							this.itemCombo2List[i] = new GUIContent("アナルバイブ");
							break;
						case 37:
							this.itemCombo2List[i] = new GUIContent("双頭バイブ");
							break;
						case 38:
							this.itemCombo2List[i] = new GUIContent("前：バイブ");
							break;
						case 39:
							this.itemCombo2List[i] = new GUIContent("前：太バイブ");
							break;
						case 40:
							this.itemCombo2List[i] = new GUIContent("前：ピンクバイブ");
							break;
						case 41:
							this.itemCombo2List[i] = new GUIContent("後：アナルバイブ");
							break;
						case 42:
							this.itemCombo2List[i] = new GUIContent("後：バイブ");
							break;
						case 43:
							this.itemCombo2List[i] = new GUIContent("後：太バイブ");
							break;
						case 44:
							this.itemCombo2List[i] = new GUIContent("後：ピンクバイブ");
							break;
						case 45:
							this.itemCombo2List[i] = new GUIContent("カレー");
							break;
						case 46:
							this.itemCombo2List[i] = new GUIContent("カラオケマイク");
							break;
						case 47:
							this.itemCombo2List[i] = new GUIContent("パスタ");
							break;
						case 48:
							this.itemCombo2List[i] = new GUIContent("オムライス1");
							break;
						case 49:
							this.itemCombo2List[i] = new GUIContent("オムライス2");
							break;
						case 50:
							this.itemCombo2List[i] = new GUIContent("オムライス3");
							break;
						case 51:
							this.itemCombo2List[i] = new GUIContent("ビールボトル");
							break;
						case 52:
							this.itemCombo2List[i] = new GUIContent("ビールボトル(開)");
							break;
						case 53:
							this.itemCombo2List[i] = new GUIContent("ビールグラス");
							break;
						case 54:
							this.itemCombo2List[i] = new GUIContent("スイカ");
							break;
						case 55:
							this.itemCombo2List[i] = new GUIContent("日記");
							break;
						case 56:
							this.itemCombo2List[i] = new GUIContent("DVD1");
							break;
						case 57:
							this.itemCombo2List[i] = new GUIContent("DVD2");
							break;
						case 58:
							this.itemCombo2List[i] = new GUIContent("DVD3");
							break;
						case 59:
							this.itemCombo2List[i] = new GUIContent("DVD4");
							break;
						case 60:
							this.itemCombo2List[i] = new GUIContent("DVD5");
							break;
						case 61:
							this.itemCombo2List[i] = new GUIContent("フォーク");
							break;
						case 62:
							this.itemCombo2List[i] = new GUIContent("手持ち花火");
							break;
						case 63:
							this.itemCombo2List[i] = new GUIContent("じょうろ");
							break;
						case 64:
							this.itemCombo2List[i] = new GUIContent("小瓶");
							break;
						case 65:
							this.itemCombo2List[i] = new GUIContent("串焼き");
							break;
						case 66:
							this.itemCombo2List[i] = new GUIContent("牛乳");
							break;
						case 67:
							this.itemCombo2List[i] = new GUIContent("牛乳(開)");
							break;
						case 68:
							this.itemCombo2List[i] = new GUIContent("マグカップ");
							break;
						case 69:
							this.itemCombo2List[i] = new GUIContent("夏みかん");
							break;
						case 70:
							this.itemCombo2List[i] = new GUIContent("ニンジン");
							break;
						case 71:
							this.itemCombo2List[i] = new GUIContent("お猪口");
							break;
						case 72:
							this.itemCombo2List[i] = new GUIContent("さつまいも");
							break;
						case 73:
							this.itemCombo2List[i] = new GUIContent("スコップ");
							break;
						case 74:
							this.itemCombo2List[i] = new GUIContent("線香花火");
							break;
						case 75:
							this.itemCombo2List[i] = new GUIContent("貝殻");
							break;
						case 76:
							this.itemCombo2List[i] = new GUIContent("紙片");
							break;
						case 77:
							this.itemCombo2List[i] = new GUIContent("スプーン(カレー)");
							break;
						case 78:
							this.itemCombo2List[i] = new GUIContent("スプーン(オムライス)");
							break;
						case 79:
							this.itemCombo2List[i] = new GUIContent("スイカ2");
							break;
						case 80:
							this.itemCombo2List[i] = new GUIContent("トマト");
							break;
						case 81:
							this.itemCombo2List[i] = new GUIContent("トウモロコシ");
							break;
						case 82:
							this.itemCombo2List[i] = new GUIContent("焼きトウモロコシ");
							break;
						case 83:
							this.itemCombo2List[i] = new GUIContent("トロピカルグラス");
							break;
						case 84:
							this.itemCombo2List[i] = new GUIContent("うちわ");
							break;
						case 85:
							this.itemCombo2List[i] = new GUIContent("浮き輪");
							break;
						case 86:
							this.itemCombo2List[i] = new GUIContent("フライドポテト1本");
							break;
						case 87:
							this.itemCombo2List[i] = new GUIContent("ケチャップ");
							break;
						case 88:
							this.itemCombo2List[i] = new GUIContent("メロンソーダ");
							break;
						case 89:
							this.itemCombo2List[i] = new GUIContent("パフェスプーン");
							break;
						case 90:
							this.itemCombo2List[i] = new GUIContent("マラカス");
							break;
						case 91:
							this.itemCombo2List[i] = new GUIContent("扇子");
							break;
						case 92:
							this.itemCombo2List[i] = new GUIContent("カクテル・赤");
							break;
						case 93:
							this.itemCombo2List[i] = new GUIContent("カクテル・青");
							break;
						case 94:
							this.itemCombo2List[i] = new GUIContent("カクテル・黄");
							break;
						case 95:
							this.itemCombo2List[i] = new GUIContent("ポッキー");
							break;
						case 96:
							this.itemCombo2List[i] = new GUIContent("スムージー・赤");
							break;
						case 97:
							this.itemCombo2List[i] = new GUIContent("スムージー・緑");
							break;
						case 98:
							this.itemCombo2List[i] = new GUIContent("ティーソーサー");
							break;
						case 99:
							this.itemCombo2List[i] = new GUIContent("ティーカップ");
							break;
						case 100:
							this.itemCombo2List[i] = new GUIContent("桂むき大根");
							break;
						case 101:
							this.itemCombo2List[i] = new GUIContent("薄刃包丁");
							break;
						case 102:
							this.itemCombo2List[i] = new GUIContent("カルテ");
							break;
						case 103:
							this.itemCombo2List[i] = new GUIContent("注射器");
							break;
						case 104:
							this.itemCombo2List[i] = new GUIContent("クラッカー");
							break;
						case 105:
							this.itemCombo2List[i] = new GUIContent("ハートフルねい人形");
							break;
						case 106:
							this.itemCombo2List[i] = new GUIContent("シェイカー");
							break;
						case 107:
							this.itemCombo2List[i] = new GUIContent("スマートフォン");
							break;
						default:
							goto IL_2E56;
					}
				IL_2E6F:
					i++;
					continue;
				IL_2E56:
					this.itemCombo2List[i] = new GUIContent(this.itemBArray[i]);
					goto IL_2E6F;
				}
				for (int k = 0; k < this.doguCombo.Length; k++)
				{
					this.doguCombo[k].selectedItemIndex = 0;
					this.doguComboList.Add(new GUIContent[this.doguBArray[k].Length]);
					for (i = 0; i < this.doguBArray[k].Length; i++)
					{
						string text2 = this.doguBArray[k][i];
						for (int j = 0; j < this.doguNameList.Count; j++)
						{
							string[] array = this.doguNameList[j].Split(new char[]
							{
								','
							});
							if (text2 == array[0])
							{
								text2 = array[1];
							}
						}
						this.doguComboList[k][i] = new GUIContent(text2);
					}
				}
				this.kankyo2InitFlg = true;
			}
			this.listStyle3.padding.top = this.GetPix(1);
			this.listStyle3.padding.bottom = this.GetPix(0);
			this.listStyle3.fontSize = this.GetPix(13);
			if (this.poseCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.poseGroupCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.sceneLevel == 3 || this.sceneLevel == 5 || this.isF6)
			{
				if (!this.isF6)
				{
					bool value = true;
					if (this.faceFlg || this.poseFlg || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
					{
						value = false;
					}
					if (GUI.Toggle(new Rect((float)this.GetPix(2), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), value, "配置", guistyle6))
					{
						this.faceFlg = false;
						this.poseFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
						this.bGui = true;
						this.isGuiInit = true;
					}
				}
				if (!this.yotogiFlg)
				{
					if (GUI.Toggle(new Rect((float)this.GetPix(42), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.poseFlg, "操作", guistyle6))
					{
						this.poseFlg = true;
						this.faceFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
					}
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(82), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.faceFlg, "表情", guistyle6))
				{
					this.faceFlg = true;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					if (!this.faceFlg2)
					{
						this.isFaceInit = true;
						this.faceFlg2 = true;
						this.maidArray[this.selectMaidIndex].boMabataki = false;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					this.isFaceInit = true;
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(122), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyoFlg, "環境", guistyle6))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = true;
					this.kankyo2Flg = false;
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(162), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyo2Flg, "環2", guistyle6))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = true;
				}
				if (!this.line1)
				{
					this.line1 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.3f));
					this.line2 = this.MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 0.6f));
				}
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(200), 2f), this.line1);
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(200), 1f), this.line2);
			}
			if (this.isDanceStop)
			{
				this.isStop[this.selectMaidIndex] = true;
				this.isDanceStop = false;
			}
			if (this.kankyoCombo.isClickedComboButton || this.slotCombo.isClickedComboButton || this.itemCombo2.isClickedComboButton || this.bgCombo2.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.doguSelectFlg3)
			{
				int stockMaidCount = characterMgr.GetStockMaidCount();
				float num2 = (float)this.GetPix(45);
				Rect position;
				Rect viewRect;
				if (this.sceneLevel != 5)
				{
					position = new Rect((float)this.GetPix(7), (float)this.GetPix(108), (float)(this.GetPix(44) * 4 + this.GetPix(20)), this.rectWin.height * 0.825f);
					viewRect = new Rect(0f, 0f, position.width * 0.85f, num2 * (float)Math.Ceiling((double)this.sortList.Count / 4.0) + (float)this.GetPix(50));
				}
				else
				{
					position = new Rect((float)this.GetPix(7), (float)this.GetPix(108), (float)(this.GetPix(44) * 4 + this.GetPix(20)), this.rectWin.height * 0.825f * 0.96f);
					viewRect = new Rect(0f, 0f, position.width * 0.85f, num2 * (float)Math.Ceiling((double)this.sortList.Count / 4.0) + (float)this.GetPix(50) * 0.92f);
				}
				this.scrollPos = GUI.BeginScrollView(position, this.scrollPos, viewRect);
				for (int i = 0; i < this.sortList.Count; i++)
				{
					Rect position2 = new Rect((float)this.GetPix(i % 4 * 45), (float)this.GetPix(i / 4 * 45), (float)this.GetPix(44), (float)this.GetPix(44));
					if (GUI.Button(position2, "Button"))
					{
						string text3 = this.sortList[i].menu;
						byte[] array2 = null;
						using (AFileBase afileBase = GameUty.FileOpen(text3, null))
						{
							NDebug.Assert(afileBase.IsValid(), "メニューファイルが存在しません。 :" + text3);
							if (array2 == null)
							{
								array2 = new byte[afileBase.GetSize()];
							}
							else if (array2.Length < afileBase.GetSize())
							{
								array2 = new byte[afileBase.GetSize()];
							}
							afileBase.Read(ref array2, afileBase.GetSize());
						}
						string[] array3 = MultipleMaids.ProcScriptBin(this.maidArray[0], array2, text3, false);
						GameObject gameObject = ImportCM2.LoadSkinMesh_R(array3[0], array3, "", this.maidArray[0].body0.goSlot[8], 1);
						this.doguBObject.Add(gameObject);
						gameObject.name = text3;
						Vector3 zero = Vector3.zero;
						Vector3 zero2 = Vector3.zero;
						zero.z = 0.4f;
						gameObject.transform.localPosition = zero;
						gameObject.transform.localRotation = Quaternion.Euler(zero2);
						this.doguCnt = this.doguBObject.Count - 1;
						this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
						this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
						this.gDogu[this.doguCnt].layer = 8;
						this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
						this.gDogu[this.doguCnt].SetActive(false);
						this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
						this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
						this.mDogu[this.doguCnt].isScale = false;
						this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
						this.mDogu[this.doguCnt].maid = gameObject;
						this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
						this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
						this.mDogu[this.doguCnt].ido = 1;
					}
					GUI.DrawTexture(position2, this.sortList[i].tex);
				}
				GUI.EndScrollView();
			}
			GUI.enabled = true;
			if (GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(25), (float)this.GetPix(39), (float)this.GetPix(20)), this.doguSelectFlg1, "道具", guistyle6))
			{
				this.doguSelectFlg1 = true;
				this.doguSelectFlg2 = false;
				this.doguSelectFlg3 = false;
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(56), (float)this.GetPix(25), (float)this.GetPix(50), (float)this.GetPix(20)), this.doguSelectFlg2, "ﾏｲﾙｰﾑ", guistyle6))
			{
				this.doguSelectFlg1 = false;
				this.doguSelectFlg2 = true;
				this.doguSelectFlg3 = false;
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(117), (float)this.GetPix(25), (float)this.GetPix(86), (float)this.GetPix(20)), this.doguSelectFlg3, "服装･ｱｸｾｻﾘ", guistyle6))
			{
				this.doguSelectFlg1 = false;
				this.doguSelectFlg2 = false;
				this.doguSelectFlg3 = true;
			}
			GUI.enabled = true;
			if (this.doguSelectFlg3)
			{
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(78), (float)this.GetPix(100), (float)this.GetPix(25)), "服装", guistyle2);
				guistyle2.fontSize = this.GetPix(9);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(89), (float)this.GetPix(100), (float)this.GetPix(25)), "アクセサリ", guistyle2);
				guistyle2.fontSize = this.GetPix(11);
			}
			if (this.doguSelectFlg1)
			{
				int num3 = 58;
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3), (float)this.GetPix(100), (float)this.GetPix(24)), "家具", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 30), (float)this.GetPix(100), (float)this.GetPix(24)), "道具", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 60), (float)this.GetPix(100), (float)this.GetPix(24)), "文房具", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 90), (float)this.GetPix(100), (float)this.GetPix(24)), "グルメ", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 120), (float)this.GetPix(100), (float)this.GetPix(24)), "ドリンク", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 150), (float)this.GetPix(100), (float)this.GetPix(24)), "カジノ", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 180), (float)this.GetPix(100), (float)this.GetPix(24)), "プレイ", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 210), (float)this.GetPix(100), (float)this.GetPix(24)), "ﾊﾟｰﾃｨｸﾙ", guistyle2);
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(num3 + 240), (float)this.GetPix(100), (float)this.GetPix(24)), "その他", guistyle2);
			}
			if (this.doguSelectFlg2)
			{
				if (this.myCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				float num2 = (float)this.GetPix(45);
				Rect position;
				Rect viewRect;
				if (this.sceneLevel != 5)
				{
					position = new Rect((float)this.GetPix(7), (float)this.GetPix(92), (float)(this.GetPix(44) * 4 + this.GetPix(20)), this.rectWin.height * 0.85f);
					viewRect = new Rect(0f, 0f, position.width * 0.85f, num2 * (float)Math.Ceiling((double)this.sortListMy.Count / 4.0) + (float)this.GetPix(50));
				}
				else
				{
					position = new Rect((float)this.GetPix(7), (float)this.GetPix(92), (float)(this.GetPix(44) * 4 + this.GetPix(20)), this.rectWin.height * 0.85f * 0.96f);
					viewRect = new Rect(0f, 0f, position.width * 0.85f, num2 * (float)Math.Ceiling((double)this.sortListMy.Count / 4.0) + (float)this.GetPix(50) * 0.92f);
				}
				this.scrollPos = GUI.BeginScrollView(position, this.scrollPos, viewRect);
				for (int i = 0; i < this.sortListMy.Count; i++)
				{
					Rect position2 = new Rect((float)this.GetPix(i % 4 * 45), (float)this.GetPix(i / 4 * 45), (float)this.GetPix(44), (float)this.GetPix(44));
					if (GUI.Button(position2, "Button"))
					{
						this.createMyRoomObject(this.sortListMy[i].order.ToString());
					}
					GUI.DrawTexture(position2, this.sortListMy[i].tex);
				}
				GUI.EndScrollView();
				GUI.enabled = true;
			}
			if (this.doguSelectFlg2)
			{
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(60), (float)this.GetPix(100), (float)this.GetPix(24)), "カテゴリ", guistyle2);
				int num4 = this.myCombo.List(new Rect((float)this.GetPix(51), (float)this.GetPix(58), (float)this.GetPix(100), (float)this.GetPix(23)), this.myComboList[this.myIndex].text, this.myComboList, guistyle4, "box", this.listStyle3);
				if (num4 != this.myIndex)
				{
					this.myIndex = num4;
					this.sortListMy.Clear();
					if (this.myIndex > 0)
					{
						List<int> categoryIDList = PlacementData.CategoryIDList;
						int placementObjCategoryID = categoryIDList[this.myIndex - 1];
						List<PlacementData.Data> datas = PlacementData.GetDatas((PlacementData.Data datam) => datam.categoryID == placementObjCategoryID);
						this.scrollPos = new Vector2(0f, 0f);
						if (this.sortListMy.Count == 0)
						{
							foreach (PlacementData.Data data in datas)
							{
								MultipleMaids.SortItemMy sortItemMy = new MultipleMaids.SortItemMy();
								sortItemMy.order = data.ID;
								sortItemMy.name = data.assetName;
								sortItemMy.tex = data.GetThumbnail();
								this.sortListMy.Add(sortItemMy);
							}
						}
					}
				}
			}
			GUI.enabled = true;
			if (this.doguSelectFlg1)
			{
				bool flag = false;
				for (int l = 0; l < this.doguBArray.Count; l++)
				{
					if (this.doguCombo[l].isClickedComboButton)
					{
						flag = true;
					}
				}
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(448), (float)this.GetPix(100), (float)this.GetPix(24)), "背景(小)", guistyle2);
				if (flag || this.itemCombo2.isClickedComboButton || this.parCombo1.isClickedComboButton || this.doguCombo2.isClickedComboButton || this.parCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.bgIndexB = this.bgCombo2.List(new Rect((float)this.GetPix(51), (float)this.GetPix(445), (float)this.GetPix(100), (float)this.GetPix(23)), this.bgCombo2List[this.bgIndexB].text, this.bgCombo2List, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(445), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
				{
					GameObject gameObject = null;
					UnityEngine.Object @object = GameMain.Instance.BgMgr.CreateAssetBundle(this.bgArray[this.bgIndexB]);
					if (@object == null)
					{
						@object = Resources.Load("BG/" + this.bgArray[this.bgIndexB]);
					}
					gameObject = (UnityEngine.Object.Instantiate(@object) as GameObject);
					this.doguBObject.Add(gameObject);
					gameObject.name = "BG_" + this.bgArray[this.bgIndexB];
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					zero.z = 0.4f;
					gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					gameObject.transform.localPosition = zero;
					gameObject.transform.localRotation = Quaternion.Euler(zero2);
					this.doguCnt = this.doguBObject.Count - 1;
					this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
					this.gDogu[this.doguCnt].layer = 8;
					this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
					this.gDogu[this.doguCnt].SetActive(false);
					this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
					this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
					this.mDogu[this.doguCnt].isScale = false;
					this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
					this.mDogu[this.doguCnt].maid = gameObject;
					this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
					this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
					this.mDogu[this.doguCnt].ido = 1;
				}
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(418), (float)this.GetPix(100), (float)this.GetPix(24)), "アイテム", guistyle2);
				if (flag || this.parCombo1.isClickedComboButton || this.doguCombo2.isClickedComboButton || this.parCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.itemIndexB = this.itemCombo2.List(new Rect((float)this.GetPix(51), (float)this.GetPix(415), (float)this.GetPix(100), (float)this.GetPix(23)), this.itemCombo2List[this.itemIndexB].text, this.itemCombo2List, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(415), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
				{
					string text3 = this.itemBArray[this.itemIndexB].Split(new char[]
					{
						','
					})[1];
					byte[] array2 = null;
					using (AFileBase afileBase = GameUty.FileOpen(text3, null))
					{
						NDebug.Assert(afileBase.IsValid(), "メニューファイルが存在しません。 :" + text3);
						if (array2 == null)
						{
							array2 = new byte[afileBase.GetSize()];
						}
						else if (array2.Length < afileBase.GetSize())
						{
							array2 = new byte[afileBase.GetSize()];
						}
						afileBase.Read(ref array2, afileBase.GetSize());
					}
					string[] array3 = MultipleMaids.ProcScriptBin(this.maidArray[0], array2, text3, false);
					GameObject gameObject = ImportCM2.LoadSkinMesh_R(array3[0], array3, "", this.maidArray[0].body0.goSlot[8], 1);
					this.doguBObject.Add(gameObject);
					gameObject.name = text3;
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					zero.z = 0.4f;
					int num5 = this.itemIndexB;
					switch (num5)
					{
						case 0:
							zero2.z = 90f;
							zero.y = 0.04f;
							goto IL_47BA;
						case 1:
							zero2.z = 90f;
							goto IL_47BA;
						case 2:
						case 3:
							break;
						case 4:
							zero2.x = 90f;
							goto IL_47BA;
						default:
							switch (num5)
							{
								case 19:
									goto IL_47BA;
								case 20:
									goto IL_47BA;
								case 21:
									goto IL_47BA;
								case 22:
									goto IL_47BA;
								case 23:
									goto IL_47BA;
								case 25:
									zero2.z = 90f;
									goto IL_47BA;
								case 26:
									zero2.z = 90f;
									goto IL_47BA;
								case 27:
									zero2.z = 90f;
									goto IL_47BA;
							}
							break;
					}
					zero2.x = -90f;
				IL_47BA:
					string text = gameObject.name;
					switch (text)
					{
						case "HandItemR_Curry_I_.menu":
						case "HandItemR_Pasta_I_.menu":
						case "HandItemR_Omurice1_I_.menu":
						case "HandItemR_Omurice2_I_.menu":
						case "HandItemR_Omurice3_I_.menu":
						case "HandItemR_Kushiyaki_I_.menu":
						case "HandItemR_Tomorokoshi_I_.menu":
						case "HandItemR_Tomorokoshi_yaki_I_.menu":
						case "HandItemR_Spoon_Curry_I_.menu":
						case "HandItemR_Spoon_Omurice_I_.menu":
						case "HandItemR_Folk_I_.menu":
						case "HandItemR_Crops_Suika_I_.menu":
						case "HandItemR_Ninjin_I_.menu":
						case "HandItemR_Satumaimo_I_.menu":
						case "HandItemL_Karaoke_Mike_I_.menu":
						case "HandItemR_Hanabi_I_.menu":
						case "HandItemR_Senkouhanabi_I_.menu":
						case "HandItemR_DVD1_I_.menu":
						case "HandItemR_DVD2_I_.menu":
						case "HandItemR_DVD3_I_.menu":
						case "HandItemR_DVD4_I_.menu":
						case "HandItemR_DVD5_I_.menu":
						case "HandItemR_Scoop_I_.menu":
						case "HandItemR_Shell_I_.menu":
						case "HandItemR_Uchiwa_I_.menu":
							zero2.z = 90f;
							break;
						case "HandItemR_Tomato_I_.menu":
						case "HandItemR_Natumikan_I_.menu":
						case "HandItemR_Kobin_I_.menu":
						case "HandItemR_Jyouro_I_.menu":
						case "HandItemR_Mugcup_I_.menu":
						case "HandItemR_Suika_I_.menu":
						case "HandItemR_BeerBottle(cap_on)_I_.menu":
						case "HandItemR_BeerBottle(cap_off)_I_.menu":
						case "HandItemR_BeerGlass_I_.menu":
						case "HandItemR_TropicalGlass_I_.menu":
						case "HandItemR_MilkBottle(cap_on)_I_.menu":
						case "HandItemR_MilkBottle(cap_off)_I_.menu":
						case "HandItemR_Ochoko_I_.menu":
						case "HandItemR_Ketchup_I_.menu":
						case "HandItemR_MelonSoda_I_.menu":
						case "HandItemR_cocktail_red_I_.menu":
						case "HandItemR_cocktail_blue_I_.menu":
						case "HandItemR_cocktail_yellow_I_.menu":
						case "HandItemR_smoothie_red_I_.menu":
						case "HandItemR_smoothie_green_I_.menu":
						case "HandItemL_Etoile_Saucer_I_.menu":
						case "HandItemR_Etoile_Teacup_I_.menu":
						case "HandItemL_Katuramuki_Daikon_I_.menu":
						case "HandItemL_Karte_I_.menu":
						case "HandItemR_Nei_Heartful_I_.menu":
						case "HandItemR_Shaker_I_.menu":
						case "HandItemR_SmartPhone_I_.menu":
							zero2.x = 0f;
							zero2.z = 90f;
							break;
					}
					gameObject.transform.localPosition = zero;
					gameObject.transform.localRotation = Quaternion.Euler(zero2);
					this.doguCnt = this.doguBObject.Count - 1;
					this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
					this.gDogu[this.doguCnt].layer = 8;
					this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
					this.gDogu[this.doguCnt].SetActive(false);
					this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
					this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
					this.mDogu[this.doguCnt].isScale = false;
					this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
					this.mDogu[this.doguCnt].maid = gameObject;
					this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
					this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
					this.mDogu[this.doguCnt].ido = 1;
				}
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(388), (float)this.GetPix(150), (float)this.GetPix(24)), "大道具2", guistyle2);
				if (flag || this.doguCombo2.isClickedComboButton || this.parCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.parIndex1 = this.parCombo1.List(new Rect((float)this.GetPix(51), (float)this.GetPix(385), (float)this.GetPix(100), (float)this.GetPix(23)), this.parCombo1List[this.parIndex1].text, this.parCombo1List, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(385), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
				{
					GameObject gameObject = null;
					bool flag2 = false;
					bool flag3 = false;
					if (this.parArray1[this.parIndex1].Contains("#"))
					{
						string[] array = this.parArray1[this.parIndex1].Split(new char[]
						{
							'#'
						});
						gameObject = GameMain.Instance.BgMgr.CreateAssetBundle(array[1]);
						if (gameObject != null)
						{
							gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
							MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
							for (int l = 0; l < componentsInChildren.Length; l++)
							{
								if (componentsInChildren[l] != null)
								{
									componentsInChildren[l].shadowCastingMode = ShadowCastingMode.Off;
								}
							}
						}
						flag2 = true;
						if (!this.parArray1[this.parIndex1].Contains("Odogu_"))
						{
							flag3 = true;
						}
						this.doguBObject.Add(gameObject);
					}
					else if (!this.parArray1[this.parIndex1].StartsWith("mirror") && this.parArray1[this.parIndex1].IndexOf(":") < 0)
					{
						UnityEngine.Object @object = Resources.Load("Prefab/" + this.parArray1[this.parIndex1]);
						gameObject = (UnityEngine.Object.Instantiate(@object) as GameObject);
						this.doguBObject.Add(gameObject);
					}
					else if (this.parArray1[this.parIndex1].StartsWith("mirror"))
					{
						Material material = new Material(Shader.Find("Mirror"));
						GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
						gameObject2.GetComponent<Renderer>().material = material;
						gameObject2.AddComponent<MirrorReflection2>();
						MirrorReflection2 component = gameObject2.GetComponent<MirrorReflection2>();
						component.m_TextureSize = 2048;
						component.m_ClipPlaneOffset = 0.07f;
						gameObject2.GetComponent<Renderer>().enabled = true;
						gameObject = gameObject2;
						this.doguBObject.Add(gameObject);
					}
					else
					{
						string[] array = this.parArray1[this.parIndex1].Split(new char[]
						{
							':'
						});
						UnityEngine.Object @object = GameMain.Instance.BgMgr.CreateAssetBundle(array[0]);
						if (@object == null)
						{
							@object = Resources.Load("BG/" + array[0]);
						}
						GameObject gameObject3 = UnityEngine.Object.Instantiate(@object) as GameObject;
						int num6 = 0;
						int num7 = 0;
						int.TryParse(array[1], out num7);
						foreach (object obj in gameObject3.transform)
						{
							Transform transform = (Transform)obj;
							if (num7 == num6)
							{
								gameObject = transform.gameObject;
								break;
							}
							num6++;
						}
						this.doguBObject.Add(gameObject);
						gameObject.transform.parent = null;
						UnityEngine.Object.Destroy(gameObject3);
						gameObject3.SetActive(false);
					}
					gameObject.name = this.parArray1[this.parIndex1];
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					switch (this.parIndex1)
					{
						case 0:
							zero.z = -0.6f;
							zero.y = 0.96f;
							zero2.z = 180f;
							zero2.x = -90f;
							gameObject.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
							break;
						case 1:
							zero.z = -0.6f;
							zero.y = 0.96f;
							zero2.z = 180f;
							zero2.x = -90f;
							gameObject.transform.localScale = new Vector3(0.1f, 0.4f, 0.2f);
							break;
						case 2:
							zero.z = -0.6f;
							zero.y = 0.85f;
							zero2.z = 180f;
							zero2.x = -90f;
							gameObject.transform.localScale = new Vector3(0.03f, 0.18f, 0.124f);
							break;
						case 3:
						case 4:
						case 5:
						case 6:
						case 7:
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 13:
						case 14:
						case 15:
						case 16:
						case 17:
							zero.z = 0.5f;
							zero2.x = -90f;
							break;
					}
					if (flag2)
					{
						zero.z = 0.4f;
						if (flag3)
						{
							zero2.x = -90f;
						}
					}
					if (gameObject.name == "Salon_Entrance:3" || gameObject.name == "Salon_Entrance:4" || gameObject.name == "Salon_Entrance:1" || gameObject.name == "Salon_Entrance:2" || gameObject.name == "Salon_Entrance:0" || gameObject.name == "Shitsumu:23" || gameObject.name == "Shitsumu_Night:23")
					{
						zero.z = 0.5f;
						zero2.x = -90f;
					}
					if (gameObject.name == "Pool:26")
					{
						zero.z = 0.5f;
						zero2.x = -90f;
						zero2.z = 90f;
						zero.y = 0.15f;
					}
					if (gameObject.name == "Particle/pstarY_act2")
					{
						zero2.y = 90f;
					}
					gameObject.transform.localPosition = zero;
					gameObject.transform.localRotation = Quaternion.Euler(zero2);
					this.doguCnt = this.doguBObject.Count - 1;
					this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
					this.gDogu[this.doguCnt].layer = 8;
					this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
					this.gDogu[this.doguCnt].SetActive(false);
					this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
					this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
					this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
					this.mDogu[this.doguCnt].maid = gameObject;
					this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
					this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
					this.mDogu[this.doguCnt].ido = 1;
					this.mDogu[this.doguCnt].isScale = false;
					if (gameObject.name == "Particle/pLineY")
					{
						this.mDogu[this.doguCnt].count = 180;
						gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
					}
					if (gameObject.name == "Particle/pLineP02")
					{
						this.mDogu[this.doguCnt].count = 115;
						gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
					}
					if (gameObject.name == "Particle/pLine_act2")
					{
						this.mDogu[this.doguCnt].count = 90;
						gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
					}
					if (gameObject.name == "Particle/pHeart01")
					{
						this.mDogu[this.doguCnt].count = 77;
					}
					if (this.parIndex1 < 3)
					{
						this.mDogu[this.doguCnt].isScale = true;
						this.mDogu[this.doguCnt].isScale2 = true;
						this.mDogu[this.doguCnt].scale2 = gameObject.transform.localScale;
						if (this.parIndex1 == 0)
						{
							this.mDogu[this.doguCnt].scale = new Vector3(gameObject.transform.localScale.x * 5f, gameObject.transform.localScale.y * 5f, gameObject.transform.localScale.z * 5f);
						}
						if (this.parIndex1 == 1)
						{
							this.mDogu[this.doguCnt].scale = new Vector3(gameObject.transform.localScale.x * 10f, gameObject.transform.localScale.y * 10f, gameObject.transform.localScale.z * 10f);
						}
						if (this.parIndex1 == 2)
						{
							this.mDogu[this.doguCnt].scale = new Vector3(gameObject.transform.localScale.x * 33f, gameObject.transform.localScale.y * 33f, gameObject.transform.localScale.z * 33f);
						}
					}
					if (gameObject.GetComponent<Collider>() != null)
					{
						gameObject.GetComponent<Collider>().enabled = false;
					}
				}
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(358), (float)this.GetPix(100), (float)this.GetPix(24)), "大道具1", guistyle2);
				if (flag || this.parCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.doguB2Index = this.doguCombo2.List(new Rect((float)this.GetPix(51), (float)this.GetPix(355), (float)this.GetPix(100), (float)this.GetPix(23)), this.doguCombo2List[this.doguB2Index].text, this.doguCombo2List, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(355), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
				{
					GameObject gameObject = null;
					if (!this.doguB1Array[this.doguB2Index].StartsWith("mirror"))
					{
						UnityEngine.Object @object;
						if (this.doguB1Array[this.doguB2Index].StartsWith("BG"))
						{
							string text2 = this.doguB1Array[this.doguB2Index].Replace("BG", "");
							@object = GameMain.Instance.BgMgr.CreateAssetBundle(text2);
							if (@object == null)
							{
								@object = Resources.Load("BG/" + text2);
							}
						}
						else
						{
							@object = Resources.Load("Prefab/" + this.doguB1Array[this.doguB2Index]);
						}
						gameObject = (UnityEngine.Object.Instantiate(@object) as GameObject);
						this.doguBObject.Add(gameObject);
					}
					else
					{
						Material material = new Material(Shader.Find("Mirror"));
						GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
						gameObject2.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
						gameObject2.GetComponent<Renderer>().material = material;
						gameObject2.AddComponent<MirrorReflection2>();
						MirrorReflection2 component = gameObject2.GetComponent<MirrorReflection2>();
						component.m_TextureSize = 2048;
						component.m_ClipPlaneOffset = 0f;
						gameObject2.GetComponent<Renderer>().enabled = true;
						gameObject = gameObject2;
						this.doguBObject.Add(gameObject);
					}
					gameObject.name = this.doguB1Array[this.doguB2Index];
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					string text = gameObject.name;
					switch (text)
					{
						case "Odogu_XmasTreeMini_photo_ver":
							zero.z = 0.6f;
							gameObject.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
							foreach (object obj2 in gameObject.transform)
							{
								Transform transform2 = (Transform)obj2;
								if (transform2.GetComponent<Collider>() != null)
								{
									transform2.GetComponent<Collider>().enabled = false;
								}
							}
							goto IL_6AE3;
						case "Odogu_KadomatsuMini_photo_ver":
							zero.z = 0.6f;
							gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
							foreach (object obj3 in gameObject.transform)
							{
								Transform transform2 = (Transform)obj3;
								if (transform2.GetComponent<Collider>() != null)
								{
									transform2.GetComponent<Collider>().enabled = false;
								}
							}
							goto IL_6AE3;
						case "Odogu_ClassRoomDesk":
							zero.z = 0.5f;
							zero2.x = -90f;
							goto IL_6AE3;
						case "Odogu_SimpleTable":
							zero.z = 0.5f;
							zero2.x = -90f;
							goto IL_6AE3;
						case "Odogu_DildoBox":
							zero.z = 0.5f;
							zero2.x = -90f;
							goto IL_6AE3;
						case "PlayAreaOut":
							zero.z = 0.5f;
							zero.y = 0.2f;
							goto IL_6AE3;
						case "Odogu_Dresser_photo_ver":
							GameObject.Find("Prim.00000000").GetComponent<Collider>().enabled = false;
							GameObject.Find("Prim.00000001").GetComponent<Collider>().enabled = false;
							GameObject.Find("Prim.00000002").GetComponent<Collider>().enabled = false;
							GameObject.Find("Prim.00000004").GetComponent<Collider>().enabled = false;
							goto IL_6AE3;
						case "BGodogu_bbqgrill":
						case "BGodogu_bucket":
						case "BGodogu_coolerbox":
						case "BGodogu_game_darts":
						case "BGodogu_game_dartsboard":
						case "BGodogu_nabe_huta":
						case "BGodogu_nabe_water":
						case "BGodogu_natumikan":
						case "BGodogu_rb_chair":
						case "BGodogu_rb_duck":
						case "BGodogu_rb_obon":
						case "BGodogu_rb_tokkuri":
						case "BGodogu_saracorn":
						case "BGodogu_saraimo":
						case "BGodogu_saratomato":
						case "BGodogu_sunanoshiro":
						case "BGodogu_sunanoyama":
						case "BGodogu_tsutsuhanabi":
						case "BGodogu_ukiwa":
						case "BGodogu_vf_crops_corn":
						case "BGodogu_vf_crops_gekkabijin":
						case "BGodogu_vf_crops_gekkabijinflower":
						case "BGodogu_vf_crops_himawari":
						case "BGodogu_vf_crops_natsumikan":
						case "BGodogu_vf_crops_suika":
						case "BGodogu_vf_crops_zakuro":
						case "BGodogu_villa_table":
						case "BGodogu_villa_tvrimocon":
						case "BGodogu_villabr_sideboard":
						case "BGOdogu_Game_Nei_USB":
						case "BGOdogu_Game_Wanage":
						case "BGOdogu_Game_Wa":
						case "BGodogu_pafe":
						case "BGodogu_furaidopoteto":
						case "BGodogu_karaoketable":
						case "BGodogu_omuriceh":
						case "BGodogu_omuricekao1":
						case "BGodogu_omuricekao2":
						case "BGodogu_omuriceoppai":
						case "BGodogu_kakigori":
						case "BGodogu_pretzel_sara":
						case "BGodogu_karaoke_box":
						case "BGOdogu_alicesoft_bluehoney":
						case "BGOdogu_alicesoft_brownhoney":
						case "BGOdogu_alicesoft_greenhoney":
						case "BGOdogu_alicesoft_redhoney":
						case "BGOdogu_sp001_beercan":
						case "BGOdogu_sp001_beerjug":
						case "BGOdogu_sp001_cake":
						case "BGOdogu_sp001_food":
						case "BGOdogu_sp001_yakisoba":
						case "BGOdogu_denkigai2018s_beachball_blue":
						case "BGOdogu_denkigai2018s_beachball_green":
						case "BGOdogu_denkigai2018s_beachball_red":
						case "BGOdogu_denkigai2018s_beachball_yellow":
						case "BGOdogu_denkigai2018s_coneice_chocomint":
						case "BGOdogu_denkigai2018s_coneice_strawberry":
						case "BGOdogu_denkigai2018s_coneice_vanilla":
						case "BGOdogu_denkigai2018s_melondish":
						case "BGOdogu_denkigai2018s_syatifloot":
						case "BGOdogu_denkigai2018s_toropicalicetea":
						case "BGOdogu_sp002_oharaibou":
						case "BGOdogu_sp002_omamori":
						case "BGOdogu_sp002_susuki":
						case "BGOdogu_sp002_takebouki":
						case "BGOdogu_sp002_tukimidango":
						case "BGOdogu_sp002_waraningyou":
						case "BGOdogu_sp002_waraningyou_gosunkugi":
						case "Odogu_StandMike":
						case "Odogu_StandMikeBase":
						case "Odogu_HeroineChair_muku":
						case "Odogu_HeroineChair_mazime":
						case "Odogu_HeroineChair_rindere":
						case "Odogu_HeroineChair_tsumdere":
						case "Odogu_HeroineChair_cooldere":
						case "Odogu_HeroineChair_junshin":
						case "Odogu_TabletPC":
						case "Odogu_Styluspen_black":
						case "Odogu_Styluspen_white":
						case "Odogu_Styluspen_red":
						case "Odogu_Styluspen_blue":
						case "Odogu_Styluspen_yellow":
						case "Odogu_Styluspen_green":
						case "Odogu_Omurice1":
						case "Odogu_Omurice3":
						case "Odogu_OmuriceH":
						case "Odogu_OmuriceKao1":
						case "Odogu_OmuriceKao2":
						case "Odogu_OmuriceOppai":
						case "Odogu_AcquaPazza":
						case "Odogu_Sandwich":
						case "Odogu_vichyssoise":
						case "Odogu_BirthdayCake":
						case "Odogu_Shortcake":
						case "Odogu_MontBlanc":
						case "Odogu_Pafe":
						case "Odogu_Smoothie_Red":
						case "Odogu_Smoothie_Green":
						case "Odogu_Cocktail_Red":
						case "Odogu_Cocktail_Blue":
						case "Odogu_Cocktail_Yellow":
						case "Odogu_Coffiecup":
						case "Odogu_WineBottle(cap_off)":
						case "Odogu_WineBottle(cap_on)":
						case "Odogu_Jyouro":
						case "Odogu_Planter_Red":
						case "Odogu_Planter_Lightblue":
						case "Odogu_MariGold":
						case "Odogu_CasinoChip_10":
						case "Odogu_CasinoChip_100":
						case "Odogu_CasinoChip_1000":
						case "Odogu_CardShooter":
						case "Odogu_CardsDeck":
						case "Odogu_Card_s1":
						case "Odogu_Card_s2":
						case "Odogu_Card_s3":
						case "Odogu_Card_s4":
						case "Odogu_Card_s5":
						case "Odogu_Card_s6":
						case "Odogu_Card_s7":
						case "Odogu_Card_s8":
						case "Odogu_Card_s9":
						case "Odogu_Card_s10":
						case "Odogu_Card_s11":
						case "Odogu_Card_s12":
						case "Odogu_Card_s13":
						case "Odogu_Card_h1":
						case "Odogu_Card_h2":
						case "Odogu_Card_h3":
						case "Odogu_Card_h4":
						case "Odogu_Card_h5":
						case "Odogu_Card_h6":
						case "Odogu_Card_h7":
						case "Odogu_Card_h8":
						case "Odogu_Card_h9":
						case "Odogu_Card_h10":
						case "Odogu_Card_h11":
						case "Odogu_Card_h12":
						case "Odogu_Card_h13":
						case "Odogu_Card_d1":
						case "Odogu_Card_d2":
						case "Odogu_Card_d3":
						case "Odogu_Card_d4":
						case "Odogu_Card_d5":
						case "Odogu_Card_d6":
						case "Odogu_Card_d7":
						case "Odogu_Card_d8":
						case "Odogu_Card_d9":
						case "Odogu_Card_d10":
						case "Odogu_Card_d11":
						case "Odogu_Card_d12":
						case "Odogu_Card_d13":
						case "Odogu_Card_c1":
						case "Odogu_Card_c2":
						case "Odogu_Card_c3":
						case "Odogu_Card_c4":
						case "Odogu_Card_c5":
						case "Odogu_Card_c6":
						case "Odogu_Card_c7":
						case "Odogu_Card_c8":
						case "Odogu_Card_c9":
						case "Odogu_Card_c10":
						case "Odogu_Card_c11":
						case "Odogu_Card_c12":
						case "Odogu_Card_c13":
						case "Odogu_Card_joker":
							zero.z = 0.5f;
							zero2.x = -90f;
							goto IL_6AE3;
					}
					zero.z = 0.5f;
					if (gameObject.name.StartsWith("Odogu_"))
					{
						foreach (object obj4 in gameObject.transform)
						{
							Transform transform2 = (Transform)obj4;
							if (transform2.GetComponent<Collider>() != null)
							{
								transform2.GetComponent<Collider>().enabled = false;
							}
						}
					}
					else if (gameObject.GetComponent<Collider>() != null)
					{
						gameObject.GetComponent<Collider>().enabled = false;
					}
				IL_6AE3:
					gameObject.transform.localPosition = zero;
					gameObject.transform.localRotation = Quaternion.Euler(zero2);
					this.doguCnt = this.doguBObject.Count - 1;
					this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
					this.gDogu[this.doguCnt].layer = 8;
					this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
					this.gDogu[this.doguCnt].SetActive(false);
					this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
					this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
					this.mDogu[this.doguCnt].isScale = false;
					this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
					this.mDogu[this.doguCnt].maid = gameObject;
					this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
					this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
					this.mDogu[this.doguCnt].ido = 1;
					if (this.doguB2Index == 6 || this.doguB2Index == 7)
					{
						this.mDogu[this.doguCnt].isScale2 = true;
						this.mDogu[this.doguCnt].scale2 = gameObject.transform.localScale;
					}
				}
				GUI.enabled = true;
				if (this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(3), (float)this.GetPix(328), (float)this.GetPix(150), (float)this.GetPix(24)), "デスク", guistyle2);
				if (flag)
				{
					GUI.enabled = false;
				}
				this.parIndex = this.parCombo.List(new Rect((float)this.GetPix(51), (float)this.GetPix(325), (float)this.GetPix(100), (float)this.GetPix(23)), this.parComboList[this.parIndex].text, this.parComboList, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(325), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
				{
					GameObject gameObject = null;
					if (this.parArray[this.parIndex].Contains("#"))
					{
						string[] array = this.parArray[this.parIndex].Split(new char[]
						{
							'#'
						});
						gameObject = GameMain.Instance.BgMgr.CreateAssetBundle(array[1]);
						if (gameObject != null)
						{
							gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
							MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
							for (int l = 0; l < componentsInChildren.Length; l++)
							{
								if (componentsInChildren[l] != null)
								{
									componentsInChildren[l].shadowCastingMode = ShadowCastingMode.Off;
								}
							}
						}
						this.doguBObject.Add(gameObject);
					}
					else if (!this.parArray[this.parIndex].StartsWith("mirror") && this.parArray[this.parIndex].IndexOf(":") < 0)
					{
						UnityEngine.Object @object = Resources.Load("Prefab/" + this.parArray[this.parIndex]);
						gameObject = (UnityEngine.Object.Instantiate(@object) as GameObject);
						this.doguBObject.Add(gameObject);
					}
					else if (this.parArray[this.parIndex].StartsWith("mirror"))
					{
						Material material = new Material(Shader.Find("Mirror"));
						GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
						gameObject2.GetComponent<Renderer>().material = material;
						gameObject2.AddComponent<MirrorReflection2>();
						MirrorReflection2 component = gameObject2.GetComponent<MirrorReflection2>();
						component.m_TextureSize = 2048;
						component.m_ClipPlaneOffset = 0.07f;
						gameObject2.GetComponent<Renderer>().enabled = true;
						gameObject = gameObject2;
						this.doguBObject.Add(gameObject);
					}
					else
					{
						string[] array = this.parArray[this.parIndex].Split(new char[]
						{
							':'
						});
						UnityEngine.Object @object = GameMain.Instance.BgMgr.CreateAssetBundle(array[0]);
						if (@object == null)
						{
							@object = Resources.Load("BG/" + array[0]);
						}
						GameObject gameObject3 = UnityEngine.Object.Instantiate(@object) as GameObject;
						int num6 = 0;
						int num7 = 0;
						int.TryParse(array[1], out num7);
						foreach (object obj5 in gameObject3.transform)
						{
							Transform transform = (Transform)obj5;
							if (num7 == num6)
							{
								gameObject = transform.gameObject;
								break;
							}
							num6++;
						}
						this.doguBObject.Add(gameObject);
						gameObject.transform.parent = null;
						UnityEngine.Object.Destroy(gameObject3);
						gameObject3.SetActive(false);
					}
					gameObject.name = this.parArray[this.parIndex];
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					zero.z = 0.5f;
					zero2.x = -90f;
					gameObject.transform.localPosition = zero;
					gameObject.transform.localRotation = Quaternion.Euler(zero2);
					this.doguCnt = this.doguBObject.Count - 1;
					this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
					this.gDogu[this.doguCnt].layer = 8;
					this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
					this.gDogu[this.doguCnt].SetActive(false);
					this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
					this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
					this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
					this.mDogu[this.doguCnt].maid = gameObject;
					this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
					this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
					this.mDogu[this.doguCnt].ido = 1;
					this.mDogu[this.doguCnt].isScale = false;
					if (gameObject.GetComponent<Collider>() != null)
					{
						gameObject.GetComponent<Collider>().enabled = false;
					}
				}
				GUI.enabled = true;
				if (this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				int k = this.doguBArray.Count - 1;
				while (0 <= k)
				{
					int num8 = this.doguBIndex[k];
					string text4 = this.doguComboList[k][this.doguBIndex[k]].text;
					if (flag)
					{
						GUI.enabled = false;
					}
					if (this.doguCombo[k].isClickedComboButton)
					{
						GUI.enabled = true;
						flag = false;
					}
					this.doguBIndex[k] = this.doguCombo[k].List(new Rect((float)this.GetPix(51), (float)this.GetPix(55 + k * 30), (float)this.GetPix(100), (float)this.GetPix(23)), this.doguComboList[k][this.doguBIndex[k]].text, this.doguComboList[k], guistyle4, "box", this.listStyle3);
					GUI.enabled = true;
					if (GUI.Button(new Rect((float)this.GetPix(156), (float)this.GetPix(55 + k * 30), (float)this.GetPix(38), (float)this.GetPix(23)), "追加", guistyle3))
					{
						GameObject gameObject = null;
						string text2;
						if (!this.doguBArray[k][this.doguBIndex[k]].StartsWith("mirror"))
						{
							UnityEngine.Object @object;
							if (this.doguBArray[k][this.doguBIndex[k]].StartsWith("BG"))
							{
								text2 = this.doguBArray[k][this.doguBIndex[k]].Replace("BG", "");
								@object = GameMain.Instance.BgMgr.CreateAssetBundle(text2);
								if (@object == null)
								{
									@object = Resources.Load("BG/" + text2);
								}
							}
							else
							{
								@object = Resources.Load("Prefab/" + this.doguBArray[k][this.doguBIndex[k]]);
								if (@object == null)
								{
									GameObject original = GameMain.Instance.BgMgr.CreateAssetBundle(this.doguBArray[k][this.doguBIndex[k]]);
									gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
								}
							}
							if (gameObject == null)
							{
								gameObject = (UnityEngine.Object.Instantiate(@object) as GameObject);
							}
							this.doguBObject.Add(gameObject);
						}
						else
						{
							Material material = new Material(Shader.Find("Mirror"));
							GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
							gameObject2.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
							gameObject2.GetComponent<Renderer>().material = material;
							gameObject2.AddComponent<MirrorReflection2>();
							MirrorReflection2 component = gameObject2.GetComponent<MirrorReflection2>();
							component.m_TextureSize = 2048;
							component.m_ClipPlaneOffset = 0f;
							gameObject2.GetComponent<Renderer>().enabled = true;
							gameObject = gameObject2;
							this.doguBObject.Add(gameObject);
						}
						gameObject.name = this.doguBArray[k][this.doguBIndex[k]];
						Vector3 zero = Vector3.zero;
						Vector3 zero2 = Vector3.zero;
						string text = gameObject.name;
						if (text == null)
						{
							goto IL_8500;
						}
						//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-6 == null)
						if (OdoguUIArray == null)
						{
							//<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-6 = new Dictionary<string, int>(173)
							OdoguUIArray = new Dictionary<string, int>(173)
							{
								{
									"Odogu_XmasTreeMini_photo_ver",
									0
								},
								{
									"Odogu_KadomatsuMini_photo_ver",
									1
								},
								{
									"Odogu_ClassRoomDesk",
									2
								},
								{
									"Odogu_SimpleTable",
									3
								},
								{
									"Odogu_DildoBox",
									4
								},
								{
									"PlayAreaOut",
									5
								},
								{
									"Odogu_Dresser_photo_ver",
									6
								},
								{
									"BGodogu_bbqgrill",
									7
								},
								{
									"BGodogu_bucket",
									8
								},
								{
									"BGodogu_coolerbox",
									9
								},
								{
									"BGodogu_game_darts",
									10
								},
								{
									"BGodogu_game_dartsboard",
									11
								},
								{
									"BGodogu_nabe_huta",
									12
								},
								{
									"BGodogu_nabe_water",
									13
								},
								{
									"BGodogu_natumikan",
									14
								},
								{
									"BGodogu_rb_chair",
									15
								},
								{
									"BGodogu_rb_duck",
									16
								},
								{
									"BGodogu_rb_obon",
									17
								},
								{
									"BGodogu_rb_tokkuri",
									18
								},
								{
									"BGodogu_saracorn",
									19
								},
								{
									"BGodogu_saraimo",
									20
								},
								{
									"BGodogu_saratomato",
									21
								},
								{
									"BGodogu_sunanoshiro",
									22
								},
								{
									"BGodogu_sunanoyama",
									23
								},
								{
									"BGodogu_tsutsuhanabi",
									24
								},
								{
									"BGodogu_ukiwa",
									25
								},
								{
									"BGodogu_vf_crops_corn",
									26
								},
								{
									"BGodogu_vf_crops_gekkabijin",
									27
								},
								{
									"BGodogu_vf_crops_gekkabijinflower",
									28
								},
								{
									"BGodogu_vf_crops_himawari",
									29
								},
								{
									"BGodogu_vf_crops_natsumikan",
									30
								},
								{
									"BGodogu_vf_crops_suika",
									31
								},
								{
									"BGodogu_vf_crops_zakuro",
									32
								},
								{
									"BGodogu_villa_table",
									33
								},
								{
									"BGodogu_villa_tvrimocon",
									34
								},
								{
									"BGodogu_villabr_sideboard",
									35
								},
								{
									"BGOdogu_Game_Nei_USB",
									36
								},
								{
									"BGOdogu_Game_Wanage",
									37
								},
								{
									"BGOdogu_Game_Wa",
									38
								},
								{
									"BGodogu_pafe",
									39
								},
								{
									"BGodogu_furaidopoteto",
									40
								},
								{
									"BGodogu_karaoketable",
									41
								},
								{
									"BGodogu_omuriceh",
									42
								},
								{
									"BGodogu_omuricekao1",
									43
								},
								{
									"BGodogu_omuricekao2",
									44
								},
								{
									"BGodogu_omuriceoppai",
									45
								},
								{
									"BGodogu_kakigori",
									46
								},
								{
									"BGodogu_pretzel_sara",
									47
								},
								{
									"BGodogu_karaoke_box",
									48
								},
								{
									"BGOdogu_alicesoft_bluehoney",
									49
								},
								{
									"BGOdogu_alicesoft_brownhoney",
									50
								},
								{
									"BGOdogu_alicesoft_greenhoney",
									51
								},
								{
									"BGOdogu_alicesoft_redhoney",
									52
								},
								{
									"BGOdogu_sp001_beercan",
									53
								},
								{
									"BGOdogu_sp001_beerjug",
									54
								},
								{
									"BGOdogu_sp001_cake",
									55
								},
								{
									"BGOdogu_sp001_food",
									56
								},
								{
									"BGOdogu_sp001_yakisoba",
									57
								},
								{
									"BGOdogu_denkigai2018s_beachball_blue",
									58
								},
								{
									"BGOdogu_denkigai2018s_beachball_green",
									59
								},
								{
									"BGOdogu_denkigai2018s_beachball_red",
									60
								},
								{
									"BGOdogu_denkigai2018s_beachball_yellow",
									61
								},
								{
									"BGOdogu_denkigai2018s_coneice_chocomint",
									62
								},
								{
									"BGOdogu_denkigai2018s_coneice_strawberry",
									63
								},
								{
									"BGOdogu_denkigai2018s_coneice_vanilla",
									64
								},
								{
									"BGOdogu_denkigai2018s_melondish",
									65
								},
								{
									"BGOdogu_denkigai2018s_syatifloot",
									66
								},
								{
									"BGOdogu_denkigai2018s_toropicalicetea",
									67
								},
								{
									"BGOdogu_sp002_oharaibou",
									68
								},
								{
									"BGOdogu_sp002_omamori",
									69
								},
								{
									"BGOdogu_sp002_susuki",
									70
								},
								{
									"BGOdogu_sp002_takebouki",
									71
								},
								{
									"BGOdogu_sp002_tukimidango",
									72
								},
								{
									"BGOdogu_sp002_waraningyou",
									73
								},
								{
									"BGOdogu_sp002_waraningyou_gosunkugi",
									74
								},
								{
									"Odogu_StandMike",
									75
								},
								{
									"Odogu_StandMikeBase",
									76
								},
								{
									"Odogu_HeroineChair_muku",
									77
								},
								{
									"Odogu_HeroineChair_mazime",
									78
								},
								{
									"Odogu_HeroineChair_rindere",
									79
								},
								{
									"Odogu_HeroineChair_tsumdere",
									80
								},
								{
									"Odogu_HeroineChair_cooldere",
									81
								},
								{
									"Odogu_HeroineChair_junshin",
									82
								},
								{
									"Odogu_TabletPC",
									83
								},
								{
									"Odogu_Styluspen_black",
									84
								},
								{
									"Odogu_Styluspen_white",
									85
								},
								{
									"Odogu_Styluspen_red",
									86
								},
								{
									"Odogu_Styluspen_blue",
									87
								},
								{
									"Odogu_Styluspen_yellow",
									88
								},
								{
									"Odogu_Styluspen_green",
									89
								},
								{
									"Odogu_Omurice1",
									90
								},
								{
									"Odogu_Omurice3",
									91
								},
								{
									"Odogu_OmuriceH",
									92
								},
								{
									"Odogu_OmuriceKao1",
									93
								},
								{
									"Odogu_OmuriceKao2",
									94
								},
								{
									"Odogu_OmuriceOppai",
									95
								},
								{
									"Odogu_AcquaPazza",
									96
								},
								{
									"Odogu_Sandwich",
									97
								},
								{
									"Odogu_vichyssoise",
									98
								},
								{
									"Odogu_BirthdayCake",
									99
								},
								{
									"Odogu_Shortcake",
									100
								},
								{
									"Odogu_MontBlanc",
									101
								},
								{
									"Odogu_Pafe",
									102
								},
								{
									"Odogu_Smoothie_Red",
									103
								},
								{
									"Odogu_Smoothie_Green",
									104
								},
								{
									"Odogu_Cocktail_Red",
									105
								},
								{
									"Odogu_Cocktail_Blue",
									106
								},
								{
									"Odogu_Cocktail_Yellow",
									107
								},
								{
									"Odogu_Coffiecup",
									108
								},
								{
									"Odogu_WineBottle(cap_off)",
									109
								},
								{
									"Odogu_WineBottle(cap_on)",
									110
								},
								{
									"Odogu_Jyouro",
									111
								},
								{
									"Odogu_Planter_Red",
									112
								},
								{
									"Odogu_Planter_Lightblue",
									113
								},
								{
									"Odogu_MariGold",
									114
								},
								{
									"Odogu_CasinoChip_10",
									115
								},
								{
									"Odogu_CasinoChip_100",
									116
								},
								{
									"Odogu_CasinoChip_1000",
									117
								},
								{
									"Odogu_CardShooter",
									118
								},
								{
									"Odogu_CardsDeck",
									119
								},
								{
									"Odogu_Card_s1",
									120
								},
								{
									"Odogu_Card_s2",
									121
								},
								{
									"Odogu_Card_s3",
									122
								},
								{
									"Odogu_Card_s4",
									123
								},
								{
									"Odogu_Card_s5",
									124
								},
								{
									"Odogu_Card_s6",
									125
								},
								{
									"Odogu_Card_s7",
									126
								},
								{
									"Odogu_Card_s8",
									127
								},
								{
									"Odogu_Card_s9",
									128
								},
								{
									"Odogu_Card_s10",
									129
								},
								{
									"Odogu_Card_s11",
									130
								},
								{
									"Odogu_Card_s12",
									131
								},
								{
									"Odogu_Card_s13",
									132
								},
								{
									"Odogu_Card_h1",
									133
								},
								{
									"Odogu_Card_h2",
									134
								},
								{
									"Odogu_Card_h3",
									135
								},
								{
									"Odogu_Card_h4",
									136
								},
								{
									"Odogu_Card_h5",
									137
								},
								{
									"Odogu_Card_h6",
									138
								},
								{
									"Odogu_Card_h7",
									139
								},
								{
									"Odogu_Card_h8",
									140
								},
								{
									"Odogu_Card_h9",
									141
								},
								{
									"Odogu_Card_h10",
									142
								},
								{
									"Odogu_Card_h11",
									143
								},
								{
									"Odogu_Card_h12",
									144
								},
								{
									"Odogu_Card_h13",
									145
								},
								{
									"Odogu_Card_d1",
									146
								},
								{
									"Odogu_Card_d2",
									147
								},
								{
									"Odogu_Card_d3",
									148
								},
								{
									"Odogu_Card_d4",
									149
								},
								{
									"Odogu_Card_d5",
									150
								},
								{
									"Odogu_Card_d6",
									151
								},
								{
									"Odogu_Card_d7",
									152
								},
								{
									"Odogu_Card_d8",
									153
								},
								{
									"Odogu_Card_d9",
									154
								},
								{
									"Odogu_Card_d10",
									155
								},
								{
									"Odogu_Card_d11",
									156
								},
								{
									"Odogu_Card_d12",
									157
								},
								{
									"Odogu_Card_d13",
									158
								},
								{
									"Odogu_Card_c1",
									159
								},
								{
									"Odogu_Card_c2",
									160
								},
								{
									"Odogu_Card_c3",
									161
								},
								{
									"Odogu_Card_c4",
									162
								},
								{
									"Odogu_Card_c5",
									163
								},
								{
									"Odogu_Card_c6",
									164
								},
								{
									"Odogu_Card_c7",
									165
								},
								{
									"Odogu_Card_c8",
									166
								},
								{
									"Odogu_Card_c9",
									167
								},
								{
									"Odogu_Card_c10",
									168
								},
								{
									"Odogu_Card_c11",
									169
								},
								{
									"Odogu_Card_c12",
									170
								},
								{
									"Odogu_Card_c13",
									171
								},
								{
									"Odogu_Card_joker",
									172
								}
							};
						}
						int num;
						//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000014-6.TryGetValue(text, out num))
						//{
						//	goto IL_8500;
						//}
						if (!OdoguUIArray.TryGetValue(text, out num))
						{
							goto IL_8500;
						}
						switch (num)
						{
							case 0:
								zero.z = 0.6f;
								gameObject.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
								foreach (object obj6 in gameObject.transform)
								{
									Transform transform2 = (Transform)obj6;
									if (transform2.GetComponent<Collider>() != null)
									{
										transform2.GetComponent<Collider>().enabled = false;
									}
								}
								break;
							case 1:
								zero.z = 0.6f;
								gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
								foreach (object obj7 in gameObject.transform)
								{
									Transform transform2 = (Transform)obj7;
									if (transform2.GetComponent<Collider>() != null)
									{
										transform2.GetComponent<Collider>().enabled = false;
									}
								}
								break;
							case 2:
								zero.z = 0.5f;
								zero2.x = -90f;
								break;
							case 3:
								zero.z = 0.5f;
								zero2.x = -90f;
								break;
							case 4:
								zero.z = 0.5f;
								zero2.x = -90f;
								break;
							case 5:
								zero.z = 0.5f;
								zero.y = 0.2f;
								break;
							case 6:
								GameObject.Find("Prim.00000000").GetComponent<Collider>().enabled = false;
								GameObject.Find("Prim.00000001").GetComponent<Collider>().enabled = false;
								GameObject.Find("Prim.00000002").GetComponent<Collider>().enabled = false;
								GameObject.Find("Prim.00000004").GetComponent<Collider>().enabled = false;
								break;
							case 7:
							case 8:
							case 9:
							case 10:
							case 11:
							case 12:
							case 13:
							case 14:
							case 15:
							case 16:
							case 17:
							case 18:
							case 19:
							case 20:
							case 21:
							case 22:
							case 23:
							case 24:
							case 25:
							case 26:
							case 27:
							case 28:
							case 29:
							case 30:
							case 31:
							case 32:
							case 33:
							case 34:
							case 35:
							case 36:
							case 37:
							case 38:
							case 39:
							case 40:
							case 41:
							case 42:
							case 43:
							case 44:
							case 45:
							case 46:
							case 47:
							case 48:
							case 49:
							case 50:
							case 51:
							case 52:
							case 53:
							case 54:
							case 55:
							case 56:
							case 57:
							case 58:
							case 59:
							case 60:
							case 61:
							case 62:
							case 63:
							case 64:
							case 65:
							case 66:
							case 67:
							case 68:
							case 69:
							case 70:
							case 71:
							case 72:
							case 73:
							case 74:
							case 75:
							case 76:
							case 77:
							case 78:
							case 79:
							case 80:
							case 81:
							case 82:
							case 83:
							case 84:
							case 85:
							case 86:
							case 87:
							case 88:
							case 89:
							case 90:
							case 91:
							case 92:
							case 93:
							case 94:
							case 95:
							case 96:
							case 97:
							case 98:
							case 99:
							case 100:
							case 101:
							case 102:
							case 103:
							case 104:
							case 105:
							case 106:
							case 107:
							case 108:
							case 109:
							case 110:
							case 111:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
							case 122:
							case 123:
							case 124:
							case 125:
							case 126:
							case 127:
							case 128:
							case 129:
							case 130:
							case 131:
							case 132:
							case 133:
							case 134:
							case 135:
							case 136:
							case 137:
							case 138:
							case 139:
							case 140:
							case 141:
							case 142:
							case 143:
							case 144:
							case 145:
							case 146:
							case 147:
							case 148:
							case 149:
							case 150:
							case 151:
							case 152:
							case 153:
							case 154:
							case 155:
							case 156:
							case 157:
							case 158:
							case 159:
							case 160:
							case 161:
							case 162:
							case 163:
							case 164:
							case 165:
							case 166:
							case 167:
							case 168:
							case 169:
							case 170:
							case 171:
							case 172:
								zero.z = 0.5f;
								zero2.x = -90f;
								break;
							default:
								goto IL_8500;
						}
					IL_865E:
						gameObject.transform.localPosition = zero;
						gameObject.transform.localRotation = Quaternion.Euler(zero2);
						this.doguCnt = this.doguBObject.Count - 1;
						this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
						this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
						this.gDogu[this.doguCnt].layer = 8;
						this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
						this.gDogu[this.doguCnt].SetActive(false);
						this.gDogu[this.doguCnt].transform.position = gameObject.transform.position;
						this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
						this.mDogu[this.doguCnt].isScale = false;
						this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
						this.mDogu[this.doguCnt].maid = gameObject;
						this.mDogu[this.doguCnt].angles = gameObject.transform.eulerAngles;
						this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
						this.mDogu[this.doguCnt].ido = 1;
						if (this.doguBIndex[k] == 6 || this.doguBIndex[k] == 7)
						{
							this.mDogu[this.doguCnt].isScale2 = true;
							this.mDogu[this.doguCnt].scale2 = gameObject.transform.localScale;
						}
						goto IL_8847;
					IL_8500:
						text2 = gameObject.name;
						bool flag4 = false;
						for (int j = 0; j < this.doguNameList.Count; j++)
						{
							string[] array = this.doguNameList[j].Split(new char[]
							{
								','
							});
							if (text2 == array[0])
							{
								flag4 = true;
							}
						}
						if (flag4)
						{
							zero.z = 0.5f;
							zero2.x = -90f;
							goto IL_865E;
						}
						zero.z = 0.5f;
						if (gameObject.name.StartsWith("Odogu_"))
						{
							foreach (object obj8 in gameObject.transform)
							{
								Transform transform2 = (Transform)obj8;
								if (transform2.GetComponent<Collider>() != null)
								{
									transform2.GetComponent<Collider>().enabled = false;
								}
							}
						}
						else if (gameObject.GetComponent<Collider>() != null)
						{
							gameObject.GetComponent<Collider>().enabled = false;
						}
						goto IL_865E;
					}
				IL_8847:
					k--;
				}
			}
			GUI.enabled = true;
			if (this.doguSelectFlg3)
			{
				bool flag5 = GUI.Toggle(new Rect((float)this.GetPix(12), (float)this.GetPix(52), (float)this.GetPix(50), (float)this.GetPix(20)), this.modFlg, "MOD", guistyle6);
				if (flag5 != this.modFlg)
				{
					this.modFlg = true;
					this.nmodFlg = false;
					this.itemDataList = new List<MultipleMaids.ItemData>(this.itemDataListMod);
				}
				bool flag6 = GUI.Toggle(new Rect((float)this.GetPix(82), (float)this.GetPix(52), (float)this.GetPix(39), (float)this.GetPix(20)), this.nmodFlg, "公式", guistyle6);
				if (flag6 != this.nmodFlg)
				{
					this.modFlg = false;
					this.nmodFlg = true;
					this.itemDataList = new List<MultipleMaids.ItemData>(this.itemDataListNMod);
				}
				int num9 = this.slotCombo.List(new Rect((float)this.GetPix(51), (float)this.GetPix(81), (float)this.GetPix(100), (float)this.GetPix(23)), this.slotComboList[this.slotIndex].text, this.slotComboList, guistyle4, "box", this.listStyle3);
				if (num9 != this.slotIndex)
				{
					this.slotIndex = num9;
					this.sortList.Clear();
					this.scrollPos = new Vector2(0f, 0f);
					if (this.itemDataList.Count == 0)
					{
						string[] fileListAtExtension;
						if (this.modFlg)
						{
							fileListAtExtension = GameUty.FileSystemMod.GetFileListAtExtension(".menu");
						}
						else
						{
							fileListAtExtension = GameUty.FileSystem.GetFileListAtExtension(".menu");
						}
						foreach (string path in fileListAtExtension)
						{
							string text5 = Path.GetFileNameWithoutExtension(path) + ".menu";
							byte[] array2 = null;
							using (AFileBase afileBase = GameUty.FileOpen(text5, null))
							{
								NDebug.Assert(afileBase.IsValid(), "メニューファイルが存在しません。 :" + text5);
								array2 = new byte[afileBase.GetSize()];
								afileBase.Read(ref array2, afileBase.GetSize());
							}
							BinaryReader binaryReader = new BinaryReader(new MemoryStream(array2), Encoding.UTF8);
							if (binaryReader.ReadString() != "CM3D2_MENU")
							{
								binaryReader.Close();
							}
							else
							{
								binaryReader.ReadInt32();
								binaryReader.ReadString();
								string text6 = binaryReader.ReadString();
								string info = binaryReader.ReadString();
								string s = "";
								binaryReader.ReadString();
								binaryReader.ReadInt32();
								string text7 = "";
								try
								{
									for (; ; )
									{
										int num10 = (int)binaryReader.ReadByte();
										if (num10 != 0)
										{
											for (int n = 0; n < num10; n++)
											{
												string a = binaryReader.ReadString();
												if (a == "icons")
												{
													text7 = binaryReader.ReadString();
													break;
												}
												if (a == "priority")
												{
													s = binaryReader.ReadString();
													break;
												}
											}
											if (text7 != "")
											{
												break;
											}
										}
									}
									int order = 0;
									int.TryParse(s, out order);
									MultipleMaids.ItemData itemData = new MultipleMaids.ItemData();
									itemData.info = info;
									itemData.name = text7;
									itemData.menu = text5;
									itemData.order = order;
									itemData.cd = array2;
									this.itemDataList.Add(itemData);
								}
								catch
								{
								}
								binaryReader.Close();
							}
						}
						if (this.modFlg)
						{
							this.itemDataListMod = new List<MultipleMaids.ItemData>(this.itemDataList);
						}
						else
						{
							this.itemDataListNMod = new List<MultipleMaids.ItemData>(this.itemDataList);
						}
					}
					foreach (MultipleMaids.ItemData itemData in this.itemDataList)
					{
						if (this.slotIndex != 0 && !(itemData.info.ToLower() != this.slotArray[this.slotIndex]))
						{
							if (itemData.order > 0)
							{
								MultipleMaids.SortItem sortItem = new MultipleMaids.SortItem();
								sortItem.order = itemData.order;
								sortItem.name = itemData.name;
								sortItem.menu = itemData.menu;
								sortItem.tex = itemData.tex;
								this.sortList.Add(sortItem);
							}
						}
					}
					IOrderedEnumerable<MultipleMaids.SortItem> orderedEnumerable = from p in this.sortList
																				   orderby p.order, p.name
																				   select p;
					List<MultipleMaids.SortItem> list = new List<MultipleMaids.SortItem>();
					string b = "";
					foreach (MultipleMaids.SortItem sortItem in orderedEnumerable)
					{
						try
						{
							if (!(sortItem.menu == b))
							{
								if (sortItem.tex == null)
								{
									byte[] data2 = ImportCM.LoadTexture(GameUty.FileSystem, sortItem.name, false).data;
									Texture2D texture2D = new Texture2D(50, 50, TextureFormat.RGB565, false);
									texture2D.LoadImage(data2);
									sortItem.tex = texture2D;
								}
								b = sortItem.menu;
								list.Add(sortItem);
							}
						}
						catch
						{
						}
					}
					this.sortList = list;
				}
			}
		}

		private void GuiFunc6(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "label";
			guistyle2.fontSize = this.GetPix(11);
			guistyle2.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle3 = "button";
			guistyle3.fontSize = this.GetPix(20);
			GUIStyle guistyle4 = "button";
			guistyle4.fontSize = this.GetPix(14);
			guistyle4.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle5 = "button";
			guistyle5.fontSize = this.GetPix(12);
			guistyle5.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle6 = new GUIStyle("toggle");
			guistyle6.fontSize = this.GetPix(13);
			CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
			Maid maid = this.maidArray[this.selectMaidIndex];
			if (!this.kankyoInitFlg)
			{
				this.listStyle2.normal.textColor = Color.white;
				this.listStyle2.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle2.onHover.background = (this.listStyle2.hover.background = new Texture2D(2, 2));
				RectOffset padding = this.listStyle2.padding;
				RectOffset padding2 = this.listStyle2.padding;
				RectOffset padding3 = this.listStyle2.padding;
				int num = this.listStyle2.padding.bottom = this.GetPix(0);
				num = (padding3.top = num);
				num = (padding2.right = num);
				padding.left = num;
				this.listStyle2.fontSize = this.GetPix(11);
				this.listStyle3.normal.textColor = Color.white;
				this.listStyle3.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle3.onHover.background = (this.listStyle3.hover.background = new Texture2D(2, 2));
				RectOffset padding4 = this.listStyle3.padding;
				RectOffset padding5 = this.listStyle3.padding;
				num = (this.listStyle3.padding.top = this.GetPix(0));
				num = (padding5.right = num);
				padding4.left = num;
				this.listStyle3.padding.bottom = this.GetPix(0);
				this.listStyle3.fontSize = this.GetPix(12);
				this.bgmCombo.selectedItemIndex = this.bgmIndex;
				if (this.sceneLevel == 5)
				{
					this.bgmCombo.selectedItemIndex = 2;
				}
				this.bgmComboList = new GUIContent[this.bgmArray.Length];
				int i = 0;
				while (i < this.bgmArray.Length)
				{
					string text = this.bgmArray[i];
					if (text == null)
					{
						goto IL_501;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-1 == null)
					if (DanceArray == null)
					{
						//<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-1 = new Dictionary<string, int>(11)
						DanceArray = new Dictionary<string, int>(11)
						{
							{
								"dokidokifallinlove_short",
								0
							},
							{
								"entrancetoyou_short",
								1
							},
							{
								"scarlet leap_short",
								2
							},
							{
								"stellarmytears_short",
								3
							},
							{
								"stellarmytears_short2",
								4
							},
							{
								"stellarmytears_short3",
								5
							},
							{
								"RhythmixToYou",
								6
							},
							{
								"happy_happy_scandal1",
								7
							},
							{
								"happy_happy_scandal2",
								8
							},
							{
								"happy_happy_scandal3",
								9
							},
							{
								"can_know_two_close",
								10
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-1.TryGetValue(text, out num))
					//{
					//	goto IL_501;
					//}
					if (!DanceArray.TryGetValue(text, out num))
					{
						goto IL_501;
					}
					switch (num)
					{
						case 0:
							this.bgmComboList[i] = new GUIContent("ドキドキ☆Fallin' Love");
							break;
						case 1:
							this.bgmComboList[i] = new GUIContent("entrance to you");
							break;
						case 2:
							this.bgmComboList[i] = new GUIContent("scarlet leap");
							break;
						case 3:
							this.bgmComboList[i] = new GUIContent("stellar my tears1");
							break;
						case 4:
							this.bgmComboList[i] = new GUIContent("stellar my tears2");
							break;
						case 5:
							this.bgmComboList[i] = new GUIContent("stellar my tears3");
							break;
						case 6:
							this.bgmComboList[i] = new GUIContent("rhythmix to you");
							break;
						case 7:
							this.bgmComboList[i] = new GUIContent("happy!happy!スキャンダル!! 1");
							break;
						case 8:
							this.bgmComboList[i] = new GUIContent("happy!happy!スキャンダル!! 2");
							break;
						case 9:
							this.bgmComboList[i] = new GUIContent("happy!happy!スキャンダル!! 3");
							break;
						case 10:
							this.bgmComboList[i] = new GUIContent("Can Know Two Close");
							break;
						default:
							goto IL_501;
					}
				IL_51A:
					i++;
					continue;
				IL_501:
					this.bgmComboList[i] = new GUIContent(this.bgmArray[i]);
					goto IL_51A;
				}
				this.bgCombo.selectedItemIndex = this.bgIndex;
				this.bgComboList = new GUIContent[this.bgArray.Length];
				i = 0;
				while (i < this.bgArray.Length)
				{
					string text = this.bgArray[i];
					if (text == null)
					{
						goto IL_18DC;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-2 == null)
					if (bgUiArray == null)
					{
						bgUiArray = new Dictionary<string, int>(120)
						{
							{
								"Salon",
								0
							},
							{
								"Syosai",
								1
							},
							{
								"Syosai_Night",
								2
							},
							{
								"DressRoom_NoMirror",
								3
							},
							{
								"MyBedRoom",
								4
							},
							{
								"MyBedRoom_Night",
								5
							},
							{
								"MyBedRoom_NightOff",
								6
							},
							{
								"Bathroom",
								7
							},
							{
								"PlayRoom",
								8
							},
							{
								"Pool",
								9
							},
							{
								"SMRoom",
								10
							},
							{
								"PlayRoom2",
								11
							},
							{
								"Salon_Garden",
								12
							},
							{
								"LargeBathRoom",
								13
							},
							{
								"MaidRoom",
								14
							},
							{
								"OiranRoom",
								15
							},
							{
								"Penthouse",
								16
							},
							{
								"Town",
								17
							},
							{
								"Kitchen",
								18
							},
							{
								"Kitchen_Night",
								19
							},
							{
								"Shitsumu",
								20
							},
							{
								"Shitsumu_Night",
								21
							},
							{
								"Salon_Entrance",
								22
							},
							{
								"Bar",
								23
							},
							{
								"Toilet",
								24
							},
							{
								"Train",
								25
							},
							{
								"SMRoom2",
								26
							},
							{
								"LockerRoom",
								27
							},
							{
								"Oheya",
								28
							},
							{
								"Salon_Day",
								29
							},
							{
								"ClassRoom",
								30
							},
							{
								"ClassRoom_Play",
								31
							},
							{
								"HoneymoonRoom",
								32
							},
							{
								"OutletPark",
								33
							},
							{
								"BigSight",
								34
							},
							{
								"BigSight_Night",
								35
							},
							{
								"PrivateRoom",
								36
							},
							{
								"PrivateRoom_Night",
								37
							},
							{
								"Sea",
								38
							},
							{
								"Sea_Night",
								39
							},
							{
								"Yashiki",
								40
							},
							{
								"Yashiki_Day",
								41
							},
							{
								"Yashiki_Pillow",
								42
							},
							{
								"rotenburo",
								43
							},
							{
								"rotenburo_night",
								44
							},
							{
								"villa",
								45
							},
							{
								"villa_night",
								46
							},
							{
								"villa_bedroom",
								47
							},
							{
								"villa_bedroom_night",
								48
							},
							{
								"villa_farm",
								49
							},
							{
								"villa_farm_night",
								50
							},
							{
								"karaokeroom",
								51
							},
							{
								"Theater",
								52
							},
							{
								"Theater_LightOff",
								53
							},
							{
								"LiveStage",
								54
							},
							{
								"LiveStage_Side",
								55
							},
							{
								"LiveStage_use_dance",
								56
							},
							{
								"BackStage",
								57
							},
							{
								"DanceRoom",
								58
							},
							{
								"EmpireClub_Rotary",
								59
							},
							{
								"EmpireClub_Rotary_Night",
								60
							},
							{
								"EmpireClub_Entrance",
								61
							},
							{
								"ShinShitsumu",
								62
							},
							{
								"ShinShitsumu_ChairRot",
								63
							},
							{
								"ShinShitsumu_Night",
								64
							},
							{
								"MyRoom",
								65
							},
							{
								"MyRoom_Night",
								66
							},
							{
								"OpemCafe",
								67
							},
							{
								"OpemCafe_Night",
								68
							},
							{
								"Restaurant",
								69
							},
							{
								"Restaurant_Night",
								70
							},
							{
								"MainKitchen",
								71
							},
							{
								"MainKitchen_Night",
								72
							},
							{
								"MainKitchen_LightOff",
								73
							},
							{
								"BarLounge",
								74
							},
							{
								"Casino",
								75
							},
							{
								"CasinoMini",
								76
							},
							{
								"SMClub",
								77
							},
							{
								"Soap",
								78
							},
							{
								"Spa",
								79
							},
							{
								"Spa_Night",
								80
							},
							{
								"ShoppingMall",
								81
							},
							{
								"ShoppingMall_Night",
								82
							},
							{
								"GameShop",
								83
							},
							{
								"MusicShop",
								84
							},
							{
								"HeroineRoom_A1",
								85
							},
							{
								"HeroineRoom_A1_Night",
								86
							},
							{
								"HeroineRoom_B1",
								87
							},
							{
								"HeroineRoom_B1_Night",
								88
							},
							{
								"HeroineRoom_C1",
								89
							},
							{
								"HeroineRoom_C1_Night",
								90
							},
							{
								"HeroineRoom_A",
								91
							},
							{
								"HeroineRoom_A_Night",
								92
							},
							{
								"HeroineRoom_B",
								93
							},
							{
								"HeroineRoom_B_Night",
								94
							},
							{
								"HeroineRoom_C",
								95
							},
							{
								"HeroineRoom_C_Night",
								96
							},
							{
								"Shukuhakubeya_BedRoom",
								97
							},
							{
								"Shukuhakubeya_BedRoom_Night",
								98
							},
							{
								"Shukuhakubeya_Other_BedRoom",
								99
							},
							{
								"Shukuhakubeya_Living",
								100
							},
							{
								"Shukuhakubeya_Living_Night",
								101
							},
							{
								"Shukuhakubeya_Toilet",
								102
							},
							{
								"Shukuhakubeya_Toilet_Night",
								103
							},
							{
								"Shukuhakubeya_WashRoom",
								104
							},
							{
								"Shukuhakubeya_WashRoom_Night",
								105
							},
							{
								"opemcafe_rance10",
								106
							},
							{
								"opemcafe_rance10_night",
								107
							},
							{
								"opemcafe_riddlejoker",
								108
							},
							{
								"opemcafe_riddlejoker_night",
								109
							},
							{
								"opemcafe_wanko",
								110
							},
							{
								"opemcafe_wanko_night",
								111
							},
							{
								"opemcafe_raspberry",
								112
							},
							{
								"opemcafe_raspberry_night",
								113
							},
							{
								"seacafe",
								114
							},
							{
								"seacafe_night",
								115
							},
							{
								"com3d2pool",
								116
							},
							{
								"com3d2pool_night",
								117
							},
							{
								"shrine",
								118
							},
							{
								"shrine_night",
								119
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-2.TryGetValue(text, out num))
					//{
					//	goto IL_18DC;
					//}
					if (!bgUiArray.TryGetValue(text, out num))
					{
						goto IL_18DC;
					}
					switch (num)
					{
						case 0:
							this.bgComboList[i] = new GUIContent("サロン");
							break;
						case 1:
							this.bgComboList[i] = new GUIContent("書斎");
							break;
						case 2:
							this.bgComboList[i] = new GUIContent("書斎(夜)");
							break;
						case 3:
							this.bgComboList[i] = new GUIContent("ドレスルーム");
							break;
						case 4:
							this.bgComboList[i] = new GUIContent("自室");
							break;
						case 5:
							this.bgComboList[i] = new GUIContent("自室(夜)");
							break;
						case 6:
							this.bgComboList[i] = new GUIContent("自室(消灯)");
							break;
						case 7:
							this.bgComboList[i] = new GUIContent("風呂");
							break;
						case 8:
							this.bgComboList[i] = new GUIContent("プレイルーム");
							break;
						case 9:
							this.bgComboList[i] = new GUIContent("プール");
							break;
						case 10:
							this.bgComboList[i] = new GUIContent("SMルーム");
							break;
						case 11:
							this.bgComboList[i] = new GUIContent("プレイルーム2");
							break;
						case 12:
							this.bgComboList[i] = new GUIContent("サロン(中庭)");
							break;
						case 13:
							this.bgComboList[i] = new GUIContent("大浴場");
							break;
						case 14:
							this.bgComboList[i] = new GUIContent("メイド部屋");
							break;
						case 15:
							this.bgComboList[i] = new GUIContent("花魁ルーム");
							break;
						case 16:
							this.bgComboList[i] = new GUIContent("ペントハウス");
							break;
						case 17:
							this.bgComboList[i] = new GUIContent("街");
							break;
						case 18:
							this.bgComboList[i] = new GUIContent("キッチン");
							break;
						case 19:
							this.bgComboList[i] = new GUIContent("キッチン(夜)");
							break;
						case 20:
							this.bgComboList[i] = new GUIContent("執務室");
							break;
						case 21:
							this.bgComboList[i] = new GUIContent("執務室(夜)");
							break;
						case 22:
							this.bgComboList[i] = new GUIContent("エントランス");
							break;
						case 23:
							this.bgComboList[i] = new GUIContent("バー");
							break;
						case 24:
							this.bgComboList[i] = new GUIContent("トイレ");
							break;
						case 25:
							this.bgComboList[i] = new GUIContent("電車");
							break;
						case 26:
							this.bgComboList[i] = new GUIContent("地下室");
							break;
						case 27:
							this.bgComboList[i] = new GUIContent("ロッカールーム");
							break;
						case 28:
							this.bgComboList[i] = new GUIContent("四畳半部屋");
							break;
						case 29:
							this.bgComboList[i] = new GUIContent("サロン(昼)");
							break;
						case 30:
							this.bgComboList[i] = new GUIContent("教室");
							break;
						case 31:
							this.bgComboList[i] = new GUIContent("教室(夜伽)");
							break;
						case 32:
							this.bgComboList[i] = new GUIContent("ハネムーンルーム");
							break;
						case 33:
							this.bgComboList[i] = new GUIContent("アウトレットパーク");
							break;
						case 34:
							this.bgComboList[i] = new GUIContent("ビッグサイト");
							break;
						case 35:
							this.bgComboList[i] = new GUIContent("ビッグサイト(夜)");
							break;
						case 36:
							this.bgComboList[i] = new GUIContent("プライベートルーム");
							break;
						case 37:
							this.bgComboList[i] = new GUIContent("プライベートルーム(夜)");
							break;
						case 38:
							this.bgComboList[i] = new GUIContent("海");
							break;
						case 39:
							this.bgComboList[i] = new GUIContent("海(夜)");
							break;
						case 40:
							this.bgComboList[i] = new GUIContent("屋敷(夜)");
							break;
						case 41:
							this.bgComboList[i] = new GUIContent("屋敷");
							break;
						case 42:
							this.bgComboList[i] = new GUIContent("屋敷(夜・枕)");
							break;
						case 43:
							this.bgComboList[i] = new GUIContent("露天風呂");
							break;
						case 44:
							this.bgComboList[i] = new GUIContent("露天風呂(夜)");
							break;
						case 45:
							this.bgComboList[i] = new GUIContent("ヴィラ1F");
							break;
						case 46:
							this.bgComboList[i] = new GUIContent("ヴィラ1F(夜)");
							break;
						case 47:
							this.bgComboList[i] = new GUIContent("ヴィラ2F");
							break;
						case 48:
							this.bgComboList[i] = new GUIContent("ヴィラ2F(夜)");
							break;
						case 49:
							this.bgComboList[i] = new GUIContent("畑");
							break;
						case 50:
							this.bgComboList[i] = new GUIContent("畑(夜)");
							break;
						case 51:
							this.bgComboList[i] = new GUIContent("カラオケルーム");
							break;
						case 52:
							this.bgComboList[i] = new GUIContent("劇場");
							break;
						case 53:
							this.bgComboList[i] = new GUIContent("劇場(夜)");
							break;
						case 54:
							this.bgComboList[i] = new GUIContent("ステージ");
							break;
						case 55:
							this.bgComboList[i] = new GUIContent("ステージ(ライト)");
							break;
						case 56:
							this.bgComboList[i] = new GUIContent("ステージ(オフ)");
							break;
						case 57:
							this.bgComboList[i] = new GUIContent("ステージ裏");
							break;
						case 58:
							this.bgComboList[i] = new GUIContent("トレーニングルーム");
							break;
						case 59:
							this.bgComboList[i] = new GUIContent("ロータリー");
							break;
						case 60:
							this.bgComboList[i] = new GUIContent("ロータリー(夜)");
							break;
						case 61:
							this.bgComboList[i] = new GUIContent("エントランス");
							break;
						case 62:
							this.bgComboList[i] = new GUIContent("執務室");
							break;
						case 63:
							this.bgComboList[i] = new GUIContent("執務室(椅子)");
							break;
						case 64:
							this.bgComboList[i] = new GUIContent("執務室(夜)");
							break;
						case 65:
							this.bgComboList[i] = new GUIContent("主人公部屋");
							break;
						case 66:
							this.bgComboList[i] = new GUIContent("主人公部屋(夜)");
							break;
						case 67:
							this.bgComboList[i] = new GUIContent("カフェ");
							break;
						case 68:
							this.bgComboList[i] = new GUIContent("カフェ(夜)");
							break;
						case 69:
							this.bgComboList[i] = new GUIContent("レストラン");
							break;
						case 70:
							this.bgComboList[i] = new GUIContent("レストラン(夜)");
							break;
						case 71:
							this.bgComboList[i] = new GUIContent("キッチン");
							break;
						case 72:
							this.bgComboList[i] = new GUIContent("キッチン(夜)");
							break;
						case 73:
							this.bgComboList[i] = new GUIContent("キッチン(オフ)");
							break;
						case 74:
							this.bgComboList[i] = new GUIContent("バー");
							break;
						case 75:
							this.bgComboList[i] = new GUIContent("カジノ");
							break;
						case 76:
							this.bgComboList[i] = new GUIContent("カジノミニ");
							break;
						case 77:
							this.bgComboList[i] = new GUIContent("SMクラブ");
							break;
						case 78:
							this.bgComboList[i] = new GUIContent("ソープ");
							break;
						case 79:
							this.bgComboList[i] = new GUIContent("スパ");
							break;
						case 80:
							this.bgComboList[i] = new GUIContent("スパ(夜)");
							break;
						case 81:
							this.bgComboList[i] = new GUIContent("ショッピングモール");
							break;
						case 82:
							this.bgComboList[i] = new GUIContent("ショッピングモール(夜)");
							break;
						case 83:
							this.bgComboList[i] = new GUIContent("ゲームショップ");
							break;
						case 84:
							this.bgComboList[i] = new GUIContent("ミュージックショップ");
							break;
						case 85:
							this.bgComboList[i] = new GUIContent("無垢部屋");
							break;
						case 86:
							this.bgComboList[i] = new GUIContent("無垢部屋(夜)");
							break;
						case 87:
							this.bgComboList[i] = new GUIContent("真面目部屋");
							break;
						case 88:
							this.bgComboList[i] = new GUIContent("真面目部屋(夜)");
							break;
						case 89:
							this.bgComboList[i] = new GUIContent("凜デレ部屋");
							break;
						case 90:
							this.bgComboList[i] = new GUIContent("凜デレ部屋(夜)");
							break;
						case 91:
							this.bgComboList[i] = new GUIContent("ツンデレ部屋");
							break;
						case 92:
							this.bgComboList[i] = new GUIContent("ツンデレ部屋(夜)");
							break;
						case 93:
							this.bgComboList[i] = new GUIContent("クーデレ部屋");
							break;
						case 94:
							this.bgComboList[i] = new GUIContent("クーデレ部屋(夜)");
							break;
						case 95:
							this.bgComboList[i] = new GUIContent("純真部屋");
							break;
						case 96:
							this.bgComboList[i] = new GUIContent("純真部屋(夜)");
							break;
						case 97:
							this.bgComboList[i] = new GUIContent("宿泊-ベッドルーム");
							break;
						case 98:
							this.bgComboList[i] = new GUIContent("宿泊-ベッドルーム(夜)");
							break;
						case 99:
							this.bgComboList[i] = new GUIContent("宿泊-他ベッドルーム(夜)");
							break;
						case 100:
							this.bgComboList[i] = new GUIContent("宿泊-リビング");
							break;
						case 101:
							this.bgComboList[i] = new GUIContent("宿泊-リビング(夜)");
							break;
						case 102:
							this.bgComboList[i] = new GUIContent("宿泊-トイレ");
							break;
						case 103:
							this.bgComboList[i] = new GUIContent("宿泊-トイレ(夜)");
							break;
						case 104:
							this.bgComboList[i] = new GUIContent("宿泊-洗面所");
							break;
						case 105:
							this.bgComboList[i] = new GUIContent("宿泊-洗面所(夜)");
							break;
						case 106:
							this.bgComboList[i] = new GUIContent("ランス10");
							break;
						case 107:
							this.bgComboList[i] = new GUIContent("ランス10(夜)");
							break;
						case 108:
							this.bgComboList[i] = new GUIContent("リドルジョーカー");
							break;
						case 109:
							this.bgComboList[i] = new GUIContent("リドルジョーカー(夜)");
							break;
						case 110:
							this.bgComboList[i] = new GUIContent("わんこ");
							break;
						case 111:
							this.bgComboList[i] = new GUIContent("わんこ(夜)");
							break;
						case 112:
							this.bgComboList[i] = new GUIContent("ラズベリー");
							break;
						case 113:
							this.bgComboList[i] = new GUIContent("ラズベリー(夜)");
							break;
						case 114:
							this.bgComboList[i] = new GUIContent("シーカフェ");
							break;
						case 115:
							this.bgComboList[i] = new GUIContent("シーカフェ(夜)");
							break;
						case 116:
							this.bgComboList[i] = new GUIContent("プール");
							break;
						case 117:
							this.bgComboList[i] = new GUIContent("プール(夜)");
							break;
						case 118:
							this.bgComboList[i] = new GUIContent("神社");
							break;
						case 119:
							this.bgComboList[i] = new GUIContent("神社(夜)");
							break;
						default:
							goto IL_18DC;
					}
				IL_195B:
					Dictionary<string, string> saveDataDic = CreativeRoomManager.GetSaveDataDic();
					if (saveDataDic != null)
					{
						foreach (KeyValuePair<string, string> keyValuePair in saveDataDic)
						{
							if (this.bgArray[i] == keyValuePair.Key)
							{
								this.bgComboList[i] = new GUIContent(keyValuePair.Value);
							}
						}
					}
					i++;
					continue;
				IL_18DC:
					string text2 = this.bgArray[i];
					for (int j = 0; j < this.bgNameList.Count; j++)
					{
						string[] array = this.bgNameList[j].Split(new char[]
						{
							','
						});
						if (text2 == array[0])
						{
							text2 = array[1];
						}
					}
					this.bgComboList[i] = new GUIContent(text2);
					goto IL_195B;
				}
				this.doguCombo2.selectedItemIndex = 0;
				this.doguCombo2List = new GUIContent[this.doguB1Array.Length];
				i = 0;
				while (i < this.doguB1Array.Length)
				{
					string text = this.doguB1Array[i];
					if (text == null)
					{
						goto IL_4852;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-3 == null)
					if (OdoguUIArray == null)
					{
						OdoguUIArray = new Dictionary<string, int>(276)
						{
							{
								"Odogu_KousokuKijyouiChair_photo_ver",
								0
							},
							{
								"Odogu_VirginRoad_photo_ver",
								1
							},
							{
								"neirobo",
								2
							},
							{
								"Odogu_ClassRoomDesk_photo_ver",
								3
							},
							{
								"Odogu_ClassRoomChair_photo_ver",
								4
							},
							{
								"Odogu_TrumpTowerSmall_photo_ver",
								5
							},
							{
								"Odogu_TrumpTowerBig_photo_ver",
								6
							},
							{
								"Odogu_VVLight_photo_ver",
								7
							},
							{
								"Odogu_OXCamera_photo_ver",
								8
							},
							{
								"Odogu_HandCameraVV_photo_ver",
								9
							},
							{
								"Odogu_PC_photo_ver",
								10
							},
							{
								"Odogu_PC_Monitor_photo_ver",
								11
							},
							{
								"Odogu_PC_Keyboard_photo_ver",
								12
							},
							{
								"Odogu_PC_Mouse_photo_ver",
								13
							},
							{
								"Odogu_MaidRoomBook001_photo_ver",
								14
							},
							{
								"Odogu_MaidRoomBook002_photo_ver",
								15
							},
							{
								"Odogu_MaidRoomBook003_photo_ver",
								16
							},
							{
								"Odogu_MaidRoomBook004_photo_ver",
								17
							},
							{
								"Odogu_MaidRoomBook005_photo_ver",
								18
							},
							{
								"Odogu_Pen_photo_ver",
								19
							},
							{
								"Odogu_Pen_Black_photo_ver",
								20
							},
							{
								"Odogu_Pen_Brown_photo_ver",
								21
							},
							{
								"Odogu_Pen_Green_photo_ver",
								22
							},
							{
								"Odogu_Enpitsu_photo_ver",
								23
							},
							{
								"Odogu_Enpitsu_Black_photo_ver",
								24
							},
							{
								"Odogu_Enpitsu_Red_photo_ver",
								25
							},
							{
								"Odogu_Keshigomu_photo_ver",
								26
							},
							{
								"Odogu_Keshigomu_Purple_photo_ver",
								27
							},
							{
								"Odogu_Keshigomu_Yellow_photo_ver",
								28
							},
							{
								"Odogu_StickNori_photo_ver",
								29
							},
							{
								"Odogu_Condom_Close_photo_ver",
								30
							},
							{
								"Odogu_Condom_Open_photo_ver",
								31
							},
							{
								"Odogu_Condom_Pack_photo_ver",
								32
							},
							{
								"Odogu_SalonSofa_long_photo_ver",
								33
							},
							{
								"Odogu_SalonSofa_4P_photo_ver",
								34
							},
							{
								"Odogu_Girochin_A_photo_ver",
								35
							},
							{
								"Odogu_SankakuMokuba_photo_ver",
								36
							},
							{
								"Odogu_SMRoom2_SankakuMokuba_photo_ver",
								37
							},
							{
								"Odogu_Kousokudai_photo_ver",
								38
							},
							{
								"Odogu_XmasTreeMini_photo_ver",
								39
							},
							{
								"Odogu_KadomatsuMini_photo_ver",
								40
							},
							{
								"Odogu_Kitchen_photo_ver",
								41
							},
							{
								"Odogu_TableFlower_photo_ver",
								42
							},
							{
								"Odogu_Kadou_photo_ver",
								43
							},
							{
								"Odogu_Dresser_photo_ver",
								44
							},
							{
								"Odogu_ClassRoomDesk",
								45
							},
							{
								"Odogu_KadouChair_photo_ver",
								46
							},
							{
								"Odogu_DresserChair_photo_ver",
								47
							},
							{
								"Odogu_MaidRoomChair_photo_ver",
								48
							},
							{
								"Odogu_PublicToiletBenki_photo_ver",
								49
							},
							{
								"Odogu_Sukebeisu_photo_ver",
								50
							},
							{
								"Odogu_Mat_photo_ver",
								51
							},
							{
								"Odogu_Seikaku_Tsundere",
								52
							},
							{
								"Odogu_Seikaku_Jyunshin",
								53
							},
							{
								"Odogu_Seikaku_Cool",
								54
							},
							{
								"Odogu_Manaita_photo_ver",
								55
							},
							{
								"Odogu_Nabe_photo_ver",
								56
							},
							{
								"Odogu_NoteBook_photo_ver",
								57
							},
							{
								"Odogu_Sankousyo_photo_ver",
								58
							},
							{
								"Odogu_Sentaku_Kago_photo_ver",
								59
							},
							{
								"Odogu_Sentaku_Towel_photo_ver",
								60
							},
							{
								"Odogu_Sentakumono_photo_ver",
								61
							},
							{
								"Odogu_SalonScreen_photo_ver",
								62
							},
							{
								"Odogu_WineGlass_photo_ver",
								63
							},
							{
								"Odogu_SalonSofa_small_photo_ver",
								64
							},
							{
								"Odogu_Seikaku_Tsundere_photo_ver",
								65
							},
							{
								"Odogu_Seikaku_Jyunshin_photo_ver",
								66
							},
							{
								"Odogu_Seikaku_Cool_photo_ver",
								67
							},
							{
								"Megane001_z2_Scenario_Model",
								68
							},
							{
								"nei_photo_ver",
								69
							},
							{
								"neirobo_photo_ver",
								70
							},
							{
								"Odogu_SimpleTable",
								71
							},
							{
								"Odogu_DildoBox",
								72
							},
							{
								"PlayAreaOut",
								73
							},
							{
								"DesktopScreen",
								74
							},
							{
								"Odogu_ChuukaSet_chahan_photo_ver",
								75
							},
							{
								"Odogu_ChuukaSet_gyouza_photo_ver",
								76
							},
							{
								"Odogu_ChuukaSet_mabo_photo_ver",
								77
							},
							{
								"Odogu_ChuukaSet_tea_photo_ver",
								78
							},
							{
								"Odogu_WasyokuSet_gohan_photo_ver",
								79
							},
							{
								"Odogu_WasyokuSet_hashi_photo_ver",
								80
							},
							{
								"Odogu_WasyokuSet_misoshiru_photo_ver",
								81
							},
							{
								"Odogu_WasyokuSet_nimono_photo_ver",
								82
							},
							{
								"Odogu_WasyokuSet_ocha_photo_ver",
								83
							},
							{
								"Odogu_YousyokuSet_ChickenRice_photo_ver",
								84
							},
							{
								"Odogu_YousyokuSet_Coffee_photo_ver",
								85
							},
							{
								"Odogu_YousyokuSet_CornSoup_photo_ver",
								86
							},
							{
								"Odogu_YousyokuSet_Hamburg_photo_ver",
								87
							},
							{
								"Odogu_YousyokuSet_SakiwareSpoon_photo_ver",
								88
							},
							{
								"Odogu_PR_Table_photo_ver",
								89
							},
							{
								"Odogu_PR_Table_Chuuka_photo_ver",
								90
							},
							{
								"Odogu_PR_Table_Wasyoku_photo_ver",
								91
							},
							{
								"Odogu_PR_Table_Yousyoku_photo_ver",
								92
							},
							{
								"Odogu_LongDaiza_photo_ver",
								93
							},
							{
								"BGanimal_cat",
								94
							},
							{
								"BGanimal_dog",
								95
							},
							{
								"BGanimal_niwatori",
								96
							},
							{
								"BGanimal_suzume",
								97
							},
							{
								"BGodogu_bbqgrill",
								98
							},
							{
								"BGodogu_bucket",
								99
							},
							{
								"BGodogu_coolerbox",
								100
							},
							{
								"BGodogu_game_darts",
								101
							},
							{
								"BGodogu_game_dartsboard",
								102
							},
							{
								"BGodogu_nabe_huta",
								103
							},
							{
								"BGodogu_nabe_water",
								104
							},
							{
								"BGodogu_natumikan",
								105
							},
							{
								"BGodogu_rb_chair",
								106
							},
							{
								"BGodogu_rb_duck",
								107
							},
							{
								"BGodogu_rb_obon",
								108
							},
							{
								"BGodogu_rb_tokkuri",
								109
							},
							{
								"BGodogu_saracorn",
								110
							},
							{
								"BGodogu_saraimo",
								111
							},
							{
								"BGodogu_saratomato",
								112
							},
							{
								"BGodogu_sunanoshiro",
								113
							},
							{
								"BGodogu_sunanoyama",
								114
							},
							{
								"BGodogu_tsutsuhanabi",
								115
							},
							{
								"BGodogu_ukiwa",
								116
							},
							{
								"BGodogu_vf_crops_corn",
								117
							},
							{
								"BGodogu_vf_crops_gekkabijin",
								118
							},
							{
								"BGodogu_vf_crops_gekkabijinflower",
								119
							},
							{
								"BGodogu_vf_crops_himawari",
								120
							},
							{
								"BGodogu_vf_crops_natsumikan",
								121
							},
							{
								"BGodogu_vf_crops_suika",
								122
							},
							{
								"BGodogu_vf_crops_zakuro",
								123
							},
							{
								"BGodogu_vf_kanban_ok",
								124
							},
							{
								"BGodogu_vf_kanban_saibai",
								125
							},
							{
								"BGodogu_vf_kanban_taiki",
								126
							},
							{
								"BGodogu_vf_radio",
								127
							},
							{
								"BGodogu_villa_coffeemaker",
								128
							},
							{
								"BGodogu_villa_reizouko",
								129
							},
							{
								"BGodogu_villa_table",
								130
							},
							{
								"BGodogu_villa_tvrimocon",
								131
							},
							{
								"BGodogu_villa_winecellar",
								132
							},
							{
								"BGodogu_villabr_sideboard",
								133
							},
							{
								"BGOdogu_Game_Nei_USB",
								134
							},
							{
								"BGOdogu_Game_Wanage",
								135
							},
							{
								"BGOdogu_Game_Wa",
								136
							},
							{
								"BGodogu_pafe",
								137
							},
							{
								"BGodogu_furaidopoteto",
								138
							},
							{
								"BGodogu_karaoketable",
								139
							},
							{
								"BGodogu_omuriceh",
								140
							},
							{
								"BGodogu_omuricekao1",
								141
							},
							{
								"BGodogu_omuricekao2",
								142
							},
							{
								"BGodogu_omuriceoppai",
								143
							},
							{
								"BGodogu_kakigori",
								144
							},
							{
								"BGodogu_pretzel_sara",
								145
							},
							{
								"BGodogu_karaoke_box",
								146
							},
							{
								"Odogu_StandMike",
								147
							},
							{
								"Odogu_StandMikeBase",
								148
							},
							{
								"photo_ver/Odogu_Umeko_Mike_photo_ver",
								149
							},
							{
								"Odogu_HeroineChair_muku",
								150
							},
							{
								"Odogu_HeroineChair_mazime",
								151
							},
							{
								"Odogu_HeroineChair_rindere",
								152
							},
							{
								"Odogu_HeroineChair_tsumdere",
								153
							},
							{
								"Odogu_HeroineChair_cooldere",
								154
							},
							{
								"Odogu_HeroineChair_junshin",
								155
							},
							{
								"photo_ver/Odogu_Etoile_Chair_photo_ver",
								156
							},
							{
								"Odogu_LoveSofa",
								157
							},
							{
								"Odogu_TabletPC",
								158
							},
							{
								"Odogu_Styluspen_black",
								159
							},
							{
								"Odogu_Styluspen_white",
								160
							},
							{
								"Odogu_Styluspen_red",
								161
							},
							{
								"Odogu_Styluspen_blue",
								162
							},
							{
								"Odogu_Styluspen_yellow",
								163
							},
							{
								"Odogu_Styluspen_green",
								164
							},
							{
								"Odogu_Omurice1",
								165
							},
							{
								"Odogu_Omurice3",
								166
							},
							{
								"Odogu_OmuriceH",
								167
							},
							{
								"Odogu_OmuriceKao1",
								168
							},
							{
								"Odogu_OmuriceKao2",
								169
							},
							{
								"Odogu_OmuriceOppai",
								170
							},
							{
								"Odogu_AcquaPazza",
								171
							},
							{
								"Odogu_Sandwich",
								172
							},
							{
								"Odogu_vichyssoise",
								173
							},
							{
								"Odogu_BirthdayCake",
								174
							},
							{
								"Odogu_Shortcake",
								175
							},
							{
								"Odogu_MontBlanc",
								176
							},
							{
								"Odogu_Pafe",
								177
							},
							{
								"Odogu_Smoothie_Red",
								178
							},
							{
								"Odogu_Smoothie_Green",
								179
							},
							{
								"Odogu_Cocktail_Red",
								180
							},
							{
								"Odogu_Cocktail_Blue",
								181
							},
							{
								"Odogu_Cocktail_Yellow",
								182
							},
							{
								"Odogu_Coffiecup",
								183
							},
							{
								"Odogu_WineBottle(cap_off)",
								184
							},
							{
								"Odogu_WineBottle(cap_on)",
								185
							},
							{
								"Odogu_Jyouro",
								186
							},
							{
								"Odogu_Planter_Red",
								187
							},
							{
								"Odogu_Planter_Lightblue",
								188
							},
							{
								"Odogu_MariGold",
								189
							},
							{
								"Odogu_CasinoChip_10",
								190
							},
							{
								"Odogu_CasinoChip_100",
								191
							},
							{
								"Odogu_CasinoChip_1000",
								192
							},
							{
								"Odogu_CardShooter",
								193
							},
							{
								"Odogu_CardsDeck",
								194
							},
							{
								"Odogu_Card_s1",
								195
							},
							{
								"Odogu_Card_s2",
								196
							},
							{
								"Odogu_Card_s3",
								197
							},
							{
								"Odogu_Card_s4",
								198
							},
							{
								"Odogu_Card_s5",
								199
							},
							{
								"Odogu_Card_s6",
								200
							},
							{
								"Odogu_Card_s7",
								201
							},
							{
								"Odogu_Card_s8",
								202
							},
							{
								"Odogu_Card_s9",
								203
							},
							{
								"Odogu_Card_s10",
								204
							},
							{
								"Odogu_Card_s11",
								205
							},
							{
								"Odogu_Card_s12",
								206
							},
							{
								"Odogu_Card_s13",
								207
							},
							{
								"Odogu_Card_h1",
								208
							},
							{
								"Odogu_Card_h2",
								209
							},
							{
								"Odogu_Card_h3",
								210
							},
							{
								"Odogu_Card_h4",
								211
							},
							{
								"Odogu_Card_h5",
								212
							},
							{
								"Odogu_Card_h6",
								213
							},
							{
								"Odogu_Card_h7",
								214
							},
							{
								"Odogu_Card_h8",
								215
							},
							{
								"Odogu_Card_h9",
								216
							},
							{
								"Odogu_Card_h10",
								217
							},
							{
								"Odogu_Card_h11",
								218
							},
							{
								"Odogu_Card_h12",
								219
							},
							{
								"Odogu_Card_h13",
								220
							},
							{
								"Odogu_Card_d1",
								221
							},
							{
								"Odogu_Card_d2",
								222
							},
							{
								"Odogu_Card_d3",
								223
							},
							{
								"Odogu_Card_d4",
								224
							},
							{
								"Odogu_Card_d5",
								225
							},
							{
								"Odogu_Card_d6",
								226
							},
							{
								"Odogu_Card_d7",
								227
							},
							{
								"Odogu_Card_d8",
								228
							},
							{
								"Odogu_Card_d9",
								229
							},
							{
								"Odogu_Card_d10",
								230
							},
							{
								"Odogu_Card_d11",
								231
							},
							{
								"Odogu_Card_d12",
								232
							},
							{
								"Odogu_Card_d13",
								233
							},
							{
								"Odogu_Card_c1",
								234
							},
							{
								"Odogu_Card_c2",
								235
							},
							{
								"Odogu_Card_c3",
								236
							},
							{
								"Odogu_Card_c4",
								237
							},
							{
								"Odogu_Card_c5",
								238
							},
							{
								"Odogu_Card_c6",
								239
							},
							{
								"Odogu_Card_c7",
								240
							},
							{
								"Odogu_Card_c8",
								241
							},
							{
								"Odogu_Card_c9",
								242
							},
							{
								"Odogu_Card_c10",
								243
							},
							{
								"Odogu_Card_c11",
								244
							},
							{
								"Odogu_Card_c12",
								245
							},
							{
								"Odogu_Card_c13",
								246
							},
							{
								"Odogu_Card_joker",
								247
							},
							{
								"BGNeiMaid",
								248
							},
							{
								"BGOdogu_alicesoft_bluehoney",
								249
							},
							{
								"BGOdogu_alicesoft_brownhoney",
								250
							},
							{
								"BGOdogu_alicesoft_greenhoney",
								251
							},
							{
								"BGOdogu_alicesoft_redhoney",
								252
							},
							{
								"BGOdogu_sp001_beercan",
								253
							},
							{
								"BGOdogu_sp001_beerjug",
								254
							},
							{
								"BGOdogu_sp001_cake",
								255
							},
							{
								"BGOdogu_sp001_food",
								256
							},
							{
								"BGOdogu_sp001_yakisoba",
								257
							},
							{
								"BGOdogu_denkigai2018s_beachball_blue",
								258
							},
							{
								"BGOdogu_denkigai2018s_beachball_green",
								259
							},
							{
								"BGOdogu_denkigai2018s_beachball_red",
								260
							},
							{
								"BGOdogu_denkigai2018s_beachball_yellow",
								261
							},
							{
								"BGOdogu_denkigai2018s_coneice_chocomint",
								262
							},
							{
								"BGOdogu_denkigai2018s_coneice_strawberry",
								263
							},
							{
								"BGOdogu_denkigai2018s_coneice_vanilla",
								264
							},
							{
								"BGOdogu_denkigai2018s_melondish",
								265
							},
							{
								"BGOdogu_denkigai2018s_syatifloot",
								266
							},
							{
								"BGOdogu_denkigai2018s_toropicalicetea",
								267
							},
							{
								"BGOdogu_sp002_oharaibou",
								268
							},
							{
								"BGOdogu_sp002_omamori",
								269
							},
							{
								"BGOdogu_sp002_susuki",
								270
							},
							{
								"BGOdogu_sp002_takebouki",
								271
							},
							{
								"BGOdogu_sp002_tukimidango",
								272
							},
							{
								"BGOdogu_sp002_waraningyou",
								273
							},
							{
								"BGOdogu_sp002_waraningyou_gosunkugi",
								274
							},
							{
								"BGOdogu",
								275
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-3.TryGetValue(text, out num))
					//{
					//	goto IL_4852;
					//}
					if (!OdoguUIArray.TryGetValue(text, out num))
					{
						goto IL_4852;
					}
					switch (num)
					{
						case 0:
							this.doguCombo2List[i] = new GUIContent("拘束椅子");
							break;
						case 1:
							this.doguCombo2List[i] = new GUIContent("バージンロード");
							break;
						case 2:
							this.doguCombo2List[i] = new GUIContent("ロボねい人形");
							break;
						case 3:
							this.doguCombo2List[i] = new GUIContent("教室机");
							break;
						case 4:
							this.doguCombo2List[i] = new GUIContent("教室椅子");
							break;
						case 5:
							this.doguCombo2List[i] = new GUIContent("トランプタワー(小)");
							break;
						case 6:
							this.doguCombo2List[i] = new GUIContent("トランプタワー");
							break;
						case 7:
							this.doguCombo2List[i] = new GUIContent("Wライト");
							break;
						case 8:
							this.doguCombo2List[i] = new GUIContent("OXカメラ");
							break;
						case 9:
							this.doguCombo2List[i] = new GUIContent("レトロカメラ");
							break;
						case 10:
							this.doguCombo2List[i] = new GUIContent("PC");
							break;
						case 11:
							this.doguCombo2List[i] = new GUIContent("モニター");
							break;
						case 12:
							this.doguCombo2List[i] = new GUIContent("キーボード");
							break;
						case 13:
							this.doguCombo2List[i] = new GUIContent("マウス");
							break;
						case 14:
							this.doguCombo2List[i] = new GUIContent("参考書A");
							break;
						case 15:
							this.doguCombo2List[i] = new GUIContent("参考書B");
							break;
						case 16:
							this.doguCombo2List[i] = new GUIContent("参考書C");
							break;
						case 17:
							this.doguCombo2List[i] = new GUIContent("参考書D");
							break;
						case 18:
							this.doguCombo2List[i] = new GUIContent("参考書E");
							break;
						case 19:
							this.doguCombo2List[i] = new GUIContent("ペン(桃)");
							break;
						case 20:
							this.doguCombo2List[i] = new GUIContent("ペン(黒)");
							break;
						case 21:
							this.doguCombo2List[i] = new GUIContent("ペン(茶)");
							break;
						case 22:
							this.doguCombo2List[i] = new GUIContent("ペン(緑)");
							break;
						case 23:
							this.doguCombo2List[i] = new GUIContent("鉛筆(緑)");
							break;
						case 24:
							this.doguCombo2List[i] = new GUIContent("鉛筆(黒)");
							break;
						case 25:
							this.doguCombo2List[i] = new GUIContent("鉛筆(赤)");
							break;
						case 26:
							this.doguCombo2List[i] = new GUIContent("消しゴム(青)");
							break;
						case 27:
							this.doguCombo2List[i] = new GUIContent("消しゴム(紫)");
							break;
						case 28:
							this.doguCombo2List[i] = new GUIContent("消しゴム(黄)");
							break;
						case 29:
							this.doguCombo2List[i] = new GUIContent("スティック糊");
							break;
						case 30:
							this.doguCombo2List[i] = new GUIContent("コンドーム(閉)");
							break;
						case 31:
							this.doguCombo2List[i] = new GUIContent("コンドーム(開)");
							break;
						case 32:
							this.doguCombo2List[i] = new GUIContent("コンドーム(袋)");
							break;
						case 33:
							this.doguCombo2List[i] = new GUIContent("ソファー");
							break;
						case 34:
							this.doguCombo2List[i] = new GUIContent("ソファー(大)");
							break;
						case 35:
							this.doguCombo2List[i] = new GUIContent("ギロチン");
							break;
						case 36:
							this.doguCombo2List[i] = new GUIContent("三角木馬");
							break;
						case 37:
							this.doguCombo2List[i] = new GUIContent("三角木馬2");
							break;
						case 38:
							this.doguCombo2List[i] = new GUIContent("拘束台");
							break;
						case 39:
							this.doguCombo2List[i] = new GUIContent("クリスマスツリー");
							break;
						case 40:
							this.doguCombo2List[i] = new GUIContent("門松");
							break;
						case 41:
							this.doguCombo2List[i] = new GUIContent("キッチン");
							break;
						case 42:
							this.doguCombo2List[i] = new GUIContent("花とテーブル");
							break;
						case 43:
							this.doguCombo2List[i] = new GUIContent("華道");
							break;
						case 44:
							this.doguCombo2List[i] = new GUIContent("ドレッサー");
							break;
						case 45:
							this.doguCombo2List[i] = new GUIContent("教室机");
							break;
						case 46:
							this.doguCombo2List[i] = new GUIContent("華道椅子");
							break;
						case 47:
							this.doguCombo2List[i] = new GUIContent("ドレッサー椅子");
							break;
						case 48:
							this.doguCombo2List[i] = new GUIContent("メイド部屋椅子");
							break;
						case 49:
							this.doguCombo2List[i] = new GUIContent("ベンキ");
							break;
						case 50:
							this.doguCombo2List[i] = new GUIContent("スケベ椅子");
							break;
						case 51:
							this.doguCombo2List[i] = new GUIContent("マット");
							break;
						case 52:
							this.doguCombo2List[i] = new GUIContent("ツンデレ");
							break;
						case 53:
							this.doguCombo2List[i] = new GUIContent("純真");
							break;
						case 54:
							this.doguCombo2List[i] = new GUIContent("クール");
							break;
						case 55:
							this.doguCombo2List[i] = new GUIContent("まな板");
							break;
						case 56:
							this.doguCombo2List[i] = new GUIContent("鍋");
							break;
						case 57:
							this.doguCombo2List[i] = new GUIContent("ノート");
							break;
						case 58:
							this.doguCombo2List[i] = new GUIContent("参考書");
							break;
						case 59:
							this.doguCombo2List[i] = new GUIContent("洗濯かご");
							break;
						case 60:
							this.doguCombo2List[i] = new GUIContent("重ねたタオル");
							break;
						case 61:
							this.doguCombo2List[i] = new GUIContent("洗濯物");
							break;
						case 62:
							this.doguCombo2List[i] = new GUIContent("スクリーン");
							break;
						case 63:
							this.doguCombo2List[i] = new GUIContent("ワイングラス");
							break;
						case 64:
							this.doguCombo2List[i] = new GUIContent("ソファー(小)");
							break;
						case 65:
							this.doguCombo2List[i] = new GUIContent("ツンデレ");
							break;
						case 66:
							this.doguCombo2List[i] = new GUIContent("純真");
							break;
						case 67:
							this.doguCombo2List[i] = new GUIContent("クール");
							break;
						case 68:
							this.doguCombo2List[i] = new GUIContent("メガネ");
							break;
						case 69:
							this.doguCombo2List[i] = new GUIContent("ねい人形");
							break;
						case 70:
							this.doguCombo2List[i] = new GUIContent("ロボねい人形");
							break;
						case 71:
							this.doguCombo2List[i] = new GUIContent("テーブル");
							break;
						case 72:
							this.doguCombo2List[i] = new GUIContent("ディルドボックス");
							break;
						case 73:
							this.doguCombo2List[i] = new GUIContent("プレイエリア外");
							break;
						case 74:
							this.doguCombo2List[i] = new GUIContent("デスクトップスクリーン");
							break;
						case 75:
							this.doguCombo2List[i] = new GUIContent("チャーハン");
							break;
						case 76:
							this.doguCombo2List[i] = new GUIContent("餃子");
							break;
						case 77:
							this.doguCombo2List[i] = new GUIContent("麻婆豆腐");
							break;
						case 78:
							this.doguCombo2List[i] = new GUIContent("お茶");
							break;
						case 79:
							this.doguCombo2List[i] = new GUIContent("ご飯");
							break;
						case 80:
							this.doguCombo2List[i] = new GUIContent("箸");
							break;
						case 81:
							this.doguCombo2List[i] = new GUIContent("味噌汁");
							break;
						case 82:
							this.doguCombo2List[i] = new GUIContent("煮物");
							break;
						case 83:
							this.doguCombo2List[i] = new GUIContent("緑茶");
							break;
						case 84:
							this.doguCombo2List[i] = new GUIContent("チキンライス");
							break;
						case 85:
							this.doguCombo2List[i] = new GUIContent("コーヒー");
							break;
						case 86:
							this.doguCombo2List[i] = new GUIContent("コーンスープ");
							break;
						case 87:
							this.doguCombo2List[i] = new GUIContent("ハンバーグ");
							break;
						case 88:
							this.doguCombo2List[i] = new GUIContent("先割れスプーン");
							break;
						case 89:
							this.doguCombo2List[i] = new GUIContent("テーブル");
							break;
						case 90:
							this.doguCombo2List[i] = new GUIContent("中華テーブル");
							break;
						case 91:
							this.doguCombo2List[i] = new GUIContent("和食テーブル");
							break;
						case 92:
							this.doguCombo2List[i] = new GUIContent("洋食テーブル");
							break;
						case 93:
							this.doguCombo2List[i] = new GUIContent("エッチする時の台");
							break;
						case 94:
							this.doguCombo2List[i] = new GUIContent("猫");
							break;
						case 95:
							this.doguCombo2List[i] = new GUIContent("犬");
							break;
						case 96:
							this.doguCombo2List[i] = new GUIContent("ニワトリ");
							break;
						case 97:
							this.doguCombo2List[i] = new GUIContent("スズメ");
							break;
						case 98:
							this.doguCombo2List[i] = new GUIContent("バーベキューグリル");
							break;
						case 99:
							this.doguCombo2List[i] = new GUIContent("バケツ");
							break;
						case 100:
							this.doguCombo2List[i] = new GUIContent("クーラーボックス");
							break;
						case 101:
							this.doguCombo2List[i] = new GUIContent("ダーツ");
							break;
						case 102:
							this.doguCombo2List[i] = new GUIContent("ダーツボード");
							break;
						case 103:
							this.doguCombo2List[i] = new GUIContent("鍋");
							break;
						case 104:
							this.doguCombo2List[i] = new GUIContent("鍋");
							break;
						case 105:
							this.doguCombo2List[i] = new GUIContent("夏みかん");
							break;
						case 106:
							this.doguCombo2List[i] = new GUIContent("風呂椅子");
							break;
						case 107:
							this.doguCombo2List[i] = new GUIContent("アヒル");
							break;
						case 108:
							this.doguCombo2List[i] = new GUIContent("おぼん");
							break;
						case 109:
							this.doguCombo2List[i] = new GUIContent("とっくり");
							break;
						case 110:
							this.doguCombo2List[i] = new GUIContent("コーン皿");
							break;
						case 111:
							this.doguCombo2List[i] = new GUIContent("イモ皿");
							break;
						case 112:
							this.doguCombo2List[i] = new GUIContent("トマト皿");
							break;
						case 113:
							this.doguCombo2List[i] = new GUIContent("砂の城");
							break;
						case 114:
							this.doguCombo2List[i] = new GUIContent("砂山");
							break;
						case 115:
							this.doguCombo2List[i] = new GUIContent("筒花火");
							break;
						case 116:
							this.doguCombo2List[i] = new GUIContent("浮き輪");
							break;
						case 117:
							this.doguCombo2List[i] = new GUIContent("作物(コーン)");
							break;
						case 118:
							this.doguCombo2List[i] = new GUIContent("作物(月下美人)");
							break;
						case 119:
							this.doguCombo2List[i] = new GUIContent("作物(月下美人・咲)");
							break;
						case 120:
							this.doguCombo2List[i] = new GUIContent("作物(向日葵)");
							break;
						case 121:
							this.doguCombo2List[i] = new GUIContent("作物(夏みかん)");
							break;
						case 122:
							this.doguCombo2List[i] = new GUIContent("作物(スイカ)");
							break;
						case 123:
							this.doguCombo2List[i] = new GUIContent("作物(ザクロ)");
							break;
						case 124:
							this.doguCombo2List[i] = new GUIContent("");
							break;
						case 125:
							this.doguCombo2List[i] = new GUIContent("");
							break;
						case 126:
							this.doguCombo2List[i] = new GUIContent("");
							break;
						case 127:
							this.doguCombo2List[i] = new GUIContent("ラジオ");
							break;
						case 128:
							this.doguCombo2List[i] = new GUIContent("コーヒーメーカー");
							break;
						case 129:
							this.doguCombo2List[i] = new GUIContent("冷蔵庫");
							break;
						case 130:
							this.doguCombo2List[i] = new GUIContent("テーブル");
							break;
						case 131:
							this.doguCombo2List[i] = new GUIContent("テレビリモコン");
							break;
						case 132:
							this.doguCombo2List[i] = new GUIContent("ワインセラー");
							break;
						case 133:
							this.doguCombo2List[i] = new GUIContent("サイドボード");
							break;
						case 134:
							this.doguCombo2List[i] = new GUIContent("ねい人形USB");
							break;
						case 135:
							this.doguCombo2List[i] = new GUIContent("輪投げ");
							break;
						case 136:
							this.doguCombo2List[i] = new GUIContent("輪");
							break;
						case 137:
							this.doguCombo2List[i] = new GUIContent("パフェ");
							break;
						case 138:
							this.doguCombo2List[i] = new GUIContent("フライドポテト");
							break;
						case 139:
							this.doguCombo2List[i] = new GUIContent("カラオケテーブル");
							break;
						case 140:
							this.doguCombo2List[i] = new GUIContent("オムライスH");
							break;
						case 141:
							this.doguCombo2List[i] = new GUIContent("オムライス顔1");
							break;
						case 142:
							this.doguCombo2List[i] = new GUIContent("オムライス顔2");
							break;
						case 143:
							this.doguCombo2List[i] = new GUIContent("オムライスおっぱい");
							break;
						case 144:
							this.doguCombo2List[i] = new GUIContent("かき氷");
							break;
						case 145:
							this.doguCombo2List[i] = new GUIContent("スナックプレート");
							break;
						case 146:
							this.doguCombo2List[i] = new GUIContent("箱");
							break;
						case 147:
							this.doguCombo2List[i] = new GUIContent("スタンドマイク");
							break;
						case 148:
							this.doguCombo2List[i] = new GUIContent("スタンドマイクベース");
							break;
						case 149:
							this.doguCombo2List[i] = new GUIContent("コアラマイク");
							break;
						case 150:
							this.doguCombo2List[i] = new GUIContent("無垢椅子");
							break;
						case 151:
							this.doguCombo2List[i] = new GUIContent("真面目椅子");
							break;
						case 152:
							this.doguCombo2List[i] = new GUIContent("凛デレ椅子");
							break;
						case 153:
							this.doguCombo2List[i] = new GUIContent("ツンデレ椅子");
							break;
						case 154:
							this.doguCombo2List[i] = new GUIContent("クーデレ椅子");
							break;
						case 155:
							this.doguCombo2List[i] = new GUIContent("純真椅子");
							break;
						case 156:
							this.doguCombo2List[i] = new GUIContent("ふかふかチェア");
							break;
						case 157:
							this.doguCombo2List[i] = new GUIContent("ラブソファー");
							break;
						case 158:
							this.doguCombo2List[i] = new GUIContent("タブレットPC");
							break;
						case 159:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(黒)");
							break;
						case 160:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(白)");
							break;
						case 161:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(赤)");
							break;
						case 162:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(青)");
							break;
						case 163:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(黄)");
							break;
						case 164:
							this.doguCombo2List[i] = new GUIContent("スタイラスペン(緑)");
							break;
						case 165:
							this.doguCombo2List[i] = new GUIContent("オムライス1");
							break;
						case 166:
							this.doguCombo2List[i] = new GUIContent("オムライス3");
							break;
						case 167:
							this.doguCombo2List[i] = new GUIContent("オムライスH");
							break;
						case 168:
							this.doguCombo2List[i] = new GUIContent("オムライス顔1");
							break;
						case 169:
							this.doguCombo2List[i] = new GUIContent("オムライス顔2");
							break;
						case 170:
							this.doguCombo2List[i] = new GUIContent("オムライスおっぱい");
							break;
						case 171:
							this.doguCombo2List[i] = new GUIContent("アクアパッザ");
							break;
						case 172:
							this.doguCombo2List[i] = new GUIContent("サンドイッチ");
							break;
						case 173:
							this.doguCombo2List[i] = new GUIContent("スープ");
							break;
						case 174:
							this.doguCombo2List[i] = new GUIContent("バースデーケーキ");
							break;
						case 175:
							this.doguCombo2List[i] = new GUIContent("ショートケーキ");
							break;
						case 176:
							this.doguCombo2List[i] = new GUIContent("モンブラン");
							break;
						case 177:
							this.doguCombo2List[i] = new GUIContent("パフェ");
							break;
						case 178:
							this.doguCombo2List[i] = new GUIContent("スムージー・赤");
							break;
						case 179:
							this.doguCombo2List[i] = new GUIContent("スムージー・緑");
							break;
						case 180:
							this.doguCombo2List[i] = new GUIContent("カクテル・赤");
							break;
						case 181:
							this.doguCombo2List[i] = new GUIContent("カクテル・青");
							break;
						case 182:
							this.doguCombo2List[i] = new GUIContent("カクテル・黄");
							break;
						case 183:
							this.doguCombo2List[i] = new GUIContent("コーヒーカップ");
							break;
						case 184:
							this.doguCombo2List[i] = new GUIContent("ワインボトル");
							break;
						case 185:
							this.doguCombo2List[i] = new GUIContent("ワインボトル(蓋)");
							break;
						case 186:
							this.doguCombo2List[i] = new GUIContent("如雨露");
							break;
						case 187:
							this.doguCombo2List[i] = new GUIContent("プランター(赤)");
							break;
						case 188:
							this.doguCombo2List[i] = new GUIContent("プランター(青)");
							break;
						case 189:
							this.doguCombo2List[i] = new GUIContent("マリーゴールド");
							break;
						case 190:
							this.doguCombo2List[i] = new GUIContent("カジノチップ10");
							break;
						case 191:
							this.doguCombo2List[i] = new GUIContent("カジノチップ100");
							break;
						case 192:
							this.doguCombo2List[i] = new GUIContent("カジノチップ1000");
							break;
						case 193:
							this.doguCombo2List[i] = new GUIContent("カードシューター");
							break;
						case 194:
							this.doguCombo2List[i] = new GUIContent("カードデッキ");
							break;
						case 195:
							this.doguCombo2List[i] = new GUIContent("カード・スペードA");
							break;
						case 196:
							this.doguCombo2List[i] = new GUIContent("カード・スペード2");
							break;
						case 197:
							this.doguCombo2List[i] = new GUIContent("カード・スペード3");
							break;
						case 198:
							this.doguCombo2List[i] = new GUIContent("カード・スペード4");
							break;
						case 199:
							this.doguCombo2List[i] = new GUIContent("カード・スペード5");
							break;
						case 200:
							this.doguCombo2List[i] = new GUIContent("カード・スペード6");
							break;
						case 201:
							this.doguCombo2List[i] = new GUIContent("カード・スペード7");
							break;
						case 202:
							this.doguCombo2List[i] = new GUIContent("カード・スペード8");
							break;
						case 203:
							this.doguCombo2List[i] = new GUIContent("カード・スペード9");
							break;
						case 204:
							this.doguCombo2List[i] = new GUIContent("カード・スペード10");
							break;
						case 205:
							this.doguCombo2List[i] = new GUIContent("カード・スペードJ");
							break;
						case 206:
							this.doguCombo2List[i] = new GUIContent("カード・スペードQ");
							break;
						case 207:
							this.doguCombo2List[i] = new GUIContent("カード・スペードK");
							break;
						case 208:
							this.doguCombo2List[i] = new GUIContent("カード・ハートA");
							break;
						case 209:
							this.doguCombo2List[i] = new GUIContent("カード・ハート2");
							break;
						case 210:
							this.doguCombo2List[i] = new GUIContent("カード・ハート3");
							break;
						case 211:
							this.doguCombo2List[i] = new GUIContent("カード・ハート4");
							break;
						case 212:
							this.doguCombo2List[i] = new GUIContent("カード・ハート5");
							break;
						case 213:
							this.doguCombo2List[i] = new GUIContent("カード・ハート6");
							break;
						case 214:
							this.doguCombo2List[i] = new GUIContent("カード・ハート7");
							break;
						case 215:
							this.doguCombo2List[i] = new GUIContent("カード・ハート8");
							break;
						case 216:
							this.doguCombo2List[i] = new GUIContent("カード・ハート9");
							break;
						case 217:
							this.doguCombo2List[i] = new GUIContent("カード・ハート10");
							break;
						case 218:
							this.doguCombo2List[i] = new GUIContent("カード・ハートJ");
							break;
						case 219:
							this.doguCombo2List[i] = new GUIContent("カード・ハートQ");
							break;
						case 220:
							this.doguCombo2List[i] = new GUIContent("カード・ハートK");
							break;
						case 221:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤA");
							break;
						case 222:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ2");
							break;
						case 223:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ3");
							break;
						case 224:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ4");
							break;
						case 225:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ5");
							break;
						case 226:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ6");
							break;
						case 227:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ7");
							break;
						case 228:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ8");
							break;
						case 229:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ9");
							break;
						case 230:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤ10");
							break;
						case 231:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤJ");
							break;
						case 232:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤQ");
							break;
						case 233:
							this.doguCombo2List[i] = new GUIContent("カード・ダイヤK");
							break;
						case 234:
							this.doguCombo2List[i] = new GUIContent("カード・クラブA");
							break;
						case 235:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ2");
							break;
						case 236:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ3");
							break;
						case 237:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ4");
							break;
						case 238:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ5");
							break;
						case 239:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ6");
							break;
						case 240:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ7");
							break;
						case 241:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ8");
							break;
						case 242:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ9");
							break;
						case 243:
							this.doguCombo2List[i] = new GUIContent("カード・クラブ10");
							break;
						case 244:
							this.doguCombo2List[i] = new GUIContent("カード・クラブJ");
							break;
						case 245:
							this.doguCombo2List[i] = new GUIContent("カード・クラブQ");
							break;
						case 246:
							this.doguCombo2List[i] = new GUIContent("カード・クラブK");
							break;
						case 247:
							this.doguCombo2List[i] = new GUIContent("カード・ジョーカー");
							break;
						case 248:
							this.doguCombo2List[i] = new GUIContent("メイドねい人形");
							break;
						case 249:
							this.doguCombo2List[i] = new GUIContent("ハニー(青)");
							break;
						case 250:
							this.doguCombo2List[i] = new GUIContent("ハニー(茶)");
							break;
						case 251:
							this.doguCombo2List[i] = new GUIContent("ハニー(緑)");
							break;
						case 252:
							this.doguCombo2List[i] = new GUIContent("ハニー(赤)");
							break;
						case 253:
							this.doguCombo2List[i] = new GUIContent("缶ビール");
							break;
						case 254:
							this.doguCombo2List[i] = new GUIContent("ジョッキビール");
							break;
						case 255:
							this.doguCombo2List[i] = new GUIContent("ケーキ");
							break;
						case 256:
							this.doguCombo2List[i] = new GUIContent("フードプレート");
							break;
						case 257:
							this.doguCombo2List[i] = new GUIContent("焼きそば");
							break;
						case 258:
							this.doguCombo2List[i] = new GUIContent("ビーチボール(青)");
							break;
						case 259:
							this.doguCombo2List[i] = new GUIContent("ビーチボール(緑)");
							break;
						case 260:
							this.doguCombo2List[i] = new GUIContent("ビーチボール(赤)");
							break;
						case 261:
							this.doguCombo2List[i] = new GUIContent("ビーチボール(黄)");
							break;
						case 262:
							this.doguCombo2List[i] = new GUIContent("アイス(チョコミント)");
							break;
						case 263:
							this.doguCombo2List[i] = new GUIContent("アイス(ストロベリー)");
							break;
						case 264:
							this.doguCombo2List[i] = new GUIContent("アイス(バニラ)");
							break;
						case 265:
							this.doguCombo2List[i] = new GUIContent("メロン");
							break;
						case 266:
							this.doguCombo2List[i] = new GUIContent("シャチ");
							break;
						case 267:
							this.doguCombo2List[i] = new GUIContent("トロピカルアイスティー");
							break;
						case 268:
							this.doguCombo2List[i] = new GUIContent("お祓い棒");
							break;
						case 269:
							this.doguCombo2List[i] = new GUIContent("お守り");
							break;
						case 270:
							this.doguCombo2List[i] = new GUIContent("ススキ");
							break;
						case 271:
							this.doguCombo2List[i] = new GUIContent("竹帚");
							break;
						case 272:
							this.doguCombo2List[i] = new GUIContent("月見団子");
							break;
						case 273:
							this.doguCombo2List[i] = new GUIContent("藁人形");
							break;
						case 274:
							this.doguCombo2List[i] = new GUIContent("藁人形(釘)");
							break;
						case 275:
							this.doguCombo2List[i] = new GUIContent("ハニー");
							break;
						default:
							goto IL_4852;
					}
				IL_48D1:
					i++;
					continue;
				IL_4852:
					string text2 = this.doguB1Array[i];
					for (int j = 0; j < this.doguNameList.Count; j++)
					{
						string[] array = this.doguNameList[j].Split(new char[]
						{
							','
						});
						if (text2 == array[0])
						{
							text2 = array[1];
						}
					}
					this.doguCombo2List[i] = new GUIContent(text2);
					goto IL_48D1;
				}
				this.parCombo1.selectedItemIndex = 0;
				this.parCombo1List = new GUIContent[this.parArray1.Length];
				i = 0;
				while (i < this.parArray1.Length)
				{
					string text = this.parArray1[i];
					if (text == null)
					{
						goto IL_50A8;
					}
					//if (<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-4 == null)
					if (bgUiArrayB == null)
					{
						//<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-4 = new Dictionary<string, int>(46)
						bgUiArrayB = new Dictionary<string, int>(46)
						{
							{
								"Salon:63",
								0
							},
							{
								"Salon:65",
								1
							},
							{
								"Salon:69",
								2
							},
							{
								"Salon_Entrance:3",
								3
							},
							{
								"Salon_Entrance:4",
								4
							},
							{
								"Salon_Entrance:1",
								5
							},
							{
								"Salon_Entrance:2",
								6
							},
							{
								"Salon_Entrance:0",
								7
							},
							{
								"Pool:26",
								8
							},
							{
								"Shitsumu:23",
								9
							},
							{
								"Shitsumu_Night:23",
								10
							},
							{
								"OutletPark:54",
								11
							},
							{
								"HoneymoonRoom:102",
								12
							},
							{
								"mirror1",
								13
							},
							{
								"mirror2",
								14
							},
							{
								"mirror3",
								15
							},
							{
								"Mob_Man_Stand001",
								16
							},
							{
								"Mob_Man_Stand002",
								17
							},
							{
								"Mob_Man_Stand003",
								18
							},
							{
								"Mob_Man_Sit001",
								19
							},
							{
								"Mob_Man_Sit002",
								20
							},
							{
								"Mob_Man_Sit003",
								21
							},
							{
								"Mob_Girl_Stand001",
								22
							},
							{
								"Mob_Girl_Stand002",
								23
							},
							{
								"Mob_Girl_Stand003",
								24
							},
							{
								"Mob_Girl_Sit001",
								25
							},
							{
								"Mob_Girl_Sit002",
								26
							},
							{
								"Mob_Girl_Sit003",
								27
							},
							{
								"p_dance_star_photo_ver",
								28
							},
							{
								"p_kamihubuki_photo_ver",
								29
							},
							{
								"p_mizu001_photo_ver",
								30
							},
							{
								"p_powder_snow2_photo_ver",
								31
							},
							{
								"p_powder_snow_photo_ver",
								32
							},
							{
								"p_smoke_dance_photo_ver",
								33
							},
							{
								"p_soap_bubble01_photo_ver",
								34
							},
							{
								"p_soap_bukubuku_photo_ver",
								35
							},
							{
								"p_soap_photo_ver",
								36
							},
							{
								"p_steam001_photo_ver",
								37
							},
							{
								"p_steam002_photo_ver",
								38
							},
							{
								"p_steam_black_photo_ver",
								39
							},
							{
								"p_yuge_large_photo_ver",
								40
							},
							{
								"Particle/pLineP02",
								41
							},
							{
								"Particle/pLineY",
								42
							},
							{
								"Particle/pLine_act2",
								43
							},
							{
								"Particle/pstarY_act2",
								44
							},
							{
								"Particle/pHeart01",
								45
							}
						};
					}
					//if (!<PrivateImplementationDetails>{54936A6C-01FC-422B-BFCA-C5A294F216D4}.$$method0x6000015-4.TryGetValue(text, out num))
					//{
					//	goto IL_50A8;
					//}
					if (!bgUiArrayB.TryGetValue(text, out num))
					{
						goto IL_50A8;
					}
					switch (num)
					{
						case 0:
							this.parCombo1List[i] = new GUIContent("ステージライト(赤)");
							break;
						case 1:
							this.parCombo1List[i] = new GUIContent("ステージライト(黄)");
							break;
						case 2:
							this.parCombo1List[i] = new GUIContent("ステージライト(青)");
							break;
						case 3:
							this.parCombo1List[i] = new GUIContent("ドア(左)");
							break;
						case 4:
							this.parCombo1List[i] = new GUIContent("ドア(右)");
							break;
						case 5:
							this.parCombo1List[i] = new GUIContent("ホールドア(左)");
							break;
						case 6:
							this.parCombo1List[i] = new GUIContent("ホールドア(右)");
							break;
						case 7:
							this.parCombo1List[i] = new GUIContent("エントランス(扉無し)");
							break;
						case 8:
							this.parCombo1List[i] = new GUIContent("水面");
							break;
						case 9:
							this.parCombo1List[i] = new GUIContent("執務室(外・昼)");
							break;
						case 10:
							this.parCombo1List[i] = new GUIContent("執務室(外・夜)");
							break;
						case 11:
							this.parCombo1List[i] = new GUIContent("青空");
							break;
						case 12:
							this.parCombo1List[i] = new GUIContent("夜景");
							break;
						case 13:
							this.parCombo1List[i] = new GUIContent("鏡");
							break;
						case 14:
							this.parCombo1List[i] = new GUIContent("鏡(縦長)");
							break;
						case 15:
							this.parCombo1List[i] = new GUIContent("鏡(メイド部屋用)");
							break;
						case 16:
							this.parCombo1List[i] = new GUIContent("モブ男1");
							break;
						case 17:
							this.parCombo1List[i] = new GUIContent("モブ男2");
							break;
						case 18:
							this.parCombo1List[i] = new GUIContent("モブ男3");
							break;
						case 19:
							this.parCombo1List[i] = new GUIContent("モブ男1 座り");
							break;
						case 20:
							this.parCombo1List[i] = new GUIContent("モブ男2 座り");
							break;
						case 21:
							this.parCombo1List[i] = new GUIContent("モブ男3 座り");
							break;
						case 22:
							this.parCombo1List[i] = new GUIContent("モブ女1");
							break;
						case 23:
							this.parCombo1List[i] = new GUIContent("モブ女2");
							break;
						case 24:
							this.parCombo1List[i] = new GUIContent("モブ女3");
							break;
						case 25:
							this.parCombo1List[i] = new GUIContent("モブ女1 座り");
							break;
						case 26:
							this.parCombo1List[i] = new GUIContent("モブ女2 座り");
							break;
						case 27:
							this.parCombo1List[i] = new GUIContent("モブ女3 座り");
							break;
						case 28:
							this.parCombo1List[i] = new GUIContent("星");
							break;
						case 29:
							this.parCombo1List[i] = new GUIContent("紙吹雪");
							break;
						case 30:
							this.parCombo1List[i] = new GUIContent("水");
							break;
						case 31:
							this.parCombo1List[i] = new GUIContent("粉雪2");
							break;
						case 32:
							this.parCombo1List[i] = new GUIContent("粉雪");
							break;
						case 33:
							this.parCombo1List[i] = new GUIContent("煙");
							break;
						case 34:
							this.parCombo1List[i] = new GUIContent("泡(空間)");
							break;
						case 35:
							this.parCombo1List[i] = new GUIContent("泡");
							break;
						case 36:
							this.parCombo1List[i] = new GUIContent("手元の泡");
							break;
						case 37:
							this.parCombo1List[i] = new GUIContent("湯気1");
							break;
						case 38:
							this.parCombo1List[i] = new GUIContent("スチーム");
							break;
						case 39:
							this.parCombo1List[i] = new GUIContent("スチーム(黒)");
							break;
						case 40:
							this.parCombo1List[i] = new GUIContent("湯気2");
							break;
						case 41:
							this.parCombo1List[i] = new GUIContent("ライン：ハート");
							break;
						case 42:
							this.parCombo1List[i] = new GUIContent("ライン：星");
							break;
						case 43:
							this.parCombo1List[i] = new GUIContent("星2");
							break;
						case 44:
							this.parCombo1List[i] = new GUIContent("流れ星");
							break;
						case 45:
							this.parCombo1List[i] = new GUIContent("ハート");
							break;
						default:
							goto IL_50A8;
					}
				IL_50C1:
					i++;
					continue;
				IL_50A8:
					this.parCombo1List[i] = new GUIContent(this.parArray1[i]);
					goto IL_50C1;
				}
				this.parCombo.selectedItemIndex = 0;
				this.parComboList = new GUIContent[this.parArray.Length];
				for (i = 0; i < this.parArray.Length; i++)
				{
					string text3 = this.parArray[i];
					this.parComboList[i] = new GUIContent(this.parArray[i]);
				}
				this.lightCombo.selectedItemIndex = 0;
				this.lightList = new List<GameObject>();
				this.lightList.Add(GameMain.Instance.MainLight.gameObject);
				this.lightComboList = new GUIContent[this.lightList.Count];
				for (i = 0; i < this.lightList.Count; i++)
				{
					if (i == 0)
					{
						this.lightComboList[i] = new GUIContent("メイン");
					}
					else
					{
						this.lightComboList[i] = new GUIContent("追加" + i);
					}
				}
				this.kankyoCombo.selectedItemIndex = 0;
				this.kankyoComboList = new GUIContent[this.kankyoMax];
				for (i = 0; i < this.kankyoMax; i++)
				{
					IniKey iniKey = base.Preferences["kankyo"]["kankyo" + (i + 1)];
					this.kankyoComboList[i] = new GUIContent(iniKey.Value);
				}
				this.kankyoInitFlg = true;
			}
			this.listStyle3.padding.top = this.GetPix(1);
			this.listStyle3.padding.bottom = this.GetPix(0);
			this.listStyle3.fontSize = this.GetPix(12);
			if (this.poseCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.poseGroupCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.sceneLevel == 3 || this.sceneLevel == 5 || this.isF6)
			{
				if (!this.isF6)
				{
					bool value = true;
					if (this.faceFlg || this.poseFlg || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
					{
						value = false;
					}
					if (GUI.Toggle(new Rect((float)this.GetPix(2), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), value, "配置", guistyle6))
					{
						this.faceFlg = false;
						this.poseFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
						this.bGui = true;
						this.isGuiInit = true;
					}
				}
				if (!this.yotogiFlg)
				{
					if (GUI.Toggle(new Rect((float)this.GetPix(42), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.poseFlg, "操作", guistyle6))
					{
						this.poseFlg = true;
						this.faceFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
					}
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(82), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.faceFlg, "表情", guistyle6))
				{
					this.faceFlg = true;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					if (!this.faceFlg2)
					{
						this.isFaceInit = true;
						this.faceFlg2 = true;
						this.maidArray[this.selectMaidIndex].boMabataki = false;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					this.isFaceInit = true;
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(122), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyoFlg, "環境", guistyle6))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = true;
					this.kankyo2Flg = false;
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(162), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyo2Flg, "環2", guistyle6))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = true;
				}
				if (!this.line1)
				{
					this.line1 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.3f));
					this.line2 = this.MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 0.6f));
				}
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(200), 2f), this.line1);
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(200), 1f), this.line2);
			}
			if (this.isDanceStop)
			{
				this.isStop[this.selectMaidIndex] = true;
				this.isDanceStop = false;
			}
			this.yotogiFlg = false;
			if (this.sceneLevel == 14)
			{
				if (GameObject.Find("/UI Root/YotogiPlayPanel/CommandViewer/SkillViewer/MaskGroup/SkillGroup/CommandParent/CommandUnit"))
				{
					this.yotogiFlg = true;
				}
			}
			if (!this.isF6)
			{
				if (GUI.Button(new Rect((float)this.GetPix(157), (float)this.GetPix(32), (float)this.GetPix(46), (float)this.GetPix(35)), "シーン\n 管 理", guistyle3))
				{
					this.sceneFlg = true;
					this.faceFlg = false;
					this.poseFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					this.bGui = true;
					int i = 0;
					while (i < 10)
					{
						this.date[i] = "未保存";
						this.ninzu[i] = "";
						string path = string.Concat(new object[]
						{
							Path.GetFullPath(".\\"),
							"Mod\\MultipleMaidsScene\\",
							this.page * 10 + i + 1,
							".png"
						});
						if (File.Exists(path))
						{
							FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
							BinaryReader binaryReader = new BinaryReader(input);
							byte[] array2 = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
							byte[] value2 = new byte[]
							{
								array2[36],
								array2[35],
								array2[34],
								array2[33]
							};
							int count = BitConverter.ToInt32(value2, 0) - 8;
							byte[] bytes = array2.Skip(49).Take(count).ToArray<byte>();
							string text4 = Encoding.UTF8.GetString(bytes);
							text4 = MultipleMaids.StringFromBase64Comp(text4);
							if (text4 != "")
							{
								string[] array3 = text4.Split(new char[]
								{
									'_'
								});
								if (array3.Length >= 2)
								{
									string[] array4 = array3[0].Split(new char[]
									{
										','
									});
									this.date[i] = array4[0];
									this.ninzu[i] = array4[1] + "人";
								}
							}
						}
						else
						{
							IniKey iniKey2 = base.Preferences["scene"]["s" + (this.page * 10 + i + 1)];
							if (iniKey2.Value != null && iniKey2.Value.ToString() != "")
							{
								string[] array3 = iniKey2.Value.Split(new char[]
								{
									'_'
								});
								if (array3.Length >= 2)
								{
									string[] array4 = array3[0].Split(new char[]
									{
										','
									});
									this.date[i] = array4[0];
									this.ninzu[i] = array4[1] + "人";
								}
							}
						}
						//IL_59EB:
						i++;
						continue;
						//goto IL_59EB;
					}
				}
			}
			if (this.parCombo.isClickedComboButton || this.bgCombo.isClickedComboButton || this.bgmCombo.isClickedComboButton || this.lightCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			int num2 = -1;
			if (this.lightIndex[this.selectLightIndex] == 0)
			{
				this.isIdx1 = true;
			}
			if (this.lightIndex[this.selectLightIndex] == 1)
			{
				this.isIdx2 = true;
			}
			if (this.lightIndex[this.selectLightIndex] == 2)
			{
				this.isIdx3 = true;
			}
			if (this.lightIndex[this.selectLightIndex] == 3)
			{
				this.isIdx4 = true;
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(185), (float)this.GetPix(45), (float)this.GetPix(20)), this.isIdx1, "通常", guistyle6))
			{
				if (this.lightIndex[this.selectLightIndex] != 0)
				{
					this.isIdx1 = true;
					this.isIdx2 = false;
					this.isIdx3 = false;
					this.isIdx4 = false;
					num2 = 0;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(50), (float)this.GetPix(185), (float)this.GetPix(45), (float)this.GetPix(20)), this.isIdx2, "Spot", guistyle6))
			{
				if (this.lightIndex[this.selectLightIndex] != 1)
				{
					this.isIdx1 = false;
					this.isIdx2 = true;
					this.isIdx3 = false;
					this.isIdx4 = false;
					num2 = 1;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(99), (float)this.GetPix(185), (float)this.GetPix(45), (float)this.GetPix(20)), this.isIdx3, "Point", guistyle6))
			{
				if (this.lightIndex[this.selectLightIndex] != 2)
				{
					this.isIdx1 = false;
					this.isIdx2 = false;
					this.isIdx3 = true;
					this.isIdx4 = false;
					num2 = 2;
				}
			}
			if (this.selectLightIndex == 0)
			{
				if (GUI.Toggle(new Rect((float)this.GetPix(150), (float)this.GetPix(185), (float)this.GetPix(45), (float)this.GetPix(20)), this.isIdx4, "単色", guistyle6))
				{
					if (this.lightIndex[this.selectLightIndex] != 3)
					{
						this.isIdx1 = false;
						this.isIdx2 = false;
						this.isIdx3 = false;
						this.isIdx4 = true;
						num2 = 3;
					}
				}
			}
			else if (GUI.Toggle(new Rect((float)this.GetPix(150), (float)this.GetPix(185), (float)this.GetPix(45), (float)this.GetPix(20)), this.isIdx4, "無効", guistyle6))
			{
				if (this.lightIndex[this.selectLightIndex] != 3)
				{
					this.isIdx1 = false;
					this.isIdx2 = false;
					this.isIdx3 = false;
					this.isIdx4 = true;
					num2 = 3;
				}
			}
			if (num2 >= 0)
			{
				this.lightIndex[this.selectLightIndex] = num2;
				if (this.selectLightIndex == 0)
				{
					GameMain.Instance.MainLight.Reset();
					GameMain.Instance.MainLight.SetIntensity(0.95f);
					GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 50f;
					GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
					GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
					if (this.lightIndex[this.selectLightIndex] == 0)
					{
						GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
						this.bgObject.SetActive(true);
						this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
					}
					else if (this.lightIndex[this.selectLightIndex] == 1)
					{
						GameMain.Instance.MainLight.transform.eulerAngles += Vector3.right * 40f;
						GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Spot;
						this.bgObject.SetActive(true);
						this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
					}
					else if (this.lightIndex[this.selectLightIndex] == 2)
					{
						GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Point;
						this.bgObject.SetActive(true);
						this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
					}
					else if (this.lightIndex[this.selectLightIndex] == 3)
					{
						GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
						this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
						this.bgObject.SetActive(false);
					}
				}
				else
				{
					this.lightList[this.selectLightIndex].SetActive(true);
					if (this.lightIndex[this.selectLightIndex] == 0)
					{
						this.lightList[this.selectLightIndex].GetComponent<Light>().type = LightType.Directional;
					}
					else if (this.lightIndex[this.selectLightIndex] == 1)
					{
						this.lightList[this.selectLightIndex].transform.eulerAngles += Vector3.right * 40f;
						this.lightList[this.selectLightIndex].GetComponent<Light>().type = LightType.Spot;
					}
					else if (this.lightIndex[this.selectLightIndex] == 2)
					{
						this.lightList[this.selectLightIndex].GetComponent<Light>().type = LightType.Point;
					}
					else if (this.lightIndex[this.selectLightIndex] == 3)
					{
						this.lightList[this.selectLightIndex].GetComponent<Light>().type = LightType.Directional;
						this.lightList[this.selectLightIndex].SetActive(false);
					}
				}
				this.lightColorR[this.selectLightIndex] = 1f;
				this.lightColorG[this.selectLightIndex] = 1f;
				this.lightColorB[this.selectLightIndex] = 1f;
				this.lightX[this.selectLightIndex] = 40f;
				this.lightY[this.selectLightIndex] = 180f;
				this.lightAkarusa[this.selectLightIndex] = 0.95f;
				this.lightKage[this.selectLightIndex] = 0.098f;
				this.lightRange[this.selectLightIndex] = 50f;
				if (this.lightIndex[this.selectLightIndex] == 1)
				{
					this.lightX[this.selectLightIndex] = 90f;
				}
			}
			GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(108), (float)this.GetPix(100), (float)this.GetPix(25)), "キューブ表示", guistyle2);
			guistyle6.fontSize = this.GetPix(12);
			this.isCube2 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(123), (float)this.GetPix(55), (float)this.GetPix(20)), this.isCube2, "大道具(", guistyle6);
			if (!this.isF6)
			{
				this.isCube = GUI.Toggle(new Rect((float)this.GetPix(102), (float)this.GetPix(123), (float)this.GetPix(54), (float)this.GetPix(20)), this.isCube, "メイド", guistyle6);
			}
			bool flag = GUI.Toggle(new Rect((float)this.GetPix(160), (float)this.GetPix(123), (float)this.GetPix(44), (float)this.GetPix(20)), this.isCube3, "背景", guistyle6);
			guistyle6.fontSize = this.GetPix(13);
			bool flag2 = GUI.Toggle(new Rect((float)this.GetPix(61), (float)this.GetPix(123), (float)this.GetPix(38), (float)this.GetPix(20)), this.isCubeS, "小)", guistyle6);
			if (this.isCubeS != flag2)
			{
				this.isCubeS = flag2;
				if (this.isCubeS)
				{
					this.cubeSize = 0.05f;
				}
				else
				{
					this.cubeSize = 0.12f;
				}
				for (int i = 0; i < this.doguBObject.Count; i++)
				{
					this.gDogu[i].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
				}
			}
			if (this.isCube3 != flag)
			{
				this.isCube3 = flag;
				if (this.gBg == null)
				{
					this.gBg = GameObject.CreatePrimitive(PrimitiveType.Cube);
					this.gBg.GetComponent<Renderer>().material = this.m_material;
					this.gBg.layer = 8;
					this.gBg.GetComponent<Renderer>().enabled = false;
					this.gBg.SetActive(false);
					this.gBg.transform.position = this.bgObject.transform.position;
					this.mBg = this.gBg.AddComponent<MouseDrag6>();
					this.mBg.obj = this.gBg;
					this.mBg.maid = this.bgObject;
					this.mBg.angles = this.bg.eulerAngles;
					this.gBg.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
					this.mBg.ido = 1;
					this.mBg.isScale = false;
				}
				if (this.isCube3)
				{
					this.gBg.SetActive(true);
				}
				else
				{
					this.gBg.SetActive(false);
				}
			}
			int num3 = 0;
			if (this.lightIndex[this.selectLightIndex] == 0 || this.lightIndex[this.selectLightIndex] == 1 || (this.selectLightIndex == 0 && this.lightIndex[this.selectLightIndex] == 3))
			{
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(207), (float)this.GetPix(100), (float)this.GetPix(25)), "向きX", guistyle2);
				this.lightX[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(221), (float)this.GetPix(192), (float)this.GetPix(20)), this.lightX[this.selectLightIndex], 220f, -140f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(231), (float)this.GetPix(100), (float)this.GetPix(25)), "向きY", guistyle2);
				this.lightY[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(245), (float)this.GetPix(192), (float)this.GetPix(20)), this.lightY[this.selectLightIndex], 0f, 360f);
			}
			else
			{
				num3 = 50;
			}
			if (this.lightIndex[this.selectLightIndex] != 3 || this.selectLightIndex <= 0)
			{
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(255 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "明るさ", guistyle2);
				this.lightAkarusa[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(269 - num3), (float)this.GetPix(192), (float)this.GetPix(20)), this.lightAkarusa[this.selectLightIndex], 0f, 1.9f);
				if (this.lightIndex[this.selectLightIndex] == 0 || this.lightIndex[this.selectLightIndex] == 3)
				{
					if (this.selectLightIndex == 0)
					{
						GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(279 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "影", guistyle2);
						this.lightKage[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(293 - num3), (float)this.GetPix(192), (float)this.GetPix(20)), this.lightKage[this.selectLightIndex], 0f, 1f);
					}
					else
					{
						num3 = 25;
					}
				}
				else if (this.lightIndex[this.selectLightIndex] == 1 || this.lightIndex[this.selectLightIndex] == 2)
				{
					GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(281 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "範囲", guistyle2);
					this.lightRange[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(295 - num3), (float)this.GetPix(192), (float)this.GetPix(20)), this.lightRange[this.selectLightIndex], 0f, 150f);
				}
				else
				{
					num3 = 75;
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(303 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "赤", guistyle2);
				this.lightColorR[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(317 - num3), (float)this.GetPix(92), (float)this.GetPix(20)), this.lightColorR[this.selectLightIndex], 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(303 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "緑", guistyle2);
				this.lightColorG[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(317 - num3), (float)this.GetPix(92), (float)this.GetPix(20)), this.lightColorG[this.selectLightIndex], 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(327 - num3), (float)this.GetPix(100), (float)this.GetPix(25)), "青", guistyle2);
				this.lightColorB[this.selectLightIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(341 - num3), (float)this.GetPix(92), (float)this.GetPix(20)), this.lightColorB[this.selectLightIndex], 0f, 1f);
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(361), (float)this.GetPix(67), (float)this.GetPix(20)), this.isBloomS, "ブルーム", guistyle6))
			{
				this.isBloomS = true;
				this.isDepthS = false;
				this.isBlurS = false;
				this.isFogS = false;
			}
			if (this.isBloomS)
			{
				this.isBloom = GUI.Toggle(new Rect((float)this.GetPix(8), (float)this.GetPix(382), (float)this.GetPix(40), (float)this.GetPix(20)), this.isBloom, "有効", guistyle6);
				if (!this.isBloom)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "強さ", guistyle2);
				this.bloom1 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.bloom1, 0f, 5.7f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "広さ", guistyle2);
				this.bloom2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.bloom2, 0f, 15f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "赤", guistyle2);
				this.bloom3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.bloom3, 0f, 0.5f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "緑", guistyle2);
				this.bloom4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.bloom4, 0f, 0.5f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(450), (float)this.GetPix(100), (float)this.GetPix(25)), "青", guistyle2);
				this.bloom5 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(465), (float)this.GetPix(92), (float)this.GetPix(20)), this.bloom5, 0f, 0.5f);
				this.isBloomA = GUI.Toggle(new Rect((float)this.GetPix(110), (float)this.GetPix(461), (float)this.GetPix(50), (float)this.GetPix(20)), this.isBloomA, "HDR", guistyle6);
				if (!this.parCombo.isClickedComboButton && !this.bgCombo.isClickedComboButton && !this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = true;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(73), (float)this.GetPix(361), (float)this.GetPix(40), (float)this.GetPix(20)), this.isDepthS, "深度", guistyle6))
			{
				this.isBloomS = false;
				this.isDepthS = true;
				this.isBlurS = false;
				this.isFogS = false;
			}
			if (this.isDepthS)
			{
				this.isDepth = GUI.Toggle(new Rect((float)this.GetPix(8), (float)this.GetPix(382), (float)this.GetPix(40), (float)this.GetPix(20)), this.isDepth, "有効", guistyle6);
				if (!this.isDepth)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "焦点距離", guistyle2);
				this.depth1 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(415), (float)this.GetPix(192), (float)this.GetPix(20)), this.depth1, 0f, 10f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "焦点領域サイズ", guistyle2);
				this.depth2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.depth2, 0f, 2f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "カメラ絞り", guistyle2);
				this.depth3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.depth3, 0f, 60f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(450), (float)this.GetPix(100), (float)this.GetPix(25)), "ブレ", guistyle2);
				this.depth4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(465), (float)this.GetPix(92), (float)this.GetPix(20)), this.depth4, 0f, 10f);
				this.isDepthA = GUI.Toggle(new Rect((float)this.GetPix(110), (float)this.GetPix(461), (float)this.GetPix(100), (float)this.GetPix(20)), this.isDepthA, "深度表示", guistyle6);
				if (!this.parCombo.isClickedComboButton && !this.bgCombo.isClickedComboButton && !this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = true;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(114), (float)this.GetPix(361), (float)this.GetPix(40), (float)this.GetPix(20)), this.isBlurS, "光学", guistyle6))
			{
				this.isBloomS = false;
				this.isDepthS = false;
				this.isBlurS = true;
				this.isFogS = false;
			}
			if (this.isBlurS)
			{
				this.isBlur = GUI.Toggle(new Rect((float)this.GetPix(8), (float)this.GetPix(382), (float)this.GetPix(40), (float)this.GetPix(20)), this.isBlur, "有効", guistyle6);
				if (!this.isBlur)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "強さ", guistyle2);
				this.blur1 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.blur1, -40f, 70f);
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "ブラー", guistyle2);
				this.blur2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.blur2, 0f, 5f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "ブレ", guistyle2);
				this.blur3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.blur3, 0f, 40f);
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "色収差", guistyle2);
				this.blur4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.blur4, -30f, 30f);
				if (!this.parCombo.isClickedComboButton && !this.bgCombo.isClickedComboButton && !this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = true;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(155), (float)this.GetPix(361), (float)this.GetPix(60), (float)this.GetPix(20)), this.isFogS, "フォグ", guistyle6))
			{
				this.isBloomS = false;
				this.isDepthS = false;
				this.isBlurS = false;
				this.isFogS = true;
			}
			if (this.isFogS)
			{
				this.isFog = GUI.Toggle(new Rect((float)this.GetPix(8), (float)this.GetPix(382), (float)this.GetPix(40), (float)this.GetPix(20)), this.isFog, "有効", guistyle6);
				if (!this.isFog)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(376), (float)this.GetPix(100), (float)this.GetPix(24)), "発生距離", guistyle2);
				this.fog1 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(390), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog1, 0f, 30f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "密度", guistyle2);
				this.fog2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog2, 0f, 10f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(400), (float)this.GetPix(100), (float)this.GetPix(25)), "強度", guistyle2);
				this.fog3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(415), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog3, -5f, 20f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "高さ", guistyle2);
				this.fog4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog4, -10f, 10f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(425), (float)this.GetPix(100), (float)this.GetPix(25)), "赤", guistyle2);
				this.fog5 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(440), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog5, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(450), (float)this.GetPix(100), (float)this.GetPix(25)), "緑", guistyle2);
				this.fog6 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(465), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog6, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(110), (float)this.GetPix(450), (float)this.GetPix(100), (float)this.GetPix(25)), "青", guistyle2);
				this.fog7 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(465), (float)this.GetPix(92), (float)this.GetPix(20)), this.fog7, 0f, 1f);
				if (!this.parCombo.isClickedComboButton && !this.bgCombo.isClickedComboButton && !this.bgmCombo.isClickedComboButton)
				{
					GUI.enabled = true;
				}
			}
			this.isSepian = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(485), (float)this.GetPix(80), (float)this.GetPix(20)), this.isSepian, "セピア", guistyle6);
			GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(482), (float)this.GetPix(100), (float)this.GetPix(25)), "ぼかし", guistyle2);
			this.bokashi = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(497), (float)this.GetPix(92), (float)this.GetPix(20)), this.bokashi, 0f, 18f);
			if (GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(515), (float)this.GetPix(80), (float)this.GetPix(20)), this.isHairSetting, "髪の設定", guistyle6))
			{
				this.isHairSetting = true;
				this.isSkirtSetting = false;
			}
			if (this.isHairSetting)
			{
				bool flag3 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(536), (float)this.GetPix(40), (float)this.GetPix(20)), this.isKamiyure, "有効", guistyle6);
				if (this.isKamiyure != flag3)
				{
					this.isKamiyure = flag3;
					if (this.isKamiyure)
					{
						base.Preferences["config"]["hair_setting"].Value = "true";
						base.Preferences["config"]["hair_radius"].Value = this.kamiyure4.ToString();
						base.Preferences["config"]["hair_elasticity"].Value = this.kamiyure3.ToString();
						base.Preferences["config"]["hair_damping"].Value = this.kamiyure2.ToString();
						base.SaveConfig();
					}
					else
					{
						base.Preferences["config"]["hair_setting"].Value = "false";
						base.SaveConfig();
						for (int k = 0; k < this.maidCnt; k++)
						{
							for (int l = 0; l < this.maidArray[k].body0.goSlot.Count; l++)
							{
								if (l >= 3 && l <= 6)
								{
									if (this.maidArray[k].body0.goSlot[l].obj != null)
									{
										DynamicBone component = this.maidArray[k].body0.goSlot[l].obj.GetComponent<DynamicBone>();
										if (component != null)
										{
											component.m_Damping = 0.6f;
											component.m_Elasticity = 1f;
											if (l == 5)
											{
												component.m_Elasticity = 0.05f;
											}
											component.m_Radius = 0.02f;
											component.UpdateParameters();
										}
									}
								}
							}
						}
					}
				}
				if (!this.isKamiyure)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(535), (float)this.GetPix(100), (float)this.GetPix(25)), "当たり判定半径", guistyle2);
				float num4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(549), (float)this.GetPix(92), (float)this.GetPix(20)), this.kamiyure4, 0f, 0.04f);
				if (this.kamiyure4 != num4)
				{
					this.kamiyure4 = num4;
					base.Preferences["config"]["hair_radius"].Value = this.kamiyure4.ToString();
					base.SaveConfig();
				}
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(560), (float)this.GetPix(100), (float)this.GetPix(25)), "減衰率", guistyle2);
				float num5 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(574), (float)this.GetPix(92), (float)this.GetPix(20)), this.kamiyure2, 0.2f, 1f);
				if (this.kamiyure2 != num5)
				{
					this.kamiyure2 = num5;
					base.Preferences["config"]["hair_damping"].Value = this.kamiyure2.ToString();
					base.SaveConfig();
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(560), (float)this.GetPix(100), (float)this.GetPix(25)), "復元率", guistyle2);
				float num6 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(574), (float)this.GetPix(92), (float)this.GetPix(20)), this.kamiyure3, 0f, 2f);
				if (this.kamiyure3 != num6)
				{
					this.kamiyure3 = num6;
					base.Preferences["config"]["hair_elasticity"].Value = this.kamiyure3.ToString();
					base.SaveConfig();
				}
				GUI.enabled = true;
			}
			if (this.bgCombo.isClickedComboButton || this.bgmCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(105), (float)this.GetPix(515), (float)this.GetPix(100), (float)this.GetPix(20)), this.isSkirtSetting, "スカート設定", guistyle6))
			{
				this.isHairSetting = false;
				this.isSkirtSetting = true;
			}
			if (this.isSkirtSetting)
			{
				bool flag4 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(536), (float)this.GetPix(40), (float)this.GetPix(20)), this.isSkirtyure, "有効", guistyle6);
				if (this.isSkirtyure != flag4)
				{
					this.isSkirtyure = flag4;
					if (this.isSkirtyure)
					{
						base.Preferences["config"]["skirt_setting"].Value = "true";
						base.Preferences["config"]["skirt_radius"].Value = this.skirtyure4.ToString();
						base.Preferences["config"]["skirt_elasticity"].Value = this.skirtyure3.ToString();
						base.Preferences["config"]["skirt_damping"].Value = this.skirtyure2.ToString();
						base.SaveConfig();
					}
					else
					{
						base.Preferences["config"]["skirt_setting"].Value = "false";
						base.SaveConfig();
						for (int k = 0; k < this.maidCnt; k++)
						{
							for (int l = 0; l < this.maidArray[k].body0.goSlot.Count; l++)
							{
								if (this.maidArray[k].body0.goSlot[l].obj != null)
								{
									DynamicSkirtBone fieldValue = MultipleMaids.GetFieldValue<BoneHair3, DynamicSkirtBone>(this.maidArray[k].body0.goSlot[l].bonehair3, "m_SkirtBone");
									if (fieldValue != null)
									{
										fieldValue.m_fPanierForce = 0.05f;
										fieldValue.m_fPanierForceDistanceThreshold = 0.1f;
										fieldValue.m_fRegDefaultRadius = 0.1f;
									}
								}
							}
						}
					}
				}
				if (!this.isSkirtyure)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(535), (float)this.GetPix(100), (float)this.GetPix(25)), "足側カプセル半径", guistyle2);
				float num4 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(549), (float)this.GetPix(92), (float)this.GetPix(20)), this.skirtyure4, 0f, 0.2f);
				if (this.skirtyure4 != num4)
				{
					this.skirtyure4 = num4;
					base.Preferences["config"]["skirt_radius"].Value = this.skirtyure4.ToString();
					base.SaveConfig();
				}
				GUI.Label(new Rect((float)this.GetPix(108), (float)this.GetPix(560), (float)this.GetPix(100), (float)this.GetPix(25)), "足との距離パニエ力", guistyle2);
				float num5 = GUI.HorizontalSlider(new Rect((float)this.GetPix(108), (float)this.GetPix(574), (float)this.GetPix(92), (float)this.GetPix(20)), this.skirtyure2, 0f, 0.2f);
				if (this.skirtyure2 != num5)
				{
					this.skirtyure2 = num5;
					base.Preferences["config"]["skirt_damping"].Value = this.skirtyure2.ToString();
					base.SaveConfig();
				}
				GUI.Label(new Rect((float)this.GetPix(10), (float)this.GetPix(560), (float)this.GetPix(100), (float)this.GetPix(25)), "パニエ力", guistyle2);
				float num6 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(574), (float)this.GetPix(92), (float)this.GetPix(20)), this.skirtyure3, 0f, 0.1f);
				if (this.skirtyure3 != num6)
				{
					this.skirtyure3 = num6;
					base.Preferences["config"]["skirt_elasticity"].Value = this.skirtyure3.ToString();
					base.SaveConfig();
				}
				GUI.enabled = true;
			}
			GUI.enabled = true;
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(359), (float)this.GetPix(195), 2f), this.line1);
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(359), (float)this.GetPix(195), 1f), this.line2);
			if (this.bgCombo.isClickedComboButton || this.bgmCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(146), (float)this.GetPix(195), 2f), this.line1);
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(146), (float)this.GetPix(195), 1f), this.line2);
			GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(157), (float)this.GetPix(100), (float)this.GetPix(25)), "照明", guistyle2);
			this.listStyle3.padding.top = this.GetPix(3);
			this.listStyle3.padding.bottom = this.GetPix(2);
			this.listStyle3.fontSize = this.GetPix(13);
			int num7 = this.lightCombo.List(new Rect((float)this.GetPix(34), (float)this.GetPix(155), (float)this.GetPix(78), (float)this.GetPix(23)), this.lightComboList[this.selectLightIndex].text, this.lightComboList, guistyle4, "box", this.listStyle3);
			if (num7 != this.selectLightIndex)
			{
				this.selectLightIndex = num7;
				this.isIdx1 = false;
				this.isIdx2 = false;
				this.isIdx3 = false;
				this.isIdx4 = false;
			}
			if (GUI.Button(new Rect((float)this.GetPix(115), (float)this.GetPix(155), (float)this.GetPix(35), (float)this.GetPix(23)), "追加", guistyle3))
			{
				GameObject gameObject = new GameObject("Light");
				gameObject.AddComponent<Light>();
				this.lightList.Add(gameObject);
				this.lightColorR.Add(1f);
				this.lightColorG.Add(1f);
				this.lightColorB.Add(1f);
				this.lightIndex.Add(0);
				this.lightX.Add(40f);
				this.lightY.Add(180f);
				this.lightAkarusa.Add(0.95f);
				this.lightKage.Add(0.098f);
				this.lightRange.Add(50f);
				gameObject.transform.position = GameMain.Instance.MainLight.transform.position;
				this.selectLightIndex = this.lightList.Count - 1;
				this.lightComboList = new GUIContent[this.lightList.Count];
				for (int i = 0; i < this.lightList.Count; i++)
				{
					if (i == 0)
					{
						this.lightComboList[i] = new GUIContent("メイン");
					}
					else
					{
						this.lightComboList[i] = new GUIContent("追加" + i);
					}
				}
				this.lightCombo.selectedItemIndex = this.selectLightIndex;
				gameObject.GetComponent<Light>().intensity = 0.95f;
				gameObject.GetComponent<Light>().spotAngle = 50f;
				gameObject.GetComponent<Light>().range = 10f;
				gameObject.GetComponent<Light>().type = LightType.Directional;
				gameObject.GetComponent<Light>().color = new Color(0.5f, 1f, 0f);
				if (this.gLight[this.selectLightIndex] == null)
				{
					this.gLight[this.selectLightIndex] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Material material = new Material(Shader.Find("Transparent/Diffuse"));
					material.color = new Color(0.5f, 0.5f, 1f, 0.8f);
					this.gLight[this.selectLightIndex].GetComponent<Renderer>().material = material;
					this.gLight[this.selectLightIndex].layer = 8;
					this.gLight[this.selectLightIndex].GetComponent<Renderer>().enabled = false;
					this.gLight[this.selectLightIndex].SetActive(false);
					this.gLight[this.selectLightIndex].transform.position = gameObject.transform.position;
					this.mLight[this.selectLightIndex] = this.gLight[this.selectLightIndex].AddComponent<MouseDrag6>();
					this.mLight[this.selectLightIndex].obj = this.gLight[this.selectLightIndex];
					this.mLight[this.selectLightIndex].maid = gameObject.gameObject;
					this.mLight[this.selectLightIndex].angles = gameObject.gameObject.transform.eulerAngles;
					this.gLight[this.selectLightIndex].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
					this.mLight[this.selectLightIndex].ido = 1;
					this.mLight[this.selectLightIndex].isScale = false;
				}
			}
			if (GUI.Button(new Rect((float)this.GetPix(153), (float)this.GetPix(155), (float)this.GetPix(23), (float)this.GetPix(23)), "R", guistyle3))
			{
				for (int i = 1; i < this.lightList.Count; i++)
				{
					UnityEngine.Object.Destroy(this.lightList[i]);
				}
				this.lightIndex = new List<int>();
				this.lightIndex.Add(0);
				this.lightColorR = new List<float>();
				this.lightColorR.Add(1f);
				this.lightColorG = new List<float>();
				this.lightColorG.Add(1f);
				this.lightColorB = new List<float>();
				this.lightColorB.Add(1f);
				this.lightX = new List<float>();
				this.lightX.Add(40f);
				this.lightY = new List<float>();
				this.lightY.Add(180f);
				this.lightAkarusa = new List<float>();
				this.lightAkarusa.Add(0.95f);
				this.lightKage = new List<float>();
				this.lightKage.Add(0.098f);
				this.lightRange = new List<float>();
				this.lightRange.Add(50f);
				this.lightCombo.selectedItemIndex = 0;
				this.lightList = new List<GameObject>();
				this.lightList.Add(GameMain.Instance.MainLight.gameObject);
				this.lightComboList = new GUIContent[this.lightList.Count];
				for (int i = 0; i < this.lightList.Count; i++)
				{
					if (i == 0)
					{
						this.lightComboList[i] = new GUIContent("メイン");
					}
					else
					{
						this.lightComboList[i] = new GUIContent("追加" + i);
					}
				}
				this.selectLightIndex = 0;
				GameMain.Instance.MainLight.Reset();
				GameMain.Instance.MainLight.SetIntensity(0.95f);
				GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 50f;
				GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
				GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
				GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
				this.bgObject.SetActive(true);
				this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
				this.isIdx1 = false;
				this.isIdx2 = false;
				this.isIdx3 = false;
				this.isIdx4 = false;
			}
			GUI.enabled = true;
			if (this.bgCombo.isClickedComboButton || this.bgmCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			this.listStyle3.padding.top = this.GetPix(0);
			this.listStyle3.padding.bottom = this.GetPix(0);
			this.listStyle3.fontSize = this.GetPix(12);
			if (this.nameFlg)
			{
				this.inName2 = GUI.TextField(new Rect((float)this.GetPix(5), (float)this.GetPix(86), (float)this.GetPix(100), (float)this.GetPix(20)), this.inName2);
				if (GUI.Button(new Rect((float)this.GetPix(110), (float)this.GetPix(86), (float)this.GetPix(35), (float)this.GetPix(20)), "更新", guistyle3))
				{
					this.nameFlg = false;
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					base.Preferences["kankyo"]["kankyo" + (this.kankyoCombo.selectedItemIndex + 1)].Value = this.inName2;
					base.SaveConfig();
					this.kankyoComboList = new GUIContent[this.kankyoMax];
					for (int i = 0; i < this.kankyoMax; i++)
					{
						IniKey iniKey = base.Preferences["kankyo"]["kankyo" + (i + 1)];
						this.kankyoComboList[i] = new GUIContent(iniKey.Value);
					}
				}
			}
			else
			{
				if (GUI.Button(new Rect((float)this.GetPix(180), (float)this.GetPix(86), (float)this.GetPix(24), (float)this.GetPix(20)), "名", guistyle3))
				{
					this.nameFlg = true;
					this.inName2 = this.kankyoComboList[this.kankyoIndex].text;
				}
				this.kankyoIndex = this.kankyoCombo.List(new Rect((float)this.GetPix(4), (float)this.GetPix(86), (float)this.GetPix(91), (float)this.GetPix(23)), this.kankyoComboList[this.kankyoIndex].text, this.kankyoComboList, guistyle4, "box", this.listStyle3);
				if (GUI.Button(new Rect((float)this.GetPix(100), (float)this.GetPix(86), (float)this.GetPix(35), (float)this.GetPix(20)), "保存", guistyle3))
				{
					this.saveScene = 10000 + this.kankyoIndex;
					this.saveScene2 = this.saveScene;
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					try
					{
						this.thum_byte_to_base64_ = string.Empty;
						this.thum_file_path_ = Path.Combine(Path.GetTempPath(), "cm3d2_" + Guid.NewGuid().ToString() + ".png");
						GameMain.Instance.MainCamera.ScreenShot(this.thum_file_path_, 1, false);
					}
					catch
					{
					}
				}
				if (GUI.Button(new Rect((float)this.GetPix(140), (float)this.GetPix(86), (float)this.GetPix(35), (float)this.GetPix(20)), "読込", guistyle3))
				{
					this.loadScene = 10000 + this.kankyoIndex;
					this.kankyoLoadFlg = true;
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
				}
				GUI.enabled = false;
				IniKey iniKey2 = base.Preferences["scene"]["s" + (10000 + this.kankyoIndex)];
				if (iniKey2.Value != null && iniKey2.Value.ToString() != "")
				{
					GUI.enabled = true;
				}
			}
			GUI.enabled = true;
			if (this.bgmCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			this.listStyle3.padding.top = this.GetPix(1);
			this.listStyle3.padding.bottom = this.GetPix(0);
			this.listStyle3.fontSize = this.GetPix(12);
			int num8 = this.bgCombo.List(new Rect((float)this.GetPix(31), (float)this.GetPix(53), (float)this.GetPix(95), (float)this.GetPix(23)), this.bgComboList[this.bgIndex].text, this.bgComboList, guistyle4, "box", this.listStyle3);
			if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
			{
				num8--;
				if (num8 <= -1)
				{
					num8 = this.bgArray.Length - 1;
				}
			}
			if (GUI.Button(new Rect((float)this.GetPix(129), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
			{
				num8++;
				if (num8 == this.bgArray.Length)
				{
					num8 = 0;
				}
			}
			if (this.bgIndex != num8)
			{
				this.bgIndex = num8;
				this.bg.localScale = new Vector3(1f, 1f, 1f);
				if (this.bgArray[this.bgIndex].Length == 36)
				{
					GameMain.Instance.BgMgr.ChangeBgMyRoom(this.bgArray[this.bgIndex]);
				}
				else
				{
					GameMain.Instance.BgMgr.ChangeBg(this.bgArray[this.bgIndex]);
				}
				this.bgCombo.selectedItemIndex = this.bgIndex;
				if (this.bgArray[this.bgIndex] == "karaokeroom")
				{
					this.bg.transform.position = this.bgObject.transform.position;
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					zero2.y = 90f;
					zero.z = 4f;
					zero.x = 1f;
					this.bg.transform.localPosition = zero;
					this.bg.transform.localRotation = Quaternion.Euler(zero2);
				}
			}
			GUI.enabled = true;
			int num9 = this.bgmCombo.List(new Rect((float)this.GetPix(31), (float)this.GetPix(25), (float)this.GetPix(95), (float)this.GetPix(23)), this.bgmComboList[this.bgmIndex].text, this.bgmComboList, guistyle4, "box", this.listStyle3);
			if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(25), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
			{
				num9--;
				if (num9 <= -1)
				{
					num9 = this.bgmArray.Length - 1;
				}
			}
			if (GUI.Button(new Rect((float)this.GetPix(129), (float)this.GetPix(25), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
			{
				num9++;
				if (num9 == this.bgmArray.Length)
				{
					num9 = 0;
				}
			}
			if (this.bgmIndex != num9)
			{
				this.bgmIndex = num9;
				GameMain.Instance.SoundMgr.PlayBGM(this.bgmArray[this.bgmIndex] + ".ogg", 0f, true);
				this.bgmCombo.selectedItemIndex = this.bgmIndex;
			}
			if (this.bgmCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
		}

		private void GuiFunc4(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "label";
			guistyle2.fontSize = this.GetPix(11);
			guistyle2.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle3 = "button";
			guistyle3.fontSize = this.GetPix(20);
			GUIStyle guistyle4 = "button";
			guistyle4.fontSize = this.GetPix(14);
			guistyle4.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle5 = "button";
			guistyle5.fontSize = this.GetPix(12);
			guistyle5.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle6 = new GUIStyle("toggle");
			guistyle6.fontSize = this.GetPix(13);
			CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
			Maid maid = this.maidArray[this.selectMaidIndex];
			if (!this.poseInitFlg)
			{
				this.listStyle2.normal.textColor = Color.white;
				this.listStyle2.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle2.onHover.background = (this.listStyle2.hover.background = new Texture2D(2, 2));
				RectOffset padding = this.listStyle2.padding;
				RectOffset padding2 = this.listStyle2.padding;
				RectOffset padding3 = this.listStyle2.padding;
				int num = this.listStyle2.padding.bottom = this.GetPix(0);
				num = (padding3.top = num);
				num = (padding2.right = num);
				padding.left = num;
				this.listStyle2.fontSize = this.GetPix(12);
				this.listStyle3.normal.textColor = Color.white;
				this.listStyle3.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle3.onHover.background = (this.listStyle3.hover.background = new Texture2D(2, 2));
				RectOffset padding4 = this.listStyle3.padding;
				RectOffset padding5 = this.listStyle3.padding;
				num = (this.listStyle3.padding.top = this.GetPix(1));
				num = (padding5.right = num);
				padding4.left = num;
				this.listStyle3.padding.bottom = this.GetPix(0);
				this.listStyle3.fontSize = this.GetPix(12);
				this.listStyle4.normal.textColor = Color.white;
				this.listStyle4.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle4.onHover.background = (this.listStyle4.hover.background = new Texture2D(2, 2));
				RectOffset padding6 = this.listStyle4.padding;
				RectOffset padding7 = this.listStyle4.padding;
				num = (this.listStyle4.padding.top = 3);
				num = (padding7.right = num);
				padding6.left = num;
				this.listStyle4.padding.bottom = 3;
				this.listStyle4.fontSize = this.GetPix(13);
				this.poseCombo.selectedItemIndex = 0;
				int num2 = (int)this.groupList[0];
				this.poseComboList = new GUIContent[num2];
				for (int i = 0; i < num2; i++)
				{
					this.poseComboList[i] = new GUIContent(i + 1 + ":" + this.poseArray[i]);
				}
				this.poseGroupCombo.selectedItemIndex = 0;
				this.poseGroupComboList = new GUIContent[this.poseGroupArray.Length + 1];
				this.poseGroupComboList[0] = new GUIContent("1:通常");
				for (int i = 0; i < this.poseGroupArray.Length; i++)
				{
					if (this.poseGroupArray[i] == "maid_dressroom01")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":立ち");
					}
					if (this.poseGroupArray[i] == "tennis_kamae_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":中腰");
					}
					if (this.poseGroupArray[i] == "senakanagasi_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":膝をつく");
					}
					if (this.poseGroupArray[i] == "work_hansei")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":座り");
					}
					if (this.poseGroupArray[i] == "inu_taiki_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":四つん這い");
					}
					if (this.poseGroupArray[i] == "syagami_pose_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":床座り");
					}
					if (this.poseGroupArray[i] == "densyasuwari_taiki_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":椅子座り");
					}
					if (this.poseGroupArray[i] == "work_kaiwa")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":ソファー座り");
					}
					if (this.poseGroupArray[i] == "dance_cm3d2_001_f1,14.14")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":ドキドキ☆Fallin' Love");
					}
					if (this.poseGroupArray[i] == "dance_cm3d_001_f1,39.25")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":entrance to you");
					}
					if (this.poseGroupArray[i] == "dance_cm3d_002_end_f1,50.71")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":scarlet leap");
					}
					if (this.poseGroupArray[i] == "dance_cm3d2_002_smt_f,7.76,")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":stellar my tears");
					}
					if (this.poseGroupArray[i] == "dance_cm3d_003_sp2_f1,90.15")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":rhythmix to you");
					}
					if (this.poseGroupArray[i] == "dance_cm3d2_003_hs_f1,0.01,")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":happy!happy!スキャンダル!!");
					}
					if (this.poseGroupArray[i] == "dance_cm3d_004_kano_f1,124.93")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":Can Know Two Close");
					}
					if (this.poseGroupArray[i] == "dance_cm3d2_004_sse_f1,0.01")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":sweet sweet everyday");
					}
					if (this.poseGroupArray[i] == "turusi_sex_in_taiki_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":拘束");
					}
					if (this.poseGroupArray[i] == "rosyutu_pose01_f")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":エロ");
					}
					if (this.poseGroupArray[i] == "rosyutu_aruki_f_once_,1.37")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":歩き");
					}
					if (this.poseGroupArray[i] == "stand_desk1")
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":その他");
					}
					if (this.poseGroupArray[i] == this.poseArray5[0])
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":ダンスMC");
					}
					if (this.poseGroupArray[i] == this.poseArray6[0])
					{
						this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":ダンス");
					}
					if (this.existPose && this.strS != "")
					{
						if (i == this.poseGroupArray.Length - 4)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":通常2");
						}
						if (i == this.poseGroupArray.Length - 3)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":エロ2");
						}
						if (i == this.poseGroupArray.Length - 2)
						{
							this.poseGroupComboList[i + 1] = new GUIContent("98:撮影モード");
						}
						if (i == this.poseGroupArray.Length - 1)
						{
							this.poseGroupComboList[i + 1] = new GUIContent("99:登録ポーズ");
						}
					}
					else if (this.existPose && this.strS == "")
					{
						if (i == this.poseGroupArray.Length - 3)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":通常2");
						}
						if (i == this.poseGroupArray.Length - 2)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":エロ2");
						}
						if (i == this.poseGroupArray.Length - 1)
						{
							this.poseGroupComboList[i + 1] = new GUIContent("99:登録ポーズ");
						}
					}
					else if (!this.existPose && this.strS != "")
					{
						if (i == this.poseGroupArray.Length - 3)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":通常2");
						}
						if (i == this.poseGroupArray.Length - 2)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":エロ2");
						}
						if (i == this.poseGroupArray.Length - 1)
						{
							this.poseGroupComboList[i + 1] = new GUIContent("98:撮影モード");
						}
					}
					else
					{
						if (i == this.poseGroupArray.Length - 2)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":通常2");
						}
						if (i == this.poseGroupArray.Length - 1)
						{
							this.poseGroupComboList[i + 1] = new GUIContent(i + 2 + ":エロ2");
						}
					}
				}
				this.poseInitFlg = true;
				this.itemCombo.selectedItemIndex = 0;
				num2 = this.itemArray.Length;
				this.itemComboList = new GUIContent[num2 - 1];
				for (int i = 0; i < num2; i++)
				{
					if (i == 0)
					{
						this.itemComboList[i] = new GUIContent("アイテム無し");
					}
					else
					{
						string text = this.itemArray[i];
						switch (text)
						{
							case "handitem,HandItemR_WineGlass_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ワイングラス");
								break;
							case "handitem,HandItemR_WineBottle_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ワインボトル");
								break;
							case "handitem,handitemr_racket_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ラケット");
								break;
							case "handitem,HandItemR_Hataki_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ハタキ");
								break;
							case "handitem,HandItemR_Mop_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":モップ");
								break;
							case "handitem,HandItemR_Houki_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ほうき");
								break;
							case "handitem,HandItemR_Zoukin2_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":雑巾");
								break;
							case "handitem,HandItemR_Chu-B_Lip_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":Chu-B Lip");
								break;
							case "handitem,HandItemR_Mimikaki_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":耳かき");
								break;
							case "handitem,HandItemR_Pen_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":ペン");
								break;
							case "handitem,HandItemR_Otama_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":おたま");
								break;
							case "handitem,HandItemR_Houchou_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":包丁");
								break;
							case "handitem,HandItemR_Book_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":本");
								break;
							case "handitem,HandItemR_Puff_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":パフ");
								break;
							case "handitem,HandItemR_Rip_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":リップ");
								break;
							case "handitem,HandItemD_Shisyuu_Hari_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":刺繍");
								break;
							case "handitem,HandItemD_Sara_Sponge_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":皿・スポンジ");
								break;
							case "kousoku_upper,KousokuU_TekaseOne_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":手枷1");
								break;
							case "kousoku_upper,KousokuU_TekaseTwo_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":手枷2");
								break;
							case "kousoku_lower,KousokuL_AshikaseUp_I_.menu":
								this.itemComboList[i] = new GUIContent(i + ":足枷");
								break;
							case "handitem,HandItemR_Usuba_Houchou_I_.menu":
								this.itemComboList[i] = new GUIContent(i + "薄刃包丁");
								break;
							case "handitem,HandItemR_Chusyaki_I_.menu":
								this.itemComboList[i] = new GUIContent(i + "注射器");
								break;
							case "handitem,HandItemR_Nei_Heartful_I_.menu":
								this.itemComboList[i] = new GUIContent(i + "ハートフルねい人形");
								break;
							case "handitem,HandItemR_Shaker_I_.menu":
								this.itemComboList[i] = new GUIContent(i + "シェイカー");
								break;
							case "handitem,HandItemR_SmartPhone_I_.menu":
								this.itemComboList[i] = new GUIContent(i + "スマートフォン");
								break;
							case "kousoku_upper,KousokuU_Ushirode_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":後ろ手拘束具");
								break;
							case "kousoku_upper,KousokuU_SMRoom_Haritsuke_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":磔台・手枷足枷");
								break;
							case "kousoku_upper,KousokuU_SMRoom2_Haritsuke_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":磔台・手枷足枷2");
								break;
							case "handitem,HandItemL_Dance_Hataki_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ダンスハタキ");
								break;
							case "handitem,HandItemL_Dance_Mop_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ダンスモップ");
								break;
							case "handitem,HandItemL_Dance_Zoukin_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ダンス雑巾");
								break;
							case "handitem,HandItemL_Kozara_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":小皿");
								break;
							case "handitem,HandItemR_Teacup_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ティーカップ");
								break;
							case "handitem,HandItemL_Teasaucer_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ティーソーサー");
								break;
							case "handitem,HandItemR_Wholecake_I_.menu":
								this.itemComboList[i - 1] = new GUIContent("ホールケーキ");
								break;
							case "handitem,HandItemR_Menu_I_.menu":
								this.itemComboList[i - 1] = new GUIContent("メニュー表");
								break;
							case "handitem,HandItemR_Vibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":バイブ");
								break;
							case "handitem,HandItemR_VibePink_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":ピンクバイブ");
								break;
							case "handitem,HandItemR_VibeBig_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":太バイブ");
								break;
							case "handitem,HandItemR_AnalVibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":アナルバイブ");
								break;
							case "handitem,HandItemH_SoutouVibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":前：双頭バイブ");
								break;
							case "accvag,accVag_Vibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":前：バイブ");
								break;
							case "accvag,accVag_VibeBig_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":前：太バイブ");
								break;
							case "accvag,accVag_VibePink_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":前：ピンクバイブ");
								break;
							case "accanl,accAnl_AnalVibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":後：アナルバイブ");
								break;
							case "accanl,accAnl_Photo_NomalVibe_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":後：バイブ");
								break;
							case "accanl,accAnl_Photo_VibeBig_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":後：太バイブ");
								break;
							case "accanl,accAnl_Photo_VibePink_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + ":後：ピンクバイブ");
								break;
							case "handitem,HandItemL_Etoile_Saucer_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + "ティーソーサー");
								break;
							case "handitem,HandItemR_Etoile_Teacup_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + "ティーカップ");
								break;
							case "handitem,HandItemL_Katuramuki_Daikon_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + "桂むき大根");
								break;
							case "handitem,HandItemL_Karte_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + "カルテ");
								break;
							case "handitem,HandItemL_Cracker_I_.menu":
								this.itemComboList[i - 1] = new GUIContent(i - 1 + "クラッカー");
								break;
						}
						if (i == 12)
						{
							this.itemComboList[i] = new GUIContent(i + ":手枷・足枷");
						}
						if (i == 13)
						{
							this.itemComboList[i] = new GUIContent(i + ":手枷・足枷(下)");
						}
						if (i == 24)
						{
							this.itemComboList[i - 1] = new GUIContent(i - 1 + ":カップ＆ソーサー");
						}
					}
				}
			}
			if (this.poseCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.poseGroupCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.sceneLevel == 3 || this.sceneLevel == 5 || this.isF6)
			{
				if (!this.isF6)
				{
					bool value = true;
					if (this.faceFlg || this.poseFlg || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
					{
						value = false;
					}
					if (GUI.Toggle(new Rect((float)this.GetPix(2), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), value, "配置", guistyle6))
					{
						this.faceFlg = false;
						this.poseFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
						this.bGui = true;
						this.isGuiInit = true;
					}
				}
				if (!this.yotogiFlg)
				{
					if (GUI.Toggle(new Rect((float)this.GetPix(41), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.poseFlg, "操作", guistyle6))
					{
						this.poseFlg = true;
						this.faceFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
					}
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(80), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.faceFlg, "表情", guistyle6))
				{
					this.faceFlg = true;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					if (!this.faceFlg2)
					{
						this.isFaceInit = true;
						this.faceFlg2 = true;
						this.maidArray[this.selectMaidIndex].boMabataki = false;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					this.isFaceInit = true;
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(119), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyoFlg, "環境", guistyle6))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = true;
					this.kankyo2Flg = false;
				}
				if (!this.line1)
				{
					this.line1 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.3f));
					this.line2 = this.MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 0.6f));
				}
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 2f), this.line1);
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 1f), this.line2);
				guistyle.fontSize = this.GetPix(13);
				guistyle.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect((float)this.GetPix(125), (float)this.GetPix(25), (float)this.GetPix(40), (float)this.GetPix(25)), string.Concat(this.selectMaidIndex + 1), guistyle);
				guistyle.fontSize = this.GetPix(11);
				guistyle.alignment = TextAnchor.UpperLeft;
			}
			if (this.sceneLevel > 0)
			{
				int i = this.selectMaidIndex;
				if (this.sceneLevel == 3 || (this.sceneLevel == 5 && (this.isF7 || this.maidCnt > 1)))
				{
					if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(32)), "＜", guistyle3))
					{
						this.selectMaidIndex--;
						if (this.selectMaidIndex < 0)
						{
							this.selectMaidIndex = this.selectList.Count - 1;
						}
						this.isPoseInit = true;
						this.poseFlg = true;
						this.copyIndex = 0;
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex];
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(32)), "＞", guistyle3))
					{
						this.selectMaidIndex++;
						if (this.selectList.Count <= this.selectMaidIndex)
						{
							this.selectMaidIndex = 0;
						}
						this.isPoseInit = true;
						this.poseFlg = true;
						this.copyIndex = 0;
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex];
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
				}
				if (this.maidArray[this.selectMaidIndex].GetThumIcon())
				{
					GUI.DrawTexture(new Rect((float)this.GetPix(30), (float)this.GetPix(25), (float)this.GetPix(60), (float)this.GetPix(60)), this.maidArray[this.selectMaidIndex].GetThumIcon());
				}
				string text2 = this.maidArray[this.selectMaidIndex].status.lastName + "\n" + this.maidArray[this.selectMaidIndex].status.firstName;
				GUI.Label(new Rect((float)this.GetPix(90), (float)this.GetPix(50), (float)this.GetPix(140), (float)this.GetPix(210)), text2, guistyle);
			}
			if (!this.isF6)
			{
				if (this.isDanceStop)
				{
					this.isStop[this.selectMaidIndex] = true;
					this.isDanceStop = false;
				}
				if (this.sceneLevel == 5)
				{
					if (this.maidCnt > 1)
					{
						bool value2 = false;
						if (this.selectMaidIndex == this.isEditNo)
						{
							value2 = true;
						}
						this.isEdit[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(25), (float)this.GetPix(50), (float)this.GetPix(16)), value2, "Edit", guistyle6);
						if (this.isEdit[this.selectMaidIndex] && this.selectMaidIndex != this.isEditNo)
						{
							this.isEditNo = this.selectMaidIndex;
							for (int j = 0; j < this.maidCnt; j++)
							{
								if (j != this.isEditNo)
								{
									this.isEdit[j] = false;
								}
							}
							SceneEdit component = GameObject.Find("__SceneEdit__").GetComponent<SceneEdit>();
							MultipleMaids.SetFieldValue<SceneEdit, Maid>(component, "m_maid", this.maidArray[this.selectMaidIndex]);
							component.PartsTypeCamera(MPN.stkg);
							this.editSelectMaid = this.maidArray[this.selectMaidIndex];
						}
					}
				}
				if (this.poseGroupCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				if (this.isLock[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(125), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
				{
					this.poseIndex[this.selectMaidIndex]--;
					if (this.poseGroupIndex > 0)
					{
						if ((int)this.groupList[this.poseGroupIndex - 1] > this.poseIndex[this.selectMaidIndex])
						{
							if (this.poseGroupIndex >= this.groupList.Count)
							{
								this.poseIndex[this.selectMaidIndex] = this.poseArray.Length - 1;
							}
							else
							{
								this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex] - 1;
							}
						}
					}
					else if (this.poseIndex[this.selectMaidIndex] < 0)
					{
						this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex] - 1;
					}
					this.isPoseInit = true;
					if (this.poseGroupIndex > 0)
					{
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex] - (int)this.groupList[this.poseGroupIndex - 1];
					}
					else
					{
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex];
					}
					if (!this.isLock[this.selectMaidIndex])
					{
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
				}
				if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(125), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
				{
					this.poseIndex[this.selectMaidIndex]++;
					if (this.poseIndex[this.selectMaidIndex] > (int)this.groupList[this.groupList.Count - 1])
					{
						if (this.poseIndex[this.selectMaidIndex] >= this.poseArray.Length)
						{
							this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex - 1];
						}
					}
					else if (this.poseIndex[this.selectMaidIndex] >= (int)this.groupList[this.poseGroupIndex])
					{
						if (this.poseGroupIndex > 0)
						{
							this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex - 1];
						}
						else
						{
							this.poseIndex[this.selectMaidIndex] = 0;
						}
					}
					this.isPoseInit = true;
					if (this.poseGroupIndex > 0)
					{
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex] - (int)this.groupList[this.poseGroupIndex - 1];
					}
					else
					{
						this.poseCombo.selectedItemIndex = this.poseIndex[this.selectMaidIndex];
					}
					if (!this.isLock[this.selectMaidIndex])
					{
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
				}
				GUI.enabled = true;
				int num5 = -1;
				for (int k = 0; k < this.groupList.Count; k++)
				{
					if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
					{
						num5 = k;
						break;
					}
				}
				int num6 = (int)this.groupList[0];
				int num7 = 0;
				if (num5 > 0)
				{
					num6 = (int)this.groupList[num5] - (int)this.groupList[num5 - 1];
					num7 = (int)this.groupList[num5 - 1];
				}
				if (num5 < 0)
				{
					num5 = this.groupList.Count;
					num6 = this.poseArray.Length - (int)this.groupList[num5 - 1];
					num7 = (int)this.groupList[num5 - 1];
				}
				if (this.poseGroupCombo.selectedItemIndex != num5)
				{
					this.poseComboList = new GUIContent[num6];
					int j = 0;
					for (int i = num7; i < num7 + num6; i++)
					{
						bool flag = false;
						List<IniKey> keys = base.Preferences["pose"].Keys;
						foreach (IniKey iniKey in keys)
						{
							if (this.poseArray[i] == iniKey.Key)
							{
								IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
								if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
								{
									this.poseComboList[j] = new GUIContent(string.Concat(new object[]
									{
										j + 1,
										":",
										iniKey2.Value.Split(new char[]
										{
											'_'
										})[0],
										"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
										iniKey.Key
									}));
									flag = true;
								}
							}
						}
						if (!flag)
						{
							this.poseComboList[j] = new GUIContent(j + 1 + ":" + this.poseArray[i]);
						}
						j++;
					}
					this.poseGroupCombo.selectedItemIndex = num5;
					this.poseGroupIndex = num5;
					this.poseCombo.selectedItemIndex = 0;
				}
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				this.isLook[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(8), (float)this.GetPix(155), (float)this.GetPix(65), (float)this.GetPix(16)), this.isLook[this.selectMaidIndex], "顔の向き", guistyle6);
				this.isPoseEdit = GUI.Toggle(new Rect((float)this.GetPix(86), (float)this.GetPix(155), (float)this.GetPix(90), (float)this.GetPix(16)), this.isPoseEdit, "ポーズ登録", guistyle6);
				if (this.isPoseEdit)
				{
					this.inName3 = GUI.TextField(new Rect((float)this.GetPix(5), (float)this.GetPix(180), (float)this.GetPix(100), (float)this.GetPix(20)), this.inName3);
					if (GUI.Button(new Rect((float)this.GetPix(107), (float)this.GetPix(180), (float)this.GetPix(35), (float)this.GetPix(20)), "追加", guistyle3))
					{
						this.isSavePose = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
						this.existPose = true;
						GUI.FocusControl("");
					}
					if (this.poseGroupComboList[this.poseGroupCombo.selectedItemIndex].text != "99:登録ポーズ")
					{
						GUI.enabled = false;
					}
					if (GUI.Button(new Rect((float)this.GetPix(144), (float)this.GetPix(180), (float)this.GetPix(24), (float)this.GetPix(20)), "削", guistyle3))
					{
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
						List<string> list = new List<string>();
						list.AddRange(this.poseArray);
						if (this.poseComboList[this.poseCombo.selectedItemIndex].text.Contains("MultipleMaidsPose"))
						{
							string text3 = this.poseArray[this.poseIndex[this.selectMaidIndex]];
							list.Remove(text3);
							string path2 = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								'/'
							})[1].Replace("\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000", "");
							if (File.Exists(path2))
							{
								File.Delete(path2);
							}
						}
						else
						{
							string[] array4 = this.poseComboList[this.poseCombo.selectedItemIndex].text.Split(new char[]
							{
								'p'
							});
							string text3 = array4[array4.Length - 1];
							IniKey iniKey3 = base.Preferences["pose"]["p" + text3];
							if (iniKey3.Value != "" || iniKey3.Value != "del")
							{
								base.Preferences["pose"]["p" + text3].Value = "del";
								base.SaveConfig();
							}
							list.Remove("p" + text3);
						}
						GUI.FocusControl("");
						this.poseArray = list.ToArray();
						num5 = -1;
						for (int k = 0; k < this.groupList.Count; k++)
						{
							if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
							{
								num5 = k;
								break;
							}
						}
						num6 = (int)this.groupList[0];
						num7 = 0;
						if (num5 > 0)
						{
							num6 = (int)this.groupList[num5] - (int)this.groupList[num5 - 1];
							num7 = (int)this.groupList[num5 - 1];
						}
						if (num5 < 0)
						{
							num5 = this.groupList.Count;
							num6 = this.poseArray.Length - (int)this.groupList[num5 - 1];
							num7 = (int)this.groupList[num5 - 1];
						}
						this.poseComboList = new GUIContent[num6];
						int num8 = 0;
						bool existEdit = false;
						for (int l = num7; l < num7 + num6; l++)
						{
							bool flag = false;
							List<IniKey> keys = base.Preferences["pose"].Keys;
							foreach (IniKey iniKey in keys)
							{
								if (this.poseArray[l] == iniKey.Key)
								{
									IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
									if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
									{
										this.poseComboList[num8] = new GUIContent(string.Concat(new object[]
										{
											num8 + 1,
											":",
											iniKey2.Value.Split(new char[]
											{
												'_'
											})[0],
											"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
											iniKey.Key
										}));
										flag = true;
										existEdit = true;
									}
								}
							}
							if (!flag)
							{
								this.poseComboList[num8] = new GUIContent(num8 + 1 + ":" + this.poseArray[l]);
							}
							num8++;
						}
						Action<string, List<string>> action = delegate (string path, List<string> result_list)
						{
							string[] files = Directory.GetFiles(path);
							this.countS = 0;
							for (int n = 0; n < files.Length; n++)
							{
								if (Path.GetExtension(files[n]) == ".anm")
								{
									existEdit = true;
									break;
								}
							}
						};
						List<string> arg = new List<string>();
						action(Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose", arg);
						if (!existEdit)
						{
							this.poseIniStr = "";
							List<string> list2 = new List<string>(50 + this.poseGroupArray2.Length);
							list2.AddRange(this.poseGroupArray2);
							list2.AddRange(this.poseGroupArrayVP);
							list2.AddRange(this.poseGroupArrayFB);
							list2.AddRange(this.poseGroupArray3);
							list2.Add(this.poseArray5[0]);
							list2.Add(this.poseArray6[0]);
							list2.Add(this.strList2[0]);
							list2.Add(this.strListE2[0]);
							this.existPose = false;
							this.poseGroupArray = list2.ToArray();
							this.groupList = new ArrayList();
							for (int k = 0; k < this.poseArray.Length; k++)
							{
								for (int j = 0; j < this.poseGroupArray.Length; j++)
								{
									if (this.poseGroupArray[j] == this.poseArray[k])
									{
										this.groupList.Add(k);
										if (this.poseGroupArray[j] == this.strList2[0])
										{
											this.sPoseCount = k;
										}
									}
								}
							}
							this.poseIndex[this.selectMaidIndex] = 0;
							this.poseGroupComboList = new GUIContent[this.poseGroupArray.Length + 1];
							this.poseGroupComboList[0] = new GUIContent("1:通常");
							for (int m = 0; m < this.poseGroupArray.Length; m++)
							{
								if (this.poseGroupArray[m] == "maid_dressroom01")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":立ち");
								}
								if (this.poseGroupArray[m] == "tennis_kamae_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":中腰");
								}
								if (this.poseGroupArray[m] == "senakanagasi_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":膝をつく");
								}
								if (this.poseGroupArray[m] == "work_hansei")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":座り");
								}
								if (this.poseGroupArray[m] == "inu_taiki_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":四つん這い");
								}
								if (this.poseGroupArray[m] == "syagami_pose_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":床座り");
								}
								if (this.poseGroupArray[m] == "densyasuwari_taiki_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":椅子座り");
								}
								if (this.poseGroupArray[m] == "work_kaiwa")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":ソファー座り");
								}
								if (this.poseGroupArray[m] == "dance_cm3d2_001_f1,14.14")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":ドキドキ☆Fallin' Love");
								}
								if (this.poseGroupArray[m] == "dance_cm3d_001_f1,39.25")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":entrance to you");
								}
								if (this.poseGroupArray[m] == "dance_cm3d_002_end_f1,50.71")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":scarlet leap");
								}
								if (this.poseGroupArray[m] == "dance_cm3d2_002_smt_f,7.76,")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":stellar my tears");
								}
								if (this.poseGroupArray[m] == "dance_cm3d_003_sp2_f1,90.15")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":rhythmix to you");
								}
								if (this.poseGroupArray[m] == "dance_cm3d2_003_hs_f1,0.01,")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":happy!happy!スキャンダル!!");
								}
								if (this.poseGroupArray[m] == "dance_cm3d_004_kano_f1,124.93")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":Can Know Two Close");
								}
								if (this.poseGroupArray[m] == "dance_cm3d2_004_sse_f1,0.01")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":sweet sweet everyday");
								}
								if (this.poseGroupArray[m] == "turusi_sex_in_taiki_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":拘束");
								}
								if (this.poseGroupArray[m] == "rosyutu_pose01_f")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":エロ");
								}
								if (this.poseGroupArray[m] == "rosyutu_aruki_f_once_,1.37")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":歩き");
								}
								if (this.poseGroupArray[m] == "stand_desk1")
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":その他");
								}
								if (this.poseGroupArray[m] == this.poseArray5[0])
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":ダンスMC");
								}
								if (this.poseGroupArray[m] == this.poseArray6[0])
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":ダンス");
								}
								if (m == this.poseGroupArray.Length - 2)
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":通常2");
								}
								if (m == this.poseGroupArray.Length - 1)
								{
									this.poseGroupComboList[m + 1] = new GUIContent(m + 2 + ":エロ2");
								}
							}
						}
						else
						{
							this.poseGroupCombo.selectedItemIndex = num5;
							this.poseGroupIndex = num5;
							this.poseCombo.selectedItemIndex = 0;
							this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.groupList.Count - 1];
							if (this.poseArray.Length <= this.poseIndex[this.selectMaidIndex])
							{
								this.poseIndex[this.selectMaidIndex]--;
							}
						}
					}
					if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton)
					{
						GUI.enabled = false;
					}
					else
					{
						GUI.enabled = true;
					}
				}
				else
				{
					if (!this.isLook[this.selectMaidIndex])
					{
						GUI.enabled = false;
					}
					GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(175), (float)this.GetPix(100), (float)this.GetPix(25)), "顔の向きX", guistyle2);
					this.lookX[this.selectMaidIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(191), (float)this.GetPix(70), (float)this.GetPix(20)), this.lookX[this.selectMaidIndex], -0.6f, 0.6f);
					GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(175), (float)this.GetPix(100), (float)this.GetPix(25)), "顔の向きY", guistyle2);
					this.lookY[this.selectMaidIndex] = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(191), (float)this.GetPix(70), (float)this.GetPix(20)), this.lookY[this.selectMaidIndex], 0.5f, -0.55f);
					if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton)
					{
						GUI.enabled = false;
					}
					else
					{
						GUI.enabled = true;
					}
				}
				int num9 = 0;
				if (this.poseGroupIndex > 0)
				{
					num9 = this.poseIndex[this.selectMaidIndex] - (int)this.groupList[this.poseGroupIndex - 1];
				}
				else
				{
					num9 = this.poseIndex[this.selectMaidIndex];
				}
				if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(215), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
				{
					this.itemIndex[this.selectMaidIndex]--;
					if (this.itemIndex[this.selectMaidIndex] <= -1)
					{
						this.itemIndex[this.selectMaidIndex] = this.itemArray.Length - 2;
					}
					string[] array = new string[2];
					array = this.itemArray[this.itemIndex[this.selectMaidIndex]].Split(new char[]
					{
						','
					});
					if (this.itemIndex[this.selectMaidIndex] > 13)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
					}
					maid.DelProp(MPN.handitem, true);
					maid.DelProp(MPN.accvag, true);
					maid.DelProp(MPN.accanl, true);
					bool flag2 = false;
					if (this.itemIndex[this.selectMaidIndex] == 12 || this.itemIndex[this.selectMaidIndex] == 13 || this.itemIndex[this.selectMaidIndex] == 23)
					{
						flag2 = true;
					}
					if (!flag2)
					{
						maid.DelProp(MPN.kousoku_upper, true);
						maid.DelProp(MPN.kousoku_lower, true);
					}
					if (array[0] != "")
					{
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 12)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] - 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 13)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 23)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex]].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
						this.cafeFlg[this.selectMaidIndex] = true;
					}
					maid.AllProcPropSeqStart();
					this.itemCombo.selectedItemIndex = this.itemIndex[this.selectMaidIndex];
				}
				if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(215), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
				{
					this.itemIndex[this.selectMaidIndex]++;
					if (this.itemIndex[this.selectMaidIndex] >= this.itemArray.Length - 1)
					{
						this.itemIndex[this.selectMaidIndex] = 0;
					}
					string[] array = new string[2];
					array = this.itemArray[this.itemIndex[this.selectMaidIndex]].Split(new char[]
					{
						','
					});
					if (this.itemIndex[this.selectMaidIndex] > 13)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
					}
					maid.DelProp(MPN.handitem, true);
					maid.DelProp(MPN.accvag, true);
					maid.DelProp(MPN.accanl, true);
					bool flag2 = false;
					if (this.itemIndex[this.selectMaidIndex] == 12 || this.itemIndex[this.selectMaidIndex] == 13)
					{
						flag2 = true;
					}
					if (!flag2)
					{
						maid.DelProp(MPN.kousoku_upper, true);
						maid.DelProp(MPN.kousoku_lower, true);
					}
					if (array[0] != "")
					{
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 12)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] - 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 13)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex[this.selectMaidIndex] == 23)
					{
						array = this.itemArray[this.itemIndex[this.selectMaidIndex]].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
						this.cafeFlg[this.selectMaidIndex] = true;
					}
					maid.AllProcPropSeqStart();
					this.itemCombo.selectedItemIndex = this.itemIndex[this.selectMaidIndex];
				}
				if (this.itemCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.isWear = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(248), (float)this.GetPix(70), (float)this.GetPix(20)), this.isWear, "トップス", guistyle6);
				this.isSkirt = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(248), (float)this.GetPix(70), (float)this.GetPix(20)), this.isSkirt, "ボトムス", guistyle6);
				this.isBra = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(273), (float)this.GetPix(80), (float)this.GetPix(20)), this.isBra, "ブラジャー", guistyle6);
				this.isPanz = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(273), (float)this.GetPix(60), (float)this.GetPix(20)), this.isPanz, "パンツ", guistyle6);
				this.isHeadset = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(298), (float)this.GetPix(70), (float)this.GetPix(20)), this.isHeadset, "ヘッド", guistyle6);
				this.isMegane = GUI.Toggle(new Rect((float)this.GetPix(95), (float)this.GetPix(298), (float)this.GetPix(70), (float)this.GetPix(20)), this.isMegane, "メガネ", guistyle6);
				this.isAccUde = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(323), (float)this.GetPix(40), (float)this.GetPix(20)), this.isAccUde, "腕", guistyle6);
				this.isGlove = GUI.Toggle(new Rect((float)this.GetPix(50), (float)this.GetPix(323), (float)this.GetPix(40), (float)this.GetPix(20)), this.isGlove, "手袋", guistyle6);
				this.isAccSenaka = GUI.Toggle(new Rect((float)this.GetPix(95), (float)this.GetPix(323), (float)this.GetPix(40), (float)this.GetPix(20)), this.isAccSenaka, "背中", guistyle6);
				this.isStkg = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(348), (float)this.GetPix(40), (float)this.GetPix(20)), this.isStkg, "靴下", guistyle6);
				this.isShoes = GUI.Toggle(new Rect((float)this.GetPix(50), (float)this.GetPix(348), (float)this.GetPix(40), (float)this.GetPix(20)), this.isShoes, "靴", guistyle6);
				this.isMaid = GUI.Toggle(new Rect((float)this.GetPix(95), (float)this.GetPix(348), (float)this.GetPix(70), (float)this.GetPix(20)), this.isMaid, "メイド", guistyle6);
				this.mekure1[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(373), (float)this.GetPix(62), (float)this.GetPix(20)), this.mekure1[this.selectMaidIndex], "めくれ前", guistyle6);
				this.mekure2[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(67), (float)this.GetPix(373), (float)this.GetPix(40), (float)this.GetPix(20)), this.mekure2[this.selectMaidIndex], "後ろ", guistyle6);
				this.zurasi[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(105), (float)this.GetPix(373), (float)this.GetPix(50), (float)this.GetPix(20)), this.zurasi[this.selectMaidIndex], "ずらし", guistyle6);
				this.voice1[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(530), (float)this.GetPix(70), (float)this.GetPix(20)), this.zFlg[this.selectMaidIndex], "ボイス", guistyle6);
				this.voice2[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(70), (float)this.GetPix(530), (float)this.GetPix(70), (float)this.GetPix(20)), this.xFlg[this.selectMaidIndex], "Hボイス", guistyle6);
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton || this.itemCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(402), (float)this.GetPix(160), 2f), this.line1);
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(402), (float)this.GetPix(160), 1f), this.line2);
				this.isIK[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(411), (float)this.GetPix(30), (float)this.GetPix(20)), this.isIK[this.selectMaidIndex], "IK", guistyle6);
				if (!this.isLock[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				this.isLock[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(45), (float)this.GetPix(411), (float)this.GetPix(40), (float)this.GetPix(20)), this.isLock[this.selectMaidIndex], "解除", guistyle6);
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton || this.itemCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				if (!this.isIK[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				this.isBone[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(100), (float)this.GetPix(411), (float)this.GetPix(60), (float)this.GetPix(20)), this.isBone[this.selectMaidIndex], "ボーン", guistyle6);
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				if (this.isBone[this.selectMaidIndex] != this.isBoneN[this.selectMaidIndex])
				{
					this.isBoneN[this.selectMaidIndex] = this.isBone[this.selectMaidIndex];
					this.isChange[this.selectMaidIndex] = true;
				}
				if (!this.isLock[this.selectMaidIndex] && this.unLockFlg != this.isLock[this.selectMaidIndex])
				{
					string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
					{
						','
					});
					this.isStop[this.selectMaidIndex] = false;
					this.poseCount[this.selectMaidIndex] = 20;
					if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
					{
						maid.body0.MuneYureL(0f);
						maid.body0.MuneYureR(0f);
					}
					else
					{
						maid.body0.MuneYureL(1f);
						maid.body0.MuneYureR(1f);
					}
					int num4;
					if (array[0].Contains("MultipleMaidsPose"))
					{
						string path3 = array[0].Split(new char[]
						{
							'/'
						})[1];
						byte[] array2 = new byte[0];
						try
						{
							using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
							{
								array2 = new byte[fileStream.Length];
								fileStream.Read(array2, 0, array2.Length);
							}
						}
						catch
						{
						}
						if (0 < array2.Length)
						{
							string fileName = Path.GetFileName(path3);
							long num3 = (long)fileName.GetHashCode();
							maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
							Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
							{
								default(Maid.AutoTwist),
								Maid.AutoTwist.ShoulderR,
								Maid.AutoTwist.WristL,
								Maid.AutoTwist.WristR,
								Maid.AutoTwist.ThighL,
								Maid.AutoTwist.ThighR
							};
							for (int i = 0; i < array3.Length; i++)
							{
								maid.SetAutoTwist(array3[i], true);
							}
						}
					}
					else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
					{
						this.loadPose[this.selectMaidIndex] = array[0];
					}
					else if (!array[0].StartsWith("dance_"))
					{
						this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
					}
					else
					{
						if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
						{
							maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
						}
						maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
					}
					if (array.Length > 1)
					{
						maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
						this.isDanceStop = true;
						if (array.Length > 2)
						{
							Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
							this.isPoseIti[this.selectMaidIndex] = true;
							this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
							this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
						}
					}
					this.mHandL[this.selectMaidIndex].initFlg = false;
					this.mHandR[this.selectMaidIndex].initFlg = false;
					this.mFootL[this.selectMaidIndex].initFlg = false;
					this.mFootR[this.selectMaidIndex].initFlg = false;
					this.pHandL[this.selectMaidIndex] = 0;
					this.pHandR[this.selectMaidIndex] = 0;
					this.hanten[this.selectMaidIndex] = false;
					this.hantenn[this.selectMaidIndex] = false;
					this.muneIKL[this.selectMaidIndex] = false;
					this.muneIKR[this.selectMaidIndex] = false;
					maid.body0.jbMuneL.enabled = true;
					maid.body0.jbMuneR.enabled = true;
					if (!GameMain.Instance.VRMode)
					{
						this.maidArray[this.selectMaidIndex].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[this.selectMaidIndex]];
						this.maidArray[this.selectMaidIndex].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[this.selectMaidIndex]];
					}
				}
				this.unLockFlg = this.isLock[this.selectMaidIndex];
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton || this.itemCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				if (!this.isIK[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				this.hanten[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(479), (float)this.GetPix(70), (float)this.GetPix(20)), this.hanten[this.selectMaidIndex], "左右反転", guistyle6);
				bool flag3 = GUI.Toggle(new Rect((float)this.GetPix(80), (float)this.GetPix(479), (float)this.GetPix(100), (float)this.GetPix(20)), this.kotei[this.selectMaidIndex], "スカート固定", guistyle6);
				if (this.kotei[this.selectMaidIndex] != flag3)
				{
					this.kotei[this.selectMaidIndex] = flag3;
					if (flag3)
					{
						this.SkirtListArray[this.selectMaidIndex] = new DynamicSkirtBone[100];
						for (int l = 0; l < maid.body0.goSlot.Count; l++)
						{
							DynamicSkirtBone fieldValue = MultipleMaids.GetFieldValue<BoneHair3, DynamicSkirtBone>(maid.body0.goSlot[l].bonehair3, "m_SkirtBone");
							this.SkirtListArray[this.selectMaidIndex][l] = fieldValue;
							MultipleMaids.SetFieldValue8<BoneHair3, DynamicSkirtBone>(maid.body0.goSlot[l].bonehair3, "m_SkirtBone", null);
						}
					}
					else
					{
						for (int l = 0; l < maid.body0.goSlot.Count; l++)
						{
							MultipleMaids.SetFieldValue8<BoneHair3, DynamicSkirtBone>(maid.body0.goSlot[l].bonehair3, "m_SkirtBone", this.SkirtListArray[this.selectMaidIndex][l]);
						}
					}
				}
				GUI.Label(new Rect((float)this.GetPix(29), (float)this.GetPix(433), (float)this.GetPix(100), (float)this.GetPix(25)), "右手", guistyle);
				GUI.Label(new Rect((float)this.GetPix(109), (float)this.GetPix(433), (float)this.GetPix(100), (float)this.GetPix(25)), "左手", guistyle);
				string text4 = "未選択";
				if (this.copyIndex > 0)
				{
					text4 = this.copyIndex + ":" + this.maidArray[this.copyIndex - 1].status.firstName;
				}
				if (this.maidCnt <= 1)
				{
					GUI.enabled = false;
				}
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(505), (float)this.GetPix(100), (float)this.GetPix(25)), "コピー", guistyle);
				GUI.Label(new Rect((float)this.GetPix(70), (float)this.GetPix(505), (float)this.GetPix(100), (float)this.GetPix(25)), text4, guistyle);
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton || this.itemCombo.isClickedComboButton || !this.isIK[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				else
				{
					GUI.enabled = true;
				}
				guistyle.fontSize = this.GetPix(13);
				guistyle.alignment = TextAnchor.UpperCenter;
				GUI.Label(new Rect((float)this.GetPix(-10), (float)this.GetPix(449), (float)this.GetPix(100), (float)this.GetPix(25)), this.pHandR[this.selectMaidIndex].ToString(), guistyle);
				GUI.Label(new Rect((float)this.GetPix(70), (float)this.GetPix(449), (float)this.GetPix(100), (float)this.GetPix(25)), this.pHandL[this.selectMaidIndex].ToString(), guistyle);
				guistyle.fontSize = this.GetPix(11);
				guistyle.alignment = TextAnchor.UpperLeft;
				if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(448), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
				{
					this.pHandR[this.selectMaidIndex]--;
					if (this.pHandR[this.selectMaidIndex] < 1)
					{
						this.pHandR[this.selectMaidIndex] = this.fingerRArray.GetLength(0);
					}
					for (int j = 0; j < 20; j++)
					{
						string[] array = this.fingerRArray[this.pHandR[this.selectMaidIndex] - 1, j].Split(new char[]
						{
							','
						});
						this.Finger[this.selectMaidIndex, j + 20].localEulerAngles = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					}
					this.isStop[this.selectMaidIndex] = true;
					this.isLock[this.selectMaidIndex] = true;
					for (int j = 0; j < 10; j++)
					{
						if (j == 0 || j == 5)
						{
							if (this.mFinger[this.selectMaidIndex, j * 3])
							{
								this.mFinger[this.selectMaidIndex, j * 3].reset = true;
							}
						}
					}
				}
				if (GUI.Button(new Rect((float)this.GetPix(55), (float)this.GetPix(448), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
				{
					this.pHandR[this.selectMaidIndex]++;
					if (this.pHandR[this.selectMaidIndex] > this.fingerRArray.GetLength(0))
					{
						this.pHandR[this.selectMaidIndex] = 1;
					}
					for (int j = 0; j < 20; j++)
					{
						string[] array = this.fingerRArray[this.pHandR[this.selectMaidIndex] - 1, j].Split(new char[]
						{
							','
						});
						this.Finger[this.selectMaidIndex, j + 20].localEulerAngles = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					}
					this.isStop[this.selectMaidIndex] = true;
					this.isLock[this.selectMaidIndex] = true;
					for (int j = 0; j < 10; j++)
					{
						if (j == 0 || j == 5)
						{
							if (this.mFinger[this.selectMaidIndex, j * 3])
							{
								this.mFinger[this.selectMaidIndex, j * 3].reset = true;
							}
						}
					}
				}
				if (GUI.Button(new Rect((float)this.GetPix(85), (float)this.GetPix(448), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
				{
					this.pHandL[this.selectMaidIndex]--;
					if (this.pHandL[this.selectMaidIndex] < 1)
					{
						this.pHandL[this.selectMaidIndex] = this.fingerLArray.GetLength(0);
					}
					for (int j = 0; j < 20; j++)
					{
						string[] array = this.fingerLArray[this.pHandL[this.selectMaidIndex] - 1, j].Split(new char[]
						{
							','
						});
						this.Finger[this.selectMaidIndex, j].localEulerAngles = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					}
					this.isStop[this.selectMaidIndex] = true;
					this.isLock[this.selectMaidIndex] = true;
				}
				if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(448), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
				{
					this.pHandL[this.selectMaidIndex]++;
					if (this.pHandL[this.selectMaidIndex] > this.fingerRArray.GetLength(0))
					{
						this.pHandL[this.selectMaidIndex] = 1;
					}
					for (int j = 0; j < 20; j++)
					{
						string[] array = this.fingerLArray[this.pHandL[this.selectMaidIndex] - 1, j].Split(new char[]
						{
							','
						});
						this.Finger[this.selectMaidIndex, j].localEulerAngles = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					}
					this.isStop[this.selectMaidIndex] = true;
					this.isLock[this.selectMaidIndex] = true;
				}
				if (this.maidCnt <= 1)
				{
					GUI.enabled = false;
				}
				if (GUI.Button(new Rect((float)this.GetPix(45), (float)this.GetPix(504), (float)this.GetPix(22), (float)this.GetPix(20)), "＞", guistyle3))
				{
					this.copyIndex++;
					if (this.copyIndex - 1 == this.selectMaidIndex)
					{
						this.copyIndex++;
					}
					if (this.copyIndex > this.maidCnt)
					{
						this.copyIndex = 0;
					}
				}
				if (this.isCopy)
				{
					this.isCopy = false;
					this.CopyIK2(this.maidArray[this.selectMaidIndex], this.selectMaidIndex, this.maidArray[this.copyIndex - 1], this.copyIndex - 1);
				}
				if (this.copyIndex == 0)
				{
					GUI.enabled = false;
				}
				if (GUI.Button(new Rect((float)this.GetPix(123), (float)this.GetPix(504), (float)this.GetPix(35), (float)this.GetPix(20)), "決定", guistyle3))
				{
					this.CopyIK(this.maidArray[this.selectMaidIndex], this.selectMaidIndex, this.maidArray[this.copyIndex - 1], this.copyIndex - 1);
					this.isCopy = true;
					this.pHandL[this.selectMaidIndex] = this.pHandL[this.copyIndex - 1];
					this.pHandR[this.selectMaidIndex] = this.pHandR[this.copyIndex - 1];
					this.hanten[this.selectMaidIndex] = this.hanten[this.copyIndex - 1];
					this.hantenn[this.selectMaidIndex] = this.hantenn[this.copyIndex - 1];
				}
				GUI.enabled = true;
				if (this.poseCombo.isClickedComboButton || this.poseGroupCombo.isClickedComboButton)
				{
					GUI.enabled = false;
				}
				this.itemIndex2[this.selectMaidIndex] = this.itemCombo.List(new Rect((float)this.GetPix(35), (float)this.GetPix(215), (float)this.GetPix(95), (float)this.GetPix(23)), this.itemComboList[this.itemIndex[this.selectMaidIndex]].text, this.itemComboList, guistyle4, "box", this.listStyle3);
				GUI.enabled = true;
				if (this.poseGroupCombo.isClickedComboButton || this.isLock[this.selectMaidIndex])
				{
					GUI.enabled = false;
				}
				this.poseCombo.List(new Rect((float)this.GetPix(35), (float)this.GetPix(125), (float)this.GetPix(95), (float)this.GetPix(23)), this.poseComboList[num9].text, this.poseComboList, guistyle4, "box", this.listStyle2);
				if (!this.isLock[this.selectMaidIndex])
				{
					GUI.enabled = true;
				}
				int num10 = -1;
				for (int k = 0; k < this.groupList.Count; k++)
				{
					if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
					{
						num10 = k;
						break;
					}
				}
				if (num10 < 0)
				{
					num10 = this.groupList.Count;
				}
				if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(95), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
				{
					this.isPoseInit = true;
					if (!this.isLock[this.selectMaidIndex])
					{
						for (int k = 0; k < this.groupList.Count; k++)
						{
							if (k == 0 && this.poseIndex[this.selectMaidIndex] <= (int)this.groupList[k])
							{
								if (this.poseIndex[this.selectMaidIndex] == 0)
								{
									this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.groupList.Count - 1];
								}
								else
								{
									this.poseIndex[this.selectMaidIndex] = 0;
								}
								break;
							}
							if (k > 0 && this.poseIndex[this.selectMaidIndex] > (int)this.groupList[k - 1] && this.poseIndex[this.selectMaidIndex] <= (int)this.groupList[k])
							{
								this.poseIndex[this.selectMaidIndex] = (int)this.groupList[k - 1];
								break;
							}
						}
						if (this.poseIndex[this.selectMaidIndex] > (int)this.groupList[this.groupList.Count - 1])
						{
							this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.groupList.Count - 1];
						}
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
					int num11 = -1;
					for (int k = 0; k < this.groupList.Count; k++)
					{
						if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
						{
							num11 = k;
							break;
						}
					}
					int num2 = (int)this.groupList[0];
					int num12 = 0;
					if (num11 > 0)
					{
						num2 = (int)this.groupList[num11] - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					if (num11 < 0)
					{
						num11 = this.groupList.Count;
						num2 = this.poseArray.Length - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					this.poseComboList = new GUIContent[num2];
					int j = 0;
					for (int i = num12; i < num12 + num2; i++)
					{
						bool flag = false;
						List<IniKey> keys = base.Preferences["pose"].Keys;
						foreach (IniKey iniKey in keys)
						{
							if (this.poseArray[i] == iniKey.Key)
							{
								IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
								if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
								{
									this.poseComboList[j] = new GUIContent(string.Concat(new object[]
									{
										j + 1,
										":",
										iniKey2.Value.Split(new char[]
										{
											'_'
										})[0],
										"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
										iniKey.Key
									}));
									flag = true;
								}
							}
						}
						if (!flag)
						{
							this.poseComboList[j] = new GUIContent(j + 1 + ":" + this.poseArray[i]);
						}
						j++;
					}
					this.poseCombo.scrollPos = new Vector2(0f, 0f);
					this.poseGroupCombo.selectedItemIndex = num11;
					this.poseCombo.selectedItemIndex = 0;
				}
				if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(95), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
				{
					this.isPoseInit = true;
					if (!this.isLock[this.selectMaidIndex])
					{
						int num13 = this.poseIndex[this.selectMaidIndex];
						for (int k = 0; k < this.groupList.Count; k++)
						{
							if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
							{
								this.poseIndex[this.selectMaidIndex] = (int)this.groupList[k];
								break;
							}
						}
						if (num13 == this.poseIndex[this.selectMaidIndex] && this.poseIndex[this.selectMaidIndex] >= (int)this.groupList[this.groupList.Count - 1])
						{
							this.poseIndex[this.selectMaidIndex] = 0;
						}
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
					int num11 = -1;
					for (int k = 0; k < this.groupList.Count; k++)
					{
						if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
						{
							num11 = k;
							break;
						}
					}
					int num2 = (int)this.groupList[0];
					int num12 = 0;
					if (num11 > 0)
					{
						num2 = (int)this.groupList[num11] - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					if (num11 < 0)
					{
						num11 = this.groupList.Count;
						num2 = this.poseArray.Length - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					this.poseComboList = new GUIContent[num2];
					int j = 0;
					for (int i = num12; i < num12 + num2; i++)
					{
						bool flag = false;
						List<IniKey> keys = base.Preferences["pose"].Keys;
						foreach (IniKey iniKey in keys)
						{
							if (this.poseArray[i] == iniKey.Key)
							{
								IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
								if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
								{
									this.poseComboList[j] = new GUIContent(string.Concat(new object[]
									{
										j + 1,
										":",
										iniKey2.Value.Split(new char[]
										{
											'_'
										})[0],
										"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
										iniKey.Key
									}));
									flag = true;
								}
							}
						}
						if (!flag)
						{
							this.poseComboList[j] = new GUIContent(j + 1 + ":" + this.poseArray[i]);
						}
						j++;
					}
					this.poseCombo.scrollPos = new Vector2(0f, 0f);
					this.poseGroupCombo.selectedItemIndex = num11;
					this.poseCombo.selectedItemIndex = 0;
				}
				this.poseGroupIndex = this.poseGroupCombo.List(new Rect((float)this.GetPix(35), (float)this.GetPix(95), (float)this.GetPix(95), (float)this.GetPix(23)), this.poseGroupComboList[num10].text, this.poseGroupComboList, guistyle5, "box", this.listStyle4);
				if (this.poseGroupCombo.isClickedComboButton)
				{
					this.isCombo2 = true;
				}
				else if (this.isCombo2)
				{
					this.isCombo2 = false;
					this.isPoseInit = true;
					if (this.poseGroupIndex > 0)
					{
						this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex - 1];
					}
					else
					{
						this.poseIndex[this.selectMaidIndex] = 0;
					}
					if (!this.isLock[this.selectMaidIndex])
					{
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
					int num11 = -1;
					for (int k = 0; k < this.groupList.Count; k++)
					{
						if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
						{
							num11 = k;
							break;
						}
					}
					int num2 = (int)this.groupList[0];
					int num12 = 0;
					if (num11 > 0)
					{
						num2 = (int)this.groupList[num11] - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					if (num11 < 0)
					{
						num11 = this.groupList.Count;
						num2 = this.poseArray.Length - (int)this.groupList[num11 - 1];
						num12 = (int)this.groupList[num11 - 1];
					}
					this.poseComboList = new GUIContent[num2];
					int j = 0;
					for (int i = num12; i < num12 + num2; i++)
					{
						bool flag = false;
						List<IniKey> keys = base.Preferences["pose"].Keys;
						foreach (IniKey iniKey in keys)
						{
							if (this.poseArray[i] == iniKey.Key)
							{
								IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
								if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
								{
									this.poseComboList[j] = new GUIContent(string.Concat(new object[]
									{
										j + 1,
										":",
										iniKey2.Value.Split(new char[]
										{
											'_'
										})[0],
										"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
										iniKey.Key
									}));
									flag = true;
								}
							}
						}
						if (!flag)
						{
							this.poseComboList[j] = new GUIContent(j + 1 + ":" + this.poseArray[i]);
						}
						j++;
					}
					this.poseCombo.scrollPos = new Vector2(0f, 0f);
					this.poseGroupCombo.selectedItemIndex = num11;
					this.poseCombo.selectedItemIndex = 0;
				}
				if (this.poseCombo.isClickedComboButton)
				{
					this.isCombo = true;
				}
				else if (this.isCombo)
				{
					this.isCombo = false;
					this.isPoseInit = true;
					if (this.poseGroupIndex > 0)
					{
						this.poseIndex[this.selectMaidIndex] = (int)this.groupList[this.poseGroupIndex - 1] + this.poseCombo.selectedItemIndex;
					}
					else
					{
						this.poseIndex[this.selectMaidIndex] = this.poseCombo.selectedItemIndex;
					}
					if (this.poseIndex[this.selectMaidIndex] == this.poseArray.Length)
					{
						this.poseIndex[this.selectMaidIndex] = 0;
					}
					if (!this.isLock[this.selectMaidIndex])
					{
						if (maid && maid.Visible)
						{
							string[] array = this.poseArray[this.poseIndex[this.selectMaidIndex]].Split(new char[]
							{
								','
							});
							this.isStop[this.selectMaidIndex] = false;
							this.poseCount[this.selectMaidIndex] = 20;
							if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
							{
								maid.body0.MuneYureL(0f);
								maid.body0.MuneYureR(0f);
							}
							else
							{
								maid.body0.MuneYureL(1f);
								maid.body0.MuneYureR(1f);
							}
							int num4;
							if (array[0].Contains("MultipleMaidsPose"))
							{
								string path3 = array[0].Split(new char[]
								{
									'/'
								})[1];
								byte[] array2 = new byte[0];
								try
								{
									using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
									{
										array2 = new byte[fileStream.Length];
										fileStream.Read(array2, 0, array2.Length);
									}
								}
								catch
								{
								}
								if (0 < array2.Length)
								{
									string fileName = Path.GetFileName(path3);
									long num3 = (long)fileName.GetHashCode();
									maid.body0.CrossFade(num3.ToString(), array2, false, false, false, 0f, 1f);
									Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
									{
										default(Maid.AutoTwist),
										Maid.AutoTwist.ShoulderR,
										Maid.AutoTwist.WristL,
										Maid.AutoTwist.WristR,
										Maid.AutoTwist.ThighL,
										Maid.AutoTwist.ThighR
									};
									for (int i = 0; i < array3.Length; i++)
									{
										maid.SetAutoTwist(array3[i], true);
									}
								}
							}
							else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num4))
							{
								this.loadPose[this.selectMaidIndex] = array[0];
							}
							else if (!array[0].StartsWith("dance_"))
							{
								this.maidArray[this.selectMaidIndex].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
							}
							else
							{
								if (!maid.body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
								{
									maid.body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
								}
								maid.body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
							}
							if (array.Length > 1)
							{
								maid.body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
								this.isDanceStop = true;
								if (array.Length > 2)
								{
									Transform transform = CMT.SearchObjName(this.maidArray[this.selectMaidIndex].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[this.selectMaidIndex] = true;
									this.poseIti[this.selectMaidIndex] = this.maidArray[this.selectMaidIndex].transform.position;
									this.maidArray[this.selectMaidIndex].transform.position = new Vector3(100f, 100f, 100f);
								}
							}
						}
					}
				}
				if (this.itemCombo.isClickedComboButton)
				{
					this.isCombo3 = true;
				}
				else if (this.isCombo3)
				{
					this.isCombo3 = false;
					string[] array = new string[2];
					array = this.itemArray[this.itemIndex2[this.selectMaidIndex]].Split(new char[]
					{
						','
					});
					if (this.itemIndex2[this.selectMaidIndex] > 13)
					{
						array = this.itemArray[this.itemIndex2[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
					}
					maid.DelProp(MPN.handitem, true);
					bool flag2 = false;
					if (this.itemIndex2[this.selectMaidIndex] == 0)
					{
						maid.DelProp(MPN.accvag, true);
						maid.DelProp(MPN.accanl, true);
					}
					if (this.itemIndex2[this.selectMaidIndex] == 12 || this.itemIndex2[this.selectMaidIndex] == 13)
					{
						flag2 = true;
					}
					if (!flag2)
					{
						maid.DelProp(MPN.kousoku_upper, true);
						maid.DelProp(MPN.kousoku_lower, true);
					}
					if (array[0] != "")
					{
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex2[this.selectMaidIndex] == 12)
					{
						array = this.itemArray[this.itemIndex2[this.selectMaidIndex] - 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex2[this.selectMaidIndex] == 13)
					{
						array = this.itemArray[this.itemIndex2[this.selectMaidIndex] + 1].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
					}
					if (this.itemIndex2[this.selectMaidIndex] == 23)
					{
						array = this.itemArray[this.itemIndex2[this.selectMaidIndex]].Split(new char[]
						{
							','
						});
						maid.SetProp(array[0], array[1], 0, true, false);
						this.cafeFlg[this.selectMaidIndex] = true;
					}
					maid.AllProcPropSeqStart();
					this.itemCombo.selectedItemIndex = this.itemIndex2[this.selectMaidIndex];
					this.itemIndex[this.selectMaidIndex] = this.itemIndex2[this.selectMaidIndex];
				}
			}
			else
			{
				this.isWear = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(98), (float)this.GetPix(70), (float)this.GetPix(20)), this.isWear, "トップス", guistyle6);
				this.isSkirt = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(98), (float)this.GetPix(70), (float)this.GetPix(20)), this.isSkirt, "ボトムス", guistyle6);
				this.isBra = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(123), (float)this.GetPix(80), (float)this.GetPix(20)), this.isBra, "ブラジャー", guistyle6);
				this.isPanz = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(123), (float)this.GetPix(60), (float)this.GetPix(20)), this.isPanz, "パンツ", guistyle6);
				this.isHeadset = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(148), (float)this.GetPix(70), (float)this.GetPix(20)), this.isHeadset, "ヘッド", guistyle6);
				this.isMegane = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(148), (float)this.GetPix(70), (float)this.GetPix(20)), this.isMegane, "メガネ", guistyle6);
				this.isAccUde = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(173), (float)this.GetPix(40), (float)this.GetPix(20)), this.isAccUde, "腕", guistyle6);
				this.isGlove = GUI.Toggle(new Rect((float)this.GetPix(45), (float)this.GetPix(173), (float)this.GetPix(40), (float)this.GetPix(20)), this.isGlove, "手袋", guistyle6);
				this.isAccSenaka = GUI.Toggle(new Rect((float)this.GetPix(97), (float)this.GetPix(173), (float)this.GetPix(40), (float)this.GetPix(20)), this.isAccSenaka, "背中", guistyle6);
				this.isStkg = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(198), (float)this.GetPix(70), (float)this.GetPix(20)), this.isStkg, "ソックス", guistyle6);
				this.isShoes = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(198), (float)this.GetPix(70), (float)this.GetPix(20)), this.isShoes, "シューズ", guistyle6);
				this.mekure1[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(223), (float)this.GetPix(62), (float)this.GetPix(20)), this.mekure1[this.selectMaidIndex], "めくれ前", guistyle6);
				this.mekure2[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(67), (float)this.GetPix(223), (float)this.GetPix(40), (float)this.GetPix(20)), this.mekure2[this.selectMaidIndex], "後ろ", guistyle6);
				this.zurasi[this.selectMaidIndex] = GUI.Toggle(new Rect((float)this.GetPix(105), (float)this.GetPix(223), (float)this.GetPix(50), (float)this.GetPix(20)), this.zurasi[this.selectMaidIndex], "ずらし", guistyle6);
			}
		}

		private void GuiFunc2(int winID)
		{
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(12);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "label";
			guistyle2.fontSize = this.GetPix(11);
			guistyle2.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle3 = "button";
			guistyle3.fontSize = this.GetPix(20);
			GUIStyle guistyle4 = "button";
			guistyle4.fontSize = this.GetPix(12);
			guistyle4.alignment = TextAnchor.MiddleLeft;
			GUIStyle guistyle5 = new GUIStyle("toggle");
			guistyle5.fontSize = this.GetPix(13);
			CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
			if (!this.faceInitFlg)
			{
				this.listStyle2.normal.textColor = Color.white;
				this.listStyle2.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle2.onHover.background = (this.listStyle2.hover.background = new Texture2D(2, 2));
				this.listStyle2.padding.left = (this.listStyle2.padding.right = (this.listStyle2.padding.top = (this.listStyle2.padding.bottom = this.GetPix(0))));
				this.listStyle2.fontSize = this.GetPix(12);
				this.faceCombo.selectedItemIndex = 0;
				List<string> list = new List<string>(300);
				list.AddRange(this.faceArray);
				for (int i = 1; i < 300; i++)
				{
					IniKey iniKey = base.Preferences["face"]["f" + i];
					if (iniKey.Value == null)
					{
						break;
					}
					string[] array = iniKey.Value.Split(new char[]
					{
						':'
					});
					if (array.Length > 1)
					{
						list.Add(string.Concat(new object[]
						{
							array[0],
							"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000:",
							i,
							":",
							array[1]
						}));
					}
				}
				this.faceCombo.selectedItemIndex = 0;
				this.faceComboList = new GUIContent[list.ToArray().Length];
				for (int i = 0; i < list.ToArray().Length; i++)
				{
					this.faceComboList[i] = new GUIContent(list.ToArray()[i]);
				}
				this.faceInitFlg = true;
			}
			if (this.faceCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (this.sceneLevel == 3 || this.sceneLevel == 5 || this.isF6)
			{
				if (!this.isF6)
				{
					bool value = true;
					if (this.faceFlg || this.poseFlg || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
					{
						value = false;
					}
					if (GUI.Toggle(new Rect((float)this.GetPix(2), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), value, "配置", guistyle5))
					{
						this.faceFlg = false;
						this.poseFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
						this.bGui = true;
						this.isGuiInit = true;
					}
				}
				if (!this.yotogiFlg)
				{
					if (GUI.Toggle(new Rect((float)this.GetPix(41), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.poseFlg, "操作", guistyle5))
					{
						this.poseFlg = true;
						this.faceFlg = false;
						this.sceneFlg = false;
						this.kankyoFlg = false;
						this.kankyo2Flg = false;
					}
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(80), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.faceFlg, "表情", guistyle5))
				{
					this.faceFlg = true;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					if (!this.faceFlg2)
					{
						this.isFaceInit = true;
						this.faceFlg2 = true;
						this.maidArray[this.selectMaidIndex].boMabataki = false;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
				}
				if (GUI.Toggle(new Rect((float)this.GetPix(119), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyoFlg, "環境", guistyle5))
				{
					this.poseFlg = false;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = true;
					this.kankyo2Flg = false;
				}
				if (!this.line1)
				{
					this.line1 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.3f));
					this.line2 = this.MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 0.6f));
				}
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 2f), this.line1);
				GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 1f), this.line2);
				guistyle.fontSize = this.GetPix(13);
				guistyle.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect((float)this.GetPix(125), (float)this.GetPix(25), (float)this.GetPix(40), (float)this.GetPix(25)), string.Concat(this.selectMaidIndex + 1), guistyle);
				guistyle.fontSize = this.GetPix(11);
				guistyle.alignment = TextAnchor.UpperLeft;
			}
			if (this.sceneLevel > 0)
			{
				int i = this.selectMaidIndex;
				if (this.sceneLevel == 3 || (this.sceneLevel == 5 && (this.isF7 || this.maidCnt > 1)))
				{
					if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(32)), "＜", guistyle3))
					{
						this.selectMaidIndex--;
						if (this.selectMaidIndex < 0)
						{
							this.selectMaidIndex = this.selectList.Count - 1;
						}
						this.isFaceInit = true;
						this.faceFlg = true;
						this.copyIndex = 0;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
					if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(53), (float)this.GetPix(23), (float)this.GetPix(32)), "＞", guistyle3))
					{
						this.selectMaidIndex++;
						if (this.selectList.Count <= this.selectMaidIndex)
						{
							this.selectMaidIndex = 0;
						}
						this.isFaceInit = true;
						this.faceFlg = true;
						this.copyIndex = 0;
						this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
					}
				}
				if (this.maidArray[this.selectMaidIndex].GetThumIcon())
				{
					GUI.DrawTexture(new Rect((float)this.GetPix(30), (float)this.GetPix(25), (float)this.GetPix(60), (float)this.GetPix(60)), this.maidArray[this.selectMaidIndex].GetThumIcon());
				}
				string text = this.maidArray[this.selectMaidIndex].status.lastName + "\n" + this.maidArray[this.selectMaidIndex].status.firstName;
				GUI.Label(new Rect((float)this.GetPix(90), (float)this.GetPix(50), (float)this.GetPix(140), (float)this.GetPix(210)), text, guistyle);
				bool flag = GUI.Toggle(new Rect((float)this.GetPix(90), (float)this.GetPix(25), (float)this.GetPix(50), (float)this.GetPix(16)), this.isShosai, "詳細", guistyle5);
				if (flag != this.isShosai)
				{
					this.isShosai = flag;
					if (this.isShosai)
					{
						base.Preferences["config"]["hair_details"].Value = "true";
					}
					else
					{
						base.Preferences["config"]["hair_details"].Value = "false";
					}
					base.SaveConfig();
				}
				if (this.isFace[i])
				{
					if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(25), (float)this.GetPix(23), (float)this.GetPix(23)), "有", guistyle3))
					{
						TMorph morph = this.maidArray[i].body0.Face.morph;
						this.maidArray[i].boMabataki = false;
						this.isFace[i] = false;
					}
					this.maidArray[i].boMabataki = false;
				}
				else
				{
					if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(25), (float)this.GetPix(23), (float)this.GetPix(23)), "無", guistyle3))
					{
						TMorph morph = this.maidArray[i].body0.Face.morph;
						this.maidArray[i].boMabataki = false;
						morph.EyeMabataki = 0f;
						this.isFaceInit = true;
						this.isFace[i] = true;
						this.faceCombo.selectedItemIndex = this.faceIndex[i];
					}
					GUI.enabled = false;
					this.maidArray[i].boMabataki = true;
				}
			}
			if (GUI.Button(new Rect((float)this.GetPix(5), (float)this.GetPix(95), (float)this.GetPix(23), (float)this.GetPix(23)), "＜", guistyle3))
			{
				this.faceIndex[this.selectMaidIndex]--;
				if (this.faceIndex[this.selectMaidIndex] <= -1)
				{
					this.faceIndex[this.selectMaidIndex] = this.faceComboList.Length - 1;
				}
				TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
				this.maidArray[this.selectMaidIndex].boMabataki = false;
				morph.EyeMabataki = 0f;
				if (this.faceIndex[this.selectMaidIndex] < this.faceArray.Length)
				{
					morph.MulBlendValues(this.faceArray[this.faceIndex[this.selectMaidIndex]], 1f);
					if (morph.bodyskin.PartsVersion >= 120)
					{
						float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
						fieldValue[(int)morph.hash["eyeclose3"]] *= 3f;
					}
				}
				else
				{
					float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
					float[] fieldValue2 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
					if (!this.isVR)
					{
						this.maidArray[this.selectMaidIndex].boMabataki = false;
					}
					string[] array = this.faceComboList[this.faceIndex[this.selectMaidIndex]].text.Split(new char[]
					{
						':'
					})[2].Split(new char[]
					{
						','
					});
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue2[(int)morph.hash["eyeclose"]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2"]] = float.Parse(array[1]);
						fieldValue2[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]);
						fieldValue2[(int)morph.hash["eyeclose6"]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5"]] = float.Parse(array[17]);
						if (morph.hash["eyeclose7"] != null)
						{
							if (array.Length > 37)
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = float.Parse(array[36]);
								fieldValue2[(int)morph.hash["eyeclose8"]] = float.Parse(array[37]);
							}
							else
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = 0f;
								fieldValue2[(int)morph.hash["eyeclose8"]] = 0f;
							}
						}
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						fieldValue2[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[1]);
						fieldValue[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]) * 3f;
						fieldValue2[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[17]);
						if (array.Length > 37)
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[36]);
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[37]);
						}
						else
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = 0f;
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = 0f;
						}
					}
					fieldValue[(int)morph.hash["hitomih"]] = float.Parse(array[4]);
					fieldValue[(int)morph.hash["hitomis"]] = float.Parse(array[5]);
					fieldValue[(int)morph.hash["mayuha"]] = float.Parse(array[6]);
					fieldValue[(int)morph.hash["mayuup"]] = float.Parse(array[7]);
					fieldValue[(int)morph.hash["mayuv"]] = float.Parse(array[8]);
					fieldValue[(int)morph.hash["mayuvhalf"]] = float.Parse(array[9]);
					fieldValue[(int)morph.hash["moutha"]] = float.Parse(array[10]);
					fieldValue[(int)morph.hash["mouths"]] = float.Parse(array[11]);
					fieldValue[(int)morph.hash["mouthdw"]] = float.Parse(array[12]);
					fieldValue[(int)morph.hash["mouthup"]] = float.Parse(array[13]);
					fieldValue[(int)morph.hash["tangout"]] = float.Parse(array[14]);
					fieldValue[(int)morph.hash["tangup"]] = float.Parse(array[15]);
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]);
					}
					else
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]) * 3f;
					}
					fieldValue[(int)morph.hash["mayuw"]] = float.Parse(array[18]);
					fieldValue[(int)morph.hash["mouthhe"]] = float.Parse(array[19]);
					fieldValue[(int)morph.hash["mouthc"]] = float.Parse(array[20]);
					fieldValue[(int)morph.hash["mouthi"]] = float.Parse(array[21]);
					fieldValue[(int)morph.hash["mouthuphalf"]] = float.Parse(array[22]) + 0.01f;
					try
					{
						fieldValue[(int)morph.hash["tangopen"]] = float.Parse(array[23]);
					}
					catch
					{
					}
					if (float.Parse(array[24]) == 1f)
					{
						fieldValue[(int)morph.hash["namida"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["namida"]] = 0f;
					}
					if (float.Parse(array[25]) == 1f)
					{
						fieldValue[(int)morph.hash["tear1"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear1"]] = 0f;
					}
					if (float.Parse(array[26]) == 1f)
					{
						fieldValue[(int)morph.hash["tear2"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear2"]] = 0f;
					}
					if (float.Parse(array[27]) == 1f)
					{
						fieldValue[(int)morph.hash["tear3"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear3"]] = 0f;
					}
					if (float.Parse(array[28]) == 1f)
					{
						fieldValue[(int)morph.hash["shock"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["shock"]] = 0f;
					}
					if (float.Parse(array[29]) == 1f)
					{
						fieldValue[(int)morph.hash["yodare"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["yodare"]] = 0f;
					}
					if (float.Parse(array[30]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho"]] = 0f;
					}
					if (float.Parse(array[31]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0f;
					}
					if (float.Parse(array[32]) == 1f)
					{
						fieldValue[(int)morph.hash["hohos"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohos"]] = 0f;
					}
					if (float.Parse(array[33]) == 1f)
					{
						fieldValue[(int)morph.hash["hohol"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohol"]] = 0f;
					}
					if (float.Parse(array[34]) == 1f)
					{
						fieldValue[(int)morph.hash["toothoff"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["toothoff"]] = 0f;
					}
					if (array.Length > 35)
					{
						if (float.Parse(array[35]) == 1f)
						{
							morph.boNoseFook = true;
						}
						else
						{
							morph.boNoseFook = false;
						}
					}
				}
				this.maidArray[this.selectMaidIndex].body0.Face.morph.FixBlendValues_Face();
				this.isFaceInit = true;
				this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
			}
			if (GUI.Button(new Rect((float)this.GetPix(135), (float)this.GetPix(95), (float)this.GetPix(23), (float)this.GetPix(23)), "＞", guistyle3))
			{
				this.faceIndex[this.selectMaidIndex]++;
				if (this.faceIndex[this.selectMaidIndex] == this.faceComboList.Length)
				{
					this.faceIndex[this.selectMaidIndex] = 0;
				}
				TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
				this.maidArray[this.selectMaidIndex].boMabataki = false;
				morph.EyeMabataki = 0f;
				if (this.faceIndex[this.selectMaidIndex] < this.faceArray.Length)
				{
					morph.MulBlendValues(this.faceArray[this.faceIndex[this.selectMaidIndex]], 1f);
					if (morph.bodyskin.PartsVersion >= 120)
					{
						float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
						fieldValue[(int)morph.hash["eyeclose3"]] *= 3f;
					}
				}
				else
				{
					float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
					float[] fieldValue2 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
					if (!this.isVR)
					{
						this.maidArray[this.selectMaidIndex].boMabataki = false;
					}
					string[] array = this.faceComboList[this.faceIndex[this.selectMaidIndex]].text.Split(new char[]
					{
						':'
					})[2].Split(new char[]
					{
						','
					});
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue2[(int)morph.hash["eyeclose"]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2"]] = float.Parse(array[1]);
						fieldValue2[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]);
						fieldValue2[(int)morph.hash["eyeclose6"]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5"]] = float.Parse(array[17]);
						if (morph.hash["eyeclose7"] != null)
						{
							if (array.Length > 37)
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = float.Parse(array[36]);
								fieldValue2[(int)morph.hash["eyeclose8"]] = float.Parse(array[37]);
							}
							else
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = 0f;
								fieldValue2[(int)morph.hash["eyeclose8"]] = 0f;
							}
						}
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						fieldValue2[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[1]);
						fieldValue[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]) * 3f;
						fieldValue2[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[17]);
						if (array.Length > 37)
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[36]);
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[37]);
						}
						else
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = 0f;
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = 0f;
						}
					}
					fieldValue[(int)morph.hash["hitomih"]] = float.Parse(array[4]);
					fieldValue[(int)morph.hash["hitomis"]] = float.Parse(array[5]);
					fieldValue[(int)morph.hash["mayuha"]] = float.Parse(array[6]);
					fieldValue[(int)morph.hash["mayuup"]] = float.Parse(array[7]);
					fieldValue[(int)morph.hash["mayuv"]] = float.Parse(array[8]);
					fieldValue[(int)morph.hash["mayuvhalf"]] = float.Parse(array[9]);
					fieldValue[(int)morph.hash["moutha"]] = float.Parse(array[10]);
					fieldValue[(int)morph.hash["mouths"]] = float.Parse(array[11]);
					fieldValue[(int)morph.hash["mouthdw"]] = float.Parse(array[12]);
					fieldValue[(int)morph.hash["mouthup"]] = float.Parse(array[13]);
					fieldValue[(int)morph.hash["tangout"]] = float.Parse(array[14]);
					fieldValue[(int)morph.hash["tangup"]] = float.Parse(array[15]);
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]);
					}
					else
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]) * 3f;
					}
					fieldValue[(int)morph.hash["mayuw"]] = float.Parse(array[18]);
					fieldValue[(int)morph.hash["mouthhe"]] = float.Parse(array[19]);
					fieldValue[(int)morph.hash["mouthc"]] = float.Parse(array[20]);
					fieldValue[(int)morph.hash["mouthi"]] = float.Parse(array[21]);
					fieldValue[(int)morph.hash["mouthuphalf"]] = float.Parse(array[22]) + 0.01f;
					try
					{
						fieldValue[(int)morph.hash["tangopen"]] = float.Parse(array[23]);
					}
					catch
					{
					}
					if (float.Parse(array[24]) == 1f)
					{
						fieldValue[(int)morph.hash["namida"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["namida"]] = 0f;
					}
					if (float.Parse(array[25]) == 1f)
					{
						fieldValue[(int)morph.hash["tear1"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear1"]] = 0f;
					}
					if (float.Parse(array[26]) == 1f)
					{
						fieldValue[(int)morph.hash["tear2"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear2"]] = 0f;
					}
					if (float.Parse(array[27]) == 1f)
					{
						fieldValue[(int)morph.hash["tear3"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear3"]] = 0f;
					}
					if (float.Parse(array[28]) == 1f)
					{
						fieldValue[(int)morph.hash["shock"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["shock"]] = 0f;
					}
					if (float.Parse(array[29]) == 1f)
					{
						fieldValue[(int)morph.hash["yodare"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["yodare"]] = 0f;
					}
					if (float.Parse(array[30]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho"]] = 0f;
					}
					if (float.Parse(array[31]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0f;
					}
					if (float.Parse(array[32]) == 1f)
					{
						fieldValue[(int)morph.hash["hohos"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohos"]] = 0f;
					}
					if (float.Parse(array[33]) == 1f)
					{
						fieldValue[(int)morph.hash["hohol"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohol"]] = 0f;
					}
					if (float.Parse(array[34]) == 1f)
					{
						fieldValue[(int)morph.hash["toothoff"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["toothoff"]] = 0f;
					}
					if (array.Length > 35)
					{
						if (float.Parse(array[35]) == 1f)
						{
							morph.boNoseFook = true;
						}
						else
						{
							morph.boNoseFook = false;
						}
					}
				}
				this.maidArray[this.selectMaidIndex].body0.Face.morph.FixBlendValues_Face();
				this.isFaceInit = true;
				this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
			}
			TMorph morph2 = this.maidArray[this.selectMaidIndex].body0.Face.morph;
			if (!this.isShosai)
			{
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(130), (float)this.GetPix(100), (float)this.GetPix(25)), "目の開閉", guistyle);
				this.eyeclose = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(150), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(130), (float)this.GetPix(100), (float)this.GetPix(25)), "にっこり", guistyle);
				this.eyeclose2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(150), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose2, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(170), (float)this.GetPix(100), (float)this.GetPix(25)), "ジト目", guistyle);
				if (morph2.bodyskin.PartsVersion < 120)
				{
					this.eyeclose3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(190), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose3, 0f, 1f);
				}
				else
				{
					this.eyeclose3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(190), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose3, 0f, 3f);
				}
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(170), (float)this.GetPix(100), (float)this.GetPix(25)), "ウインク", guistyle);
				this.eyeclose6 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(190), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose6, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(210), (float)this.GetPix(100), (float)this.GetPix(25)), "ハイライト", guistyle);
				this.hitomih = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(230), (float)this.GetPix(70), (float)this.GetPix(20)), this.hitomih, 0f, 2f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(210), (float)this.GetPix(100), (float)this.GetPix(25)), "瞳サイズ", guistyle);
				this.hitomis = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(230), (float)this.GetPix(70), (float)this.GetPix(20)), this.hitomis, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(250), (float)this.GetPix(100), (float)this.GetPix(25)), "眉角度", guistyle);
				this.mayuha = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(270), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuha, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(290), (float)this.GetPix(100), (float)this.GetPix(25)), "眉上げ", guistyle);
				this.mayuup = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(310), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuup, 0f, 0.8f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(290), (float)this.GetPix(100), (float)this.GetPix(25)), "眉下げ", guistyle);
				this.mayuv = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(310), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuv, 0f, 0.8f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(330), (float)this.GetPix(100), (float)this.GetPix(25)), "口開け1", guistyle);
				this.moutha = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(350), (float)this.GetPix(70), (float)this.GetPix(20)), this.moutha, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(330), (float)this.GetPix(100), (float)this.GetPix(25)), "口開け2", guistyle);
				this.mouths = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(350), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouths, 0f, 0.9f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(370), (float)this.GetPix(100), (float)this.GetPix(25)), "口角上げ", guistyle);
				this.mouthup = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(390), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthup, 0f, 1.4f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(370), (float)this.GetPix(100), (float)this.GetPix(25)), "口角下げ", guistyle);
				this.mouthdw = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(390), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthdw, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(410), (float)this.GetPix(100), (float)this.GetPix(25)), "舌出し", guistyle);
				this.tangout = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(430), (float)this.GetPix(70), (float)this.GetPix(20)), this.tangout, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(410), (float)this.GetPix(100), (float)this.GetPix(25)), "舌上げ", guistyle);
				this.tangup = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(430), (float)this.GetPix(70), (float)this.GetPix(20)), this.tangup, 0f, 0.7f);
				this.isHoho2 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(455), (float)this.GetPix(50), (float)this.GetPix(20)), this.isHoho2, "赤面", guistyle5);
				this.isShock = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(455), (float)this.GetPix(70), (float)this.GetPix(20)), this.isShock, "ショック", guistyle5);
				this.isNamida = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(480), (float)this.GetPix(50), (float)this.GetPix(20)), this.isNamida, "涙", guistyle5);
				this.isYodare = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(480), (float)this.GetPix(50), (float)this.GetPix(20)), this.isYodare, "涎", guistyle5);
				this.isTear1 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(505), (float)this.GetPix(50), (float)this.GetPix(20)), this.isTear1, "涙1", guistyle5);
				this.isTear2 = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(505), (float)this.GetPix(50), (float)this.GetPix(20)), this.isTear2, "涙2", guistyle5);
				this.isTear3 = GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(505), (float)this.GetPix(50), (float)this.GetPix(20)), this.isTear3, "涙3", guistyle5);
				this.isHohos = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(530), (float)this.GetPix(50), (float)this.GetPix(20)), this.isHohos, "頬1", guistyle5);
				this.isHoho = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(530), (float)this.GetPix(50), (float)this.GetPix(20)), this.isHoho, "頬2", guistyle5);
				this.isHohol = GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(530), (float)this.GetPix(50), (float)this.GetPix(20)), this.isHohol, "頬3", guistyle5);
			}
			else
			{
				int num2 = 120;
				int num3 = 135;
				int num4 = 28;
				if (morph2.bodyskin.PartsVersion < 120)
				{
					if (morph2.hash["eyeclose7"] != null)
					{
						num4 = 26;
					}
				}
				else
				{
					num4 = 26;
				}
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2), (float)this.GetPix(100), (float)this.GetPix(25)), "目の開閉", guistyle2);
				this.eyeclose = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2), (float)this.GetPix(100), (float)this.GetPix(25)), "にっこり", guistyle2);
				this.eyeclose2 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose2, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4), (float)this.GetPix(100), (float)this.GetPix(25)), "ジト目", guistyle2);
				if (morph2.bodyskin.PartsVersion < 120)
				{
					this.eyeclose3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose3, 0f, 1f);
				}
				else
				{
					this.eyeclose3 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose3, 0f, 3f);
				}
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4), (float)this.GetPix(100), (float)this.GetPix(25)), "見開く", guistyle2);
				if (morph2.bodyskin.PartsVersion < 120)
				{
					this.eyebig = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyebig, 0f, 1f);
				}
				else
				{
					this.eyebig = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyebig, 0f, 3f);
				}
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "ウインク1", guistyle2);
				this.eyeclose6 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose6, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "ウインク2", guistyle2);
				this.eyeclose5 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose5, 0f, 1f);
				if (morph2.bodyskin.PartsVersion < 120)
				{
					if (morph2.hash["eyeclose7"] != null)
					{
						num2 += num4;
						num3 += num4;
						GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "右ウインク1", guistyle2);
						this.eyeclose8 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose8, 0f, 1f);
						GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "右ウインク2", guistyle2);
						this.eyeclose7 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose7, 0f, 1f);
					}
				}
				else
				{
					num2 += num4;
					num3 += num4;
					GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "右ウインク1", guistyle2);
					this.eyeclose8 = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose8, 0f, 1f);
					GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 2), (float)this.GetPix(100), (float)this.GetPix(25)), "右ウインク2", guistyle2);
					this.eyeclose7 = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 2), (float)this.GetPix(70), (float)this.GetPix(20)), this.eyeclose7, 0f, 1f);
				}
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 3), (float)this.GetPix(100), (float)this.GetPix(25)), "ハイライト", guistyle2);
				this.hitomih = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 3), (float)this.GetPix(70), (float)this.GetPix(20)), this.hitomih, 0f, 2f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 3), (float)this.GetPix(100), (float)this.GetPix(25)), "瞳サイズ", guistyle2);
				this.hitomis = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 3), (float)this.GetPix(70), (float)this.GetPix(20)), this.hitomis, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 4), (float)this.GetPix(100), (float)this.GetPix(25)), "眉角度1", guistyle2);
				this.mayuha = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 4), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuha, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 4), (float)this.GetPix(100), (float)this.GetPix(25)), "眉角度2", guistyle2);
				this.mayuw = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 4), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuw, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 5), (float)this.GetPix(100), (float)this.GetPix(25)), "眉上げ", guistyle2);
				this.mayuup = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 5), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuup, 0f, 0.8f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 5), (float)this.GetPix(100), (float)this.GetPix(25)), "眉下げ1", guistyle2);
				this.mayuv = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 5), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuv, 0f, 0.8f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 6), (float)this.GetPix(100), (float)this.GetPix(25)), "眉下げ2", guistyle2);
				this.mayuvhalf = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 6), (float)this.GetPix(70), (float)this.GetPix(20)), this.mayuvhalf, 0f, 0.9f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 7), (float)this.GetPix(100), (float)this.GetPix(25)), "口開け1", guistyle2);
				this.moutha = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 7), (float)this.GetPix(70), (float)this.GetPix(20)), this.moutha, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 7), (float)this.GetPix(100), (float)this.GetPix(25)), "口開け2", guistyle2);
				this.mouths = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 7), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouths, 0f, 0.9f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 8), (float)this.GetPix(100), (float)this.GetPix(25)), "口幅狭く", guistyle2);
				this.mouthc = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 8), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthc, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 8), (float)this.GetPix(100), (float)this.GetPix(25)), "口幅広く", guistyle2);
				this.mouthi = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 8), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthi, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 9), (float)this.GetPix(100), (float)this.GetPix(25)), "口角上げ", guistyle2);
				this.mouthup = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 9), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthup, 0f, 1.4f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 9), (float)this.GetPix(100), (float)this.GetPix(25)), "口角下げ", guistyle2);
				this.mouthdw = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 9), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthdw, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 10), (float)this.GetPix(100), (float)this.GetPix(25)), "口中央上げ", guistyle2);
				this.mouthhe = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 10), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthhe, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 10), (float)this.GetPix(100), (float)this.GetPix(25)), "左口角上げ", guistyle2);
				this.mouthuphalf = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 10), (float)this.GetPix(70), (float)this.GetPix(20)), this.mouthuphalf, 0f, 2f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 11), (float)this.GetPix(100), (float)this.GetPix(25)), "舌出し", guistyle2);
				this.tangout = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 11), (float)this.GetPix(70), (float)this.GetPix(20)), this.tangout, 0f, 1f);
				GUI.Label(new Rect((float)this.GetPix(88), (float)this.GetPix(num2 + num4 * 11), (float)this.GetPix(100), (float)this.GetPix(25)), "舌上げ", guistyle2);
				this.tangup = GUI.HorizontalSlider(new Rect((float)this.GetPix(88), (float)this.GetPix(num3 + num4 * 11), (float)this.GetPix(70), (float)this.GetPix(20)), this.tangup, 0f, 0.7f);
				GUI.Label(new Rect((float)this.GetPix(8), (float)this.GetPix(num2 + num4 * 12), (float)this.GetPix(100), (float)this.GetPix(25)), "舌根上げ", guistyle2);
				this.tangopen = GUI.HorizontalSlider(new Rect((float)this.GetPix(8), (float)this.GetPix(num3 + num4 * 12), (float)this.GetPix(70), (float)this.GetPix(20)), this.tangopen, 0f, 1f);
				bool enabled = GUI.enabled;
				if (!this.faceCombo.isClickedComboButton)
				{
					GUI.enabled = true;
				}
				GUI.enabled = enabled;
				this.isHoho2 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(489), (float)this.GetPix(50), (float)this.GetPix(16)), this.isHoho2, "赤面", guistyle5);
				this.isShock = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(489), (float)this.GetPix(58), (float)this.GetPix(16)), this.isShock, "ショック", guistyle5);
				this.isNosefook = GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(489), (float)this.GetPix(62), (float)this.GetPix(16)), this.isNosefook, "鼻フック", guistyle5);
				this.isNamida = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(511), (float)this.GetPix(50), (float)this.GetPix(16)), this.isNamida, "涙", guistyle5);
				this.isYodare = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(511), (float)this.GetPix(50), (float)this.GetPix(16)), this.isYodare, "涎", guistyle5);
				this.isToothoff = !GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(511), (float)this.GetPix(50), (float)this.GetPix(16)), !this.isToothoff, "歯", guistyle5);
				this.isTear1 = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(533), (float)this.GetPix(50), (float)this.GetPix(16)), this.isTear1, "涙1", guistyle5);
				this.isTear2 = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(533), (float)this.GetPix(50), (float)this.GetPix(16)), this.isTear2, "涙2", guistyle5);
				this.isTear3 = GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(533), (float)this.GetPix(50), (float)this.GetPix(16)), this.isTear3, "涙3", guistyle5);
				this.isHohos = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(555), (float)this.GetPix(50), (float)this.GetPix(16)), this.isHohos, "頬1", guistyle5);
				this.isHoho = GUI.Toggle(new Rect((float)this.GetPix(60), (float)this.GetPix(555), (float)this.GetPix(50), (float)this.GetPix(16)), this.isHoho, "頬2", guistyle5);
				this.isHohol = GUI.Toggle(new Rect((float)this.GetPix(115), (float)this.GetPix(555), (float)this.GetPix(50), (float)this.GetPix(16)), this.isHohol, "頬3", guistyle5);
			}
			int num5 = 0;
			if (this.isShosai)
			{
				num5 = 22;
			}
			this.isFaceEdit = GUI.Toggle(new Rect((float)this.GetPix(5), (float)this.GetPix(555 + num5), (float)this.GetPix(50), (float)this.GetPix(16)), this.isFaceEdit, "登録", guistyle5);
			if (this.isFaceEdit)
			{
				this.inName4 = GUI.TextField(new Rect((float)this.GetPix(5), (float)this.GetPix(575 + num5), (float)this.GetPix(100), (float)this.GetPix(20)), this.inName4);
				if (GUI.Button(new Rect((float)this.GetPix(107), (float)this.GetPix(575 + num5), (float)this.GetPix(35), (float)this.GetPix(20)), "追加", guistyle3))
				{
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					GUI.FocusControl("");
					int num6 = 1;
					for (int i = 1; i < 1000; i++)
					{
						IniKey iniKey = base.Preferences["face"]["f" + i];
						if (iniKey.Value == null)
						{
							num6 = i;
							break;
						}
					}
					TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
					float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
					float[] fieldValue2 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
					string text2 = this.inName4 + ":";
					if (morph.bodyskin.PartsVersion < 120)
					{
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose"]] + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose2"]] + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose3"]] + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose6"]] + ",";
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num]]] + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num]]] + ",";
						text2 = text2 + fieldValue[(int)morph.hash["eyeclose3"]] / 3f + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num]]] + ",";
					}
					text2 = text2 + fieldValue[(int)morph.hash["hitomih"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["hitomis"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mayuha"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mayuup"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mayuv"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mayuvhalf"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["moutha"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouths"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthdw"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthup"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["tangout"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["tangup"]] + ",";
					if (morph.bodyskin.PartsVersion < 120)
					{
						text2 = text2 + fieldValue[(int)morph.hash["eyebig"]] + ",";
					}
					else
					{
						text2 = text2 + fieldValue[(int)morph.hash["eyebig"]] / 3f + ",";
					}
					if (morph.bodyskin.PartsVersion < 120)
					{
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose5"]] + ",";
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num]]] + ",";
					}
					text2 = text2 + fieldValue[(int)morph.hash["mayuw"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthhe"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthc"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthi"]] + ",";
					text2 = text2 + fieldValue[(int)morph.hash["mouthuphalf"]] + ",";
					try
					{
						text2 = text2 + fieldValue[(int)morph.hash["tangopen"]] + ",";
					}
					catch
					{
						text2 += "0,";
					}
					if (fieldValue[(int)morph.hash["namida"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["tear1"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["tear2"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["tear3"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["shock"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["yodare"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["hoho"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["hoho2"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["hohos"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["hohol"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (fieldValue[(int)morph.hash["toothoff"]] > 0f)
					{
						text2 = text2 + 1 + ",";
					}
					else
					{
						text2 = text2 + 0 + ",";
					}
					if (morph.bodyskin.PartsVersion < 120)
					{
						if (morph.hash["eyeclose7"] != null)
						{
							text2 = text2 + fieldValue2[(int)morph.hash["eyeclose7"]] + ",";
							text2 = text2 + fieldValue2[(int)morph.hash["eyeclose8"]] + ":";
						}
						else
						{
							text2 += "0,0:";
						}
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] + ",";
						text2 = text2 + fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] + ":";
					}
					base.Preferences["face"]["f" + num6].Value = text2;
					base.SaveConfig();
					List<string> list = new List<string>(300);
					list.AddRange(this.faceArray);
					for (int i = 1; i < 300; i++)
					{
						IniKey iniKey = base.Preferences["face"]["f" + i];
						if (iniKey.Value == null)
						{
							break;
						}
						string[] array = iniKey.Value.Split(new char[]
						{
							':'
						});
						if (array.Length > 1)
						{
							list.Add(string.Concat(new object[]
							{
								array[0],
								"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000:",
								i,
								":",
								array[1]
							}));
						}
					}
					this.faceCombo.selectedItemIndex = 0;
					this.faceComboList = new GUIContent[list.ToArray().Length];
					for (int i = 0; i < list.ToArray().Length; i++)
					{
						this.faceComboList[i] = new GUIContent(list.ToArray()[i]);
					}
					this.faceCombo.selectedItemIndex = list.ToArray().Length - 1;
					this.inName4 = "";
				}
				if (this.faceIndex[this.selectMaidIndex] < this.faceArray.Length)
				{
					GUI.enabled = false;
				}
				if (GUI.Button(new Rect((float)this.GetPix(144), (float)this.GetPix(575 + num5), (float)this.GetPix(24), (float)this.GetPix(20)), "削", guistyle3))
				{
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					string[] array = this.faceComboList[this.faceIndex[this.selectMaidIndex]].text.Split(new char[]
					{
						':'
					});
					base.Preferences["face"]["f" + array[1]].Value = "del";
					base.SaveConfig();
					List<string> list = new List<string>(300);
					list.AddRange(this.faceArray);
					for (int i = 1; i < 300; i++)
					{
						IniKey iniKey = base.Preferences["face"]["f" + i];
						if (iniKey.Value == null)
						{
							break;
						}
						string[] array2 = iniKey.Value.Split(new char[]
						{
							':'
						});
						if (array2.Length > 1)
						{
							list.Add(string.Concat(new object[]
							{
								array2[0],
								"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000:",
								i,
								":",
								array2[1]
							}));
						}
					}
					this.faceCombo.selectedItemIndex = 0;
					this.faceComboList = new GUIContent[list.ToArray().Length];
					for (int i = 0; i < list.ToArray().Length; i++)
					{
						this.faceComboList[i] = new GUIContent(list.ToArray()[i]);
					}
					this.faceCombo.selectedItemIndex = 0;
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (list.ToArray().Length <= this.faceIndex[j])
							{
								this.faceIndex[j] = 0;
							}
						}
					}
				}
				GUI.enabled = true;
			}
			if (this.faceCombo.isClickedComboButton)
			{
				GUI.enabled = true;
			}
			if (this.isFace[this.selectMaidIndex])
			{
				this.faceIndex[this.selectMaidIndex] = this.faceCombo.List(new Rect((float)this.GetPix(35), (float)this.GetPix(95), (float)this.GetPix(95), (float)this.GetPix(23)), this.faceComboList[this.faceIndex[this.selectMaidIndex]].text, this.faceComboList, guistyle4, "box", this.listStyle2);
			}
			else
			{
				this.faceCombo.List(new Rect((float)this.GetPix(35), (float)this.GetPix(95), (float)this.GetPix(95), (float)this.GetPix(23)), this.faceComboList[this.faceIndex[this.selectMaidIndex]].text, this.faceComboList, guistyle4, "box", this.listStyle2);
			}
			if (this.faceCombo.isClickedComboButton)
			{
				this.isCombo = true;
			}
			else if (this.isCombo)
			{
				this.isCombo = false;
				TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
				float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
				morph.EyeMabataki = 0f;
				if (this.faceIndex[this.selectMaidIndex] < this.faceArray.Length)
				{
					morph.MulBlendValues(this.faceArray[this.faceIndex[this.selectMaidIndex]], 1f);
					if (morph.bodyskin.PartsVersion >= 120)
					{
						fieldValue[(int)morph.hash["eyeclose3"]] *= 3f;
					}
				}
				else
				{
					float[] fieldValue2 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
					if (!this.isVR)
					{
						this.maidArray[this.selectMaidIndex].boMabataki = false;
					}
					string[] array = this.faceComboList[this.faceIndex[this.selectMaidIndex]].text.Split(new char[]
					{
						':'
					})[2].Split(new char[]
					{
						','
					});
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue2[(int)morph.hash["eyeclose"]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2"]] = float.Parse(array[1]);
						fieldValue2[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]);
						fieldValue2[(int)morph.hash["eyeclose6"]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5"]] = float.Parse(array[17]);
						if (morph.hash["eyeclose7"] != null)
						{
							if (array.Length > 37)
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = float.Parse(array[36]);
								fieldValue2[(int)morph.hash["eyeclose8"]] = float.Parse(array[37]);
							}
							else
							{
								fieldValue2[(int)morph.hash["eyeclose7"]] = 0f;
								fieldValue2[(int)morph.hash["eyeclose8"]] = 0f;
							}
						}
					}
					else
					{
						int num = 0;
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
						{
							num = 1;
						}
						if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
						{
							num = 2;
						}
						fieldValue2[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[0]);
						fieldValue2[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[1]);
						fieldValue[(int)morph.hash["eyeclose3"]] = float.Parse(array[2]) * 3f;
						fieldValue2[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[3]);
						fieldValue2[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[17]);
						if (array.Length > 37)
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[36]);
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = float.Parse(array[37]);
						}
						else
						{
							fieldValue2[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num]]] = 0f;
							fieldValue2[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num]]] = 0f;
						}
					}
					fieldValue[(int)morph.hash["hitomih"]] = float.Parse(array[4]);
					fieldValue[(int)morph.hash["hitomis"]] = float.Parse(array[5]);
					fieldValue[(int)morph.hash["mayuha"]] = float.Parse(array[6]);
					fieldValue[(int)morph.hash["mayuup"]] = float.Parse(array[7]);
					fieldValue[(int)morph.hash["mayuv"]] = float.Parse(array[8]);
					fieldValue[(int)morph.hash["mayuvhalf"]] = float.Parse(array[9]);
					fieldValue[(int)morph.hash["moutha"]] = float.Parse(array[10]);
					fieldValue[(int)morph.hash["mouths"]] = float.Parse(array[11]);
					fieldValue[(int)morph.hash["mouthdw"]] = float.Parse(array[12]);
					fieldValue[(int)morph.hash["mouthup"]] = float.Parse(array[13]);
					fieldValue[(int)morph.hash["tangout"]] = float.Parse(array[14]);
					fieldValue[(int)morph.hash["tangup"]] = float.Parse(array[15]);
					if (morph.bodyskin.PartsVersion < 120)
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]);
					}
					else
					{
						fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array[16]) * 3f;
					}
					fieldValue[(int)morph.hash["mayuw"]] = float.Parse(array[18]);
					fieldValue[(int)morph.hash["mouthhe"]] = float.Parse(array[19]);
					fieldValue[(int)morph.hash["mouthc"]] = float.Parse(array[20]);
					fieldValue[(int)morph.hash["mouthi"]] = float.Parse(array[21]);
					fieldValue[(int)morph.hash["mouthuphalf"]] = float.Parse(array[22]) + 0.01f;
					try
					{
						fieldValue[(int)morph.hash["tangopen"]] = float.Parse(array[23]);
					}
					catch
					{
					}
					if (float.Parse(array[24]) == 1f)
					{
						fieldValue[(int)morph.hash["namida"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["namida"]] = 0f;
					}
					if (float.Parse(array[25]) == 1f)
					{
						fieldValue[(int)morph.hash["tear1"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear1"]] = 0f;
					}
					if (float.Parse(array[26]) == 1f)
					{
						fieldValue[(int)morph.hash["tear2"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear2"]] = 0f;
					}
					if (float.Parse(array[27]) == 1f)
					{
						fieldValue[(int)morph.hash["tear3"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["tear3"]] = 0f;
					}
					if (float.Parse(array[28]) == 1f)
					{
						fieldValue[(int)morph.hash["shock"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["shock"]] = 0f;
					}
					if (float.Parse(array[29]) == 1f)
					{
						fieldValue[(int)morph.hash["yodare"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["yodare"]] = 0f;
					}
					if (float.Parse(array[30]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho"]] = 0f;
					}
					if (float.Parse(array[31]) == 1f)
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0.5f;
					}
					else
					{
						fieldValue[(int)morph.hash["hoho2"]] = 0f;
					}
					if (float.Parse(array[32]) == 1f)
					{
						fieldValue[(int)morph.hash["hohos"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohos"]] = 0f;
					}
					if (float.Parse(array[33]) == 1f)
					{
						fieldValue[(int)morph.hash["hohol"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["hohol"]] = 0f;
					}
					if (float.Parse(array[34]) == 1f)
					{
						fieldValue[(int)morph.hash["toothoff"]] = 1f;
					}
					else
					{
						fieldValue[(int)morph.hash["toothoff"]] = 0f;
					}
					if (array.Length > 35)
					{
						if (float.Parse(array[35]) == 1f)
						{
							morph.boNoseFook = true;
						}
						else
						{
							morph.boNoseFook = false;
						}
					}
				}
				this.maidArray[this.selectMaidIndex].body0.Face.morph.FixBlendValues_Face();
				this.isFaceInit = true;
				this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
			}
			if (this.faceCombo.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			else
			{
				GUI.enabled = true;
			}
			this.yotogiFlg = false;
			if (this.sceneLevel == 14)
			{
				if (GameObject.Find("/UI Root/YotogiPlayPanel/CommandViewer/SkillViewer/MaskGroup/SkillGroup/CommandParent/CommandUnit"))
				{
					this.yotogiFlg = true;
				}
			}
		}

		private void GuiFunc(int winID)
		{
			CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
			GUIStyle guistyle = "label";
			guistyle.fontSize = this.GetPix(14);
			guistyle.alignment = TextAnchor.UpperLeft;
			GUIStyle guistyle2 = "button";
			guistyle2.fontSize = this.GetPix(16);
			guistyle2.alignment = TextAnchor.MiddleCenter;
			GUIStyle guistyle3 = new GUIStyle("toggle");
			guistyle3.fontSize = this.GetPix(13);
			float num = (float)this.GetPix(70);
			if (this.comboBoxList == null)
			{
				this.comboBoxControl.selectedItemIndex = 0;
				this.comboBoxList = new GUIContent[11];
				this.comboBoxList[0] = new GUIContent("通常");
				this.comboBoxList[1] = new GUIContent("横一列");
				this.comboBoxList[2] = new GUIContent("縦一列");
				this.comboBoxList[3] = new GUIContent("斜め");
				this.comboBoxList[4] = new GUIContent("円（外向き）");
				this.comboBoxList[5] = new GUIContent("円（内向き）");
				this.comboBoxList[6] = new GUIContent("扇");
				this.comboBoxList[7] = new GUIContent("Ｖ");
				this.comboBoxList[8] = new GUIContent("^");
				this.comboBoxList[9] = new GUIContent("Ｍ");
				this.comboBoxList[10] = new GUIContent("Ｗ");
			}
			bool value = true;
			if (this.faceFlg || this.poseFlg || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
			{
				value = false;
			}
			if (!this.isF6)
			{
				if (GUI.Toggle(new Rect((float)this.GetPix(2), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), value, "配置", guistyle3))
				{
					this.faceFlg = false;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					this.bGui = true;
					this.isGuiInit = true;
				}
			}
			if (!this.yotogiFlg)
			{
				if (GUI.Toggle(new Rect((float)this.GetPix(41), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.poseFlg, "操作", guistyle3))
				{
					this.poseFlg = true;
					this.faceFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(80), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.faceFlg, "表情", guistyle3))
			{
				this.faceFlg = true;
				this.poseFlg = false;
				this.sceneFlg = false;
				this.kankyoFlg = false;
				this.kankyo2Flg = false;
				if (!this.faceFlg2)
				{
					this.isFaceInit = true;
					this.faceFlg2 = true;
					this.maidArray[this.selectMaidIndex].boMabataki = false;
					this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
				}
			}
			if (GUI.Toggle(new Rect((float)this.GetPix(119), (float)this.GetPix(2), (float)this.GetPix(39), (float)this.GetPix(20)), this.kankyoFlg, "環境", guistyle3))
			{
				this.poseFlg = false;
				this.faceFlg = false;
				this.sceneFlg = false;
				this.kankyoFlg = true;
				this.kankyo2Flg = false;
			}
			if (!this.line1)
			{
				this.line1 = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.3f));
				this.line2 = this.MakeTex(2, 2, new Color(0.7f, 0.7f, 0.7f, 0.6f));
			}
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 2f), this.line1);
			GUI.DrawTexture(new Rect((float)this.GetPix(5), (float)this.GetPix(20), (float)this.GetPix(160), 1f), this.line2);
			int stockMaidCount = characterMgr.GetStockMaidCount();
			Rect position;
			Rect viewRect;
			if (this.sceneLevel != 5)
			{
				position = new Rect((float)this.GetPix(7), (float)this.GetPix(110), this.rectWin.width - (float)this.GetPix(14), this.rectWin.height * 0.83f);
				viewRect = new Rect(0f, 0f, position.width * 0.85f, (num + (float)this.GetPix(5)) * (float)stockMaidCount + (float)this.GetPix(15));
			}
			else
			{
				position = new Rect((float)this.GetPix(7), (float)this.GetPix(110), this.rectWin.width - (float)this.GetPix(14), this.rectWin.height * 0.83f * 0.98f);
				viewRect = new Rect(0f, 0f, position.width * 0.85f, (num + (float)this.GetPix(5)) * (float)stockMaidCount + (float)this.GetPix(15) * 0.92f);
			}
			float num2 = 0f;
			if (this.comboBoxControl.isClickedComboButton)
			{
				GUI.enabled = false;
			}
			if (GUI.Button(new Rect((float)this.GetPix(10), (float)this.GetPix(78), this.rectWin.width * 0.85f, (float)this.GetPix(28)), "呼び出す", guistyle2))
			{
				this.isYobidashi = true;
				this.selectMaidIndex = 0;
				this.copyIndex = 0;
				for (int i = 0; i < this.maxMaidCnt; i++)
				{
					if (!this.isLock[i])
					{
						if (this.maidArray[i] && this.maidArray[i].Visible)
						{
							this.maidArray[i].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							this.maidArray[i].SetAutoTwistAll(true);
						}
					}
					this.maidArray[i] = null;
				}
				for (int i = 0; i < this.maxMaidCnt; i++)
				{
					this.isStop[i] = false;
				}
				this.bGui = false;
				this.isFadeOut = true;
				GameMain.Instance.MainCamera.FadeOut(0f, false, null, true, default(Color));
				for (int j = 0; j < characterMgr.GetStockMaidCount(); j++)
				{
					characterMgr.GetStockMaidList()[j].Visible = false;
				}
			}
			GUIStyle guistyle4 = "button";
			guistyle4.fontSize = this.GetPix(13);
			GUIStyleState guistyleState = new GUIStyleState();
			if (GUI.Button(new Rect((float)this.GetPix(10), (float)this.GetPix(52), this.rectWin.width * 0.4f, (float)this.GetPix(23)), "7人選択", guistyle4))
			{
				if (this.sceneLevel != 5)
				{
					this.selectList = new ArrayList();
					this.selectList.Add(0);
					this.selectList.Add(1);
					this.selectList.Add(2);
					this.selectList.Add(3);
					this.selectList.Add(4);
					this.selectList.Add(5);
					this.selectList.Add(6);
				}
				else
				{
					int stockMaidCount2 = characterMgr.GetStockMaidCount();
					this.selectList = new ArrayList();
					this.selectList.Add(this.editMaid);
					if (stockMaidCount2 > 1)
					{
						if (this.editMaid >= 1)
						{
							this.selectList.Add(0);
						}
						else if (stockMaidCount2 > 2)
						{
							this.selectList.Add(1);
						}
					}
					if (stockMaidCount2 > 2)
					{
						if (this.editMaid >= 2)
						{
							this.selectList.Add(1);
						}
						else if (stockMaidCount2 > 3)
						{
							this.selectList.Add(2);
						}
					}
					if (stockMaidCount2 > 3)
					{
						if (this.editMaid >= 3)
						{
							this.selectList.Add(2);
						}
						else if (stockMaidCount2 > 4)
						{
							this.selectList.Add(3);
						}
					}
					if (stockMaidCount2 > 4)
					{
						if (this.editMaid >= 4)
						{
							this.selectList.Add(3);
						}
						else if (stockMaidCount2 > 5)
						{
							this.selectList.Add(4);
						}
					}
					if (stockMaidCount2 > 5)
					{
						if (this.editMaid >= 5)
						{
							this.selectList.Add(4);
						}
						else if (stockMaidCount2 > 6)
						{
							this.selectList.Add(5);
						}
					}
					if (stockMaidCount2 > 6)
					{
						if (this.editMaid >= 6)
						{
							this.selectList.Add(5);
						}
						else if (stockMaidCount2 > 7)
						{
							this.selectList.Add(6);
						}
					}
				}
			}
			if (GUI.Button(new Rect(this.rectWin.width * 0.5f, (float)this.GetPix(52), this.rectWin.width * 0.4f, (float)this.GetPix(23)), "選択解除", guistyle4))
			{
				this.selectList = new ArrayList();
				if (this.sceneLevel == 5)
				{
					this.selectList.Add(this.editMaid);
				}
			}
			GUI.enabled = true;
			this.scrollPos = GUI.BeginScrollView(position, this.scrollPos, viewRect);
			for (int k = 0; k < characterMgr.GetStockMaidCount(); k++)
			{
				GUI.enabled = true;
				GUI.DrawTexture(new Rect(2f, num2 + 2f, this.rectWin.width * 0.83f - 4f, num - 4f), Texture2D.whiteTexture);
				bool flag = false;
				for (int j = 0; j < this.selectList.Count; j++)
				{
					if ((int)this.selectList[j] == k)
					{
						flag = true;
						break;
					}
				}
				if (this.comboBoxControl.isClickedComboButton)
				{
					GUI.enabled = false;
					GUI.Button(new Rect(0f, num2, this.rectWin.width * 0.83f, num), "", guistyle2);
					GUI.Button(new Rect(0f, num2, this.rectWin.width * 0.83f, num), "", guistyle2);
				}
				if (GUI.Button(new Rect(0f, num2, this.rectWin.width * 0.83f, num), "", guistyle2))
				{
					if (flag)
					{
						for (int j = 0; j < this.selectList.Count; j++)
						{
							if ((int)this.selectList[j] == k)
							{
								if (this.sceneLevel != 5 || (int)this.selectList[j] != this.editMaid)
								{
									this.selectList.Remove(k);
									break;
								}
							}
						}
					}
					else
					{
						if (this.selectList.Count > this.maxMaidCnt - 1)
						{
							this.selectList.Remove(this.selectList[this.maxMaidCnt - 1]);
						}
						this.selectList.Add(k);
					}
				}
				GUI.enabled = true;
				if (flag)
				{
					GUI.DrawTexture(new Rect(5f, num2 + 5f, this.rectWin.width * 0.83f - 10f, num - 10f), Texture2D.whiteTexture);
				}
				if (characterMgr.GetStockMaid(k).GetThumIcon())
				{
					GUI.DrawTexture(new Rect(0f, num2 - 5f, num, num), characterMgr.GetStockMaid(k).GetThumIcon());
				}
				string text = characterMgr.GetStockMaid(k).status.lastName + "\n" + characterMgr.GetStockMaid(k).status.firstName;
				guistyleState.textColor = Color.black;
				guistyle.normal = guistyleState;
				GUI.Label(new Rect((float)this.GetPix(65), num2 + num / 4f, num * 2f, num * 3f), text, guistyle);
				if (flag)
				{
					for (int j = 0; j < this.selectList.Count; j++)
					{
						if ((int)this.selectList[j] == k)
						{
							GUI.Label(new Rect(this.rectWin.width * 0.7f, num2 + 6f, num, num), (j + 1).ToString(), guistyle);
							break;
						}
					}
				}
				num2 += num + (float)this.GetPix(5);
			}
			GUI.EndScrollView();
			guistyleState.textColor = Color.white;
			guistyle.normal = guistyleState;
			int num3 = this.comboBoxControl.GetSelectedItemIndex();
			num3 = this.comboBoxControl.List(new Rect((float)this.GetPix(10), (float)this.GetPix(25), this.rectWin.width * 0.56f, (float)this.GetPix(24)), this.comboBoxList[num3].text, this.comboBoxList, this.listStyle);
			if (GUI.Button(new Rect(this.rectWin.width * 0.66f, (float)this.GetPix(25), this.rectWin.width * 0.24f, (float)this.GetPix(24)), "決定", guistyle4))
			{
				for (int i = 0; i < this.maxMaidCnt; i++)
				{
					this.isStop[i] = false;
				}
				switch (this.comboBoxControl.GetSelectedItemIndex())
				{
					case 0:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							if (this.selectList.Count <= 7)
							{
								if (this.selectList.Count % 2 == 1)
								{
									switch (i)
									{
										case 0:
											this.maidArray[i].SetPos(new Vector3(0f, 0f, 0f));
											break;
										case 1:
											this.maidArray[i].SetPos(new Vector3(-0.6f, 0f, 0.26f));
											break;
										case 2:
											this.maidArray[i].SetPos(new Vector3(0.6f, 0f, 0.26f));
											break;
										case 3:
											this.maidArray[i].SetPos(new Vector3(-1.1f, 0f, 0.69f));
											break;
										case 4:
											this.maidArray[i].SetPos(new Vector3(1.1f, 0f, 0.69f));
											break;
										case 5:
											this.maidArray[i].SetPos(new Vector3(-1.47f, 0f, 1.1f));
											break;
										case 6:
											this.maidArray[i].SetPos(new Vector3(1.47f, 0f, 1.1f));
											break;
									}
								}
								else
								{
									switch (i)
									{
										case 0:
											this.maidArray[i].SetPos(new Vector3(0.3f, 0f, 0f));
											break;
										case 1:
											this.maidArray[i].SetPos(new Vector3(-0.3f, 0f, 0f));
											break;
										case 2:
											this.maidArray[i].SetPos(new Vector3(0.7f, 0f, 0.4f));
											break;
										case 3:
											this.maidArray[i].SetPos(new Vector3(-0.7f, 0f, 0.4f));
											break;
										case 4:
											this.maidArray[i].SetPos(new Vector3(1f, 0f, 0.9f));
											break;
										case 5:
											this.maidArray[i].SetPos(new Vector3(-1f, 0f, 0.9f));
											break;
									}
								}
							}
							else
							{
								float num4 = 0f;
								if (this.selectList.Count >= 11)
								{
									num4 = -0.4f;
									if (this.selectList.Count % 2 == 1)
									{
										switch (i)
										{
											case 0:
												this.maidArray[i].SetPos(new Vector3(0f, 0f, 0f + num4));
												break;
											case 1:
												this.maidArray[i].SetPos(new Vector3(-0.5f, 0f, 0.2f + num4));
												break;
											case 2:
												this.maidArray[i].SetPos(new Vector3(0.5f, 0f, 0.2f + num4));
												break;
											case 3:
												this.maidArray[i].SetPos(new Vector3(-0.9f, 0f, 0.55f + num4));
												break;
											case 4:
												this.maidArray[i].SetPos(new Vector3(0.9f, 0f, 0.55f + num4));
												break;
											case 5:
												this.maidArray[i].SetPos(new Vector3(-1.25f, 0f, 0.9f + num4));
												break;
											case 6:
												this.maidArray[i].SetPos(new Vector3(1.25f, 0f, 0.9f + num4));
												break;
											case 7:
												this.maidArray[i].SetPos(new Vector3(-1.57f, 0f, 1.3f + num4));
												break;
											case 8:
												this.maidArray[i].SetPos(new Vector3(1.57f, 0f, 1.3f + num4));
												break;
											case 9:
												this.maidArray[i].SetPos(new Vector3(-1.77f, 0f, 1.72f + num4));
												break;
											case 10:
												this.maidArray[i].SetPos(new Vector3(1.77f, 0f, 1.72f + num4));
												break;
											case 11:
												this.maidArray[i].SetPos(new Vector3(-1.85f, 0f, 2.17f + num4));
												break;
											case 12:
												this.maidArray[i].SetPos(new Vector3(1.85f, 0f, 2.17f + num4));
												break;
											default:
												this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f + num4));
												break;
										}
									}
									else
									{
										switch (i)
										{
											case 0:
												this.maidArray[i].SetPos(new Vector3(0.25f, 0f, 0f + num4));
												break;
											case 1:
												this.maidArray[i].SetPos(new Vector3(-0.25f, 0f, 0f + num4));
												break;
											case 2:
												this.maidArray[i].SetPos(new Vector3(0.7f, 0f, 0.25f + num4));
												break;
											case 3:
												this.maidArray[i].SetPos(new Vector3(-0.7f, 0f, 0.25f + num4));
												break;
											case 4:
												this.maidArray[i].SetPos(new Vector3(1.05f, 0f, 0.6f + num4));
												break;
											case 5:
												this.maidArray[i].SetPos(new Vector3(-1.05f, 0f, 0.6f + num4));
												break;
											case 6:
												this.maidArray[i].SetPos(new Vector3(1.35f, 0f, 0.9f + num4));
												break;
											case 7:
												this.maidArray[i].SetPos(new Vector3(-1.35f, 0f, 0.9f + num4));
												break;
											case 8:
												this.maidArray[i].SetPos(new Vector3(1.6f, 0f, 1.3f + num4));
												break;
											case 9:
												this.maidArray[i].SetPos(new Vector3(-1.6f, 0f, 1.3f + num4));
												break;
											case 10:
												this.maidArray[i].SetPos(new Vector3(1.8f, 0f, 1.72f + num4));
												break;
											case 11:
												this.maidArray[i].SetPos(new Vector3(-1.8f, 0f, 1.72f + num4));
												break;
											case 12:
												this.maidArray[i].SetPos(new Vector3(1.9f, 0f, 2.17f + num4));
												break;
											case 13:
												this.maidArray[i].SetPos(new Vector3(-1.9f, 0f, 2.17f + num4));
												break;
											default:
												this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f + num4));
												break;
										}
									}
								}
								else if (this.selectList.Count >= 8)
								{
									if (this.selectList.Count >= 9)
									{
										num4 = -0.2f;
									}
									if (this.selectList.Count % 2 == 1)
									{
										switch (i)
										{
											case 0:
												this.maidArray[i].SetPos(new Vector3(0f, 0f, 0f + num4));
												break;
											case 1:
												this.maidArray[i].SetPos(new Vector3(-0.55f, 0f, 0.2f + num4));
												break;
											case 2:
												this.maidArray[i].SetPos(new Vector3(0.55f, 0f, 0.2f + num4));
												break;
											case 3:
												this.maidArray[i].SetPos(new Vector3(-1f, 0f, 0.6f + num4));
												break;
											case 4:
												this.maidArray[i].SetPos(new Vector3(1f, 0f, 0.6f + num4));
												break;
											case 5:
												this.maidArray[i].SetPos(new Vector3(-1.35f, 0f, 1f + num4));
												break;
											case 6:
												this.maidArray[i].SetPos(new Vector3(1.35f, 0f, 1f + num4));
												break;
											case 7:
												this.maidArray[i].SetPos(new Vector3(-1.6f, 0f, 1.4f + num4));
												break;
											case 8:
												this.maidArray[i].SetPos(new Vector3(1.6f, 0f, 1.4f + num4));
												break;
										}
									}
									else
									{
										switch (i)
										{
											case 0:
												this.maidArray[i].SetPos(new Vector3(0.28f, 0f, 0f + num4));
												break;
											case 1:
												this.maidArray[i].SetPos(new Vector3(-0.28f, 0f, 0f + num4));
												break;
											case 2:
												this.maidArray[i].SetPos(new Vector3(0.78f, 0f, 0.3f + num4));
												break;
											case 3:
												this.maidArray[i].SetPos(new Vector3(-0.78f, 0f, 0.3f + num4));
												break;
											case 4:
												this.maidArray[i].SetPos(new Vector3(1.22f, 0f, 0.7f + num4));
												break;
											case 5:
												this.maidArray[i].SetPos(new Vector3(-1.22f, 0f, 0.7f + num4));
												break;
											case 6:
												this.maidArray[i].SetPos(new Vector3(1.55f, 0f, 1.1f + num4));
												break;
											case 7:
												this.maidArray[i].SetPos(new Vector3(-1.55f, 0f, 1.1f + num4));
												break;
											case 8:
												this.maidArray[i].SetPos(new Vector3(1.77f, 0f, 1.58f + num4));
												break;
											case 9:
												this.maidArray[i].SetPos(new Vector3(-1.77f, 0f, 1.58f + num4));
												break;
										}
									}
								}
							}
							zero2.y = (float)(Math.Atan2((double)this.maidArray[i].transform.position.x, (double)(this.maidArray[i].transform.position.z - 1.5f)) * 180.0 / 3.1415926535897931) + 180f;
							this.maidArray[i].SetRot(zero2);
						}
						break;
					case 1:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							if (this.maidCnt < 9)
							{
								switch (i)
								{
									case 1:
										zero.x = -0.6f;
										break;
									case 2:
										zero.x = 0.6f;
										break;
									case 3:
										zero.x = -1.2f;
										break;
									case 4:
										zero.x = 1.2f;
										break;
									case 5:
										zero.x = -1.8f;
										break;
									case 6:
										zero.x = 1.8f;
										break;
									case 7:
										zero.x = -2.4f;
										break;
									case 8:
										zero.x = 2.4f;
										break;
									case 9:
										zero.x = -3f;
										break;
									case 10:
										zero.x = 3f;
										break;
								}
							}
							else
							{
								switch (i)
								{
									case 1:
										zero.x = -0.5f;
										break;
									case 2:
										zero.x = 0.5f;
										break;
									case 3:
										zero.x = -1f;
										break;
									case 4:
										zero.x = 1f;
										break;
									case 5:
										zero.x = -1.5f;
										break;
									case 6:
										zero.x = 1.5f;
										break;
									case 7:
										zero.x = -2f;
										break;
									case 8:
										zero.x = 2f;
										break;
									case 9:
										zero.x = -2.5f;
										break;
									case 10:
										zero.x = 2.5f;
										break;
									case 11:
										zero.x = -3f;
										break;
									case 12:
										zero.x = 3f;
										break;
									case 13:
										zero.x = -3.5f;
										break;
								}
							}
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 2:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							if (this.maidCnt < 9)
							{
								switch (i)
								{
									case 1:
										zero.z = 0.6f;
										break;
									case 2:
										zero.z = -0.6f;
										break;
									case 3:
										zero.z = 1.2f;
										break;
									case 4:
										zero.z = -1.2f;
										break;
									case 5:
										zero.z = 1.8f;
										break;
									case 6:
										zero.z = -1.8f;
										break;
									case 7:
										zero.z = 2.4f;
										break;
									case 8:
										zero.z = -2.4f;
										break;
									case 9:
										zero.z = 3f;
										break;
									case 10:
										zero.z = -3f;
										break;
								}
							}
							else
							{
								switch (i)
								{
									case 1:
										zero.z = 0.5f;
										break;
									case 2:
										zero.z = -0.5f;
										break;
									case 3:
										zero.z = 1f;
										break;
									case 4:
										zero.z = -1f;
										break;
									case 5:
										zero.z = 1.5f;
										break;
									case 6:
										zero.z = -1.5f;
										break;
									case 7:
										zero.z = 2f;
										break;
									case 8:
										zero.z = -2f;
										break;
									case 9:
										zero.z = 2.5f;
										break;
									case 10:
										zero.z = -2.5f;
										break;
									case 11:
										zero.z = 3f;
										break;
									case 12:
										zero.z = -3f;
										break;
									case 13:
										zero.z = 3.5f;
										break;
								}
							}
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 3:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num5 = 0.5f;
							if (this.maidCnt > 11)
							{
								num5 = 0.25f;
							}
							else if (this.maidCnt > 9)
							{
								num5 = 0.32f;
							}
							else if (this.maidCnt > 7)
							{
								num5 = 0.4f;
							}
							switch (i)
							{
								case 1:
									zero.x = -num5;
									zero.z = num5;
									break;
								case 2:
									zero.x = num5;
									zero.z = -num5;
									break;
								case 3:
									zero.x = -num5 * 2f;
									zero.z = num5 * 2f;
									break;
								case 4:
									zero.x = num5 * 2f;
									zero.z = -num5 * 2f;
									break;
								case 5:
									zero.x = -num5 * 3f;
									zero.z = num5 * 3f;
									break;
								case 6:
									zero.x = num5 * 3f;
									zero.z = -num5 * 3f;
									break;
								case 7:
									zero.x = -num5 * 4f;
									zero.z = num5 * 4f;
									break;
								case 8:
									zero.x = num5 * 4f;
									zero.z = -num5 * 4f;
									break;
								case 9:
									zero.x = -num5 * 5f;
									zero.z = num5 * 5f;
									break;
								case 10:
									zero.x = num5 * 5f;
									zero.z = -num5 * 5f;
									break;
								case 11:
									zero.x = -num5 * 6f;
									zero.z = num5 * 6f;
									break;
								case 12:
									zero.x = num5 * 6f;
									zero.z = -num5 * 6f;
									break;
								case 13:
									zero.x = -num5 * 7f;
									zero.z = num5 * 7f;
									break;
							}
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 4:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num6 = 0.4f + 0.08f * (float)this.maidCnt;
							zero.x = (float)((double)num6 * Math.Cos(0.017453292519943295 * (double)(90 + 360 * i / this.maidCnt)));
							zero.z = (float)((double)num6 * Math.Sin(0.017453292519943295 * (double)(90 + 360 * i / this.maidCnt)));
							this.maidArray[i].SetPos(zero);
							zero2.y = (float)(Math.Atan2((double)zero.x, (double)zero.z) * 180.0 / 3.1415926535897931);
							this.maidArray[i].SetRot(zero2);
						}
						break;
					case 5:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num6 = 0.4f + 0.08f * (float)this.maidCnt;
							zero.x = (float)((double)num6 * Math.Cos(0.017453292519943295 * (double)(90 + 360 * i / this.maidCnt)));
							zero.z = (float)((double)num6 * Math.Sin(0.017453292519943295 * (double)(90 + 360 * i / this.maidCnt)));
							this.maidArray[i].SetPos(zero);
							zero2.y = (float)(Math.Atan2((double)zero.x, (double)zero.z) * 180.0 / 3.1415926535897931) + 180f;
							this.maidArray[i].SetRot(zero2);
						}
						break;
					case 6:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num7 = 0f;
							if (this.maidCnt > 9)
							{
								num7 = -0.4f;
							}
							else if (this.maidCnt > 7)
							{
								num7 = -0.2f;
							}
							switch (i)
							{
								case 0:
									zero.z = 0.3f;
									break;
								case 1:
									zero.x = -0.9f;
									zero.z = -0.4f;
									zero2.y = 40f;
									break;
								case 2:
									zero.x = 0.9f;
									zero.z = -0.4f;
									zero2.y = -40f;
									break;
								case 3:
									zero.x = -0.4f;
									zero.z = -0.8f;
									zero2.y = 20f;
									break;
								case 4:
									zero.x = 0.4f;
									zero.z = -0.8f;
									zero2.y = -20f;
									break;
								case 5:
									zero.x = -1.2f;
									zero.z = 0.1f;
									zero2.y = 60f;
									break;
								case 6:
									zero.x = 1.2f;
									zero.z = 0.1f;
									zero2.y = -60f;
									break;
								case 7:
									zero.x = -1.5f;
									zero.z = 0.6f;
									zero2.y = 80f;
									break;
								case 8:
									zero.x = 1.5f;
									zero.z = 0.6f;
									zero2.y = -80f;
									break;
								case 9:
									zero.x = -1.6f;
									zero.z = 1.15f;
									zero2.y = 100f;
									break;
								case 10:
									zero.x = 1.6f;
									zero.z = 1.15f;
									zero2.y = -100f;
									break;
								case 11:
									zero.x = -1.6f;
									zero.z = 1.65f;
									zero2.y = 110f;
									break;
								case 12:
									zero.x = 1.65f;
									zero.z = 1.65f;
									zero2.y = -110f;
									break;
								case 13:
									zero.x = -1.65f;
									zero.z = 2.15f;
									zero2.y = 120f;
									break;
							}
							if (i > 0)
							{
								zero.z += num7;
							}
							else
							{
								zero.z -= num7;
							}
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 7:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num7 = 0f;
							if (this.maidCnt > 11)
							{
								num7 = 0.6f;
							}
							else if (this.maidCnt > 9)
							{
								num7 = 0.4f;
							}
							else if (this.maidCnt > 7)
							{
								num7 = 0.2f;
							}
							switch (i)
							{
								case 0:
									zero.z = 0.8f;
									break;
								case 1:
									zero.x = -0.45f;
									zero.z = 0.3f;
									zero2.y = -10f;
									break;
								case 2:
									zero.x = 0.45f;
									zero.z = 0.3f;
									zero2.y = 10f;
									break;
								case 3:
									zero.x = -0.8f;
									zero.z = -0.2f;
									zero2.y = -20f;
									break;
								case 4:
									zero.x = 0.8f;
									zero.z = -0.2f;
									zero2.y = 20f;
									break;
								case 5:
									zero.x = -1.2f;
									zero.z = -0.75f;
									zero2.y = -30f;
									break;
								case 6:
									zero.x = 1.2f;
									zero.z = -0.75f;
									zero2.y = 30f;
									break;
								case 7:
									zero.x = -1.6f;
									zero.z = -1.25f;
									zero2.y = -40f;
									break;
								case 8:
									zero.x = 1.6f;
									zero.z = -1.25f;
									zero2.y = 40f;
									break;
								case 9:
									zero.x = -2f;
									zero.z = -1.75f;
									zero2.y = -50f;
									break;
								case 10:
									zero.x = 2f;
									zero.z = -1.75f;
									zero2.y = 50f;
									break;
								case 11:
									zero.x = -2.4f;
									zero.z = -2.25f;
									zero2.y = -60f;
									break;
								case 12:
									zero.x = 2.4f;
									zero.z = -2.25f;
									zero2.y = 60f;
									break;
								case 13:
									zero.x = -2.8f;
									zero.z = -2.75f;
									zero2.y = -70f;
									break;
							}
							zero.z += num7;
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 8:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num7 = 0f;
							if (this.maidCnt > 11)
							{
								num7 = -0.6f;
							}
							else if (this.maidCnt > 9)
							{
								num7 = -0.4f;
							}
							else if (this.maidCnt > 7)
							{
								num7 = -0.2f;
							}
							switch (i)
							{
								case 0:
									zero.z = -0.75f;
									break;
								case 1:
									zero.x = -0.45f;
									zero.z = -0.2f;
									zero2.y = 20f;
									break;
								case 2:
									zero.x = 0.45f;
									zero.z = -0.2f;
									zero2.y = -20f;
									break;
								case 3:
									zero.x = -0.8f;
									zero.z = 0.3f;
									zero2.y = 35f;
									break;
								case 4:
									zero.x = 0.8f;
									zero.z = 0.3f;
									zero2.y = -35f;
									break;
								case 5:
									zero.x = -1.2f;
									zero.z = 0.8f;
									zero2.y = 50f;
									break;
								case 6:
									zero.x = 1.2f;
									zero.z = 0.8f;
									zero2.y = -50f;
									break;
								case 7:
									zero.x = -1.6f;
									zero.z = 1.3f;
									zero2.y = 65f;
									break;
								case 8:
									zero.x = 1.6f;
									zero.z = 1.3f;
									zero2.y = -65f;
									break;
								case 9:
									zero.x = -2f;
									zero.z = 1.8f;
									zero2.y = 80f;
									break;
								case 10:
									zero.x = 2f;
									zero.z = 1.8f;
									zero2.y = -80f;
									break;
								case 11:
									zero.x = -2.4f;
									zero.z = 2.3f;
									zero2.y = 90f;
									break;
								case 12:
									zero.x = 2.4f;
									zero.z = 2.3f;
									zero2.y = -90f;
									break;
								case 13:
									zero.x = -2.8f;
									zero.z = 2.8f;
									zero2.y = 100f;
									break;
							}
							zero.z += num7;
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 9:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num5 = 1f;
							if (this.maidCnt > 9)
							{
								num5 = 0.84f;
							}
							else if (this.maidCnt > 7)
							{
								num5 = 0.92f;
							}
							switch (i)
							{
								case 0:
									zero.z = 0f;
									break;
								case 1:
									zero.x = -0.5f;
									zero.z = -0.5f;
									break;
								case 2:
									zero.x = 0.5f;
									zero.z = -0.5f;
									break;
								case 3:
									zero.x = -1f;
									zero.z = 0.2f;
									break;
								case 4:
									zero.x = 1f;
									zero.z = 0.2f;
									break;
								case 5:
									zero.x = -1.5f;
									zero.z = -0.5f;
									break;
								case 6:
									zero.x = 1.5f;
									zero.z = -0.5f;
									break;
								case 7:
									zero.x = -2f;
									zero.z = 0.2f;
									break;
								case 8:
									zero.x = 2f;
									zero.z = 0.2f;
									break;
								case 9:
									zero.x = -2.5f;
									zero.z = -0.5f;
									break;
								case 10:
									zero.x = 2.5f;
									zero.z = -0.5f;
									break;
								case 11:
									zero.x = -3f;
									zero.z = 0.2f;
									break;
								case 12:
									zero.x = 3f;
									zero.z = 0.2f;
									break;
								case 13:
									zero.x = -3.5f;
									zero.z = -0.5f;
									break;
							}
							zero.x *= num5;
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
					case 10:
						for (int i = 0; i < this.maidCnt; i++)
						{
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							float num5 = 1f;
							if (this.maidCnt > 9)
							{
								num5 = 0.84f;
							}
							else if (this.maidCnt > 7)
							{
								num5 = 0.92f;
							}
							switch (i)
							{
								case 0:
									zero.z = -0.4f;
									break;
								case 1:
									zero.x = -0.5f;
									zero.z = 0.2f;
									break;
								case 2:
									zero.x = 0.5f;
									zero.z = 0.2f;
									break;
								case 3:
									zero.x = -1f;
									zero.z = -0.5f;
									break;
								case 4:
									zero.x = 1f;
									zero.z = -0.5f;
									break;
								case 5:
									zero.x = -1.5f;
									zero.z = 0.2f;
									break;
								case 6:
									zero.x = 1.5f;
									zero.z = 0.2f;
									break;
								case 7:
									zero.x = -2f;
									zero.z = -0.5f;
									break;
								case 8:
									zero.x = 2f;
									zero.z = -0.5f;
									break;
								case 9:
									zero.x = -2.5f;
									zero.z = 0.2f;
									break;
								case 10:
									zero.x = 2.5f;
									zero.z = 0.2f;
									break;
								case 11:
									zero.x = -3f;
									zero.z = -0.5f;
									break;
								case 12:
									zero.x = 3f;
									zero.z = -0.5f;
									break;
								case 13:
									zero.x = -3.5f;
									zero.z = 0.2f;
									break;
							}
							zero.x *= num5;
							this.maidArray[i].SetPos(zero);
							this.maidArray[i].SetRot(zero2);
							if (i >= 14)
							{
								this.maidArray[i].SetPos(new Vector3(0f, 0f, 0.7f));
							}
						}
						break;
				}
				for (int i = 0; i < this.maxMaidCnt; i++)
				{
					if (!this.isLock[i])
					{
						if (this.maidArray[i] != null && this.maidArray[i].Visible)
						{
							this.maidArray[i].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							this.maidArray[i].SetAutoTwistAll(true);
						}
					}
				}
			}
		}
	}
}
