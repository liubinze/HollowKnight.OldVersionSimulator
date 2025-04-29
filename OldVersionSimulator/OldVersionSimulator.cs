using Modding;
using System.Collections.Generic;
using Vasi;
namespace OldVersionSimulator
{
	public class OldVersionSimulator:Mod,IMenuMod
	{
		public bool ToggleButtonInsideMenu=>true;
		private bool oldTakeHealth;
		private int oldCanOpenInventory;
		private int oldCanQuickMap;
		public override string GetVersion()=>VersionUtil.GetVersion<OldVersionSimulator>();
		public override void Initialize()
		{
			oldTakeHealth=false;
			oldCanOpenInventory=1578;
			On.HeroController.CanOpenInventory+=CanOpenInventory1578;
			oldCanQuickMap=1315;
			On.HeroController.CanQuickMap+=CanQuickMap1315;
			Log("Initialized!");
		}
		public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)=>
			new List<IMenuMod.MenuEntry>
			{
				new IMenuMod.MenuEntry
				{
					Name="1.4 TakeHealth",
					Description="Damage double the rest when lifeblood is less than it",
					Values=new string[]{"Off","On"},
					Saver=o=>
					{
						if(!this.oldTakeHealth&&o==1)
							On.PlayerData.TakeHealth+=oldTakeHealth_;
						if(this.oldTakeHealth&&o==0)
							On.PlayerData.TakeHealth-=oldTakeHealth_;
						this.oldTakeHealth=o!=0;
					},
					Loader=()=>this.oldTakeHealth?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanOpenInventory",
					Description="Choose whether inventory can be open in air, etc. or not",
					Values=new string[]{"1006","1028&1221","1315&1432","1578"},
					Saver=o=>
					{
						On.HeroController.CanOpenInventory-=this.oldCanOpenInventory switch
						{1006=>CanOpenInventory1006,1028=>CanOpenInventory1028,1315=>CanOpenInventory1315,1578=>CanOpenInventory1578};
						On.HeroController.CanOpenInventory+=o switch
						{0=>CanOpenInventory1006,1=>CanOpenInventory1028,2=>CanOpenInventory1315,3=>CanOpenInventory1578};
						this.oldCanOpenInventory=o switch{0=>1006,1=>1028,2=>1315,3=>1578};
					},
					Loader=()=>this.oldCanOpenInventory switch{1006=>0,1028=>1,1315=>2,1578=>3}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanQuickMap",
					Description="Choose whether it can quick map when transition, etc. or not",
					Values=new string[]{"1006","1028&1221","1315&1432&1578"},
					Saver=o=>
					{
						On.HeroController.CanQuickMap-=this.oldCanQuickMap switch
						{1006=>CanQuickMap1006,1028=>CanQuickMap1028,1315=>CanQuickMap1315};
						On.HeroController.CanQuickMap+=o switch
						{0=>CanQuickMap1006,1=>CanQuickMap1028,2=>CanQuickMap1315};
						this.oldCanQuickMap=o switch{0=>1006,1=>1028,2=>1315};
					},
					Loader=()=>this.oldCanQuickMap switch{1006=>0,1028=>1,1315=>2}
				}
			};
		private void oldTakeHealth_(On.PlayerData.orig_TakeHealth orig,PlayerData self,int amount)
		{
			if(self.healthBlue>0&&amount>self.healthBlue)
				amount+=amount-self.healthBlue;
			orig(self,amount);
		}
		private bool CanOpenInventory1006(On.HeroController.orig_CanOpenInventory orig,HeroController self)=>
		(!GameManager.instance.isPaused&&!self.controlReqlinquished&&!self.cState.transitioning&&!self.playerData.disablePause&&self.CanInput())||self.playerData.atBench;
		private bool CanOpenInventory1028(On.HeroController.orig_CanOpenInventory orig,HeroController self)=>
		CanOpenInventory1006(orig,self)&&((!self.cState.hazardDeath&&!self.cState.hazardRespawning)||self.playerData.atBench);
		private bool CanOpenInventory1315(On.HeroController.orig_CanOpenInventory orig,HeroController self)=>
		CanOpenInventory1028(orig,self)&&((self.hero_state!=GlobalEnums.ActorStates.airborne&&!self.cState.recoiling)||self.playerData.atBench);
		private bool CanOpenInventory1578(On.HeroController.orig_CanOpenInventory orig,HeroController self)=>
		CanOpenInventory1315(orig,self)&&((self.cState.onGround&&!self.cState.dashing)||self.playerData.atBench);
		private bool CanQuickMap1006(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		!GameManager.instance.isPaused&&!self.cState.onConveyor&&!self.cState.dashing&&!self.cState.backDashing&&(!self.cState.attacking||Mirror.GetField<HeroController,float>(self,"attack_time")>=self.ATTACK_RECOVERY_TIME)&&!self.cState.recoiling&&!self.cState.transitioning&&!self.cState.recoilFrozen&&self.cState.onGround&&self.CanInput();
		private bool CanQuickMap1028(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		CanQuickMap1006(orig,self)&&!self.cState.hazardDeath&&!self.cState.hazardRespawning;
		private bool CanQuickMap1315(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		CanQuickMap1028(orig,self)&&!self.controlReqlinquished&&self.hero_state!=GlobalEnums.ActorStates.no_input;
	}
}
