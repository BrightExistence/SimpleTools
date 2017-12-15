using Pipliz.JSON;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace BrightExistence.SimpleTools
{
    [ModLoader.ModManager]
    public static class Main
    {
        const string NAMESPACE = "BrightExistence.SimpleTools";
        
        /// <summary>
        /// OnAssemblyLoaded callback entrypoint. Used for mod configuration / setup.
        /// </summary>
        /// <param name="path">The starting point of mod file structure.</param>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, NAMESPACE + ".OnAssemblyLoaded")]
		public static void OnAssemblyLoaded (string path)
		{
            // Announce ourselves.
            Pipliz.Log.Write("{0} loading.", NAMESPACE);
            Pipliz.Log.Write("Built using SimpleTools version {0}", Variables.toolkitVersion);
            Pipliz.Log.Write("Thanks and credit to Pandaros for the localization routines.");

            // Get a properly formatted version of our mod directory.
            Variables.ModGamedataDirectory = Path.GetDirectoryName(path).Replace("\\", "/");
        }

        /// <summary>
        /// AfterSelectedWorld callback entry point. Used for adding textures.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, NAMESPACE + ".afterSelectedWorld"), ModLoader.ModCallbackProvidesFor("pipliz.server.registertexturemappingtextures")]
        public static void afterSelectedWorld()
        {
            // ---------------AUTOMATED TEXTURE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning texture loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateTextureObjects();
            List<SpecificTexture> AutoTextures = new List<SpecificTexture>();
            foreach (SpecificTexture Tex in Variables.SpecificTextures) AutoTextures.Add(Tex); 
            foreach (SpecificTexture thisTexture in AutoTextures) thisTexture.registerTexture();
            Pipliz.Log.Write("{0}: Texture loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// The afterAddingBaseTypes entrypoint. Used for adding blocks.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, NAMESPACE == null ? "" : NAMESPACE + ".afterAddingBaseTypes")]
        public static void afterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            Variables.itemsMaster = items;

            // ---------------(AUTOMATED BLOCK REGISTRATION)---------------
            Pipliz.Log.Write("{0}: Beginning Item loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateItemObjects();
            List<SimpleItem> AutoItems = new List<SimpleItem>();
            foreach (SimpleItem Item in Variables.Items) AutoItems.Add(Item);
            foreach (SimpleItem Item in AutoItems) Item.registerItem(items);
            Pipliz.Log.Write("{0}: Item loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// The afterItemType callback entrypoint. Used for registering jobs and recipes.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            //---------------AUTOMATED RECIPE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Recipe loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateRecipeObjects();
            foreach (SimpleRecipe Rec in Variables.Recipes) Rec.addRecipeToLimitType();
            Pipliz.Log.Write("{0}: Recipe and Job loading complete.", NAMESPACE == null ? "" : NAMESPACE);

            //---------------AUTOMATED INVENTORY BLOCK REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning crate registration.", NAMESPACE == null ? "" : NAMESPACE);
            foreach (SimpleItem Item in Variables.Items) Item.registerAsCrate();
            Pipliz.Log.Write("{0}: Crate registration complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// AfterDefiningNPCTypes callback. Used for registering jobs.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterDefiningNPCTypes")]
        [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
        public static void AfterDefiningNPCTypes()
        {
            // ---------------AUTOMATED JOBS REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Job loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateJobs();
            Pipliz.Log.Write("{0}: Job loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAddResearchables, NAMESPACE == null ? "" : NAMESPACE + ".OnAddResearchables")]
        public static void OnAddResearchables ()
        {
            //---------------AUTOMATED RESEARCHABLE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Research loading.", NAMESPACE == null ? "" : NAMESPACE);
            Data.populateResearchObjects();
            foreach(SimpleResearchable R in Variables.Researchables)
            {
                R.Register();
            }
            Pipliz.Log.Write("{0}: Research loading complete.", NAMESPACE == null ? "" : NAMESPACE);
        }

        /// <summary>
        /// AfterWorldLoad callback entry point. Used for localization routines.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, NAMESPACE == null ? "" : NAMESPACE + ".AfterWorldLoad")]
        [ModLoader.ModCallbackDependsOn("pipliz.server.localization.waitforloading")]
        [ModLoader.ModCallbackProvidesFor("pipliz.server.localization.convert")]
        public static void AfterWorldLoad ()
        // BEGIN borrowed Pandaros code..............................................
        {
            try
            {
                string[] array = new string[]
                {
                    "translation.json"
                };
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    string[] files = Directory.GetFiles(Path.Combine(Variables.ModGamedataDirectory,"localization"), text, SearchOption.AllDirectories);
                    string[] array2 = files;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text2 = array2[j];
                        try
                        {
                            JSONNode jsonFromMod;
                            if (JSON.Deserialize(text2, out jsonFromMod, false))
                            {
                                string name = Directory.GetParent(text2).Name;

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(text))
                                {
                                    Pipliz.Log.Write("{0}: Found mod localization file for '{1}' localization", NAMESPACE == null ? "" : NAMESPACE, name);
                                    Localize(name, text, jsonFromMod);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Pipliz.Log.Write("{0}: Exception reading localization from {1}; {2}", NAMESPACE == null ? "" : NAMESPACE, text2, ex.Message);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Pipliz.Log.Write("{0}: Localization directory not found at {1}", NAMESPACE == null ? "" : NAMESPACE, Path.Combine(Variables.ModGamedataDirectory,"localization"));
            }
        }


        public static void Localize(string locName, string locFilename, JSONNode jsonFromMod)
        {
            try
            {
                if (Server.Localization.Localization.LoadedTranslation == null)
                {
                    Pipliz.Log.Write("{0} :Unable to localize. Server.Localization.Localization.LoadedTranslation is null.", NAMESPACE == null ? "" : NAMESPACE);
                }
                else
                {
                	JSONNode jsn;
                	if (Server.Localization.Localization.LoadedTranslation.TryGetValue(locName, out jsn))
                    {
                        if (jsn != null)
                        {
                            foreach (KeyValuePair<string, JSONNode> modNode in jsonFromMod.LoopObject())
                            {
                                Pipliz.Log.Write("{0} : Adding localization for '{1}' from '{2}'.", NAMESPACE == null ? "" : NAMESPACE, modNode.Key, Path.Combine(locName, locFilename));
                                AddRecursive(jsn, modNode);
                            }
                        }
                        else
                            Pipliz.Log.Write("{0}: Unable to localize. Localization '{01 not found and is null.", NAMESPACE == null ? "" : NAMESPACE, locName);
                    }
                    else
                        Pipliz.Log.Write("{0}: Localization '{1}' not supported", NAMESPACE == null ? "" : NAMESPACE, locName);
                }

                Pipliz.Log.Write("{0}: Patched mod localization file '{1}/{2}'", NAMESPACE == null ? "" : NAMESPACE, locName, locFilename);

            }
            catch (Exception ex)
            {
                Pipliz.Log.WriteError(ex.ToString(), "{0}: Exception while localizing {1}", NAMESPACE == null ? "" : NAMESPACE, Path.Combine(locName, locFilename));
            }
        }

        private static void AddRecursive(JSONNode gameJson, KeyValuePair<string, JSONNode> modNode)
        {
            int childCount = 0;

            try
            {
                childCount = modNode.Value.ChildCount;
            }
            catch { }

            if (childCount != 0)
            {
                if (gameJson.HasChild(modNode.Key))
                {
                    foreach (var child in modNode.Value.LoopObject())
                        AddRecursive(gameJson[modNode.Key], child);
                }
                else
                {
                    gameJson[modNode.Key] = modNode.Value;
                }
            }
            else
            {
                gameJson[modNode.Key] = modNode.Value;
            }
        }
        // END borrowed Pandaros code..............................................
    }
}
