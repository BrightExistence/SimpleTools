using System.Collections.Generic;
using BrightExistence.SimpleTools;
using System.IO;

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

        // DECLARE ASSETS HERE
        // example assets:
        public static SpecificTexture MyTexture = new SpecificTexture("MyTexture", NAMESPACE);
        public static SimpleItem MyItem = new SimpleItem("MyItemName", NAMESPACE, true);
        public static SimpleRecipe MyItemRecipe = new SimpleRecipe(MyItem, "pipliz.crafter");
        /* Note that the above example recipe was given a limit type of 'pipliz.crafter' which will add the
         * recipe to the workbench recipe list. You could give it an entirely different limit type, or no
         * limit type at all for a recipe which can only be crafted by hand.
         */
        
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
            // POPULATE TEXTURE DATA
            // example texture
            MyTexture.AlbedoPath = UtilityFunctions.albedoPath("MyTextureFile.png", NAMESPACE);
            /* Note that the above lines assumes that the .png file will be located in a "textures/albedo"
             * folder within the mod folder. Height, emissive, and normal .png files may be used to augment
             * the texture in the same way.
             */
            // your texture here

            // register them
            UtilityFunctions.registerTextures();
        }

        /// <summary>
        /// The afterAddingBaseTypes entrypoint. Used for adding blocks.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, NAMESPACE == null ? "" : NAMESPACE + ".afterAddingBaseTypes")]
        public static void afterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            // POPULATE ITEM DATA
            // example item
            MyItem.isSolid = true;
            MyItem.isPlaceable = true;
            MyItem.Icon = UtilityFunctions.iconPath("MyItemIcon.png", NAMESPACE);
            /* Note that the iconPath() method assumes the icon file will be located in an
             * 'icons' folder within the mod folder.
             */
            // your item here

            // register them
            UtilityFunctions.registerItems(items);
        }

        /// <summary>
        /// The afterItemType callback entrypoint. Used for registering recipes.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            // POPULATE RECIPE DATA
            // example recipe
            MyItemRecipe.userCraftable = true;
            MyItemRecipe.Requirements.Add(new ItemShell("planks", 2));
            /* Note that the example used an 'ItemShell' object to represent an existing in-game item,
             * but you could pass it an item you created, as-in:
             * MyItemRecipe.addRequirement(MyItem, 1);
             */
            // your recipe here

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
            // POPULATE RESEARCHABLES HERE

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
            /* Note that your localizations should be in a 'translation.json' file located
             * within an appropriate subdirectory under the 'localization' folder in your
             * mod's folder. Ex:
             * MyHandle/MyMod/localization/en-US/translation.json
             */
        }        
    }
}
