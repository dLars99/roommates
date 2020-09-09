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
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);

            // Main program loop
            bool exitProgram = false;
            while (!exitProgram)
            {
                // Main Menu
                Console.Clear();
                Console.SetCursorPosition((Console.WindowWidth - 32) / 2, Console.CursorTop);
                Console.WriteLine("Welcome to the Roommate Database");
                Console.SetCursorPosition((Console.WindowWidth - 18) / 2, Console.CursorTop);
                Console.WriteLine("Copyright (C) MMXX");
                Console.SetCursorPosition((Console.WindowWidth - 32) / 2, Console.CursorTop);
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
                Console.WriteLine(" 11) Display all chores");
                Console.WriteLine(" 12) Add a new chore");
                Console.WriteLine(" 13) Assign a chore to a roommate");
                Console.WriteLine(" 14) Edit a chore's name");
                Console.WriteLine(" 15) Unassign a chore from a roommate");
                Console.WriteLine(" 16) Delete a chore");
                Console.WriteLine(" 17) Show a report of all roommates and their room assignments");
                Console.WriteLine(" 18) Show a report of all roommates and their chore assigments");
                Console.WriteLine("\n Or enter anything else to exit the program");
                string userSelection = Console.ReadLine();
                bool correctEntry = Int32.TryParse(userSelection, out int menuNumber);

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
                        GetAllRoommates(roommateRepo);
                        break;
                    case 7:
                        GetSingleRoommate(roommateRepo);
                        break;
                    case 8:
                        AddNewRoommate(roomRepo, roommateRepo);
                        break;
                    case 9:
                        EditRoommate(roommateRepo);
                        break;
                    case 10:
                        DeleteRoommate(roommateRepo);
                        break;
                    case 11:
                        GetAllChores(choreRepo);
                        break;
                    case 12:
                        AddNewChore(choreRepo);
                        break;
                    case 13:
                        AssignChore(choreRepo, roommateRepo);
                        break;
                    case 14:
                        EditChore(choreRepo);
                        break;
                    case 15:
                        UnassignChore(choreRepo);
                        break;
                    case 16:
                        DeleteChore(choreRepo);
                        break;
                    case 17:
                        RoommateReport(roomRepo, roommateRepo);
                        break;
                    case 18:
                        ChoreReport(choreRepo, roommateRepo);
                        break;
                    default:
                        exitProgram = true;
                        break;
                }
                if (!exitProgram)
                { 
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
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

        static void GetAllRoommates(RoommateRepository roommateRepo)
        {
            Console.WriteLine("\nGetting All Roommates");
            Console.WriteLine("---------------------");
            List<Roommate> allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate}");
            }
        }

        static void GetSingleRoommate(RoommateRepository roommateRepo)
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roommate with Id 2");

            Roommate singleRoommate = roommateRepo.GetById(2);

            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.Firstname} {singleRoommate.Lastname} {singleRoommate.RentPortion} {singleRoommate.MovedInDate}");
        }

        static void GetRoommatesInRoom(RoommateRepository roommateRepo)
        {
            // Get all roommates in the Living Room (room id of 3)
            Console.WriteLine("\nGetting All Roommates in the Living Room");
            Console.WriteLine("------------------------------------------");
            List<Roommate> roommatesInRoom = roommateRepo.GetAllWithRoom(3);
            foreach (Roommate roommate in roommatesInRoom)
            {
                Console.WriteLine($"{roommate.Id} {roommate.Firstname} {roommate.Lastname} {roommate.RentPortion} {roommate.MovedInDate} {roommate.Room.Name}");
            }

        }

        static void AddNewRoommate(RoomRepository roomRepo, RoommateRepository roommateRepo)
        {
            Roommate fred = new Roommate()
            {
                Firstname = "Friedrich",
                Lastname = "Franzferdinand",
                RentPortion = 25,
                MovedInDate = new DateTime(2020, 03, 04),
                Room = roomRepo.GetById(5)
            };

            roommateRepo.Insert(fred);

            Console.WriteLine("\n-----------------------------");
            Console.WriteLine($"Added new roommate with id {fred.Id}");
        }

        static void EditRoommate(RoommateRepository roommateRepo)
        {
            Roommate roommateToEdit = roommateRepo.GetById(3);
            roommateToEdit.RentPortion = 20;

            roommateRepo.Update(roommateToEdit);
            Console.WriteLine($"{roommateToEdit.Firstname} {roommateToEdit.Lastname} has been updated!");
        }

        static void DeleteRoommate(RoommateRepository roommateRepo)
        {
            roommateRepo.Delete(5);
            Console.Clear();
            Console.WriteLine($"Roommate has been removed.");
        }

        static void RoommateReport(RoomRepository roomRepo, RoommateRepository roommateRepo)
        {
            // Display a report of all roommates and their rooms
            Console.WriteLine("\nReport -- All Roommates and their Rooms");
            Console.WriteLine("=======================================");
            List<Room> allRooms = roomRepo.GetAll();
            foreach (Room room in allRooms)
            {
                List<Roommate> roommatesInRoom = roommateRepo.GetAllWithRoom(room.Id);
                foreach (Roommate roommate in roommatesInRoom)
                {
                    Console.WriteLine($"{roommate.Firstname} {roommate.Lastname}: {room.Name}");
                }
            }
        }

        static void GetAllChores(ChoreRepository choreRepo)
        {
            List<Chore> allChores = choreRepo.GetAll();

            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 10) / 2, Console.CursorTop);
            Console.WriteLine("All Chores");
            Console.SetCursorPosition((Console.WindowWidth - 10) / 2, Console.CursorTop);
            Console.WriteLine("----------");

            foreach (Chore chore in allChores)
            {
                Console.WriteLine($"{chore.Id} {chore.Name}");
            }
        }
        static void AddNewChore(ChoreRepository choreRepo)
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 9) / 2, Console.CursorTop);
            Console.WriteLine("New Chore");
            Console.SetCursorPosition((Console.WindowWidth - 9) / 2, Console.CursorTop);
            Console.WriteLine("---------");
            Console.WriteLine("\nPlease enter the name of the new chore.");
            Chore newChore = new Chore()
            {
                Name = Console.ReadLine()
            };
            Console.WriteLine(" ...");

            choreRepo.Add(newChore);

            Console.WriteLine($"\n{newChore.Name} added to the list");
        }

        static void AssignChore(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            List<Chore> allChores = choreRepo.GetAll();
            List<Roommate> allRoommates = roommateRepo.GetAll();

            // Select a chore
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("Assign Chores");
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("-------------");
            Console.WriteLine("\nPlease choose a chore to assign:");
            for (int i = 1; i < allChores.Count + 1; i++)
            {
                Console.WriteLine($"{i}) {allChores[i-1].Name}");
            }
            Console.WriteLine();
            string userSelection = Console.ReadLine();
            bool validSelection = int.TryParse(userSelection, out int choreNum);
            while (!validSelection || choreNum < 0 || choreNum > allChores.Count)
            {
                Console.WriteLine("Invalid selection");
                userSelection = Console.ReadLine();
                validSelection = int.TryParse(userSelection, out choreNum);
            }
            Chore assignedChore = allChores[choreNum - 1];

            // Select a roommate
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("Assign Chores");
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("-------------");
            Console.WriteLine($"\nAssign {assignedChore.Name} to a roommate:");
            for (int i = 1; i < allRoommates.Count + 1; i++)
            {
                Console.WriteLine($"{i}) {allRoommates[i-1].Firstname} {allRoommates[i-1].Lastname}");
            }
            Console.WriteLine();
            userSelection = Console.ReadLine();
            validSelection = int.TryParse(userSelection, out int roommateNum);
            while (!validSelection || roommateNum < 0 || roommateNum > allRoommates.Count)
            {
                Console.WriteLine("Invalid selection");
                userSelection = Console.ReadLine();
                validSelection = int.TryParse(userSelection, out roommateNum);
            }
            Roommate assignedRoommate = allRoommates[roommateNum - 1];

            choreRepo.Assign(assignedChore, assignedRoommate);

            Console.WriteLine($"{assignedChore.Name} has been assigned to {assignedRoommate.Firstname}");
        }

        static void UnassignChore(ChoreRepository choreRepo)
        {
            Console.Clear();
            Console.WriteLine("This function coming soon...");
            //dictionary<chore, roommate> allassignments = chorerepo.getallassignments();

            //for (int i = 1; i < allassignments.count + 1; i++)
            //{
            //    console.writeline("")
            //}
        }

        static void EditChore(ChoreRepository choreRepo)
        {
            List<Chore> allChores = choreRepo.GetAll();

            // Select a chore
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("Assign Chores");
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("-------------");
            Console.WriteLine("\nPlease choose a chore to edit:");
            for (int i = 1; i < allChores.Count + 1; i++)
            {
                Console.WriteLine($"{i}) {allChores[i-1].Name}");
            }
            Console.WriteLine();
            string userSelection = Console.ReadLine();
            bool validSelection = int.TryParse(userSelection, out int choreNum);
            while (!validSelection || choreNum < 0 || choreNum > allChores.Count)
            {
                Console.WriteLine("Invalid selection");
                userSelection = Console.ReadLine();
                validSelection = int.TryParse(userSelection, out choreNum);
            }
            Chore editedChore = allChores[choreNum - 1];

            Console.WriteLine("Please enter a new name for this chore:");
            editedChore.Name = Console.ReadLine();

            choreRepo.Update(editedChore);

            Console.WriteLine($"{editedChore.Name} has been updated.");
        }

        static void DeleteChore(ChoreRepository choreRepo)
        {
            List<Chore> allChores = choreRepo.GetAll();

            // Select a chore
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("Assign Chores");
            Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop);
            Console.WriteLine("-------------");
            Console.WriteLine("\nPlease choose a chore to delete:");
            for (int i = 1; i < allChores.Count + 1; i++)
            {
                Console.WriteLine($"{i}) {allChores[i-1].Name}");
            }
            Console.WriteLine();
            string userSelection = Console.ReadLine();
            bool validSelection = int.TryParse(userSelection, out int choreNum);
            while (!validSelection || choreNum < 0 || choreNum > allChores.Count)
            {
                Console.WriteLine("Invalid selection");
                userSelection = Console.ReadLine();
                validSelection = int.TryParse(userSelection, out choreNum);
            }
            Chore deletedChore = allChores[choreNum - 1];

            choreRepo.Delete(deletedChore.Id);

            Console.WriteLine($"{deletedChore.Name} has been removed from the list.");
        }

        static void ChoreReport(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            Dictionary<Chore, Roommate> allAssignments = choreRepo.GetAllAssignments();

            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - 26) / 2, Console.CursorTop);
            Console.WriteLine("All Chores and Assignments");
            Console.SetCursorPosition((Console.WindowWidth - 26) / 2, Console.CursorTop);
            Console.WriteLine("--------------------------\n");

            foreach (KeyValuePair<Chore, Roommate> assignment in allAssignments)
            {
                Console.WriteLine($"{assignment.Key.Name}: {assignment.Value.Firstname} {assignment.Value.Lastname}");
            }

        }
    }
}