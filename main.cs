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
        /// <summary>
        /// OnAssemblyLoaded callback entrypoint. Used for mod configuration / setup.
        /// </summary>
        /// <param name="path">The starting point of mod file structure.</param>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, MyHandle.MyMod.Data.NAMESPACE + ".OnAssemblyLoaded")]
		public static void OnAssemblyLoaded (string path)
		{
            // Announce ourselves.
            Pipliz.Log.Write("Mod {0} loading.", MyHandle.MyMod.Data.NAMESPACE);
            Pipliz.Log.Write("Built using SimpleTools version {0}", Variables.toolkitVersion);
            Pipliz.Log.Write("Thanks and credit to Pandaros for the localization routines.");

            // Get a properly formatted version of our mod directory and subdirectories.
            Variables.ModGamedataDirectory = Path.GetDirectoryName(path).Replace("\\", "/");
            Variables.JobsPath = Path.Combine(Variables.ModGamedataDirectory, "jobs").Replace("\\", "/");
            Variables.IconPath = Path.Combine(Variables.ModGamedataDirectory, "icons").Replace("\\", "/");
            Variables.TexturePath = Path.Combine(Variables.ModGamedataDirectory, "Textures").Replace("\\", "/");
            Variables.MeshPath = Path.Combine(Variables.ModGamedataDirectory, "meshes").Replace("\\", "/");
            Variables.ResearchablesPath = Path.Combine(Variables.ModGamedataDirectory, "researchables").Replace("\\", "/");
        }

        /// <summary>
        /// AfterSelectedWorld callback entry point. Used for adding textures.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, MyHandle.MyMod.Data.NAMESPACE + ".afterSelectedWorld"), ModLoader.ModCallbackProvidesFor("pipliz.server.registertexturemappingtextures")]
        public static void afterSelectedWorld()
        {
            // ---------------AUTOMATED TEXTURE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning texture loading.", MyHandle.MyMod.Data.NAMESPACE);
            MyHandle.MyMod.Data.populateTextureObjects();
            foreach (SimpleTexture thisTexture in Variables.Textures) thisTexture.registerTexture();
            Pipliz.Log.Write("{0}: Texture loading complete.", MyHandle.MyMod.Data.NAMESPACE);
        }

        /// <summary>
        /// The afterAddingBaseTypes entrypoint. Used for adding blocks.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, MyHandle.MyMod.Data.NAMESPACE + ".afterAddingBaseTypes")]
        public static void afterAddingBaseTypes(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            Variables.itemsMaster = items;

            // ---------------(AUTOMATED BLOCK REGISTRATION)---------------
            MyHandle.MyMod.Data.populateItemObjects();
            Pipliz.Log.Write("{0}: Beginning Item loading.", MyHandle.MyMod.Data.NAMESPACE);
            foreach (SimpleItem Item in Variables.Items) Item.registerItem(items);
            Pipliz.Log.Write("{0}: Item loading complete.", MyHandle.MyMod.Data.NAMESPACE);
        }

        /// <summary>
        /// The afterItemType callback entrypoint. Used for registering jobs and recipes.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, MyHandle.MyMod.Data.NAMESPACE + ".AfterItemTypesDefined")]
        public static void AfterItemTypesDefined()
        {
            //---------------AUTOMATED RECIPE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Recipe loading.", MyHandle.MyMod.Data.NAMESPACE);
            MyHandle.MyMod.Data.populateRecipeObjects();
            foreach (SimpleRecipe Rec in Variables.Recipes) Rec.addRecipeToLimitType();
            Pipliz.Log.Write("{0}: Recipe and Job loading complete.", MyHandle.MyMod.Data.NAMESPACE);

            //---------------AUTOMATED INVENTORY BLOCK REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning crate registration.", MyHandle.MyMod.Data.NAMESPACE);
            foreach (SimpleItem Item in Variables.Items) Item.registerAsCrate();
            Pipliz.Log.Write("{0}: Crate registration complete.", MyHandle.MyMod.Data.NAMESPACE);
        }

        /// <summary>
        /// AfterDefiningNPCTypes callback. Used for registering jobs.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, MyHandle.MyMod.Data.NAMESPACE + ".AfterDefiningNPCTypes")]
        [ModLoader.ModCallbackProvidesFor("pipliz.apiprovider.jobs.resolvetypes")]
        public static void AfterDefiningNPCTypes()
        {
            // ---------------AUTOMATED JOBS REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Job loading.", MyHandle.MyMod.Data.NAMESPACE);
            MyHandle.MyMod.Data.populateJobs();
            Pipliz.Log.Write("{0}: Job loading complete.", MyHandle.MyMod.Data.NAMESPACE);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAddResearchables, MyHandle.MyMod.Data.NAMESPACE + ".OnAddResearchables")]
        [ModLoader.ModCallbackProvidesForAttribute("pipliz.server.loadplayers")]
        public static void OnAddResearchables ()
        {
            //---------------AUTOMATED RESEARCHABLE REGISTRATION---------------
            Pipliz.Log.Write("{0}: Beginning Research loading.", MyHandle.MyMod.Data.NAMESPACE);
            MyHandle.MyMod.Data.populateResearchObjects();
            foreach(SimpleResearchable R in Variables.Researchables)
            {
                R.Register();
            }
            Pipliz.Log.Write("{0}: Research loading complete.", MyHandle.MyMod.Data.NAMESPACE);
        }

        /// <summary>
        /// AfterWorldLoad callback entry point. Used for localization routines.
        /// </summary>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, MyHandle.MyMod.Data.NAMESPACE + ".AfterWorldLoad")]
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
                                    Pipliz.Log.Write("{0}: Found mod localization file for '{1}' localization", MyHandle.MyMod.Data.NAMESPACE, name);
                                    Localize(name, text, jsonFromMod);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Pipliz.Log.Write("{0}: Exception reading localization from {1}; {2}", MyHandle.MyMod.Data.NAMESPACE, text2, ex.Message);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Pipliz.Log.Write("{0}: Localization directory not found at {1}", MyHandle.MyMod.Data.NAMESPACE, Path.Combine(Variables.ModGamedataDirectory,"localization"));
            }
        }


        public static void Localize(string locName, string locFilename, JSONNode jsonFromMod)
        {
            try
            {
                if (Server.Localization.Localization.LoadedTranslation == null)
                {
                    Pipliz.Log.Write("{0} :Unable to localize. Server.Localization.Localization.LoadedTranslation is null.", MyHandle.MyMod.Data.NAMESPACE);
                }
                else
                {
                    if (Server.Localization.Localization.LoadedTranslation.TryGetValue(locName, out JSONNode jsn))
                    {
                        if (jsn != null)
                        {
                            foreach (KeyValuePair<string, JSONNode> modNode in jsonFromMod.LoopObject())
                            {
                                Pipliz.Log.Write("{0} : Adding localization for '{1}' from '{2}'.", MyHandle.MyMod.Data.NAMESPACE, modNode.Key, Path.Combine(locName, locFilename));
                                AddRecursive(jsn, modNode);
                            }
                        }
                        else
                            Pipliz.Log.Write("{0}: Unable to localize. Localization '{01 not found and is null.", MyHandle.MyMod.Data.NAMESPACE, locName);
                    }
                    else
                        Pipliz.Log.Write("{0}: Localization '{1}' not supported", MyHandle.MyMod.Data.NAMESPACE, locName);
                }

                Pipliz.Log.Write("{0}: Patched mod localization file '{1}/{2}'", MyHandle.MyMod.Data.NAMESPACE, locName, locFilename);

            }
            catch (Exception ex)
            {
                Pipliz.Log.WriteError(ex.ToString(), "{0}: Exception while localizing {1}", MyHandle.MyMod.Data.NAMESPACE, Path.Combine(locName, locFilename));
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

        /// <summary>
        /// Converts the name of an item to its in-game ID by prefixing NAMESPACE.
        /// </summary>
        /// <param name="itemName">Name of item.</param>
        /// <returns>Game ID of item. (NAMESPACE + Name)</returns>
        public static string getLocalID (string itemName)
        {
            return MyHandle.MyMod.Data.NAMESPACE + "." + itemName;
        }

        /// <summary>
        /// Converts the name of an icon to a full path.
        /// </summary>
        /// <param name="iconName">Name of icon file including extension but NOT directories.</param>
        /// <returns>Full path of icon file.</returns>
        public static string getLocalIcon (string iconName)
        {
            return Path.Combine(Variables.IconPath, iconName);
        }
    }
}
