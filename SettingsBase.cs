using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Serialization;

// Change list:
// 2023-03-17 Initial version. Adapted from Settings class that did not use inheritance.
// 2023-03-21 Change namespace to SimpleSettings

namespace SimpleSettings
{
    /// <summary>
    /// Base class for Settings values.
    /// Implement INotifyPropertyChanged.
    /// Persist settings to XML file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a base class for defining a Settings class (which defines Settings properties)
    /// It implements common methods used by all Settings classes.
    /// </para>
    /// <para>
    /// This class will persist the settings data to an XML file.
    /// </para>
    /// <para>
    /// The settings filename and path can be specified with the SettingsPath property. If not
    /// specified, it defaults to the file name Settings.xml, in the folder AppData/<exe-name>.
    /// </para>
    /// </remarks>
    public class SettingsBase : INotifyPropertyChanged, IChangeTracking
    {
        private static string SettingsPathValue = "";
        /// <summary>
        /// The full path and filename of the settings file.
        /// This property is not persisted and does not trigger property changed.
        /// </summary>
        /// <remarks>
        /// If null or empty string, will use the default value of %AppData%/<exe-file-name>.xml
        /// </remarks>
        [XmlIgnore]
        public static string SettingsPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SettingsPathValue))
                    SettingsPathValue = DefaultPath();
                return SettingsPathValue;
            }
            set
            {
                SettingsPathValue = value;
            }
        }

        private static string DefaultPath()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string exeName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string path = Path.Combine(folder, exeName, "Settings.xml");
            return path;
        }

        /// <summary>
        /// Return the directory portion of the settingsPath.
        /// </summary>
        [XmlIgnore]
        public static string SettingsFolder
        {
            get
            {
                string path = SettingsPath;
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Implementation of IChangeTracking
        // This property is not persisted and does not trigger property changed.
        /// <summary>
        /// Gets the object's changed status
        /// </summary>
        /// <value>True if the object's content has changed since the last call to AcceptChanges().</value>
        [XmlIgnore]
        public bool IsChanged { get; protected set; }

        /// <summary>
        /// Resets the object's state to unchanged by accepting the modifications.
        /// </summary>
        public void AcceptChanges()
        {
            IsChanged = false;
        }

        /// <summary>
        /// Helper method to implement a property setter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue">The backing field for the property</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="markChanged">If true, and the property has changed, will mark the object as changed</param>
        /// <returns>True if the property changed</returns>
        /// <param name="propertyName">The name of the property</param>
        /// <remarks>
        /// If the property value has changed raises the PropertyChanged event.
        /// Marks the class as changed only if <paramref name="markChanged"/> is true and the property value has changed.
        /// The <paramref name="markChanged"/> parameter is normally the inverse of the XmlIgore attribute.
        /// </remarks>
        protected bool SetProperty<T>(ref T oldValue, T newValue, bool markChanged, [CallerMemberName] string propertyName = "")
        {
            //if (oldValue != null && oldValue.Equals(newValue))
            if (oldValue != null && EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            oldValue = newValue;
            OnPropertyChanged(propertyName);
            if (markChanged)
                IsChanged = true;
            return true;
        }

        /// <summary>
        /// Implement the Load() method for derived classes
        /// </summary>
        /// <typeparam name="DerivedType"></typeparam>
        /// <returns></returns>
        protected static DerivedType Load<DerivedType>() where DerivedType : SettingsBase, new()
        {
            string path = SettingsPath;
            if (path.Length > 0 && File.Exists(path))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DerivedType));
                    TextReader reader = new StreamReader(path);

                    DerivedType mySettings = (DerivedType)serializer.Deserialize(reader);
                    reader.Close();

                    // Success
                    return mySettings;
                }
                catch (Exception e1)
                {
                    // We failed to deserialize the object
                    string msg = "Error loading settings from " + path;
                    msg += " -- " + e1.Message;
                    MessageBox.Show(msg);
                }
            }

            // No file found.
            // Return a default settings object.
            DerivedType defaultSettings = new DerivedType();
            defaultSettings.SetDefaults();
            return defaultSettings;
        }

        /// <summary>
        /// Set any default values for a new settings object.
        /// The base implementation does nothing.
        /// Can be overridden by derived class if necessary.
        /// Typically not necessary, as the property initializers will set the default values
        /// when the object is constructed.
        /// </summary>
        protected virtual void SetDefaults()
        {
        }

        /// <summary>
        /// Implement the SaveIfChanged() method for derived classes
        /// </summary>
        protected void SaveIfChanged(System.Type DerivedType)
        {
            if (IsChanged)
            {
                if (Save(DerivedType))
                {
                    AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Implement the Save() method for derived classes
        /// </summary>
        protected bool Save(System.Type DerivedType)
        {
            try
            {
                string path = SettingsFolder;
                // Ensure destination folder exists
                System.IO.Directory.CreateDirectory(path);
                path = SettingsPath;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                XmlSerializer serializer = new XmlSerializer(DerivedType);
                TextWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, this);
                writer.Close();
                return true;
            }
            catch (Exception e)
            {
                string msg = "Error saving settings - " + e.ToString();
                MessageBox.Show(msg);
                return false;
            }
        }
    }
}
