﻿using System.Threading.Tasks;
using DistributedSystems.API.Models;
using Dapper;
using DistributedSystems.API.Factories;
using System.Data;

namespace DistributedSystems.API.Repositories
{
    public interface IImagesRepository
    {
        Task<Image> InsertImage(Image image);
    }

    public class ImagesRepository : IImagesRepository
    {
        private IDbConnection _connection;

        public ImagesRepository(IDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetDbConnection();
        }

        public async Task<Image> InsertImage(Image image)
        {
            await _connection.ExecuteAsync("meme");

            return new Image(new Models.Requests.ImageRequest());
        }
    }
}
