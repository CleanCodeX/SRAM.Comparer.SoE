﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SramComparer.SoE.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SramComparer.SoE.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Steps: 
        ///1.1) Have a look into UnknownOffsets.txt to see examples of what parts of SRAM structure are still 
        ///     considered &quot;unknown&quot;.
        ///1.2) Most emulators have the option to save the game&apos;s S-RAM automatically after a change occurs. 
        ///     Make sure this is enabled if existing. Otherwise you have manually ensure that the emulator updates 
        ///     the srm file.
        ///1.3) Start the tool by passing the game&apos;s srm filepath as first command parameter. The file can also be 
        ///     dragged onto the tool.
        ///
        ///2)   Switc [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string AppManualCommandsTemplate {
            get {
                return ResourceManager.GetString("AppManualCommandsTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die changes at every in-game save ähnelt.
        /// </summary>
        internal static string ChangesAtEveryInGameSave {
            get {
                return ResourceManager.GetString("ChangesAtEveryInGameSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Show all game checksums ähnelt.
        /// </summary>
        internal static string CommandIncludeAllChecksums {
            get {
                return ResourceManager.GetString("CommandIncludeAllChecksums", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Show all game checksums ähnelt.
        /// </summary>
        internal static string CommandIncludeAllUnknown12Bs {
            get {
                return ResourceManager.GetString("CommandIncludeAllUnknown12Bs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Show game checksum ähnelt.
        /// </summary>
        internal static string CommandIncludeChecksum {
            get {
                return ResourceManager.GetString("CommandIncludeChecksum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Show game Unknown12Bs ähnelt.
        /// </summary>
        internal static string CommandIncludeUnknown12B {
            get {
                return ResourceManager.GetString("CommandIncludeUnknown12B", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die compared with game {0} ähnelt.
        /// </summary>
        internal static string ComparedWithGameTemplated {
            get {
                return ResourceManager.GetString("ComparedWithGameTemplated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Game&apos;s current &apos;Checksum&apos; values ähnelt.
        /// </summary>
        internal static string GamesCurrentChecksumValues {
            get {
                return ResourceManager.GetString("GamesCurrentChecksumValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Game&apos;s current &apos;UnknownB12&apos; values ähnelt.
        /// </summary>
        internal static string GamesCurrentUnknown12BValues {
            get {
                return ResourceManager.GetString("GamesCurrentUnknown12BValues", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Queued commands ähnelt.
        /// </summary>
        internal static string QueuedCommands {
            get {
                return ResourceManager.GetString("QueuedCommands", resourceCulture);
            }
        }
    }
}
