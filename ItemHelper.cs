using System;
using System.Collections.Generic;
using Pipliz.APIProvider.Jobs;
using Pipliz.JSON;

namespace BrightExistence.SimpleTools
{
    public static class ItemHelper
    {
        /// <summary>
        /// Attempts to remove an item from the server's database.
        /// </summary>
        /// <param name="itemName">string: Item's Key.</param>
        /// <returns>True if item was removed. False if it was not for any reason.</returns>
        public static bool tryRemoveItem (string itemName, string NAMESPACE = null)
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

    /// <summary>
    /// Represents an InventoryItem object without relying on the Variables.itemsMaster to be ready or throwing exceptions.
    /// </summary>
    public class ItemShell
    {
        public string strItemkey = "[unknown]";
        public int intAmount;
        public SimpleItem asSimpleItem;

        public ItemShell(string Key, int amount = 1)
        {
            if (Key != null) strItemkey = Key;
            intAmount = amount;
        }

        public ItemShell(SimpleItem asThis, int amount = 1)
        {
            asSimpleItem = asThis;
            intAmount = amount;
        }
    }

    /// <summary>
    /// A helper class representing an Item. Self-registering.
    /// </summary>
    public class SimpleItem
    {
        /// <summary>
        /// Stores the mod's namespace.
        /// </summary>
        public string NAMESPACE { get; protected set; }

        /// <summary>
        /// Name of Item, excluding prefix. Ex: myItem instead of myHandle.myMod.myItem
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to icon .png file Ex: gamedata/textures/icons/vanillaIconName.png or getLocalIcon("myIconFile.png")
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Can the item be placed in the world; is it a block?
        /// </summary>
        public bool isPlaceable = false;

        /// <summary>
        /// When a player attempts to remove the block, is it actually removed?
        /// </summary>
        public bool? isDestructible { get; set; }

        /// <summary>
        /// Is it a solid block, or can players and NPCs walk through it?
        /// </summary>
        public bool? isSolid { get; set; }

        /// <summary>
        /// The name of the texture that will be displayed on all sides of the block unless otherwise specified.
        /// </summary>
        public string sideAll { get; set; }

        /// <summary>
        /// The ID of a texture that will be displayed on the top (y+) side of the block as a (NOT rotation-compatible) texture group.
        /// </summary>
        public string sideTop
        {
            get
            {
                return grpSideTop == null ? topTexture == null ? sideAll : topTexture : grpSideTop.Default.ID;
            }
            set
            {
                if (grpSideTop != null)
                {
                    Pipliz.Log.Write("{0} WARNING: Top texture of item {1} was assigned a (rotatable) texture group and is now being overwritten by a (non-rotatable) texture ID string.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                    grpSideTop = null;
                }
                topTexture = value;
            }
        }
        protected string topTexture;

        /// <summary>
        /// The texture that will be displayed on the top (y+) side of the block as a (rotation-compatible) texture group.
        /// </summary>
        public TextureGroup grpSideTop
        {
            get; set;
        }

        /// <summary>
        /// The ID of a texture that will be displayed on the bottom (y-) side of the block as a (NOT rotation-compatible) texture group.
        /// </summary>
        public string sideBottom
        {
            get
            {
                return grpSideBottom == null ? bottomTexture == null ? sideAll : bottomTexture : grpSideBottom.Default.ID;
            }
            set
            {
                if (grpSideBottom != null)
                {
                    Pipliz.Log.Write("{0} WARNING: Bottom texture of item {1} was assigned a (rotatable) texture group and is now being overwritten by a (non-rotatable) texture ID string.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                    grpSideBottom = null;
                }
                bottomTexture = value;
            }
        }
        protected string bottomTexture;

        /// <summary>
        /// The texture that will be displayed on the bottom (y-) side of the block as a (rotation-compatible) texture group.
        /// </summary>
        public TextureGroup grpSideBottom
        {
            get;set;
        }

        /// <summary>
        /// The name of the texture which will be displayed on the front (x+) side of the block only.
        /// </summary>
        public string sideFront { get; set; }

        /// <summary>
        /// The name of the texture which will be displayed on the back (x-) side of the block only.
        /// </summary>
        public string sideBack { get; set; }

        /// <summary>
        /// The name of the texture which will be displayed on the left (z-) side of the block only.
        /// </summary>
        public string sideLeft { get; set; }

        /// <summary>
        /// The name of the texture which will be displayed on the right (z+) side of the block only.
        /// </summary>
        public string sideRight { get; set; }

        /// <summary>
        /// The location of a file that is the mesh for this item. If omitted, the item will be a perfect cube.
        /// </summary>
        public string mesh { get; set; }

        /// <summary>
        /// The amount of time the user must hold down the left mouse button to remove a block of this item.
        /// </summary>
        public int? destructionTime = 500;

        /// <summary>
        /// The name of an audio asset which will be played when a block of this item is placed.
        /// </summary>
        public string onPlaceAudio { get; set; }

        /// <summary>
        /// The name of an audio asset which will be played when a block of this item is removed.
        /// </summary>
        public string onRemoveAudio { get; set; }

        /// <summary>
        /// If true, the registerAsCrate method will register this item as a crate (a type of tracked block) when called during the proper callback.
        /// </summary>
        public bool isCrate = false;

        /// <summary>
        /// A list of DropItem objects describing what types are added to inventory when a block of this type is removed, and by what chance.
        /// </summary>
        public List<DropItem> Drops = new List<DropItem>();

        /// <summary>
        /// Will things grow on it?
        /// </summary>
        public bool? isFertile = false;

        /// <summary>
        /// Can it be mined by NPCs
        /// </summary>
        public bool? minerIsMineable;

        /// <summary>
        /// How quickly do NPCs mine it, if they're allowed to?
        /// </summary>
        public int minerMiningTime = 2;

        /// <summary>
        /// Replaces an item of this key in the server's item database.
        /// </summary>
        public string maskItem;

        /// <summary>
        /// Used to make the block glow by using a SimpleItem.Light object.
        /// </summary>
        public SimpleItem.Light lightSource;

        /// <summary>
        /// True to overwrite an existing item by the same ID if it is found, is ignored if masking is used.
        /// </summary>
        public bool overwrite = false;

        /// <summary>
        /// Set to false if you do NOT want this item, and any SimpleRecipe/SimpleResearch objects which know about and depend upon it, registered.
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// The mesh file this item should use when rotated 90 degrees counter-clockwise.
        /// </summary>
        public string meshZP { get; set; }

        /// <summary>
        /// The mesh file this item should use when rotated 180 degrees counter-clockwise.
        /// </summary>
        public string meshXM { get; set; }

        /// <summary>
        /// The mesh file this item should use when rotated 270 degrees counter-clockwise.
        /// </summary>
        public string meshZM { get; set; }

        /// <summary>
        /// This items X+ (default) variant.
        /// </summary>
        protected SimpleItem XP { get; set; }

        /// <summary>
        /// This items Z- (rotated 270) variant.
        /// </summary>
        protected SimpleItem ZM { get; set; }

        /// <summary>
        /// This items X- (rotated 180) variant.
        /// </summary>
        protected SimpleItem XM { get; set; }

        /// <summary>
        /// This items Z+ (rotated 90) variant.
        /// </summary>
        protected SimpleItem ZP { get; set; }

        /// <summary>
        /// Set to true to set this item to register itself as rotatable, and generate all necessary subtypes.
        /// </summary>
        public bool isRotatable = false;

        /// <summary>
        /// Specify a parent type.
        /// </summary>
        public string parentType;

        /// <summary>
        /// Set to false to remove this item automatically adding itself as a drop.
        /// </summary>
        protected bool dropsSelf = true;

        /// <summary>
        /// Set to true to have generated items for rotatable types to expose their type in-game.
        /// </summary>
        public bool debug = false;

        /// <summary>
        /// The ID, or name of this item as it will be stored in the server database.
        /// </summary>
        public string ID
        {
            get
            {
                if (maskItem == null)
                {
                    if (NAMESPACE == null || NAMESPACE.Length < 1) return Name;
                    else return NAMESPACE + "." + Name;
                }
                else return maskItem;
            }
        }

        /// <summary>
        /// Generates an ItemTypesServer.ItemTypeRaw object using this object's properties.
        /// </summary>
        protected ItemTypesServer.ItemTypeRaw itrThisItem;
        protected ItemTypesServer.ItemTypeRaw thisItemRaw
        {
            get
            {
                this.itrThisItem = new ItemTypesServer.ItemTypeRaw(this.ID, itemAsJSON());
                return itrThisItem; 
            }
            set
            {
                itrThisItem = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strNamespace">Namespace of mod. Ex: DeveloperHandle.ModName Will be used as a prefix to generate item IDs.</param>
        /// <param name="strName">Name of item excluding any prefixes. Ex: MyItem NOT DeveloperHandle.ModName.MyItem</param>
        public SimpleItem(string strName, string strNAMESPACE = null, bool blnDropsSelf = true)
        {
            NAMESPACE = strNAMESPACE;
            Name = (strName == null || strName.Length < 1) ? "NewItem" : strName;
            dropsSelf = blnDropsSelf;
            Pipliz.Log.Write("{0}: Initialized Item {1} (it is not yet registered.)", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            try
            {
                if (!Variables.Items.Contains(this)) Variables.Items.Add(this);
            }
            catch (Exception)
            {
                Pipliz.Log.Write("{0} : WARNING : Item {1} could not be automatically added to auto-load list. Make sure you explicityly added it.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
        }

        /// <summary>
        /// Registers this item in the server's database of items.Should be called during the afterAddingBaseTypes callback.
        /// </summary>
        /// <param name="items">The server's item database (a Dictionary object). Will be passed to the afterAddingBaseTypes callback method.</param>
        public void registerItem(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            if (enabled)
            {
                Pipliz.Log.Write("{0}: Attempting to register item {1}", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                // add itself as a drop unless told otherwise
                if (dropsSelf) Drops.Add(new DropItem(this.ID));
                // handle rotation
                if (this.isRotatable)
                {
                    Rotate(items);
                }
                // handle masking
                if (maskItem != null)
                {
                    Pipliz.Log.Write("{0}: Attempting to mask item {1} with {2}.", NAMESPACE == null ? "" : NAMESPACE, this.ID, this.Name);
                    // Masking is being used, see if there is an existing item to mask.
                    if (items.ContainsKey(this.ID))
                    {
                        // It exists, merge values.
                        ItemTypesServer.ItemTypeRaw originalItem;
                        if (items.TryGetValue(this.ID, out originalItem))
                        {
                            // Successfully retrieved item, overwrite its properties which are explicity specified in this object.
                            itemAsJSON(originalItem.description);
                            Pipliz.Log.Write("{0}: Masking complete.", NAMESPACE == null ? "" : NAMESPACE);
                        }
                        else
                        {
                            // Item exists, but we can't retrieve it.
                            Pipliz.Log.Write("{0}: Masking failed, item {1} exists but we could not retrieve it. Overwriting instead.", NAMESPACE == null ? "" : NAMESPACE, this.ID);
                            // Remove existing item.
                            ItemHelper.tryRemoveItem(this.ID);
                            // Add this item.
                            items.Add(this.ID, thisItemRaw);
                        }

                    }
                    else
                    {
                        // Item did not already exist, let's add it.
                        Pipliz.Log.Write("{0}: Masking was enabled, but masked item was not found. Adding {1} as a new item with ID {2}", NAMESPACE == null ? "" : NAMESPACE, this.Name, this.ID);
                        items.Add(this.ID, thisItemRaw);
                    }
                }
                else
                {
                    // masking NOT used
                    Pipliz.Log.Write("{0}: Registering block {1} to ID {2}", NAMESPACE == null ? "" : NAMESPACE, this.Name, this.ID);
                    if (items.ContainsKey(this.ID))
                    {
                        // Item already exists, do we overwrite?
                        if (overwrite)
                        {
                            Pipliz.Log.Write("{0}: Item {1} already exists, overwriting item entry.", NAMESPACE == null ? "" : NAMESPACE, this.ID);
                            // Remove existing item.
                            ItemHelper.tryRemoveItem(this.ID);
                            // Add this item.
                            items.Add(this.ID, thisItemRaw);
                        }
                        else
                        {
                            // Do nothing, it already exists and we're neither masking nor overwriting.
                            Pipliz.Log.Write("{0}: Item {1} already exists, registration is not necessary and is being aborted.", NAMESPACE == null ? "" : NAMESPACE, this.ID);
                        }
                    }
                    else
                    {
                        // Item does not already exist, add it.
                        items.Add(this.ID, thisItemRaw);                    }
                }

                Pipliz.Log.Write("{0}: Block {1} registration complete.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
            else
            {
                Pipliz.Log.Write("{0}: Block {1} has been disabled, and will NOT be registered.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
        }

        /// <summary>
        /// Registers this block as a crate if the isCrate property is set to true. Should be called during the AfterItemTypesDefined callback.
        /// </summary>
        public void registerAsCrate()
        {
            if (this.isCrate)
            {
                Pipliz.Log.Write("{0}: Attempting to register {1} as a crate.", NAMESPACE == null ? "" : NAMESPACE, this.ID);

                try
                {
                    ItemTypesServer.RegisterOnAdd(this.ID, StockpileBlockTracker.Add);
                    ItemTypesServer.RegisterOnRemove(this.ID, StockpileBlockTracker.Remove);
                }
                catch (Exception ex)
                {
                    Pipliz.Log.Write("{0}: Crate registration error: {1}", NAMESPACE == null ? "" : NAMESPACE, ex.Message);
                }
            }
        }

        /// <summary>
        /// Associates a job class to this block.
        /// </summary>
        /// <typeparam name="T">A class which describes the job being associated with the block, must impliment ITrackableBlock,
        /// IBlockJobBase, INPCTypeDefiner, and have a default constructor. Should be called during the AfterDefiningNPCTypes callback.</typeparam>
        public void registerJob<T>() where T : ITrackableBlock, IBlockJobBase, INPCTypeDefiner, new()
        {
            Pipliz.Log.Write("{0}: Attempting to register a job to block {1}", NAMESPACE == null ? "" : NAMESPACE, this.ID);
            try
            {
                BlockJobManagerTracker.Register<T>(this.ID);
            }
            catch (Exception ex)
            {
                Pipliz.Log.Write("{0}: Registration error: {1}", NAMESPACE == null ? "" : NAMESPACE, ex.Message);
            }
        }

        /// <summary>
        /// Populates and registers rotated types. Called by registerItem().
        /// </summary>
        /// <param name="items">The server's item database (a Dictionary object). Will be passed to the afterAddingBaseTypes callback method.</param>
        protected void Rotate(Dictionary<string, ItemTypesServer.ItemTypeRaw> items)
        {
            try
            {
                Pipliz.Log.Write("{0}: Generating rotated variants of block {1}", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                // Rotated x+
                XP = new SimpleItem(this.Name + "x+", this.NAMESPACE, false);
                XP.parentType = this.ID;
                XP.isPlaceable = true;
                XP.Drops = this.Drops;
                // sideall remains untouched.
                XP.sideAll = this.sideAll;
                // sides get switched around
                if (debug) XP.sideFront = NAMESPACE == null ? "" : XP.ID;
                else XP.sideFront = XP.sideRight;
                XP.sideRight = this.sideFront;
                XP.sideBack = this.sideLeft;
                XP.sideLeft = this.sideBack;
                if (grpSideTop == null) XP.sideTop = this.sideTop;
                else XP.sideTop = this.grpSideTop.getTexture(Rotation.counter90d).ID;
                if (grpSideBottom == null) XP.sideBottom = this.sideBottom;
                else XP.sideBottom = grpSideBottom.getTexture(Rotation.counter90d).ID;
                // use proper mesh
                XP.mesh = this.mesh;

                // Rotated z+
                ZP = new SimpleItem(this.Name + "z+", this.NAMESPACE, false);
                ZP.parentType = this.ID;
                ZP.isPlaceable = true;
                ZP.Drops = this.Drops;
                // sideall remains untouched.
                ZP.sideAll = this.sideAll;
                // sides get switched around
                if (debug) ZP.sideFront = NAMESPACE == null ? "" : ZP.ID;
                else ZP.sideFront = this.sideFront;
                ZP.sideRight = this.sideLeft;
                ZP.sideBack = this.sideBack;
                ZP.sideLeft = this.sideRight;
                if (grpSideTop == null) ZP.sideTop = this.sideTop;
                else ZP.sideTop = this.grpSideTop.getTexture(Rotation.counter180d).ID;
                if (grpSideBottom == null) ZP.sideBottom = this.sideBottom;
                else ZP.sideBottom = grpSideBottom.getTexture(Rotation.zero).ID;
                Pipliz.Log.Write("DEBUG: Setting ZP bottom texture to {0}", XP.sideBottom);
                // use proper mesh
                ZP.mesh = this.meshZP;

                // Rotated x-
                XM = new SimpleItem(this.Name + "x-", this.NAMESPACE, false);
                XM.parentType = this.ID;
                XM.isPlaceable = true;
                XM.Drops = this.Drops;
                // sideall remains untouched.
                XM.sideAll = this.sideAll;
                // sides get switched around
                if (debug) XM.sideFront = NAMESPACE == null ? "" : XM.ID;
                else XM.sideFront = this.sideLeft;
                XM.sideRight = this.sideBack;
                XM.sideBack = this.sideRight;
                XM.sideLeft = this.sideFront;
                if (this.grpSideTop == null) XM.sideTop = this.sideTop;
                else XM.sideTop = this.grpSideTop.getTexture(Rotation.counter270d).ID;
                if (grpSideBottom == null) XM.sideBottom = this.sideBottom;
                else XM.sideBottom = grpSideBottom.getTexture(Rotation.counter270d).ID;
                Pipliz.Log.Write("DEBUG: Setting XM bottom texture to {0}", XP.sideBottom);
                // use proper mesh
                XM.mesh = this.meshXM;

                // Rotated z-
                ZM = new SimpleItem(this.Name + "z-", this.NAMESPACE, false);
                ZM.parentType = this.ID;
                ZM.isPlaceable = true;
                ZM.Drops = this.Drops;
                // sideall remains untouched.
                ZM.sideAll = this.sideAll;
                // sides get switched around
                if (debug) ZM.sideFront = NAMESPACE == null ? "" : ZM.ID;
                else ZM.sideFront = this.sideBack;
                ZM.sideRight = this.sideRight;
                ZM.sideBack = this.sideFront;
                ZM.sideLeft = this.sideLeft;
                if (grpSideTop == null) ZM.sideTop = this.sideTop;
                else ZM.sideTop = this.grpSideTop.getTexture(Rotation.zero).ID;
                if (grpSideBottom == null) ZM.sideBottom = this.sideBottom;
                else ZM.sideBottom = grpSideBottom.getTexture(Rotation.counter180d).ID;
                Pipliz.Log.Write("DEBUG: Setting ZM bottom texture to {0}", XP.sideBottom);
                // use proper mesh
                ZM.mesh = this.meshZM;

                Pipliz.Log.Write("{0}: Registering rotated variants of type {1}", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                this.XM.registerItem(items);
                this.XP.registerItem(items);
                this.ZP.registerItem(items);
                this.ZM.registerItem(items);
            }
            catch (Exception ex)
            {
                Pipliz.Log.Write("{0}: CRITICAL ERROR! Generating rotated variants of block {1} caused the following exception: {2}", NAMESPACE == null ? "" : NAMESPACE, this.Name, ex.Message);
                this.isRotatable = false;
            }
        }

        /// <summary>
        /// Returns this item's properties as a JSONNode.
        /// </summary>
        /// <param name="thisItemJSON">Optional. If provided overwrites the values in provided JSONNode with this object's values and returns provided JSON.</param>
        /// <returns>A JSONNode object containing original data (if provided by thisItemJSON parameter) overwritten by this item's data.</returns>
        protected JSONNode itemAsJSON(JSONNode thisItemJSON = null)
        {
            if (thisItemJSON == null) thisItemJSON = new JSONNode();
            if (parentType != null) thisItemJSON.SetAs("parentType", parentType);
            if (this.isRotatable)
            {
                if (XP == null || XM == null || ZP == null || ZM == null)
                {
                    Pipliz.Log.Write("{0}: ERROR! Item {1} is set as rotatable but the rotated variant types have not been populated.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
                }
                else
                {
                    thisItemJSON.SetAs("isRotatable", true);
                    thisItemJSON.SetAs("rotatablex+", this.XP.ID);
                    thisItemJSON.SetAs("rotatablex-", this.XM.ID);
                    thisItemJSON.SetAs("rotatablez+", this.ZP.ID);
                    thisItemJSON.SetAs("rotatablez-", this.ZM.ID);
                }
            }
            if (Icon != null) thisItemJSON.SetAs("icon", Icon);
            thisItemJSON.SetAs("isPlaceable", isPlaceable);
            if (isDestructible != null) thisItemJSON.SetAs("isDestructible", isDestructible);
            if (isSolid != null) thisItemJSON.SetAs("isSolid", isSolid);
            if (this.Drops.Count > 0)
            {
                JSONNode DropsNode = new JSONNode(NodeType.Array);
                foreach (SimpleItem.DropItem thisDrop in Drops)
                {
                    DropsNode.AddToArray(thisDrop.asJSONNode());
                }
                thisItemJSON.SetAs("onRemove", DropsNode);
            }
            if (sideAll != null) thisItemJSON.SetAs("sideall", sideAll);
            if (destructionTime != null) thisItemJSON.SetAs("destructionTime", destructionTime);
            if (isFertile != null) thisItemJSON.SetAs("isFertile", isFertile);
            if (mesh != null) thisItemJSON.SetAs("mesh", mesh);
            if (minerIsMineable != null || lightSource != null)
            {
                JSONNode customData = new JSONNode();
                if (minerIsMineable != null && minerIsMineable == true)
                {
                    JSONNode MiningData = new JSONNode();
                    customData.SetAs("minerIsMineable", true);
                    customData.SetAs("minerMiningTime", minerMiningTime);
                }
                if (lightSource != null)
                {
                    customData.SetAs("torches", lightSource.asJSONNode());
                }
                thisItemJSON.SetAs("customData", customData);
            }
            if (sideTop != null) thisItemJSON.SetAs("sidey+", sideTop);
            if (sideBottom != null) thisItemJSON.SetAs("sidey-", sideBottom);
            if (sideFront != null) thisItemJSON.SetAs("sidez+", sideFront);
            if (sideBack != null) thisItemJSON.SetAs("sidez-", sideBack);
            if (sideLeft != null) thisItemJSON.SetAs("sidex-", sideLeft);
            if (sideRight != null) thisItemJSON.SetAs("sidex+", sideRight);
            if (onPlaceAudio != null) thisItemJSON.SetAs("onPlaceAudio", onPlaceAudio);
            if (onRemoveAudio != null) thisItemJSON.SetAs("onRemoveAudio", onRemoveAudio);
            Pipliz.Log.Write("{0}: Created raw item type {1}.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            try
            {
                JSON.Serialize("gamedata/itemjsons/" + this.Name + "Alpha.json", thisItemJSON);
            }
            catch (Exception ex)
            {
                Pipliz.Log.Write("{0}: Error serializing {1}'s JSON: {2}", NAMESPACE == null ? "" : NAMESPACE, this.Name, ex.Message);
            }

            return thisItemJSON;
        }

        /// <summary>
        /// Helper class used in building ItemTypeRaw JSONs
        /// </summary>
        public struct DropItem
        {
            string type;
            int amount;
            float chance;

            public DropItem(string strType)
            {
                type = strType;
                amount = 1;
                chance = 1f;
            }

            public DropItem(string strType, int intAmount)
            {
                type = strType;
                amount = intAmount;
                chance = 1f;
            }

            public DropItem(string strType, int intAmount, float fltChance)
            {
                type = strType;
                amount = intAmount;
                chance = fltChance;
            }

            public JSONNode asJSONNode ()
            {
                JSONNode returnMe = new JSONNode();
                returnMe.SetAs("type", type);
                returnMe.SetAs("amount", amount);
                returnMe.SetAs("chance", chance);

                return returnMe;
            }
        }

        public class Light
        {
            public float volume = 0.5f;
            public float intensity = 10f;
            public int range = 10;
            public float red = 195f;
            public float green = 135f;
            public float blue = 46f;

            public JSONNode asJSONNode ()
            {
                JSONNode torches = new JSONNode();
                JSONNode a = new JSONNode();
                a.SetAs("volume", volume);
                a.SetAs("intensity", intensity);
                a.SetAs("range", range);
                a.SetAs("red", red);
                a.SetAs("green", green);
                a.SetAs("blue", blue);
                torches.SetAs("a", a);

                return torches;
            }
        }
    }
}
