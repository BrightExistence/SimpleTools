using System;
using System.Collections.Generic;

namespace BrightExistence.SimpleTools
{
    /// <summary>
    /// Stores internal variables of SimpleTools
    /// </summary>
    public class Variables
    {
        /// <summary>
        /// Stores a reference to the mod's directory.
        /// </summary>
        public static string modDirectory = "";
        /// <summary>
        /// Stores a reference to the server's dictionary of raw item types.
        /// </summary>
        public static Dictionary<string, ItemTypesServer.ItemTypeRaw> itemsMaster;
        protected const int simpleToolsMajor = 0;
        protected const int simpleToolsMinor = 3;
        protected const int simpleToolsBuild = 1;
        /// <summary>
        /// The version of the toolkit being used.
        /// </summary>
        public static string toolkitVersion
        {
            get
            {
                return Convert.ToString(simpleToolsMajor) + "." + Convert.ToString(simpleToolsMinor) + "." + Convert.ToString(simpleToolsBuild);
            }
        }

        // AUTO-REGISTERED TEXTURES
        /// <summary>
        /// The list of SpecificTexture objects which will automatically be registered by the routines in class 'main.'
        /// SpecificTexture objects are automatically added to this list in their constructors.
        /// </summary>
        public static List<SpecificTexture> SpecificTextures = new List<SpecificTexture>();

        // AUTO-REGISTERED ITEMS
        /// <summary>
        /// The list of SimpleItem objects which will automatically be registered by the routines in class 'main.'
        /// SimpleItem objects are automatically added to this list in their constructors.
        /// </summary>
        public static List<SimpleItem> Items = new List<SimpleItem>();

        // AUTO-REGISTERED RECIPES
        /// <summary>
        /// The list of SimpleRecipe objects which will automatically be registered by the routines in class 'main.'
        /// SimpleRecipe objects are automatically added to this list in their constructors.
        /// </summary>
        public static List<SimpleRecipe> Recipes = new List<SimpleRecipe>();

        // AUTO-REGISTERED RESEARCHABLES
        /// <summary>
        /// The list of SimpleResearchable objects which will automatically be registered by the routines in class 'main.'
        /// SimpleResearchable objects are automatically added to this list in their constructors.
        /// </summary>
        public static List<SimpleResearchable> Researchables = new List<SimpleResearchable>();
    }
}
