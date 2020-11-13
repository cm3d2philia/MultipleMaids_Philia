using ExIni;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CM3D2.MultipleMaids.Plugin
{
    public partial class MultipleMaids
	{
		private void MaidUpdate()
		{
			if (this.isHaiti)
			{
				for (int i = 0; i < this.maxMaidCnt; i++)
				{
					if (this.gHandL[i])
					{
						this.HandL1[i] = null;
						Object.Destroy(this.gHandL[i]);
						Object.Destroy(this.gArmL[i]);
						Object.Destroy(this.gFootL[i]);
						Object.Destroy(this.gHizaL[i]);
						Object.Destroy(this.gHandR[i]);
						Object.Destroy(this.gArmR[i]);
						Object.Destroy(this.gFootR[i]);
						Object.Destroy(this.gHizaR[i]);
						Object.Destroy(this.gClavicleL[i]);
						Object.Destroy(this.gClavicleR[i]);
						Object.Destroy(this.gNeck[i]);
						Object.Destroy(this.gSpine[i]);
						Object.Destroy(this.gSpine0a[i]);
						Object.Destroy(this.gSpine1a[i]);
						Object.Destroy(this.gSpine1[i]);
						Object.Destroy(this.gPelvis[i]);
					}
				}
				for (int j = 0; j < this.maidCnt; j++)
				{
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					if (this.selectList.Count <= 7)
					{
						if (this.selectList.Count % 2 == 1)
						{
							switch (j)
							{
								case 0:
									this.maidArray[j].SetPos(new Vector3(0f, 0f, 0f));
									break;
								case 1:
									this.maidArray[j].SetPos(new Vector3(-0.6f, 0f, 0.26f));
									break;
								case 2:
									this.maidArray[j].SetPos(new Vector3(0.6f, 0f, 0.26f));
									break;
								case 3:
									this.maidArray[j].SetPos(new Vector3(-1.1f, 0f, 0.69f));
									break;
								case 4:
									this.maidArray[j].SetPos(new Vector3(1.1f, 0f, 0.69f));
									break;
								case 5:
									this.maidArray[j].SetPos(new Vector3(-1.47f, 0f, 1.1f));
									break;
								case 6:
									this.maidArray[j].SetPos(new Vector3(1.47f, 0f, 1.1f));
									break;
							}
						}
						else
						{
							switch (j)
							{
								case 0:
									this.maidArray[j].SetPos(new Vector3(0.3f, 0f, 0f));
									break;
								case 1:
									this.maidArray[j].SetPos(new Vector3(-0.3f, 0f, 0f));
									break;
								case 2:
									this.maidArray[j].SetPos(new Vector3(0.7f, 0f, 0.4f));
									break;
								case 3:
									this.maidArray[j].SetPos(new Vector3(-0.7f, 0f, 0.4f));
									break;
								case 4:
									this.maidArray[j].SetPos(new Vector3(1f, 0f, 0.9f));
									break;
								case 5:
									this.maidArray[j].SetPos(new Vector3(-1f, 0f, 0.9f));
									break;
							}
						}
					}
					else
					{
						float num = 0f;
						if (this.selectList.Count >= 11)
						{
							num = -0.4f;
							if (this.selectList.Count % 2 == 1)
							{
								switch (j)
								{
									case 0:
										this.maidArray[j].SetPos(new Vector3(0f, 0f, 0f + num));
										break;
									case 1:
										this.maidArray[j].SetPos(new Vector3(-0.5f, 0f, 0.2f + num));
										break;
									case 2:
										this.maidArray[j].SetPos(new Vector3(0.5f, 0f, 0.2f + num));
										break;
									case 3:
										this.maidArray[j].SetPos(new Vector3(-0.9f, 0f, 0.55f + num));
										break;
									case 4:
										this.maidArray[j].SetPos(new Vector3(0.9f, 0f, 0.55f + num));
										break;
									case 5:
										this.maidArray[j].SetPos(new Vector3(-1.25f, 0f, 0.9f + num));
										break;
									case 6:
										this.maidArray[j].SetPos(new Vector3(1.25f, 0f, 0.9f + num));
										break;
									case 7:
										this.maidArray[j].SetPos(new Vector3(-1.57f, 0f, 1.3f + num));
										break;
									case 8:
										this.maidArray[j].SetPos(new Vector3(1.57f, 0f, 1.3f + num));
										break;
									case 9:
										this.maidArray[j].SetPos(new Vector3(-1.77f, 0f, 1.72f + num));
										break;
									case 10:
										this.maidArray[j].SetPos(new Vector3(1.77f, 0f, 1.72f + num));
										break;
									case 11:
										this.maidArray[j].SetPos(new Vector3(-1.85f, 0f, 2.17f + num));
										break;
									case 12:
										this.maidArray[j].SetPos(new Vector3(1.85f, 0f, 2.17f + num));
										break;
									default:
										this.maidArray[j].SetPos(new Vector3(0f, 0f, 0.7f + num));
										break;
								}
							}
							else
							{
								switch (j)
								{
									case 0:
										this.maidArray[j].SetPos(new Vector3(0.25f, 0f, 0f + num));
										break;
									case 1:
										this.maidArray[j].SetPos(new Vector3(-0.25f, 0f, 0f + num));
										break;
									case 2:
										this.maidArray[j].SetPos(new Vector3(0.7f, 0f, 0.25f + num));
										break;
									case 3:
										this.maidArray[j].SetPos(new Vector3(-0.7f, 0f, 0.25f + num));
										break;
									case 4:
										this.maidArray[j].SetPos(new Vector3(1.05f, 0f, 0.6f + num));
										break;
									case 5:
										this.maidArray[j].SetPos(new Vector3(-1.05f, 0f, 0.6f + num));
										break;
									case 6:
										this.maidArray[j].SetPos(new Vector3(1.35f, 0f, 0.9f + num));
										break;
									case 7:
										this.maidArray[j].SetPos(new Vector3(-1.35f, 0f, 0.9f + num));
										break;
									case 8:
										this.maidArray[j].SetPos(new Vector3(1.6f, 0f, 1.3f + num));
										break;
									case 9:
										this.maidArray[j].SetPos(new Vector3(-1.6f, 0f, 1.3f + num));
										break;
									case 10:
										this.maidArray[j].SetPos(new Vector3(1.8f, 0f, 1.72f + num));
										break;
									case 11:
										this.maidArray[j].SetPos(new Vector3(-1.8f, 0f, 1.72f + num));
										break;
									case 12:
										this.maidArray[j].SetPos(new Vector3(1.9f, 0f, 2.17f + num));
										break;
									case 13:
										this.maidArray[j].SetPos(new Vector3(-1.9f, 0f, 2.17f + num));
										break;
									default:
										this.maidArray[j].SetPos(new Vector3(0f, 0f, 0.7f + num));
										break;
								}
							}
						}
						else if (this.selectList.Count >= 8)
						{
							if (this.selectList.Count >= 9)
							{
								num = -0.2f;
							}
							if (this.selectList.Count % 2 == 1)
							{
								switch (j)
								{
									case 0:
										this.maidArray[j].SetPos(new Vector3(0f, 0f, 0f + num));
										break;
									case 1:
										this.maidArray[j].SetPos(new Vector3(-0.55f, 0f, 0.2f + num));
										break;
									case 2:
										this.maidArray[j].SetPos(new Vector3(0.55f, 0f, 0.2f + num));
										break;
									case 3:
										this.maidArray[j].SetPos(new Vector3(-1f, 0f, 0.6f + num));
										break;
									case 4:
										this.maidArray[j].SetPos(new Vector3(1f, 0f, 0.6f + num));
										break;
									case 5:
										this.maidArray[j].SetPos(new Vector3(-1.35f, 0f, 1f + num));
										break;
									case 6:
										this.maidArray[j].SetPos(new Vector3(1.35f, 0f, 1f + num));
										break;
									case 7:
										this.maidArray[j].SetPos(new Vector3(-1.6f, 0f, 1.4f + num));
										break;
									case 8:
										this.maidArray[j].SetPos(new Vector3(1.6f, 0f, 1.4f + num));
										break;
								}
							}
							else
							{
								switch (j)
								{
									case 0:
										this.maidArray[j].SetPos(new Vector3(0.28f, 0f, 0f + num));
										break;
									case 1:
										this.maidArray[j].SetPos(new Vector3(-0.28f, 0f, 0f + num));
										break;
									case 2:
										this.maidArray[j].SetPos(new Vector3(0.78f, 0f, 0.3f + num));
										break;
									case 3:
										this.maidArray[j].SetPos(new Vector3(-0.78f, 0f, 0.3f + num));
										break;
									case 4:
										this.maidArray[j].SetPos(new Vector3(1.22f, 0f, 0.7f + num));
										break;
									case 5:
										this.maidArray[j].SetPos(new Vector3(-1.22f, 0f, 0.7f + num));
										break;
									case 6:
										this.maidArray[j].SetPos(new Vector3(1.55f, 0f, 1.1f + num));
										break;
									case 7:
										this.maidArray[j].SetPos(new Vector3(-1.55f, 0f, 1.1f + num));
										break;
									case 8:
										this.maidArray[j].SetPos(new Vector3(1.77f, 0f, 1.58f + num));
										break;
									case 9:
										this.maidArray[j].SetPos(new Vector3(-1.77f, 0f, 1.58f + num));
										break;
								}
							}
						}
					}
					zero2.y = (float)(Math.Atan2((double)this.maidArray[j].transform.position.x, (double)(this.maidArray[j].transform.position.z - 1.5f)) * 180.0 / 3.1415926535897931) + 180f;
					this.maidArray[j].SetRot(zero2);
				}
				this.isHaiti = false;
			}
			if (this.isYobidashi)
			{
				bool flag = false;
				for (int j = 0; j < this.maxMaidCnt; j++)
				{
					if (this.selectList.Count > j && this.maidArray[j] != null && this.maidArray[j].IsBusy)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					for (int k = 0; k < this.selectList.Count; k++)
					{
						if ((k == this.maxMaidCnt - 1 || (k < this.maxMaidCnt - 1 && this.maidArray[k + 1] == null)) && this.maidArray[k] == null)
						{
							if ((k != 0 || !(this.maidArray[k + 1] == null) || !(this.maidArray[k] == null)) && (k <= 0 || !(this.maidArray[k - 1] != null) || this.maidArray[k - 1].IsBusy))
							{
								return;
							}
							if ((int)this.selectList[k] >= 12)
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.GetStockMaid((int)this.selectList[k]);
								if (!this.maidArray[k].body0.isLoadedBody)
								{
									this.maidArray[k].DutPropAll();
									this.maidArray[k].AllProcPropSeqStart();
								}
								this.maidArray[k].Visible = true;
							}
							else if (this.sceneLevel != 5)
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.Activate((int)this.selectList[k], (int)this.selectList[k], false, false);
								this.maidArray[k] = GameMain.Instance.CharacterMgr.CharaVisible((int)this.selectList[k], true, false);
							}
							else if (k == 0 && (int)this.selectList[k] == 0)
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.Activate((int)this.selectList[k], (int)this.selectList[k], false, false);
								this.maidArray[k] = GameMain.Instance.CharacterMgr.CharaVisible((int)this.selectList[k], true, false);
							}
							else if (k == 0)
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.Activate(0, (int)this.selectList[k], false, false);
								this.maidArray[k] = GameMain.Instance.CharacterMgr.CharaVisible(0, true, false);
							}
							else if ((int)this.selectList[k] + 1 == 12)
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.GetStockMaid((int)this.selectList[k]);
								if (!this.maidArray[k].body0.isLoadedBody)
								{
									this.maidArray[k].DutPropAll();
									this.maidArray[k].AllProcPropSeqStart();
								}
								this.maidArray[k].Visible = true;
							}
							else
							{
								this.maidArray[k] = GameMain.Instance.CharacterMgr.Activate((int)this.selectList[k] + 1, (int)this.selectList[k], false, false);
								this.maidArray[k] = GameMain.Instance.CharacterMgr.CharaVisible((int)this.selectList[k] + 1, true, false);
							}
							if (this.maidArray[k] && this.maidArray[k].Visible)
							{
								this.maidArray[k].body0.boHeadToCam = true;
								this.maidArray[k].body0.boEyeToCam = true;
							}
						}
					}
					this.isHaiti = true;
					this.isYobidashi = false;
				}
			}
			if (this.sceneLevel == 5 && !this.isFadeOut && Input.GetKeyDown(KeyCode.F7) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
			{
				if (!this.isF7S)
				{
					this.isF7S = true;
					this.isF7SInit = true;
					this.bGui = false;
					base.Preferences["config"]["shift_f7"].Value = "true";
					base.SaveConfig();
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					this.bgmCombo.selectedItemIndex = 2;
				}
				else
				{
					this.isF7S = false;
					base.Preferences["config"]["shift_f7"].Value = "false";
					base.SaveConfig();
					if (!this.isF7)
					{
						this.init2();
						this.okFlg = false;
						this.bGui = false;
					}
					GameMain.Instance.SoundMgr.PlaySe("se003.ogg", false);
				}
			}
			else if (this.sceneLevel == 5 && !this.isFadeOut && Input.GetKeyDown(KeyCode.F7))
			{
				if (this.isF7S && !this.isF7)
				{
					this.okFlg = true;
					this.faceFlg = false;
					this.poseFlg = false;
					this.sceneFlg = false;
					this.kankyoFlg = false;
					this.kankyo2Flg = false;
					this.bGui = true;
					this.isGuiInit = true;
					this.isF7 = true;
					this.bgmCombo.selectedItemIndex = 2;
				}
				else if (this.isF7S && this.isF7)
				{
					this.bGui = false;
					this.isF7 = false;
				}
				else if (!this.isF7)
				{
					this.init();
					this.okFlg = true;
					this.isGuiInit = true;
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
					this.listStyle.normal.textColor = Color.white;
					this.listStyle.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
					this.listStyle.onHover.background = (this.listStyle.hover.background = new Texture2D(2, 2));
					this.listStyle.padding.left = (this.listStyle.padding.right = (this.listStyle.padding.top = (this.listStyle.padding.bottom = 4)));
					this.listStyle.fontSize = this.GetPix(13);
					this.isYobidashi = true;
					this.bGui = true;
					this.isFadeOut = true;
					if (!this.isF7S)
					{
						this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
					}
					List<Maid> stockMaidList = GameMain.Instance.CharacterMgr.GetStockMaidList();
					for (int i = 0; i < stockMaidList.Count; i++)
					{
						if (this.maidArray[0] == stockMaidList[i])
						{
							this.editMaid = i;
						}
					}
					this.selectList = new ArrayList();
					this.selectList.Add(this.editMaid);
					this.bgmCombo.selectedItemIndex = 2;
					try
					{
						this.shodaiFlg[(int)this.selectList[0]] = false;
						TMorph morph = this.maidArray[0].body0.Face.morph;
						float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
						float num2 = fieldValue[(int)morph.hash["tangopen"]];
					}
					catch
					{
						this.shodaiFlg[(int)this.selectList[0]] = true;
					}
					if (!this.isVR)
					{
						this.eyeL[(int)this.selectList[0]] = this.maidArray[0].body0.quaDefEyeL.eulerAngles;
						this.eyeR[(int)this.selectList[0]] = this.maidArray[0].body0.quaDefEyeR.eulerAngles;
					}
					this.isF7 = true;
				}
				else if (!this.isF7S)
				{
					if (!this.isVR)
					{
						if (!this.isDialog)
						{
							this.isDialog = true;
							GameMain.Instance.SysDlg.Show("複数メイド撮影を終了します。\nよろしいですか？", SystemDialog.TYPE.OK_CANCEL, delegate ()
							{
								GameMain.Instance.SysDlg.Close();
								this.isDialog = false;
								this.init();
								this.maidArray[0] = GameMain.Instance.CharacterMgr.Activate(0, this.editMaid, false, false);
								this.maidArray[0] = GameMain.Instance.CharacterMgr.CharaVisible(0, true, false);
								this.okFlg = false;
								this.bGui = false;
								this.isF7 = false;
							}, delegate ()
							{
								GameMain.Instance.SysDlg.Close();
								this.isDialog = false;
							});
						}
					}
					else
					{
						this.init();
						this.maidArray[0] = GameMain.Instance.CharacterMgr.Activate(0, this.editMaid, false, false);
						this.maidArray[0] = GameMain.Instance.CharacterMgr.CharaVisible(0, true, false);
						this.okFlg = false;
						this.bGui = false;
						this.isF7 = false;
					}
				}
				else
				{
					this.isF7SInit = true;
					this.bGui = false;
					this.isF7 = false;
				}
			}
			if (this.sceneLevel == 5 && this.isF7S && this.isF7SInit)
			{
				this.isF7SInit = false;
				this.init2();
				this.okFlg = true;
				this.ikMaid = 0;
				this.ikBui = 5;
				this.ikMode[0] = 0;
				this.bgmCombo.selectedItemIndex = 2;
				this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
				List<Maid> stockMaidList = GameMain.Instance.CharacterMgr.GetStockMaidList();
				for (int i = 0; i < stockMaidList.Count; i++)
				{
					if (this.maidArray[0] == stockMaidList[i])
					{
						this.editMaid = i;
					}
				}
				this.selectList = new ArrayList();
				this.selectList.Add(this.editMaid);
				this.listStyle.normal.textColor = Color.white;
				this.listStyle.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
				this.listStyle.onHover.background = (this.listStyle.hover.background = new Texture2D(2, 2));
				this.listStyle.padding.left = (this.listStyle.padding.right = (this.listStyle.padding.top = (this.listStyle.padding.bottom = 4)));
				this.listStyle.fontSize = this.GetPix(13);
				try
				{
					this.shodaiFlg[(int)this.selectList[0]] = false;
					TMorph morph = this.maidArray[0].body0.Face.morph;
					float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
					float num2 = fieldValue[(int)morph.hash["tangopen"]];
				}
				catch
				{
					this.shodaiFlg[(int)this.selectList[0]] = true;
				}
				if (!this.isVR)
				{
					this.eyeL[(int)this.selectList[0]] = this.maidArray[0].body0.quaDefEyeL.eulerAngles;
					this.eyeR[(int)this.selectList[0]] = this.maidArray[0].body0.quaDefEyeR.eulerAngles;
				}
				this.bGui = false;
			}
			if (this.sceneLevel == 3 && !this.isFadeOut && Input.GetKeyDown(KeyCode.F7))
			{
				if (this.isPanel)
				{
					GameObject gameObject = GameObject.Find("UI Root");
					GameObject gameObject2 = gameObject.transform.Find("DailyPanel").gameObject;
					if (!this.okFlg && !gameObject2.activeSelf)
					{
						return;
					}
					bool flag2 = GameMain.Instance.CharacterMgr.status.GetFlag("時間帯") == 3;
					CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
					for (int k = 0; k < characterMgr.GetStockMaidCount(); k++)
					{
						characterMgr.GetStockMaidList()[k].Visible = false;
					}
					this.init();
					this.okFlg = true;
					this.isF7 = true;
					this.isPanel = false;
					this.isGuiInit = true;
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
					this.listStyle.normal.textColor = Color.white;
					this.listStyle.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
					this.listStyle.onHover.background = (this.listStyle.hover.background = new Texture2D(2, 2));
					this.listStyle.padding.left = (this.listStyle.padding.right = (this.listStyle.padding.top = (this.listStyle.padding.bottom = 4)));
					this.listStyle.fontSize = this.GetPix(13);
					GameMain.Instance.BgMgr.ChangeBg("Theater");
					if (!this.isVR)
					{
						GameMain.Instance.MainCamera.Reset(0, true);
						CameraMain cameraMain = GameMain.Instance.MainCamera;
						cameraMain.SetTargetPos(new Vector3(0f, 0.9f, 0f), true);
						cameraMain.SetDistance(3f, true);
					}
					this.isYobidashi = true;
					this.bGui = false;
					this.isFadeOut = true;
					GameMain.Instance.MainCamera.FadeOut(0f, false, null, true, default(Color));
					this.selectList = new ArrayList();
					this.selectList.Add(0);
					GameMain.Instance.SoundMgr.PlayBGM("BGM008.ogg", 0f, true);
					this.bgmCombo.selectedItemIndex = 0;
					gameObject2.SetActive(this.isPanel);
				}
				else if (!this.isVR)
				{
					if (!this.isDialog)
					{
						this.isDialog = true;
						GameMain.Instance.SysDlg.Show("複数メイド撮影を終了します。\nよろしいですか？", SystemDialog.TYPE.OK_CANCEL, delegate ()
						{
							GameMain.Instance.SysDlg.Close();
							this.isDialog = false;
							GameObject gameObject7 = GameObject.Find("UI Root");
							GameObject gameObject8 = gameObject7.transform.Find("DailyPanel").gameObject;
							if (this.okFlg || gameObject8.activeSelf)
							{
								bool flag14 = GameMain.Instance.CharacterMgr.status.GetFlag("時間帯") == 3;
								CharacterMgr characterMgr2 = GameMain.Instance.CharacterMgr;
								for (int num16 = 0; num16 < characterMgr2.GetStockMaidCount(); num16++)
								{
									characterMgr2.GetStockMaidList()[num16].Visible = false;
								}
								this.init();
								this.isPanel = true;
								this.isF7 = false;
								this.bGui = false;
								GameMain.Instance.SoundMgr.PlayBGM("BGM009.ogg", 1f, true);
								if (flag14)
								{
									GameMain.Instance.BgMgr.ChangeBg("ShinShitsumu_ChairRot_Night");
								}
								else
								{
									GameMain.Instance.BgMgr.ChangeBg("ShinShitsumu_ChairRot");
								}
								if (!this.isVR)
								{
									GameMain.Instance.MainCamera.Reset(0, true);
									GameMain.Instance.MainCamera.SetTargetPos(new Vector3(0.5609447f, 1.380762f, -1.382336f), true);
									GameMain.Instance.MainCamera.SetDistance(1.6f, true);
									GameMain.Instance.MainCamera.SetAroundAngle(new Vector2(245.5691f, 6.273283f), true);
								}
								gameObject8.SetActive(this.isPanel);
							}
						}, delegate ()
						{
							GameMain.Instance.SysDlg.Close();
							this.isDialog = false;
						});
					}
				}
				else
				{
					GameObject gameObject = GameObject.Find("UI Root");
					GameObject gameObject2 = gameObject.transform.Find("DailyPanel").gameObject;
					if (!this.okFlg && !gameObject2.activeSelf)
					{
						return;
					}
					bool flag2 = GameMain.Instance.CharacterMgr.status.GetFlag("時間帯") == 3;
					CharacterMgr characterMgr = GameMain.Instance.CharacterMgr;
					for (int k = 0; k < characterMgr.GetStockMaidCount(); k++)
					{
						characterMgr.GetStockMaidList()[k].Visible = false;
					}
					this.init();
					this.isPanel = true;
					this.isF7 = false;
					this.bGui = false;
					GameMain.Instance.SoundMgr.PlayBGM("BGM009.ogg", 1f, true);
					if (flag2)
					{
						GameMain.Instance.BgMgr.ChangeBg("ShinShitsumu_ChairRot_Night");
					}
					else
					{
						GameMain.Instance.BgMgr.ChangeBg("ShinShitsumu_ChairRot");
					}
					gameObject2.SetActive(this.isPanel);
				}
			}
			if (!this.okFlg)
			{
				if (this.maidArray[0] && this.maidArray[0].Visible)
				{
					int num3 = (int)this.maidArray[0].transform.position.y;
					if (num3 == 100)
					{
						this.okFlg = true;
						Vector3 vector = Vector3.zero;
						this.maidArray[0].SetPos(vector);
						this.isScript = true;
					}
				}
				else if (this.maidArray[0] && !this.maidArray[0].Visible)
				{
					this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
				}
				else if (!this.maidArray[0])
				{
					this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
				}
			}
			if (this.okFlg)
			{
				this.hFlg = false;
				this.mFlg = false;
				this.fFlg = false;
				this.sFlg = false;
				this.atFlg = false;
				this.yFlg = false;
				this.escFlg = false;
				this.maidCnt = 0;
				if (!this.cameraObj && !this.isVR)
				{
					this.cameraObj = new GameObject("subCamera");
					this.subcamera = this.cameraObj.AddComponent<Camera>();
					this.subcamera.CopyFrom(Camera.main);
					this.cameraObj.SetActive(true);
					this.subcamera.clearFlags = CameraClearFlags.Depth;
					this.subcamera.cullingMask = 256;
					this.subcamera.depth = 1f;
					this.subcamera.transform.parent = this.mainCamera.transform;
					float num2 = 2f;
					if (Application.unityVersion.StartsWith("4"))
					{
						num2 = 1f;
					}
					GameObject gameObject3 = new GameObject("Light");
					gameObject3.AddComponent<Light>();
					this.lightList.Add(gameObject3);
					this.lightColorR.Add(1f);
					this.lightColorG.Add(1f);
					this.lightColorB.Add(1f);
					this.lightIndex.Add(0);
					this.lightX.Add(40f);
					this.lightY.Add(180f);
					this.lightAkarusa.Add(num2);
					this.lightKage.Add(0.098f);
					this.lightRange.Add(50f);
					gameObject3.transform.position = GameMain.Instance.MainLight.transform.position;
					gameObject3.GetComponent<Light>().intensity = num2;
					gameObject3.GetComponent<Light>().spotAngle = 50f;
					gameObject3.GetComponent<Light>().range = 10f;
					gameObject3.GetComponent<Light>().type = LightType.Directional;
					gameObject3.GetComponent<Light>().color = new Color(0.5f, 1f, 0f);
					gameObject3.GetComponent<Light>().cullingMask = 256;
				}
				if (this.getModKeyPressing(MultipleMaids.modKey.Shift) && !this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && !this.getModKeyPressing(MultipleMaids.modKey.Alt) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.V) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
				{
					float axis = Input.GetAxis("Mouse ScrollWheel");
					if (axis > 0f)
					{
						this.mainCamera.SetDistance(this.mainCamera.GetDistance() - 0.5f, true);
						if (this.mainCamera.GetDistance() < 0.1f)
						{
							this.mainCamera.SetDistance(0.1f, true);
						}
					}
					else if (axis < 0f)
					{
						this.mainCamera.SetDistance(this.mainCamera.GetDistance() + 0.5f, true);
						if (this.mainCamera.GetDistance() > 25f)
						{
							this.mainCamera.SetDistance(25f, true);
						}
					}
				}
				for (int j = 0; j < this.maxMaidCnt; j++)
				{
					if (this.maidArray[j] && this.maidArray[j].Visible)
					{
						this.maidCnt++;
					}
				}
				if (this.maidArray[0] != null && this.maidArray[0].Visible)
				{
					if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.speed = 5f * Time.deltaTime * 60f;
					}
					else
					{
						this.speed = 1f * Time.deltaTime * 60f;
					}
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					if (!this.isVR || this.isVR2)
					{
						if (!this.isVR)
						{
							if (this.isBloom)
							{
								Bloom fieldValue2 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
								fieldValue2.enabled = true;
								fieldValue2.bloomIntensity = this.bloom1;
								fieldValue2.bloomBlurIterations = (int)this.bloom2;
								fieldValue2.bloomThreshholdColor = new Color(1f - this.bloom3, 1f - this.bloom4, 1f - this.bloom5);
								if (this.isBloomA)
								{
									fieldValue2.hdr = Bloom.HDRBloomMode.On;
								}
								else
								{
									fieldValue2.hdr = Bloom.HDRBloomMode.Auto;
								}
								this.isBloom2 = true;
							}
							else if (this.isBloom2)
							{
								this.isBloom2 = false;
								Bloom fieldValue2 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
								fieldValue2.enabled = true;
								fieldValue2.bloomIntensity = 2.85f;
								fieldValue2.hdr = 0;
								fieldValue2.bloomThreshholdColor = new Color(1f, 1f, 1f);
								fieldValue2.bloomBlurIterations = 3;
							}
							if (this.isDepth)
							{
								this.depth_field_.enabled = true;
								this.depth_field_.focalLength = this.depth1;
								this.depth_field_.focalSize = this.depth2;
								this.depth_field_.aperture = this.depth3;
								this.depth_field_.maxBlurSize = this.depth4;
								if (this.isDepthA)
								{
									this.depth_field_.visualizeFocus = true;
								}
								else
								{
									this.depth_field_.visualizeFocus = false;
								}
							}
							else
							{
								if (this.depth_field_ == null)
								{
									this.depth_field_ = GameMain.Instance.MainCamera.gameObject.AddComponent<DepthOfFieldScatter>();
									if (this.depth_field_.dofHdrShader == null)
									{
										this.depth_field_.dofHdrShader = Shader.Find("Hidden/Dof/DepthOfFieldHdr");
									}
									if (this.depth_field_.dx11BokehShader == null)
									{
										this.depth_field_.dx11BokehShader = Shader.Find("Hidden/Dof/DX11Dof");
									}
									if (this.depth_field_.dx11BokehTexture == null)
									{
										this.depth_field_.dx11BokehTexture = (Resources.Load("Textures/hexShape") as Texture2D);
									}
								}
								this.depth_field_.enabled = false;
							}
							if (this.isFog)
							{
								if (this.fog_.fogShader == null)
								{
									this.fog_.fogShader = Shader.Find("Hidden/GlobalFog");
								}
								this.fog_.enabled = true;
								this.fog_.startDistance = this.fog1;
								this.fog_.globalDensity = this.fog2;
								this.fog_.heightScale = this.fog3;
								this.fog_.height = this.fog4;
								this.fog_.globalFogColor.r = this.fog5;
								this.fog_.globalFogColor.g = this.fog6;
								this.fog_.globalFogColor.b = this.fog7;
							}
							else
							{
								if (this.fog_ == null)
								{
									this.fog_ = GameMain.Instance.MainCamera.gameObject.AddComponent<GlobalFog>();
								}
								this.fog_.enabled = false;
							}
							if (this.isSepia != this.isSepian)
							{
								this.isSepia = this.isSepian;
								if (this.isSepia)
								{
									if (this.sepia_tone_.shader == null)
									{
										this.sepia_tone_.shader = Shader.Find("Hidden/Sepiatone Effect");
									}
									this.sepia_tone_.enabled = true;
								}
								else
								{
									if (this.sepia_tone_ == null)
									{
										this.sepia_tone_ = GameMain.Instance.MainCamera.gameObject.AddComponent<SepiaToneEffect>();
									}
									this.sepia_tone_.enabled = false;
								}
							}
							if (this.bokashi > 0f)
							{
								Blur component = GameMain.Instance.MainCamera.gameObject.GetComponent<Blur>();
								component.enabled = true;
								component.blurSize = this.bokashi / 10f;
								component.blurIterations = 0;
								component.downsample = 0;
								if (this.bokashi > 3f)
								{
									component.blurSize -= 0.3f;
									component.blurIterations = 1;
									component.downsample = 1;
								}
							}
							else
							{
								Blur component = GameMain.Instance.MainCamera.gameObject.GetComponent<Blur>();
								component.enabled = false;
							}
							if (this.kamiyure > 0f)
							{
								for (int j = 0; j < this.maidCnt; j++)
								{
									Maid maid = this.maidArray[j];
									for (int l = 0; l < maid.body0.goSlot.Count; l++)
									{
										if (maid.body0.goSlot[l].obj != null)
										{
											DynamicBone component2 = maid.body0.goSlot[l].obj.GetComponent<DynamicBone>();
											if (component2 != null && component2.enabled)
											{
												component2.m_Gravity = new Vector3(this.softG.x * 5f, (this.softG.y + 0.003f) * 5f, this.softG.z * 5f);
											}
										}
										List<THair1> fieldValue3 = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid.body0.goSlot[l].bonehair, "hair1list");
										for (int k = 0; k < fieldValue3.Count; k++)
										{
											fieldValue3[k].SoftG = new Vector3(this.softG.x, this.softG.y + this.kamiyure, this.softG.z);
										}
									}
								}
							}
							if (this.isBlur)
							{
								Vignetting component3 = GameMain.Instance.MainCamera.gameObject.GetComponent<Vignetting>();
								component3.mode = 0;
								component3.intensity = this.blur1;
								component3.chromaticAberration = this.blur4;
								component3.blur = this.blur2;
								component3.blurSpread = this.blur3;
								component3.enabled = true;
								this.isBlur2 = true;
							}
							else if (this.isBlur2)
							{
								this.isBlur2 = false;
								Vignetting component3 = GameMain.Instance.MainCamera.gameObject.GetComponent<Vignetting>();
								component3.mode = 0;
								component3.intensity = -3.98f;
								component3.chromaticAberration = -0.75f;
								component3.axialAberration = 1.18f;
								component3.blurSpread = 4.19f;
								component3.luminanceDependency = 0.494f;
								component3.blurDistance = 1.71f;
								component3.enabled = false;
							}
						}
						if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.I))
						{
							GameMain.Instance.MainLight.transform.eulerAngles -= Vector3.right / 2f;
							this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.K))
						{
							if (GameMain.Instance.MainLight.transform.eulerAngles.x < 85f)
							{
								GameMain.Instance.MainLight.transform.eulerAngles += Vector3.right / 2f;
								this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.J))
						{
							GameMain.Instance.MainLight.transform.eulerAngles -= Vector3.up / 1.5f;
							this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.L))
						{
							GameMain.Instance.MainLight.transform.eulerAngles += Vector3.up / 1.5f;
							this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.BackQuote))
						{
							GameMain.Instance.MainLight.Reset();
							GameMain.Instance.MainLight.SetIntensity(0.95f);
							GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
							this.lightIndex[0] = 0;
							this.lightColorR[0] = 1f;
							this.lightColorG[0] = 1f;
							this.lightColorB[0] = 1f;
							this.lightX[0] = 40f;
							this.lightY[0] = 180f;
							this.lightAkarusa[0] = 0.95f;
							this.lightKage[0] = 0.098f;
							this.lightRange[0] = 50f;
							this.bgObject.SetActive(true);
							this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
							this.isIdx1 = false;
							this.isIdx2 = false;
							this.isIdx3 = false;
							this.isIdx4 = false;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Minus) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							List<float> list;
							(list = this.lightColorR)[0] = list[0] + 0.01f;
							if (this.lightColorR[0] > 1f)
							{
								this.lightColorR[0] = 1f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Quote) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							List<float> list;
							(list = this.lightColorG)[0] = list[0] + 0.01f;
							if (this.lightColorG[0] > 1f)
							{
								this.lightColorG[0] = 1f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftBracket) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							List<float> list;
							(list = this.lightColorB)[0] = list[0] + 0.01f;
							if (this.lightColorB[0] > 1f)
							{
								this.lightColorB[0] = 1f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Minus))
						{
							List<float> list;
							(list = this.lightColorR)[0] = list[0] - 0.01f;
							if (this.lightColorR[0] < 0f)
							{
								this.lightColorR[0] = 0f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Quote))
						{
							List<float> list;
							(list = this.lightColorG)[0] = list[0] - 0.01f;
							if (this.lightColorG[0] < 0f)
							{
								this.lightColorG[0] = 0f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftBracket))
						{
							List<float> list;
							(list = this.lightColorB)[0] = list[0] - 0.01f;
							if (this.lightColorB[0] < 0f)
							{
								this.lightColorB[0] = 0f;
							}
							if (this.lightIndex[0] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Alpha0))
						{
							GameMain.Instance.MainLight.GetComponent<Light>().intensity += 0.12f * Time.deltaTime;
							this.lightAkarusa[0] = GameMain.Instance.MainLight.GetComponent<Light>().intensity;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.P))
						{
							GameMain.Instance.MainLight.GetComponent<Light>().intensity -= 0.12f * Time.deltaTime;
							this.lightAkarusa[0] = GameMain.Instance.MainLight.GetComponent<Light>().intensity;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Alpha9))
						{
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle += 0.2f;
							GameMain.Instance.MainLight.GetComponent<Light>().range += 0.2f;
							this.lightRange[0] = GameMain.Instance.MainLight.GetComponent<Light>().spotAngle;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.O))
						{
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle -= 0.2f;
							GameMain.Instance.MainLight.GetComponent<Light>().range -= 0.2f;
							this.lightRange[0] = GameMain.Instance.MainLight.GetComponent<Light>().spotAngle;
						}
						else if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.U))
						{
							List<int> list2;
							(list2 = this.lightIndex)[0] = list2[0] + 1;
							if (this.lightIndex[0] == 4)
							{
								this.lightIndex[0] = 0;
							}
							this.lightColorR[0] = 1f;
							this.lightColorG[0] = 1f;
							this.lightColorB[0] = 1f;
							this.lightX[0] = 40f;
							this.lightY[0] = 180f;
							this.lightAkarusa[0] = 0.95f;
							this.lightKage[0] = 0.098f;
							this.lightRange[0] = 50f;
							GameMain.Instance.MainLight.Reset();
							GameMain.Instance.MainLight.SetIntensity(0.95f);
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 50f;
							GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
							GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
							if (this.lightIndex[0] == 0)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
								this.bgObject.SetActive(true);
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
							}
							else if (this.lightIndex[0] == 1)
							{
								GameMain.Instance.MainLight.transform.eulerAngles += Vector3.right * 40f;
								this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
								GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Spot;
							}
							else if (this.lightIndex[0] == 2)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Point;
							}
							else if (this.lightIndex[0] == 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
								this.bgObject.SetActive(false);
							}
							this.isIdx1 = false;
							this.isIdx2 = false;
							this.isIdx3 = false;
							this.isIdx4 = false;
						}
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Insert))
					{
						GameMain.Instance.MainLight.transform.eulerAngles -= Vector3.right / 2f;
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Delete))
					{
						if (GameMain.Instance.MainLight.transform.eulerAngles.x < 85f)
						{
							GameMain.Instance.MainLight.transform.eulerAngles += Vector3.right / 2f;
						}
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Home))
					{
						GameMain.Instance.MainLight.transform.eulerAngles -= Vector3.up / 1.5f;
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.End))
					{
						GameMain.Instance.MainLight.transform.eulerAngles += Vector3.up / 1.5f;
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.BackQuote))
					{
						GameMain.Instance.MainLight.Reset();
						GameMain.Instance.MainLight.SetIntensity(0.95f);
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.PageUp))
					{
						GameMain.Instance.MainLight.GetComponent<Light>().intensity += 0.1f * Time.deltaTime;
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.PageDown))
					{
						GameMain.Instance.MainLight.GetComponent<Light>().intensity -= 0.1f * Time.deltaTime;
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.Quote))
					{
						if (this.lightIndex[0] == 1)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle += 0.2f;
						}
						else
						{
							GameMain.Instance.MainLight.GetComponent<Light>().range += 0.2f;
						}
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftBracket))
					{
						if (this.lightIndex[0] == 1)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle -= 0.2f;
						}
						else
						{
							GameMain.Instance.MainLight.GetComponent<Light>().range -= 0.2f;
						}
					}
					else if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.Minus))
					{
						List<int> list2;
						(list2 = this.lightIndex)[0] = list2[0] + 1;
						if (this.lightIndex[0] == 3)
						{
							this.lightIndex[0] = 0;
						}
						GameMain.Instance.MainLight.Reset();
						GameMain.Instance.MainLight.SetIntensity(0.95f);
						GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 50f;
						GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
						GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
						if (this.lightIndex[0] == 0)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
						}
						else if (this.lightIndex[0] == 1)
						{
							GameMain.Instance.MainLight.transform.eulerAngles += Vector3.right * 40f;
							GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Spot;
						}
						else if (this.lightIndex[0] == 2)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Point;
						}
					}
					int i;
					for (i = 0; i < this.lightList.Count; i++)
					{
						if (i > 0)
						{
							this.lightList[i].GetComponent<Light>().color = new Color(this.lightColorR[i], this.lightColorG[i], this.lightColorB[i]);
							this.lightList[i].GetComponent<Light>().intensity = this.lightAkarusa[i];
							this.lightList[i].GetComponent<Light>().spotAngle = this.lightRange[i];
							this.lightList[i].GetComponent<Light>().range = this.lightRange[i] / 5f;
							if (!Input.GetKey(KeyCode.X) && (!Input.GetKey(KeyCode.Z) || !this.getModKeyPressing(MultipleMaids.modKey.Shift)))
							{
								this.lightList[i].transform.eulerAngles = new Vector3(this.lightX[i], this.lightY[i], 18f);
							}
						}
						else
						{
							GameMain.Instance.MainLight.SetIntensity(this.lightAkarusa[0]);
							GameMain.Instance.MainLight.GetComponent<Light>().shadowStrength = this.lightKage[0];
							if (!Input.GetKey(KeyCode.X) && (!Input.GetKey(KeyCode.Z) || !this.getModKeyPressing(MultipleMaids.modKey.Shift)))
							{
								GameMain.Instance.MainLight.SetRotation(new Vector3(this.lightX[0], this.lightY[0], 18f));
							}
							GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = this.lightRange[i];
							GameMain.Instance.MainLight.GetComponent<Light>().range = this.lightRange[i] / 5f;
							if (this.lightIndex[i] != 3)
							{
								GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
							else
							{
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							}
						}
					}
					bool flag3 = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isVR && this.sceneLevel == 5 && j == this.isEditNo)
						{
							bool flag4 = this.shodaiFlg[(int)this.selectList[j]];
							this.shodaiFlg[(int)this.selectList[j]] = false;
							try
							{
								TMorph morph = this.maidArray[j].body0.Face.morph;
								float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
								float num2 = fieldValue[(int)morph.hash["tangopen"]];
							}
							catch
							{
								this.shodaiFlg[(int)this.selectList[j]] = true;
							}
							if (this.shodaiFlg[(int)this.selectList[j]] != flag4)
							{
								if (!this.isVR)
								{
									this.eyeL[(int)this.selectList[j]] = this.maidArray[j].body0.quaDefEyeL.eulerAngles;
									this.eyeR[(int)this.selectList[j]] = this.maidArray[j].body0.quaDefEyeR.eulerAngles;
								}
							}
						}
						if (this.maidArray[j] && this.maidArray[j].Visible && this.isStop[j])
						{
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Stop();
						}
						if (this.sceneLevel == 5)
						{
							if (this.editSelectMaid == this.maidArray[j])
							{
								flag3 = true;
							}
						}
						Maid maid = this.maidArray[j];
					}
					if (this.sceneLevel == 5 && !flag3 && this.maidCnt > 0)
					{
						this.isEditNo = 0;
						SceneEdit component4 = GameObject.Find("__SceneEdit__").GetComponent<SceneEdit>();
						MultipleMaids.SetFieldValue<SceneEdit, Maid>(component4, "m_maid", this.maidArray[0]);
					}
					i = 0;
					while (i < this.maidCnt)
					{
						Transform transform = this.maidArray[i].transform;
						Maid maid = this.maidArray[i];
						if (this.cafeFlg[i])
						{
							this.cafeCount[i]++;
							if (this.cafeCount[i] > 1)
							{
								maid.DelProp(MPN.handitem, true);
								maid.SetProp("handitem", "HandItemR_Etoile_Teacup_I_.menu", 0, true, false);
								maid.AllProcPropSeqStart();
								this.cafeFlg[i] = false;
								this.cafeCount[i] = 0;
							}
						}
						KeyCode key;
						if (i >= 14)
						{
							bool flag5 = false;
							bool flag6 = false;
							for (int k = 0; k < this.keyArray.Length; k++)
							{
								if (Input.GetKey(this.keyArray[k]))
								{
									flag5 = true;
									break;
								}
								if (Input.GetKeyUp(this.keyArray[k]))
								{
									flag6 = true;
									break;
								}
							}
							key = this.keyArray[6];
							if (!flag5 || this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
							{
								if (!flag6 || this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
								{
									goto IL_49FE;
								}
								this.idoFlg[i - 7] = false;
							}
						}
						else if (i >= 7)
						{
							bool flag5 = false;
							bool flag6 = false;
							for (int k = 0; k < this.keyArray.Length; k++)
							{
								if (Input.GetKey(this.keyArray[k]))
								{
									flag5 = true;
									break;
								}
								if (Input.GetKeyUp(this.keyArray[k]))
								{
									flag6 = true;
									break;
								}
							}
							key = this.keyArray[i - 7];
							if (!flag5 || this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
							{
								if (!flag6 || this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
								{
									goto IL_49FE;
								}
								this.idoFlg[i - 7] = false;
							}
						}
						else
						{
							bool flag5 = false;
							for (int k = 0; k < this.keyArray.Length; k++)
							{
								if (Input.GetKey(this.keyArray[k]))
								{
									flag5 = true;
									break;
								}
							}
							if (!flag5 || !this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
							{
								key = this.keyArray[i];
								goto IL_49FE;
							}
						}
					IL_165DC:
						i++;
						continue;
					IL_49FE:
						if (this.xFlg[i])
						{
							if (!maid.AudioMan.audiosource.isPlaying)
							{
								if (maid.status.personal.uniqueName == "Pride")
								{
									string text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.tunArray.Length; k++)
									{
										if (this.tunArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									string text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.coolArray.Length; k++)
									{
										if (this.coolArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									string text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.pureArray.Length; k++)
									{
										if (this.pureArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									string text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.yanArray.Length; k++)
									{
										if (this.yanArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									string text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h0Array.Length; k++)
									{
										if (this.h0Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									string text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h1Array.Length; k++)
									{
										if (this.h1Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									string text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h2Array.Length; k++)
									{
										if (this.h2Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
							}
						}
						if (this.zFlg[i])
						{
							if (!maid.AudioMan.audiosource.isPlaying)
							{
								string text = "";
								if (maid.status.personal.uniqueName == "Pride")
								{
									text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.tunArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.tunArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.coolArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.coolArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.pureArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.pureArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.yanArray.Length);
									text = text + string.Format("{0:00000}", this.yanArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h0tArray.Length);
									text = text + string.Format("{0:00000}", this.h0tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h1tArray.Length);
									text = text + string.Format("{0:00000}", this.h1tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h2tArray.Length);
									text = text + string.Format("{0:00000}", this.h2tArray[num4]) + ".ogg";
								}
								maid.AudioMan.LoadPlay(text, 0f, false, false);
							}
						}
						if (!this.isVR || this.isVR2)
						{
							if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Q))
							{
								this.ikMaid = i;
								this.isIK[i] = true;
								this.ikBui = 1;
								this.ikMode[i] = 0;
								this.SetIK(maid, i);
								this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Hand", true);
								this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R UpperArm", true);
								this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Forearm", true);
								this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Clavicle", true);
								this.IK_hand = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.qFlg = true;
							}
							else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.W))
							{
								this.ikMaid = i;
								this.isIK[i] = true;
								this.ikBui = 2;
								this.ikMode[i] = 0;
								this.SetIK(maid, i);
								this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Hand", true);
								this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L UpperArm", true);
								this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Forearm", true);
								this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Clavicle", true);
								this.IK_hand = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.A))
							{
								this.ikMaid = i;
								this.isIK[i] = true;
								this.ikBui = 3;
								this.ikMode[i] = 0;
								this.SetIK(maid, i);
								this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Foot", true);
								this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Thigh", true);
								this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Calf", true);
								this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_L", true);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.S))
							{
								this.ikMaid = i;
								this.isIK[i] = true;
								this.ikBui = 4;
								this.ikMode[i] = 0;
								this.SetIK(maid, i);
								this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Foot", true);
								this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Thigh", true);
								this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Calf", true);
								this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_L", true);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.sFlg = true;
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.U))
							{
								maid.body0.transform.localRotation = Quaternion.Euler(maid.body0.transform.localEulerAngles) * Quaternion.AngleAxis(-1.5f, Vector3.up);
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.idoFlg[i] = true;
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.O))
							{
								maid.body0.transform.localRotation = Quaternion.Euler(maid.body0.transform.localEulerAngles) * Quaternion.AngleAxis(1.5f, Vector3.up);
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.idoFlg[i] = true;
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.F))
							{
								this.bGui = true;
								this.isPoseInit = true;
								this.isGuiInit = true;
								this.poseFlg = true;
								this.faceFlg = false;
								this.sceneFlg = false;
								this.kankyoFlg = false;
								this.kankyo2Flg = false;
								this.fFlg = true;
								for (int k = 0; k < this.maidCnt; k++)
								{
									if (maid == this.maidArray[k])
									{
										this.selectMaidIndex = k;
									}
								}
								this.copyIndex = 0;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.idoFlg[i] = true;
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.V))
							{
								this.bGui = true;
								this.isFaceInit = true;
								this.isGuiInit = true;
								this.faceFlg = true;
								this.poseFlg = false;
								this.sceneFlg = false;
								this.kankyoFlg = false;
								this.kankyo2Flg = false;
								maid.boMabataki = false;
								for (int k = 0; k < this.maidCnt; k++)
								{
									if (maid == this.maidArray[k])
									{
										this.selectMaidIndex = k;
									}
								}
								this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
								this.idoFlg[i] = true;
							}
							else if (Input.GetKey(key) && this.getModKeyPressing(MultipleMaids.modKey.Shift) && (Input.GetKeyDown(KeyCode.X) || (this.isVR && Input.GetKeyDown(KeyCode.UpArrow))))
							{
								if (!this.xFlg[i])
								{
									this.xFlg[i] = true;
									this.zFlg[i] = false;
									if (maid.status.personal.uniqueName == "Pride")
									{
										string text = "s0_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.tunArray.Length; k++)
										{
											if (this.tunArray[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (!flag7)
										{
											text = text + "0" + num4.ToString() + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Cool")
									{
										string text = "s1_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.coolArray.Length; k++)
										{
											if (this.coolArray[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (!flag7)
										{
											text = text + "0" + num4.ToString() + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Pure")
									{
										string text = "s2_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.pureArray.Length; k++)
										{
											if (this.pureArray[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (!flag7)
										{
											text = text + "0" + num4.ToString() + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Yandere")
									{
										string text = "s3_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.yanArray.Length; k++)
										{
											if (this.yanArray[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (!flag7)
										{
											text = text + string.Format("{0:00000}", num4) + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Muku")
									{
										string text = "h0_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.h0Array.Length; k++)
										{
											if (this.h0Array[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (flag7)
										{
											text = text + string.Format("{0:00000}", num4) + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Majime")
									{
										string text = "h1_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.h1Array.Length; k++)
										{
											if (this.h1Array[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (flag7)
										{
											text = text + string.Format("{0:00000}", num4) + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
									if (maid.status.personal.uniqueName == "Rindere")
									{
										string text = "h2_";
										System.Random random = new System.Random();
										int num4 = random.Next(10000);
										bool flag7 = false;
										for (int k = 0; k < this.h2Array.Length; k++)
										{
											if (this.h2Array[k] == num4)
											{
												flag7 = true;
												break;
											}
										}
										if (flag7)
										{
											text = text + string.Format("{0:00000}", num4) + ".ogg";
											if (GameUty.FileSystem.IsExistentFile(text))
											{
												maid.AudioMan.LoadPlay(text, 0f, false, false);
											}
										}
									}
								}
								else
								{
									this.xFlg[i] = false;
									maid.AudioMan.Clear();
								}
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && this.getModKeyPressing(MultipleMaids.modKey.Shift) && (Input.GetKeyDown(KeyCode.Z) || (this.isVR && Input.GetKeyDown(KeyCode.DownArrow))))
							{
								if (!this.zFlg[i])
								{
									this.zFlg[i] = true;
									this.xFlg[i] = false;
									string text = "";
									if (maid.status.personal.uniqueName == "Pride")
									{
										text = "s0_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.tunArray.Length);
										object obj = text;
										text = string.Concat(new object[]
										{
											obj,
											"0",
											this.tunArray[num4],
											".ogg"
										});
									}
									if (maid.status.personal.uniqueName == "Cool")
									{
										text = "s1_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.coolArray.Length);
										object obj = text;
										text = string.Concat(new object[]
										{
											obj,
											"0",
											this.coolArray[num4],
											".ogg"
										});
									}
									if (maid.status.personal.uniqueName == "Pure")
									{
										text = "s2_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.pureArray.Length);
										object obj = text;
										text = string.Concat(new object[]
										{
											obj,
											"0",
											this.pureArray[num4],
											".ogg"
										});
									}
									if (maid.status.personal.uniqueName == "Yandere")
									{
										text = "s3_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.yanArray.Length);
										text = text + string.Format("{0:00000}", this.yanArray[num4]) + ".ogg";
									}
									if (maid.status.personal.uniqueName == "Muku")
									{
										text = "h0_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.h0tArray.Length);
										text = text + string.Format("{0:00000}", this.h0tArray[num4]) + ".ogg";
									}
									if (maid.status.personal.uniqueName == "Majime")
									{
										text = "h1_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.h1tArray.Length);
										text = text + string.Format("{0:00000}", this.h1tArray[num4]) + ".ogg";
									}
									if (maid.status.personal.uniqueName == "Rindere")
									{
										text = "h2_";
										System.Random random = new System.Random();
										int num4 = random.Next(this.h2tArray.Length);
										text = text + string.Format("{0:00000}", this.h2tArray[num4]) + ".ogg";
									}
									maid.AudioMan.LoadPlay(text, 0f, false, false);
								}
								else
								{
									this.zFlg[i] = false;
									maid.AudioMan.Clear();
								}
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && (Input.GetKeyDown(KeyCode.Z) || (this.isVR && Input.GetKeyDown(KeyCode.DownArrow))))
							{
								string text = "";
								if (maid.status.personal.uniqueName == "Pride")
								{
									text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.tunArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.tunArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.coolArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.coolArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.pureArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.pureArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.yanArray.Length);
									text = text + string.Format("{0:00000}", this.yanArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h0tArray.Length);
									text = text + string.Format("{0:00000}", this.h0tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h1tArray.Length);
									text = text + string.Format("{0:00000}", this.h1tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h2tArray.Length);
									text = text + string.Format("{0:00000}", this.h2tArray[num4]) + ".ogg";
								}
								maid.AudioMan.LoadPlay(text, 0f, false, false);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && (Input.GetKeyDown(KeyCode.X) || (this.isVR && Input.GetKeyDown(KeyCode.UpArrow))))
							{
								if (maid.status.personal.uniqueName == "Pride")
								{
									string text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.tunArray.Length; k++)
									{
										if (this.tunArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									string text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.coolArray.Length; k++)
									{
										if (this.coolArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									string text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.pureArray.Length; k++)
									{
										if (this.pureArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									string text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.yanArray.Length; k++)
									{
										if (this.yanArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									string text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h0Array.Length; k++)
									{
										if (this.h0Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									string text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h1Array.Length; k++)
									{
										if (this.h1Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									string text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h2Array.Length; k++)
									{
										if (this.h2Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && (Input.GetKeyDown(KeyCode.LeftBracket) || (Input.GetKeyDown(KeyCode.BackQuote) && this.getModKeyPressing(MultipleMaids.modKey.Alt))))
							{
								for (int k = 0; k < 10; k++)
								{
									maid.transform.RotateAround(maid.transform.position, Vector3.right, -maid.transform.rotation.eulerAngles.x);
									maid.transform.RotateAround(maid.transform.position, Vector3.forward, -maid.transform.rotation.eulerAngles.z);
								}
								this.idoFlg[i] = true;
								this.atFlg = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && ((Input.GetKey(KeyCode.Minus) && this.getModKeyPressing(MultipleMaids.modKey.Shift)) || (Input.GetKey(KeyCode.I) && this.getModKeyPressing(MultipleMaids.modKey.Alt))))
							{
								maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), 0.4f);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && ((Input.GetKey(KeyCode.Quote) && this.getModKeyPressing(MultipleMaids.modKey.Shift)) || (Input.GetKey(KeyCode.K) && this.getModKeyPressing(MultipleMaids.modKey.Alt))))
							{
								maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), -0.4f);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && (Input.GetKey(KeyCode.Minus) || (Input.GetKey(KeyCode.J) && this.getModKeyPressing(MultipleMaids.modKey.Alt))))
							{
								maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), 0.4f);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && (Input.GetKey(KeyCode.Quote) || (Input.GetKey(KeyCode.L) && this.getModKeyPressing(MultipleMaids.modKey.Alt))))
							{
								maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), -0.4f);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.I))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.K))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = maid.transform.position;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.Alpha0))
							{
								Vector3 vector = maid.transform.position;
								vector.y += 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKey(KeyCode.P))
							{
								Vector3 vector = maid.transform.position;
								vector.y -= 0.0075f * this.speed;
								maid.SetPos(vector);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (maid.boMabataki && Input.GetKey(key) && Input.GetKeyDown(KeyCode.Alpha8) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
							{
								this.faceIndex[i]--;
								if (this.faceIndex[i] <= -1)
								{
									this.faceIndex[i] = this.faceArray.Length - 1;
								}
								maid.FaceAnime(this.faceArray[this.faceIndex[i]], 1f, 0);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (maid.boMabataki && Input.GetKey(key) && Input.GetKeyDown(KeyCode.Alpha8))
							{
								this.faceIndex[i]++;
								if (this.faceIndex[i] == this.faceArray.Length)
								{
									this.faceIndex[i] = 0;
								}
								maid.FaceAnime(this.faceArray[this.faceIndex[i]], 1f, 0);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Alpha9))
							{
								this.faceBlendIndex[i]++;
								if (this.faceBlendIndex[i] == this.faceBlendArray.Length)
								{
									this.faceBlendIndex[i] = 0;
								}
								maid.FaceBlend(this.faceBlendArray[this.faceBlendIndex[i]]);
								this.idoFlg[i] = true;
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Space))
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								if (array[0].Contains("_momi") || array[0].Contains("paizuri_"))
								{
									maid.body0.MuneYureL(0f);
									maid.body0.MuneYureR(0f);
								}
								else
								{
									maid.body0.MuneYureL(1f);
									maid.body0.MuneYureR(1f);
									maid.body0.jbMuneL.enabled = true;
									maid.body0.jbMuneR.enabled = true;
								}
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
								}
								if (array.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
								this.isLock[i] = false;
								this.idoFlg[i] = true;
								this.mHandL[i].initFlg = false;
								this.mHandR[i].initFlg = false;
								this.mFootL[i].initFlg = false;
								this.mFootR[i].initFlg = false;
								this.pHandL[i] = 0;
								this.pHandR[i] = 0;
								this.muneIKL[i] = false;
								this.muneIKR[i] = false;
								if (!this.isVR)
								{
									this.maidArray[i].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[i]];
									this.maidArray[i].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[i]];
								}
								if (i >= 7)
								{
									this.idoFlg[i - 7] = true;
								}
							}
							else if (maid.body0.isLoadedBody)
							{
								Transform transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Calf", true);
								Transform transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Calf", true);
								float num7 = (transform3.position - transform4.position).magnitude / 1.8f;
								num7 -= 0.1f;
								if (num7 < 0f)
								{
								}
								float num8 = transform4.localEulerAngles.y;
								float num9 = transform3.localEulerAngles.y;
								if (num8 < 100f)
								{
									num8 = 360f;
								}
								if (num9 > 260f)
								{
									num9 = 0f;
								}
								float num10 = num8 - num9;
								num10 /= 300f;
								if (this.muneIKL[i] && this.vIKMuneL[i].x != 0f)
								{
									this.IKMuneLSub[i].localEulerAngles = this.vIKMuneLSub[i];
									this.IKMuneL[i].localEulerAngles = this.vIKMuneL[i];
								}
								if (this.muneIKR[i] && this.vIKMuneR[i].x != 0f)
								{
									this.IKMuneRSub[i].localEulerAngles = this.vIKMuneRSub[i];
									this.IKMuneR[i].localEulerAngles = this.vIKMuneR[i];
								}
								if (!this.HandL1[i])
								{
									this.SetIKInit(i);
									this.SetIK(maid, i);
									this.ikBui = 5;
								}
								else
								{
									bool flag5 = false;
									for (int k = 0; k < this.keyArray.Length; k++)
									{
										if (Input.GetKey(this.keyArray[k]))
										{
											flag5 = true;
											break;
										}
									}
									if ((!Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W)) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Y))
									{
										flag5 = true;
									}
									if (!flag5)
									{
										if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && this.getModKeyPressing(MultipleMaids.modKey.Alt) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
										{
											this.ikMode[i] = 15;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Shift) && this.getModKeyPressing(MultipleMaids.modKey.Alt))
										{
											this.ikMode[i] = 5;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Shift) && Input.GetKey(KeyCode.Space))
										{
											this.ikMode[i] = 6;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && Input.GetKey(KeyCode.Space))
										{
											this.ikMode[i] = 7;
										}
										else if (Input.GetKey(KeyCode.Z) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
										{
											this.ikMode[i] = 11;
										}
										else if (Input.GetKey(KeyCode.Z) && this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
										{
											this.ikMode[i] = 12;
										}
										else if (Input.GetKey(KeyCode.X) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
										{
											this.ikMode[i] = 14;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
										{
											this.ikMode[i] = 1;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && this.getModKeyPressing(MultipleMaids.modKey.Alt))
										{
											this.ikMode[i] = 8;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
										{
											this.ikMode[i] = 2;
										}
										else if (this.getModKeyPressing(MultipleMaids.modKey.Alt))
										{
											this.ikMode[i] = 3;
										}
										else if (Input.GetKey(KeyCode.Space))
										{
											this.ikMode[i] = 4;
										}
										else if (Input.GetKey(KeyCode.X))
										{
											this.ikMode[i] = 9;
										}
										else if (Input.GetKey(KeyCode.Z))
										{
											this.ikMode[i] = 10;
										}
										else if (Input.GetKey(KeyCode.C))
										{
											this.ikMode[i] = 13;
										}
										else if (Input.GetKey(KeyCode.A))
										{
											this.ikMode[i] = 16;
										}
										else
										{
											this.ikMode[i] = 0;
										}
										if (!this.isIK[i])
										{
											if (this.ikMode[i] < 9)
											{
												this.ikMode[i] = 0;
											}
										}
										bool flag8 = false;
										bool flag9 = false;
										bool flag10 = false;
										bool flag11 = false;
										if (this.gFinger[i, 0])
										{
											for (int j = 0; j < 15; j++)
											{
												if (this.mFinger[i, j].isStop)
												{
													flag8 = true;
												}
											}
											for (int j = 15; j < 30; j++)
											{
												if (this.mFinger[i, j].isStop)
												{
													flag9 = true;
												}
											}
											for (int j = 0; j < 6; j++)
											{
												if (this.mFinger2[i, j].isStop)
												{
													flag10 = true;
												}
											}
											for (int j = 6; j < 12; j++)
											{
												if (this.mFinger2[i, j].isStop)
												{
													flag11 = true;
												}
											}
										}
										if (this.gMaid[i] != null)
										{
											if (this.ikMode[i] >= 9 && this.ikMode[i] <= 14)
											{
												this.gMaid[i].SetActive(true);
												if (this.isCube)
												{
													this.gMaidC[i].SetActive(true);
												}
											}
											else
											{
												this.gMaid[i].SetActive(false);
												this.gMaidC[i].SetActive(false);
											}
										}
										if (flag9 || this.mHandR[i].isSelect || this.mArmR[i].isSelect || this.mClavicleR[i].isSelect || (this.ikMode[i] == 4 && Input.GetKeyDown(KeyCode.Q)))
										{
											this.ikBui = 1;
											if (this.ikMode[i] != 4 || !Input.GetKeyDown(KeyCode.Q))
											{
												this.ikMaid = i;
											}
											if (this.ikMaid == i)
											{
												this.SetIK(maid, i);
												this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Hand", true);
												this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R UpperArm", true);
												this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Forearm", true);
												this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Clavicle", true);
												this.IK_hand = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
											}
											if (Input.GetKeyDown(KeyCode.Q))
											{
												this.qFlg = true;
											}
										}
										else if (flag8 || this.mHandL[i].isSelect || this.mArmL[i].isSelect || this.mClavicleL[i].isSelect || (this.ikMode[i] == 4 && Input.GetKeyDown(KeyCode.W)))
										{
											this.ikBui = 2;
											if (this.ikMode[i] != 4 || !Input.GetKeyDown(KeyCode.W))
											{
												this.ikMaid = i;
											}
											if (this.ikMaid == i)
											{
												this.SetIK(maid, i);
												this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Hand", true);
												this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L UpperArm", true);
												this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Forearm", true);
												this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Clavicle", true);
												this.IK_hand = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
											}
										}
										else if (flag11 || this.mFootR[i].isSelect || this.mHizaR[i].isSelect || (this.ikMode[i] == 4 && Input.GetKeyDown(KeyCode.A)))
										{
											this.ikBui = 3;
											if (this.ikMode[i] != 4 || !Input.GetKeyDown(KeyCode.A))
											{
												this.ikMaid = i;
											}
											if (this.ikMaid == i)
											{
												this.SetIK(maid, i);
												this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Foot", true);
												this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Thigh", true);
												this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Calf", true);
												this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_L", true);
											}
										}
										else if (flag10 || this.mFootL[i].isSelect || this.mHizaL[i].isSelect || (this.ikMode[i] == 4 && Input.GetKeyDown(KeyCode.S)))
										{
											this.ikBui = 4;
											if (this.ikMode[i] != 4 || !Input.GetKeyDown(KeyCode.S))
											{
												this.ikMaid = i;
											}
											if (this.ikMaid == i)
											{
												this.SetIK(maid, i);
												this.HandL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Foot", true);
												this.UpperArmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Thigh", true);
												this.ForearmL = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Calf", true);
												this.Clavicle = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_L", true);
											}
											if (Input.GetKeyDown(KeyCode.S))
											{
												this.sFlg = true;
											}
										}
										this.mHandR[i].isSelect = false;
										this.mArmR[i].isSelect = false;
										this.mHandL[i].isSelect = false;
										this.mArmL[i].isSelect = false;
										this.mFootR[i].isSelect = false;
										this.mHizaR[i].isSelect = false;
										this.mFootL[i].isSelect = false;
										this.mHizaL[i].isSelect = false;
										this.mClavicleL[i].isSelect = false;
										this.mClavicleR[i].isSelect = false;
										if (this.ikMode[i] == 16)
										{
											if (!this.gHead2[i])
											{
												this.SetIKInit6(i);
											}
											if (this.mHead2[i].isClick)
											{
												this.mHead2[i].isClick = false;
												this.bGui = true;
												this.isFaceInit = true;
												this.isGuiInit = true;
												this.faceFlg = true;
												this.poseFlg = false;
												this.sceneFlg = false;
												this.kankyoFlg = false;
												this.kankyo2Flg = false;
												maid.boMabataki = false;
												this.selectMaidIndex = this.mHead2[i].no;
												this.faceCombo.selectedItemIndex = this.faceIndex[this.selectMaidIndex];
											}
											if (this.mMaid2[i].isClick)
											{
												this.mMaid2[i].isClick = false;
												this.bGui = true;
												this.isPoseInit = true;
												this.isGuiInit = true;
												this.poseFlg = true;
												this.faceFlg = false;
												this.sceneFlg = false;
												this.kankyoFlg = false;
												this.kankyo2Flg = false;
												this.selectMaidIndex = this.mMaid2[i].no;
												this.copyIndex = 0;
											}
											this.gHead2[i].transform.position = new Vector3(this.Head2[i].position.x, (this.Head2[i].position.y * 1.2f + this.Head3[i].position.y * 0.8f) / 2f, this.Head2[i].position.z);
											this.gHead2[i].transform.eulerAngles = new Vector3(this.Head2[i].transform.eulerAngles.x, this.Head2[i].transform.eulerAngles.y, this.Head2[i].transform.eulerAngles.z + 90f);
											this.mHead2[i].no = i;
											this.mHead2[i].maid = maid;
											this.mHead2[i].ido = 1;
											this.gMaid2[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
											this.gMaid2[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
											this.mMaid2[i].no = i;
											this.mMaid2[i].maid = maid;
											this.mMaid2[i].ido = 2;
											this.gArmL[i].SetActive(false);
											this.gFootL[i].SetActive(false);
											this.gHizaL[i].SetActive(false);
											this.gHandR[i].SetActive(false);
											this.gArmR[i].SetActive(false);
											this.gFootR[i].SetActive(false);
											this.gHizaR[i].SetActive(false);
											this.gClavicleL[i].SetActive(false);
											this.gClavicleR[i].SetActive(false);
											this.gNeck[i].SetActive(false);
											this.gSpine[i].SetActive(false);
											this.gSpine0a[i].SetActive(false);
											this.gSpine1a[i].SetActive(false);
											this.gSpine1[i].SetActive(false);
											this.gPelvis[i].SetActive(false);
										}
										else if (this.ikMode[i] == 8)
										{
											if (this.ikModeOld[i] == 15 && this.gIKHandL[i])
											{
												this.mIKHandL[i].ido = 1;
												this.mIKHandL[i].reset = true;
												this.mIKHandR[i].ido = 1;
												this.mIKHandR[i].reset = true;
											}
											else if (this.ikModeOld[i] == 3 && this.gHead[i])
											{
												this.mHead[i].ido = 7;
												this.mHead[i].reset = true;
											}
											else
											{
												if (!this.gIKHandL[i])
												{
													this.SetIKInit5(i);
												}
												this.gIKHandL[i].transform.position = this.IKHandL[i].position;
												this.gIKHandL[i].transform.eulerAngles = this.IKHandL[i].eulerAngles;
												this.mIKHandL[i].maid = maid;
												this.mIKHandL[i].HandL = this.IKHandL[i];
												this.mIKHandL[i].ido = 1;
												this.gIKHandR[i].transform.position = this.IKHandR[i].position;
												this.gIKHandR[i].transform.eulerAngles = this.IKHandR[i].eulerAngles;
												this.mIKHandR[i].maid = maid;
												this.mIKHandR[i].HandL = this.IKHandR[i];
												this.mIKHandR[i].ido = 1;
												if (!this.gIKMuneL[i])
												{
													this.SetIKInit7(i);
												}
												if (!this.gHead[i])
												{
													this.SetIKInit4(i);
												}
												this.mIKMuneL[i].maid = maid;
												this.mIKMuneL[i].HandL = this.IKMuneLSub[i];
												this.mIKMuneL[i].UpperArmL = this.IKMuneL[i];
												this.mIKMuneL[i].ForearmL = this.IKMuneL[i];
												this.gIKMuneL[i].transform.position = (this.IKMuneL[i].position + this.IKMuneLSub[i].position) / 2f;
												this.mIKMuneR[i].maid = maid;
												this.mIKMuneR[i].HandL = this.IKMuneRSub[i];
												this.mIKMuneR[i].UpperArmL = this.IKMuneR[i];
												this.mIKMuneR[i].ForearmL = this.IKMuneR[i];
												this.gIKMuneR[i].transform.position = (this.IKMuneR[i].position + this.IKMuneRSub[i].position) / 2f;
												this.gHead[i].transform.position = new Vector3(this.Head2[i].position.x, (this.Head2[i].position.y * 1.2f + this.Head3[i].position.y * 0.8f) / 2f, this.Head2[i].position.z);
												this.gHead[i].transform.eulerAngles = new Vector3(this.Head2[i].transform.eulerAngles.x, this.Head2[i].transform.eulerAngles.y, this.Head2[i].transform.eulerAngles.z + 90f);
												this.mHead[i].head = this.Head1[i];
												this.mHead[i].maid = maid;
												this.mHead[i].ido = 7;
												this.mHead[i].shodaiFlg = this.shodaiFlg[(int)this.selectList[i]];
												this.gHead[i].SetActive(true);
												if (this.mHead[i].isClick)
												{
													this.mHead[i].isClick = false;
													this.mHead[i].isClick2 = false;
													maid.body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[i]];
													maid.body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[i]];
												}
												if (this.mIKMuneL[i].isMouseUp && this.mIKMuneL[i].isMouseDrag)
												{
													this.IKMuneLSub[i].localEulerAngles = this.mIKMuneL[i].HandLangles;
													this.IKMuneL[i].localEulerAngles = this.mIKMuneL[i].UpperArmLangles;
													this.vIKMuneLSub[i] = new Vector3(this.IKMuneLSub[i].localEulerAngles.x, this.IKMuneLSub[i].localEulerAngles.y, this.IKMuneLSub[i].localEulerAngles.z);
													this.vIKMuneL[i] = new Vector3(this.IKMuneL[i].localEulerAngles.x, this.IKMuneL[i].localEulerAngles.y, this.IKMuneL[i].localEulerAngles.z);
													this.muneIKL[i] = true;
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mIKMuneL[i].isMouseDrag = false;
												}
												if (this.mIKMuneL[i].isMouseDown)
												{
													maid.body0.MuneYureL(0f);
													maid.body0.MuneYureR(0f);
													maid.body0.jbMuneL.enabled = false;
													maid.body0.jbMuneR.enabled = false;
													this.muneIKL[i] = false;
													this.mIKMuneL[i].isMouseUp = false;
												}
												if (this.mIKMuneR[i].isMouseUp && this.mIKMuneR[i].isMouseDrag)
												{
													this.IKMuneRSub[i].localEulerAngles = this.mIKMuneR[i].HandLangles;
													this.IKMuneR[i].localEulerAngles = this.mIKMuneR[i].UpperArmLangles;
													this.vIKMuneRSub[i] = new Vector3(this.IKMuneRSub[i].localEulerAngles.x, this.IKMuneRSub[i].localEulerAngles.y, this.IKMuneRSub[i].localEulerAngles.z);
													this.vIKMuneR[i] = new Vector3(this.IKMuneR[i].localEulerAngles.x, this.IKMuneR[i].localEulerAngles.y, this.IKMuneR[i].localEulerAngles.z);
													this.muneIKR[i] = true;
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mIKMuneR[i].isMouseDrag = false;
												}
												if (this.mIKMuneR[i].isMouseDown)
												{
													maid.body0.MuneYureL(0f);
													maid.body0.MuneYureR(0f);
													maid.body0.jbMuneL.enabled = false;
													maid.body0.jbMuneR.enabled = false;
													this.muneIKR[i] = false;
													this.mIKMuneL[i].isMouseUp = false;
												}
												if (this.mHead[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mHead[i].isStop = false;
												}
												this.gJotai[i].SetActive(false);
												this.gKahuku[i].SetActive(false);
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
										}
										else if (this.ikMode[i] == 15)
										{
											if (this.ikModeOld[i] == 8 && this.gIKHandL[i])
											{
												this.mIKHandL[i].ido = 2;
												this.mIKHandL[i].reset = true;
												this.mIKHandR[i].ido = 2;
												this.mIKHandR[i].reset = true;
											}
											else
											{
												if (!this.gIKHandL[i])
												{
													this.SetIKInit5(i);
												}
												this.gIKHandL[i].transform.position = this.IKHandL[i].position;
												this.gIKHandL[i].transform.eulerAngles = this.IKHandL[i].eulerAngles;
												this.mIKHandL[i].maid = maid;
												this.mIKHandL[i].HandL = this.IKHandL[i];
												this.mIKHandL[i].ido = 2;
												this.gIKHandR[i].transform.position = this.IKHandR[i].position;
												this.gIKHandR[i].transform.eulerAngles = this.IKHandR[i].eulerAngles;
												this.mIKHandR[i].maid = maid;
												this.mIKHandR[i].HandL = this.IKHandR[i];
												this.mIKHandR[i].ido = 2;
												Object.Destroy(this.gHead[i]);
												Object.Destroy(this.gJotai[i]);
												Object.Destroy(this.gKahuku[i]);
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
										}
										else if (this.ikMode[i] == 1)
										{
											this.mHandL[i].ido = 10;
											this.mHandR[i].ido = 10;
											this.mFootL[i].ido = 10;
											this.mFootR[i].ido = 10;
											if (this.ikModeOld[i] == 0 && this.gNeck[i])
											{
												if (this.isBone[i])
												{
													this.mNeck[i].ido = 4;
													this.mNeck[i].reset = true;
													this.mSpine[i].ido = 4;
													this.mSpine[i].reset = true;
													this.mSpine0a[i].ido = 4;
													this.mSpine0a[i].reset = true;
													this.mSpine1a[i].ido = 4;
													this.mSpine1a[i].reset = true;
													this.mSpine1[i].ido = 4;
													this.mSpine1[i].reset = true;
													this.mPelvis[i].ido = 4;
													this.mPelvis[i].reset = true;
													this.mHizaL[i].ido = 5;
													this.mHizaL[i].reset = true;
													this.mHizaR[i].ido = 5;
													this.mHizaR[i].reset = true;
												}
											}
											else
											{
												this.gArmL[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												if (!this.isBone[i])
												{
													this.gHizaL[i].SetActive(false);
													this.gHizaR[i].SetActive(false);
												}
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												if (this.isBone[i])
												{
													this.mNeck[i].maid = maid;
													this.mNeck[i].head = this.Neck[i];
													this.mNeck[i].no = i;
													this.mNeck[i].ido = 4;
													this.gNeck[i].transform.position = this.Neck[i].position;
													this.gNeck[i].transform.localEulerAngles = this.Neck[i].localEulerAngles;
													if (this.mNeck[i].isHead)
													{
														this.mNeck[i].isHead = false;
														this.isLookn[i] = this.isLook[i];
														this.isLook[i] = !this.isLook[i];
													}
													this.mSpine[i].maid = maid;
													this.mSpine[i].head = this.Spine2[i];
													this.mSpine[i].no = i;
													this.mSpine[i].ido = 4;
													this.gSpine[i].transform.position = this.Spine2[i].position;
													this.gSpine[i].transform.localEulerAngles = this.Spine2[i].localEulerAngles;
													this.mSpine0a[i].maid = maid;
													this.mSpine0a[i].head = this.Spine0a2[i];
													this.mSpine0a[i].no = i;
													this.mSpine0a[i].ido = 4;
													this.gSpine0a[i].transform.position = this.Spine0a2[i].position;
													this.gSpine0a[i].transform.localEulerAngles = this.Spine0a2[i].localEulerAngles;
													this.mSpine1a[i].maid = maid;
													this.mSpine1a[i].head = this.Spine1a2[i];
													this.mSpine1a[i].no = i;
													this.mSpine1a[i].ido = 4;
													this.gSpine1a[i].transform.position = this.Spine1a2[i].position;
													this.gSpine1a[i].transform.localEulerAngles = this.Spine1a2[i].localEulerAngles;
													this.mSpine1[i].maid = maid;
													this.mSpine1[i].head = this.Spine12[i];
													this.mSpine1[i].no = i;
													this.mSpine1[i].ido = 4;
													this.gSpine1[i].transform.position = this.Spine12[i].position;
													this.gSpine1[i].transform.localEulerAngles = this.Spine12[i].localEulerAngles;
													this.mPelvis[i].maid = maid;
													this.mPelvis[i].head = this.Pelvis2[i];
													this.mPelvis[i].no = i;
													this.mPelvis[i].ido = 8;
													this.gPelvis[i].transform.position = this.Pelvis2[i].position;
													this.gPelvis[i].transform.localEulerAngles = this.Pelvis2[i].localEulerAngles;
													this.mHizaL[i].ido = 5;
													this.mHizaR[i].ido = 5;
												}
											}
										}
										else if (this.ikMode[i] == 2)
										{
											if (this.ikModeOld[i] == 0 && this.gPelvis[i])
											{
												this.mPelvis[i].ido = 9;
												this.mPelvis[i].reset = true;
											}
											else
											{
												this.gArmL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.mPelvis[i].maid = maid;
												this.mPelvis[i].head = this.Pelvis2[i];
												this.mPelvis[i].no = i;
												this.mPelvis[i].ido = 9;
												this.gPelvis[i].transform.position = this.Pelvis2[i].position;
												this.gPelvis[i].transform.localEulerAngles = this.Pelvis2[i].localEulerAngles;
											}
										}
										else if (this.ikMode[i] == 3)
										{
											if ((this.ikModeOld[i] == 5 || this.ikModeOld[i] == 8) && this.gHead[i])
											{
												this.mHead[i].ido = 1;
												this.mHead[i].reset = true;
												this.mJotai[i].ido = 2;
												this.mJotai[i].reset = true;
												this.mKahuku[i].ido = 3;
												this.mKahuku[i].reset = true;
												this.mHandL[i].ido = 1;
												this.mHandR[i].ido = 1;
												this.mFootL[i].ido = 3;
												this.mFootR[i].ido = 3;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.mHandL[i].reset = true;
												this.mHandR[i].reset = true;
												this.mFootL[i].reset = true;
												this.mFootR[i].reset = true;
												this.mHizaL[i].reset = true;
												this.mHizaR[i].reset = true;
											}
											else if (this.ikModeOld[i] == 0)
											{
												this.mHandL[i].ido = 1;
												this.mHandR[i].ido = 1;
												this.mFootL[i].ido = 3;
												this.mFootR[i].ido = 3;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.mHandL[i].reset = true;
												this.mHandR[i].reset = true;
												this.mFootL[i].reset = true;
												this.mFootR[i].reset = true;
												this.mHizaL[i].reset = true;
												this.mHizaR[i].reset = true;
											}
											else
											{
												if (!this.gHead[i])
												{
													this.SetIKInit4(i);
												}
												if (this.mHead[i].isHead)
												{
													this.mHead[i].isHead = false;
													this.isLookn[i] = this.isLook[i];
													this.isLook[i] = !this.isLook[i];
												}
												this.gHead[i].transform.position = new Vector3(this.Head2[i].position.x, (this.Head2[i].position.y * 1.2f + this.Head3[i].position.y * 0.8f) / 2f, this.Head2[i].position.z);
												this.gHead[i].transform.eulerAngles = new Vector3(this.Head2[i].transform.eulerAngles.x, this.Head2[i].transform.eulerAngles.y, this.Head2[i].transform.eulerAngles.z + 90f);
												this.mHead[i].head = this.Head1[i];
												this.mHead[i].maid = maid;
												this.mHead[i].no = i;
												this.mHead[i].ido = 1;
												Transform spine0a = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine0a", true);
												Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1", true);
												Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1a", true);
												Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine", true);
												Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Pelvis", true);
												this.gJotai[i].transform.position = new Vector3(transform5.position.x, (transform5.position.y * 0f + transform6.position.y * 2f) / 2f, transform5.position.z);
												this.gJotai[i].transform.eulerAngles = new Vector3(transform5.transform.eulerAngles.x, transform5.transform.eulerAngles.y, transform5.transform.eulerAngles.z + 90f);
												this.mJotai[i].Spine0a = spine0a;
												this.mJotai[i].Spine1 = transform5;
												this.mJotai[i].Spine1a = transform6;
												this.mJotai[i].Spine = transform7;
												this.mJotai[i].maid = maid;
												this.mJotai[i].ido = 2;
												this.gKahuku[i].transform.position = new Vector3(transform8.position.x, (transform8.position.y + transform7.position.y) / 2f, transform8.position.z);
												this.gKahuku[i].transform.eulerAngles = new Vector3(transform8.transform.eulerAngles.x + 90f, transform8.transform.eulerAngles.y + 90f, transform8.transform.eulerAngles.z);
												this.mKahuku[i].Pelvis = transform8;
												this.mKahuku[i].maid = maid;
												this.mKahuku[i].ido = 3;
												this.mHandL[i].ido = 1;
												this.mHandR[i].ido = 1;
												this.mFootL[i].ido = 3;
												this.mFootR[i].ido = 3;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.gJotai[i].SetActive(true);
												this.gKahuku[i].SetActive(true);
												this.gHandL[i].SetActive(true);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(true);
												this.gHizaL[i].SetActive(true);
												this.gHandR[i].SetActive(true);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(true);
												this.gHizaR[i].SetActive(true);
												Object.Destroy(this.gIKHandL[i]);
												Object.Destroy(this.gIKHandR[i]);
												Object.Destroy(this.gIKMuneL[i]);
												Object.Destroy(this.gIKMuneR[i]);
												if (this.isBone[i])
												{
													this.gHizaL[i].SetActive(false);
													this.gHizaR[i].SetActive(false);
													this.gHead[i].SetActive(false);
													this.gJotai[i].SetActive(false);
													this.gKahuku[i].SetActive(false);
												}
												else
												{
													this.gHizaL[i].SetActive(true);
													this.gHizaR[i].SetActive(true);
													this.gHead[i].SetActive(true);
													this.gJotai[i].SetActive(true);
													this.gKahuku[i].SetActive(true);
												}
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
												if (this.mHead[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mHead[i].isStop = false;
												}
												if (this.mJotai[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mJotai[i].isStop = false;
												}
												if (this.mKahuku[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mKahuku[i].isStop = false;
												}
												if (this.mKahuku[i].isSelect)
												{
													this.mKahuku[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
												if (this.mJotai[i].isSelect)
												{
													this.mJotai[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
												if (this.mKahuku[i].isSelect)
												{
													this.mKahuku[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
											}
										}
										else if (this.ikMode[i] == 5)
										{
											if (this.ikModeOld[i] == 3 && this.gHead[i])
											{
												this.mHead[i].ido = 4;
												this.mHead[i].reset = true;
												this.mJotai[i].ido = 5;
												this.mJotai[i].reset = true;
												this.mKahuku[i].ido = 6;
												this.mKahuku[i].reset = true;
												this.mHandL[i].ido = 2;
												this.mHandR[i].ido = 2;
												this.mFootL[i].ido = 4;
												this.mFootR[i].ido = 4;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.mHandL[i].reset = true;
												this.mHandR[i].reset = true;
												this.mFootL[i].reset = true;
												this.mFootR[i].reset = true;
												this.mHizaL[i].reset = true;
												this.mHizaR[i].reset = true;
											}
											else if (this.ikModeOld[i] == 0)
											{
												this.mHandL[i].ido = 2;
												this.mHandR[i].ido = 2;
												this.mFootL[i].ido = 4;
												this.mFootR[i].ido = 4;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.mHandL[i].reset = true;
												this.mHandR[i].reset = true;
												this.mFootL[i].reset = true;
												this.mFootR[i].reset = true;
												this.mHizaL[i].reset = true;
												this.mHizaR[i].reset = true;
											}
											else
											{
												if (!this.gHead[i])
												{
													this.SetIKInit4(i);
												}
												if (this.mHead[i].isHead)
												{
													this.mHead[i].isHead = false;
													this.isLookn[i] = this.isLook[i];
													this.isLook[i] = !this.isLook[i];
												}
												this.gHead[i].transform.position = new Vector3(this.Head2[i].position.x, (this.Head2[i].position.y * 1.2f + this.Head3[i].position.y * 0.8f) / 2f, this.Head2[i].position.z);
												this.gHead[i].transform.eulerAngles = new Vector3(this.Head2[i].transform.eulerAngles.x, this.Head2[i].transform.eulerAngles.y, this.Head2[i].transform.eulerAngles.z + 90f);
												this.mHead[i].head = this.Head1[i];
												this.mHead[i].maid = maid;
												this.mHead[i].ido = 4;
												Transform spine0a = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine0a", true);
												Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1", true);
												Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1a", true);
												Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine", true);
												Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Pelvis", true);
												this.gJotai[i].transform.position = new Vector3(transform5.position.x, (transform5.position.y * 0f + transform6.position.y * 2f) / 2f, transform5.position.z);
												this.gJotai[i].transform.eulerAngles = new Vector3(transform5.transform.eulerAngles.x, transform5.transform.eulerAngles.y, transform5.transform.eulerAngles.z + 90f);
												this.mJotai[i].Spine0a = spine0a;
												this.mJotai[i].Spine1 = transform5;
												this.mJotai[i].Spine1a = transform6;
												this.mJotai[i].Spine = transform7;
												this.mJotai[i].maid = maid;
												this.mJotai[i].ido = 5;
												this.gKahuku[i].transform.position = new Vector3(transform8.position.x, (transform8.position.y + transform7.position.y) / 2f, transform8.position.z);
												this.gKahuku[i].transform.eulerAngles = new Vector3(transform8.transform.eulerAngles.x + 90f, transform8.transform.eulerAngles.y + 90f, transform8.transform.eulerAngles.z);
												this.mKahuku[i].Pelvis = transform8;
												this.mKahuku[i].maid = maid;
												this.mKahuku[i].ido = 6;
												this.mHandL[i].ido = 2;
												this.mHandR[i].ido = 2;
												this.mFootL[i].ido = 4;
												this.mFootR[i].ido = 4;
												this.mHizaL[i].ido = 5;
												this.mHizaR[i].ido = 5;
												this.gArmL[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
												if (this.isBone[i])
												{
													this.gHizaL[i].SetActive(false);
													this.gHizaR[i].SetActive(false);
													this.gHead[i].SetActive(false);
													this.gJotai[i].SetActive(false);
													this.gKahuku[i].SetActive(false);
												}
												else
												{
													this.gHizaL[i].SetActive(true);
													this.gHizaR[i].SetActive(true);
													this.gHead[i].SetActive(true);
													this.gJotai[i].SetActive(true);
													this.gKahuku[i].SetActive(true);
												}
												if (this.mHead[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mHead[i].isStop = false;
												}
												if (this.mJotai[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mJotai[i].isStop = false;
												}
												if (this.mKahuku[i].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mKahuku[i].isStop = false;
												}
												if (this.mHead[i].isSelect)
												{
													this.mHead[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
												if (this.mJotai[i].isSelect)
												{
													this.mJotai[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
												if (this.mKahuku[i].isSelect)
												{
													this.mKahuku[i].isSelect = false;
													if (this.ikMaid != i)
													{
														this.ikMaid = i;
														this.ikBui = 5;
														this.SetIK(maid, i);
													}
												}
											}
										}
										else if (this.ikMode[i] == 13)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 13 && this.gMaid[i])
											{
												this.mMaid[i].ido = 5;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 5;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 5;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 5;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 11)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 11 && this.gMaid[i])
											{
												this.mMaid[i].ido = 3;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 3;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 3;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 3;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 12)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 12 && this.gMaid[i])
											{
												this.mMaid[i].ido = 2;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 2;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 2;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 2;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 10)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 10 && this.gMaid[i])
											{
												this.mMaid[i].ido = 1;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 1;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 1;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 1;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 9)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 9 && this.gMaid[i])
											{
												this.mMaid[i].ido = 4;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 4;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 4;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 4;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 14)
										{
											if ((this.ikModeOld[i] == 0 || this.ikModeOld[i] >= 9) && this.ikModeOld[i] != 14 && this.gMaid[i])
											{
												this.mMaid[i].ido = 6;
												this.mMaid[i].reset = true;
												this.mMaidC[i].ido = 6;
												this.mMaidC[i].reset = true;
												this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f);
											}
											else
											{
												if (!this.gMaid[i])
												{
													this.SetIKInit3(i);
												}
												this.gMaid[i].transform.position = new Vector3((this.Pelvis2[i].position.x + this.Spine0a2[i].position.x) / 2f, (this.Spine12[i].position.y + this.Spine0a2[i].position.y) / 2f, (this.Spine0a2[i].position.z + this.Pelvis2[i].position.z) / 2f);
												this.gMaid[i].transform.eulerAngles = new Vector3(this.Spine0a2[i].transform.eulerAngles.x, this.Spine0a2[i].transform.eulerAngles.y, this.Spine0a2[i].transform.eulerAngles.z + 90f);
												this.mMaid[i].maid = maid;
												this.mMaid[i].ido = 6;
												this.gMaidC[i].transform.position = maid.transform.position;
												this.gMaidC[i].transform.eulerAngles = maid.transform.eulerAngles;
												this.mMaidC[i].maid = maid;
												this.mMaidC[i].ido = 6;
												if (this.isCube)
												{
													this.gMaidC[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f);
													this.gMaidC[i].SetActive(true);
												}
												else
												{
													this.gMaidC[i].SetActive(false);
												}
												this.gHandL[i].SetActive(false);
												this.gArmL[i].SetActive(false);
												this.gFootL[i].SetActive(false);
												this.gHizaL[i].SetActive(false);
												this.gHandR[i].SetActive(false);
												this.gArmR[i].SetActive(false);
												this.gFootR[i].SetActive(false);
												this.gHizaR[i].SetActive(false);
												this.gClavicleL[i].SetActive(false);
												this.gClavicleR[i].SetActive(false);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
											}
											if (this.mMaid[i].isSelect)
											{
												this.mMaid[i].isSelect = false;
												if (this.ikMaid != i)
												{
													this.ikMaid = i;
													this.ikBui = 5;
													this.SetIK(maid, i);
												}
											}
										}
										else if (this.ikMode[i] == 6)
										{
											if (!this.gFinger[i, 0])
											{
												this.SetIKInit2(i);
											}
											Object.Destroy(this.gMaid[i]);
											Object.Destroy(this.gMaidC[i]);
											Object.Destroy(this.gHead[i]);
											Object.Destroy(this.gJotai[i]);
											Object.Destroy(this.gKahuku[i]);
											this.gHandL[i].SetActive(false);
											this.gArmL[i].SetActive(false);
											this.gFootL[i].SetActive(false);
											this.gHizaL[i].SetActive(false);
											this.gHandR[i].SetActive(false);
											this.gArmR[i].SetActive(false);
											this.gFootR[i].SetActive(false);
											this.gHizaR[i].SetActive(false);
											this.gClavicleL[i].SetActive(false);
											this.gClavicleR[i].SetActive(false);
											this.gNeck[i].SetActive(false);
											this.gSpine[i].SetActive(false);
											this.gSpine0a[i].SetActive(false);
											this.gSpine1a[i].SetActive(false);
											this.gSpine1[i].SetActive(false);
											this.gPelvis[i].SetActive(false);
											for (int j = 0; j < 10; j++)
											{
												this.mFinger[i, j * 3 + 2].maid = maid;
												this.mFinger[i, j * 3 + 2].HandL = this.Finger[i, j * 4 + 3];
												this.mFinger[i, j * 3 + 2].UpperArmL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 2].ForearmL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 2].ido = 12;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3 + 2].ido = 15;
												}
												this.mFinger[i, j * 3 + 1].maid = maid;
												this.mFinger[i, j * 3 + 1].HandL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 1].UpperArmL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3 + 1].ForearmL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3 + 1].ido = 11;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3 + 1].ido = 14;
												}
												this.mFinger[i, j * 3].maid = maid;
												this.mFinger[i, j * 3].HandL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3].UpperArmL = this.Finger[i, j * 4];
												this.mFinger[i, j * 3].ForearmL = this.Finger[i, j * 4];
												this.mFinger[i, j * 3].ido = 13;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3].ido = 0;
												}
												this.gFinger[i, j * 3 + 2].SetActive(false);
												this.gFinger[i, j * 3 + 1].SetActive(false);
												this.gFinger[i, j * 3].SetActive(true);
												this.gFinger[i, j * 3].SetActive(true);
												this.mFinger[i, j * 3].ido = 16;
												if (this.ikModeOld[i] != 6)
												{
													this.mFinger[i, j * 3].reset = true;
												}
												this.gFinger[i, j * 3 + 2].transform.position = (this.Finger[i, j * 4 + 3].position + this.Finger[i, j * 4 + 3].position + this.Finger[i, j * 4 + 2].position) / 3f;
												this.gFinger[i, j * 3 + 1].transform.position = this.Finger[i, j * 4 + 2].position;
												this.gFinger[i, j * 3].transform.position = this.Finger[i, j * 4 + 1].position;
												if (this.mFinger[i, j * 3 + 2].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3 + 2].isStop = false;
												}
												if (this.mFinger[i, j * 3 + 1].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3 + 1].isStop = false;
												}
												if (this.mFinger[i, j * 3].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3].isStop = false;
												}
											}
											for (int j = 0; j < 6; j++)
											{
												this.mFinger2[i, j * 2 + 1].maid = maid;
												this.mFinger2[i, j * 2 + 1].HandL = this.Finger2[i, j * 3 + 2];
												this.mFinger2[i, j * 2 + 1].UpperArmL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2 + 1].ForearmL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2 + 1].ido = 0;
												this.mFinger2[i, j * 2].maid = maid;
												this.mFinger2[i, j * 2].HandL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2].UpperArmL = this.Finger2[i, j * 3];
												this.mFinger2[i, j * 2].ForearmL = this.Finger2[i, j * 3];
												this.mFinger2[i, j * 2].ido = 0;
												this.gFinger2[i, j * 2 + 1].SetActive(false);
												this.gFinger2[i, j * 2].SetActive(false);
												this.gFinger2[i, j * 2 + 1].transform.position = this.Finger2[i, j * 3 + 2].position;
												this.gFinger2[i, j * 2].transform.position = this.Finger2[i, j * 3 + 1].position;
												if (this.mFinger2[i, j * 2 + 1].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger2[i, j * 2 + 1].isStop = false;
												}
												if (this.mFinger2[i, j * 2].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger2[i, j * 2].isStop = false;
												}
											}
										}
										else if (this.ikMode[i] == 4)
										{
											if (!this.gFinger[i, 0])
											{
												this.SetIKInit2(i);
											}
											Object.Destroy(this.gMaid[i]);
											Object.Destroy(this.gMaidC[i]);
											Object.Destroy(this.gHead[i]);
											Object.Destroy(this.gJotai[i]);
											Object.Destroy(this.gKahuku[i]);
											this.gHandL[i].SetActive(false);
											this.gArmL[i].SetActive(false);
											this.gFootL[i].SetActive(false);
											this.gHizaL[i].SetActive(false);
											this.gHandR[i].SetActive(false);
											this.gArmR[i].SetActive(false);
											this.gFootR[i].SetActive(false);
											this.gHizaR[i].SetActive(false);
											this.gClavicleL[i].SetActive(false);
											this.gClavicleR[i].SetActive(false);
											this.gNeck[i].SetActive(false);
											this.gSpine[i].SetActive(false);
											this.gSpine0a[i].SetActive(false);
											this.gSpine1a[i].SetActive(false);
											this.gSpine1[i].SetActive(false);
											this.gPelvis[i].SetActive(false);
											for (int j = 0; j < 10; j++)
											{
												this.mFinger[i, j * 3 + 2].maid = maid;
												this.mFinger[i, j * 3 + 2].HandL = this.Finger[i, j * 4 + 3];
												this.mFinger[i, j * 3 + 2].UpperArmL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 2].ForearmL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 2].ido = 12;
												this.mFinger[i, j * 3 + 2].onFlg = true;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3 + 2].ido = 15;
												}
												this.mFinger[i, j * 3 + 1].maid = maid;
												this.mFinger[i, j * 3 + 1].HandL = this.Finger[i, j * 4 + 2];
												this.mFinger[i, j * 3 + 1].UpperArmL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3 + 1].ForearmL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3 + 1].ido = 11;
												this.mFinger[i, j * 3 + 1].onFlg = true;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3 + 1].ido = 14;
												}
												this.mFinger[i, j * 3].maid = maid;
												this.mFinger[i, j * 3].HandL = this.Finger[i, j * 4 + 1];
												this.mFinger[i, j * 3].UpperArmL = this.Finger[i, j * 4];
												this.mFinger[i, j * 3].ForearmL = this.Finger[i, j * 4];
												this.mFinger[i, j * 3].ido = 13;
												this.mFinger[i, j * 3].onFlg = true;
												if (j == 0 || j == 5)
												{
													this.mFinger[i, j * 3].ido = 0;
												}
												if (this.ikModeOld[i] != 4)
												{
													this.mFinger[i, j * 3].reset = true;
												}
												this.gFinger[i, j * 3 + 2].SetActive(true);
												this.gFinger[i, j * 3 + 1].SetActive(true);
												this.gFinger[i, j * 3].SetActive(true);
												this.gFinger[i, j * 3 + 2].transform.position = (this.Finger[i, j * 4 + 3].position + this.Finger[i, j * 4 + 3].position + this.Finger[i, j * 4 + 2].position) / 3f;
												this.gFinger[i, j * 3 + 1].transform.position = this.Finger[i, j * 4 + 2].position;
												this.gFinger[i, j * 3].transform.position = this.Finger[i, j * 4 + 1].position;
												if (this.mFinger[i, j * 3 + 2].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3 + 2].isStop = false;
												}
												if (this.mFinger[i, j * 3 + 1].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3 + 1].isStop = false;
												}
												if (this.mFinger[i, j * 3].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger[i, j * 3].isStop = false;
												}
											}
											for (int j = 0; j < 6; j++)
											{
												this.mFinger2[i, j * 2 + 1].maid = maid;
												this.mFinger2[i, j * 2 + 1].HandL = this.Finger2[i, j * 3 + 2];
												this.mFinger2[i, j * 2 + 1].UpperArmL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2 + 1].ForearmL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2 + 1].ido = 17;
												this.mFinger2[i, j * 2].maid = maid;
												this.mFinger2[i, j * 2].HandL = this.Finger2[i, j * 3 + 1];
												this.mFinger2[i, j * 2].UpperArmL = this.Finger2[i, j * 3];
												this.mFinger2[i, j * 2].ForearmL = this.Finger2[i, j * 3];
												this.mFinger2[i, j * 2].ido = 0;
												this.gFinger2[i, j * 2 + 1].SetActive(true);
												this.gFinger2[i, j * 2].SetActive(true);
												this.gFinger2[i, j * 2 + 1].transform.position = this.Finger2[i, j * 3 + 2].position;
												this.gFinger2[i, j * 2].transform.position = this.Finger2[i, j * 3 + 1].position;
												if (this.mFinger2[i, j * 2 + 1].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger2[i, j * 2 + 1].isStop = false;
												}
												if (this.mFinger2[i, j * 2].isStop)
												{
													this.isStop[i] = true;
													this.isLock[i] = true;
													this.mFinger2[i, j * 2].isStop = false;
												}
											}
										}
										else if (this.ikModeOld[i] == 1 || this.ikModeOld[i] == 3 || this.ikModeOld[i] == 5 || this.ikModeOld[i] == 8)
										{
											this.mHandL[i].ido = 0;
											this.mHandR[i].ido = 0;
											this.mFootL[i].ido = 0;
											this.mFootR[i].ido = 0;
											this.mHizaL[i].ido = 0;
											this.mHizaR[i].ido = 0;
											this.mHandL[i].reset = true;
											this.mHandR[i].reset = true;
											this.mFootL[i].reset = true;
											this.mFootR[i].reset = true;
											this.mHizaL[i].reset = true;
											this.mHizaR[i].reset = true;
										}
										else
										{
											if (this.gFinger[i, 0])
											{
												for (int j = 0; j < 30; j++)
												{
													this.gFinger[i, j].SetActive(false);
												}
												for (int j = 0; j < 12; j++)
												{
													this.gFinger2[i, j].SetActive(false);
												}
											}
											Object.Destroy(this.gHead[i]);
											Object.Destroy(this.gJotai[i]);
											Object.Destroy(this.gKahuku[i]);
											Object.Destroy(this.gIKHandL[i]);
											Object.Destroy(this.gIKHandR[i]);
											Object.Destroy(this.gIKMuneL[i]);
											Object.Destroy(this.gIKMuneR[i]);
											Object.Destroy(this.gHead2[i]);
											Object.Destroy(this.gMaid2[i]);
											this.gHandL[i].SetActive(true);
											this.gArmL[i].SetActive(true);
											this.gFootL[i].SetActive(true);
											this.gHizaL[i].SetActive(true);
											this.gHandR[i].SetActive(true);
											this.gArmR[i].SetActive(true);
											this.gFootR[i].SetActive(true);
											this.gHizaR[i].SetActive(true);
											this.mHandL[i].ido = 0;
											this.mHandR[i].ido = 0;
											this.mFootL[i].ido = 0;
											this.mFootR[i].ido = 0;
											this.gClavicleL[i].SetActive(true);
											this.gClavicleR[i].SetActive(true);
										}
										if (!this.isIK[i])
										{
											this.gHandL[i].SetActive(false);
											this.gArmL[i].SetActive(false);
											this.gFootL[i].SetActive(false);
											this.gHizaL[i].SetActive(false);
											this.gHandR[i].SetActive(false);
											this.gArmR[i].SetActive(false);
											this.gFootR[i].SetActive(false);
											this.gHizaR[i].SetActive(false);
											this.gClavicleL[i].SetActive(false);
											this.gClavicleR[i].SetActive(false);
											this.gNeck[i].SetActive(false);
											this.gSpine[i].SetActive(false);
											this.gSpine0a[i].SetActive(false);
											this.gSpine1a[i].SetActive(false);
											this.gSpine1[i].SetActive(false);
											this.gPelvis[i].SetActive(false);
										}
										if (this.isIK[i])
										{
											this.mHandL[i].maid = maid;
											this.mHandL[i].HandL = this.HandL1[i];
											if (this.ikMode[i] == 2)
											{
												this.mHandL[i].UpperArmL = this.ForearmL1[i];
											}
											else
											{
												this.mHandL[i].UpperArmL = this.UpperArmL1[i];
												if (this.ikModeOld[i] == 2 && this.isLock[i])
												{
													this.mHandL[i].OnMouseDown();
													this.mHandL[i].isSelect = false;
												}
											}
											this.mHandL[i].ForearmL = this.ForearmL1[i];
											this.mArmL[i].maid = maid;
											this.mArmL[i].HandL = this.ForearmL1[i];
											this.mArmL[i].UpperArmL = this.UpperArmL1[i];
											this.mArmL[i].ForearmL = this.UpperArmL1[i];
											this.mArmL[i].onFlg = true;
											if (this.mArmL[i].onFlg2)
											{
												this.mArmL[i].onFlg2 = false;
												this.mHandL[i].initFlg = false;
											}
											if (this.ikMode[i] == 2)
											{
												this.mHandL[i].initFlg = false;
												this.mHandR[i].initFlg = false;
												this.mFootL[i].initFlg = false;
												this.mFootR[i].initFlg = false;
												this.mHandL[i].onFlg = true;
												this.mHandR[i].onFlg = true;
												this.mFootL[i].onFlg = true;
												this.mFootR[i].onFlg = true;
											}
											else
											{
												this.mHandL[i].onFlg = false;
												this.mHandR[i].onFlg = false;
												this.mFootL[i].onFlg = false;
												this.mFootR[i].onFlg = false;
											}
											this.gHandL[i].transform.position = this.HandL1[i].position;
											this.gArmL[i].transform.position = this.ForearmL1[i].position;
											this.mClavicleL[i].maid = maid;
											this.mClavicleL[i].HandL = this.UpperArmL1[i];
											this.mClavicleL[i].UpperArmL = this.ClavicleL1[i];
											this.mClavicleL[i].ForearmL = this.ClavicleL1[i];
											this.gClavicleL[i].transform.position = this.UpperArmL1[i].position;
											if (this.mHandL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mHandL[i].isStop = false;
											}
											if (this.mArmL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mArmL[i].isStop = false;
											}
											if (this.mClavicleL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mClavicleL[i].isStop = false;
											}
											this.mHandR[i].maid = maid;
											this.mHandR[i].HandL = this.HandR1[i];
											if (this.ikMode[i] == 2)
											{
												this.mHandR[i].UpperArmL = this.ForearmR1[i];
											}
											else
											{
												this.mHandR[i].UpperArmL = this.UpperArmR1[i];
												if (this.ikModeOld[i] == 2 && this.isLock[i])
												{
													this.mHandR[i].OnMouseDown();
													this.mHandR[i].isSelect = false;
												}
											}
											this.mHandR[i].ForearmL = this.ForearmR1[i];
											this.mArmR[i].maid = maid;
											this.mArmR[i].HandL = this.ForearmR1[i];
											this.mArmR[i].UpperArmL = this.UpperArmR1[i];
											this.mArmR[i].ForearmL = this.UpperArmR1[i];
											this.mArmR[i].onFlg = true;
											if (this.mArmR[i].onFlg2)
											{
												this.mArmR[i].onFlg2 = false;
												this.mHandR[i].initFlg = false;
											}
											this.gHandR[i].transform.position = this.HandR1[i].position;
											this.gArmR[i].transform.position = this.ForearmR1[i].position;
											this.mClavicleR[i].maid = maid;
											this.mClavicleR[i].HandL = this.UpperArmR1[i];
											this.mClavicleR[i].UpperArmL = this.ClavicleR1[i];
											this.mClavicleR[i].ForearmL = this.ClavicleR1[i];
											this.gClavicleR[i].transform.position = this.UpperArmR1[i].position;
											if (this.mHandR[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mHandR[i].isStop = false;
											}
											if (this.mArmR[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mArmR[i].isStop = false;
											}
											if (this.mClavicleL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mClavicleL[i].isStop = false;
											}
											this.mFootL[i].maid = maid;
											this.mFootL[i].HandL = this.HandL2[i];
											if (this.ikMode[i] == 2)
											{
												this.mFootL[i].UpperArmL = this.ForearmL2[i];
											}
											else
											{
												this.mFootL[i].UpperArmL = this.UpperArmL2[i];
												if (this.ikModeOld[i] == 2 && this.isLock[i])
												{
													this.mFootL[i].OnMouseDown();
													this.mFootL[i].isSelect = false;
												}
											}
											this.mFootL[i].ForearmL = this.ForearmL2[i];
											this.mHizaL[i].maid = maid;
											this.mHizaL[i].HandL = this.ForearmL2[i];
											this.mHizaL[i].UpperArmL = this.UpperArmL2[i];
											this.mHizaL[i].ForearmL = this.UpperArmL2[i];
											this.mHizaL[i].onFlg = true;
											if (this.mHizaL[i].onFlg2)
											{
												this.mHizaL[i].onFlg2 = false;
												this.mFootL[i].initFlg = false;
											}
											this.gFootL[i].transform.position = this.HandL2[i].position;
											this.gHizaL[i].transform.position = this.ForearmL2[i].position;
											if (this.mFootL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mFootL[i].isStop = false;
											}
											if (this.mHizaL[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mHizaL[i].isStop = false;
											}
											this.mFootR[i].maid = maid;
											this.mFootR[i].HandL = this.HandR2[i];
											if (this.ikMode[i] == 2)
											{
												this.mFootR[i].UpperArmL = this.ForearmR2[i];
											}
											else
											{
												this.mFootR[i].UpperArmL = this.UpperArmR2[i];
												if (this.ikModeOld[i] == 2 && this.isLock[i])
												{
													this.mFootR[i].OnMouseDown();
													this.mFootR[i].isSelect = false;
												}
											}
											this.mFootR[i].ForearmL = this.ForearmR2[i];
											this.mHizaR[i].maid = maid;
											this.mHizaR[i].HandL = this.ForearmR2[i];
											this.mHizaR[i].UpperArmL = this.UpperArmR2[i];
											this.mHizaR[i].ForearmL = this.UpperArmR2[i];
											this.mHizaR[i].onFlg = true;
											if (this.mHizaR[i].onFlg2)
											{
												this.mHizaR[i].onFlg2 = false;
												this.mFootR[i].initFlg = false;
											}
											this.gFootR[i].transform.position = this.HandR2[i].position;
											this.gHizaR[i].transform.position = this.ForearmR2[i].position;
											if (this.mFootR[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mFootR[i].isStop = false;
											}
											if (this.mHizaR[i].isStop)
											{
												this.isStop[i] = true;
												this.isLock[i] = true;
												this.mHizaR[i].isStop = false;
											}
											bool flag12 = false;
											if (this.ikModeOld[i] == 9 || this.ikModeOld[i] == 10 || this.ikModeOld[i] == 11 || this.ikModeOld[i] == 12 || this.ikModeOld[i] == 14)
											{
												flag12 = true;
											}
											if (this.isBone[i])
											{
												if ((this.ikMode[i] != 3 && this.ikMode[i] != 5 && this.ikMode[i] != 8) || this.ikMode[i] != this.ikModeOld[i] || this.isChange[i])
												{
													this.gizmoFootL[i].Visible = false;
													this.gizmoHandR[i].Visible = false;
													this.gizmoHandL[i].Visible = false;
													this.gizmoFootR[i].Visible = false;
												}
												else
												{
													bool fieldValue4 = MultipleMaids.GetFieldValue<GizmoRender, bool>(this.gizmoHandL[i], "is_drag_");
													bool fieldValue5 = MultipleMaids.GetFieldValue<GizmoRender, bool>(this.gizmoHandR[i], "is_drag_");
													bool fieldValue6 = MultipleMaids.GetFieldValue<GizmoRender, bool>(this.gizmoFootL[i], "is_drag_");
													bool fieldValue7 = MultipleMaids.GetFieldValue<GizmoRender, bool>(this.gizmoFootR[i], "is_drag_");
													if (this.ikMode[i] == 3)
													{
														this.gizmoHandL[i].transform.position = this.HandL1[i].transform.position;
														this.gizmoHandR[i].transform.position = this.HandR1[i].transform.position;
														this.gizmoFootL[i].transform.position = this.HandL2[i].transform.position;
														this.gizmoFootR[i].transform.position = this.HandR2[i].transform.position;
														if (fieldValue4 || fieldValue5 || fieldValue6 || fieldValue7)
														{
															this.HandL1[i].transform.rotation = this.gizmoHandL[i].transform.rotation;
															this.HandR1[i].transform.rotation = this.gizmoHandR[i].transform.rotation;
															this.HandL2[i].transform.rotation = this.gizmoFootL[i].transform.rotation;
															this.HandR2[i].transform.rotation = this.gizmoFootR[i].transform.rotation;
														}
														else
														{
															this.gizmoHandL[i].transform.rotation = this.HandL1[i].transform.rotation;
															this.gizmoHandR[i].transform.rotation = this.HandR1[i].transform.rotation;
															this.gizmoFootL[i].transform.rotation = this.HandL2[i].transform.rotation;
															this.gizmoFootR[i].transform.rotation = this.HandR2[i].transform.rotation;
														}
													}
													else if (this.ikMode[i] == 5)
													{
														this.gizmoHandL[i].transform.position = this.UpperArmL1[i].transform.position;
														this.gizmoHandR[i].transform.position = this.UpperArmR1[i].transform.position;
														this.gizmoFootL[i].transform.position = this.UpperArmL2[i].transform.position;
														this.gizmoFootR[i].transform.position = this.UpperArmR2[i].transform.position;
														if (fieldValue4 || fieldValue5 || fieldValue6 || fieldValue7)
														{
															this.UpperArmL1[i].transform.rotation = this.gizmoHandL[i].transform.rotation;
															this.UpperArmR1[i].transform.rotation = this.gizmoHandR[i].transform.rotation;
															this.UpperArmL2[i].transform.rotation = this.gizmoFootL[i].transform.rotation;
															this.UpperArmR2[i].transform.rotation = this.gizmoFootR[i].transform.rotation;
															flag12 = true;
														}
														else
														{
															this.gizmoHandL[i].transform.rotation = this.UpperArmL1[i].transform.rotation;
															this.gizmoHandR[i].transform.rotation = this.UpperArmR1[i].transform.rotation;
															this.gizmoFootL[i].transform.rotation = this.UpperArmL2[i].transform.rotation;
															this.gizmoFootR[i].transform.rotation = this.UpperArmR2[i].transform.rotation;
														}
													}
													else if (this.ikMode[i] == 8)
													{
														this.gizmoHandL[i].transform.position = this.ForearmL1[i].transform.position;
														this.gizmoHandR[i].transform.position = this.ForearmR1[i].transform.position;
														this.gizmoFootL[i].transform.position = this.ForearmL2[i].transform.position;
														this.gizmoFootR[i].transform.position = this.ForearmR2[i].transform.position;
														if (fieldValue4 || fieldValue5 || fieldValue6 || fieldValue7)
														{
															this.ForearmL1[i].transform.rotation = this.gizmoHandL[i].transform.rotation;
															this.ForearmR1[i].transform.rotation = this.gizmoHandR[i].transform.rotation;
															this.ForearmL2[i].transform.rotation = this.gizmoFootL[i].transform.rotation;
															this.ForearmR2[i].transform.rotation = this.gizmoFootR[i].transform.rotation;
															flag12 = true;
														}
														else
														{
															this.gizmoHandL[i].transform.rotation = this.ForearmL1[i].transform.rotation;
															this.gizmoHandR[i].transform.rotation = this.ForearmR1[i].transform.rotation;
															this.gizmoFootL[i].transform.rotation = this.ForearmL2[i].transform.rotation;
															this.gizmoFootR[i].transform.rotation = this.ForearmR2[i].transform.rotation;
														}
													}
													this.gizmoHandL[i].Visible = true;
													this.gizmoHandR[i].Visible = true;
													this.gizmoFootL[i].Visible = true;
													this.gizmoFootR[i].Visible = true;
													this.gHandL[i].SetActive(false);
													this.gFootL[i].SetActive(false);
													this.gHandR[i].SetActive(false);
													this.gFootR[i].SetActive(false);
													if (!this.isLock[i])
													{
														if (fieldValue4 || fieldValue5 || fieldValue6 || fieldValue7)
														{
															this.isStop[i] = true;
															this.isLock[i] = true;
														}
													}
												}
												if (this.isChange[i])
												{
													this.isChange[i] = false;
													this.gHandL[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gHandR[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gArmL[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gArmR[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gFootL[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gFootR[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gHizaL[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gHizaR[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gClavicleL[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gClavicleR[i].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
													this.gHandL[i].GetComponent<Renderer>().enabled = true;
													this.gHandR[i].GetComponent<Renderer>().enabled = true;
													this.gArmL[i].GetComponent<Renderer>().enabled = true;
													this.gArmR[i].GetComponent<Renderer>().enabled = true;
													this.gFootL[i].GetComponent<Renderer>().enabled = true;
													this.gFootR[i].GetComponent<Renderer>().enabled = true;
													this.gHizaL[i].GetComponent<Renderer>().enabled = true;
													this.gHizaR[i].GetComponent<Renderer>().enabled = true;
													this.gClavicleL[i].GetComponent<Renderer>().enabled = true;
													this.gClavicleR[i].GetComponent<Renderer>().enabled = true;
													this.gHandL[i].GetComponent<Renderer>().material = this.m_material2;
													this.gHandR[i].GetComponent<Renderer>().material = this.m_material2;
													this.gArmL[i].GetComponent<Renderer>().material = this.m_material2;
													this.gArmR[i].GetComponent<Renderer>().material = this.m_material2;
													this.gFootL[i].GetComponent<Renderer>().material = this.m_material2;
													this.gFootR[i].GetComponent<Renderer>().material = this.m_material2;
													this.gHizaL[i].GetComponent<Renderer>().material = this.m_material2;
													this.gHizaR[i].GetComponent<Renderer>().material = this.m_material2;
													this.gClavicleL[i].GetComponent<Renderer>().material = this.m_material2;
													this.gClavicleR[i].GetComponent<Renderer>().material = this.m_material2;
												}
												if (this.ikMode[i] == 0 && (this.ikModeOld[i] == 1 || this.ikModeOld[i] == 2) && this.gNeck[i])
												{
													this.mNeck[i].ido = 1;
													this.mNeck[i].reset = true;
													this.mSpine[i].ido = 1;
													this.mSpine[i].reset = true;
													this.mSpine0a[i].ido = 1;
													this.mSpine0a[i].reset = true;
													this.mSpine1a[i].ido = 1;
													this.mSpine1a[i].reset = true;
													this.mSpine1[i].ido = 1;
													this.mSpine1[i].reset = true;
													this.mPelvis[i].ido = 1;
													this.mPelvis[i].reset = true;
												}
												else if (this.ikMode[i] == 0)
												{
													this.gNeck[i].SetActive(true);
													this.gSpine[i].SetActive(true);
													this.gSpine0a[i].SetActive(true);
													this.gSpine1a[i].SetActive(true);
													this.gSpine1[i].SetActive(true);
													this.gPelvis[i].SetActive(true);
													if (this.mNeck[i].isHead)
													{
														this.mNeck[i].isHead = false;
														this.isLookn[i] = this.isLook[i];
														this.isLook[i] = !this.isLook[i];
													}
													this.mNeck[i].maid = maid;
													this.mNeck[i].head = this.Neck[i];
													this.mNeck[i].no = i;
													this.mNeck[i].ido = 1;
													this.gNeck[i].transform.position = this.Neck[i].position;
													this.gNeck[i].transform.localEulerAngles = this.Neck[i].localEulerAngles;
													this.mSpine[i].maid = maid;
													this.mSpine[i].head = this.Spine2[i];
													this.mSpine[i].no = i;
													this.mSpine[i].ido = 1;
													this.gSpine[i].transform.position = this.Spine2[i].position;
													this.gSpine[i].transform.eulerAngles = new Vector3(this.Spine2[i].localEulerAngles.z, this.Spine2[i].localEulerAngles.y, this.Spine2[i].localEulerAngles.x);
													this.mSpine0a[i].maid = maid;
													this.mSpine0a[i].head = this.Spine0a2[i];
													this.mSpine0a[i].no = i;
													this.mSpine0a[i].ido = 1;
													this.gSpine0a[i].transform.position = this.Spine0a2[i].position;
													this.gSpine0a[i].transform.eulerAngles = new Vector3(-this.Spine0a2[i].localEulerAngles.z, this.Spine0a2[i].localEulerAngles.x, this.Spine0a2[i].localEulerAngles.y);
													this.mSpine1a[i].maid = maid;
													this.mSpine1a[i].head = this.Spine1a2[i];
													this.mSpine1a[i].no = i;
													this.mSpine1a[i].ido = 1;
													this.gSpine1a[i].transform.position = this.Spine1a2[i].position;
													this.gSpine1a[i].transform.localEulerAngles = new Vector3(-this.Spine1a2[i].localEulerAngles.z, this.Spine1a2[i].localEulerAngles.x, this.Spine1a2[i].localEulerAngles.y);
													this.mSpine1[i].maid = maid;
													this.mSpine1[i].head = this.Spine12[i];
													this.mSpine1[i].no = i;
													this.mSpine1[i].ido = 1;
													this.gSpine1[i].transform.position = this.Spine12[i].position;
													this.gSpine1[i].transform.localEulerAngles = new Vector3(-this.Spine12[i].localEulerAngles.z, this.Spine12[i].localEulerAngles.x, this.Spine12[i].localEulerAngles.y);
													this.mPelvis[i].maid = maid;
													this.mPelvis[i].head = this.Pelvis2[i];
													this.mPelvis[i].no = i;
													this.mPelvis[i].ido = 1;
													this.gPelvis[i].transform.position = this.Pelvis2[i].position;
													this.gPelvis[i].transform.localEulerAngles = this.Pelvis2[i].localEulerAngles;
													if (this.mNeck[i].isIdo)
													{
														this.mNeck[i].isIdo = false;
														this.isLock[i] = true;
													}
													if (this.mSpine[i].isIdo)
													{
														this.mSpine[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mSpine0a[i].isIdo)
													{
														this.mSpine0a[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mSpine1a[i].isIdo)
													{
														this.mSpine1a[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mSpine1[i].isIdo)
													{
														this.mSpine1[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mPelvis[i].isIdo)
													{
														this.mPelvis[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mClavicleL[i].isIdo)
													{
														this.mClavicleL[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
													if (this.mClavicleR[i].isIdo)
													{
														this.mClavicleR[i].isIdo = false;
														flag12 = true;
														this.isLock[i] = true;
													}
												}
											}
											else if (this.isChange[i])
											{
												this.isChange[i] = false;
												this.gHandL[i].GetComponent<Renderer>().enabled = false;
												this.gArmL[i].GetComponent<Renderer>().enabled = false;
												this.gFootL[i].GetComponent<Renderer>().enabled = false;
												this.gHizaL[i].GetComponent<Renderer>().enabled = false;
												this.gHandR[i].GetComponent<Renderer>().enabled = false;
												this.gArmR[i].GetComponent<Renderer>().enabled = false;
												this.gFootR[i].GetComponent<Renderer>().enabled = false;
												this.gHizaR[i].GetComponent<Renderer>().enabled = false;
												this.gClavicleL[i].GetComponent<Renderer>().enabled = false;
												this.gClavicleR[i].GetComponent<Renderer>().enabled = false;
												this.gHandL[i].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
												this.gHandR[i].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
												this.gArmL[i].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
												this.gArmR[i].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
												this.gFootL[i].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
												this.gFootR[i].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
												this.gHizaL[i].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
												this.gHizaR[i].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
												this.gClavicleL[i].transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
												this.mClavicleR[i].transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
												this.gNeck[i].SetActive(false);
												this.gSpine[i].SetActive(false);
												this.gSpine0a[i].SetActive(false);
												this.gSpine1a[i].SetActive(false);
												this.gSpine1[i].SetActive(false);
												this.gPelvis[i].SetActive(false);
												this.gizmoHandL[i].Visible = false;
												this.gizmoHandR[i].Visible = false;
												this.gizmoFootL[i].Visible = false;
												this.gizmoFootR[i].Visible = false;
											}
											if (flag12)
											{
												this.mHandL[i].initFlg = false;
												this.mHandR[i].initFlg = false;
												this.mFootL[i].initFlg = false;
												this.mFootR[i].initFlg = false;
											}
										}
										this.ikModeOld[i] = this.ikMode[i];
										bool flag13 = false;
										if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.Q) && i == this.ikMaid)
										{
											if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.BackQuote))
											{
												for (int k = 0; k < 10; k++)
												{
													maid.transform.RotateAround(maid.transform.position, Vector3.right, -maid.transform.rotation.eulerAngles.x);
													maid.transform.RotateAround(maid.transform.position, Vector3.forward, -maid.transform.rotation.eulerAngles.z);
												}
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), 0.3f);
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), 0.3f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), 0.3f);
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), -0.3f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), -0.3f);
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), 0.3f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), -0.3f);
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), -0.3f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.I))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), 0.4f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.K))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), -0.4f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.J))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), 0.4f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.L))
											{
												maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), -0.4f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.U))
											{
												maid.body0.transform.localRotation = Quaternion.Euler(maid.body0.transform.localEulerAngles) * Quaternion.AngleAxis(-1.5f, Vector3.up);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.O))
											{
												maid.body0.transform.localRotation = Quaternion.Euler(maid.body0.transform.localEulerAngles) * Quaternion.AngleAxis(1.5f, Vector3.up);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.U))
											{
												maid.body0.transform.RotateAround(maid.transform.position, Vector3.up, -1.5f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.O))
											{
												maid.body0.transform.RotateAround(maid.transform.position, Vector3.up, 1.5f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
												vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
												vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
												vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
												vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.I))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.K))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.J))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.L))
											{
												Vector3 vector = maid.transform.position;
												vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X)) && Input.GetKey(KeyCode.Alpha0))
											{
												Vector3 vector = maid.transform.position;
												vector.y += 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X)) && Input.GetKey(KeyCode.P))
											{
												Vector3 vector = maid.transform.position;
												vector.y -= 0.0075f * this.speed;
												maid.SetPos(vector);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.I))
											{
												maid.transform.localScale = new Vector3(maid.transform.localScale.x * 1.005f, maid.transform.localScale.y * 1.005f, maid.transform.localScale.z * 1.005f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.K))
											{
												maid.transform.localScale = new Vector3(maid.transform.localScale.x * 0.995f, maid.transform.localScale.y * 0.995f, maid.transform.localScale.z * 0.995f);
												flag13 = true;
											}
											else if (Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.BackQuote))
											{
												maid.transform.localScale = new Vector3(1f, 1f, 1f);
												flag13 = true;
											}
										}
										if (!flag13 && (this.ikBui != 5 || (this.ikBui == 5 && (this.ikMode[i] == 4 || this.ikMode[i] == 6 || this.ikMode[i] == 7))) && !Input.GetKey(KeyCode.V) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.Q) && i == this.ikMaid)
										{
											if (Input.GetKey(KeyCode.I))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.004f;
													this.ikLeftArm.Init(this.UpperArmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.004f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 3)
												{
													if (this.ikBui == 1 || this.ikBui == 2)
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector3.x, 0f, vector3.z), 0.8f);
													}
													else
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector3.x, 0f, vector3.z), -0.8f);
													}
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, new Vector3(vector3.x, 0f, vector3.z), 0.4f);
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, new Vector3(vector3.x, 0f, vector3.z), 0.6f);
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, new Vector3(vector3.x, 0f, vector3.z), 0.03f);
													this.Spine1.RotateAround(this.Spine1.position, new Vector3(vector3.x, 0f, vector3.z), 0.1f);
													this.Spine0a.RotateAround(this.Spine0a.position, new Vector3(vector3.x, 0f, vector3.z), 0.09f);
													this.Spine.RotateAround(this.Spine.position, new Vector3(vector3.x, 0f, vector3.z), 0.07f);
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, new Vector3(vector3.x, 0f, vector3.z), -0.2f);
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
											else if (Input.GetKey(KeyCode.K))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.004f;
													this.ikLeftArm.Init(this.UpperArmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.004f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 3)
												{
													if (this.ikBui == 1 || this.ikBui == 2)
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector3.x, 0f, vector3.z), -0.8f);
													}
													else
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector3.x, 0f, vector3.z), 0.8f);
													}
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, new Vector3(vector3.x, 0f, vector3.z), -0.4f);
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, new Vector3(vector3.x, 0f, vector3.z), -0.6f);
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, new Vector3(vector3.x, 0f, vector3.z), -0.03f);
													this.Spine1.RotateAround(this.Spine1.position, new Vector3(vector3.x, 0f, vector3.z), -0.1f);
													this.Spine0a.RotateAround(this.Spine0a.position, new Vector3(vector3.x, 0f, vector3.z), -0.09f);
													this.Spine.RotateAround(this.Spine.position, new Vector3(vector3.x, 0f, vector3.z), -0.07f);
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, new Vector3(vector3.x, 0f, vector3.z), 0.2f);
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
											else if (Input.GetKey(KeyCode.J))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.004f;
													this.ikLeftArm.Init(this.UpperArmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.004f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 3)
												{
													if (this.ikBui == 1 || this.ikBui == 2)
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector2.x, 0f, vector2.z), 0.8f);
													}
													else
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector2.x, 0f, vector2.z), -0.8f);
													}
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, new Vector3(vector2.x, 0f, vector2.z), 0.4f);
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, new Vector3(vector2.x, 0f, vector2.z), 0.6f);
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, new Vector3(vector2.x, 0f, vector2.z), 0.03f);
													this.Spine1.RotateAround(this.Spine1.position, new Vector3(vector2.x, 0f, vector2.z), 0.1f);
													this.Spine0a.RotateAround(this.Spine0a.position, new Vector3(vector2.x, 0f, vector2.z), 0.09f);
													this.Spine.RotateAround(this.Spine.position, new Vector3(vector2.x, 0f, vector2.z), 0.07f);
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, new Vector3(vector2.x, 0f, vector2.z), -0.2f);
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
											else if (Input.GetKey(KeyCode.L))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.004f;
													this.ikLeftArm.Init(this.UpperArmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.004f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 3)
												{
													if (this.ikBui == 1 || this.ikBui == 2)
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector2.x, 0f, vector2.z), -0.8f);
													}
													else
													{
														this.HandL.RotateAround(this.HandL.position, new Vector3(vector2.x, 0f, vector2.z), 0.8f);
													}
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, new Vector3(vector2.x, 0f, vector2.z), -0.4f);
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, new Vector3(vector2.x, 0f, vector2.z), -0.6f);
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, new Vector3(vector2.x, 0f, vector2.z), -0.03f);
													this.Spine1.RotateAround(this.Spine1.position, new Vector3(vector2.x, 0f, vector2.z), -0.1f);
													this.Spine0a.RotateAround(this.Spine0a.position, new Vector3(vector2.x, 0f, vector2.z), -0.09f);
													this.Spine.RotateAround(this.Spine.position, new Vector3(vector2.x, 0f, vector2.z), -0.07f);
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, new Vector3(vector2.x, 0f, vector2.z), 0.2f);
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
											else if (Input.GetKey(KeyCode.U))
											{
												if (this.ikMode[i] == 3)
												{
													this.HandL.localRotation = Quaternion.Euler(this.HandL.localEulerAngles) * Quaternion.AngleAxis(1f, Vector3.right);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, Vector3.up, -0.7f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, Vector3.up, -0.7f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, Vector3.up, -0.084f);
													this.Spine0a.RotateAround(this.Spine0a.position, Vector3.up, -0.156f);
													this.Spine.RotateAround(this.Spine.position, Vector3.up, -0.156f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, Vector3.up, 0.4f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if ((this.ikMode[i] == 0 || this.ikMode[i] == 1 || this.ikMode[i] == 2) && (this.ikBui == 3 || this.ikBui == 4))
												{
													this.UpperArmL.localRotation = Quaternion.Euler(this.UpperArmL.localEulerAngles) * Quaternion.AngleAxis(0.5f, Vector3.right);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if ((this.ikMode[i] == 0 || this.ikMode[i] == 1 || this.ikMode[i] == 2) && (this.ikBui == 1 || this.ikBui == 2))
												{
													this.UpperArmL.RotateAround(this.UpperArmL.position, Vector3.right, 0.5f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
											}
											else if (Input.GetKey(KeyCode.O))
											{
												if (this.ikMode[i] == 3)
												{
													this.HandL.localRotation = Quaternion.Euler(this.HandL.localEulerAngles) * Quaternion.AngleAxis(-1f, Vector3.right);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 4)
												{
													this.Head.RotateAround(this.Head.position, Vector3.up, 0.7f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 8)
												{
													this.IK_hand.RotateAround(this.IK_hand.position, Vector3.up, 0.7f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 6)
												{
													this.Spine1a.RotateAround(this.Spine1a.position, Vector3.up, 0.084f);
													this.Spine0a.RotateAround(this.Spine0a.position, Vector3.up, 0.156f);
													this.Spine.RotateAround(this.Spine.position, Vector3.up, 0.156f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if (this.ikMode[i] == 7)
												{
													this.Pelvis.RotateAround(this.Pelvis.position, Vector3.up, -0.4f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if ((this.ikMode[i] == 0 || this.ikMode[i] == 1 || this.ikMode[i] == 2) && (this.ikBui == 3 || this.ikBui == 4))
												{
													this.UpperArmL.localRotation = Quaternion.Euler(this.UpperArmL.localEulerAngles) * Quaternion.AngleAxis(-0.5f, Vector3.right);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
												else if ((this.ikMode[i] == 0 || this.ikMode[i] == 1 || this.ikMode[i] == 2) && (this.ikBui == 1 || this.ikBui == 2))
												{
													this.UpperArmL.RotateAround(this.UpperArmL.position, Vector3.right, -0.5f);
													this.isStop[i] = true;
													this.isLock[i] = true;
												}
											}
											else if (Input.GetKey(KeyCode.Alpha0))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5.y += 0.005f;
													this.ikLeftArm.Init(this.UpperArmL, this.HandL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.HandL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5.y += 0.005f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5.y += 0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5.y += 0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
											else if (Input.GetKey(KeyCode.P))
											{
												if (this.ikMode[i] == 0)
												{
													Vector3 vector5 = this.HandL.position;
													vector5.y -= 0.005f;
													this.ikLeftArm.Init(this.UpperArmL, this.HandL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.HandL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 2)
												{
													Vector3 vector5 = this.HandL.position;
													vector5.y -= 0.005f;
													this.ikLeftArm.Init(this.ForearmL, this.ForearmL, this.HandL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.ForearmL, this.ForearmL, this.HandL, vector5, default(Vector3));
												}
												else if (this.ikMode[i] == 5)
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5.y -= 0.0035f;
													this.ikLeftArm.Init(this.Clavicle, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.Clavicle, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												else
												{
													Vector3 vector5 = this.ForearmL.position;
													vector5.y -= 0.0035f;
													this.ikLeftArm.Init(this.UpperArmL, this.UpperArmL, this.ForearmL, this.maidArray[i].body0);
													this.ikLeftArm.Porc(this.UpperArmL, this.UpperArmL, this.ForearmL, vector5, default(Vector3));
												}
												this.isStop[i] = true;
												this.isLock[i] = true;
											}
										}
									}
								}
							}
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.LeftBracket) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							maid.transform.RotateAround(maid.transform.position, Vector3.up, -0.75f);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.LeftBracket))
						{
							maid.transform.RotateAround(maid.transform.position, Vector3.up, 0.75f);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.DownArrow) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							if (!this.xFlg[i])
							{
								this.xFlg[i] = true;
								this.zFlg[i] = false;
								if (maid.status.personal.uniqueName == "Pride")
								{
									string text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.tunArray.Length; k++)
									{
										if (this.tunArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									string text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.coolArray.Length; k++)
									{
										if (this.coolArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									string text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.pureArray.Length; k++)
									{
										if (this.pureArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + "0" + num4.ToString() + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									string text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.yanArray.Length; k++)
									{
										if (this.yanArray[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (!flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									string text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h0Array.Length; k++)
									{
										if (this.h0Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									string text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h1Array.Length; k++)
									{
										if (this.h1Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									string text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(10000);
									bool flag7 = false;
									for (int k = 0; k < this.h2Array.Length; k++)
									{
										if (this.h2Array[k] == num4)
										{
											flag7 = true;
											break;
										}
									}
									if (flag7)
									{
										text = text + string.Format("{0:00000}", num4) + ".ogg";
										if (GameUty.FileSystem.IsExistentFile(text))
										{
											maid.AudioMan.LoadPlay(text, 0f, false, false);
										}
									}
								}
							}
							else
							{
								this.xFlg[i] = false;
								maid.AudioMan.Clear();
							}
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.UpArrow) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							if (!this.zFlg[i])
							{
								this.zFlg[i] = true;
								this.xFlg[i] = false;
								string text = "";
								if (maid.status.personal.uniqueName == "Pride")
								{
									text = "s0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.tunArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.tunArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Cool")
								{
									text = "s1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.coolArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.coolArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Pure")
								{
									text = "s2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.pureArray.Length);
									object obj = text;
									text = string.Concat(new object[]
									{
										obj,
										"0",
										this.pureArray[num4],
										".ogg"
									});
								}
								if (maid.status.personal.uniqueName == "Yandere")
								{
									text = "s3_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.yanArray.Length);
									text = text + string.Format("{0:00000}", this.yanArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Muku")
								{
									text = "h0_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h0tArray.Length);
									text = text + string.Format("{0:00000}", this.h0tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Majime")
								{
									text = "h1_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h1tArray.Length);
									text = text + string.Format("{0:00000}", this.h1tArray[num4]) + ".ogg";
								}
								if (maid.status.personal.uniqueName == "Rindere")
								{
									text = "h2_";
									System.Random random = new System.Random();
									int num4 = random.Next(this.h2tArray.Length);
									text = text + string.Format("{0:00000}", this.h2tArray[num4]) + ".ogg";
								}
								maid.AudioMan.LoadPlay(text, 0f, false, false);
							}
							else
							{
								this.zFlg[i] = false;
								maid.AudioMan.Clear();
							}
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.UpArrow))
						{
							string text = "";
							if (maid.status.personal.uniqueName == "Pride")
							{
								text = "s0_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.tunArray.Length);
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									"0",
									this.tunArray[num4],
									".ogg"
								});
							}
							if (maid.status.personal.uniqueName == "Cool")
							{
								text = "s1_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.coolArray.Length);
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									"0",
									this.coolArray[num4],
									".ogg"
								});
							}
							if (maid.status.personal.uniqueName == "Pure")
							{
								text = "s2_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.pureArray.Length);
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									"0",
									this.pureArray[num4],
									".ogg"
								});
							}
							if (maid.status.personal.uniqueName == "Yandere")
							{
								text = "s3_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.yanArray.Length);
								text = text + string.Format("{0:00000}", this.yanArray[num4]) + ".ogg";
							}
							if (maid.status.personal.uniqueName == "Muku")
							{
								text = "h0_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.h0tArray.Length);
								text = text + string.Format("{0:00000}", this.h0tArray[num4]) + ".ogg";
							}
							if (maid.status.personal.uniqueName == "Majime")
							{
								text = "h1_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.h1tArray.Length);
								text = text + string.Format("{0:00000}", this.h1tArray[num4]) + ".ogg";
							}
							if (maid.status.personal.uniqueName == "Rindere")
							{
								text = "h2_";
								System.Random random = new System.Random();
								int num4 = random.Next(this.h2tArray.Length);
								text = text + string.Format("{0:00000}", this.h2tArray[num4]) + ".ogg";
							}
							maid.AudioMan.LoadPlay(text, 0f, false, false);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.DownArrow))
						{
							if (maid.status.personal.uniqueName == "Pride")
							{
								string text = "s0_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.tunArray.Length; k++)
								{
									if (this.tunArray[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (!flag7)
								{
									text = text + "0" + num4.ToString() + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Cool")
							{
								string text = "s1_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.coolArray.Length; k++)
								{
									if (this.coolArray[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (!flag7)
								{
									text = text + "0" + num4.ToString() + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Pure")
							{
								string text = "s2_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.pureArray.Length; k++)
								{
									if (this.pureArray[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (!flag7)
								{
									text = text + "0" + num4.ToString() + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Yandere")
							{
								string text = "s3_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.yanArray.Length; k++)
								{
									if (this.yanArray[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (!flag7)
								{
									text = text + string.Format("{0:00000}", num4) + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Muku")
							{
								string text = "h0_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.h0Array.Length; k++)
								{
									if (this.h0Array[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (flag7)
								{
									text = text + string.Format("{0:00000}", num4) + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Majime")
							{
								string text = "h1_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.h1Array.Length; k++)
								{
									if (this.h1Array[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (flag7)
								{
									text = text + string.Format("{0:00000}", num4) + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							if (maid.status.personal.uniqueName == "Rindere")
							{
								string text = "h2_";
								System.Random random = new System.Random();
								int num4 = random.Next(10000);
								bool flag7 = false;
								for (int k = 0; k < this.h2Array.Length; k++)
								{
									if (this.h2Array[k] == num4)
									{
										flag7 = true;
										break;
									}
								}
								if (flag7)
								{
									text = text + string.Format("{0:00000}", num4) + ".ogg";
									if (GameUty.FileSystem.IsExistentFile(text))
									{
										maid.AudioMan.LoadPlay(text, 0f, false, false);
									}
								}
							}
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Insert) && Input.GetKey(KeyCode.Home))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Insert) && Input.GetKey(KeyCode.End))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Delete) && Input.GetKey(KeyCode.Home))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Delete) && Input.GetKey(KeyCode.End))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Insert))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Delete))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Home))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.End))
						{
							Vector3 vector = maid.transform.position;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.PageUp))
						{
							Vector3 vector = maid.transform.position;
							vector.y += 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.PageDown))
						{
							Vector3 vector = maid.transform.position;
							vector.y -= 0.0075f * this.speed;
							maid.SetPos(vector);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Minus) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							this.faceIndex[i]--;
							if (this.faceIndex[i] <= -1)
							{
								this.faceIndex[i] = this.faceArray.Length - 1;
							}
							maid.FaceAnime(this.faceArray[this.faceIndex[i]], 1f, 0);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Minus))
						{
							this.faceIndex[i]++;
							if (this.faceIndex[i] == this.faceArray.Length)
							{
								this.faceIndex[i] = 0;
							}
							maid.FaceAnime(this.faceArray[this.faceIndex[i]], 1f, 0);
							this.idoFlg[i] = true;
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.Quote))
						{
							this.faceBlendIndex[i]++;
							if (this.faceBlendIndex[i] == this.faceBlendArray.Length)
							{
								this.faceBlendIndex[i] = 0;
							}
							maid.FaceBlend(this.faceBlendArray[this.faceBlendIndex[i]]);
							this.idoFlg[i] = true;
						}
						if (Input.GetKey(key) && Input.GetKey(KeyCode.E))
						{
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
							maid.transform.localScale = new Vector3(maid.transform.localScale.x * 1.005f, maid.transform.localScale.y * 1.005f, maid.transform.localScale.z * 1.005f);
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.D))
						{
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
							maid.transform.localScale = new Vector3(maid.transform.localScale.x * 0.995f, maid.transform.localScale.y * 0.995f, maid.transform.localScale.z * 0.995f);
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.C))
						{
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
							maid.transform.localScale = new Vector3(1f, 1f, 1f);
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.H))
						{
							maid.body0.SetMask(TBody.SlotID.wear, true);
							maid.body0.SetMask(TBody.SlotID.skirt, true);
							maid.body0.SetMask(TBody.SlotID.bra, true);
							maid.body0.SetMask(TBody.SlotID.panz, true);
							maid.body0.SetMask(TBody.SlotID.mizugi, true);
							maid.body0.SetMask(TBody.SlotID.onepiece, true);
							this.isWear = true;
							this.isSkirt = true;
							this.isBra = true;
							this.isPanz = true;
							System.Random random = new System.Random();
							int num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.wear, false);
							}
							if (num4 == 1)
							{
								this.isWear = false;
							}
							num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.skirt, false);
							}
							if (num4 == 1)
							{
								this.isSkirt = false;
							}
							num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.bra, false);
							}
							if (num4 == 1)
							{
								this.isBra = false;
							}
							num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.panz, false);
							}
							if (num4 == 1)
							{
								this.isPanz = false;
							}
							num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.mizugi, false);
							}
							num4 = random.Next(2);
							if (num4 == 1)
							{
								maid.body0.SetMask(TBody.SlotID.onepiece, false);
							}
							this.hFlg = true;
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKeyDown(key) && Input.GetKey(KeyCode.LeftShift) && !this.isLock[i])
						{
							this.poseIndex[i]--;
							if (this.poseIndex[i] <= -1)
							{
								this.poseIndex[i] = this.poseArray.Length - 1;
							}
							if (maid && maid.Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
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
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKeyDown(key) && Input.GetKey(KeyCode.RightAlt) && !this.isLock[i])
						{
							int num11 = this.poseIndex[i];
							for (int k = 0; k < this.groupList.Count; k++)
							{
								if (this.poseIndex[i] < (int)this.groupList[k])
								{
									this.poseIndex[i] = (int)this.groupList[k];
									break;
								}
							}
							if (num11 == this.poseIndex[i] && this.poseIndex[i] >= (int)this.groupList[this.groupList.Count - 1])
							{
								this.poseIndex[i] = 0;
							}
							if (maid && maid.Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									maid.CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
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
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKeyDown(key) && Input.GetKey(KeyCode.LeftAlt) && !this.isLock[i])
						{
							for (int k = 0; k < this.groupList.Count; k++)
							{
								if (k == 0 && this.poseIndex[i] <= (int)this.groupList[k])
								{
									if (this.poseIndex[i] == 0)
									{
										this.poseIndex[i] = (int)this.groupList[this.groupList.Count - 1];
									}
									else
									{
										this.poseIndex[i] = 0;
									}
									break;
								}
								if (k > 0 && this.poseIndex[i] > (int)this.groupList[k - 1] && this.poseIndex[i] <= (int)this.groupList[k])
								{
									this.poseIndex[i] = (int)this.groupList[k - 1];
									break;
								}
							}
							if (this.poseIndex[i] > (int)this.groupList[this.groupList.Count - 1])
							{
								this.poseIndex[i] = (int)this.groupList[this.groupList.Count - 1];
							}
							if (maid && maid.Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									maid.CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
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
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKey(key) && Input.GetKeyDown(KeyCode.BackQuote) && !this.atFlg)
						{
							this.headEyeIndex[i]++;
							if (this.headEyeIndex[i] == this.headEyeArray.Length)
							{
								this.headEyeIndex[i] = 0;
							}
							maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
							maid.body0.boHeadToCam = true;
							maid.body0.boEyeToCam = true;
							if (this.headEyeIndex[i] == 0)
							{
								maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
								maid.body0.boHeadToCam = true;
								maid.body0.boEyeToCam = true;
							}
							else
							{
								maid.body0.trsLookTarget = null;
								CameraMain cameraMain = GameMain.Instance.MainCamera;
								if (this.headEyeIndex[i] == 1)
								{
									maid.body0.offsetLookTarget = new Vector3(-0.35f, 1f, -0.3f);
								}
								if (this.headEyeIndex[i] == 2)
								{
									maid.body0.offsetLookTarget = new Vector3(-0.4f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 3)
								{
									maid.body0.offsetLookTarget = new Vector3(-0.35f, 1f, 0.3f);
								}
								if (this.headEyeIndex[i] == 4)
								{
									maid.body0.offsetLookTarget = new Vector3(0f, 1f, -0.4f);
								}
								if (this.headEyeIndex[i] == 5)
								{
									maid.body0.offsetLookTarget = new Vector3(0f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 6)
								{
									maid.body0.offsetLookTarget = new Vector3(0f, 1f, 0.4f);
								}
								if (this.headEyeIndex[i] == 7)
								{
									maid.body0.offsetLookTarget = new Vector3(0.4f, 1f, -0.4f);
								}
								if (this.headEyeIndex[i] == 8)
								{
									maid.body0.offsetLookTarget = new Vector3(0.4f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 9)
								{
									maid.body0.offsetLookTarget = new Vector3(0.4f, 1f, 0.4f);
								}
							}
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKey(key) && Input.GetKey(KeyCode.Return))
						{
							this.idoFlg[i] = true;
							if (i >= 7)
							{
								this.idoFlg[i - 7] = true;
							}
						}
						else if (Input.GetKeyUp(key))
						{
							if (this.idoFlg[i])
							{
								this.idoFlg[i] = false;
							}
							else if (!this.getModKeyPressing(MultipleMaids.modKey.Ctrl) || i >= 7)
							{
								if (!this.isLock[i])
								{
									this.poseIndex[i]++;
									if (this.poseIndex[i] == this.poseArray.Length)
									{
										this.poseIndex[i] = 0;
									}
									if (maid && maid.Visible)
									{
										string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
										{
											','
										});
										this.isStop[i] = false;
										this.poseCount[i] = 20;
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
										int num6;
										if (array[0].Contains("MultipleMaidsPose"))
										{
											string path = array[0].Split(new char[]
											{
												'/'
											})[1];
											byte[] array2 = new byte[0];
											try
											{
												using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
												string text = Path.GetFileName(path);
												long num5 = (long)text.GetHashCode();
												maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
												Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
												{
													default(Maid.AutoTwist),
													Maid.AutoTwist.ShoulderR,
													Maid.AutoTwist.WristL,
													Maid.AutoTwist.WristR,
													Maid.AutoTwist.ThighL,
													Maid.AutoTwist.ThighR
												};
												for (int m = 0; m < array3.Length; m++)
												{
													maid.SetAutoTwist(array3[m], true);
												}
											}
										}
										else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
										{
											this.loadPose[i] = array[0];
										}
										else if (!array[0].StartsWith("dance_"))
										{
											this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
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
											this.isStop[i] = true;
											if (array.Length > 2)
											{
												Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
												this.isPoseIti[i] = true;
												this.poseIti[i] = this.maidArray[i].transform.position;
												this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
											}
										}
									}
								}
							}
						}
						goto IL_165DC;
					}
				}
				if (Input.GetKeyDown(KeyCode.E))
				{
				}
				if (!this.isVR && Input.GetKeyDown(KeyCode.F8) && this.sceneFlg && this.bGui)
				{
					this.bGui = false;
				}
				else if (!this.isVR && Input.GetKeyDown(KeyCode.F8))
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
						string path2 = string.Concat(new object[]
						{
							Path.GetFullPath(".\\"),
							"Mod\\MultipleMaidsScene\\",
							this.page * 10 + i + 1,
							".png"
						});
						if (File.Exists(path2))
						{
							FileStream fileStream = new FileStream(path2, FileMode.Open, FileAccess.Read);
							BinaryReader binaryReader = new BinaryReader(fileStream);
							byte[] array4 = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
							byte[] value = new byte[]
							{
								array4[36],
								array4[35],
								array4[34],
								array4[33]
							};
							int count = BitConverter.ToInt32(value, 0) - 8;
							byte[] bytes = array4.Skip(49).Take(count).ToArray<byte>();
							string text2 = Encoding.UTF8.GetString(bytes);
							text2 = MultipleMaids.StringFromBase64Comp(text2);
							if (text2 != "")
							{
								string[] array5 = text2.Split(new char[]
								{
									'_'
								});
								if (array5.Length >= 2)
								{
									string[] array6 = array5[0].Split(new char[]
									{
										','
									});
									this.date[i] = array6[0];
									this.ninzu[i] = array6[1] + "人";
								}
							}
						}
						else
						{
							IniKey iniKey = base.Preferences["scene"]["s" + (this.page * 10 + i + 1)];
							if (iniKey.Value != null && iniKey.Value.ToString() != "")
							{
								string[] array5 = iniKey.Value.Split(new char[]
								{
									'_'
								});
								if (array5.Length >= 2)
								{
									string[] array6 = array5[0].Split(new char[]
									{
										','
									});
									this.date[i] = array6[0];
									this.ninzu[i] = array6[1] + "人";
								}
							}
						}
						//IL_168F6:
						i++;
						continue;
						//goto IL_168F6;
					}
				}
				if (Input.GetKeyDown(KeyCode.F) && this.getModKeyPressing(MultipleMaids.modKey.Shift) && !this.fFlg)
				{
					this.bgmIndex--;
					if (this.bgmIndex <= -1)
					{
						this.bgmIndex = this.bgmArray.Length - 1;
					}
					GameMain.Instance.SoundMgr.PlayBGM(this.bgmArray[this.bgmIndex] + ".ogg", 0f, true);
					this.bgmCombo.selectedItemIndex = this.bgmIndex;
				}
				else if (Input.GetKeyDown(KeyCode.F) && !this.fFlg)
				{
					this.bgmIndex++;
					if (this.bgmIndex == this.bgmArray.Length)
					{
						this.bgmIndex = 0;
					}
					GameMain.Instance.SoundMgr.PlayBGM(this.bgmArray[this.bgmIndex] + ".ogg", 0f, true);
					this.bgmCombo.selectedItemIndex = this.bgmIndex;
				}
				if (!this.isVR && Input.GetKeyDown(KeyCode.M) && !this.mFlg)
				{
					GameObject gameObject4 = GameObject.Find("__GameMain__/SystemUI Root");
					GameObject gameObject5 = gameObject4.transform.Find("MessageWindowPanel").gameObject;
					MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
					if (this.isMessage)
					{
						this.isMessage = false;
						messageWindowMgr.CloseMessageWindowPanel();
					}
					else if (!this.bGuiMessage)
					{
						this.bGuiMessage = true;
					}
					else
					{
						this.bGuiMessage = false;
						this.isMessage = false;
						messageWindowMgr.CloseMessageWindowPanel();
					}
				}
				if (this.isDanceChu && Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Escape))
				{
					this.escFlg = true;
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].StopKuchipakuPattern();
							if (this.isDanceStart7V)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
							}
							if (this.isDanceStart8V)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
							}
							if (this.isDanceStart11V)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
							}
							if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								int i = j;
								Maid maid = this.maidArray[j];
								maid.DelProp(MPN.handitem, true);
								maid.DelProp(MPN.accvag, true);
								maid.DelProp(MPN.accanl, true);
								maid.DelProp(MPN.kousoku_upper, true);
								maid.DelProp(MPN.kousoku_lower, true);
								maid.AllProcPropSeqStart();
							}
							if (this.isDanceStart14V)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
							}
							if (this.isDanceStart15V)
							{
								this.maidArray[j].SetPos(this.dancePos[j]);
								this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
							}
						}
					}
					this.isDanceStart1F = false;
					this.isDanceStart1K = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart3K = false;
					this.isDanceStart4F = false;
					this.isDanceStart4K = false;
					this.isDanceStart5F = false;
					this.isDanceStart5K = false;
					this.isDanceStart6F = false;
					this.isDanceStart6K = false;
					this.isDanceStart7F = false;
					this.isDanceStart7V = false;
					this.isDanceStart8F = false;
					this.isDanceStart8V = false;
					this.isDanceStart8P = false;
					this.isDanceStart9F = false;
					this.isDanceStart9K = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart11V = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart13K = false;
					this.isDanceStart14F = false;
					this.isDanceStart14V = false;
					this.isDanceStart15F = false;
					this.isDanceStart15V = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
						this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
						this.maidArray[j].SetAutoTwistAll(true);
					}
					this.danceCheckIndex = 0;
					for (int k = 0; k < this.danceCheck.Length; k++)
					{
						this.danceCheck[this.danceCheckIndex] = 1f;
					}
					this.isDance = false;
					this.isDanceChu = false;
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					GameMain.Instance.SoundMgr.PlayBGM(this.bgmArray[this.bgmIndex] + ".ogg", 0f, true);
				}
				if (Input.GetKeyDown(KeyCode.Tab) || (!this.escFlg && Input.GetKeyDown(KeyCode.Escape)))
				{
					if (this.isScript)
					{
						GameObject gameObject4 = GameObject.Find("__GameMain__/SystemUI Root");
						GameObject gameObject5 = gameObject4.transform.Find("MessageWindowPanel").gameObject;
						if (this.isPanel)
						{
							this.isPanel = false;
						}
						else
						{
							this.isPanel = true;
						}
						gameObject5.SetActive(this.isPanel);
					}
					else
					{
						this.bGui = !this.bGui;
					}
				}
				if (Input.GetKeyDown(KeyCode.Y) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
				{
					this.keyFlg = true;
					this.bgIndex--;
					if (this.bgIndex <= -1)
					{
						this.bgIndex = this.bgArray.Length - 1;
					}
					if (!this.moveBg)
					{
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
							Vector3 vector = Vector3.zero;
							Vector3 zero3 = Vector3.zero;
							zero3.y = 90f;
							vector.z = 4f;
							vector.x = 1f;
							this.bg.transform.localPosition = vector;
							this.bg.transform.localRotation = Quaternion.Euler(zero3);
						}
					}
				}
				else if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.E))
				{
					this.keyFlg = true;
					this.bg.localScale = new Vector3(this.bg.localScale.x * 1.005f, this.bg.localScale.y * 1.005f, this.bg.localScale.z * 1.005f);
				}
				else if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.D))
				{
					this.keyFlg = true;
					this.bg.localScale = new Vector3(this.bg.localScale.x * 0.995f, this.bg.localScale.y * 0.995f, this.bg.localScale.z * 0.995f);
				}
				else if (Input.GetKey(KeyCode.Y) && Input.GetKeyDown(KeyCode.C))
				{
					this.keyFlg = true;
					this.bg.localScale = new Vector3(1f, 1f, 1f);
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
					vector += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
					vector += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
					vector += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
					vector += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.I))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.J))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.L))
				{
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					Vector3 vector = this.bg.position;
					vector += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.Alpha0))
				{
					Vector3 vector = this.bg.position;
					vector.y -= 0.015f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.P))
				{
					Vector3 vector = this.bg.position;
					vector.y += 0.015f * this.speed;
					this.bg.localPosition = vector;
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.U))
				{
					this.bg.RotateAround(this.maidArray[0].transform.position, Vector3.up, 0.7f);
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.O))
				{
					this.bg.RotateAround(this.maidArray[0].transform.position, Vector3.up, -0.7f);
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.Return))
				{
					if (Input.GetKey(KeyCode.Y))
					{
						this.keyFlg = true;
					}
				}
				else if (Input.GetKeyUp(KeyCode.Y) && !this.yFlg)
				{
					if (this.keyFlg)
					{
						this.keyFlg = false;
					}
					else
					{
						this.bgIndex++;
						if (this.bgIndex == this.bgArray.Length)
						{
							this.bgIndex = 0;
						}
						if (!this.moveBg)
						{
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
								Vector3 vector = Vector3.zero;
								Vector3 zero3 = Vector3.zero;
								zero3.y = 90f;
								vector.z = 4f;
								vector.x = 1f;
								this.bg.transform.localPosition = vector;
								this.bg.transform.localRotation = Quaternion.Euler(zero3);
							}
						}
					}
				}
				if (this.maidArray[0] != null && this.maidArray[0].Visible)
				{
					if (Input.GetKeyDown(KeyCode.Comma) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						int[] array7 = new int[1];
						int[] array8 = array7;
						if (this.maidCnt == 2)
						{
							array8 = new int[]
							{
								0,
								1
							};
						}
						if (this.maidCnt == 3)
						{
							array8 = new int[]
							{
								0,
								1,
								2
							};
						}
						if (this.maidCnt == 4)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3
							};
						}
						if (this.maidCnt == 5)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4
							};
						}
						if (this.maidCnt == 6)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5
							};
						}
						if (this.maidCnt == 7)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6
							};
						}
						if (this.maidCnt == 8)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6,
								7
							};
						}
						if (this.maidCnt == 9)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6,
								7,
								8
							};
						}
						if (this.maidCnt == 10)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6,
								7,
								8,
								9
							};
						}
						if (this.maidCnt == 11)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6,
								7,
								8,
								9,
								10
							};
						}
						if (this.maidCnt == 12)
						{
							array8 = new int[]
							{
								0,
								1,
								2,
								3,
								4,
								5,
								6,
								7,
								8,
								9,
								10,
								11
							};
						}
						System.Random random = new System.Random();
						int n = array8.Length;
						while (n > 1)
						{
							n--;
							int j = random.Next(n + 1);
							int num12 = array8[j];
							array8[j] = array8[n];
							array8[n] = num12;
						}
						int[] array9 = new int[this.maidCnt];
						Vector3[] array10 = new Vector3[this.maidCnt];
						Vector3[] array11 = new Vector3[this.maidCnt];
						int[] array12 = new int[this.maidCnt];
						for (int i = 0; i < this.maidCnt; i++)
						{
							array9[array8[i]] = this.poseIndex[array8[i]];
							array10[array8[i]] = this.maidArray[array8[i]].transform.localRotation.eulerAngles;
							array11[array8[i]] = this.maidArray[array8[i]].transform.position;
							array12[array8[i]] = this.headEyeIndex[array8[i]];
						}
						for (int i = 0; i < this.maidCnt; i++)
						{
							if (this.maidArray[i] && this.maidArray[i].Visible && !this.isLock[i])
							{
								string[] array13 = this.poseArray[array9[array8[i]]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								Maid maid = this.maidArray[i];
								if (array13[0].Contains("_momi") || array13[0].Contains("paizuri_"))
								{
									maid.body0.MuneYureL(0f);
									maid.body0.MuneYureR(0f);
								}
								else
								{
									maid.body0.MuneYureL(1f);
									maid.body0.MuneYureR(1f);
								}
								int num6;
								if (array13[0].Contains("MultipleMaidsPose"))
								{
									string path = array13[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array13[0].StartsWith("p") && int.TryParse(array13[0].Substring(1), out num6))
								{
									this.loadPose[i] = array13[0];
								}
								else if (!array13[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array13[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array13[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array13[0] + ".anm", GameUty.FileSystem, array13[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array13[0] + ".anm");
								}
								if (array13.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array13[0] + ".anm"].time = float.Parse(array13[1]);
									this.isStop[i] = true;
									if (array13.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
							this.poseIndex[i] = array9[array8[i]];
							this.maidArray[i].SetRot(array10[array8[i]]);
							this.maidArray[i].SetPos(array11[array8[i]]);
							this.headEyeIndex[i] = array12[array8[i]];
							this.maidArray[i].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
							this.maidArray[i].body0.boHeadToCam = true;
							this.maidArray[i].body0.boEyeToCam = true;
							if (this.headEyeIndex[i] == 0)
							{
								this.maidArray[i].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
								this.maidArray[i].body0.boHeadToCam = true;
								this.maidArray[i].body0.boEyeToCam = true;
							}
							else
							{
								this.maidArray[i].body0.trsLookTarget = null;
								if (this.headEyeIndex[i] == 1)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.7f, 1f, -0.6f);
								}
								if (this.headEyeIndex[i] == 2)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.6f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 3)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.7f, 1f, 0.6f);
								}
								if (this.headEyeIndex[i] == 4)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.1f, 1f, -0.4f);
								}
								if (this.headEyeIndex[i] == 5)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 6)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.1f, 1f, 0.4f);
								}
								if (this.headEyeIndex[i] == 7)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.4f, 1f, -0.5f);
								}
								if (this.headEyeIndex[i] == 8)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.3f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 9)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.4f, 1f, 0.5f);
								}
							}
						}
					}
					else if (Input.GetKeyDown(KeyCode.Comma))
					{
						int[] array9 = new int[this.maidCnt];
						Vector3[] array10 = new Vector3[this.maidCnt];
						Vector3[] array11 = new Vector3[this.maidCnt];
						int[] array12 = new int[this.maidCnt];
						for (int i = 0; i < this.maidCnt; i++)
						{
							array9[i] = this.poseIndex[i];
							array10[i] = this.maidArray[i].transform.localRotation.eulerAngles;
							array11[i] = this.maidArray[i].transform.position;
							array12[i] = this.headEyeIndex[i];
						}
						for (int i = 0; i < this.maidCnt; i++)
						{
							if (i == 0)
							{
								if (!this.isLock[i])
								{
									if (this.maidArray[i] && this.maidArray[i].Visible)
									{
										string[] array13 = this.poseArray[array9[this.maidCnt - i - 1]].Split(new char[]
										{
											','
										});
										this.isStop[i] = false;
										this.poseCount[i] = 20;
										Maid maid = this.maidArray[i];
										if (array13[0].Contains("_momi") || array13[0].Contains("paizuri_"))
										{
											maid.body0.MuneYureL(0f);
											maid.body0.MuneYureR(0f);
										}
										else
										{
											maid.body0.MuneYureL(1f);
											maid.body0.MuneYureR(1f);
										}
										int num6;
										if (array13[0].Contains("MultipleMaidsPose"))
										{
											string path = array13[0].Split(new char[]
											{
												'/'
											})[1];
											byte[] array2 = new byte[0];
											try
											{
												using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
												string text = Path.GetFileName(path);
												long num5 = (long)text.GetHashCode();
												maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
												Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
												{
													default(Maid.AutoTwist),
													Maid.AutoTwist.ShoulderR,
													Maid.AutoTwist.WristL,
													Maid.AutoTwist.WristR,
													Maid.AutoTwist.ThighL,
													Maid.AutoTwist.ThighR
												};
												for (int m = 0; m < array3.Length; m++)
												{
													maid.SetAutoTwist(array3[m], true);
												}
											}
										}
										else if (array13[0].StartsWith("p") && int.TryParse(array13[0].Substring(1), out num6))
										{
											this.loadPose[i] = array13[0];
										}
										else if (!array13[0].StartsWith("dance_"))
										{
											this.maidArray[i].CrossFade(array13[0] + ".anm", false, true, false, 0f, 1f);
										}
										else
										{
											if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array13[0] + ".anm"))
											{
												this.maidArray[i].body0.LoadAnime(array13[0] + ".anm", GameUty.FileSystem, array13[0] + ".anm", false, false);
											}
											this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array13[0] + ".anm");
										}
										if (array13.Length > 1)
										{
											this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array13[0] + ".anm"].time = float.Parse(array13[1]);
											this.isStop[i] = true;
											if (array13.Length > 2)
											{
												Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
												this.isPoseIti[i] = true;
												this.poseIti[i] = this.maidArray[i].transform.position;
												this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
											}
										}
									}
								}
								this.poseIndex[i] = array9[this.maidCnt - i - 1];
								this.headEyeIndex[i] = array12[this.maidCnt - i - 1];
							}
							else
							{
								if (!this.isLock[i])
								{
									if (this.maidArray[i] && this.maidArray[i].Visible)
									{
										string[] array13 = this.poseArray[array9[i - 1]].Split(new char[]
										{
											','
										});
										this.isStop[i] = false;
										this.poseCount[i] = 20;
										Maid maid = this.maidArray[i];
										if (array13[0].Contains("_momi") || array13[0].Contains("paizuri_"))
										{
											maid.body0.MuneYureL(0f);
											maid.body0.MuneYureR(0f);
										}
										else
										{
											maid.body0.MuneYureL(1f);
											maid.body0.MuneYureR(1f);
										}
										int num6;
										if (array13[0].Contains("MultipleMaidsPose"))
										{
											string path = array13[0].Split(new char[]
											{
												'/'
											})[1];
											byte[] array2 = new byte[0];
											try
											{
												using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
												string text = Path.GetFileName(path);
												long num5 = (long)text.GetHashCode();
												maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
												Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
												{
													default(Maid.AutoTwist),
													Maid.AutoTwist.ShoulderR,
													Maid.AutoTwist.WristL,
													Maid.AutoTwist.WristR,
													Maid.AutoTwist.ThighL,
													Maid.AutoTwist.ThighR
												};
												for (int m = 0; m < array3.Length; m++)
												{
													maid.SetAutoTwist(array3[m], true);
												}
											}
										}
										else if (array13[0].StartsWith("p") && int.TryParse(array13[0].Substring(1), out num6))
										{
											this.loadPose[i] = array13[0];
										}
										else if (!array13[0].StartsWith("dance_"))
										{
											this.maidArray[i].CrossFade(array13[0] + ".anm", false, true, false, 0f, 1f);
										}
										else
										{
											if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array13[0] + ".anm"))
											{
												this.maidArray[i].body0.LoadAnime(array13[0] + ".anm", GameUty.FileSystem, array13[0] + ".anm", false, false);
											}
											this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array13[0] + ".anm");
										}
										if (array13.Length > 1)
										{
											this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array13[0] + ".anm"].time = float.Parse(array13[1]);
											this.isStop[i] = true;
											if (array13.Length > 2)
											{
												Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
												this.isPoseIti[i] = true;
												this.poseIti[i] = this.maidArray[i].transform.position;
												this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
											}
										}
									}
								}
								this.poseIndex[i] = array9[i - 1];
								this.headEyeIndex[i] = array12[i - 1];
							}
							this.maidArray[i].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
							this.maidArray[i].body0.boHeadToCam = true;
							this.maidArray[i].body0.boEyeToCam = true;
							if (this.headEyeIndex[i] == 0)
							{
								this.maidArray[i].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
								this.maidArray[i].body0.boHeadToCam = true;
								this.maidArray[i].body0.boEyeToCam = true;
							}
							else
							{
								this.maidArray[i].body0.trsLookTarget = null;
								if (this.headEyeIndex[i] == 1)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.7f, 1f, -0.6f);
								}
								if (this.headEyeIndex[i] == 2)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.6f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 3)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.7f, 1f, 0.6f);
								}
								if (this.headEyeIndex[i] == 4)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.1f, 1f, -0.4f);
								}
								if (this.headEyeIndex[i] == 5)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 6)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(-0.1f, 1f, 0.4f);
								}
								if (this.headEyeIndex[i] == 7)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.4f, 1f, -0.5f);
								}
								if (this.headEyeIndex[i] == 8)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.3f, 1f, 0f);
								}
								if (this.headEyeIndex[i] == 9)
								{
									this.maidArray[i].body0.offsetLookTarget = new Vector3(0.4f, 1f, 0.5f);
								}
							}
							if (i == 0)
							{
								this.maidArray[i].SetRot(array10[this.maidCnt - i - 1]);
								this.maidArray[i].SetPos(array11[this.maidCnt - i - 1]);
							}
							else
							{
								this.maidArray[i].SetRot(array10[i - 1]);
								this.maidArray[i].SetPos(array11[i - 1]);
							}
						}
					}
					for (int i = 0; i < this.maidCnt; i++)
					{
						if (Input.GetKey(KeyCode.B) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								this.maidArray[i].body0.transform.localRotation = Quaternion.Euler(this.maidArray[i].body0.transform.localEulerAngles) * Quaternion.AngleAxis(-1.5f, Vector3.up);
							}
						}
						else if (Input.GetKey(KeyCode.B))
						{
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								this.maidArray[i].body0.transform.localRotation = Quaternion.Euler(this.maidArray[i].body0.transform.localEulerAngles) * Quaternion.AngleAxis(1.5f, Vector3.up);
							}
						}
						if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftAlt) && !this.isLock[i])
						{
							if (this.maidArray[1] && this.maidArray[1].Visible)
							{
								if (this.maidArray[0].transform.position == this.maidArray[1].transform.position || (this.maidArray[2] && this.maidArray[0].transform.position == this.maidArray[2].transform.position))
								{
									Vector3 vector = Vector3.zero;
									vector.x = 0.6f;
									this.maidArray[1].SetPos(vector);
									if (this.maidArray[2] && this.maidArray[2].Visible)
									{
										vector.x = -0.6f;
										this.maidArray[2].SetPos(vector);
									}
									if (this.maidArray[3] && this.maidArray[3].Visible)
									{
										vector.x = 1.2f;
										this.maidArray[3].SetPos(vector);
									}
									if (this.maidArray[4] && this.maidArray[4].Visible)
									{
										vector.x = -1.2f;
										this.maidArray[4].SetPos(vector);
									}
									if (this.maidArray[5] && this.maidArray[5].Visible)
									{
										vector.x = 1.8f;
										this.maidArray[5].SetPos(vector);
									}
									if (this.maidArray[6] && this.maidArray[6].Visible)
									{
										vector.x = -1.8f;
										this.maidArray[6].SetPos(vector);
									}
									if (this.maidArray[7] && this.maidArray[7].Visible)
									{
										vector.x = 2.4f;
										this.maidArray[7].SetPos(vector);
									}
									if (this.maidArray[8] && this.maidArray[8].Visible)
									{
										vector.x = -2.4f;
										this.maidArray[8].SetPos(vector);
									}
									if (this.maidArray[9] && this.maidArray[9].Visible)
									{
										vector.x = 3f;
										this.maidArray[9].SetPos(vector);
									}
									if (this.maidArray[10] && this.maidArray[10].Visible)
									{
										vector.x = -3f;
										this.maidArray[10].SetPos(vector);
									}
									if (this.maidArray[11] && this.maidArray[11].Visible)
									{
										vector.x = 3.6f;
										this.maidArray[11].SetPos(vector);
									}
									if (this.maidArray[12] && this.maidArray[12].Visible)
									{
										vector.x = -3.6f;
										this.maidArray[12].SetPos(vector);
									}
									if (this.maidArray[13] && this.maidArray[13].Visible)
									{
										vector.x = 4.2f;
										this.maidArray[13].SetPos(vector);
									}
								}
							}
							for (int k = 0; k < this.groupList.Count; k++)
							{
								if (k == 0 && this.poseIndex[i] <= (int)this.groupList[k])
								{
									if (this.poseIndex[i] == 0)
									{
										this.poseIndex[i] = (int)this.groupList[this.groupList.Count - 1];
									}
									else
									{
										this.poseIndex[i] = 0;
									}
									break;
								}
								if (k > 0 && this.poseIndex[i] > (int)this.groupList[k - 1] && this.poseIndex[i] <= (int)this.groupList[k])
								{
									this.poseIndex[i] = (int)this.groupList[k - 1];
									break;
								}
							}
							if (this.poseIndex[i] > (int)this.groupList[this.groupList.Count - 1])
							{
								this.poseIndex[i] = (int)this.groupList[this.groupList.Count - 1];
							}
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								Maid maid = this.maidArray[i];
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
								}
								if (array.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
						}
						else if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.RightAlt) && !this.isLock[i])
						{
							if (this.maidArray[1] && this.maidArray[1].Visible)
							{
								if (this.maidArray[0].transform.position == this.maidArray[1].transform.position || (this.maidArray[2] && this.maidArray[0].transform.position == this.maidArray[2].transform.position))
								{
									Vector3 vector = Vector3.zero;
									vector.x = 0.6f;
									this.maidArray[1].SetPos(vector);
									if (this.maidArray[2] && this.maidArray[2].Visible)
									{
										vector.x = -0.6f;
										this.maidArray[2].SetPos(vector);
									}
									if (this.maidArray[3] && this.maidArray[3].Visible)
									{
										vector.x = 1.2f;
										this.maidArray[3].SetPos(vector);
									}
									if (this.maidArray[4] && this.maidArray[4].Visible)
									{
										vector.x = -1.2f;
										this.maidArray[4].SetPos(vector);
									}
									if (this.maidArray[5] && this.maidArray[5].Visible)
									{
										vector.x = 1.8f;
										this.maidArray[5].SetPos(vector);
									}
									if (this.maidArray[6] && this.maidArray[6].Visible)
									{
										vector.x = -1.8f;
										this.maidArray[6].SetPos(vector);
									}
									if (this.maidArray[7] && this.maidArray[7].Visible)
									{
										vector.x = 2.4f;
										this.maidArray[7].SetPos(vector);
									}
									if (this.maidArray[8] && this.maidArray[8].Visible)
									{
										vector.x = -2.4f;
										this.maidArray[8].SetPos(vector);
									}
									if (this.maidArray[9] && this.maidArray[9].Visible)
									{
										vector.x = 3f;
										this.maidArray[9].SetPos(vector);
									}
									if (this.maidArray[10] && this.maidArray[10].Visible)
									{
										vector.x = -3f;
										this.maidArray[10].SetPos(vector);
									}
									if (this.maidArray[11] && this.maidArray[11].Visible)
									{
										vector.x = 3.6f;
										this.maidArray[11].SetPos(vector);
									}
									if (this.maidArray[12] && this.maidArray[12].Visible)
									{
										vector.x = -3.6f;
										this.maidArray[12].SetPos(vector);
									}
									if (this.maidArray[13] && this.maidArray[13].Visible)
									{
										vector.x = 4.2f;
										this.maidArray[13].SetPos(vector);
									}
								}
							}
							int num11 = this.poseIndex[i];
							for (int k = 0; k < this.groupList.Count; k++)
							{
								if (this.poseIndex[i] < (int)this.groupList[k])
								{
									this.poseIndex[i] = (int)this.groupList[k];
									break;
								}
							}
							if (num11 == this.poseIndex[i] && this.poseIndex[i] >= (int)this.groupList[this.groupList.Count - 1])
							{
								this.poseIndex[i] = 0;
							}
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								Maid maid = this.maidArray[i];
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
								}
								if (array.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
						}
						else if (Input.GetKeyDown(KeyCode.N) && this.getModKeyPressing(MultipleMaids.modKey.Shift) && !this.isLock[i])
						{
							if (this.maidArray[1] && this.maidArray[1].Visible)
							{
								if (this.maidArray[0].transform.position == this.maidArray[1].transform.position || (this.maidArray[2] && this.maidArray[0].transform.position == this.maidArray[2].transform.position))
								{
									Vector3 vector = Vector3.zero;
									vector.x = 0.6f;
									this.maidArray[1].SetPos(vector);
									if (this.maidArray[2] && this.maidArray[2].Visible)
									{
										vector.x = -0.6f;
										this.maidArray[2].SetPos(vector);
									}
									if (this.maidArray[3] && this.maidArray[3].Visible)
									{
										vector.x = 1.2f;
										this.maidArray[3].SetPos(vector);
									}
									if (this.maidArray[4] && this.maidArray[4].Visible)
									{
										vector.x = -1.2f;
										this.maidArray[4].SetPos(vector);
									}
									if (this.maidArray[5] && this.maidArray[5].Visible)
									{
										vector.x = 1.8f;
										this.maidArray[5].SetPos(vector);
									}
									if (this.maidArray[6] && this.maidArray[6].Visible)
									{
										vector.x = -1.8f;
										this.maidArray[6].SetPos(vector);
									}
									if (this.maidArray[7] && this.maidArray[7].Visible)
									{
										vector.x = 2.4f;
										this.maidArray[7].SetPos(vector);
									}
									if (this.maidArray[8] && this.maidArray[8].Visible)
									{
										vector.x = -2.4f;
										this.maidArray[8].SetPos(vector);
									}
									if (this.maidArray[9] && this.maidArray[9].Visible)
									{
										vector.x = 3f;
										this.maidArray[9].SetPos(vector);
									}
									if (this.maidArray[10] && this.maidArray[10].Visible)
									{
										vector.x = -3f;
										this.maidArray[10].SetPos(vector);
									}
									if (this.maidArray[11] && this.maidArray[11].Visible)
									{
										vector.x = 3.6f;
										this.maidArray[11].SetPos(vector);
									}
									if (this.maidArray[12] && this.maidArray[12].Visible)
									{
										vector.x = -3.6f;
										this.maidArray[12].SetPos(vector);
									}
									if (this.maidArray[13] && this.maidArray[13].Visible)
									{
										vector.x = 4.2f;
										this.maidArray[13].SetPos(vector);
									}
								}
							}
							this.poseIndex[i]--;
							if (this.poseIndex[i] <= -1)
							{
								this.poseIndex[i] = this.poseArray.Length - 1;
							}
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								Maid maid = this.maidArray[i];
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
								}
								if (array.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
						}
						else if (Input.GetKeyDown(KeyCode.N) && !this.isLock[i])
						{
							if (this.maidArray[1] && this.maidArray[1].Visible)
							{
								if (this.maidArray[0].transform.position == this.maidArray[1].transform.position || (this.maidArray[2] && this.maidArray[0].transform.position == this.maidArray[2].transform.position))
								{
									Vector3 vector = Vector3.zero;
									vector.x = 0.6f;
									this.maidArray[1].SetPos(vector);
									if (this.maidArray[2] && this.maidArray[2].Visible)
									{
										vector.x = -0.6f;
										this.maidArray[2].SetPos(vector);
									}
									if (this.maidArray[3] && this.maidArray[3].Visible)
									{
										vector.x = 1.2f;
										this.maidArray[3].SetPos(vector);
									}
									if (this.maidArray[4] && this.maidArray[4].Visible)
									{
										vector.x = -1.2f;
										this.maidArray[4].SetPos(vector);
									}
									if (this.maidArray[5] && this.maidArray[5].Visible)
									{
										vector.x = 1.8f;
										this.maidArray[5].SetPos(vector);
									}
									if (this.maidArray[6] && this.maidArray[6].Visible)
									{
										vector.x = -1.8f;
										this.maidArray[6].SetPos(vector);
									}
									if (this.maidArray[7] && this.maidArray[7].Visible)
									{
										vector.x = 2.4f;
										this.maidArray[7].SetPos(vector);
									}
									if (this.maidArray[8] && this.maidArray[8].Visible)
									{
										vector.x = -2.4f;
										this.maidArray[8].SetPos(vector);
									}
									if (this.maidArray[9] && this.maidArray[9].Visible)
									{
										vector.x = 3f;
										this.maidArray[9].SetPos(vector);
									}
									if (this.maidArray[10] && this.maidArray[10].Visible)
									{
										vector.x = -3f;
										this.maidArray[10].SetPos(vector);
									}
									if (this.maidArray[11] && this.maidArray[11].Visible)
									{
										vector.x = 3.6f;
										this.maidArray[11].SetPos(vector);
									}
									if (this.maidArray[12] && this.maidArray[12].Visible)
									{
										vector.x = -3.6f;
										this.maidArray[12].SetPos(vector);
									}
									if (this.maidArray[13] && this.maidArray[13].Visible)
									{
										vector.x = 4.2f;
										this.maidArray[13].SetPos(vector);
									}
								}
							}
							this.poseIndex[i]++;
							if (Input.GetKey(KeyCode.Space))
							{
								this.poseIndex[i] += 9;
							}
							if (this.poseIndex[i] == this.poseArray.Length)
							{
								this.poseIndex[i] = 0;
							}
							if (this.maidArray[i] && this.maidArray[i].Visible)
							{
								string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = false;
								this.poseCount[i] = 20;
								Maid maid = this.maidArray[i];
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
								int num6;
								if (array[0].Contains("MultipleMaidsPose"))
								{
									string path = array[0].Split(new char[]
									{
										'/'
									})[1];
									byte[] array2 = new byte[0];
									try
									{
										using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
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
										string text = Path.GetFileName(path);
										long num5 = (long)text.GetHashCode();
										maid.body0.CrossFade(num5.ToString(), array2, false, false, false, 0f, 1f);
										Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
										{
											default(Maid.AutoTwist),
											Maid.AutoTwist.ShoulderR,
											Maid.AutoTwist.WristL,
											Maid.AutoTwist.WristR,
											Maid.AutoTwist.ThighL,
											Maid.AutoTwist.ThighR
										};
										for (int m = 0; m < array3.Length; m++)
										{
											maid.SetAutoTwist(array3[m], true);
										}
									}
								}
								else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num6))
								{
									this.loadPose[i] = array[0];
								}
								else if (!array[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array[0] + ".anm", GameUty.FileSystem, array[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array[0] + ".anm");
								}
								if (array.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array[0] + ".anm"].time = float.Parse(array[1]);
									this.isStop[i] = true;
									if (array.Length > 2)
									{
										Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
							}
						}
					}
				}
				if (this.isDanceChu)
				{
					int j = 0;
					while (j < this.maidCnt)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							string text3 = this.danceName[j];
							float num = 0f;
							if (text3 == "dance_cmo_002_sd_f1.anm" || text3 == "dance_cmo_002_sd_f2.anm" || text3 == "dance_cm3d_004_kano_f1.anm")
							{
								num = 0.166666f;
							}
							if (text3 == "dance_cm3d21_kara_001_nmf_f1.anm")
							{
								num = -0.35f;
							}
							if (text3 != null)
							{
								if (this.danceCount > 0)
								{
									this.danceCount--;
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>()[text3].time = this.audioSourceBgm.time - 0.03333f - num;
								}
								if (this.maidArray[j].body0.m_Bones.GetComponent<Animation>()[text3].time + num < this.audioSourceBgm.time - 0.1f)
								{
									this.danceCount = 20;
								}
							}
						}
						//IL_1C012:
						j++;
						continue;
						//goto IL_1C012;
					}
				}
				if (this.isDanceStart1)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								if (j == 0)
								{
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_001_f1.anm");
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_001_f1.anm"].time = 0f;
									this.danceName[j] = "dance_cm3d2_001_f1.anm";
								}
								else if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
								{
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_001_f2.anm");
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_001_f2.anm"].time = 0f;
									this.danceName[j] = "dance_cm3d2_001_f2.anm";
								}
								else
								{
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_001_f3.anm");
									this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_001_f3.anm"].time = 0f;
									this.danceName[j] = "dance_cm3d2_001_f3.anm";
								}
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara_003_ddfl_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara_003_ddfl_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara_003_ddfl_f1.anm";
							}
						}
					}
					this.isDanceStart1 = false;
					this.isDanceStart1F = true;
					this.isShift = false;
				}
				if (this.isDanceStart2)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d_001_f1.anm");
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d_001_f1.anm"].time = 0f;
							this.danceName[j] = "dance_cm3d_001_f1.anm";
						}
					}
					this.isDanceStart2 = false;
					this.isDanceStart2F = true;
				}
				if (this.isDanceStart3)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d_002_end_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d_002_end_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d_002_end_f1.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara_001_sl_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara_001_sl_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara_001_sl_f1.anm";
							}
						}
					}
					this.isDanceStart3 = false;
					this.isDanceStart3F = true;
					this.isShift = false;
				}
				if (this.isDanceStart4)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_002_smt_f.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_002_smt_f.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_002_smt_f.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara02_001_smt_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara02_001_smt_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara02_001_smt_f1.anm";
							}
						}
					}
					this.isDanceStart4 = false;
					this.isDanceStart4F = true;
					this.isShift = false;
				}
				if (this.isDanceStart5)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d_003_sp2_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d_003_sp2_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d_003_sp2_f1.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara02_002_rty_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara02_002_rty_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara02_002_rty_f1.anm";
							}
						}
					}
					this.isDanceStart5 = false;
					this.isDanceStart5F = true;
					this.isShift = false;
				}
				if (this.isDanceStart6)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f1.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara02_003_hs_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara02_003_hs_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara02_003_hs_f1.anm";
							}
						}
					}
					this.isDanceStart6 = false;
					this.isDanceStart6F = true;
					this.isShift = false;
				}
				if (this.isDanceStart7)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f2.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f2.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f2.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f1.anm";
							}
						}
					}
					this.isDanceStart7 = false;
					this.isDanceStart7V = true;
					this.isDanceStart7F = true;
				}
				if (this.isDanceStart8)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (j == 0 || j == 4 || j == 8 || j == 12)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f1.anm";
							}
							else if (j == 1 || j == 5 || j == 9 || j == 13)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f2.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f2.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f2.anm";
							}
							else if (j == 2 || j == 6 || j == 10)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f3.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f3.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f3.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_003_hs_f4.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_003_hs_f4.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_003_hs_f4.anm";
							}
						}
					}
					this.isDanceStart8 = false;
					this.isDanceStart8V = true;
					this.isDanceStart8F = true;
				}
				if (this.isDanceStart9)
				{
					this.isDanceStart9Count++;
					if (this.isShift)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_kara_002_cktc_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_kara_002_cktc_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_kara_002_cktc_f1.anm";
							}
						}
						this.isDanceStart9 = false;
						this.isDanceStart9F = true;
						this.isDanceStart9Count = 0;
						this.isShift = false;
					}
					if (this.isDanceStart9Count == 10)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d_004_kano_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d_004_kano_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d_004_kano_f1.anm";
							}
						}
						this.isDanceStart9 = false;
						this.isDanceStart9F = true;
						this.isDanceStart9Count = 0;
					}
				}
				if (this.isDanceStart10)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_004_sse_f1.anm");
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_004_sse_f1.anm"].time = 0f;
							this.danceName[j] = "dance_cm3d2_004_sse_f1.anm";
						}
					}
					this.isDanceStart10 = false;
					this.isDanceStart10F = true;
				}
				if (this.isDanceStart11)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (j == 1 || j == 4 || j == 7 || j == 10 || j == 13)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play(this.nameA + this.danceNo2 + ".anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()[this.nameA + this.danceNo2 + ".anm"].time = 0f;
								this.danceName[j] = this.nameA + this.danceNo2 + ".anm";
							}
							else if (j == 2 || j == 5 || j == 8 || j == 11)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play(this.nameA + this.danceNo3 + ".anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()[this.nameA + this.danceNo3 + ".anm"].time = 0f;
								this.danceName[j] = this.nameA + this.danceNo3 + ".anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play(this.nameA + this.danceNo1 + ".anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()[this.nameA + this.danceNo1 + ".anm"].time = 0f;
								this.danceName[j] = this.nameA + this.danceNo1 + ".anm";
							}
						}
					}
					this.isDanceStart11 = false;
					this.isDanceStart11V = true;
					this.isDanceStart11F = true;
				}
				if (this.isDanceStart12)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_005_khg_f.anm");
							this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_005_khg_f.anm"].time = 0f;
							this.danceName[j] = "dance_cm3d2_005_khg_f.anm";
						}
					}
					this.isDanceStart12 = false;
					this.isDanceStart12F = true;
				}
				if (this.isDanceStart13)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (!this.isShift)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_006_ssn_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_006_ssn_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_006_ssn_f1.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d21_kara_001_nmf_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d21_kara_001_nmf_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d21_kara_001_nmf_f1.anm";
							}
						}
					}
					this.isDanceStart13 = false;
					this.isDanceStart13F = true;
					this.isShift = false;
				}
				if (this.isDanceStart14)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (j == 1 || j == 4 || j == 7 || j == 10 || j == 13)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_006_ssn_f2.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_006_ssn_f2.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_006_ssn_f2.anm";
							}
							else if (j == 2 || j == 5 || j == 8 || j == 11)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_006_ssn_f3.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_006_ssn_f3.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_006_ssn_f3.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cm3d2_006_ssn_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cm3d2_006_ssn_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cm3d2_006_ssn_f1.anm";
							}
						}
					}
					this.isDanceStart14 = false;
					this.isDanceStart14V = true;
					this.isDanceStart14F = true;
				}
				if (this.isDanceStart15)
				{
					this.isDanceStart15Count++;
				}
				if (this.isDanceStart15Count == 10)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.maidArray[j].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
							if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cmo_002_sd_f2.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cmo_002_sd_f2.anm"].time = 0f;
								this.danceName[j] = "dance_cmo_002_sd_f2.anm";
							}
							else
							{
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>().Play("dance_cmo_002_sd_f1.anm");
								this.maidArray[j].body0.m_Bones.GetComponent<Animation>()["dance_cmo_002_sd_f1.anm"].time = 0f;
								this.danceName[j] = "dance_cmo_002_sd_f1.anm";
							}
						}
					}
					this.isDanceStart15 = false;
					this.isDanceStart15V = true;
					this.isDanceStart15F = true;
					this.isDanceStart15Count = 0;
				}
				if (this.isDance)
				{
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j].Visible)
						{
							if (this.isDanceStart1F)
							{
								if (this.isDanceStart1K)
								{
									foreach (string text4 in this.dance1KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 0)
								{
									foreach (string text4 in this.dance1Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
								{
									foreach (string text4 in this.dance1BArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance1CArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart2F)
							{
								foreach (string text4 in this.dance2Array)
								{
									string[] array = text4.Split(new char[]
									{
										','
									});
									float num14 = float.Parse(array[0]);
									if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
									{
										this.danceFace[j] = num14;
										this.FaceName[j] = this.FaceName2[j];
										this.FaceName2[j] = array[1];
										this.FaceName3[j] = array[2];
										this.FaceTime[j] = 0.0166666675f;
										break;
									}
								}
							}
							if (this.isDanceStart3F)
							{
								if (this.isDanceStart3K)
								{
									foreach (string text4 in this.dance3KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance3Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart4F)
							{
								if (this.isDanceStart4K)
								{
									foreach (string text4 in this.dance4KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance4Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart5F)
							{
								if (this.isDanceStart5K)
								{
									foreach (string text4 in this.dance5KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance5Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart6F)
							{
								if (this.isDanceStart6K)
								{
									foreach (string text4 in this.dance6KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance6Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart7F)
							{
								if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
								{
									foreach (string text4 in this.dance6BArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance6Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart7V && (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13))
							{
								if (this.audioSourceBgm.time > 42f && this.audioSourceBgm.time < 43f)
								{
									this.maidArray[j].SetPos(this.maidArray[j - 1].transform.position);
								}
								if (this.audioSourceBgm.time > 58.17f && this.audioSourceBgm.time < 60f)
								{
									this.maidArray[j].SetPos(new Vector3(this.maidArray[j - 1].transform.position.x, 100f, this.maidArray[j - 1].transform.position.z));
								}
							}
							if (this.isDanceStart8F)
							{
								if (j == 1 || j == 5 || j == 9 || j == 13)
								{
									foreach (string text4 in this.dance6BArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 2 || j == 6 || j == 10)
								{
									foreach (string text4 in this.dance6CArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 3 || j == 9 || j == 11)
								{
									foreach (string text4 in this.dance6DArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance6Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart8V && (j == 1 || j == 5 || j == 9 || j == 13))
							{
								if (this.audioSourceBgm.time > 42f && this.audioSourceBgm.time < 43f)
								{
									this.maidArray[j].SetPos(this.maidArray[j - 1].transform.position);
								}
								if (this.audioSourceBgm.time > 58.17f && this.audioSourceBgm.time < 60f)
								{
									this.maidArray[j].SetPos(new Vector3(this.maidArray[j - 1].transform.position.x, 100f, this.maidArray[j - 1].transform.position.z));
								}
							}
							if (this.isDanceStart8V)
							{
								if (!this.isDanceStart8P && this.audioSourceBgm.time > 40f && this.audioSourceBgm.time < 41f)
								{
									this.isDanceStart8P = true;
									for (int num15 = 0; num15 < this.maidCnt; num15++)
									{
										if (this.maidArray[num15].Visible)
										{
											if (num15 == 0 || num15 == 4 || num15 == 8 || num15 == 12)
											{
												Object original = Resources.Load("Prefab/Particle/pHeart01");
												GameObject gameObject6 = Object.Instantiate(original) as GameObject;
												gameObject6.transform.position = CMT.SearchObjName(this.maidArray[num15].body0.m_Bones.transform, "Bip01 Spine", true).position;
											}
										}
									}
								}
							}
							if (this.isDanceStart9F)
							{
								if (this.isDanceStart9K)
								{
									foreach (string text4 in this.dance9KArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance7Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart10F)
							{
								foreach (string text4 in this.danceO1Array)
								{
									string[] array = text4.Split(new char[]
									{
										','
									});
									float num14 = float.Parse(array[0]);
									if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
									{
										this.danceFace[j] = num14;
										this.FaceName[j] = this.FaceName2[j];
										this.FaceName2[j] = array[1];
										this.FaceName3[j] = array[2];
										this.FaceTime[j] = 0.0166666675f;
										break;
									}
								}
							}
							if (this.isDanceStart11F)
							{
								string[] array15 = null;
								string[] array13 = null;
								string[] array16 = null;
								if (this.isSS1)
								{
									array15 = this.danceO1Array;
									array13 = this.danceO1CArray;
									array16 = this.danceO1BArray;
								}
								if (this.isSS2)
								{
									array15 = this.danceO2Array;
									array13 = this.danceO2BArray;
									array16 = this.danceO2CArray;
								}
								if (this.isSS3)
								{
									array15 = this.danceO3CArray;
									array13 = this.danceO3Array;
									array16 = this.danceO3BArray;
								}
								if (this.isSS4)
								{
									array15 = this.danceO5BArray;
									array13 = this.danceO5Array;
									array16 = this.danceO5CArray;
								}
								if (this.isSS5)
								{
									array15 = this.danceO4Array;
									array13 = this.danceO4BArray;
									array16 = this.danceO4CArray;
								}
								if (j == 1 || j == 4 || j == 7 || j == 10 || j == 13)
								{
									foreach (string text4 in array13)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]) - 0.03f;
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 2 || j == 5 || j == 8 || j == 11)
								{
									foreach (string text4 in array16)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]) - 0.03f;
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in array15)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]) - 0.03f;
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart12F)
							{
								foreach (string text4 in this.dance12Array)
								{
									string[] array = text4.Split(new char[]
									{
										','
									});
									float num14 = float.Parse(array[0]);
									if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
									{
										this.danceFace[j] = num14;
										this.FaceName[j] = this.FaceName2[j];
										this.FaceName2[j] = array[1];
										this.FaceName3[j] = array[2];
										this.FaceTime[j] = 0.0166666675f;
										break;
									}
								}
							}
							if (this.isDanceStart13F)
							{
								if (!this.isDanceStart13K)
								{
									foreach (string text4 in this.dance13Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart14F)
							{
								if (j == 1 || j == 4 || j == 7 || j == 10 || j == 13)
								{
									foreach (string text4 in this.dance13BArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else if (j == 2 || j == 5 || j == 8 || j == 11)
								{
									foreach (string text4 in this.dance13CArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance13Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (this.isDanceStart15F)
							{
								if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
								{
									foreach (string text4 in this.dance15BArray)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
								else
								{
									foreach (string text4 in this.dance15Array)
									{
										string[] array = text4.Split(new char[]
										{
											','
										});
										float num14 = float.Parse(array[0]);
										if (this.danceFace[j] < num14 && num14 < this.audioSourceBgm.time)
										{
											this.danceFace[j] = num14;
											this.FaceName[j] = this.FaceName2[j];
											this.FaceName2[j] = array[1];
											this.FaceName3[j] = array[2];
											this.FaceTime[j] = 0.0166666675f;
											break;
										}
									}
								}
							}
							if (!this.isFadeOut)
							{
								Maid maid = this.maidArray[j];
								if (maid && maid.Visible)
								{
									if (maid.transform.position.y != 100f)
									{
										this.FaceTime[j] += Time.deltaTime;
										if (this.FaceTime[j] < 1f)
										{
											TMorph morph = maid.body0.Face.morph;
											maid.boMabataki = false;
											maid.body0.Face.morph.EyeMabataki = 0f;
											if (this.FaceName[j] != string.Empty)
											{
												maid.body0.Face.morph.MulBlendValues(this.FaceName[j], 1f);
											}
											if (this.FaceName2[j] != "")
											{
												maid.body0.Face.morph.MulBlendValues(this.FaceName2[j], UTY.COSS2(Mathf.Pow(this.FaceTime[j], 0.4f), 4f));
											}
											if (this.FaceName3[j] != string.Empty)
											{
												maid.body0.Face.morph.AddBlendValues(this.FaceName3[j], 1f);
											}
											maid.body0.Face.morph.FixBlendValues_Face();
										}
										else
										{
											this.FaceName[j] = this.FaceName2[j];
										}
										if (this.isHenkou)
										{
											TMorph morph = maid.body0.Face.morph;
											float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
											maid.boMabataki = false;
											if (this.isNamidaH)
											{
												fieldValue[(int)morph.hash["namida"]] = 1f;
											}
											else
											{
												fieldValue[(int)morph.hash["namida"]] = 0f;
											}
											if (this.isSekimenH)
											{
												fieldValue[(int)morph.hash["hohol"]] = 1f;
											}
											else
											{
												fieldValue[(int)morph.hash["hohol"]] = 0f;
											}
											if (this.isHohoH)
											{
												fieldValue[(int)morph.hash["hoho2"]] = 1f;
											}
											else
											{
												fieldValue[(int)morph.hash["hoho2"]] = 0f;
											}
											maid.body0.Face.morph.FixBlendValues_Face();
										}
									}
								}
							}
							this.FoceKuchipakuUpdate2(this.audioSourceBgm.time, this.maidArray[j], j);
						}
					}
					if (Input.GetKey(KeyCode.H) || this.isVR)
					{
						if (Input.GetKeyDown(KeyCode.KeypadPeriod))
						{
							this.h2Flg = true;
							this.isNamidaH = !this.isNamidaH;
							this.isHenkou = true;
						}
						if (Input.GetKeyDown(KeyCode.KeypadPlus))
						{
							this.h2Flg = true;
							this.isSekimenH = !this.isSekimenH;
							this.isHenkou = true;
						}
						if (Input.GetKeyDown(KeyCode.KeypadEnter))
						{
							this.h2Flg = true;
							this.isHohoH = !this.isHohoH;
							this.isHenkou = true;
						}
					}
					this.danceCheckIndex++;
					if (this.danceCheckIndex == 10)
					{
						this.danceCheckIndex = 0;
					}
					this.danceCheck[this.danceCheckIndex] = this.audioSourceBgm.time;
					this.isDanceChu = false;
					for (int k = 0; k < this.danceCheck.Length; k++)
					{
						if (this.danceCheck[k] > 0f)
						{
							this.isDanceChu = true;
							break;
						}
					}
					if (!this.isDanceChu)
					{
						this.danceWait--;
						if (this.danceWait > 0)
						{
							this.isDanceChu = true;
						}
					}
					if (!this.isDanceChu)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								this.maidArray[j].StopKuchipakuPattern();
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
								if (this.isDanceStart8V && (j == 1 || j == 5 || j == 9 || j == 13))
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
									this.isStop[i] = true;
									Transform transform2 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
									this.isPoseIti[i] = true;
									this.poseIti[i] = this.maidArray[i].transform.position;
									this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
								}
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart1F = false;
						this.isDanceStart1K = false;
						this.isDanceStart2F = false;
						this.isDanceStart3F = false;
						this.isDanceStart3K = false;
						this.isDanceStart4F = false;
						this.isDanceStart4K = false;
						this.isDanceStart5F = false;
						this.isDanceStart5K = false;
						this.isDanceStart6F = false;
						this.isDanceStart6K = false;
						this.isDanceStart7F = false;
						this.isDanceStart7V = false;
						this.isDanceStart8F = false;
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
						this.isDanceStart9F = false;
						this.isDanceStart9K = false;
						this.isDanceStart10F = false;
						this.isDanceStart11F = false;
						this.isDanceStart11V = false;
						this.isDanceStart12F = false;
						this.isDanceStart13F = false;
						this.isDanceStart13K = false;
						this.isDanceStart14F = false;
						this.isDanceStart14V = false;
						this.isDanceStart15F = false;
						this.isDanceStart15V = false;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.danceFace[j] = 0f;
						}
						this.danceCheckIndex = 0;
						for (int k = 0; k < this.danceCheck.Length; k++)
						{
							this.danceCheck[this.danceCheckIndex] = 1f;
						}
						this.isDance = false;
					}
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha1)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Keypad1)))
				{
					if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.isShift = true;
					}
					TextAsset textAsset = Resources.Load("SceneDance/dance_kp_m0") as TextAsset;
					string text5 = Regex.Replace(textAsset.text, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					if (!this.isShift)
					{
						GameMain.Instance.CharacterMgr.ResetCharaPosAll();
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						string text6 = "dance_cm3d2_001_f1.anm";
						switch (j)
						{
							case 1:
							case 3:
							case 5:
							case 7:
							case 9:
							case 11:
							case 13:
								text6 = "dance_cm3d2_001_f2.anm";
								break;
							case 2:
							case 4:
							case 6:
							case 8:
							case 10:
							case 12:
								text6 = "dance_cm3d2_001_f3.anm";
								break;
						}
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara_003_ddfl_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara_003_ddfl_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara_003_ddfl_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/DDF_kara_voice_2") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("dokidokifallinlove_short.ogg", 0f, false);
						this.sw = new Stopwatch();
						this.sw.Start();
					}
					else if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("dokidokifallinlove_short_sasaki_kara.ogg", 0f, false);
					}
					else
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("dokidokifallinlove_short_nao_kara.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						Vector3 vector = Vector3.zero;
						switch (j)
						{
							case 3:
								vector.x = 1f;
								break;
							case 4:
								vector.x = -1f;
								break;
							case 5:
								vector.x = 2f;
								break;
							case 6:
								vector.x = -2f;
								break;
							case 7:
								vector.x = 3f;
								break;
							case 8:
								vector.x = -3f;
								break;
							case 9:
								vector.x = 4f;
								break;
							case 10:
								vector.x = -4f;
								break;
							case 11:
								vector.x = 5f;
								break;
							case 12:
								vector.x = -5f;
								break;
							case 13:
								vector.x = 6f;
								break;
						}
						if (!this.isShift)
						{
							this.maidArray[j].SetPos(vector);
						}
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.isShift)
					{
						if (this.maidArray[1] && this.maidArray[1].Visible)
						{
							if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
							{
								Vector3 vector = Vector3.zero;
								vector.x = 0.6f;
								this.maidArray[1].SetPos(vector);
								if (this.maidArray[2] && this.maidArray[2].Visible)
								{
									vector.x = -0.6f;
									this.maidArray[2].SetPos(vector);
								}
								if (this.maidArray[3] && this.maidArray[3].Visible)
								{
									vector.x = 1.2f;
									this.maidArray[3].SetPos(vector);
								}
								if (this.maidArray[4] && this.maidArray[4].Visible)
								{
									vector.x = -1.2f;
									this.maidArray[4].SetPos(vector);
								}
								if (this.maidArray[5] && this.maidArray[5].Visible)
								{
									vector.x = 1.8f;
									this.maidArray[5].SetPos(vector);
								}
								if (this.maidArray[6] && this.maidArray[6].Visible)
								{
									vector.x = -1.8f;
									this.maidArray[6].SetPos(vector);
								}
								if (this.maidArray[7] && this.maidArray[7].Visible)
								{
									vector.x = 2.4f;
									this.maidArray[7].SetPos(vector);
								}
								if (this.maidArray[8] && this.maidArray[8].Visible)
								{
									vector.x = -2.4f;
									this.maidArray[8].SetPos(vector);
								}
								if (this.maidArray[9] && this.maidArray[9].Visible)
								{
									vector.x = 3f;
									this.maidArray[9].SetPos(vector);
								}
								if (this.maidArray[10] && this.maidArray[10].Visible)
								{
									vector.x = -3f;
									this.maidArray[10].SetPos(vector);
								}
								if (this.maidArray[11] && this.maidArray[11].Visible)
								{
									vector.x = 3.6f;
									this.maidArray[11].SetPos(vector);
								}
								if (this.maidArray[12] && this.maidArray[12].Visible)
								{
									vector.x = -3.6f;
									this.maidArray[12].SetPos(vector);
								}
								if (this.maidArray[13] && this.maidArray[13].Visible)
								{
									vector.x = 4.2f;
									this.maidArray[13].SetPos(vector);
								}
							}
						}
						this.isDanceStart1K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart1 = true;
				}
				if (this.isF2)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_EtY_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d_001_f1.anm"))
						{
							this.maidArray[j].body0.LoadAnime("dance_cm3d_001_f1.anm", GameUty.FileSystem, "dance_cm3d_001_f1.anm", false, false);
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					GameMain.Instance.SoundMgr.PlayDanceBGM("entrancetoyou_short.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					this.isDance = true;
					this.isDanceStart2 = true;
					this.isF2 = false;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha2)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Keypad2)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isF2 = true;
				}
				if (this.isF3)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_Scl_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d_002_end_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d_002_end_f1.anm", GameUty.FileSystem, "dance_cm3d_002_end_f1.anm", false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara_001_sl_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara_001_sl_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara_001_sl_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/scaret_kara_voice_2") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("scarlet leap_short.ogg", 0f, false);
					}
					else
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("scarlet leap_short_kara_1.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart3K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart3 = true;
					this.isF3 = false;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha3)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Keypad3)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isF3 = true;
					if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.isShift = true;
					}
				}
				if (this.isSF1 || this.isSF2 || this.isSF3)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_SmT_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_002_smt_f.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_002_smt_f.anm", GameUty.FileSystem, "dance_cm3d2_002_smt_f.anm", false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara02_001_smt_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara02_001_smt_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara02_001_smt_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/stellar my tears_kara_voice") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						if (this.isSF1)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short.ogg", 0f, false);
						}
						if (this.isSF2)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short2.ogg", 0f, false);
						}
						if (this.isSF3)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short3.ogg", 0f, false);
						}
					}
					else
					{
						if (this.isSF1)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short_nao_kara.ogg", 0f, false);
						}
						if (this.isSF2)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short_sasaki_kara.ogg", 0f, false);
						}
						if (this.isSF3)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("stellarmytears_short_misato_kara.ogg", 0f, false);
						}
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart4K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart4 = true;
					this.isSF1 = false;
					this.isSF2 = false;
					this.isSF3 = false;
				}
				if (this.isHS1 || this.isHS2 || this.isHS3)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_Hhs_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_003_hs_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_003_hs_f1.anm", GameUty.FileSystem, "dance_cm3d2_003_hs_f1.anm", false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara02_003_hs_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara02_003_hs_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara02_003_hs_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/happy_happy_scandal_kara_voice") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						if (this.isHS1)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal1.ogg", 0f, false);
						}
						if (this.isHS2)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal2.ogg", 0f, false);
						}
						if (this.isHS3)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal3.ogg", 0f, false);
						}
					}
					else
					{
						if (this.isHS1)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happyhappyscandal_short_nao_kara.ogg", 0f, false);
						}
						if (this.isHS2)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal_sasaki_kara.ogg", 0f, false);
						}
						if (this.isHS3)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal_misato_kara.ogg", 0f, false);
						}
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart6K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart6 = true;
					this.isHS1 = false;
					this.isHS2 = false;
					this.isHS3 = false;
				}
				if (this.isHS4 || this.isHS5 || this.isHS6)
				{
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						TextAsset textAsset = Resources.Load("SceneDance/dance_Hhs_m0") as TextAsset;
						string text6 = "dance_cm3d2_003_hs_f1.anm";
						switch (j)
						{
							case 1:
							case 3:
							case 5:
							case 7:
							case 9:
							case 11:
							case 13:
								text6 = "dance_cm3d2_003_hs_f2.anm";
								textAsset = (Resources.Load("SceneDance/dance_Hhs_m1") as TextAsset);
								break;
						}
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
							string text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
							this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
						}
					}
					if (this.isHS4)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal1.ogg", 0f, false);
					}
					if (this.isHS5)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal2.ogg", 0f, false);
					}
					if (this.isHS6)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal3.ogg", 0f, false);
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
						if (this.maidArray[j].transform.position.y != 100f)
						{
							this.dancePos[j] = this.maidArray[j].transform.position;
						}
						else
						{
							this.dancePos[j] = new Vector3(this.maidArray[j].transform.position.x, 0f, this.maidArray[j].transform.position.z);
						}
						this.danceRot[j] = this.maidArray[j].transform.localRotation;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[1].SetPos(new Vector3(this.maidArray[0].transform.position.x, 100f, this.maidArray[0].transform.position.z));
						this.maidArray[1].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[3] && this.maidArray[3].Visible)
					{
						this.maidArray[3].SetPos(new Vector3(this.maidArray[2].transform.position.x, 100f, this.maidArray[2].transform.position.z));
						this.maidArray[3].body0.transform.localRotation = this.maidArray[2].body0.transform.localRotation;
					}
					if (this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[5].SetPos(new Vector3(this.maidArray[4].transform.position.x, 100f, this.maidArray[4].transform.position.z));
						this.maidArray[5].body0.transform.localRotation = this.maidArray[4].body0.transform.localRotation;
					}
					if (this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[7].SetPos(new Vector3(this.maidArray[6].transform.position.x, 100f, this.maidArray[6].transform.position.z));
						this.maidArray[7].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[9] && this.maidArray[9].Visible)
					{
						this.maidArray[9].SetPos(new Vector3(this.maidArray[8].transform.position.x, 100f, this.maidArray[8].transform.position.z));
						this.maidArray[9].body0.transform.localRotation = this.maidArray[8].body0.transform.localRotation;
					}
					if (this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[11].SetPos(new Vector3(this.maidArray[10].transform.position.x, 100f, this.maidArray[10].transform.position.z));
						this.maidArray[11].body0.transform.localRotation = this.maidArray[10].body0.transform.localRotation;
					}
					if (this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[13].SetPos(new Vector3(this.maidArray[12].transform.position.x, 100f, this.maidArray[12].transform.position.z));
						this.maidArray[13].body0.transform.localRotation = this.maidArray[12].body0.transform.localRotation;
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[2].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					this.isDance = true;
					this.isDanceStart7 = true;
					this.isHS4 = false;
					this.isHS5 = false;
					this.isHS6 = false;
				}
				if (this.isHS7 || this.isHS8 || this.isHS9)
				{
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						string text6 = "dance_cm3d2_003_hs_f1.anm";
						TextAsset textAsset = Resources.Load("SceneDance/dance_Hhs_m0") as TextAsset;
						switch (j)
						{
							case 1:
							case 5:
							case 9:
							case 13:
								text6 = "dance_cm3d2_003_hs_f2.anm";
								textAsset = (Resources.Load("SceneDance/dance_Hhs_m1") as TextAsset);
								break;
							case 2:
							case 6:
							case 10:
								text6 = "dance_cm3d2_003_hs_f3.anm";
								textAsset = (Resources.Load("SceneDance/dance_Hhs_m2") as TextAsset);
								break;
							case 3:
							case 7:
							case 11:
								text6 = "dance_cm3d2_003_hs_f4.anm";
								textAsset = (Resources.Load("SceneDance/dance_Hhs_m2") as TextAsset);
								break;
						}
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
						}
						string text5 = textAsset.text;
						text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (this.isHS7)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal1.ogg", 0f, false);
					}
					if (this.isHS8)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal2.ogg", 0f, false);
					}
					if (this.isHS9)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("happy_happy_scandal3.ogg", 0f, false);
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
						if (this.maidArray[j].transform.position.y != 100f)
						{
							this.dancePos[j] = this.maidArray[j].transform.position;
						}
						else
						{
							this.dancePos[j] = new Vector3(this.maidArray[j].transform.position.x, 0f, this.maidArray[j].transform.position.z);
						}
						this.danceRot[j] = this.maidArray[j].transform.localRotation;
					}
					if ((this.maidArray[0].transform.position.x == 0.3f || this.maidArray[0].transform.position.x == -0.3f) && this.maidArray[0].transform.position.y == 0f && this.maidArray[0].transform.position.z == 0f)
					{
						this.maidArray[0].SetPos(Vector3.zero);
						this.maidArray[0].SetRot(Vector3.zero);
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[1].SetPos(new Vector3(this.maidArray[0].transform.position.x, 100f, this.maidArray[0].transform.position.z));
						this.maidArray[1].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						this.maidArray[2].SetPos(this.maidArray[0].transform.position);
						this.maidArray[2].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[3] && this.maidArray[3].Visible)
					{
						this.maidArray[3].SetPos(this.maidArray[0].transform.position);
						this.maidArray[3].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[5].SetPos(new Vector3(this.maidArray[4].transform.position.x, 100f, this.maidArray[4].transform.position.z));
						this.maidArray[5].body0.transform.localRotation = this.maidArray[4].body0.transform.localRotation;
					}
					if (this.maidArray[6] && this.maidArray[6].Visible)
					{
						this.maidArray[6].SetPos(this.maidArray[4].transform.position);
						this.maidArray[6].body0.transform.localRotation = this.maidArray[4].body0.transform.localRotation;
					}
					if (this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[7].SetPos(this.maidArray[4].transform.position);
						this.maidArray[7].body0.transform.localRotation = this.maidArray[4].body0.transform.localRotation;
					}
					if (this.maidArray[9] && this.maidArray[9].Visible)
					{
						this.maidArray[9].SetPos(new Vector3(this.maidArray[8].transform.position.x, 100f, this.maidArray[8].transform.position.z));
						this.maidArray[9].body0.transform.localRotation = this.maidArray[8].body0.transform.localRotation;
					}
					if (this.maidArray[10] && this.maidArray[10].Visible)
					{
						this.maidArray[10].SetPos(this.maidArray[8].transform.position);
						this.maidArray[10].body0.transform.localRotation = this.maidArray[8].body0.transform.localRotation;
					}
					if (this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[11].SetPos(this.maidArray[8].transform.position);
						this.maidArray[11].body0.transform.localRotation = this.maidArray[8].body0.transform.localRotation;
					}
					if (this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[13].SetPos(new Vector3(this.maidArray[12].transform.position.x, 100f, this.maidArray[12].transform.position.z));
						this.maidArray[13].body0.transform.localRotation = this.maidArray[12].body0.transform.localRotation;
					}
					if (this.maidArray[4] && this.maidArray[4].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[4].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					this.isDance = true;
					this.isDanceStart8 = true;
					this.isHS7 = false;
					this.isHS8 = false;
					this.isHS9 = false;
				}
				if (this.isCF1)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_RtY_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d_003_sp2_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d_003_sp2_f1.anm", GameUty.FileSystem, "dance_cm3d_003_sp2_f1.anm", false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara02_002_rty_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara02_002_rty_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara02_002_rty_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/rhythmix to you_kara_voice") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("RhythmixToYou.ogg", 0f, false);
					}
					else
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("rhythmixtoyou_kara.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart5K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart5 = true;
					this.isCF1 = false;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha4)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Keypad4)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isCF1 = true;
					if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.isShift = true;
					}
				}
				if (this.isKT1)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_cktc_1_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d_004_kano_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d_004_kano_f1.anm", GameUty.FileSystem, "dance_cm3d_004_kano_f1.anm", false, false);
							}
							this.danceName[j] = "dance_cm3d_004_kano_f1.anm";
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_kara_002_cktc_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_kara_002_cktc_f1.anm", GameUty.FileSystem, "dance_cm3d2_kara_002_cktc_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/cktc_kara_voice_2") as TextAsset);
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("can_know_two_close.ogg", 0f, false);
					}
					else
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("canknowtwoclose_short_kara.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[2].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart9K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart9 = true;
					this.isKT1 = false;
				}
				if (this.isSS && (this.isSS1 || this.isSS2 || this.isSS3 || this.isSS4 || this.isSS5 || this.isSS6))
				{
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					if (this.isSS1)
					{
						this.nameK = "dance_BlD_m";
						this.nameA = "dance_cm3d21_002_bid_f";
						this.nameS = "bloomingdreaming_short";
						this.danceNo1 = 1;
						this.danceNo2 = 3;
						this.danceNo3 = 2;
					}
					if (this.isSS2)
					{
						this.nameK = "dance_KAD_m";
						this.nameA = "dance_cm3d21_003_kad_f";
						this.nameS = "kiminiaijodelicious_short";
						this.danceNo1 = 1;
						this.danceNo2 = 2;
						this.danceNo3 = 3;
					}
					if (this.isSS3)
					{
						this.nameK = "dance_LUM_m";
						this.nameA = "dance_cm3d21_004_lm_f";
						this.nameS = "luminousmoment_short";
						this.danceNo1 = 3;
						this.danceNo2 = 1;
						this.danceNo3 = 2;
					}
					if (this.isSS4)
					{
						this.nameK = "dance_NmF_m";
						this.nameA = "dance_cm3d21_001_nmf_f";
						this.nameS = "nightmagicfire_short";
						this.danceNo1 = 2;
						this.danceNo2 = 1;
						this.danceNo3 = 3;
					}
					if (this.isSS5)
					{
						this.nameK = "dance_MoE_m";
						this.nameA = "dance_cm3d21_005_moe_f";
						this.nameS = "melodyofempire_short";
						this.danceNo1 = 1;
						this.danceNo2 = 3;
						this.danceNo3 = 2;
					}
					if (this.isSS6)
					{
						this.nameK = "dance_NmF_m";
						this.nameA = "dance_cm3d21_kara_001_nmf_f";
						this.nameS = "nightmagicfire_short";
						this.danceNo1 = 1;
						this.danceNo2 = 1;
						this.danceNo3 = 1;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						TextAsset textAsset = Resources.Load("SceneDance/" + this.nameK + (this.danceNo1 - 1)) as TextAsset;
						string text6 = this.nameA + this.danceNo1 + ".anm";
						switch (j)
						{
							case 1:
							case 4:
							case 7:
							case 10:
							case 13:
								text6 = this.nameA + this.danceNo2 + ".anm";
								textAsset = (Resources.Load("SceneDance/" + this.nameK + (this.danceNo2 - 1)) as TextAsset);
								break;
							case 2:
							case 5:
							case 8:
							case 11:
								text6 = this.nameA + this.danceNo3 + ".anm";
								textAsset = (Resources.Load("SceneDance/" + this.nameK + (this.danceNo3 - 1)) as TextAsset);
								break;
						}
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
							string text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
							this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
						}
					}
					GameMain.Instance.SoundMgr.PlayDanceBGM(this.nameS + ".ogg", 0f, false);
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
						if (this.maidArray[j].transform.position.y != 100f)
						{
							this.dancePos[j] = this.maidArray[j].transform.position;
						}
						else
						{
							this.dancePos[j] = new Vector3(this.maidArray[j].transform.position.x, 0f, this.maidArray[j].transform.position.z);
						}
						this.danceRot[j] = this.maidArray[j].transform.localRotation;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[1].SetPos(new Vector3(this.maidArray[0].transform.position.x, this.maidArray[0].transform.position.y, this.maidArray[0].transform.position.z));
						this.maidArray[1].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						this.maidArray[2].SetPos(new Vector3(this.maidArray[0].transform.position.x, this.maidArray[0].transform.position.y, this.maidArray[0].transform.position.z));
						this.maidArray[2].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[4] && this.maidArray[4].Visible)
					{
						this.maidArray[4].SetPos(new Vector3(this.maidArray[3].transform.position.x, this.maidArray[3].transform.position.y, this.maidArray[3].transform.position.z));
						this.maidArray[4].body0.transform.localRotation = this.maidArray[3].body0.transform.localRotation;
					}
					if (this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[5].SetPos(new Vector3(this.maidArray[3].transform.position.x, this.maidArray[3].transform.position.y, this.maidArray[3].transform.position.z));
						this.maidArray[5].body0.transform.localRotation = this.maidArray[3].body0.transform.localRotation;
					}
					if (this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[7].SetPos(new Vector3(this.maidArray[6].transform.position.x, this.maidArray[6].transform.position.y, this.maidArray[6].transform.position.z));
						this.maidArray[7].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[8] && this.maidArray[8].Visible)
					{
						this.maidArray[8].SetPos(new Vector3(this.maidArray[6].transform.position.x, this.maidArray[6].transform.position.y, this.maidArray[6].transform.position.z));
						this.maidArray[8].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[10] && this.maidArray[10].Visible)
					{
						this.maidArray[10].SetPos(new Vector3(this.maidArray[9].transform.position.x, this.maidArray[9].transform.position.y, this.maidArray[9].transform.position.z));
						this.maidArray[10].body0.transform.localRotation = this.maidArray[9].body0.transform.localRotation;
					}
					if (this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[11].SetPos(new Vector3(this.maidArray[9].transform.position.x, this.maidArray[9].transform.position.y, this.maidArray[9].transform.position.z));
						this.maidArray[11].body0.transform.localRotation = this.maidArray[9].body0.transform.localRotation;
					}
					if (this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[13].SetPos(new Vector3(this.maidArray[12].transform.position.x, this.maidArray[12].transform.position.y, this.maidArray[12].transform.position.z));
						this.maidArray[13].body0.transform.localRotation = this.maidArray[12].body0.transform.localRotation;
					}
					this.isSS = false;
					this.isDance = true;
					this.isDanceStart11 = true;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha5)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha5)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSS1 = true;
					this.isSS2 = false;
					this.isSS3 = false;
					this.isSS4 = false;
					this.isSS5 = false;
					this.isSS6 = false;
					this.isSS = true;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha6)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha6)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSS1 = false;
					this.isSS2 = true;
					this.isSS3 = false;
					this.isSS4 = false;
					this.isSS5 = false;
					this.isSS6 = false;
					this.isSS = true;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha7)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha7)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSS1 = false;
					this.isSS2 = false;
					this.isSS3 = true;
					this.isSS4 = false;
					this.isSS5 = false;
					this.isSS6 = false;
					this.isSS = true;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha8)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha8)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSS1 = false;
					this.isSS2 = false;
					this.isSS3 = false;
					this.isSS4 = true;
					this.isSS5 = false;
					this.isSS6 = false;
					this.isSS = true;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha9)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha9)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSS1 = false;
					this.isSS2 = false;
					this.isSS3 = false;
					this.isSS4 = false;
					this.isSS5 = true;
					this.isSS6 = false;
					this.isSS = true;
				}
				if (this.isKHG1 || this.isKHG2)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_KhG_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_005_khg_f.anm"))
						{
							this.maidArray[j].body0.LoadAnime("dance_cm3d2_005_khg_f.anm", GameUty.FileSystem, "dance_cm3d2_005_khg_f.anm", false, false);
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (this.isKHG1)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("kaikaku_short1.ogg", 0f, false);
					}
					if (this.isKHG2)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("kaikaku_short2.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					this.isDance = true;
					this.isDanceStart12 = true;
					this.isKHG1 = false;
					this.isKHG2 = false;
				}
				if (this.isDanceStart13Count > 0)
				{
					this.isDanceStart13Count++;
				}
				if (this.isDanceStart13Count == 1)
				{
					GameMain.Instance.SoundMgr.PlayDanceBGM("nightmagicfire_short.ogg", 0f, false);
					this.isDanceStart13Count = 0;
					TextAsset textAsset = Resources.Load("SceneDance/dance_SsN_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
					}
				}
				if ((this.isSN1 || this.isSN2 || this.isSN3) && this.isShift)
				{
					TextAsset textAsset = Resources.Load("SceneDance/dance_SsN_m0") as TextAsset;
					string text5 = textAsset.text;
					text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (!this.isShift)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d2_006_ssn_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d2_006_ssn_f1.anm", GameUty.FileSystem, "dance_cm3d2_006_ssn_f1.anm", false, false);
							}
						}
						else
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip("dance_cm3d21_kara_001_nmf_f1.anm"))
							{
								this.maidArray[j].body0.LoadAnime("dance_cm3d21_kara_001_nmf_f1.anm", GameUty.FileSystem, "dance_cm3d21_kara_001_nmf_f1.anm", false, false);
							}
							textAsset = (Resources.Load("SceneDance/dance_NmF_m1") as TextAsset);
							string text6 = this.nameA + this.danceNo1 + ".anm";
							switch (j)
							{
								case 1:
								case 4:
								case 7:
								case 10:
								case 13:
									textAsset = (Resources.Load("SceneDance/dance_NmF_m0") as TextAsset);
									break;
								case 2:
								case 5:
								case 8:
								case 11:
									textAsset = (Resources.Load("SceneDance/dance_NmF_m2") as TextAsset);
									break;
							}
							text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							if (!this.isDanceStart1K && !this.isDanceStart3K && !this.isDanceStart4K && !this.isDanceStart5K && !this.isDanceStart6K && !this.isDanceStart9K && !this.isDanceStart13K)
							{
								this.dancePos[j] = this.maidArray[j].transform.position;
								this.danceRot[j] = this.maidArray[j].transform.localRotation;
							}
						}
						this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
					}
					if (!this.isShift)
					{
						if (this.isSN1)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("nightmagicfire_short.ogg", 0f, false);
						}
						if (this.isSN2)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("sunshinenatsu2.ogg", 0f, false);
						}
						if (this.isSN3)
						{
							GameMain.Instance.SoundMgr.PlayDanceBGM("sunshinenatsu3.ogg", 0f, false);
						}
					}
					else
					{
						this.danceWait = 400;
						this.isDanceStart13Count++;
						GameMain.Instance.SoundMgr.PlayDanceBGM("nightmagicfire_short.ogg", 0f, false);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[1].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					if (this.isShift)
					{
						this.isDanceStart13K = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							this.maidArray[j].SetRot(new Vector3(this.maidArray[j].body0.transform.localRotation.x, this.maidArray[j].body0.transform.localRotation.y + 90f, this.maidArray[j].body0.transform.localRotation.z));
							this.maidArray[j].SetPos(new Vector3(this.maidArray[j].body0.transform.position.x + 1f, this.maidArray[j].body0.transform.position.y, this.maidArray[j].body0.transform.position.z + 4f));
							string text3 = "handitem,HanditemL_Karaoke_Mike_I_.menu";
							int i = j;
							Maid maid = this.maidArray[j];
							string[] array = new string[2];
							array = text3.Split(new char[]
							{
								','
							});
							maid.DelProp(MPN.handitem, true);
							maid.DelProp(MPN.accvag, true);
							maid.DelProp(MPN.accanl, true);
							maid.DelProp(MPN.kousoku_upper, true);
							maid.DelProp(MPN.kousoku_lower, true);
							if (array[0] != "")
							{
								maid.SetProp(array[0], array[1], 0, true, false);
							}
							maid.AllProcPropSeqStart();
						}
					}
					this.isDance = true;
					this.isDanceStart13 = true;
					this.isSN1 = false;
					this.isSN2 = false;
					this.isSN3 = false;
				}
				if ((Input.GetKey(KeyCode.Return) && Input.GetKeyDown(KeyCode.Alpha8)) || (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Alpha8)))
				{
					GameMain.Instance.SoundMgr.StopBGM(0f);
					this.isSN1 = true;
					this.hFlg = true;
					if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.isShift = true;
					}
				}
				if (this.isSN4 || this.isSN5 || this.isSN6)
				{
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						TextAsset textAsset = Resources.Load("SceneDance/dance_SsN_m0") as TextAsset;
						string text6 = "dance_cm3d2_006_ssn_f1.anm";
						switch (j)
						{
							case 1:
							case 4:
							case 7:
							case 10:
							case 13:
								text6 = "dance_cm3d2_006_ssn_f2.anm";
								textAsset = (Resources.Load("SceneDance/dance_SsN_m1") as TextAsset);
								break;
							case 2:
							case 5:
							case 8:
							case 11:
								text6 = "dance_cm3d2_006_ssn_f3.anm";
								textAsset = (Resources.Load("SceneDance/dance_SsN_m1") as TextAsset);
								break;
						}
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
							string text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
							this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
						}
					}
					if (this.isSN4)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("sunshinenatsu1.ogg", 0f, false);
					}
					if (this.isSN5)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("sunshinenatsu2.ogg", 0f, false);
					}
					if (this.isSN6)
					{
						GameMain.Instance.SoundMgr.PlayDanceBGM("sunshinenatsu3.ogg", 0f, false);
					}
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
						if (this.maidArray[j].transform.position.y != 100f)
						{
							this.dancePos[j] = this.maidArray[j].transform.position;
						}
						else
						{
							this.dancePos[j] = new Vector3(this.maidArray[j].transform.position.x, 0f, this.maidArray[j].transform.position.z);
						}
						this.danceRot[j] = this.maidArray[j].transform.localRotation;
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[1].SetPos(new Vector3(this.maidArray[0].transform.position.x, this.maidArray[0].transform.position.y, this.maidArray[0].transform.position.z));
						this.maidArray[1].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						this.maidArray[2].SetPos(new Vector3(this.maidArray[0].transform.position.x, this.maidArray[0].transform.position.y, this.maidArray[0].transform.position.z));
						this.maidArray[2].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[4] && this.maidArray[4].Visible)
					{
						this.maidArray[4].SetPos(new Vector3(this.maidArray[3].transform.position.x, this.maidArray[3].transform.position.y, this.maidArray[3].transform.position.z));
						this.maidArray[4].body0.transform.localRotation = this.maidArray[3].body0.transform.localRotation;
					}
					if (this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[5].SetPos(new Vector3(this.maidArray[3].transform.position.x, this.maidArray[3].transform.position.y, this.maidArray[3].transform.position.z));
						this.maidArray[5].body0.transform.localRotation = this.maidArray[3].body0.transform.localRotation;
					}
					if (this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[7].SetPos(new Vector3(this.maidArray[6].transform.position.x, this.maidArray[6].transform.position.y, this.maidArray[6].transform.position.z));
						this.maidArray[7].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[8] && this.maidArray[8].Visible)
					{
						this.maidArray[8].SetPos(new Vector3(this.maidArray[6].transform.position.x, this.maidArray[6].transform.position.y, this.maidArray[6].transform.position.z));
						this.maidArray[8].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[10] && this.maidArray[10].Visible)
					{
						this.maidArray[10].SetPos(new Vector3(this.maidArray[9].transform.position.x, this.maidArray[9].transform.position.y, this.maidArray[9].transform.position.z));
						this.maidArray[10].body0.transform.localRotation = this.maidArray[9].body0.transform.localRotation;
					}
					if (this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[11].SetPos(new Vector3(this.maidArray[9].transform.position.x, this.maidArray[9].transform.position.y, this.maidArray[9].transform.position.z));
						this.maidArray[11].body0.transform.localRotation = this.maidArray[9].body0.transform.localRotation;
					}
					if (this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[13].SetPos(new Vector3(this.maidArray[12].transform.position.x, this.maidArray[12].transform.position.y, this.maidArray[12].transform.position.z));
						this.maidArray[13].body0.transform.localRotation = this.maidArray[12].body0.transform.localRotation;
					}
					this.isDance = true;
					this.isDanceStart14 = true;
					this.isSN4 = false;
					this.isSN5 = false;
					this.isSN6 = false;
				}
				if (this.isSD1)
				{
					this.audioSourceBgm = GameMain.Instance.SoundMgr.GetAudioSourceBgm();
					GameMain.Instance.SoundMgr.PlayBGM("se002.ogg", 0f, false);
					for (int j = 0; j < this.maidCnt; j++)
					{
						TextAsset textAsset = Resources.Load("SceneDance/uta_mmlop_mix_Bok") as TextAsset;
						string text6 = "dance_cmo_002_sd_f1.anm";
						switch (j)
						{
							case 1:
							case 3:
							case 5:
							case 7:
							case 9:
							case 11:
							case 13:
								text6 = "dance_cmo_002_sd_f2.anm";
								textAsset = (Resources.Load("SceneDance/uta_mmlop_mix_Aok") as TextAsset);
								break;
						}
						this.danceName[j] = text6;
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							if (!this.maidArray[j].body0.m_Bones.GetComponent<Animation>().GetClip(text6))
							{
								this.maidArray[j].body0.LoadAnime(text6, GameUty.FileSystem, text6, false, false);
							}
							string text5 = textAsset.text;
							text5 = Regex.Replace(text5, "(\\r|\\n| )", string.Empty);
							this.maidArray[j].StartKuchipakuPattern(0f, text5, true);
							this.m_baKuchipakuPattern[j] = Convert.FromBase64String(text5);
						}
					}
					GameMain.Instance.SoundMgr.PlayDanceBGM("selfishdestiny_due_short_1.ogg", 0f, false);
					this.ikBui = 5;
					this.isDanceStart1F = false;
					this.isDanceStart2F = false;
					this.isDanceStart3F = false;
					this.isDanceStart4F = false;
					this.isDanceStart5F = false;
					this.isDanceStart6F = false;
					this.isDanceStart7F = false;
					this.isDanceStart8F = false;
					this.isDanceStart9F = false;
					this.isDanceStart10F = false;
					this.isDanceStart11F = false;
					this.isDanceStart12F = false;
					this.isDanceStart13F = false;
					this.isDanceStart14F = false;
					this.isDanceStart15F = false;
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.danceFace[j] = 0f;
					}
					if (this.isDanceStart7V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart7V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart7V = false;
					}
					if (this.isDanceStart8V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart8V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart8V = false;
						this.isDanceStart8P = false;
					}
					if (this.isDanceStart11V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart11V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart11V = false;
					}
					if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart1K || this.isDanceStart3K || this.isDanceStart4K || this.isDanceStart5K || this.isDanceStart6K || this.isDanceStart9K || this.isDanceStart13K)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
									int i = j;
									Maid maid = this.maidArray[j];
									maid.DelProp(MPN.handitem, true);
									maid.DelProp(MPN.accvag, true);
									maid.DelProp(MPN.accanl, true);
									maid.DelProp(MPN.kousoku_upper, true);
									maid.DelProp(MPN.kousoku_lower, true);
									maid.AllProcPropSeqStart();
								}
							}
						}
						this.isDanceStart1K = false;
						this.isDanceStart3K = false;
						this.isDanceStart4K = false;
						this.isDanceStart5K = false;
						this.isDanceStart6K = false;
						this.isDanceStart9K = false;
						this.isDanceStart13K = false;
					}
					if (this.isDanceStart14V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart14V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart14V = false;
					}
					if (this.isDanceStart15V)
					{
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isDanceStart15V)
								{
									this.maidArray[j].SetPos(this.dancePos[j]);
									this.maidArray[j].body0.transform.localRotation = this.danceRot[j];
								}
							}
						}
						this.isDanceStart15V = false;
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						this.isStop[j] = false;
						this.isLock[j] = false;
						this.pHandL[j] = 0;
						this.pHandR[j] = 0;
						this.muneIKL[j] = false;
						this.muneIKR[j] = false;
						if (!this.isVR)
						{
							this.maidArray[j].body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[j]];
							this.maidArray[j].body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[j]];
						}
						if (this.maidArray[j].transform.position.y != 100f)
						{
							this.dancePos[j] = this.maidArray[j].transform.position;
						}
						else
						{
							this.dancePos[j] = new Vector3(this.maidArray[j].transform.position.x, 0f, this.maidArray[j].transform.position.z);
						}
						this.danceRot[j] = this.maidArray[j].transform.localRotation;
					}
					if (this.maidArray[0] && this.maidArray[0].Visible && this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[0].SetPos(new Vector3((this.maidArray[0].transform.position.x + this.maidArray[1].transform.position.x) / 2f, (this.maidArray[0].transform.position.y + this.maidArray[1].transform.position.y) / 2f, (this.maidArray[0].transform.position.z + this.maidArray[1].transform.position.z) / 2f));
						this.maidArray[0].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[0].transform.localRotation, this.maidArray[1].transform.localRotation, 0.5f);
					}
					if (this.maidArray[2] && this.maidArray[2].Visible && this.maidArray[3] && this.maidArray[3].Visible)
					{
						this.maidArray[2].SetPos(new Vector3((this.maidArray[2].transform.position.x + this.maidArray[3].transform.position.x) / 2f, (this.maidArray[2].transform.position.y + this.maidArray[3].transform.position.y) / 2f, (this.maidArray[2].transform.position.z + this.maidArray[3].transform.position.z) / 2f));
						this.maidArray[2].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[2].transform.localRotation, this.maidArray[3].transform.localRotation, 0.5f);
					}
					if (this.maidArray[4] && this.maidArray[4].Visible && this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[4].SetPos(new Vector3((this.maidArray[4].transform.position.x + this.maidArray[5].transform.position.x) / 2f, (this.maidArray[4].transform.position.y + this.maidArray[5].transform.position.y) / 2f, (this.maidArray[4].transform.position.z + this.maidArray[5].transform.position.z) / 2f));
						this.maidArray[4].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[4].transform.localRotation, this.maidArray[5].transform.localRotation, 0.5f);
					}
					if (this.maidArray[6] && this.maidArray[6].Visible && this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[6].SetPos(new Vector3((this.maidArray[6].transform.position.x + this.maidArray[7].transform.position.x) / 2f, (this.maidArray[6].transform.position.y + this.maidArray[7].transform.position.y) / 2f, (this.maidArray[6].transform.position.z + this.maidArray[7].transform.position.z) / 2f));
						this.maidArray[6].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[6].transform.localRotation, this.maidArray[7].transform.localRotation, 0.5f);
					}
					if (this.maidArray[8] && this.maidArray[8].Visible && this.maidArray[9] && this.maidArray[9].Visible)
					{
						this.maidArray[8].SetPos(new Vector3((this.maidArray[8].transform.position.x + this.maidArray[9].transform.position.x) / 2f, (this.maidArray[8].transform.position.y + this.maidArray[9].transform.position.y) / 2f, (this.maidArray[8].transform.position.z + this.maidArray[9].transform.position.z) / 2f));
						this.maidArray[8].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[8].transform.localRotation, this.maidArray[9].transform.localRotation, 0.5f);
					}
					if (this.maidArray[10] && this.maidArray[10].Visible && this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[10].SetPos(new Vector3((this.maidArray[10].transform.position.x + this.maidArray[11].transform.position.x) / 2f, (this.maidArray[10].transform.position.y + this.maidArray[11].transform.position.y) / 2f, (this.maidArray[10].transform.position.z + this.maidArray[11].transform.position.z) / 2f));
						this.maidArray[10].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[10].transform.localRotation, this.maidArray[11].transform.localRotation, 0.5f);
					}
					if (this.maidArray[12] && this.maidArray[12].Visible && this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[12].SetPos(new Vector3((this.maidArray[12].transform.position.x + this.maidArray[13].transform.position.x) / 2f, (this.maidArray[12].transform.position.y + this.maidArray[13].transform.position.y) / 2f, (this.maidArray[12].transform.position.z + this.maidArray[13].transform.position.z) / 2f));
						this.maidArray[12].body0.transform.localRotation = Quaternion.Lerp(this.maidArray[12].transform.localRotation, this.maidArray[13].transform.localRotation, 0.5f);
					}
					if (this.maidArray[1] && this.maidArray[1].Visible)
					{
						this.maidArray[1].SetPos(new Vector3(this.maidArray[0].transform.position.x, this.maidArray[0].transform.position.y, this.maidArray[0].transform.position.z));
						this.maidArray[1].body0.transform.localRotation = this.maidArray[0].body0.transform.localRotation;
					}
					if (this.maidArray[3] && this.maidArray[3].Visible)
					{
						this.maidArray[3].SetPos(new Vector3(this.maidArray[2].transform.position.x, this.maidArray[2].transform.position.y, this.maidArray[2].transform.position.z));
						this.maidArray[3].body0.transform.localRotation = this.maidArray[2].body0.transform.localRotation;
					}
					if (this.maidArray[5] && this.maidArray[5].Visible)
					{
						this.maidArray[5].SetPos(new Vector3(this.maidArray[4].transform.position.x, this.maidArray[4].transform.position.y, this.maidArray[4].transform.position.z));
						this.maidArray[5].body0.transform.localRotation = this.maidArray[4].body0.transform.localRotation;
					}
					if (this.maidArray[7] && this.maidArray[7].Visible)
					{
						this.maidArray[7].SetPos(new Vector3(this.maidArray[6].transform.position.x, this.maidArray[6].transform.position.y, this.maidArray[6].transform.position.z));
						this.maidArray[7].body0.transform.localRotation = this.maidArray[6].body0.transform.localRotation;
					}
					if (this.maidArray[9] && this.maidArray[9].Visible)
					{
						this.maidArray[9].SetPos(new Vector3(this.maidArray[8].transform.position.x, this.maidArray[8].transform.position.y, this.maidArray[8].transform.position.z));
						this.maidArray[9].body0.transform.localRotation = this.maidArray[8].body0.transform.localRotation;
					}
					if (this.maidArray[11] && this.maidArray[11].Visible)
					{
						this.maidArray[11].SetPos(new Vector3(this.maidArray[10].transform.position.x, this.maidArray[10].transform.position.y, this.maidArray[10].transform.position.z));
						this.maidArray[11].body0.transform.localRotation = this.maidArray[10].body0.transform.localRotation;
					}
					if (this.maidArray[13] && this.maidArray[13].Visible)
					{
						this.maidArray[13].SetPos(new Vector3(this.maidArray[12].transform.position.x, this.maidArray[12].transform.position.y, this.maidArray[12].transform.position.z));
						this.maidArray[13].body0.transform.localRotation = this.maidArray[12].body0.transform.localRotation;
					}
					if (this.maidArray[2] && this.maidArray[2].Visible)
					{
						if (this.maidArray[0].transform.position == this.maidArray[2].transform.position)
						{
							Vector3 vector = Vector3.zero;
							vector.x = 0.6f;
							this.maidArray[1].SetPos(vector);
							if (this.maidArray[2] && this.maidArray[2].Visible)
							{
								vector.x = -0.6f;
								this.maidArray[2].SetPos(vector);
							}
							if (this.maidArray[3] && this.maidArray[3].Visible)
							{
								vector.x = 1.2f;
								this.maidArray[3].SetPos(vector);
							}
							if (this.maidArray[4] && this.maidArray[4].Visible)
							{
								vector.x = -1.2f;
								this.maidArray[4].SetPos(vector);
							}
							if (this.maidArray[5] && this.maidArray[5].Visible)
							{
								vector.x = 1.8f;
								this.maidArray[5].SetPos(vector);
							}
							if (this.maidArray[6] && this.maidArray[6].Visible)
							{
								vector.x = -1.8f;
								this.maidArray[6].SetPos(vector);
							}
							if (this.maidArray[7] && this.maidArray[7].Visible)
							{
								vector.x = 2.4f;
								this.maidArray[7].SetPos(vector);
							}
							if (this.maidArray[8] && this.maidArray[8].Visible)
							{
								vector.x = -2.4f;
								this.maidArray[8].SetPos(vector);
							}
							if (this.maidArray[9] && this.maidArray[9].Visible)
							{
								vector.x = 3f;
								this.maidArray[9].SetPos(vector);
							}
							if (this.maidArray[10] && this.maidArray[10].Visible)
							{
								vector.x = -3f;
								this.maidArray[10].SetPos(vector);
							}
							if (this.maidArray[11] && this.maidArray[11].Visible)
							{
								vector.x = 3.6f;
								this.maidArray[11].SetPos(vector);
							}
							if (this.maidArray[12] && this.maidArray[12].Visible)
							{
								vector.x = -3.6f;
								this.maidArray[12].SetPos(vector);
							}
							if (this.maidArray[13] && this.maidArray[13].Visible)
							{
								vector.x = 4.2f;
								this.maidArray[13].SetPos(vector);
							}
						}
					}
					this.isDance = true;
					this.isDanceStart15 = true;
					this.isSD1 = false;
				}
				if (!Input.GetKeyUp(KeyCode.Return) && Input.GetKeyUp(KeyCode.H) && !this.hFlg)
				{
					if (this.h2Flg)
					{
						this.h2Flg = false;
					}
					else
					{
						string text5 = "";
						if (this.wearIndex == 0)
						{
							text5 = "Underwear";
							this.wearIndex = 1;
							this.isWear = false;
							this.isSkirt = false;
							this.isBra = true;
							this.isPanz = true;
							this.isHeadset = false;
							this.isGlove = false;
							this.isStkg = true;
							this.isShoes = false;
						}
						else if (this.wearIndex == 1)
						{
							text5 = "Nude";
							this.wearIndex = 2;
							this.isWear = false;
							this.isSkirt = false;
							this.isBra = false;
							this.isPanz = false;
							this.isHeadset = false;
							this.isGlove = false;
							this.isStkg = false;
							this.isShoes = false;
						}
						else if (this.wearIndex == 2)
						{
							text5 = "None";
							this.wearIndex = 0;
							this.isWear = true;
							this.isSkirt = true;
							this.isBra = true;
							this.isPanz = true;
							this.isHeadset = true;
							this.isGlove = true;
							this.isStkg = true;
							this.isShoes = true;
						}
						TBody.MaskMode maskMode = (TBody.MaskMode)Enum.Parse(typeof(TBody.MaskMode), text5);
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								this.maidArray[j].body0.SetMaskMode(maskMode);
							}
						}
					}
				}
				if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && Input.GetKeyDown(KeyCode.S) && !this.sFlg)
				{
					this.saveScene = 9999;
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
				if (this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && Input.GetKeyDown(KeyCode.A))
				{
					this.loadScene = 9999;
					GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
				}
				if (this.isScreen)
				{
					this.isScreen = false;
					this.bGui = this.isGui;
					this.isGui = false;
					if (!this.isMessage)
					{
						foreach (UICamera uicamera in this.ui_cam_hide_list_)
						{
							uicamera.GetComponent<Camera>().enabled = true;
						}
						this.ui_cam_hide_list_.Clear();
					}
					else
					{
						if (this.editUI != null)
						{
							this.editUI.SetActive(true);
						}
						if (GameMain.Instance.CMSystem.ViewFps)
						{
							GameObject childObject = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/FpsCounter", false);
							childObject.SetActive(true);
						}
						GameObject childObject2 = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/SystemDialog", false);
						GameObject childObject3 = UTY.GetChildObject(GameMain.Instance.gameObject, "SystemUI Root/SystemShortcut", false);
						childObject2.SetActive(true);
						childObject3.SetActive(true);
					}
					for (int j = 0; j < this.maidCnt; j++)
					{
						if (this.maidArray[j] && this.maidArray[j].Visible)
						{
							this.isBone[j] = this.isBoneN[j];
							if (this.isBone[j])
							{
								this.gNeck[j].SetActive(true);
								this.gSpine[j].SetActive(true);
								this.gSpine0a[j].SetActive(true);
								this.gSpine1a[j].SetActive(true);
								this.gSpine1[j].SetActive(true);
								this.gPelvis[j].SetActive(true);
								this.gHandL[j].SetActive(true);
								this.gArmL[j].SetActive(true);
								this.gFootL[j].SetActive(true);
								this.gHizaL[j].SetActive(true);
								this.gHandR[j].SetActive(true);
								this.gArmR[j].SetActive(true);
								this.gFootR[j].SetActive(true);
								this.gHizaR[j].SetActive(true);
								this.gClavicleL[j].SetActive(true);
								this.gClavicleR[j].SetActive(true);
							}
						}
					}
				}
				if (this.isScreen2)
				{
					this.isScreen = true;
					this.isScreen2 = false;
				}
				if (!this.isVR && Input.GetKeyDown(KeyCode.S) && !this.sFlg && !Input.GetKey(KeyCode.KeypadEnter) && !Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.Q))
				{
					if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftAlt) && !this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
					{
						this.isScreen = true;
						for (int j = 0; j < this.maidCnt; j++)
						{
							if (this.maidArray[j] && this.maidArray[j].Visible)
							{
								if (this.isBone[j])
								{
									this.gNeck[j].SetActive(false);
									this.gSpine[j].SetActive(false);
									this.gSpine0a[j].SetActive(false);
									this.gSpine1a[j].SetActive(false);
									this.gSpine1[j].SetActive(false);
									this.gPelvis[j].SetActive(false);
									this.gHandL[j].SetActive(false);
									this.gArmL[j].SetActive(false);
									this.gFootL[j].SetActive(false);
									this.gHizaL[j].SetActive(false);
									this.gHandR[j].SetActive(false);
									this.gArmR[j].SetActive(false);
									this.gFootR[j].SetActive(false);
									this.gHizaR[j].SetActive(false);
									this.gClavicleL[j].SetActive(false);
									this.gClavicleR[j].SetActive(false);
								}
								this.isBoneN[j] = this.isBone[j];
								this.isBone[j] = false;
							}
						}
						if (!this.isMessage)
						{
							this.ui_cam_hide_list_.Clear();
							UICamera[] array17 = NGUITools.FindActive<UICamera>();
							foreach (UICamera uicamera2 in array17)
							{
								if (uicamera2.GetComponent<Camera>().enabled)
								{
									uicamera2.GetComponent<Camera>().enabled = false;
									this.ui_cam_hide_list_.Add(uicamera2);
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
						GameMain.Instance.MainCamera.ScreenShot(false);
						GameMain.Instance.SoundMgr.PlaySe("se022.ogg", false);
					}
				}
				if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.U) && !this.qFlg)
				{
					this.mainCameraTransform.Rotate(0f, 0f, -0.15f);
				}
				else if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.O) && !this.qFlg)
				{
					this.mainCameraTransform.Rotate(0f, 0f, 0.15f);
				}
				else if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.BackQuote) && !this.qFlg)
				{
					this.mainCameraTransform.eulerAngles = new Vector3(this.mainCameraTransform.rotation.eulerAngles.x, this.mainCameraTransform.rotation.eulerAngles.y, 0f);
				}
				else if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.R) && !this.qFlg)
				{
					GameMain.Instance.MainCamera.Reset(0, true);
					this.mainCamera.SetTargetPos(new Vector3(0f, 0.9f, 0f), true);
					this.mainCamera.SetDistance(3f, true);
				}
				else if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.S) && !this.qFlg)
				{
					this.cameraIti = this.mainCamera.GetTargetPos();
					this.cameraIti2 = this.mainCamera.GetPos();
					this.cameraItiAngle = this.mainCamera.GetAroundAngle();
					this.cameraItiDistance = this.mainCamera.GetDistance();
				}
				else if (!this.isVR && Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.A) && !this.qFlg)
				{
					this.mainCamera.SetTargetPos(this.cameraIti, true);
					this.mainCamera.SetPos(this.cameraIti2);
					this.mainCamera.SetAroundAngle(this.cameraItiAngle, true);
					this.mainCamera.SetDistance(this.cameraItiDistance, true);
				}
				if (!this.isVR && Input.GetKeyUp(KeyCode.Q) && this.qFlg)
				{
					this.qFlg = false;
				}
				if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.Space))
				{
					bool flag5 = false;
					for (int k = 0; k < this.keyArray.Length; k++)
					{
						if (Input.GetKey(this.keyArray[k]))
						{
							flag5 = true;
							break;
						}
					}
					if (!this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						if (!flag5)
						{
							Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
							Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
							Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
							if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 6E-05f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 6E-05f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -6E-05f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -6E-05f * this.speed;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector2.x, 0f, vector2.z) * -6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.J))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector3.x, 0f, vector3.z) * -6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
							{
								Vector3 vector = this.softG;
								vector += new Vector3(vector3.x, 0f, vector3.z) * 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Alpha0))
							{
								Vector3 vector = this.softG;
								vector.y += 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.P))
							{
								Vector3 vector = this.softG;
								vector.y -= 6E-05f * this.speed;
								this.softG = vector;
							}
							else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.BackQuote) && !this.atFlg)
							{
								this.softG = new Vector3(0f, -0.003f, 0f);
							}
							for (int j = 0; j < this.maidCnt; j++)
							{
								Maid maid = this.maidArray[j];
								for (int l = 0; l < maid.body0.goSlot.Count; l++)
								{
									if (maid.body0.goSlot[l].obj != null)
									{
										DynamicBone component2 = maid.body0.goSlot[l].obj.GetComponent<DynamicBone>();
										if (component2 != null && component2.enabled)
										{
											component2.m_Gravity = new Vector3(this.softG.x * 5f, (this.softG.y + 0.003f) * 5f, this.softG.z * 5f);
										}
									}
									List<THair1> fieldValue3 = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid.body0.goSlot[l].bonehair, "hair1list");
									for (int k = 0; k < fieldValue3.Count; k++)
									{
										fieldValue3[k].SoftG = new Vector3(this.softG.x, this.softG.y + this.kamiyure, this.softG.z);
									}
								}
							}
						}
					}
					else if (!flag5)
					{
						Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
						Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
						Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
						if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.J))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 4E-05f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 4E-05f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -4E-05f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -4E-05f * this.speed;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.I))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * 4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.K))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector2.x, 0f, vector2.z) * -4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.J))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector3.x, 0f, vector3.z) * -4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector = this.softG2;
							vector += new Vector3(vector3.x, 0f, vector3.z) * 4E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Alpha0))
						{
							Vector3 vector = this.softG2;
							vector.y += 2E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.P))
						{
							Vector3 vector = this.softG2;
							vector.y -= 2E-05f * this.speed;
							this.softG2 = vector;
						}
						else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.BackQuote) && !this.atFlg)
						{
							this.softG2 = new Vector3(0f, -0.005f, 0f);
						}
						for (int j = 0; j < this.maidCnt; j++)
						{
							Maid maid = this.maidArray[j];
							for (int l = 0; l < maid.body0.goSlot.Count; l++)
							{
								if (maid.body0.goSlot[l].obj != null)
								{
									DynamicSkirtBone fieldValue8 = MultipleMaids.GetFieldValue<BoneHair3, DynamicSkirtBone>(this.maidArray[j].body0.goSlot[l].bonehair3, "m_SkirtBone");
									if (fieldValue8 != null)
									{
										fieldValue8.m_vGravity = new Vector3(this.softG2.x, this.softG2.y, this.softG2.z);
										fieldValue8.UpdateParameters();
									}
								}
							}
						}
					}
				}
				if (!this.isVR)
				{
					int i;
					for (i = 0; i < 999; i++)
					{
						if (this.gDogu[i] != null)
						{
							this.gDogu[i].GetComponent<Renderer>().enabled = false;
							this.gDogu[i].SetActive(false);
							if (this.mDogu[i].del)
							{
								this.mDogu[i].del = false;
								Object.Destroy(this.doguBObject[i]);
								this.doguBObject.RemoveAt(i);
							}
							else if (this.mDogu[i].copy)
							{
								this.mDogu[i].copy = false;
								GameObject gameObject6 = Object.Instantiate<GameObject>(this.doguBObject[i]);
								gameObject6.transform.Translate(-0.3f, 0f, 0f);
								this.doguBObject.Add(gameObject6);
								gameObject6.name = this.doguBObject[i].name;
								this.doguCnt = this.doguBObject.Count - 1;
								this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
								this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
								this.gDogu[this.doguCnt].layer = 8;
								this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
								this.gDogu[this.doguCnt].SetActive(false);
								this.gDogu[this.doguCnt].transform.position = gameObject6.transform.position;
								this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
								this.mDogu[this.doguCnt].isScale = false;
								this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
								this.mDogu[this.doguCnt].maid = gameObject6;
								this.mDogu[this.doguCnt].angles = gameObject6.transform.eulerAngles;
								this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
								this.mDogu[this.doguCnt].ido = 1;
							}
							else if (this.mDogu[i].count > 0)
							{
								this.mDogu[i].count--;
								if (this.doguBObject.Count > i && this.doguBObject[i] != null && this.doguBObject[i].name.StartsWith("Particle/p"))
								{
									if (this.mDogu[i].count == 1)
									{
										this.doguBObject[i].SetActive(false);
									}
									if (this.mDogu[i].count == 0)
									{
										this.doguBObject[i].SetActive(true);
										string name = this.doguBObject[i].name;
										if (name != null)
										{
											if (!(name == "Particle/pLineY"))
											{
												if (!(name == "Particle/pLineP02"))
												{
													if (!(name == "Particle/pLine_act2"))
													{
														if (name == "Particle/pHeart01")
														{
															this.mDogu[i].count = 77;
														}
													}
													else
													{
														this.mDogu[i].count = 90;
													}
												}
												else
												{
													this.mDogu[i].count = 115;
												}
											}
											else
											{
												this.mDogu[i].count = 180;
											}
										}
									}
								}
							}
						}
					}
					if (Input.GetKey(KeyCode.Z) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.ikMode2 = 11;
					}
					else if (Input.GetKey(KeyCode.Z) && this.getModKeyPressing(MultipleMaids.modKey.Ctrl))
					{
						this.ikMode2 = 12;
					}
					else if (Input.GetKey(KeyCode.X) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						this.ikMode2 = 14;
					}
					else if (Input.GetKey(KeyCode.X))
					{
						this.ikMode2 = 9;
					}
					else if (Input.GetKey(KeyCode.Z))
					{
						this.ikMode2 = 10;
					}
					else if (Input.GetKey(KeyCode.C))
					{
						this.ikMode2 = 13;
					}
					else if (Input.GetKey(KeyCode.D))
					{
						this.ikMode2 = 15;
					}
					else if (Input.GetKey(KeyCode.V))
					{
						this.ikMode2 = 16;
					}
					else
					{
						this.ikMode2 = 0;
					}
					if (this.gBg != null)
					{
						if (!this.isCube3)
						{
							this.gBg.GetComponent<Renderer>().enabled = false;
							this.gBg.SetActive(false);
						}
						else
						{
							if (this.ikMode2 > 0 && this.ikMode2 != 15 && this.ikMode2 != 16)
							{
								this.gBg.GetComponent<Renderer>().enabled = true;
								this.gBg.SetActive(true);
							}
							else
							{
								this.gBg.GetComponent<Renderer>().enabled = false;
								this.gBg.SetActive(false);
							}
							if (this.ikMode2 == 10 || this.ikMode2 == 11 || this.ikMode2 == 12)
							{
								this.gBg.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
							}
							if (this.ikMode2 == 9 || this.ikMode2 == 14)
							{
								this.gBg.GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
								this.mBg.Update();
							}
							if (this.ikMode2 == 13)
							{
								this.gBg.GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f, 0.5f);
								this.mBg.Update();
							}
							if (this.ikMode2 == 13)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 13 && this.gBg)
								{
									this.mBg.ido = 5;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 5;
								}
							}
							else if (this.ikMode2 == 11)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 11 && this.gBg)
								{
									this.mBg.ido = 3;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 3;
								}
							}
							else if (this.ikMode2 == 12)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 12 && this.gBg)
								{
									this.mBg.ido = 2;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 2;
								}
							}
							else if (this.ikMode2 == 10)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 10 && this.gBg)
								{
									this.mBg.ido = 1;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 1;
								}
							}
							else if (this.ikMode2 == 9)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 9 && this.gBg)
								{
									this.mBg.ido = 4;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 4;
								}
							}
							else if (this.ikMode2 == 14)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 14 && this.gBg)
								{
									this.mBg.ido = 6;
									this.mBg.reset = true;
								}
								else
								{
									this.gBg.transform.position = this.bg.position;
									this.gBg.transform.eulerAngles = this.bg.eulerAngles;
									this.mBg.maid = this.bgObject;
									this.mBg.ido = 6;
								}
							}
						}
					}
					i = 0;
					while (i < this.lightIndex.Count)
					{
						if (this.gLight[0] == null)
						{
							this.gLight[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
							Material material = new Material(Shader.Find("Transparent/Diffuse"));
							material.color = new Color(0.5f, 0.5f, 1f, 0.8f);
							this.gLight[0].GetComponent<Renderer>().material = material;
							this.gLight[0].layer = 8;
							this.gLight[0].GetComponent<Renderer>().enabled = false;
							this.gLight[0].SetActive(false);
							this.gLight[0].transform.position = GameMain.Instance.MainLight.transform.position;
							this.mLight[0] = this.gLight[0].AddComponent<MouseDrag6>();
							this.mLight[0].obj = this.gLight[0];
							this.mLight[0].maid = GameMain.Instance.MainLight.gameObject;
							this.mLight[0].angles = GameMain.Instance.MainLight.gameObject.transform.eulerAngles;
							this.gLight[0].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
							this.mLight[0].ido = 1;
							this.mLight[0].isScale = false;
						}
						if (this.gLight[i] != null)
						{
							if (!this.isCube4)
							{
								this.gLight[i].GetComponent<Renderer>().enabled = false;
								this.gLight[i].SetActive(false);
							}
							else if (this.lightList[i].GetComponent<Light>().type == LightType.Spot || this.lightList[i].GetComponent<Light>().type == LightType.Point)
							{
								if (this.ikMode2 > 0 && this.ikMode2 != 15)
								{
									this.gLight[i].GetComponent<Renderer>().enabled = true;
									this.gLight[i].SetActive(true);
								}
								else
								{
									this.gLight[i].GetComponent<Renderer>().enabled = false;
									this.gLight[i].SetActive(false);
									this.mLight[i].isAlt = false;
								}
								if (this.ikMode2 == 10 || this.ikMode2 == 11 || this.ikMode2 == 12)
								{
									this.gLight[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
									if (this.mLight[i].isAlt)
									{
										this.gLight[i].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1f, 0.5f);
									}
								}
								if (this.ikMode2 == 9 || this.ikMode2 == 14)
								{
									this.gLight[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
									this.mLight[i].Update();
								}
								if (this.ikMode2 == 13)
								{
									this.gLight[i].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f, 0.5f);
									this.mLight[i].Update();
								}
								if (this.ikMode2 == 13)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 13 && this.gLight[i])
									{
										this.mLight[i].ido = 15;
										this.mLight[i].reset = true;
									}
									else
									{
										if (this.lightList[i].transform.localScale.x == 1f)
										{
											this.lightList[i].transform.localScale = new Vector3(this.lightRange[i], this.lightRange[i], this.lightRange[i]);
										}
										this.lightRange[i] = this.lightList[i].transform.localScale.x;
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].ido = 15;
									}
								}
								else if (this.ikMode2 == 11)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 11 && this.gLight[i])
									{
										this.mLight[i].ido = 3;
										this.mLight[i].reset = true;
									}
									else
									{
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.lightX[i] = this.gLight[i].transform.eulerAngles.x;
										this.lightY[i] = this.gLight[i].transform.eulerAngles.y;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].ido = 3;
									}
								}
								else if (this.ikMode2 == 12)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 12 && this.gLight[i])
									{
										this.mLight[i].ido = 2;
										this.mLight[i].reset = true;
									}
									else
									{
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].ido = 2;
									}
								}
								else if (this.ikMode2 == 10)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 10 && this.gLight[i])
									{
										this.mLight[i].ido = 1;
										this.mLight[i].reset = true;
									}
									else
									{
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].maidArray = this.lightList.ToArray();
										this.mLight[i].mArray = this.mLight.ToArray<MouseDrag6>();
										this.mLight[i].ido = 1;
									}
								}
								else if (this.ikMode2 == 9)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 9 && this.gLight[i])
									{
										this.mLight[i].ido = 4;
										this.mLight[i].reset = true;
									}
									else
									{
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.lightX[i] = this.gLight[i].transform.eulerAngles.x;
										this.lightY[i] = this.gLight[i].transform.eulerAngles.y;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].ido = 4;
									}
								}
								else if (this.ikMode2 == 14)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 14 && this.gLight[i])
									{
										this.mLight[i].ido = 6;
										this.mLight[i].reset = true;
									}
									else
									{
										this.gLight[i].transform.position = this.lightList[i].transform.position;
										this.gLight[i].transform.eulerAngles = this.lightList[i].transform.eulerAngles;
										this.lightX[i] = this.gLight[i].transform.eulerAngles.x;
										this.lightY[i] = this.gLight[i].transform.eulerAngles.y;
										this.mLight[i].maid = this.lightList[i];
										this.mLight[i].ido = 6;
									}
								}
							}
						}
						//IL_31E59:
						i++;
						continue;
						//goto IL_31E59;
					}
					for (i = 0; i < this.doguBObject.Count; i++)
					{
						if (!this.isCube2)
						{
							this.gDogu[i].GetComponent<Renderer>().enabled = false;
							this.gDogu[i].SetActive(false);
						}
						else
						{
							if (this.ikMode2 > 0)
							{
								this.gDogu[i].GetComponent<Renderer>().enabled = true;
								this.gDogu[i].SetActive(true);
							}
							else
							{
								this.gDogu[i].GetComponent<Renderer>().enabled = false;
								this.gDogu[i].SetActive(false);
								this.mDogu[i].isAlt = false;
							}
							if (this.ikMode2 == 10 || this.ikMode2 == 11 || this.ikMode2 == 12)
							{
								this.gDogu[i].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
								if (this.mDogu[i].isAlt)
								{
									this.gDogu[i].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1f, 0.5f);
								}
							}
							if (this.ikMode2 == 9 || this.ikMode2 == 14)
							{
								this.gDogu[i].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
								this.mDogu[i].Update();
							}
							if (this.ikMode2 == 15)
							{
								this.gDogu[i].GetComponent<Renderer>().material.color = new Color(1f, 0.3f, 0.3f, 0.5f);
							}
							if (this.ikMode2 == 16)
							{
								this.gDogu[i].GetComponent<Renderer>().material.color = new Color(0.7f, 0.3f, 0.7f, 0.5f);
								this.mDogu[i].Update();
							}
							if (this.ikMode2 == 13)
							{
								this.gDogu[i].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f, 0.5f);
								this.mDogu[i].Update();
							}
							if (this.ikMode2 == 13)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 13 && this.gDogu[i])
								{
									this.mDogu[i].ido = 5;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 5;
								}
							}
							else if (this.ikMode2 == 11)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 11 && this.gDogu[i])
								{
									this.mDogu[i].ido = 3;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 3;
								}
							}
							else if (this.ikMode2 == 12)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 12 && this.gDogu[i])
								{
									this.mDogu[i].ido = 2;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 2;
								}
							}
							else if (this.ikMode2 == 10)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 10 && this.gDogu[i])
								{
									this.mDogu[i].ido = 1;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].maidArray = this.doguBObject.ToArray();
									this.mDogu[i].mArray = this.mDogu.ToArray<MouseDrag6>();
									this.mDogu[i].ido = 1;
								}
							}
							else if (this.ikMode2 == 9)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 9 && this.gDogu[i])
								{
									this.mDogu[i].ido = 4;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 4;
								}
							}
							else if (this.ikMode2 == 14)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 14 && this.gDogu[i])
								{
									this.mDogu[i].ido = 6;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 6;
								}
							}
							else if (this.ikMode2 == 15)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 15 && this.gDogu[i])
								{
									this.mDogu[i].ido = 7;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 7;
								}
							}
							else if (this.ikMode2 == 16)
							{
								if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 16 && this.gDogu[i])
								{
									this.mDogu[i].ido = 8;
									this.mDogu[i].reset = true;
								}
								else
								{
									this.gDogu[i].transform.position = this.doguBObject[i].transform.position;
									this.gDogu[i].transform.eulerAngles = this.doguBObject[i].transform.eulerAngles;
									this.mDogu[i].maid = this.doguBObject[i];
									this.mDogu[i].ido = 8;
								}
							}
						}
					}
					this.ikModeOld2 = this.ikMode2;
				}
				if (this.isVR)
				{
					if (Input.GetKeyDown(KeyCode.F8) && this.getModKeyPressing(MultipleMaids.modKey.Shift))
					{
						if (!this.isVR2)
						{
							this.isVR2 = true;
							base.Preferences["config"]["shift_f8"].Value = "true";
							base.SaveConfig();
							GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
						}
						else
						{
							this.isVR2 = false;
							base.Preferences["config"]["shift_f8"].Value = "false";
							base.SaveConfig();
							GameMain.Instance.SoundMgr.PlaySe("se003.ogg", false);
						}
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha1))
					{
						this.loadScene = 1;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha2))
					{
						this.loadScene = 2;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha3))
					{
						this.loadScene = 3;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha4))
					{
						this.loadScene = 4;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha5))
					{
						this.loadScene = 5;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha6))
					{
						this.loadScene = 6;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha7))
					{
						this.loadScene = 7;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha8))
					{
						this.loadScene = 8;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha9))
					{
						this.loadScene = 9;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F8) && Input.GetKeyDown(KeyCode.Alpha0))
					{
						this.loadScene = 10;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					for (int j = 0; j < 7; j++)
					{
						if (!this.maidArray[j])
						{
							this.maidArray[j] = GameMain.Instance.CharacterMgr.GetMaid(j);
						}
					}
				}
			}
		}
	}
}
