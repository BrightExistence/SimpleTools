using System;
using System.Collections.Generic;
using Pipliz.APIProvider.Science;
using Server.Science;

namespace BrightExistence.SimpleTools
{
    public static class ResearchHelper
    {

    }

    /// <summary>
    /// Custom IReasearchable Implimentation which handles custom research as well as replacement of vanilla research.
    /// </summary>
    public class SimpleResearchable : IResearchable
    {
        /// <summary>
        /// Internal storage of constructor provided mod namespace.
        /// </summary>
        protected string NAMESPACE;

        /// <summary>
        /// Name of researchable omitting prefixes. ex: 'MyItem' NOT 'MyHandle.MyMod.MyItem'
        /// </summary>
        public string Name = "New Researchable";

        /// <summary>
        /// Specifies a string key of a vanilla research which should be removed and replaced with this one if it exists.
        /// </summary>
        public string Replaces { get; set; }

        /// <summary>
        /// Path to the icon file.
        /// </summary>
        public string Icon = "";

        /// <summary>
        /// How many times must the recipe be 'crafted' to be completed.
        /// </summary>
        public int IterationCount = 1;

        /// <summary>
        /// String list of other research id's which must be completed first.
        /// </summary>
        public List<string> Dependencies = new List<string>();

        /// <summary>
        /// Internal list of InventoryItem objects required. Populated in the Register() method.
        /// </summary>
        protected List<InventoryItem> Requirements = new List<InventoryItem>();

        /// <summary>
        /// List of ItemShell objects which must be turned into InventoryItems by the Register() method.
        /// </summary>
        public List<ItemShell> IterationRequirements = new List<ItemShell>();

        /// <summary>
        /// List of Unlock objects describing specific recipies which will be enable automatically when this researchable is completed in-game.
        /// </summary>
        public List<Unlock> Unlocks = new List<Unlock>();

        /// <summary>
        /// Set to false in order to disable auto-registration of this Research.
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strName">Name of research excluding prefixes. Ex: MyResearch NOT MyHandle.MyMod.MyResearch</param>
        /// <param name="strNAMESPACE">Prefix of research. Ex: 'MyHandle.MyMod'. Will use mod namespace if not provided.</param>
        public SimpleResearchable(string strName, string strNAMESPACE = null)
        {
            if (strName != null && strName.Length > 0) Name = strName;
            NAMESPACE = strNAMESPACE;
            Variables.Researchables.Add(this);
            Pipliz.Log.Write("{0}: Initialized Researchable {1} (it is not yet registered.)", NAMESPACE == null ? "" : NAMESPACE, this.Name);
        }

        /// <summary>
        /// Registers this research with the server.
        /// </summary>
        public void Register ()
        {
            if (enabled)
            {
                // Convert shell items to real items.
                foreach (ItemShell I in IterationRequirements)
                {
                    if (Variables.itemsMaster == null)
                    {
                        Pipliz.Log.WriteError("{0} CRITICAL ERROR: SimpleResearchable {1} cannot register properly because 'Variables.itemsMaster' is still null.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                    }
                    else
                    {
                        Pipliz.Log.Write("{0}: Converting shell references in researchable {1} to InventoryItem objects.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                        if (Variables.itemsMaster.ContainsKey(I.strItemkey))
                        {
                            Requirements.Add(new InventoryItem(I.strItemkey, I.intAmount));
                        }
                        else
                        {
                            Pipliz.Log.Write("{0} Researchable {1} was given an item key '{2}' as an iteration requirement which was not found by the server.", NAMESPACE == null ? "" : NAMESPACE, this.Name, I.strItemkey);
                        }
                    }
                }

                ScienceManager.RegisterResearchable(this);
                Pipliz.Log.Write("{0}: Researchable {1} has been registered with the ScienceManager.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
            else
            {
                Pipliz.Log.Write("{0}: Research {1} has been disabled, and will NOT be registered.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
        }

        /// <summary>
        /// Add an item required for each research 'crafting' cycle.
        /// </summary>
        /// <param name="itemKey">Valid item key as string.</param>
        /// <param name="amount">Number of item required.</param>
        public void addRequirement(string itemKey, int amount = 1)
        {
            if (itemKey == null || itemKey.Length < 1)
            {
                Pipliz.Log.Write("{0}: Research {1} was given a null or invalid item key.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
            else
            {
                IterationRequirements.Add(new ItemShell(itemKey, amount));
            }
        }

        /// <summary>
        /// Add an item required for each research 'crafting' cycle.
        /// </summary>
        /// <param name="requiredItem">Valid item as SimpleItem. </param>
        /// <param name="amount">Number of item required.</param>
        public void addRequirement(SimpleItem requiredItem, int amount = 1)
        {
            if (requiredItem == null || requiredItem.Name.Length < 1)
            {
                Pipliz.Log.Write("{0}: Research {1} was given a null or invalid SimpleItem object.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
            else
            {
                IterationRequirements.Add(new ItemShell(requiredItem.ID, amount));
                if (!requiredItem.enabled) this.enabled = false;
            }
        }

        /// <summary>
        /// This research's identifying key. Used by ScienceManager.
        /// </summary>
        /// <returns>This research's identifying key as string.</returns>
        public string GetKey()
        {
            if (Replaces == null) return NAMESPACE + ".Research." + Name;
            else return Replaces;
        }

        /// <summary>
        /// This research's icon path. Used by ScienceManager.
        /// </summary>
        /// <returns>This research's icon path as string.</returns>
        public string GetIcon()
        {
            return Icon;
        }

        /// <summary>
        /// Research which must be completed before this one becomes available.
        /// </summary>
        /// <returns>This research's dependencies as string list.</returns>
        public IList<string> GetDependencies()
        {
            return Dependencies;
        }

        /// <summary>
        /// This research's requirements. Used by ScienceManager.
        /// </summary>
        /// <returns>This research's requirements as InventoryItem list. Populated by the Register() method.</returns>
        public IList<InventoryItem> GetScienceRequirements()
        {
            return Requirements;
        }

        /// <summary>
        /// Number of times this recipe must be 'crafted' to be completed. Used by ScienceManager.
        /// </summary>
        /// <returns>This research's dependencies as string list.</returns>
        public int GetResearchIterationCount()
        {
            return IterationCount;
        }

        /// <summary>
        /// Called when this researchable is completed by a player.
        /// </summary>
        /// <param name="manager">Player's individual science manager.</param>
        /// <param name="reason">Will equal EResearchCompletionReason.ProgressCompleted when this research is completed by a player.</param>
        public void OnResearchComplete(ScienceManagerPlayer manager, EResearchCompletionReason reason)
        {
            if (reason == EResearchCompletionReason.ProgressCompleted)
            {
                foreach (Unlock U in Unlocks)
                {
                    if (U.limitType != null)
                    {
                        RecipeStorage.GetPlayerStorage(manager.Player).SetRecipeAvailability(U.NPCCrafted, true, U.limitType);
                    }
                    if (U.PlayerCrafted != null)
                    {
                        RecipePlayer.UnlockOptionalRecipe(manager.Player, U.PlayerCrafted);
                    }
                }
            }
        }

        /// <summary>
        /// Utility class for describing what recipes this research unlocks when it is completed.
        /// </summary>
        public class Unlock
        {
            /// <summary>
            /// Recipe name excluding prefix. Ex: SpecifiedRecipe NOT Handle.LimitType.Recipe
            /// </summary>
            public string recipeName = "";

            /// <summary>
            /// Generated by constructor. A recipe key as it would have been added to a player's crafting recipes by SimpleItem. May be null.
            /// </summary>
            public string PlayerCrafted = null;

            /// <summary>
            /// Generated by constructor. A recipe key as it would have been added to a specified limit type by SimpleRecipe. May be null.
            /// </summary>
            public string NPCCrafted = null;

            /// <summary>
            /// Limit type to which recipe would have been added by SimpleRecipe.
            /// </summary>
            public string limitType = null;


            /// <summary>
            /// Set to false to disable auto registration for any recipes which are generated by a SimpleRecipe object which is passed this object.
            /// </summary>
            public bool enabled = true;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="strRecipeName">Name of recipe excluding prefix. Ex: TargetedRecipe NOT Handle.LimitType.TargetedRecipe OR Player.TargetedRecipe</param>
            /// <param name="strLimitType">The limit type to which the recipe was added by SimpleRecipe. My be left null.</param>
            public Unlock(string strRecipeName, string strLimitType = null)
            {
                if (strRecipeName != null)
                {
                    recipeName = strRecipeName;
                    if (strLimitType != null)
                    {
                        limitType = strLimitType;
                        NPCCrafted = strLimitType + "." + strRecipeName;
                    }
                    else
                    {
                        PlayerCrafted = "Player." + strRecipeName;
                    }
                }
                else
                {
                    throw new ArgumentException("Unlock: strRecipeName parameter for Unlock object may not be left null.");
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="unlockMe">A SimpleRecipe object from which to build data.</param>
            public Unlock(SimpleRecipe unlockMe)
            {
                if (unlockMe == null)
                {
                    throw new ArgumentException("Unlock: Unlock constructor argument unlockMe may not be null.");
                }
                else
                {
                    if (unlockMe.limitType != null)
                    {
                        this.limitType = unlockMe.limitType;
                        this.NPCCrafted = unlockMe.limitType + "." + unlockMe.Name;
                    }
                    if (unlockMe.userCraftable) PlayerCrafted = "Player." + unlockMe.Name;
                    if (!unlockMe.enabled) this.enabled = false;
                }
            }
        }
    }
}
