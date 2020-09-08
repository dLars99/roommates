using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate FROM Roommate";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Roommate> roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        Roommate roommate = new Roommate()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = null
                        };

                        roommates.Add(roommate);
                        
                    }

                    reader.Close();
                    return roommates;
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT FirstName, LastName, RentPortion, MoveInDate FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate()
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate"))
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }

        public List<Roommate> GetAllWithRoom(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT r.Id AS RoommateId, r.FirstName, r.LastName, r.RentPortion, r.MoveInDate, s.Id AS RoomId, s.Name, s.MaxOccupancy FROM Roommate r LEFT JOIN room s ON r.RoomId = s.Id WHERE s.Id = @RoomId";
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();
                    while (reader.Read())
                    {
                        Room room = new Room()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                        };
                        Roommate roommate = new Roommate()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = room
                        };
                        roommates.Add(roommate);
                    }

                    conn.Close();
                    return roommates;
                }
            }
        }
    }
}
