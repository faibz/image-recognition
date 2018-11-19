﻿using DistributedSystems.API.Factories;
using DistributedSystems.API.Models;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DistributedSystems.API.Controllers
{
    public interface IWorkerClientVersionsRepository
    {
        Task<WorkerClientVersion> GetLatestWorkerClient();
        Task<WorkerClientVersion> GetWorkerByVersion(string clientVersion);
        Task<WorkerClientVersion> InsertWorkerClientVersion(WorkerClientVersion workerClientVersion);
    }

    public class WorkerClientVersionsRepository : IWorkerClientVersionsRepository
    {
        private readonly IDbConnection _connection;

        public WorkerClientVersionsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.GetDbConnection();
        }

        public async Task<WorkerClientVersion> GetLatestWorkerClient()
        {
            var latestClientVersion = await _connection.QueryFirstAsync<Models.DTOs.WorkerClientVersion>("SELECT [Version], [Location], [Hash] FROM [dbo].[WorkerClientVersion] ORDER BY [Version] DESC");

            return (WorkerClientVersion) latestClientVersion;
        }

        public async Task<WorkerClientVersion> GetWorkerByVersion(string clientVersion)
        {
            var workerClient = await _connection.QueryFirstOrDefaultAsync<Models.DTOs.WorkerClientVersion>("SELECT [Version] FROM [dbo].[WorkerClientVersions] WHERE [Version] = @Version", new { Version = clientVersion });

            return workerClient == null ? null : (WorkerClientVersion) workerClient;
        }

        public async Task<WorkerClientVersion> InsertWorkerClientVersion(WorkerClientVersion workerClientVersion)
        {
            await _connection.ExecuteAsync("INSERT INTO [dbo].[WorkerClientVersions] ([Version], [ReleaseDate],[Location], [Hash]) VALUES (@Version, @ReleaseDate, @Location, @Hash)", new { workerClientVersion.Version, workerClientVersion.ReleaseDate, workerClientVersion.Location, workerClientVersion.Hash });

            return workerClientVersion;
        }
    }
}