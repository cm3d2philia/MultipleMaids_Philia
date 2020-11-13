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
		public void Update()
		{
			if (this.isInit)
			{
				if (this.allowUpdate)
				{
					this.MaidUpdate();
					if (this.isFadeOut)
					{
						bool flag = false;
						for (int i2 = 0; i2 < this.maxMaidCnt; i2++)
						{
							if (this.maidArray[i2] && this.maidArray[i2].Visible && this.maidArray[i2].IsAllProcPropBusy)
							{
								flag = true;
							}
						}
						if (!flag)
						{
							if (!this.isBusyInit)
							{
								this.isBusyInit = true;
							}
							else
							{
								for (int i2 = 0; i2 < this.maxMaidCnt; i2++)
								{
									if (!this.isLock[i2])
									{
										if (this.maidArray[i2] != null)
										{
											this.maidArray[i2].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
											this.maidArray[i2].SetAutoTwistAll(true);
										}
									}
									this.poseCount[i2] = 30;
									if (this.maidArray[i2] && this.maidArray[i2].Visible)
									{
										this.maidArray[i2].body0.BoneHitHeightY = -10f;
										if (this.selectList.Count > i2)
										{
											if (this.goSlot[(int)this.selectList[i2]] == null)
											{
												this.maidArray[i2].CrossFade(this.poseArray[0] + ".anm", false, true, false, 0f, 1f);
												this.maidArray[i2].SetAutoTwistAll(true);
												this.goSlot[(int)this.selectList[i2]] = new List<TBodySkin>(this.maidArray[i2].body0.goSlot);
												if (!this.isVR)
												{
													try
													{
														this.shodaiFlg[(int)this.selectList[i2]] = false;
														TMorph morph = this.maidArray[i2].body0.Face.morph;
														float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
														float item = fieldValue[(int)morph.hash["tangopen"]];
													}
													catch
													{
														this.shodaiFlg[(int)this.selectList[i2]] = true;
													}
													if (!this.isVR)
													{
														this.eyeL[(int)this.selectList[i2]] = this.maidArray[i2].body0.quaDefEyeL.eulerAngles;
														this.eyeR[(int)this.selectList[i2]] = this.maidArray[i2].body0.quaDefEyeR.eulerAngles;
													}
												}
												if (this.isKamiyure)
												{
													for (int j = 0; j < this.maidArray[i2].body0.goSlot.Count; j++)
													{
														if (j >= 3 && j <= 6)
														{
															if (this.maidArray[i2].body0.goSlot[j].obj != null)
															{
																DynamicBone component = this.maidArray[i2].body0.goSlot[j].obj.GetComponent<DynamicBone>();
																if (component != null)
																{
																	component.m_Damping = this.kamiyure2;
																	component.m_Elasticity = this.kamiyure3;
																	if (j == 5)
																	{
																		component.m_Elasticity = this.kamiyure3 / 20f;
																	}
																	component.m_Radius = this.kamiyure4;
																	component.UpdateParameters();
																}
															}
														}
													}
												}
											}
										}
									}
								}
								this.isBusyInit = false;
								GameMain.Instance.MainCamera.FadeIn(1f, false, null, true, true, default(Color));
								this.isFadeOut = false;
								this.bGui = true;
							}
						}
					}
				}
				if (this.isVR && this.isVRScroll)
				{
					if (!this.getModKeyPressing(MultipleMaids.modKey.Ctrl) && !this.getModKeyPressing(MultipleMaids.modKey.Alt) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.X) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.R) && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Y) && !Input.GetKey(KeyCode.V) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
					{
						Vector3 vector = MultipleMaids.GetFieldValue<OvrCamera, Vector3>(GameMain.Instance.OvrMgr.OvrCamera, "v");
						string text = "TrackingSpace/CenterEyeAnchor";
						if (GameMain.Instance.VRFamily == GameMain.VRFamilyType.HTC)
						{
							text = "Main Camera (eye)";
						}
						GameObject childObject = UTY.GetChildObject(this.mainCamera.gameObject, text, false);
						Transform transform = childObject.transform;
						Vector3 a = transform.rotation * Vector3.forward;
						vector += a * (Input.GetAxis("Mouse ScrollWheel") * (5f * Time.deltaTime * 2f * 5f));
						MultipleMaids.SetFieldValue3<OvrCamera, Vector3>(GameMain.Instance.OvrMgr.OvrCamera, "v", vector);
						Transform fieldValue2 = MultipleMaids.GetFieldValue<OvrCamera, Transform>(GameMain.Instance.OvrMgr.OvrCamera, "m_trBaseHead");
						fieldValue2.position = vector;
						MultipleMaids.SetFieldValue4<OvrCamera, Transform>(GameMain.Instance.OvrMgr.OvrCamera, "m_trBaseHead", fieldValue2);
					}
				}
				if (this.isMekure1a || this.isMekure2a || this.isZurasia)
				{
					for (int i2 = 0; i2 < this.maidCnt; i2++)
					{
						if (this.maidArray[i2] && this.maidArray[i2].Visible)
						{
							if (this.isMekure1a)
							{
								if (this.isMekure1)
								{
									this.maidArray[i2].ItemChangeTemp("skirt", "めくれスカート");
									this.maidArray[i2].ItemChangeTemp("onepiece", "めくれスカート");
								}
								else
								{
									this.ResetProp(this.maidArray[i2], MPN.skirt);
									this.ResetProp(this.maidArray[i2], MPN.onepiece);
								}
								this.maidArray[i2].AllProcPropSeqStart();
							}
							if (this.isMekure2a)
							{
								if (this.isMekure2)
								{
									this.maidArray[i2].ItemChangeTemp("skirt", "めくれスカート後ろ");
									this.maidArray[i2].ItemChangeTemp("onepiece", "めくれスカート後ろ");
								}
								else
								{
									this.ResetProp(this.maidArray[i2], MPN.skirt);
									this.ResetProp(this.maidArray[i2], MPN.onepiece);
								}
								this.maidArray[i2].AllProcPropSeqStart();
							}
							if (this.isZurasia)
							{
								if (this.isZurasi)
								{
									this.maidArray[i2].ItemChangeTemp("panz", "パンツずらし");
									this.maidArray[i2].ItemChangeTemp("mizugi", "パンツずらし");
								}
								else
								{
									this.ResetProp(this.maidArray[i2], MPN.panz);
									this.ResetProp(this.maidArray[i2], MPN.mizugi);
								}
								this.maidArray[i2].AllProcPropSeqStart();
							}
						}
					}
					this.isMekure1a = false;
					this.isMekure2a = false;
					this.isZurasia = false;
				}
				if (this.isKamiyure)
				{
					int num = this.maidCnt;
					if (num == 0)
					{
						num = 3;
					}
					for (int i2 = 0; i2 < num; i2++)
					{
						if (this.maidArray[i2] && this.maidArray[i2].Visible)
						{
							Maid maid = this.maidArray[i2];
							for (int j = 0; j < maid.body0.goSlot.Count; j++)
							{
								if (j >= 3 && j <= 6)
								{
									if (maid.body0.goSlot[j].obj != null)
									{
										DynamicBone component = maid.body0.goSlot[j].obj.GetComponent<DynamicBone>();
										if (component != null)
										{
											if (component.m_Damping != this.kamiyure2 || component.m_Elasticity != this.kamiyure3 || component.m_Radius != this.kamiyure4)
											{
												if (j == 5)
												{
													if (component.m_Damping == this.kamiyure2 && component.m_Elasticity == this.kamiyure3 / 20f && component.m_Radius == this.kamiyure4)
													{
														goto IL_A35;
													}
												}
												component.m_Damping = this.kamiyure2;
												component.m_Elasticity = this.kamiyure3;
												if (j == 5)
												{
													component.m_Elasticity = this.kamiyure3 / 20f;
												}
												component.m_Radius = this.kamiyure4;
												component.UpdateParameters();
											}
										}
									}
								}
							IL_A35:;
							}
						}
					}
				}
				if (this.isSkirtyure)
				{
					int num = this.maidCnt;
					if (num == 0)
					{
						num = 3;
					}
					for (int i2 = 0; i2 < num; i2++)
					{
						if (this.maidArray[i2] && this.maidArray[i2].Visible)
						{
							int j = 0;
							while (j < this.maidArray[i2].body0.goSlot.Count)
							{
								if (this.maidArray[i2].body0.goSlot[j].obj != null)
								{
									DynamicSkirtBone fieldValue3 = MultipleMaids.GetFieldValue<BoneHair3, DynamicSkirtBone>(this.maidArray[i2].body0.goSlot[j].bonehair3, "m_SkirtBone");
									if (fieldValue3 != null)
									{
										fieldValue3.m_vGravity = new Vector3(0.5f, 0.5f, 0.5f);
										if (fieldValue3.m_fPanierForce != this.skirtyure3 || fieldValue3.m_fPanierForceDistanceThreshold != this.skirtyure2 || fieldValue3.m_fRegDefaultRadius != this.skirtyure4)
										{
											fieldValue3.m_fPanierForce = this.skirtyure3;
											fieldValue3.m_fPanierForceDistanceThreshold = this.skirtyure2;
											fieldValue3.m_fRegDefaultRadius = this.skirtyure4;
										}
									}
								}
								//IL_BC8:
								j++;
								continue;
								//goto IL_BC8;
							}
						}
					}
				}
				if ((!Input.GetKey(KeyCode.KeypadEnter) && Input.GetKeyDown(KeyCode.Keypad1)) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.KeypadDivide) || Input.GetKeyDown(KeyCode.KeypadMultiply) || Input.GetKeyDown(KeyCode.KeypadMinus))
				{
					if (Input.GetKey(KeyCode.H) || this.isVR)
					{
						if (Input.GetKeyDown(KeyCode.Keypad1))
						{
							this.isWear = !this.isWear;
						}
						if (Input.GetKeyDown(KeyCode.Keypad2))
						{
							this.isSkirt = !this.isSkirt;
						}
						if (Input.GetKeyDown(KeyCode.Keypad3))
						{
							this.isBra = !this.isBra;
						}
						if (Input.GetKeyDown(KeyCode.Keypad4))
						{
							this.isPanz = !this.isPanz;
						}
						if (Input.GetKeyDown(KeyCode.Keypad5))
						{
							this.isHeadset = !this.isHeadset;
						}
						if (Input.GetKeyDown(KeyCode.Keypad6))
						{
							this.isAccUde = !this.isAccUde;
						}
						if (Input.GetKeyDown(KeyCode.Keypad7))
						{
							this.isGlove = !this.isGlove;
						}
						if (Input.GetKeyDown(KeyCode.Keypad8))
						{
							this.isStkg = !this.isStkg;
						}
						if (Input.GetKeyDown(KeyCode.Keypad9))
						{
							this.isShoes = !this.isShoes;
						}
						if (Input.GetKeyDown(KeyCode.Keypad0))
						{
							this.isAccSenaka = !this.isAccSenaka;
						}
						if (Input.GetKeyDown(KeyCode.KeypadDivide))
						{
							this.isMekure1 = !this.isMekure1;
						}
						if (Input.GetKeyDown(KeyCode.KeypadMultiply))
						{
							this.isMekure2 = !this.isMekure2;
						}
						if (Input.GetKeyDown(KeyCode.KeypadMinus))
						{
							this.isZurasi = !this.isZurasi;
						}
						this.h2Flg = true;
						if (this.sceneLevel != 3 && this.sceneLevel != 5)
						{
							this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
							this.maidArray[1] = GameMain.Instance.CharacterMgr.GetMaid(1);
							this.maidArray[2] = GameMain.Instance.CharacterMgr.GetMaid(2);
							this.maidArray[3] = GameMain.Instance.CharacterMgr.GetMaid(3);
							this.maidCnt = 4;
						}
						for (int i2 = 0; i2 < this.maidCnt; i2++)
						{
							if (this.maidArray[i2] && this.maidArray[i2].Visible)
							{
								if (Input.GetKeyDown(KeyCode.KeypadDivide) || Input.GetKeyDown(KeyCode.KeypadMultiply) || Input.GetKeyDown(KeyCode.KeypadMinus))
								{
									if (this.sceneLevel == 3 || this.sceneLevel == 5)
									{
										if (Input.GetKeyDown(KeyCode.KeypadDivide))
										{
											this.isMekure1a = true;
										}
										if (Input.GetKeyDown(KeyCode.KeypadMultiply))
										{
											this.isMekure2a = true;
										}
										if (Input.GetKeyDown(KeyCode.KeypadMinus))
										{
											this.isZurasia = true;
										}
									}
									else
									{
										if (Input.GetKeyDown(KeyCode.KeypadDivide))
										{
											if (this.isMekure1)
											{
												this.maidArray[i2].ItemChangeTemp("skirt", "めくれスカート");
												this.maidArray[i2].ItemChangeTemp("onepiece", "めくれスカート");
											}
											else
											{
												this.ResetProp(this.maidArray[i2], MPN.skirt);
												this.ResetProp(this.maidArray[i2], MPN.onepiece);
											}
											this.maidArray[i2].AllProcPropSeqStart();
										}
										if (Input.GetKeyDown(KeyCode.KeypadMultiply))
										{
											if (this.isMekure2)
											{
												this.maidArray[i2].ItemChangeTemp("skirt", "めくれスカート後ろ");
												this.maidArray[i2].ItemChangeTemp("onepiece", "めくれスカート後ろ");
											}
											else
											{
												this.ResetProp(this.maidArray[i2], MPN.skirt);
												this.ResetProp(this.maidArray[i2], MPN.onepiece);
											}
											this.maidArray[i2].AllProcPropSeqStart();
										}
										if (Input.GetKeyDown(KeyCode.KeypadMinus))
										{
											if (this.isZurasi)
											{
												this.maidArray[i2].ItemChangeTemp("panz", "パンツずらし");
												this.maidArray[i2].ItemChangeTemp("mizugi", "パンツずらし");
											}
											else
											{
												this.ResetProp(this.maidArray[i2], MPN.panz);
												this.ResetProp(this.maidArray[i2], MPN.mizugi);
											}
											this.maidArray[i2].AllProcPropSeqStart();
										}
									}
								}
								else
								{
									Hashtable fieldValue4 = MultipleMaids.GetFieldValue<TBody, Hashtable>(this.maidArray[i2].body0, "m_hFoceHide");
									if (Input.GetKeyDown(KeyCode.Keypad1))
									{
										fieldValue4[7] = this.isWear;
										fieldValue4[9] = this.isWear;
										fieldValue4[10] = this.isWear;
									}
									if (Input.GetKeyDown(KeyCode.Keypad2))
									{
										fieldValue4[8] = this.isSkirt;
									}
									if (Input.GetKeyDown(KeyCode.Keypad3))
									{
										fieldValue4[12] = this.isBra;
									}
									if (Input.GetKeyDown(KeyCode.Keypad4))
									{
										fieldValue4[11] = this.isPanz;
									}
									if (Input.GetKeyDown(KeyCode.Keypad5))
									{
										fieldValue4[15] = this.isHeadset;
										fieldValue4[40] = this.isHeadset;
									}
									if (Input.GetKeyDown(KeyCode.Keypad6))
									{
										fieldValue4[29] = this.isAccUde;
									}
									if (Input.GetKeyDown(KeyCode.Keypad7))
									{
										fieldValue4[16] = this.isGlove;
									}
									if (Input.GetKeyDown(KeyCode.Keypad8))
									{
										fieldValue4[13] = this.isStkg;
									}
									if (Input.GetKeyDown(KeyCode.Keypad9))
									{
										fieldValue4[14] = this.isShoes;
									}
									if (Input.GetKeyDown(KeyCode.Keypad0))
									{
										fieldValue4[31] = this.isAccSenaka;
									}
									MultipleMaids.SetFieldValue6<TBody, Hashtable>(this.maidArray[i2].body0, "m_hFoceHide", fieldValue4);
									this.maidArray[i2].body0.FixMaskFlag();
									this.maidArray[i2].body0.FixVisibleFlag(false);
								}
							}
						}
					}
				}
				if (!this.yotogiFlg && (this.sceneLevel == 14 || this.sceneLevel == 24))
				{
					Maid maid = this.maidArray[0];
					Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
					Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
					Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
					if (Input.GetKeyDown(KeyCode.LeftBracket) || (Input.GetKeyDown(KeyCode.BackQuote) && this.getModKeyPressing(MultipleMaids.modKey.Alt)))
					{
						for (int k = 0; k < 10; k++)
						{
							maid.transform.RotateAround(maid.transform.position, Vector3.right, -maid.transform.rotation.eulerAngles.x);
							maid.transform.RotateAround(maid.transform.position, Vector3.forward, -maid.transform.rotation.eulerAngles.z);
						}
					}
					else if ((Input.GetKey(KeyCode.Minus) && this.getModKeyPressing(MultipleMaids.modKey.Shift)) || (Input.GetKey(KeyCode.I) && this.getModKeyPressing(MultipleMaids.modKey.Alt)))
					{
						maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), 0.4f);
					}
					else if ((Input.GetKey(KeyCode.Quote) && this.getModKeyPressing(MultipleMaids.modKey.Shift)) || (Input.GetKey(KeyCode.K) && this.getModKeyPressing(MultipleMaids.modKey.Alt)))
					{
						maid.transform.RotateAround(maid.transform.position, new Vector3(vector3.x, 0f, vector3.z), -0.4f);
					}
					else if (Input.GetKey(KeyCode.Minus) || (Input.GetKey(KeyCode.J) && this.getModKeyPressing(MultipleMaids.modKey.Alt)))
					{
						maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), 0.4f);
					}
					else if (Input.GetKey(KeyCode.Quote) || (Input.GetKey(KeyCode.L) && this.getModKeyPressing(MultipleMaids.modKey.Alt)))
					{
						maid.transform.RotateAround(maid.transform.position, new Vector3(vector2.x, 0f, vector2.z), -0.4f);
					}
					if (Input.GetKeyUp(KeyCode.H) && !this.hFlg)
					{
						if (this.h2Flg)
						{
							this.h2Flg = false;
						}
						else
						{
							if (this.isVR)
							{
								this.isF6 = true;
							}
							if (!this.maidArray[0])
							{
								this.maidArray[0] = GameMain.Instance.CharacterMgr.GetMaid(0);
							}
							string value = "";
							if (this.wearIndex == 0)
							{
								value = "Underwear";
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
								value = "Nude";
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
								value = "None";
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
							TBody.MaskMode maskMode = (TBody.MaskMode)Enum.Parse(typeof(TBody.MaskMode), value);
							for (int i2 = 0; i2 < this.maidCnt; i2++)
							{
								if (this.maidArray[i2] && this.maidArray[i2].Visible)
								{
									this.maidArray[i2].body0.SetMaskMode(maskMode);
								}
							}
						}
					}
				}
				if (this.isVR)
				{
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha1))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10000;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha2))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10001;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha3))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10002;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha4))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10003;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha5))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10004;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha6))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10005;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha7))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10006;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha8))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10007;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha9))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10008;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
					if (Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.Alpha0))
					{
						string text2 = GameMain.Instance.BgMgr.GetBGName();
						int l = 0;
						foreach (string text3 in this.bgArray)
						{
							if (text3 == text2)
							{
								this.bgIndex = l;
								this.bgIndex6 = l;
								break;
							}
							l++;
						}
						this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
						this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
						this.lightX6 = this.lightX[0];
						this.lightY6 = this.lightY[0];
						this.loadScene = 10009;
						this.kankyoLoadFlg = true;
						GameMain.Instance.SoundMgr.PlaySe("se002.ogg", false);
					}
				}
				if (this.isSavePose4)
				{
					this.isSavePose4 = false;
					Maid maid = this.maidArray[this.selectMaidIndex];
					Vector3 localEulerAngles = maid.transform.localEulerAngles;
					maid.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					Vector3 position = maid.transform.position;
					maid.transform.position = new Vector3(0f, 0f, 0f);
					Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
					transform2.eulerAngles = this.bipRotation;
					transform2.position = this.bipPosition;
					maid.transform.localEulerAngles = localEulerAngles;
					maid.transform.position = position;
					CacheBoneDataArray cacheBoneDataArray = maid.gameObject.AddComponent<CacheBoneDataArray>();
					cacheBoneDataArray.CreateCache(maid.body0.GetBone("Bip01"));
					byte[] anmBinary = cacheBoneDataArray.GetAnmBinary(true, true);
					string path4 = Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose\\" + this.inName3 + ".anm";
					for (int l = 0; l < 100; l++)
					{
						if (!File.Exists(path4))
						{
							break;
						}
						this.inName3 += "_";
						path4 = Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose\\" + this.inName3 + ".anm";
					}
					File.WriteAllBytes(path4, anmBinary);
					this.strList2 = new List<string>();
					this.strListE = new List<string>();
					this.strListE2 = new List<string>();
					this.strListS = new List<string>();
					this.strListD = new List<string>();
					this.strS = "";
					List<string> list = new List<string>(350 + this.poseArray2.Length);
					list.AddRange(this.poseArray2);
					List<string> list2 = new List<string>();
					for (int l = 11; l < 200; l++)
					{
						if (l < 100)
						{
							using (AFileBase afileBase = GameUty.FileSystem.FileOpen("edit_pose_0" + l + "_f.anm"))
							{
								if (afileBase.IsValid())
								{
									list2.Add("edit_pose_0" + l + "_f");
								}
							}
						}
						else
						{
							using (AFileBase afileBase = GameUty.FileSystem.FileOpen("edit_pose_" + l + "_f.anm"))
							{
								if (afileBase.IsValid())
								{
									list2.Add("edit_pose_" + l + "_f");
								}
							}
						}
					}
					for (int l = 15; l < 25; l++)
					{
						for (int i2 = 0; i2 < 2; i2++)
						{
							string text = "s";
							if (i2 == 1)
							{
								text = "w";
							}
							for (int k = 1; k < 20; k++)
							{
								if (k < 10)
								{
									using (AFileBase afileBase = GameUty.FileSystem.FileOpen(string.Concat(new object[]
									{
										"edit_pose_dg",
										l,
										text,
										"_00",
										k,
										"_f.anm"
									})))
									{
										if (afileBase.IsValid())
										{
											list2.Add(string.Concat(new object[]
											{
												"edit_pose_dg",
												l,
												text,
												"_00",
												k,
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
										text,
										"_0",
										k,
										"_f.anm"
									})))
									{
										if (afileBase.IsValid())
										{
											list2.Add(string.Concat(new object[]
											{
												"edit_pose_dg",
												l,
												text,
												"_0",
												k,
												"_f"
											}));
										}
									}
								}
							}
						}
					}
					if (list2.Count > 0)
					{
						list.AddRange(list2.ToArray());
					}
					list.AddRange(this.poseArrayVP2);
					list.AddRange(this.poseArrayFB);
					list.AddRange(this.poseArray4);
					list.AddRange(this.poseArray5);
					list.AddRange(this.poseArray6);
					this.poseArray = list.ToArray();
					Action<string, List<string>> action = delegate (string path, List<string> result_list)
					{
						string[] files = Directory.GetFiles(path);
						this.countS = 0;
						for (int num208 = 0; num208 < files.Length; num208++)
						{
							if (Path.GetExtension(files[num208]) == ".anm")
							{
								string text21 = files[num208].Split(new char[]
								{
									'\\'
								})[files[num208].Split(new char[]
								{
									'\\'
								}).Length - 1];
								text21 = text21.Split(new char[]
								{
									'.'
								})[0];
								this.strListS.Add(text21 + "\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000/" + files[num208]);
								this.countS++;
							}
						}
					};
					List<string> arg = new List<string>();
					action(Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose", arg);
					string[] list3 = GameUty.FileSystem.GetList("motion", AFileSystemBase.ListType.AllFile);
					int num2 = 0;
					List<string> list4 = new List<string>();
					string[] array = list3;
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
											bool flag2 = false;
											foreach (string text in this.strListS)
											{
												string text4 = text.Split(new char[]
												{
													'/'
												})[0].Replace("\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000", "");
												if (text3 == text4)
												{
													flag2 = true;
													break;
												}
											}
											if (!flag2)
											{
												list4.Add(text3);
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
						//IL_2E50:
						m++;
						continue;
						//goto IL_2E50;
					}
					foreach (string text in list4)
					{
						bool flag3 = false;
						for (int l = 0; l < this.poseArray.Length; l++)
						{
							if (text == this.poseArray[l])
							{
								flag3 = true;
							}
						}
						if (!flag3 && text.StartsWith("edit_"))
						{
							this.strList2.Add(text);
						}
					}
					foreach (string text in list4)
					{
						bool flag3 = false;
						for (int l = 0; l < this.poseArray.Length; l++)
						{
							if (text == this.poseArray[l])
							{
								flag3 = true;
							}
						}
						if (!flag3 && text.StartsWith("pose_"))
						{
							this.strList2.Add(text);
						}
					}
					foreach (string text in list4)
					{
						bool flag3 = false;
						for (int l = 0; l < this.poseArray.Length; l++)
						{
							if (text == this.poseArray[l])
							{
								flag3 = true;
							}
						}
						if (!flag3 && !text.StartsWith("edit_") && !text.StartsWith("pose_"))
						{
							this.strList2.Add(text);
						}
					}
					foreach (string text in this.strListE)
					{
						bool flag3 = false;
						for (int l = 0; l < this.poseArray.Length; l++)
						{
							if (text == this.poseArray[l])
							{
								flag3 = true;
							}
						}
						if (!flag3)
						{
							this.strListE2.Add(text);
							num2++;
						}
					}
					list.AddRange(this.strList2.ToArray());
					list.AddRange(this.strListE2.ToArray());
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
								list.AddRange(new string[]
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
					list.AddRange(this.strListS.ToArray());
					this.poseArray = list.ToArray();
					List<string> list5 = new List<string>(50 + this.poseGroupArray2.Length);
					list5.AddRange(this.poseGroupArray2);
					list5.AddRange(this.poseGroupArrayVP);
					list5.AddRange(this.poseGroupArrayFB);
					list5.AddRange(this.poseGroupArray3);
					list5.Add(this.poseArray5[0]);
					list5.Add(this.poseArray6[0]);
					list5.Add(this.strList2[0]);
					list5.Add(this.strListE2[0]);
					this.existPose = true;
					if (this.strListS.Count > 0 && this.poseIniStr == "")
					{
						list5.Add(this.strListS[0]);
					}
					if (this.poseIniStr != "")
					{
						list5.Add(this.poseIniStr);
					}
					this.poseGroupArray = list5.ToArray();
					this.groupList = new ArrayList();
					for (int k = 0; k < this.poseArray.Length; k++)
					{
						for (int i2 = 0; i2 < this.poseGroupArray.Length; i2++)
						{
							if (this.poseGroupArray[i2] == this.poseArray[k])
							{
								this.groupList.Add(k);
								if (this.poseGroupArray[i2] == this.strList2[0])
								{
									this.sPoseCount = k;
								}
							}
						}
					}
					this.poseGroupComboList = new GUIContent[this.poseGroupArray.Length + 1];
					this.poseGroupComboList[0] = new GUIContent("1:通常");
					for (int n = 0; n < this.poseGroupArray.Length; n++)
					{
						if (this.poseGroupArray[n] == "maid_dressroom01")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":立ち");
						}
						if (this.poseGroupArray[n] == "tennis_kamae_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":中腰");
						}
						if (this.poseGroupArray[n] == "senakanagasi_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":膝をつく");
						}
						if (this.poseGroupArray[n] == "work_hansei")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":座り");
						}
						if (this.poseGroupArray[n] == "inu_taiki_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":四つん這い");
						}
						if (this.poseGroupArray[n] == "syagami_pose_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":床座り");
						}
						if (this.poseGroupArray[n] == "densyasuwari_taiki_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":椅子座り");
						}
						if (this.poseGroupArray[n] == "work_kaiwa")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":ソファー座り");
						}
						if (this.poseGroupArray[n] == "dance_cm3d2_001_f1,14.14")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":ドキドキ☆Fallin' Love");
						}
						if (this.poseGroupArray[n] == "dance_cm3d_001_f1,39.25")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":entrance to you");
						}
						if (this.poseGroupArray[n] == "dance_cm3d_002_end_f1,50.71")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":scarlet leap");
						}
						if (this.poseGroupArray[n] == "dance_cm3d2_002_smt_f,7.76,")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":stellar my tears");
						}
						if (this.poseGroupArray[n] == "dance_cm3d_003_sp2_f1,90.15")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":rhythmix to you");
						}
						if (this.poseGroupArray[n] == "dance_cm3d2_003_hs_f1,0.01,")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":happy!happy!スキャンダル!!");
						}
						if (this.poseGroupArray[n] == "dance_cm3d_004_kano_f1,124.93")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":Can Know Two Close");
						}
						if (this.poseGroupArray[n] == "dance_cm3d2_004_sse_f1,0.01")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":sweet sweet everyday");
						}
						if (this.poseGroupArray[n] == "turusi_sex_in_taiki_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":拘束");
						}
						if (this.poseGroupArray[n] == "rosyutu_pose01_f")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":エロ");
						}
						if (this.poseGroupArray[n] == "rosyutu_aruki_f_once_,1.37")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":歩き");
						}
						if (this.poseGroupArray[n] == "stand_desk1")
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":その他");
						}
						if (this.poseGroupArray[n] == this.poseArray5[0])
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":ダンスMC");
						}
						if (this.poseGroupArray[n] == this.poseArray6[0])
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":ダンス");
						}
						if (n == this.poseGroupArray.Length - 3)
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":通常2");
						}
						if (n == this.poseGroupArray.Length - 2)
						{
							this.poseGroupComboList[n + 1] = new GUIContent(n + 2 + ":エロ2");
						}
						if (n == this.poseGroupArray.Length - 1)
						{
							this.poseGroupComboList[n + 1] = new GUIContent("99:登録ポーズ");
						}
					}
					int num3 = -1;
					for (int k = 0; k < this.groupList.Count; k++)
					{
						if (this.poseIndex[this.selectMaidIndex] < (int)this.groupList[k])
						{
							num3 = k;
							break;
						}
					}
					int num4 = (int)this.groupList[0];
					int num5 = 0;
					if (num3 > 0)
					{
						num4 = (int)this.groupList[num3] - (int)this.groupList[num3 - 1];
						num5 = (int)this.groupList[num3 - 1];
					}
					if (num3 < 0)
					{
						num3 = this.groupList.Count;
						num4 = this.poseArray.Length - (int)this.groupList[num3 - 1];
						num5 = (int)this.groupList[num3 - 1];
					}
					this.poseComboList = new GUIContent[num4];
					int num6 = 0;
					for (int j = num5; j < num5 + num4; j++)
					{
						bool flag4 = false;
						List<IniKey> keys2 = base.Preferences["pose"].Keys;
						foreach (IniKey iniKey in keys2)
						{
							if (this.poseArray[j] == iniKey.Key)
							{
								IniKey iniKey2 = base.Preferences["pose"][iniKey.Key];
								if (iniKey2.Value != null && iniKey2.Value.ToString() != "" && iniKey2.Value.ToString() != "del")
								{
									this.poseComboList[num6] = new GUIContent(string.Concat(new object[]
									{
										num6 + 1,
										":",
										iniKey2.Value.Split(new char[]
										{
											'_'
										})[0],
										"\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000",
										iniKey.Key
									}));
									flag4 = true;
								}
							}
						}
						if (!flag4)
						{
							this.poseComboList[num6] = new GUIContent(num6 + 1 + ":" + this.poseArray[j]);
						}
						num6++;
					}
					this.poseGroupCombo.selectedItemIndex = num3;
					this.poseGroupIndex = num3;
					this.poseCombo.selectedItemIndex = 0;
					for (int l = (int)this.groupList[this.groupList.Count - 1]; l < this.poseArray.Length; l++)
					{
						string text4 = this.poseArray[l].Split(new char[]
						{
							'/'
						})[0].Replace("\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000\u3000", "");
						if (text4 == this.inName3)
						{
							this.poseIndex[this.selectMaidIndex] = l;
							string path3 = this.poseArray[l].Split(new char[]
							{
								'/'
							})[1];
							byte[] array210 = new byte[0];
							try
							{
								using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
								{
									array210 = new byte[fileStream.Length];
									fileStream.Read(array210, 0, array210.Length);
								}
							}
							catch
							{
							}
							if (0 < array210.Length)
							{
								string fileName = Path.GetFileName(path3);
								long num7 = (long)fileName.GetHashCode();
								maid.body0.CrossFade(num7.ToString(), array210, false, false, false, 0f, 1f);
								Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
								{
									default(Maid.AutoTwist),
									Maid.AutoTwist.ShoulderR,
									Maid.AutoTwist.WristL,
									Maid.AutoTwist.WristR,
									Maid.AutoTwist.ThighL,
									Maid.AutoTwist.ThighR
								};
								for (int k = 0; k < array3.Length; k++)
								{
									maid.SetAutoTwist(array3[k], true);
								}
							}
							break;
						}
					}
					this.isLock[this.selectMaidIndex] = false;
					this.inName3 = "";
					this.isSavePose = false;
				}
				if (this.isSavePose3)
				{
					Maid maid = this.maidArray[this.selectMaidIndex];
					this.isSavePose3 = false;
					this.isSavePose4 = true;
					Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
					transform2.eulerAngles = this.bipRotation;
					transform2.position = this.bipPosition;
				}
				if (this.isSavePose2)
				{
					Maid maid = this.maidArray[this.selectMaidIndex];
					this.isSavePose2 = false;
					this.isSavePose3 = true;
					Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
					transform2.eulerAngles = this.bipRotation;
					transform2.position = this.bipPosition;
				}
				if (this.isSavePose)
				{
					Maid maid = this.maidArray[this.selectMaidIndex];
					Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
					this.bipPosition = new Vector3(transform2.position.x - maid.transform.position.x, transform2.position.y, transform2.position.z - maid.transform.position.z);
					this.bipRotation = transform2.eulerAngles;
					maid.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					maid.transform.position = new Vector3(maid.transform.position.x, 0f, maid.transform.position.z);
					this.isSavePose = false;
					this.isSavePose2 = true;
					this.isStop[this.selectMaidIndex] = true;
				}
				if (this.saveScene > 0)
				{
					string text = "";
					string text5 = "";
					DateTime now = DateTime.Now;
					int year = now.Year;
					int month = now.Month;
					int day = now.Day;
					int hour = now.Hour;
					int minute = now.Minute;
					string text6 = string.Concat(new object[]
					{
						year,
						"/",
						month.ToString("00"),
						"/",
						day.ToString("00"),
						" ",
						hour.ToString("00"),
						":",
						minute.ToString("00")
					});
					if (this.saveScene < 9999)
					{
						this.date[this.saveScene - 1 - this.page * 10] = text6;
						this.ninzu[this.saveScene - 1 - this.page * 10] = this.maidCnt + "人";
					}
					text5 = text5 + text6 + ",";
					text5 = text5 + this.maidCnt + ",";
					text5 = text5 + this.bgArray[this.bgIndex].Replace("_", " ") + ",";
					string text7 = text5;
					string[] array4 = new string[7];
					array4[0] = text7;
					string[] array5 = array4;
					int num8 = 1;
					float num9 = this.bg.localEulerAngles.x;
					array5[num8] = num9.ToString("0.###");
					array4[2] = ",";
					string[] array6 = array4;
					int num10 = 3;
					num9 = this.bg.localEulerAngles.y;
					array6[num10] = num9.ToString("0.###");
					array4[4] = ",";
					string[] array7 = array4;
					int num11 = 5;
					num9 = this.bg.localEulerAngles.z;
					array7[num11] = num9.ToString("0.###");
					array4[6] = ",";
					text5 = string.Concat(array4);
					text7 = text5;
					array4 = new string[7];
					array4[0] = text7;
					string[] array8 = array4;
					int num12 = 1;
					num9 = this.bg.position.x;
					array8[num12] = num9.ToString("0.###");
					array4[2] = ",";
					string[] array9 = array4;
					int num13 = 3;
					num9 = this.bg.position.y;
					array9[num13] = num9.ToString("0.###");
					array4[4] = ",";
					string[] array10 = array4;
					int num14 = 5;
					num9 = this.bg.position.z;
					array10[num14] = num9.ToString("0.###");
					array4[6] = ",";
					text5 = string.Concat(array4);
					text7 = text5;
					array4 = new string[7];
					array4[0] = text7;
					string[] array11 = array4;
					int num15 = 1;
					num9 = this.bg.localScale.x;
					array11[num15] = num9.ToString("0.###");
					array4[2] = ",";
					string[] array12 = array4;
					int num16 = 3;
					num9 = this.bg.localScale.y;
					array12[num16] = num9.ToString("0.###");
					array4[4] = ",";
					string[] array13 = array4;
					int num17 = 5;
					num9 = this.bg.localScale.z;
					array13[num17] = num9.ToString("0.###");
					array4[6] = ",";
					text5 = string.Concat(array4);
					text7 = text5;
					text5 = string.Concat(new string[]
					{
						text7,
						this.softG.x.ToString("0.###"),
						",",
						this.softG.y.ToString("0.###"),
						",",
						this.softG.z.ToString("0.###"),
						","
					});
					text5 = text5 + this.bgmIndex + ",";
					text5 = text5 + this.effectIndex + ",";
					text5 = text5 + this.lightIndex[0] + ",";
					text5 = text5 + this.lightColorR[0] + ",";
					text5 = text5 + this.lightColorG[0] + ",";
					text5 = text5 + this.lightColorB[0] + ",";
					text7 = text5;
					array4 = new string[7];
					array4[0] = text7;
					string[] array14 = array4;
					int num18 = 1;
					num9 = GameMain.Instance.MainLight.transform.eulerAngles.x;
					array14[num18] = num9.ToString("0.###");
					array4[2] = ",";
					string[] array15 = array4;
					int num19 = 3;
					num9 = GameMain.Instance.MainLight.transform.eulerAngles.y;
					array15[num19] = num9.ToString("0.###");
					array4[4] = ",";
					string[] array16 = array4;
					int num20 = 5;
					num9 = GameMain.Instance.MainLight.transform.eulerAngles.z;
					array16[num20] = num9.ToString("0.###");
					array4[6] = ",";
					text5 = string.Concat(array4);
					text5 = text5 + GameMain.Instance.MainLight.GetComponent<Light>().intensity + ",";
					text5 = text5 + GameMain.Instance.MainLight.GetComponent<Light>().spotAngle + ",";
					text5 = text5 + GameMain.Instance.MainLight.GetComponent<Light>().range + ",";
					text5 = text5 + this.mainCamera.GetTargetPos().x + ",";
					text5 = text5 + this.mainCamera.GetTargetPos().y + ",";
					text5 = text5 + this.mainCamera.GetTargetPos().z + ",";
					text5 = text5 + this.mainCamera.GetDistance() + ",";
					text7 = text5;
					array4 = new string[7];
					array4[0] = text7;
					string[] array17 = array4;
					int num21 = 1;
					num9 = this.mainCamera.transform.eulerAngles.x;
					array17[num21] = num9.ToString("0.###");
					array4[2] = ",";
					string[] array18 = array4;
					int num22 = 3;
					num9 = this.mainCamera.transform.eulerAngles.y;
					array18[num22] = num9.ToString("0.###");
					array4[4] = ",";
					string[] array19 = array4;
					int num23 = 5;
					num9 = this.mainCamera.transform.eulerAngles.z;
					array19[num23] = num9.ToString("0.###");
					array4[6] = ",";
					text5 = string.Concat(array4);
					this.inName = this.inName.Replace("_", " ").Replace(",", " ");
					this.inText = this.inText.Replace("_", " ").Replace(",", " ");
					if (this.isMessage)
					{
						text7 = text5;
						text5 = string.Concat(new string[]
						{
							text7,
							"1,",
							this.inName,
							",",
							this.inText.Replace("\n", "&kaigyo")
						});
					}
					else
					{
						text5 += "0,,";
					}
					if (this.doguObject.Count > 0)
					{
						text5 = text5 + "," + this.doguArray[this.doguIndex[this.doguSelectIndex]].Replace("_", " ") + ",";
						text7 = text5;
						array4 = new string[7];
						array4[0] = text7;
						string[] array20 = array4;
						int num24 = 1;
						num9 = this.doguObject[this.doguSelectIndex].transform.localEulerAngles.x;
						array20[num24] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array21 = array4;
						int num25 = 3;
						num9 = this.doguObject[this.doguSelectIndex].transform.localEulerAngles.y;
						array21[num25] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array22 = array4;
						int num26 = 5;
						num9 = this.doguObject[this.doguSelectIndex].transform.localEulerAngles.z;
						array22[num26] = num9.ToString("0.###");
						array4[6] = ",";
						text5 = string.Concat(array4);
						text7 = text5;
						array4 = new string[7];
						array4[0] = text7;
						string[] array23 = array4;
						int num27 = 1;
						num9 = this.doguObject[this.doguSelectIndex].transform.position.x;
						array23[num27] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array24 = array4;
						int num28 = 3;
						num9 = this.doguObject[this.doguSelectIndex].transform.position.y;
						array24[num28] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array25 = array4;
						int num29 = 5;
						num9 = this.doguObject[this.doguSelectIndex].transform.position.z;
						array25[num29] = num9.ToString("0.###");
						array4[6] = ",";
						text5 = string.Concat(array4);
						text7 = text5;
						array4 = new string[6];
						array4[0] = text7;
						string[] array26 = array4;
						int num30 = 1;
						num9 = this.doguObject[this.doguSelectIndex].transform.localScale.x;
						array26[num30] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array27 = array4;
						int num31 = 3;
						num9 = this.doguObject[this.doguSelectIndex].transform.localScale.y;
						array27[num31] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array28 = array4;
						int num32 = 5;
						num9 = this.doguObject[this.doguSelectIndex].transform.localScale.z;
						array28[num32] = num9.ToString("0.###");
						text5 = string.Concat(array4);
					}
					text5 += "_";
					for (int l = 0; l < this.maidCnt; l++)
					{
						Maid maid = this.maidArray[l];
						string text8 = "";
						string text9 = "";
						this.SetIK(maid, l);
						for (int i2 = 0; i2 < 20; i2++)
						{
							text7 = text8;
							array4 = new string[7];
							array4[0] = text7;
							string[] array29 = array4;
							int num33 = 1;
							num9 = this.Finger[l, i2].localEulerAngles.x;
							array29[num33] = num9.ToString("0.###");
							array4[2] = ",";
							string[] array30 = array4;
							int num34 = 3;
							num9 = this.Finger[l, i2].localEulerAngles.y;
							array30[num34] = num9.ToString("0.###");
							array4[4] = ",";
							string[] array31 = array4;
							int num35 = 5;
							num9 = this.Finger[l, i2].localEulerAngles.z;
							array31[num35] = num9.ToString("0.###");
							array4[6] = ":";
							text8 = string.Concat(array4);
						}
						for (int i2 = 20; i2 < 40; i2++)
						{
							text7 = text9;
							array4 = new string[7];
							array4[0] = text7;
							string[] array32 = array4;
							int num36 = 1;
							num9 = this.Finger[l, i2].localEulerAngles.x;
							array32[num36] = num9.ToString("0.###");
							array4[2] = ",";
							string[] array33 = array4;
							int num37 = 3;
							num9 = this.Finger[l, i2].localEulerAngles.y;
							array33[num37] = num9.ToString("0.###");
							array4[4] = ",";
							string[] array34 = array4;
							int num38 = 5;
							num9 = this.Finger[l, i2].localEulerAngles.z;
							array34[num38] = num9.ToString("0.###");
							array4[6] = ":";
							text9 = string.Concat(array4);
						}
						string text10 = "";
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array35 = array4;
						int num39 = 1;
						num9 = this.Spine.eulerAngles.x;
						array35[num39] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array36 = array4;
						int num40 = 3;
						num9 = this.Spine.eulerAngles.y;
						array36[num40] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array37 = array4;
						int num41 = 5;
						num9 = this.Spine.eulerAngles.z;
						array37[num41] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array38 = array4;
						int num42 = 1;
						num9 = this.Spine0a.eulerAngles.x;
						array38[num42] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array39 = array4;
						int num43 = 3;
						num9 = this.Spine0a.eulerAngles.y;
						array39[num43] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array40 = array4;
						int num44 = 5;
						num9 = this.Spine0a.eulerAngles.z;
						array40[num44] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array41 = array4;
						int num45 = 1;
						num9 = this.Spine1.eulerAngles.x;
						array41[num45] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array42 = array4;
						int num46 = 3;
						num9 = this.Spine1.eulerAngles.y;
						array42[num46] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array43 = array4;
						int num47 = 5;
						num9 = this.Spine1.eulerAngles.z;
						array43[num47] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array44 = array4;
						int num48 = 1;
						num9 = this.Spine1a.eulerAngles.x;
						array44[num48] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array45 = array4;
						int num49 = 3;
						num9 = this.Spine1a.eulerAngles.y;
						array45[num49] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array46 = array4;
						int num50 = 5;
						num9 = this.Spine1a.eulerAngles.z;
						array46[num50] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array47 = array4;
						int num51 = 1;
						num9 = this.Pelvis.eulerAngles.x;
						array47[num51] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array48 = array4;
						int num52 = 3;
						num9 = this.Pelvis.eulerAngles.y;
						array48[num52] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array49 = array4;
						int num53 = 5;
						num9 = this.Pelvis.eulerAngles.z;
						array49[num53] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array50 = array4;
						int num54 = 1;
						num9 = this.HandL1[l].localEulerAngles.x;
						array50[num54] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array51 = array4;
						int num55 = 3;
						num9 = this.HandL1[l].localEulerAngles.y;
						array51[num55] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array52 = array4;
						int num56 = 5;
						num9 = this.HandL1[l].localEulerAngles.z;
						array52[num56] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array53 = array4;
						int num57 = 1;
						num9 = this.UpperArmL1[l].eulerAngles.x;
						array53[num57] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array54 = array4;
						int num58 = 3;
						num9 = this.UpperArmL1[l].eulerAngles.y;
						array54[num58] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array55 = array4;
						int num59 = 5;
						num9 = this.UpperArmL1[l].eulerAngles.z;
						array55[num59] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array56 = array4;
						int num60 = 1;
						num9 = this.ForearmL1[l].eulerAngles.x;
						array56[num60] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array57 = array4;
						int num61 = 3;
						num9 = this.ForearmL1[l].eulerAngles.y;
						array57[num61] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array58 = array4;
						int num62 = 5;
						num9 = this.ForearmL1[l].eulerAngles.z;
						array58[num62] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array59 = array4;
						int num63 = 1;
						num9 = this.HandR1[l].localEulerAngles.x;
						array59[num63] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array60 = array4;
						int num64 = 3;
						num9 = this.HandR1[l].localEulerAngles.y;
						array60[num64] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array61 = array4;
						int num65 = 5;
						num9 = this.HandR1[l].localEulerAngles.z;
						array61[num65] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array62 = array4;
						int num66 = 1;
						num9 = this.UpperArmR1[l].eulerAngles.x;
						array62[num66] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array63 = array4;
						int num67 = 3;
						num9 = this.UpperArmR1[l].eulerAngles.y;
						array63[num67] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array64 = array4;
						int num68 = 5;
						num9 = this.UpperArmR1[l].eulerAngles.z;
						array64[num68] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array65 = array4;
						int num69 = 1;
						num9 = this.ForearmR1[l].eulerAngles.x;
						array65[num69] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array66 = array4;
						int num70 = 3;
						num9 = this.ForearmR1[l].eulerAngles.y;
						array66[num70] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array67 = array4;
						int num71 = 5;
						num9 = this.ForearmR1[l].eulerAngles.z;
						array67[num71] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array68 = array4;
						int num72 = 1;
						num9 = this.HandL2[l].localEulerAngles.x;
						array68[num72] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array69 = array4;
						int num73 = 3;
						num9 = this.HandL2[l].localEulerAngles.y;
						array69[num73] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array70 = array4;
						int num74 = 5;
						num9 = this.HandL2[l].localEulerAngles.z;
						array70[num74] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array71 = array4;
						int num75 = 1;
						num9 = this.UpperArmL2[l].eulerAngles.x;
						array71[num75] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array72 = array4;
						int num76 = 3;
						num9 = this.UpperArmL2[l].eulerAngles.y;
						array72[num76] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array73 = array4;
						int num77 = 5;
						num9 = this.UpperArmL2[l].eulerAngles.z;
						array73[num77] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array74 = array4;
						int num78 = 1;
						num9 = this.ForearmL2[l].eulerAngles.x;
						array74[num78] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array75 = array4;
						int num79 = 3;
						num9 = this.ForearmL2[l].eulerAngles.y;
						array75[num79] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array76 = array4;
						int num80 = 5;
						num9 = this.ForearmL2[l].eulerAngles.z;
						array76[num80] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array77 = array4;
						int num81 = 1;
						num9 = this.HandR2[l].localEulerAngles.x;
						array77[num81] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array78 = array4;
						int num82 = 3;
						num9 = this.HandR2[l].localEulerAngles.y;
						array78[num82] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array79 = array4;
						int num83 = 5;
						num9 = this.HandR2[l].localEulerAngles.z;
						array79[num83] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array80 = array4;
						int num84 = 1;
						num9 = this.UpperArmR2[l].eulerAngles.x;
						array80[num84] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array81 = array4;
						int num85 = 3;
						num9 = this.UpperArmR2[l].eulerAngles.y;
						array81[num85] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array82 = array4;
						int num86 = 5;
						num9 = this.UpperArmR2[l].eulerAngles.z;
						array82[num86] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array83 = array4;
						int num87 = 1;
						num9 = this.ForearmR2[l].eulerAngles.x;
						array83[num87] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array84 = array4;
						int num88 = 3;
						num9 = this.ForearmR2[l].eulerAngles.y;
						array84[num88] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array85 = array4;
						int num89 = 5;
						num9 = this.ForearmR2[l].eulerAngles.z;
						array85[num89] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array86 = array4;
						int num90 = 1;
						num9 = this.Head.eulerAngles.x;
						array86[num90] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array87 = array4;
						int num91 = 3;
						num9 = this.Head.eulerAngles.y;
						array87[num91] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array88 = array4;
						int num92 = 5;
						num9 = this.Head.eulerAngles.z;
						array88[num92] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array89 = array4;
						int num93 = 1;
						num9 = maid.transform.localEulerAngles.x;
						array89[num93] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array90 = array4;
						int num94 = 3;
						num9 = maid.transform.localEulerAngles.y;
						array90[num94] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array91 = array4;
						int num95 = 5;
						num9 = maid.transform.localEulerAngles.z;
						array91[num95] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array92 = array4;
						int num96 = 1;
						num9 = maid.transform.position.x;
						array92[num96] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array93 = array4;
						int num97 = 3;
						num9 = maid.transform.position.y;
						array93[num97] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array94 = array4;
						int num98 = 5;
						num9 = maid.transform.position.z;
						array94[num98] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						text7 = text10;
						array4 = new string[7];
						array4[0] = text7;
						string[] array95 = array4;
						int num99 = 1;
						num9 = maid.transform.localScale.x;
						array95[num99] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array96 = array4;
						int num100 = 3;
						num9 = maid.transform.localScale.y;
						array96[num100] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array97 = array4;
						int num101 = 5;
						num9 = maid.transform.localScale.z;
						array97[num101] = num9.ToString("0.###");
						array4[6] = ":";
						text10 = string.Concat(array4);
						if (this.poseArray[this.poseIndex[l]].Contains("MultipleMaidsPose"))
						{
							string text4 = this.poseArray[this.poseIndex[l]].Replace("\u3000", "").Split(new char[]
							{
								'/'
							})[0];
							text10 = text10 + "MultipleMaidsPose" + text4.Replace("_", " ").Replace(",", "|") + ":";
						}
						else
						{
							text10 = text10 + this.poseArray[this.poseIndex[l]].Replace("_", " ").Replace(",", "|") + ":";
						}
						text10 = text10 + this.faceIndex[l] + ":";
						TMorph morph = maid.body0.Face.morph;
						float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
						float[] fieldValue5 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
						if (morph.bodyskin.PartsVersion < 120)
						{
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose"]] + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose2"]] + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose3"]] + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose6"]] + ",";
						}
						else
						{
							int num102 = 0;
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
							{
								num102 = 1;
							}
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
							{
								num102 = 2;
							}
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num102]]] + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num102]]] + ",";
							text10 = text10 + fieldValue[(int)morph.hash["eyeclose3"]] / 3f + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num102]]] + ",";
						}
						text10 = text10 + fieldValue[(int)morph.hash["hitomih"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["hitomis"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mayuha"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mayuup"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mayuv"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mayuvhalf"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["moutha"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouths"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthdw"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthup"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["tangout"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["tangup"]] + ",";
						if (morph.bodyskin.PartsVersion < 120)
						{
							text10 = text10 + fieldValue[(int)morph.hash["eyebig"]] + ",";
						}
						else
						{
							text10 = text10 + fieldValue[(int)morph.hash["eyebig"]] / 3f + ",";
						}
						if (morph.bodyskin.PartsVersion < 120)
						{
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose5"]] + ",";
						}
						else
						{
							int num102 = 0;
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
							{
								num102 = 1;
							}
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
							{
								num102 = 2;
							}
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num102]]] + ",";
						}
						text10 = text10 + fieldValue[(int)morph.hash["mayuw"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthhe"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthc"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthi"]] + ",";
						text10 = text10 + fieldValue[(int)morph.hash["mouthuphalf"]] + ",";
						try
						{
							text10 = text10 + fieldValue[(int)morph.hash["tangopen"]] + ",";
						}
						catch
						{
							text10 += "0,";
						}
						if (fieldValue[(int)morph.hash["namida"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["tear1"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["tear2"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["tear3"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["shock"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["yodare"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["hoho"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["hoho2"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["hohos"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["hohol"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["toothoff"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (fieldValue[(int)morph.hash["nosefook"]] > 0f)
						{
							text10 = text10 + 1 + ",";
						}
						else
						{
							text10 = text10 + 0 + ",";
						}
						if (morph.bodyskin.PartsVersion < 120)
						{
							if (morph.hash["eyeclose7"] != null)
							{
								text10 = text10 + fieldValue5[(int)morph.hash["eyeclose7"]] + ",";
								text10 = text10 + fieldValue5[(int)morph.hash["eyeclose8"]] + ":";
							}
							else
							{
								text10 += "0,0:";
							}
						}
						else
						{
							int num102 = 0;
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
							{
								num102 = 1;
							}
							if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
							{
								num102 = 2;
							}
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]] + ",";
							text10 = text10 + fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]] + ":";
						}
						string text11 = "";
						if (this.isLook[l])
						{
							text11 = text11 + 1 + ",";
							text11 = text11 + this.lookX[l].ToString("0.###") + ",";
							text11 = text11 + this.lookY[l].ToString("0.###") + ":";
						}
						else
						{
							text11 = text11 + 0 + ",0,0:";
						}
						text11 = text11 + this.itemIndex[l] + ":";
						Transform transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
						Transform transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array98 = array4;
						int num103 = 1;
						num9 = transform3.localEulerAngles.x;
						array98[num103] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array99 = array4;
						int num104 = 3;
						num9 = transform3.localEulerAngles.y;
						array99[num104] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array100 = array4;
						int num105 = 5;
						num9 = transform3.localEulerAngles.z;
						array100[num105] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array101 = array4;
						int num106 = 1;
						num9 = transform4.localEulerAngles.x;
						array101[num106] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array102 = array4;
						int num107 = 3;
						num9 = transform4.localEulerAngles.y;
						array102[num107] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array103 = array4;
						int num108 = 5;
						num9 = transform4.localEulerAngles.z;
						array103[num108] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array104 = array4;
						int num109 = 1;
						num9 = this.ClavicleL1[l].eulerAngles.x;
						array104[num109] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array105 = array4;
						int num110 = 3;
						num9 = this.ClavicleL1[l].eulerAngles.y;
						array105[num110] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array106 = array4;
						int num111 = 5;
						num9 = this.ClavicleL1[l].eulerAngles.z;
						array106[num111] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array107 = array4;
						int num112 = 1;
						num9 = this.ClavicleR1[l].eulerAngles.x;
						array107[num112] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array108 = array4;
						int num113 = 3;
						num9 = this.ClavicleR1[l].eulerAngles.y;
						array108[num113] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array109 = array4;
						int num114 = 5;
						num9 = this.ClavicleR1[l].eulerAngles.z;
						array109[num114] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						if (this.hanten[l])
						{
							text11 += "1:";
						}
						else
						{
							text11 += "0:";
						}
						Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
						Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
						Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
						Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
						Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
						Transform transform9 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
						Transform transform10 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
						Transform transform11 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
						Transform transform12 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
						Transform transform13 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
						Transform transform14 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
						Transform transform15 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
						Transform transform16 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
						Transform transform17 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
						Transform transform18 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
						Transform transform19 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
						Transform transform20 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
						Transform transform21 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
						Transform transform22 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array110 = array4;
						int num115 = 1;
						num9 = transform2.eulerAngles.x;
						array110[num115] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array111 = array4;
						int num116 = 3;
						num9 = transform2.eulerAngles.y;
						array111[num116] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array112 = array4;
						int num117 = 5;
						num9 = transform2.eulerAngles.z;
						array112[num117] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array113 = array4;
						int num118 = 1;
						num9 = transform5.localEulerAngles.x;
						array113[num118] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array114 = array4;
						int num119 = 3;
						num9 = transform5.localEulerAngles.y;
						array114[num119] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array115 = array4;
						int num120 = 5;
						num9 = transform5.localEulerAngles.z;
						array115[num120] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array116 = array4;
						int num121 = 1;
						num9 = transform6.localEulerAngles.x;
						array116[num121] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array117 = array4;
						int num122 = 3;
						num9 = transform6.localEulerAngles.y;
						array117[num122] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array118 = array4;
						int num123 = 5;
						num9 = transform6.localEulerAngles.z;
						array118[num123] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array119 = array4;
						int num124 = 1;
						num9 = transform7.localEulerAngles.x;
						array119[num124] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array120 = array4;
						int num125 = 3;
						num9 = transform7.localEulerAngles.y;
						array120[num125] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array121 = array4;
						int num126 = 5;
						num9 = transform7.localEulerAngles.z;
						array121[num126] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array122 = array4;
						int num127 = 1;
						num9 = transform8.localEulerAngles.x;
						array122[num127] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array123 = array4;
						int num128 = 3;
						num9 = transform8.localEulerAngles.y;
						array123[num128] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array124 = array4;
						int num129 = 5;
						num9 = transform8.localEulerAngles.z;
						array124[num129] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array125 = array4;
						int num130 = 1;
						num9 = transform9.localEulerAngles.x;
						array125[num130] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array126 = array4;
						int num131 = 3;
						num9 = transform9.localEulerAngles.y;
						array126[num131] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array127 = array4;
						int num132 = 5;
						num9 = transform9.localEulerAngles.z;
						array127[num132] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array128 = array4;
						int num133 = 1;
						num9 = transform10.localEulerAngles.x;
						array128[num133] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array129 = array4;
						int num134 = 3;
						num9 = transform10.localEulerAngles.y;
						array129[num134] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array130 = array4;
						int num135 = 5;
						num9 = transform10.localEulerAngles.z;
						array130[num135] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array131 = array4;
						int num136 = 1;
						num9 = transform11.localEulerAngles.x;
						array131[num136] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array132 = array4;
						int num137 = 3;
						num9 = transform11.localEulerAngles.y;
						array132[num137] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array133 = array4;
						int num138 = 5;
						num9 = transform11.localEulerAngles.z;
						array133[num138] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array134 = array4;
						int num139 = 1;
						num9 = transform12.localEulerAngles.x;
						array134[num139] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array135 = array4;
						int num140 = 3;
						num9 = transform12.localEulerAngles.y;
						array135[num140] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array136 = array4;
						int num141 = 5;
						num9 = transform12.localEulerAngles.z;
						array136[num141] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array137 = array4;
						int num142 = 1;
						num9 = transform13.localEulerAngles.x;
						array137[num142] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array138 = array4;
						int num143 = 3;
						num9 = transform13.localEulerAngles.y;
						array138[num143] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array139 = array4;
						int num144 = 5;
						num9 = transform13.localEulerAngles.z;
						array139[num144] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array140 = array4;
						int num145 = 1;
						num9 = transform14.localEulerAngles.x;
						array140[num145] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array141 = array4;
						int num146 = 3;
						num9 = transform14.localEulerAngles.y;
						array141[num146] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array142 = array4;
						int num147 = 5;
						num9 = transform14.localEulerAngles.z;
						array142[num147] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array143 = array4;
						int num148 = 1;
						num9 = transform15.localEulerAngles.x;
						array143[num148] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array144 = array4;
						int num149 = 3;
						num9 = transform15.localEulerAngles.y;
						array144[num149] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array145 = array4;
						int num150 = 5;
						num9 = transform15.localEulerAngles.z;
						array145[num150] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array146 = array4;
						int num151 = 1;
						num9 = transform16.localEulerAngles.x;
						array146[num151] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array147 = array4;
						int num152 = 3;
						num9 = transform16.localEulerAngles.y;
						array147[num152] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array148 = array4;
						int num153 = 5;
						num9 = transform16.localEulerAngles.z;
						array148[num153] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array149 = array4;
						int num154 = 1;
						num9 = transform17.localEulerAngles.x;
						array149[num154] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array150 = array4;
						int num155 = 3;
						num9 = transform17.localEulerAngles.y;
						array150[num155] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array151 = array4;
						int num156 = 5;
						num9 = transform17.localEulerAngles.z;
						array151[num156] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array152 = array4;
						int num157 = 1;
						num9 = transform18.localEulerAngles.x;
						array152[num157] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array153 = array4;
						int num158 = 3;
						num9 = transform18.localEulerAngles.y;
						array153[num158] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array154 = array4;
						int num159 = 5;
						num9 = transform18.localEulerAngles.z;
						array154[num159] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array155 = array4;
						int num160 = 1;
						num9 = transform19.localEulerAngles.x;
						array155[num160] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array156 = array4;
						int num161 = 3;
						num9 = transform19.localEulerAngles.y;
						array156[num161] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array157 = array4;
						int num162 = 5;
						num9 = transform19.localEulerAngles.z;
						array157[num162] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array158 = array4;
						int num163 = 1;
						num9 = transform20.localEulerAngles.x;
						array158[num163] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array159 = array4;
						int num164 = 3;
						num9 = transform20.localEulerAngles.y;
						array159[num164] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array160 = array4;
						int num165 = 5;
						num9 = transform20.localEulerAngles.z;
						array160[num165] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array161 = array4;
						int num166 = 1;
						num9 = transform21.localEulerAngles.x;
						array161[num166] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array162 = array4;
						int num167 = 3;
						num9 = transform21.localEulerAngles.y;
						array162[num167] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array163 = array4;
						int num168 = 5;
						num9 = transform21.localEulerAngles.z;
						array163[num168] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array164 = array4;
						int num169 = 1;
						num9 = transform22.localEulerAngles.x;
						array164[num169] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array165 = array4;
						int num170 = 3;
						num9 = transform22.localEulerAngles.y;
						array165[num170] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array166 = array4;
						int num171 = 5;
						num9 = transform22.localEulerAngles.z;
						array166[num171] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array167 = array4;
						int num172 = 1;
						num9 = maid.body0.quaDefEyeL.eulerAngles.x;
						array167[num172] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array168 = array4;
						int num173 = 3;
						num9 = maid.body0.quaDefEyeL.eulerAngles.y;
						array168[num173] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array169 = array4;
						int num174 = 5;
						num9 = maid.body0.quaDefEyeL.eulerAngles.z;
						array169[num174] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array170 = array4;
						int num175 = 1;
						num9 = maid.body0.quaDefEyeR.eulerAngles.x;
						array170[num175] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array171 = array4;
						int num176 = 3;
						num9 = maid.body0.quaDefEyeR.eulerAngles.y;
						array171[num176] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array172 = array4;
						int num177 = 5;
						num9 = maid.body0.quaDefEyeR.eulerAngles.z;
						array172[num177] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						Transform transform23 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_L", true);
						Transform transform24 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_L_sub", true);
						Transform transform25 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_R", true);
						Transform transform26 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_R_sub", true);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array173 = array4;
						int num178 = 1;
						num9 = transform23.localEulerAngles.x;
						array173[num178] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array174 = array4;
						int num179 = 3;
						num9 = transform23.localEulerAngles.y;
						array174[num179] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array175 = array4;
						int num180 = 5;
						num9 = transform23.localEulerAngles.z;
						array175[num180] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array176 = array4;
						int num181 = 1;
						num9 = transform24.localEulerAngles.x;
						array176[num181] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array177 = array4;
						int num182 = 3;
						num9 = transform24.localEulerAngles.y;
						array177[num182] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array178 = array4;
						int num183 = 5;
						num9 = transform24.localEulerAngles.z;
						array178[num183] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array179 = array4;
						int num184 = 1;
						num9 = transform25.localEulerAngles.x;
						array179[num184] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array180 = array4;
						int num185 = 3;
						num9 = transform25.localEulerAngles.y;
						array180[num185] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array181 = array4;
						int num186 = 5;
						num9 = transform25.localEulerAngles.z;
						array181[num186] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[7];
						array4[0] = text7;
						string[] array182 = array4;
						int num187 = 1;
						num9 = transform26.localEulerAngles.x;
						array182[num187] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array183 = array4;
						int num188 = 3;
						num9 = transform26.localEulerAngles.y;
						array183[num188] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array184 = array4;
						int num189 = 5;
						num9 = transform26.localEulerAngles.z;
						array184[num189] = num9.ToString("0.###");
						array4[6] = ":";
						text11 = string.Concat(array4);
						text7 = text11;
						array4 = new string[6];
						array4[0] = text7;
						string[] array185 = array4;
						int num190 = 1;
						num9 = transform2.position.x;
						array185[num190] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array186 = array4;
						int num191 = 3;
						num9 = transform2.position.y;
						array186[num191] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array187 = array4;
						int num192 = 5;
						num9 = transform2.position.z;
						array187[num192] = num9.ToString("0.###");
						text11 = string.Concat(array4);
						text7 = text;
						text = string.Concat(new string[]
						{
							text7,
							text8,
							text9,
							text10,
							text11
						});
						if (l + 1 != this.maidCnt)
						{
							text += ";";
						}
					}
					string text12 = "_";
					text12 = text12 + this.lightKage[0] + ",";
					if (this.isBloom)
					{
						text12 += "1,";
						text12 = text12 + this.bloom1 + ",";
						text12 = text12 + this.bloom2 + ",";
						text12 = text12 + this.bloom3 + ",";
						text12 = text12 + this.bloom4 + ",";
						text12 = text12 + this.bloom5 + ",";
						if (this.isBloomA)
						{
							text12 += "1,";
						}
						else
						{
							text12 += "0,";
						}
					}
					else
					{
						text12 += "0,0,0,0,0,0,0,";
					}
					if (this.isBlur)
					{
						text12 += "1,";
						text12 = text12 + this.blur1 + ",";
						text12 = text12 + this.blur2 + ",";
						text12 = text12 + this.blur3 + ",";
						text12 = text12 + this.blur4 + ",";
					}
					else
					{
						text12 += "0,0,0,0,0,";
					}
					text12 = text12 + this.bokashi + ",";
					text12 = text12 + this.kamiyure + ",";
					if (this.isDepth)
					{
						text12 += "1,";
						text12 = text12 + this.depth1 + ",";
						text12 = text12 + this.depth2 + ",";
						text12 = text12 + this.depth3 + ",";
						text12 = text12 + this.depth4 + ",";
						if (this.isDepthA)
						{
							text12 += "1,";
						}
						else
						{
							text12 += "0,";
						}
					}
					else
					{
						text12 += "0,0,0,0,0,0,";
					}
					if (this.isFog)
					{
						text12 += "1,";
						text12 = text12 + this.fog1 + ",";
						text12 = text12 + this.fog2 + ",";
						text12 = text12 + this.fog3 + ",";
						text12 = text12 + this.fog4 + ",";
						text12 = text12 + this.fog5 + ",";
						text12 = text12 + this.fog6 + ",";
						text12 = text12 + this.fog7 + ",";
					}
					else
					{
						text12 += "0,0,0,0,0,0,0,0,";
					}
					if (this.isSepia)
					{
						text12 += "1";
					}
					else
					{
						text12 += "0";
					}
					string text13 = "_";
					for (int l = 1; l < this.lightList.Count; l++)
					{
						text13 = text13 + this.lightIndex[l] + ",";
						text13 = text13 + this.lightColorR[l] + ",";
						text13 = text13 + this.lightColorG[l] + ",";
						text13 = text13 + this.lightColorB[l] + ",";
						text13 = text13 + this.lightX[l] + ",";
						text13 = text13 + this.lightY[l] + ",";
						text13 = text13 + this.lightAkarusa[l] + ",";
						text13 = text13 + this.lightRange[l] + ";";
					}
					string text14 = "_";
					for (int l = 0; l < this.doguBObject.Count; l++)
					{
						text14 = text14 + this.doguBObject[l].name.Replace("_", " ") + ",";
						text7 = text14;
						array4 = new string[7];
						array4[0] = text7;
						string[] array188 = array4;
						int num193 = 1;
						num9 = this.doguBObject[l].transform.localEulerAngles.x;
						array188[num193] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array189 = array4;
						int num194 = 3;
						num9 = this.doguBObject[l].transform.localEulerAngles.y;
						array189[num194] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array190 = array4;
						int num195 = 5;
						num9 = this.doguBObject[l].transform.localEulerAngles.z;
						array190[num195] = num9.ToString("0.###");
						array4[6] = ",";
						text14 = string.Concat(array4);
						text7 = text14;
						array4 = new string[7];
						array4[0] = text7;
						string[] array191 = array4;
						int num196 = 1;
						num9 = this.doguBObject[l].transform.position.x;
						array191[num196] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array192 = array4;
						int num197 = 3;
						num9 = this.doguBObject[l].transform.position.y;
						array192[num197] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array193 = array4;
						int num198 = 5;
						num9 = this.doguBObject[l].transform.position.z;
						array193[num198] = num9.ToString("0.###");
						array4[6] = ",";
						text14 = string.Concat(array4);
						text7 = text14;
						array4 = new string[6];
						array4[0] = text7;
						string[] array194 = array4;
						int num199 = 1;
						num9 = this.doguBObject[l].transform.localScale.x;
						array194[num199] = num9.ToString("0.###");
						array4[2] = ",";
						string[] array195 = array4;
						int num200 = 3;
						num9 = this.doguBObject[l].transform.localScale.y;
						array195[num200] = num9.ToString("0.###");
						array4[4] = ",";
						string[] array196 = array4;
						int num201 = 5;
						num9 = this.doguBObject[l].transform.localScale.z;
						array196[num201] = num9.ToString("0.###");
						text14 = string.Concat(array4);
						text14 += ";";
					}
					string text15 = "_";
					for (int l = 0; l < this.lightList.Count; l++)
					{
						text15 = text15 + this.lightList[l].transform.position.x + ",";
						text15 = text15 + this.lightList[l].transform.position.y + ",";
						text15 = text15 + this.lightList[l].transform.position.z + ";";
					}
					if (this.saveScene >= 10000)
					{
						base.Preferences["scene"]["s" + this.saveScene].Value = string.Concat(new string[]
						{
							text5,
							text,
							text12,
							text13,
							text14,
							text15
						});
						base.SaveConfig();
					}
					else
					{
						this.saveData = string.Concat(new string[]
						{
							text5,
							text,
							text12,
							text13,
							text14,
							text15
						});
					}
					this.saveScene = 0;
				}
				for (int i2 = 0; i2 < this.maidCnt; i2++)
				{
					if (this.haraCount[i2] > 0)
					{
						this.haraCount[i2]--;
						Transform transform2 = CMT.SearchObjName(this.maidArray[i2].body0.m_Bones.transform, "Bip01", true);
						transform2.position = this.haraPosition[i2];
					}
					if (this.isLoadFace[i2])
					{
						this.isLoadFace[i2] = false;
						TMorph morph = this.maidArray[i2].body0.Face.morph;
						float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
						fieldValue[(int)morph.hash["mouthuphalf"]] = fieldValue[(int)morph.hash["mouthuphalf"]] - 0.01f;
						this.maidArray[i2].body0.Face.morph.FixBlendValues_Face();
					}
				}
				if (this.isScene)
				{
					this.isScene = false;
					string path4 = string.Concat(new object[]
					{
						Path.GetFullPath(".\\"),
						"Mod\\MultipleMaidsScene\\",
						this.loadScene,
						".png"
					});
					string text16;
					if (File.Exists(path4))
					{
						FileStream fileStream = new FileStream(path4, FileMode.Open, FileAccess.Read);
						BinaryReader binaryReader = new BinaryReader(fileStream);
						byte[] array197 = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
						byte[] value2 = new byte[]
						{
							array197[36],
							array197[35],
							array197[34],
							array197[33]
						};
						int count = BitConverter.ToInt32(value2, 0) - 8;
						byte[] bytes = array197.Skip(49).Take(count).ToArray<byte>();
						text16 = Encoding.UTF8.GetString(bytes);
						text16 = MultipleMaids.StringFromBase64Comp(text16);
					}
					else
					{
						text16 = base.Preferences["scene"]["s" + this.loadScene].Value;
					}
					string[] array198 = text16.Split(new char[]
					{
						'_'
					});
					string[] array199 = array198[1].Split(new char[]
					{
						';'
					});
					string[] array200 = array198[0].Split(new char[]
					{
						','
					});
					string[] array201 = null;
					string[] array202 = null;
					string[] array203 = null;
					string[] array204 = null;
					if (array198.Length >= 5)
					{
						array201 = array198[2].Split(new char[]
						{
							','
						});
						array202 = array198[3].Split(new char[]
						{
							';'
						});
						array203 = array198[4].Split(new char[]
						{
							';'
						});
					}
					if (array198.Length >= 6)
					{
						array204 = array198[5].Split(new char[]
						{
							';'
						});
					}
					if (!int.TryParse(array200[2], out this.bgIndex))
					{
						string text2 = array200[2].Replace(" ", "_");
						for (int i2 = 0; i2 < this.bgArray.Length; i2++)
						{
							if (text2 == this.bgArray[i2])
							{
								this.bgIndex = i2;
								break;
							}
						}
					}
					if (this.bgArray[this.bgIndex].Length == 36)
					{
						GameMain.Instance.BgMgr.ChangeBgMyRoom(this.bgArray[this.bgIndex]);
					}
					else
					{
						GameMain.Instance.BgMgr.ChangeBg(this.bgArray[this.bgIndex]);
					}
					this.bgCombo.selectedItemIndex = this.bgIndex;
					this.bg.localEulerAngles = new Vector3(float.Parse(array200[3]), float.Parse(array200[4]), float.Parse(array200[5]));
					this.bg.position = new Vector3(float.Parse(array200[6]), float.Parse(array200[7]), float.Parse(array200[8]));
					this.bg.localScale = new Vector3(float.Parse(array200[9]), float.Parse(array200[10]), float.Parse(array200[11]));
					this.softG = new Vector3(float.Parse(array200[12]), float.Parse(array200[13]), float.Parse(array200[14]));
					for (int i2 = 0; i2 < this.maidCnt; i2++)
					{
						Maid maid = this.maidArray[i2];
						for (int j = 0; j < maid.body0.goSlot.Count; j++)
						{
							if (maid.body0.goSlot[j].obj != null)
							{
								DynamicBone component = maid.body0.goSlot[j].obj.GetComponent<DynamicBone>();
								if (component != null && component.enabled)
								{
									component.m_Gravity = new Vector3(this.softG.x * 5f, (this.softG.y + 0.003f) * 5f, this.softG.z * 5f);
								}
							}
							List<THair1> fieldValue6 = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid.body0.goSlot[j].bonehair, "hair1list");
							for (int k = 0; k < fieldValue6.Count; k++)
							{
								fieldValue6[k].SoftG = new Vector3(this.softG.x, this.softG.y + this.kamiyure, this.softG.z);
							}
						}
					}
					if (!this.kankyoLoadFlg)
					{
						int num202 = this.bgmIndex;
						this.bgmIndex = int.Parse(array200[15]);
						if (num202 != this.bgmIndex)
						{
							GameMain.Instance.SoundMgr.PlayBGM(this.bgmArray[this.bgmIndex] + ".ogg", 0f, true);
							this.bgmCombo.selectedItemIndex = this.bgmIndex;
						}
					}
					if (this.doguObject.Count > 0 && this.doguObject[this.doguSelectIndex] != null)
					{
						UnityEngine.Object.Destroy(this.doguObject[this.doguSelectIndex]);
						this.doguObject.RemoveAt(this.doguSelectIndex);
						this.doguIndex.RemoveAt(this.doguSelectIndex);
					}
					this.doguSelectIndex = 0;
					if (array200.Length > 16)
					{
						this.effectIndex = int.Parse(array200[16]);
						if (!this.isVR)
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
							if (this.kami)
							{
								this.kami.SetActive(false);
							}
							if (this.effectIndex == 0)
							{
								component2.enabled = false;
							}
							if (this.effectIndex == 1)
							{
								component2.mode = 0;
								component2.intensity = -1f;
								component2.chromaticAberration = 0f;
								component2.axialAberration = 0f;
								component2.blurSpread = 5f;
								component2.luminanceDependency = 0f;
								component2.blurDistance = 0f;
								component2.enabled = true;
							}
							if (this.effectIndex == 2)
							{
								component2.enabled = true;
							}
							if (this.effectIndex == 3)
							{
								component2.mode = 0;
								component2.intensity = 9f;
								component2.chromaticAberration = 0f;
								component2.axialAberration = 0f;
								component2.blurSpread = 5f;
								component2.luminanceDependency = 0f;
								component2.blurDistance = 0f;
								component2.enabled = true;
							}
							if (this.effectIndex == 4)
							{
								UnityEngine.Object @object = Resources.Load("Prefab/p_kamihubuki_photo_ver");
								this.kami = (GameObject)UnityEngine.Object.Instantiate(@object);
								Vector3 localPosition = this.kami.transform.localPosition;
								localPosition.y = 3.5f;
								this.kami.transform.localPosition = localPosition;
							}
							if (this.effectIndex == 5)
							{
								UnityEngine.Object @object = Resources.Load("Prefab/p_kamihubuki_photo_ver");
								this.kami = (GameObject)UnityEngine.Object.Instantiate(@object);
								Vector3 localPosition = this.kami.transform.localPosition;
								localPosition.y = 3.5f;
								this.kami.transform.localPosition = localPosition;
								component2.enabled = true;
								component2.mode = 0;
								component2.intensity = -2.5f;
								component2.chromaticAberration = -0.3f;
								component2.axialAberration = 0.5f;
								component2.blurSpread = 4.5f;
								component2.luminanceDependency = 0.2f;
								component2.blurDistance = 0.8f;
								component2.enabled = true;
							}
							if (this.effectIndex == 6)
							{
								this.kami.SetActive(false);
								UnityEngine.Object @object = Resources.Load("Prefab/p_powder_snow_photo_ver");
								this.kami = (GameObject)UnityEngine.Object.Instantiate(@object);
								Vector3 localPosition = this.kami.transform.localPosition;
								localPosition.y = 3.5f;
								this.kami.transform.localPosition = localPosition;
							}
							if (this.effectIndex == 7)
							{
								this.kami.SetActive(false);
								UnityEngine.Object @object = Resources.Load("Prefab/p_powder_snow_photo_ver");
								this.kami = (GameObject)UnityEngine.Object.Instantiate(@object);
								Vector3 localPosition = this.kami.transform.localPosition;
								localPosition.y = 3.5f;
								this.kami.transform.localPosition = localPosition;
								component2.enabled = true;
								component2.mode = 0;
								component2.intensity = -2.5f;
								component2.chromaticAberration = -0.3f;
								component2.axialAberration = 0.5f;
								component2.blurSpread = 4.5f;
								component2.luminanceDependency = 0.2f;
								component2.blurDistance = 0.8f;
								component2.enabled = true;
							}
							if (this.effectIndex == 8)
							{
								this.kami.SetActive(false);
								UnityEngine.Object @object = Resources.Load("Prefab/p_powder_snow_photo_ver");
								this.kami = (GameObject)UnityEngine.Object.Instantiate(@object);
								Vector3 localPosition = this.kami.transform.localPosition;
								localPosition.y = 3.5f;
								this.kami.transform.localPosition = localPosition;
								component2.mode = 0;
								component2.intensity = 8f;
								component2.chromaticAberration = 0f;
								component2.axialAberration = 0f;
								component2.blurSpread = 5f;
								component2.luminanceDependency = 0f;
								component2.blurDistance = 0f;
								component2.enabled = true;
							}
						}
						for (int l = 1; l < this.lightList.Count; l++)
						{
							UnityEngine.Object.Destroy(this.lightList[l]);
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
						for (int l = 0; l < this.lightList.Count; l++)
						{
							if (l == 0)
							{
								this.lightComboList[l] = new GUIContent("メイン");
							}
							else
							{
								this.lightComboList[l] = new GUIContent("追加" + l);
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
						this.lightIndex[0] = int.Parse(array200[17]);
						this.lightColorR[0] = float.Parse(array200[18]);
						this.lightColorG[0] = float.Parse(array200[19]);
						this.lightColorB[0] = float.Parse(array200[20]);
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
						else if (this.lightIndex[0] == 3)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
							this.bgObject.SetActive(false);
						}
						if (this.lightIndex[0] != 3)
						{
							GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
							this.bgObject.SetActive(true);
							this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
						}
						else
						{
							this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
						}
						GameMain.Instance.MainLight.transform.eulerAngles = new Vector3(float.Parse(array200[21]), float.Parse(array200[22]), float.Parse(array200[23]));
						GameMain.Instance.MainLight.GetComponent<Light>().intensity = float.Parse(array200[24]);
						GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = float.Parse(array200[25]);
						GameMain.Instance.MainLight.GetComponent<Light>().range = float.Parse(array200[26]);
						this.lightX = new List<float>();
						this.lightX.Add(float.Parse(array200[21]));
						this.lightY = new List<float>();
						this.lightY.Add(float.Parse(array200[22]));
						this.lightAkarusa = new List<float>();
						this.lightAkarusa.Add(float.Parse(array200[24]));
						this.lightKage = new List<float>();
						this.lightKage.Add(0.098f);
						this.lightRange = new List<float>();
						this.lightRange.Add(float.Parse(array200[25]));
						this.selectLightIndex = 0;
						this.isIdx1 = false;
						this.isIdx2 = false;
						this.isIdx3 = false;
						this.isIdx4 = false;
						if (!this.kankyoLoadFlg)
						{
							if (!this.isVR)
							{
								this.mainCamera.SetTargetPos(new Vector3(float.Parse(array200[27]), float.Parse(array200[28]), float.Parse(array200[29])), true);
								this.mainCamera.SetDistance(float.Parse(array200[30]), true);
								this.mainCamera.transform.eulerAngles = new Vector3(float.Parse(array200[31]), float.Parse(array200[32]), float.Parse(array200[33]));
								if (int.Parse(array200[34]) == 1)
								{
									this.inName = array200[35];
									this.inText = array200[36];
									this.inText = this.inText.Replace("&kaigyo", "\n");
									this.isMessage = true;
									this.bGuiMessage = false;
									GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
									GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
									MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
									messageWindowMgr.OpenMessageWindowPanel();
									MessageClass messageClass = new MessageClass(gameObject2, messageWindowMgr);
									messageClass.SetText(this.inName, this.inText, "", 0);
									messageClass.FinishChAnime();
								}
								else if (this.isMessage)
								{
									this.isMessage = false;
									GameObject gameObject = GameObject.Find("__GameMain__/SystemUI Root");
									GameObject gameObject2 = gameObject.transform.Find("MessageWindowPanel").gameObject;
									MessageWindowMgr messageWindowMgr = GameMain.Instance.ScriptMgr.adv_kag.MessageWindowMgr;
									messageWindowMgr.CloseMessageWindowPanel();
								}
							}
						}
					}
					if (array200.Length > 37)
					{
						if (array200[37] != "")
						{
							if (this.doguSelectIndex == this.doguObject.Count)
							{
								this.doguIndex.Add(0);
							}
							else if (this.doguObject.Count > 0 && this.doguObject[this.doguSelectIndex] != null)
							{
								UnityEngine.Object.Destroy(this.doguObject[this.doguSelectIndex]);
								this.doguObject.RemoveAt(this.doguSelectIndex);
							}
							else
							{
								this.doguIndex.Add(0);
								this.doguSelectIndex = this.doguIndex.Count - 1;
							}
							string text17 = array200[37].Replace(" ", "_");
							for (int i2 = 0; i2 < this.doguArray.Length; i2++)
							{
								if (text17 == this.doguArray[i2])
								{
									this.doguIndex[this.doguSelectIndex] = i2;
									break;
								}
							}
							if (!this.doguArray[this.doguIndex[this.doguSelectIndex]].StartsWith("mirror"))
							{
								UnityEngine.Object @object = null;
								GameObject gameObject3 = null;
								for (int i2 = 0; i2 < 5; i2++)
								{
									@object = Resources.Load("Prefab/" + this.doguArray[this.doguIndex[this.doguSelectIndex]]);
									if (@object == null)
									{
										GameObject gameObject4 = GameMain.Instance.BgMgr.CreateAssetBundle(text17);
										if (gameObject4 != null)
										{
											gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
										}
									}
									if (@object != null)
									{
										break;
									}
									if (gameObject3 != null)
									{
										break;
									}
									List<int> list6;
									int index;
									(list6 = this.doguIndex)[index = this.doguSelectIndex] = list6[index] + 1;
								}
								if (gameObject3 == null)
								{
									gameObject3 = (UnityEngine.Object.Instantiate(@object) as GameObject);
								}
								this.doguObject.Add(gameObject3);
							}
							else
							{
								Material material = new Material(Shader.Find("Mirror"));
								GameObject gameObject5 = GameObject.CreatePrimitive(PrimitiveType.Plane);
								gameObject5.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
								gameObject5.GetComponent<Renderer>().material = material;
								gameObject5.AddComponent<MirrorReflection2>();
								MirrorReflection2 component3 = gameObject5.GetComponent<MirrorReflection2>();
								component3.m_TextureSize = 2048;
								component3.m_ClipPlaneOffset = 0f;
								gameObject5.GetComponent<Renderer>().enabled = true;
								this.doguObject.Add(gameObject5);
							}
							Vector3 vector5 = Vector3.zero;
							Vector3 zero = Vector3.zero;
							zero.x = -90f;
							switch (this.doguIndex[this.doguSelectIndex])
							{
								case 1:
									vector5.z = -0.5f;
									goto IL_A298;
								case 2:
									vector5.z = -0.5f;
									goto IL_A298;
								case 3:
									vector5.z = 0.5f;
									zero.z = 90f;
									goto IL_A298;
								case 4:
									vector5.z = 0.9f;
									goto IL_A298;
								case 5:
									vector5.z = 0.9f;
									goto IL_A298;
								case 6:
									vector5.z = 0.6f;
									zero.x = 0f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
									goto IL_A298;
								case 7:
									vector5.z = 0.6f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
									goto IL_A298;
								case 8:
									vector5.z = 0.5f;
									zero.x = 0f;
									goto IL_A298;
								case 9:
									vector5.z = 0.32f;
									zero.z = 180f;
									goto IL_A298;
								case 10:
									vector5.z = 0.6f;
									goto IL_A298;
								case 11:
									vector5.z = 0.6f;
									goto IL_A298;
								case 12:
									vector5.z = 0.5f;
									zero.z = 180f;
									goto IL_A298;
								case 13:
									vector5.z = 0.7f;
									zero.z = 180f;
									goto IL_A298;
								case 14:
									vector5.z = -0.4f;
									goto IL_A298;
								case 15:
									vector5.z = -0.4f;
									goto IL_A298;
								case 16:
									vector5.z = -0.4f;
									zero.z = 180f;
									goto IL_A298;
								case 17:
									vector5.z = -0.4f;
									goto IL_A298;
								case 18:
									vector5.z = -0.2f;
									goto IL_A298;
								case 19:
									vector5.z = 0f;
									goto IL_A298;
								case 20:
									vector5.z = 3.4f;
									vector5.x = -1f;
									goto IL_A298;
								case 21:
									vector5.z = 3.4f;
									vector5.x = -1f;
									goto IL_A298;
								case 22:
									vector5.z = 3.4f;
									vector5.y = 0.3f;
									vector5.x = -1f;
									goto IL_A298;
								case 23:
									vector5.z = 2.7f;
									vector5.x = 0.77f;
									zero.z = 90f;
									goto IL_A298;
								case 25:
									vector5.z = 0.5f;
									vector5.y = 0.9f;
									zero.z = 180f;
									goto IL_A298;
								case 26:
									vector5.z = 0.5f;
									vector5.y = 0.9f;
									zero.z = 180f;
									goto IL_A298;
								case 29:
									vector5.z = 0.5f;
									vector5.x = -1.05f;
									goto IL_A298;
								case 30:
									vector5.z = 0f;
									zero.z = 0f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
									goto IL_A298;
								case 31:
									vector5.z = -0.6f;
									vector5.y = 0.96f;
									zero.z = 180f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
									goto IL_A298;
								case 32:
									vector5.z = -0.6f;
									vector5.y = 0.96f;
									zero.z = 180f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(0.1f, 0.4f, 0.2f);
									goto IL_A298;
								case 33:
									vector5.z = -0.6f;
									vector5.y = 0.85f;
									zero.z = 180f;
									this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(0.03f, 0.18f, 0.124f);
									goto IL_A298;
							}
							vector5.z = 0.5f;
						IL_A298:
							this.doguObject[this.doguSelectIndex].transform.localPosition = vector5;
							this.doguObject[this.doguSelectIndex].transform.localRotation = Quaternion.Euler(zero);
							this.doguObject[this.doguSelectIndex].transform.localEulerAngles = new Vector3(float.Parse(array200[38]), float.Parse(array200[39]), float.Parse(array200[40]));
							this.doguObject[this.doguSelectIndex].transform.position = new Vector3(float.Parse(array200[41]), float.Parse(array200[42]), float.Parse(array200[43]));
							this.doguObject[this.doguSelectIndex].transform.localScale = new Vector3(float.Parse(array200[44]), float.Parse(array200[45]), float.Parse(array200[46]));
						}
					}
					if (!this.kankyoLoadFlg)
					{
						for (int l = 0; l < array199.Length; l++)
						{
							if (this.maidCnt <= l)
							{
								break;
							}
							Maid maid = this.maidArray[l];
							if (maid && maid.Visible)
							{
								this.SetIK(maid, l);
								Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
								Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
								Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
								Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
								Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
								Transform transform9 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
								Transform transform10 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
								Transform transform11 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
								Transform transform12 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
								Transform transform13 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
								Transform transform14 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
								Transform transform15 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
								Transform transform16 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
								Transform transform17 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
								Transform transform18 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
								Transform transform19 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
								Transform transform20 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
								Transform transform21 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
								Transform transform22 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
								string[] array205 = array199[l].Split(new char[]
								{
									':'
								});
								for (int i2 = 0; i2 < 40; i2++)
								{
									string[] array206 = array205[i2].Split(new char[]
									{
										','
									});
									this.Finger[l, i2].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
								}
								bool flag5 = false;
								if (array205.Length == 64)
								{
									flag5 = true;
								}
								if (!this.isVR)
								{
									maid.body0.quaDefEyeL.eulerAngles = this.eyeL[(int)this.selectList[l]];
									maid.body0.quaDefEyeR.eulerAngles = this.eyeR[(int)this.selectList[l]];
								}
								bool flag6 = false;
								for (int k = 0; k < 3; k++)
								{
									for (int i2 = 40; i2 < array205.Length; i2++)
									{
										string[] array206 = array205[i2].Split(new char[]
										{
											','
										});
										switch (i2)
										{
											case 40:
												if (flag5)
												{
													this.Spine.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 41:
												if (flag5)
												{
													this.Spine0a.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine0a.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 42:
												if (flag5)
												{
													this.Spine1.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine1.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 43:
												if (flag5)
												{
													this.Spine1a.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine1a.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 44:
												if (flag5)
												{
													this.Pelvis.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Pelvis.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 45:
												this.HandL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 46:
												if (flag5)
												{
													this.UpperArmL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 47:
												if (flag5)
												{
													this.ForearmL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 48:
												this.HandR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 49:
												if (flag5)
												{
													this.UpperArmR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 50:
												if (flag5)
												{
													this.ForearmR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 51:
												this.HandL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 52:
												if (flag5)
												{
													this.UpperArmL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmL2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 53:
												if (flag5)
												{
													this.ForearmL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmL2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 54:
												this.HandR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 55:
												if (flag5)
												{
													this.UpperArmR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmR2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 56:
												if (flag5)
												{
													this.ForearmR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmR2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 57:
												if (flag5)
												{
													this.Head.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Head.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 58:
												maid.transform.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 59:
												maid.transform.position = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 60:
												maid.transform.localScale = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 63:
												if (k <= 0)
												{
													this.hanten[l] = false;
													this.hantenn[l] = false;
													TMorph morph = maid.body0.Face.morph;
													float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
													float[] fieldValue5 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
													if (!this.isVR)
													{
														maid.boMabataki = false;
													}
													if (morph.bodyskin.PartsVersion < 120)
													{
														fieldValue5[(int)morph.hash["eyeclose"]] = float.Parse(array206[0]);
														fieldValue5[(int)morph.hash["eyeclose2"]] = float.Parse(array206[1]);
														fieldValue5[(int)morph.hash["eyeclose3"]] = float.Parse(array206[2]);
														fieldValue5[(int)morph.hash["eyeclose6"]] = float.Parse(array206[3]);
														fieldValue5[(int)morph.hash["eyeclose5"]] = float.Parse(array206[17]);
														if (morph.hash["eyeclose7"] != null)
														{
															if (array206.Length > 37)
															{
																fieldValue5[(int)morph.hash["eyeclose7"]] = float.Parse(array206[36]);
																fieldValue5[(int)morph.hash["eyeclose8"]] = float.Parse(array206[37]);
															}
															else
															{
																fieldValue5[(int)morph.hash["eyeclose7"]] = 0f;
																fieldValue5[(int)morph.hash["eyeclose8"]] = 0f;
															}
														}
													}
													else
													{
														int num102 = 0;
														if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
														{
															num102 = 1;
														}
														if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
														{
															num102 = 2;
														}
														fieldValue5[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[0]);
														fieldValue5[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[1]);
														fieldValue[(int)morph.hash["eyeclose3"]] = float.Parse(array206[2]) * 3f;
														fieldValue5[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[3]);
														fieldValue5[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[17]);
														if (array206.Length > 37)
														{
															fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[36]);
															fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]] = float.Parse(array206[37]);
														}
														else
														{
															fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]] = 0f;
															fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]] = 0f;
														}
													}
													fieldValue[(int)morph.hash["hitomih"]] = float.Parse(array206[4]);
													fieldValue[(int)morph.hash["hitomis"]] = float.Parse(array206[5]);
													fieldValue[(int)morph.hash["mayuha"]] = float.Parse(array206[6]);
													fieldValue[(int)morph.hash["mayuup"]] = float.Parse(array206[7]);
													fieldValue[(int)morph.hash["mayuv"]] = float.Parse(array206[8]);
													fieldValue[(int)morph.hash["mayuvhalf"]] = float.Parse(array206[9]);
													fieldValue[(int)morph.hash["moutha"]] = float.Parse(array206[10]);
													fieldValue[(int)morph.hash["mouths"]] = float.Parse(array206[11]);
													fieldValue[(int)morph.hash["mouthdw"]] = float.Parse(array206[12]);
													fieldValue[(int)morph.hash["mouthup"]] = float.Parse(array206[13]);
													fieldValue[(int)morph.hash["tangout"]] = float.Parse(array206[14]);
													fieldValue[(int)morph.hash["tangup"]] = float.Parse(array206[15]);
													if (morph.bodyskin.PartsVersion < 120)
													{
														fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array206[16]);
													}
													else
													{
														fieldValue[(int)morph.hash["eyebig"]] = float.Parse(array206[16]) * 3f;
													}
													fieldValue[(int)morph.hash["mayuw"]] = float.Parse(array206[18]);
													fieldValue[(int)morph.hash["mouthhe"]] = float.Parse(array206[19]);
													fieldValue[(int)morph.hash["mouthc"]] = float.Parse(array206[20]);
													fieldValue[(int)morph.hash["mouthi"]] = float.Parse(array206[21]);
													fieldValue[(int)morph.hash["mouthuphalf"]] = float.Parse(array206[22]) + 0.01f;
													this.isLoadFace[l] = true;
													try
													{
														fieldValue[(int)morph.hash["tangopen"]] = float.Parse(array206[23]);
													}
													catch
													{
													}
													if (float.Parse(array206[24]) == 1f)
													{
														fieldValue[(int)morph.hash["namida"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["namida"]] = 0f;
													}
													if (float.Parse(array206[25]) == 1f)
													{
														fieldValue[(int)morph.hash["tear1"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["tear1"]] = 0f;
													}
													if (float.Parse(array206[26]) == 1f)
													{
														fieldValue[(int)morph.hash["tear2"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["tear2"]] = 0f;
													}
													if (float.Parse(array206[27]) == 1f)
													{
														fieldValue[(int)morph.hash["tear3"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["tear3"]] = 0f;
													}
													if (float.Parse(array206[28]) == 1f)
													{
														fieldValue[(int)morph.hash["shock"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["shock"]] = 0f;
													}
													if (float.Parse(array206[29]) == 1f)
													{
														fieldValue[(int)morph.hash["yodare"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["yodare"]] = 0f;
													}
													if (float.Parse(array206[30]) == 1f)
													{
														fieldValue[(int)morph.hash["hoho"]] = 0.5f;
													}
													else
													{
														fieldValue[(int)morph.hash["hoho"]] = 0f;
													}
													if (float.Parse(array206[31]) == 1f)
													{
														fieldValue[(int)morph.hash["hoho2"]] = 0.5f;
													}
													else
													{
														fieldValue[(int)morph.hash["hoho2"]] = 0f;
													}
													if (float.Parse(array206[32]) == 1f)
													{
														fieldValue[(int)morph.hash["hohos"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["hohos"]] = 0f;
													}
													if (float.Parse(array206[33]) == 1f)
													{
														fieldValue[(int)morph.hash["hohol"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["hohol"]] = 0f;
													}
													if (float.Parse(array206[34]) == 1f)
													{
														fieldValue[(int)morph.hash["toothoff"]] = 1f;
													}
													else
													{
														fieldValue[(int)morph.hash["toothoff"]] = 0f;
													}
													if (array206.Length > 35)
													{
														if (float.Parse(array206[35]) == 1f)
														{
															morph.boNoseFook = true;
														}
														else
														{
															morph.boNoseFook = false;
														}
													}
													this.isFace[l] = true;
													this.isFaceInit = true;
												}
												break;
											case 64:
												if (k <= 0)
												{
													if (!this.isVR)
													{
														if (int.Parse(array206[0]) == 1)
														{
															this.isLook[l] = true;
															this.lookX[l] = float.Parse(array206[1]);
															this.lookY[l] = float.Parse(array206[2]);
															if (maid.body0.offsetLookTarget.x != this.lookY[l])
															{
																if (this.isLock[l] && this.lookY[l] < 0f)
																{
																	maid.body0.offsetLookTarget = new Vector3(this.lookY[l] * 0.6f, 1f, this.lookX[l]);
																}
																else
																{
																	maid.body0.offsetLookTarget = new Vector3(this.lookY[l], 1f, this.lookX[l]);
																}
															}
															if (this.lookX[l] != this.lookXn[l])
															{
																this.lookXn[l] = this.lookX[l];
																maid.body0.offsetLookTarget = new Vector3(this.lookY[l], 1f, this.lookX[l]);
															}
															if (this.lookY[l] != this.lookYn[l])
															{
																this.lookYn[l] = this.lookY[l];
																if (this.isLock[l] && this.lookY[l] < 0f)
																{
																	maid.body0.offsetLookTarget = new Vector3(this.lookY[l] * 0.6f, 1f, this.lookX[l]);
																}
																else
																{
																	maid.body0.offsetLookTarget = new Vector3(this.lookY[l], 1f, this.lookX[l]);
																}
															}
														}
														else
														{
															this.isLook[l] = false;
														}
														if (this.isLook[l])
														{
															maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
															maid.body0.boHeadToCam = true;
															maid.body0.boEyeToCam = true;
															maid.body0.trsLookTarget = null;
														}
														else
														{
															maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
															maid.body0.boHeadToCam = true;
															maid.body0.boEyeToCam = true;
														}
													}
												}
												break;
											case 65:
												if (k <= 0)
												{
													if (this.itemIndex[l] != int.Parse(array206[0]))
													{
														this.itemIndex[l] = int.Parse(array206[0]);
														if (this.itemIndex[l] == this.itemArray.Length - 1)
														{
															this.itemIndex[l] = 0;
														}
														string[] array207 = new string[2];
														array207 = this.itemArray[this.itemIndex[l]].Split(new char[]
														{
														','
														});
														if (this.itemIndex[l] > 13)
														{
															array207 = this.itemArray[this.itemIndex[l] + 1].Split(new char[]
															{
															','
															});
														}
														maid.DelProp(MPN.handitem, true);
														maid.DelProp(MPN.accvag, true);
														maid.DelProp(MPN.accanl, true);
														bool flag7 = false;
														if (this.itemIndex[l] == 12 || this.itemIndex[l] == 13)
														{
															flag7 = true;
														}
														if (!flag7)
														{
															maid.DelProp(MPN.kousoku_upper, true);
															maid.DelProp(MPN.kousoku_lower, true);
														}
														if (array207[0] != "")
														{
															maid.SetProp(array207[0], array207[1], 0, true, false);
														}
														if (this.itemIndex[l] == 12)
														{
															array207 = this.itemArray[this.itemIndex[l] - 1].Split(new char[]
															{
															','
															});
															maid.SetProp(array207[0], array207[1], 0, true, false);
														}
														if (this.itemIndex[l] == 13)
														{
															array207 = this.itemArray[this.itemIndex[l] + 1].Split(new char[]
															{
															','
															});
															maid.SetProp(array207[0], array207[1], 0, true, false);
														}
														if (this.itemIndex[l] == 23)
														{
															array207 = this.itemArray[this.itemIndex[l]].Split(new char[]
															{
															','
															});
															maid.SetProp(array207[0], array207[1], 0, true, false);
															this.cafeFlg[l] = true;
														}
														maid.AllProcPropSeqStart();
													}
													this.IK_hand = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
												}
												break;
											case 66:
												{
													Transform transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
													transform3.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
													break;
												}
											case 67:
												{
													Transform transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
													transform4.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
													break;
												}
											case 68:
												if (array206.Length >= 2)
												{
													this.ClavicleL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 69:
												this.ClavicleR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 70:
												if (int.Parse(array206[0]) == 1)
												{
													this.hanten[l] = true;
													this.hantenn[l] = true;
												}
												break;
											case 71:
												transform2.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 72:
												transform5.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 73:
												transform6.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 74:
												transform7.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 75:
												transform8.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 76:
												transform9.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 77:
												transform10.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 78:
												transform11.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 79:
												transform12.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 80:
												transform13.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 81:
												transform14.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 82:
												transform15.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 83:
												transform16.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 84:
												transform17.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 85:
												transform18.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 86:
												transform19.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 87:
												transform20.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 88:
												transform21.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 89:
												transform22.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 90:
												{
													float num203 = float.Parse(array206[0]);
													if (maid.body0.Face.morph.bodyskin.PartsVersion < 120)
													{
														if (num203 > 4.1f && num203 < 5.1f)
														{
															flag6 = true;
														}
													}
													else if (num203 > 0f && num203 < 0.6f)
													{
														flag6 = true;
													}
													if (!this.isVR && flag6)
													{
														maid.body0.quaDefEyeL.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
													}
													break;
												}
											case 91:
												if (!this.isVR && flag6)
												{
													maid.body0.quaDefEyeR.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 92:
												this.vIKMuneL[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												this.muneIKL[l] = true;
												break;
											case 93:
												this.vIKMuneLSub[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 94:
												this.vIKMuneR[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												this.muneIKR[l] = true;
												break;
											case 95:
												this.vIKMuneRSub[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 96:
												this.haraCount[l] = 2;
												this.haraPosition[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												transform2.position = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
										}
									}
								}
								this.isStop[l] = true;
								this.isLock[l] = true;
							}
						}
					}
					this.isBloom = false;
					this.bloom1 = 2.85f;
					this.bloom2 = 3f;
					this.bloom3 = 0f;
					this.bloom4 = 0f;
					this.bloom5 = 0f;
					this.isBloomA = false;
					this.isBlur = false;
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
					this.depth4 = 3f;
					this.isFog = false;
					this.fog1 = 4f;
					this.fog2 = 1f;
					this.fog3 = 1f;
					this.fog4 = 0f;
					this.fog5 = 1f;
					this.fog6 = 1f;
					this.fog7 = 1f;
					this.isSepia = false;
					if (array201 != null)
					{
						this.lightKage[0] = float.Parse(array201[0]);
						if (int.Parse(array201[1]) == 1)
						{
							this.isBloom = true;
							this.bloom1 = float.Parse(array201[2]);
							this.bloom2 = float.Parse(array201[3]);
							this.bloom3 = float.Parse(array201[4]);
							this.bloom4 = float.Parse(array201[5]);
							this.bloom5 = float.Parse(array201[6]);
							if (int.Parse(array201[7]) == 1)
							{
								this.isBloomA = true;
							}
						}
						if (int.Parse(array201[8]) == 1)
						{
							this.isBlur = true;
							this.blur1 = float.Parse(array201[9]);
							this.blur2 = float.Parse(array201[10]);
							this.blur3 = float.Parse(array201[11]);
							this.blur4 = float.Parse(array201[12]);
						}
						this.bokashi = float.Parse(array201[13]);
						this.kamiyure = float.Parse(array201[14]);
						if (array201.Length > 15)
						{
							if (int.Parse(array201[15]) == 1)
							{
								this.isDepth = true;
								this.depth1 = float.Parse(array201[16]);
								this.depth2 = float.Parse(array201[17]);
								this.depth3 = float.Parse(array201[18]);
								this.depth4 = float.Parse(array201[19]);
								if (int.Parse(array201[20]) == 1)
								{
									this.isDepthA = true;
								}
							}
							if (int.Parse(array201[21]) == 1)
							{
								this.isFog = true;
								this.fog1 = float.Parse(array201[22]);
								this.fog2 = float.Parse(array201[23]);
								this.fog3 = float.Parse(array201[24]);
								this.fog4 = float.Parse(array201[25]);
								this.fog5 = float.Parse(array201[26]);
								this.fog6 = float.Parse(array201[27]);
								this.fog7 = float.Parse(array201[28]);
							}
							if (int.Parse(array201[29]) == 1)
							{
								this.isSepia = true;
							}
						}
					}
					if (array202 != null)
					{
						for (int l = 0; l < array202.Length - 1; l++)
						{
							string[] array206 = array202[l].Split(new char[]
							{
								','
							});
							GameObject gameObject6 = new GameObject("Light");
							gameObject6.AddComponent<Light>();
							this.lightList.Add(gameObject6);
							this.lightIndex.Add(int.Parse(array206[0]));
							this.lightColorR.Add(float.Parse(array206[1]));
							this.lightColorG.Add(float.Parse(array206[2]));
							this.lightColorB.Add(float.Parse(array206[3]));
							this.lightX.Add(float.Parse(array206[4]));
							this.lightY.Add(float.Parse(array206[5]));
							this.lightAkarusa.Add(float.Parse(array206[6]));
							this.lightRange.Add(float.Parse(array206[7]));
							this.lightKage.Add(0.098f);
							gameObject6.transform.position = GameMain.Instance.MainLight.transform.position;
							this.selectLightIndex = this.lightList.Count - 1;
							this.lightComboList = new GUIContent[this.lightList.Count];
							for (int k = 0; k < this.lightList.Count; k++)
							{
								if (k == 0)
								{
									this.lightComboList[k] = new GUIContent("メイン");
								}
								else
								{
									this.lightComboList[k] = new GUIContent("追加" + k);
								}
							}
							this.lightCombo.selectedItemIndex = this.selectLightIndex;
							gameObject6.GetComponent<Light>().intensity = this.lightAkarusa[l];
							gameObject6.GetComponent<Light>().spotAngle = this.lightRange[l];
							gameObject6.GetComponent<Light>().range = this.lightRange[l] / 5f;
							if (this.lightIndex[this.selectLightIndex] == 0)
							{
								this.lightList[this.selectLightIndex].GetComponent<Light>().type = LightType.Directional;
							}
							else if (this.lightIndex[this.selectLightIndex] == 1)
							{
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
					}
					if (array204 != null)
					{
						for (int l = 0; l < this.lightList.Count; l++)
						{
							string[] array206 = array204[l].Split(new char[]
							{
								','
							});
							this.lightList[l].transform.position = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
							if (this.gLight[l] == null)
							{
								this.gLight[l] = GameObject.CreatePrimitive(PrimitiveType.Cube);
								Material material2 = new Material(Shader.Find("Transparent/Diffuse"));
								material2.color = new Color(0.5f, 0.5f, 1f, 0.8f);
								this.gLight[l].GetComponent<Renderer>().material = material2;
								this.gLight[l].layer = 8;
								this.gLight[l].GetComponent<Renderer>().enabled = false;
								this.gLight[l].SetActive(false);
								this.gLight[l].transform.position = this.lightList[l].transform.position;
								this.mLight[l] = this.gLight[l].AddComponent<MouseDrag6>();
								this.mLight[l].obj = this.gLight[l];
								this.mLight[l].maid = this.lightList[l].gameObject;
								this.mLight[l].angles = this.lightList[l].transform.eulerAngles;
								this.gLight[l].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
								this.mLight[l].ido = 1;
								this.mLight[l].isScale = false;
							}
						}
					}
					if (array203 != null)
					{
						for (int l = 0; l < this.doguBObject.Count; l++)
						{
							UnityEngine.Object.Destroy(this.doguBObject[l]);
						}
						this.doguBObject = new List<GameObject>();
						List<string> list4 = new List<string>();
						for (int l = 0; l < array203.Length - 1; l++)
						{
							string[] array206 = array203[l].Split(new char[]
							{
								','
							});
							string text17 = array206[0].Replace(" ", "_");
							GameObject gameObject3 = null;
							bool flag8 = false;
							if (text17.Contains(".menu"))
							{
								string text18 = text17;
								byte[] array208 = null;
								try
								{
									using (AFileBase afileBase = GameUty.FileOpen(text18, null))
									{
										NDebug.Assert(afileBase.IsValid(), "メニューファイルが存在しません。 :" + text18);
										if (array208 == null)
										{
											array208 = new byte[afileBase.GetSize()];
										}
										else if (array208.Length < afileBase.GetSize())
										{
											array208 = new byte[afileBase.GetSize()];
										}
										afileBase.Read(ref array208, afileBase.GetSize());
									}
									string[] array209 = MultipleMaids.ProcScriptBin(this.maidArray[0], array208, text18, false);
									gameObject3 = ImportCM2.LoadSkinMesh_R(array209[0], array209, "", this.maidArray[0].body0.goSlot[8], 1);
									this.doguBObject.Add(gameObject3);
									gameObject3.name = text18;
									Vector3 vector5 = Vector3.zero;
									Vector3 zero = Vector3.zero;
									vector5.z = 0.4f;
									if (text17.Contains("HandItem"))
									{
										string text19 = text17;
										if (text19 != null)
										{
											if (text19 == "HandItemR_WineGlass_I_.menu")
											{
												zero.z = 90f;
												vector5.y = 0.04f;
												goto IL_D31E;
											}
											if (text19 == "HandItemR_WineBottle_I_.menu")
											{
												zero.z = 90f;
												goto IL_D31E;
											}
											if (text19 == "HandItemR_Mop_I_.menu")
											{
												zero.x = 90f;
												goto IL_D31E;
											}
											if (text19 == "HandItemL_Dance_Hataki_I_.menu")
											{
												goto IL_D31E;
											}
											if (text19 == "HandItemL_Dance_Mop_I_.menu")
											{
												goto IL_D31E;
											}
										}
										zero.x = -90f;
									IL_D31E:;
									}
									gameObject3.transform.localPosition = vector5;
									gameObject3.transform.localRotation = Quaternion.Euler(zero);
								}
								catch
								{
									flag8 = true;
								}
							}
							else
							{
								try
								{
									bool flag9 = false;
									bool flag10 = false;
									if (text17.StartsWith("BG_"))
									{
										UnityEngine.Object @object = GameMain.Instance.BgMgr.CreateAssetBundle(text17.Replace("BG_", ""));
										if (@object == null)
										{
											@object = Resources.Load(text17.Replace("BG_", "BG/"));
										}
										gameObject3 = (UnityEngine.Object.Instantiate(@object) as GameObject);
										this.doguBObject.Add(gameObject3);
									}
									else if (text17.StartsWith("MYR_"))
									{
										GameObject gameObject = GameObject.Find("Deployment Object Parent");
										if (gameObject == null)
										{
											gameObject = new GameObject("Deployment Object Parent");
										}
										int num204 = int.Parse(text17.Replace("MYR_", ""));
										PlacementData.Data data = PlacementData.GetData(num204);
										GameObject prefab = data.GetPrefab();
										GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(prefab);
										GameObject gameObject4 = new GameObject(gameObject7.name);
										gameObject7.transform.SetParent(gameObject4.transform, true);
										gameObject4.transform.SetParent(gameObject.transform, false);
										gameObject3 = gameObject4;
										this.doguBObject.Add(gameObject3);
									}
									else if (text17.Contains("#"))
									{
										string[] array205 = text17.Split(new char[]
										{
											'#'
										});
										gameObject3 = GameMain.Instance.BgMgr.CreateAssetBundle(array205[1]);
										if (gameObject3 != null)
										{
											gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
											MeshRenderer[] componentsInChildren = gameObject3.GetComponentsInChildren<MeshRenderer>();
											for (int j = 0; j < componentsInChildren.Length; j++)
											{
												if (componentsInChildren[j] != null)
												{
													componentsInChildren[j].shadowCastingMode = ShadowCastingMode.Off;
												}
											}
										}
										flag9 = true;
										if (!this.parArray[this.parIndex].Contains("Odogu_"))
										{
											flag10 = true;
										}
										this.doguBObject.Add(gameObject3);
									}
									else if (!text17.StartsWith("mirror") && text17.IndexOf(":") < 0)
									{
										UnityEngine.Object @object;
										if (text17.StartsWith("BG"))
										{
											string text3 = text17.Replace("BG", "");
											@object = GameMain.Instance.BgMgr.CreateAssetBundle(text3);
											if (@object == null)
											{
												@object = Resources.Load("BG/" + text3);
											}
										}
										else
										{
											@object = Resources.Load("Prefab/" + text17);
											if (@object == null)
											{
												GameObject gameObject4 = GameMain.Instance.BgMgr.CreateAssetBundle(text17);
												gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
											}
										}
										if (gameObject3 == null)
										{
											gameObject3 = (UnityEngine.Object.Instantiate(@object) as GameObject);
										}
										this.doguBObject.Add(gameObject3);
									}
									else if (text17.StartsWith("mirror"))
									{
										Material material = new Material(Shader.Find("Mirror"));
										GameObject gameObject5 = GameObject.CreatePrimitive(PrimitiveType.Plane);
										gameObject5.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
										gameObject5.GetComponent<Renderer>().material = material;
										gameObject5.AddComponent<MirrorReflection2>();
										MirrorReflection2 component3 = gameObject5.GetComponent<MirrorReflection2>();
										component3.m_TextureSize = 2048;
										component3.m_ClipPlaneOffset = 0f;
										gameObject5.GetComponent<Renderer>().enabled = true;
										gameObject3 = gameObject5;
										this.doguBObject.Add(gameObject3);
									}
									else
									{
										string[] array205 = text17.Split(new char[]
										{
											':'
										});
										UnityEngine.Object @object = GameMain.Instance.BgMgr.CreateAssetBundle(array205[0]);
										if (@object == null)
										{
											@object = Resources.Load("BG/" + array205[0]);
										}
										GameObject gameObject8 = UnityEngine.Object.Instantiate(@object) as GameObject;
										int num205 = 0;
										int num206 = 0;
										int.TryParse(array205[1], out num206);
										foreach (object obj in gameObject8.transform)
										{
											Transform transform27 = (Transform)obj;
											if (num206 == num205)
											{
												gameObject3 = transform27.gameObject;
												break;
											}
											num205++;
										}
										this.doguBObject.Add(gameObject3);
										gameObject3.transform.parent = null;
										UnityEngine.Object.Destroy(gameObject8);
										gameObject8.SetActive(false);
									}
									gameObject3.name = text17;
									Vector3 vector5 = Vector3.zero;
									Vector3 zero = Vector3.zero;
									if (text17.StartsWith("BG_"))
									{
										gameObject3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
									}
									string text19 = text17;
									switch (text19)
									{
										case "Odogu_XmasTreeMini_photo_ver":
											vector5.z = 0.6f;
											gameObject3.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
											foreach (object obj2 in gameObject3.transform)
											{
												Transform transform28 = (Transform)obj2;
												if (transform28.GetComponent<Collider>() != null)
												{
													transform28.GetComponent<Collider>().enabled = false;
												}
											}
											goto IL_DEE9;
										case "Odogu_KadomatsuMini_photo_ver":
											vector5.z = 0.6f;
											gameObject3.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
											foreach (object obj3 in gameObject3.transform)
											{
												Transform transform28 = (Transform)obj3;
												if (transform28.GetComponent<Collider>() != null)
												{
													transform28.GetComponent<Collider>().enabled = false;
												}
											}
											goto IL_DEE9;
										case "Odogu_ClassRoomDesk":
											vector5.z = 0.5f;
											zero.x = -90f;
											goto IL_DEE9;
										case "mirror1":
											vector5.z = -0.6f;
											vector5.y = 0.96f;
											zero.z = 180f;
											zero.x = -90f;
											gameObject3.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
											goto IL_DEE9;
										case "mirror2":
											vector5.z = -0.6f;
											vector5.y = 0.96f;
											zero.z = 180f;
											zero.x = -90f;
											gameObject3.transform.localScale = new Vector3(0.1f, 0.4f, 0.2f);
											goto IL_DEE9;
										case "mirror3":
											vector5.z = -0.6f;
											vector5.y = 0.85f;
											zero.z = 180f;
											zero.x = -90f;
											gameObject3.transform.localScale = new Vector3(0.03f, 0.18f, 0.124f);
											goto IL_DEE9;
										case "Particle/pLineY":
										case "Particle/pLineP02":
										case "Particle/pLine_act2":
											gameObject3.transform.localScale = new Vector3(3f, 3f, 3f);
											goto IL_DEE9;
										case "Mob_Man_Stand001":
										case "Mob_Man_Stand002":
										case "Mob_Man_Stand003":
										case "Mob_Man_Sit001":
										case "Mob_Man_Sit002":
										case "Mob_Man_Sit003":
										case "Mob_Girl_Stand001":
										case "Mob_Girl_Stand002":
										case "Mob_Girl_Stand003":
										case "Mob_Girl_Sit001":
										case "Mob_Girl_Sit002":
										case "Mob_Girl_Sit003":
										case "Salon:63":
										case "Salon:65":
										case "Salon:69":
										case "Salon_Entrance:0":
										case "Salon_Entrance:1":
										case "Salon_Entrance:2":
										case "Salon_Entrance:3":
										case "Salon_Entrance:4":
										case "Pool:26":
										case "Shitsumu:23":
										case "Shitsumu_Night:23":
											vector5.z = 0.5f;
											zero.x = -90f;
											goto IL_DEE9;
										case "Odogu_Dresser_photo_ver":
											GameObject.Find("Prim.00000000").GetComponent<Collider>().enabled = false;
											GameObject.Find("Prim.00000001").GetComponent<Collider>().enabled = false;
											GameObject.Find("Prim.00000002").GetComponent<Collider>().enabled = false;
											GameObject.Find("Prim.00000004").GetComponent<Collider>().enabled = false;
											goto IL_DEE9;
									}
									vector5.z = 0.5f;
									if (text17.StartsWith("Odogu_"))
									{
										foreach (object obj4 in gameObject3.transform)
										{
											Transform transform28 = (Transform)obj4;
											if (transform28.GetComponent<Collider>() != null)
											{
												transform28.GetComponent<Collider>().enabled = false;
											}
										}
									}
									else if (gameObject3.GetComponent<Collider>() != null)
									{
										gameObject3.GetComponent<Collider>().enabled = false;
									}
								IL_DEE9:
									if (flag9)
									{
										vector5.z = 0.4f;
										if (flag10)
										{
											zero.x = -90f;
										}
									}
									gameObject3.transform.localPosition = vector5;
									gameObject3.transform.localRotation = Quaternion.Euler(zero);
								}
								catch
								{
									flag8 = true;
								}
							}
							if (!flag8)
							{
								this.doguCnt = this.doguBObject.Count - 1;
								this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
								Vector3 localEulerAngles2 = gameObject3.transform.localEulerAngles;
								gameObject3.transform.localEulerAngles = new Vector3(float.Parse(array206[1]), float.Parse(array206[2]), float.Parse(array206[3]));
								gameObject3.transform.position = new Vector3(float.Parse(array206[4]), float.Parse(array206[5]), float.Parse(array206[6]));
								this.gDogu[this.doguCnt].transform.localEulerAngles = new Vector3(float.Parse(array206[1]), float.Parse(array206[2]), float.Parse(array206[3]));
								this.gDogu[this.doguCnt].transform.position = new Vector3(float.Parse(array206[4]), float.Parse(array206[5]), float.Parse(array206[6]));
								this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
								this.gDogu[this.doguCnt].layer = 8;
								this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
								this.gDogu[this.doguCnt].SetActive(false);
								this.gDogu[this.doguCnt].transform.position = gameObject3.transform.position;
								this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
								this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
								this.mDogu[this.doguCnt].maid = gameObject3;
								this.mDogu[this.doguCnt].angles = localEulerAngles2;
								this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
								this.mDogu[this.doguCnt].ido = 1;
								this.mDogu[this.doguCnt].isScale = false;
								if (text17 == "Particle/pLineY")
								{
									this.mDogu[this.doguCnt].count = 180;
								}
								if (text17 == "Particle/pLineP02")
								{
									this.mDogu[this.doguCnt].count = 115;
								}
								if (gameObject3.name == "Particle/pLine_act2")
								{
									this.mDogu[this.doguCnt].count = 80;
									gameObject3.transform.localScale = new Vector3(3f, 3f, 3f);
								}
								if (gameObject3.name == "Particle/pHeart01")
								{
									this.mDogu[this.doguCnt].count = 80;
								}
								if (text17 == "mirror1" || text17 == "mirror2" || text17 == "mirror3")
								{
									this.mDogu[this.doguCnt].isScale = true;
									this.mDogu[this.doguCnt].isScale2 = true;
									this.mDogu[this.doguCnt].scale2 = gameObject3.transform.localScale;
									if (text17 == "mirror1")
									{
										this.mDogu[this.doguCnt].scale = new Vector3(gameObject3.transform.localScale.x * 5f, gameObject3.transform.localScale.y * 5f, gameObject3.transform.localScale.z * 5f);
									}
									if (text17 == "mirror2")
									{
										this.mDogu[this.doguCnt].scale = new Vector3(gameObject3.transform.localScale.x * 10f, gameObject3.transform.localScale.y * 10f, gameObject3.transform.localScale.z * 10f);
									}
									if (text17 == "mirror3")
									{
										this.mDogu[this.doguCnt].scale = new Vector3(gameObject3.transform.localScale.x * 33f, gameObject3.transform.localScale.y * 33f, gameObject3.transform.localScale.z * 33f);
									}
								}
								if (text17 == "Odogu_XmasTreeMini_photo_ver" || text17 == "Odogu_KadomatsuMini_photo_ver")
								{
									this.mDogu[this.doguCnt].isScale2 = true;
									this.mDogu[this.doguCnt].scale2 = gameObject3.transform.localScale;
								}
								gameObject3.transform.localScale = new Vector3(float.Parse(array206[7]), float.Parse(array206[8]), float.Parse(array206[9]));
							}
							else
							{
								list4.Add(text17);
							}
						}
						if (list4.Count > 0)
						{
							Console.WriteLine("\nシーンの読み込みに以下のファイルが不足しています。");
							foreach (string text in list4)
							{
								Console.WriteLine(text);
							}
						}
					}
					this.loadScene = 0;
					this.kankyoLoadFlg = false;
				}
				for (int l = 0; l < this.maidCnt; l++)
				{
					if (this.loadPose[l] != "" && this.isLoadPose[l])
					{
						IniKey iniKey3 = base.Preferences["pose"][this.loadPose[l]];
						if (iniKey3.Value == null || !(iniKey3.Value.ToString() != "") || !(iniKey3.Value.ToString() != "del"))
						{
							this.loadPose[l] = "";
							this.isLoadPose[l] = false;
						}
						else
						{
							if (this.loadCount[l] > 4)
							{
								this.isLoadPose[l] = false;
								this.loadPose[l] = "";
								this.loadCount[l] = 0;
							}
							for (int k = 0; k < 10; k++)
							{
							}
							this.loadCount[l]++;
							Maid maid = this.maidArray[l];
							this.isStop[l] = true;
							if (maid && maid.Visible)
							{
								this.SetIK(maid, l);
								Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
								Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
								Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
								Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
								Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
								Transform transform9 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
								Transform transform10 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
								Transform transform11 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
								Transform transform12 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
								Transform transform13 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
								Transform transform14 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
								Transform transform15 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
								Transform transform16 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
								Transform transform17 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
								Transform transform18 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
								Transform transform19 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
								Transform transform20 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
								Transform transform21 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
								Transform transform22 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
								string[] array205 = iniKey3.Value.Split(new char[]
								{
									'_'
								})[1].Split(new char[]
								{
									':'
								});
								for (int i2 = 0; i2 < 40; i2++)
								{
									string[] array206 = array205[i2].Split(new char[]
									{
										','
									});
									this.Finger[l, i2].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
								}
								bool flag5 = false;
								if (array205.Length == 64)
								{
									flag5 = true;
								}
								Vector3 localEulerAngles = maid.transform.localEulerAngles;
								maid.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
								Vector3 position2 = maid.transform.position;
								maid.transform.position = new Vector3(0f, 0f, 0f);
								for (int k = 0; k < 3; k++)
								{
									for (int i2 = 40; i2 < array205.Length; i2++)
									{
										string[] array206 = array205[i2].Split(new char[]
										{
											','
										});
										switch (i2)
										{
											case 40:
												if (flag5)
												{
													this.Spine.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 41:
												if (flag5)
												{
													this.Spine0a.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine0a.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 42:
												if (flag5)
												{
													this.Spine1.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine1.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 43:
												if (flag5)
												{
													this.Spine1a.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Spine1a.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 44:
												if (flag5)
												{
													this.Pelvis.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Pelvis.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 45:
												this.HandL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 46:
												if (flag5)
												{
													this.UpperArmL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 47:
												if (flag5)
												{
													this.ForearmL1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 48:
												this.HandR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 49:
												if (flag5)
												{
													this.UpperArmR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 50:
												if (flag5)
												{
													this.ForearmR1[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 51:
												this.HandL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 52:
												if (flag5)
												{
													this.UpperArmL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmL2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 53:
												if (flag5)
												{
													this.ForearmL2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmL2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 54:
												this.HandR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 55:
												if (flag5)
												{
													this.UpperArmR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.UpperArmR2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 56:
												if (flag5)
												{
													this.ForearmR2[l].localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.ForearmR2[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 57:
												if (flag5)
												{
													this.Head.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												if (!flag5)
												{
													this.Head.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 66:
												{
													Transform transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
													transform3.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
													break;
												}
											case 67:
												{
													Transform transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
													transform4.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
													break;
												}
											case 68:
												if (array206.Length >= 2)
												{
													this.ClavicleL1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												}
												break;
											case 69:
												this.ClavicleR1[l].eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 71:
												transform2.eulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 72:
												transform5.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 73:
												transform6.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 74:
												transform7.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 75:
												transform8.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 76:
												transform9.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 77:
												transform10.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 78:
												transform11.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 79:
												transform12.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 80:
												transform13.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 81:
												transform14.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 82:
												transform15.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 83:
												transform16.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 84:
												transform17.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 85:
												transform18.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 86:
												transform19.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 87:
												transform20.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 88:
												transform21.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 89:
												transform22.localEulerAngles = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 92:
												this.vIKMuneL[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												this.muneIKL[l] = true;
												break;
											case 93:
												this.vIKMuneLSub[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 94:
												this.vIKMuneR[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												this.muneIKR[l] = true;
												break;
											case 95:
												this.vIKMuneRSub[l] = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
											case 96:
												transform2.position = new Vector3(float.Parse(array206[0]), float.Parse(array206[1]), float.Parse(array206[2]));
												break;
										}
									}
								}
								maid.transform.localEulerAngles = localEulerAngles;
								maid.transform.position = position2;
							}
						}
					}
					if (this.loadPose[l] != "" && !this.isLoadPose[l])
					{
						IniKey iniKey3 = base.Preferences["pose"][this.loadPose[l]];
						if (iniKey3.Value != null && iniKey3.Value.ToString() != "" && iniKey3.Value.ToString() != "del")
						{
							this.isStop[l] = true;
						}
						this.isLoadPose[l] = true;
					}
				}
				if (this.loadScene > 0)
				{
					string path4 = string.Concat(new object[]
					{
						Path.GetFullPath(".\\"),
						"Mod\\MultipleMaidsScene\\",
						this.loadScene,
						".png"
					});
					string text20 = "";
					IniKey iniKey2 = null;
					if (File.Exists(path4))
					{
						FileStream fileStream = new FileStream(path4, FileMode.Open, FileAccess.Read);
						BinaryReader binaryReader = new BinaryReader(fileStream);
						byte[] array197 = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
						byte[] value2 = new byte[]
						{
							array197[36],
							array197[35],
							array197[34],
							array197[33]
						};
						int count = BitConverter.ToInt32(value2, 0) - 8;
						byte[] bytes = array197.Skip(49).Take(count).ToArray<byte>();
						text20 = Encoding.UTF8.GetString(bytes);
						text20 = MultipleMaids.StringFromBase64Comp(text20);
					}
					else
					{
						iniKey2 = base.Preferences["scene"]["s" + this.loadScene];
					}
					if (text20 == "" && (iniKey2.Value == null || !(iniKey2.Value.ToString() != "")))
					{
						this.loadScene = 0;
					}
					else
					{
						this.isScene = true;
						if (!this.kankyoLoadFlg)
						{
							string text16 = text20;
							if (text20 == "")
							{
								text16 = iniKey2.Value;
							}
							string[] array198 = text16.Split(new char[]
							{
								'_'
							});
							string[] array199 = array198[1].Split(new char[]
							{
								';'
							});
							int i;
							for (i = 0; i < array199.Length; i++)
							{
								if (this.maidCnt <= i)
								{
									break;
								}
								Maid maid = this.maidArray[i];
								for (int k = 0; k < 10; k++)
								{
									maid.transform.RotateAround(maid.transform.position, Vector3.right, -maid.transform.rotation.eulerAngles.x);
									maid.transform.RotateAround(maid.transform.position, Vector3.forward, -maid.transform.rotation.eulerAngles.z);
								}
								string[] array2 = array199[i].Split(new char[]
								{
									':'
								});
								if (!int.TryParse(array2[61], out this.poseIndex[i]))
								{
									string a2 = array2[61].Replace(" ", "_").Replace("|", ",");
									for (int i2 = 0; i2 < this.poseArray.Length; i2++)
									{
										if (a2 == this.poseArray[i2])
										{
											this.poseIndex[i] = i2;
											break;
										}
									}
								}
								string[] array206 = this.poseArray[this.poseIndex[i]].Split(new char[]
								{
									','
								});
								this.isStop[i] = true;
								this.poseCount[i] = 20;
								int num207;
								if (array2[61].Contains("MultipleMaidsPose"))
								{
									string url = "";
									Action<string, List<string>> action2 = delegate (string path, List<string> result_list)
									{
										string[] files = Directory.GetFiles(path);
										for (int num208 = 0; num208 < files.Length; num208++)
										{
											if (Path.GetExtension(files[num208]) == ".anm")
											{
												string text21 = files[num208].Split(new char[]
												{
													'\\'
												})[files[num208].Split(new char[]
												{
													'\\'
												}).Length - 1];
												text21 = text21.Split(new char[]
												{
													'.'
												})[0];
												if (text21 == array2[61].Replace("MultipleMaidsPose", ""))
												{
													url = files[num208];
													for (int num209 = 0; num209 < this.poseArray.Length; num209++)
													{
														string b = this.poseArray[num209].Replace("\u3000", "").Split(new char[]
														{
															'/'
														})[0];
														if (text21 == b)
														{
															this.poseIndex[i] = num209;
														}
													}
													break;
												}
											}
										}
									};
									List<string> arg = new List<string>();
									action2(Path.GetFullPath(".\\") + "Mod\\MultipleMaidsPose", arg);
									if (url != "")
									{
										string path3 = url;
										byte[] array210 = new byte[0];
										try
										{
											using (FileStream fileStream = new FileStream(path3, FileMode.Open, FileAccess.Read))
											{
												array210 = new byte[fileStream.Length];
												fileStream.Read(array210, 0, array210.Length);
											}
										}
										catch
										{
										}
										if (0 < array210.Length)
										{
											string fileName = Path.GetFileName(path3);
											long num7 = (long)fileName.GetHashCode();
											maid.body0.CrossFade(num7.ToString(), array210, false, false, false, 0f, 1f);
											Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
											{
												default(Maid.AutoTwist),
												Maid.AutoTwist.ShoulderR,
												Maid.AutoTwist.WristL,
												Maid.AutoTwist.WristR,
												Maid.AutoTwist.ThighL,
												Maid.AutoTwist.ThighR
											};
											for (int n = 0; n < array3.Length; n++)
											{
												maid.SetAutoTwist(array3[n], true);
											}
										}
									}
								}
								else if (array206[0].StartsWith("p") && int.TryParse(array206[0].Substring(1), out num207))
								{
									this.loadPose[i] = array206[0];
								}
								else if (!array206[0].StartsWith("dance_"))
								{
									this.maidArray[i].CrossFade(array206[0] + ".anm", false, true, false, 0f, 1f);
								}
								else
								{
									if (!this.maidArray[i].body0.m_Bones.GetComponent<Animation>().GetClip(array206[0] + ".anm"))
									{
										this.maidArray[i].body0.LoadAnime(array206[0] + ".anm", GameUty.FileSystem, array206[0] + ".anm", false, false);
									}
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>().Play(array206[0] + ".anm");
								}
								if (array206.Length > 1)
								{
									this.maidArray[i].body0.m_Bones.GetComponent<Animation>()[array206[0] + ".anm"].time = float.Parse(array206[1]);
									this.isStop[i] = true;
									if (array206.Length > 2)
									{
										Transform transform29 = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
										this.isPoseIti[i] = true;
										this.poseIti[i] = this.maidArray[i].transform.position;
										this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
									}
								}
								this.faceIndex[i] = int.Parse(array2[62]);
								if (this.faceIndex[i] < this.faceArray.Length)
								{
									maid.FaceAnime(this.faceArray[this.faceIndex[i]], 0.01f, 0);
								}
								else
								{
									this.faceIndex[i] = 0;
								}
								TMorph morph = maid.body0.Face.morph;
								if (!this.isVR)
								{
									maid.boMabataki = false;
									morph.EyeMabataki = 0f;
									maid.body0.Face.morph.FixBlendValues_Face();
								}
							}
						}
					}
				}
				if (this.allowUpdate || this.sceneLevel == 14 || this.sceneLevel == 24)
				{
					if (this.isF6 && !this.cameraObj && !this.isVR)
					{
						this.cameraObj = new GameObject("subCamera");
						this.subcamera = this.cameraObj.AddComponent<Camera>();
						this.subcamera.CopyFrom(Camera.main);
						this.cameraObj.SetActive(true);
						this.subcamera.clearFlags = CameraClearFlags.Depth;
						this.subcamera.cullingMask = 256;
						this.subcamera.depth = 1f;
						this.subcamera.transform.parent = this.mainCamera.transform;
						float item = 2f;
						if (Application.unityVersion.StartsWith("4"))
						{
							item = 1f;
						}
						GameObject gameObject6 = new GameObject("Light");
						gameObject6.AddComponent<Light>();
						this.lightList.Add(gameObject6);
						this.lightColorR.Add(1f);
						this.lightColorG.Add(1f);
						this.lightColorB.Add(1f);
						this.lightIndex.Add(0);
						this.lightX.Add(40f);
						this.lightY.Add(180f);
						this.lightAkarusa.Add(item);
						this.lightKage.Add(0.098f);
						this.lightRange.Add(50f);
						gameObject6.transform.position = GameMain.Instance.MainLight.transform.position;
						gameObject6.GetComponent<Light>().intensity = 2f;
						gameObject6.GetComponent<Light>().spotAngle = 50f;
						gameObject6.GetComponent<Light>().range = 10f;
						gameObject6.GetComponent<Light>().type = LightType.Directional;
						gameObject6.GetComponent<Light>().color = new Color(0.5f, 1f, 0f);
						gameObject6.GetComponent<Light>().cullingMask = 256;
					}
					if (!this.isF6S || !this.getModKeyPressing(MultipleMaids.modKey.Shift) || !Input.GetKeyDown(KeyCode.F6))
					{
						if (this.isF6S || !this.getModKeyPressing(MultipleMaids.modKey.Shift) || !Input.GetKeyDown(KeyCode.F6))
						{
							if (!this.isF6 && Input.GetKeyDown(KeyCode.F6) && this.sceneLevel != 5 && this.sceneLevel != 3 && !this.isVR && this.maidArray[0] && this.maidArray[0].Visible)
							{
								this.isF6 = true;
								this.bGui = true;
								this.isFaceInit = true;
								this.isGuiInit = true;
								this.maidArray[0].boMabataki = false;
								this.selectMaidIndex = 0;
								this.maidCnt = 1;
								this.isFace[0] = true;
								this.faceFlg = true;
								this.kankyoFlg = false;
								string text2 = GameMain.Instance.BgMgr.GetBGName();
								int l = 0;
								foreach (string text3 in this.bgArray)
								{
									if (text3 == text2)
									{
										this.bgIndex = l;
										this.bgIndex6 = l;
										break;
									}
									l++;
								}
								this.bgCombo.selectedItemIndex = this.bgIndex6;
								this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
								this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
							}
							else if (!this.isF6 && Input.GetKeyDown(KeyCode.F7) && this.sceneLevel != 5 && this.sceneLevel != 3 && !this.isVR && this.maidArray[0] && this.maidArray[0].Visible)
							{
								this.isF6 = true;
								this.bGui = true;
								this.isGuiInit = true;
								this.selectMaidIndex = 0;
								this.maidCnt = 1;
								this.isFace[0] = false;
								this.faceFlg = false;
								this.kankyoFlg = true;
								string text2 = GameMain.Instance.BgMgr.GetBGName();
								int l = 0;
								foreach (string text3 in this.bgArray)
								{
									if (text3 == text2)
									{
										this.bgIndex = l;
										this.bgIndex6 = l;
										break;
									}
									l++;
								}
								this.bgmCombo.selectedItemIndex = this.bgmIndex;
								this.bgCombo.selectedItemIndex = this.bgIndex6;
								this.lightX[0] = GameMain.Instance.MainLight.transform.eulerAngles.x;
								this.lightY[0] = GameMain.Instance.MainLight.transform.eulerAngles.y;
								this.lightX6 = this.lightX[0];
								this.lightY6 = this.lightY[0];
							}
							else if ((this.isVR && Input.GetKey(KeyCode.F7) && Input.GetKeyDown(KeyCode.BackQuote)) || (this.isF6 && (Input.GetKeyDown(KeyCode.F6) || Input.GetKeyDown(KeyCode.F7))))
							{
								this.isF6 = false;
								this.bGui = false;
								this.maidArray[0].boMabataki = true;
								this.bgIndex = this.bgIndex6;
								this.bg.localScale = new Vector3(1f, 1f, 1f);
								if (!this.isVR)
								{
									Bloom fieldValue7 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
									fieldValue7.enabled = true;
									fieldValue7.bloomIntensity = 2.85f;
									fieldValue7.hdr = 0;
									fieldValue7.bloomThreshholdColor = new Color(1f, 1f, 1f);
									fieldValue7.bloomBlurIterations = 3;
								}
								else if (this.bgArray[this.bgIndex].Length == 36)
								{
									GameMain.Instance.BgMgr.ChangeBgMyRoom(this.bgArray[this.bgIndex]);
								}
								else
								{
									GameMain.Instance.BgMgr.ChangeBg(this.bgArray[this.bgIndex]);
								}
								this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f);
								this.maidCnt = 0;
								this.wearIndex = 0;
								this.faceFlg = false;
								this.faceFlg2 = false;
								this.sceneFlg = false;
								this.poseFlg = false;
								this.kankyoFlg = false;
								this.kankyo2Flg = false;
								this.unLockFlg = false;
								this.ikMaid = 0;
								this.ikBui = 0;
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
								this.isToothoff = false;
								this.isNosefook = false;
								this.isFaceInit = false;
								this.isPoseInit = false;
								this.isWear = true;
								this.isSkirt = true;
								this.isBra = true;
								this.isPanz = true;
								this.isHeadset = true;
								this.isAccUde = true;
								this.isStkg = true;
								this.isShoes = true;
								this.isGlove = true;
								this.isMegane = true;
								this.isAccSenaka = true;
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
								this.isBloomS = true;
								this.isDepthS = false;
								this.isBlurS = false;
								this.isFogS = false;
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
								this.effectIndex = 0;
								this.selectMaidIndex = 0;
								this.copyIndex = 0;
								this.selectLightIndex = 0;
								this.parIndex = 0;
								this.isEditNo = 0;
								this.editSelectMaid = null;
								for (int i2 = 0; i2 < 10; i2++)
								{
									this.date[i2] = "";
									this.ninzu[i2] = "";
								}
								if (this.kami)
								{
									this.kami.SetActive(false);
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
								this.lightX.Add(this.lightX6);
								this.lightY = new List<float>();
								this.lightY.Add(this.lightY6);
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
								GameMain.Instance.MainLight.gameObject.transform.position = new Vector3(0f, 2f, 0f);
								GameMain.Instance.MainLight.GetComponent<Light>().type = LightType.Directional;
								GameMain.Instance.MainLight.transform.eulerAngles = new Vector3(this.lightX6, this.lightY6, GameMain.Instance.MainLight.transform.eulerAngles.z);
								for (int l = 0; l < this.doguBObject.Count; l++)
								{
									UnityEngine.Object.Destroy(this.doguBObject[l]);
								}
								this.doguBObject.Clear();
								this.parIndex = 0;
								for (int l = 0; l < this.doguCombo.Length; l++)
								{
									this.doguCombo[l] = new ComboBox2();
									this.doguCombo[l].selectedItemIndex = 0;
								}
								this.parCombo.selectedItemIndex = 0;
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
								GameMain.Instance.BgMgr.ChangeBg(this.bgArray[this.bgIndex6]);
								this.bgCombo.selectedItemIndex = this.bgIndex6;
								this.bgCombo2.selectedItemIndex = 0;
								this.itemCombo2.selectedItemIndex = 0;
								this.myCombo.selectedItemIndex = 0;
								this.slotCombo.selectedItemIndex = 0;
								this.sortList.Clear();
								this.scrollPos = new Vector2(0f, 0f);
								if (!this.isVR)
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
								for (int i2 = 0; i2 < this.doguObject.Count; i2++)
								{
									if (this.doguObject[i2] != null)
									{
										UnityEngine.Object.Destroy(this.doguObject[i2]);
										this.doguObject[i2] = null;
									}
								}
								this.doguObject.Clear();
							}
						}
					}
					for (int l = 0; l < this.maidCnt; l++)
					{
						if (this.maidArray[l] && this.maidArray[l].Visible)
						{
							Maid maid = this.maidArray[l];
							if (this.isLook[l] != this.isLookn[l])
							{
								this.isLookn[l] = this.isLook[l];
								if (this.isLook[l])
								{
									maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
									maid.body0.boHeadToCam = true;
									maid.body0.boEyeToCam = true;
									maid.body0.trsLookTarget = null;
								}
								else
								{
									maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
									maid.body0.boHeadToCam = true;
									maid.body0.boEyeToCam = true;
								}
							}
						}
					}
					if (this.maidArray[this.selectMaidIndex] && this.maidArray[this.selectMaidIndex].Visible)
					{
						if ((!this.faceFlg && !this.poseFlg && !this.sceneFlg && !this.kankyoFlg && !this.kankyo2Flg) || this.sceneFlg || this.kankyoFlg || this.kankyo2Flg)
						{
							for (int k = 0; k < this.maidCnt; k++)
							{
								if (this.maidArray[k] && !this.maidArray[k].boMabataki)
								{
									this.maidArray[k].body0.Face.morph.FixBlendValues_Face();
								}
							}
						}
					}
					if (this.maidArray[this.selectMaidIndex] && this.maidArray[this.selectMaidIndex].Visible && this.poseFlg)
					{
						if (this.isPoseInit)
						{
							if (!this.isDanceChu)
							{
								Maid maid = this.maidArray[this.selectMaidIndex];
								this.isPoseInit = false;
								if (maid.body0.GetMask(TBody.SlotID.wear) || maid.body0.GetMask(TBody.SlotID.mizugi) || maid.body0.GetMask(TBody.SlotID.onepiece))
								{
									this.isWear = true;
								}
								else
								{
									this.isWear = false;
								}
								this.isSkirt = maid.body0.GetMask(TBody.SlotID.skirt);
								this.isBra = maid.body0.GetMask(TBody.SlotID.bra);
								this.isPanz = maid.body0.GetMask(TBody.SlotID.panz);
								this.isMaid = maid.body0.GetMask(TBody.SlotID.body);
								if (maid.body0.GetMask(TBody.SlotID.headset) || maid.body0.GetMask(TBody.SlotID.accHat) || maid.body0.GetMask(TBody.SlotID.accHead) || maid.body0.GetMask(TBody.SlotID.accKamiSubL) || maid.body0.GetMask(TBody.SlotID.accKamiSubR) || maid.body0.GetMask(TBody.SlotID.accKami_1_) || maid.body0.GetMask(TBody.SlotID.accKami_2_) || maid.body0.GetMask(TBody.SlotID.accKami_3_))
								{
									this.isHeadset = true;
								}
								else
								{
									this.isHeadset = false;
								}
								this.isAccUde = maid.body0.GetMask(TBody.SlotID.accUde);
								this.isStkg = maid.body0.GetMask(TBody.SlotID.stkg);
								this.isShoes = maid.body0.GetMask(TBody.SlotID.shoes);
								this.isGlove = maid.body0.GetMask(TBody.SlotID.glove);
								this.isMegane = maid.body0.GetMask(TBody.SlotID.megane);
								this.isAccSenaka = maid.body0.GetMask(TBody.SlotID.accSenaka);
								TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
								float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
								float[] fieldValue5 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
								if (morph.bodyskin.PartsVersion < 120)
								{
									this.eyeclose = fieldValue5[(int)morph.hash["eyeclose"]];
									this.eyeclose2 = fieldValue5[(int)morph.hash["eyeclose2"]];
									this.eyeclose3 = fieldValue5[(int)morph.hash["eyeclose3"]];
									this.eyeclose6 = fieldValue5[(int)morph.hash["eyeclose6"]];
									this.eyeclose5 = fieldValue5[(int)morph.hash["eyeclose5"]];
									if (morph.hash["eyeclose7"] != null)
									{
										this.eyeclose7 = fieldValue5[(int)morph.hash["eyeclose7"]];
										this.eyeclose8 = fieldValue5[(int)morph.hash["eyeclose8"]];
									}
								}
								else
								{
									int num102 = 0;
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
									{
										num102 = 1;
									}
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
									{
										num102 = 2;
									}
									this.eyeclose = fieldValue5[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose2 = fieldValue5[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose3 = fieldValue[(int)morph.hash["eyeclose3"]];
									this.eyeclose6 = fieldValue5[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose5 = fieldValue5[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose8 = fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose7 = fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]];
								}
								this.hitomih = fieldValue[(int)morph.hash["hitomih"]];
								this.hitomis = fieldValue[(int)morph.hash["hitomis"]];
								this.mayuha = fieldValue[(int)morph.hash["mayuha"]];
								this.mayuup = fieldValue[(int)morph.hash["mayuup"]];
								this.mayuv = fieldValue[(int)morph.hash["mayuv"]];
								this.mayuvhalf = fieldValue[(int)morph.hash["mayuvhalf"]];
								this.moutha = fieldValue[(int)morph.hash["moutha"]];
								this.mouths = fieldValue[(int)morph.hash["mouths"]];
								this.mouthdw = fieldValue[(int)morph.hash["mouthdw"]];
								this.mouthup = fieldValue[(int)morph.hash["mouthup"]];
								this.tangout = fieldValue[(int)morph.hash["tangout"]];
								this.tangup = fieldValue[(int)morph.hash["tangup"]];
								this.eyebig = fieldValue[(int)morph.hash["eyebig"]];
								this.mayuw = fieldValue[(int)morph.hash["mayuw"]];
								this.mouthhe = fieldValue[(int)morph.hash["mouthhe"]];
								this.mouthc = fieldValue[(int)morph.hash["mouthc"]];
								this.mouthi = fieldValue[(int)morph.hash["mouthi"]];
								this.mouthuphalf = fieldValue[(int)morph.hash["mouthuphalf"]];
								try
								{
									this.tangopen = fieldValue[(int)morph.hash["tangopen"]];
								}
								catch
								{
								}
								if (fieldValue[(int)morph.hash["namida"]] > 0f)
								{
									this.isNamida = true;
								}
								else
								{
									this.isNamida = false;
								}
								if (fieldValue[(int)morph.hash["tear1"]] > 0f)
								{
									this.isTear1 = true;
								}
								else
								{
									this.isTear1 = false;
								}
								if (fieldValue[(int)morph.hash["tear2"]] > 0f)
								{
									this.isTear2 = true;
								}
								else
								{
									this.isTear2 = false;
								}
								if (fieldValue[(int)morph.hash["tear3"]] > 0f)
								{
									this.isTear3 = true;
								}
								else
								{
									this.isTear3 = false;
								}
								if (fieldValue[(int)morph.hash["shock"]] > 0f)
								{
									this.isShock = true;
								}
								else
								{
									this.isShock = false;
								}
								if (fieldValue[(int)morph.hash["yodare"]] > 0f)
								{
									this.isYodare = true;
								}
								else
								{
									this.isYodare = false;
								}
								if (fieldValue[(int)morph.hash["hoho"]] > 0f)
								{
									this.isHoho = true;
								}
								else
								{
									this.isHoho = false;
								}
								if (fieldValue[(int)morph.hash["hoho2"]] > 0f)
								{
									this.isHoho2 = true;
								}
								else
								{
									this.isHoho2 = false;
								}
								if (fieldValue[(int)morph.hash["hohos"]] > 0f)
								{
									this.isHohos = true;
								}
								else
								{
									this.isHohos = false;
								}
								if (fieldValue[(int)morph.hash["hohol"]] > 0f)
								{
									this.isHohol = true;
								}
								else
								{
									this.isHohol = false;
								}
								if (fieldValue[(int)morph.hash["toothoff"]] > 0f)
								{
									this.isToothoff = true;
								}
								else
								{
									this.isToothoff = false;
								}
								if (fieldValue[(int)morph.hash["nosefook"]] > 0f)
								{
									this.isNosefook = true;
								}
								else
								{
									this.isNosefook = false;
								}
							}
						}
						else
						{
							Maid maid = this.maidArray[this.selectMaidIndex];
							if (maid.body0.GetMask(TBody.SlotID.wear) != this.isWear)
							{
								maid.body0.SetMask(TBody.SlotID.wear, this.isWear);
							}
							if (maid.body0.GetMask(TBody.SlotID.mizugi) != this.isWear)
							{
								maid.body0.SetMask(TBody.SlotID.mizugi, this.isWear);
							}
							if (maid.body0.GetMask(TBody.SlotID.onepiece) != this.isWear)
							{
								maid.body0.SetMask(TBody.SlotID.onepiece, this.isWear);
							}
							if (maid.body0.GetMask(TBody.SlotID.skirt) != this.isSkirt)
							{
								maid.body0.SetMask(TBody.SlotID.skirt, this.isSkirt);
							}
							if (maid.body0.GetMask(TBody.SlotID.bra) != this.isBra)
							{
								maid.body0.SetMask(TBody.SlotID.bra, this.isBra);
							}
							if (maid.body0.GetMask(TBody.SlotID.panz) != this.isPanz)
							{
								maid.body0.SetMask(TBody.SlotID.panz, this.isPanz);
							}
							if (maid.body0.GetMask(TBody.SlotID.headset) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.headset, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accHat) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accHat, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accHead) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accHead, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accKamiSubL) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accKamiSubL, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accKamiSubR) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accKamiSubR, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accKami_1_) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accKami_1_, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accKami_2_) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accKami_2_, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accKami_3_) != this.isHeadset)
							{
								maid.body0.SetMask(TBody.SlotID.accKami_3_, this.isHeadset);
							}
							if (maid.body0.GetMask(TBody.SlotID.accUde) != this.isAccUde)
							{
								maid.body0.SetMask(TBody.SlotID.accUde, this.isAccUde);
							}
							if (maid.body0.GetMask(TBody.SlotID.stkg) != this.isStkg)
							{
								maid.body0.SetMask(TBody.SlotID.stkg, this.isStkg);
							}
							if (maid.body0.GetMask(TBody.SlotID.shoes) != this.isShoes)
							{
								maid.body0.SetMask(TBody.SlotID.shoes, this.isShoes);
							}
							if (maid.body0.GetMask(TBody.SlotID.glove) != this.isGlove)
							{
								maid.body0.SetMask(TBody.SlotID.glove, this.isGlove);
							}
							if (maid.body0.GetMask(TBody.SlotID.megane) != this.isMegane)
							{
								maid.body0.SetMask(TBody.SlotID.megane, this.isMegane);
							}
							if (maid.body0.GetMask(TBody.SlotID.accSenaka) != this.isAccSenaka)
							{
								maid.body0.SetMask(TBody.SlotID.accSenaka, this.isAccSenaka);
							}
							if (this.mekure1[this.selectMaidIndex] != this.mekure1n[this.selectMaidIndex])
							{
								this.mekure1n[this.selectMaidIndex] = this.mekure1[this.selectMaidIndex];
								if (this.mekure1[this.selectMaidIndex])
								{
									maid.ItemChangeTemp("skirt", "めくれスカート");
									maid.ItemChangeTemp("onepiece", "めくれスカート");
									this.mekure2[this.selectMaidIndex] = false;
									this.mekure2n[this.selectMaidIndex] = false;
								}
								else
								{
									this.ResetProp(maid, MPN.skirt);
									this.ResetProp(maid, MPN.onepiece);
								}
								maid.AllProcPropSeqStart();
							}
							if (this.mekure2[this.selectMaidIndex] != this.mekure2n[this.selectMaidIndex])
							{
								this.mekure2n[this.selectMaidIndex] = this.mekure2[this.selectMaidIndex];
								if (this.mekure2[this.selectMaidIndex])
								{
									maid.ItemChangeTemp("skirt", "めくれスカート後ろ");
									maid.ItemChangeTemp("onepiece", "めくれスカート後ろ");
									this.mekure1[this.selectMaidIndex] = false;
									this.mekure1n[this.selectMaidIndex] = false;
								}
								else
								{
									this.ResetProp(maid, MPN.skirt);
									this.ResetProp(maid, MPN.onepiece);
								}
								maid.AllProcPropSeqStart();
							}
							if (this.zurasi[this.selectMaidIndex] != this.zurasin[this.selectMaidIndex])
							{
								this.zurasin[this.selectMaidIndex] = this.zurasi[this.selectMaidIndex];
								if (this.zurasi[this.selectMaidIndex])
								{
									maid.ItemChangeTemp("panz", "パンツずらし");
									maid.ItemChangeTemp("mizugi", "パンツずらし");
								}
								else
								{
									this.ResetProp(maid, MPN.panz);
									this.ResetProp(maid, MPN.mizugi);
								}
								maid.AllProcPropSeqStart();
							}
							if (!this.isDanceChu)
							{
								if (maid.body0.GetMask(0) != this.isMaid)
								{
									Hashtable fieldValue4 = MultipleMaids.GetFieldValue<TBody, Hashtable>(maid.body0, "m_hFoceHide");
									fieldValue4[0] = this.isMaid;
									fieldValue4[1] = this.isMaid;
									fieldValue4[2] = this.isMaid;
									fieldValue4[3] = this.isMaid;
									fieldValue4[4] = this.isMaid;
									fieldValue4[5] = this.isMaid;
									fieldValue4[6] = this.isMaid;
									fieldValue4[18] = this.isMaid;
									fieldValue4[39] = this.isMaid;
									fieldValue4[56] = this.isMaid;
									fieldValue4[57] = this.isMaid;
									if (maid.body0.goSlot[19].m_strModelFileName.Contains("melala_body"))
									{
										fieldValue4[19] = this.isMaid;
									}
									maid.body0.FixMaskFlag();
									maid.body0.FixVisibleFlag(false);
								}
								if (this.isLook[this.selectMaidIndex] != this.isLookn[this.selectMaidIndex])
								{
									this.isLookn[this.selectMaidIndex] = this.isLook[this.selectMaidIndex];
									if (this.isLook[this.selectMaidIndex])
									{
										maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
										maid.body0.boHeadToCam = true;
										maid.body0.boEyeToCam = true;
										maid.body0.trsLookTarget = null;
									}
									else
									{
										maid.body0.trsLookTarget = GameMain.Instance.MainCamera.transform;
										maid.body0.boHeadToCam = true;
										maid.body0.boEyeToCam = true;
									}
								}
								if (this.isLook[this.selectMaidIndex])
								{
									if (maid.body0.offsetLookTarget.x != this.lookY[this.selectMaidIndex])
									{
										if (this.isLock[this.selectMaidIndex] && this.lookY[this.selectMaidIndex] < 0f)
										{
											maid.body0.offsetLookTarget = new Vector3(this.lookY[this.selectMaidIndex] * 0.6f, 1f, this.lookX[this.selectMaidIndex]);
										}
										else
										{
											maid.body0.offsetLookTarget = new Vector3(this.lookY[this.selectMaidIndex], 1f, this.lookX[this.selectMaidIndex]);
										}
									}
									if (this.lookX[this.selectMaidIndex] != this.lookXn[this.selectMaidIndex])
									{
										this.lookXn[this.selectMaidIndex] = this.lookX[this.selectMaidIndex];
										maid.body0.offsetLookTarget = new Vector3(this.lookY[this.selectMaidIndex], 1f, this.lookX[this.selectMaidIndex]);
									}
									if (this.lookY[this.selectMaidIndex] != this.lookYn[this.selectMaidIndex])
									{
										this.lookYn[this.selectMaidIndex] = this.lookY[this.selectMaidIndex];
										if (this.isLock[this.selectMaidIndex] && this.lookY[this.selectMaidIndex] < 0f)
										{
											maid.body0.offsetLookTarget = new Vector3(this.lookY[this.selectMaidIndex] * 0.6f, 1f, this.lookX[this.selectMaidIndex]);
										}
										else
										{
											maid.body0.offsetLookTarget = new Vector3(this.lookY[this.selectMaidIndex], 1f, this.lookX[this.selectMaidIndex]);
										}
									}
								}
								if (this.isHanten)
								{
									this.isHanten = false;
									this.SetHanten(maid, this.selectMaidIndex);
								}
								if (this.hanten[this.selectMaidIndex] != this.hantenn[this.selectMaidIndex])
								{
									this.hantenn[this.selectMaidIndex] = this.hanten[this.selectMaidIndex];
									this.isStop[this.selectMaidIndex] = true;
									this.isLock[this.selectMaidIndex] = true;
									this.isHanten = true;
								}
								if (this.voice1[this.selectMaidIndex] != this.voice1n[this.selectMaidIndex])
								{
									this.voice1n[this.selectMaidIndex] = this.voice1[this.selectMaidIndex];
									if (this.voice1[this.selectMaidIndex])
									{
										this.zFlg[this.selectMaidIndex] = true;
										this.xFlg[this.selectMaidIndex] = false;
										this.voice2[this.selectMaidIndex] = false;
										this.voice2n[this.selectMaidIndex] = false;
									}
									else
									{
										this.zFlg[this.selectMaidIndex] = false;
									}
								}
								if (this.voice2[this.selectMaidIndex] != this.voice2n[this.selectMaidIndex])
								{
									this.voice2n[this.selectMaidIndex] = this.voice2[this.selectMaidIndex];
									if (this.voice2[this.selectMaidIndex])
									{
										this.xFlg[this.selectMaidIndex] = true;
										this.zFlg[this.selectMaidIndex] = false;
										this.voice1[this.selectMaidIndex] = false;
										this.voice1n[this.selectMaidIndex] = false;
									}
									else
									{
										this.xFlg[this.selectMaidIndex] = false;
									}
								}
								for (int k = 0; k < this.maidCnt; k++)
								{
									if (!this.maidArray[k].boMabataki)
									{
										this.maidArray[k].body0.Face.morph.FixBlendValues_Face();
									}
								}
							}
						}
					}
					if (this.maidArray[this.selectMaidIndex] && this.maidArray[this.selectMaidIndex].Visible && (this.isF6 || (this.okFlg && this.faceFlg)))
					{
						if (this.isFaceInit)
						{
							if (!this.isDanceChu)
							{
								TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
								float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
								float[] fieldValue5 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
								Maid maid = this.maidArray[this.selectMaidIndex];
								this.maidArray[this.selectMaidIndex].boMabataki = false;
								morph.EyeMabataki = 0f;
								this.isFaceInit = false;
								this.maidArray[this.selectMaidIndex].body0.Face.morph.FixBlendValues_Face();
								if (morph.bodyskin.PartsVersion < 120)
								{
									this.eyeclose = fieldValue5[(int)morph.hash["eyeclose"]];
									this.eyeclose2 = fieldValue5[(int)morph.hash["eyeclose2"]];
									this.eyeclose3 = fieldValue5[(int)morph.hash["eyeclose3"]];
									this.eyeclose6 = fieldValue5[(int)morph.hash["eyeclose6"]];
									this.eyeclose5 = fieldValue5[(int)morph.hash["eyeclose5"]];
									if (morph.hash["eyeclose7"] != null)
									{
										this.eyeclose7 = fieldValue5[(int)morph.hash["eyeclose7"]];
										this.eyeclose8 = fieldValue5[(int)morph.hash["eyeclose8"]];
									}
								}
								else
								{
									int num102 = 0;
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
									{
										num102 = 1;
									}
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
									{
										num102 = 2;
									}
									this.eyeclose = fieldValue5[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose2 = fieldValue5[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose3 = fieldValue[(int)morph.hash["eyeclose3"]];
									this.eyeclose6 = fieldValue5[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose5 = fieldValue5[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose8 = fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]];
									this.eyeclose7 = fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]];
								}
								this.hitomih = fieldValue[(int)morph.hash["hitomih"]];
								this.hitomis = fieldValue[(int)morph.hash["hitomis"]];
								this.mayuha = fieldValue[(int)morph.hash["mayuha"]];
								this.mayuup = fieldValue[(int)morph.hash["mayuup"]];
								this.mayuv = fieldValue[(int)morph.hash["mayuv"]];
								this.mayuvhalf = fieldValue[(int)morph.hash["mayuvhalf"]];
								this.moutha = fieldValue[(int)morph.hash["moutha"]];
								this.mouths = fieldValue[(int)morph.hash["mouths"]];
								this.mouthdw = fieldValue[(int)morph.hash["mouthdw"]];
								this.mouthup = fieldValue[(int)morph.hash["mouthup"]];
								this.tangout = fieldValue[(int)morph.hash["tangout"]];
								this.tangup = fieldValue[(int)morph.hash["tangup"]];
								this.eyebig = fieldValue[(int)morph.hash["eyebig"]];
								this.mayuw = fieldValue[(int)morph.hash["mayuw"]];
								this.mouthhe = fieldValue[(int)morph.hash["mouthhe"]];
								this.mouthc = fieldValue[(int)morph.hash["mouthc"]];
								this.mouthi = fieldValue[(int)morph.hash["mouthi"]];
								this.mouthuphalf = fieldValue[(int)morph.hash["mouthuphalf"]];
								try
								{
									this.tangopen = fieldValue[(int)morph.hash["tangopen"]];
								}
								catch
								{
								}
								if (maid.body0.GetMask(TBody.SlotID.wear) || maid.body0.GetMask(TBody.SlotID.mizugi) || maid.body0.GetMask(TBody.SlotID.onepiece))
								{
									this.isWear = true;
								}
								else
								{
									this.isWear = false;
								}
								this.isSkirt = maid.body0.GetMask(TBody.SlotID.skirt);
								this.isBra = maid.body0.GetMask(TBody.SlotID.bra);
								this.isPanz = maid.body0.GetMask(TBody.SlotID.panz);
								if (maid.body0.GetMask(TBody.SlotID.headset) || maid.body0.GetMask(TBody.SlotID.accHat) || maid.body0.GetMask(TBody.SlotID.accHead) || maid.body0.GetMask(TBody.SlotID.accKamiSubL) || maid.body0.GetMask(TBody.SlotID.accKamiSubR) || maid.body0.GetMask(TBody.SlotID.accKami_1_) || maid.body0.GetMask(TBody.SlotID.accKami_2_) || maid.body0.GetMask(TBody.SlotID.accKami_3_))
								{
									this.isHeadset = true;
								}
								else
								{
									this.isHeadset = false;
								}
								this.isAccUde = maid.body0.GetMask(TBody.SlotID.accUde);
								this.isStkg = maid.body0.GetMask(TBody.SlotID.stkg);
								this.isShoes = maid.body0.GetMask(TBody.SlotID.shoes);
								this.isGlove = maid.body0.GetMask(TBody.SlotID.glove);
								this.isMegane = maid.body0.GetMask(TBody.SlotID.megane);
								this.isAccSenaka = maid.body0.GetMask(TBody.SlotID.accSenaka);
								if (fieldValue[(int)morph.hash["namida"]] > 0f)
								{
									this.isNamida = true;
								}
								else
								{
									this.isNamida = false;
								}
								if (fieldValue[(int)morph.hash["tear1"]] > 0f)
								{
									this.isTear1 = true;
								}
								else
								{
									this.isTear1 = false;
								}
								if (fieldValue[(int)morph.hash["tear2"]] > 0f)
								{
									this.isTear2 = true;
								}
								else
								{
									this.isTear2 = false;
								}
								if (fieldValue[(int)morph.hash["tear3"]] > 0f)
								{
									this.isTear3 = true;
								}
								else
								{
									this.isTear3 = false;
								}
								if (fieldValue[(int)morph.hash["shock"]] > 0f)
								{
									this.isShock = true;
								}
								else
								{
									this.isShock = false;
								}
								if (fieldValue[(int)morph.hash["yodare"]] > 0f)
								{
									this.isYodare = true;
								}
								else
								{
									this.isYodare = false;
								}
								if (fieldValue[(int)morph.hash["hoho"]] > 0f)
								{
									this.isHoho = true;
								}
								else
								{
									this.isHoho = false;
								}
								if (fieldValue[(int)morph.hash["hoho2"]] > 0f)
								{
									this.isHoho2 = true;
								}
								else
								{
									this.isHoho2 = false;
								}
								if (fieldValue[(int)morph.hash["hohos"]] > 0f)
								{
									this.isHohos = true;
								}
								else
								{
									this.isHohos = false;
								}
								if (fieldValue[(int)morph.hash["hohol"]] > 0f)
								{
									this.isHohol = true;
								}
								else
								{
									this.isHohol = false;
								}
								if (fieldValue[(int)morph.hash["toothoff"]] > 0f)
								{
									this.isToothoff = true;
								}
								else
								{
									this.isToothoff = false;
								}
								if (fieldValue[(int)morph.hash["nosefook"]] > 0f)
								{
									this.isNosefook = true;
								}
								else
								{
									this.isNosefook = false;
								}
							}
						}
						else
						{
							Maid maid = this.maidArray[this.selectMaidIndex];
							if (!this.yotogiFlg && this.sceneLevel != 5 && this.sceneLevel != 3)
							{
								if (maid.body0.GetMask(TBody.SlotID.wear) != this.isWear)
								{
									maid.body0.SetMask(TBody.SlotID.wear, this.isWear);
								}
								if (maid.body0.GetMask(TBody.SlotID.mizugi) != this.isWear)
								{
									maid.body0.SetMask(TBody.SlotID.mizugi, this.isWear);
								}
								if (maid.body0.GetMask(TBody.SlotID.onepiece) != this.isWear)
								{
									maid.body0.SetMask(TBody.SlotID.onepiece, this.isWear);
								}
								if (maid.body0.GetMask(TBody.SlotID.skirt) != this.isSkirt)
								{
									maid.body0.SetMask(TBody.SlotID.skirt, this.isSkirt);
								}
								if (maid.body0.GetMask(TBody.SlotID.bra) != this.isBra)
								{
									maid.body0.SetMask(TBody.SlotID.bra, this.isBra);
								}
								if (maid.body0.GetMask(TBody.SlotID.panz) != this.isPanz)
								{
									maid.body0.SetMask(TBody.SlotID.panz, this.isPanz);
								}
								if (maid.body0.GetMask(TBody.SlotID.headset) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.headset, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accHat) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accHat, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accHead) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accHead, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accKamiSubL) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accKamiSubL, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accKamiSubR) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accKamiSubR, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accKami_1_) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accKami_1_, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accKami_2_) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accKami_2_, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accKami_3_) != this.isHeadset)
								{
									maid.body0.SetMask(TBody.SlotID.accKami_3_, this.isHeadset);
								}
								if (maid.body0.GetMask(TBody.SlotID.accUde) != this.isAccUde)
								{
									maid.body0.SetMask(TBody.SlotID.accUde, this.isAccUde);
								}
								if (maid.body0.GetMask(TBody.SlotID.stkg) != this.isStkg)
								{
									maid.body0.SetMask(TBody.SlotID.stkg, this.isStkg);
								}
								if (maid.body0.GetMask(TBody.SlotID.shoes) != this.isShoes)
								{
									maid.body0.SetMask(TBody.SlotID.shoes, this.isShoes);
								}
								if (maid.body0.GetMask(TBody.SlotID.glove) != this.isGlove)
								{
									maid.body0.SetMask(TBody.SlotID.glove, this.isGlove);
								}
								if (maid.body0.GetMask(TBody.SlotID.megane) != this.isMegane)
								{
									maid.body0.SetMask(TBody.SlotID.megane, this.isMegane);
								}
								if (maid.body0.GetMask(TBody.SlotID.accSenaka) != this.isAccSenaka)
								{
									maid.body0.SetMask(TBody.SlotID.accSenaka, this.isAccSenaka);
								}
								if (this.mekure1[this.selectMaidIndex] != this.mekure1n[this.selectMaidIndex])
								{
									this.mekure1n[this.selectMaidIndex] = this.mekure1[this.selectMaidIndex];
									if (this.mekure1[this.selectMaidIndex])
									{
										maid.ItemChangeTemp("skirt", "めくれスカート");
										maid.ItemChangeTemp("onepiece", "めくれスカート");
										this.mekure2[this.selectMaidIndex] = false;
										this.mekure2n[this.selectMaidIndex] = false;
									}
									else
									{
										this.ResetProp(maid, MPN.skirt);
										this.ResetProp(maid, MPN.onepiece);
									}
									maid.AllProcPropSeqStart();
								}
								if (this.mekure2[this.selectMaidIndex] != this.mekure2n[this.selectMaidIndex])
								{
									this.mekure2n[this.selectMaidIndex] = this.mekure2[this.selectMaidIndex];
									if (this.mekure2[this.selectMaidIndex])
									{
										maid.ItemChangeTemp("skirt", "めくれスカート後ろ");
										maid.ItemChangeTemp("onepiece", "めくれスカート後ろ");
										this.mekure1[this.selectMaidIndex] = false;
										this.mekure1n[this.selectMaidIndex] = false;
									}
									else
									{
										this.ResetProp(maid, MPN.skirt);
										this.ResetProp(maid, MPN.onepiece);
									}
									maid.AllProcPropSeqStart();
								}
								if (this.zurasi[this.selectMaidIndex] != this.zurasin[this.selectMaidIndex])
								{
									this.zurasin[this.selectMaidIndex] = this.zurasi[this.selectMaidIndex];
									if (this.zurasi[this.selectMaidIndex])
									{
										maid.ItemChangeTemp("panz", "パンツずらし");
										maid.ItemChangeTemp("mizugi", "パンツずらし");
									}
									else
									{
										this.ResetProp(maid, MPN.panz);
										this.ResetProp(maid, MPN.mizugi);
									}
									maid.AllProcPropSeqStart();
								}
							}
							if (!this.isDanceChu)
							{
								TMorph morph = this.maidArray[this.selectMaidIndex].body0.Face.morph;
								float[] fieldValue = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValues");
								float[] fieldValue5 = MultipleMaids.GetFieldValue<TMorph, float[]>(morph, "BlendValuesBackup");
								if (morph.bodyskin.PartsVersion < 120)
								{
									fieldValue5[(int)morph.hash["eyeclose"]] = this.eyeclose;
									fieldValue5[(int)morph.hash["eyeclose2"]] = this.eyeclose2;
									if (this.eyeclose3 > 1f)
									{
										this.eyeclose3 = 1f;
									}
									fieldValue5[(int)morph.hash["eyeclose3"]] = this.eyeclose3;
									fieldValue5[(int)morph.hash["eyeclose6"]] = this.eyeclose6;
									fieldValue5[(int)morph.hash["eyeclose5"]] = this.eyeclose5;
									if (morph.hash["eyeclose7"] != null)
									{
										fieldValue5[(int)morph.hash["eyeclose7"]] = this.eyeclose7;
										fieldValue5[(int)morph.hash["eyeclose8"]] = this.eyeclose8;
									}
								}
								else
								{
									int num102 = 0;
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TARE)
									{
										num102 = 1;
									}
									if (morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.TSURI || morph.GetFaceTypeGP01FB() == TMorph.GP01FB_FACE_TYPE.MAX)
									{
										num102 = 2;
									}
									fieldValue5[(int)morph.hash["eyeclose1" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose;
									fieldValue5[(int)morph.hash["eyeclose2" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose2;
									fieldValue5[(int)morph.hash["eyeclose5" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose5;
									fieldValue5[(int)morph.hash["eyeclose6" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose6;
									fieldValue5[(int)morph.hash["eyeclose7" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose7;
									fieldValue5[(int)morph.hash["eyeclose8" + TMorph.crcFaceTypesStr[num102]]] = this.eyeclose8;
									fieldValue[(int)morph.hash["eyeclose3"]] = this.eyeclose3;
								}
								fieldValue[(int)morph.hash["hitomih"]] = this.hitomih;
								fieldValue[(int)morph.hash["hitomis"]] = this.hitomis;
								fieldValue[(int)morph.hash["mayuha"]] = this.mayuha;
								fieldValue[(int)morph.hash["mayuup"]] = this.mayuup;
								fieldValue[(int)morph.hash["mayuv"]] = this.mayuv;
								fieldValue[(int)morph.hash["mayuvhalf"]] = this.mayuvhalf;
								fieldValue[(int)morph.hash["tangout"]] = this.tangout;
								fieldValue[(int)morph.hash["tangup"]] = this.tangup;
								if (morph.bodyskin.PartsVersion < 120)
								{
									if (this.eyebig > 1f)
									{
										this.eyebig = 1f;
									}
								}
								fieldValue[(int)morph.hash["eyebig"]] = this.eyebig;
								fieldValue[(int)morph.hash["mayuw"]] = this.mayuw;
								try
								{
									fieldValue[(int)morph.hash["tangopen"]] = this.tangopen;
								}
								catch
								{
								}
								if (!this.isDanceChu)
								{
									fieldValue[(int)morph.hash["moutha"]] = this.moutha;
									fieldValue[(int)morph.hash["mouths"]] = this.mouths;
									fieldValue[(int)morph.hash["mouthdw"]] = this.mouthdw;
									fieldValue[(int)morph.hash["mouthup"]] = this.mouthup;
									fieldValue[(int)morph.hash["mouthhe"]] = this.mouthhe;
									fieldValue[(int)morph.hash["mouthc"]] = this.mouthc;
									fieldValue[(int)morph.hash["mouthi"]] = this.mouthi;
									fieldValue[(int)morph.hash["mouthuphalf"]] = this.mouthuphalf;
								}
								if (this.isNamida)
								{
									fieldValue[(int)morph.hash["namida"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["namida"]] = 0f;
								}
								if (this.isTear1)
								{
									fieldValue[(int)morph.hash["tear1"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["tear1"]] = 0f;
								}
								if (this.isTear2)
								{
									fieldValue[(int)morph.hash["tear2"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["tear2"]] = 0f;
								}
								if (this.isTear3)
								{
									fieldValue[(int)morph.hash["tear3"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["tear3"]] = 0f;
								}
								if (this.isShock)
								{
									fieldValue[(int)morph.hash["shock"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["shock"]] = 0f;
								}
								if (this.isYodare)
								{
									fieldValue[(int)morph.hash["yodare"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["yodare"]] = 0f;
								}
								if (this.isHoho)
								{
									fieldValue[(int)morph.hash["hoho"]] = 0.5f;
								}
								else
								{
									fieldValue[(int)morph.hash["hoho"]] = 0f;
								}
								if (this.isHoho2)
								{
									fieldValue[(int)morph.hash["hoho2"]] = 0.5f;
								}
								else
								{
									fieldValue[(int)morph.hash["hoho2"]] = 0f;
								}
								if (this.isHohos)
								{
									fieldValue[(int)morph.hash["hohos"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["hohos"]] = 0f;
								}
								if (this.isHohol)
								{
									fieldValue[(int)morph.hash["hohol"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["hohol"]] = 0f;
								}
								if (this.isToothoff)
								{
									fieldValue[(int)morph.hash["toothoff"]] = 1f;
								}
								else
								{
									fieldValue[(int)morph.hash["toothoff"]] = 0f;
								}
								if (this.isNosefook)
								{
									morph.boNoseFook = true;
								}
								else
								{
									morph.boNoseFook = false;
								}
								for (int k = 0; k < this.maidCnt; k++)
								{
									this.maidArray[k].body0.Face.morph.FixBlendValues_Face();
								}
							}
						}
					}
					if (this.isF6 && !this.okFlg && ((!this.escFlg && Input.GetKeyDown(KeyCode.Escape)) || Input.GetKeyDown(KeyCode.Tab)))
					{
						this.bGui = !this.bGui;
					}
					if (this.isF6 && this.maidArray[0] != null && this.maidArray[0].Visible)
					{
						int l;
						for (l = 0; l < 999; l++)
						{
							if (this.gDogu[l] != null)
							{
								this.gDogu[l].GetComponent<Renderer>().enabled = false;
								this.gDogu[l].SetActive(false);
								if (this.mDogu[l].del)
								{
									this.mDogu[l].del = false;
									UnityEngine.Object.Destroy(this.doguBObject[l]);
									this.doguBObject.RemoveAt(l);
								}
								else if (this.mDogu[l].copy)
								{
									this.mDogu[l].copy = false;
									GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.doguBObject[l]);
									gameObject3.transform.Translate(-0.3f, 0f, 0f);
									this.doguBObject.Add(gameObject3);
									gameObject3.name = this.doguBObject[l].name;
									this.doguCnt = this.doguBObject.Count - 1;
									this.gDogu[this.doguCnt] = GameObject.CreatePrimitive(PrimitiveType.Cube);
									this.gDogu[this.doguCnt].GetComponent<Renderer>().material = this.m_material;
									this.gDogu[this.doguCnt].layer = 8;
									this.gDogu[this.doguCnt].GetComponent<Renderer>().enabled = false;
									this.gDogu[this.doguCnt].SetActive(false);
									this.gDogu[this.doguCnt].transform.position = gameObject3.transform.position;
									this.mDogu[this.doguCnt] = this.gDogu[this.doguCnt].AddComponent<MouseDrag6>();
									this.mDogu[this.doguCnt].isScale = false;
									this.mDogu[this.doguCnt].obj = this.gDogu[this.doguCnt];
									this.mDogu[this.doguCnt].maid = gameObject3;
									this.mDogu[this.doguCnt].angles = gameObject3.transform.eulerAngles;
									this.gDogu[this.doguCnt].transform.localScale = new Vector3(this.cubeSize, this.cubeSize, this.cubeSize);
									this.mDogu[this.doguCnt].ido = 1;
								}
								else if (this.mDogu[l].count > 0)
								{
									this.mDogu[l].count--;
									if (this.doguBObject.Count > l && this.doguBObject[l] != null && this.doguBObject[l].name.StartsWith("Particle/p"))
									{
										if (this.mDogu[l].count == 1)
										{
											this.doguBObject[l].SetActive(false);
										}
										if (this.mDogu[l].count == 0)
										{
											this.doguBObject[l].SetActive(true);
											string text19 = this.doguBObject[l].name;
											if (text19 != null)
											{
												if (!(text19 == "Particle/pLineY"))
												{
													if (!(text19 == "Particle/pLineP02"))
													{
														if (!(text19 == "Particle/pLine_act2"))
														{
															if (text19 == "Particle/pHeart01")
															{
																this.mDogu[l].count = 77;
															}
														}
														else
														{
															this.mDogu[l].count = 90;
														}
													}
													else
													{
														this.mDogu[l].count = 115;
													}
												}
												else
												{
													this.mDogu[l].count = 180;
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
						l = 0;
						while (l < this.lightIndex.Count)
						{
							if (this.gLight[0] == null)
							{
								this.gLight[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
								Material material2 = new Material(Shader.Find("Transparent/Diffuse"));
								material2.color = new Color(0.5f, 0.5f, 1f, 0.8f);
								this.gLight[0].GetComponent<Renderer>().material = material2;
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
							if (this.gLight[l] != null)
							{
								if (!this.isCube4)
								{
									this.gLight[l].GetComponent<Renderer>().enabled = false;
									this.gLight[l].SetActive(false);
								}
								else if (this.lightList[l].GetComponent<Light>().type == LightType.Spot || this.lightList[l].GetComponent<Light>().type == LightType.Point)
								{
									if (this.ikMode2 > 0 && this.ikMode2 != 15)
									{
										this.gLight[l].GetComponent<Renderer>().enabled = true;
										this.gLight[l].SetActive(true);
									}
									else
									{
										this.gLight[l].GetComponent<Renderer>().enabled = false;
										this.gLight[l].SetActive(false);
										this.mLight[l].isAlt = false;
									}
									if (this.ikMode2 == 10 || this.ikMode2 == 11 || this.ikMode2 == 12)
									{
										this.gLight[l].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
										if (this.mLight[l].isAlt)
										{
											this.gLight[l].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1f, 0.5f);
										}
									}
									if (this.ikMode2 == 9 || this.ikMode2 == 14)
									{
										this.gLight[l].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
										this.mLight[l].Update();
									}
									if (this.ikMode2 == 13)
									{
										this.gLight[l].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f, 0.5f);
										this.mLight[l].Update();
									}
									if (this.ikMode2 == 13)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 13 && this.gLight[l])
										{
											this.mLight[l].ido = 15;
											this.mLight[l].reset = true;
										}
										else
										{
											if (this.lightList[l].transform.localScale.x == 1f)
											{
												this.lightList[l].transform.localScale = new Vector3(this.lightRange[l], this.lightRange[l], this.lightRange[l]);
											}
											this.lightRange[l] = this.lightList[l].transform.localScale.x;
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 15;
										}
									}
									else if (this.ikMode2 == 11)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 11 && this.gLight[l])
										{
											this.mLight[l].ido = 3;
											this.mLight[l].reset = true;
										}
										else
										{
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.lightX[l] = this.gLight[l].transform.eulerAngles.x;
											this.lightY[l] = this.gLight[l].transform.eulerAngles.y;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 3;
										}
									}
									else if (this.ikMode2 == 12)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 12 && this.gLight[l])
										{
											this.mLight[l].ido = 2;
											this.mLight[l].reset = true;
										}
										else
										{
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 2;
										}
									}
									else if (this.ikMode2 == 10)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 10 && this.gLight[l])
										{
											this.mLight[l].ido = 1;
											this.mLight[l].reset = true;
										}
										else
										{
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 1;
										}
									}
									else if (this.ikMode2 == 9)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 9 && this.gLight[l])
										{
											this.mLight[l].ido = 4;
											this.mLight[l].reset = true;
										}
										else
										{
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.lightX[l] = this.gLight[l].transform.eulerAngles.x;
											this.lightY[l] = this.gLight[l].transform.eulerAngles.y;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 4;
										}
									}
									else if (this.ikMode2 == 14)
									{
										if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 14 && this.gLight[l])
										{
											this.mLight[l].ido = 6;
											this.mLight[l].reset = true;
										}
										else
										{
											this.gLight[l].transform.position = this.lightList[l].transform.position;
											this.gLight[l].transform.eulerAngles = this.lightList[l].transform.eulerAngles;
											this.lightX[l] = this.gLight[l].transform.eulerAngles.x;
											this.lightY[l] = this.gLight[l].transform.eulerAngles.y;
											this.mLight[l].maid = this.lightList[l];
											this.mLight[l].ido = 6;
										}
									}
								}
							}
							//IL_159C0:
							l++;
							continue;
							//goto IL_159C0;
						}
						for (l = 0; l < this.doguBObject.Count; l++)
						{
							if (!this.isCube2)
							{
								this.gDogu[l].GetComponent<Renderer>().enabled = false;
								this.gDogu[l].SetActive(false);
							}
							else
							{
								if (this.ikMode2 > 0)
								{
									this.gDogu[l].GetComponent<Renderer>().enabled = true;
									this.gDogu[l].SetActive(true);
								}
								else
								{
									this.gDogu[l].GetComponent<Renderer>().enabled = false;
									this.gDogu[l].SetActive(false);
									this.mDogu[l].isAlt = false;
								}
								if (this.ikMode2 == 10 || this.ikMode2 == 11 || this.ikMode2 == 12)
								{
									this.gDogu[l].GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1f, 0.5f);
									if (this.mDogu[l].isAlt)
									{
										this.gDogu[l].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1f, 0.5f);
									}
								}
								if (this.ikMode2 == 9 || this.ikMode2 == 14)
								{
									this.gDogu[l].GetComponent<Renderer>().material.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
									this.mDogu[l].Update();
								}
								if (this.ikMode2 == 15)
								{
									this.gDogu[l].GetComponent<Renderer>().material.color = new Color(1f, 0.3f, 0.3f, 0.5f);
								}
								if (this.ikMode2 == 16)
								{
									this.gDogu[l].GetComponent<Renderer>().material.color = new Color(0.7f, 0.3f, 0.7f, 0.5f);
									this.mDogu[l].Update();
								}
								if (this.ikMode2 == 13)
								{
									this.gDogu[l].GetComponent<Renderer>().material.color = new Color(0.7f, 0.7f, 0.3f, 0.5f);
									this.mDogu[l].Update();
								}
								if (this.ikMode2 == 13)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 13 && this.gDogu[l])
									{
										this.mDogu[l].ido = 5;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 5;
									}
								}
								else if (this.ikMode2 == 11)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 11 && this.gDogu[l])
									{
										this.mDogu[l].ido = 3;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 3;
									}
								}
								else if (this.ikMode2 == 12)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 12 && this.gDogu[l])
									{
										this.mDogu[l].ido = 2;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 2;
									}
								}
								else if (this.ikMode2 == 10)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 10 && this.gDogu[l])
									{
										this.mDogu[l].ido = 1;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].maidArray = this.doguBObject.ToArray();
										this.mDogu[l].mArray = this.mDogu.ToArray<MouseDrag6>();
										this.mDogu[l].ido = 1;
									}
								}
								else if (this.ikMode2 == 9)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 9 && this.gDogu[l])
									{
										this.mDogu[l].ido = 4;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 4;
									}
								}
								else if (this.ikMode2 == 14)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 14 && this.gDogu[l])
									{
										this.mDogu[l].ido = 6;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 6;
									}
								}
								else if (this.ikMode2 == 15)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 15 && this.gDogu[l])
									{
										this.mDogu[l].ido = 7;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 7;
									}
								}
								else if (this.ikMode2 == 16)
								{
									if ((this.ikModeOld2 == 0 || this.ikModeOld2 >= 9) && this.ikModeOld2 != 16 && this.gDogu[l])
									{
										this.mDogu[l].ido = 8;
										this.mDogu[l].reset = true;
									}
									else
									{
										this.gDogu[l].transform.position = this.doguBObject[l].transform.position;
										this.gDogu[l].transform.eulerAngles = this.doguBObject[l].transform.eulerAngles;
										this.mDogu[l].maid = this.doguBObject[l];
										this.mDogu[l].ido = 8;
									}
								}
							}
						}
						this.ikModeOld2 = this.ikMode2;
						Vector3 vector2 = this.mainCameraTransform.TransformDirection(Vector3.forward);
						Vector3 vector3 = this.mainCameraTransform.TransformDirection(Vector3.right);
						Vector3 vector4 = this.mainCameraTransform.TransformDirection(Vector3.up);
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
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.J))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.I))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * -0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.K))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector2.x, 0f, vector2.z) * 0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.J))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * 0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.L))
						{
							Vector3 vector5 = this.bg.position;
							vector5 += new Vector3(vector3.x, 0f, vector3.z) * -0.025f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.Alpha0))
						{
							Vector3 vector5 = this.bg.position;
							vector5.y -= 0.015f * this.speed;
							this.bg.localPosition = vector5;
							if (Input.GetKey(KeyCode.Y))
							{
								this.keyFlg = true;
							}
						}
						else if ((Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.V)) && Input.GetKey(KeyCode.P))
						{
							Vector3 vector5 = this.bg.position;
							vector5.y += 0.015f * this.speed;
							this.bg.localPosition = vector5;
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
						else if (Input.GetKeyUp(KeyCode.Y))
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
								}
							}
						}
						if (this.getModKeyPressing(MultipleMaids.modKey.Shift))
						{
							this.speed = 5f * Time.deltaTime * 60f;
						}
						else
						{
							this.speed = 1f * Time.deltaTime * 60f;
						}
						if (!this.isVR || this.isVR2)
						{
							if (!this.isVR)
							{
								if (this.isBloom)
								{
									Bloom fieldValue7 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
									fieldValue7.enabled = true;
									fieldValue7.bloomIntensity = this.bloom1;
									fieldValue7.bloomBlurIterations = (int)this.bloom2;
									fieldValue7.bloomThreshholdColor = new Color(1f - this.bloom3, 1f - this.bloom4, 1f - this.bloom5);
									if (this.isBloomA)
									{
										fieldValue7.hdr = Bloom.HDRBloomMode.On;
									}
									else
									{
										fieldValue7.hdr = Bloom.HDRBloomMode.Auto;
									}
									this.isBloom2 = true;
								}
								else if (this.isBloom2)
								{
									this.isBloom2 = false;
									Bloom fieldValue7 = MultipleMaids.GetFieldValue<CameraMain, Bloom>(this.mainCamera, "m_gcBloom");
									fieldValue7.enabled = true;
									fieldValue7.bloomIntensity = 2.85f;
									fieldValue7.hdr = 0;
									fieldValue7.bloomThreshholdColor = new Color(1f, 1f, 1f);
									fieldValue7.bloomBlurIterations = 3;
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
								if (this.bokashi > 0f)
								{
									Blur component4 = GameMain.Instance.MainCamera.gameObject.GetComponent<Blur>();
									component4.enabled = true;
									component4.blurSize = this.bokashi / 10f;
									component4.blurIterations = 0;
									component4.downsample = 0;
									if (this.bokashi > 3f)
									{
										component4.blurSize -= 0.3f;
										component4.blurIterations = 1;
										component4.downsample = 1;
									}
								}
								else
								{
									Blur component4 = GameMain.Instance.MainCamera.gameObject.GetComponent<Blur>();
									component4.enabled = false;
								}
								if (this.kamiyure > 0f)
								{
									for (int i2 = 0; i2 < this.maidCnt; i2++)
									{
										Maid maid = this.maidArray[i2];
										for (int j = 0; j < maid.body0.goSlot.Count; j++)
										{
											if (maid.body0.goSlot[j].obj != null)
											{
												DynamicBone component = maid.body0.goSlot[j].obj.GetComponent<DynamicBone>();
												if (component != null && component.enabled)
												{
													component.m_Gravity = new Vector3(this.softG.x * 5f, (this.softG.y + 0.003f) * 5f, this.softG.z * 5f);
												}
											}
											List<THair1> fieldValue6 = MultipleMaids.GetFieldValue<TBoneHair_, List<THair1>>(maid.body0.goSlot[j].bonehair, "hair1list");
											for (int k = 0; k < fieldValue6.Count; k++)
											{
												fieldValue6[k].SoftG = new Vector3(this.softG.x, this.softG.y + this.kamiyure, this.softG.z);
											}
										}
									}
								}
								if (this.isBlur)
								{
									Vignetting component2 = GameMain.Instance.MainCamera.gameObject.GetComponent<Vignetting>();
									component2.mode = 0;
									component2.intensity = this.blur1;
									component2.chromaticAberration = this.blur4;
									component2.blur = this.blur2;
									component2.blurSpread = this.blur3;
									component2.enabled = true;
									this.isBlur2 = true;
								}
								else if (this.isBlur2)
								{
									this.isBlur2 = false;
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
								List<float> list7;
								(list7 = this.lightColorR)[0] = list7[0] + 0.01f;
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
								List<float> list7;
								(list7 = this.lightColorG)[0] = list7[0] + 0.01f;
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
								List<float> list7;
								(list7 = this.lightColorB)[0] = list7[0] + 0.01f;
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
								List<float> list7;
								(list7 = this.lightColorR)[0] = list7[0] - 0.01f;
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
								List<float> list7;
								(list7 = this.lightColorG)[0] = list7[0] - 0.01f;
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
								List<float> list7;
								(list7 = this.lightColorB)[0] = list7[0] - 0.01f;
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
								List<int> list6;
								(list6 = this.lightIndex)[0] = list6[0] + 1;
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
									if (this.gLight[0] == null)
									{
										this.gLight[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
										Material material2 = new Material(Shader.Find("Transparent/Diffuse"));
										material2.color = new Color(0.5f, 0.5f, 1f, 0.8f);
										this.gLight[0].GetComponent<Renderer>().material = material2;
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
							List<int> list6;
							(list6 = this.lightIndex)[0] = list6[0] + 1;
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
						for (l = 0; l < this.lightList.Count; l++)
						{
							if (l > 0)
							{
								this.lightList[l].GetComponent<Light>().color = new Color(this.lightColorR[l], this.lightColorG[l], this.lightColorB[l]);
								this.lightList[l].GetComponent<Light>().intensity = this.lightAkarusa[l];
								this.lightList[l].GetComponent<Light>().spotAngle = this.lightRange[l];
								this.lightList[l].GetComponent<Light>().range = this.lightRange[l] / 5f;
								if (!Input.GetKey(KeyCode.X) && (!Input.GetKey(KeyCode.Z) || !this.getModKeyPressing(MultipleMaids.modKey.Shift)))
								{
									this.lightList[l].transform.eulerAngles = new Vector3(this.lightX[l], this.lightY[l], 18f);
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
								GameMain.Instance.MainLight.GetComponent<Light>().spotAngle = this.lightRange[l];
								GameMain.Instance.MainLight.GetComponent<Light>().range = this.lightRange[l] / 5f;
								if (this.lightIndex[l] != 3)
								{
									GameMain.Instance.MainLight.GetComponent<Light>().color = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
								}
								else
								{
									this.mainCamera.GetComponent<Camera>().backgroundColor = new Color(this.lightColorR[0], this.lightColorG[0], this.lightColorB[0]);
								}
							}
						}
					}
				}
			}
		}
	}
}
