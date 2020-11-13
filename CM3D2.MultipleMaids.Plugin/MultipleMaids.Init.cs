using ExIni;
using MyRoomCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.PostProcessing;
using wf;

namespace CM3D2.MultipleMaids.Plugin
{
    public partial class MultipleMaids
    {
		public void init2()
		{
			for (int i = 0; i < GameMain.Instance.CharacterMgr.GetStockMaidCount(); i++)
			{
				Maid maid = GameMain.Instance.CharacterMgr.GetStockMaidList()[i];
				if (this.goSlot[i] != null)
				{
					maid.body0.goSlot = new List<TBodySkin>(this.goSlot[i]);
					if (!GameMain.Instance.VRMode)
					{
						maid.body0.quaDefEyeL.eulerAngles = this.eyeL[i];
						maid.body0.quaDefEyeR.eulerAngles = this.eyeR[i];
					}
					this.shodaiFlg[i] = false;
				}
				if (this.SkirtListArray[i] != null)
				{
					for (int j = 0; j < maid.body0.goSlot.Count; j++)
					{
						MultipleMaids.SetFieldValue8<BoneHair3, DynamicSkirtBone>(maid.body0.goSlot[j].bonehair3, "m_SkirtBone", this.SkirtListArray[i][j]);
					}
				}
			}
			for (int k = 0; k < this.maxMaidCnt; k++)
			{
				if (this.maidArray[k])
				{
					this.maidArray[k].StopKuchipakuPattern();
				}
				if (this.maidArray[k])
				{
					this.maidArray[k].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
				}
				if (this.maidArray[k] && this.maidArray[k].Visible && this.maidArray[k].body0.isLoadedBody)
				{
					this.maidArray[k].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
					this.maidArray[k].SetAutoTwistAll(true);
					this.maidArray[k].body0.MuneYureL(1f);
					this.maidArray[k].body0.MuneYureR(1f);
					this.maidArray[k].body0.jbMuneL.enabled = true;
					this.maidArray[k].body0.jbMuneR.enabled = true;
				}
				if (this.maidArray[k])
				{
					this.maidArray[k].boMabataki = true;
					if (this.maidArray[k].body0.isLoadedBody)
					{
						Maid maid2 = this.maidArray[k];
						for (int j = 0; j < maid2.body0.goSlot.Count; j++)
						{
							List<THair1> fieldValue = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid2.body0.goSlot[j].bonehair, "hair1list");
							for (int i = 0; i < fieldValue.Count; i++)
							{
								fieldValue[i].SoftG = new Vector3(0f, -0.003f, 0f);
							}
						}
					}
				}
			}
			this.goSlot = new List<TBodySkin>[500];
			this.bodyHit = new List<TBodyHit>[500];
			this.allowUpdate = true;
			this.okFlg = false;
			this.isDance = false;
			this.isDanceChu = false;
			this.isSavePose = false;
			this.bgIndex = 0;
			this.bgIndexB = 0;
			this.bg.localScale = new Vector3(1f, 1f, 1f);
			this.softG = new Vector3(0f, -0.003f, 0f);
			this.softG2 = new Vector3(0f, -0.005f, 0f);
			if (!GameMain.Instance.VRMode)
			{
				Bloom fieldValue2 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
				fieldValue2.enabled = true;
				fieldValue2.bloomIntensity = 2.85f;
				fieldValue2.hdr = 0;
				fieldValue2.bloomThreshholdColor = new Color(1f, 1f, 1f);
				fieldValue2.bloomBlurIterations = 3;
			}
			this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
			this.maidCnt = 1;
			this.editMaid = 0;
			this.wearIndex = 0;
			this.bGuiMessage = false;
			this.inName = "";
			this.inName2 = "";
			this.inName3 = "";
			this.inText = "";
			this.fontSize = 25;
			this.isMessage = false;
			GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
			GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
			MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
			MessageClass messageClass = new MessageClass(gameObject2, messageWindowMgr);
			messageClass.Clear();
			messageWindowMgr.CloseMessageWindowPanel();
			UILabel component = UTY.GetChildObject(gameObject2, "MessageViewer/MsgParent/Message", false).GetComponent<UILabel>();
			MultipleMaids.SetFieldValue2<UILabel, int>(component, "mFontSize", this.fontSize);
			this.ikMaid = 0;
			this.ikBui = 0;
			this.faceFlg = false;
			this.faceFlg2 = false;
			this.sceneFlg = false;
			this.poseFlg = false;
			this.kankyoFlg = false;
			this.kankyo2Flg = false;
			this.doguSelectFlg1 = true;
			this.doguSelectFlg2 = false;
			this.doguSelectFlg3 = false;
			this.unLockFlg = false;
			this.isNamida = false;
			this.isNamidaH = false;
			this.isSekimenH = false;
			this.isHohoH = false;
			this.isTear1 = false;
			this.isTear2 = false;
			this.isTear3 = false;
			this.isShock = false;
			this.isYodare = false;
			this.isHoho = false;
			this.isHoho2 = false;
			this.isHohos = false;
			this.isHohol = false;
			this.isToothoff = false;
			this.isNosefook = false;
			this.isFaceInit = false;
			this.isPoseInit = false;
			this.isWear = true;
			this.isSkirt = true;
			this.isMekure1 = false;
			this.isMekure2 = false;
			this.isZurasi = false;
			this.isBra = true;
			this.isPanz = true;
			this.isHeadset = true;
			this.isAccUde = true;
			this.isStkg = true;
			this.isShoes = true;
			this.isGlove = true;
			this.isMegane = true;
			this.isAccSenaka = true;
			this.isMaid = true;
			this.isF6 = false;
			this.isPanel = true;
			this.isBloom = false;
			this.isBloomA = false;
			this.isBlur = false;
			this.isBlur2 = false;
			this.bloom1 = 2.85f;
			this.bloom2 = 3f;
			this.bloom3 = 0f;
			this.bloom4 = 0f;
			this.bloom5 = 0f;
			this.blur1 = -3.98f;
			this.blur4 = -0.75f;
			this.blur2 = 0.9f;
			this.blur3 = 4.19f;
			this.bokashi = 0f;
			this.kamiyure = 0f;
			this.isDepth = false;
			this.isDepthA = false;
			this.depth1 = 3f;
			this.depth2 = 0.1f;
			this.depth3 = 15f;
			this.depth4 = 2.5f;
			this.isFog = false;
			this.fog1 = 4f;
			this.fog2 = 1f;
			this.fog3 = 1f;
			this.fog4 = 0f;
			this.fog5 = 1f;
			this.fog6 = 1f;
			this.fog7 = 1f;
			this.isSepia = false;
			this.isSepian = false;
			this.isBloomS = true;
			this.isDepthS = false;
			this.isBlurS = false;
			this.isFogS = false;
			this.isHairSetting = true;
			this.isSkirtSetting = false;
			if (this.depth_field_ != null)
			{
				this.depth_field_.enabled = false;
			}
			if (this.fog_ != null)
			{
				this.fog_.enabled = false;
			}
			if (this.sepia_tone_ != null)
			{
				this.sepia_tone_.enabled = false;
			}
			this.isCube = false;
			this.isCube2 = true;
			this.isCube3 = false;
			this.isCube4 = true;
			this.isCubeS = false;
			this.cubeSize = 0.12f;
			this.isPoseEdit = false;
			this.isFaceEdit = false;
			this.bgmIndex = 0;
			if (this.sceneLevel == 5)
			{
				this.bgmIndex = 2;
			}
			this.effectIndex = 0;
			this.selectMaidIndex = 0;
			this.copyIndex = 0;
			this.selectLightIndex = 0;
			this.doguB2Index = 0;
			this.parIndex = 0;
			this.parIndex1 = 0;
			this.isEditNo = 0;
			this.editSelectMaid = null;
			for (int k = 0; k < 10; k++)
			{
				this.date[k] = "";
				this.ninzu[k] = "";
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
			for (int k = 0; k < this.maxMaidCnt; k++)
			{
				this.danceFace[k] = 0f;
				this.FaceName[k] = "";
				this.FaceName2[k] = "";
				this.FaceName3[k] = "";
				this.isStop[k] = false;
				this.isBone[k] = false;
				this.isBoneN[k] = false;
				this.poseIndex[k] = 0;
				this.itemIndex[k] = 0;
				this.itemIndex2[k] = 0;
				this.faceIndex[k] = 0;
				this.faceBlendIndex[k] = 0;
				this.headEyeIndex[k] = 0;
				this.isLock[k] = false;
				this.isFace[k] = true;
				this.mekure1[k] = false;
				this.mekure2[k] = false;
				this.zurasi[k] = false;
				this.mekure1n[k] = false;
				this.mekure2n[k] = false;
				this.zurasin[k] = false;
				this.isLook[k] = false;
				this.isLookn[k] = false;
				this.lookX[k] = 0f;
				this.lookY[k] = -0f;
				this.lookXn[k] = 0f;
				this.lookYn[k] = -0f;
				this.voice1[k] = false;
				this.voice2[k] = false;
				this.voice1n[k] = false;
				this.voice2n[k] = false;
				this.hanten[k] = false;
				this.hantenn[k] = false;
				this.kotei[k] = false;
				this.xFlg[k] = false;
				this.zFlg[k] = false;
				this.ikMode[k] = 0;
				UnityEngine.Object.Destroy(this.gMaid[k]);
				UnityEngine.Object.Destroy(this.gMaidC[k]);
				UnityEngine.Object.Destroy(this.gHandL[k]);
				UnityEngine.Object.Destroy(this.gArmL[k]);
				UnityEngine.Object.Destroy(this.gHandR[k]);
				UnityEngine.Object.Destroy(this.gArmR[k]);
				UnityEngine.Object.Destroy(this.gFootL[k]);
				UnityEngine.Object.Destroy(this.gHizaL[k]);
				UnityEngine.Object.Destroy(this.gFootR[k]);
				UnityEngine.Object.Destroy(this.gHizaR[k]);
				UnityEngine.Object.Destroy(this.gClavicleL[k]);
				UnityEngine.Object.Destroy(this.gClavicleR[k]);
				UnityEngine.Object.Destroy(this.gNeck[k]);
				UnityEngine.Object.Destroy(this.gSpine[k]);
				UnityEngine.Object.Destroy(this.gSpine0a[k]);
				UnityEngine.Object.Destroy(this.gSpine1a[k]);
				UnityEngine.Object.Destroy(this.gSpine1[k]);
				UnityEngine.Object.Destroy(this.gPelvis[k]);
				UnityEngine.Object.Destroy(this.gizmoHandL[k]);
				UnityEngine.Object.Destroy(this.gizmoHandR[k]);
				UnityEngine.Object.Destroy(this.gizmoFootL[k]);
				UnityEngine.Object.Destroy(this.gizmoFootR[k]);
				this.HandL1[k] = null;
				for (int i = 0; i < 30; i++)
				{
					UnityEngine.Object.Destroy(this.gFinger[k, i]);
				}
				for (int i = 0; i < 12; i++)
				{
					UnityEngine.Object.Destroy(this.gFinger2[k, i]);
				}
				if (this.isIKAll)
				{
					this.isIK[k] = true;
				}
				else
				{
					this.isIK[k] = false;
				}
				this.pHandL[k] = 0;
				this.pHandR[k] = 0;
				this.muneIKL[k] = false;
				this.muneIKR[k] = false;
			}
			if (this.kami)
			{
				this.kami.SetActive(false);
			}
			this.danceCheckIndex = 0;
			for (int i = 0; i < this.danceCheck.Length; i++)
			{
				this.danceCheck[this.danceCheckIndex] = 1f;
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
			this.isIdx1 = false;
			this.isIdx2 = false;
			this.isIdx3 = false;
			this.isIdx4 = false;
			this.bgObject.SetActive(true);
			GameMain.Instance.MainLight.Reset();
			GameMain.Instance.MainLight.SetIntensity(0.95f);
			GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 30f;
			GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
			GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
			GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
			for (int l = 0; l < this.doguBObject.Count; l++)
			{
				UnityEngine.Object.Destroy(this.doguBObject[l]);
			}
			this.doguBObject.Clear();
			this.doguB2Index = 0;
			this.parIndex = 0;
			this.parIndex1 = 0;
			this.doguCombo2.selectedItemIndex = 0;
			for (int l = 0; l < this.doguCombo.Length; l++)
			{
				this.doguCombo[l] = new ComboBox2();
				this.doguCombo[l].selectedItemIndex = 0;
			}
			this.parCombo.selectedItemIndex = 0;
			this.parCombo1.selectedItemIndex = 0;
			for (int l = 1; l < this.lightList.Count; l++)
			{
				UnityEngine.Object.Destroy(this.lightList[l]);
			}
			this.lightList = new List<GameObject>();
			this.lightList.Add(GameMain.Instance.MainLight.gameObject);
			this.lightComboList = new GUIContent[this.lightList.Count];
			this.lightComboList[0] = new GUIContent("メイン");
			this.lightCombo.selectedItemIndex = 0;
			this.selectLightIndex = 0;
			this.bgCombo.selectedItemIndex = 0;
			this.kankyoCombo.selectedItemIndex = 0;
			this.bgCombo2.selectedItemIndex = 0;
			this.itemCombo2.selectedItemIndex = 0;
			this.myCombo.selectedItemIndex = 0;
			this.slotCombo.selectedItemIndex = 0;
			this.sortList.Clear();
			this.itemDataList.Clear();
			this.itemDataListMod.Clear();
			this.itemDataListNMod.Clear();
			this.scrollPos = new Vector2(0f, 0f);
			Vignetting component2 = GameMain.Instance.MainCamera.gameObject.GetComponent<Vignetting>();
			component2.mode = 0;
			component2.intensity = -3.98f;
			component2.chromaticAberration = -0.75f;
			component2.axialAberration = 1.18f;
			component2.blurSpread = 4.19f;
			component2.luminanceDependency = 0.494f;
			component2.blurDistance = 1.71f;
			component2.enabled = false;
			this.doguIndex.Clear();
			this.doguSelectIndex = 0;
			for (int k = 0; k < this.doguObject.Count; k++)
			{
				if (this.doguObject[k] != null)
				{
					UnityEngine.Object.Destroy(this.doguObject[k]);
					this.doguObject[k] = null;
				}
			}
			this.doguObject.Clear();
		}

		public void init()
		{
			this.isInit = true;
			this.isVR = GameMain.Instance.VRMode;
			for (int i = 0; i < GameMain.Instance.CharacterMgr.GetStockMaidCount(); i++)
			{
				Maid maid = GameMain.Instance.CharacterMgr.GetStockMaidList()[i];
				if (this.goSlot[i] != null)
				{
					maid.body0.goSlot = new List<TBodySkin>(this.goSlot[i]);
					if (!GameMain.Instance.VRMode)
					{
						maid.body0.quaDefEyeL.eulerAngles = this.eyeL[i];
						maid.body0.quaDefEyeR.eulerAngles = this.eyeR[i];
					}
					this.shodaiFlg[i] = false;
				}
				if (this.SkirtListArray[i] != null)
				{
					for (int j = 0; j < maid.body0.goSlot.Count; j++)
					{
						MultipleMaids.SetFieldValue8<BoneHair3, DynamicSkirtBone>(maid.body0.goSlot[j].bonehair3, "m_SkirtBone", this.SkirtListArray[i][j]);
					}
				}
			}
			for (int k = 0; k < this.maxMaidCnt; k++)
			{
				if (this.maidArray[k])
				{
					this.maidArray[k].StopKuchipakuPattern();
				}
				if (this.maidArray[k])
				{
					this.maidArray[k].body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
				}
				if (this.maidArray[k] && this.maidArray[k].Visible && this.maidArray[k].body0.isLoadedBody)
				{
					this.maidArray[k].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
					this.maidArray[k].SetAutoTwistAll(true);
					this.maidArray[k].body0.MuneYureL(1f);
					this.maidArray[k].body0.MuneYureR(1f);
					this.maidArray[k].body0.jbMuneL.enabled = true;
					this.maidArray[k].body0.jbMuneR.enabled = true;
				}
				if (this.maidArray[k])
				{
					this.maidArray[k].body0.SetMask(TBody.SlotID.wear, true);
					this.maidArray[k].body0.SetMask(TBody.SlotID.skirt, true);
					this.maidArray[k].body0.SetMask(TBody.SlotID.bra, true);
					this.maidArray[k].body0.SetMask(TBody.SlotID.panz, true);
					this.maidArray[k].body0.SetMask(TBody.SlotID.mizugi, true);
					this.maidArray[k].body0.SetMask(TBody.SlotID.onepiece, true);
					if (this.maidArray[k].body0.isLoadedBody)
					{
						Maid maid2 = this.maidArray[k];
						for (int j = 0; j < maid2.body0.goSlot.Count; j++)
						{
							List<THair1> fieldValue = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid2.body0.goSlot[j].bonehair, "hair1list");
							for (int i = 0; i < fieldValue.Count; i++)
							{
								fieldValue[i].SoftG = new Vector3(0f, -0.003f, 0f);
							}
						}
					}
				}
				this.maidArray[k] = null;
			}
			this.goSlot = new List<TBodySkin>[500];
			this.bodyHit = new List<TBodyHit>[500];
			this.allowUpdate = true;
			if (this.okFlg)
			{
				GameMain.Instance.CharacterMgr.ResetCharaPosAll();
				for (int i = 0; i < GameMain.Instance.CharacterMgr.GetStockMaidCount(); i++)
				{
					if (this.sceneLevel != 5 || i != this.editMaid)
					{
						Maid maid = GameMain.Instance.CharacterMgr.GetStockMaidList()[i];
						maid.transform.localScale = new Vector3(1f, 1f, 1f);
						TBody.MaskMode maskMode = (TBody.MaskMode)Enum.Parse(typeof(TBody.MaskMode), "None");
						maid.body0.SetMaskMode(maskMode);
						maid.boMabataki = true;
					}
				}
				GameMain.Instance.CharacterMgr.DeactivateCharaAll();
				for (int i = 11; i < GameMain.Instance.CharacterMgr.GetStockMaidCount(); i++)
				{
					Maid maid = GameMain.Instance.CharacterMgr.GetStockMaidList()[i];
					Vector3 zero = Vector3.zero;
					maid.SetPos(zero);
					maid.SetRot(zero);
					maid.SetPosOffset(zero);
					if (maid.body0 != null)
					{
						maid.body0.SetBoneHitHeightY(0f);
					}
					maid.Visible = false;
					maid.ActiveSlotNo = -1;
					maid.DelPrefabAll();
					if (maid.body0.isLoadedBody)
					{
						maid.CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
						maid.SetAutoTwistAll(true);
					}
					maid.boMabataki = true;
				}
			}
			this.okFlg = false;
			this.isDance = false;
			this.isDanceChu = false;
			this.isSavePose = false;
			this.bgIndex = 0;
			this.bgIndexB = 0;
			this.bg.localScale = new Vector3(1f, 1f, 1f);
			this.softG = new Vector3(0f, -0.003f, 0f);
			this.softG2 = new Vector3(0f, -0.005f, 0f);
			if (!GameMain.Instance.VRMode)
			{
				Bloom fieldValue2 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
				fieldValue2.enabled = true;
				fieldValue2.bloomIntensity = 2.85f;
				fieldValue2.hdr = 0;
				fieldValue2.bloomThreshholdColor = new Color(1f, 1f, 1f);
				fieldValue2.bloomBlurIterations = 3;
			}
			this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
			this.maidCnt = 0;
			this.wearIndex = 0;
			this.isF6 = false;
			this.faceFlg = false;
			this.faceFlg2 = false;
			this.sceneFlg = false;
			this.poseFlg = false;
			this.kankyoFlg = false;
			this.kankyo2Flg = false;
			this.unLockFlg = false;
			this.inName = "";
			this.inName2 = "";
			this.inName3 = "";
			this.inText = "";
			this.fontSize = 25;
			this.bGuiMessage = false;
			this.isMessage = false;
			GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
			GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
			MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
			MessageClass messageClass = new MessageClass(gameObject2, messageWindowMgr);
			messageClass.Clear();
			messageWindowMgr.CloseMessageWindowPanel();
			UILabel component = UTY.GetChildObject(gameObject2, "MessageViewer/MsgParent/Message", false).GetComponent<UILabel>();
			MultipleMaids.SetFieldValue2<UILabel, int>(component, "mFontSize", this.fontSize);
			this.ikMaid = 0;
			this.ikBui = 0;
			this.isNamida = false;
			this.isNamidaH = false;
			this.isSekimenH = false;
			this.isHohoH = false;
			this.isTear1 = false;
			this.isTear2 = false;
			this.isTear3 = false;
			this.isShock = false;
			this.isYodare = false;
			this.isHoho = false;
			this.isHoho2 = false;
			this.isHohos = false;
			this.isHohol = false;
			this.isToothoff = false;
			this.isNosefook = false;
			this.isFaceInit = false;
			this.isPoseInit = false;
			this.isWear = true;
			this.isSkirt = true;
			this.isMekure1 = false;
			this.isMekure2 = false;
			this.isZurasi = false;
			this.isBra = true;
			this.isPanz = true;
			this.isHeadset = true;
			this.isAccUde = true;
			this.isStkg = true;
			this.isShoes = true;
			this.isGlove = true;
			this.isMegane = true;
			this.isAccSenaka = true;
			this.isMaid = true;
			this.isPanel = true;
			this.isBloom = false;
			this.isBloomA = false;
			this.isBlur = false;
			this.isBlur2 = false;
			this.bloom1 = 2.85f;
			this.bloom2 = 3f;
			this.bloom3 = 0f;
			this.bloom4 = 0f;
			this.bloom5 = 0f;
			this.blur1 = -3.98f;
			this.blur4 = -0.75f;
			this.blur2 = 0.9f;
			this.blur3 = 4.19f;
			this.bokashi = 0f;
			this.kamiyure = 0f;
			this.isDepth = false;
			this.isDepthA = false;
			this.depth1 = 3f;
			this.depth2 = 0.1f;
			this.depth3 = 15f;
			this.depth4 = 2.5f;
			this.isFog = false;
			this.fog1 = 4f;
			this.fog2 = 1f;
			this.fog3 = 1f;
			this.fog4 = 0f;
			this.fog5 = 1f;
			this.fog6 = 1f;
			this.fog7 = 1f;
			this.isSepia = false;
			this.isSepian = false;
			this.isBloomS = true;
			this.isDepthS = false;
			this.isBlurS = false;
			this.isFogS = false;
			this.isHairSetting = true;
			this.isSkirtSetting = false;
			if (this.depth_field_ != null)
			{
				this.depth_field_.enabled = false;
			}
			if (this.fog_ != null)
			{
				this.fog_.enabled = false;
			}
			if (this.sepia_tone_ != null)
			{
				this.sepia_tone_.enabled = false;
			}
			this.isCube = false;
			this.isCube2 = true;
			this.isCube3 = false;
			this.isCube4 = true;
			this.isCubeS = false;
			this.cubeSize = 0.12f;
			this.isPoseEdit = false;
			this.isFaceEdit = false;
			this.bgmIndex = 0;
			if (this.sceneLevel == 5)
			{
				this.bgmIndex = 2;
			}
			this.effectIndex = 0;
			this.selectMaidIndex = 0;
			this.copyIndex = 0;
			this.selectLightIndex = 0;
			this.doguB2Index = 0;
			this.parIndex = 0;
			this.parIndex1 = 0;
			this.isEditNo = 0;
			this.editSelectMaid = null;
			for (int k = 0; k < 10; k++)
			{
				this.date[k] = "";
				this.ninzu[k] = "";
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
			for (int k = 0; k < this.maxMaidCnt; k++)
			{
				this.danceFace[k] = 0f;
				this.FaceName[k] = "";
				this.FaceName2[k] = "";
				this.FaceName3[k] = "";
				this.isStop[k] = false;
				this.isBone[k] = false;
				this.isBoneN[k] = false;
				this.poseIndex[k] = 0;
				this.itemIndex[k] = 0;
				this.itemIndex2[k] = 0;
				this.faceIndex[k] = 0;
				this.faceBlendIndex[k] = 0;
				this.headEyeIndex[k] = 0;
				this.isLock[k] = false;
				this.isFace[k] = true;
				this.mekure1[k] = false;
				this.mekure2[k] = false;
				this.zurasi[k] = false;
				this.mekure1n[k] = false;
				this.mekure2n[k] = false;
				this.zurasin[k] = false;
				this.isLook[k] = false;
				this.isLookn[k] = false;
				this.lookX[k] = 0f;
				this.lookY[k] = -0f;
				this.lookXn[k] = 0f;
				this.lookYn[k] = -0f;
				this.voice1[k] = false;
				this.voice2[k] = false;
				this.voice1n[k] = false;
				this.voice2n[k] = false;
				this.hanten[k] = false;
				this.hantenn[k] = false;
				this.kotei[k] = false;
				this.xFlg[k] = false;
				this.zFlg[k] = false;
				this.ikMode[k] = 0;
				UnityEngine.Object.Destroy(this.gMaid[k]);
				UnityEngine.Object.Destroy(this.gMaidC[k]);
				UnityEngine.Object.Destroy(this.gHandL[k]);
				UnityEngine.Object.Destroy(this.gArmL[k]);
				UnityEngine.Object.Destroy(this.gHandR[k]);
				UnityEngine.Object.Destroy(this.gArmR[k]);
				UnityEngine.Object.Destroy(this.gFootL[k]);
				UnityEngine.Object.Destroy(this.gHizaL[k]);
				UnityEngine.Object.Destroy(this.gFootR[k]);
				UnityEngine.Object.Destroy(this.gHizaR[k]);
				UnityEngine.Object.Destroy(this.gClavicleL[k]);
				UnityEngine.Object.Destroy(this.gClavicleR[k]);
				UnityEngine.Object.Destroy(this.gNeck[k]);
				UnityEngine.Object.Destroy(this.gSpine[k]);
				UnityEngine.Object.Destroy(this.gSpine0a[k]);
				UnityEngine.Object.Destroy(this.gSpine1a[k]);
				UnityEngine.Object.Destroy(this.gSpine1[k]);
				UnityEngine.Object.Destroy(this.gPelvis[k]);
				UnityEngine.Object.Destroy(this.gizmoHandL[k]);
				UnityEngine.Object.Destroy(this.gizmoHandR[k]);
				UnityEngine.Object.Destroy(this.gizmoFootL[k]);
				UnityEngine.Object.Destroy(this.gizmoFootR[k]);
				this.HandL1[k] = null;
				for (int i = 0; i < 30; i++)
				{
					UnityEngine.Object.Destroy(this.gFinger[k, i]);
				}
				for (int i = 0; i < 12; i++)
				{
					UnityEngine.Object.Destroy(this.gFinger2[k, i]);
				}
				if (this.isIKAll)
				{
					this.isIK[k] = true;
				}
				else
				{
					this.isIK[k] = false;
				}
				this.pHandL[k] = 0;
				this.pHandR[k] = 0;
				this.muneIKL[k] = false;
				this.muneIKR[k] = false;
			}
			if (this.kami)
			{
				this.kami.SetActive(false);
			}
			this.danceCheckIndex = 0;
			for (int i = 0; i < this.danceCheck.Length; i++)
			{
				this.danceCheck[this.danceCheckIndex] = 1f;
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
			this.isIdx1 = false;
			this.isIdx2 = false;
			this.isIdx3 = false;
			this.isIdx4 = false;
			UnityEngine.Object.Destroy(this.cameraObj);
			UnityEngine.Object.Destroy(this.subcamera);
			this.bgObject.SetActive(true);
			GameMain.Instance.MainLight.Reset();
			GameMain.Instance.MainLight.SetIntensity(0.95f);
			GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = 30f;
			GameMain.Instance.MainLight.GetComponent<Light>().range = 10f;
			GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
			GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
			if (!GameMain.Instance.VRMode)
			{
				this.m_material = new Material(Shader.Find("Transparent/Diffuse"));
				this.m_material.color = new Color(0.4f, 0.4f, 1f, 0.8f);
				this.m_UOCamera = MultipleMaids.GetFieldValue<CameraMain, UltimateOrbitCamera>(this.mainCamera, "m_UOCamera");
				if (this.sceneLevel == 3)
				{
					this.m_UOCamera.enabled = true;
				}
			}
			for (int l = 0; l < this.doguBObject.Count; l++)
			{
				UnityEngine.Object.Destroy(this.doguBObject[l]);
			}
			this.doguBObject.Clear();
			this.doguB2Index = 0;
			this.parIndex = 0;
			this.parIndex1 = 0;
			this.doguCombo2.selectedItemIndex = 0;
			for (int l = 0; l < this.doguCombo.Length; l++)
			{
				this.doguCombo[l] = new ComboBox2();
				this.doguCombo[l].selectedItemIndex = 0;
			}
			this.parCombo.selectedItemIndex = 0;
			this.parCombo1.selectedItemIndex = 0;
			for (int l = 1; l < this.lightList.Count; l++)
			{
				UnityEngine.Object.Destroy(this.lightList[l]);
			}
			this.lightList = new List<GameObject>();
			this.lightList.Add(GameMain.Instance.MainLight.gameObject);
			this.lightComboList = new GUIContent[this.lightList.Count];
			this.lightComboList[0] = new GUIContent("メイン");
			this.lightCombo.selectedItemIndex = 0;
			this.selectLightIndex = 0;
			this.bgCombo.selectedItemIndex = 0;
			this.kankyoCombo.selectedItemIndex = 0;
			this.bgCombo2.selectedItemIndex = 0;
			this.itemCombo2.selectedItemIndex = 0;
			this.myCombo.selectedItemIndex = 0;
			this.slotCombo.selectedItemIndex = 0;
			this.sortList.Clear();
			this.itemDataList.Clear();
			this.itemDataListMod.Clear();
			this.itemDataListNMod.Clear();
			this.scrollPos = new Vector2(0f, 0f);
			if (!GameMain.Instance.VRMode)
			{
				Vignetting component2 = GameMain.Instance.MainCamera.gameObject.GetComponent<Vignetting>();
				component2.mode = 0;
				component2.intensity = -3.98f;
				component2.chromaticAberration = -0.75f;
				component2.axialAberration = 1.18f;
				component2.blurSpread = 4.19f;
				component2.luminanceDependency = 0.494f;
				component2.blurDistance = 1.71f;
				component2.enabled = false;
			}
			this.doguIndex.Clear();
			this.doguSelectIndex = 0;
			for (int k = 0; k < this.doguObject.Count; k++)
			{
				if (this.doguObject[k] != null)
				{
					UnityEngine.Object.Destroy(this.doguObject[k]);
					this.doguObject[k] = null;
				}
			}
			this.doguObject.Clear();
			string text = Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose";
			if (!File.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path3 = Path.GetFullPath(".\\") + "Mod\\MultipleMaidsScene";
			if (!File.Exists(path3))
			{
				Directory.CreateDirectory(path3);
			}
			int num = this.countS;
			Action<string, List<string>> action = delegate (string path, List<string> result_list)
			{
				string[] files = Directory.GetFiles(path);
				this.countS = 0;
				for (int n = 0; n < files.Length; n++)
				{
					if (Path.GetExtension(files[n]) == ".anm")
					{
						string text4 = files[n].Split(new char[]
						{
							'\\'
						})[files[n].Split(new char[]
						{
							'\\'
						}).Length - 1];
						text4 = text4.Split(new char[]
						{
							'.'
						})[0];
						this.strListS.Add(text4 + "\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000/" + files[n]);
						this.countS++;
					}
				}
			};
			List<string> arg = new List<string>();
			action(text, arg);
			if (this.countS != num)
			{
				this.poseArray = null;
			}
			if (this.poseArray == null)
			{
				this.strList2 = new List<string>();
				this.strListE = new List<string>();
				this.strListE2 = new List<string>();
				this.strListS = new List<string>();
				this.strListD = new List<string>();
				this.strS = "";
				List<string> list = new List<string>();
				list.Add(string.Empty);
				list.AddRange(GameUty.PathList);
				List<string> bgList2 = new List<string>();
				HashSet<int> hashSet = new HashSet<int>();
				CsvCommonIdManager.ReadEnabledIdList(0, true, "phot_bg_enabled_list", ref hashSet);
				Action<string> action2 = delegate (string strFileName)
				{
					if (GameUty.FileSystem.IsExistentFile(strFileName))
					{
						using (AFileBase afileBase2 = GameUty.FileSystem.FileOpen(strFileName))
						{
							using (CsvParser csvParser2 = new CsvParser())
							{
								csvParser2.Open(afileBase2);
								for (int n = 1; n < csvParser2.max_cell_y; n++)
								{
									if (csvParser2.IsCellToExistData(0, n) && hashSet.Contains(csvParser2.GetCellAsInteger(0, n)))
									{
										bgList2.Add(csvParser2.GetCellAsString(3, n));
										string text4 = csvParser2.GetCellAsString(2, n);
										if (csvParser2.GetCellAsString(1, n) == "夜")
										{
											text4 += "(夜)";
										}
										this.bgNameList.Add(csvParser2.GetCellAsString(3, n) + "," + text4);
									}
								}
							}
						}
					}
				};
				action2("phot_bg_list.nei");
				for (int j = 0; j < list.Count; j++)
				{
					action2("edit_bg_" + list[j] + ".nei");
				}
				List<string> list2 = new List<string>();
				for (int i = 0; i < bgList2.Count; i++)
				{
					bool flag = false;
					for (int k = 0; k < this.bgArray21.Length; k++)
					{
						if (bgList2[i] == this.bgArray21[k])
						{
							flag = true;
						}
					}
					if (!flag)
					{
						if (bgList2[i] != "HoneymoonRoom" && bgList2[i] != "ClassRoom_Play" && bgList2[i] != "BigSight" && bgList2[i] != "PrivateRoom" && bgList2[i] != "Sea_Night" && bgList2[i] != "Yashiki")
						{
							list2.Add(bgList2[i]);
						}
					}
				}
				UnityEngine.Object x = GameMain.Instance.BgMgr.CreateAssetBundle("SMRoom2");
				if (x != null)
				{
					this.isVP = true;
				}
				x = GameMain.Instance.BgMgr.CreateAssetBundle("Train");
				if (x != null)
				{
					this.isPP = true;
				}
				x = GameMain.Instance.BgMgr.CreateAssetBundle("HoneymoonRoom");
				if (x != null)
				{
					this.isPP2 = true;
				}
				x = GameMain.Instance.BgMgr.CreateAssetBundle("BigSight");
				if (x != null)
				{
					this.isPP3 = true;
				}
				x = GameMain.Instance.BgMgr.CreateAssetBundle("rotenburo");
				if (x != null)
				{
					this.isVA = true;
				}
				x = GameMain.Instance.BgMgr.CreateAssetBundle("karaokeroom");
				if (x != null)
				{
					this.isKA = true;
				}
				if (GameUty.IsEnabledCompatibilityMode)
				{
					this.isCM3D2 = true;
				}
				List<string> list3 = new List<string>(350 + this.poseArray2.Length);
				list3.AddRange(this.poseArray2);
				List<string> list4 = new List<string>();
				for (int l = 11; l < 200; l++)
				{
					if (l < 100)
					{
						using (AFileBase afileBase = GameUty.FileSystem.FileOpen("edit_pose_0" + l + "_f.anm"))
						{
							if (afileBase.IsValid())
							{
								list4.Add("edit_pose_0" + l + "_f");
							}
						}
					}
					else
					{
						using (AFileBase afileBase = GameUty.FileSystem.FileOpen("edit_pose_" + l + "_f.anm"))
						{
							if (afileBase.IsValid())
							{
								list4.Add("edit_pose_" + l + "_f");
							}
						}
					}
				}
				for (int l = 15; l < 25; l++)
				{
					for (int k = 0; k < 2; k++)
					{
						string text2 = "s";
						if (k == 1)
						{
							text2 = "w";
						}
						for (int i = 1; i < 20; i++)
						{
							if (i < 10)
							{
								using (AFileBase afileBase = GameUty.FileSystem.FileOpen(string.Concat(new object[]
								{
									"edit_pose_dg",
									l,
									text2,
									"_00",
									i,
									"_f.anm"
								})))
								{
									if (afileBase.IsValid())
									{
										list4.Add(string.Concat(new object[]
										{
											"edit_pose_dg",
											l,
											text2,
											"_00",
											i,
											"_f"
										}));
									}
								}
							}
							else
							{
								using (AFileBase afileBase = GameUty.FileSystem.FileOpen(string.Concat(new object[]
								{
									"edit_pose_dg",
									l,
									text2,
									"_0",
									i,
									"_f.anm"
								})))
								{
									if (afileBase.IsValid())
									{
										list4.Add(string.Concat(new object[]
										{
											"edit_pose_dg",
											l,
											text2,
											"_0",
											i,
											"_f"
										}));
									}
								}
							}
						}
					}
				}
				if (list4.Count > 0)
				{
					list3.AddRange(list4.ToArray());
				}
				using (GameUty.FileSystem.FileOpen("dance_cm3d_003_sp2_f1.anm"))
				{
				}
				using (GameUty.FileSystem.FileOpen("dance_cm3d2_kara_003_ddfl_f1.anm"))
				{
				}
				using (GameUty.FileSystem.FileOpen("dance_cm3d2_kara02_001_smt_f1.anm"))
				{
				}
				list3.AddRange(this.poseArrayVP2);
				list3.AddRange(this.poseArrayFB);
				list3.AddRange(this.poseArray4);
				list3.AddRange(this.poseArray5);
				list3.AddRange(this.poseArray6);
				this.poseArray = list3.ToArray();
				Action<string, List<string>> action3 = delegate (string path, List<string> result_list)
				{
					string[] files = Directory.GetFiles(path);
					this.countS = 0;
					for (int n = 0; n < files.Length; n++)
					{
						if (Path.GetExtension(files[n]) == ".anm")
						{
							string text4 = files[n].Split(new char[]
							{
								'\\'
							})[files[n].Split(new char[]
							{
								'\\'
							}).Length - 1];
							text4 = text4.Split(new char[]
							{
								'.'
							})[0];
							this.strListS.Add(text4 + "\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000/" + files[n]);
							this.countS++;
						}
					}
				};
				List<string> arg2 = new List<string>();
				action3(Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose", arg2);
				string[] list5 = GameUty.FileSystem.GetList("motion", AFileSystemBase.ListType.AllFile);
				int num2 = 0;
				List<string> list6 = new List<string>();
				string[] array = list5;
				int m = 0;
				while (m < array.Length)
				{
					string path2 = array[m];
					string text3 = Path.GetFileNameWithoutExtension(path2);
					string directoryName = Path.GetDirectoryName(path2);
					if (!text3.StartsWith("maid_motion_") && !text3.StartsWith("work_00") && !text3.EndsWith("_3_") && !text3.EndsWith("_5_") && !text3.StartsWith("ck_") && !text3.StartsWith("vr_") && !text3.StartsWith("dance_mc") && !text3.Contains("a01_") && !text3.StartsWith("j_") && !text3.StartsWith("k_") && !text3.StartsWith("t_") && !text3.StartsWith("cbl_") && !text3.Contains("b01_") && !text3.Contains("b02_") && !text3.Contains("_kubi_") && !text3.EndsWith("_m2") && !text3.EndsWith("_m3") && !text3.Contains("_m2_once") && !text3.Contains("_m3_once") && !text3.StartsWith("h_") && !text3.StartsWith("event_") && !text3.StartsWith("man_") && !text3.EndsWith("_m") && !text3.Contains("_m_") && !text3.Contains("_man"))
					{
						if (!(text3 == "dance_cm3d2_001_zoukin") && !(text3 == "dance_cm3d2_001_mop") && !(text3 == "maid_motion") && !(text3 == "aruki_1_idougo_f") && !(text3 == "sleep2") && !(text3 == "stand_akire2") && !(text3 == "ero_scene_001") && !(text3 == "ero_scenefm_001") && !(text3 == "training_001") && !(text3 == "workff_001") && !(text3 == "workfm_001") && !(text3 == "dance_cm3d21_005_moe_mset") && !(text3 == "hinpyoukai_001"))
						{
							if (!directoryName.Contains("\\sex\\"))
							{
								if (!text3.StartsWith("sex_"))
								{
									if (text3.StartsWith("dance_test"))
									{
										this.strListD.Add(text3);
									}
									else
									{
										bool flag = false;
										foreach (string text2 in this.strListS)
										{
											string b = text2.Split(new char[]
											{
												'/'
											})[0].Replace("\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000", "");
											if (text3 == b)
											{
												flag = true;
												break;
											}
										}
										if (!flag)
										{
											list6.Add(text3);
										}
									}
								}
							}
							else if (!text3.StartsWith("pose_"))
							{
								this.strListE.Add(text3);
							}
						}
					}
					//IL_217A:
					m++;
					continue;
					//goto IL_217A;
				}
				foreach (string text2 in list6)
				{
					bool flag2 = false;
					for (int l = 0; l < this.poseArray.Length; l++)
					{
						if (text2 == this.poseArray[l])
						{
							flag2 = true;
						}
					}
					if (!flag2 && text2.StartsWith("edit_"))
					{
						this.strList2.Add(text2);
					}
				}
				foreach (string text2 in list6)
				{
					bool flag2 = false;
					for (int l = 0; l < this.poseArray.Length; l++)
					{
						if (text2 == this.poseArray[l])
						{
							flag2 = true;
						}
					}
					if (!flag2 && text2.StartsWith("pose_"))
					{
						this.strList2.Add(text2);
					}
				}
				foreach (string text2 in list6)
				{
					bool flag2 = false;
					for (int l = 0; l < this.poseArray.Length; l++)
					{
						if (text2 == this.poseArray[l])
						{
							flag2 = true;
						}
					}
					if (!flag2 && !text2.StartsWith("edit_") && !text2.StartsWith("pose_"))
					{
						this.strList2.Add(text2);
					}
				}
				foreach (string text2 in this.strListE)
				{
					bool flag2 = false;
					for (int l = 0; l < this.poseArray.Length; l++)
					{
						if (text2 == this.poseArray[l])
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						this.strListE2.Add(text2);
						num2++;
					}
				}
				list3.AddRange(this.strList2.ToArray());
				list3.AddRange(this.strListE2.ToArray());
				this.existPose = false;
				this.poseIniStr = "";
				List<IniKey> keys = base.Preferences["pose"].Keys;
				foreach (IniKey iniKey in keys)
				{
					IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
					if (iniKey2.Value != null && iniKey2.Value.ToString() != "")
					{
						if (iniKey2.Value.ToString() != "del")
						{
							list3.AddRange(new string[]
							{
								iniKey.Key
							});
							this.existPose = true;
							if (this.poseIniStr == "")
							{
								this.poseIniStr = iniKey.Key;
							}
						}
					}
				}
				list3.AddRange(this.strListS.ToArray());
				this.poseArray = list3.ToArray();
				List<string> list7 = new List<string>(50 + this.poseGroupArray2.Length);
				list7.AddRange(this.poseGroupArray2);
				list7.AddRange(this.poseGroupArrayVP);
				list7.AddRange(this.poseGroupArrayFB);
				list7.AddRange(this.poseGroupArray3);
				list7.Add(this.poseArray5[0]);
				list7.Add(this.poseArray6[0]);
				list7.Add(this.strList2[0]);
				list7.Add(this.strListE2[0]);
				if (this.strListS.Count > 0 && this.poseIniStr == "")
				{
					list7.Add(this.strListS[0]);
					this.existPose = true;
				}
				if (this.poseIniStr != "")
				{
					list7.Add(this.poseIniStr);
				}
				this.poseGroupArray = list7.ToArray();
				this.groupList = new ArrayList();
				for (int i = 0; i < this.poseArray.Length; i++)
				{
					for (int k = 0; k < this.poseGroupArray.Length; k++)
					{
						if (this.poseGroupArray[k] == this.poseArray[i])
						{
							this.groupList.Add(i);
							if (this.poseGroupArray[k] == this.strList2[0])
							{
								this.sPoseCount = i;
							}
						}
					}
				}
				string[] collection = new string[]
				{
					"Salon_Day"
				};
				string[] collection2 = new string[]
				{
					"SMRoom2",
					"LockerRoom"
				};
				string[] collection3 = new string[]
				{
					"Train",
					"Toilet",
					"Oheya",
					"MyBedRoom_NightOff"
				};
				string[] collection4 = new string[]
				{
					"ClassRoom",
					"ClassRoom_Play",
					"HoneymoonRoom",
					"OutletPark"
				};
				string[] collection5 = new string[]
				{
					"BigSight",
					"BigSight_Night",
					"PrivateRoom",
					"PrivateRoom_Night",
					"Sea",
					"Sea_Night",
					"Yashiki_Day",
					"Yashiki",
					"Yashiki_Pillow"
				};
				string[] collection6 = new string[]
				{
					"rotenburo",
					"rotenburo_night",
					"villa",
					"villa_night",
					"villa_bedroom",
					"villa_bedroom_night",
					"villa_farm",
					"villa_farm_night"
				};
				string[] collection7 = new string[]
				{
					"karaokeroom"
				};
				List<string> list8 = new List<string>(50 + this.poseArray2.Length);
				list8.AddRange(this.bgArray2);
				List<string> list9 = new List<string>();
				Dictionary<string, string> saveDataDic = CreativeRoomManager.GetSaveDataDic();
				if (saveDataDic != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in saveDataDic)
					{
						list9.Add(keyValuePair.Key);
					}
				}
				list8.AddRange(list9.ToArray());
				if (this.isCM3D2)
				{
					list8.AddRange(this.bgArray21);
					if (this.isVP)
					{
						list8.AddRange(collection2);
					}
					if (this.isPP)
					{
						list8.AddRange(collection3);
					}
					if (this.isPP2)
					{
						list8.AddRange(collection4);
					}
					if (this.isPP3)
					{
						list8.AddRange(collection5);
					}
					if (this.isVA)
					{
						list8.AddRange(collection6);
					}
					if (this.isKA || this.isKA2)
					{
						list8.AddRange(collection7);
					}
					list8.AddRange(collection);
				}
				List<string> list10 = new List<string>();
				for (int i = 0; i < bgList2.Count; i++)
				{
					bool flag3 = false;
					for (int k = 0; k < list8.Count; k++)
					{
						if (list8[k] == bgList2[i])
						{
							flag3 = true;
						}
					}
					if (!flag3)
					{
						list10.Add(bgList2[i]);
					}
				}
				list8.AddRange(list10.ToArray());
				this.bgArray = list8.ToArray();
				string[] collection8 = new string[]
				{
					"dokidokifallinlove_short_inst",
					"dokidokifallinlove_short",
					"entrancetoyou_short"
				};
				string[] collection9 = new string[]
				{
					"scarlet leap_short"
				};
				string[] array2 = new string[]
				{
					"stellarmytears_short",
					"stellarmytears_short2",
					"stellarmytears_short3"
				};
				string[] collection10 = new string[]
				{
					"RhythmixToYou"
				};
				array2 = new string[]
				{
					"happy_happy_scandal1",
					"happy_happy_scandal2",
					"happy_happy_scandal3"
				};
				array2 = new string[]
				{
					"can_know_two_close"
				};
				array2 = new string[]
				{
					"sweetsweeteveryday_short1",
					"sweetsweeteveryday_short2",
					"sweetsweeteveryday_short3"
				};
				string[] collection11 = new string[]
				{
					"bloomingdreaming_short",
					"kiminiaijodelicious_short",
					"luminousmoment_short",
					"nightmagicfire_short",
					"melodyofempire_short"
				};
				List<string> list11 = new List<string>(50);
				list11.AddRange(this.bgmArray2);
				List<string> list12 = new List<string>();
				for (int l = 18; l < 210; l++)
				{
					if (l < 100)
					{
						using (AFileBase afileBase = GameUty.FileSystem.FileOpen("bgm0" + l + ".ogg"))
						{
							if (afileBase.IsValid())
							{
								list12.Add("bgm0" + l);
							}
						}
					}
					else
					{
						using (AFileBase afileBase = GameUty.FileSystem.FileOpen("bgm" + l + ".ogg"))
						{
							if (afileBase.IsValid())
							{
								list12.Add("bgm" + l);
							}
						}
					}
				}
				if (list12.Count > 0)
				{
					list11.AddRange(list12.ToArray());
				}
				list11.AddRange(collection8);
				list11.AddRange(collection9);
				list11.AddRange(collection10);
				list11.AddRange(collection11);
				this.bgmArray = list11.ToArray();
				array2 = new string[]
				{
					"OutletPark:54",
					"HoneymoonRoom:102"
				};
				List<string> list13 = new List<string>(200);
				using (AFileBase afileBase = GameUty.FileSystem.FileOpen("desk_item_detail.nei"))
				{
					using (CsvParser csvParser = new CsvParser())
					{
						bool flag4 = csvParser.Open(afileBase);
						NDebug.Assert(flag4, "desk_item_detail.nei\nopen failed.");
						for (int i = 1; i < csvParser.max_cell_y; i++)
						{
							if (csvParser.IsCellToExistData(0, i))
							{
								int cellAsInteger = csvParser.GetCellAsInteger(0, i);
								MultipleMaids.ItemData2 itemData = new MultipleMaids.ItemData2(csvParser, i);
								if (itemData.asset_name != "")
								{
									if (GameMain.Instance.BgMgr.CreateAssetBundle(itemData.asset_name) != null)
									{
										list13.AddRange(new string[]
										{
											itemData.name + "\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000#" + itemData.asset_name
										});
									}
								}
							}
						}
					}
				}
				this.parArray = list13.ToArray();
				List<string> list14 = new List<string>(50 + this.parArray2.Length);
				list14.AddRange(this.parArray2);
				list14.AddRange(this.parArray3);
				this.parArray1 = list14.ToArray();
				List<string> list15 = new List<string>(50 + this.doguB1Array.Length);
				list15.AddRange(this.doguB1Array);
				if (this.isCM3D2)
				{
					if (this.isVA)
					{
						list15.AddRange(this.doguB2Array);
					}
					if (this.isKA)
					{
						list15.AddRange(this.doguB3Array);
					}
					if (this.isKA2)
					{
						list15.AddRange(this.doguB4Array);
					}
				}
				this.doguB1Array = list15.ToArray();
				List<string>[] array3 = new List<string>[9];
				for (int l = 0; l < 9; l++)
				{
					array3[l] = new List<string>();
				}
				HashSet<int> hashSet2 = new HashSet<int>();
				CsvCommonIdManager.ReadEnabledIdList(0, true, "phot_bg_object_enabled_list", ref hashSet2);
				PhotoBGObjectData.Create();
				List<PhotoBGObjectData> data = PhotoBGObjectData.data;
				for (int l = 0; l < data.Count; l++)
				{
					string text3 = data[l].create_prefab_name;
					if (text3 == "")
					{
						text3 = data[l].create_asset_bundle_name;
					}
					string category = data[l].category;
					switch (category)
					{
						case "家具":
							array3[0].Add(text3);
							break;
						case "道具":
							array3[1].Add(text3);
							break;
						case "文房具":
							array3[2].Add(text3);
							break;
						case "グルメ":
							array3[3].Add(text3);
							break;
						case "ドリンク":
							array3[4].Add(text3);
							break;
						case "カジノアイテム":
							array3[5].Add(text3);
							break;
						case "プレイアイテム":
							array3[6].Add(text3);
							break;
						case "パーティクル":
							array3[7].Add(text3);
							break;
						case "その他":
							array3[8].Add(text3);
							break;
					}
					string name = data[l].name;
					this.doguNameList.Add(text3 + "," + name);
				}
				for (int l = 0; l < array3.Length; l++)
				{
					this.doguBArray.Add(array3[l].ToArray());
				}
				List<string> list16 = new List<string>(100 + this.itemBArray.Length);
				list16.AddRange(this.itemBArray);
				if (this.isCM3D2)
				{
					if (this.isVA)
					{
						list16.AddRange(this.itemB2Array);
					}
					if (this.isKA)
					{
						list16.AddRange(this.itemB3Array);
					}
					if (this.isKA2)
					{
						list16.AddRange(this.itemB4Array);
					}
				}
				this.itemBArray = list16.ToArray();
				List<string>[] array4 = new List<string>[9];
				for (int l = 0; l < 9; l++)
				{
					array4[l] = new List<string>();
				}
				List<int> categoryIDList = PlacementData.CategoryIDList;
				List<int> list17 = new List<int>(20);
				list17.Add(-1);
				list17.AddRange(categoryIDList.ToArray());
				this.myArray = list17.ToArray();
			}
		}
	}
}
