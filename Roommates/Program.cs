﻿using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRooms = roomRepo.GetAll();

            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");

            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            Room bathroom = new Room
            {
                Name = "Bathroom",
                MaxOccupancy = 1
            };

            roomRepo.Insert(bathroom);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Room with id {bathroom.Id}");

            bathroom.MaxOccupancy = 3;
            roomRepo.Update(bathroom);

            allRooms = roomRepo.GetAll();

            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Updated {bathroom.Name} Max Occupancy");
            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            roomRepo.Delete(bathroom.Id);
            allRooms = roomRepo.GetAll();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Deleted new bathroom");
            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

            // Get and list all roommates
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            Console.WriteLine("\nGetting All Roommates");
            Console.WriteLine("---------------------");
            List<Roommate> allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate}");
            }

            // Get roommate with a specific id
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate with Id 2");

            Roommate singleRoommate = roommateRepo.GetById(2);

            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.Firstname} {singleRoommate.Lastname} {singleRoommate.RentPortion} {singleRoommate.MovedInDate}");

            // Get all roommates in the Living Room (room id of 3)
            Console.WriteLine("\nGetting All Roommates in the Living Room");
            Console.WriteLine("------------------------------------------");
            List<Roommate> roommatesInRoom = roommateRepo.GetAllWithRoom(3);
            foreach (Roommate roommate in roommatesInRoom)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate} {roommate.Room.Name}");
            }

            // Create a new roommate
            Roommate fred = new Roommate()
            {
                Firstname = "Friedrich",
                Lastname = "Franzferdinand",
                RentPortion = 25,
                MovedInDate = new DateTime(2020, 03, 04),
                Room = allRooms[1]
            };

            roommateRepo.Insert(fred);

            Console.WriteLine("\n-----------------------------");
            Console.WriteLine($"Added new roommate with id {fred.Id}");


            // Update roommate
            fred.RentPortion = 20;

            roommateRepo.Update(fred);
            Console.WriteLine("Updated Friedrich to show a lower rent portion");
            Console.WriteLine("-----------------------------------------");
            allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate}");
            }


            // Delete roommate
            roommateRepo.Delete(fred.Id);
            allRoommates = roommateRepo.GetAll();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Deleted Friedrich");
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate}");
            }





        }
    }
}