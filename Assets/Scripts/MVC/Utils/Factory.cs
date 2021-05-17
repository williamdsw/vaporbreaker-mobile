using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

namespace MVC.Utils
{
    public static class Factory<T>
    {
        public static T CreateOne(IDataReader reader)
        {
            T instance = (T) Activator.CreateInstance(typeof(T));

            if (reader.FieldCount > 0)
            {
                int index = 0;
                foreach (FieldInfo prop in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try
                    {
                        if (index < reader.FieldCount)
                        {
                            var value = (prop.Name != "Status") ? (reader[prop.Name] != DBNull.Value ? (prop.FieldType == typeof(string) ? (string) reader[prop.Name] : reader[prop.Name]) : null) : ((long) reader[prop.Name] == 1);
                            prop.SetValue(instance, value);
                        }

                        index++;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogErrorFormat("{0} ===== {1}", prop.Name, ex.Message);
                        throw ex;
                    }
                }
            }

            return instance;
        }

        public static List<T> CreateMany(IDataReader reader)
        {
            List<T> instances = new List<T>();

            if (reader.FieldCount > 0)
            {
                while(reader.Read())
                {
                    instances.Add(CreateOne(reader));
                }
            }

            return instances;
        }
    }
}