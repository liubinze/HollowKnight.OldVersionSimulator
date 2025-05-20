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
		private bool oldFinishedEnteringScene;
		private bool oldTakeHealth;
		private bool oldcharmCost_11;
		private bool oldcharmCost_32;
		private int oldRuins_Lever;
		private int oldCanOpenInventory;
		private int oldCanQuickMap;
		private int oldCancelWallsliding;
		private int oldFinishedDashing;
		private int oldOnCollisionExit2D;
		private int oldSetStartingMotionState;
		private int oldSoulGain;
		public override string GetVersion()=>VersionUtil.GetVersion<OldVersionSimulator>();
		public override void Initialize()
		{
			oldFinishedEnteringScene=false;
			On.HeroController.FinishedEnteringScene+=FinishedEnteringScene;
			oldcharmCost_11=false;
			oldcharmCost_32=false;
			On.PlayerData.SetupNewPlayerData+=charmCost_;
			oldTakeHealth=false;
			On.PlayerData.TakeHealth+=TakeHealth;
			oldRuins_Lever=1315;
			On.PlayMakerFSM.OnEnable+=OnEnable;
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
			oldSetStartingMotionState=0;
			On.HeroController.SetStartingMotionState_bool+=SetStartingMotionState;
			oldSoulGain=0;
			On.HeroController.SoulGain+=SoulGain;
		}
		public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)=>
			new List<IMenuMod.MenuEntry>
			{
				new IMenuMod.MenuEntry
				{
					Name="Old FinishedEnteringScene",
					Description="(1.2 and older) Allow pause in start of tutorial",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldFinishedEnteringScene=o!=0,
					Loader=()=>this.oldFinishedEnteringScene?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old Flukenest charm cost",
					Description="(1.2 and older) 2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldcharmCost_11=o!=0,
					Loader=()=>this.oldcharmCost_11?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old Quick Slash charm cost",
					Description="(1.0.0.7 and older) 2 instead of 3 when creating new save",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldcharmCost_32=o!=0,
					Loader=()=>this.oldcharmCost_32?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old TakeHealth",
					Description="(1.4) Damage double the rest when lifeblood is less than it",
					Values=new string[]{"Off","On"},
					Saver=o=>this.oldTakeHealth=o!=0,
					Loader=()=>this.oldTakeHealth?1:0
				},
				new IMenuMod.MenuEntry
				{
					Name="Old Ruins Lever FSM",
					Description="Make some levers hittable behind walls",
					Values=new string[]{"1006-1028","1221","1315-1578"},
					Saver=o=>this.oldRuins_Lever=o switch{0=>1006,1=>1221,2=>1315,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldRuins_Lever switch{1006=>0,1221=>1,1315=>2,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanOpenInventory",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1432","1578"},
					Saver=o=>this.oldCanOpenInventory=o switch{0=>0,1=>1006,2=>1028,3=>1315,4=>1578,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldCanOpenInventory switch{0=>0,1006=>1,1028=>2,1315=>3,1578=>4,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CanQuickMap",
					Description="",
					Values=new string[]{"Off","1006","1028-1221","1315-1578"},
					Saver=o=>this.oldCanQuickMap=o switch{0=>0,1=>1006,2=>1028,3=>1315,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldCanQuickMap switch{0=>0,1006=>1,1028=>2,1315=>3,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old CancelWallsliding",
					Description="Allow clinging with WCS",
					Values=new string[]{"Off","1006-1221","1315-1578"},
					Saver=o=>this.oldCancelWallsliding=o switch{0=>0,1=>1006,2=>1315,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldCancelWallsliding switch{0=>0,1006=>1,1315=>2,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old FinishedDashing",
					Description="Retain WCS when dashing",
					Values=new string[]{"Off","1006-1221","1315","1424-1578"},
					Saver=o=>this.oldFinishedDashing=o switch{0=>0,1=>1006,2=>1315,3=>1424,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldFinishedDashing switch{0=>0,1006=>1,1315=>2,1424=>3,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old OnCollisionExit2D",
					Description="Retain WCS on collision exit",
					Values=new string[]{"Off","1006","1028","1221-1432","1578"},
					Saver=o=>this.oldOnCollisionExit2D=o switch{0=>0,1=>1006,2=>1028,3=>1221,4=>1578,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldOnCollisionExit2D switch{0=>0,1006=>1,1028=>2,1221=>3,1578=>4,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old SetStartingMotionState",
					Description="Retain WCS through transitions",
					Values=new string[]{"Off","1006-1221","1315-1432","1578"},
					Saver=o=>this.oldSetStartingMotionState=o switch{0=>0,1=>1006,2=>1315,3=>1578,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldSetStartingMotionState switch{0=>0,1006=>1,1315=>2,1578=>3,_=>throw new InvalidOperationException()}
				},
				new IMenuMod.MenuEntry
				{
					Name="Old SoulGain",
					Description="Weaken Soul Eater",
					Values=new string[]{"Off","1006","1028-1578"},
					Saver=o=>this.oldSoulGain=o switch{0=>0,1=>1006,2=>1028,_=>throw new InvalidOperationException()},
					Loader=()=>this.oldSoulGain switch{0=>0,1006=>1,1028=>2,_=>throw new InvalidOperationException()}
				}
			};
		private void FinishedEnteringScene(On.HeroController.orig_FinishedEnteringScene orig,HeroController self,bool setHarzardMarker,bool preventRunBob)
		{
			if(oldFinishedEnteringScene)
				self.isEnteringFirstLevel=false;
			orig(self,setHarzardMarker,preventRunBob);
		}
		private void charmCost_(On.PlayerData.orig_SetupNewPlayerData orig,PlayerData self)
		{
			orig(self);
			if(oldcharmCost_11)
				self.charmCost_11=2;
			if(oldcharmCost_32)
				self.charmCost_32=2;
		}
		private void TakeHealth(On.PlayerData.orig_TakeHealth orig,PlayerData self,int amount)
		{
			if(oldTakeHealth&&self.healthBlue>0&&amount>self.healthBlue)
				amount+=amount-self.healthBlue;
			orig(self,amount);
		}
		private void OnEnable(On.PlayMakerFSM.orig_OnEnable orig,PlayMakerFSM self)
		{
			switch(self.FsmName)
			{
				case"Switch Control"when self.name.StartsWith("Ruins Lever"):
					if(oldRuins_Lever<1221)
						self.ChangeTransition("Idle","NAIL HIT","Check If Nail");
					else if(oldRuins_Lever<1315)
						self.ChangeTransition("Idle","NAIL HIT","Player Data?");
					break;
			}
			orig(self);
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
			return!GameManager.instance.isPaused
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
		private bool CheckStillTouchingWall(HeroController self,CollisionSide side,bool checkTop=false)
		{
			Collider2D col2d=Mirror.GetField<HeroController,Collider2D>(self,"col2d");
			Vector2 origin=new Vector2(col2d.bounds.min.x,col2d.bounds.max.y);
			Vector2 origin2=new Vector2(col2d.bounds.min.x,col2d.bounds.center.y);
			Vector2 origin3=new Vector2(col2d.bounds.min.x,col2d.bounds.min.y);
			Vector2 origin4=new Vector2(col2d.bounds.max.x,col2d.bounds.max.y);
			Vector2 origin5=new Vector2(col2d.bounds.max.x,col2d.bounds.center.y);
			Vector2 origin6=new Vector2(col2d.bounds.max.x,col2d.bounds.min.y);
			float distance=0.1f;
			RaycastHit2D raycastHit2D=default(RaycastHit2D);
			RaycastHit2D raycastHit2D2=default(RaycastHit2D);
			RaycastHit2D raycastHit2D3=default(RaycastHit2D);
			if(side==CollisionSide.left)
			{
				if(checkTop)
					raycastHit2D=Physics2D.Raycast(origin,Vector2.left,distance,256);
				raycastHit2D2=Physics2D.Raycast(origin2,Vector2.left,distance,256);
				raycastHit2D3=Physics2D.Raycast(origin3,Vector2.left,distance,256);
			}
			else
			{
				if(side!=CollisionSide.right)
				{
					Debug.LogError("Invalid CollisionSide specified.");
					return false;
				}
				if(checkTop)
					raycastHit2D=Physics2D.Raycast(origin4,Vector2.right,distance,256);
				raycastHit2D2=Physics2D.Raycast(origin5,Vector2.right,distance,256);
				raycastHit2D3=Physics2D.Raycast(origin6,Vector2.right,distance,256);
			}
			if(raycastHit2D2.collider!=null)
			{
				bool flag=true;
				if(raycastHit2D2.collider.isTrigger)
					flag=false;
				if(raycastHit2D2.collider.GetComponent<SteepSlope>()!=null)
					flag=false;
				if(raycastHit2D2.collider.GetComponent<NonSlider>()!=null)
					flag=false;
				if(flag)
					return true;
			}
			if(raycastHit2D3.collider!=null)
			{
				bool flag2=true;
				if(raycastHit2D3.collider.isTrigger)
					flag2=false;
				if(raycastHit2D3.collider.GetComponent<SteepSlope>()!=null)
					flag2=false;
				if(raycastHit2D3.collider.GetComponent<NonSlider>()!=null)
					flag2=false;
				if(flag2)
					return true;
			}
			if(checkTop&&raycastHit2D.collider!=null)
			{
				bool flag3=true;
				if(raycastHit2D.collider.isTrigger)
					flag3=false;
				if(raycastHit2D.collider.GetComponent<SteepSlope>()!=null)
					flag3=false;
				if(raycastHit2D.collider.GetComponent<NonSlider>()!=null)
					flag3=false;
				if(flag3)
					return true;
			}
			return false;
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
				if(self.touchingWallL&&!CheckStillTouchingWall(self,CollisionSide.left,false))
				{
					self.cState.touchingWall=false;
					self.touchingWallL=false;
				}
				if(self.touchingWallR&&!CheckStillTouchingWall(self,CollisionSide.right,false))
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
		private void SetStartingMotionState(On.HeroController.orig_SetStartingMotionState_bool orig,HeroController self,bool preventRunDip)
		{
			if(oldSetStartingMotionState==0)
			{
				orig(self,preventRunDip);
				return;
			}
			self.move_input=((oldSetStartingMotionState<1315||self.acceptingInput||preventRunDip)?InputHandler.Instance.inputActions.moveVector.X:0f);
			if(oldSetStartingMotionState>=1315)
				self.cState.touchingWall=false;
			if(self.CheckTouchingGround())
			{
				self.cState.onGround=true;
				SetState(self,ActorStates.grounded);
				if(oldSetStartingMotionState>=1578)
					self.ResetAirMoves();
				if(Mirror.GetField<HeroController,bool>(self,"enteringVertically"))
				{
					self.SpawnSoftLandingPrefab();
					Mirror.GetFieldRef<HeroController,HeroAnimationController>(self,"animCtrl").playLanding=true;
					Mirror.SetField<HeroController,bool>(self,"enteringVertically",false);
				}
			}
			else
			{
				self.cState.onGround=false;
				SetState(self,ActorStates.airborne);
			}
			Mirror.GetFieldRef<HeroController,HeroAnimationController>(self,"animCtrl").UpdateState(self.hero_state);
		}
		private void SoulGain(On.HeroController.orig_SoulGain orig,HeroController self)
		{
			if(oldSoulGain==0)
			{
				orig(self);
				return;
			}
			int num;
			if(self.playerData.MPCharge<self.playerData.maxMP)
			{
				num=11;
				if(self.playerData.equippedCharm_20)
					num+=3;
				if(self.playerData.equippedCharm_21)
					num+=oldSoulGain<1028?6:8;
			}
			else
			{
				num=6;
				if(self.playerData.equippedCharm_20)
					num+=2;
				if(self.playerData.equippedCharm_21)
					num+=oldSoulGain<1028?4:6;
			}
			int mpreserve=self.playerData.MPReserve;
			self.playerData.AddMPCharge(num);
			GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN");
			if(self.playerData.MPReserve!=mpreserve)
				GameManager.instance.soulVessel_fsm.SendEvent("MP RESERVE UP");
		}
	}
}
