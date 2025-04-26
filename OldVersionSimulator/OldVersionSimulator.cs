using Modding;
using System.Collections.Generic;
namespace OldVersionSimulator
{
	public class OldVersionSimulator:Mod,IMenuMod
	{
		public bool ToggleButtonInsideMenu=>true;
		private bool oldTakeHealth;
		public override string GetVersion()=>"1.0.0.0";
		public override void Initialize()
		{
			oldTakeHealth=false;
		}
		public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)=>
			new List<IMenuMod.MenuEntry>
			{
				new IMenuMod.MenuEntry
				{
					Name="1.4 TakeHealth",
					Description="Damage double of rest when lifeblood is less than it",
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
				}
			};
		private void oldTakeHealth_(On.PlayerData.orig_TakeHealth orig,PlayerData self,int amount)
		{
			if(self.healthBlue>0&&amount>self.healthBlue)
				amount+=amount-self.healthBlue;
			orig(self,amount);
		}
	}
}