using System;
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

            // Main Menu
            Console.Clear();
            Console.WriteLine("Welcome to the Roommate Database");
            Console.WriteLine("Copyright (C) MMXX");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("What would you like to do? Choose a number:");
            Console.WriteLine(" 1) Show all rooms");
            Console.WriteLine(" 2) View room #1");
            Console.WriteLine(" 3) Add a new room");
            Console.WriteLine(" 4) Update a room listing");
            Console.WriteLine(" 5) Remove a room");
            Console.WriteLine(" 6) Show all roommates");
            Console.WriteLine(" 7) Show roommate #1");
            Console.WriteLine(" 8) Add a new roommate");
            Console.WriteLine(" 9) Update a roommate's information");
            Console.WriteLine(" 10) Remove a roommate");
            Console.WriteLine(" 11) Show a report of all roommates and their room assignments");
            string userSelection = Console.ReadLine();
            bool correctEntry = Int32.TryParse(userSelection, out int menuNumber);
            while (!correctEntry || menuNumber < 1 || menuNumber > 9)
            {
                Console.WriteLine("Invalid selection.");
                userSelection = Console.ReadLine();
                correctEntry = Int32.TryParse(userSelection, out menuNumber);
            }

            switch (menuNumber)
            {
                case 1:
                    GetAllRooms(roomRepo);
                    break;
                case 2:
                    GetSingleRoom(roomRepo);
                    break;
                case 3:
                    AddNewRoom(roomRepo);
                    break;
                case 4:
                    EditRoom(roomRepo);
                    break;
                case 5:
                    DeleteRoom(roomRepo);
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                default:
                    break;
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


            // Final report - roommates with rooms
            Console.WriteLine("\nReport -- All Roommates and their Rooms");
            Console.WriteLine("=======================================");
            foreach (Room room in allRooms)
            {
                roommatesInRoom = roommateRepo.GetAllWithRoom(room.Id);
                foreach (Roommate roommate in roommatesInRoom)
                {
                    Console.WriteLine($"{roommate.Firstname} {roommate.Lastname}: {room.Name}");
                }
            }
        }

        static void GetAllRooms(RoomRepository roomRepo)
        {
            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            List<Room> allRooms = roomRepo.GetAll();

            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }

        }

        static void GetSingleRoom(RoomRepository roomRepo)
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");

            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");
        }

        static void AddNewRoom(RoomRepository roomRepo)
        {
            Room bathroom = new Room
            {
                Name = "Bathroom",
                MaxOccupancy = 1
            };

            roomRepo.Insert(bathroom);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Room with id {bathroom.Id}");
        }

        static void EditRoom(RoomRepository roomRepo)
        {
            Room roomToEdit = roomRepo.GetById(5);
            roomToEdit.MaxOccupancy = 3;
            roomRepo.Update(roomToEdit);

            Console.Clear();
            Console.WriteLine($"{roomToEdit.Name} has been updated.");
        }

        static void DeleteRoom(RoomRepository roomRepo)
        {
            roomRepo.Delete(5);

            Console.Clear();
            Console.WriteLine("Room has been deleted.");
        }
    }
}