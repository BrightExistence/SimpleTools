using BrightExistence.SimpleTools;

namespace MyHandle.MyMod
{
    public interface ModData
    {
        // !$!@!$@$ This approach won't work either.
        
        //----------------- CONSTANTS ------------------
        public string myNAMESPACE();

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

        }

        public static void populateResearchObjects()
        {

        }
    }
}
