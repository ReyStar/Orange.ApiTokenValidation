﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orange.ApiTokenValidation.Repositories {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class I18n {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal I18n() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Orange.ApiTokenValidation.Repositories.I18n", typeof(I18n).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to The add operation caused an unhandled error.
        /// </summary>
        internal static string AddOperationException {
            get {
                return ResourceManager.GetString("AddOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The insert or update operation caused an unhandled error.
        /// </summary>
        internal static string AddOrUpdateOperationException {
            get {
                return ResourceManager.GetString("AddOrUpdateOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The delete operation caused an unhandled error.
        /// </summary>
        internal static string DeleteOperationException {
            get {
                return ResourceManager.GetString("DeleteOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The get all operation caused an unhandled error.
        /// </summary>
        internal static string GetAllOperationException {
            get {
                return ResourceManager.GetString("GetAllOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The get operation caused an unhandled error.
        /// </summary>
        internal static string GetOperationException {
            get {
                return ResourceManager.GetString("GetOperationException", resourceCulture);
            }
        }
    }
}