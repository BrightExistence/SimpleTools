using System;
using System.Collections.Generic;

namespace BrightExistence.SimpleTools
{
    public enum Rotation
    {
        zero = 0,
        counter90d = 1,
        counter180d = 2,
        counter270d = 3
    }

    public enum TexturePart
    {
        albedo = 0,
        emissive = 1,
        height = 2,
        normal = 3
    }

    public class SpecificTexture
    {
        /// <summary>
        /// Name of texture excluding prefix. Ex: MyTextureTop NOT MyHandle.MyMod.MyTextureTop
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Name of texture including prefix Ex: MyHandle.MyMod.MyTextureTop NOT MyTextureTop
        /// </summary>
        public string NAMESPACE { get; protected set; }
        
        /// <summary>
        /// The string by which this texture group be referenced.
        /// </summary>
        public string ID
        {
            get
            {
                if (NAMESPACE == null) return this.Name;
                else return NAMESPACE + "." + Name;
            }
        }

        public string AlbedoPath;

        public string NormalPath;

        public string EmissivePath;

        public string HeightPath;

        public SpecificTexture(string strName, string strNAMESPACE = null)
        {
            Name = strName == null ? "NewSpecificTexture" : strName;
            NAMESPACE = strNAMESPACE;
            try
            {
                if (!Variables.SpecificTextures.Contains(this)) Variables.SpecificTextures.Add(this);
            }
            catch (Exception)
            {
                Pipliz.Log.Write("{0} : WARNING : Specific texture {1} could not be automatically added to auto-load list. Make sure you explicityly added it.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
            }
        }

        /// <summary>
        /// Returns this item as a ItemTypeServer.TextureMapping struct. (Note this will strip name, ID, and namespace properties.)
        /// </summary>
        /// <returns>ItemTypeServer.TextureMapping struct</returns>
        protected ItemTypesServer.TextureMapping asTextureMapping()
        {
            ItemTypesServer.TextureMapping thisMapping = new ItemTypesServer.TextureMapping(new Pipliz.JSON.JSONNode());
            if (this.AlbedoPath != null) thisMapping.AlbedoPath = this.AlbedoPath;
            if (this.NormalPath != null) thisMapping.NormalPath = this.NormalPath;
            if (this.EmissivePath != null) thisMapping.EmissivePath = this.EmissivePath;
            if (this.HeightPath != null) thisMapping.HeightPath = this.HeightPath;
            return thisMapping;
        }

        /// <summary>
        /// Registers this texture in the server database. Should be called during the afterSelectedWorld callback method.
        /// </summary>
        public void registerTexture()
        {
            Pipliz.Log.Write("{0}: Registering specific texture as {1}", NAMESPACE == null ? "" : NAMESPACE, this.ID);
            foreach (string S in new List <string> { this.AlbedoPath, this.EmissivePath, this.HeightPath, this.NormalPath})
            {
                if (S != null)
                {
                    if (System.IO.File.Exists(S))
                    {
                        Pipliz.Log.Write("{0}: Looks good, albedo file exists.", NAMESPACE == null ? "" : NAMESPACE);
                    }
                    else
                    {
                        Pipliz.Log.WriteError("{0}: ERROR! Registering texture to a file {1} which does not exist!", NAMESPACE == null ? "" : NAMESPACE, S);
                    }
                }
            }
            ItemTypesServer.SetTextureMapping(this.ID, this.asTextureMapping());
            Pipliz.Log.Write("{0}: Specific texture registered: {1}", NAMESPACE == null ? "" : NAMESPACE, this.Name);
        }
    }

    /// <summary>
    /// Front-end for built-in ItemTypesServer.TextureMapping class to enable auto-registration.
    /// </summary>
    public class TextureGroup
    {
        /// <summary>
        /// Name of texture, excluding any prefixes. Ex: myTexture NOT myHandle.myMod.myTexture
        /// </summary>
        public string Name = "MyTexture";

        /// <summary>
        /// Prefix used to generate ID. Ex: myHandle.myMod
        /// </summary>
        protected string NAMESPACE;

        /// <summary>
        /// The non-rotated (default) texture files.
        /// </summary>
        public SpecificTexture Default;

        /// <summary>
        /// The texture files rotated 90 degrees counter-clockwise.
        /// </summary>
        public SpecificTexture counter90d;

        /// <summary>
        /// The texture files rotated 180 degrees counter-clockwise.
        /// </summary>
        public SpecificTexture counter270d;

        /// <summary>
        /// The texture files rotated 270 degrees counter-clockwise.
        /// </summary>
        public SpecificTexture counter180d;

        /// <summary>
        /// </summary>
        /// <param name="strName">Name of texture, excluding any prefixes. Ex: myTexture NOT myHandle.myMod.myTexture</param>
        /// <param name="strNAMESPACE">Prefix used to generate ID. Ex: myHandle.myMod</param>
        public TextureGroup(string strName, string strNAMESPACE = null)
        {
            Name = (strName == null || strName.Length < 1) ? "NewTexture" : strName;
            NAMESPACE = strNAMESPACE == null ? "" : strNAMESPACE;
            Default = new SpecificTexture(this.Name, this.NAMESPACE);
            Pipliz.Log.Write("{0}: Initialized texturegroup {1}, it is not yet registered.", NAMESPACE == null ? "" : NAMESPACE, this.Name);
        }

        /// <summary>
        /// Automates specific texture retrieval, accounting for null entries.
        /// </summary>
        /// <param name="facing">Orientation of desired texture.</param>
        /// <returns>Returns the specified specific texture, or the default texture if the specified texture is null.</returns>
        public SpecificTexture getTexture (Rotation facing)
        {
            switch (facing)
            {
                case Rotation.counter180d:
                    return this.counter180d == null ? this.Default : this.counter180d;
                case Rotation.zero:
                    return this.Default;
                case Rotation.counter270d:
                    return this.counter270d == null ? this.Default : this.counter270d;
                case Rotation.counter90d:
                    return this.counter90d == null ? this.Default : this.counter90d;
                default:
                    return this.Default;
            }
        }

        /// <summary>
        /// Automates setting specific textures, preventing accidental reference exceptions.
        /// </summary>
        /// <param name="whenRotated">Orientation of desired texture.</param>
        /// <param name="part">Which part of that texture is being set.</param>
        /// <param name="path">Path to physical texture file.</param>
        public void setRotatedTexture(Rotation whenRotated, TexturePart part, string path)
        {
            switch (whenRotated)
            {
                case Rotation.zero:
                    if (this.Default == null) this.Default = new SpecificTexture(this.Name, this.NAMESPACE);
                    switch (part)
                    {
                        case TexturePart.albedo:
                            this.Default.AlbedoPath = path;
                            break;
                        case TexturePart.emissive:
                            this.Default.EmissivePath = path;
                            break;
                        case TexturePart.height:
                            this.Default.HeightPath = path;
                            break;
                        case TexturePart.normal:
                            this.Default.NormalPath = path;
                            break;
                        default:
                            break;
                    }
                    break;
                case Rotation.counter90d:
                    if (this.counter90d == null) this.counter90d = new SpecificTexture(this.Name+"z+", this.NAMESPACE);
                    switch (part)
                    {
                        case TexturePart.albedo:
                            this.counter90d.AlbedoPath = path;
                            break;
                        case TexturePart.emissive:
                            this.counter90d.EmissivePath = path;
                            break;
                        case TexturePart.height:
                            this.counter90d.HeightPath = path;
                            break;
                        case TexturePart.normal:
                            this.counter90d.NormalPath = path;
                            break;
                        default:
                            break;
                    }
                    break;
                case Rotation.counter180d:
                    if (this.counter180d == null) this.counter180d = new SpecificTexture(this.Name + "x-", this.NAMESPACE);
                    switch (part)
                    {
                        case TexturePart.albedo:
                            this.counter180d.AlbedoPath = path;
                            break;
                        case TexturePart.emissive:
                            this.counter180d.EmissivePath = path;
                            break;
                        case TexturePart.height:
                            this.counter180d.HeightPath = path;
                            break;
                        case TexturePart.normal:
                            this.counter180d.NormalPath = path;
                            break;
                        default:
                            break;
                    }
                    break;
                case Rotation.counter270d:
                    if (this.counter270d == null) this.counter270d = new SpecificTexture(this.Name + "z-", this.NAMESPACE);
                    switch (part)
                    {
                        case TexturePart.albedo:
                            this.counter270d.AlbedoPath = path;
                            break;
                        case TexturePart.emissive:
                            this.counter270d.EmissivePath = path;
                            break;
                        case TexturePart.height:
                            this.counter270d.HeightPath = path;
                            break;
                        case TexturePart.normal:
                            this.counter270d.NormalPath = path;
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Registers this texture in the server database. Should be called during the afterSelectedWorld callback method.
        /// </summary>
        public void registerGroup()
        {
            Pipliz.Log.Write("{0}: Registering texture group {1}", NAMESPACE == null ? "" : NAMESPACE, this.Name);

            this.Default.registerTexture();
            if (this.counter180d != null) counter180d.registerTexture();
            if (this.counter90d != null) counter90d.registerTexture();
            if (this.counter270d != null) counter270d.registerTexture();
        }
    }
}
