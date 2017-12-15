using BrightExistence.SimpleTools;

namespace BrightExistence.SimpleTools
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
        public const string NAMESPACE = "BrightExistence.BetterTrees";

        // ----------------- DATA MEMBERS ------------------
        // Declare mod assets like Items and Recipies below (as static) if you need them to reference each other later on.
        // Ex: SimpleItem MyItem = new SimpleItem("MyItem");
        // -------------------------------------------------

        // (STATIC) SIMPLETEXTURE OBJECTS
        public static SpecificTexture XM = new SpecificTexture("XM", NAMESPACE);
        public static SpecificTexture XP = new SpecificTexture("XP", NAMESPACE);
        public static SpecificTexture ZM = new SpecificTexture("ZM", NAMESPACE);
        public static SpecificTexture ZP = new SpecificTexture("ZP", NAMESPACE);
        static SpecificTexture TexFir = new SpecificTexture("leavestaiga", NAMESPACE);
        static SpecificTexture TexLeaves = new SpecificTexture("leavestemperate", NAMESPACE);
        static SpecificTexture TexLogTaiga = new SpecificTexture("logtaiga", NAMESPACE);
        static SpecificTexture TexLogTemperate = new SpecificTexture("logtemperate", NAMESPACE);
        static SpecificTexture TexStumpTaiga = new SpecificTexture("stumptaiga", NAMESPACE);
        static SpecificTexture TexStumpTemperate = new SpecificTexture("stumptemperate", NAMESPACE);
        static SpecificTexture TexTaigaHorizontal = new SpecificTexture("taigahorizontal", NAMESPACE);
        static SpecificTexture TexTemperateHorizontal = new SpecificTexture("temperatehorizontal", NAMESPACE);

        static TextureGroup TGTaigaHorizontal = new TextureGroup("tgtaigahorizontal", NAMESPACE);
        static TextureGroup TGTemperateHorizontal = new TextureGroup("tgtemperatehorizontal", NAMESPACE);

        // (STATIC) SIMPLEITEM OBJECTS
        static SimpleItem BetterTaiga = new SimpleItem("betterTaiga", NAMESPACE, false);
        static SimpleItem BetterTemperate = new SimpleItem("betterTemperate", NAMESPACE, false);
        static SimpleItem BetterLogTaiga = new SimpleItem("betterLogTaiga", NAMESPACE);
        static SimpleItem BetterLogTemperate = new SimpleItem("betterLogTemperate", NAMESPACE);
        static SimpleItem LogTaigaHorizontal = new SimpleItem("logtaigahorizontal", NAMESPACE);
        static SimpleItem logTemperateHorizontal = new SimpleItem("logtemperatehorizontal", NAMESPACE);

        // (STATIC) SIMPLERECIPE OBJECTS
        static SimpleRecipe RecTaigaVerticalToHorizontal = new SimpleRecipe(LogTaigaHorizontal, "pipliz.crafter");
        static SimpleRecipe RecTaigaHorizontalToVertical = new SimpleRecipe(BetterLogTaiga, "pipliz.crafter");

        static SimpleRecipe RecTemperateVerticalToHorizontal = new SimpleRecipe(logTemperateHorizontal, "pipliz.crafter");
        static SimpleRecipe RecTemperateHorizontalToVerical = new SimpleRecipe(BetterLogTemperate, "pipliz.crafter");

        // (STATIC) SIMPLERESEARCH OBJECTS


        // ----------------- DATA ------------------
        /// <summary>
        /// Populate the data of assets in the following methods so that code will be executed at the correct times and
        /// exceptions will not be generated which might break all mod loading.
        /// </summary>
        public static void populateTextureObjects()
        {
            XM.AlbedoPath = UtilityFunctions.albedoPath("x-.png");
            XP.AlbedoPath = UtilityFunctions.albedoPath("x+.png");
            ZM.AlbedoPath = UtilityFunctions.albedoPath("z-.png");
            ZP.AlbedoPath = UtilityFunctions.albedoPath("z+.png");
            TexFir.AlbedoPath = UtilityFunctions.albedoPath("firTree.png");
            TexLeaves.AlbedoPath = UtilityFunctions.albedoPath("leavestemperate.png");
            TexLogTaiga.AlbedoPath = UtilityFunctions.albedoPath("logTaiga.png");
            TexLogTemperate.AlbedoPath = UtilityFunctions.albedoPath("logTemperate.png");
            TexStumpTaiga.AlbedoPath = UtilityFunctions.albedoPath("stumptaiga.png");
            TexStumpTemperate.AlbedoPath = UtilityFunctions.albedoPath("stumptemperate.png");
            TexTaigaHorizontal.AlbedoPath = UtilityFunctions.albedoPath("loghorizontaltaiga.png");
            TexTemperateHorizontal.AlbedoPath = UtilityFunctions.albedoPath("loghorizontaltemperate.png");

            TGTaigaHorizontal.Default = TexLogTaiga;
            TGTaigaHorizontal.counter90d = TexTaigaHorizontal;
            TGTaigaHorizontal.counter180d = TexLogTaiga;
            TGTaigaHorizontal.counter270d = TexTaigaHorizontal;

            TGTemperateHorizontal.Default = TexLogTemperate;
            TGTemperateHorizontal.counter90d = TexTemperateHorizontal;
            TGTemperateHorizontal.counter180d = TexLogTemperate;
            TGTemperateHorizontal.counter270d = TexTemperateHorizontal;
        }

        public static void populateItemObjects()
        {
            // taiga leaves
            BetterTaiga.Icon = UtilityFunctions.iconPath("firTree.png");
            BetterTaiga.maskItem = "leavestaiga";
            BetterTaiga.sideAll = TexFir.ID;
            BetterTaiga.isPlaceable = true;

            // temperate leaves
            BetterTemperate.Icon = UtilityFunctions.iconPath("leavestemperate.png");
            BetterTemperate.maskItem = "leavestemperate";
            BetterTemperate.sideAll = TexLeaves.ID;
            BetterTemperate.isPlaceable = true;

            // taiga log
            BetterLogTaiga.Icon = UtilityFunctions.iconPath("logTaiga.png");
            BetterLogTaiga.maskItem = "logtaiga";
            BetterLogTaiga.sideAll = TexLogTaiga.ID;
            BetterLogTaiga.sideTop = TexStumpTaiga.ID;
            BetterLogTaiga.sideBottom = TexStumpTaiga.ID;
            BetterLogTaiga.isPlaceable = true;

            // horizontal taiga log
            LogTaigaHorizontal.Icon = UtilityFunctions.iconPath("loghorizontaltaiga.png");
            LogTaigaHorizontal.sideAll = TexTaigaHorizontal.ID;
            LogTaigaHorizontal.sideFront = TexStumpTaiga.ID;
            LogTaigaHorizontal.sideBack = TexStumpTaiga.ID;
            LogTaigaHorizontal.grpSideTop = TGTaigaHorizontal;
            LogTaigaHorizontal.grpSideBottom = TGTaigaHorizontal;
            LogTaigaHorizontal.isPlaceable = true;
            LogTaigaHorizontal.isSolid = true;
            LogTaigaHorizontal.isRotatable = true;

            // temperate log
            BetterLogTemperate.Icon = UtilityFunctions.iconPath("logtemperate.png");
            BetterLogTemperate.maskItem = "logtemperate";
            BetterLogTemperate.sideAll = TexLogTemperate.ID;
            BetterLogTemperate.sideTop = TexStumpTemperate.ID;
            BetterLogTemperate.sideBottom = TexStumpTemperate.ID;
            BetterLogTemperate.isPlaceable = true;

            // horizontal temperate log
            logTemperateHorizontal.Icon = UtilityFunctions.iconPath("loghorizontaltemperate.png");
            logTemperateHorizontal.sideAll = TexTemperateHorizontal.ID;
            logTemperateHorizontal.sideFront = TexStumpTemperate.ID;
            logTemperateHorizontal.sideBack = TexStumpTemperate.ID;
            logTemperateHorizontal.grpSideBottom = TGTemperateHorizontal;
            logTemperateHorizontal.grpSideTop = TGTemperateHorizontal;
            logTemperateHorizontal.isPlaceable = true;
            logTemperateHorizontal.isSolid = true;
            logTemperateHorizontal.isRotatable = true;
        }

        public static void populateRecipeObjects()
        {
            RecTaigaVerticalToHorizontal.addRequirement(BetterLogTaiga);
            RecTaigaVerticalToHorizontal.userCraftable = true;

            RecTaigaHorizontalToVertical.addRequirement(LogTaigaHorizontal);
            RecTaigaHorizontalToVertical.userCraftable = true;

            RecTemperateVerticalToHorizontal.addRequirement(BetterLogTemperate);
            RecTemperateVerticalToHorizontal.userCraftable = true;

            RecTemperateHorizontalToVerical.addRequirement(logTemperateHorizontal);
            RecTemperateHorizontalToVerical.userCraftable = true;
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
