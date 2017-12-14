using BrightExistence.SimpleTools;

namespace MyMod
{
    public static class UtilityFunctions
    {
        public static string iconPath(string iconName)
        {
            return "gamedata/mods/" + Data.NAMESPACE.Replace('.', '/') + "/icons/" + iconName;
        }

        public static string albedoPath(string textureName)
        {
            return "gamedata/mods/" + Data.NAMESPACE.Replace('.', '/') + "/textures/albedo/" + textureName;
        }

        public static string emissivepath(string textureName)
        {
            return "gamedata/mods/" + Data.NAMESPACE.Replace('.', '/') + "/textures/emissive/" + textureName;
        }

        public static string heightpath(string textureName)
        {
            return "gamedata/mods/" + Data.NAMESPACE.Replace('.', '/') + "/textures/height/" + textureName;
        }

        public static string normalpath(string textureName)
        {
            return "gamedata/mods/" + Data.NAMESPACE.Replace('.', '/') + "/textures/normal/" + textureName;
        }
    }


    public static class Data
    {
        //----------------- CONSTANTS ------------------
        public const string NAMESPACE = "MyMod";

        // ----------------- DATA MEMBERS ------------------
        // Declare mod assets like Items and Recipies below (as static) if you need them to reference each other later on.
        // Ex: SimpleItem MyItem = new SimpleItem("MyItem");
        // -------------------------------------------------

        // (STATIC) SIMPLETEXTURE OBJECTS


        // (STATIC) SIMPLEITEM OBJECTS


        // (STATIC) SIMPLERECIPE OBJECTS


        // (STATIC) SIMPLERESEARCH OBJECTS


        // ----------------- DATA ------------------
        /// <summary>
        /// Populate the data of assets in the following methods so that code will be executed at the correct times and
        /// exceptions will not be generated which might break all mod loading.
        /// </summary>
        public static void populateTextureObjects()
        {
            
        }

        public static void populateItemObjects()
        {
            

        }

        public static void populateRecipeObjects()
        {
            
        }

        public static void populateJobs()
        {
            //ColonyShopBlock.registerJob<ColonyShop.jobs.ColonyShopJob>();
        }

        public static void populateResearchObjects()
        {

        }
    }
}
