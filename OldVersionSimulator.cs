using Modding;
using System.Collections.Generic;
using Vasi;
namespace OldVersionSimulator
{
	public class OldVersionSimulator:Mod,IMenuMod
	{
		public bool ToggleButtonInsideMenu=>true;
		private bool oldTakeHealth;
		private bool oldTutorialEntryPauser;
		private bool oldcharmCost_11;
		private bool oldcharmCost_32;
		private int oldCanOpenInventory;
		private int oldCanQuickMap;
		public override string GetVersion()=>VersionUtil.GetVersion<OldVersionSimulator>();
		public override void Initialize()
		{
			oldTakeHealth=false;
			oldTutorialEntryPauser=false;
			oldcharmCost_11=false;
			oldcharmCost_32=false;
			oldCanOpenInventory=0;
			oldCanQuickMap=0;
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
/*				new IMenuMod.MenuEntry
				{
					Name="Old TutorialEntryPauser",
					Description="This has no effect at all",
					Values=new string[]{"Off","On"},
					Saver=o=>
					{
						if(!this.oldTutorialEntryPauser&&o==1)
							On.TutorialEntryPauser.Start+=oldTutorialEntryPauser_;
						if(this.oldTutorialEntryPauser&&o==0)
							On.TutorialEntryPauser.Start-=oldTutorialEntryPauser_;
						this.oldTutorialEntryPauser=o!=0;
					},
					Loader=()=>this.oldTutorialEntryPauser?1:0
				},
*/				new IMenuMod.MenuEntry
				{
					Name="Old Flukenest charm cost",
					Description="2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>
					{
						if(!this.oldcharmCost_11&&o==1)
							On.PlayerData.SetupNewPlayerData+=oldcharmCost_11_;
						if(this.oldcharmCost_11&&o==0)
							On.PlayerData.SetupNewPlayerData-=oldcharmCost_11_;
						this.oldcharmCost_11=o!=0;
					},
					Loader=()=>this.oldcharmCost_11?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old Quick Slash charm cost",
					Description="2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>
					{
						if(!this.oldcharmCost_32&&o==1)
							On.PlayerData.SetupNewPlayerData+=oldcharmCost_32_;
						if(this.oldcharmCost_32&&o==0)
							On.PlayerData.SetupNewPlayerData-=oldcharmCost_32_;
						this.oldcharmCost_32=o!=0;
					},
					Loader=()=>this.oldcharmCost_32?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanOpenInventory",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1432","1578"},
					Saver=o=>
					{
						if(this.oldCanOpenInventory!=0)
							On.HeroController.CanOpenInventory-=this.oldCanOpenInventory switch
							{1006=>CanOpenInventory1006,1028=>CanOpenInventory1028,1315=>CanOpenInventory1315,1578=>CanOpenInventory1578};
						if(o!=0)
							On.HeroController.CanOpenInventory+=o switch
							{1=>CanOpenInventory1006,2=>CanOpenInventory1028,3=>CanOpenInventory1315,4=>CanOpenInventory1578};
						this.oldCanOpenInventory=o switch{0=>0,1=>1006,2=>1028,3=>1315,4=>1578};
					},
					Loader=()=>this.oldCanOpenInventory switch{0=>0,1006=>1,1028=>2,1315=>3,1578=>4}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanQuickMap",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1578"},
					Saver=o=>
					{
						if(this.oldCanQuickMap!=0)
							On.HeroController.CanQuickMap-=this.oldCanQuickMap switch
							{1006=>CanQuickMap1006,1028=>CanQuickMap1028,1315=>CanQuickMap1315};
						if(o!=0)
							On.HeroController.CanQuickMap+=o switch
							{1=>CanQuickMap1006,2=>CanQuickMap1028,3=>CanQuickMap1315};
						this.oldCanQuickMap=o switch{0=>0,1=>1006,2=>1028,3=>1315};
					},
					Loader=()=>this.oldCanQuickMap switch{0=>0,1006=>1,1028=>2,1315=>3}
				}
			};
		private void oldTakeHealth_(On.PlayerData.orig_TakeHealth orig,PlayerData self,int amount)
		{
			if(self.healthBlue>0&&amount>self.healthBlue)
				amount+=amount-self.healthBlue;
			orig(self,amount);
		}
		private void oldTutorialEntryPauser_(On.TutorialEntryPauser.orig_Start orig,TutorialEntryPauser self)
		{
			orig(self);
			PlayerData.instance.disablePause=false;
		}
		private void oldcharmCost_11_(On.PlayerData.orig_SetupNewPlayerData orig,PlayerData self)
		{
			orig(self);
			self.charmCost_11=2;
		}
		private void oldcharmCost_32_(On.PlayerData.orig_SetupNewPlayerData orig,PlayerData self)
		{
			orig(self);
			self.charmCost_32=2;
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
