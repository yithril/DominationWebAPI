using AutoWrapper.Wrappers;
using Domination_WebAPI.Data;
using Domination_WebAPI.Domain.Interface;
using Domination_WebAPI.Enum;
using Domination_WebAPI.Models;
using Domination_WebAPI.Settings;
using Microsoft.EntityFrameworkCore;

namespace Domination_WebAPI.Domain
{
    public class MapDomain : IMapDomain
    {
        private readonly ApplicationDbContext _context;
        private readonly GameSettings _settings;
        private Random _random = new Random();

        public MapDomain(ApplicationDbContext dbContext, GameSettings settings)
        {
            _context = dbContext;
            _settings = settings;
        }

        public async Task<ApiResponse> SetupNewPlayer(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new ApiResponse(Enum.ApiError.USER_NOT_FOUND, 500);
            }

            var zones = await _context.GameZones.ToListAsync();
            var zoneList = new List<GameZone>();

            var nextZone = GetNextZoneCoordinate(zones);

            return new ApiResponse(true);
        }

        public async Task<ApiResponse> CreateGameZone(int x, int y)
        {
            try
            {
                var gameZone = new GameZone()
                {
                    xCoord = x,
                    yCoord = y,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.GameZones.Add(gameZone);

                await _context.SaveChangesAsync();

                //We need to roll for a template.
                var zoneTemplate = RollForZoneTemplate();

                var gameNodeList = new List<GameNode>();

                //Each game zone is 10 x 10
                for (var i = 0; i < 6; i++)
                {
                    for (var j = 0; j < 6; j++)
                    {
                        var node = new GameNode();

                        node.xCoord = i;
                        node.yCoord = j;

                        node.GameZone = gameZone;
                        node.GameZoneId = gameZone.Id;

                        node.CurrentResourceType = RollForGameNodeType(zoneTemplate);

                        if (node.CurrentResourceType != ResourceTypeEnum.Wasteland || node.CurrentResourceType != ResourceTypeEnum.Water)
                        {
                            node.IsDevelopable = true;
                            node.NodeQuality = RollForNodeQuality(node);
                        }

                        node.CreatedDate = DateTime.Now;
                        node.IsActive = true;

                        gameNodeList.Add(node);
                    }
                }

                _context.GameNodes.AddRange(gameNodeList);

                await _context.SaveChangesAsync();

                gameZone.Nodes = gameNodeList;

                return new ApiResponse(gameZone);
            }
            catch(Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        public async Task<ApiResponse> GetGameZoneById(int id)
        {
            try
            {
                var zone = await _context.GameZones
                    .Where(x => x.Id == id)
                    .Include(y => y.Nodes)
                    .ThenInclude(z => z.NodeImprovement)
                    .FirstOrDefaultAsync();

                return new ApiResponse(zone);
            }
            catch (Exception ex)
            {
                //Log

                return new ApiResponse(ex.Message, 500);
            }
        }

        public async Task<ApiResponse> ClaimGameZone(int targetX, int targetY, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var zone = await _context.GameZones.Where(x => x.xCoord == targetX && x.yCoord == targetY).FirstOrDefaultAsync();

            if(user == null)
            {
                return new ApiResponse(Enum.ApiError.USER_NOT_FOUND, 500);
            }

            if(zone == null)
            {
                return new ApiResponse(Enum.ApiError.ZONE_NOT_FOUND, 500);
            }

            //Get all claims
            var claims = await _context.GameZoneClaims
                .Where(x => x.GameZoneId == zone.Id)
                .ToListAsync();

            if(claims.Any(x => x.UserId == user.Id))
            {
                return new ApiResponse(Enum.ApiError.ALREADY_CLAIMED, 200);
            }

            var cost = (claims.Where(x => x.UserId != user.Id).Count() * _settings.OpponentClaimModifier) + _settings.BaseClaimCost;

            if(user.AcquisitionPoints < cost)
            {
                return new ApiResponse(Enum.ApiError.NOT_ENOUGH_ACQUISITION, 200);
            }

            //All clear make the claim
            user.AcquisitionPoints -= cost;

            var newClaim = new GameZoneClaim()
            {
                UserId = user.Id,
                GameZoneId = zone.Id
            };

            _context.Add(newClaim);

            await _context.SaveChangesAsync();

            return new ApiResponse(true);
        }

        private NodeQuality RollForNodeQuality(GameNode node)
        {
            var dieRoll = _random.Next(1, 6);

            return (NodeQuality)dieRoll;
        }

        private ResourceTypeEnum RollForGameNodeType(GameZoneTemplate template)
        {
            var dieRoll = _random.NextDouble();

            //check if it's land
            if((dieRoll -= template.LandTileProbability) < 0)
            {
                return ResourceTypeEnum.None;
            }
            else if((dieRoll -= template.WaterTileProbability) < 0)
            {
                return ResourceTypeEnum.Water;
            }
            else
            {
                return ResourceTypeEnum.Wasteland;
            }
        }

        private GameZoneTemplate RollForZoneTemplate()
        {
            var dieRoll = _random.Next(1, 101);

            var template = new GameZoneTemplate();

            if(dieRoll >0 && dieRoll <= 10)
            {
                template.TemplateType = Enum.TemplateType.Ocean;
                template.WastelandProbability = .3;
                template.WaterTileProbability = .9;
                template.LandTileProbability = .7;               
            }
            else if(dieRoll > 10 && dieRoll <= 30)
            {
                template.TemplateType = Enum.TemplateType.Plains;
                template.WastelandProbability = .4;
                template.LandTileProbability = .5;
                template.WaterTileProbability = .1;
            }
            else if(dieRoll > 30 && dieRoll <= 50)
            {
                template.TemplateType = Enum.TemplateType.Archipelago;
                template.WaterTileProbability = .75;
                template.LandTileProbability = .25;
            }
            else if(dieRoll > 50 && dieRoll <= 60)
            {
                template.TemplateType = Enum.TemplateType.Wasteland;
                template.WastelandProbability = .7;
                template.LandTileProbability = .25;
                template.WaterTileProbability = .5;
            }
            else if(dieRoll > 60 && dieRoll <= 90)
            {
                template.TemplateType = Enum.TemplateType.Land_Locked;
                template.LandTileProbability = .9;
                template.WaterTileProbability = .5;
                template.WastelandProbability = .5;
            }
            else
            {
                template.TemplateType = Enum.TemplateType.Riverland;
                template.LandTileProbability = .7;
                template.WaterTileProbability = .25;
                template.WastelandProbability = .5;
            }

            return template;
        }

        private (int, int) GetNextZoneCoordinate(List<GameZone> gameZones)
        {
            var targetTuples = new List<(int, int)>();

            foreach(var zone in gameZones)
            {
                //The x,y coordinate of zones that are empty
                if(!gameZones.Any(x => x.xCoord == zone.xCoord - 1 && x.yCoord == zone.yCoord))
                {
                    targetTuples.Add(new (zone.xCoord - 1, zone.yCoord));
                }

                if (!gameZones.Any(x => x.xCoord == zone.xCoord && x.yCoord == zone.yCoord + 1))
                {
                    targetTuples.Add(new(zone.xCoord, zone.yCoord + 1));
                }

                if (!gameZones.Any(x => x.xCoord == zone.xCoord + 1 && x.yCoord == zone.yCoord))
                {
                    targetTuples.Add(new(zone.xCoord + 1, zone.yCoord));
                }

                if (!gameZones.Any(x => x.xCoord == zone.xCoord && x.yCoord == zone.yCoord - 1))
                {
                    targetTuples.Add(new(zone.xCoord, zone.yCoord - 1));
                }
            }

            var diceRoller = new Random();
            var random = diceRoller.Next(1, targetTuples.Count);

            return (targetTuples[random].Item1, targetTuples[random].Item2);
        }
    }
}
