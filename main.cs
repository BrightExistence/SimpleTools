using System.Collections.Generic;
using BrightExistence.SimpleTools;

namespace MyHandle.MyMod
{

    /// <summary>
    /// Internal class which uses callbacks to register this mod's data.
    /// </summary>
    [ModLoader.ModManager]
    public static class Main
    {
        /// <summary>
        /// Make sure this string matches the namespace above, or these methods will not execute.
        /// </summary>
        const string NAMESPACE = "MyHandle.MyMod";
        
        /// <summary>
        /// OnAssemblyLoaded callback entrypoint. Used for mod configuration / setup.
        /// </summary>
        /// <param name="path">The starting point of mod file structure.</param>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, NAMESPACE + ".OnAssemblyLoaded")]
		public static void OnAssemblyLoaded (string path)
		{
            // Announce ourselves.
            Pipliz.Log.Write("{0} loading.", path);
            Pipliz.Log.Write("Built using SimpleTools version {0}", Variables.toolkitVersion);
            Pipliz.Log.Write("Thanks and credit to Pandaros for the localization routines.");

            // capture mod directory
            Variables.modDirectory = Path.GetDirectoryName(path);
        }

        /// <summary>
        /// AfterSelectedWorld callback entry point. Used for adding textures.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, NAMESPACE + ".afterSelectedWorld"), ModLoader.ModCallbackProvidesFor("pipliz.server.registertexturemappingtextures")]
        public static void afterSelectedWorld()
        {
            // make your textures

            // register them
            UtilityFunctions.registerTextures();
        }

        /// <summary>
        /// The afterAddingBaseTypes entrypoint. Used for adding blocks.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, NAMESPACE == null ? "" : NAMESPACE + ".afterAddingBaseTypes")]
        public static void afterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            // make your items

            // register them
            UtilityFunctions.registerItems(items);
        }

        /// <summary>
        /// The afterItemType callback entrypoint. Used for registering recipes.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            // make your recipes

            // register them
            UtilityFunctions.recipesAndInventoryBlocks();
        }

        /// <summary>
        /// AfterDefiningNPCTypes callback. Used for registering jobs.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterDefiningNPCTypes")]
        [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
        public static void AfterDefiningNPCTypes()
        {
            // ---------------JOBS REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning manual job loading.", NAMESPACE == null ? "" : NAMESPACE);
            // register jobs here.
            Pipliz.Log.Write("{0}: Manual job loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// OnAddResearchables callback. Used to populate and register SimpleResearchable objects.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAddResearchables, NAMESPACE == null ? "" : NAMESPACE + ".OnAddResearchables")]
        public static void OnAddResearchables ()
        {
            // make your researchables

            // register them
            UtilityFunctions.registerResearchables();
        }

        /// <summary>
        /// AfterWorldLoad callback entry point. Used for localization routines.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, NAMESPACE == null ? "" : NAMESPACE + ".AfterWorldLoad")]
        [ModLoader.ModCallbackDependsOn("pipliz.server.localization.waitforloading")]
        [ModLoader.ModCallbackProvidesFor("pipliz.server.localization.convert")]
        public static void AfterWorldLoad ()
        {
            // let simpletools do this
            UtilityFunctions.loadLocalizationFiles();
        }        
    }
}
