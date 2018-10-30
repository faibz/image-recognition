using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Repositories;

namespace DistributedSystems.API.Validators
{
    public interface IMapValidator
    {
        IList<Error> ValidateCreateImageMapRequest(int columnCount, int rowCount);
        Task<IList<Error>> ValidateMapEntry(MapData mapData);
    }

    public class MapValidator : IMapValidator
    {
        private readonly IMapsRepository _mapsRepository;

        public MapValidator(IMapsRepository mapsRepository)
        {
            _mapsRepository = mapsRepository;
        }

        public IList<Error> ValidateCreateImageMapRequest(int columnCount, int rowCount)
        {
            var errors = new List<Error>();

            if (columnCount <= 0 || rowCount <= 0) errors.Add(new Error("map", "Column and row counts must be above zero."));

            return errors;
        }

        public async Task<IList<Error>> ValidateMapEntry(MapData mapData)
        {
            var errors = new List<Error>();

            var map = await _mapsRepository.GetMapById(mapData.MapId);
            if (map == null)
            {
                errors.Add(new Error("map", "Could not find a map with that Id."));

                return errors;
            }

            if (!PointWithinMap(mapData.Coordinates, map.ColumnCount, map.RowCount))
                errors.Add(new Error("map", $"Selected map coordinate: ({mapData.Coordinates.X},{mapData.Coordinates.Y}) is not within the map boundaries."));
            if (await _mapsRepository.GetMapImagePartByIdAndLocation(map.Id, mapData.Coordinates.X, mapData.Coordinates.Y) != null)
                errors.Add(new Error("map", $"Selected map coordinate: ({mapData.Coordinates.X},{mapData.Coordinates.Y}) is already occupied."));

            return errors;
        }

        private bool PointWithinMap(Coordinate coordinates, int mapColumns, int mapRows) 
            => !(coordinates.X < 1 || coordinates.Y < 1 || coordinates.X > mapColumns || coordinates.Y > mapRows);
    }
}