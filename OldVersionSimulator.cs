using GlobalEnums;
using Modding;
using Mono.Cecil.Cil;
using MonoMod.Cil;
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
			On.PlayerData.TakeHealth+=oldTakeHealth_;
			oldTutorialEntryPauser=false;
			On.TutorialEntryPauser.Start+=oldTutorialEntryPauser_;
			oldcharmCost_11=false;
			On.PlayerData.SetupNewPlayerData+=oldcharmCost_11_;
			oldcharmCost_32=false;
			On.PlayerData.SetupNewPlayerData+=oldcharmCost_32_;
			oldCanOpenInventory=0;
			On.HeroController.CanOpenInventory+=CanOpenInventory;
			oldCanQuickMap=0;
			On.HeroController.CanQuickMap+=CanQuickMap;
			oldCancelWallsliding=0;
			On.HeroController.CancelWallsliding+=CancelWallsliding;
			oldFinishedDashing=0;
			On.HeroController.FinishedDashing+=FinishedDashing;
			oldOnCollisionExit2D=0;
			On.HeroController.OnCollisionExit2D+=OnCollisionExit2D;
			oldSetStartingMotionState=1578;
			IL.HeroController.SetStartingMotionState_bool+=SetStartingMotionState;
		}
		public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)=>
			new List<IMenuMod.MenuEntry>
			{
				new IMenuMod.MenuEntry
				{
					Name="1.4 TakeHealth",
					Description="Damage double the rest when lifeblood is less than it",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldTakeHealth=o!=0,
					Loader=()=>this.oldTakeHealth?1:0
				},
/*				new IMenuMod.MenuEntry
				{
					Name="Old TutorialEntryPauser",
					Description="This has no effect at all",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldTutorialEntryPauser=o!=0,
					Loader=()=>this.oldTutorialEntryPauser?1:0
				},
*/				new IMenuMod.MenuEntry
				{
					Name="Old Flukenest charm cost",
					Description="2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldcharmCost_11=o!=0,
					Loader=()=>this.oldcharmCost_11?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old Quick Slash charm cost",
					Description="2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldcharmCost_32=o!=0,
					Loader=()=>this.oldcharmCost_32?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanOpenInventory",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1432","1578"},
					Saver=o=>this.oldCanOpenInventory=o switch{0=>0,1=>1006,2=>1028,3=>1315,4=>1578},
					Loader=()=>this.oldCanOpenInventory switch{0=>0,1006=>1,1028=>2,1315=>3,1578=>4}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanQuickMap",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1578"},
					Saver=o=>this.oldCanQuickMap=o switch{0=>0,1=>1006,2=>1028,3=>1315},
					Loader=()=>this.oldCanQuickMap switch{0=>0,1006=>1,1028=>2,1315=>3}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CancelWallsliding",
					Description="Allow clinging with WCS",
					Values=new string[]{"Off","1006-1221","1315-1578"},
					Saver=o=>this.oldCancelWallsliding=o switch{0=>0,1=>1006,2=>1315},
					Loader=()=>this.oldCancelWallsliding switch{0=>0,1006=>1,1315=>2}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old FinishedDashing",
					Description="Retain WCS when dashing",
					Values=new string[]{"Off","1006-1221","1315","1424-1578"},
					Saver=o=>this.oldFinishedDashing=o switch{0=>0,1=>1006,2=>1315,3=>1424},
					Loader=()=>this.oldFinishedDashing switch{0=>0,1006=>1,1315=>2,1424=>3}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old OnCollisionExit2D",
					Description="Retain WCS on collision exit",
					Values=new string[]{"Off","1006","1028","1221-1432","1578"},
					Saver=o=>this.oldOnCollisionExit2D=o switch{0=>0,1=>1006,2=>1028,3=>1221,4=>1578},
					Loader=()=>this.oldOnCollisionExit2D switch{0=>0,1006=>1,1028=>2,1221=>3,1578=>4}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old SetStartingMotionState",
					Description="Retain WCS through transitions",
					Values=new string[]{"1006-1221","1315-1432","1578"},
					Saver=o=>this.oldSetStartingMotionState=o switch{0=>1006,1=>1315,2=>1578},
					Loader=()=>this.oldSetStartingMotionState switch{1006=>0,1315=>1,1578=>2}
				}
			};
		private void oldTakeHealth_(On.PlayerData.orig_TakeHealth orig,PlayerData self,int amount)
		{
			if(oldTakeHealth&&self.healthBlue>0&&amount>self.healthBlue)
				amount+=amount-self.healthBlue;
			orig(self,amount);
		}
		private void oldTutorialEntryPauser_(On.TutorialEntryPauser.orig_Start orig,TutorialEntryPauser self)
		{
			orig(self);
			if(oldTutorialEntryPauser)
				PlayerData.instance.disablePause=false;
		}
		private void oldcharmCost_11_(On.PlayerData.orig_SetupNewPlayerData orig,PlayerData self)
		{
			orig(self);
			if(oldcharmCost_11)
				self.charmCost_11=2;
		}
		private void oldcharmCost_32_(On.PlayerData.orig_SetupNewPlayerData orig,PlayerData self)
		{
			orig(self);
			if(oldcharmCost_32)
				self.charmCost_32=2;
		}
		private bool CanOpenInventory(On.HeroController.orig_CanOpenInventory orig,HeroController self)
		{
			if(oldCanOpenInventory==0)
				return orig(self);
			return(!GameManager.instance.isPaused
				&&(oldCanOpenInventory<1315||self.hero_state!=ActorStates.airborne)
				&&!self.controlReqlinquished
				&&(oldCanOpenInventory<1315||!self.cState.recoiling)
				&&!self.cState.transitioning
				&&(oldCanOpenInventory<1028||!self.cState.hazardDeath)
				&&(oldCanOpenInventory<1028||!self.cState.hazardRespawning)
				&&(oldCanOpenInventory<1578||self.cState.onGround)
				&&!self.playerData.disablePause
				&&(oldCanOpenInventory<1578||!self.cState.dashing)
				&&self.CanInput()
			)||self.playerData.atBench;
		}
		private bool CanQuickMap(On.HeroController.orig_CanQuickMap orig,HeroController self)
		{
			if(oldCanQuickMap==0)
				return orig(self);
			return !GameManager.instance.isPaused
			&&(oldCanQuickMap<1315||!self.controlReqlinquished)
			&&(oldCanQuickMap<1315||self.hero_state!=ActorStates.no_input)
			&&!self.cState.onConveyor
			&&!self.cState.dashing
			&&!self.cState.backDashing
			&&(!self.cState.attacking||Mirror.GetField<HeroController,float>(self,"attack_time")>=self.ATTACK_RECOVERY_TIME)
			&&!self.cState.recoiling
			&&!self.cState.transitioning
			&&(oldCanQuickMap<1028||!self.cState.hazardDeath)
			&&(oldCanQuickMap<1028||!self.cState.hazardRespawning)
			&&!self.cState.recoilFrozen
			&&self.cState.onGround
			&&self.CanInput();
		}
		private void CancelWallsliding(On.HeroController.orig_CancelWallsliding orig,HeroController self)
		{
			if(oldCancelWallsliding==0)
			{
				orig(self);
				return;
			}
			self.wallslideDustPrefab.enableEmission=false;
			if(oldCancelWallsliding>=1315)
				self.wallSlideVibrationPlayer.Stop();
			self.cState.wallSliding=false;
			self.wallSlidingL=false;
			self.wallSlidingR=false;
			if(oldCancelWallsliding>=1315)
			{
				self.touchingWallL=false;
				self.touchingWallR=false;
			}
		}
		private void CancelDash(HeroController self)
		{
			if(self.cState.shadowDashing)
			{
				self.cState.shadowDashing=false;
				if(oldFinishedDashing<1315)
					self.proxyFSM.SendEvent("HeroCtrl-ShadowDashEnd");
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
		private void FinishedDashing(On.HeroController.orig_FinishedDashing orig,HeroController self)
		{
			if(oldFinishedDashing==0)
			{
				orig(self);
				return;
			}
			CancelDash(self);
			self.AffectedByGravity(true);
			Mirror.GetFieldRef<HeroController,HeroAnimationController>(self,"animCtrl").FinishedDash();
			self.proxyFSM.SendEvent("HeroCtrl-DashEnd");
			if(self.cState.touchingWall&&!self.cState.onGround&&self.playerData.hasWalljump&&(oldFinishedDashing<1315||self.touchingWallL||self.touchingWallR))
			{
				self.wallslideDustPrefab.enableEmission=true;
				self.wallSlideVibrationPlayer.Play();
				self.cState.wallSliding=true;
				self.cState.willHardLand=false;
				if(self.touchingWallL)
					self.wallSlidingL=true;
				if(self.touchingWallR)
					self.wallSlidingR=true;
				if(oldFinishedDashing>=1424&&self.dashingDown)
					self.FlipSprite();
			}
		}
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
		private void OnCollisionExit2D(On.HeroController.orig_OnCollisionExit2D orig,HeroController self,Collision2D collision)
		{
			if(oldOnCollisionExit2D==0)
			{
				orig(self,collision);
				return;
			}
			if(self.cState.recoilingLeft||self.cState.recoilingRight)
			{
				self.cState.touchingWall=false;
				self.touchingWallL=false;
				self.touchingWallR=false;
				self.cState.touchingNonSlider=false;
			}
			if(oldOnCollisionExit2D>=1578)
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
			if(self.hero_state!=ActorStates.no_input&&(oldOnCollisionExit2D<1221||!self.cState.recoiling)&&collision.gameObject.layer==8&&!self.CheckTouchingGround())
			{
				if(!self.cState.jumping&&!Mirror.GetField<HeroController,bool>(self,"fallTrailGenerated")&&self.cState.onGround)
				{
					if(oldOnCollisionExit2D<1028)
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
		private void SetStartingMotionState(ILContext il)
		{
			ILCursor cursor=new(il);
			if(oldSetStartingMotionState<1315)
			{
				cursor.GotoNext
				(
					i=>i.MatchLdarg(1),
					i=>i.MatchOr()
				);
				cursor.Remove();
				cursor.Emit(OpCodes.Ldc_I4_1);
				cursor.GotoNext
				(
					i=>i.MatchLdarg(0),
					i=>i.MatchLdfld<HeroController>("cState"),
					i=>i.MatchLdcI4(0),
					i=>i.MatchStfld<HeroControllerStates>("touchingWall")
				);
				cursor.RemoveRange(4);
			}
			if(oldSetStartingMotionState<1578)
			{
				cursor.GotoNext
				(
					i=>i.MatchLdarg(0),
					i=>i.MatchCall<HeroController>("ResetAirMoves")
				);
				cursor.RemoveRange(2);
			}
		}
	}
}
