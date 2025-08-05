﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class TodoItemDatabase
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<TodoItem>();
        }

        public async Task<List<TodoItem>> GetItemsAsync()
        {
            await Init();
            return await database.Table<TodoItem>().OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            await Init();
            return await database.Table<TodoItem>()
                .Where(t => !t.Done)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetItemsDoneAsync()
        {
            await Init();
            return await database.Table<TodoItem>()
                .Where(t => t.Done)
                .OrderByDescending(x => x.CompletedDate)
                .ToListAsync();
        }

        public async Task<TodoItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(TodoItem item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(TodoItem item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

        public async Task<int> ToggleItemAsync(TodoItem item)
        {
            await Init();
            item.Done = !item.Done;
            item.CompletedDate = item.Done ? DateTime.Now : null;
            return await database.UpdateAsync(item);
        }
    }
}