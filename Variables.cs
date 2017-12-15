using System;
using System.Collections.Generic;

namespace BrightExistence.SimpleTools
{
    public class Variables
    {
        public static string ModGamedataDirectory;
        public static Dictionary<string, ItemTypesServer.ItemTypeRaw> itemsMaster;
        protected const int simpleToolsMajor = 0;
        protected const int simpleToolsMinor = 2;
        protected const int simpleToolsBuild = 1;
        public static string toolkitVersion
        {
            get
            {
                return Convert.ToString(simpleToolsMajor) + "." + Convert.ToString(simpleToolsMinor) + "." + Convert.ToString(simpleToolsBuild);
            }
        }

        // AUTO-REGISTERED TEXTURES
        public static List<SpecificTexture> SpecificTextures = new List<SpecificTexture>();

        // AUTO-REGISTERED ITEMS
        public static List<SimpleItem> Items = new List<SimpleItem>();

        // AUTO-REGISTERED RECIPES
        public static List<SimpleRecipe> Recipes = new List<SimpleRecipe>();

        // AUTO-REGISTERED RESEARCHABLES
        public static List<SimpleResearchable> Researchables = new List<SimpleResearchable>();
    }
}
