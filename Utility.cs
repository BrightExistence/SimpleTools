using BrightExistence.SimpleTools;
using Pipliz.JSON;
using System.Collections.Generic;
using System.IO;

namespace BrightExistence.SimpleTools
{
    /// <summary>
    /// A set of utility functions for getting file paths.
    /// </summary>
    public static class UtilityFunctions
    {
        /// <summary>
        /// OnAssemblyLoaded callback entrypoint. Used for mod configuration / setup.
        /// </summary>
        /// <param name="path">The starting point of mod file structure.</param>
        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, "BrightExistence.SimpleTools.OnAssemblyLoaded")]
        public static void OnAssemblyLoaded(string path)
        {
            // Announce ourselves.
            Pipliz.Log.Write("{0} loading.", path);
            Pipliz.Log.Write("Built using SimpleTools version {0}", Variables.toolkitVersion);
            Pipliz.Log.Write("Thanks and credit to Pandaros for the localization routines.");

            // capture mod directory
            Variables.modDirectory = path;
        }

        /// <summary>
        /// Registers all SimpleTexture objects. Should be called within the callback:
        /// [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, NAMESPACE + ".afterSelectedWorld"), ModLoader.ModCallbackProvidesFor("pipliz.server.registertexturemappingtextures")]
        /// </summary>
        public static void registerTextures()
        {
            // ---------------AUTOMATED TEXTURE REGISTRATION---------------
            Pipliz.Log.Write("{0} ({1}): Beginning texture registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            //foreach (SimpleMod Mod in Variables.SimpleMods) Mod.populateTextureObjects();
            List<SpecificTexture> AutoTextures = new List<SpecificTexture>();
            foreach (SpecificTexture Tex in Variables.SpecificTextures) AutoTextures.Add(Tex);
            foreach (SpecificTexture thisTexture in AutoTextures) thisTexture.registerTexture();
            Pipliz.Log.Write("{0} ({1}): Texture registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
        }

        /// <summary>
        /// Registers all SimpleItem objects. Should be called within the callback:
        /// [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterAddingBaseTypes, NAMESPACE == null ? "" : NAMESPACE + ".afterAddingBaseTypes")]
        /// </summary>
        /// <param name="items">Server's master list of items.</param>
        public static void registerItems (Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            // Store items reference.
            Variables.itemsMaster = items;

            // ---------------(AUTOMATED BLOCK REGISTRATION)---------------
            Pipliz.Log.Write("{0} ({1}): Beginning item registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            //foreach (SimpleMod Mod in Variables.SimpleMods) Mod.populateItemObjects();
            List<SimpleItem> AutoItems = new List<SimpleItem>();
            foreach (SimpleItem Item in Variables.Items) AutoItems.Add(Item);
            foreach (SimpleItem Item in AutoItems) Item.registerItem(items);
            Pipliz.Log.Write("{0} ({1}): Item registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
        }

        /// <summary>
        /// Registers all SimpleRecipe objects, as well as registering inventory type items as such. Should be called within:
        /// [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, NAMESPACE == null ? "" : NAMESPACE + ".AfterItemTypesDefined")]
        /// </summary>
        public static void recipesAndInventoryBlocks ()
        {
            //---------------AUTOMATED RECIPE REGISTRATION---------------
            Pipliz.Log.Write("{0} ({1}): Beginning recipe registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            //foreach (SimpleMod Mod in Variables.SimpleMods) Mod.populateRecipeObjects();
            foreach (SimpleRecipe Rec in Variables.Recipes) Rec.addRecipeToLimitType();
            Pipliz.Log.Write("{0} ({1}): Recipe registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);

            //---------------AUTOMATED INVENTORY TYPE BLOCK REGISTRATION---------------
            Pipliz.Log.Write("{0} ({1}): Beginning block types registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            foreach (SimpleItem Item in Variables.Items) Item.registerAsCrate();
            Pipliz.Log.Write("{0} ({1}): Blocks type registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
        }

        /// <summary>
        /// NOT YET IMPLIMENTED. DO NOT USE.
        /// </summary>
        public static void registerJobs ()
        {
            // ---------------AUTOMATED JOBS REGISTRATION---------------
            Pipliz.Log.Write("{0} ({1}): Beginning jobs registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            // NOT YET IMPLIMENTED
            Pipliz.Log.Write("{0} ({1}): Jobs registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
        }

        /// <summary>
        /// Registers SimpleResearchable objects. Should be called during callback:
        /// [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAddResearchables, NAMESPACE == null ? "" : NAMESPACE + ".OnAddResearchables")]
        /// </summary>
        public static void registerResearchables()
        {
            //---------------AUTOMATED RESEARCHABLE REGISTRATION---------------
            Pipliz.Log.Write("{0} ({1}): Beginning researchables registration.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
            //foreach (SimpleMod Mod in Variables.SimpleMods) Mod.populateResearchObjects();
            foreach (SimpleResearchable R in Variables.Researchables)
            {
                R.Register();
            }
            Pipliz.Log.Write("{0} ({1}): Researchables registration complete.", "SimpleTools v" + Variables.toolkitVersion, Variables.modDirectory == null ? "" : Variables.modDirectory);
        }

        /// <summary>
        /// Automatically searches for and loads any localization files in the mod's directory. Pay attention to proper directory
        /// structure or localization will not be loaded correctly. For example the en-US localization file should be located in a
        /// '[moddirectory]/localization/en-US' folder. Should be called during callback:
        /// [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterWorldLoad, NAMESPACE == null ? "" : NAMESPACE + ".AfterWorldLoad")]
        /// [ModLoader.ModCallbackDependsOn("pipliz.server.localization.waitforloading")]
        /// [ModLoader.ModCallbackProvidesFor("pipliz.server.localization.convert")]
        /// </summary>
        public static void loadLocalizationFiles()
        // BEGIN borrowed Pandaros code (comments added)...........................
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
                    string[] files = Directory.GetFiles(Path.Combine(Variables.modDirectory, "localization"), text, SearchOption.AllDirectories);
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
                                    Localize(name, text, jsonFromMod);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Pipliz.Log.WriteError(ex.Message);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locName">The name of the directory in which the localization file is located, ex: 'en-US'</param>
        /// <param name="locFilename">The name of the file itself.</param>
        /// <param name="jsonFromMod">The file's contents as a JSONNode.</param>
        public static void Localize(string locName, string locFilename, JSONNode jsonFromMod)
        {
            try
            {
                if (Server.Localization.Localization.LoadedTranslation == null)
                {
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
                                AddRecursive(jsn, modNode);
                            }
                        }
                    }
                }


            }
            catch (System.Exception ex)
            {
                Pipliz.Log.WriteError(ex.Message);
            }
        }

        /// <summary>
        /// Given the game's loaded localization JSON, and a keyvaluepair containing a JSONNode and its key,
        /// recursively adds that JSONNode's contents and its children's contents to the server's loaded localization JSON.
        /// </summary>
        /// <param name="gameJson">The server's loaded localization JSON.</param>
        /// <param name="modNode">A JSONNode object containing data to be added to the server's JSON.</param>
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
        /// Relative path to icon files based on the string NAMESPACE.
        /// </summary>
        /// <param name="iconName">Filename of icon file.</param>
        /// <param name="NAMESPACE">Namespace of mod, assumed to correspond to the directory the mod is in.</param>
        /// <returns>A completed relative path to the icon file.</returns>
        public static string iconPath(string iconName, string NAMESPACE)
        {
            return "gamedata/mods/" + NAMESPACE.Replace('.', '/') + "/icons/" + iconName;
        }

        /// <summary>
        /// Relative path to an albedo texture file based on the string NAMESPACE.
        /// </summary>
        /// <param name="textureName">Filename of an albedo texture file.</param>
        /// <param name="NAMESPACE">Namespace of mod, assumed to correspond to the directory the mod is in.</param>
        /// <returns>A completed relative path to the icon file.</returns>
        public static string albedoPath(string textureName, string NAMESPACE)
        {
            return "gamedata/mods/" + NAMESPACE.Replace('.', '/') + "/textures/albedo/" + textureName;
        }

        /// <summary>
        /// Relative path to an emissive texture file based on the string NAMESPACE.
        /// </summary>
        /// <param name="textureName">Filename of an emissive texture file.</param>
        /// <param name="NAMESPACE">Namespace of mod, assumed to correspond to the directory the mod is in.</param>
        /// <returns>A completed relative path to the icon file.</returns>
        public static string emissivepath(string textureName, string NAMESPACE)
        {
            return "gamedata/mods/" + NAMESPACE.Replace('.', '/') + "/textures/emissive/" + textureName;
        }

        /// <summary>
        /// Relative path to a height texture file based on the string NAMESPACE.
        /// </summary>
        /// <param name="textureName">Filename of a height texture file.</param>
        /// <param name="NAMESPACE">Namespace of mod, assumed to correspond to the directory the mod is in.</param>
        /// <returns>A completed relative path to the icon file.</returns>
        public static string heightpath(string textureName, string NAMESPACE)
        {
            return "gamedata/mods/" + NAMESPACE.Replace('.', '/') + "/textures/height/" + textureName;
        }

        /// <summary>
        /// Relative path to a 'normal' texture file based on the string NAMESPACE.
        /// </summary>
        /// <param name="textureName">Filename of a 'normal' texture file.</param>
        /// <param name="NAMESPACE">Namespace of mod, assumed to correspond to the directory the mod is in.</param>
        /// <returns>A completed relative path to the icon file.</returns>
        public static string normalpath(string textureName, string NAMESPACE)
        {
            return "gamedata/mods/" + NAMESPACE.Replace('.', '/') + "/textures/normal/" + textureName;
        }

        /// <summary>
        /// Attempts to remove an existing recipe from the server's database.
        /// </summary>
        /// <param name="recName">Name of recipe.</param>
        /// <param name="NAMESPACE">This mod's namespace.</param>
        /// <returns>True if recipe was removed, False if recipe was not found or removal was not successful.</returns>
        public static bool tryRemoveRecipe(string recName, string NAMESPACE = null)
        {
            try
            {
                Recipe Rec;
                if (RecipeStorage.TryGetRecipe(recName, out Rec))
                {
                    Pipliz.Log.Write("{0}: Recipe {1} found, attempting to remove.", NAMESPACE == null ? "" : NAMESPACE, Rec.Name);
                    RecipeStorage.Recipes.Remove(recName);

                    Recipe Rec2;
                    if (!RecipeStorage.TryGetRecipe(recName, out Rec2))
                    {
                        Pipliz.Log.Write("{0}: Recipe {1} successfully removed", NAMESPACE == null ? "" : NAMESPACE, Rec.Name);
                        return true;
                    }
                    else
                    {
                        Pipliz.Log.Write("{0}: Recipe {1} removal failed for unknown reason.", NAMESPACE == null ? "" : NAMESPACE, Rec.Name);
                        return false;
                    }
                }
                else
                {
                    Pipliz.Log.Write("{0}: Recipe {1} not found.", NAMESPACE == null ? "" : NAMESPACE, recName);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Pipliz.Log.Write("{0}: tryRemoveRecipe has reached an exception: {1}", NAMESPACE == null ? "" : NAMESPACE, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Attempts to remove an item from the server's database.
        /// </summary>
        /// <param name="itemName">string: Item's Key.</param>
        /// <param name="NAMESPACE">The mod's namespace.</param>
        /// <returns>True if item was removed. False if it was not for any reason.</returns>
        public static bool tryRemoveItem(string itemName, string NAMESPACE = null)
        {
            if (itemName == null || itemName.Length < 1)
            {
                Pipliz.Log.WriteError("{0}: tryRemoveItem has been called but was not given a valid item identifier.", NAMESPACE == null ? "" : NAMESPACE);
                return false;
            }
            else
            {
                if (Variables.itemsMaster == null)
                {
                    Pipliz.Log.WriteError("{0}: tryRemoveItem was called on {1} before Items master dictionary has been obtained. Cannot complete action.", NAMESPACE == null ? "" : NAMESPACE, itemName);
                    return false;
                }
                else
                {
                    if (!Variables.itemsMaster.ContainsKey(itemName))
                    {
                        Pipliz.Log.WriteError("{0}: tryRemoveItem was called on key {1} that was not found.", NAMESPACE == null ? "" : NAMESPACE, itemName);
                        return false;
                    }
                    else
                    {
                        Pipliz.Log.Write("{0}: Item key {1} found, attempting removal", NAMESPACE == null ? "" : NAMESPACE, itemName);
                        Variables.itemsMaster.Remove(itemName);

                        if (!Variables.itemsMaster.ContainsKey(itemName))
                        {
                            Pipliz.Log.Write("{0}: Item {1} successfully removed.", NAMESPACE == null ? "" : NAMESPACE, itemName);
                            return true;
                        }
                        else
                        {
                            Pipliz.Log.Write("{0}: Item {1} removal was not successful for an unknown reason.", NAMESPACE == null ? "" : NAMESPACE, itemName);
                            return false;
                        }
                    }
                }
            }
        }
    }
}
