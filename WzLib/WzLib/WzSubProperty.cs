﻿namespace WzLib
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class WzSubProperty : IWzImageProperty, IWzObject, IDisposable
    {
        internal WzImage imgParent;
        internal string name;
        internal IWzObject parent;
        internal List<IWzImageProperty> properties;

        public WzSubProperty()
        {
            this.properties = new List<IWzImageProperty>();
        }

        public WzSubProperty(string name)
        {
            this.properties = new List<IWzImageProperty>();
            this.name = name;
        }

        public void AddProperty(IWzImageProperty prop)
        {
            prop.Parent = this;
            prop.ParentImage = this.ParentImage;
            switch (prop.PropertyType)
            {
                case WzPropertyType.SubProperty:
                case WzPropertyType.Canvas:
                case WzPropertyType.Vector:
                case WzPropertyType.Convex:
                case WzPropertyType.Sound:
                case WzPropertyType.UOL:
                    this.properties.Add(new WzExtendedProperty(prop.Name) { ExtendedProperty = prop });
                    return;
            }
            this.properties.Add(prop);
        }

        public void ClearProperties()
        {
            this.properties.Clear();
        }

        public void Dispose()
        {
            this.name = null;
            foreach (IWzImageProperty property in this.properties)
            {
                property.Dispose();
            }
            this.properties.Clear();
            this.properties = null;
        }

        public void RemoveProperty(string name)
        {
            this.properties.Remove(GetRawProperty(name));
        }

        public IWzImageProperty GetRawProperty(string name)
        {
            foreach (IWzImageProperty property in this.properties)
            {
                if (property.Name == name)
                {
                    return property;
                }
            }
            throw new KeyNotFoundException("A wz property with the specified name was not found. (TL; DR: tell this to haha01haha01 and he will fix it)");
        }

        public IWzImageProperty this[string name]
        {
            get
            {
                foreach (IWzImageProperty property in this.properties)
                {
                    if (property.Name == name)
                    {
                        if (property.PropertyType == WzPropertyType.Extended)
                        {
                            return ((WzExtendedProperty) property).ExtendedProperty;
                        }
                        return property;
                    }
                }
                throw new KeyNotFoundException("A wz property with the specified name was not found");
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public WzObjectType ObjectType
        {
            get
            {
                return WzObjectType.Property;
            }
        }

        public IWzObject Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public WzImage ParentImage
        {
            get
            {
                return this.imgParent;
            }
            set
            {
                this.imgParent = value;
            }
        }

        public WzPropertyType PropertyType
        {
            get
            {
                return WzPropertyType.SubProperty;
            }
        }

        public IWzImageProperty[] WzProperties
        {
            get
            {
                return this.properties.ToArray();
            }
        }
    }
}

