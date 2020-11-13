using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CM3D2.MultipleMaids.Plugin
{
    public partial class MultipleMaids
    {
		private void SetIKInit6(int k)
		{
			this.gHead2[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHead2[k].GetComponent<Renderer>().enabled = false;
			this.mHead2[k] = this.gHead2[k].AddComponent<MouseDrag5>();
			this.mHead2[k].obj = this.gHead2[k];
			this.gHead2[k].transform.localScale = new Vector3(0.2f, 0.24f, 0.2f);
			this.gHead2[k].layer = 8;
			this.gMaid2[k] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			this.gMaid2[k].GetComponent<Renderer>().enabled = false;
			this.mMaid2[k] = this.gMaid2[k].AddComponent<MouseDrag5>();
			this.mMaid2[k].obj = this.gMaid2[k];
			this.gMaid2[k].transform.localScale = new Vector3(0.2f, 0.3f, 0.24f);
			this.gMaid2[k].layer = 8;
		}

		private void SetIKInit7(int k)
		{
			this.gIKMuneL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gIKMuneL[k].GetComponent<Renderer>().enabled = false;
			this.mIKMuneL[k] = this.gIKMuneL[k].AddComponent<MouseDrag>();
			this.mIKMuneL[k].obj = this.gIKMuneL[k];
			this.gIKMuneL[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
			this.gIKMuneL[k].layer = 8;
			this.gIKMuneR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gIKMuneR[k].GetComponent<Renderer>().enabled = false;
			this.mIKMuneR[k] = this.gIKMuneR[k].AddComponent<MouseDrag>();
			this.mIKMuneR[k].obj = this.gIKMuneR[k];
			this.gIKMuneR[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
			this.gIKMuneR[k].layer = 8;
		}

		private void SetIKInit5(int k)
		{
			this.gIKHandL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gIKHandL[k].GetComponent<Renderer>().enabled = false;
			this.mIKHandL[k] = this.gIKHandL[k].AddComponent<MouseDrag4>();
			this.mIKHandL[k].obj = this.gIKHandL[k];
			this.gIKHandL[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
			this.gIKHandR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gIKHandR[k].GetComponent<Renderer>().enabled = false;
			this.mIKHandR[k] = this.gIKHandR[k].AddComponent<MouseDrag4>();
			this.mIKHandR[k].obj = this.gIKHandR[k];
			this.gIKHandR[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
		}

		private void SetIKInit4(int k)
		{
			this.gHead[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHead[k].GetComponent<Renderer>().enabled = false;
			this.mHead[k] = this.gHead[k].AddComponent<MouseDrag3>();
			this.mHead[k].obj = this.gHead[k];
			this.gHead[k].transform.localScale = new Vector3(0.2f, 0.24f, 0.2f);
			this.gJotai[k] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			this.gJotai[k].GetComponent<Renderer>().enabled = false;
			this.mJotai[k] = this.gJotai[k].AddComponent<MouseDrag3>();
			this.mJotai[k].obj = this.gJotai[k];
			this.gJotai[k].transform.localScale = new Vector3(0.2f, 0.19f, 0.24f);
			this.gJotai[k].layer = 8;
			this.gKahuku[k] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			this.gKahuku[k].GetComponent<Renderer>().enabled = false;
			this.mKahuku[k] = this.gKahuku[k].AddComponent<MouseDrag3>();
			this.mKahuku[k].obj = this.gKahuku[k];
			this.gKahuku[k].transform.localScale = new Vector3(0.2f, 0.15f, 0.24f);
			this.gKahuku[k].layer = 8;
		}

		private void SetIKInit3(int k)
		{
			this.gMaid[k] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			this.gMaid[k].GetComponent<Renderer>().enabled = false;
			this.mMaid[k] = this.gMaid[k].AddComponent<MouseDrag2>();
			this.mMaid[k].obj = this.gMaid[k];
			this.gMaid[k].transform.localScale = new Vector3(0.2f, 0.3f, 0.24f);
			this.gMaid[k].layer = 8;
			this.gMaidC[k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.gMaidC[k].GetComponent<Renderer>().enabled = true;
			this.mMaidC[k] = this.gMaidC[k].AddComponent<MouseDrag2>();
			this.mMaidC[k].obj = this.gMaidC[k];
			this.gMaidC[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
			Material material = new Material(Shader.Find("Transparent/Diffuse"));
			material.color = new Color(0.5f, 0.5f, 1f, 0.8f);
			this.gMaidC[k].GetComponent<Renderer>().material = material;
			this.gMaidC[k].layer = 8;
		}

		private void SetIKInit2(int k)
		{
			for (int i = 0; i < 30; i++)
			{
				this.gFinger[k, i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				if (i % 3 == 0)
				{
				}
				this.mFinger[k, i] = this.gFinger[k, i].AddComponent<MouseDrag>();
				this.mFinger[k, i].obj = this.gFinger[k, i];
				float num = 0f;
				switch (i)
				{
					case 0:
					case 15:
						num = 0.024f;
						break;
					case 1:
					case 16:
						num = 0.018f;
						break;
					case 2:
					case 17:
						num = 0.014f;
						break;
					case 3:
					case 6:
					case 9:
					case 18:
					case 21:
					case 24:
						num = 0.017f;
						break;
					case 4:
					case 7:
					case 10:
					case 19:
					case 22:
					case 25:
						num = 0.015f;
						break;
					case 5:
					case 8:
					case 11:
					case 20:
					case 23:
					case 26:
						num = 0.013f;
						break;
					case 12:
					case 27:
						num = 0.015f;
						break;
					case 13:
					case 28:
						num = 0.013f;
						break;
					case 14:
					case 29:
						num = 0.012f;
						break;
				}
				this.gFinger[k, i].transform.localScale = new Vector3(num, num, num);
				Material material = new Material(Shader.Find("Transparent/Diffuse"));
				this.gFinger[k, i].GetComponent<Renderer>().material = material;
				MeshRenderer component = this.gFinger[k, i].GetComponent<MeshRenderer>();
				component.material.color = new Color(0f, 0f, 1f, 0.2f);
			}
			for (int i = 0; i < 12; i++)
			{
				this.gFinger2[k, i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				this.mFinger2[k, i] = this.gFinger2[k, i].AddComponent<MouseDrag>();
				this.mFinger2[k, i].obj = this.gFinger2[k, i];
				float num = 0f;
				switch (i)
				{
					case 0:
					case 6:
						num = 0.017f;
						break;
					case 1:
					case 7:
						num = 0.016f;
						break;
					case 2:
					case 8:
						num = 0.024f;
						break;
					case 3:
					case 9:
						num = 0.017f;
						break;
					case 4:
					case 10:
						num = 0.024f;
						break;
					case 5:
					case 11:
						num = 0.018f;
						break;
				}
				this.gFinger2[k, i].transform.localScale = new Vector3(num, num, num);
				Material material = new Material(Shader.Find("Transparent/Diffuse"));
				this.gFinger2[k, i].GetComponent<Renderer>().material = material;
				MeshRenderer component = this.gFinger2[k, i].GetComponent<MeshRenderer>();
				component.material.color = new Color(0f, 0f, 1f, 0.2f);
			}
		}

		private void SetIKInit(int k)
		{
			Material material = new Material(Shader.Find("Transparent/Diffuse"));
			material.color = new Color(0.4f, 0.4f, 1f, 0.3f);
			this.m_material2 = new Material(Shader.Find("Transparent/Diffuse"));
			this.m_material2.color = new Color(0.4f, 0.4f, 1f, 0.26f);
			this.m_material3 = new Material(Shader.Find("Transparent/Diffuse"));
			this.m_material3.color = new Color(0.4f, 0.4f, 1f, 0.36f);
			this.gHandL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHandL[k].GetComponent<Renderer>().enabled = false;
			this.gHandL[k].layer = 8;
			this.mHandL[k] = this.gHandL[k].AddComponent<MouseDrag>();
			this.mHandL[k].obj = this.gHandL[k];
			this.gArmL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gArmL[k].GetComponent<Renderer>().enabled = false;
			this.gArmL[k].layer = 8;
			this.mArmL[k] = this.gArmL[k].AddComponent<MouseDrag>();
			this.mArmL[k].obj = this.gArmL[k];
			this.gFootL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gFootL[k].GetComponent<Renderer>().enabled = false;
			this.gFootL[k].layer = 8;
			this.mFootL[k] = this.gFootL[k].AddComponent<MouseDrag>();
			this.mFootL[k].obj = this.gFootL[k];
			this.gHizaL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHizaL[k].GetComponent<Renderer>().enabled = false;
			this.gHizaL[k].layer = 8;
			this.mHizaL[k] = this.gHizaL[k].AddComponent<MouseDrag>();
			this.mHizaL[k].obj = this.gHizaL[k];
			this.gHandR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHandR[k].GetComponent<Renderer>().enabled = false;
			this.gHandR[k].layer = 8;
			this.mHandR[k] = this.gHandR[k].AddComponent<MouseDrag>();
			this.mHandR[k].obj = this.gHandR[k];
			this.gArmR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gArmR[k].GetComponent<Renderer>().enabled = false;
			this.gArmR[k].layer = 8;
			this.mArmR[k] = this.gArmR[k].AddComponent<MouseDrag>();
			this.mArmR[k].obj = this.gArmR[k];
			this.gFootR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gFootR[k].GetComponent<Renderer>().enabled = false;
			this.gFootR[k].layer = 8;
			this.mFootR[k] = this.gFootR[k].AddComponent<MouseDrag>();
			this.mFootR[k].obj = this.gFootR[k];
			this.gHizaR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gHizaR[k].GetComponent<Renderer>().enabled = false;
			this.gHizaR[k].layer = 8;
			this.mHizaR[k] = this.gHizaR[k].AddComponent<MouseDrag>();
			this.mHizaR[k].obj = this.gHizaR[k];
			this.gClavicleL[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gClavicleL[k].GetComponent<Renderer>().enabled = false;
			this.gClavicleL[k].layer = 8;
			this.mClavicleL[k] = this.gClavicleL[k].AddComponent<MouseDrag>();
			this.mClavicleL[k].obj = this.gClavicleL[k];
			this.gClavicleR[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gClavicleR[k].GetComponent<Renderer>().enabled = false;
			this.gClavicleR[k].layer = 8;
			this.mClavicleR[k] = this.gClavicleR[k].AddComponent<MouseDrag>();
			this.mClavicleR[k].obj = this.gClavicleR[k];
			if (!this.isBone[k])
			{
				this.gHandL[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
				this.gHandR[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
				this.gArmL[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
				this.gArmR[k].transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
				this.gFootL[k].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
				this.gFootR[k].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
				this.gHizaL[k].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
				this.gHizaR[k].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
				this.gClavicleL[k].transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
				this.mClavicleR[k].transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
			}
			else
			{
				this.gHandL[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gHandR[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gArmL[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gArmR[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gFootL[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gFootR[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gHizaL[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gHizaR[k].transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
				this.gClavicleL[k].transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
				this.gClavicleR[k].transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
				this.gHandL[k].GetComponent<Renderer>().enabled = true;
				this.gHandR[k].GetComponent<Renderer>().enabled = true;
				this.gArmL[k].GetComponent<Renderer>().enabled = true;
				this.gArmR[k].GetComponent<Renderer>().enabled = true;
				this.gFootL[k].GetComponent<Renderer>().enabled = true;
				this.gFootR[k].GetComponent<Renderer>().enabled = true;
				this.gHizaL[k].GetComponent<Renderer>().enabled = true;
				this.gHizaR[k].GetComponent<Renderer>().enabled = true;
				this.gClavicleL[k].GetComponent<Renderer>().enabled = true;
				this.gClavicleR[k].GetComponent<Renderer>().enabled = true;
				this.gHandL[k].GetComponent<Renderer>().material = this.m_material2;
				this.gHandR[k].GetComponent<Renderer>().material = this.m_material2;
				this.gArmL[k].GetComponent<Renderer>().material = this.m_material2;
				this.gArmR[k].GetComponent<Renderer>().material = this.m_material2;
				this.gFootL[k].GetComponent<Renderer>().material = this.m_material2;
				this.gFootR[k].GetComponent<Renderer>().material = this.m_material2;
				this.gHizaL[k].GetComponent<Renderer>().material = this.m_material2;
				this.gHizaR[k].GetComponent<Renderer>().material = this.m_material2;
				this.gClavicleL[k].GetComponent<Renderer>().material = this.m_material2;
				this.gClavicleR[k].GetComponent<Renderer>().material = this.m_material2;
			}
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(base.transform, false);
			this.gizmoHandL[k] = gameObject.AddComponent<GizmoRender>();
			this.gizmoHandL[k].eRotate = true;
			this.gizmoHandL[k].offsetScale = 0.25f;
			this.gizmoHandL[k].lineRSelectedThick = 0.25f;
			this.gizmoHandL[k].Visible = false;
			GameObject gameObject2 = new GameObject();
			gameObject2.transform.SetParent(base.transform, false);
			this.gizmoHandR[k] = gameObject2.AddComponent<GizmoRender>();
			this.gizmoHandR[k].eRotate = true;
			this.gizmoHandR[k].offsetScale = 0.25f;
			this.gizmoHandR[k].lineRSelectedThick = 0.25f;
			this.gizmoHandR[k].Visible = false;
			GameObject gameObject3 = new GameObject();
			gameObject3.transform.SetParent(base.transform, false);
			this.gizmoFootL[k] = gameObject3.AddComponent<GizmoRender>();
			this.gizmoFootL[k].eRotate = true;
			this.gizmoFootL[k].offsetScale = 0.25f;
			this.gizmoFootL[k].lineRSelectedThick = 0.25f;
			this.gizmoFootL[k].Visible = false;
			GameObject gameObject4 = new GameObject();
			gameObject4.transform.SetParent(base.transform, false);
			this.gizmoFootR[k] = gameObject4.AddComponent<GizmoRender>();
			this.gizmoFootR[k].eRotate = true;
			this.gizmoFootR[k].offsetScale = 0.25f;
			this.gizmoFootR[k].lineRSelectedThick = 0.25f;
			this.gizmoFootR[k].Visible = false;
			this.gNeck[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gNeck[k].GetComponent<Renderer>().material = material;
			this.gNeck[k].layer = 8;
			this.mNeck[k] = this.gNeck[k].AddComponent<MouseDrag3>();
			this.mNeck[k].obj = this.gNeck[k];
			this.gNeck[k].transform.localScale = new Vector3(0.055f, 0.055f, 0.055f);
			this.gNeck[k].SetActive(false);
			this.gSpine[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gSpine[k].GetComponent<Renderer>().material = material;
			this.gSpine[k].layer = 8;
			this.mSpine[k] = this.gSpine[k].AddComponent<MouseDrag3>();
			this.mSpine[k].obj = this.gSpine[k];
			this.gSpine[k].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
			this.gSpine[k].SetActive(false);
			this.gSpine0a[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gSpine0a[k].GetComponent<Renderer>().material = material;
			this.gSpine0a[k].layer = 8;
			this.mSpine0a[k] = this.gSpine0a[k].AddComponent<MouseDrag3>();
			this.mSpine0a[k].obj = this.gSpine0a[k];
			this.gSpine0a[k].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
			this.gSpine0a[k].SetActive(false);
			this.gSpine1a[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gSpine1a[k].GetComponent<Renderer>().material = material;
			this.gSpine1a[k].layer = 8;
			this.mSpine1a[k] = this.gSpine1a[k].AddComponent<MouseDrag3>();
			this.mSpine1a[k].obj = this.gSpine1a[k];
			this.gSpine1a[k].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
			this.gSpine1a[k].SetActive(false);
			this.gSpine1[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this.gSpine1[k].GetComponent<Renderer>().material = material;
			this.gSpine1[k].layer = 8;
			this.mSpine1[k] = this.gSpine1[k].AddComponent<MouseDrag3>();
			this.mSpine1[k].obj = this.gSpine1[k];
			this.gSpine1[k].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
			this.gSpine1[k].SetActive(false);
			this.gPelvis[k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.gPelvis[k].GetComponent<Renderer>().material = this.m_material3;
			this.gPelvis[k].layer = 8;
			this.mPelvis[k] = this.gPelvis[k].AddComponent<MouseDrag3>();
			this.mPelvis[k].obj = this.gPelvis[k];
			this.gPelvis[k].transform.localScale = new Vector3(0.045f, 0.045f, 0.045f);
			this.gPelvis[k].SetActive(false);
		}

		private void SetIK(Maid maid, int i)
		{
			this.Head = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Neck", true);
			this.Head1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Neck", true);
			this.Head2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Head", true);
			this.Head3[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 HeadNub", true);
			this.IKHandL[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handL", true);
			this.IKHandR[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "_IK_handR", true);
			this.IKMuneL[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_L", true);
			this.IKMuneLSub[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_L_sub", true);
			this.IKMuneR[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_R", true);
			this.IKMuneRSub[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Mune_R_sub", true);
			this.Spine = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine", true);
			this.Spine0a = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine0a", true);
			this.Spine1 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1", true);
			this.Spine1a = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1a", true);
			this.Pelvis = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Pelvis", true);
			this.Neck[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Neck", true);
			this.Pelvis2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
			this.Spine12[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1", true);
			this.Spine0a2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine0a", true);
			this.Spine2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine", true);
			this.Spine1a2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 Spine1a", true);
			this.HandL1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Hand", true);
			this.UpperArmL1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L UpperArm", true);
			this.ForearmL1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Forearm", true);
			this.HandR1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Hand", true);
			this.UpperArmR1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R UpperArm", true);
			this.ForearmR1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Forearm", true);
			this.HandL2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Foot", true);
			this.UpperArmL2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Thigh", true);
			this.ForearmL2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Calf", true);
			this.HandR2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Foot", true);
			this.UpperArmR2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Thigh", true);
			this.ForearmR2[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Calf", true);
			this.ClavicleL1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Clavicle", true);
			this.ClavicleR1[i] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Clavicle", true);
			this.Finger[i, 0] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger0", true);
			this.Finger[i, 1] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger01", true);
			this.Finger[i, 2] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger02", true);
			this.Finger[i, 3] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger0Nub", true);
			this.Finger[i, 4] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger1", true);
			this.Finger[i, 5] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger11", true);
			this.Finger[i, 6] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger12", true);
			this.Finger[i, 7] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger1Nub", true);
			this.Finger[i, 8] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger2", true);
			this.Finger[i, 9] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger21", true);
			this.Finger[i, 10] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger22", true);
			this.Finger[i, 11] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger2Nub", true);
			this.Finger[i, 12] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger3", true);
			this.Finger[i, 13] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger31", true);
			this.Finger[i, 14] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger32", true);
			this.Finger[i, 15] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger3Nub", true);
			this.Finger[i, 16] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger4", true);
			this.Finger[i, 17] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger41", true);
			this.Finger[i, 18] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger42", true);
			this.Finger[i, 19] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Finger4Nub", true);
			this.Finger[i, 20] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger0", true);
			this.Finger[i, 21] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger01", true);
			this.Finger[i, 22] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger02", true);
			this.Finger[i, 23] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger0Nub", true);
			this.Finger[i, 24] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger1", true);
			this.Finger[i, 25] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger11", true);
			this.Finger[i, 26] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger12", true);
			this.Finger[i, 27] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger1Nub", true);
			this.Finger[i, 28] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger2", true);
			this.Finger[i, 29] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger21", true);
			this.Finger[i, 30] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger22", true);
			this.Finger[i, 31] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger2Nub", true);
			this.Finger[i, 32] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger3", true);
			this.Finger[i, 33] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger31", true);
			this.Finger[i, 34] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger32", true);
			this.Finger[i, 35] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger3Nub", true);
			this.Finger[i, 36] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger4", true);
			this.Finger[i, 37] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger41", true);
			this.Finger[i, 38] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger42", true);
			this.Finger[i, 39] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Finger4Nub", true);
			this.Finger2[i, 0] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
			this.Finger2[i, 1] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
			this.Finger2[i, 2] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
			this.Finger2[i, 3] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
			this.Finger2[i, 4] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
			this.Finger2[i, 5] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
			this.Finger2[i, 6] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
			this.Finger2[i, 7] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
			this.Finger2[i, 8] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
			this.Finger2[i, 9] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
			this.Finger2[i, 10] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
			this.Finger2[i, 11] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
			this.Finger2[i, 12] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
			this.Finger2[i, 13] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
			this.Finger2[i, 14] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
			this.Finger2[i, 15] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
			this.Finger2[i, 16] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
			this.Finger2[i, 17] = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
		}

		private void CopyIK(Maid maid, int i, Maid maid2, int i2)
		{
			this.poseIndex[i] = this.poseIndex[i2];
			if (maid && maid.Visible)
			{
				string[] array = this.poseArray[this.poseIndex[i]].Split(new char[]
				{
					','
				});
				this.isStop[i] = false;
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
				int num2;
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
						string fileName = Path.GetFileName(path);
						long num = (long)fileName.GetHashCode();
						maid.body0.CrossFade(num.ToString(), array2, false, false, false, 0f, 1f);
						Maid.AutoTwist[] array3 = new Maid.AutoTwist[]
						{
							default(Maid.AutoTwist),
							Maid.AutoTwist.ShoulderR,
							Maid.AutoTwist.WristL,
							Maid.AutoTwist.WristR,
							Maid.AutoTwist.ThighL,
							Maid.AutoTwist.ThighR
						};
						for (int j = 0; j < array3.Length; j++)
						{
							maid.SetAutoTwist(array3[j], true);
						}
					}
				}
				else if (array[0].StartsWith("p") && int.TryParse(array[0].Substring(1), out num2))
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
						Transform transform = CMT.SearchObjName(this.maidArray[i].body0.m_Bones.transform, "Bip01", true);
						this.isPoseIti[i] = true;
						this.poseIti[i] = this.maidArray[i].transform.position;
						this.maidArray[i].transform.position = new Vector3(100f, 100f, 100f);
					}
				}
			}
		}

		private void CopyIK2(Maid maid, int i, Maid maid2, int i2)
		{
			this.isStop[i] = true;
			this.isLock[i] = true;
			maid.transform.position = new Vector3(maid.transform.position.x, maid2.transform.position.y, maid.transform.position.z);
			this.SetIK(maid2, i2);
			Transform transform = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe0", true);
			Transform transform2 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe0", true);
			Transform transform3 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe01", true);
			Transform transform4 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe01", true);
			Transform transform5 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
			Transform transform6 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
			Transform transform7 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe1", true);
			Transform transform8 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe1", true);
			Transform transform9 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe11", true);
			Transform transform10 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe11", true);
			Transform transform11 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
			Transform transform12 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
			Transform transform13 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe2", true);
			Transform transform14 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe2", true);
			Transform transform15 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe21", true);
			Transform transform16 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe21", true);
			Transform transform17 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
			Transform transform18 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
			Transform transform19 = CMT.SearchObjName(maid2.body0.m_Bones.transform, "Bip01", true);
			Vector3 eulerAngles = this.Head.eulerAngles;
			Vector3 eulerAngles2 = this.Spine.eulerAngles;
			Vector3 eulerAngles3 = this.Spine0a.eulerAngles;
			Vector3 eulerAngles4 = this.Spine1.eulerAngles;
			Vector3 eulerAngles5 = this.Spine1a.eulerAngles;
			Vector3 eulerAngles6 = this.Pelvis.eulerAngles;
			Vector3 eulerAngles7 = this.ClavicleL1[i2].eulerAngles;
			Vector3 eulerAngles8 = this.ClavicleR1[i2].eulerAngles;
			Vector3 localEulerAngles = transform.localEulerAngles;
			Vector3 localEulerAngles2 = transform2.localEulerAngles;
			Vector3 localEulerAngles3 = transform3.localEulerAngles;
			Vector3 localEulerAngles4 = transform4.localEulerAngles;
			Vector3 localEulerAngles5 = transform5.localEulerAngles;
			Vector3 localEulerAngles6 = transform6.localEulerAngles;
			Vector3 localEulerAngles7 = transform7.localEulerAngles;
			Vector3 localEulerAngles8 = transform8.localEulerAngles;
			Vector3 localEulerAngles9 = transform9.localEulerAngles;
			Vector3 localEulerAngles10 = transform10.localEulerAngles;
			Vector3 localEulerAngles11 = transform11.localEulerAngles;
			Vector3 localEulerAngles12 = transform12.localEulerAngles;
			Vector3 localEulerAngles13 = transform13.localEulerAngles;
			Vector3 localEulerAngles14 = transform14.localEulerAngles;
			Vector3 localEulerAngles15 = transform15.localEulerAngles;
			Vector3 localEulerAngles16 = transform16.localEulerAngles;
			Vector3 localEulerAngles17 = transform17.localEulerAngles;
			Vector3 localEulerAngles18 = transform18.localEulerAngles;
			Vector3 eulerAngles9 = transform19.eulerAngles;
			Vector3 localEulerAngles19 = this.HandL1[i2].localEulerAngles;
			Vector3 localEulerAngles20 = this.HandR1[i2].localEulerAngles;
			Vector3 localEulerAngles21 = this.HandL2[i2].localEulerAngles;
			Vector3 localEulerAngles22 = this.HandR2[i2].localEulerAngles;
			Vector3[] array = new Vector3[40];
			for (int j = 0; j < 20; j++)
			{
				array[j] = this.Finger[i2, j].localEulerAngles;
			}
			for (int j = 20; j < 40; j++)
			{
				array[j] = this.Finger[i2, j].localEulerAngles;
			}
			Vector3 eulerAngles10 = this.UpperArmL1[i2].eulerAngles;
			Vector3 eulerAngles11 = this.ForearmL1[i2].eulerAngles;
			Vector3 eulerAngles12 = this.UpperArmR1[i2].eulerAngles;
			Vector3 eulerAngles13 = this.ForearmR1[i2].eulerAngles;
			Vector3 eulerAngles14 = this.UpperArmL2[i2].eulerAngles;
			Vector3 eulerAngles15 = this.ForearmL2[i2].eulerAngles;
			Vector3 eulerAngles16 = this.UpperArmR2[i2].eulerAngles;
			Vector3 eulerAngles17 = this.ForearmR2[i2].eulerAngles;
			maid.transform.localEulerAngles = maid2.transform.localEulerAngles;
			this.SetIK(maid, i);
			transform = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
			transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
			transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
			transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
			transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
			transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
			transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
			transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
			transform9 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
			transform10 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
			transform11 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
			transform12 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
			transform13 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
			transform14 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
			transform15 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
			transform16 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
			transform17 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
			transform18 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
			transform19 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
			transform19.eulerAngles = eulerAngles9;
			this.Pelvis.eulerAngles = eulerAngles6;
			this.Spine.eulerAngles = eulerAngles2;
			this.Spine0a.eulerAngles = eulerAngles3;
			this.Spine1.eulerAngles = eulerAngles4;
			this.Spine1a.eulerAngles = eulerAngles5;
			this.ClavicleL1[i].eulerAngles = eulerAngles7;
			this.ClavicleR1[i].eulerAngles = eulerAngles8;
			this.Head.eulerAngles = eulerAngles;
			this.HandL1[i].localEulerAngles = localEulerAngles19;
			this.HandR1[i].localEulerAngles = localEulerAngles20;
			this.HandL2[i].localEulerAngles = localEulerAngles21;
			this.HandR2[i].localEulerAngles = localEulerAngles22;
			for (int j = 0; j < 20; j++)
			{
				this.Finger[i, j].localEulerAngles = array[j];
			}
			for (int j = 20; j < 40; j++)
			{
				this.Finger[i, j].localEulerAngles = array[j];
			}
			transform.localEulerAngles = localEulerAngles;
			transform2.localEulerAngles = localEulerAngles2;
			transform3.localEulerAngles = localEulerAngles3;
			transform4.localEulerAngles = localEulerAngles4;
			transform5.localEulerAngles = localEulerAngles5;
			transform6.localEulerAngles = localEulerAngles6;
			transform7.localEulerAngles = localEulerAngles7;
			transform8.localEulerAngles = localEulerAngles8;
			transform9.localEulerAngles = localEulerAngles9;
			transform10.localEulerAngles = localEulerAngles10;
			transform11.localEulerAngles = localEulerAngles11;
			transform12.localEulerAngles = localEulerAngles12;
			transform13.localEulerAngles = localEulerAngles13;
			transform14.localEulerAngles = localEulerAngles14;
			transform15.localEulerAngles = localEulerAngles15;
			transform16.localEulerAngles = localEulerAngles16;
			transform17.localEulerAngles = localEulerAngles17;
			transform18.localEulerAngles = localEulerAngles18;
			this.UpperArmL1[i].eulerAngles = eulerAngles10;
			this.UpperArmR1[i].eulerAngles = eulerAngles12;
			this.ForearmL1[i].eulerAngles = eulerAngles11;
			this.ForearmR1[i].eulerAngles = eulerAngles13;
			this.UpperArmL2[i].eulerAngles = eulerAngles14;
			this.UpperArmR2[i].eulerAngles = eulerAngles16;
			this.ForearmL2[i].eulerAngles = eulerAngles15;
			this.ForearmR2[i].eulerAngles = eulerAngles17;
		}

		private void SetHanten(Maid maid, int i)
		{
			this.SetIK(maid, i);
			int num = this.pHandL[this.selectMaidIndex];
			this.pHandL[this.selectMaidIndex] = this.pHandR[this.selectMaidIndex];
			this.pHandR[this.selectMaidIndex] = num;
			this.isStop[i] = true;
			this.isLock[i] = true;
			Transform transform = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_L", true);
			Transform transform2 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Hip_R", true);
			Transform transform3 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0", true);
			Transform transform4 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0", true);
			Transform transform5 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe01", true);
			Transform transform6 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe01", true);
			Transform transform7 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe0Nub", true);
			Transform transform8 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe0Nub", true);
			Transform transform9 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1", true);
			Transform transform10 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1", true);
			Transform transform11 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe11", true);
			Transform transform12 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe11", true);
			Transform transform13 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe1Nub", true);
			Transform transform14 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe1Nub", true);
			Transform transform15 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2", true);
			Transform transform16 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2", true);
			Transform transform17 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe21", true);
			Transform transform18 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe21", true);
			Transform transform19 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 L Toe2Nub", true);
			Transform transform20 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01 R Toe2Nub", true);
			Transform transform21 = CMT.SearchObjName(maid.body0.m_Bones.transform, "Bip01", true);
			Vector3 eulerAngles = this.Head.eulerAngles;
			Vector3 eulerAngles2 = this.Spine.eulerAngles;
			Vector3 eulerAngles3 = this.Spine0a.eulerAngles;
			Vector3 eulerAngles4 = this.Spine1.eulerAngles;
			Vector3 eulerAngles5 = this.Spine1a.eulerAngles;
			Vector3 eulerAngles6 = this.Pelvis.eulerAngles;
			Vector3 eulerAngles7 = this.ClavicleL1[i].eulerAngles;
			Vector3 eulerAngles8 = this.ClavicleR1[i].eulerAngles;
			Vector3 eulerAngles9 = transform.eulerAngles;
			Vector3 eulerAngles10 = transform2.eulerAngles;
			Vector3 localEulerAngles = transform3.localEulerAngles;
			Vector3 localEulerAngles2 = transform4.localEulerAngles;
			Vector3 localEulerAngles3 = transform5.localEulerAngles;
			Vector3 localEulerAngles4 = transform6.localEulerAngles;
			Vector3 localEulerAngles5 = transform7.localEulerAngles;
			Vector3 localEulerAngles6 = transform8.localEulerAngles;
			Vector3 localEulerAngles7 = transform9.localEulerAngles;
			Vector3 localEulerAngles8 = transform10.localEulerAngles;
			Vector3 localEulerAngles9 = transform11.localEulerAngles;
			Vector3 localEulerAngles10 = transform12.localEulerAngles;
			Vector3 localEulerAngles11 = transform13.localEulerAngles;
			Vector3 localEulerAngles12 = transform14.localEulerAngles;
			Vector3 localEulerAngles13 = transform15.localEulerAngles;
			Vector3 localEulerAngles14 = transform16.localEulerAngles;
			Vector3 localEulerAngles15 = transform17.localEulerAngles;
			Vector3 localEulerAngles16 = transform18.localEulerAngles;
			Vector3 localEulerAngles17 = transform19.localEulerAngles;
			Vector3 localEulerAngles18 = transform20.localEulerAngles;
			Vector3 eulerAngles11 = transform21.eulerAngles;
			Vector3 localEulerAngles19 = this.HandL1[i].localEulerAngles;
			Vector3 localEulerAngles20 = this.HandR1[i].localEulerAngles;
			Vector3 localEulerAngles21 = this.HandL2[i].localEulerAngles;
			Vector3 localEulerAngles22 = this.HandR2[i].localEulerAngles;
			Vector3[] array = new Vector3[40];
			for (int j = 0; j < 20; j++)
			{
				array[j] = this.Finger[i, j].localEulerAngles;
			}
			for (int j = 20; j < 40; j++)
			{
				array[j] = this.Finger[i, j].localEulerAngles;
			}
			Vector3 eulerAngles12 = this.UpperArmL1[i].eulerAngles;
			Vector3 eulerAngles13 = this.ForearmL1[i].eulerAngles;
			Vector3 eulerAngles14 = this.UpperArmR1[i].eulerAngles;
			Vector3 eulerAngles15 = this.ForearmR1[i].eulerAngles;
			Vector3 eulerAngles16 = this.UpperArmL2[i].eulerAngles;
			Vector3 eulerAngles17 = this.ForearmL2[i].eulerAngles;
			Vector3 eulerAngles18 = this.UpperArmR2[i].eulerAngles;
			Vector3 eulerAngles19 = this.ForearmR2[i].eulerAngles;
			transform21.eulerAngles = new Vector3(360f - (eulerAngles11.x + 270f) - 270f, 360f - (eulerAngles11.y + 90f) - 90f, 360f - eulerAngles11.z);
			transform.eulerAngles = this.getHanten(eulerAngles10);
			transform2.eulerAngles = this.getHanten(eulerAngles9);
			this.Pelvis.eulerAngles = this.getHanten(eulerAngles6);
			this.Spine.eulerAngles = this.getHanten(eulerAngles2);
			this.Spine0a.eulerAngles = this.getHanten(eulerAngles3);
			this.Spine1.eulerAngles = this.getHanten(eulerAngles4);
			this.Spine1a.eulerAngles = this.getHanten(eulerAngles5);
			this.ClavicleL1[i].eulerAngles = this.getHanten(eulerAngles8);
			this.ClavicleR1[i].eulerAngles = this.getHanten(eulerAngles7);
			this.Head.eulerAngles = this.getHanten(eulerAngles);
			this.HandR1[i].localEulerAngles = this.getHanten2(localEulerAngles19);
			this.HandL1[i].localEulerAngles = this.getHanten2(localEulerAngles20);
			this.HandR2[i].localEulerAngles = this.getHanten2(localEulerAngles21);
			this.HandL2[i].localEulerAngles = this.getHanten2(localEulerAngles22);
			for (int j = 0; j < 20; j++)
			{
				this.Finger[i, j].localEulerAngles = this.getHanten2(array[j + 20]);
			}
			for (int j = 20; j < 40; j++)
			{
				this.Finger[i, j].localEulerAngles = this.getHanten2(array[j - 20]);
			}
			transform4.localEulerAngles = this.getHanten2(localEulerAngles);
			transform3.localEulerAngles = this.getHanten2(localEulerAngles2);
			transform6.localEulerAngles = this.getHanten2(localEulerAngles3);
			transform5.localEulerAngles = this.getHanten2(localEulerAngles4);
			transform8.localEulerAngles = this.getHanten2(localEulerAngles5);
			transform7.localEulerAngles = this.getHanten2(localEulerAngles6);
			transform10.localEulerAngles = this.getHanten2(localEulerAngles7);
			transform9.localEulerAngles = this.getHanten2(localEulerAngles8);
			transform12.localEulerAngles = this.getHanten2(localEulerAngles9);
			transform11.localEulerAngles = this.getHanten2(localEulerAngles10);
			transform14.localEulerAngles = this.getHanten2(localEulerAngles11);
			transform13.localEulerAngles = this.getHanten2(localEulerAngles12);
			transform16.localEulerAngles = this.getHanten2(localEulerAngles13);
			transform15.localEulerAngles = this.getHanten2(localEulerAngles14);
			transform18.localEulerAngles = this.getHanten2(localEulerAngles15);
			transform17.localEulerAngles = this.getHanten2(localEulerAngles16);
			transform20.localEulerAngles = this.getHanten2(localEulerAngles17);
			transform19.localEulerAngles = this.getHanten2(localEulerAngles18);
			this.UpperArmR1[i].eulerAngles = this.getHanten(eulerAngles12);
			this.UpperArmL1[i].eulerAngles = this.getHanten(eulerAngles14);
			this.ForearmR1[i].eulerAngles = this.getHanten(eulerAngles13);
			this.ForearmL1[i].eulerAngles = this.getHanten(eulerAngles15);
			this.UpperArmR2[i].eulerAngles = this.getHanten(eulerAngles16);
			this.UpperArmL2[i].eulerAngles = this.getHanten(eulerAngles18);
			this.ForearmR2[i].eulerAngles = this.getHanten(eulerAngles17);
			this.ForearmL2[i].eulerAngles = this.getHanten(eulerAngles19);
		}
	}
}
