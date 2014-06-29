﻿/* Copyright 2014 Glen/CleaveTV

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. */
using System;
using System.Collections.Generic;
using CleaveFramework.Scene;
using UnityEngine;

namespace CleaveFramework.Factory
{
    /// <summary>
    /// defines a post instantiation constructor for the object
    /// </summary>
    /// <param name="obj">object to construct</param>
    /// <returns>constructed object</returns>
    public delegate object Constructor(object obj);

    /// <summary>
    /// generic Factory implementation
    /// </summary>
    static class Factory
    {
        static Dictionary<Type, Constructor> _constructors = new Dictionary<Type, Constructor>();

        /// <summary>
        /// Sets a post instantiation constructor to an object type
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="constructor">delegate to act as constructor</param>
        static public void SetConstructor<T>(Constructor constructor)
        {
            // for now we are a single delegate
            if (_constructors.ContainsKey(typeof (T)))
            {
                _constructors[typeof (T)] = constructor;
                return;
            }
            _constructors.Add(typeof(T), constructor);
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <returns></returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName)
        {
            var component = ResolveComponent<T>(goName);
            component = (MonoBehaviour) InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <returns></returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go)
        {
            var component = ResolveComponent<T>(go);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor)
        {
            var component = ResolveComponent<T>(goName);
            component = (MonoBehaviour) InvokeConstructor<T>(component, constructor);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor)
        {
            var component = ResolveComponent<T>(go);
            component = (MonoBehaviour)InvokeConstructor<T>(component, constructor);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(goName, constructor);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(go, constructor);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(goName);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(go);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(goName);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">Name of GameObject holding the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(go);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(goName, constructor);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(go, constructor);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Create object of type T, if a constructor exists in the Factory use it on the object
        /// before returning it
        /// </summary>
        /// <typeparam name="T">Type of object to construct</typeparam>
        /// <returns>constructed object</returns>
        static public object Create<T>()
        {
            var obj = Activator.CreateInstance<T>();
            obj = (T)InvokeDefaultConstructor<T>(obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T and run this custom constructor on it
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">constructor to run on object</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor)
        {
            var obj = Activator.CreateInstance<T>();

            if (constructor != null)
            {
                obj = (T) constructor.Invoke(obj);
            }

            return obj;
        }

        /// <summary>
        /// Create an object of type T and insert it as singleton into the scene objects data
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="data">Instance of SceneObjectsData to insert to</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(SceneObjectData data)
        {
            var obj = Create<T>();
            PushSingleton<T>(data, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T and insert it as transient into the objects data
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="data">Instance of scene data to insert to</param>
        /// <param name="name">Name of object to use in transients library</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(SceneObjectData data, string name)
        {
            var obj = Create<T>();
            PushTransient<T>(data, name, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T with a custom constructor and insert it into singleton library
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">custom constructor to use</param>
        /// <param name="data">Instance of scene data</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor, SceneObjectData data)
        {
            var obj = Create<T>(constructor);
            PushSingleton<T>(data, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T with a custom constructor and insert it into transients library
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">custom constructor to use</param>
        /// <param name="data">Instance of scene data</param>
        /// <param name="name">Name to use in transients library</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor, SceneObjectData data, string name)
        {
            var obj = Create<T>(constructor);
            PushTransient<T>(data, name, obj);
            return obj;
        }

        static private void PushSingleton<T>(SceneObjectData data, object obj)
        {
            if (data == null)
            {
                throw new Exception("SceneObjectsData passed to Create() was null.");
            }
            data.PushObjectAsSingleton((T)obj);
        }

        static private void PushTransient<T>(SceneObjectData data, string name, object obj)
        {
            if (data == null)
            {
                throw new Exception("SceneObjectsData passed to Create() was null.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name passed to Create() was empty or null");
            }
            data.PushObjectAsTransient(name, (T)obj);
        }

        static private object InvokeConstructor<T>(object obj, Constructor constructor)
        {
            if (constructor != null)
            {
                obj = constructor.Invoke(obj);
            }

            return obj;
        }

        static private object InvokeDefaultConstructor<T>(object obj)
        {
            if (_constructors.ContainsKey(typeof(T)))
            {
                if (_constructors[typeof(T)] != null)
                    obj = (T)_constructors[typeof(T)].Invoke(obj);
            }
            return obj;
        }
        static private MonoBehaviour ResolveComponent<T>(string goName)
        {
            var go = GameObject.Find(goName);
            if (go == null)
            {
                throw new Exception("ResolveComponent: GameObject was null.");
            }
            return ResolveComponent<T>(go);
        }
        static private MonoBehaviour ResolveComponent<T>(GameObject go)
        {
            var component = go.GetComponent(typeof(T).Name);
            if (component == null)
            {
                throw new Exception("ResolveComponent: Component was null.");
            }
            return (MonoBehaviour)component;
        }
    }
}
