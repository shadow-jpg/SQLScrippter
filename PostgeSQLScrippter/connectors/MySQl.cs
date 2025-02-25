﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter
{
    internal class MySQl
    {
        private readonly string _connectionString;

        // Конструктор класса, принимающий строку подключения
        public MySQl(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для выполнения SQL-запроса без возврата данных (INSERT, UPDATE, DELETE)
        public int ExecuteNonQuery(string sql)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    return command.ExecuteNonQuery();
                }
            }
        }

        // Метод для выполнения SQL-запроса с возвратом данных (SELECT)
        public List<Dictionary<string, object>> ExecuteQuery(string sql)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }

        // Метод для выполнения SQL-запроса с параметрами
        public int ExecuteNonQueryWithParameters(string sql, Dictionary<string, object> parameters)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        // Метод для выполнения SQL-запроса с параметрами и возвратом данных
        public List<Dictionary<string, object>> ExecuteQueryWithParameters(string sql, Dictionary<string, object> parameters)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }
    }
}
