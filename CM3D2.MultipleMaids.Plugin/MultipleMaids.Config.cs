using ExIni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityInjector;

namespace CM3D2.MultipleMaids.Plugin
{
    public partial class MultipleMaids
    {
		public void Preference()
		{
			if (!this.isPref)
			{
				this.isPref = true;
				IniKey iniKey = base.Preferences["config"]["hair_setting"];
				if (iniKey.Value == "true")
				{
					this.isKamiyure = true;
					IniKey iniKey2 = base.Preferences["config"]["hair_radius"];
					IniKey iniKey3 = base.Preferences["config"]["hair_elasticity"];
					IniKey iniKey4 = base.Preferences["config"]["hair_damping"];
					this.kamiyure2 = float.Parse(iniKey4.Value);
					this.kamiyure3 = float.Parse(iniKey3.Value);
					this.kamiyure4 = float.Parse(iniKey2.Value);
				}
				else
				{
					this.isKamiyure = false;
					this.kamiyure2 = 0.6f;
					this.kamiyure3 = 1f;
					this.kamiyure4 = 0.02f;
				}
				IniKey iniKey5 = base.Preferences["config"]["skirt_setting"];
				if (iniKey5.Value == "true")
				{
					this.isSkirtyure = true;
					IniKey iniKey2 = base.Preferences["config"]["skirt_radius"];
					IniKey iniKey3 = base.Preferences["config"]["skirt_elasticity"];
					IniKey iniKey4 = base.Preferences["config"]["skirt_damping"];
					this.skirtyure2 = float.Parse(iniKey4.Value);
					this.skirtyure3 = float.Parse(iniKey3.Value);
					this.skirtyure4 = float.Parse(iniKey2.Value);
				}
				else
				{
					this.isSkirtyure = false;
					this.skirtyure2 = 0.1f;
					this.skirtyure3 = 0.05f;
					this.skirtyure4 = 0.1f;
				}
				IniKey iniKey6 = base.Preferences["config"]["hair_details"];
				if (iniKey6.Value == "true")
				{
					this.isShosai = true;
				}
				IniKey iniKey7 = base.Preferences["config"]["vr_scroll"];
				if (iniKey7.Value == "false")
				{
					this.isVRScroll = false;
				}
				else if (iniKey7.Value != "true")
				{
					base.Preferences["config"]["vr_scroll"].Value = "true";
					base.SaveConfig();
				}
				IniKey iniKey8 = base.Preferences["config"]["shift_f7"];
				if (iniKey8.Value == "true")
				{
					this.isF7S = true;
				}
				IniKey iniKey9 = base.Preferences["config"]["shift_f8"];
				if (iniKey9.Value == "false")
				{
					this.isVR2 = false;
				}
				IniKey iniKey10 = base.Preferences["config"]["ik_all"];
				if (iniKey10.Value == "true")
				{
					this.isIKAll = true;
					for (int i = 0; i < this.maxMaidCnt; i++)
					{
						this.isIK[i] = true;
					}
				}
				else if (iniKey10.Value != "false")
				{
					base.Preferences["config"]["ik_all"].Value = "true";
					base.SaveConfig();
					this.isIKAll = true;
					for (int i = 0; i < this.maxMaidCnt; i++)
					{
						this.isIK[i] = true;
					}
				}
				IniKey iniKey11 = base.Preferences["config"]["scene_max"];
				if (!int.TryParse(iniKey11.Value, out this.maxPage))
				{
					this.maxPage = 100;
					base.Preferences["config"]["scene_max"].Value = "100";
					base.SaveConfig();
				}
				IniKey iniKey12 = base.Preferences["config"]["kankyo_max"];
				if (!int.TryParse(iniKey12.Value, out this.kankyoMax))
				{
					this.kankyoMax = 20;
					base.Preferences["config"]["kankyo_max"].Value = "20";
					base.SaveConfig();
				}
				for (int j = 0; j < this.kankyoMax; j++)
				{
					IniKey iniKey13 = base.Preferences["kankyo"]["kankyo" + (j + 1)];
					if (iniKey13.Value == null || iniKey13.Value == "")
					{
						base.Preferences["kankyo"]["kankyo" + (j + 1)].Value = "環境" + (j + 1);
						base.SaveConfig();
					}
				}
				this.maxPage /= 10;
			}
		}
	}
}
