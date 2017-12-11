using BrightExistence.SimpleTools;

namespace MyHandle.MyMod
{
    public class Data
    {
        //----------------- CONSTANTS ------------------
        public const string NAMESPACE = "MyHandle.MyMod";

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
            /*As-In: 
             *  If you have created a SimpleItem called Bowyer above, associated with a job you made like so:
             *  Bowyer.registerJob<Jobs.BowyerJob>();
             */
        }

        public static void populateResearchObjects()
        {

        }
    }
}
