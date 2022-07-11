using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TestAppWSS.DAL;
using TestAppWSS.Services.Data;
using TestAppWSS.Services.Interfaces;

namespace TestAppWSS.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly Database _db;

        private readonly ILogger<DbInitializer> _logger;


        public DbInitializer(Database db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _logger = Logger;
        }

        public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
        {
            _logger.LogInformation("Инициализация БД");
            if (RemoveBefore)
            {
                await RemoveAsync(Cancel).ConfigureAwait(false);
            }

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel);
            if (pending_migrations.Any())
            {
                _logger.LogInformation("Выполнение миграции БД");
                await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);  //создание базы, если ее нет и перевод на последнюю миграцию
                _logger.LogInformation("Миграция выполнена успешно");
            }

            await InitializeDepartmentsAsync(Cancel).ConfigureAwait(false);

            _logger.LogInformation("Инициализация БД выполнена успешно");
        }

        public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
        {
            _logger.LogInformation("Удаление БД");
            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);
            if (result)
                _logger.LogInformation("Удаление БД прошло успешно");
            else
                _logger.LogInformation("Удаление БД не требуется (отсутсвтует)");

            return result;
        }

        private async Task InitializeDepartmentsAsync(CancellationToken Cancel)
        {
            if (_db.Departments.Any())
            {
                _logger.LogInformation("Инициализация тестовых данных БД не требуется");
                return;
            }

            _logger.LogInformation("Инициализация тестовых данных БД ...");


            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Departments.AddRangeAsync(TestData.Departments.Select(d=>new Domain.Entities.Node()
                {
                    Name = $"{d.Name} {d.DepthId}",
                    Id=d.Id,
                    Parent=d.Parent,
                    Depth=d.Depth,
                    Children=d.Children,
                    DepthId=d.DepthId,
                    ParentId=d.ParentId
                }), Cancel);

                await _db.SaveChangesAsync(Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _logger.LogInformation("Инициализация тестовых данных БД выполнена успешно");
        }

    }
}