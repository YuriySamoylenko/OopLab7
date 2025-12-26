The application represents functionality for the creation and management of water vending machines.

# Water Vending Machine — Lab 7 (CSV / JSON Serialization)

## Project Overview
This lab extends the application from Lab-6 by adding **serialization and deserialization**
of a collection of objects to **CSV** and **JSON** files.

---

## Object Storage
- All objects are stored in `List<WaterVendingMachine>`
- Collection can be saved, loaded, and cleared at runtime

---

## Serialization Features

### CSV
- Save collection to `*.csv` file
- Load collection from `*.csv` file
- Invalid or corrupted rows are safely skipped using `TryParse`
- After loading, user is informed about the number of deserialized objects

### JSON
- Save collection to `*.json` file
- Load collection from `*.json` file
- Uses JSON serialization for full object data
- After loading, user is informed about the number of deserialized objects

---

## Program Changes
- Added methods for:
  - Saving collection to file
  - Reading collection from file
  - Clearing the collection
- Unit tests updated and added for new logic
- All existing and new tests pass successfully

---

## Menu
1. Add object  
2. View all objects  
3. Find object  
4. Demonstrate behavior  
5. Delete object  
6. Demonstrate static methods  
7. Save collection to file  
8. Load collection from file  
9. Clear collection  
0) Exit  

---

## Object Creation Options
- Create default
- Manually enter data
- Manually enter data partly
- Enter entity string
- Exit to main menu
