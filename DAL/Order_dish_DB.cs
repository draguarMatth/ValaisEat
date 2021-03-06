﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;
using DTO;

namespace DAL
{
    /*
     * Interface qui définit le comportement de la classe Order_Dish_DB suivante
     */
    public interface IOrder_Dish_DB : IDB
    {

        List<Order_Dish> GetAll();
        Order_Dish GetByID(int id);
        int Delete(int id);
        int Update(Order_Dish order_Dish);
        Order_Dish Add(Order_Dish order_Dish);
    }
    public class Order_Dish_DB : IOrder_Dish_DB
    {
        public IConfiguration Configuration { get; }
        private string connectionString { get; }
        public Order_Dish_DB(IConfiguration conf)
        {
            Configuration = conf;
            connectionString = Configuration.GetConnectionString("DefaultConnection");
        }


        public int Update(Order_Dish order_Dish)
        {
            
            int result = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Order_Dish SET Quantity=@Quantity, IdDish=@IdDish, IdOrder=@IdOrder " +
                        "WHERE IdOrder_Dish=@id;";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@Quantity", order_Dish.Quantity);
                    cmd.Parameters.AddWithValue("@IdDish", order_Dish.IdDish);
                    cmd.Parameters.AddWithValue("@IdOrder", order_Dish.IdOrder);
                    cmd.Parameters.AddWithValue("@iD", order_Dish.IdOrder_Dish);

                    cn.Open();

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        /*
        * Méthode pour supprimer une commande de plat grâce à son id 
        * avec requête SQL
        */
        public int Delete(int id)
        {
            int result = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Order_Dish WHERE IdOrder_Dish=@id";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cn.Open();

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        /*
        * Méthode pour ajouter une commande de plats
        * avec requête SQL
        */
        public Order_Dish Add(Order_Dish order_Dish)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Order_Dish(Quantity, IdDish, IdOrder) " +
                        "VALUES(@Quantity, @IdDish, @IdOrder); SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("@Quantity", order_Dish.Quantity);
                    cmd.Parameters.AddWithValue("@IdDish", order_Dish.IdDish);
                    cmd.Parameters.AddWithValue("@IdOrder", order_Dish.IdOrder);
                    cn.Open();

                    order_Dish.IdOrder_Dish = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return order_Dish;
        }

        /*
        * Méthode récuperer une commande de plat par son ID
        * avec requête SQL
        */
        public Order_Dish GetByID(int id)
        {
            Order_Dish order_Dish = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Order_Dish WHERE IdOrder_Dish = @id";
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            order_Dish = serializeOrderDish(dr);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return order_Dish;
        }

        /*
        * Méthode pour récuperer une liste de toutes les commandes de plats
        * avec requête SQL
        */
        public List<Order_Dish> GetAll()
        {
            List<Order_Dish> results = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Order_Dish";
                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (results == null)
                                results = new List<Order_Dish>();
                            results.Add(serializeOrderDish(dr));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }
        /*
         * Méthode de serialisation qui permet de transformer le résultat d'un SqlDataReader en un objet
         */
        private Order_Dish serializeOrderDish(SqlDataReader dr)
        {
            Order_Dish order_dish = new Order_Dish();

            order_dish.IdOrder_Dish = (int)dr["IdOrder_Dish"];
            order_dish.Quantity = (int)dr["Quantity"]; 
            order_dish.IdDish = (int)dr["IdDish"]; ;
            order_dish.IdOrder = (int)dr["IdOrder"]; ;

            return order_dish;
        }
    }
}
