using GlobalEnums;
using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;
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
		private int oldCancelWallsliding;
		private int oldFinishedDashing;
		private int oldOnCollisionExit2D;
		private int oldSetStartingMotionState;
		public override string GetVersion()=>VersionUtil.GetVersion<OldVersionSimulator>();
		public override void Initialize()
		{
			oldTakeHealth=false;
			oldTutorialEntryPauser=false;
			oldcharmCost_11=false;
			oldcharmCost_32=false;
			oldCanOpenInventory=0;
			oldCanQuickMap=0;
			oldCancelWallsliding=0;
			oldFinishedDashing=0;
			oldOnCollisionExit2D=0;
			oldSetStartingMotionState=0;
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
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CancelWallsliding",
					Description="Allow clinging with WCS",
					Values=new string[]{"Off","1006-1221","1315-1578"},
					Saver=o=>
					{
						if(this.oldCancelWallsliding!=0)
							On.HeroController.CancelWallsliding-=this.oldCancelWallsliding switch
							{1006=>CancelWallsliding1006,1315=>CancelWallsliding1315};
						if(o!=0)
							On.HeroController.CancelWallsliding+=o switch
							{1=>CancelWallsliding1006,2=>CancelWallsliding1315};
						this.oldCancelWallsliding=o switch{0=>0,1=>1006,2=>1315};
					},
					Loader=()=>this.oldCancelWallsliding switch{0=>0,1006=>1,1315=>2}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old FinishedDashing",
					Description="Retain WCS when dashing",
					Values=new string[]{"Off","1006-1221","1315","1424-1578"},
					Saver=o=>
					{
						if(this.oldFinishedDashing!=0)
							On.HeroController.FinishedDashing-=this.oldFinishedDashing switch
							{1006=>FinishedDashing1006,1315=>FinishedDashing1315,1424=>FinishedDashing1424};
						if(o!=0)
							On.HeroController.FinishedDashing+=o switch
							{1=>FinishedDashing1006,2=>FinishedDashing1315,3=>FinishedDashing1424};
						this.oldFinishedDashing=o switch{0=>0,1=>1006,2=>1315,3=>1424};
					},
					Loader=()=>this.oldFinishedDashing switch{0=>0,1006=>1,1315=>2,1424=>3}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old OnCollisionExit2D",
					Description="Retain WCS on collision exit",
					Values=new string[]{"Off","1006","1028","1221-1432","1578"},
					Saver=o=>
					{
						if(this.oldOnCollisionExit2D!=0)
							On.HeroController.OnCollisionExit2D-=this.oldOnCollisionExit2D switch
							{1006=>OnCollisionExit2D1006,1028=>OnCollisionExit2D1028,1221=>OnCollisionExit2D1221,1578=>OnCollisionExit2D1578};
						if(o!=0)
							On.HeroController.OnCollisionExit2D+=o switch
							{1=>OnCollisionExit2D1006,2=>OnCollisionExit2D1028,3=>OnCollisionExit2D1221,4=>OnCollisionExit2D1578};
						this.oldOnCollisionExit2D=o switch{0=>0,1=>1006,2=>1028,3=>1221,4=>1578};
					},
					Loader=()=>this.oldOnCollisionExit2D switch{0=>0,1006=>1,1028=>2,1221=>3,1578=>4}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old SetStartingMotionState",
					Description="Retain WCS through transitions",
					Values=new string[]{"Off","1006-1221","1315-1432","1578"},
					Saver=o=>
					{
						if(this.oldSetStartingMotionState!=0)
							On.HeroController.SetStartingMotionState_bool-=this.oldSetStartingMotionState switch
							{1006=>SetStartingMotionState1006,1315=>SetStartingMotionState1315,1578=>SetStartingMotionState1578};
						if(o!=0)
							On.HeroController.SetStartingMotionState_bool+=o switch
							{1=>SetStartingMotionState1006,2=>SetStartingMotionState1315,3=>SetStartingMotionState1578};
						this.oldSetStartingMotionState=o switch{0=>0,1=>1006,2=>1315,3=>1578};
					},
					Loader=()=>this.oldSetStartingMotionState switch{0=>0,1006=>1,1315=>2,1578=>3}
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
		CanOpenInventory1028(orig,self)&&((self.hero_state!=ActorStates.airborne&&!self.cState.recoiling)||self.playerData.atBench);
		private bool CanOpenInventory1578(On.HeroController.orig_CanOpenInventory orig,HeroController self)=>
		CanOpenInventory1315(orig,self)&&((self.cState.onGround&&!self.cState.dashing)||self.playerData.atBench);
		private bool CanQuickMap1006(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		!GameManager.instance.isPaused&&!self.cState.onConveyor&&!self.cState.dashing&&!self.cState.backDashing&&(!self.cState.attacking||Mirror.GetField<HeroController,float>(self,"attack_time")>=self.ATTACK_RECOVERY_TIME)&&!self.cState.recoiling&&!self.cState.transitioning&&!self.cState.recoilFrozen&&self.cState.onGround&&self.CanInput();
		private bool CanQuickMap1028(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		CanQuickMap1006(orig,self)&&!self.cState.hazardDeath&&!self.cState.hazardRespawning;
		private bool CanQuickMap1315(On.HeroController.orig_CanQuickMap orig,HeroController self)=>
		CanQuickMap1028(orig,self)&&!self.controlReqlinquished&&self.hero_state!=ActorStates.no_input;
		private void CancelWallsliding1006(On.HeroController.orig_CancelWallsliding orig,HeroController self)
		{
			self.wallslideDustPrefab.enableEmission=false;
			self.cState.wallSliding=false;
			self.wallSlidingL=false;
			self.wallSlidingR=false;
		}
		private void CancelWallsliding1315(On.HeroController.orig_CancelWallsliding orig,HeroController self)
		{
			self.wallslideDustPrefab.enableEmission=false;
			self.wallSlideVibrationPlayer.Stop();
			self.cState.wallSliding=false;
			self.wallSlidingL=false;
			self.wallSlidingR=false;
			self.touchingWallL=false;
			self.touchingWallR=false;
		}
		private void CancelDash(HeroController self)
		{
			if(self.cState.shadowDashing)
			{
				self.cState.shadowDashing=false;
				//self.proxyFSM.SendEvent("HeroCtrl-ShadowDashEnd"); //This only appears in 1221-
			}
			self.cState.dashing=false;
			Mirror.SetField<HeroController,float>(self,"dash_timer",0f);
			self.AffectedByGravity(true);
			self.sharpShadowPrefab.SetActive(false);
			if(self.dashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission)
				self.dashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission=false;
			if(self.shadowdashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission)
				self.shadowdashParticlesPrefab.GetComponent<ParticleSystem>().enableEmission=false;
		}
		private void FinishedDashing(HeroController self,bool touchingWall,bool FlipSprite)
		{
			CancelDash(self);
			self.AffectedByGravity(true);
			Mirror.GetFieldRef<HeroController,HeroAnimationController>(self,"animCtrl").FinishedDash();
			self.proxyFSM.SendEvent("HeroCtrl-DashEnd");
			if(self.cState.touchingWall&&!self.cState.onGround&&self.playerData.hasWalljump&&(!touchingWall||self.touchingWallL||self.touchingWallR))
			{
				self.wallslideDustPrefab.enableEmission=true;
				self.wallSlideVibrationPlayer.Play();
				self.cState.wallSliding=true;
				self.cState.willHardLand=false;
				if(self.touchingWallL)
					self.wallSlidingL=true;
				if(self.touchingWallR)
					self.wallSlidingR=true;
				if(FlipSprite&&self.dashingDown)
					self.FlipSprite();
			}
		}
		private void FinishedDashing1006(On.HeroController.orig_FinishedDashing orig,HeroController self)=>
		FinishedDashing(self,false,false);
		private void FinishedDashing1315(On.HeroController.orig_FinishedDashing orig,HeroController self)=>
		FinishedDashing(self,true,false);
		private void FinishedDashing1424(On.HeroController.orig_FinishedDashing orig,HeroController self)=>
		FinishedDashing(self,true,true);
		private void SetState(HeroController self,ActorStates newState)
		{
			if(newState==ActorStates.grounded)
				if(Mathf.Abs(self.move_input)>Mathf.Epsilon)
					newState=ActorStates.running;
				else
					newState=ActorStates.idle;
			else if(newState==ActorStates.previous)
				newState=self.prev_hero_state;
			if(newState!=self.hero_state)
			{
				self.prev_hero_state=self.hero_state;
				self.hero_state=newState;
				Mirror.GetFieldRef<HeroController,HeroAnimationController>(self,"animCtrl").UpdateState(newState);
			}
		}
		private void OnCollisionExit2D(HeroController self,Collision2D collision,bool fsm_fallTrail,bool recoiling,bool CheckStillTouchingWall)
		{
			if(self.cState.recoilingLeft||self.cState.recoilingRight)
			{
				self.cState.touchingWall=false;
				self.touchingWallL=false;
				self.touchingWallR=false;
				self.cState.touchingNonSlider=false;
			}
			if(CheckStillTouchingWall)
			{
				if(self.touchingWallL&&!Mirror.GetField<HeroController,Func<CollisionSide,bool,bool>>(self,"CheckStillTouchingWall")(CollisionSide.left,false))
				{
					self.cState.touchingWall=false;
					self.touchingWallL=false;
				}
				if(self.touchingWallR&&!Mirror.GetField<HeroController,Func<CollisionSide,bool,bool>>(self,"CheckStillTouchingWall")(CollisionSide.right,false))
				{
					self.cState.touchingWall=false;
					self.touchingWallR=false;
				}
			}
			if(self.hero_state!=ActorStates.no_input&&(!recoiling||!self.cState.recoiling)&&collision.gameObject.layer==8&&!self.CheckTouchingGround())
			{
				if(!self.cState.jumping&&!Mirror.GetField<HeroController,bool>(self,"fallTrailGenerated")&&self.cState.onGround)
				{
					if(!fsm_fallTrail)
						self.fallEffectPrefab.Spawn(self.transform.position);
					else if(self.playerData.environmentType!=6)
						self.fsm_fallTrail.SendEvent("PLAY");
					Mirror.SetField<HeroController,bool>(self,"fallTrailGenerated",true);
				}
				self.cState.onGround=false;
				self.proxyFSM.SendEvent("HeroCtrl-LeftGround");
				SetState(self,ActorStates.airborne);
				if(self.cState.wasOnGround)
					Mirror.SetField<HeroController,int>(self,"ledgeBufferSteps",Mirror.GetField<HeroController,int>(self,"LEDGE_BUFFER_STEPS"));
			}
		}
		private void OnCollisionExit2D1006(On.HeroController.orig_OnCollisionExit2D orig,HeroController self,Collision2D collision)=>
		OnCollisionExit2D(self,collision,false,false,false);
		private void OnCollisionExit2D1028(On.HeroController.orig_OnCollisionExit2D orig,HeroController self,Collision2D collision)=>
		OnCollisionExit2D(self,collision,true,false,false);
		private void OnCollisionExit2D1221(On.HeroController.orig_OnCollisionExit2D orig,HeroController self,Collision2D collision)=>
		OnCollisionExit2D(self,collision,true,true,false);
		private void OnCollisionExit2D1578(On.HeroController.orig_OnCollisionExit2D orig,HeroController self,Collision2D collision)=>
		OnCollisionExit2D(self,collision,true,true,true);
		private void SetStartingMotionState1006(On.HeroController.orig_SetStartingMotionState_bool orig,HeroController self,bool preventRunDip)
		{
			var touchingWall=self.cState.touchingWall;
			var doubleJumped=self.doubleJumped;
			var airDashed=self.airDashed;
			orig(self,true);
			self.cState.touchingWall=touchingWall;
			self.doubleJumped=doubleJumped;
			self.airDashed=airDashed;
		}
		private void SetStartingMotionState1315(On.HeroController.orig_SetStartingMotionState_bool orig,HeroController self,bool preventRunDip)
		{
			var doubleJumped=self.doubleJumped;
			var airDashed=self.airDashed;
			orig(self,preventRunDip);
			self.doubleJumped=doubleJumped;
			self.airDashed=airDashed;
		}
		private void SetStartingMotionState1578(On.HeroController.orig_SetStartingMotionState_bool orig,HeroController self,bool preventRunDip)
		orig(self,preventRunDip);
	}
}
