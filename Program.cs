using System;
using System.Collections.Generic;
using System.IO;

// Enum for room types
enum RoomType { Single, Double, Suite }

// Interface for billing
interface IBillable
{
    double CalculateBill();
}

// Abstract Room class
abstract class Room : IBillable
{
    public int RoomNumber { get; set; }
    public RoomType Type { get; set; }
    public bool IsBooked { get; set; }

    public abstract double CalculateBill();

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Room {RoomNumber} | Type: {Type} | Booked: {IsBooked}");
    }
}

// Derived classes
class SingleRoom : Room
{
    public override double CalculateBill() => 1000;
}

class DoubleRoom : Room
{
    public override double CalculateBill() => 1800;
}

class SuiteRoom : Room
{
    public override double CalculateBill() => 3000;
}

// Booking Manager
class BookingManager
{
    private List<Room> rooms = new List<Room>();
    private string filePath = "bookings.txt";

    public BookingManager()
    {
        // Predefined rooms
        rooms.Add(new SingleRoom { RoomNumber = 101, Type = RoomType.Single });
        rooms.Add(new DoubleRoom { RoomNumber = 201, Type = RoomType.Double });
        rooms.Add(new SuiteRoom { RoomNumber = 301, Type = RoomType.Suite });
    }

    public void ShowRooms()
    {
        Console.WriteLine("\nAvailable Rooms:");
        foreach (var room in rooms)
        {
            room.DisplayInfo();
        }
    }

    public void BookRoom(int roomNumber)
    {
        Room room = rooms.Find(r => r.RoomNumber == roomNumber);

        if (room == null)
        {
            Console.WriteLine("Invalid room number.");
            return;
        }

        if (room.IsBooked)
        {
            Console.WriteLine("Room already booked!");
            return;
        }

        room.IsBooked = true;
        double bill = room.CalculateBill();

        Console.WriteLine($"Room {room.RoomNumber} booked successfully!");
        Console.WriteLine($"Total Bill: ₹{bill}");

        // Save booking info to file
        File.AppendAllText(filePath, $"Room {room.RoomNumber} ({room.Type}) booked on {DateTime.Now}. Bill: ₹{bill}\n");
    }
}

class Program
{
    static void Main()
    {
        BookingManager manager = new BookingManager();
        int choice;

        do
        {
            Console.WriteLine("\n====== HOTEL ROOM BOOKING SYSTEM ======");
            Console.WriteLine("1. Show All Rooms");
            Console.WriteLine("2. Book a Room");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            bool validInput = int.TryParse(Console.ReadLine(), out choice);

            if (!validInput)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    manager.ShowRooms();
                    break;

                case 2:
                    Console.Write("Enter Room Number to book: ");
                    if (int.TryParse(Console.ReadLine(), out int roomNo))
                        manager.BookRoom(roomNo);
                    else
                        Console.WriteLine("Please enter a valid number.");
                    break;

                case 3:
                    Console.WriteLine(" Thank you for using Hotel Room Booking System!");
                    break;

                default:
                    Console.WriteLine(" Invalid choice. Try again.");
                    break;
            }

        } while (choice != 3);
    }
}
